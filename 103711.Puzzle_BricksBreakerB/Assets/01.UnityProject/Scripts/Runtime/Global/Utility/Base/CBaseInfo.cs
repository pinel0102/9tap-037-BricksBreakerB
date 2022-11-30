using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.Runtime.Serialization;
using MessagePack;

/** 기본 정보 */
[Union(0, typeof(CAppInfo))]
[Union(1, typeof(CUserInfo))]
[Union(2, typeof(CGameInfo))]
[Union(3, typeof(CClearInfo))]
[Union(4, typeof(CTargetInfo))]
[MessagePackObject][System.Serializable]
public abstract partial class CBaseInfo : IMessagePackSerializationCallbackReceiver {
#region 변수
	[Key(0)] public Dictionary<string, string> m_oStrDict = new Dictionary<string, string>();
#endregion // 변수

#region 상수
	private const string KEY_VER = "Ver";
	private const string KEY_SAVE_TIME = "SaveTime";
#endregion // 상수

#region 프로퍼티
	[IgnoreMember] public System.Version Ver { get { return System.Version.Parse(m_oStrDict.GetValueOrDefault(KEY_VER, KCDefine.B_DEF_VER)); } set { m_oStrDict.ExReplaceVal(KEY_VER, value.ToString(KCDefine.B_VAL_3_INT)); } }
	[IgnoreMember] public System.DateTime SaveTime { get { return this.SaveTimeStr.ExIsValid() ? this.CorrectSaveTimeStr.ExToTime(KCDefine.B_DATE_T_FMT_SLASH_YYYY_MM_DD_HH_MM_SS) : System.DateTime.Now; } set { m_oStrDict.ExReplaceVal(KEY_SAVE_TIME, value.ExToLongStr()); } }

	[IgnoreMember] public virtual bool IsIgnoreVer => false;
	[IgnoreMember] public virtual bool IsIgnoreSaveTime => false;

	[IgnoreMember] private string SaveTimeStr => m_oStrDict.GetValueOrDefault(KEY_SAVE_TIME, string.Empty);
	[IgnoreMember] private string CorrectSaveTimeStr => this.SaveTimeStr.Contains(KCDefine.B_TOKEN_SLASH) ? this.SaveTimeStr : this.SaveTimeStr.ExToTime(KCDefine.B_DATE_T_FMT_YYYY_MM_DD_HH_MM_SS).ExToLongStr();
#endregion // 프로퍼티
	
#region IMessagePackSerializationCallbackReceiver
	/** 직렬화 될 경우 */
	public virtual void OnBeforeSerialize() {
		// 버전 무시 모드 일 경우
		if(this.IsIgnoreVer) {
			m_oStrDict.ExRemoveVal(KEY_VER);
		}

		// 저장 시간 무시 모드가 아닐 경우
		if(!this.IsIgnoreSaveTime) {
			this.SaveTime = System.DateTime.Now;
		} else {
			m_oStrDict.ExRemoveVal(KEY_SAVE_TIME);
		}
	}

	/** 역직렬화 되었을 경우 */
	public virtual void OnAfterDeserialize() {
		m_oStrDict = m_oStrDict ?? new Dictionary<string, string>();
	}
#endregion // IMessagePackSerializationCallbackReceiver

#region 함수
	/** 생성자 */
	public CBaseInfo(System.Version a_stVer) {
		this.Ver = a_stVer;
	}

	/** 직렬화 될 경우 */
	[OnSerializing]
	private void OnSerializingMethod(StreamingContext a_oContext) {
		this.OnBeforeSerialize();
    }

	/** 역직렬화 되었을 경우 */
	[OnDeserialized]
	private void OnDeserializedMethod(StreamingContext a_oContext) {
		this.OnAfterDeserialize();
	}
#endregion // 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using MessagePack;

/** 공용 기본 정보 */
[Union(0, typeof(CCommonAppInfo))]
[Union(1, typeof(CCommonUserInfo))]
[Union(2, typeof(CCommonGameInfo))]
[MessagePackObject]
[System.Serializable]
public abstract partial class CCommonBaseInfo : IMessagePackSerializationCallbackReceiver {
	#region 변수
	[Key(0)] public Dictionary<string, string> m_oStrDict = new Dictionary<string, string>();
	#endregion // 변수

	#region 상수
	private const string KEY_VER = "Ver";
	private const string KEY_SAVE_TIME = "SaveTime";
	#endregion // 상수

	#region 프로퍼티
	[IgnoreMember]
	public System.Version Ver {
		get { return System.Version.Parse(m_oStrDict.GetValueOrDefault(KEY_VER, KCDefine.B_DEF_VER)); }
		set { m_oStrDict.ExReplaceVal(KEY_VER, value.ToString(KCDefine.B_VAL_3_INT)); }
	}

	[IgnoreMember]
	public System.DateTime SaveTime {
		get { return this.SaveTimeStr.ExIsValid() ? this.CorrectSaveTimeStr.ExToTime(KCDefine.B_DATE_T_FMT_SLASH_YYYY_MM_DD_HH_MM_SS) : System.DateTime.Now; }
		set { m_oStrDict.ExReplaceVal(KEY_SAVE_TIME, value.ExToLongStr()); }
	}

	[IgnoreMember] private string SaveTimeStr => m_oStrDict.GetValueOrDefault(KEY_SAVE_TIME, string.Empty);
	[IgnoreMember] private string CorrectSaveTimeStr => this.SaveTimeStr.Contains(KCDefine.B_TOKEN_SLASH) ? this.SaveTimeStr : this.SaveTimeStr.ExToTime(KCDefine.B_DATE_T_FMT_YYYY_MM_DD_HH_MM_SS).ExToLongStr();
	#endregion // 프로퍼티

	#region IMessagePackSerializationCallbackReceiver
	/** 직렬화 될 경우 */
	public virtual void OnBeforeSerialize() {
		this.SaveTime = System.DateTime.Now;
	}

	/** 역직렬화 되었을 경우 */
	public virtual void OnAfterDeserialize() {
		m_oStrDict = m_oStrDict ?? new Dictionary<string, string>();
	}
	#endregion // IMessagePackSerializationCallbackReceiver

	#region 함수
	/** 생성자 */
	public CCommonBaseInfo(System.Version a_stVer) {
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

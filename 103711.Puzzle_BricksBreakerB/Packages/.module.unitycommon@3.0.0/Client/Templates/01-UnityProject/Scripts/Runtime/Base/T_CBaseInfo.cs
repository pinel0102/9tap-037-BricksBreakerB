#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.Runtime.Serialization;
using MessagePack;

#if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
using Newtonsoft.Json;
#endif // #if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE

/** 기본 정보 */
[MessagePackObject]
[System.Serializable]
public partial struct STBaseInfo : System.ICloneable, IMessagePackSerializationCallbackReceiver {
#region 변수
	[Key(0)] public Dictionary<string, string> m_oStrDict;
#endregion // 변수

#region ICloneable
	/** 사본 객체를 생성한다 */
	public object Clone() {
		var stBaseInfo = new STBaseInfo(null);
		this.SetupCloneInst(ref stBaseInfo);

		stBaseInfo.OnAfterDeserialize();
		return stBaseInfo;
	}
#endregion // ICloneable

#region IMessagePackSerializationCallbackReceiver
	/** 직렬화 될 경우 */
	public void OnBeforeSerialize() {
		// Do Something
	}

	/** 역직렬화 되었을 경우 */
	public void OnAfterDeserialize() {
		m_oStrDict = m_oStrDict ?? new Dictionary<string, string>();
	}
#endregion // IMessagePackSerializationCallbackReceiver

#region 함수
	/** 생성자 */
	public STBaseInfo(Dictionary<string, string> a_oStrDict) {
		m_oStrDict = a_oStrDict ?? new Dictionary<string, string>();
	}

	/** 사본 객체를 설정한다 */
	private void SetupCloneInst(ref STBaseInfo a_stOutBaseInfo) {
		m_oStrDict.ExCopyTo(a_stOutBaseInfo.m_oStrDict, (a_oStr) => a_oStr);
	}
#endregion // 함수

#region 조건부 함수
#if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
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
#endif // #if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
#endregion // 조건부 함수
}

/** 기본 정보 */
[Union(0, typeof(CAppInfo))]
[Union(1, typeof(CUserInfo))]
[Union(2, typeof(CGameInfo))]
[Union(3, typeof(CClearInfo))]
[Union(4, typeof(CTargetInfo))]
[MessagePackObject]
[System.Serializable]
public abstract partial class CBaseInfo : IMessagePackSerializationCallbackReceiver {
#region 변수
	[Key(0)] public Dictionary<string, string> m_oStrDict = new Dictionary<string, string>();
#endregion // 변수

#region 상수
	private const string KEY_VER = "Ver";
	private const string KEY_SAVE_TIME = "SaveTime";
#endregion // 상수

#region 프로퍼티
#if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
	[JsonIgnore]
	[IgnoreMember]
	public System.Version Ver {
		get { return System.Version.Parse(m_oStrDict.GetValueOrDefault(KEY_VER, KCDefine.B_DEF_VER)); }
		set { m_oStrDict.ExReplaceVal(KEY_VER, value.ToString(KCDefine.B_VAL_3_INT)); }
	}

	[JsonIgnore]
	[IgnoreMember]
	public System.DateTime SaveTime {
		get { return this.SaveTimeStr.ExIsValid() ? this.CorrectSaveTimeStr.ExToTime(KCDefine.B_DATE_T_FMT_SLASH_YYYY_MM_DD_HH_MM_SS) : System.DateTime.Now; }
		set { m_oStrDict.ExReplaceVal(KEY_SAVE_TIME, value.ExToLongStr()); }
	}

	[JsonIgnore][IgnoreMember] public virtual bool IsIgnoreVer => false;
	[JsonIgnore][IgnoreMember] public virtual bool IsIgnoreSaveTime => false;

	[JsonIgnore][IgnoreMember] private string SaveTimeStr => m_oStrDict.GetValueOrDefault(KEY_SAVE_TIME, string.Empty);
	[JsonIgnore][IgnoreMember] private string CorrectSaveTimeStr => this.SaveTimeStr.Contains(KCDefine.B_TOKEN_SLASH) ? this.SaveTimeStr : this.SaveTimeStr.ExToTime(KCDefine.B_DATE_T_FMT_YYYY_MM_DD_HH_MM_SS).ExToLongStr();
#else
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

	[IgnoreMember] public virtual bool IsIgnoreVer => false;
	[IgnoreMember] public virtual bool IsIgnoreSaveTime => false;

	[IgnoreMember] private string SaveTimeStr => m_oStrDict.GetValueOrDefault(KEY_SAVE_TIME, string.Empty);
	[IgnoreMember] private string CorrectSaveTimeStr => this.SaveTimeStr.Contains(KCDefine.B_TOKEN_SLASH) ? this.SaveTimeStr : this.SaveTimeStr.ExToTime(KCDefine.B_DATE_T_FMT_YYYY_MM_DD_HH_MM_SS).ExToLongStr();
#endif // #if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
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
#endregion // 함수

#region 조건부 함수
#if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
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
#endif // #if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
#endregion // 조건부 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

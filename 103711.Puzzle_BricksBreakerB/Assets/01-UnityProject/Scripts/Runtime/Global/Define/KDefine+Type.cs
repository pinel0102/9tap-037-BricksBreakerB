using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using MessagePack;

#if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
using Newtonsoft.Json;
#endif // #if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE

#region 기본
/** 타겟 정보 */
[MessagePackObject]
[System.Serializable]
public struct STTargetInfo : System.IEquatable<STTargetInfo> {
	[Key(1)] public int m_nKinds;
	[Key(11)] public ETargetKinds m_eTargetKinds;
	[Key(12)] public EKindsGroupType m_eKindsGroupType;

	[Key(21)] public STValInfo m_stValInfo01;
	[Key(22)] public STValInfo m_stValInfo02;
	[Key(23)] public STValInfo m_stValInfo03;

#region 상수
	public static readonly STTargetInfo INVALID = new STTargetInfo() {
		m_nKinds = KCDefine.B_IDX_INVALID, m_eTargetKinds = ETargetKinds.NONE, m_eKindsGroupType = EKindsGroupType.NONE, m_stValInfo01 = STValInfo.INVALID, m_stValInfo02 = STValInfo.INVALID, m_stValInfo03 = STValInfo.INVALID
	};
#endregion // 상수

#region 프로퍼티
#if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
	[JsonIgnore][IgnoreMember] public int Kinds => m_nKinds.ExKindsToCorrectKinds(m_eKindsGroupType);
	[JsonIgnore][IgnoreMember] public ulong UniqueTargetInfoID => Factory.MakeUTargetInfoID(m_eTargetKinds, m_nKinds);

	[JsonIgnore][IgnoreMember] public ETargetType TargetType => (ETargetType)((int)m_eTargetKinds).ExKindsToType();
	[JsonIgnore][IgnoreMember] public ETargetKinds BaseTargetKinds => (ETargetKinds)((int)m_eTargetKinds).ExKindsToSubKindsType();
#else
	[IgnoreMember] public int Kinds => m_nKinds.ExKindsToCorrectKinds(m_eKindsGroupType);
	[IgnoreMember] public ulong UniqueTargetInfoID => Factory.MakeUTargetInfoID(m_eTargetKinds, m_nKinds);
	
	[IgnoreMember] public ETargetType TargetType => (ETargetType)((int)m_eTargetKinds).ExKindsToType();
	[IgnoreMember] public ETargetKinds BaseTargetKinds => (ETargetKinds)((int)m_eTargetKinds).ExKindsToSubKindsType();
#endif // #if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
#endregion // 프로퍼티

#region IEquatable
	/** 동일 여부를 검사한다 */
	public bool Equals(STTargetInfo a_stTargetInfo) {
		return m_nKinds == a_stTargetInfo.m_nKinds && m_eTargetKinds == a_stTargetInfo.m_eTargetKinds && m_eKindsGroupType == a_stTargetInfo.m_eKindsGroupType && m_stValInfo01.Equals(a_stTargetInfo.m_stValInfo01) && m_stValInfo02.Equals(a_stTargetInfo.m_stValInfo02) && m_stValInfo03.Equals(a_stTargetInfo.m_stValInfo03);
	}
#endregion // IEquatable

#region 함수
	/** 생성자 */
	public STTargetInfo(SimpleJSON.JSONNode a_oTargetInfo, int a_nSrcIdx = KCDefine.B_VAL_0_INT) {
		m_nKinds = a_oTargetInfo[a_nSrcIdx + KCDefine.B_VAL_2_INT].ExIsValid() ? a_oTargetInfo[a_nSrcIdx + KCDefine.B_VAL_2_INT].AsInt : KCDefine.B_IDX_INVALID;
		m_eTargetKinds = a_oTargetInfo[a_nSrcIdx + KCDefine.B_VAL_0_INT].ExIsValid() ? (ETargetKinds)a_oTargetInfo[a_nSrcIdx + KCDefine.B_VAL_0_INT].AsInt : ETargetKinds.NONE;
		m_eKindsGroupType = a_oTargetInfo[a_nSrcIdx + KCDefine.B_VAL_1_INT].ExIsValid() ? (EKindsGroupType)a_oTargetInfo[a_nSrcIdx + KCDefine.B_VAL_1_INT].AsInt : EKindsGroupType.NONE;

		m_stValInfo01 = new STValInfo(a_oTargetInfo, a_nSrcIdx + KCDefine.B_VAL_3_INT);
		m_stValInfo02 = new STValInfo(a_oTargetInfo, a_nSrcIdx + KCDefine.B_VAL_5_INT);
		m_stValInfo03 = new STValInfo(a_oTargetInfo, a_nSrcIdx + KCDefine.B_VAL_7_INT);
	}

	/** 생성자 */
	public STTargetInfo(ETargetKinds a_eTargetKinds, int a_nKinds, STValInfo a_stValInfo, EKindsGroupType a_eKindsGroupType = EKindsGroupType.NONE) : this(a_eTargetKinds, a_nKinds, a_stValInfo, STValInfo.INVALID, a_eKindsGroupType) {
		// Do Something
	}

	public STTargetInfo(ETargetKinds a_eTargetKinds, int a_nKinds, STValInfo a_stValInfo01, STValInfo a_stValInfo02, EKindsGroupType a_eKindsGroupType = EKindsGroupType.NONE) : this(a_eTargetKinds, a_nKinds, a_stValInfo01, a_stValInfo02, STValInfo.INVALID, a_eKindsGroupType) {
		// Do Something
	}

	/** 생성자 */
	public STTargetInfo(ETargetKinds a_eTargetKinds, int a_nKinds, STValInfo a_stValInfo01, STValInfo a_stValInfo02, STValInfo a_stValInfo03, EKindsGroupType a_eKindsGroupType = EKindsGroupType.NONE) {
		m_nKinds = a_nKinds;
		m_eTargetKinds = a_eTargetKinds;
		m_eKindsGroupType = a_eKindsGroupType;

		m_stValInfo01 = a_stValInfo01;
		m_stValInfo02 = a_stValInfo02;
		m_stValInfo03 = a_stValInfo03;
	}
#endregion // 함수

#region 조건부 함수
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 타겟 정보를 저장한다 */
	public void SaveTargetInfo(SimpleJSON.JSONNode a_oOutTargetInfo, int a_nSrcIdx = KCDefine.B_VAL_0_INT) {
		a_oOutTargetInfo[a_nSrcIdx + KCDefine.B_VAL_2_INT] = $"{(int)m_nKinds}";
		a_oOutTargetInfo[a_nSrcIdx + KCDefine.B_VAL_0_INT] = $"{(int)m_eTargetKinds}";
		a_oOutTargetInfo[a_nSrcIdx + KCDefine.B_VAL_1_INT] = $"{(int)m_eKindsGroupType}";

		m_stValInfo01.SaveValInfo(a_oOutTargetInfo, a_nSrcIdx + KCDefine.B_VAL_3_INT);
		m_stValInfo02.SaveValInfo(a_oOutTargetInfo, a_nSrcIdx + KCDefine.B_VAL_5_INT);
		m_stValInfo03.SaveValInfo(a_oOutTargetInfo, a_nSrcIdx + KCDefine.B_VAL_7_INT);
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
#endregion // 조건부 함수
}

/** 타입 랩퍼 */
[MessagePackObject]
public struct STTypeWrapper {
	[Key(51)] public List<ulong> m_oULevelIDList;
	[Key(161)] public Dictionary<int, Dictionary<int, Dictionary<int, CLevelInfo>>> m_oLevelInfoDictContainer;
}
#endregion // 기본
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

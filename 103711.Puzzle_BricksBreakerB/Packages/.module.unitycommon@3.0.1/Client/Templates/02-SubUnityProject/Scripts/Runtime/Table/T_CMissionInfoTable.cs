#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.IO;

/** 미션 정보 */
[System.Serializable]
public struct STMissionInfo {
	public STCommonInfo m_stCommonInfo;

	public EMissionKinds m_eMissionKinds;
	public EMissionKinds m_ePrevMissionKinds;
	public EMissionKinds m_eNextMissionKinds;

	public List<ERewardKinds> m_oRewardKindsList;

#region 상수
	public static STMissionInfo INVALID = new STMissionInfo() {
		m_eMissionKinds = EMissionKinds.NONE, m_ePrevMissionKinds = EMissionKinds.NONE, m_eNextMissionKinds = EMissionKinds.NONE
	};
#endregion // 상수

#region 프로퍼티
	public EMissionType MissionType => (EMissionType)((int)m_eMissionKinds).ExKindsToType();
	public EMissionKinds BaseMissionKinds => (EMissionKinds)((int)m_eMissionKinds).ExKindsToSubKindsType();
#endregion // 프로퍼티

#region 함수
	/** 생성자 */
	public STMissionInfo(SimpleJSON.JSONNode a_oMissionInfo) {
		m_stCommonInfo = new STCommonInfo(a_oMissionInfo);

		m_eMissionKinds = a_oMissionInfo[KCDefine.U_KEY_MISSION_KINDS].ExIsValid() ? (EMissionKinds)a_oMissionInfo[KCDefine.U_KEY_MISSION_KINDS].AsInt : EMissionKinds.NONE;
		m_ePrevMissionKinds = a_oMissionInfo[KCDefine.U_KEY_PREV_MISSION_KINDS].ExIsValid() ? (EMissionKinds)a_oMissionInfo[KCDefine.U_KEY_PREV_MISSION_KINDS].AsInt : EMissionKinds.NONE;
		m_eNextMissionKinds = a_oMissionInfo[KCDefine.U_KEY_NEXT_MISSION_KINDS].ExIsValid() ? (EMissionKinds)a_oMissionInfo[KCDefine.U_KEY_NEXT_MISSION_KINDS].AsInt : EMissionKinds.NONE;

		m_oRewardKindsList = Factory.MakeVals(a_oMissionInfo, KCDefine.U_KEY_FMT_REWARD_KINDS, (a_oJSONNode) => (ERewardKinds)a_oJSONNode.AsInt);
	}
#endregion // 함수

#region 조건부 함수
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 미션 정보를 저장한다 */
	public void SaveMissionInfo(SimpleJSON.JSONNode a_oOutMissionInfo) {
		m_stCommonInfo.SaveCommonInfo(a_oOutMissionInfo);

		a_oOutMissionInfo[KCDefine.U_KEY_MISSION_KINDS] = $"{(int)m_eMissionKinds}";
		a_oOutMissionInfo[KCDefine.U_KEY_PREV_MISSION_KINDS] = $"{(int)m_ePrevMissionKinds}";
		a_oOutMissionInfo[KCDefine.U_KEY_NEXT_MISSION_KINDS] = $"{(int)m_eNextMissionKinds}";

		Func.SaveVals(m_oRewardKindsList, KCDefine.U_KEY_FMT_REWARD_KINDS, (a_eRewardKinds) => $"{(int)a_eRewardKinds}", a_oOutMissionInfo);
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
#endregion // 조건부 함수
}

/** 미션 정보 테이블 */
public partial class CMissionInfoTable : CSingleton<CMissionInfoTable> {
#region 프로퍼티
	public Dictionary<EMissionKinds, STMissionInfo> MissionInfoDict { get; } = new Dictionary<EMissionKinds, STMissionInfo>();
#endregion // 프로퍼티

#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetMissionInfos();
	}

	/** 미션 정보를 리셋한다 */
	public virtual void ResetMissionInfos() {
		this.MissionInfoDict.Clear();
	}

	/** 미션 정보를 리셋한다 */
	public virtual void ResetMissionInfos(string a_oJSONStr) {
		this.ResetMissionInfos();
		this.DoLoadMissionInfos(a_oJSONStr);
	}

	/** 미션 정보를 반환한다 */
	public STMissionInfo GetMissionInfo(EMissionKinds a_eMissionKinds) {
		bool bIsValid = this.TryGetMissionInfo(a_eMissionKinds, out STMissionInfo stMissionInfo);
		CAccess.Assert(bIsValid);

		return stMissionInfo;
	}

	/** 미션 정보를 반환한다 */
	public bool TryGetMissionInfo(EMissionKinds a_eMissionKinds, out STMissionInfo a_stOutMissionInfo) {
		a_stOutMissionInfo = this.MissionInfoDict.GetValueOrDefault(a_eMissionKinds, STMissionInfo.INVALID);
		return this.MissionInfoDict.ContainsKey(a_eMissionKinds);
	}

	/** 미션 정보를 로드한다 */
	public Dictionary<EMissionKinds, STMissionInfo> LoadMissionInfos() {
		this.ResetMissionInfos();
		return this.LoadMissionInfos(Access.MissionInfoTableLoadPath);
	}

	/** 미션 정보를 저장한다 */
	public void SaveMissionInfos(string a_oJSONStr, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oJSONStr != null);

		// JSON 문자열이 존재 할 경우
		if(a_oJSONStr != null) {
			this.ResetMissionInfos(a_oJSONStr);

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
			CFunc.WriteStr(Access.MissionInfoTableSavePath, a_oJSONStr, false);
#else
			CFunc.WriteStr(Access.MissionInfoTableSavePath, a_oJSONStr, true);
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)

#if UNITY_ANDROID && (DEBUG || DEVELOPMENT)
			CUnityMsgSender.Inst.SendShowToastMsg($"CMissionInfoTable.SaveMissionInfos: {File.Exists(Access.MissionInfoTableSavePath)}");
#endif // #if UNITY_ANDROID && (DEBUG || DEVELOPMENT)
		}
	}

	/** JSON 노드를 설정한다 */
	private void SetupJSONNodes(SimpleJSON.JSONNode a_oJSONNode, out SimpleJSON.JSONNode a_oOutCommonInfos) {
		var oSheetNameDictContainer = Access.GetSheetNames(this.GetType(), Access.MissionTableInfo);
		a_oOutCommonInfos = a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_COMMON]].ExIsValid() ? a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_COMMON]] : KCDefine.B_EMPTY_JSON_ARRAY;
	}

	/** 미션 정보를 로드한다 */
	private Dictionary<EMissionKinds, STMissionInfo> LoadMissionInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		return this.DoLoadMissionInfos(this.LoadMissionInfosJSONStr(a_oFilePath));
	}

	/** 미션 정보 JSON 문자열을 로드한다 */
	private string LoadMissionInfosJSONStr(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
		return File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, false) : CFunc.ReadStrFromRes(a_oFilePath, false);
#else
		return File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, true) : CFunc.ReadStrFromRes(a_oFilePath, false);
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 미션 정보를 로드한다 */
	private Dictionary<EMissionKinds, STMissionInfo> DoLoadMissionInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		this.SetupJSONNodes(SimpleJSON.JSON.Parse(a_oJSONStr), out SimpleJSON.JSONNode oCommonInfos);

		for(int i = 0; i < oCommonInfos.Count; ++i) {
			var stMissionInfo = new STMissionInfo(oCommonInfos[i]);

			// 미션 정보 추가 가능 할 경우
			if(stMissionInfo.m_eMissionKinds.ExIsValid() && (!this.MissionInfoDict.ContainsKey(stMissionInfo.m_eMissionKinds) || oCommonInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
				this.MissionInfoDict.ExReplaceVal(stMissionInfo.m_eMissionKinds, stMissionInfo);
			}
		}

		return this.MissionInfoDict;
	}
#endregion // 함수

#region 조건부 함수
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 미션 정보를 저장한다 */
	public void SaveMissionInfos() {
		var oMissionInfos = SimpleJSON.JSONNode.Parse(this.LoadMissionInfosJSONStr(Access.MissionInfoTableLoadPath));
		this.SetupJSONNodes(oMissionInfos, out SimpleJSON.JSONNode oCommonInfos);

		for(int i = 0; i < oCommonInfos.Count; ++i) {
			var oMissionKinds = oCommonInfos[i][KCDefine.U_KEY_MISSION_KINDS].ExIsValid() ? (EMissionKinds)oCommonInfos[i][KCDefine.U_KEY_MISSION_KINDS].AsInt : EMissionKinds.NONE;

			// 미션 정보가 존재 할 경우
			if(this.MissionInfoDict.TryGetValue(oMissionKinds, out STMissionInfo stMissionInfo)) {
				stMissionInfo.SaveMissionInfo(oCommonInfos[i]);
			}
		}

		this.SaveMissionInfos(oMissionInfos.ToString());
	}

	/** 미션 정보 값을 생성한다 */
	public Dictionary<string, List<List<string>>> MakeMissionInfoVals() {
		var oCommonKeyInfoList = CCollectionManager.Inst.SpawnList<STKeyInfo>();
		var oMissionInfoValDictContainer = new Dictionary<string, List<List<string>>>();

		try {
			this.SetupKeyInfos(oCommonKeyInfoList);
			this.SetupJSONNodes(SimpleJSON.JSONNode.Parse(this.LoadMissionInfosJSONStr(Access.MissionInfoTableSavePath)), out SimpleJSON.JSONNode oCommonInfos);

			oMissionInfoValDictContainer.TryAdd(Access.GetSheetNames(this.GetType(), Access.MissionTableInfo)[KCDefine.B_KEY_COMMON], oCommonInfos.AsArray.ExToInfoVals(oCommonKeyInfoList));
		} finally {
			CCollectionManager.Inst.DespawnList(oCommonKeyInfoList);
		}

		return oMissionInfoValDictContainer;
	}

	/** 키 정보를 설정한다 */
	private void SetupKeyInfos(List<STKeyInfo> a_oOutCommonKeyInfoList) {
		KDefine.G_KEY_INFO_GOOGLE_SHEET_COMMON_LIST.ExCopyTo(a_oOutCommonKeyInfoList, (a_stKeyInfo) => a_stKeyInfo);
		Access.MissionTableInfo.m_oKeyInfoDictContainer[this.GetType()].GetValueOrDefault(KCDefine.B_KEY_COMMON)?.ExCopyTo(a_oOutCommonKeyInfoList, (a_stKeyInfo) => a_stKeyInfo, false, false);
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
#endregion // 조건부 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.IO;

/** 보상 정보 */
[System.Serializable]
public struct STRewardInfo {
	public STCommonInfo m_stCommonInfo;

	public ERewardKinds m_eRewardKinds;
	public ERewardKinds m_ePrevRewardKinds;
	public ERewardKinds m_eNextRewardKinds;
	public ERewardQuality m_eRewardQuality;

	public Dictionary<ulong, STTargetInfo> m_oAcquireTargetInfoDict;

#region 상수
	public static STRewardInfo INVALID = new STRewardInfo() {
		m_eRewardKinds = ERewardKinds.NONE, m_ePrevRewardKinds = ERewardKinds.NONE, m_eNextRewardKinds = ERewardKinds.NONE
	};
#endregion // 상수

#region 프로퍼티
	public ERewardType RewardType => (ERewardType)((int)m_eRewardKinds).ExKindsToType();
	public ERewardKinds BaseRewardKinds => (ERewardKinds)((int)m_eRewardKinds).ExKindsToSubKindsType();
#endregion // 프로퍼티

#region 함수
	/** 생성자 */
	public STRewardInfo(SimpleJSON.JSONNode a_oRewardInfo) {
		m_stCommonInfo = new STCommonInfo(a_oRewardInfo);

		m_eRewardKinds = a_oRewardInfo[KCDefine.U_KEY_REWARD_KINDS].ExIsValid() ? (ERewardKinds)a_oRewardInfo[KCDefine.U_KEY_REWARD_KINDS].AsInt : ERewardKinds.NONE;
		m_ePrevRewardKinds = a_oRewardInfo[KCDefine.U_KEY_PREV_REWARD_KINDS].ExIsValid() ? (ERewardKinds)a_oRewardInfo[KCDefine.U_KEY_PREV_REWARD_KINDS].AsInt : ERewardKinds.NONE;
		m_eNextRewardKinds = a_oRewardInfo[KCDefine.U_KEY_NEXT_REWARD_KINDS].ExIsValid() ? (ERewardKinds)a_oRewardInfo[KCDefine.U_KEY_NEXT_REWARD_KINDS].AsInt : ERewardKinds.NONE;
		m_eRewardQuality = a_oRewardInfo[KCDefine.U_KEY_REWARD_QUALITY].ExIsValid() ? (ERewardQuality)a_oRewardInfo[KCDefine.U_KEY_REWARD_QUALITY].AsInt : ERewardQuality.NONE;

		m_oAcquireTargetInfoDict = Factory.MakeTargetInfos(a_oRewardInfo, KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO);
	}
#endregion // 함수

#region 조건부 함수
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 보상 정보를 저장한다 */
	public void SaveRewardInfo(SimpleJSON.JSONNode a_oOutRewardInfo) {
		m_stCommonInfo.SaveCommonInfo(a_oOutRewardInfo);

		a_oOutRewardInfo[KCDefine.U_KEY_REWARD_KINDS] = $"{(int)m_eRewardKinds}";
		a_oOutRewardInfo[KCDefine.U_KEY_PREV_REWARD_KINDS] = $"{(int)m_ePrevRewardKinds}";
		a_oOutRewardInfo[KCDefine.U_KEY_NEXT_REWARD_KINDS] = $"{(int)m_eNextRewardKinds}";
		a_oOutRewardInfo[KCDefine.U_KEY_REWARD_QUALITY] = $"{(int)m_eRewardQuality}";

		Func.SaveTargetInfos(m_oAcquireTargetInfoDict, KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, a_oOutRewardInfo);
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
#endregion // 조건부 함수
}

/** 보상 정보 테이블 */
public partial class CRewardInfoTable : CSingleton<CRewardInfoTable> {
#region 프로퍼티
	public Dictionary<ERewardKinds, STRewardInfo> RewardInfoDict { get; } = new Dictionary<ERewardKinds, STRewardInfo>();
#endregion // 프로퍼티

#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetRewardInfos();
	}

	/** 보상 정보를 리셋한다 */
	public virtual void ResetRewardInfos() {
		this.RewardInfoDict.Clear();
	}

	/** 보상 정보를 리셋한다 */
	public virtual void ResetRewardInfos(string a_oJSONStr) {
		this.ResetRewardInfos();
		this.DoLoadRewardInfos(a_oJSONStr);
	}

	/** 보상 정보를 반환한다 */
	public STRewardInfo GetRewardInfo(ERewardKinds a_eRewardKinds) {
		bool bIsValid = this.TryGetRewardInfo(a_eRewardKinds, out STRewardInfo stRewardInfo);
		CAccess.Assert(bIsValid);

		return stRewardInfo;
	}

	/** 획득 타겟 정보를 반환한다 */
	public STTargetInfo GetAcquireTargetInfo(ERewardKinds a_eRewardKinds, ETargetKinds a_eTargetKinds, int a_nKinds) {
		bool bIsValid = this.TryGetAcquireTargetInfo(a_eRewardKinds, a_eTargetKinds, a_nKinds, out STTargetInfo stAcquireTargetInfo);
		CAccess.Assert(bIsValid);

		return stAcquireTargetInfo;
	}

	/** 보상 정보를 반환한다 */
	public bool TryGetRewardInfo(ERewardKinds a_eRewardKinds, out STRewardInfo a_stOutRewardInfo) {
		a_stOutRewardInfo = this.RewardInfoDict.GetValueOrDefault(a_eRewardKinds, STRewardInfo.INVALID);
		return this.RewardInfoDict.ContainsKey(a_eRewardKinds);
	}

	/** 획득 타겟 정보를 반환한다 */
	public bool TryGetAcquireTargetInfo(ERewardKinds a_eRewardKinds, ETargetKinds a_eTargetKinds, int a_nKinds, out STTargetInfo a_stOutAcquireTargetInfo) {
		a_stOutAcquireTargetInfo = this.TryGetRewardInfo(a_eRewardKinds, out STRewardInfo stRewardInfo) ? stRewardInfo.m_oAcquireTargetInfoDict.GetValueOrDefault(Factory.MakeUTargetInfoID(a_eTargetKinds, a_nKinds), STTargetInfo.INVALID) : STTargetInfo.INVALID;
		return a_stOutAcquireTargetInfo.m_eTargetKinds != ETargetKinds.NONE;
	}

	/** 보상 정보를 로드한다 */
	public Dictionary<ERewardKinds, STRewardInfo> LoadRewardInfos() {
		this.ResetRewardInfos();
		return this.LoadRewardInfos(Access.RewardInfoTableLoadPath);
	}

	/** 보상 정보를 저장한다 */
	public void SaveRewardInfos(string a_oJSONStr, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oJSONStr != null);

		// JSON 문자열이 존재 할 경우
		if(a_oJSONStr != null) {
			this.ResetRewardInfos(a_oJSONStr);

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
			CFunc.WriteStr(Access.RewardInfoTableSavePath, a_oJSONStr, false);
#else
			CFunc.WriteStr(Access.RewardInfoTableSavePath, a_oJSONStr, true);
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)

#if UNITY_ANDROID && (DEBUG || DEVELOPMENT)
			CUnityMsgSender.Inst.SendShowToastMsg($"CRewardInfoTable.SaveRewardInfos: {File.Exists(Access.RewardInfoTableSavePath)}");
#endif // #if UNITY_ANDROID && (DEBUG || DEVELOPMENT)
		}
	}

	/** JSON 노드를 설정한다 */
	private void SetupJSONNodes(SimpleJSON.JSONNode a_oJSONNode, out SimpleJSON.JSONNode a_oOutCommonInfos) {
		var oSheetNameDictContainer = Access.GetSheetNames(this.GetType(), Access.RewardTableInfo);
		a_oOutCommonInfos = a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_COMMON]].ExIsValid() ? a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_COMMON]] : KCDefine.B_EMPTY_JSON_ARRAY;
	}

	/** 보상 정보를 로드한다 */
	private Dictionary<ERewardKinds, STRewardInfo> LoadRewardInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		return this.DoLoadRewardInfos(this.LoadRewardInfosJSONStr(a_oFilePath));
	}

	/** 보상 정보 JSON 문자열을 로드한다 */
	private string LoadRewardInfosJSONStr(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
		return File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, false) : CFunc.ReadStrFromRes(a_oFilePath, false);
#else
		return File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, true) : CFunc.ReadStrFromRes(a_oFilePath, false);
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 보상 정보를 로드한다 */
	private Dictionary<ERewardKinds, STRewardInfo> DoLoadRewardInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		this.SetupJSONNodes(SimpleJSON.JSON.Parse(a_oJSONStr), out SimpleJSON.JSONNode oCommonInfos);

		for(int i = 0; i < oCommonInfos.Count; ++i) {
			var stRewardInfo = new STRewardInfo(oCommonInfos[i]);

			// 보상 정보 추가 가능 할 경우
			if(stRewardInfo.m_eRewardKinds.ExIsValid() && (!this.RewardInfoDict.ContainsKey(stRewardInfo.m_eRewardKinds) || oCommonInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
				this.RewardInfoDict.ExReplaceVal(stRewardInfo.m_eRewardKinds, stRewardInfo);
			}
		}

		return this.RewardInfoDict;
	}
#endregion // 함수

#region 조건부 함수
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 보상 정보를 저장한다 */
	public void SaveRewardInfos() {
		var oRewardInfos = SimpleJSON.JSONNode.Parse(this.LoadRewardInfosJSONStr(Access.RewardInfoTableLoadPath));
		this.SetupJSONNodes(oRewardInfos, out SimpleJSON.JSONNode oCommonInfos);

		for(int i = 0; i < oCommonInfos.Count; ++i) {
			var eRewardKinds = oCommonInfos[i][KCDefine.U_KEY_REWARD_KINDS].ExIsValid() ? (ERewardKinds)oCommonInfos[i][KCDefine.U_KEY_REWARD_KINDS].AsInt : ERewardKinds.NONE;

			// 보상 정보가 존재 할 경우
			if(this.RewardInfoDict.TryGetValue(eRewardKinds, out STRewardInfo stRewardInfo)) {
				stRewardInfo.SaveRewardInfo(oCommonInfos[i]);
			}
		}

		this.SaveRewardInfos(oRewardInfos.ToString());
	}

	/** 보상 정보 값을 생성한다 */
	public Dictionary<string, List<List<string>>> MakeRewardInfoVals() {
		var oCommonKeyInfoList = CCollectionManager.Inst.SpawnList<STKeyInfo>();
		var oRewardInfoValDictContainer = new Dictionary<string, List<List<string>>>();

		try {
			this.SetupKeyInfos(oCommonKeyInfoList);
			this.SetupJSONNodes(SimpleJSON.JSONNode.Parse(this.LoadRewardInfosJSONStr(Access.RewardInfoTableSavePath)), out SimpleJSON.JSONNode oCommonInfos);

			oRewardInfoValDictContainer.TryAdd(Access.GetSheetNames(this.GetType(), Access.RewardTableInfo)[KCDefine.B_KEY_COMMON], oCommonInfos.AsArray.ExToInfoVals(oCommonKeyInfoList));
		} finally {
			CCollectionManager.Inst.DespawnList(oCommonKeyInfoList);
		}

		return oRewardInfoValDictContainer;
	}

	/** 키 정보를 설정한다 */
	private void SetupKeyInfos(List<STKeyInfo> a_oOutCommonKeyInfoList) {
		KDefine.G_KEY_INFO_GOOGLE_SHEET_COMMON_LIST.ExCopyTo(a_oOutCommonKeyInfoList, (a_stKeyInfo) => a_stKeyInfo);
		Access.RewardTableInfo.m_oKeyInfoDictContainer[this.GetType()].GetValueOrDefault(KCDefine.B_KEY_COMMON)?.ExCopyTo(a_oOutCommonKeyInfoList, (a_stKeyInfo) => a_stKeyInfo, false, false);
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
#endregion // 조건부 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

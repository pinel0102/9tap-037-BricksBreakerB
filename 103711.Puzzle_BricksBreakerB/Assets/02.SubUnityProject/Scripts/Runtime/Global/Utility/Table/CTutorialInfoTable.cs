using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.IO;

/** 튜토리얼 정보 */
[System.Serializable]
public struct STTutorialInfo {
	public STCommonInfo m_stCommonInfo;

	public ETutorialKinds m_eTutorialKinds;
	public ETutorialKinds m_ePrevTutorialKinds;
	public ETutorialKinds m_eNextTutorialKinds;

	public List<string> m_oStrList;
	public List<ERewardKinds> m_oRewardKindsList;

	#region 상수
	public static STTutorialInfo INVALID = new STTutorialInfo() {
		m_eTutorialKinds = ETutorialKinds.NONE, m_ePrevTutorialKinds = ETutorialKinds.NONE, m_eNextTutorialKinds = ETutorialKinds.NONE
	};
	#endregion // 상수

	#region 프로퍼티
	public ETutorialType TutorialType => (ETutorialType)((int)m_eTutorialKinds).ExKindsToType();
	public ETutorialKinds BaseTutorialKinds => (ETutorialKinds)((int)m_eTutorialKinds).ExKindsToSubKindsType();
	#endregion // 프로퍼티

	#region 함수
	/** 생성자 */
	public STTutorialInfo(SimpleJSON.JSONNode a_oTutorialInfo) {
		m_stCommonInfo = new STCommonInfo(a_oTutorialInfo);

		m_eTutorialKinds = a_oTutorialInfo[KCDefine.U_KEY_TUTORIAL_KINDS].ExIsValid() ? (ETutorialKinds)a_oTutorialInfo[KCDefine.U_KEY_TUTORIAL_KINDS].AsInt : ETutorialKinds.NONE;
		m_ePrevTutorialKinds = a_oTutorialInfo[KCDefine.U_KEY_PREV_TUTORIAL_KINDS].ExIsValid() ? (ETutorialKinds)a_oTutorialInfo[KCDefine.U_KEY_PREV_TUTORIAL_KINDS].AsInt : ETutorialKinds.NONE;
		m_eNextTutorialKinds = a_oTutorialInfo[KCDefine.U_KEY_NEXT_TUTORIAL_KINDS].ExIsValid() ? (ETutorialKinds)a_oTutorialInfo[KCDefine.U_KEY_NEXT_TUTORIAL_KINDS].AsInt : ETutorialKinds.NONE;

		m_oStrList = Factory.MakeVals(a_oTutorialInfo, KCDefine.U_KEY_FMT_STRS, (a_oJSONNode) => a_oJSONNode.Value);
		m_oRewardKindsList = Factory.MakeVals(a_oTutorialInfo, KCDefine.U_KEY_FMT_REWARD_KINDS, (a_oJSONNode) => (ERewardKinds)a_oJSONNode.AsInt);
	}
	#endregion // 함수

	#region 조건부 함수
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 튜토리얼 정보를 저장한다 */
	public void SaveTutorialInfo(SimpleJSON.JSONNode a_oOutTutorialInfo) {
		m_stCommonInfo.SaveCommonInfo(a_oOutTutorialInfo);

		a_oOutTutorialInfo[KCDefine.U_KEY_TUTORIAL_KINDS] = $"{(int)m_eTutorialKinds}";
		a_oOutTutorialInfo[KCDefine.U_KEY_PREV_TUTORIAL_KINDS] = $"{(int)m_ePrevTutorialKinds}";
		a_oOutTutorialInfo[KCDefine.U_KEY_NEXT_TUTORIAL_KINDS] = $"{(int)m_eNextTutorialKinds}";

		Func.SaveVals(m_oStrList, KCDefine.U_KEY_FMT_STRS, (a_oStr) => a_oStr, a_oOutTutorialInfo);
		Func.SaveVals(m_oRewardKindsList, KCDefine.U_KEY_FMT_REWARD_KINDS, (a_eRewardKinds) => $"{(int)a_eRewardKinds}", a_oOutTutorialInfo);
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	#endregion // 조건부 함수
}

/** 튜토리얼 정보 테이블 */
public partial class CTutorialInfoTable : CSingleton<CTutorialInfoTable> {
	#region 프로퍼티
	public Dictionary<ETutorialKinds, STTutorialInfo> TutorialInfoDict { get; } = new Dictionary<ETutorialKinds, STTutorialInfo>();
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetTutorialInfos();
	}

	/** 튜토리얼 정보를 리셋한다 */
	public virtual void ResetTutorialInfos() {
		this.TutorialInfoDict.Clear();
	}

	/** 튜토리얼 정보를 리셋한다 */
	public virtual void ResetTutorialInfos(string a_oJSONStr) {
		this.ResetTutorialInfos();
		this.DoLoadTutorialInfos(a_oJSONStr);
	}

	/** 튜토리얼 정보를 반환한다 */
	public STTutorialInfo GetTutorialInfo(ETutorialKinds a_eTutorialKinds) {
		bool bIsValid = this.TryGetTutorialInfo(a_eTutorialKinds, out STTutorialInfo stTutorialInfo);
		CAccess.Assert(bIsValid);

		return stTutorialInfo;
	}

	/** 튜토리얼 정보를 반환한다 */
	public bool TryGetTutorialInfo(ETutorialKinds a_eTutorialKinds, out STTutorialInfo a_stOutTutorialInfo) {
		a_stOutTutorialInfo = this.TutorialInfoDict.GetValueOrDefault(a_eTutorialKinds, STTutorialInfo.INVALID);
		return this.TutorialInfoDict.ContainsKey(a_eTutorialKinds);
	}

	/** 튜토리얼 정보를 로드한다 */
	public Dictionary<ETutorialKinds, STTutorialInfo> LoadTutorialInfos() {
		this.ResetTutorialInfos();
		return this.LoadTutorialInfos(Access.TutorialInfoTableLoadPath);
	}

	/** 튜토리얼 정보를 저장한다 */
	public void SaveTutorialInfos(string a_oJSONStr, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oJSONStr != null);

		// JSON 문자열이 존재 할 경우
		if(a_oJSONStr != null) {
			this.ResetTutorialInfos(a_oJSONStr);
		}
	}

	/** JSON 노드를 설정한다 */
	private void SetupJSONNodes(SimpleJSON.JSONNode a_oJSONNode, out SimpleJSON.JSONNode a_oOutCommonInfos) {
		var oSheetNameDictContainer = Access.GetSheetNames(this.GetType(), Access.TutorialTableInfo);
		a_oOutCommonInfos = a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_COMMON]].ExIsValid() ? a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_COMMON]] : KCDefine.B_EMPTY_JSON_ARRAY;
	}

	/** 튜토리얼 정보를 로드한다 */
	private Dictionary<ETutorialKinds, STTutorialInfo> LoadTutorialInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		return this.DoLoadTutorialInfos(this.LoadTutorialInfosJSONStr(a_oFilePath));
	}

	/** 튜토리얼 정보 JSON 문자열을 로드한다 */
	private string LoadTutorialInfosJSONStr(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
		return File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, false) : CFunc.ReadStrFromRes(a_oFilePath, false);
#else
		return File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, true) : CFunc.ReadStrFromRes(a_oFilePath, false);
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 튜토리얼 정보를 로드한다 */
	private Dictionary<ETutorialKinds, STTutorialInfo> DoLoadTutorialInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		this.SetupJSONNodes(SimpleJSON.JSON.Parse(a_oJSONStr), out SimpleJSON.JSONNode oCommonInfos);

		for(int i = 0; i < oCommonInfos.Count; ++i) {
			var stTutorialInfo = new STTutorialInfo(oCommonInfos[i]);

			// 튜토리얼 정보 추가 가능 할 경우
			if(stTutorialInfo.m_eTutorialKinds.ExIsValid() && (!this.TutorialInfoDict.ContainsKey(stTutorialInfo.m_eTutorialKinds) || oCommonInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
				this.TutorialInfoDict.ExReplaceVal(stTutorialInfo.m_eTutorialKinds, stTutorialInfo);
			}
		}

		return this.TutorialInfoDict;
	}
	#endregion // 함수

	#region 조건부 함수
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 튜토리얼 정보를 저장한다 */
	public void SaveTutorialInfos(SimpleJSON.JSONNode a_oOutTutorialInfos) {
		this.SetupJSONNodes(a_oOutTutorialInfos, out SimpleJSON.JSONNode oCommonInfos);

		for(int i = 0; i < oCommonInfos.Count; ++i) {
			var eTutorialKinds = oCommonInfos[i][KCDefine.U_KEY_TUTORIAL_KINDS].ExIsValid() ? (ETutorialKinds)oCommonInfos[i][KCDefine.U_KEY_TUTORIAL_KINDS].AsInt : ETutorialKinds.NONE;

			// 튜토리얼 정보가 존재 할 경우
			if(this.TutorialInfoDict.ContainsKey(eTutorialKinds)) {
				this.TutorialInfoDict[eTutorialKinds].SaveTutorialInfo(oCommonInfos[i]);
			}
		}
	}

	/** 튜토리얼 정보 값을 생성한다 */
	public void MakeTutorialInfoVals(SimpleJSON.JSONNode a_oTutorialInfos, Dictionary<string, List<List<string>>> a_oOutTutorialInfoValDictContainer) {
		var oCommonKeyInfoList = CCollectionManager.Inst.SpawnList<STKeyInfo>();

		try {
			this.SetupKeyInfos(oCommonKeyInfoList);
			this.SetupJSONNodes(a_oTutorialInfos, out SimpleJSON.JSONNode oCommonInfos);

			a_oOutTutorialInfoValDictContainer.TryAdd(Access.GetSheetNames(this.GetType(), Access.TutorialTableInfo)[KCDefine.B_KEY_COMMON], oCommonInfos.AsArray.ExToInfoVals(oCommonKeyInfoList));
		} finally {
			CCollectionManager.Inst.DespawnList(oCommonKeyInfoList);
		}
	}

	/** 키 정보를 설정한다 */
	private void SetupKeyInfos(List<STKeyInfo> a_oOutCommonKeyInfoList) {
		KDefine.G_KEY_INFO_GOOGLE_SHEET_COMMON_LIST.ExCopyTo(a_oOutCommonKeyInfoList, (a_stKeyInfo) => a_stKeyInfo);
		Access.TutorialTableInfo.m_oKeyInfoDictContainer[this.GetType()].GetValueOrDefault(KCDefine.B_KEY_COMMON)?.ExCopyTo(a_oOutCommonKeyInfoList, (a_stKeyInfo) => a_stKeyInfo, false, false);
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	#endregion // 조건부 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

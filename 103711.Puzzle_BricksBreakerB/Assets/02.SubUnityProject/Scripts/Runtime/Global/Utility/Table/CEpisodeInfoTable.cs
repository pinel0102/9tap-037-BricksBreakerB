using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.IO;

/** 에피소드 정보 */
[System.Serializable]
public struct STEpisodeInfo {
	public STCommonInfo m_stCommonInfo;

	public int m_nNumSubEpisodes;
	public int m_nMaxNumEnemyObjs;
	public Vector3 m_stSize;

	public STIDInfo m_stIDInfo;
	public STIDInfo m_stPrevIDInfo;
	public STIDInfo m_stNextIDInfo;

	public EDifficulty m_eDifficulty;
	public EEpisodeKinds m_eEpisodeKinds;
	public ETutorialKinds m_eTutorialKinds;

	public List<ERewardKinds> m_oRewardKindsList;
	public List<STValInfo> m_oRecordValInfoList;

	public Dictionary<ulong, STTargetInfo> m_oClearTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oUnlockTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oDropItemTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oEnemyObjTargetInfoDict;

	#region 상수
	public static STEpisodeInfo INVALID = new STEpisodeInfo() {
		m_stIDInfo = STIDInfo.INVALID, m_stPrevIDInfo = STIDInfo.INVALID, m_stNextIDInfo = STIDInfo.INVALID
	};
	#endregion // 상수

	#region 프로퍼티
	public ulong ULevelID => CFactory.MakeULevelID(m_stIDInfo.m_nID01, m_stIDInfo.m_nID02, m_stIDInfo.m_nID03);
	public ulong PrevULevelID => CFactory.MakeULevelID(m_stPrevIDInfo.m_nID01, m_stPrevIDInfo.m_nID02, m_stPrevIDInfo.m_nID03);
	public ulong NextULevelID => CFactory.MakeULevelID(m_stNextIDInfo.m_nID01, m_stNextIDInfo.m_nID02, m_stNextIDInfo.m_nID03);

	public ulong UStageID => CFactory.MakeUStageID(m_stIDInfo.m_nID02, m_stIDInfo.m_nID03);
	public ulong PrevUStageID => CFactory.MakeUStageID(m_stPrevIDInfo.m_nID02, m_stPrevIDInfo.m_nID03);
	public ulong NextUStageID => CFactory.MakeUStageID(m_stNextIDInfo.m_nID02, m_stNextIDInfo.m_nID03);

	public ulong UChapterID => CFactory.MakeUChapterID(m_stIDInfo.m_nID03);
	public ulong PrevUChapterID => CFactory.MakeUChapterID(m_stPrevIDInfo.m_nID03);
	public ulong NextUChapterID => CFactory.MakeUChapterID(m_stNextIDInfo.m_nID03);

	public EEpisodeType EpisodeType => (EEpisodeType)((int)m_eEpisodeKinds).ExKindsToType();
	public EEpisodeKinds BaseEpisodeKinds => (EEpisodeKinds)((int)m_eEpisodeKinds).ExKindsToSubKindsType();

	public ETutorialType TutorialType => (ETutorialType)((int)m_eTutorialKinds).ExKindsToType();
	public ETutorialKinds BaseTutorialKinds => (ETutorialKinds)((int)m_eTutorialKinds).ExKindsToSubKindsType();
	#endregion // 프로퍼티

	#region 함수
	/** 생성자 */
	public STEpisodeInfo(SimpleJSON.JSONNode a_oEpisodeInfo) {
		m_stCommonInfo = new STCommonInfo(a_oEpisodeInfo);

		m_nNumSubEpisodes = a_oEpisodeInfo[KCDefine.U_KEY_NUM_SUB_EPISODES].ExIsValid() ? a_oEpisodeInfo[KCDefine.U_KEY_NUM_SUB_EPISODES].AsInt : KCDefine.B_VAL_0_INT;
		m_nMaxNumEnemyObjs = a_oEpisodeInfo[KCDefine.U_KEY_MAX_NUM_ENEMY_OBJS].ExIsValid() ? a_oEpisodeInfo[KCDefine.U_KEY_MAX_NUM_ENEMY_OBJS].AsInt : KCDefine.B_VAL_0_INT;
		m_stSize = a_oEpisodeInfo[KCDefine.U_KEY_SIZE].ExIsValid() ? new Vector3(a_oEpisodeInfo[KCDefine.U_KEY_SIZE][KCDefine.B_VAL_0_INT].AsFloat, a_oEpisodeInfo[KCDefine.U_KEY_SIZE][KCDefine.B_VAL_1_INT].AsFloat, a_oEpisodeInfo[KCDefine.U_KEY_SIZE][KCDefine.B_VAL_2_INT].AsFloat) : Vector3.zero;

		m_eDifficulty = a_oEpisodeInfo[KCDefine.U_KEY_DIFFICULTY].ExIsValid() ? (EDifficulty)a_oEpisodeInfo[KCDefine.U_KEY_DIFFICULTY].AsInt : EDifficulty.NONE;
		m_eEpisodeKinds = a_oEpisodeInfo[KCDefine.U_KEY_EPISODE_KINDS].ExIsValid() ? (EEpisodeKinds)a_oEpisodeInfo[KCDefine.U_KEY_EPISODE_KINDS].AsInt : EEpisodeKinds.NONE;
		m_eTutorialKinds = a_oEpisodeInfo[KCDefine.U_KEY_TUTORIAL_KINDS].ExIsValid() ? (ETutorialKinds)a_oEpisodeInfo[KCDefine.U_KEY_TUTORIAL_KINDS].AsInt : ETutorialKinds.NONE;

		m_stIDInfo = new STIDInfo(a_oEpisodeInfo, KCDefine.U_KEY_FMT_ID);
		m_stPrevIDInfo = new STIDInfo(a_oEpisodeInfo, KCDefine.U_KEY_FMT_PREV_ID);
		m_stNextIDInfo = new STIDInfo(a_oEpisodeInfo, KCDefine.U_KEY_FMT_NEXT_ID);

		m_oRewardKindsList = Factory.MakeVals(a_oEpisodeInfo, KCDefine.U_KEY_FMT_REWARD_KINDS, (a_oJSONNode) => (ERewardKinds)a_oJSONNode.AsInt);
		m_oRecordValInfoList = Factory.MakeValInfos(a_oEpisodeInfo, KCDefine.U_KEY_FMT_RECORD_VAL_INFO);

		m_oClearTargetInfoDict = Factory.MakeTargetInfos(a_oEpisodeInfo, KCDefine.U_KEY_FMT_CLEAR_TARGET_INFO);
		m_oUnlockTargetInfoDict = Factory.MakeTargetInfos(a_oEpisodeInfo, KCDefine.U_KEY_FMT_UNLOCK_TARGET_INFO);
		m_oDropItemTargetInfoDict = Factory.MakeTargetInfos(a_oEpisodeInfo, KCDefine.U_KEY_FMT_DROP_ITEM_TARGET_INFO);
		m_oEnemyObjTargetInfoDict = Factory.MakeTargetInfos(a_oEpisodeInfo, KCDefine.U_KEY_FMT_ENEMY_OBJ_TARGET_INFO);
	}
	#endregion // 함수

	#region 조건부 함수
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 에피소드 정보를 저장한다 */
	public void SaveEpisodeInfo(SimpleJSON.JSONNode a_oOutEpisodeInfo) {
		m_stCommonInfo.SaveCommonInfo(a_oOutEpisodeInfo);

		a_oOutEpisodeInfo[KCDefine.U_KEY_NUM_SUB_EPISODES] = $"{m_nNumSubEpisodes}";
		a_oOutEpisodeInfo[KCDefine.U_KEY_MAX_NUM_ENEMY_OBJS] = $"{m_nMaxNumEnemyObjs}";

		a_oOutEpisodeInfo[KCDefine.U_KEY_SIZE][KCDefine.B_VAL_0_INT] = $"{m_stSize.x:0.0}";
		a_oOutEpisodeInfo[KCDefine.U_KEY_SIZE][KCDefine.B_VAL_1_INT] = $"{m_stSize.y:0.0}";
		a_oOutEpisodeInfo[KCDefine.U_KEY_SIZE][KCDefine.B_VAL_2_INT] = $"{m_stSize.z:0.0}";

		a_oOutEpisodeInfo[KCDefine.U_KEY_DIFFICULTY] = $"{(int)m_eDifficulty}";
		a_oOutEpisodeInfo[KCDefine.U_KEY_EPISODE_KINDS] = $"{(int)m_eEpisodeKinds}";
		a_oOutEpisodeInfo[KCDefine.U_KEY_TUTORIAL_KINDS] = $"{(int)m_eTutorialKinds}";

		m_stIDInfo.SaveIDInfo(a_oOutEpisodeInfo, KCDefine.U_KEY_FMT_ID);
		m_stPrevIDInfo.SaveIDInfo(a_oOutEpisodeInfo, KCDefine.U_KEY_FMT_PREV_ID);
		m_stNextIDInfo.SaveIDInfo(a_oOutEpisodeInfo, KCDefine.U_KEY_FMT_NEXT_ID);

		Func.SaveVals(m_oRewardKindsList, KCDefine.U_KEY_FMT_REWARD_KINDS, (a_eRewardKinds) => $"{(int)a_eRewardKinds}", a_oOutEpisodeInfo);
		Func.SaveValInfos(m_oRecordValInfoList, KCDefine.U_KEY_FMT_RECORD_VAL_INFO, a_oOutEpisodeInfo);

		Func.SaveTargetInfos(m_oClearTargetInfoDict, KCDefine.U_KEY_FMT_CLEAR_TARGET_INFO, a_oOutEpisodeInfo);
		Func.SaveTargetInfos(m_oUnlockTargetInfoDict, KCDefine.U_KEY_FMT_UNLOCK_TARGET_INFO, a_oOutEpisodeInfo);
		Func.SaveTargetInfos(m_oDropItemTargetInfoDict, KCDefine.U_KEY_FMT_DROP_ITEM_TARGET_INFO, a_oOutEpisodeInfo);
		Func.SaveTargetInfos(m_oEnemyObjTargetInfoDict, KCDefine.U_KEY_FMT_ENEMY_OBJ_TARGET_INFO, a_oOutEpisodeInfo);
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	#endregion // 조건부 함수
}

/** 에피소드 정보 테이블 */
public partial class CEpisodeInfoTable : CSingleton<CEpisodeInfoTable> {
	#region 프로퍼티
	public Dictionary<ulong, STEpisodeInfo> LevelEpisodeInfoDict { get; } = new Dictionary<ulong, STEpisodeInfo>();
	public Dictionary<ulong, STEpisodeInfo> StageEpisodeInfoDict { get; } = new Dictionary<ulong, STEpisodeInfo>();
	public Dictionary<ulong, STEpisodeInfo> ChapterEpisodeInfoDict { get; } = new Dictionary<ulong, STEpisodeInfo>();
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetEpisodeInfos();
	}

	/** 에피소드 정보를 리셋한다 */
	public virtual void ResetEpisodeInfos() {
		this.LevelEpisodeInfoDict.Clear();
		this.StageEpisodeInfoDict.Clear();
		this.ChapterEpisodeInfoDict.Clear();
	}

	/** 에피소드 정보를 리셋한다 */
	public virtual void ResetEpisodeInfos(string a_oJSONStr) {
		this.ResetEpisodeInfos();
		this.DoLoadEpisodeInfos(a_oJSONStr);
	}

	/** 레벨 에피소드 정보를 반환한다 */
	public STEpisodeInfo GetLevelEpisodeInfo(int a_nLevelID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetLevelEpisodeInfo(a_nLevelID, out STEpisodeInfo stEpisodeInfo, a_nStageID, a_nChapterID);
		CAccess.Assert(bIsValid);

		return stEpisodeInfo;
	}

	/** 스테이지 에피소드 정보를 반환한다 */
	public STEpisodeInfo GetStageEpisodeInfo(int a_nStageID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetStageEpisodeInfo(a_nStageID, out STEpisodeInfo stEpisodeInfo, a_nChapterID);
		CAccess.Assert(bIsValid);

		return stEpisodeInfo;
	}

	/** 챕터 에피소드 정보를 반환한다 */
	public STEpisodeInfo GetChapterEpisodeInfo(int a_nChapterID) {
		bool bIsValid = this.TryGetChapterEpisodeInfo(a_nChapterID, out STEpisodeInfo stEpisodeInfo);
		CAccess.Assert(bIsValid);

		return stEpisodeInfo;
	}

	/** 레벨 에피소드 정보를 반환한다 */
	public bool TryGetLevelEpisodeInfo(int a_nLevelID, out STEpisodeInfo a_stOutEpisodeInfo, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		a_stOutEpisodeInfo = this.LevelEpisodeInfoDict.GetValueOrDefault(CFactory.MakeULevelID(a_nLevelID, a_nStageID, a_nChapterID), STEpisodeInfo.INVALID);
		return this.LevelEpisodeInfoDict.ContainsKey(CFactory.MakeULevelID(a_nLevelID, a_nStageID, a_nChapterID));
	}

	/** 스테이지 에피소드 정보를 반환한다 */
	public bool TryGetStageEpisodeInfo(int a_nStageID, out STEpisodeInfo a_stOutEpisodeInfo, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		a_stOutEpisodeInfo = this.StageEpisodeInfoDict.GetValueOrDefault(CFactory.MakeUStageID(a_nStageID, a_nChapterID), STEpisodeInfo.INVALID);
		return this.StageEpisodeInfoDict.ContainsKey(CFactory.MakeUStageID(a_nStageID, a_nChapterID));
	}

	/** 챕터 에피소드 정보를 반환한다 */
	public bool TryGetChapterEpisodeInfo(int a_nChapterID, out STEpisodeInfo a_stOutEpisodeInfo) {
		a_stOutEpisodeInfo = this.ChapterEpisodeInfoDict.GetValueOrDefault(CFactory.MakeUChapterID(a_nChapterID), STEpisodeInfo.INVALID);
		return this.ChapterEpisodeInfoDict.ContainsKey(CFactory.MakeUChapterID(a_nChapterID));
	}

	/** 에피소드 정보를 로드한다 */
	public (Dictionary<ulong, STEpisodeInfo>, Dictionary<ulong, STEpisodeInfo>, Dictionary<ulong, STEpisodeInfo>) LoadEpisodeInfos() {
		this.ResetEpisodeInfos();
		return this.LoadEpisodeInfos(Access.EpisodeInfoTableLoadPath);
	}

	/** 에피소드 정보를 저장한다 */
	public void SaveEpisodeInfos(string a_oJSONStr, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oJSONStr != null);

		// JSON 문자열이 존재 할 경우
		if(a_oJSONStr != null) {
			this.ResetEpisodeInfos(a_oJSONStr);
		}
	}

	/** JSON 노드를 설정한다 */
	private void SetupJSONNodes(SimpleJSON.JSONNode a_oJSONNode, out SimpleJSON.JSONNode a_oOutLevelEpisodeInfos, out SimpleJSON.JSONNode a_oOutStageEpisodeInfos, out SimpleJSON.JSONNode a_oOutChapterEpisodeInfos) {
		var oSheetNameDictContainer = Access.GetSheetNames(this.GetType(), Access.EpisodeTableInfo);
		a_oOutLevelEpisodeInfos = a_oJSONNode[oSheetNameDictContainer[KCDefine.U_KEY_LEVEL_EPISODE]].ExIsValid() ? a_oJSONNode[oSheetNameDictContainer[KCDefine.U_KEY_LEVEL_EPISODE]] : KCDefine.B_EMPTY_JSON_ARRAY;
		a_oOutStageEpisodeInfos = a_oJSONNode[oSheetNameDictContainer[KCDefine.U_KEY_STAGE_EPISODE]].ExIsValid() ? a_oJSONNode[oSheetNameDictContainer[KCDefine.U_KEY_STAGE_EPISODE]] : KCDefine.B_EMPTY_JSON_ARRAY;
		a_oOutChapterEpisodeInfos = a_oJSONNode[oSheetNameDictContainer[KCDefine.U_KEY_CHAPTER_EPISODE]].ExIsValid() ? a_oJSONNode[oSheetNameDictContainer[KCDefine.U_KEY_CHAPTER_EPISODE]] : KCDefine.B_EMPTY_JSON_ARRAY;
	}

	/** 에피소드 정보를 로드한다 */
	private (Dictionary<ulong, STEpisodeInfo>, Dictionary<ulong, STEpisodeInfo>, Dictionary<ulong, STEpisodeInfo>) LoadEpisodeInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		return this.DoLoadEpisodeInfos(this.LoadEpisodeInfosJSONStr(a_oFilePath));
	}

	/** 에피소드 정보 JSON 문자열을 로드한다 */
	private string LoadEpisodeInfosJSONStr(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
		return File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, false) : CFunc.ReadStrFromRes(a_oFilePath, false);
#else
		return File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, true) : CFunc.ReadStrFromRes(a_oFilePath, false);
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 에피소드 정보를 로드한다 */
	private (Dictionary<ulong, STEpisodeInfo>, Dictionary<ulong, STEpisodeInfo>, Dictionary<ulong, STEpisodeInfo>) DoLoadEpisodeInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		this.SetupJSONNodes(SimpleJSON.JSON.Parse(a_oJSONStr), out SimpleJSON.JSONNode oLevelEpisodeInfos, out SimpleJSON.JSONNode oStageEpisodeInfos, out SimpleJSON.JSONNode oChapterEpisodeInfos);

		for(int i = 0; i < oLevelEpisodeInfos.Count; ++i) {
			var stEpisodeInfo = new STEpisodeInfo(oLevelEpisodeInfos[i]);

			// 레벨 에피소드 정보 추가 가능 할 경우
			if(stEpisodeInfo.m_stIDInfo.m_nID01.ExIsValidIdx() && (!this.LevelEpisodeInfoDict.ContainsKey(stEpisodeInfo.m_stIDInfo.UniqueID01) || oLevelEpisodeInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
				this.LevelEpisodeInfoDict.ExReplaceVal(stEpisodeInfo.m_stIDInfo.UniqueID01, stEpisodeInfo);
			}
		}

		for(int i = 0; i < oStageEpisodeInfos.Count; ++i) {
			var stEpisodeInfo = new STEpisodeInfo(oStageEpisodeInfos[i]);

			// 스테이지 에피소드 정보 추가 가능 할 경우
			if(stEpisodeInfo.m_stIDInfo.m_nID02.ExIsValidIdx() && (!this.StageEpisodeInfoDict.ContainsKey(stEpisodeInfo.m_stIDInfo.UniqueID02) || oStageEpisodeInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
				this.StageEpisodeInfoDict.ExReplaceVal(stEpisodeInfo.m_stIDInfo.UniqueID02, stEpisodeInfo);
			}
		}

		for(int i = 0; i < oChapterEpisodeInfos.Count; ++i) {
			var stEpisodeInfo = new STEpisodeInfo(oChapterEpisodeInfos[i]);

			// 챕터 에피소드 정보 추가 가능 할 경우
			if(stEpisodeInfo.m_stIDInfo.m_nID03.ExIsValidIdx() && (!this.ChapterEpisodeInfoDict.ContainsKey(stEpisodeInfo.m_stIDInfo.UniqueID03) || oChapterEpisodeInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
				this.ChapterEpisodeInfoDict.ExReplaceVal(stEpisodeInfo.m_stIDInfo.UniqueID03, stEpisodeInfo);
			}
		}

		return (this.LevelEpisodeInfoDict, this.StageEpisodeInfoDict, this.ChapterEpisodeInfoDict);
	}
	#endregion // 함수

	#region 조건부 함수
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 에피소드 정보를 저장한다 */
	public void SaveEpisodeInfos(SimpleJSON.JSONNode a_oOutEpisodeInfos) {
		this.SetupJSONNodes(a_oOutEpisodeInfos, out SimpleJSON.JSONNode oLevelEpisodeInfos, out SimpleJSON.JSONNode oStageEpisodeInfos, out SimpleJSON.JSONNode oChapterEpisodeInfos);

		for(int i = 0; i < oLevelEpisodeInfos.Count; ++i) {
			var stIDInfo = new STIDInfo(oLevelEpisodeInfos[i], KCDefine.U_KEY_FMT_ID);

			// 레벨 에피소드 정보가 존재 할 경우
			if(this.LevelEpisodeInfoDict.ContainsKey(stIDInfo.UniqueID01)) {
				this.LevelEpisodeInfoDict[stIDInfo.UniqueID01].SaveEpisodeInfo(oLevelEpisodeInfos[i]);
			}
		}

		for(int i = 0; i < oStageEpisodeInfos.Count; ++i) {
			var stIDInfo = new STIDInfo(oStageEpisodeInfos[i], KCDefine.U_KEY_FMT_ID);

			// 스테이지 에피소드 정보가 존재 할 경우
			if(this.StageEpisodeInfoDict.ContainsKey(stIDInfo.UniqueID02)) {
				this.StageEpisodeInfoDict[stIDInfo.UniqueID02].SaveEpisodeInfo(oStageEpisodeInfos[i]);
			}
		}

		for(int i = 0; i < oChapterEpisodeInfos.Count; ++i) {
			var stIDInfo = new STIDInfo(oChapterEpisodeInfos[i], KCDefine.U_KEY_FMT_ID);

			// 챕터 에피소드 정보가 존재 할 경우
			if(this.ChapterEpisodeInfoDict.ContainsKey(stIDInfo.UniqueID03)) {
				this.ChapterEpisodeInfoDict[stIDInfo.UniqueID03].SaveEpisodeInfo(oChapterEpisodeInfos[i]);
			}
		}
	}

	/** 에피소드 정보 값을 설정한다 */
	public void MakeEpisodeInfoVals(SimpleJSON.JSONNode a_oEpisodeInfos, Dictionary<string, List<List<string>>> a_oOutEpisodeInfoValDictContainer) {
		var oLevelEpisodeKeyInfoList = CCollectionManager.Inst.SpawnList<STKeyInfo>();
		var oStageEpisodeKeyInfoList = CCollectionManager.Inst.SpawnList<STKeyInfo>();
		var oChapterEpisodeKeyInfoList = CCollectionManager.Inst.SpawnList<STKeyInfo>();

		try {
			this.SetupKeyInfos(oLevelEpisodeKeyInfoList, oStageEpisodeKeyInfoList, oChapterEpisodeKeyInfoList);
			this.SetupJSONNodes(a_oEpisodeInfos, out SimpleJSON.JSONNode oLevelEpisodeInfos, out SimpleJSON.JSONNode oStageEpisodeInfos, out SimpleJSON.JSONNode oChapterEpisodeInfos);

			a_oOutEpisodeInfoValDictContainer.TryAdd(Access.GetSheetNames(this.GetType(), Access.EpisodeTableInfo)[KCDefine.U_KEY_LEVEL_EPISODE], oLevelEpisodeInfos.AsArray.ExToInfoVals(oLevelEpisodeKeyInfoList));
			a_oOutEpisodeInfoValDictContainer.TryAdd(Access.GetSheetNames(this.GetType(), Access.EpisodeTableInfo)[KCDefine.U_KEY_STAGE_EPISODE], oStageEpisodeInfos.AsArray.ExToInfoVals(oStageEpisodeKeyInfoList));
			a_oOutEpisodeInfoValDictContainer.TryAdd(Access.GetSheetNames(this.GetType(), Access.EpisodeTableInfo)[KCDefine.U_KEY_CHAPTER_EPISODE], oChapterEpisodeInfos.AsArray.ExToInfoVals(oChapterEpisodeKeyInfoList));
		} finally {
			CCollectionManager.Inst.DespawnList(oLevelEpisodeKeyInfoList);
			CCollectionManager.Inst.DespawnList(oStageEpisodeKeyInfoList);
			CCollectionManager.Inst.DespawnList(oChapterEpisodeKeyInfoList);
		}
	}

	/** 키 정보를 설정한다 */
	private void SetupKeyInfos(List<STKeyInfo> a_oOutLevelEpisodeInfoList, List<STKeyInfo> a_oOutStageEpisodeInfoList, List<STKeyInfo> a_oOutChapterEpisodeInfoList) {
		KDefine.G_KEY_INFO_GOOGLE_SHEET_COMMON_LIST.ExCopyTo(a_oOutLevelEpisodeInfoList, (a_stKeyInfo) => a_stKeyInfo);
		KDefine.G_KEY_INFO_GOOGLE_SHEET_COMMON_LIST.ExCopyTo(a_oOutStageEpisodeInfoList, (a_stKeyInfo) => a_stKeyInfo);
		KDefine.G_KEY_INFO_GOOGLE_SHEET_COMMON_LIST.ExCopyTo(a_oOutChapterEpisodeInfoList, (a_stKeyInfo) => a_stKeyInfo);

		Access.EpisodeTableInfo.m_oKeyInfoDictContainer[this.GetType()].GetValueOrDefault(KCDefine.U_KEY_LEVEL_EPISODE)?.ExCopyTo(a_oOutLevelEpisodeInfoList, (a_stKeyInfo) => a_stKeyInfo, false, false);
		Access.EpisodeTableInfo.m_oKeyInfoDictContainer[this.GetType()].GetValueOrDefault(KCDefine.U_KEY_STAGE_EPISODE)?.ExCopyTo(a_oOutStageEpisodeInfoList, (a_stKeyInfo) => a_stKeyInfo, false, false);
		Access.EpisodeTableInfo.m_oKeyInfoDictContainer[this.GetType()].GetValueOrDefault(KCDefine.U_KEY_CHAPTER_EPISODE)?.ExCopyTo(a_oOutChapterEpisodeInfoList, (a_stKeyInfo) => a_stKeyInfo, false, false);
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	#endregion // 조건부 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

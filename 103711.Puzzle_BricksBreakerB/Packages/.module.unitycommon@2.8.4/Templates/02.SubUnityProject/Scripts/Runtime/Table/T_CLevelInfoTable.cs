#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using MessagePack;

#if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
using Newtonsoft.Json;
#endif // #if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE

/** 셀 정보 */
[MessagePackObject]
[System.Serializable]
public struct STCellInfo : System.ICloneable, IMessagePackSerializationCallbackReceiver {
#region 변수
	[Key(161)] public Dictionary<EObjType, List<EObjKinds>> m_oObjKindsDictContainer;

#if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
	[JsonIgnore][IgnoreMember][System.NonSerialized] public Vector3Int m_stIdx;
#else
	[IgnoreMember][System.NonSerialized] public Vector3Int m_stIdx;
#endif // #if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
#endregion // 변수

#region 상수
	public static readonly STCellInfo INVALID = new STCellInfo() {
		m_stIdx = KCDefine.B_IDX_INVALID_3D
	};
#endregion // 상수

#region ICloneable
	/** 사본 객체를 생성한다 */
	public object Clone() {
		var stCellInfo = new STCellInfo() {
			m_stIdx = this.m_stIdx, m_oObjKindsDictContainer = new Dictionary<EObjType, List<EObjKinds>>()
		};

		m_oObjKindsDictContainer.ExCopyTo(stCellInfo.m_oObjKindsDictContainer, this.MakeCloneObjKinds);

		stCellInfo.OnAfterDeserialize();
		return stCellInfo;
	}
#endregion // ICloneable

#region IMessagePackSerializationCallbackReceiver
	/** 직렬화 될 경우 */
	public void OnBeforeSerialize() {
		// Do Something
	}

	/** 역직렬화 되었을 경우 */
	public void OnAfterDeserialize() {
		m_oObjKindsDictContainer = m_oObjKindsDictContainer ?? new Dictionary<EObjType, List<EObjKinds>>();
	}
#endregion // IMessagePackSerializationCallbackReceiver

#region 함수
	/** 사본 객체 종류를 생성한다 */
	private List<EObjKinds> MakeCloneObjKinds(List<EObjKinds> a_oObjKindsList) {
		var oObjKindsList = new List<EObjKinds>();
		a_oObjKindsList.ExCopyTo(oObjKindsList, (a_eObjKinds) => a_eObjKinds);

		return oObjKindsList;
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

/** 레벨 정보 */
[MessagePackObject]
[System.Serializable]
public partial class CLevelInfo : CBaseInfo, System.ICloneable {
#region 상수
	private const string KEY_CELL_INFO_VER = "CellInfoVer";
#endregion // 상수

#region 변수
	[Key(165)] public Dictionary<int, Dictionary<int, STCellInfo>> m_oCellInfoDictContainer = new Dictionary<int, Dictionary<int, STCellInfo>>();

#if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
	[JsonIgnore][IgnoreMember][System.NonSerialized] public STIDInfo m_stIDInfo;
#else
	[IgnoreMember][System.NonSerialized] public STIDInfo m_stIDInfo;
#endif // #if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
#endregion // 변수

#region 프로퍼티
#if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
	[JsonIgnore][IgnoreMember] public System.Version CellInfoVer { get { return System.Version.Parse(m_oStrDict.GetValueOrDefault(KEY_CELL_INFO_VER, KCDefine.B_DEF_VER)); } set { m_oStrDict.ExReplaceVal(KEY_CELL_INFO_VER, value.ToString(KCDefine.B_VAL_3_INT)); } }
	[JsonIgnore][IgnoreMember] public Dictionary<ulong, STTargetInfo> ClearTargetInfoDict { get; } = new Dictionary<ulong, STTargetInfo>();
	[JsonIgnore][IgnoreMember] public Dictionary<ulong, STTargetInfo> UnlockTargetInfoDict { get; } = new Dictionary<ulong, STTargetInfo>();

	[JsonIgnore][IgnoreMember] public ulong ULevelID => CFactory.MakeULevelID(m_stIDInfo.m_nID01, m_stIDInfo.m_nID02, m_stIDInfo.m_nID03);
	[JsonIgnore][IgnoreMember] public Vector3Int NumCells => new Vector3Int(m_oCellInfoDictContainer.ExIsValid() ? m_oCellInfoDictContainer.Max((a_stKeyVal) => a_stKeyVal.Value.Count) : KCDefine.B_VAL_0_INT, m_oCellInfoDictContainer.Count, KCDefine.B_VAL_0_INT);
#else
	[IgnoreMember] public System.Version CellInfoVer { get { return System.Version.Parse(m_oStrDict.GetValueOrDefault(KEY_CELL_INFO_VER, KCDefine.B_DEF_VER)); } set { m_oStrDict.ExReplaceVal(KEY_CELL_INFO_VER, value.ToString(KCDefine.B_VAL_3_INT)); } }
	[IgnoreMember] public Dictionary<ulong, STTargetInfo> ClearTargetInfoDict { get; } = new Dictionary<ulong, STTargetInfo>();
	[IgnoreMember] public Dictionary<ulong, STTargetInfo> UnlockTargetInfoDict { get; } = new Dictionary<ulong, STTargetInfo>();

	[IgnoreMember] public ulong ULevelID => CFactory.MakeULevelID(m_stIDInfo.m_nID01, m_stIDInfo.m_nID02, m_stIDInfo.m_nID03);
	[IgnoreMember] public Vector3Int NumCells => new Vector3Int(m_oCellInfoDictContainer.ExIsValid() ? m_oCellInfoDictContainer.Max((a_stKeyVal) => a_stKeyVal.Value.Count) : KCDefine.B_VAL_0_INT, m_oCellInfoDictContainer.Count, KCDefine.B_VAL_0_INT);
#endif // #if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
#endregion // 프로퍼티

#region ICloneable
	/** 사본 객체를 생성한다 */
	public virtual object Clone() {
		var oLevelInfo = new CLevelInfo();
		this.SetupCloneInst(oLevelInfo);

		oLevelInfo.OnAfterDeserialize();
		return oLevelInfo;
	}
#endregion // ICloneable

#region IMessagePackSerializationCallbackReceiver
	/** 직렬화 될 경우 */
	public override void OnBeforeSerialize() {
		base.OnBeforeSerialize();
	}

	/** 역직렬화 되었을 경우 */
	public override void OnAfterDeserialize() {
		base.OnAfterDeserialize();
		m_oCellInfoDictContainer = m_oCellInfoDictContainer ?? new Dictionary<int, Dictionary<int, STCellInfo>>();

		// 셀을 설정한다
		for(int i = 0; i < m_oCellInfoDictContainer.Count; ++i) {
			for(int j = 0; j < m_oCellInfoDictContainer[i].Count; ++j) {
				var stCellInfo = m_oCellInfoDictContainer[i][j];
				this.SetupCellInfo(new Vector3Int(j, i, KCDefine.B_VAL_0_INT), ref stCellInfo);

				m_oCellInfoDictContainer[i][j] = stCellInfo;
			}
		}

		// 버전이 다를 경우
		if(this.Ver.CompareTo(KDefine.G_VER_LEVEL_INFO) < KCDefine.B_COMPARE_EQUALS) {
			// Do Something
		}
	}

	/** 셀 정보를 설정한다 */
	protected virtual void SetupCellInfo(Vector3Int a_stIdx, ref STCellInfo a_stOutCellInfo) {
		a_stOutCellInfo.m_stIdx = a_stIdx;

		foreach(var stKeyVal in a_stOutCellInfo.m_oObjKindsDictContainer) {
			for(int i = 0; i < stKeyVal.Value.Count; ++i) {
				// Do Something
			}
		}

		// 버전이 다를 경우
		if(this.CellInfoVer.CompareTo(KDefine.G_VER_CELL_INFO) < KCDefine.B_COMPARE_EQUALS) {
			// Do Something
		}
	}
#endregion // IMessagePackSerializationCallbackReceiver

#region 함수
	/** 생성자 */
	public CLevelInfo() : base(KDefine.G_VER_LEVEL_INFO) {
		this.CellInfoVer = KDefine.G_VER_CELL_INFO;
	}

	/** 셀 정보를 반환한다 */
	public STCellInfo GetCellInfo(Vector3Int a_stIdx) {
		bool bIsValid = this.TryGetCellInfo(a_stIdx, out STCellInfo stCellInfo);
		CAccess.Assert(bIsValid);

		return stCellInfo;
	}

	/** 셀 정보를 반환한다 */
	public bool TryGetCellInfo(Vector3Int a_stIdx, out STCellInfo a_stOutCellInfo) {
		a_stOutCellInfo = m_oCellInfoDictContainer.ContainsKey(a_stIdx.y) ? m_oCellInfoDictContainer[a_stIdx.y].GetValueOrDefault(a_stIdx.x, STCellInfo.INVALID) : STCellInfo.INVALID;
		return !a_stOutCellInfo.m_stIdx.Equals(KCDefine.B_IDX_INVALID_3D);
	}

	/** 사본 객체를 설정한다 */
	protected virtual void SetupCloneInst(CLevelInfo a_oLevelInfo) {
		a_oLevelInfo.m_stIDInfo = m_stIDInfo;

		// 셀 정보를 설정한다
		for(int i = 0; i < m_oCellInfoDictContainer.Count; ++i) {
			var oCellInfoDict = new Dictionary<int, STCellInfo>();

			for(int j = 0; j < m_oCellInfoDictContainer[i].Count; ++j) {
				oCellInfoDict.TryAdd(j, (STCellInfo)m_oCellInfoDictContainer[i][j].Clone());
			}

			a_oLevelInfo.m_oCellInfoDictContainer.TryAdd(i, oCellInfoDict);
		}
	}
#endregion // 함수
}

/** 레벨 정보 테이블 */
public partial class CLevelInfoTable : CSingleton<CLevelInfoTable> {
#region 프로퍼티
	public Dictionary<int, Dictionary<int, Dictionary<int, CLevelInfo>>> LevelInfoDictContainer = new Dictionary<int, Dictionary<int, Dictionary<int, CLevelInfo>>>();

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
	public int NumChapterInfos => this.LevelInfoDictContainer.Count;
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
#endregion // 프로퍼티

#region 함수
	/** 레벨 정보를 로드한다 */
	public CLevelInfo LoadLevelInfo(int a_nLevelID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		return this.LoadLevelInfo(this.GetLevelInfoPath(a_nLevelID, a_nStageID, a_nChapterID), a_nLevelID, a_nStageID, a_nChapterID);
	}

	/** 레벨 정보를 로드한다 */
	public Dictionary<int, Dictionary<int, Dictionary<int, CLevelInfo>>> LoadLevelInfos() {
		return this.LoadLevelInfos(Access.LevelInfoTableLoadPath);
	}

	/** 레벨 정보를 저장한다 */
	public void SaveLevelInfos() {
		var oLevelIDList = new List<ulong>();
		string oFilePath = Access.LevelInfoTableLoadPath.Replace(KCDefine.B_FILE_EXTENSION_BYTES, KCDefine.B_FILE_EXTENSION_JSON);

		for(int i = 0; i < this.LevelInfoDictContainer.Count; ++i) {
			for(int j = 0; j < this.LevelInfoDictContainer[i].Count; ++j) {
				for(int k = 0; k < this.LevelInfoDictContainer[i][j].Count; ++k) {
					this.LevelInfoDictContainer[i][j][k].m_stIDInfo = new STIDInfo(k, j, i);
					this.SaveLevelInfo(this.LevelInfoDictContainer[i][j][k], oLevelIDList);
				}
			}
		}

		CFunc.WriteMsgPackJSONObj(oFilePath, oLevelIDList, false);
	}

	/** 레벨 정보 경로를 반환한다 */
	private string GetLevelInfoPath(int a_nLevelID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		ulong nULevelID = CFactory.MakeULevelID(a_nLevelID, a_nStageID, a_nChapterID);

#if AB_TEST_ENABLE
		return string.Format((CCommonUserInfoStorage.Inst.UserInfo.UserType == EUserType.B) ? KCDefine.U_RUNTIME_DATA_P_FMT_G_LEVEL_INFO_SET_B : KCDefine.U_RUNTIME_DATA_P_FMT_G_LEVEL_INFO_SET_A, nULevelID + KCDefine.B_VAL_1_INT);
#else
		return string.Format(KCDefine.U_RUNTIME_DATA_P_FMT_G_LEVEL_INFO, nULevelID + KCDefine.B_VAL_1_INT);
#endif // #if AB_TEST_ENABLE
	}

	/** 레벨 정보를 로드한다 */
	private CLevelInfo LoadLevelInfo(string a_oFilePath, int a_nLevelID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		CFunc.ShowLog($"CLevelInfoTable.LoadLevelInfo: {a_oFilePath}");

#if MSG_PACK_SERIALIZE_DESERIALIZE_ENABLE
		var oLevelInfo = File.Exists(a_oFilePath) ? CFunc.ReadMsgPackObj<CLevelInfo>(a_oFilePath, true) : CFunc.ReadMsgPackObjFromRes<CLevelInfo>(a_oFilePath, true);
#elif NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
		a_oFilePath = a_oFilePath.Replace(KCDefine.B_FILE_EXTENSION_BYTES, KCDefine.B_FILE_EXTENSION_JSON);
		var oLevelInfo = File.Exists(a_oFilePath) ? CFunc.ReadJSONObj<CLevelInfo>(a_oFilePath, true) : CFunc.ReadJSONObjFromRes<CLevelInfo>(a_oFilePath, true);
#endif // #if MSG_PACK_SERIALIZE_DESERIALIZE_ENABLE

		oLevelInfo.m_stIDInfo = new STIDInfo(a_nLevelID, a_nStageID, a_nChapterID);
		return oLevelInfo;
	}

	/** 레벨 정보를 로드한다 */
	private Dictionary<int, Dictionary<int, Dictionary<int, CLevelInfo>>> LoadLevelInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		CFunc.ShowLog($"CLevelInfoTable.LoadLevelInfos: {a_oFilePath}");

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
		var oLevelIDList = File.Exists(a_oFilePath) ? CFunc.ReadMsgPackJSONObj<List<ulong>>(a_oFilePath, false) : CFunc.ReadMsgPackJSONObjFromRes<List<ulong>>(a_oFilePath, false);

		for(int i = 0; i < oLevelIDList.Count; ++i) {
			this.AddLevelInfo(this.LoadLevelInfo(oLevelIDList[i].ExULevelIDToLevelID(), oLevelIDList[i].ExULevelIDToStageID(), oLevelIDList[i].ExULevelIDToChapterID()));
		}
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)

		return this.LevelInfoDictContainer;
	}

	/** 레벨 정보를 저장한다 */
	private void SaveLevelInfo(CLevelInfo a_oLevelInfo, List<ulong> a_oOutLevelIDList) {
		CAccess.Assert(a_oLevelInfo != null && a_oOutLevelIDList != null);
		a_oOutLevelIDList.Add(a_oLevelInfo.m_stIDInfo.UniqueID01);

#if MSG_PACK_SERIALIZE_DESERIALIZE_ENABLE
		CFunc.WriteMsgPackObj(this.GetLevelInfoPath(a_oLevelInfo.m_stIDInfo.m_nID01, a_oLevelInfo.m_stIDInfo.m_nID02, a_oLevelInfo.m_stIDInfo.m_nID03), a_oLevelInfo, true);
#elif NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
		string oFilePath = this.GetLevelInfoPath(a_oLevelInfo.m_stIDInfo.m_nID01, a_oLevelInfo.m_stIDInfo.m_nID02, a_oLevelInfo.m_stIDInfo.m_nID03);
		CFunc.WriteJSONObj(oFilePath.Replace(KCDefine.B_FILE_EXTENSION_BYTES, KCDefine.B_FILE_EXTENSION_JSON), a_oLevelInfo, true);
#endif // #if MSG_PACK_SERIALIZE_DESERIALIZE_ENABLE
	}
#endregion // 함수

#region 조건부 함수
#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
	/** 레벨 정보 개수를 반환한다 */
	public int GetNumLevelInfos(int a_nStageID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		CAccess.Assert(this.LevelInfoDictContainer.ContainsKey(a_nChapterID) && this.LevelInfoDictContainer[a_nChapterID].ContainsKey(a_nStageID));
		return this.LevelInfoDictContainer[a_nChapterID][a_nStageID].Count;
	}

	/** 스테이지 정보 개수를 반화한다 */
	public int GetNumStageInfos(int a_nChapterID) {
		bool bIsValid = this.LevelInfoDictContainer.TryGetValue(a_nChapterID, out Dictionary<int, Dictionary<int, CLevelInfo>> oChapterLevelInfoDictContainer);
		CAccess.Assert(bIsValid);

		return oChapterLevelInfoDictContainer.Count;
	}

	/** 레벨 정보를 반환한다 */
	public CLevelInfo GetLevelInfo(int a_nLevelID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetLevelInfo(a_nLevelID, out CLevelInfo oLevelInfo, a_nStageID, a_nChapterID);
		CAccess.Assert(bIsValid);

		return oLevelInfo;
	}

	/** 스테이지 레벨 정보를 반환한다 */
	public Dictionary<int, CLevelInfo> GetStageLevelInfos(int a_nStageID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetStageLevelInfos(a_nStageID, out Dictionary<int, CLevelInfo> oStageLevelInfoDict, a_nChapterID);
		CAccess.Assert(bIsValid);

		return oStageLevelInfoDict;
	}

	/** 챕터 레벨 정보를 반환한다 */
	public Dictionary<int, Dictionary<int, CLevelInfo>> GetChapterLevelInfos(int a_nChapterID) {
		bool bIsValid = this.TryGetChapterLevelInfos(a_nChapterID, out Dictionary<int, Dictionary<int, CLevelInfo>> oChapterLevelInfoDictContainer);
		CAccess.Assert(bIsValid);

		return oChapterLevelInfoDictContainer;
	}

	/** 레벨 정보를 반환한다 */
	public bool TryGetLevelInfo(int a_nLevelID, out CLevelInfo a_oOutLevelInfo, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.TryGetStageLevelInfos(a_nStageID, out Dictionary<int, CLevelInfo> oStageLevelInfoDict, a_nChapterID);
		a_oOutLevelInfo = oStageLevelInfoDict?.GetValueOrDefault(a_nLevelID);

		return a_oOutLevelInfo != null;
	}

	/** 스테이지 레벨 정보를 반환한다 */
	public bool TryGetStageLevelInfos(int a_nStageID, out Dictionary<int, CLevelInfo> a_oOutStageLevelInfoDict, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		this.TryGetChapterLevelInfos(a_nChapterID, out Dictionary<int, Dictionary<int, CLevelInfo>> oChapterLevelInfoDictContainer);
		a_oOutStageLevelInfoDict = oChapterLevelInfoDictContainer?.GetValueOrDefault(a_nStageID);

		return a_oOutStageLevelInfoDict != null;
	}

	/** 챕터 레벨 정보를 반환한다 */
	public bool TryGetChapterLevelInfos(int a_nChapterID, out Dictionary<int, Dictionary<int, CLevelInfo>> a_oOutChapterLevelInfoDictContainer) {
		a_oOutChapterLevelInfoDictContainer = this.LevelInfoDictContainer.GetValueOrDefault(a_nChapterID);
		return a_oOutChapterLevelInfoDictContainer != null;
	}

	/** 레벨 정보를 추가한다 */
	public void AddLevelInfo(CLevelInfo a_oLevelInfo, bool a_bIsReplace = false) {
		CAccess.Assert(a_oLevelInfo != null);

		var oChapterLevelInfoDictContainer = this.LevelInfoDictContainer.GetValueOrDefault(a_oLevelInfo.m_stIDInfo.m_nID03);
		oChapterLevelInfoDictContainer = oChapterLevelInfoDictContainer ?? new Dictionary<int, Dictionary<int, CLevelInfo>>();

		var oStageLevelInfoDict = oChapterLevelInfoDictContainer.GetValueOrDefault(a_oLevelInfo.m_stIDInfo.m_nID02);
		oStageLevelInfoDict = oStageLevelInfoDict ?? new Dictionary<int, CLevelInfo>();

		// 레벨 정보 추가가 가능 할 경우
		if(a_bIsReplace || !oStageLevelInfoDict.ContainsKey(a_oLevelInfo.m_stIDInfo.m_nID01)) {
			oStageLevelInfoDict.ExReplaceVal(a_oLevelInfo.m_stIDInfo.m_nID01, a_oLevelInfo);
			oChapterLevelInfoDictContainer.ExReplaceVal(a_oLevelInfo.m_stIDInfo.m_nID02, oStageLevelInfoDict);
			this.LevelInfoDictContainer.ExReplaceVal(a_oLevelInfo.m_stIDInfo.m_nID03, oChapterLevelInfoDictContainer);
		}
	}

	/** 스테이지 레벨 정보를 추가한다 */
	public void AddStageLevelInfos(Dictionary<int, CLevelInfo> a_oStageLevelInfoDict, bool a_bIsReplace = false) {
		CAccess.Assert(a_oStageLevelInfoDict != null);
		var oStageLevelInfoList = a_oStageLevelInfoDict.OrderBy((a_stKeyVal) => a_stKeyVal.Key).ToList();

		for(int i = 0; i < oStageLevelInfoList.Count; ++i) {
			int nNumLevelInfos = this.GetNumLevelInfos(oStageLevelInfoList[i].Value.m_stIDInfo.m_nID02, oStageLevelInfoList[i].Value.m_stIDInfo.m_nID03);
			oStageLevelInfoList[i].Value.m_stIDInfo = new STIDInfo(nNumLevelInfos, oStageLevelInfoList[i].Value.m_stIDInfo.m_nID02, oStageLevelInfoList[i].Value.m_stIDInfo.m_nID03);

			this.AddLevelInfo(oStageLevelInfoList[i].Value, a_bIsReplace);
		}
	}

	/** 챕터 레벨 정보를 추가한다 */
	public void AddChapterLevelInfos(Dictionary<int, Dictionary<int, CLevelInfo>> a_oChapterLevelInfoDict, bool a_bIsReplace = false) {
		CAccess.Assert(a_oChapterLevelInfoDict != null);
		var oChapterLevelInfoList = a_oChapterLevelInfoDict.OrderBy((a_stKeyVal) => a_stKeyVal.Key).ToList();

		for(int i = 0; i < oChapterLevelInfoList.Count; ++i) {
			for(int j = 0; j < oChapterLevelInfoList[i].Value.Count; ++i) {
				int nNumStageInfos = this.GetNumStageInfos(oChapterLevelInfoList[i].Value[j].m_stIDInfo.m_nID03);
				oChapterLevelInfoList[i].Value[j].m_stIDInfo = new STIDInfo(oChapterLevelInfoList[i].Value[j].m_stIDInfo.m_nID01, nNumStageInfos, oChapterLevelInfoList[i].Value[j].m_stIDInfo.m_nID03);
			}

			this.AddStageLevelInfos(oChapterLevelInfoList[i].Value, a_bIsReplace);
		}
	}

	/** 레벨 정보를 제거한다 */
	public void RemoveLevelInfo(int a_nLevelID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetStageLevelInfos(a_nStageID, out Dictionary<int, CLevelInfo> oStageLevelInfoDict, a_nChapterID);
		CAccess.Assert(bIsValid && oStageLevelInfoDict.ExIsValid());

		for(int i = a_nLevelID + KCDefine.B_VAL_1_INT; i < oStageLevelInfoDict.Count; ++i) {
			oStageLevelInfoDict[i].m_stIDInfo.m_nID01 -= KCDefine.B_VAL_1_INT;
			oStageLevelInfoDict.ExReplaceVal(i - KCDefine.B_VAL_1_INT, oStageLevelInfoDict[i]);
		}

		oStageLevelInfoDict.ExRemoveVal(oStageLevelInfoDict.Count - KCDefine.B_VAL_1_INT);

		// 스테이지 레벨 정보가 없을 경우
		if(!oStageLevelInfoDict.ExIsValid()) {
			this.RemoveStageLevelInfos(a_nStageID, a_nChapterID);
		}
	}

	/** 스테이지 레벨 정보를 제거한다 */
	public void RemoveStageLevelInfos(int a_nStageID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetChapterLevelInfos(a_nChapterID, out Dictionary<int, Dictionary<int, CLevelInfo>> oChapterLevelInfoDictContainer);
		CAccess.Assert(bIsValid && oChapterLevelInfoDictContainer.ExIsValid());

		for(int i = a_nStageID + KCDefine.B_VAL_1_INT; i < oChapterLevelInfoDictContainer.Count; ++i) {
			for(int j = 0; j < oChapterLevelInfoDictContainer[i].Count; ++j) {
				oChapterLevelInfoDictContainer[i][j].m_stIDInfo.m_nID02 -= KCDefine.B_VAL_1_INT;
			}

			oChapterLevelInfoDictContainer.ExReplaceVal(i - KCDefine.B_VAL_1_INT, oChapterLevelInfoDictContainer[i]);
		}

		oChapterLevelInfoDictContainer.ExRemoveVal(oChapterLevelInfoDictContainer.Count - KCDefine.B_VAL_1_INT);

		// 챕터 레벨 정보가 없을 경우
		if(!oChapterLevelInfoDictContainer.ExIsValid()) {
			this.RemoveChapterLevelInfos(a_nChapterID);
		}
	}

	/** 챕터 레벨 정보를 제거한다 */
	public void RemoveChapterLevelInfos(int a_nChapterID) {
		CAccess.Assert(this.LevelInfoDictContainer.ContainsKey(a_nChapterID));

		for(int i = a_nChapterID + KCDefine.B_VAL_1_INT; i < this.LevelInfoDictContainer.Count; ++i) {
			for(int j = 0; j < this.LevelInfoDictContainer[i].Count; ++j) {
				for(int k = 0; k < this.LevelInfoDictContainer[i][j].Count; ++k) {
					this.LevelInfoDictContainer[i][j][k].m_stIDInfo.m_nID03 -= KCDefine.B_VAL_1_INT;
				}
			}

			this.LevelInfoDictContainer.ExReplaceVal(i - KCDefine.B_VAL_1_INT, this.LevelInfoDictContainer[i]);
		}

		this.LevelInfoDictContainer.ExRemoveVal(this.LevelInfoDictContainer.Count - KCDefine.B_VAL_1_INT);
	}

	/** 레벨 정보를 이동한다 */
	public void MoveLevelInfo(int a_nSrcID, int a_nDestID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetStageLevelInfos(a_nStageID, out Dictionary<int, CLevelInfo> oStageLevelInfoDict, a_nChapterID);

		CAccess.Assert(bIsValid && oStageLevelInfoDict.ExIsValid());
		CAccess.Assert(oStageLevelInfoDict.ContainsKey(a_nSrcID) && oStageLevelInfoDict.ContainsKey(a_nDestID));

		int nOffset = (a_nSrcID <= a_nDestID) ? KCDefine.B_VAL_1_INT : -KCDefine.B_VAL_1_INT;
		var oSrcLevelInfo = oStageLevelInfoDict.GetValueOrDefault(a_nSrcID);

		oStageLevelInfoDict.ExRemoveVal(a_nSrcID);

		for(int i = a_nSrcID + nOffset; i != a_nDestID + nOffset; i += nOffset) {
			oStageLevelInfoDict[i].m_stIDInfo.m_nID01 -= nOffset;
			oStageLevelInfoDict.ExReplaceVal(i - nOffset, oStageLevelInfoDict[i]);
		}

		oSrcLevelInfo.m_stIDInfo.m_nID01 = a_nDestID;
		oStageLevelInfoDict.ExReplaceVal(a_nDestID, oSrcLevelInfo);
	}

	/** 스테이지 레벨 정보를 이동한다 */
	public void MoveStageLevelInfos(int a_nSrcID, int a_nDestID, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		bool bIsValid = this.TryGetChapterLevelInfos(a_nChapterID, out Dictionary<int, Dictionary<int, CLevelInfo>> oChapterLevelInfoDictContainer);

		CAccess.Assert(bIsValid && oChapterLevelInfoDictContainer.ExIsValid());
		CAccess.Assert(oChapterLevelInfoDictContainer.ContainsKey(a_nSrcID) && oChapterLevelInfoDictContainer.ContainsKey(a_nDestID));

		int nOffset = (a_nSrcID <= a_nDestID) ? KCDefine.B_VAL_1_INT : -KCDefine.B_VAL_1_INT;
		var oSrcStageLevelInfoDict = oChapterLevelInfoDictContainer[a_nSrcID];

		oChapterLevelInfoDictContainer.ExRemoveVal(a_nSrcID);

		for(int i = a_nSrcID + nOffset; i != a_nDestID + nOffset; i += nOffset) {
			for(int j = 0; j < oChapterLevelInfoDictContainer[i].Count; ++j) {
				oChapterLevelInfoDictContainer[i][j].m_stIDInfo.m_nID02 -= nOffset;
			}

			oChapterLevelInfoDictContainer.ExReplaceVal(i - nOffset, oChapterLevelInfoDictContainer[i]);
		}

		for(int i = 0; i < oSrcStageLevelInfoDict.Count; ++i) {
			oSrcStageLevelInfoDict[i].m_stIDInfo.m_nID02 = a_nDestID;
		}

		oChapterLevelInfoDictContainer.ExReplaceVal(a_nDestID, oSrcStageLevelInfoDict);
	}

	/** 챕터 레벨 정보를 이동한다 */
	public void MoveChapterLevelInfos(int a_nSrcID, int a_nDestID) {
		CAccess.Assert(this.LevelInfoDictContainer.ContainsKey(a_nSrcID) && this.LevelInfoDictContainer.ContainsKey(a_nDestID));

		int nOffset = (a_nSrcID <= a_nDestID) ? KCDefine.B_VAL_1_INT : -KCDefine.B_VAL_1_INT;
		var oSrcChapterLevelInfoDict = this.LevelInfoDictContainer[a_nSrcID];

		for(int i = a_nSrcID + nOffset; i != a_nDestID + nOffset; i += nOffset) {
			for(int j = 0; j < this.LevelInfoDictContainer[i].Count; ++j) {
				for(int k = 0; k < this.LevelInfoDictContainer[i][j].Count; ++k) {
					this.LevelInfoDictContainer[i][j][k].m_stIDInfo.m_nID03 -= nOffset;
				}
			}

			this.LevelInfoDictContainer.ExReplaceVal(i - nOffset, this.LevelInfoDictContainer[i]);
		}

		for(int i = 0; i < oSrcChapterLevelInfoDict.Count; ++i) {
			for(int j = 0; j < oSrcChapterLevelInfoDict[i].Count; ++j) {
				oSrcChapterLevelInfoDict[i][j].m_stIDInfo.m_nID03 = a_nDestID;
			}
		}

		this.LevelInfoDictContainer.ExReplaceVal(a_nDestID, oSrcChapterLevelInfoDict);
	}
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
#endregion // 조건부 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

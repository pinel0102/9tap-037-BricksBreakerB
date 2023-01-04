using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.IO;

/** 효과 정보 */
[System.Serializable]
public struct STFXInfo {
	public STCommonInfo m_stCommonInfo;
	public STTimeInfo m_stTimeInfo;

	public EFXKinds m_eFXKinds;
	public EFXKinds m_ePrevFXKinds;
	public EFXKinds m_eNextFXKinds;

	List<EResKinds> m_oResKindsList;

#region 상수
	public static STFXInfo INVALID = new STFXInfo() {
		m_eFXKinds = EFXKinds.NONE, m_ePrevFXKinds = EFXKinds.NONE, m_eNextFXKinds = EFXKinds.NONE
	};
#endregion // 상수

#region 프로퍼티
	public EFXType FXType => (EFXType)((int)m_eFXKinds).ExKindsToType();
	public EFXKinds BaseFXKinds => (EFXKinds)((int)m_eFXKinds).ExKindsToSubKindsType();
#endregion // 프로퍼티

#region 함수
	/** 생성자 */
	public STFXInfo(SimpleJSON.JSONNode a_oFXInfo) {
		m_stCommonInfo = new STCommonInfo(a_oFXInfo);
		m_stTimeInfo = new STTimeInfo(a_oFXInfo[KCDefine.U_KEY_TIME_INFO]);

		m_eFXKinds = a_oFXInfo[KCDefine.U_KEY_FX_KINDS].ExIsValid() ? (EFXKinds)a_oFXInfo[KCDefine.U_KEY_FX_KINDS].AsInt : EFXKinds.NONE;
		m_ePrevFXKinds = a_oFXInfo[KCDefine.U_KEY_PREV_FX_KINDS].ExIsValid() ? (EFXKinds)a_oFXInfo[KCDefine.U_KEY_PREV_FX_KINDS].AsInt : EFXKinds.NONE;
		m_eNextFXKinds = a_oFXInfo[KCDefine.U_KEY_NEXT_FX_KINDS].ExIsValid() ? (EFXKinds)a_oFXInfo[KCDefine.U_KEY_NEXT_FX_KINDS].AsInt : EFXKinds.NONE;

		m_oResKindsList = Factory.MakeVals(a_oFXInfo, KCDefine.U_KEY_FMT_RES_KINDS, (a_oJSONNode) => (EResKinds)a_oJSONNode.AsInt);
	}
#endregion // 함수

#region 조건부 함수
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 효과 정보를 저장한다 */
	public void SaveFXInfo(SimpleJSON.JSONNode a_oOutFXInfo) {
		m_stCommonInfo.SaveCommonInfo(a_oOutFXInfo);
		m_stTimeInfo.SaveTimeInfo(a_oOutFXInfo);

		a_oOutFXInfo[KCDefine.U_KEY_FX_KINDS] = $"{(int)m_eFXKinds}";
		a_oOutFXInfo[KCDefine.U_KEY_PREV_FX_KINDS] = $"{(int)m_ePrevFXKinds}";
		a_oOutFXInfo[KCDefine.U_KEY_NEXT_FX_KINDS] = $"{(int)m_eNextFXKinds}";

		Func.SaveVals(m_oResKindsList, KCDefine.U_KEY_FMT_RES_KINDS, (a_eResKinds) => $"{(int)a_eResKinds}", a_oOutFXInfo);
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
#endregion // 조건부 함수
}

/** 효과 정보 테이블 */
public partial class CFXInfoTable : CSingleton<CFXInfoTable> {
#region 프로퍼티
	public Dictionary<EFXKinds, STFXInfo> FXInfoDict { get; } = new Dictionary<EFXKinds, STFXInfo>();
#endregion // 프로퍼티

#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetFXInfos();
	}

	/** 효과 정보를 리셋한다 */
	public virtual void ResetFXInfos() {
		this.FXInfoDict.Clear();
	}

	/** 효과 정보를 리셋한다 */
	public virtual void ResetFXInfos(string a_oJSONStr) {
		this.ResetFXInfos();
		this.DoLoadFXInfos(a_oJSONStr);
	}

	/** 효과 정보를 반환한다 */
	public STFXInfo GetFXInfo(EFXKinds a_eFXKinds) {
		bool bIsValid = this.TryGetFXInfo(a_eFXKinds, out STFXInfo stFXInfo);
		CAccess.Assert(bIsValid);

		return stFXInfo;
	}

	/** 효과 정보를 반환한다 */
	public bool TryGetFXInfo(EFXKinds a_eFXKinds, out STFXInfo a_stOutFXInfo) {
		a_stOutFXInfo = this.FXInfoDict.GetValueOrDefault(a_eFXKinds, STFXInfo.INVALID);
		return this.FXInfoDict.ContainsKey(a_eFXKinds);
	}

	/** 효과 정보를 로드한다 */
	public Dictionary<EFXKinds, STFXInfo> LoadFXInfos() {
		this.ResetFXInfos();
		return this.LoadFXInfos(Access.FXInfoTableLoadPath);
	}

	/** 효과 정보를 저장한다 */
	public void SaveFXInfos(string a_oJSONStr, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oJSONStr != null);

		// JSON 문자열이 존재 할 경우
		if(a_oJSONStr != null) {
			this.ResetFXInfos(a_oJSONStr);
		}
	}

	/** JSON 노드를 설정한다 */
	private void SetupJSONNodes(SimpleJSON.JSONNode a_oJSONNode, out SimpleJSON.JSONNode a_oOutCommonInfos) {
		var oSheetNameDictContainer = Access.GetSheetNames(this.GetType(), Access.FXTableInfo);
		a_oOutCommonInfos = a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_COMMON]].ExIsValid() ? a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_COMMON]] : KCDefine.B_EMPTY_JSON_ARRAY;
	}

	/** 효과 정보를 로드한다 */
	private Dictionary<EFXKinds, STFXInfo> LoadFXInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		return this.DoLoadFXInfos(this.LoadFXInfosJSONStr(a_oFilePath));
	}

	/** 효과 정보 JSON 문자열을 로드한다 */
	private string LoadFXInfosJSONStr(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
		return File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, false) : CFunc.ReadStrFromRes(a_oFilePath, false);
#else
		return File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, true) : CFunc.ReadStrFromRes(a_oFilePath, false);
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 효과 정보를 로드한다 */
	private Dictionary<EFXKinds, STFXInfo> DoLoadFXInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		this.SetupJSONNodes(SimpleJSON.JSONNode.Parse(a_oJSONStr), out SimpleJSON.JSONNode oCommonInfos);

		for(int i = 0; i < oCommonInfos.Count; ++i) {
			var stFXInfo = new STFXInfo(oCommonInfos[i]);

			// 효과 정보 추가 가능 할 경우
			if(stFXInfo.m_eFXKinds.ExIsValid() && (!this.FXInfoDict.ContainsKey(stFXInfo.m_eFXKinds) || oCommonInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
				this.FXInfoDict.ExReplaceVal(stFXInfo.m_eFXKinds, stFXInfo);
			}
		}

		return this.FXInfoDict;
	}
#endregion // 함수

#region 조건부 함수
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 효과 정보를 저장한다 */
	public void SaveFXInfos(SimpleJSON.JSONNode a_oOutFXInfos) {
		this.SetupJSONNodes(a_oOutFXInfos, out SimpleJSON.JSONNode oCommonInfos);

		for(int i = 0; i < oCommonInfos.Count; ++i) {
			var eFXKinds = oCommonInfos[i][KCDefine.U_KEY_FX_KINDS].ExIsValid() ? (EFXKinds)oCommonInfos[i][KCDefine.U_KEY_FX_KINDS].AsInt : EFXKinds.NONE;

			// 효과 정보가 존재 할 경우
			if(this.FXInfoDict.TryGetValue(eFXKinds, out STFXInfo stFXInfo)) {
				stFXInfo.SaveFXInfo(oCommonInfos[i]);
			}
		}
	}

	/** 효과 정보 값을 생성한다 */
	public void MakeFXInfoVals(SimpleJSON.JSONNode a_oFXInfos, Dictionary<string, List<List<string>>> a_oOutFXInfoValDictContainer) {
		var oCommonKeyInfoList = CCollectionManager.Inst.SpawnList<STKeyInfo>();

		try {
			this.SetupKeyInfos(oCommonKeyInfoList);
			this.SetupJSONNodes(a_oFXInfos, out SimpleJSON.JSONNode oCommonInfos);

			a_oOutFXInfoValDictContainer.TryAdd(Access.GetSheetNames(this.GetType(), Access.FXTableInfo)[KCDefine.B_KEY_COMMON], oCommonInfos.AsArray.ExToInfoVals(oCommonKeyInfoList));
		} finally {
			CCollectionManager.Inst.DespawnList(oCommonKeyInfoList);
		}
	}

	/** 키 정보를 설정한다 */
	private void SetupKeyInfos(List<STKeyInfo> a_oOutCommonKeyInfoList) {
		KDefine.G_KEY_INFO_GOOGLE_SHEET_COMMON_LIST.ExCopyTo(a_oOutCommonKeyInfoList, (a_stKeyInfo) => a_stKeyInfo);
		Access.FXTableInfo.m_oKeyInfoDictContainer[this.GetType()].GetValueOrDefault(KCDefine.B_KEY_COMMON)?.ExCopyTo(a_oOutCommonKeyInfoList, (a_stKeyInfo) => a_stKeyInfo, false, false);
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
#endregion // 조건부 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

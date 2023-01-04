using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.IO;
using System.Globalization;

/** 리소스 정보 */
[System.Serializable]
public struct STResInfo {
	public STCommonInfo m_stCommonInfo;

	public string m_oRate;
	public string m_oResPath;

	public EResKinds m_eResKinds;
	public EResKinds m_ePrevResKinds;
	public EResKinds m_eNextResKinds;

#region 상수
	public static STResInfo INVALID = new STResInfo() {
		m_eResKinds = EResKinds.NONE, m_ePrevResKinds = EResKinds.NONE, m_eNextResKinds = EResKinds.NONE
	};
#endregion // 상수

#region 프로퍼티
	public int IntRate => int.TryParse(m_oRate, NumberStyles.Any, null, out int nRate) ? nRate : KCDefine.B_VAL_0_INT;
	public float RealRate => float.TryParse(m_oRate, NumberStyles.Any, null, out float fRate) ? fRate : KCDefine.B_VAL_0_INT;

	public EResType ResType => (EResType)((int)m_eResKinds).ExKindsToType();
	public EResKinds BaseResKinds => (EResKinds)((int)m_eResKinds).ExKindsToSubKindsType();
#endregion // 프로퍼티

#region 함수
	/** 생성자 */
	public STResInfo(SimpleJSON.JSONNode a_oResInfo) {
		m_stCommonInfo = new STCommonInfo(a_oResInfo);

		m_oRate = a_oResInfo[KCDefine.U_KEY_RATE].ExIsValid() ? a_oResInfo[KCDefine.U_KEY_RATE] : KCDefine.B_STR_0_INT;
		m_oResPath = a_oResInfo[KCDefine.U_KEY_RES_PATH].ExIsValid() ? a_oResInfo[KCDefine.U_KEY_RES_PATH].Value.Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH) : string.Empty;

		m_eResKinds = a_oResInfo[KCDefine.U_KEY_RES_KINDS].ExIsValid() ? (EResKinds)a_oResInfo[KCDefine.U_KEY_RES_KINDS].AsInt : EResKinds.NONE;
		m_ePrevResKinds = a_oResInfo[KCDefine.U_KEY_PREV_RES_KINDS].ExIsValid() ? (EResKinds)a_oResInfo[KCDefine.U_KEY_PREV_RES_KINDS].AsInt : EResKinds.NONE;
		m_eNextResKinds = a_oResInfo[KCDefine.U_KEY_NEXT_RES_KINDS].ExIsValid() ? (EResKinds)a_oResInfo[KCDefine.U_KEY_NEXT_RES_KINDS].AsInt : EResKinds.NONE;
	}
#endregion // 함수

#region 조건부 함수
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 리소스 정보를 저장한다 */
	public void SaveResInfo(SimpleJSON.JSONNode a_oOutResInfo) {
		m_stCommonInfo.SaveCommonInfo(a_oOutResInfo);

		a_oOutResInfo[KCDefine.U_KEY_RATE] = m_oRate;
		a_oOutResInfo[KCDefine.U_KEY_RES_PATH] = m_oResPath;

		a_oOutResInfo[KCDefine.U_KEY_RES_KINDS] = $"{(int)m_eResKinds}";
		a_oOutResInfo[KCDefine.U_KEY_PREV_RES_KINDS] = $"{(int)m_ePrevResKinds}";
		a_oOutResInfo[KCDefine.U_KEY_NEXT_RES_KINDS] = $"{(int)m_eNextResKinds}";
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
#endregion // 조건부 함수
}

/** 리소스 정보 테이블 */
public partial class CResInfoTable : CSingleton<CResInfoTable> {
#region 프로퍼티
	public Dictionary<EResKinds, STResInfo> ResInfoDict { get; } = new Dictionary<EResKinds, STResInfo>();
#endregion // 프로퍼티

#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetResInfos();
	}

	/** 리소스 정보를 리셋한다 */
	public virtual void ResetResInfos() {
		this.ResInfoDict.Clear();
	}

	/** 리소스 정보를 리셋한다 */
	public virtual void ResetResInfos(string a_oJSONStr) {
		this.ResetResInfos();
		this.DoLoadResInfos(a_oJSONStr);
	}

	/** 리소스 정보를 반환한다 */
	public STResInfo GetResInfo(EResKinds a_eResKinds) {
		bool bIsValid = this.TryGetResInfo(a_eResKinds, out STResInfo stResInfo);
		CAccess.Assert(bIsValid);

		return stResInfo;
	}

	/** 리소스 정보를 반환한다 */
	public bool TryGetResInfo(EResKinds a_eResKinds, out STResInfo a_stOutResInfo) {
		a_stOutResInfo = this.ResInfoDict.GetValueOrDefault(a_eResKinds, STResInfo.INVALID);
		return this.ResInfoDict.ContainsKey(a_eResKinds);
	}

	/** 리소스 정보를 로드한다 */
	public Dictionary<EResKinds, STResInfo> LoadResInfos() {
		this.ResetResInfos();
		return this.LoadResInfos(Access.ResInfoTableLoadPath);
	}

	/** 리소스 정보를 저장한다 */
	public void SaveResInfos(string a_oJSONStr, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oJSONStr != null);

		// JSON 문자열이 존재 할 경우
		if(a_oJSONStr != null) {
			this.ResetResInfos(a_oJSONStr);

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
			CFunc.WriteStr(Access.ResInfoTableSavePath, a_oJSONStr, false);
#else
			CFunc.WriteStr(Access.ResInfoTableSavePath, a_oJSONStr, true);
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)

#if UNITY_ANDROID && (DEBUG || DEVELOPMENT)
			CUnityMsgSender.Inst.SendShowToastMsg($"CResInfoTable.SaveResInfos: {File.Exists(Access.ResInfoTableSavePath)}");
#endif // #if UNITY_ANDROID && (DEBUG || DEVELOPMENT)
		}
	}

	/** JSON 노드를 설정한다 */
	private void SetupJSONNodes(SimpleJSON.JSONNode a_oJSONNode, out SimpleJSON.JSONNode a_oOutCommonInfos) {
		var oSheetNameDictContainer = Access.GetSheetNames(this.GetType(), Access.ResTableInfo);
		a_oOutCommonInfos = a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_COMMON]].ExIsValid() ? a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_COMMON]] : KCDefine.B_EMPTY_JSON_ARRAY;
	}

	/** 리소스 정보를 로드한다 */
	private Dictionary<EResKinds, STResInfo> LoadResInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		return this.DoLoadResInfos(this.LoadResInfosJSONStr(a_oFilePath));
	}

	/** 리소스 정보 JSON 문자열을 로드한다 */
	private string LoadResInfosJSONStr(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
		return File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, false) : CFunc.ReadStrFromRes(a_oFilePath, false);
#else
		return File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, true) : CFunc.ReadStrFromRes(a_oFilePath, false);
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 리소스 정보를 로드한다 */
	private Dictionary<EResKinds, STResInfo> DoLoadResInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		this.SetupJSONNodes(SimpleJSON.JSONNode.Parse(a_oJSONStr), out SimpleJSON.JSONNode oCommonInfos);

		for(int i = 0; i < oCommonInfos.Count; ++i) {
			var stResInfo = new STResInfo(oCommonInfos[i]);

			// 리소스 정보 추가 가능 할 경우
			if(stResInfo.m_eResKinds.ExIsValid() && (!this.ResInfoDict.ContainsKey(stResInfo.m_eResKinds) || oCommonInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
				this.ResInfoDict.ExReplaceVal(stResInfo.m_eResKinds, stResInfo);
			}
		}

		return this.ResInfoDict;
	}
#endregion // 함수

#region 조건부 함수
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 리소스 정보를 저장한다 */
	public void SaveResInfos() {
		var oResInfos = SimpleJSON.JSONNode.Parse(this.LoadResInfosJSONStr(Access.ResInfoTableLoadPath));
		this.SetupJSONNodes(oResInfos, out SimpleJSON.JSONNode oCommonInfos);

		for(int i = 0; i < oCommonInfos.Count; ++i) {
			var eResKinds = oCommonInfos[i][KCDefine.U_KEY_RES_KINDS].ExIsValid() ? (EResKinds)oCommonInfos[i][KCDefine.U_KEY_RES_KINDS].AsInt : EResKinds.NONE;

			// 리소스 정보가 존재 할 경우
			if(this.ResInfoDict.TryGetValue(eResKinds, out STResInfo stResInfo)) {
				stResInfo.SaveResInfo(oCommonInfos[i]);
			}
		}

		this.SaveResInfos(oResInfos.ToString());
	}

	/** 리소스 정보 값을 생성한다 */
	public Dictionary<string, List<List<string>>> MakeResInfoVals() {
		var oCommonKeyInfoList = CCollectionManager.Inst.SpawnList<STKeyInfo>();
		var oResInfoValDictContainer = new Dictionary<string, List<List<string>>>();

		try {
			this.SetupKeyInfos(oCommonKeyInfoList);
			this.SetupJSONNodes(SimpleJSON.JSONNode.Parse(this.LoadResInfosJSONStr(Access.ResInfoTableSavePath)), out SimpleJSON.JSONNode oCommonInfos);

			oResInfoValDictContainer.TryAdd(Access.GetSheetNames(this.GetType(), Access.ResTableInfo)[KCDefine.B_KEY_COMMON], oCommonInfos.AsArray.ExToInfoVals(oCommonKeyInfoList));
		} finally {
			CCollectionManager.Inst.DespawnList(oCommonKeyInfoList);
		}

		return oResInfoValDictContainer;
	}

	/** 키 정보를 설정한다 */
	private void SetupKeyInfos(List<STKeyInfo> a_oOutCommonKeyInfoList) {
		KDefine.G_KEY_INFO_GOOGLE_SHEET_COMMON_LIST.ExCopyTo(a_oOutCommonKeyInfoList, (a_stKeyInfo) => a_stKeyInfo);
		Access.ResTableInfo.m_oKeyInfoDictContainer[this.GetType()].GetValueOrDefault(KCDefine.B_KEY_COMMON)?.ExCopyTo(a_oOutCommonKeyInfoList, (a_stKeyInfo) => a_stKeyInfo, false, false);
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
#endregion // 조건부 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

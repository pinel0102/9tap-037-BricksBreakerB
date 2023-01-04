using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.IO;

/** 상품 교환 정보 */
[System.Serializable]
public struct STProductTradeInfo {
	public STCommonInfo m_stCommonInfo;
	public int m_nProductIdx;

	public EProductKinds m_eProductKinds;
	public EProductKinds m_ePrevProductKinds;
	public EProductKinds m_eNextProductKinds;
	public EPurchaseType m_ePurchaseType;

	public Dictionary<ulong, STTargetInfo> m_oPayTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oAcquireTargetInfoDict;

#region 상수
	public static STProductTradeInfo INVALID = new STProductTradeInfo() {
		m_eProductKinds = EProductKinds.NONE, m_ePrevProductKinds = EProductKinds.NONE, m_eNextProductKinds = EProductKinds.NONE, m_ePurchaseType = EPurchaseType.NONE
	};
#endregion // 상수

#region 프로퍼티
	public EProductType ProductType => (EProductType)((int)m_eProductKinds).ExKindsToType();
	public EProductKinds BaseProductKinds => (EProductKinds)((int)m_eProductKinds).ExKindsToSubKindsType();
#endregion // 프로퍼티

#region 함수
	/** 생성자 */
	public STProductTradeInfo(SimpleJSON.JSONNode a_oProductTradeInfo) {
		m_stCommonInfo = new STCommonInfo(a_oProductTradeInfo);
		m_nProductIdx = a_oProductTradeInfo[KCDefine.U_KEY_PRODUCT_IDX].AsInt;

		m_eProductKinds = a_oProductTradeInfo[KCDefine.U_KEY_PRODUCT_KINDS].ExIsValid() ? (EProductKinds)a_oProductTradeInfo[KCDefine.U_KEY_PRODUCT_KINDS].AsInt : EProductKinds.NONE;
		m_ePrevProductKinds = a_oProductTradeInfo[KCDefine.U_KEY_PREV_PRODUCT_KINDS].ExIsValid() ? (EProductKinds)a_oProductTradeInfo[KCDefine.U_KEY_PREV_PRODUCT_KINDS].AsInt : EProductKinds.NONE;
		m_eNextProductKinds = a_oProductTradeInfo[KCDefine.U_KEY_NEXT_PRODUCT_KINDS].ExIsValid() ? (EProductKinds)a_oProductTradeInfo[KCDefine.U_KEY_NEXT_PRODUCT_KINDS].AsInt : EProductKinds.NONE;
		m_ePurchaseType = a_oProductTradeInfo[KCDefine.U_KEY_PURCHASE_TYPE].ExIsValid() ? (EPurchaseType)a_oProductTradeInfo[KCDefine.U_KEY_PURCHASE_TYPE].AsInt : EPurchaseType.NONE;

		m_oPayTargetInfoDict = Factory.MakeTargetInfos(a_oProductTradeInfo, KCDefine.U_KEY_FMT_PAY_TARGET_INFO);
		m_oAcquireTargetInfoDict = Factory.MakeTargetInfos(a_oProductTradeInfo, KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO);
	}
#endregion // 함수

#region 조건부 함수
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 객체 정보를 저장한다 */
	public void SaveProductTradeInfo(SimpleJSON.JSONNode a_oOutProductTradeInfo) {
		m_stCommonInfo.SaveCommonInfo(a_oOutProductTradeInfo);
		a_oOutProductTradeInfo[KCDefine.U_KEY_PRODUCT_IDX] = $"{m_nProductIdx}";

		a_oOutProductTradeInfo[KCDefine.U_KEY_PRODUCT_KINDS] = $"{(int)m_eProductKinds}";
		a_oOutProductTradeInfo[KCDefine.U_KEY_PREV_PRODUCT_KINDS] = $"{(int)m_ePrevProductKinds}";
		a_oOutProductTradeInfo[KCDefine.U_KEY_NEXT_PRODUCT_KINDS] = $"{(int)m_eNextProductKinds}";
		a_oOutProductTradeInfo[KCDefine.U_KEY_PURCHASE_TYPE] = $"{(int)m_ePurchaseType}";

		Func.SaveTargetInfos(m_oPayTargetInfoDict, KCDefine.U_KEY_FMT_PAY_TARGET_INFO, a_oOutProductTradeInfo);
		Func.SaveTargetInfos(m_oAcquireTargetInfoDict, KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, a_oOutProductTradeInfo);
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
#endregion // 조건부 함수
}

/** 상품 교환 정보 테이블 */
public partial class CProductTradeInfoTable : CSingleton<CProductTradeInfoTable> {
#region 프로퍼티
	public Dictionary<EProductKinds, STProductTradeInfo> BuyProductTradeInfoDict { get; } = new Dictionary<EProductKinds, STProductTradeInfo>();
#endregion // 프로퍼티

#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetProductTradeInfos();
	}

	/** 상품 교환 정보를 리셋한다 */
	public virtual void ResetProductTradeInfos() {
		this.BuyProductTradeInfoDict.Clear();
	}

	/** 상품 교환 정보를 리셋한다 */
	public virtual void ResetProductTradeInfos(string a_oJSONStr) {
		this.ResetProductTradeInfos();
		this.DoLoadProductTradeInfos(a_oJSONStr);
	}

	/** 구입 상품 교환 정보를 반환한다 */
	public STProductTradeInfo GetBuyProductTradeTradeInfo(int a_nProductIdx) {
		bool bIsValid = this.TryGetBuyProductTradeInfo(a_nProductIdx, out STProductTradeInfo stProductTradeInfo);
		CAccess.Assert(bIsValid);

		return stProductTradeInfo;
	}

	/** 구입 상품 교환 정보를 반환한다 */
	public STProductTradeInfo GetBuyProductTradeTradeInfo(EProductKinds a_eProductKinds) {
		bool bIsValid = this.TryGetBuyProductTradeInfo(a_eProductKinds, out STProductTradeInfo stProductTradeInfo);
		CAccess.Assert(bIsValid);

		return stProductTradeInfo;
	}

	/** 구입 상품 교환 정보를 반환한다 */
	public bool TryGetBuyProductTradeInfo(int a_nProductIdx, out STProductTradeInfo a_stOutProductTradeInfo) {
		a_stOutProductTradeInfo = this.BuyProductTradeInfoDict.ExGetVal((a_stKeyVal) => a_stKeyVal.Value.m_nProductIdx == a_nProductIdx, STProductTradeInfo.INVALID);
		return a_stOutProductTradeInfo.m_eProductKinds != EProductKinds.NONE;
	}

	/** 구입 상품 교환 정보를 반환한다 */
	public bool TryGetBuyProductTradeInfo(EProductKinds a_eProductKinds, out STProductTradeInfo a_stOutProductTradeInfo) {
		a_stOutProductTradeInfo = this.BuyProductTradeInfoDict.GetValueOrDefault(a_eProductKinds, STProductTradeInfo.INVALID);
		return this.BuyProductTradeInfoDict.ContainsKey(a_eProductKinds);
	}

	/** 상품 교환 정보를 로드한다 */
	public Dictionary<EProductKinds, STProductTradeInfo> LoadProductTradeInfos() {
		this.ResetProductTradeInfos();
		return this.LoadProductTradeInfos(Access.ProductTradeInfoTableLoadPath);
	}

	/** 상품 교환 정보를 저장한다 */
	public void SaveProductTradeInfos(string a_oJSONStr, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oJSONStr != null);

		// JSON 문자열이 존재 할 경우
		if(a_oJSONStr != null) {
			this.ResetProductTradeInfos(a_oJSONStr);

#if PURCHASE_MODULE_ENABLE
			CProductInfoTable.Inst.SaveProductInfos(a_oJSONStr);
#endif // #if PURCHASE_MODULE_ENABLE

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
			CFunc.WriteStr(Access.ProductTradeInfoTableSavePath, a_oJSONStr, false);
#else
			CFunc.WriteStr(Access.ProductTradeInfoTableSavePath, a_oJSONStr, true);
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)

#if UNITY_ANDROID && (DEBUG || DEVELOPMENT)
			CUnityMsgSender.Inst.SendShowToastMsg($"CProductInfoTable.SaveProductInfos: {File.Exists(Access.ProductTradeInfoTableSavePath)}");
#endif // #if UNITY_ANDROID && (DEBUG || DEVELOPMENT)
		}
	}

	/** JSON 노드를 설정한다 */
	private void SetupJSONNodes(SimpleJSON.JSONNode a_oJSONNode, out SimpleJSON.JSONNode a_oOutCommonInfos) {
		var oSheetNameDictContainer = Access.GetSheetNames(this.GetType(), Access.ProductTradeTableInfo);
		a_oOutCommonInfos = a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_COMMON]].ExIsValid() ? a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_COMMON]] : KCDefine.B_EMPTY_JSON_ARRAY;
	}

	/** 상품 교환 정보를 로드한다 */
	private Dictionary<EProductKinds, STProductTradeInfo> LoadProductTradeInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		return this.DoLoadProductTradeInfos(this.LoadProductTradeInfosJSONStr(a_oFilePath));
	}

	/** 상품 교환 정보 JSON 문자열을 로드한다 */
	private string LoadProductTradeInfosJSONStr(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
		return File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, false) : CFunc.ReadStrFromRes(a_oFilePath, false);
#else
		return File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, true) : CFunc.ReadStrFromRes(a_oFilePath, false);
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 상품 교환 정보를 로드한다 */
	private Dictionary<EProductKinds, STProductTradeInfo> DoLoadProductTradeInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		this.SetupJSONNodes(SimpleJSON.JSONNode.Parse(a_oJSONStr), out SimpleJSON.JSONNode oCommonInfos);

		for(int i = 0; i < oCommonInfos.Count; ++i) {
			var stProductTradeInfo = new STProductTradeInfo(oCommonInfos[i]);

			// 구입 상품 교환 정보 추가 가능 할 경우
			if(stProductTradeInfo.m_eProductKinds.ExIsValid() && (!this.BuyProductTradeInfoDict.ContainsKey(stProductTradeInfo.m_eProductKinds) || oCommonInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
				this.BuyProductTradeInfoDict.ExReplaceVal(stProductTradeInfo.m_eProductKinds, stProductTradeInfo);
			}
		}

		return this.BuyProductTradeInfoDict;
	}
#endregion // 함수

#region 조건부 함수
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 상품 교환 정보를 저장한다 */
	public void SaveProductTradeInfos() {
		var oProductTradeInfos = SimpleJSON.JSONNode.Parse(this.LoadProductTradeInfosJSONStr(Access.ProductTradeInfoTableLoadPath));
		this.SetupJSONNodes(oProductTradeInfos, out SimpleJSON.JSONNode oCommonInfos);

		for(int i = 0; i < oCommonInfos.Count; ++i) {
			var oProductTradeKinds = oCommonInfos[i][KCDefine.U_KEY_PRODUCT_KINDS].ExIsValid() ? (EProductKinds)oCommonInfos[i][KCDefine.U_KEY_PRODUCT_KINDS].AsInt : EProductKinds.NONE;

			// 구입 상품 교환 정보가 존재 할 경우
			if(this.BuyProductTradeInfoDict.TryGetValue(oProductTradeKinds, out STProductTradeInfo stBuyProductTradeInfo)) {
				stBuyProductTradeInfo.SaveProductTradeInfo(oCommonInfos[i]);
			}
		}

		this.SaveProductTradeInfos(oProductTradeInfos.ToString());
	}

	/** 상품 교환 정보 값을 생성한다 */
	public Dictionary<string, List<List<string>>> MakeProductTradeInfoVals() {
		var oCommonKeyInfoList = CCollectionManager.Inst.SpawnList<STKeyInfo>();
		var oProductTradeInfoValDictContainer = new Dictionary<string, List<List<string>>>();

		try {
			this.SetupKeyInfos(oCommonKeyInfoList);
			this.SetupJSONNodes(SimpleJSON.JSONNode.Parse(this.LoadProductTradeInfosJSONStr(Access.ProductTradeInfoTableSavePath)), out SimpleJSON.JSONNode oCommonInfos);

			oProductTradeInfoValDictContainer.TryAdd(Access.GetSheetNames(this.GetType(), Access.ProductTradeTableInfo)[KCDefine.B_KEY_COMMON], oCommonInfos.AsArray.ExToInfoVals(oCommonKeyInfoList));
		} finally {
			CCollectionManager.Inst.DespawnList(oCommonKeyInfoList);
		}

		return oProductTradeInfoValDictContainer;
	}

	/** 키 정보를 설정한다 */
	private void SetupKeyInfos(List<STKeyInfo> a_oOutCommonKeyInfoList) {
		KDefine.G_KEY_INFO_GOOGLE_SHEET_COMMON_LIST.ExCopyTo(a_oOutCommonKeyInfoList, (a_stKeyInfo) => a_stKeyInfo);
		Access.ProductTradeTableInfo.m_oKeyInfoDictContainer[this.GetType()].GetValueOrDefault(KCDefine.B_KEY_COMMON)?.ExCopyTo(a_oOutCommonKeyInfoList, (a_stKeyInfo) => a_stKeyInfo, false, false);
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
#endregion // 조건부 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

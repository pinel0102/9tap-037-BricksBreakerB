using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.IO;

/** 아이템 정보 */
[System.Serializable]
public struct STItemInfo {
	public STCommonInfo m_stCommonInfo;

	public EItemKinds m_eItemKinds;
	public EItemKinds m_ePrevItemKinds;
	public EItemKinds m_eNextItemKinds;

	public Dictionary<ulong, STTargetInfo> m_oAttachItemTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oSkillTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oAbilityTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oAcquireTargetInfoDict;

	#region 상수
	public static STItemInfo INVALID = new STItemInfo() {
		m_eItemKinds = EItemKinds.NONE, m_ePrevItemKinds = EItemKinds.NONE, m_eNextItemKinds = EItemKinds.NONE
	};
	#endregion // 상수

	#region 프로퍼티
	public EItemType ItemType => (EItemType)((int)m_eItemKinds).ExKindsToType();
	public EItemKinds BaseItemKinds => (EItemKinds)((int)m_eItemKinds).ExKindsToSubKindsType();
	#endregion // 프로퍼티

	#region 함수
	/** 생성자 */
	public STItemInfo(SimpleJSON.JSONNode a_oItemInfo) {
		m_stCommonInfo = new STCommonInfo(a_oItemInfo);

		m_eItemKinds = a_oItemInfo[KCDefine.U_KEY_ITEM_KINDS].ExIsValid() ? (EItemKinds)a_oItemInfo[KCDefine.U_KEY_ITEM_KINDS].AsInt : EItemKinds.NONE;
		m_ePrevItemKinds = a_oItemInfo[KCDefine.U_KEY_PREV_ITEM_KINDS].ExIsValid() ? (EItemKinds)a_oItemInfo[KCDefine.U_KEY_PREV_ITEM_KINDS].AsInt : EItemKinds.NONE;
		m_eNextItemKinds = a_oItemInfo[KCDefine.U_KEY_NEXT_ITEM_KINDS].ExIsValid() ? (EItemKinds)a_oItemInfo[KCDefine.U_KEY_NEXT_ITEM_KINDS].AsInt : EItemKinds.NONE;

		m_oAttachItemTargetInfoDict = Factory.MakeTargetInfos(a_oItemInfo, KCDefine.U_KEY_FMT_ATTACH_ITEM_TARGET_INFO);
		m_oSkillTargetInfoDict = Factory.MakeTargetInfos(a_oItemInfo, KCDefine.U_KEY_FMT_SKILL_TARGET_INFO);
		m_oAbilityTargetInfoDict = Factory.MakeTargetInfos(a_oItemInfo, KCDefine.U_KEY_FMT_ABILITY_TARGET_INFO);
		m_oAcquireTargetInfoDict = Factory.MakeTargetInfos(a_oItemInfo, KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO);
	}
	#endregion // 함수

	#region 조건부 함수
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 아이템 정보를 저장한다 */
	public void SaveItemInfo(SimpleJSON.JSONNode a_oOutItemInfo) {
		m_stCommonInfo.SaveCommonInfo(a_oOutItemInfo);

		a_oOutItemInfo[KCDefine.U_KEY_ITEM_KINDS] = $"{(int)m_eItemKinds}";
		a_oOutItemInfo[KCDefine.U_KEY_PREV_ITEM_KINDS] = $"{(int)m_ePrevItemKinds}";
		a_oOutItemInfo[KCDefine.U_KEY_NEXT_ITEM_KINDS] = $"{(int)m_eNextItemKinds}";

		Func.SaveTargetInfos(m_oAttachItemTargetInfoDict, KCDefine.U_KEY_FMT_ATTACH_ITEM_TARGET_INFO, a_oOutItemInfo);
		Func.SaveTargetInfos(m_oSkillTargetInfoDict, KCDefine.U_KEY_FMT_SKILL_TARGET_INFO, a_oOutItemInfo);
		Func.SaveTargetInfos(m_oAbilityTargetInfoDict, KCDefine.U_KEY_FMT_ABILITY_TARGET_INFO, a_oOutItemInfo);
		Func.SaveTargetInfos(m_oAcquireTargetInfoDict, KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, a_oOutItemInfo);
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	#endregion // 조건부 함수
}

/** 아이템 교환 정보 */
[System.Serializable]
public struct STItemTradeInfo {
	public STCommonInfo m_stCommonInfo;

	public EItemKinds m_eItemKinds;
	public EItemKinds m_ePrevItemKinds;
	public EItemKinds m_eNextItemKinds;

	public Dictionary<ulong, STTargetInfo> m_oPayTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oAcquireTargetInfoDict;

	#region 상수
	public static STItemTradeInfo INVALID = new STItemTradeInfo() {
		m_eItemKinds = EItemKinds.NONE, m_ePrevItemKinds = EItemKinds.NONE, m_eNextItemKinds = EItemKinds.NONE
	};
	#endregion // 상수

	#region 프로퍼티
	public EItemType ItemType => (EItemType)((int)m_eItemKinds).ExKindsToType();
	public EItemKinds BaseItemKinds => (EItemKinds)((int)m_eItemKinds).ExKindsToSubKindsType();
	#endregion // 프로퍼티

	#region 함수
	/** 생성자 */
	public STItemTradeInfo(SimpleJSON.JSONNode a_oItemTradeInfo) {
		m_stCommonInfo = new STCommonInfo(a_oItemTradeInfo);

		m_eItemKinds = a_oItemTradeInfo[KCDefine.U_KEY_ITEM_KINDS].ExIsValid() ? (EItemKinds)a_oItemTradeInfo[KCDefine.U_KEY_ITEM_KINDS].AsInt : EItemKinds.NONE;
		m_ePrevItemKinds = a_oItemTradeInfo[KCDefine.U_KEY_PREV_ITEM_KINDS].ExIsValid() ? (EItemKinds)a_oItemTradeInfo[KCDefine.U_KEY_PREV_ITEM_KINDS].AsInt : EItemKinds.NONE;
		m_eNextItemKinds = a_oItemTradeInfo[KCDefine.U_KEY_NEXT_ITEM_KINDS].ExIsValid() ? (EItemKinds)a_oItemTradeInfo[KCDefine.U_KEY_NEXT_ITEM_KINDS].AsInt : EItemKinds.NONE;

		m_oPayTargetInfoDict = Factory.MakeTargetInfos(a_oItemTradeInfo, KCDefine.U_KEY_FMT_PAY_TARGET_INFO);
		m_oAcquireTargetInfoDict = Factory.MakeTargetInfos(a_oItemTradeInfo, KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO);
	}
	#endregion // 함수

	#region 조건부 함수
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 아이템 교환 정보를 저장한다 */
	public void SaveItemTradeInfo(SimpleJSON.JSONNode a_oOutItemTradeInfo) {
		m_stCommonInfo.SaveCommonInfo(a_oOutItemTradeInfo);

		a_oOutItemTradeInfo[KCDefine.U_KEY_ITEM_KINDS] = $"{(int)m_eItemKinds}";
		a_oOutItemTradeInfo[KCDefine.U_KEY_PREV_ITEM_KINDS] = $"{(int)m_ePrevItemKinds}";
		a_oOutItemTradeInfo[KCDefine.U_KEY_NEXT_ITEM_KINDS] = $"{(int)m_eNextItemKinds}";

		Func.SaveTargetInfos(m_oPayTargetInfoDict, KCDefine.U_KEY_FMT_PAY_TARGET_INFO, a_oOutItemTradeInfo);
		Func.SaveTargetInfos(m_oAcquireTargetInfoDict, KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, a_oOutItemTradeInfo);
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	#endregion // 조건부 함수
}

/** 아이템 정보 테이블 */
public partial class CItemInfoTable : CSingleton<CItemInfoTable> {
	#region 프로퍼티
	public Dictionary<EItemKinds, STItemInfo> ItemInfoDict { get; } = new Dictionary<EItemKinds, STItemInfo>();
	public Dictionary<EItemKinds, STItemTradeInfo> BuyItemTradeInfoDict { get; } = new Dictionary<EItemKinds, STItemTradeInfo>();
	public Dictionary<EItemKinds, STItemTradeInfo> SaleItemTradeInfoDict { get; } = new Dictionary<EItemKinds, STItemTradeInfo>();
	public Dictionary<EItemKinds, STItemTradeInfo> EnhanceItemTradeInfoDict { get; } = new Dictionary<EItemKinds, STItemTradeInfo>();
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetItemInfos();
	}

	/** 아이템 정보를 리셋한다 */
	public virtual void ResetItemInfos() {
		this.ItemInfoDict.Clear();
		this.BuyItemTradeInfoDict.Clear();
		this.SaleItemTradeInfoDict.Clear();
		this.EnhanceItemTradeInfoDict.Clear();
	}

	/** 아이템 정보를 리셋한다 */
	public virtual void ResetItemInfos(string a_oJSONStr) {
		this.ResetItemInfos();
		this.DoLoadItemInfos(a_oJSONStr);
	}

	/** 아이템 정보를 반환한다 */
	public STItemInfo GetItemInfo(EItemKinds a_EItemKinds) {
		bool bIsValid = this.TryGetItemInfo(a_EItemKinds, out STItemInfo stItemInfo);
		CAccess.Assert(bIsValid);

		return stItemInfo;
	}

	/** 구입 아이템 교환 정보를 반환한다 */
	public STItemTradeInfo GetBuyItemTradeInfo(EItemKinds a_eItemKinds) {
		bool bIsValid = this.TryGetBuyItemTradeInfo(a_eItemKinds, out STItemTradeInfo stItemTradeInfo);
		CAccess.Assert(bIsValid);

		return stItemTradeInfo;
	}

	/** 판매 아이템 교환 정보를 반환한다 */
	public STItemTradeInfo GetSaleItemTradeInfo(EItemKinds a_eItemKinds) {
		bool bIsValid = this.TryGetSaleItemTradeInfo(a_eItemKinds, out STItemTradeInfo stItemTradeInfo);
		CAccess.Assert(bIsValid);

		return stItemTradeInfo;
	}

	/** 강화 아이템 교환 정보를 반환한다 */
	public STItemTradeInfo GetEnhanceItemTradeInfo(EItemKinds a_eItemKinds) {
		bool bIsValid = this.TryGetEnhanceItemTradeInfo(a_eItemKinds, out STItemTradeInfo stItemTradeInfo);
		CAccess.Assert(bIsValid);

		return stItemTradeInfo;
	}

	/** 아이템 정보를 반환한다 */
	public bool TryGetItemInfo(EItemKinds a_EItemKinds, out STItemInfo a_stOutItemInfo) {
		a_stOutItemInfo = this.ItemInfoDict.GetValueOrDefault(a_EItemKinds, STItemInfo.INVALID);
		return this.ItemInfoDict.ContainsKey(a_EItemKinds);
	}

	/** 구입 아이템 교환 정보를 반환한다 */
	public bool TryGetBuyItemTradeInfo(EItemKinds a_eItemKinds, out STItemTradeInfo a_stOutItemTradeInfo) {
		a_stOutItemTradeInfo = this.BuyItemTradeInfoDict.GetValueOrDefault(a_eItemKinds, STItemTradeInfo.INVALID);
		return this.BuyItemTradeInfoDict.ContainsKey(a_eItemKinds);
	}

	/** 판매 아이템 교환 정보를 반환한다 */
	public bool TryGetSaleItemTradeInfo(EItemKinds a_eItemKinds, out STItemTradeInfo a_stOutItemTradeInfo) {
		a_stOutItemTradeInfo = this.SaleItemTradeInfoDict.GetValueOrDefault(a_eItemKinds, STItemTradeInfo.INVALID);
		return this.SaleItemTradeInfoDict.ContainsKey(a_eItemKinds);
	}

	/** 강화 아이템 교환 정보를 반환한다 */
	public bool TryGetEnhanceItemTradeInfo(EItemKinds a_eItemKinds, out STItemTradeInfo a_stOutItemTradeInfo) {
		a_stOutItemTradeInfo = this.EnhanceItemTradeInfoDict.GetValueOrDefault(a_eItemKinds, STItemTradeInfo.INVALID);
		return this.EnhanceItemTradeInfoDict.ContainsKey(a_eItemKinds);
	}

	/** 아이템 정보를 로드한다 */
	public (Dictionary<EItemKinds, STItemInfo>, Dictionary<EItemKinds, STItemTradeInfo>, Dictionary<EItemKinds, STItemTradeInfo>, Dictionary<EItemKinds, STItemTradeInfo>) LoadItemInfos() {
		this.ResetItemInfos();
		return this.LoadItemInfos(Access.ItemInfoTableLoadPath);
	}

	/** 아이템 정보를 저장한다 */
	public void SaveItemInfos(string a_oJSONStr, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oJSONStr != null);

		// JSON 문자열이 존재 할 경우
		if(a_oJSONStr != null) {
			this.ResetItemInfos(a_oJSONStr);

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
			CFunc.WriteStr(Access.ItemInfoTableSavePath, a_oJSONStr, false);
#else
			CFunc.WriteStr(Access.ItemInfoTableSavePath, a_oJSONStr, true);
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)

#if UNITY_ANDROID && (DEBUG || DEVELOPMENT)
			CUnityMsgSender.Inst.SendShowToastMsg($"CItemInfoTable.SaveItemInfos: {File.Exists(Access.ItemInfoTableSavePath)}");
#endif // #if UNITY_ANDROID && (DEBUG || DEVELOPMENT)
		}
	}

	/** JSON 노드를 설정한다 */
	private void SetupJSONNodes(SimpleJSON.JSONNode a_oJSONNode, out SimpleJSON.JSONNode a_oOutCommonInfos, out SimpleJSON.JSONNode a_oOutBuyTradeInfos, out SimpleJSON.JSONNode a_oOutSaleTradeInfos, out SimpleJSON.JSONNode a_oOutEnhanceTradeInfos) {
		var oSheetNameDictContainer = Access.GetSheetNames(this.GetType(), Access.ItemTableInfo);
		a_oOutCommonInfos = a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_COMMON]].ExIsValid() ? a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_COMMON]] : KCDefine.B_EMPTY_JSON_ARRAY;
		a_oOutBuyTradeInfos = a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_BUY_TRADE]].ExIsValid() ? a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_BUY_TRADE]] : KCDefine.B_EMPTY_JSON_ARRAY;
		a_oOutSaleTradeInfos = a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_SALE_TRADE]].ExIsValid() ? a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_SALE_TRADE]] : KCDefine.B_EMPTY_JSON_ARRAY;
		a_oOutEnhanceTradeInfos = a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_ENHANCE_TRADE]].ExIsValid() ? a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_ENHANCE_TRADE]] : KCDefine.B_EMPTY_JSON_ARRAY;
	}

	/** 아이템 정보를 로드한다 */
	private (Dictionary<EItemKinds, STItemInfo>, Dictionary<EItemKinds, STItemTradeInfo>, Dictionary<EItemKinds, STItemTradeInfo>, Dictionary<EItemKinds, STItemTradeInfo>) LoadItemInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		return this.DoLoadItemInfos(this.LoadItemInfosJSONStr(a_oFilePath));
	}

	/** 아이템 정보 JSON 문자열을 로드한다 */
	private string LoadItemInfosJSONStr(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
		return File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, false) : CFunc.ReadStrFromRes(a_oFilePath, false);
#else
		return File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, true) : CFunc.ReadStrFromRes(a_oFilePath, false);
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 아이템 정보를 로드한다 */
	private (Dictionary<EItemKinds, STItemInfo>, Dictionary<EItemKinds, STItemTradeInfo>, Dictionary<EItemKinds, STItemTradeInfo>, Dictionary<EItemKinds, STItemTradeInfo>) DoLoadItemInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		this.SetupJSONNodes(SimpleJSON.JSONNode.Parse(a_oJSONStr), out SimpleJSON.JSONNode oCommonInfos, out SimpleJSON.JSONNode oBuyItemTradeInfos, out SimpleJSON.JSONNode oSaleItemTradeInfos, out SimpleJSON.JSONNode oEnhanceItemTradeInfos);

		for(int i = 0; i < oCommonInfos.Count; ++i) {
			var stItemInfo = new STItemInfo(oCommonInfos[i]);

			// 아이템 정보 추가 가능 할 경우
			if(stItemInfo.m_eItemKinds.ExIsValid() && (!this.ItemInfoDict.ContainsKey(stItemInfo.m_eItemKinds) || oCommonInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
				this.ItemInfoDict.ExReplaceVal(stItemInfo.m_eItemKinds, stItemInfo);
			}
		}

		for(int i = 0; i < oBuyItemTradeInfos.Count; ++i) {
			var stItemTradeInfo = new STItemTradeInfo(oBuyItemTradeInfos[i]);

			// 구입 아이템 교환 정보 추가 가능 할 경우
			if(stItemTradeInfo.m_eItemKinds.ExIsValid() && (!this.BuyItemTradeInfoDict.ContainsKey(stItemTradeInfo.m_eItemKinds) || oBuyItemTradeInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
				this.BuyItemTradeInfoDict.ExReplaceVal(stItemTradeInfo.m_eItemKinds, stItemTradeInfo);
			}
		}

		for(int i = 0; i < oSaleItemTradeInfos.Count; ++i) {
			var stItemTradeInfo = new STItemTradeInfo(oSaleItemTradeInfos[i]);

			// 판매 아이템 교환 정보 추가 가능 할 경우
			if(stItemTradeInfo.m_eItemKinds.ExIsValid() && (!this.SaleItemTradeInfoDict.ContainsKey(stItemTradeInfo.m_eItemKinds) || oSaleItemTradeInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
				this.BuyItemTradeInfoDict.ExReplaceVal(stItemTradeInfo.m_eItemKinds, stItemTradeInfo);
			}
		}

		for(int i = 0; i < oEnhanceItemTradeInfos.Count; ++i) {
			var stItemTradeInfo = new STItemTradeInfo(oEnhanceItemTradeInfos[i]);

			// 강화 아이템 교환 정보 추가 가능 할 경우
			if(stItemTradeInfo.m_eItemKinds.ExIsValid() && (!this.EnhanceItemTradeInfoDict.ContainsKey(stItemTradeInfo.m_eItemKinds) || oEnhanceItemTradeInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
				this.EnhanceItemTradeInfoDict.ExReplaceVal(stItemTradeInfo.m_eItemKinds, stItemTradeInfo);
			}
		}

		return (this.ItemInfoDict, this.BuyItemTradeInfoDict, this.SaleItemTradeInfoDict, this.EnhanceItemTradeInfoDict);
	}
	#endregion // 함수

	#region 조건부 함수
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 아이템 정보를 저장한다 */
	public void SaveItemInfos() {
		var oItemInfos = SimpleJSON.JSONNode.Parse(this.LoadItemInfosJSONStr(Access.ItemInfoTableLoadPath));
		this.SetupJSONNodes(oItemInfos, out SimpleJSON.JSONNode oCommonInfos, out SimpleJSON.JSONNode oBuyTradeInfos, out SimpleJSON.JSONNode oSaleTradeInfos, out SimpleJSON.JSONNode oEnhanceTradeInfos);

		for(int i = 0; i < oCommonInfos.Count; ++i) {
			var eItemKinds = oCommonInfos[i][KCDefine.U_KEY_ITEM_KINDS].ExIsValid() ? (EItemKinds)oCommonInfos[i][KCDefine.U_KEY_ITEM_KINDS].AsInt : EItemKinds.NONE;

			// 아이템 정보가 존재 할 경우
			if(this.ItemInfoDict.TryGetValue(eItemKinds, out STItemInfo stItemInfo)) {
				stItemInfo.SaveItemInfo(oCommonInfos[i]);
			}
		}

		for(int i = 0; i < oBuyTradeInfos.Count; ++i) {
			var eItemKinds = oBuyTradeInfos[i][KCDefine.U_KEY_ITEM_KINDS].ExIsValid() ? (EItemKinds)oBuyTradeInfos[i][KCDefine.U_KEY_ITEM_KINDS].AsInt : EItemKinds.NONE;

			// 구입 아이템 교환 정보가 존재 할 경우
			if(this.BuyItemTradeInfoDict.TryGetValue(eItemKinds, out STItemTradeInfo stBuyItemTradeInfo)) {
				stBuyItemTradeInfo.SaveItemTradeInfo(oBuyTradeInfos[i]);
			}
		}

		for(int i = 0; i < oSaleTradeInfos.Count; ++i) {
			var eItemKinds = oSaleTradeInfos[i][KCDefine.U_KEY_ITEM_KINDS].ExIsValid() ? (EItemKinds)oSaleTradeInfos[i][KCDefine.U_KEY_ITEM_KINDS].AsInt : EItemKinds.NONE;

			// 판매 아이템 교환 정보가 존재 할 경우
			if(this.SaleItemTradeInfoDict.TryGetValue(eItemKinds, out STItemTradeInfo stSaleItemTradeInfo)) {
				stSaleItemTradeInfo.SaveItemTradeInfo(oSaleTradeInfos[i]);
			}
		}

		for(int i = 0; i < oEnhanceTradeInfos.Count; ++i) {
			var eItemKinds = oEnhanceTradeInfos[i][KCDefine.U_KEY_ITEM_KINDS].ExIsValid() ? (EItemKinds)oEnhanceTradeInfos[i][KCDefine.U_KEY_ITEM_KINDS].AsInt : EItemKinds.NONE;

			// 강화 아이템 교환 정보가 존재 할 경우
			if(this.EnhanceItemTradeInfoDict.TryGetValue(eItemKinds, out STItemTradeInfo stEnhanceItemTradeInfo)) {
				stEnhanceItemTradeInfo.SaveItemTradeInfo(oEnhanceTradeInfos[i]);
			}
		}

		this.SaveItemInfos(oItemInfos.ToString());
	}

	/** 아이템 정보 값을 생성한다 */
	public Dictionary<string, List<List<string>>> MakeItemInfoVals() {
		var oCommonKeyInfoList = CCollectionManager.Inst.SpawnList<STKeyInfo>();
		var oBuyTradeKeyInfoList = CCollectionManager.Inst.SpawnList<STKeyInfo>();
		var oSaleTradeKeyInfoList = CCollectionManager.Inst.SpawnList<STKeyInfo>();
		var oEnhanceTradeKeyInfoList = CCollectionManager.Inst.SpawnList<STKeyInfo>();

		var oItemInfoValDictContainer = new Dictionary<string, List<List<string>>>();

		try {
			this.SetupKeyInfos(oCommonKeyInfoList, oBuyTradeKeyInfoList, oSaleTradeKeyInfoList, oEnhanceTradeKeyInfoList);
			this.SetupJSONNodes(SimpleJSON.JSONNode.Parse(this.LoadItemInfosJSONStr(Access.ItemInfoTableSavePath)), out SimpleJSON.JSONNode oCommonInfos, out SimpleJSON.JSONNode oBuyTradeInfos, out SimpleJSON.JSONNode oSaleTradeInfos, out SimpleJSON.JSONNode oEnhanceTradeInfos);

			oItemInfoValDictContainer.TryAdd(Access.GetSheetNames(this.GetType(), Access.ItemTableInfo)[KCDefine.B_KEY_COMMON], oCommonInfos.AsArray.ExToInfoVals(oCommonKeyInfoList));
			oItemInfoValDictContainer.TryAdd(Access.GetSheetNames(this.GetType(), Access.ItemTableInfo)[KCDefine.B_KEY_BUY_TRADE], oCommonInfos.AsArray.ExToInfoVals(oBuyTradeKeyInfoList));
			oItemInfoValDictContainer.TryAdd(Access.GetSheetNames(this.GetType(), Access.ItemTableInfo)[KCDefine.B_KEY_SALE_TRADE], oCommonInfos.AsArray.ExToInfoVals(oSaleTradeKeyInfoList));
			oItemInfoValDictContainer.TryAdd(Access.GetSheetNames(this.GetType(), Access.ItemTableInfo)[KCDefine.B_KEY_ENHANCE_TRADE], oCommonInfos.AsArray.ExToInfoVals(oEnhanceTradeKeyInfoList));
		} finally {
			CCollectionManager.Inst.DespawnList(oCommonKeyInfoList);
			CCollectionManager.Inst.DespawnList(oBuyTradeKeyInfoList);
			CCollectionManager.Inst.DespawnList(oSaleTradeKeyInfoList);
			CCollectionManager.Inst.DespawnList(oEnhanceTradeKeyInfoList);
		}

		return oItemInfoValDictContainer;
	}

	/** 키 정보를 설정한다 */
	private void SetupKeyInfos(List<STKeyInfo> a_oOutCommonKeyInfoList, List<STKeyInfo> a_oOutBuyTradeKeyInfoList, List<STKeyInfo> a_oOutSaleTradeKeyInfoList, List<STKeyInfo> a_oOutEnhanceTradeKeyInfoList) {
		KDefine.G_KEY_INFO_GOOGLE_SHEET_COMMON_LIST.ExCopyTo(a_oOutCommonKeyInfoList, (a_stKeyInfo) => a_stKeyInfo);
		KDefine.G_KEY_INFO_GOOGLE_SHEET_COMMON_LIST.ExCopyTo(a_oOutBuyTradeKeyInfoList, (a_stKeyInfo) => a_stKeyInfo);
		KDefine.G_KEY_INFO_GOOGLE_SHEET_COMMON_LIST.ExCopyTo(a_oOutSaleTradeKeyInfoList, (a_stKeyInfo) => a_stKeyInfo);
		KDefine.G_KEY_INFO_GOOGLE_SHEET_COMMON_LIST.ExCopyTo(a_oOutEnhanceTradeKeyInfoList, (a_stKeyInfo) => a_stKeyInfo);

		Access.ItemTableInfo.m_oKeyInfoDictContainer[this.GetType()].GetValueOrDefault(KCDefine.B_KEY_COMMON)?.ExCopyTo(a_oOutCommonKeyInfoList, (a_stKeyInfo) => a_stKeyInfo, false, false);
		Access.ItemTableInfo.m_oKeyInfoDictContainer[this.GetType()].GetValueOrDefault(KCDefine.B_KEY_BUY_TRADE)?.ExCopyTo(a_oOutBuyTradeKeyInfoList, (a_stKeyInfo) => a_stKeyInfo, false, false);
		Access.ItemTableInfo.m_oKeyInfoDictContainer[this.GetType()].GetValueOrDefault(KCDefine.B_KEY_SALE_TRADE)?.ExCopyTo(a_oOutSaleTradeKeyInfoList, (a_stKeyInfo) => a_stKeyInfo, false, false);
		Access.ItemTableInfo.m_oKeyInfoDictContainer[this.GetType()].GetValueOrDefault(KCDefine.B_KEY_ENHANCE_TRADE)?.ExCopyTo(a_oOutEnhanceTradeKeyInfoList, (a_stKeyInfo) => a_stKeyInfo, false, false);
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	#endregion // 조건부 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

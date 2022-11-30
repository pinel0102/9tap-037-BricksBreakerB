using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.IO;

/** 스킬 정보 */
[System.Serializable]
public struct STSkillInfo {
	public STCommonInfo m_stCommonInfo;
	public STTimeInfo m_stTimeInfo;

	public int m_nMaxApplyTimes;
	public ESkillKinds m_eSkillKinds;
	public ESkillKinds m_ePrevSkillKinds;
	public ESkillKinds m_eNextSkillKinds;
	public ESkillApplyKinds m_eSkillApplyKinds;

	public List<EFXKinds> m_oFXKindsList;
	public List<EResKinds> m_oResKindsList;

	public Dictionary<ulong, STTargetInfo> m_oAbilityTargetInfoDict;

	#region 상수
	public static STSkillInfo INVALID = new STSkillInfo() {
		m_eSkillKinds = ESkillKinds.NONE, m_ePrevSkillKinds = ESkillKinds.NONE, m_eNextSkillKinds = ESkillKinds.NONE
	};
	#endregion // 상수

	#region 프로퍼티
	public ESkillType SkillType => (ESkillType)((int)m_eSkillKinds).ExKindsToType();
	public ESkillKinds BaseSkillKinds => (ESkillKinds)((int)m_eSkillKinds).ExKindsToSubKindsType();

	public ESkillApplyType SkillApplyType => (ESkillApplyType)((int)m_eSkillApplyKinds).ExKindsToType();
	public ESkillApplyKinds BaseSkillApplyKinds => (ESkillApplyKinds)((int)m_eSkillApplyKinds).ExKindsToSubKindsType();
	#endregion // 프로퍼티

	#region 함수
	/** 생성자 */
	public STSkillInfo(SimpleJSON.JSONNode a_oSkillInfo) {
		m_stCommonInfo = new STCommonInfo(a_oSkillInfo);
		m_stTimeInfo = new STTimeInfo(a_oSkillInfo[KCDefine.U_KEY_TIME_INFO]);

		m_nMaxApplyTimes = a_oSkillInfo[KCDefine.U_KEY_MAX_APPLY_TIMES].ExIsValid() ? a_oSkillInfo[KCDefine.U_KEY_MAX_APPLY_TIMES].AsInt : KCDefine.B_VAL_0_INT;
		m_eSkillKinds = a_oSkillInfo[KCDefine.U_KEY_SKILL_KINDS].ExIsValid() ? (ESkillKinds)a_oSkillInfo[KCDefine.U_KEY_SKILL_KINDS].AsInt : ESkillKinds.NONE;
		m_ePrevSkillKinds = a_oSkillInfo[KCDefine.U_KEY_PREV_SKILL_KINDS].ExIsValid() ? (ESkillKinds)a_oSkillInfo[KCDefine.U_KEY_PREV_SKILL_KINDS].AsInt : ESkillKinds.NONE;
		m_eNextSkillKinds = a_oSkillInfo[KCDefine.U_KEY_NEXT_SKILL_KINDS].ExIsValid() ? (ESkillKinds)a_oSkillInfo[KCDefine.U_KEY_NEXT_SKILL_KINDS].AsInt : ESkillKinds.NONE;
		m_eSkillApplyKinds = a_oSkillInfo[KCDefine.U_KEY_SKILL_APPLY_KINDS].ExIsValid() ? (ESkillApplyKinds)a_oSkillInfo[KCDefine.U_KEY_SKILL_APPLY_KINDS].AsInt : ESkillApplyKinds.NONE;

		m_oFXKindsList = Factory.MakeVals(a_oSkillInfo, KCDefine.U_KEY_FMT_FX_KINDS, (a_oJSONNode) => (EFXKinds)a_oJSONNode.AsInt);
		m_oResKindsList = Factory.MakeVals(a_oSkillInfo, KCDefine.U_KEY_FMT_RES_KINDS, (a_oJSONNode) => (EResKinds)a_oJSONNode.AsInt);

		m_oAbilityTargetInfoDict = Factory.MakeTargetInfos(a_oSkillInfo, KCDefine.U_KEY_FMT_ABILITY_TARGET_INFO);
	}
	#endregion // 함수

	#region 조건부 함수
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 스킬 정보를 저장한다 */
	public void SaveSkillInfo(SimpleJSON.JSONNode a_oOutSkillInfo) {
		m_stCommonInfo.SaveCommonInfo(a_oOutSkillInfo);
		m_stTimeInfo.SaveTimeInfo(a_oOutSkillInfo);

		a_oOutSkillInfo[KCDefine.U_KEY_MAX_APPLY_TIMES] = $"{m_nMaxApplyTimes}";
		a_oOutSkillInfo[KCDefine.U_KEY_SKILL_KINDS] = $"{(int)m_eSkillKinds}";
		a_oOutSkillInfo[KCDefine.U_KEY_PREV_SKILL_KINDS] = $"{(int)m_ePrevSkillKinds}";
		a_oOutSkillInfo[KCDefine.U_KEY_NEXT_SKILL_KINDS] = $"{(int)m_eNextSkillKinds}";
		a_oOutSkillInfo[KCDefine.U_KEY_SKILL_APPLY_KINDS] = $"{(int)m_eSkillApplyKinds}";

		Func.SaveVals(m_oFXKindsList, KCDefine.U_KEY_FMT_FX_KINDS, (a_eFXKinds) => $"{(int)a_eFXKinds}", a_oOutSkillInfo);
		Func.SaveVals(m_oResKindsList, KCDefine.U_KEY_FMT_RES_KINDS, (a_eResKinds) => $"{(int)a_eResKinds}", a_oOutSkillInfo);

		Func.SaveTargetInfos(m_oAbilityTargetInfoDict, KCDefine.U_KEY_FMT_ABILITY_TARGET_INFO, a_oOutSkillInfo);
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	#endregion // 조건부 함수
}

/** 스킬 교환 정보 */
[System.Serializable]
public struct STSkillTradeInfo {
	public STCommonInfo m_stCommonInfo;

	public ESkillKinds m_eSkillKinds;
	public ESkillKinds m_ePrevSkillKinds;
	public ESkillKinds m_eNextSkillKinds;

	public Dictionary<ulong, STTargetInfo> m_oPayTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oAcquireTargetInfoDict;

	#region 상수
	public static STSkillTradeInfo INVALID = new STSkillTradeInfo() {
		m_eSkillKinds = ESkillKinds.NONE, m_ePrevSkillKinds = ESkillKinds.NONE, m_eNextSkillKinds = ESkillKinds.NONE
	};
	#endregion // 상수

	#region 프로퍼티
	public ESkillType SkillType => (ESkillType)((int)m_eSkillKinds).ExKindsToType();
	public ESkillKinds BaseSkillKinds => (ESkillKinds)((int)m_eSkillKinds).ExKindsToSubKindsType();
	#endregion // 프로퍼티

	#region 함수
	/** 생성자 */
	public STSkillTradeInfo(SimpleJSON.JSONNode a_oSkillTradeInfo) {
		m_stCommonInfo = new STCommonInfo(a_oSkillTradeInfo);

		m_eSkillKinds = a_oSkillTradeInfo[KCDefine.U_KEY_SKILL_KINDS].ExIsValid() ? (ESkillKinds)a_oSkillTradeInfo[KCDefine.U_KEY_SKILL_KINDS].AsInt : ESkillKinds.NONE;
		m_ePrevSkillKinds = a_oSkillTradeInfo[KCDefine.U_KEY_PREV_SKILL_KINDS].ExIsValid() ? (ESkillKinds)a_oSkillTradeInfo[KCDefine.U_KEY_PREV_SKILL_KINDS].AsInt : ESkillKinds.NONE;
		m_eNextSkillKinds = a_oSkillTradeInfo[KCDefine.U_KEY_NEXT_SKILL_KINDS].ExIsValid() ? (ESkillKinds)a_oSkillTradeInfo[KCDefine.U_KEY_NEXT_SKILL_KINDS].AsInt : ESkillKinds.NONE;

		m_oPayTargetInfoDict = Factory.MakeTargetInfos(a_oSkillTradeInfo, KCDefine.U_KEY_FMT_PAY_TARGET_INFO);
		m_oAcquireTargetInfoDict = Factory.MakeTargetInfos(a_oSkillTradeInfo, KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO);
	}
	#endregion // 함수

	#region 조건부 함수
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 스킬 교환 정보를 저장한다 */
	public void SaveSkillTradeInfo(SimpleJSON.JSONNode a_oOutSkillTradeInfo) {
		m_stCommonInfo.SaveCommonInfo(a_oOutSkillTradeInfo);

		a_oOutSkillTradeInfo[KCDefine.U_KEY_SKILL_KINDS] = $"{(int)m_eSkillKinds}";
		a_oOutSkillTradeInfo[KCDefine.U_KEY_PREV_SKILL_KINDS] = $"{(int)m_ePrevSkillKinds}";
		a_oOutSkillTradeInfo[KCDefine.U_KEY_NEXT_SKILL_KINDS] = $"{(int)m_eNextSkillKinds}";

		Func.SaveTargetInfos(m_oPayTargetInfoDict, KCDefine.U_KEY_FMT_PAY_TARGET_INFO, a_oOutSkillTradeInfo);
		Func.SaveTargetInfos(m_oAcquireTargetInfoDict, KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, a_oOutSkillTradeInfo);
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	#endregion // 조건부 함수
}

/** 스킬 정보 테이블 */
public partial class CSkillInfoTable : CSingleton<CSkillInfoTable> {
	#region 프로퍼티
	public Dictionary<ESkillKinds, STSkillInfo> SkillInfoDict { get; } = new Dictionary<ESkillKinds, STSkillInfo>();
	public Dictionary<ESkillKinds, STSkillTradeInfo> BuySkillTradeInfoDict { get; } = new Dictionary<ESkillKinds, STSkillTradeInfo>();
	public Dictionary<ESkillKinds, STSkillTradeInfo> SaleSkillTradeInfoDict { get; } = new Dictionary<ESkillKinds, STSkillTradeInfo>();
	public Dictionary<ESkillKinds, STSkillTradeInfo> EnhanceSkillTradeInfoDict { get; } = new Dictionary<ESkillKinds, STSkillTradeInfo>();
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetSkillInfos();
	}

	/** 스킬 정보를 리셋한다 */
	public virtual void ResetSkillInfos() {
		this.SkillInfoDict.Clear();
		this.BuySkillTradeInfoDict.Clear();
		this.SaleSkillTradeInfoDict.Clear();
		this.EnhanceSkillTradeInfoDict.Clear();
	}

	/** 스킬 정보를 리셋한다 */
	public virtual void ResetSkillInfos(string a_oJSONStr) {
		this.ResetSkillInfos();
		this.DoLoadSkillInfos(a_oJSONStr);
	}

	/** 스킬 정보를 반환한다 */
	public STSkillInfo GetSkillInfo(ESkillKinds a_ESkillKinds) {
		bool bIsValid = this.TryGetSkillInfo(a_ESkillKinds, out STSkillInfo stSkillInfo);
		CAccess.Assert(bIsValid);

		return stSkillInfo;
	}

	/** 구입 스킬 교환 정보를 반환한다 */
	public STSkillTradeInfo GetBuySkillTradeInfo(ESkillKinds a_eSkillKinds) {
		bool bIsValid = this.TryGetBuySkillTradeInfo(a_eSkillKinds, out STSkillTradeInfo stSkillTradeInfo);
		CAccess.Assert(bIsValid);

		return stSkillTradeInfo;
	}

	/** 판매 스킬 교환 정보를 반환한다 */
	public STSkillTradeInfo GetSaleSkillTradeInfo(ESkillKinds a_eSkillKinds) {
		bool bIsValid = this.TryGetSaleSkillTradeInfo(a_eSkillKinds, out STSkillTradeInfo stSkillTradeInfo);
		CAccess.Assert(bIsValid);

		return stSkillTradeInfo;
	}

	/** 강화 스킬 교환 정보를 반환한다 */
	public STSkillTradeInfo GetEnhanceSkillTradeInfo(ESkillKinds a_eSkillKinds) {
		bool bIsValid = this.TryGetEnhanceSkillTradeInfo(a_eSkillKinds, out STSkillTradeInfo stSkillTradeInfo);
		CAccess.Assert(bIsValid);

		return stSkillTradeInfo;
	}

	/** 스킬 정보를 반환한다 */
	public bool TryGetSkillInfo(ESkillKinds a_ESkillKinds, out STSkillInfo a_stOutSkillInfo) {
		a_stOutSkillInfo = this.SkillInfoDict.GetValueOrDefault(a_ESkillKinds, STSkillInfo.INVALID);
		return this.SkillInfoDict.ContainsKey(a_ESkillKinds);
	}

	/** 구입 스킬 교환 정보를 반환한다 */
	public bool TryGetBuySkillTradeInfo(ESkillKinds a_eSkillKinds, out STSkillTradeInfo a_stOutSkillTradeInfo) {
		a_stOutSkillTradeInfo = this.BuySkillTradeInfoDict.GetValueOrDefault(a_eSkillKinds, STSkillTradeInfo.INVALID);
		return this.BuySkillTradeInfoDict.ContainsKey(a_eSkillKinds);
	}

	/** 판매 스킬 교환 정보를 반환한다 */
	public bool TryGetSaleSkillTradeInfo(ESkillKinds a_eSkillKinds, out STSkillTradeInfo a_stOutSkillTradeInfo) {
		a_stOutSkillTradeInfo = this.SaleSkillTradeInfoDict.GetValueOrDefault(a_eSkillKinds, STSkillTradeInfo.INVALID);
		return this.SaleSkillTradeInfoDict.ContainsKey(a_eSkillKinds);
	}

	/** 강화 스킬 교환 정보를 반환한다 */
	public bool TryGetEnhanceSkillTradeInfo(ESkillKinds a_eSkillKinds, out STSkillTradeInfo a_stOutSkillTradeInfo) {
		a_stOutSkillTradeInfo = this.EnhanceSkillTradeInfoDict.GetValueOrDefault(a_eSkillKinds, STSkillTradeInfo.INVALID);
		return this.EnhanceSkillTradeInfoDict.ContainsKey(a_eSkillKinds);
	}

	/** 스킬 정보를 로드한다 */
	public (Dictionary<ESkillKinds, STSkillInfo>, Dictionary<ESkillKinds, STSkillTradeInfo>, Dictionary<ESkillKinds, STSkillTradeInfo>, Dictionary<ESkillKinds, STSkillTradeInfo>) LoadSkillInfos() {
		this.ResetSkillInfos();
		return this.LoadSkillInfos(Access.SkillInfoTableLoadPath);
	}

	/** 스킬 정보를 저장한다 */
	public void SaveSkillInfos(string a_oJSONStr, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oJSONStr != null);

		// JSON 문자열이 존재 할 경우
		if(a_oJSONStr != null) {
			this.ResetSkillInfos(a_oJSONStr);

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
			CFunc.WriteStr(Access.SkillInfoTableSavePath, a_oJSONStr, false);
#else
			CFunc.WriteStr(Access.SkillInfoTableSavePath, a_oJSONStr, true);
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)

#if UNITY_ANDROID && (DEBUG || DEVELOPMENT)
			CUnityMsgSender.Inst.SendShowToastMsg($"CSkillInfoTable.SaveSkillInfos: {File.Exists(Access.SkillInfoTableSavePath)}");
#endif // #if UNITY_ANDROID && (DEBUG || DEVELOPMENT)
		}
	}

	/** JSON 노드를 설정한다 */
	private void SetupJSONNodes(SimpleJSON.JSONNode a_oJSONNode, out SimpleJSON.JSONNode a_oOutCommonInfos, out SimpleJSON.JSONNode a_oOutBuyTradeInfos, out SimpleJSON.JSONNode a_oOutSaleTradeInfos, out SimpleJSON.JSONNode a_oOutEnhanceTradeInfos) {
		var oSheetNameDictContainer = Access.GetSheetNames(this.GetType(), Access.SkillTableInfo);
		a_oOutCommonInfos = a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_COMMON]].ExIsValid() ? a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_COMMON]] : KCDefine.B_EMPTY_JSON_ARRAY;
		a_oOutBuyTradeInfos = a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_BUY_TRADE]].ExIsValid() ? a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_BUY_TRADE]] : KCDefine.B_EMPTY_JSON_ARRAY;
		a_oOutSaleTradeInfos = a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_SALE_TRADE]].ExIsValid() ? a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_SALE_TRADE]] : KCDefine.B_EMPTY_JSON_ARRAY;
		a_oOutEnhanceTradeInfos = a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_ENHANCE_TRADE]].ExIsValid() ? a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_ENHANCE_TRADE]] : KCDefine.B_EMPTY_JSON_ARRAY;
	}

	/** 스킬 정보를 로드한다 */
	private (Dictionary<ESkillKinds, STSkillInfo>, Dictionary<ESkillKinds, STSkillTradeInfo>, Dictionary<ESkillKinds, STSkillTradeInfo>, Dictionary<ESkillKinds, STSkillTradeInfo>) LoadSkillInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		return this.DoLoadSkillInfos(this.LoadSkillInfosJSONStr(a_oFilePath));
	}

	/** 스킬 정보 JSON 문자열을 로드한다 */
	private string LoadSkillInfosJSONStr(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
		return File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, false) : CFunc.ReadStrFromRes(a_oFilePath, false);
#else
		return File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, true) : CFunc.ReadStrFromRes(a_oFilePath, false);
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 스킬 정보를 로드한다 */
	private (Dictionary<ESkillKinds, STSkillInfo>, Dictionary<ESkillKinds, STSkillTradeInfo>, Dictionary<ESkillKinds, STSkillTradeInfo>, Dictionary<ESkillKinds, STSkillTradeInfo>) DoLoadSkillInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		this.SetupJSONNodes(SimpleJSON.JSONNode.Parse(a_oJSONStr), out SimpleJSON.JSONNode oCommonInfos, out SimpleJSON.JSONNode oBuyTradeInfos, out SimpleJSON.JSONNode oSaleTradeInfos, out SimpleJSON.JSONNode oEnhanceTradeInfos);

		for(int i = 0; i < oCommonInfos.Count; ++i) {
			var stSkillInfo = new STSkillInfo(oCommonInfos[i]);

			// 스킬 정보 추가 가능 할 경우
			if(stSkillInfo.m_eSkillKinds.ExIsValid() && (!this.SkillInfoDict.ContainsKey(stSkillInfo.m_eSkillKinds) || oCommonInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
				this.SkillInfoDict.ExReplaceVal(stSkillInfo.m_eSkillKinds, stSkillInfo);
			}
		}

		for(int i = 0; i < oBuyTradeInfos.Count; ++i) {
			var stSkillTradeInfo = new STSkillTradeInfo(oBuyTradeInfos[i]);

			// 구입 스킬 교환 정보 추가 가능 할 경우
			if(stSkillTradeInfo.m_eSkillKinds.ExIsValid() && (!this.BuySkillTradeInfoDict.ContainsKey(stSkillTradeInfo.m_eSkillKinds) || oBuyTradeInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
				this.BuySkillTradeInfoDict.ExReplaceVal(stSkillTradeInfo.m_eSkillKinds, stSkillTradeInfo);
			}
		}

		for(int i = 0; i < oSaleTradeInfos.Count; ++i) {
			var stSkillTradeInfo = new STSkillTradeInfo(oSaleTradeInfos[i]);

			// 판매 스킬 교환 정보 추가 가능 할 경우
			if(stSkillTradeInfo.m_eSkillKinds.ExIsValid() && (!this.SaleSkillTradeInfoDict.ContainsKey(stSkillTradeInfo.m_eSkillKinds) || oSaleTradeInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
				this.SaleSkillTradeInfoDict.ExReplaceVal(stSkillTradeInfo.m_eSkillKinds, stSkillTradeInfo);
			}
		}

		for(int i = 0; i < oEnhanceTradeInfos.Count; ++i) {
			var stSkillTradeInfo = new STSkillTradeInfo(oEnhanceTradeInfos[i]);

			// 강화 스킬 교환 정보 추가 가능 할 경우
			if(stSkillTradeInfo.m_eSkillKinds.ExIsValid() && (!this.BuySkillTradeInfoDict.ContainsKey(stSkillTradeInfo.m_eSkillKinds) || oEnhanceTradeInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
				this.EnhanceSkillTradeInfoDict.ExReplaceVal(stSkillTradeInfo.m_eSkillKinds, stSkillTradeInfo);
			}
		}

		return (this.SkillInfoDict, this.BuySkillTradeInfoDict, this.SaleSkillTradeInfoDict, this.EnhanceSkillTradeInfoDict);
	}
	#endregion // 함수

	#region 조건부 함수
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 스킬 정보를 저장한다 */
	public void SaveSkillInfos() {
		var oSkillInfos = SimpleJSON.JSONNode.Parse(this.LoadSkillInfosJSONStr(Access.SkillInfoTableLoadPath));
		this.SetupJSONNodes(oSkillInfos, out SimpleJSON.JSONNode oCommonInfos, out SimpleJSON.JSONNode oBuyTradeInfos, out SimpleJSON.JSONNode oSaleTradeInfos, out SimpleJSON.JSONNode oEnhanceTradeInfos);

		for(int i = 0; i < oCommonInfos.Count; ++i) {
			var eSkillKinds = oCommonInfos[i][KCDefine.U_KEY_SKILL_KINDS].ExIsValid() ? (ESkillKinds)oCommonInfos[i][KCDefine.U_KEY_ITEM_KINDS].AsInt : ESkillKinds.NONE;

			// 스킬 정보가 존재 할 경우
			if(this.SkillInfoDict.ContainsKey(eSkillKinds)) {
				this.SkillInfoDict[eSkillKinds].SaveSkillInfo(oCommonInfos[i]);
			}
		}

		for(int i = 0; i < oBuyTradeInfos.Count; ++i) {
			var eSkillKinds = oBuyTradeInfos[i][KCDefine.U_KEY_SKILL_KINDS].ExIsValid() ? (ESkillKinds)oBuyTradeInfos[i][KCDefine.U_KEY_ITEM_KINDS].AsInt : ESkillKinds.NONE;

			// 구입 스킬 교환 정보가 존재 할 경우
			if(this.BuySkillTradeInfoDict.ContainsKey(eSkillKinds)) {
				this.BuySkillTradeInfoDict[eSkillKinds].SaveSkillTradeInfo(oBuyTradeInfos[i]);
			}
		}

		for(int i = 0; i < oSaleTradeInfos.Count; ++i) {
			var eSkillKinds = oSaleTradeInfos[i][KCDefine.U_KEY_SKILL_KINDS].ExIsValid() ? (ESkillKinds)oSaleTradeInfos[i][KCDefine.U_KEY_ITEM_KINDS].AsInt : ESkillKinds.NONE;

			// 판매 스킬 교환 정보가 존재 할 경우
			if(this.SaleSkillTradeInfoDict.ContainsKey(eSkillKinds)) {
				this.SaleSkillTradeInfoDict[eSkillKinds].SaveSkillTradeInfo(oSaleTradeInfos[i]);
			}
		}

		for(int i = 0; i < oEnhanceTradeInfos.Count; ++i) {
			var eSkillKinds = oEnhanceTradeInfos[i][KCDefine.U_KEY_SKILL_KINDS].ExIsValid() ? (ESkillKinds)oEnhanceTradeInfos[i][KCDefine.U_KEY_ITEM_KINDS].AsInt : ESkillKinds.NONE;

			// 강화 스킬 교환 정보가 존재 할 경우
			if(this.EnhanceSkillTradeInfoDict.ContainsKey(eSkillKinds)) {
				this.EnhanceSkillTradeInfoDict[eSkillKinds].SaveSkillTradeInfo(oEnhanceTradeInfos[i]);
			}
		}

		this.SaveSkillInfos(oSkillInfos.ToString());
	}

	/** 스킬 정보 값을 생성한다 */
	public Dictionary<string, List<List<string>>> MakeSkillInfoVals() {
		var oCommonKeyInfoList = CCollectionManager.Inst.SpawnList<STKeyInfo>();
		var oBuyTradeKeyInfoList = CCollectionManager.Inst.SpawnList<STKeyInfo>();
		var oSaleTradeKeyInfoList = CCollectionManager.Inst.SpawnList<STKeyInfo>();
		var oEnhanceTradeKeyInfoList = CCollectionManager.Inst.SpawnList<STKeyInfo>();

		var oSkillInfoValDictContainer = new Dictionary<string, List<List<string>>>();

		try {
			this.SetupKeyInfos(oCommonKeyInfoList, oBuyTradeKeyInfoList, oSaleTradeKeyInfoList, oEnhanceTradeKeyInfoList);
			this.SetupJSONNodes(SimpleJSON.JSONNode.Parse(this.LoadSkillInfosJSONStr(Access.SkillInfoTableSavePath)), out SimpleJSON.JSONNode oCommonInfos, out SimpleJSON.JSONNode oBuyTradeInfos, out SimpleJSON.JSONNode oSaleTradeInfos, out SimpleJSON.JSONNode oEnhanceTradeInfos);

			oSkillInfoValDictContainer.TryAdd(Access.GetSheetNames(this.GetType(), Access.SkillTableInfo)[KCDefine.B_KEY_COMMON], oCommonInfos.AsArray.ExToInfoVals(oCommonKeyInfoList));
			oSkillInfoValDictContainer.TryAdd(Access.GetSheetNames(this.GetType(), Access.SkillTableInfo)[KCDefine.B_KEY_BUY_TRADE], oCommonInfos.AsArray.ExToInfoVals(oBuyTradeKeyInfoList));
			oSkillInfoValDictContainer.TryAdd(Access.GetSheetNames(this.GetType(), Access.SkillTableInfo)[KCDefine.B_KEY_SALE_TRADE], oCommonInfos.AsArray.ExToInfoVals(oSaleTradeKeyInfoList));
			oSkillInfoValDictContainer.TryAdd(Access.GetSheetNames(this.GetType(), Access.SkillTableInfo)[KCDefine.B_KEY_ENHANCE_TRADE], oCommonInfos.AsArray.ExToInfoVals(oEnhanceTradeKeyInfoList));
		} finally {
			CCollectionManager.Inst.DespawnList(oCommonKeyInfoList);
			CCollectionManager.Inst.DespawnList(oBuyTradeKeyInfoList);
			CCollectionManager.Inst.DespawnList(oSaleTradeKeyInfoList);
			CCollectionManager.Inst.DespawnList(oEnhanceTradeKeyInfoList);
		}

		return oSkillInfoValDictContainer;
	}

	/** 키 정보를 설정한다 */
	private void SetupKeyInfos(List<STKeyInfo> a_oOutCommonKeyInfoList, List<STKeyInfo> a_oOutBuyTradeKeyInfoList, List<STKeyInfo> a_oOutSaleTradeKeyInfoList, List<STKeyInfo> a_oOutEnhanceTradeKeyInfoList) {
		KDefine.G_KEY_INFO_GOOGLE_SHEET_COMMON_LIST.ExCopyTo(a_oOutCommonKeyInfoList, (a_stKeyInfo) => a_stKeyInfo);
		KDefine.G_KEY_INFO_GOOGLE_SHEET_COMMON_LIST.ExCopyTo(a_oOutBuyTradeKeyInfoList, (a_stKeyInfo) => a_stKeyInfo);
		KDefine.G_KEY_INFO_GOOGLE_SHEET_COMMON_LIST.ExCopyTo(a_oOutSaleTradeKeyInfoList, (a_stKeyInfo) => a_stKeyInfo);
		KDefine.G_KEY_INFO_GOOGLE_SHEET_COMMON_LIST.ExCopyTo(a_oOutEnhanceTradeKeyInfoList, (a_stKeyInfo) => a_stKeyInfo);

		Access.SkillTableInfo.m_oKeyInfoDictContainer[this.GetType()].GetValueOrDefault(KCDefine.B_KEY_COMMON)?.ExCopyTo(a_oOutCommonKeyInfoList, (a_stKeyInfo) => a_stKeyInfo, false, false);
		Access.SkillTableInfo.m_oKeyInfoDictContainer[this.GetType()].GetValueOrDefault(KCDefine.B_KEY_BUY_TRADE)?.ExCopyTo(a_oOutBuyTradeKeyInfoList, (a_stKeyInfo) => a_stKeyInfo, false, false);
		Access.SkillTableInfo.m_oKeyInfoDictContainer[this.GetType()].GetValueOrDefault(KCDefine.B_KEY_SALE_TRADE)?.ExCopyTo(a_oOutSaleTradeKeyInfoList, (a_stKeyInfo) => a_stKeyInfo, false, false);
		Access.SkillTableInfo.m_oKeyInfoDictContainer[this.GetType()].GetValueOrDefault(KCDefine.B_KEY_ENHANCE_TRADE)?.ExCopyTo(a_oOutEnhanceTradeKeyInfoList, (a_stKeyInfo) => a_stKeyInfo, false, false);
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	#endregion // 조건부 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

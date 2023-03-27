using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.IO;

/** 객체 정보 */
[System.Serializable]
public struct STObjInfo {
	public STCommonInfo m_stCommonInfo;
	public Vector3Int m_stSize;

    public bool m_bIsOverlay;
	public bool m_bIsClearTarget;

	public EObjKinds m_eObjKinds;
	public EObjKinds m_ePrevObjKinds;
	public EObjKinds m_eNextObjKinds;
	public ESkillKinds m_eActionSkillKinds;

	public List<EResKinds> m_oResKindsList;
    public List<EObjKinds> m_oExtraObjKindsList;

	public Dictionary<ulong, STTargetInfo> m_oDropItemTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oEquipItemTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oSkillTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oAbilityTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oAcquireTargetInfoDict;

    public bool m_bIsOnce;
	public bool m_bIsRand;
	public bool m_bIsTransparent;
	public bool m_bIsSkillTarget;

	public bool m_bIsEnableHit;
	public bool m_bIsEnableColor;
	public bool m_bIsEnableChange;
	public bool m_bIsEnableReflect;
	public bool m_bIsEnableRefract;
	public bool m_bIsEnableMoveDown;

	public EColliderType m_eColliderType;
	public ESkillKinds m_eHitSkillKinds;
	public ESkillKinds m_eDestroySkillKinds;

	public Vector3 m_stColliderSize;

	#region 상수
	public static STObjInfo INVALID = new STObjInfo() {
		m_eObjKinds = EObjKinds.NONE, m_ePrevObjKinds = EObjKinds.NONE, m_eNextObjKinds = EObjKinds.NONE
	};
	#endregion // 상수

	#region 프로퍼티
	public EObjType ObjType => (EObjType)((int)m_eObjKinds).ExKindsToType();
	public EObjKinds BaseObjKinds => (EObjKinds)((int)m_eObjKinds).ExKindsToSubKindsType();
	#endregion // 프로퍼티

	#region 함수
	/** 생성자 */
	public STObjInfo(SimpleJSON.JSONNode a_oObjInfo) {
		m_stCommonInfo = new STCommonInfo(a_oObjInfo);
		m_stSize = a_oObjInfo[KCDefine.U_KEY_SIZE].ExIsValid() ? new Vector3Int(a_oObjInfo[KCDefine.U_KEY_SIZE][KCDefine.B_VAL_0_INT].AsInt, a_oObjInfo[KCDefine.U_KEY_SIZE][KCDefine.B_VAL_1_INT].AsInt, a_oObjInfo[KCDefine.U_KEY_SIZE][KCDefine.B_VAL_2_INT].AsInt) : Vector3Int.one;

		m_bIsOverlay = a_oObjInfo[KDefine.G_KEY_IS_OVERLAY].ExIsValid() ? a_oObjInfo[KDefine.G_KEY_IS_OVERLAY].AsInt != KCDefine.B_VAL_0_INT : false;
		m_bIsClearTarget = a_oObjInfo[KDefine.G_KEY_IS_CLEAR_TARGET].ExIsValid() ? a_oObjInfo[KDefine.G_KEY_IS_CLEAR_TARGET].AsInt != KCDefine.B_VAL_0_INT : false;

		m_eObjKinds = a_oObjInfo[KCDefine.U_KEY_OBJ_KINDS].ExIsValid() ? (EObjKinds)a_oObjInfo[KCDefine.U_KEY_OBJ_KINDS].AsInt : EObjKinds.NONE;
		m_ePrevObjKinds = a_oObjInfo[KCDefine.U_KEY_PREV_OBJ_KINDS].ExIsValid() ? (EObjKinds)a_oObjInfo[KCDefine.U_KEY_PREV_OBJ_KINDS].AsInt : EObjKinds.NONE;
		m_eNextObjKinds = a_oObjInfo[KCDefine.U_KEY_NEXT_OBJ_KINDS].ExIsValid() ? (EObjKinds)a_oObjInfo[KCDefine.U_KEY_NEXT_OBJ_KINDS].AsInt : EObjKinds.NONE;
		m_eActionSkillKinds = a_oObjInfo[KCDefine.U_KEY_ACTION_SKILL_KINDS].ExIsValid() ? (ESkillKinds)a_oObjInfo[KCDefine.U_KEY_ACTION_SKILL_KINDS].AsInt : ESkillKinds.NONE;

		m_oResKindsList = Factory.MakeVals(a_oObjInfo, KCDefine.U_KEY_FMT_RES_KINDS, (a_oJSONNode) => (EResKinds)a_oJSONNode.AsInt);
		m_oExtraObjKindsList = Factory.MakeVals(a_oObjInfo, KCDefine.U_KEY_FMT_EXTRA_OBJ_KINDS, (a_oJSONNode) => (EObjKinds)a_oJSONNode.AsInt);

		m_oDropItemTargetInfoDict = Factory.MakeTargetInfos(a_oObjInfo, KCDefine.U_KEY_FMT_DROP_ITEM_TARGET_INFO);
		m_oEquipItemTargetInfoDict = Factory.MakeTargetInfos(a_oObjInfo, KCDefine.U_KEY_FMT_EQUIP_ITEM_TARGET_INFO);
		m_oSkillTargetInfoDict = Factory.MakeTargetInfos(a_oObjInfo, KCDefine.U_KEY_FMT_SKILL_TARGET_INFO);
		m_oAbilityTargetInfoDict = Factory.MakeTargetInfos(a_oObjInfo, KCDefine.U_KEY_FMT_ABILITY_TARGET_INFO);
		m_oAcquireTargetInfoDict = Factory.MakeTargetInfos(a_oObjInfo, KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO);

		#region 추가
		m_bIsOnce = a_oObjInfo[KDefine.G_KEY_IS_ONCE].ExIsValid() ? a_oObjInfo[KDefine.G_KEY_IS_ONCE].AsInt != KCDefine.B_VAL_0_INT : false;
		m_bIsRand = a_oObjInfo[KDefine.G_KEY_IS_RAND].ExIsValid() ? a_oObjInfo[KDefine.G_KEY_IS_RAND].AsInt != KCDefine.B_VAL_0_INT : false;
		m_bIsTransparent = a_oObjInfo[KDefine.G_KEY_IS_TRANSPARENT].ExIsValid() ? a_oObjInfo[KDefine.G_KEY_IS_TRANSPARENT].AsInt != KCDefine.B_VAL_0_INT : false;
		m_bIsSkillTarget = a_oObjInfo[KDefine.G_KEY_IS_SKILL_TARGET].ExIsValid() ? a_oObjInfo[KDefine.G_KEY_IS_SKILL_TARGET].AsInt != KCDefine.B_VAL_0_INT : false;

		m_bIsEnableHit = a_oObjInfo[KDefine.G_KEY_IS_ENABLE_HIT].ExIsValid() ? a_oObjInfo[KDefine.G_KEY_IS_ENABLE_HIT].AsInt != KCDefine.B_VAL_0_INT : false;
		m_bIsEnableColor = a_oObjInfo[KDefine.G_KEY_IS_ENABLE_COLOR].ExIsValid() ? a_oObjInfo[KDefine.G_KEY_IS_ENABLE_COLOR].AsInt != KCDefine.B_VAL_0_INT : false;
		m_bIsEnableChange = a_oObjInfo[KDefine.G_KEY_IS_ENABLE_CHANGE].ExIsValid() ? a_oObjInfo[KDefine.G_KEY_IS_ENABLE_CHANGE].AsInt != KCDefine.B_VAL_0_INT : false;
		m_bIsEnableReflect = a_oObjInfo[KDefine.G_KEY_IS_ENABLE_REFLECT].ExIsValid() ? a_oObjInfo[KDefine.G_KEY_IS_ENABLE_REFLECT].AsInt != KCDefine.B_VAL_0_INT : false;
		m_bIsEnableRefract = a_oObjInfo[KDefine.G_KEY_IS_ENABLE_REFRACT].ExIsValid() ? a_oObjInfo[KDefine.G_KEY_IS_ENABLE_REFRACT].AsInt != KCDefine.B_VAL_0_INT : false;
		m_bIsEnableMoveDown = a_oObjInfo[KDefine.G_KEY_IS_ENABLE_MOVE_DOWN].ExIsValid() ? a_oObjInfo[KDefine.G_KEY_IS_ENABLE_MOVE_DOWN].AsInt != KCDefine.B_VAL_0_INT : false;

		m_eColliderType = a_oObjInfo[KDefine.G_KEY_COLLIDER_TYPE].ExIsValid() ? (EColliderType)a_oObjInfo[KDefine.G_KEY_COLLIDER_TYPE].AsInt : EColliderType.NONE;
		m_eHitSkillKinds = a_oObjInfo[KDefine.G_KEY_HIT_SKILL_KINDS].ExIsValid() ? (ESkillKinds)a_oObjInfo[KDefine.G_KEY_HIT_SKILL_KINDS].AsInt : ESkillKinds.NONE;
		m_eDestroySkillKinds = a_oObjInfo[KDefine.G_KEY_DESTROY_SKILL_KINDS].ExIsValid() ? (ESkillKinds)a_oObjInfo[KDefine.G_KEY_DESTROY_SKILL_KINDS].AsInt : ESkillKinds.NONE;

		m_stColliderSize = a_oObjInfo[KDefine.G_KEY_COLLIDER_SIZE].ExIsValid() ? new Vector3(a_oObjInfo[KDefine.G_KEY_COLLIDER_SIZE][KCDefine.B_VAL_0_INT].AsFloat, a_oObjInfo[KDefine.G_KEY_COLLIDER_SIZE][KCDefine.B_VAL_1_INT].AsFloat, a_oObjInfo[KDefine.G_KEY_COLLIDER_SIZE][KCDefine.B_VAL_2_INT].AsFloat) : Vector3.zero;
	#endregion // 추가
	}
	#endregion // 함수

	#region 조건부 함수
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 객체 정보를 저장한다 */
	public void SaveObjInfo(SimpleJSON.JSONNode a_oOutObjInfo) {
		m_stCommonInfo.SaveCommonInfo(a_oOutObjInfo);

		a_oOutObjInfo[KCDefine.U_KEY_SIZE][KCDefine.B_VAL_0_INT] = $"{m_stSize.x:0.0}";
		a_oOutObjInfo[KCDefine.U_KEY_SIZE][KCDefine.B_VAL_1_INT] = $"{m_stSize.y:0.0}";
		a_oOutObjInfo[KCDefine.U_KEY_SIZE][KCDefine.B_VAL_2_INT] = $"{m_stSize.z:0.0}";

		a_oOutObjInfo[KCDefine.U_KEY_OBJ_KINDS] = $"{(int)m_eObjKinds}";
		a_oOutObjInfo[KCDefine.U_KEY_PREV_OBJ_KINDS] = $"{(int)m_ePrevObjKinds}";
		a_oOutObjInfo[KCDefine.U_KEY_NEXT_OBJ_KINDS] = $"{(int)m_eNextObjKinds}";
		a_oOutObjInfo[KCDefine.U_KEY_ACTION_SKILL_KINDS] = $"{(int)m_eActionSkillKinds}";

		Func.SaveVals(m_oResKindsList, KCDefine.U_KEY_FMT_RES_KINDS, (a_eResKinds) => $"{(int)a_eResKinds}", a_oOutObjInfo);

		Func.SaveTargetInfos(m_oDropItemTargetInfoDict, KCDefine.U_KEY_FMT_DROP_ITEM_TARGET_INFO, a_oOutObjInfo);
		Func.SaveTargetInfos(m_oEquipItemTargetInfoDict, KCDefine.U_KEY_FMT_EQUIP_ITEM_TARGET_INFO, a_oOutObjInfo);
		Func.SaveTargetInfos(m_oSkillTargetInfoDict, KCDefine.U_KEY_FMT_SKILL_TARGET_INFO, a_oOutObjInfo);
		Func.SaveTargetInfos(m_oAbilityTargetInfoDict, KCDefine.U_KEY_FMT_ABILITY_TARGET_INFO, a_oOutObjInfo);
		Func.SaveTargetInfos(m_oAcquireTargetInfoDict, KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, a_oOutObjInfo);
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	#endregion // 조건부 함수
}

/** 객체 교환 정보 */
[System.Serializable]
public struct STObjTradeInfo {
	public STCommonInfo m_stCommonInfo;

	public EObjKinds m_eObjKinds;
	public EObjKinds m_ePrevObjKinds;
	public EObjKinds m_eNextObjKinds;

	public Dictionary<ulong, STTargetInfo> m_oPayTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oAcquireTargetInfoDict;

	#region 상수
	public static STObjTradeInfo INVALID = new STObjTradeInfo() {
		m_eObjKinds = EObjKinds.NONE, m_ePrevObjKinds = EObjKinds.NONE, m_eNextObjKinds = EObjKinds.NONE
	};
	#endregion // 상수

	#region 프로퍼티
	public EObjType ObjType => (EObjType)((int)m_eObjKinds).ExKindsToType();
	public EObjKinds BaseObjKinds => (EObjKinds)((int)m_eObjKinds).ExKindsToSubKindsType();
	#endregion // 프로퍼티

	#region 함수
	/** 생성자 */
	public STObjTradeInfo(SimpleJSON.JSONNode a_oObjTradeInfo) {
		m_stCommonInfo = new STCommonInfo(a_oObjTradeInfo);

		m_eObjKinds = a_oObjTradeInfo[KCDefine.U_KEY_OBJ_KINDS].ExIsValid() ? (EObjKinds)a_oObjTradeInfo[KCDefine.U_KEY_OBJ_KINDS].AsInt : EObjKinds.NONE;
		m_ePrevObjKinds = a_oObjTradeInfo[KCDefine.U_KEY_PREV_OBJ_KINDS].ExIsValid() ? (EObjKinds)a_oObjTradeInfo[KCDefine.U_KEY_PREV_OBJ_KINDS].AsInt : EObjKinds.NONE;
		m_eNextObjKinds = a_oObjTradeInfo[KCDefine.U_KEY_NEXT_OBJ_KINDS].ExIsValid() ? (EObjKinds)a_oObjTradeInfo[KCDefine.U_KEY_NEXT_OBJ_KINDS].AsInt : EObjKinds.NONE;

		m_oPayTargetInfoDict = Factory.MakeTargetInfos(a_oObjTradeInfo, KCDefine.U_KEY_FMT_PAY_TARGET_INFO);
		m_oAcquireTargetInfoDict = Factory.MakeTargetInfos(a_oObjTradeInfo, KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO);
	}
	#endregion // 함수

	#region 조건부 함수
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 객체 교환 정보를 저장한다 */
	public void SaveObjTradeInfo(SimpleJSON.JSONNode a_oOutObjTradeInfo) {
		m_stCommonInfo.SaveCommonInfo(a_oOutObjTradeInfo);

		a_oOutObjTradeInfo[KCDefine.U_KEY_OBJ_KINDS] = $"{(int)m_eObjKinds}";
		a_oOutObjTradeInfo[KCDefine.U_KEY_PREV_OBJ_KINDS] = $"{(int)m_ePrevObjKinds}";
		a_oOutObjTradeInfo[KCDefine.U_KEY_NEXT_OBJ_KINDS] = $"{(int)m_eNextObjKinds}";

		Func.SaveTargetInfos(m_oPayTargetInfoDict, KCDefine.U_KEY_FMT_PAY_TARGET_INFO, a_oOutObjTradeInfo);
		Func.SaveTargetInfos(m_oAcquireTargetInfoDict, KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, a_oOutObjTradeInfo);
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	#endregion // 조건부 함수
}

/** 객체 정보 테이블 */
public partial class CObjInfoTable : CSingleton<CObjInfoTable> {
	#region 프로퍼티
	public Dictionary<EObjKinds, STObjInfo> ObjInfoDict { get; } = new Dictionary<EObjKinds, STObjInfo>();
	public Dictionary<EObjKinds, STObjTradeInfo> BuyObjTradeInfoDict { get; } = new Dictionary<EObjKinds, STObjTradeInfo>();
	public Dictionary<EObjKinds, STObjTradeInfo> SaleObjTradeInfoDict { get; } = new Dictionary<EObjKinds, STObjTradeInfo>();
	public Dictionary<EObjKinds, STObjTradeInfo> EnhanceObjTradeInfoDict { get; } = new Dictionary<EObjKinds, STObjTradeInfo>();
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetObjInfos();
	}

	/** 객체 정보를 리셋한다 */
	public virtual void ResetObjInfos() {
		this.ObjInfoDict.Clear();
		this.BuyObjTradeInfoDict.Clear();
		this.SaleObjTradeInfoDict.Clear();
		this.EnhanceObjTradeInfoDict.Clear();
	}

	/** 객체 정보를 리셋한다 */
	public virtual void ResetObjInfos(string a_oJSONStr) {
		this.ResetObjInfos();
		this.DoLoadObjInfos(a_oJSONStr);
	}

	/** 객체 정보를 반환한다 */
	public STObjInfo GetObjInfo(EObjKinds a_eObjKinds) {
        bool bIsValid = this.TryGetObjInfo(a_eObjKinds, out STObjInfo stObjInfo);
		CAccess.Assert(bIsValid);

		return stObjInfo;
	}

	/** 구입 객체 교환 정보를 반환한다 */
	public STObjTradeInfo GetBuyObjTradeInfo(EObjKinds a_eObjKinds) {
		bool bIsValid = this.TryGetBuyObjTradeInfo(a_eObjKinds, out STObjTradeInfo stObjTradeInfo);
		CAccess.Assert(bIsValid);

		return stObjTradeInfo;
	}

	/** 판매 객체 교환 정보를 반환한다 */
	public STObjTradeInfo GetSaleObjTradeInfo(EObjKinds a_eObjKinds) {
		bool bIsValid = this.TryGetSaleObjTradeInfo(a_eObjKinds, out STObjTradeInfo stObjTradeInfo);
		CAccess.Assert(bIsValid);

		return stObjTradeInfo;
	}

	/** 강화 객체 교환 정보를 반환한다 */
	public STObjTradeInfo GetEnhanceObjTradeInfo(EObjKinds a_eObjKinds) {
		bool bIsValid = this.TryGetEnhanceObjTradeInfo(a_eObjKinds, out STObjTradeInfo stObjTradeInfo);
		CAccess.Assert(bIsValid);

		return stObjTradeInfo;
	}

	/** 객체 정보를 반환한다 */
	public bool TryGetObjInfo(EObjKinds a_eObjKinds, out STObjInfo a_stOutObjInfo) {
		a_stOutObjInfo = this.ObjInfoDict.GetValueOrDefault(a_eObjKinds, STObjInfo.INVALID);
		return this.ObjInfoDict.ContainsKey(a_eObjKinds);
	}

	/** 구입 객체 교환 정보를 반환한다 */
	public bool TryGetBuyObjTradeInfo(EObjKinds a_eObjKinds, out STObjTradeInfo a_stOutObjTradeInfo) {
		a_stOutObjTradeInfo = this.BuyObjTradeInfoDict.GetValueOrDefault(a_eObjKinds, STObjTradeInfo.INVALID);
		return this.BuyObjTradeInfoDict.ContainsKey(a_eObjKinds);
	}

	/** 판매 객체 교환 정보를 반환한다 */
	public bool TryGetSaleObjTradeInfo(EObjKinds a_eObjKinds, out STObjTradeInfo a_stOutObjTradeInfo) {
		a_stOutObjTradeInfo = this.SaleObjTradeInfoDict.GetValueOrDefault(a_eObjKinds, STObjTradeInfo.INVALID);
		return this.SaleObjTradeInfoDict.ContainsKey(a_eObjKinds);
	}

	/** 강화 객체 교환 정보를 반환한다 */
	public bool TryGetEnhanceObjTradeInfo(EObjKinds a_eObjKinds, out STObjTradeInfo a_stOutObjTradeInfo) {
		a_stOutObjTradeInfo = this.EnhanceObjTradeInfoDict.GetValueOrDefault(a_eObjKinds, STObjTradeInfo.INVALID);
		return this.EnhanceObjTradeInfoDict.ContainsKey(a_eObjKinds);
	}

	/** 객체 정보를 로드한다 */
	public (Dictionary<EObjKinds, STObjInfo>, Dictionary<EObjKinds, STObjTradeInfo>, Dictionary<EObjKinds, STObjTradeInfo>, Dictionary<EObjKinds, STObjTradeInfo>) LoadObjInfos() {
		this.ResetObjInfos();
		return this.LoadObjInfos(Access.ObjInfoTableLoadPath);
	}

	/** 객체 정보를 저장한다 */
	public void SaveObjInfos(string a_oJSONStr, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oJSONStr != null);

		// JSON 문자열이 존재 할 경우
		if(a_oJSONStr != null) {
			this.ResetObjInfos(a_oJSONStr);

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
			CFunc.WriteStr(Access.ObjInfoTableSavePath, a_oJSONStr, false);
#else
			CFunc.WriteStr(Access.ObjInfoTableSavePath, a_oJSONStr, true);
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)

#if UNITY_ANDROID && (DEBUG || DEVELOPMENT)
			CUnityMsgSender.Inst.SendShowToastMsg($"CObjInfoTable.SaveObjInfos: {File.Exists(Access.ObjInfoTableSavePath)}");
#endif // #if UNITY_ANDROID && (DEBUG || DEVELOPMENT)
		}
	}

	/** JSON 노드를 설정한다 */
	private void SetupJSONNodes(SimpleJSON.JSONNode a_oJSONNode, out SimpleJSON.JSONNode a_oOutCommonInfos, out SimpleJSON.JSONNode a_oOutBuyTradeInfos, out SimpleJSON.JSONNode a_oOutSaleTradeInfos, out SimpleJSON.JSONNode a_oOutEnhanceTradeInfos) {
		var oSheetNameDictContainer = Access.GetSheetNames(this.GetType(), Access.ObjTableInfo);
		a_oOutCommonInfos = a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_COMMON]].ExIsValid() ? a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_COMMON]] : KCDefine.B_EMPTY_JSON_ARRAY;
		a_oOutBuyTradeInfos = a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_BUY_TRADE]].ExIsValid() ? a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_BUY_TRADE]] : KCDefine.B_EMPTY_JSON_ARRAY;
		a_oOutSaleTradeInfos = a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_SALE_TRADE]].ExIsValid() ? a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_SALE_TRADE]] : KCDefine.B_EMPTY_JSON_ARRAY;
		a_oOutEnhanceTradeInfos = a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_ENHANCE_TRADE]].ExIsValid() ? a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_ENHANCE_TRADE]] : KCDefine.B_EMPTY_JSON_ARRAY;
	}

	/** 객체 정보를 로드한다 */
	private (Dictionary<EObjKinds, STObjInfo>, Dictionary<EObjKinds, STObjTradeInfo>, Dictionary<EObjKinds, STObjTradeInfo>, Dictionary<EObjKinds, STObjTradeInfo>) LoadObjInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
        return this.DoLoadObjInfos(this.LoadObjInfosJSONStr(a_oFilePath));
	}

	/** 객체 정보 JSON 문자열을 로드한다 */
	private string LoadObjInfosJSONStr(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
		return File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, false) : CFunc.ReadStrFromRes(a_oFilePath, false);
#else
		return File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, true) : CFunc.ReadStrFromRes(a_oFilePath, false);
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 객체 정보를 로드한다 */
	private (Dictionary<EObjKinds, STObjInfo>, Dictionary<EObjKinds, STObjTradeInfo>, Dictionary<EObjKinds, STObjTradeInfo>, Dictionary<EObjKinds, STObjTradeInfo>) DoLoadObjInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		this.SetupJSONNodes(SimpleJSON.JSONNode.Parse(a_oJSONStr), out SimpleJSON.JSONNode oCommonInfos, out SimpleJSON.JSONNode oBuyTradeInfos, out SimpleJSON.JSONNode oSaleTradeInfos, out SimpleJSON.JSONNode oEnhanceTradeInfos);

		for(int i = 0; i < oCommonInfos.Count; ++i) {
			var stObjInfo = new STObjInfo(oCommonInfos[i]);

			// 객체 정보 추가 가능 할 경우
			if(stObjInfo.m_eObjKinds.ExIsValid() && (!this.ObjInfoDict.ContainsKey(stObjInfo.m_eObjKinds) || oCommonInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
				this.ObjInfoDict.ExReplaceVal(stObjInfo.m_eObjKinds, stObjInfo);
			}
		}

		for(int i = 0; i < oBuyTradeInfos.Count; ++i) {
			var stObjTradeInfo = new STObjTradeInfo(oBuyTradeInfos[i]);

			// 구입 객체 교환 정보 추가 가능 할 경우
			if(stObjTradeInfo.m_eObjKinds.ExIsValid() && (!this.BuyObjTradeInfoDict.ContainsKey(stObjTradeInfo.m_eObjKinds) || oBuyTradeInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
				this.BuyObjTradeInfoDict.ExReplaceVal(stObjTradeInfo.m_eObjKinds, stObjTradeInfo);
			}
		}

		for(int i = 0; i < oSaleTradeInfos.Count; ++i) {
			var stObjTradeInfo = new STObjTradeInfo(oSaleTradeInfos[i]);

			// 판매 객체 교환 정보 추가 가능 할 경우
			if(stObjTradeInfo.m_eObjKinds.ExIsValid() && (!this.SaleObjTradeInfoDict.ContainsKey(stObjTradeInfo.m_eObjKinds) || oSaleTradeInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
				this.SaleObjTradeInfoDict.ExReplaceVal(stObjTradeInfo.m_eObjKinds, stObjTradeInfo);
			}
		}

		for(int i = 0; i < oEnhanceTradeInfos.Count; ++i) {
			var stObjTradeInfo = new STObjTradeInfo(oEnhanceTradeInfos[i]);

			// 강화 객체 교환 정보 추가 가능 할 경우
			if(stObjTradeInfo.m_eObjKinds.ExIsValid() && (!this.BuyObjTradeInfoDict.ContainsKey(stObjTradeInfo.m_eObjKinds) || oEnhanceTradeInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
				this.EnhanceObjTradeInfoDict.ExReplaceVal(stObjTradeInfo.m_eObjKinds, stObjTradeInfo);
			}
		}

		return (this.ObjInfoDict, this.BuyObjTradeInfoDict, this.SaleObjTradeInfoDict, this.EnhanceObjTradeInfoDict);
	}
	#endregion // 함수

	#region 조건부 함수
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 객체 정보를 저장한다 */
	public void SaveObjInfos() {
		var oObjInfos = SimpleJSON.JSONNode.Parse(this.LoadObjInfosJSONStr(Access.ObjInfoTableLoadPath));
		this.SetupJSONNodes(oObjInfos, out SimpleJSON.JSONNode oCommonInfos, out SimpleJSON.JSONNode oBuyTradeInfos, out SimpleJSON.JSONNode oSaleTradeInfos, out SimpleJSON.JSONNode oEnhanceTradeInfos);

		for(int i = 0; i < oCommonInfos.Count; ++i) {
			var eObjKinds = oCommonInfos[i][KCDefine.U_KEY_OBJ_KINDS].ExIsValid() ? (EObjKinds)oCommonInfos[i][KCDefine.U_KEY_OBJ_KINDS].AsInt : EObjKinds.NONE;

			// 객체 정보가 존재 할 경우
			if(this.ObjInfoDict.TryGetValue(eObjKinds, out STObjInfo stObjInfo)) {
				stObjInfo.SaveObjInfo(oCommonInfos[i]);
			}
		}

		for(int i = 0; i < oBuyTradeInfos.Count; ++i) {
			var eObjKinds = oBuyTradeInfos[i][KCDefine.U_KEY_OBJ_KINDS].ExIsValid() ? (EObjKinds)oBuyTradeInfos[i][KCDefine.U_KEY_OBJ_KINDS].AsInt : EObjKinds.NONE;

			// 구입 객체 교환 정보가 존재 할 경우
			if(this.BuyObjTradeInfoDict.TryGetValue(eObjKinds, out STObjTradeInfo stBuyObjTradeInfo)) {
				stBuyObjTradeInfo.SaveObjTradeInfo(oBuyTradeInfos[i]);
			}
		}

		for(int i = 0; i < oSaleTradeInfos.Count; ++i) {
			var eObjKinds = oSaleTradeInfos[i][KCDefine.U_KEY_OBJ_KINDS].ExIsValid() ? (EObjKinds)oSaleTradeInfos[i][KCDefine.U_KEY_OBJ_KINDS].AsInt : EObjKinds.NONE;

			// 판매 객체 교환 정보가 존재 할 경우
			if(this.SaleObjTradeInfoDict.TryGetValue(eObjKinds, out STObjTradeInfo stSaleObjTradeInfo)) {
				stSaleObjTradeInfo.SaveObjTradeInfo(oSaleTradeInfos[i]);
			}
		}

		for(int i = 0; i < oEnhanceTradeInfos.Count; ++i) {
			var eObjKinds = oEnhanceTradeInfos[i][KCDefine.U_KEY_OBJ_KINDS].ExIsValid() ? (EObjKinds)oEnhanceTradeInfos[i][KCDefine.U_KEY_OBJ_KINDS].AsInt : EObjKinds.NONE;

			// 강화 객체 교환 정보가 존재 할 경우
			if(this.EnhanceObjTradeInfoDict.TryGetValue(eObjKinds, out STObjTradeInfo stEnhanceObjTradeInfo)) {
				stEnhanceObjTradeInfo.SaveObjTradeInfo(oEnhanceTradeInfos[i]);
			}
		}

		this.SaveObjInfos(oObjInfos.ToString());
	}

	/** 객체 정보 값을 생성한다 */
	public Dictionary<string, List<List<string>>> MakeObjInfoVals() {
		var oCommonKeyInfoList = CCollectionManager.Inst.SpawnList<STKeyInfo>();
		var oBuyTradeKeyInfoList = CCollectionManager.Inst.SpawnList<STKeyInfo>();
		var oSaleTradeKeyInfoList = CCollectionManager.Inst.SpawnList<STKeyInfo>();
		var oEnhanceTradeKeyInfoList = CCollectionManager.Inst.SpawnList<STKeyInfo>();

		var oObjInfoValDictContainer = new Dictionary<string, List<List<string>>>();

		try {
			this.SetupKeyInfos(oCommonKeyInfoList, oBuyTradeKeyInfoList, oSaleTradeKeyInfoList, oEnhanceTradeKeyInfoList);
			this.SetupJSONNodes(SimpleJSON.JSONNode.Parse(this.LoadObjInfosJSONStr(Access.ObjInfoTableSavePath)), out SimpleJSON.JSONNode oCommonInfos, out SimpleJSON.JSONNode oBuyTradeInfos, out SimpleJSON.JSONNode oSaleTradeInfos, out SimpleJSON.JSONNode oEnhanceTradeInfos);

			oObjInfoValDictContainer.TryAdd(Access.GetSheetNames(this.GetType(), Access.ObjTableInfo)[KCDefine.B_KEY_COMMON], oCommonInfos.AsArray.ExToInfoVals(oCommonKeyInfoList));
			oObjInfoValDictContainer.TryAdd(Access.GetSheetNames(this.GetType(), Access.ObjTableInfo)[KCDefine.B_KEY_BUY_TRADE], oCommonInfos.AsArray.ExToInfoVals(oBuyTradeKeyInfoList));
			oObjInfoValDictContainer.TryAdd(Access.GetSheetNames(this.GetType(), Access.ObjTableInfo)[KCDefine.B_KEY_SALE_TRADE], oCommonInfos.AsArray.ExToInfoVals(oSaleTradeKeyInfoList));
			oObjInfoValDictContainer.TryAdd(Access.GetSheetNames(this.GetType(), Access.ObjTableInfo)[KCDefine.B_KEY_ENHANCE_TRADE], oCommonInfos.AsArray.ExToInfoVals(oEnhanceTradeKeyInfoList));
		} finally {
			CCollectionManager.Inst.DespawnList(oCommonKeyInfoList);
			CCollectionManager.Inst.DespawnList(oBuyTradeKeyInfoList);
			CCollectionManager.Inst.DespawnList(oSaleTradeKeyInfoList);
			CCollectionManager.Inst.DespawnList(oEnhanceTradeKeyInfoList);
		}

		return oObjInfoValDictContainer;
	}

	/** 키 정보를 설정한다 */
	private void SetupKeyInfos(List<STKeyInfo> a_oOutCommonKeyInfoList, List<STKeyInfo> a_oOutBuyTradeKeyInfoList, List<STKeyInfo> a_oOutSaleTradeKeyInfoList, List<STKeyInfo> a_oOutEnhanceTradeKeyInfoList) {
		KDefine.G_KEY_INFO_GOOGLE_SHEET_COMMON_LIST.ExCopyTo(a_oOutCommonKeyInfoList, (a_stKeyInfo) => a_stKeyInfo);
		KDefine.G_KEY_INFO_GOOGLE_SHEET_COMMON_LIST.ExCopyTo(a_oOutBuyTradeKeyInfoList, (a_stKeyInfo) => a_stKeyInfo);
		KDefine.G_KEY_INFO_GOOGLE_SHEET_COMMON_LIST.ExCopyTo(a_oOutSaleTradeKeyInfoList, (a_stKeyInfo) => a_stKeyInfo);
		KDefine.G_KEY_INFO_GOOGLE_SHEET_COMMON_LIST.ExCopyTo(a_oOutEnhanceTradeKeyInfoList, (a_stKeyInfo) => a_stKeyInfo);

		Access.ObjTableInfo.m_oKeyInfoDictContainer[this.GetType()].GetValueOrDefault(KCDefine.B_KEY_COMMON)?.ExCopyTo(a_oOutCommonKeyInfoList, (a_stKeyInfo) => a_stKeyInfo, false, false);
		Access.ObjTableInfo.m_oKeyInfoDictContainer[this.GetType()].GetValueOrDefault(KCDefine.B_KEY_BUY_TRADE)?.ExCopyTo(a_oOutBuyTradeKeyInfoList, (a_stKeyInfo) => a_stKeyInfo, false, false);
		Access.ObjTableInfo.m_oKeyInfoDictContainer[this.GetType()].GetValueOrDefault(KCDefine.B_KEY_SALE_TRADE)?.ExCopyTo(a_oOutSaleTradeKeyInfoList, (a_stKeyInfo) => a_stKeyInfo, false, false);
		Access.ObjTableInfo.m_oKeyInfoDictContainer[this.GetType()].GetValueOrDefault(KCDefine.B_KEY_ENHANCE_TRADE)?.ExCopyTo(a_oOutEnhanceTradeKeyInfoList, (a_stKeyInfo) => a_stKeyInfo, false, false);
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	#endregion // 조건부 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

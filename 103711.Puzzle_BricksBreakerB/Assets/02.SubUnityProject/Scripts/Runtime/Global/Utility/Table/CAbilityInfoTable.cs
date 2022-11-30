using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.IO;

/** 어빌리티 정보 */
[System.Serializable]
public struct STAbilityInfo {
	public STCommonInfo m_stCommonInfo;
	public STValInfo m_stValInfo;

	public EAbilityKinds m_eAbilityKinds;
	public EAbilityKinds m_ePrevAbilityKinds;
	public EAbilityKinds m_eNextAbilityKinds;
	public EAbilityValType m_eAbilityValType;

	public Dictionary<ulong, STTargetInfo> m_oExtraAbilityTargetInfoDict;

	#region 상수
	public static STAbilityInfo INVALID = new STAbilityInfo() {
		m_eAbilityKinds = EAbilityKinds.NONE, m_ePrevAbilityKinds = EAbilityKinds.NONE, m_eNextAbilityKinds = EAbilityKinds.NONE
	};
	#endregion // 상수

	#region 프로퍼티
	public EAbilityType AbilityType => (EAbilityType)((int)m_eAbilityKinds).ExKindsToType();
	public EAbilityKinds BaseAbilityKinds => (EAbilityKinds)((int)m_eAbilityKinds).ExKindsToSubKindsType();
	#endregion // 프로퍼티

	#region 함수
	/** 생성자 */
	public STAbilityInfo(SimpleJSON.JSONNode a_oAbilityInfo) {
		m_stCommonInfo = new STCommonInfo(a_oAbilityInfo);
		m_stValInfo = new STValInfo(a_oAbilityInfo[KCDefine.U_KEY_VAL_INFO]);

		m_eAbilityKinds = a_oAbilityInfo[KCDefine.U_KEY_ABILITY_KINDS].ExIsValid() ? (EAbilityKinds)a_oAbilityInfo[KCDefine.U_KEY_ABILITY_KINDS].AsInt : EAbilityKinds.NONE;
		m_ePrevAbilityKinds = a_oAbilityInfo[KCDefine.U_KEY_PREV_ABILITY_KINDS].ExIsValid() ? (EAbilityKinds)a_oAbilityInfo[KCDefine.U_KEY_PREV_ABILITY_KINDS].AsInt : EAbilityKinds.NONE;
		m_eNextAbilityKinds = a_oAbilityInfo[KCDefine.U_KEY_NEXT_ABILITY_KINDS].ExIsValid() ? (EAbilityKinds)a_oAbilityInfo[KCDefine.U_KEY_NEXT_ABILITY_KINDS].AsInt : EAbilityKinds.NONE;
		m_eAbilityValType = a_oAbilityInfo[KCDefine.U_KEY_ABILITY_VAL_TYPE].ExIsValid() ? (EAbilityValType)a_oAbilityInfo[KCDefine.U_KEY_ABILITY_VAL_TYPE].AsInt : EAbilityValType.NONE;

		m_oExtraAbilityTargetInfoDict = Factory.MakeTargetInfos(a_oAbilityInfo, KCDefine.U_KEY_FMT_EXTRA_ABILITY_TARGET_INFO);
	}
	#endregion // 함수

	#region 조건부 함수
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 어빌리티 정보를 설정한다 */
	public void SetupAbilityInfo(SimpleJSON.JSONNode a_oOutAbilityInfo) {
		m_stCommonInfo.SaveCommonInfo(a_oOutAbilityInfo);
		m_stValInfo.SaveValInfo(a_oOutAbilityInfo[KCDefine.U_KEY_VAL_INFO]);

		a_oOutAbilityInfo[KCDefine.U_KEY_ABILITY_KINDS] = $"{(int)m_eAbilityKinds}";
		a_oOutAbilityInfo[KCDefine.U_KEY_PREV_ABILITY_KINDS] = $"{(int)m_ePrevAbilityKinds}";
		a_oOutAbilityInfo[KCDefine.U_KEY_NEXT_ABILITY_KINDS] = $"{(int)m_eNextAbilityKinds}";
		a_oOutAbilityInfo[KCDefine.U_KEY_ABILITY_VAL_TYPE] = $"{(int)m_eAbilityValType}";

		Func.SaveTargetInfos(m_oExtraAbilityTargetInfoDict, KCDefine.U_KEY_FMT_EXTRA_ABILITY_TARGET_INFO, a_oOutAbilityInfo);
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	#endregion // 조건부 함수
}

/** 어빌리티 교환 정보 */
[System.Serializable]
public struct STAbilityTradeInfo {
	public STCommonInfo m_stCommonInfo;

	public EAbilityKinds m_eAbilityKinds;
	public EAbilityKinds m_ePrevAbilityKinds;
	public EAbilityKinds m_eNextAbilityKinds;

	public Dictionary<ulong, STTargetInfo> m_oPayTargetInfoDict;
	public Dictionary<ulong, STTargetInfo> m_oAcquireTargetInfoDict;

	#region 상수
	public static STAbilityTradeInfo INVALID = new STAbilityTradeInfo() {
		m_eAbilityKinds = EAbilityKinds.NONE, m_ePrevAbilityKinds = EAbilityKinds.NONE, m_eNextAbilityKinds = EAbilityKinds.NONE
	};
	#endregion // 상수

	#region 프로퍼티
	public EAbilityType AbilityType => (EAbilityType)((int)m_eAbilityKinds).ExKindsToType();
	public EAbilityKinds BaseAbilityKinds => (EAbilityKinds)((int)m_eAbilityKinds).ExKindsToSubKindsType();
	#endregion // 프로퍼티

	#region 함수
	/** 생성자 */
	public STAbilityTradeInfo(SimpleJSON.JSONNode a_oAbilityInfo) {
		m_stCommonInfo = new STCommonInfo(a_oAbilityInfo);

		m_eAbilityKinds = a_oAbilityInfo[KCDefine.U_KEY_ABILITY_KINDS].ExIsValid() ? (EAbilityKinds)a_oAbilityInfo[KCDefine.U_KEY_ABILITY_KINDS].AsInt : EAbilityKinds.NONE;
		m_ePrevAbilityKinds = a_oAbilityInfo[KCDefine.U_KEY_PREV_ABILITY_KINDS].ExIsValid() ? (EAbilityKinds)a_oAbilityInfo[KCDefine.U_KEY_PREV_ABILITY_KINDS].AsInt : EAbilityKinds.NONE;
		m_eNextAbilityKinds = a_oAbilityInfo[KCDefine.U_KEY_NEXT_ABILITY_KINDS].ExIsValid() ? (EAbilityKinds)a_oAbilityInfo[KCDefine.U_KEY_NEXT_ABILITY_KINDS].AsInt : EAbilityKinds.NONE;

		m_oPayTargetInfoDict = Factory.MakeTargetInfos(a_oAbilityInfo, KCDefine.U_KEY_FMT_PAY_TARGET_INFO);
		m_oAcquireTargetInfoDict = Factory.MakeTargetInfos(a_oAbilityInfo, KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO);
	}
	#endregion // 함수

	#region 조건부 함수
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 어빌리티 교환 정보를 설정한다 */
	public void SetupAbilityTradeInfo(SimpleJSON.JSONNode a_oOutAbilityTradeInfo) {
		m_stCommonInfo.SaveCommonInfo(a_oOutAbilityTradeInfo);

		a_oOutAbilityTradeInfo[KCDefine.U_KEY_ABILITY_KINDS] = $"{(int)m_eAbilityKinds}";
		a_oOutAbilityTradeInfo[KCDefine.U_KEY_PREV_ABILITY_KINDS] = $"{(int)m_ePrevAbilityKinds}";
		a_oOutAbilityTradeInfo[KCDefine.U_KEY_NEXT_ABILITY_KINDS] = $"{(int)m_eNextAbilityKinds}";

		Func.SaveTargetInfos(m_oPayTargetInfoDict, KCDefine.U_KEY_FMT_PAY_TARGET_INFO, a_oOutAbilityTradeInfo);
		Func.SaveTargetInfos(m_oAcquireTargetInfoDict, KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, a_oOutAbilityTradeInfo);
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	#endregion // 조건부 함수
}

/** 어빌리티 정보 테이블 */
public partial class CAbilityInfoTable : CSingleton<CAbilityInfoTable> {
	#region 프로퍼티
	public Dictionary<EAbilityKinds, STAbilityInfo> AbilityInfoDict { get; } = new Dictionary<EAbilityKinds, STAbilityInfo>();
	public Dictionary<EAbilityKinds, STAbilityTradeInfo> EnhanceAbilityTradeInfoDict { get; } = new Dictionary<EAbilityKinds, STAbilityTradeInfo>();
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.ResetAbilityInfos();
	}

	/** 어빌리티 정보를 리셋한다 */
	public virtual void ResetAbilityInfos() {
		this.AbilityInfoDict.Clear();
		this.EnhanceAbilityTradeInfoDict.Clear();
	}

	/** 어빌리티 정보를 리셋한다 */
	public virtual void ResetAbilityInfos(string a_oJSONStr) {
		this.ResetAbilityInfos();
		this.DoLoadAbilityInfos(a_oJSONStr);
	}

	/** 어빌리티 정보를 반환한다 */
	public STAbilityInfo GetAbilityInfo(EAbilityKinds a_eAbilityKinds) {
		bool bIsValid = this.TryGetAbilityInfo(a_eAbilityKinds, out STAbilityInfo stAbilityInfo);
		CAccess.Assert(bIsValid);

		return stAbilityInfo;
	}

	/** 강화 어빌리티 교환 정보를 반환한다 */
	public STAbilityTradeInfo GetEnhanceAbilityTradeInfo(EAbilityKinds a_eAbilityKinds) {
		bool bIsValid = this.TryGetEnhanceAbilityTradeInfo(a_eAbilityKinds, out STAbilityTradeInfo stAbilityTradeInfo);
		CAccess.Assert(bIsValid);

		return stAbilityTradeInfo;
	}

	/** 어빌리티 정보를 반환한다 */
	public bool TryGetAbilityInfo(EAbilityKinds a_eAbilityKinds, out STAbilityInfo a_stOutAbilityInfo) {
		a_stOutAbilityInfo = this.AbilityInfoDict.GetValueOrDefault(a_eAbilityKinds, STAbilityInfo.INVALID);
		return this.AbilityInfoDict.ContainsKey(a_eAbilityKinds);
	}

	/** 강화 어빌리티 교환 정보를 반환한다 */
	public bool TryGetEnhanceAbilityTradeInfo(EAbilityKinds a_eAbilityKinds, out STAbilityTradeInfo a_stOuttAbilityTradeInfo) {
		a_stOuttAbilityTradeInfo = this.EnhanceAbilityTradeInfoDict.GetValueOrDefault(a_eAbilityKinds, STAbilityTradeInfo.INVALID);
		return this.EnhanceAbilityTradeInfoDict.ContainsKey(a_eAbilityKinds);
	}

	/** 어빌리티 정보를 로드한다 */
	public Dictionary<EAbilityKinds, STAbilityInfo> LoadAbilityInfos() {
		this.ResetAbilityInfos();
		return this.LoadAbilityInfos(Access.AbilityInfoTableLoadPath);
	}

	/** 어빌리티 정보를 저장한다 */
	public void SaveAbilityInfos(string a_oJSONStr, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oJSONStr != null);

		// JSON 문자열이 존재 할 경우
		if(a_oJSONStr != null) {
			this.ResetAbilityInfos(a_oJSONStr);

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
			CFunc.WriteStr(Access.AbilityInfoTableSavePath, a_oJSONStr, false);
#else
			CFunc.WriteStr(Access.AbilityInfoTableSavePath, a_oJSONStr, true);
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)

#if UNITY_ANDROID && (DEBUG || DEVELOPMENT)
			CUnityMsgSender.Inst.SendShowToastMsg($"CAbilityInfoTable.SaveAbilityInfos: {File.Exists(Access.AbilityInfoTableSavePath)}");
#endif // #if UNITY_ANDROID && (DEBUG || DEVELOPMENT)
		}
	}

	/** JSON 노드를 설정한다 */
	private void SetupJSONNodes(SimpleJSON.JSONNode a_oJSONNode, out SimpleJSON.JSONNode a_oOutCommonInfos, out SimpleJSON.JSONNode a_oOutEnhanceTradeInfos) {
		var oSheetNameDictContainer = Access.GetSheetNames(this.GetType(), Access.AbilityTableInfo);
		a_oOutCommonInfos = a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_COMMON]].ExIsValid() ? a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_COMMON]] : KCDefine.B_EMPTY_JSON_ARRAY;
		a_oOutEnhanceTradeInfos = a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_ENHANCE_TRADE]].ExIsValid() ? a_oJSONNode[oSheetNameDictContainer[KCDefine.B_KEY_ENHANCE_TRADE]] : KCDefine.B_EMPTY_JSON_ARRAY;
	}

	/** 어빌리티 정보를 로드한다 */
	private Dictionary<EAbilityKinds, STAbilityInfo> LoadAbilityInfos(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		return this.DoLoadAbilityInfos(this.LoadAbilityInfosJSONStr(a_oFilePath));
	}

	/** 어빌리티 정보 JSON 문자열을 로드한다 */
	private string LoadAbilityInfosJSONStr(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
		return File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, false) : CFunc.ReadStrFromRes(a_oFilePath, false);
#else
		return File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, true) : CFunc.ReadStrFromRes(a_oFilePath, false);
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 어빌리티 정보를 로드한다 */
	private Dictionary<EAbilityKinds, STAbilityInfo> DoLoadAbilityInfos(string a_oJSONStr) {
		CAccess.Assert(a_oJSONStr.ExIsValid());
		this.SetupJSONNodes(SimpleJSON.JSONNode.Parse(a_oJSONStr), out SimpleJSON.JSONNode oCommonInfos, out SimpleJSON.JSONNode oEnhanceTradeInfos);

		for(int i = 0; i < oCommonInfos.Count; ++i) {
			var stAbilityInfo = new STAbilityInfo(oCommonInfos[i]);

			// 어빌리티 정보 추가 가능 할 경우
			if(stAbilityInfo.m_eAbilityKinds.ExIsValid() && (!this.AbilityInfoDict.ContainsKey(stAbilityInfo.m_eAbilityKinds) || oCommonInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
				this.AbilityInfoDict.ExReplaceVal(stAbilityInfo.m_eAbilityKinds, stAbilityInfo);
			}
		}

		for(int i = 0; i < oEnhanceTradeInfos.Count; ++i) {
			var stAbilityTradeInfo = new STAbilityTradeInfo(oEnhanceTradeInfos[i]);

			// 강화 어빌리티 교환 정보 추가 가능 할 경우
			if(stAbilityTradeInfo.m_eAbilityKinds.ExIsValid() && (!this.EnhanceAbilityTradeInfoDict.ContainsKey(stAbilityTradeInfo.m_eAbilityKinds) || oEnhanceTradeInfos[i][KCDefine.U_KEY_REPLACE].AsInt != KCDefine.B_VAL_0_INT)) {
				this.EnhanceAbilityTradeInfoDict.ExReplaceVal(stAbilityTradeInfo.m_eAbilityKinds, stAbilityTradeInfo);
			}
		}

		return this.AbilityInfoDict;
	}
	#endregion // 함수

	#region 조건부 함수
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 어빌리티 정보를 저장한다 */
	public void SaveAbilityInfos() {
		var oAbilityInfos = SimpleJSON.JSONNode.Parse(this.LoadAbilityInfosJSONStr(Access.AbilityInfoTableLoadPath));
		this.SetupJSONNodes(oAbilityInfos, out SimpleJSON.JSONNode oCommonInfos, out SimpleJSON.JSONNode oEnhanceTradeInfos);

		for(int i = 0; i < oCommonInfos.Count; ++i) {
			var eAbilityKinds = oCommonInfos[i][KCDefine.U_KEY_ABILITY_KINDS].ExIsValid() ? (EAbilityKinds)oCommonInfos[i][KCDefine.U_KEY_ABILITY_KINDS].AsInt : EAbilityKinds.NONE;

			// 어빌리티 정보가 존재 할 경우
			if(this.AbilityInfoDict.ContainsKey(eAbilityKinds)) {
				this.AbilityInfoDict[eAbilityKinds].SetupAbilityInfo(oCommonInfos[i]);
			}
		}

		for(int i = 0; i < oEnhanceTradeInfos.Count; ++i) {
			var eAbilityKinds = oEnhanceTradeInfos[i][KCDefine.U_KEY_ABILITY_KINDS].ExIsValid() ? (EAbilityKinds)oEnhanceTradeInfos[i][KCDefine.U_KEY_ABILITY_KINDS].AsInt : EAbilityKinds.NONE;

			// 강화 어빌리티 교환 정보가 존재 할 경우
			if(this.EnhanceAbilityTradeInfoDict.ContainsKey(eAbilityKinds)) {
				this.EnhanceAbilityTradeInfoDict[eAbilityKinds].SetupAbilityTradeInfo(oEnhanceTradeInfos[i]);
			}
		}

		this.SaveAbilityInfos(oAbilityInfos.ToString());
	}

	/** 어빌리티 정보 값을 생성한다 */
	public Dictionary<string, List<List<string>>> MakeAbilityInfoVals() {
		var oCommonKeyInfoList = CCollectionManager.Inst.SpawnList<STKeyInfo>();
		var oEnhanceTradeKeyInfoList = CCollectionManager.Inst.SpawnList<STKeyInfo>();

		var oAbilityInfoValDictContainer = new Dictionary<string, List<List<string>>>();

		try {
			this.SetupKeyInfos(oCommonKeyInfoList, oEnhanceTradeKeyInfoList);
			this.SetupJSONNodes(SimpleJSON.JSONNode.Parse(this.LoadAbilityInfosJSONStr(Access.AbilityInfoTableSavePath)), out SimpleJSON.JSONNode oCommonInfos, out SimpleJSON.JSONNode oEnhanceTradeInfos);

			oAbilityInfoValDictContainer.TryAdd(Access.GetSheetNames(this.GetType(), Access.AbilityTableInfo)[KCDefine.B_KEY_COMMON], oCommonInfos.AsArray.ExToInfoVals(oCommonKeyInfoList));
			oAbilityInfoValDictContainer.TryAdd(Access.GetSheetNames(this.GetType(), Access.AbilityTableInfo)[KCDefine.B_KEY_ENHANCE_TRADE], oEnhanceTradeInfos.AsArray.ExToInfoVals(oEnhanceTradeKeyInfoList));
		} finally {
			CCollectionManager.Inst.DespawnList(oCommonKeyInfoList);
			CCollectionManager.Inst.DespawnList(oEnhanceTradeKeyInfoList);
		}

		return oAbilityInfoValDictContainer;
	}

	/** 키 정보를 설정한다 */
	private void SetupKeyInfos(List<STKeyInfo> a_oOutCommonKeyInfoList, List<STKeyInfo> a_oOutEnhanceTradeKeyInfoList) {
		KDefine.G_KEY_INFO_GOOGLE_SHEET_COMMON_LIST.ExCopyTo(a_oOutCommonKeyInfoList, (a_stKeyInfo) => a_stKeyInfo);
		KDefine.G_KEY_INFO_GOOGLE_SHEET_COMMON_LIST.ExCopyTo(a_oOutEnhanceTradeKeyInfoList, (a_stKeyInfo) => a_stKeyInfo);

		Access.AbilityTableInfo.m_oKeyInfoDictContainer[this.GetType()].GetValueOrDefault(KCDefine.B_KEY_COMMON)?.ExCopyTo(a_oOutCommonKeyInfoList, (a_stKeyInfo) => a_stKeyInfo, false, false);
		Access.AbilityTableInfo.m_oKeyInfoDictContainer[this.GetType()].GetValueOrDefault(KCDefine.B_KEY_ENHANCE_TRADE)?.ExCopyTo(a_oOutEnhanceTradeKeyInfoList, (a_stKeyInfo) => a_stKeyInfo, false, false);
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	#endregion // 조건부 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

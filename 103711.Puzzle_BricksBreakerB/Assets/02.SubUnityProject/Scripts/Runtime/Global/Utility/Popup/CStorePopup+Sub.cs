using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.Linq;
using TMPro;

#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif // #if PURCHASE_MODULE_ENABLE

/** 상점 팝업 */
public partial class CStorePopup : CSubPopup {
#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
			(KCDefine.U_OBJ_N_RESTORE_BTN, this.Contents, this.OnTouchRestoreBtn)
		}, false);

#region 추가
		this.SubSetupAwake();
#endregion // 추가
	}
	
	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init();
		this.Params = a_stParams;
		
		a_stParams.m_oProductTradeInfoList.Sort((a_stLhs, a_stRhs) => a_stLhs.m_nProductIdx.CompareTo(a_stRhs.m_nProductIdx));

#region 추가
		this.SubInit();
#endregion // 추가
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {
		// 상품 UI 상태를 갱신한다
		for(int i = 0; i < m_oProductBuyUIsList.Count; ++i) {
			this.UpdateProductBuyUIsState(m_oProductBuyUIsList[i], this.Params.m_oProductTradeInfoList[i]);
		}

#region 추가
		this.SubUpdateUIsState();
#endregion // 추가
	}

	/** 상품 구입 UI 상태를 갱신한다 */
	private void UpdateProductBuyUIsState(GameObject a_oProductBuyUIs, STProductTradeInfo a_stProductTradeInfo) {
		var oPriceUIsDict = CCollectionManager.Inst.SpawnDict<EPurchaseType, GameObject>();

		try {
			// 객체를 갱신한다 {
			CFunc.SetupObjs(new List<(EPurchaseType, string, GameObject)>() {
				(EPurchaseType.ADS, KCDefine.U_OBJ_N_ADS_PRICE_UIS, a_oProductBuyUIs),
				(EPurchaseType.IN_APP_PURCHASE, KCDefine.U_OBJ_N_PURCHASE_PRICE_UIS, a_oProductBuyUIs)
			}, oPriceUIsDict);

			foreach(var stKeyVal in oPriceUIsDict) {
				stKeyVal.Value?.SetActive(a_stProductTradeInfo.m_ePurchaseType == stKeyVal.Key);
			}
			// 객체를 갱신한다 }

			// 텍스트를 갱신한다 {
			var oPriceText = a_oProductBuyUIs.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_PRICE_TEXT);
			oPriceText?.ExSetText(string.Format(KCDefine.B_TEXT_FMT_USD_PRICE, a_stProductTradeInfo.m_oPayTargetInfoDict.FirstOrDefault().Value.m_stValInfo01.m_dmVal), EFontSet._1, false);

			var oAcquireTargetInfoKeyList = a_stProductTradeInfo.m_oAcquireTargetInfoDict.Keys.ToList();
			a_oProductBuyUIs.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_NAME_TEXT)?.ExSetText(a_stProductTradeInfo.m_stCommonInfo.m_oName, EFontSet._1, false);

			for(int i = 0; i < oAcquireTargetInfoKeyList.Count; ++i) {
				var nUniqueTargetInfoID = oAcquireTargetInfoKeyList[i];
				a_oProductBuyUIs.ExFindComponent<TMP_Text>(string.Format(KCDefine.U_OBJ_N_FMT_NUM_TEXT, i + KCDefine.B_VAL_1_INT))?.ExSetText($"{a_stProductTradeInfo.m_oAcquireTargetInfoDict.GetValueOrDefault(nUniqueTargetInfoID).m_stValInfo01.m_dmVal}", EFontSet._1, false);
			}

#if !UNITY_EDITOR && PURCHASE_MODULE_ENABLE
			// 인앱 결제 상품 일 경우
			if(a_stProductTradeInfo.m_ePurchaseType == EPurchaseType.IN_APP_PURCHASE && Access.GetProduct(a_stProductTradeInfo.m_nProductIdx) != null) {
				oPriceText?.ExSetText(Access.GetPriceStr(a_stProductTradeInfo.m_nProductIdx), EFontSet._1, false);
			}
#endif // #if !UNITY_EDITOR && PURCHASE_MODULE_ENABLE
			// 텍스트를 갱신한다 }

			// 버튼을 갱신한다 {
			var oPurchaseBtn = oPriceUIsDict.GetValueOrDefault(EPurchaseType.IN_APP_PURCHASE)?.ExFindComponentInParent<Button>(KCDefine.U_OBJ_N_PURCHASE_BTN);
			oPurchaseBtn?.ExAddListener(() => this.OnTouchPurchaseBtn(a_stProductTradeInfo));

#if ADS_MODULE_ENABLE
			// 보상 광고 상품 일 경우
			if(a_stProductTradeInfo.m_ePurchaseType == EPurchaseType.ADS) {
				var oTouchInteractable = oPurchaseBtn?.gameObject.ExAddComponent<CRewardAdsTouchInteractable>();
				oTouchInteractable?.SetAdsPlatform(CPluginInfoTable.Inst.AdsPlatform);
			}
#endif // #if ADS_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
			var stProductInfo = CProductInfoTable.Inst.GetProductInfo(a_stProductTradeInfo.m_nProductIdx);

			// 비소모 상품 일 경우
			if(stProductInfo.m_eProductType == ProductType.NonConsumable) {
				oPurchaseBtn?.ExSetInteractable(!CPurchaseManager.Inst.IsPurchaseNonConsumableProduct(stProductInfo.m_oID));
			}
#endif // #if PURCHASE_MODULE_ENABLE
			// 버튼을 갱신한다 }

			// 패키지 상품 일 경우
			if(a_stProductTradeInfo.ProductType == EProductType.PKGS) {
				this.UpdatePkgsProductBuyUIsState(a_oProductBuyUIs, a_stProductTradeInfo);
			} else {
				this.UpdateSingleProductBuyUIsState(a_oProductBuyUIs, a_stProductTradeInfo);
			}
		} finally {
			CCollectionManager.Inst.DespawnDict(oPriceUIsDict);
		}
	}
#endregion // 함수
}

/** 서브 상점 팝업 */
public partial class CStorePopup : CSubPopup {
	/** 서브 식별자 */
	private enum ESubKey {
		NONE = -1,
		[HideInInspector] MAX_VAL
	}

#region 변수

#endregion // 변수

#region 프로퍼티

#endregion // 프로퍼티

#region 함수
	/** 팝업을 설정한다 */
	private void SubSetupAwake() {
		// Do Something
	}

	/** 초기화한다 */
	private void SubInit() {
		// Do Something
	}

	/** UI 상태를 갱신한다 */
	private void SubUpdateUIsState() {
		// Do Something
	}
	
	/** 패키지 상품 구입 UI 상태를 갱신한다 */
	private void UpdatePkgsProductBuyUIsState(GameObject a_oProductBuyUIs, STProductTradeInfo a_stProductTradeInfo) {
		// Do Something
	}

	/** 단일 상품 구입 UI 상태를 갱신한다 */
	private void UpdateSingleProductBuyUIsState(GameObject a_oProductBuyUIs, STProductTradeInfo a_stProductTradeInfo) {
		// Do Something
	}
#endregion // 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

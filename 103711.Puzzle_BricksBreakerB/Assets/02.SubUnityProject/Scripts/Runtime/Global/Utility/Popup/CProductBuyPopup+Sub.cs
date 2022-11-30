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

/** 상품 구입 팝업 */
public partial class CProductBuyPopup : CSubPopup {
#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

#region 추가
		this.SubSetupAwake();
#endregion // 추가
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init();
		this.Params = a_stParams;

#region 추가
		this.SubInit();
#endregion // 추가
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {
		// 상품 구입 UI 상태를 갱신한다
		for(int i = 0; i < m_oProductBuyUIsList.Count; ++i) {
			this.UpdateProductBuyUIsState(m_oProductBuyUIsList[i], CProductTradeInfoTable.Inst.GetBuyProductTradeTradeInfo(KDefine.G_PRODUCT_KINDS_SPECIAL_PKGS_LIST[i]));
		}

#region 추가
		this.SubUpdateUIsState();
#endregion // 추가
	}

	/** 상품 구입 UI 상태를 갱신한다 */
	private void UpdateProductBuyUIsState(GameObject a_oSpecialPkgsUIs, STProductTradeInfo a_stProductTradeInfo) {
		// 텍스트를 설정한다 {
		var oPriceText = a_oSpecialPkgsUIs.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_PRICE_TEXT);
		oPriceText?.ExSetText(string.Format(KCDefine.B_TEXT_FMT_USD_PRICE, a_stProductTradeInfo.m_oPayTargetInfoDict.FirstOrDefault().Value.m_stValInfo01.m_dmVal), EFontSet._1, false);

		a_oSpecialPkgsUIs.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_NAME_TEXT)?.ExSetText(CStrTable.Inst.GetStr(a_stProductTradeInfo.m_stCommonInfo.m_oName), EFontSet._1, false);
		a_oSpecialPkgsUIs.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_PRICE_TEXT)?.ExSetText(string.Format(KCDefine.B_TEXT_FMT_USD_PRICE, a_stProductTradeInfo.m_oPayTargetInfoDict.FirstOrDefault().Value.m_stValInfo01.m_dmVal), EFontSet._1, false);

#if !UNITY_EDITOR && PURCHASE_MODULE_ENABLE
		// 상품이 존재 할 경우
		if(Access.GetProduct(a_stProductTradeInfo.m_nProductIdx) != null) {
			oPriceText?.ExSetText(Access.GetPriceStr(a_stProductTradeInfo.m_nProductIdx), EFontSet._1, false);
		}
#endif // #if !UNITY_EDITOR && PURCHASE_MODULE_ENABLE
		// 텍스트를 설정한다 }

		// 버튼을 설정한다 {
		var oPurchaseBtn = a_oSpecialPkgsUIs?.ExFindComponent<Button>(KCDefine.U_OBJ_N_PURCHASE_BTN);
		oPurchaseBtn?.ExAddListener(() => this.OnTouchPurchaseBtn(a_stProductTradeInfo));

#if PURCHASE_MODULE_ENABLE
		var stProductInfo = CProductInfoTable.Inst.GetProductInfo(a_stProductTradeInfo.m_nProductIdx);

		// 비소모 상품 일 경우
		if(stProductInfo.m_eProductType == ProductType.NonConsumable) {
			oPurchaseBtn?.ExSetInteractable(!CPurchaseManager.Inst.IsPurchaseNonConsumableProduct(stProductInfo.m_oID));
		}
#endif // #if PURCHASE_MODULE_ENABLE
		// 버튼을 설정한다 }
	}
#endregion // 함수
}

/** 서브 상품 구입 팝업 */
public partial class CProductBuyPopup : CSubPopup {
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
#endregion // 함수

#region 조건부 함수
#if PURCHASE_MODULE_ENABLE
	/** 상품이 결제 되었을 경우 */
	private void OnPurchaseProduct(CPurchaseManager a_oSender, string a_oProductID, bool a_bIsSuccess) {
		// 결제 되었을 경우
		if(a_bIsSuccess) {
			// Do Something
		}

		this.UpdateUIsState();
		this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.PURCHASE)?.Invoke(a_oSender, a_oProductID, a_bIsSuccess);
	}
#endif // #if PURCHASE_MODULE_ENABLE
#endregion // 조건부 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

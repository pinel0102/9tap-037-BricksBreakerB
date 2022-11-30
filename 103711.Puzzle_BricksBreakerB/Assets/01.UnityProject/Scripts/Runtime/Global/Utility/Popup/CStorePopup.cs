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
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		SEL_PRODUCT_KINDS,
		[HideInInspector] MAX_VAL
	}

	/** 콜백 */
	public enum ECallback {
		NONE = -1,
		ADS,
		PURCHASE,
		RESTORE,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public List<STProductTradeInfo> m_oProductTradeInfoList;

#if ADS_MODULE_ENABLE
		public Dictionary<ECallback, System.Action<CAdsManager, STAdsRewardInfo, bool>> m_oAdsCallbackDict;
#endif // #if ADS_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
		public Dictionary<ECallback, System.Action<CPurchaseManager, string, bool>> m_oPurchaseCallbackDict01;
		public Dictionary<ECallback, System.Action<CPurchaseManager, List<Product>, bool>> m_oPurchaseCallbackDict02;
#endif // #if PURCHASE_MODULE_ENABLE
	}

#region 변수
	private Dictionary<EKey, EProductKinds> m_oProductKindsDict = new Dictionary<EKey, EProductKinds>();

#if PURCHASE_MODULE_ENABLE
	private List<Product> m_oRestoreProductList = new List<Product>();
#endif // #if PURCHASE_MODULE_ENABLE

	/** =====> 객체 <===== */
	[SerializeField] private List<GameObject> m_oProductBuyUIsList = new List<GameObject>();
#endregion // 변수

#region 프로퍼티
	public STParams Params { get; private set; }
#endregion // 프로퍼티

#region 함수
	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
	}

	/** 결제 버튼을 눌렀을 경우 */
	private void OnTouchPurchaseBtn(STProductTradeInfo a_stProductTradeInfo) {
		switch(a_stProductTradeInfo.m_ePurchaseType) {
			case EPurchaseType.ADS: {
#if ADS_MODULE_ENABLE
				m_oProductKindsDict.ExReplaceVal(EKey.SEL_PRODUCT_KINDS, a_stProductTradeInfo.m_eProductKinds);
				Func.ShowRewardAds(this.OnCloseRewardAds);
#endif // #if ADS_MODULE_ENABLE

				break;
			}
			case EPurchaseType.IN_APP_PURCHASE: {
#if PURCHASE_MODULE_ENABLE
				CSceneManager.GetSceneManager<OverlayScene.CSubOverlaySceneManager>(KCDefine.B_SCENE_N_OVERLAY)?.PurchaseProduct(a_stProductTradeInfo.m_eProductKinds, this.OnPurchaseProduct);
#endif // #if PURCHASE_MODULE_ENABLE

				break;
			}
			case EPurchaseType.TARGET: {
				Func.Trade(CGameInfoStorage.Inst.PlayCharacterID, a_stProductTradeInfo);
				break;
			}
		}
	}

	/** 복원 버튼을 눌렀을 경우 */
	private void OnTouchRestoreBtn() {
#if PURCHASE_MODULE_ENABLE
		m_oRestoreProductList.Clear();
		Func.RestoreProducts(this.OnRestoreProducts);
#endif // #if PURCHASE_MODULE_ENABLE
	}
#endregion // 함수

#region 클래스 함수
	/** 매개 변수를 생성한다 */
	public static STParams MakeParams(List<STProductTradeInfo> a_oProductTradeInfoList) {
		return new STParams() {
			m_oProductTradeInfoList = a_oProductTradeInfoList,

#if ADS_MODULE_ENABLE
			m_oAdsCallbackDict = new Dictionary<ECallback, System.Action<CAdsManager, STAdsRewardInfo, bool>>(),
#endif // #if ADS_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
			m_oPurchaseCallbackDict01 = new Dictionary<ECallback, System.Action<CPurchaseManager, string, bool>>(),
			m_oPurchaseCallbackDict02 = new Dictionary<ECallback, System.Action<CPurchaseManager, List<Product>, bool>>()
#endif // #if PURCHASE_MODULE_ENABLE
		};
	}
#endregion // 클래스 함수

#region 조건부 함수
#if ADS_MODULE_ENABLE
	/** 보상 광고가 닫혔을 경우 */
	private void OnCloseRewardAds(CAdsManager a_oSender, STAdsRewardInfo a_stAdsRewardInfo, bool a_bIsSuccess) {
		// 광고를 시청했을 경우
		if(a_bIsSuccess) {
			var eSelProductKinds = m_oProductKindsDict.GetValueOrDefault(EKey.SEL_PRODUCT_KINDS, EProductKinds.NONE);
			Func.Trade(CGameInfoStorage.Inst.PlayCharacterID, CProductTradeInfoTable.Inst.GetBuyProductTradeTradeInfo(eSelProductKinds));
		}

		this.UpdateUIsState();
		this.Params.m_oAdsCallbackDict?.GetValueOrDefault(ECallback.ADS)?.Invoke(a_oSender, a_stAdsRewardInfo, a_bIsSuccess);
	}
#endif // #if ADS_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
	/** 상품이 결제 되었을 경우 */
	private void OnPurchaseProduct(CPurchaseManager a_oSender, string a_oProductID, bool a_bIsSuccess) {
		// 결제 되었을 경우
		if(a_bIsSuccess) {
			// Do Something
		}

		this.UpdateUIsState();
		this.Params.m_oPurchaseCallbackDict01?.GetValueOrDefault(ECallback.PURCHASE)?.Invoke(a_oSender, a_oProductID, a_bIsSuccess);
	}

	/** 상품이 복원 되었을 경우 */
	public void OnRestoreProducts(CPurchaseManager a_oSender, List<Product> a_oProductList, bool a_bIsSuccess) {
		// 복원 되었을 경우
		if(a_bIsSuccess) {
			m_oRestoreProductList = a_oProductList;
			Func.AcquireRestoreProducts(a_oProductList);
		}

#if FIREBASE_MODULE_ENABLE
		this.ExLateCallFunc((a_oCallFuncSender) => Func.LoadTargetInfos(this.OnLoadTargetInfos));
#else
		Func.OnRestoreProducts(a_oSender, a_oProductList, a_bIsSuccess, null);
#endif // #if FIREBASE_MODULE_ENABLE

		this.UpdateUIsState();
		this.Params.m_oPurchaseCallbackDict02?.GetValueOrDefault(ECallback.RESTORE)?.Invoke(a_oSender, a_oProductList, a_bIsSuccess);
	}

#if FIREBASE_MODULE_ENABLE
	/** 타겟 정보를 로드했을 경우 */
	private void OnLoadTargetInfos(CFirebaseManager a_oSender, string a_oJSONStr, bool a_bIsSuccess) {
		// 로드 되었을 경우
		if(a_bIsSuccess && a_oJSONStr.ExIsValid()) {
			var oTargetInfoDict = a_oJSONStr.ExJSONStrToTargetInfos();
			Func.Acquire(CGameInfoStorage.Inst.PlayCharacterID, oTargetInfoDict, true);

			this.ExLateCallFunc((a_oCallFuncSender) => { oTargetInfoDict.Clear(); Func.SaveTargetInfos(oTargetInfoDict, this.OnSaveTargetInfos); });
		} else {
			Func.OnRestoreProducts(CPurchaseManager.Inst, m_oRestoreProductList, m_oRestoreProductList.ExIsValid(), null);
		}

		this.UpdateUIsState();
	}

	/** 타겟 정보를 저장했을 경우 */
	private void OnSaveTargetInfos(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		Func.OnRestoreProducts(CPurchaseManager.Inst, m_oRestoreProductList, m_oRestoreProductList.ExIsValid(), null);
	}
#endif // #if FIREBASE_MODULE_ENABLE
#endif // #if PURCHASE_MODULE_ENABLE
#endregion // 조건부 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

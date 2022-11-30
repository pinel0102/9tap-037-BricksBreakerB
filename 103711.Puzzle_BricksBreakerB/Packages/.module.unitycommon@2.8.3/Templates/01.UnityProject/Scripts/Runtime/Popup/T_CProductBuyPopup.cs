#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 상품 구입 팝업 */
public partial class CProductBuyPopup : CSubPopup {
	/** 콜백 */
	public enum ECallback {
		NONE = -1,

#if PURCHASE_MODULE_ENABLE
		PURCHASE,
#endif // #if PURCHASE_MODULE_ENABLE

		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public EProductKinds m_eProductKinds;

#if PURCHASE_MODULE_ENABLE
		public Dictionary<ECallback, System.Action<CPurchaseManager, string, bool>> m_oCallbackDict;
#endif // #if PURCHASE_MODULE_ENABLE
	}

#region 변수
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
#if PURCHASE_MODULE_ENABLE
		CSceneManager.GetSceneManager<OverlayScene.CSubOverlaySceneManager>(KCDefine.B_SCENE_N_OVERLAY)?.PurchaseProduct(a_stProductTradeInfo.m_eProductKinds, this.OnPurchaseProduct);
#endif // #if PURCHASE_MODULE_ENABLE
	}
#endregion // 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

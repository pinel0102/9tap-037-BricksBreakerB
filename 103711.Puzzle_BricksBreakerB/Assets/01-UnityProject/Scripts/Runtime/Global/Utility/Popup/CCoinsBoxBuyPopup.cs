using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using TMPro;

/** 코인 상자 구입 팝업 */
public partial class CCoinsBoxBuyPopup : CSubPopup {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		NUM_COINS_TEXT,
		[HideInInspector] MAX_VAL
	}

	#region 변수
	/** =====> UI <===== */
	private Dictionary<EKey, TMP_Text> m_oTextDict = new Dictionary<EKey, TMP_Text>();

	/** =====> 객체 <===== */
	[SerializeField] private GameObject m_oSaveUIs = null;
	[SerializeField] private GameObject m_oFullUIs = null;
	#endregion // 변수

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 텍스트를 설정한다
		CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
			(EKey.NUM_COINS_TEXT, $"{EKey.NUM_COINS_TEXT}", this.Contents)
		}, m_oTextDict);

		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
			(KCDefine.U_OBJ_N_OK_BTN, this.Contents, this.OnTouchOKBtn),
			(KCDefine.U_OBJ_N_PURCHASE_BTN, this.Contents, this.OnTouchPurchaseBtn)
		});

		this.SubAwake();
	}

	/** 초기화 */
	public override void Init() {
		base.Init();
		this.SubInit();
	}

	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
	}

	/** UI 상태를 변경한다 */
	private void UpdateUIsState() {
		long nNumCoinsBoxCoins = (long)Access.GetItemTargetVal(CGameInfoStorage.Inst.PlayCharacterID, EItemKinds.GOODS_BOX_COINS, ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_NUMS);

		// 객체를 갱신한다
		m_oSaveUIs?.SetActive(nNumCoinsBoxCoins < KDefine.G_MAX_NUM_COINS_BOX_COINS);
		m_oFullUIs?.SetActive(nNumCoinsBoxCoins >= KDefine.G_MAX_NUM_COINS_BOX_COINS);

		// 텍스트를 갱신한다
		m_oTextDict.GetValueOrDefault(EKey.NUM_COINS_TEXT)?.ExSetText($"{nNumCoinsBoxCoins}", EFontSet._1, false);

		this.SubUpdateUIsState();
	}

	/** 확인 버튼을 눌렀을 경우 */
	private void OnTouchOKBtn() {
		this.OnTouchCloseBtn();
	}

	/** 결제 버튼을 눌렀을 경우 */
	private void OnTouchPurchaseBtn() {
#if PURCHASE_MODULE_ENABLE
		//Func.PurchaseProduct(EProductKinds.SINGLE_COINS_BOX, this.OnPurchaseProduct);
#endif // #if PURCHASE_MODULE_ENABLE
	}
	#endregion // 함수

	#region 조건부 함수
#if PURCHASE_MODULE_ENABLE
	/** 상품이 결제 되었을 경우 */
	private void OnPurchaseProduct(CPurchaseManager a_oSender, string a_oProductID, bool a_bIsSuccess) {
		// 결제 되었을 경우
		if(a_bIsSuccess) {
			Func.AcquireProduct(a_oProductID);
			Func.OnPurchaseProduct(a_oSender, a_oProductID, a_bIsSuccess, null);
		}

		this.UpdateUIsState();
	}
#endif // #if PURCHASE_MODULE_ENABLE
	#endregion // 조건부 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using TMPro;

/** 코인 상자 구입 팝업 */
public partial class CCoinsBoxBuyPopup : CSubPopup {
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
		}, false);

#region 추가
		this.SubSetupAwake();
#endregion // 추가
	}

	/** 초기화 */
	public override void Init() {
		base.Init();

#region 추가
		this.SubInit();
#endregion // 추가
	}

	/** UI 상태를 변경한다 */
	private void UpdateUIsState() {
		long nNumCoinsBoxCoins = (long)Access.GetItemTargetVal(CGameInfoStorage.Inst.PlayCharacterID, EItemKinds.GOODS_COINS_BOX_COINS, ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_NUMS);

		// 객체를 갱신한다
		m_oSaveUIs?.SetActive(nNumCoinsBoxCoins < KDefine.G_MAX_NUM_COINS_BOX_COINS);
		m_oFullUIs?.SetActive(nNumCoinsBoxCoins >= KDefine.G_MAX_NUM_COINS_BOX_COINS);

		// 텍스트를 갱신한다
		m_oTextDict.GetValueOrDefault(EKey.NUM_COINS_TEXT)?.ExSetText($"{nNumCoinsBoxCoins}", EFontSet._1, false);

#region 추가
		this.SubUpdateUIsState();
#endregion // 추가
	}
#endregion // 함수
}

/** 서브 코인 상자 구입 팝업 */
public partial class CCoinsBoxBuyPopup : CSubPopup {
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

	/** 초기화 */
	private void SubInit() {
		// Do Something
	}

	/** UI 상태를 갱신한다 */
	private void SubUpdateUIsState() {
		// Do Something
	}
#endregion // 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

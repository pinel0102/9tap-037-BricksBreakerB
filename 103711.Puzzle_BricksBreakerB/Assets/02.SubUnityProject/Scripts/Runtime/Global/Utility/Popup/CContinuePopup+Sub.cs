using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using TMPro;

/** 이어하기 팝업 */
public partial class CContinuePopup : CSubPopup {
#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 텍스트를 설정한다
		CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
			(EKey.PRICE_TEXT, $"{EKey.PRICE_TEXT}", this.Contents)
		}, m_oTextDict);

		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
			(KCDefine.U_OBJ_N_RETRY_BTN, this.Contents, this.OnTouchRetryBtn),
			(KCDefine.U_OBJ_N_CONTINUE_BTN, this.Contents, this.OnTouchContinueBtn),
			(KCDefine.U_OBJ_N_LEAVE_BTN, this.Contents, this.OnTouchLeaveBtn)
		}, false);

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
		var stItemTradeInfo = CItemInfoTable.Inst.GetBuyItemTradeInfo(EItemKinds.CONSUMABLE_GAME_ITEM_CONTINUE);

		// 텍스트를 갱신한다 {
		var oTextKeyInfoList = new List<(EKey, ETargetKinds, EItemKinds)>() {
			(EKey.PRICE_TEXT, ETargetKinds.ITEM_NUMS, EItemKinds.GOODS_NORM_COINS)
		};

		for(int i = 0; i < oTextKeyInfoList.Count; ++i) {
			m_oTextDict.GetValueOrDefault(oTextKeyInfoList[i].Item1)?.ExSetText($"{stItemTradeInfo.m_oPayTargetInfoDict.ExGetTargetVal(oTextKeyInfoList[i].Item2, (int)oTextKeyInfoList[i].Item3)}", EFontSet._1, false);
		}
		// 텍스트를 갱신한다 }

#region 추가
		this.SubUpdateUIsState();
#endregion // 추가
	}
#endregion // 함수
}

/** 서브 이어하기 팝업 */
public partial class CContinuePopup : CSubPopup {
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
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

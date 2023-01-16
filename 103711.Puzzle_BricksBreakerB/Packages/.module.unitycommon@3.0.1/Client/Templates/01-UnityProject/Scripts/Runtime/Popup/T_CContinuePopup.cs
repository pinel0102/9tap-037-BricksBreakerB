#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using TMPro;

/** 이어하기 팝업 */
public partial class CContinuePopup : CSubPopup {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		PRICE_TEXT,
		[HideInInspector] MAX_VAL
	}

	/** 콜백 */
	public enum ECallback {
		NONE = -1,
		RETRY,
		CONTINUE,
		LEAVE,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public int m_nContinueTimes;
		public Dictionary<ECallback, System.Action<CContinuePopup>> m_oCallbackDict;
	}

#region 변수
	/** =====> UI <===== */
	private Dictionary<EKey, TMP_Text> m_oTextDict = new Dictionary<EKey, TMP_Text>();
#endregion // 변수

#region 프로퍼티
	public STParams Params { get; private set; }
	public override bool IsIgnoreCloseBtn => true;
#endregion // 프로퍼티

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
		});

		this.SubAwake();
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init();
		this.Params = a_stParams;

		this.SubInit();
	}

	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
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

		this.SubUpdateUIsState();
	}

	/** 닫기 버튼을 눌렀을 경우 */
	protected override void OnTouchCloseBtn() {
		base.OnTouchCloseBtn();
		this.OnTouchLeaveBtn();
	}

	/** 재시도 버튼을 눌렀을 경우 */
	private void OnTouchRetryBtn() {
		this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.RETRY)?.Invoke(this);
	}

	/** 이어하기 버튼을 눌렀을 경우 */
	private void OnTouchContinueBtn() {
		var stItemTradeInfo = CItemInfoTable.Inst.GetBuyItemTradeInfo(EItemKinds.CONSUMABLE_GAME_ITEM_CONTINUE);
		stItemTradeInfo.m_oPayTargetInfoDict.ExTryGetTargetInfo(ETargetKinds.ITEM_NUMS, (int)EItemKinds.GOODS_NORM_COINS, out STTargetInfo stTargetInfo);

		// 교환이 불가능 할 경우
		if(Access.IsEnableTrade(CGameInfoStorage.Inst.PlayCharacterID, stTargetInfo)) {
			CSceneManager.GetSceneManager<OverlayScene.CSubOverlaySceneManager>(KCDefine.B_SCENE_N_OVERLAY)?.ShowStorePopup();
		} else {
			Func.Acquire(CGameInfoStorage.Inst.PlayCharacterID, stItemTradeInfo.m_oAcquireTargetInfoDict, true);
			this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.CONTINUE)?.Invoke(this);
		}
	}

	/** 나가기 버튼을 눌렀을 경우 */
	private void OnTouchLeaveBtn() {
		this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.LEAVE)?.Invoke(this);
	}
#endregion // 함수

#region 클래스 함수
	/** 매개 변수를 생성한다 */
	public static STParams MakeParams(int a_nContinueTimes, Dictionary<ECallback, System.Action<CContinuePopup>> a_oCallbackDict = null) {
		return new STParams() {
			m_nContinueTimes = a_nContinueTimes, m_oCallbackDict = a_oCallbackDict ?? new Dictionary<ECallback, System.Action<CContinuePopup>>()
		};
	}
#endregion // 클래스 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

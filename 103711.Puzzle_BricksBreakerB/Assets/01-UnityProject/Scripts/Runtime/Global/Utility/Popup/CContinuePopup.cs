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
        public NSEngine.CEngine Engine;
	}

	#region 변수
	/** =====> UI <===== */
	private Dictionary<EKey, TMP_Text> m_oTextDict = new Dictionary<EKey, TMP_Text>();
	#endregion // 변수

	#region 프로퍼티
	public STParams Params { get; private set; }
	public override bool IsIgnoreCloseBtn => false;

    [Header("★ [Reference] Shop Button")]
    public Button shopButton;
    public TMP_Text rubyText;

    [Header("★ [Reference] Continue Button")]
    public GameObject ContinueObject;
    public GameObject ContinueObject_AD;
    public List<TMP_Text> costText_continue;
    public List<Button> ContinueButton = new List<Button>();
    public Button ContinueButton_AD;

    [Header("★ [Reference] Reward Button")]
    public Button rewardVideoButton;
    public TMP_Text rewardText_ruby;
    
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
        this.SetIgnoreNavStackEvent(true);

		// 텍스트를 설정한다
		CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
			(EKey.PRICE_TEXT, $"{EKey.PRICE_TEXT}", this.Contents)
		}, m_oTextDict);

		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
			(KCDefine.U_OBJ_N_RETRY_BTN, this.Contents, this.OnTouchRetryBtn),
			(KCDefine.U_OBJ_N_LEAVE_BTN, this.Contents, this.OnTouchLeaveBtn)
		});

        for(int i=0; i < ContinueButton.Count; i++)
        {
            ContinueButton[i].ExAddListener(this.OnTouchContinueBtn);
        }

        ContinueButton_AD.ExAddListener(this.OnTouchContinueBtn_AD);

        shopButton.ExAddListener(OnTouchShopButton);
        rewardVideoButton.ExAddListener(OnTouchRewardVideoButton);

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

        GlobalDefine.RefreshShopText(rubyText);
        for(int i=0; i < costText_continue.Count; i++)
        {
            costText_continue[i].text = string.Format(GlobalDefine.FORMAT_INT, GlobalDefine.CostRuby_Continue_Remove3Lines);
        }
        rewardText_ruby.text = string.Format(GlobalDefine.FORMAT_INT, GlobalDefine.RewardRuby_Continue);
		// 텍스트를 갱신한다 }

        ContinueObject.SetActive(!GlobalDefine.IsEnableRewardVideo());
        ContinueObject_AD.SetActive(GlobalDefine.IsEnableRewardVideo());
        rewardVideoButton.gameObject.SetActive(GlobalDefine.IsEnableRewardVideo());

        GlobalDefine.PlaySoundFX(ESoundSet.SOUND_LEVEL_FAIL);

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

	/** 나가기 버튼을 눌렀을 경우 */
	private void OnTouchLeaveBtn() {
		this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.LEAVE)?.Invoke(this);
	}

    private void OnTouchShopButton()
    {
        CSceneManager.GetSceneManager<OverlayScene.CSubOverlaySceneManager>(KCDefine.B_SCENE_N_OVERLAY)?.ShowStorePopup();
    }

    /** 이어하기 버튼을 눌렀을 경우 */
	private void OnTouchContinueBtn() 
    {   
        if (!GlobalDefine.isLevelEditor) 
        {
            if (GlobalDefine.UserInfo.Ruby < GlobalDefine.CostRuby_Continue_Remove3Lines)
            {
                CSceneManager.GetSceneManager<OverlayScene.CSubOverlaySceneManager>(KCDefine.B_SCENE_N_OVERLAY)?.ShowStorePopup();
                return;
            }
            else
                GlobalDefine.AddRuby(-GlobalDefine.CostRuby_Continue_Remove3Lines);
        }

        Params.Engine.GetReward(NSEngine.RewardVideoType.CONTINUE_3LINE, this, false);
	}

    private void OnTouchContinueBtn_AD() 
    {   
        Params.Engine.RequestVideo(NSEngine.RewardVideoType.CONTINUE_3LINE, this);
	}

    private void OnTouchRewardVideoButton()
    {
        Params.Engine.RequestVideo(NSEngine.RewardVideoType.CONTINUE_RUBY, this);
    }
	#endregion // 함수

	#region 클래스 함수
	/** 매개 변수를 생성한다 */
	public static STParams MakeParams(int a_nContinueTimes, Dictionary<ECallback, System.Action<CContinuePopup>> a_oCallbackDict = null, NSEngine.CEngine _engine = null) {
		return new STParams() {
			m_nContinueTimes = a_nContinueTimes, m_oCallbackDict = a_oCallbackDict ?? new Dictionary<ECallback, System.Action<CContinuePopup>>(),
            Engine = _engine
		};
	}
	#endregion // 클래스 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

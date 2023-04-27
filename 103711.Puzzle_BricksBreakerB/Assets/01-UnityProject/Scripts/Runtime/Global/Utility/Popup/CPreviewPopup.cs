using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using TMPro;

/** 정지 팝업 */
public partial class CPreviewPopup : CSubPopup {
	/** 식별자 */
	public enum EKey {
		NONE = -1,
        RETRY_BTN,
		LEAVE_BTN,
		[HideInInspector] MAX_VAL
	}

	/** 콜백 */
	public enum ECallback {
		NONE = -1,
        RETRY,
		LEAVE,
        RESUME,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public Dictionary<ECallback, System.Action<CPreviewPopup>> m_oCallbackDict;
        public NSEngine.CEngine Engine;
	}

	#region 변수

    [Header("★ [Reference] Play Button")]
    public GameObject playObject;
    public GameObject playObject_AD;
    public List<Button> buttonPlay = new List<Button>();
    public Button buttonPlayAD;

    [Header("★ [Reference] Booster")]
    public List<Button> buttonBooster = new List<Button>();
    public List<Button> boosterLock = new List<Button>();
    public List<GameObject> boosterOn = new List<GameObject>();
    public List<GameObject> boosterCount = new List<GameObject>();
    public List<GameObject> boosterBuy = new List<GameObject>();
    public List<TMP_Text> boosterTextCount = new List<TMP_Text>();
    public List<TMP_Text> boosterTextCost = new List<TMP_Text>();

    [Header("★ [Reference] Golden Aim")]
    public Button goldenAimButton;
    public GameObject goldenAimOK;

    [Header("★ [Reference] Preview")]
    public TMP_Text levelText;
    public SpriteMask previewMask;
    public RectTransform previewArea;

    [Header("★ [Reference] Tooltip")]
    public int currentTooltip;
    public RectTransform tooltip;
    public TMP_Text tooltipText;
    public Canvas tooltipCanvas;
    public Canvas tipsCanvas;
    
	#endregion // 변수

	#region 프로퍼티
	public STParams Params { get; private set; }

    #endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

        this.SetIgnoreNavStackEvent(true);

		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
            ($"{EKey.RETRY_BTN}", this.gameObject, this.OnTouchRetryBtn),
			($"{EKey.LEAVE_BTN}", this.gameObject, this.OnTouchLeaveBtn)
		});

        currentTooltip = -1;

        for(int i=0; i < buttonPlay.Count; i++)
        {
            buttonPlay[i].ExAddListener(this.OnTouchPlayButton);
        }

        buttonPlayAD.ExAddListener(this.OnTouchPlayButtonAD);

        for(int i=0; i < buttonBooster.Count; i++)
        {
            int index = i;
            buttonBooster[index].ExAddListener(() => this.OnTouchBoosterButton(index));
            boosterLock[index].ExAddListener(() => this.OnTouchTooltipButton(index));
        }

        goldenAimButton.ExAddListener(this.OnTouchGoldenAimButton);

        tooltipCanvas.ExSetSortingOrder(GlobalDefine.SortingInfo_PreviewTooltips);
        tipsCanvas.ExSetSortingOrder(GlobalDefine.SortingInfo_PreviewTips);

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

        Params.Engine.SetupPreview(previewArea, previewMask);

        levelText.text = string.Format(GlobalDefine.FORMAT_LEVEL, Params.Engine.currentLevel);

        for(int i=0; i< buttonBooster.Count; i++)
        {
            boosterOn[i].SetActive(false);
            buttonBooster[i].gameObject.SetActive(Params.Engine.currentLevel >= GlobalDefine.BOOSTER_LEVEL[i]);
            boosterLock[i].gameObject.SetActive(Params.Engine.currentLevel < GlobalDefine.BOOSTER_LEVEL[i]);
            boosterTextCost[i].text = string.Format(GlobalDefine.FORMAT_INT, GlobalDefine.CostRuby_Booster);
        }

        boosterTextCount[0].text = string.Format(GlobalDefine.FORMAT_ITEM_COUNT, GlobalDefine.UserInfo.Booster_Missile);
        boosterTextCount[1].text = string.Format(GlobalDefine.FORMAT_ITEM_COUNT, GlobalDefine.UserInfo.Booster_Lightning);
        boosterTextCount[2].text = string.Format(GlobalDefine.FORMAT_ITEM_COUNT, GlobalDefine.UserInfo.Booster_Bomb);

        boosterCount[0].SetActive(GlobalDefine.UserInfo.Booster_Missile > 0);
        boosterCount[1].SetActive(GlobalDefine.UserInfo.Booster_Lightning > 0);
        boosterCount[2].SetActive(GlobalDefine.UserInfo.Booster_Bomb > 0);

        boosterBuy[0].SetActive(GlobalDefine.UserInfo.Booster_Missile <= 0);
        boosterBuy[1].SetActive(GlobalDefine.UserInfo.Booster_Lightning <= 0);
        boosterBuy[2].SetActive(GlobalDefine.UserInfo.Booster_Bomb <= 0);

        tooltip.gameObject.SetActive(false);
        goldenAimButton.gameObject.SetActive(!GlobalDefine.UserInfo.Item_GoldenAim);
        goldenAimOK.SetActive(false);
        
        GlobalDefine.PlaySoundFX(ESoundSet.SOUND_LEVEL_READY);

		this.SubUpdateUIsState();
	}

    private void OnTouchPlayButton()
    {
        this.OnTouchResumeBtn();
    }

    private void OnTouchPlayButtonAD()
    {
        this.OnTouchResumeBtn();
    }

    private void OnTouchBoosterButton(int index)
    {
        bool oldValue = this.Params.Engine.boosterList[index];
        bool newValue = !oldValue;

        if (newValue)
        {
            if (!GlobalDefine.isLevelEditor) 
            {
                int currentCount = 0;
                switch(index)
                {
                    case 0 : currentCount = GlobalDefine.UserInfo.Booster_Missile; break;
                    case 1 : currentCount = GlobalDefine.UserInfo.Booster_Lightning; break;
                    case 2 : currentCount = GlobalDefine.UserInfo.Booster_Bomb; break;
                }

                if (currentCount < 1)
                {
                    if (GlobalDefine.UserInfo.Ruby < GlobalDefine.CostRuby_Booster)
                    {
                        GlobalDefine.OpenShop();
                        return;
                    }
                    else
                        GlobalDefine.AddRuby(-GlobalDefine.CostRuby_Booster);
                }
                else
                {
                    switch(index)
                    {
                        case 0 : GlobalDefine.UserInfo.Booster_Missile = Mathf.Max(0, GlobalDefine.UserInfo.Booster_Missile - 1); break;
                        case 1 : GlobalDefine.UserInfo.Booster_Lightning = Mathf.Max(0, GlobalDefine.UserInfo.Booster_Lightning - 1); break;
                        case 2 : GlobalDefine.UserInfo.Booster_Bomb = Mathf.Max(0, GlobalDefine.UserInfo.Booster_Bomb - 1); break;
                    }
                }
            }
        }
        else
        {
            switch(index)
            {
                case 0 : GlobalDefine.UserInfo.Booster_Missile = Mathf.Max(0, GlobalDefine.UserInfo.Booster_Missile + 1); break;
                case 1 : GlobalDefine.UserInfo.Booster_Lightning = Mathf.Max(0, GlobalDefine.UserInfo.Booster_Lightning + 1); break;
                case 2 : GlobalDefine.UserInfo.Booster_Bomb = Mathf.Max(0, GlobalDefine.UserInfo.Booster_Bomb + 1); break;
            }
        }

        this.Params.Engine.ChangeBooster(index, newValue);
        boosterOn[index].SetActive(newValue);
    }

    private void OnTouchTooltipButton(int index)
    {
        if (currentTooltip == index)
        {
            currentTooltip = -1;
            tooltip.gameObject.SetActive(false);
        }
        else
        {
            currentTooltip = index;
            tooltipText.text = string.Format(GlobalDefine.FORMAT_TOOLTIP_UNLOCK, GlobalDefine.BOOSTER_LEVEL[index]);
            tooltip.anchoredPosition = new Vector2(buttonBooster[index].transform.localPosition.x, 200);
            tooltip.gameObject.SetActive(true);
        }
    }

    private void OnTouchGoldenAimButton()
    {
        if (!GlobalDefine.isLevelEditor && !GlobalDefine.UserInfo.Item_GoldenAim) 
        {
            if (GlobalDefine.UserInfo.Ruby < GlobalDefine.CostRuby_GoldenAim)
            {
                GlobalDefine.OpenShop();
                return;
            }
            else
                GlobalDefine.AddRuby(-GlobalDefine.CostRuby_GoldenAim);
        }

        Params.Engine.isGoldAimOneTime = true;
        Params.Engine.isGoldAim = true;
        Params.Engine.SetAimLayer(true);
        Params.Engine.subGameSceneManager.RefreshGoldenAimButton();
        
        goldenAimButton.gameObject.SetActive(false);
        goldenAimOK.SetActive(true);
    }

    /** 재시도 버튼을 눌렀을 경우 */
	public void OnTouchRetryBtn() {
		this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.RETRY)?.Invoke(this);
	}

    private void OnTouchLeaveBtn() {
		this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.LEAVE)?.Invoke(this);
	}

	/** Play 버튼을 눌렀을 경우 */
	private void OnTouchResumeBtn() {
        this.Params.Engine.SetBooster();
        this.Params.Engine.subGameSceneManager.CheckTutorial();
		this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.RESUME)?.Invoke(this);
	}
	#endregion // 함수

	#region 클래스 함수
	/** 매개 변수를 생성한다 */
	public static STParams MakeParams(Dictionary<ECallback, System.Action<CPreviewPopup>> a_oCallbackDict = null, NSEngine.CEngine _engine = null) {
		return new STParams() {
			m_oCallbackDict = a_oCallbackDict ?? new Dictionary<ECallback, System.Action<CPreviewPopup>>(),
            Engine = _engine
		};
	}
	#endregion // 클래스 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

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

    [Header("★ [Live] Reward Video")]
    public RewardVideoType rewardVideoType = RewardVideoType.NONE;
    public bool isLoadRewardAds;

    [Header("★ [Live] Booster")]
    public int boosterEnableCount;
    public List<bool> rubyBooster = new List<bool>();
    public List<int> rewardBoosterIndex = new List<int>();
    public int rewardBoosterIndex_balloon = -1;
    public int rewardBoosterIndex_ready = -1;

    [Header("★ [Reference] Play Button")]
    public GameObject playObject;
    public GameObject playObject_AD;
    public List<Button> buttonPlay = new List<Button>();
    public Button buttonPlayAD;

    [Header("★ [Reference] Booster")]
    public GameObject leaveButton;
    public List<Button> buttonBooster = new List<Button>();
    public List<Button> boosterLock = new List<Button>();
    public List<GameObject> boosterOn = new List<GameObject>();
    public List<GameObject> boosterOnAds = new List<GameObject>();
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
        rewardBoosterIndex_balloon = -1;
        rewardBoosterIndex_ready = -1;

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

        rubyBooster = new List<bool>() { false, false, false };
        rewardBoosterIndex.Clear();

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
        
        boosterEnableCount = GetEnableBoosterCount();
        levelText.text = string.Format(GlobalDefine.FORMAT_LEVEL, Params.Engine.currentLevel);

        for(int i=0; i< buttonBooster.Count; i++)
        {
            boosterOn[i].SetActive(false);
            boosterOnAds[i].SetActive(false);
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
        goldenAimButton.gameObject.SetActive(!GlobalDefine.hasGoldenAim);
        goldenAimOK.SetActive(false);

        isLoadRewardAds = GlobalDefine.IsEnableRewardVideo();
        bool enableRewardBooster = boosterEnableCount > 0 && isLoadRewardAds;
        playObject.SetActive(!enableRewardBooster);
        playObject_AD.SetActive(enableRewardBooster);

        CheckRewardBooster();
        RefreshBoosterState();
        
        GlobalDefine.PlaySoundFX(ESoundSet.SOUND_LEVEL_READY);

		this.SubUpdateUIsState();
	}


#region Booster

    public void GetRewardBooster()
    {
        playObject.SetActive(true);
        playObject_AD.SetActive(false);

        CGameInfoStorage.Inst.GetRewardBooster(RewardVideoType.READY_BOOSTER);

        CheckRewardBooster();
    }

    private void CheckRewardBooster()
    {
        int enableCount = boosterEnableCount;

        SetRewardBooster(ref enableCount, ref rewardBoosterIndex_balloon, CGameInfoStorage.Inst.rewardBooster_balloon);
        SetRewardBooster(ref enableCount, ref rewardBoosterIndex_ready, CGameInfoStorage.Inst.rewardBooster_ready);

        //RefreshBoosterState();
    }

    public void RefreshBoosterState()
    {
        for(int i=0; i < buttonBooster.Count; i++)
        {
            bool isBoosterOn = this.Params.Engine.boosterList[i];
            bool isRewardOn = rewardBoosterIndex.Contains(i);

            if(isRewardOn)
            {
                boosterCount[i].SetActive(false);
                boosterBuy[i].SetActive(false);
            }

            boosterOn[i].SetActive(isBoosterOn && !isRewardOn);
            boosterOnAds[i].SetActive(isBoosterOn && isRewardOn);            
            buttonBooster[i].interactable = !(isBoosterOn && isRewardOn);
        }
    }

    private void SetRewardBooster(ref int _enableCount, ref int _index, bool _isBoosterOn)
    {
        if (_enableCount > 0 && _isBoosterOn)
        {
            if (_index == -1)
            {
                // index가 설정되어 있지 않았으면 새로 부여한다.
                List<int> enableList = new List<int>();
                for(int i=0; i < boosterEnableCount; i++)
                {
                    if(!rewardBoosterIndex.Contains(i))
                        enableList.Add(i);
                }
                
                if (enableList.Count > 0)
                    _index = enableList.OrderBy(g => System.Guid.NewGuid())
                                        .Take(1).ToList()[0];
            }

            if (_index > -1)
            {
                Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>enableCount : {0} / index : {1}</color>", _enableCount, _index));

                // Reward로 받은 것이 아닌 부스터가 켜져 있으면 해제한다.
                if (this.Params.Engine.boosterList[_index] && !this.Params.Engine.boosterReward[_index])
                    OnTouchBoosterButton(_index);
                
                rewardBoosterIndex.Add(_index);
                this.Params.Engine.ChangeBooster(_index, true, false, false, _isBoosterOn);
                _enableCount--;
            }
        }
    }

    private int GetEnableBoosterCount()
    {
        int enableCount = 0;
        for(int i=0; i < GlobalDefine.BOOSTER_LEVEL.Count; i++)
        {
            if(Params.Engine.currentLevel >= GlobalDefine.BOOSTER_LEVEL[i])
                enableCount++;
        }
        return enableCount;
    }

    private void OnTouchBoosterButton(int index)
    {
        bool oldValue = this.Params.Engine.boosterList[index];
        bool newValue = !oldValue;
        bool useItem = false;
        bool useRuby = false;

        if (!GlobalDefine.isLevelEditor) 
        {
            if (newValue)
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
                    if (GlobalDefine.UserInfo.Ruby - (GlobalDefine.CostRuby_Booster * GetRubyBoosterCount()) < GlobalDefine.CostRuby_Booster)
                    {
                        Debug.Log(string.Format("{0} < {1}", GlobalDefine.UserInfo.Ruby - (GlobalDefine.CostRuby_Booster * GetRubyBoosterCount()), GlobalDefine.CostRuby_Booster));
                        GlobalDefine.OpenShop();
                        return;
                    }
                    else
                    {
                        Debug.Log(string.Format("{0} >= {1}", GlobalDefine.UserInfo.Ruby - (GlobalDefine.CostRuby_Booster * GetRubyBoosterCount()), GlobalDefine.CostRuby_Booster));
                        rubyBooster[index] = true;
                        useRuby = true;
                    }
                }
                else
                {
                    useItem = true;
                }
            }
            else
            {
                if (rubyBooster[index])
                {
                    rubyBooster[index] = false;
                    useRuby = false;
                }
                else
                {
                    useItem = false;
                }
            }
        }

        this.Params.Engine.ChangeBooster(index, newValue, useItem, useRuby, false);
        boosterOn[index].SetActive(newValue);
    }

    private int GetRubyBoosterCount()
    {
        int count = 0;
        for(int i=0; i < rubyBooster.Count; i++)
        {
            if (rubyBooster[i])
                count++;
        }
        return count;
    }

#endregion Booster


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
        if (!GlobalDefine.isLevelEditor && !GlobalDefine.hasGoldenAim) 
        {
            if (GlobalDefine.UserInfo.Ruby < GlobalDefine.CostRuby_GoldenAim)
            {
                GlobalDefine.OpenShop();
                return;
            }
            else
                GlobalDefine.AddRuby(-GlobalDefine.CostRuby_GoldenAim);
        }

        Params.Engine.isGoldenAimOneTime = true;
        Params.Engine.isGoldenAim = true;
        Params.Engine.SetAimLayer(true);
        Params.Engine.subGameSceneManager.RefreshGoldenAimButton();
        
        leaveButton.SetActive(false);
        goldenAimButton.gameObject.SetActive(false);
        goldenAimOK.SetActive(true);
    }

    private void OnTouchPlayButton()
    {
        this.OnTouchResumeBtn();
    }

    private void OnTouchPlayButtonAD()
    {
        this.OnTouchRewardBoosterButton();
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

#region Reward Video

    private void OnTouchRewardBoosterButton() 
    {
        rewardVideoType = RewardVideoType.READY_BOOSTER;

        Func.ShowRewardVideoAlertPopup(this.transform.parent.gameObject, (a_oSender) => {
                var oTargetInfoDict = CCollectionManager.Inst.SpawnDict<ulong, STTargetInfo>();

                try {
                    switch(rewardVideoType)
                    {
                        case RewardVideoType.READY_BOOSTER:
                            (a_oSender as CRewardVideoAlertPopup).Init(CRewardVideoAlertPopup.MakeParams(KDefine.L_SCENE_N_PLAY, Params.Engine.currentLevel, rewardVideoType, ERewardKinds.ADS_REWARD_READY_BOOSTER, EItemKinds.BOOSTER_ITEM_04_RANDOM, KCDefine.B_VAL_0_INT, 
                            GlobalDefine.RewardVideoDesc_RandomBooster, 
                            () => { RefreshBoosterState(); }, this));
                            break;
                    }				
                } finally {
                    CCollectionManager.Inst.DespawnDict(oTargetInfoDict);
                }
            }, null, null);
        
        /*if (GlobalDefine.isLevelEditor)
        {
            this.OnCloseRewardAds(null, STAdsRewardInfo.INVALID, true);
        }
        else
        {
#if ADS_MODULE_ENABLE && !UNITY_EDITOR && !UNITY_STANDALONE
		    Func.ShowRewardAds(this.OnCloseRewardAds);
#else
            Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>ADS TEST : {0}</color>", rewardVideoType));
            this.OnCloseRewardAds(null, STAdsRewardInfo.INVALID, true);
#endif // #if ADS_MODULE_ENABLE
        }*/
	}

#if ADS_MODULE_ENABLE
    /** 보상 광고가 닫혔을 경우 */
	/*private void OnCloseRewardAds(CAdsManager a_oSender, STAdsRewardInfo a_stAdsRewardInfo, bool a_bIsSuccess) {
		// 광고를 시청했을 경우
		if(a_bIsSuccess) {
			if (rewardVideoType != RewardVideoType.NONE)
            {
                Params.Engine.GetReward(rewardVideoType, this, true);
                ShowRewardAcquirePopup(true);
            }
		}
	}*/

    /** 보상 획득 팝업을 출력한다 */
	/*public void ShowRewardAcquirePopup(bool a_bIsWatchRewardAds) {
		Func.ShowRewardAcquirePopup(this.transform.parent.gameObject, (a_oSender) => {
			var oTargetInfoDict = CCollectionManager.Inst.SpawnDict<ulong, STTargetInfo>();

			try {
                switch(rewardVideoType)
                {
                    case RewardVideoType.READY_BOOSTER:
                        (a_oSender as CRewardAcquirePopup).Init(CRewardAcquirePopup.MakeParams(KDefine.L_SCENE_N_PLAY, Params.Engine.currentLevel, ERewardKinds.ADS_REWARD_READY_BOOSTER, EItemKinds.BOOSTER_ITEM_01_MISSILE + rewardBoosterIndex_ready, KCDefine.B_VAL_0_INT, false, 
                        () => { RefreshBoosterState(); }, this));
                    break;
                }				
			} finally {
				CCollectionManager.Inst.DespawnDict(oTargetInfoDict);
			}
		}, null, null);
	}*/

#endif // #if ADS_MODULE_ENABLE

#endregion Reward Video

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

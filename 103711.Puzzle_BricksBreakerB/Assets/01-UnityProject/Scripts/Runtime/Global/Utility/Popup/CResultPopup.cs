using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using TMPro;
using DG.Tweening;

/** 결과 팝업 */
public partial class CResultPopup : CSubPopup {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		RECORD_TEXT,
		BEST_RECORD_TEXT,
		CLEAR_UIS,
		CLEAR_FAIL_UIS,
		[HideInInspector] MAX_VAL
	}

	/** 콜백 */
	public enum ECallback {
		NONE = -1,
		NEXT,
		RETRY,
		LEAVE,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public STRecordInfo m_stRecordInfo;
		public Dictionary<ECallback, System.Action<CResultPopup>> m_oCallbackDict;
        public NSEngine.CEngine Engine;
	}

	#region 변수
	/** =====> UI <===== */
	private Dictionary<EKey, TMP_Text> m_oTextDict = new Dictionary<EKey, TMP_Text>();

	/** =====> 객체 <===== */
	private Dictionary<EKey, GameObject> m_oUIsDict = new Dictionary<EKey, GameObject>();
	#endregion // 변수

	#region 프로퍼티
	public STParams Params { get; private set; }
	public override bool IsIgnoreCloseBtn => true;

    [Header("★ [Reference] Top")]
    public GameObject failBack;
    public TMP_Text rubyText;
    public TMP_Text[] levelText;
    public GameObject[] starObject;

    [Header("★ [Reference] Star Reward")]
    public Image gageImage;    
    public List<RectTransform> starRewardPoint = new List<RectTransform>();
    public List<GameObject> starRewardCheck = new List<GameObject>();
    public List<Image> starRewardIcon = new List<Image>();
    public List<TMP_Text> starRewardText = new List<TMP_Text>();
    public List<RewardIcons> rewardIcons = new List<RewardIcons>();
    public int currentPhase;
    public int currentRewardStar;

    [Header("★ [Reference] Preview")]
    public SpriteMask previewMask;
    public RectTransform previewArea;

    [Header("★ [Reference] Button")]
    public Button shopButton;
    public Button ADBlockButton;
    public Button piggyBankButton;
    
    private const string formatLevel = "Level {0}";
    private const string U_OBJ_N_LEAVE_BTN_2 = "LEAVE_BTN_2";
    private const float gageDelay = 1f;
    private const float gageDelayHalf = 0.5f;
    private WaitForSecondsRealtime wDelay = new WaitForSecondsRealtime(gageDelay);
    private WaitForSecondsRealtime wDelayHalf = new WaitForSecondsRealtime(gageDelayHalf);
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.SetIgnoreNavStackEvent(true);

		// 객체를 설정한다
		CFunc.SetupObjs(new List<(EKey, string, GameObject)>() {
			(EKey.CLEAR_UIS, $"{EKey.CLEAR_UIS}", this.Contents),
			(EKey.CLEAR_FAIL_UIS, $"{EKey.CLEAR_FAIL_UIS}", this.Contents)
		}, m_oUIsDict);

		// 텍스트를 설정한다
		CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
			(EKey.RECORD_TEXT, $"{EKey.RECORD_TEXT}", this.Contents),
			(EKey.BEST_RECORD_TEXT, $"{EKey.BEST_RECORD_TEXT}", this.Contents)
		}, m_oTextDict);

		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
			(KCDefine.U_OBJ_N_NEXT_BTN, this.Contents, this.OnTouchNextBtn),
			(KCDefine.U_OBJ_N_RETRY_BTN, this.Contents, this.OnTouchRetryBtn),
            (KCDefine.U_OBJ_N_LEAVE_BTN, this.Contents, this.OnTouchLeaveBtn),
            (U_OBJ_N_LEAVE_BTN_2, this.Contents, this.OnTouchLeaveBtn)
		});

        shopButton.ExAddListener(OnTouchShopButton);
        ADBlockButton.ExAddListener(OnTouchADBlockButton);

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

	/** 닫기 버튼을 눌렀을 경우 */
	protected override void OnTouchCloseBtn() {
        base.OnTouchCloseBtn();
		this.OnTouchLeaveBtn();
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {

        Params.Engine.SetupPreview(previewArea, previewMask);

        GlobalDefine.RefreshShopText(rubyText);
        
        ADBlockButton.gameObject.SetActive(!CUserInfoStorage.Inst.IsPurchaseRemoveAds);
        levelText[0].text = levelText[1].text = string.Format(formatLevel, Params.Engine.currentLevel);
        
		var oClearLevelInfo = Access.GetLevelClearInfo(CGameInfoStorage.Inst.PlayCharacterID, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID01, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03, false);

		// 객체를 갱신한다
		m_oUIsDict.GetValueOrDefault(EKey.CLEAR_UIS)?.SetActive(this.Params.m_stRecordInfo.m_bIsSuccess);
		m_oUIsDict.GetValueOrDefault(EKey.CLEAR_FAIL_UIS)?.SetActive(!this.Params.m_stRecordInfo.m_bIsSuccess);
        failBack.SetActive(!this.Params.m_stRecordInfo.m_bIsSuccess);

		// 텍스트를 갱신한다
		m_oTextDict.GetValueOrDefault(EKey.RECORD_TEXT)?.ExSetText($"{this.Params.m_stRecordInfo.m_nIntRecord}", EFontSet._1, false);
		m_oTextDict.GetValueOrDefault(EKey.BEST_RECORD_TEXT)?.ExSetText((oClearLevelInfo != null) ? $"{oClearLevelInfo.m_stBestRecordInfo.m_nIntRecord}" : string.Empty, EFontSet._1, false);

        for (int i=0; i < starObject.Length; i++)
        {
            starObject[i].SetActive(this.Params.m_stRecordInfo.m_bIsSuccess && this.Params.m_stRecordInfo.m_starCount > i);
        }

        RefreshStarReward();

        if (this.Params.m_stRecordInfo.m_bIsSuccess)
        {
            GlobalDefine.PlaySoundFX(ESoundSet.SOUND_LEVEL_CLEAR);
            GlobalDefine.ResultCalculate(Params.Engine);
            StarCalcurate();
        }
        else
        {
            GlobalDefine.PlaySoundFX(ESoundSet.SOUND_LEVEL_FAIL);
        }
        
		this.SubUpdateUIsState();
	}

    private void RefreshStarReward()
    {
        currentPhase = (GlobalDefine.UserInfo.Star / GlobalDefine.starRewardPoint[2]) % GlobalDefine.starReward.Count;
        currentRewardStar = GlobalDefine.UserInfo.Star % GlobalDefine.starRewardPoint[2];

        Debug.Log(CodeManager.GetMethodName() + string.Format("Phase : {0} / RewardStar : {1}", currentPhase, currentRewardStar));

        gageImage.fillAmount = ((float)currentRewardStar / (float)GlobalDefine.starRewardPoint[2]);

        RefreshIcons(currentPhase, currentRewardStar);
    }

    private void RefreshIcons(int phase, int rewardStar)
    {
        RectTransform gage = gageImage.transform as RectTransform;

        for(int i=0; i < starRewardPoint.Count; i++)
        {
            starRewardPoint[i].anchoredPosition = new Vector2(gage.sizeDelta.x * ((float)GlobalDefine.starRewardPoint[i] / (float)GlobalDefine.starRewardPoint[2]), starRewardPoint[i].anchoredPosition.y);
            starRewardIcon[i].sprite = rewardIcons.Find(item => item.kinds == GlobalDefine.starReward[phase][i].Key).sprite;
            starRewardText[i].text = GlobalDefine.starReward[phase][i].Value.ToString();
            starRewardCheck[i].SetActive(rewardStar >= GlobalDefine.starRewardPoint[i]);
        }
    }

    private void StarCalcurate()
    {
        StartCoroutine(CO_FillGage());
    }

    private IEnumerator CO_FillGage()
    {
        if (GlobalDefine.starIncrease > 0)
        {
            int newPhase = (GlobalDefine.UserInfo.Star / GlobalDefine.starRewardPoint[2]) % GlobalDefine.starReward.Count;
            if (newPhase == currentPhase)
            {
                int newStar = GlobalDefine.UserInfo.Star % GlobalDefine.starRewardPoint[2];
                float endFillAmount = ((float)newStar / (float)GlobalDefine.starRewardPoint[2]);

                Debug.Log(CodeManager.GetMethodName() + string.Format("Phase : {0} / RewardStar : {1}", currentPhase, newStar));

                for(int i=0; i < GlobalDefine.starRewardPoint.Count; i++)
                {
                    if (currentRewardStar < GlobalDefine.starRewardPoint[i] && newStar >= GlobalDefine.starRewardPoint[i])
                    {
                        GlobalDefine.AddItem(GlobalDefine.starReward[currentPhase][i].Key, GlobalDefine.starReward[currentPhase][i].Value);
                        GlobalDefine.PlaySoundFX(ESoundSet.SOUND_GET_STAR);
                    }
                }
                
                //Debug.Log(CodeManager.GetMethodName() + string.Format("Gage : {0} -> {1}", gageImage.fillAmount, endFillAmount));
                gageImage.DOFillAmount(endFillAmount, gageDelay).SetUpdate(true);
                yield return wDelay;

                for(int i=0; i < starRewardCheck.Count; i++)
                {
                    starRewardCheck[i].SetActive(newStar >= GlobalDefine.starRewardPoint[i]);
                }

                GlobalDefine.RefreshShopText(rubyText);
            }
            else
            {
                int newStar = GlobalDefine.UserInfo.Star % GlobalDefine.starRewardPoint[2];
                float endFillAmount = ((float)newStar / (float)GlobalDefine.starRewardPoint[2]);

                // get 3rd reward
                GlobalDefine.AddItem(GlobalDefine.starReward[currentPhase][2].Key, GlobalDefine.starReward[currentPhase][2].Value);
                GlobalDefine.PlaySoundFX(ESoundSet.SOUND_GET_STAR);

                Debug.Log(CodeManager.GetMethodName() + string.Format("Change Phase : {0} -> {1}", currentPhase, newPhase));
                Debug.Log(CodeManager.GetMethodName() + string.Format("Phase : {0} / RewardStar : {1}", newPhase, newStar));

                for(int i=0; i < GlobalDefine.starRewardPoint.Count; i++)
                {
                    if (newStar >= GlobalDefine.starRewardPoint[i])
                    {
                        GlobalDefine.AddItem(GlobalDefine.starReward[newPhase][i].Key, GlobalDefine.starReward[newPhase][i].Value);
                        GlobalDefine.PlaySoundFX(ESoundSet.SOUND_GET_STAR);
                    }
                }

                //Debug.Log(CodeManager.GetMethodName() + string.Format("Gage : {0} -> {1}", gageImage.fillAmount, 1f));
                gageImage.DOFillAmount(1f, gageDelayHalf).SetUpdate(true);
                yield return wDelayHalf;

                starRewardCheck[2].SetActive(true);
                
                yield return wDelayHalf;

                gageImage.fillAmount = 0;

                RefreshIcons(newPhase, newStar);

                //Debug.Log(CodeManager.GetMethodName() + string.Format("Gage : {0} -> {1}", gageImage.fillAmount, endFillAmount));
                gageImage.DOFillAmount(endFillAmount, gageDelayHalf).SetUpdate(true);
                yield return wDelayHalf;

                for(int i=0; i < starRewardCheck.Count; i++)
                {
                    starRewardCheck[i].SetActive(newStar >= GlobalDefine.starRewardPoint[i]);
                }

                GlobalDefine.RefreshShopText(rubyText);
            }
        }
    }

	/** 다음 버튼을 눌렀을 경우 */
	public void OnTouchNextBtn() {
        this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.NEXT)?.Invoke(this);
        /*if (GlobalDefine.isLevelEditor)
        {
            this.OnAdsFinished(ECallback.NEXT);
        }
        else
        {
#if ADS_MODULE_ENABLE && !UNITY_EDITOR && !UNITY_STANDALONE
		    Func.ShowFullscreenAds((a_oSender, a_bIsSuccess) => OnAdsFinished(ECallback.NEXT));
#else
            Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>ADS TEST</color>"));
            this.OnAdsFinished(ECallback.NEXT);
#endif // #if ADS_MODULE_ENABLE
        }*/
	}

	/** 재시도 버튼을 눌렀을 경우 */
	public void OnTouchRetryBtn() {
        this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.RETRY)?.Invoke(this);
        /*if (GlobalDefine.isLevelEditor)
        {
            this.OnAdsFinished(ECallback.RETRY);
        }
        else
        {
#if ADS_MODULE_ENABLE && !UNITY_EDITOR && !UNITY_STANDALONE
		    Func.ShowFullscreenAds((a_oSender, a_bIsSuccess) => OnAdsFinished(ECallback.RETRY));
#else
            Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>ADS TEST</color>"));
            this.OnAdsFinished(ECallback.RETRY);
#endif // #if ADS_MODULE_ENABLE
        }*/
	}

	/** 나가기 버튼을 눌렀을 경우 */
	public void OnTouchLeaveBtn() {
        this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.LEAVE)?.Invoke(this);
        /*if (GlobalDefine.isLevelEditor)
        {
            this.OnAdsFinished(ECallback.LEAVE);
        }
        else
        {
#if ADS_MODULE_ENABLE && !UNITY_EDITOR && !UNITY_STANDALONE
		    Func.ShowFullscreenAds((a_oSender, a_bIsSuccess) => OnAdsFinished(ECallback.LEAVE));
#else
            Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>ADS TEST</color>"));
            this.OnAdsFinished(ECallback.LEAVE);
#endif // #if ADS_MODULE_ENABLE
        }*/
	}

    public void OnAdsFinished(ECallback callback) 
    {
        this.Params.m_oCallbackDict?.GetValueOrDefault(callback)?.Invoke(this);
    }

    private void OnTouchShopButton()
    {
        GlobalDefine.OpenShop();
    }

    private void OnTouchADBlockButton()
    {
        CSceneManager.GetSceneManager<OverlayScene.CSubOverlaySceneManager>(KCDefine.B_SCENE_N_OVERLAY)?.PurchaseProduct(EProductKinds.SINGLE_PRODUCT_REMOVE_ADS, this.OnPurchaseProduct);
    }

    private void OnPurchaseProduct(CPurchaseManager a_oSender, string a_oProductID, bool a_bIsSuccess) 
    {
        // 결제 되었을 경우
        if(a_bIsSuccess) {
            ADBlockButton.gameObject.SetActive(!CUserInfoStorage.Inst.IsPurchaseRemoveAds);
        }
    }
	#endregion // 함수

	#region 클래스 함수
	/** 매개 변수를 생성한다 */
	public static STParams MakeParams(STRecordInfo a_stRecordInfo, Dictionary<ECallback, System.Action<CResultPopup>> a_oCallbackDict = null, NSEngine.CEngine _engine = null) {
		return new STParams() {
			m_stRecordInfo = a_stRecordInfo, m_oCallbackDict = a_oCallbackDict ?? new Dictionary<ECallback, System.Action<CResultPopup>>(),
            Engine = _engine
		};
	}
	#endregion // 클래스 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

[System.Serializable]
public struct RewardIcons
{
    public EItemKinds kinds;
    public Sprite sprite;
}
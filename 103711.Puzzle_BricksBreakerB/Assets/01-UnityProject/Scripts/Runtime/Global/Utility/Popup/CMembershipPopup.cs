using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Linq;
using Unity.VisualScripting;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using TMPro;

/** 정지 팝업 */
public partial class CMembershipPopup : CSubPopup {
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
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public Dictionary<ECallback, System.Action<CMembershipPopup>> m_oCallbackDict;
	}

	#region 변수
    public GameObject activatedUI;
    public GameObject defaultUI;
    public GameObject objectGet;
    public GameObject objectCheck;
    public List<Button> buttonSubs = new List<Button>();
    public Button buttonGet;
    public Button buttonService;
    public Button buttonPolicy;
    public List<TMP_Text> textSubs = new List<TMP_Text>();
    public TMP_Text textRemainDays;
    public TMP_Text textDescription;

	#endregion // 변수

	#region 프로퍼티
	public STParams Params { get; private set; }
    public List<STProductTradeInfo> subscriptionList;
    private int currentIndex = -1;
    private string serviceURL;
    private string privacyURL;
    private CUserInfoStorage cUserInfoStorage { get { return CUserInfoStorage.Inst; } }
    private CGameInfoStorage cGameInfoStorage { get { return CGameInfoStorage.Inst; } }
    private CCharacterGameInfo cCharacterGameInfo => cGameInfoStorage.GetCharacterGameInfo(cGameInfoStorage.PlayCharacterID);
    private WaitForSecondsRealtime wCheckDelay = new WaitForSecondsRealtime(1f);

    #endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
            ($"{EKey.RETRY_BTN}", this.gameObject, this.OnTouchRetryBtn),
			($"{EKey.LEAVE_BTN}", this.gameObject, this.OnTouchLeaveBtn)
		});

        InitSubscriptions();
        SetupURL();
        SetupButtons();
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init();
		this.Params = a_stParams;

        currentIndex = -1;
        StartCoroutine(CO_CheckTime());
	}

	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {

        RefreshSubscriptionUI(cUserInfoStorage.subscriptionActivated, Access.IsEnableGetSubscriptionReward(cGameInfoStorage.PlayCharacterID));
        RefreshText();
	}


#region Main Functions

    private void InitSubscriptions()
    {
        cCharacterGameInfo.PrevSubscriptionAlertTime = DateTime.Today;
        cGameInfoStorage.SaveGameInfo();

        subscriptionList = Factory.MakeProductTradeInfos(KDefine.G_PRODUCT_KINDS_SUBSCRIPTION_LIST).Values.ToList();
    }

    private void SetupURL()
    {
        // 한국 일 경우
        if(CCommonAppInfoStorage.Inst.CountryCode.Equals(KCDefine.B_KOREA_COUNTRY_CODE)) {
            serviceURL = ProjectManager.servicesURL_KR;
            privacyURL = ProjectManager.privacyURL_KR;
        } else {
            serviceURL = CProjInfoTable.Inst.CompanyInfo.m_oServicesURL;
            privacyURL = CProjInfoTable.Inst.CompanyInfo.m_oPrivacyURL;
        }
    }

    private void SetupButtons()
    {
        for(int i=0; i < buttonSubs.Count; i++)
        {
            int index = i;
            buttonSubs[i]?.ExAddListener(() => {
                    currentIndex = index;
                    this.OnTouchPurchaseBtn(subscriptionList[index]);
                });
        }

        buttonGet?.ExAddListener(this.OnClick_GetRewards);
        buttonService?.ExAddListener(this.OnClick_TermsOfService);
        buttonPolicy?.ExAddListener(this.OnClick_PrivacyPolicy);
    }

    private void RefreshSubscriptionUI(bool isActivated, bool canGetReward)
    {
        activatedUI.SetActive(isActivated);
        defaultUI.SetActive(!isActivated);

        objectGet.SetActive(canGetReward);
        objectCheck.SetActive(!canGetReward);
    }

    private void RefreshText()
    {
        for (int i=0; i < textSubs.Count; i++)
        {
            textSubs[i]?.ExSetText(string.Format(GlobalDefine.FORMAT_SUBSCRIPTION_PRICE[i], GetPrice(subscriptionList[i])), false);
        }

        int remainDays = 0;
        if (cUserInfoStorage.subscriptionInfo != null)
            remainDays = cUserInfoStorage.subscriptionInfo.getRemainingTime().Days;
        
        textRemainDays?.ExSetText((remainDays == 1) ? string.Format(GlobalDefine.FORMAT_SUBSCRIPTION_1DAY_LEFT):
                                                      string.Format(GlobalDefine.FORMAT_SUBSCRIPTION_DAYS_LEFT, remainDays), false);

        textDescription?.ExSetText(string.Format(GlobalDefine.FORMAT_SUBSCRIPTION_DESC, GetPrice(subscriptionList[0]), GetPrice(subscriptionList[1]), GetPrice(subscriptionList[2]), false));
    }

    private string GetPrice(STProductTradeInfo info)
    {
#if !UNITY_EDITOR && PURCHASE_MODULE_ENABLE
        if (Access.GetProduct(info.m_nProductIdx) != null) 
            return Access.GetPriceStr(info.m_nProductIdx);

        return GlobalDefine.STRING_0;
#else
        return string.Format(KCDefine.B_TEXT_FMT_USD_PRICE, info.m_oPayTargetInfoDict.FirstOrDefault().Value.m_stValInfo01.m_dmVal);
#endif
    }

    private IEnumerator CO_CheckTime()
    {
        DateTime prevTime = DateTime.Today;

        while(true)
        {
            if (prevTime != DateTime.Today)
            {
                prevTime = DateTime.Today;

                CSceneManager.GetSceneManager<MainScene.CSubMainSceneManager>(KCDefine.B_SCENE_N_MAIN)?.InitSubscriptions();
                this.UpdateUIsState();	
            }

            yield return wCheckDelay;
        }
    }

#endregion Main Functions


#region Buttons

    private void OnTouchPurchaseBtn(STProductTradeInfo a_stProductTradeInfo) {
#if PURCHASE_MODULE_ENABLE
		CSceneManager.GetSceneManager<OverlayScene.CSubOverlaySceneManager>(KCDefine.B_SCENE_N_OVERLAY)?.PurchaseProduct(a_stProductTradeInfo.m_eProductKinds, this.OnPurchaseProduct);
#else
        //OnPurchaseProduct(a_stProductTradeInfo.m_eProductKinds);
#endif // #if PURCHASE_MODULE_ENABLE
	}

#if PURCHASE_MODULE_ENABLE
	/** 상품이 결제 되었을 경우 */
	private void OnPurchaseProduct(CPurchaseManager a_oSender, string a_oProductID, bool a_bIsSuccess) {
		// 결제 되었을 경우
		if(a_bIsSuccess) {
            Debug.Log(CodeManager.GetMethodName() + string.Format("currentIndex : {0}", currentIndex));
			switch(currentIndex)
            {
                case 0: 
                case 1: 
                case 2: 
                    break;
            }
            currentIndex = -1;
		}

        CSceneManager.GetSceneManager<MainScene.CSubMainSceneManager>(KCDefine.B_SCENE_N_MAIN)?.InitSubscriptions();

#if UNITY_EDITOR
        cUserInfoStorage.subscriptionActivated = true;
#endif

		this.UpdateUIsState();		
        CIndicatorManager.Inst.Close();
	}
#endif

    private void OnClick_GetRewards()
    {
        Debug.Log(CodeManager.GetMethodName());

        buttonGet.ExSetInteractable(false);

        Func.ShowRewardAcquirePopup(this.transform.parent.gameObject, (a_oSender) => {
			try {
				(a_oSender as CRewardAcquirePopup).Init(CRewardAcquirePopup.MakeParams(
                    KDefine.L_SCENE_N_MAIN, 0, 
                    EItemKinds.REWARD_ITEM_SUBSCRIPTION, 0, false, 
                    () => { }
                ));
			} finally { }
		}, null, null);

        cCharacterGameInfo.PrevSubscriptionRewardTime = System.DateTime.Today;
        cGameInfoStorage.SaveGameInfo();
        GlobalDefine.RefreshShopText(CSceneManager.GetSceneManager<MainScene.CSubMainSceneManager>(KCDefine.B_SCENE_N_MAIN)?.rubyText);
        
        this.UpdateUIsState();
    }

    private void OnClick_TermsOfService()
    {
        Application.OpenURL(serviceURL);
    }

    private void OnClick_PrivacyPolicy()
    {
        Application.OpenURL(privacyURL);
    }

#endregion Buttons


    /** 재시도 버튼을 눌렀을 경우 */
	public void OnTouchRetryBtn() {
		this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.RETRY)?.Invoke(this);
	}

	/** 나가기 버튼을 눌렀을 경우 */
	private void OnTouchLeaveBtn() {        
        this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.LEAVE)?.Invoke(this);
	}
	#endregion // 함수

	#region 클래스 함수
	/** 매개 변수를 생성한다 */
	public static STParams MakeParams(Dictionary<ECallback, System.Action<CMembershipPopup>> a_oCallbackDict = null) {
		return new STParams() {
			m_oCallbackDict = a_oCallbackDict ?? new Dictionary<ECallback, System.Action<CMembershipPopup>>()
		};
	}
	#endregion // 클래스 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

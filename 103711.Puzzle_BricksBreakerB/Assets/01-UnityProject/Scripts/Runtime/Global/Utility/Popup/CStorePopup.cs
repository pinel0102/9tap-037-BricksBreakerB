using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.Linq;
using TMPro;

#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif // #if PURCHASE_MODULE_ENABLE

/** 상점 팝업 */
public partial class CStorePopup : CSubPopup {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		SEL_PRODUCT_KINDS,
		[HideInInspector] MAX_VAL
	}

	/** 콜백 */
	public enum ECallback {
		NONE = -1,
		ADS,
		PURCHASE,
		RESTORE,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public List<STProductTradeInfo> m_oProductTradeInfoList;

#if ADS_MODULE_ENABLE
		public Dictionary<ECallback, System.Action<CAdsManager, STAdsRewardInfo, bool>> m_oAdsCallbackDict;
#endif // #if ADS_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
		public Dictionary<ECallback, System.Action<CPurchaseManager, string, bool>> m_oPurchaseCallbackDict01;
		public Dictionary<ECallback, System.Action<CPurchaseManager, List<Product>, bool>> m_oPurchaseCallbackDict02;
#endif // #if PURCHASE_MODULE_ENABLE
	}

	#region 변수
    [Header("★ [Reference] UI")]
    public Canvas storeCanvas;
    public TMP_Text rubyText;
    public TMP_Text starText;
    public Button defaultTab;

    [Header("★ [Reference] Daily Store")]
    public TMP_Text remainTimeTextDaily;
    public Button dailyAdsButton;
    public GameObject dailyAdsSoldout;
    public List<Button> dailyStoreButtons = new List<Button>();
    public List<GameObject> dailyStoreSoldout = new List<GameObject>();
    
    [Header("★ [Reference] Weekly Store")]
    public TMP_Text remainTimeTextWeekly;
    public List<Button> weeklyStoreButtons = new List<Button>();
    public List<GameObject> weeklyStoreSoldout = new List<GameObject>();

    [Header("★ [Parameter] Daily Store")]
    public bool dailyStoreBoughtAds;
    public List<bool> dailyStoreBought = new List<bool>();

    [Header("★ [Parameter] Weekly Store")]
    public List<bool> weeklyStoreBought = new List<bool>();

    [Header("★ [Parameter] Privates")]
    private int currentIndex = -1;
    private long prevDailyTicks = -1;
    private long prevWeeklyTicks = -1;
    private Coroutine remainTimeCoroutine;
    private WaitForSecondsRealtime wDelay = new WaitForSecondsRealtime(0.1f);
    private const string TimeFormatDaily = "{0:00}:{1:00}:{2:00}";
    private const string TimeFormatWeekly = "{3}D+{0:00}:{1:00}:{2:00}";

    private CGameInfoStorage gameInfoStorage { get { return CGameInfoStorage.Inst; } }
    private CCharacterGameInfo characterGameInfo { get { return gameInfoStorage.GetCharacterGameInfo(gameInfoStorage.PlayCharacterID); } }

	private Dictionary<EKey, EProductKinds> m_oProductKindsDict = new Dictionary<EKey, EProductKinds>() {
		[EKey.SEL_PRODUCT_KINDS] = EProductKinds.NONE
	};

#if PURCHASE_MODULE_ENABLE
	private List<Product> m_oRestoreProductList = new List<Product>();
#endif // #if PURCHASE_MODULE_ENABLE

	/** =====> 객체 <===== */
    [Header("★ [Reference] Products")]
	[SerializeField] private List<GameObject> m_oProductBuyUIsList = new List<GameObject>();
	#endregion // 변수

	#region 프로퍼티
	public STParams Params { get; private set; }
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
            (KCDefine.U_OBJ_N_RESTORE_BTN, this.Contents, this.OnTouchRestoreBtn)
		});

        dailyAdsButton.ExAddListener(this.OnTouchAdsBtn);
        storeCanvas.ExSetSortingOrder(GlobalDefine.SortingInfo_StoreCanvas);
        defaultTab.onClick.Invoke();

		this.SubAwake();
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init();
		this.Params = a_stParams;

        currentIndex = -1;
        prevDailyTicks = GlobalDefine.GetTime_UntilTommorow().Ticks;
        prevWeeklyTicks = GlobalDefine.GetTime_UntilMonday().Ticks;

		// 상품 교환 정보를 설정한다
		a_stParams.m_oProductTradeInfoList.Sort((a_stLhs, a_stRhs) => a_stLhs.m_nProductIdx.CompareTo(a_stRhs.m_nProductIdx));

		this.SubInit();
	}

	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();

        if (remainTimeCoroutine != null) StopCoroutine(remainTimeCoroutine);
        remainTimeCoroutine = StartCoroutine(CO_UpdateRemainTime());
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {

        RefreshState();
        this.SubUpdateUIsState();
	}
    
    private void RefreshState()
    {
        GlobalDefine.RefreshShopText(CSceneManager.GetSceneManager<MainScene.CSubMainSceneManager>(KCDefine.B_SCENE_N_MAIN)?.rubyText);
        GlobalDefine.RefreshStarText(CSceneManager.GetSceneManager<MainScene.CSubMainSceneManager>(KCDefine.B_SCENE_N_MAIN)?.starText);
        GlobalDefine.RefreshShopText(rubyText);
        GlobalDefine.RefreshStarText(starText);

        CheckResetTime();

        // 상품 UI 상태를 갱신한다
		for(int i = 0; i < m_oProductBuyUIsList.Count; ++i) {
			this.UpdateProductBuyUIsState(i, m_oProductBuyUIsList[i], this.Params.m_oProductTradeInfoList[i]);
		}

        RefreshDailyStoreState();
        RefreshWeeklyStoreState();
    }

    private void CheckResetTime()
    {
        bool isNeedSave = false; 

        if(Access.IsDailyStoreResetTime(gameInfoStorage.PlayCharacterID)) 
        {
            characterGameInfo.ResetDailyStore();
            isNeedSave = true;
		}
        if(Access.IsWeeklyStoreResetTime(gameInfoStorage.PlayCharacterID)) 
        {
            characterGameInfo.ResetWeeklyStore();
            isNeedSave = true;
		}

        if (isNeedSave)
            gameInfoStorage.SaveGameInfo();
    }

    private void RefreshDailyStoreState()
    {
        dailyStoreBought.Clear();
        dailyStoreBought = characterGameInfo.GetDailyBoughtList();
        dailyStoreBoughtAds = characterGameInfo.DailyStoreBought_Ads;

        for(int i=0; i < dailyStoreButtons.Count; i++)
        {
            dailyStoreSoldout[i]?.SetActive(dailyStoreBought[i]);
            dailyStoreButtons[i]?.ExSetInteractable(!dailyStoreBought[i]);
        }

        dailyAdsSoldout?.SetActive(dailyStoreBoughtAds);
        dailyAdsButton?.ExSetInteractable(!dailyStoreBoughtAds);
    }

    private void RefreshWeeklyStoreState()
    {
        weeklyStoreBought.Clear();
        weeklyStoreBought = characterGameInfo.GetWeeklyBoughtList();

        for(int i=0; i < weeklyStoreButtons.Count; i++)
        {
            weeklyStoreSoldout[i]?.SetActive(weeklyStoreBought[i]);
            weeklyStoreButtons[i]?.ExSetInteractable(!weeklyStoreBought[i]);
        }
    }

#region Daily Store

    private IEnumerator CO_UpdateRemainTime()
    {
        //Debug.Log(CodeManager.GetMethodName() + string.Format("IsEnableGetDailyReward : {0} / GetDailyRewardID : {1}", isEnableGetDailyReward, dailyRewardID));

        while(true)
        {
            bool isNeedRefresh = false;
            TimeSpan ts1 = GlobalDefine.GetTime_UntilTommorow();
            TimeSpan ts2 = GlobalDefine.GetTime_UntilMonday();

            if (ts1.Ticks < prevDailyTicks)
            {
                remainTimeTextDaily.text = string.Format(TimeFormatDaily, ts1.Hours, ts1.Minutes, ts1.Seconds);
            }
            else
            {
                remainTimeTextDaily.text = string.Empty;
                isNeedRefresh = true;
            }
            
            if (ts2.Ticks < prevWeeklyTicks)
            {
                if (ts2.Days > 0)
                    remainTimeTextWeekly.text = string.Format(TimeFormatWeekly, ts2.Hours, ts2.Minutes, ts2.Seconds, ts2.Days);
                else
                    remainTimeTextWeekly.text = string.Format(TimeFormatDaily, ts2.Hours, ts2.Minutes, ts2.Seconds);
            }
            else
            {
                remainTimeTextWeekly.text = string.Empty;
                isNeedRefresh = true;
            }

            prevDailyTicks = ts1.Ticks;
            prevWeeklyTicks = ts2.Ticks;
            
            yield return wDelay;

            if (isNeedRefresh)
            {
                RefreshState();
            }
        }
    }

#endregion Daily Store

	/** 상품 구입 UI 상태를 갱신한다 */
	private void UpdateProductBuyUIsState(int _index, GameObject a_oProductBuyUIs, STProductTradeInfo a_stProductTradeInfo) {
		var oPriceUIsDict = CCollectionManager.Inst.SpawnDict<EPurchaseType, GameObject>();

		try {
			// 객체를 갱신한다 {
			CFunc.SetupObjs(new List<(EPurchaseType, string, GameObject)>() {
				(EPurchaseType.ADS, KCDefine.U_OBJ_N_ADS_PRICE_UIS, a_oProductBuyUIs),
				(EPurchaseType.IN_APP_PURCHASE, KCDefine.U_OBJ_N_PURCHASE_PRICE_UIS, a_oProductBuyUIs)
			}, oPriceUIsDict);

			foreach(var stKeyVal in oPriceUIsDict) {
				stKeyVal.Value?.SetActive(a_stProductTradeInfo.m_ePurchaseType == stKeyVal.Key);
			}
			// 객체를 갱신한다 }

			// 텍스트를 갱신한다 {
			var oPriceText = a_oProductBuyUIs.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_PRICE_TEXT);
            oPriceText?.ExSetText(string.Format(KCDefine.B_TEXT_FMT_USD_PRICE, a_stProductTradeInfo.m_oPayTargetInfoDict.FirstOrDefault().Value.m_stValInfo01.m_dmVal));

			var oAcquireTargetInfoKeyList = a_stProductTradeInfo.m_oAcquireTargetInfoDict.Keys.ToList();
			var oNameText = a_oProductBuyUIs.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_NAME_TEXT);
            oNameText?.ExSetText(a_stProductTradeInfo.m_stCommonInfo.m_oName);

			for(int i = 0; i < oAcquireTargetInfoKeyList.Count; ++i) {
				var nUniqueTargetInfoID = oAcquireTargetInfoKeyList[i];
				a_oProductBuyUIs.ExFindComponent<TMP_Text>(string.Format(KCDefine.U_OBJ_N_FMT_NUM_TEXT, i + KCDefine.B_VAL_1_INT))?.ExSetText($"x{a_stProductTradeInfo.m_oAcquireTargetInfoDict.GetValueOrDefault(nUniqueTargetInfoID).m_stValInfo01.m_dmVal}", false);
			}

#if !UNITY_EDITOR && PURCHASE_MODULE_ENABLE
			// 인앱 결제 상품 일 경우
			if(a_stProductTradeInfo.m_ePurchaseType == EPurchaseType.IN_APP_PURCHASE && Access.GetProduct(a_stProductTradeInfo.m_nProductIdx) != null) {
				oPriceText?.ExSetText(Access.GetPriceStr(a_stProductTradeInfo.m_nProductIdx), false);
			}
#endif // #if !UNITY_EDITOR && PURCHASE_MODULE_ENABLE
			// 텍스트를 갱신한다 }

			// 버튼을 갱신한다 {
			var oPurchaseBtn = oPriceUIsDict[EPurchaseType.IN_APP_PURCHASE]?.ExFindComponent<Button>(KCDefine.U_OBJ_N_PURCHASE_BTN);
			oPurchaseBtn?.ExAddListener(() => { currentIndex = _index;
                                                this.OnTouchPurchaseBtn(a_stProductTradeInfo); });

#if PURCHASE_MODULE_ENABLE
			var stProductInfo = CProductInfoTable.Inst.GetProductInfo(a_stProductTradeInfo.m_nProductIdx);

			// 비소모 상품 일 경우
			if(stProductInfo.m_eProductType == ProductType.NonConsumable) {
                Debug.Log(CodeManager.GetMethodName() + string.Format("NonConsumable Check : {0} : {1}", stProductInfo.m_stCommonInfo.m_oName, CPurchaseManager.Inst.IsPurchaseNonConsumableProduct(stProductInfo.m_oID)));
				oPurchaseBtn?.ExSetInteractable(!CPurchaseManager.Inst.IsPurchaseNonConsumableProduct(stProductInfo.m_oID));
			}
#endif // #if PURCHASE_MODULE_ENABLE
			// 버튼을 갱신한다 }

		} finally {
			CCollectionManager.Inst.DespawnDict(oPriceUIsDict);
		}
	}

	/** 결제 버튼을 눌렀을 경우 */
	private void OnTouchPurchaseBtn(STProductTradeInfo a_stProductTradeInfo) {
        switch(a_stProductTradeInfo.m_ePurchaseType) {
			/*case EPurchaseType.ADS: {
#if ADS_MODULE_ENABLE
				m_oProductKindsDict[EKey.SEL_PRODUCT_KINDS] = a_stProductTradeInfo.m_eProductKinds;
				Func.ShowRewardAds(this.OnCloseRewardAds);
#endif // #if ADS_MODULE_ENABLE

				break;
			}*/
			case EPurchaseType.IN_APP_PURCHASE: {
#if PURCHASE_MODULE_ENABLE
				CSceneManager.GetSceneManager<OverlayScene.CSubOverlaySceneManager>(KCDefine.B_SCENE_N_OVERLAY)?.PurchaseProduct(a_stProductTradeInfo.m_eProductKinds, this.OnPurchaseProduct);
#else
                //OnPurchaseProduct(a_stProductTradeInfo.m_eProductKinds);
#endif // #if PURCHASE_MODULE_ENABLE

				break;
			}
			case EPurchaseType.TARGET: {
				Func.Trade(gameInfoStorage.PlayCharacterID, a_stProductTradeInfo);
				break;
			}
		}
	}

	/** 복원 버튼을 눌렀을 경우 */
	private void OnTouchRestoreBtn() {
#if PURCHASE_MODULE_ENABLE
		m_oRestoreProductList.Clear();
		Func.RestoreProducts(this.OnRestoreProducts);
#endif // #if PURCHASE_MODULE_ENABLE
	}
	#endregion // 함수

	#region 클래스 함수
	/** 매개 변수를 생성한다 */
	public static STParams MakeParams(List<STProductTradeInfo> a_oProductTradeInfoList) {
		return new STParams() {
			m_oProductTradeInfoList = a_oProductTradeInfoList,

#if ADS_MODULE_ENABLE
			m_oAdsCallbackDict = new Dictionary<ECallback, System.Action<CAdsManager, STAdsRewardInfo, bool>>(),
#endif // #if ADS_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
			m_oPurchaseCallbackDict01 = new Dictionary<ECallback, System.Action<CPurchaseManager, string, bool>>(),
			m_oPurchaseCallbackDict02 = new Dictionary<ECallback, System.Action<CPurchaseManager, List<Product>, bool>>()
#endif // #if PURCHASE_MODULE_ENABLE
		};
	}
	#endregion // 클래스 함수

	#region 조건부 함수
    private void OnTouchAdsBtn()
    {
        if (GlobalDefine.isLevelEditor)
        {
            this.ShowRewardAcquirePopup(true);
        }
        else
        {
#if ADS_MODULE_ENABLE && !UNITY_EDITOR && !UNITY_STANDALONE
		    Func.ShowRewardAds(this.OnCloseRewardAds);
#else
            Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>ADS TEST</color>"));
            this.ShowRewardAcquirePopup(true);
#endif // #if ADS_MODULE_ENABLE
        }
    }

#if ADS_MODULE_ENABLE
	/** 보상 광고가 닫혔을 경우 */
	private void OnCloseRewardAds(CAdsManager a_oSender, STAdsRewardInfo a_stAdsRewardInfo, bool a_bIsSuccess) {
		// 광고를 시청했을 경우
		if(a_bIsSuccess) {
			this.ShowRewardAcquirePopup(true);
		}
	}
#endif // #if ADS_MODULE_ENABLE

	/** 보상 획득 팝업을 출력한다 */
	public void ShowRewardAcquirePopup(bool a_bIsWatchRewardAds) {
		Func.ShowRewardAcquirePopup(this.transform.parent.gameObject, (a_oSender) => {
			var oTargetInfoDict = CCollectionManager.Inst.SpawnDict<ulong, STTargetInfo>();

			try {
				(a_oSender as CRewardAcquirePopup).Init(CRewardAcquirePopup.MakeParams(KDefine.L_SCENE_N_MAIN, 0, ERewardKinds.ADS_REWARD_DAILY_RUBY, EItemKinds.GOODS_RUBY, UnityEngine.Random.Range(GlobalDefine.RewardRuby_Daily_Store_Min, GlobalDefine.RewardRuby_Daily_Store_Max + KCDefine.B_VAL_1_INT), false, 
                () => { 
                        characterGameInfo.DailyStoreBought_Ads = true;
                        gameInfoStorage.SaveGameInfo();
                        UpdateUIsState(); 
                    }, this));
			} finally {
				CCollectionManager.Inst.DespawnDict(oTargetInfoDict);
			}
		}, null, this.OnCloseRewardAcquirePopup);
	}

    /** 보상 획득 팝업이 닫혔을 경우 */
	private void OnCloseRewardAcquirePopup(CPopup a_oSender) {
		//this.Close();
	}

#if PURCHASE_MODULE_ENABLE
	/** 상품이 결제 되었을 경우 */
	private void OnPurchaseProduct(CPurchaseManager a_oSender, string a_oProductID, bool a_bIsSuccess) {
		// 결제 되었을 경우
		if(a_bIsSuccess) {
            Debug.Log(CodeManager.GetMethodName() + string.Format("currentIndex : {0}", currentIndex));
			switch(currentIndex)
            {
                case 14: 
                    characterGameInfo.DailyStoreBought_0 = true;
                    gameInfoStorage.SaveGameInfo();
                    break;

                case 15: 
                    characterGameInfo.DailyStoreBought_1 = true;
                    gameInfoStorage.SaveGameInfo();
                    break;

                case 16: 
                    characterGameInfo.DailyStoreBought_2 = true;
                    gameInfoStorage.SaveGameInfo();
                    break;

                case 17:
                    characterGameInfo.WeeklyStoreBought_0 = true;
                    gameInfoStorage.SaveGameInfo();
                    break;

                case 18:
                    characterGameInfo.WeeklyStoreBought_1 = true;
                    gameInfoStorage.SaveGameInfo();
                    break;

                case 19:
                    characterGameInfo.WeeklyStoreBought_2 = true;
                    gameInfoStorage.SaveGameInfo();
                    break;
            }
            currentIndex = -1;
		}

		this.UpdateUIsState();
		this.Params.m_oPurchaseCallbackDict01?.GetValueOrDefault(ECallback.PURCHASE)?.Invoke(a_oSender, a_oProductID, a_bIsSuccess);
	}

	/** 상품이 복원 되었을 경우 */
	public void OnRestoreProducts(CPurchaseManager a_oSender, List<Product> a_oProductList, bool a_bIsSuccess) {
		// 복원 되었을 경우
		if(a_bIsSuccess) {
			m_oRestoreProductList = a_oProductList;
			Func.AcquireRestoreProducts(a_oProductList);
            this.UpdateUIsState();
		}

#if FIREBASE_MODULE_ENABLE
		this.ExLateCallFunc((a_oFuncSender) => Func.LoadTargetInfos(this.OnLoadTargetInfos));
#else
		Func.OnRestoreProducts(a_oSender, a_oProductList, a_bIsSuccess, null);
#endif // #if FIREBASE_MODULE_ENABLE

		this.UpdateUIsState();
		this.Params.m_oPurchaseCallbackDict02?.GetValueOrDefault(ECallback.RESTORE)?.Invoke(a_oSender, a_oProductList, a_bIsSuccess);
	}

#if FIREBASE_MODULE_ENABLE
	/** 타겟 정보를 로드했을 경우 */
	private void OnLoadTargetInfos(CFirebaseManager a_oSender, string a_oJSONStr, bool a_bIsSuccess) {
		// 로드 되었을 경우
		if(a_bIsSuccess && a_oJSONStr.ExIsValid()) {
			var oTargetInfoDict = a_oJSONStr.ExJSONStrToTargetInfos();
			Func.Acquire(gameInfoStorage.PlayCharacterID, oTargetInfoDict, true);

			this.ExLateCallFunc((a_oFuncSender) => { oTargetInfoDict.Clear(); Func.SaveTargetInfos(oTargetInfoDict, this.OnSaveTargetInfos); });
		} else {
			Func.OnRestoreProducts(CPurchaseManager.Inst, m_oRestoreProductList, m_oRestoreProductList.ExIsValid(), null);
		}

		this.UpdateUIsState();
	}

	/** 타겟 정보를 저장했을 경우 */
	private void OnSaveTargetInfos(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		Func.OnRestoreProducts(CPurchaseManager.Inst, m_oRestoreProductList, m_oRestoreProductList.ExIsValid(), null);
	}
#endif // #if FIREBASE_MODULE_ENABLE
#endif // #if PURCHASE_MODULE_ENABLE
	#endregion // 조건부 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

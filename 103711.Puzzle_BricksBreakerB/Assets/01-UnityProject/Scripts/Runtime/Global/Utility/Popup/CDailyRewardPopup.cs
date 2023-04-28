using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using TMPro;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using DG.Tweening;

/** 일일 보상 팝업 */
public partial class CDailyRewardPopup : CSubPopup {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		ADS_BTN,
		ACQUIRE_BTN,
		[HideInInspector] MAX_VAL
	}

	#region 변수
	/** =====> UI <===== */
	private Dictionary<EKey, Button> m_oBtnDict = new Dictionary<EKey, Button>();

	/** =====> 객체 <===== */
	[SerializeField] private List<GameObject> m_oRewardUIsList = new List<GameObject>();
	#endregion // 변수

	#region 프로퍼티
    public TMP_Text remainTimeText;
    public bool isEnableGetDailyReward;
    public bool isEnableGetDailyAD;
    public int dailyRewardID;
    public DateTime nextRewardTime;

    private Coroutine remainTimeCoroutine;
    private WaitForSecondsRealtime wDelay = new WaitForSecondsRealtime(0.1f);
    private const string TimeFormat = "{0:00}:{1:00}:{2:00}";
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		
		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
			(EKey.ADS_BTN, $"{EKey.ADS_BTN}", this.Contents, this.OnTouchAdsBtn),
			(EKey.ACQUIRE_BTN, $"{EKey.ACQUIRE_BTN}", this.Contents, this.OnTouchAcquireBtn)
		}, m_oBtnDict);

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

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {

        RefreshState();
        
        if (remainTimeCoroutine != null) StopCoroutine(remainTimeCoroutine);
        remainTimeCoroutine = StartCoroutine(CO_UpdateRemainTime());

		this.SubUpdateUIsState();
	}

    private void RefreshState()
    {
        GlobalDefine.RefreshShopText(CSceneManager.GetSceneManager<MainScene.CSubMainSceneManager>(KCDefine.B_SCENE_N_MAIN)?.rubyText);

        isEnableGetDailyReward = Access.IsEnableGetDailyReward(CGameInfoStorage.Inst.PlayCharacterID);
        isEnableGetDailyAD = Access.IsEnableGetFreeReward(CGameInfoStorage.Inst.PlayCharacterID);
        dailyRewardID = Access.GetDailyRewardID(CGameInfoStorage.Inst.PlayCharacterID);

        var oCharacterGameInfo = CGameInfoStorage.Inst.GetCharacterGameInfo(CGameInfoStorage.Inst.PlayCharacterID);
        nextRewardTime = oCharacterGameInfo.PrevDailyRewardTime.AddDays(KCDefine.B_VAL_1_INT);

        Debug.Log(CodeManager.GetMethodName() + string.Format("[DailyReward] {0} / {1}", isEnableGetDailyReward, oCharacterGameInfo.PrevDailyRewardTime.ToString(KCDefine.B_DATE_T_FMT_SLASH_YYYY_MM_DD_HH_MM_SS)));
        Debug.Log(CodeManager.GetMethodName() + string.Format("[DailyAD]     {0} / {1}", isEnableGetDailyAD, oCharacterGameInfo.PrevFreeRewardTime.ToString(KCDefine.B_DATE_T_FMT_SLASH_YYYY_MM_DD_HH_MM_SS)));

        // 버튼을 갱신한다
        m_oBtnDict[EKey.ACQUIRE_BTN]?.ExSetInteractable(isEnableGetDailyReward);
		m_oBtnDict[EKey.ADS_BTN]?.ExSetInteractable(isEnableGetDailyAD);

		// 보상 UI 상태를 갱신한다
		for(int i = 0; i < m_oRewardUIsList.Count; ++i) {
			// 보상 정보가 존재 할 경우
			if(CRewardInfoTable.Inst.TryGetRewardInfo(KDefine.G_REWARDS_KINDS_DAILY_REWARD_LIST[i], out STRewardInfo stRewardInfo)) {
                this.UpdateRewardUIsState(m_oRewardUIsList[i], stRewardInfo);
			}
		}
    }


#region Daily Reward

    private IEnumerator CO_UpdateRemainTime()
    {
        //Debug.Log(CodeManager.GetMethodName() + string.Format("IsEnableGetDailyReward : {0} / GetDailyRewardID : {1}", isEnableGetDailyReward, dailyRewardID));

        while(true)
        {
            TimeSpan ts = TimeSpan.FromTicks(nextRewardTime.Subtract(DateTime.Now).Ticks);
            if (ts.Ticks > 0)
            {
                remainTimeText.text = string.Format(TimeFormat, ts.Hours, ts.Minutes, ts.Seconds);
                yield return wDelay;
            }
            else
            {
                remainTimeText.text = string.Empty;
                break;
            }
        }

        RefreshState();
    }

	/** 획득 버튼을 눌렀을 경우 */
	private void OnTouchAcquireBtn() {

		#region 추가
        m_oBtnDict[EKey.ACQUIRE_BTN].ExSetInteractable(false);
		//m_oBtnDict[EKey.ADS_BTN].ExSetInteractable(false);

        int index = Access.GetDailyRewardID(CGameInfoStorage.Inst.PlayCharacterID);
		var stRewardInfo = CRewardInfoTable.Inst.GetRewardInfo(KDefine.G_REWARDS_KINDS_DAILY_REWARD_LIST[index]);

        //Debug.Log(CodeManager.GetMethodName() + string.Format("{0}", stRewardInfo.m_eRewardKinds));

		Func.Acquire(CGameInfoStorage.Inst.PlayCharacterID, stRewardInfo.m_oAcquireTargetInfoDict);

        Func.SetupNextDailyRewardID(CGameInfoStorage.Inst.PlayCharacterID);
		CGameInfoStorage.Inst.SaveGameInfo();
        GlobalDefine.RefreshShopText(CSceneManager.GetSceneManager<MainScene.CSubMainSceneManager>(KCDefine.B_SCENE_N_MAIN)?.rubyText);

        GlobalDefine.PlaySoundFX(ESoundSet.SOUND_GET_STAR);

        UpdateUIsState();
		
        LogFunc.Send_C_Item_Get(KCDefine.B_IDX_INVALID, KDefine.L_SCENE_N_MAIN, LogFunc.MakeLogItemInfo(stRewardInfo.m_oAcquireTargetInfoDict));

        //this.ShowRewardAcquirePopup(false);
		
        #endregion // 추가
	}

#endregion Daily Reward


#region Reward Ads Button

    /** 광고 버튼을 눌렀을 경우 */
	private void OnTouchAdsBtn() 
    {
        if (GlobalDefine.isLevelEditor)
        {
            this.ShowRewardAcquirePopup(true);
        }
        else
        {
            GlobalDefine.RequestRewardVideo(RewardVideoType.DAILY_FREE_REWARD, this);
#if ADS_MODULE_ENABLE
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
				(a_oSender as CRewardAcquirePopup).Init(CRewardAcquirePopup.MakeParams(EItemKinds.GOODS_RUBY, GlobalDefine.RewardRuby_Daily, false, () => { RefreshState(); }, this));
			} finally {
				CCollectionManager.Inst.DespawnDict(oTargetInfoDict);
			}
		}, null, this.OnCloseRewardAcquirePopup);
	}

    /** 보상 획득 팝업이 닫혔을 경우 */
	private void OnCloseRewardAcquirePopup(CPopup a_oSender) {

		//this.Close();
	}

#endregion Reward Ads Button

	#endregion // 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

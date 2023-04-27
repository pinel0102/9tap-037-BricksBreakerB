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
        isEnableGetDailyReward = Access.IsEnableGetDailyReward(CGameInfoStorage.Inst.PlayCharacterID);
        dailyRewardID = Access.GetDailyRewardID(CGameInfoStorage.Inst.PlayCharacterID);

        var oCharacterGameInfo = CGameInfoStorage.Inst.GetCharacterGameInfo(CGameInfoStorage.Inst.PlayCharacterID);
        nextRewardTime = oCharacterGameInfo.PrevDailyRewardTime.AddDays(KCDefine.B_VAL_1_INT);

        // 버튼을 갱신한다
		m_oBtnDict[EKey.ADS_BTN]?.ExSetInteractable(isEnableGetDailyReward);
		m_oBtnDict[EKey.ACQUIRE_BTN]?.ExSetInteractable(isEnableGetDailyReward);

		// 보상 UI 상태를 갱신한다
		for(int i = 0; i < m_oRewardUIsList.Count; ++i) {
			// 보상 정보가 존재 할 경우
			if(CRewardInfoTable.Inst.TryGetRewardInfo(KDefine.G_REWARDS_KINDS_DAILY_REWARD_LIST[i], out STRewardInfo stRewardInfo)) {
                this.UpdateRewardUIsState(m_oRewardUIsList[i], stRewardInfo);
			}
		}
    }

    private IEnumerator CO_UpdateRemainTime()
    {
        Debug.Log(CodeManager.GetMethodName() + string.Format("IsEnableGetDailyReward : {0} / GetDailyRewardID : {1}", isEnableGetDailyReward, dailyRewardID));

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

	/** 광고 버튼을 눌렀을 경우 */
	private void OnTouchAdsBtn() 
    {
        GlobalDefine.RequestRewardVideo(RewardVideoType.DAILY_REWARD, this);        
#if ADS_MODULE_ENABLE
		Func.ShowRewardAds(this.OnCloseRewardAds);
#endif // #if ADS_MODULE_ENABLE
	}

	/** 획득 버튼을 눌렀을 경우 */
	private void OnTouchAcquireBtn() {
#if NEVER_USE_THIS
		this.ShowRewardAcquirePopup(false);
#endif // #if NEVER_USE_THIS

		#region 추가
		m_oBtnDict[EKey.ADS_BTN].ExSetInteractable(false);
        m_oBtnDict[EKey.ACQUIRE_BTN].ExSetInteractable(false);

        int index = Access.GetDailyRewardID(CGameInfoStorage.Inst.PlayCharacterID);
		var stRewardInfo = CRewardInfoTable.Inst.GetRewardInfo(KDefine.G_REWARDS_KINDS_DAILY_REWARD_LIST[index]);

        //Debug.Log(CodeManager.GetMethodName() + string.Format("{0}", stRewardInfo.m_eRewardKinds));

		Func.Acquire(CGameInfoStorage.Inst.PlayCharacterID, stRewardInfo.m_oAcquireTargetInfoDict);

        Func.SetupNextDailyRewardID(CGameInfoStorage.Inst.PlayCharacterID);
		CGameInfoStorage.Inst.SaveGameInfo();
        GlobalDefine.RefreshShopText(CSceneManager.GetSceneManager<MainScene.CSubMainSceneManager>(KCDefine.B_SCENE_N_MAIN)?.rubyText);

        UpdateUIsState();
		
        //this.StartAcquireAni(m_oRewardUIsList[index]);
		
        LogFunc.Send_C_Item_Get(KCDefine.B_IDX_INVALID, KDefine.L_SCENE_N_MAIN, LogFunc.MakeLogItemInfo(stRewardInfo.m_oAcquireTargetInfoDict));
		
        #endregion // 추가
	}

	/** 보상 획득 팝업이 닫혔을 경우 */
	private void OnCloseRewardAcquirePopup(CPopup a_oSender) {
		Func.SetupNextDailyRewardID(CGameInfoStorage.Inst.PlayCharacterID);
		CGameInfoStorage.Inst.SaveGameInfo();

		// UI 상태를 갱신한다
		CSceneManager.GetSceneManager<MainScene.CSubMainSceneManager>(KCDefine.B_SCENE_N_MAIN)?.gameObject.ExSendMsg(string.Empty, KCDefine.U_FUNC_N_UPDATE_UIS_STATE, a_bIsEnableAssert: false);
		CSceneManager.GetSceneManager<GameScene.CSubGameSceneManager>(KCDefine.B_SCENE_N_GAME)?.gameObject.ExSendMsg(string.Empty, KCDefine.U_FUNC_N_UPDATE_UIS_STATE, a_bIsEnableAssert: false);
		CSceneManager.GetSceneManager<TitleScene.CSubTitleSceneManager>(KCDefine.B_SCENE_N_TITLE)?.gameObject.ExSendMsg(string.Empty, KCDefine.U_FUNC_N_UPDATE_UIS_STATE, a_bIsEnableAssert: false);

		this.Close();
	}

	/** 보상 획득 팝업을 출력한다 */
	private void ShowRewardAcquirePopup(bool a_bIsWatchRewardAds) {
		Func.ShowRewardAcquirePopup(this.transform.parent.gameObject, (a_oSender) => {
			var oTargetInfoDict = CCollectionManager.Inst.SpawnDict<ulong, STTargetInfo>();

			try {
				var eRewardKinds = Access.GetDailyRewardKinds(CGameInfoStorage.Inst.PlayCharacterID);
				var stRewardInfo = CRewardInfoTable.Inst.GetRewardInfo(eRewardKinds);

				foreach(var stKeyVal in stRewardInfo.m_oAcquireTargetInfoDict) {
                    var stValInfo = new STValInfo(stKeyVal.Value.m_stValInfo01.m_eValType, a_bIsWatchRewardAds ? stKeyVal.Value.m_stValInfo01.m_dmVal * KCDefine.B_VAL_2_INT : stKeyVal.Value.m_stValInfo01.m_dmVal);
					oTargetInfoDict.TryAdd(stKeyVal.Key, new STTargetInfo(stKeyVal.Value.m_eTargetKinds, stKeyVal.Value.m_nKinds, stValInfo));
				}
				
				oTargetInfoDict.ExCopyTo(stRewardInfo.m_oAcquireTargetInfoDict, (a_stTargetInfo) => a_stTargetInfo);
				(a_oSender as CRewardAcquirePopup).Init(CRewardAcquirePopup.MakeParams(stRewardInfo.m_eRewardQuality, ERewardAcquirePopupType.DAILY, stRewardInfo.m_oAcquireTargetInfoDict));
			} finally {
				CCollectionManager.Inst.DespawnDict(oTargetInfoDict);
			}
		}, null, this.OnCloseRewardAcquirePopup);
	}
	#endregion // 함수

	#region 조건부 함수
#if ADS_MODULE_ENABLE
	/** 보상 광고가 닫혔을 경우 */
	private void OnCloseRewardAds(CAdsManager a_oSender, STAdsRewardInfo a_stAdsRewardInfo, bool a_bIsSuccess) {
		// 광고를 시청했을 경우
		if(a_bIsSuccess) {
			this.ShowRewardAcquirePopup(true);
		}
	}
#endif // #if ADS_MODULE_ENABLE
	#endregion // 조건부 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

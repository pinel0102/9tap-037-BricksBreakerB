using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.Linq;

/** 보상 획득 팝업 */
public partial class CRewardAcquirePopup : CSubPopup {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		ADS_BTN,
		ACQUIRE_BTN,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public System.Action m_oCallback;
        public CPopup parentPopup;
        public EItemKinds kinds;
        public int count;
        public bool isEnableAD;
	}

	#region 변수
	/** =====> UI <===== */
	private Dictionary<EKey, Button> m_oBtnDict = new Dictionary<EKey, Button>();

	/** =====> 객체 <===== */
    public List<GameObject> itemList = new List<GameObject>();
    public TMP_Text countText;
    public Button GetButton;
    public Button ADButton;
    public bool isRewardAD;

	#endregion // 변수

	#region 프로퍼티
	public STParams Params { get; private set; }
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		this.SetIgnoreAni(true);
		this.SetIgnoreNavStackEvent(true);

		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
			(EKey.ADS_BTN, $"{EKey.ADS_BTN}", this.Contents, this.OnTouchAdsBtn),
			(EKey.ACQUIRE_BTN, $"{EKey.ACQUIRE_BTN}", this.Contents, this.OnTouchAcquireBtn)
		}, m_oBtnDict);

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
		
        // 보상 아이템 UI 상태를 갱신한다
		for(int i = 0; i < itemList.Count; ++i) {
			itemList[i].SetActive(itemList[i].gameObject.name.Equals($"{Params.kinds}"));
		}

        countText.text = string.Format(GlobalDefine.FORMAT_ITEM_COUNT, Params.count);
        ADButton.gameObject.SetActive(Params.isEnableAD);

        isRewardAD = false;

        GlobalDefine.PlaySoundFX(ESoundSet.SOUND_GET_STAR);

		this.SubUpdateUIsState();
	}

#region Reward Get Button

    /** 획득 버튼을 눌렀을 경우 */
	private void OnTouchAcquireBtn() {
		this.AcquireRewards(isRewardAD);
	}

	/** 보상을 획득한다 */
	private void AcquireRewards(bool a_bIsWatchRewardAds) {
		m_oBtnDict.GetValueOrDefault(EKey.ADS_BTN)?.ExSetInteractable(false);
		m_oBtnDict.GetValueOrDefault(EKey.ACQUIRE_BTN)?.ExSetInteractable(false);

		var oRewardTargetInfoDict = new Dictionary<ulong, STTargetInfo>();

		GlobalDefine.AddItem(Params.kinds, a_bIsWatchRewardAds ? Params.count * KCDefine.B_VAL_2_INT : Params.count);
        
        Func.SetupNextFreeRewardID(CGameInfoStorage.Inst.PlayCharacterID);

        this.Params.m_oCallback?.Invoke();

		this.OnTouchCloseBtn();
	}

#endregion Reward Get Button


#region Reward Ads Button

	/** 광고 버튼을 눌렀을 경우 */
	private void OnTouchAdsBtn() {
        if (GlobalDefine.isLevelEditor)
        {
            SetBonusReward();
        }
        else
        {
            //GlobalDefine.RequestRewardVideo(RewardVideoType.DAILY_FREE_REWARD, this);
#if ADS_MODULE_ENABLE && !UNITY_EDITOR && !UNITY_STANDALONE
		    Func.ShowRewardAds(this.OnCloseRewardAds);
#else
            Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>ADS TEST</color>"));
            SetBonusReward();
#endif // #if ADS_MODULE_ENABLE
        }
	}

    private void SetBonusReward()
    {
        isRewardAD = true;
        ADButton.gameObject.SetActive(false);
        countText.text = string.Format(GlobalDefine.FORMAT_ITEM_COUNT, Params.count * KCDefine.B_VAL_2_INT);
    }

#if ADS_MODULE_ENABLE
	/** 보상 광고가 닫혔을 경우 */
	private void OnCloseRewardAds(CAdsManager a_oSender, STAdsRewardInfo a_stAdsRewardInfo, bool a_bIsSuccess) {
		// 광고를 시청했을 경우
		if(a_bIsSuccess) {
			SetBonusReward();
		}
	}
#endif // #if ADS_MODULE_ENABLE

#endregion Reward Ads Button


	#endregion // 함수

	#region 클래스 함수
	/** 매개 변수를 생성한다 */
	/*public static STParams MakeParams(ERewardQuality a_eQuality, ERewardAcquirePopupType a_eAgreePopup, Dictionary<ulong, STTargetInfo> a_oRewardTargetInfoDict) {
		return new STParams() {
			m_eQuality = a_eQuality, m_eAgreePopup = a_eAgreePopup, m_oRewardTargetInfoDict = a_oRewardTargetInfoDict
		};
	}*/

    public static STParams MakeParams(EItemKinds _kinds, int _count, bool _isEnableAD, System.Action _m_oCallback, CPopup _parentPopup) {
		return new STParams() {
			kinds = _kinds, count = _count, isEnableAD = _isEnableAD, m_oCallback = _m_oCallback, parentPopup = _parentPopup
		};
	}
	#endregion // 클래스 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

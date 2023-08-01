using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.Linq;

/** 보상 획득 팝업 */
public partial class CRewardVideoAlertPopup : CSubPopup {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		ADS_BTN,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public System.Action m_oCallback;
        public CPopup parentPopup;
        public ERewardKinds rewardKinds;
        public EItemKinds kinds;
        public string sceneName;
        public int currentLevel;
        public int count;
        public string description;
        public RewardVideoType rewardVideoType;
	}

	#region 변수
	/** =====> UI <===== */
	private Dictionary<EKey, Button> m_oBtnDict = new Dictionary<EKey, Button>();

	/** =====> 객체 <===== */
    public Canvas canvas;
    public List<GameObject> itemList = new List<GameObject>();
    public TMP_Text descText;
    public TMP_Text countText;
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
			(EKey.ADS_BTN, $"{EKey.ADS_BTN}", this.Contents, this.OnTouchAdsBtn)
		}, m_oBtnDict);

        canvas.ExSetSortingOrder(GlobalDefine.SortingInfo_RewardAlertCanvas);

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

        descText.text = Params.description;
        countText.text = string.Format(GlobalDefine.FORMAT_ITEM_COUNT, Params.count);
        countText.gameObject.SetActive(Params.count > 0);

        isRewardAD = false;

        GlobalDefine.PlaySoundFX(ESoundSet.SOUND_BUTTON);

		this.SubUpdateUIsState();
	}

#region Reward Get Button

    /** 보상을 획득한다 */
	private void AcquireRewards(bool a_bIsWatchRewardAds) {
		m_oBtnDict.GetValueOrDefault(EKey.ADS_BTN)?.ExSetInteractable(false);

		GlobalDefine.AddItem(Params.kinds, a_bIsWatchRewardAds ? Params.count * KCDefine.B_VAL_2_INT : Params.count);
        
        //LogFunc.Send_C_Item_Get(Params.currentLevel - 1, Params.sceneName, LogFunc.MakeLogItemInfo(CRewardInfoTable.Inst.GetRewardInfo(Params.rewardKinds).m_oAcquireTargetInfoDict));
        
        this.Params.m_oCallback?.Invoke();

		this.OnTouchCloseBtn();
	}

#endregion Reward Get Button


#region Reward Ads Button

	/** 광고 버튼을 눌렀을 경우 */
	private void OnTouchAdsBtn() {
        if (GlobalDefine.isLevelEditor)
        {
            this.OnCloseRewardAds(null, STAdsRewardInfo.INVALID, true);
        }
        else
        {   
#if ADS_MODULE_ENABLE && !UNITY_EDITOR && !UNITY_STANDALONE
		    Func.ShowRewardAds(this.OnCloseRewardAds);
#else
            Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>ADS TEST</color>"));
            this.OnCloseRewardAds(null, STAdsRewardInfo.INVALID, true);
#endif // #if ADS_MODULE_ENABLE
        }
	}

#if ADS_MODULE_ENABLE
	/** 보상 광고가 닫혔을 경우 */
	private void OnCloseRewardAds(CAdsManager a_oSender, STAdsRewardInfo a_stAdsRewardInfo, bool a_bIsSuccess) {
		// 광고를 시청했을 경우
		if(a_bIsSuccess) {
            if (Params.rewardVideoType != RewardVideoType.NONE)
            {
                ShowRewardAcquirePopup(true);
            }
		}
	}
    
    /** 보상 획득 팝업을 출력한다 */
	public void ShowRewardAcquirePopup(bool a_bIsWatchRewardAds) {

        this.Close();

		Func.ShowRewardAcquirePopup(this.transform.parent.gameObject, (a_oSender) => {            
			var oTargetInfoDict = CCollectionManager.Inst.SpawnDict<ulong, STTargetInfo>();

			try {
                switch(Params.rewardVideoType)
                {
                    case RewardVideoType.BALLOON_BOOSTER:
                        CGameInfoStorage.Inst.GetRewardBooster(RewardVideoType.BALLOON_BOOSTER);
                        CSceneManager.GetSceneManager<MainScene.CSubMainSceneManager>(KCDefine.B_SCENE_N_MAIN).RewardBalloon_Hide();

                        (a_oSender as CRewardAcquirePopup).Init(CRewardAcquirePopup.MakeParams(KDefine.L_SCENE_N_MAIN, KCDefine.B_VAL_0_INT, ERewardKinds.ADS_REWARD_BALLOON_BOOSTER, EItemKinds.BOOSTER_ITEM_04_RANDOM, KCDefine.B_VAL_0_INT, false, 
                        () => {  }, this));
                        break;
                    case RewardVideoType.READY_BOOSTER:
                        var parentPopup = Params.parentPopup as CPreviewPopup;
                        parentPopup.GetRewardBooster();

                        (a_oSender as CRewardAcquirePopup).Init(CRewardAcquirePopup.MakeParams(KDefine.L_SCENE_N_MAIN, KCDefine.B_VAL_0_INT, ERewardKinds.ADS_REWARD_READY_BOOSTER, EItemKinds.BOOSTER_ITEM_01_MISSILE + parentPopup.rewardBoosterIndex_ready, KCDefine.B_VAL_0_INT, false, 
                        () => { parentPopup.RefreshBoosterState(); }, this));
                        break;
                    case RewardVideoType.INGAME_ADDBALLS:
                        var Engine = CSceneManager.GetSceneManager<GameScene.CSubGameSceneManager>(KCDefine.B_SCENE_N_GAME).Engine;

                        Engine.AddNormalBalls(Engine.startPosition, Params.count, false);
                        Engine.BallObjList[0].NumText.text = GlobalDefine.GetBallText(Engine.BallObjList.Count - Engine.DeleteBallList.Count, Engine.DeleteBallList.Count);
                        
                        (a_oSender as CRewardAcquirePopup).Init(CRewardAcquirePopup.MakeParams(KDefine.L_SCENE_N_PLAY, 0, ERewardKinds.ADS_REWARD_INGAME_ADDBALLS, EItemKinds.GAME_ITEM_02_ADD_BALLS, Params.count, false, 
                        () => { CSceneManager.GetSceneManager<GameScene.CSubGameSceneManager>(KCDefine.B_SCENE_N_GAME).rewardBallPlusButton.interactable = false; }, this));
                        break;
                }
                
			} finally {
				CCollectionManager.Inst.DespawnDict(oTargetInfoDict);
			}
		}, null, null);
	}

#endif // #if ADS_MODULE_ENABLE

#endregion Reward Ads Button


	#endregion // 함수

	#region 클래스 함수
	public static STParams MakeParams(string _sceneName, int _currentLevel, RewardVideoType _rewardVideoType, ERewardKinds _rewardKinds, EItemKinds _kinds, int _count, string _description, System.Action _m_oCallback, CPopup _parentPopup) {
		return new STParams() {
			sceneName = _sceneName, currentLevel = _currentLevel, rewardVideoType = _rewardVideoType, rewardKinds = _rewardKinds, 
            kinds = _kinds, count = _count, description = _description, m_oCallback = _m_oCallback, parentPopup = _parentPopup
		};
	}
	#endregion // 클래스 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

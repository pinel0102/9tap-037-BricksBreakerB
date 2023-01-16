#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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
		public ERewardQuality m_eQuality;
		public ERewardAcquirePopupType m_eAgreePopup;

		public Dictionary<ulong, STTargetInfo> m_oRewardTargetInfoDict;
	}

	#region 변수
	/** =====> UI <===== */
	private Dictionary<EKey, Button> m_oBtnDict = new Dictionary<EKey, Button>();

	/** =====> 객체 <===== */
	[SerializeField] private GameObject m_oRewardUIs = null;
	[SerializeField] private List<GameObject> m_oItemUIsList = new List<GameObject>();
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
		var oRewardTargetInfoKeyList = this.Params.m_oRewardTargetInfoDict.Keys.ToList();

		// 보상 아이템 UI 상태를 갱신한다
		for(int i = 0; i < m_oItemUIsList.Count; ++i) {
			m_oItemUIsList[i].SetActive(i < oRewardTargetInfoKeyList.Count);

			// 보상 정보가 존재 할 경우
			if(i < oRewardTargetInfoKeyList.Count) {
				ulong nUniqueTargetInfoID = oRewardTargetInfoKeyList[i];
				this.UpdateItemUIsState(m_oItemUIsList[i], this.Params.m_oRewardTargetInfoDict.GetValueOrDefault(nUniqueTargetInfoID));
			}
		}

		this.SubUpdateUIsState();
	}

	/** 광고 버튼을 눌렀을 경우 */
	private void OnTouchAdsBtn() {
#if ADS_MODULE_ENABLE
		Func.ShowRewardAds(this.OnCloseRewardAds);
#endif // #if ADS_MODULE_ENABLE
	}

	/** 획득 버튼을 눌렀을 경우 */
	private void OnTouchAcquireBtn() {
		this.AcquireRewards(false);
	}

	/** 보상을 획득한다 */
	private void AcquireRewards(bool a_bIsWatchRewardAds) {
		m_oBtnDict[EKey.ADS_BTN]?.ExSetInteractable(false);
		m_oBtnDict[EKey.ACQUIRE_BTN]?.ExSetInteractable(false);

#if ADS_MODULE_ENABLE
		m_oBtnDict[EKey.ADS_BTN]?.gameObject.ExRemoveComponent<CRewardAdsTouchInteractable>();
#endif // #if ADS_MODULE_ENABLE

		var oRewardTargetInfoDict = new Dictionary<ulong, STTargetInfo>();

		foreach(var stKeyVal in this.Params.m_oRewardTargetInfoDict) {
			var stValInfo = new STValInfo(a_bIsWatchRewardAds ? stKeyVal.Value.m_stValInfo01.m_dmVal * KCDefine.B_VAL_2_INT : stKeyVal.Value.m_stValInfo01.m_dmVal, stKeyVal.Value.m_stValInfo01.m_eValType);
			oRewardTargetInfoDict.TryAdd(stKeyVal.Key, new STTargetInfo(stKeyVal.Value.m_eTargetKinds, stKeyVal.Value.m_nKinds, stValInfo));
		}

		Func.Acquire(CGameInfoStorage.Inst.PlayCharacterID, this.Params.m_oRewardTargetInfoDict, true);
		this.OnTouchCloseBtn();
	}
	#endregion // 함수

	#region 클래스 함수
	/** 매개 변수를 생성한다 */
	public static STParams MakeParams(ERewardQuality a_eQuality, ERewardAcquirePopupType a_eAgreePopup, Dictionary<ulong, STTargetInfo> a_oRewardTargetInfoDict) {
		return new STParams() {
			m_eQuality = a_eQuality, m_eAgreePopup = a_eAgreePopup, m_oRewardTargetInfoDict = a_oRewardTargetInfoDict
		};
	}
	#endregion // 클래스 함수

	#region 조건부 함수
#if ADS_MODULE_ENABLE
	/** 보상 광고가 닫혔을 경우 */
	private void OnCloseRewardAds(CAdsManager a_oSender, STAdsRewardInfo a_stAdsRewardInfo, bool a_bIsSuccess) {
		// 광고를 시청했을 경우
		if(a_bIsSuccess) {
			this.AcquireRewards(true);
		}
	}
#endif // #if ADS_MODULE_ENABLE
	#endregion // 조건부 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

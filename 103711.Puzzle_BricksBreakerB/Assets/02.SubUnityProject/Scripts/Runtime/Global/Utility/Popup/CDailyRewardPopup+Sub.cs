using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 일일 보상 팝업 */
public partial class CDailyRewardPopup : CSubPopup {
#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
			(EKey.ADS_BTN, $"{EKey.ADS_BTN}", this.Contents, this.OnTouchAdsBtn),
			(EKey.ACQUIRE_BTN, $"{EKey.ACQUIRE_BTN}", this.Contents, this.OnTouchAcquireBtn)
		}, m_oBtnDict);

#region 추가
		this.SubSetupAwake();
#endregion // 추가
	}
	
	/** 초기화 */
	public override void Init() {
		base.Init();

#region 추가
		this.SubInit();
#endregion // 추가
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {
		// 버튼을 갱신한다
		m_oBtnDict.GetValueOrDefault(EKey.ADS_BTN)?.ExSetInteractable(Access.IsEnableGetDailyReward(CGameInfoStorage.Inst.PlayCharacterID));
		m_oBtnDict.GetValueOrDefault(EKey.ACQUIRE_BTN)?.ExSetInteractable(Access.IsEnableGetDailyReward(CGameInfoStorage.Inst.PlayCharacterID));
		
		// 보상 UI 상태를 갱신한다
		for(int i = 0; i < m_oRewardUIsList.Count; ++i) {
			var oRewardUIs = m_oRewardUIsList[i];
			var stDailyRewardInfo = CRewardInfoTable.Inst.GetRewardInfo(ERewardKinds.DAILY_REWARD_SAMPLE + (i + KCDefine.B_VAL_1_INT));

			this.UpdateRewardUIsState(oRewardUIs, stDailyRewardInfo);
		}

#region 추가
		this.SubUpdateUIsState();
#endregion // 추가
	}
#endregion // 함수
}

/** 서브 일일 보상 팝업 */
public partial class CDailyRewardPopup : CSubPopup {
	/** 서브 식별자 */
	private enum ESubKey {
		NONE = -1,
		[HideInInspector] MAX_VAL
	}

#region 변수

#endregion // 변수

#region 프로퍼티

#endregion // 프로퍼티

#region 함수
	/** 팝업을 설정한다 */
	private void SubSetupAwake() {
		// Do Something
	}

	/** 초기화 */
	private void SubInit() {
		// Do Something
	}

	/** UI 상태를 갱신한다 */
	private void SubUpdateUIsState() {
		// Do Something
	}
	
	/** 보상 UI 상태를 갱신한다 */
	private void UpdateRewardUIsState(GameObject a_oRewardUIs, STRewardInfo a_stRewardInfo) {
		// Do Something
	}
#endregion // 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

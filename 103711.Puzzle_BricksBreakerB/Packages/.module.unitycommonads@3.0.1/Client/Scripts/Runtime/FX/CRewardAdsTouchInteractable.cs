using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if ADS_MODULE_ENABLE
/** 보상 광고 상호 작용 처리자 */
public partial class CRewardAdsTouchInteractable : CComponent {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		UPDATE_SKIP_TIME,
		TOUCH_INTERACTABLE,
		[HideInInspector] MAX_VAL
	}

	#region 변수
	private Dictionary<EKey, float> m_oRealDict = new Dictionary<EKey, float>() {
		[EKey.UPDATE_SKIP_TIME] = KCDefine.B_VAL_0_REAL
	};

	private Dictionary<EKey, CTouchInteractable> m_oTouchInteractableDict = new Dictionary<EKey, CTouchInteractable>();
	[SerializeField] private EAdsPlatform m_eAdsPlatform = EAdsPlatform.NONE;
	#endregion // 변수

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		CScheduleManager.Inst.AddComponent(this);

		// 터치 상호 작용 처리자를 설정한다
		CFunc.SetupComponents(new List<(EKey, GameObject)>() {
			(EKey.TOUCH_INTERACTABLE, this.gameObject)
		}, m_oTouchInteractableDict);
	}

	/** 제거 되었을 경우 */
	public override void OnDestroy() {
		base.OnDestroy();

		try {
			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				CScheduleManager.Inst.RemoveComponent(this);
			}
		} catch(System.Exception oException) {
			CFunc.ShowLogWarning($"CRewardAdsTouchInteractable.OnDestroy Exception: {oException.Message}");
		}
	}

	/** 상태를 갱신한다 */
	public override void OnLateUpdate(float a_fDeltaTime) {
		base.OnLateUpdate(a_fDeltaTime);

		// 앱이 실행 중 일 경우
		if(CSceneManager.IsAppRunning) {
			m_oRealDict[EKey.UPDATE_SKIP_TIME] += CScheduleManager.Inst.UnscaleDeltaTime;

			// 상호 작용 갱신 주기가 지났을 경우
			if(m_oRealDict[EKey.UPDATE_SKIP_TIME].ExIsGreateEquals(KCDefine.U_DELTA_T_REWARD_ATI_UPDATE)) {
				m_oRealDict[EKey.UPDATE_SKIP_TIME] = KCDefine.B_VAL_0_REAL;
				m_oTouchInteractableDict[EKey.TOUCH_INTERACTABLE].SetInteractable(CAdsManager.Inst.IsLoadRewardAds(m_eAdsPlatform.ExIsValid() ? m_eAdsPlatform : CCommonAppInfoStorage.Inst.AdsPlatform));
			}
		}
	}

	/** 광고 플랫폼을 변경한다 */
	public void SetAdsPlatform(EAdsPlatform a_eType) {
		m_eAdsPlatform = a_eType;
	}
	#endregion // 함수
}
#endif // #if ADS_MODULE_ENABLE

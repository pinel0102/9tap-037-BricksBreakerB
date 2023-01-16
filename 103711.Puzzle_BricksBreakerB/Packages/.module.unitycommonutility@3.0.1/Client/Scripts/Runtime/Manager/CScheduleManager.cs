using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Timers;

/** 스케줄 관리자 */
public partial class CScheduleManager : CSingleton<CScheduleManager> {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		DELTA_TIME,
		UPDATE_SKIP_TIME,
		UNSCALE_DELTA_TIME,
		[HideInInspector] MAX_VAL
	}

	#region 변수
	private Dictionary<EKey, float> m_oRealDict = new Dictionary<EKey, float>() {
		[EKey.DELTA_TIME] = KCDefine.B_VAL_0_REAL,
		[EKey.UPDATE_SKIP_TIME] = KCDefine.B_VAL_0_REAL,
		[EKey.UNSCALE_DELTA_TIME] = KCDefine.B_VAL_0_REAL
	};

	private CListWrapper<STCallbackInfo> m_oCallbackInfoListWrapper = new CListWrapper<STCallbackInfo>();
	private CListWrapper<STComponentInfo> m_oComponentInfoListWrapper = new CListWrapper<STComponentInfo>();
	#endregion // 변수

	#region 프로퍼티
	public float DeltaTime => m_oRealDict[EKey.DELTA_TIME];
	public float UnscaleDeltaTime => m_oRealDict[EKey.UNSCALE_DELTA_TIME];
	#endregion // 프로퍼티

	#region 함수
	/** 상태를 리셋한다 */
	public override void Reset() {
		base.Reset();

		m_oCallbackInfoListWrapper.m_oList01.Clear();
		m_oCallbackInfoListWrapper.m_oList02.Clear();
		m_oCallbackInfoListWrapper.m_oList03.Clear();

		m_oComponentInfoListWrapper.m_oList01.Clear();
		m_oComponentInfoListWrapper.m_oList02.Clear();
		m_oComponentInfoListWrapper.m_oList03.Clear();
	}

	/** 제거 되었을 경우 */
	public override void OnDestroy() {
		base.OnDestroy();

		try {
			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				CSceneManager.SetScheduleManager(null);
			}
		} catch(System.Exception oException) {
			CFunc.ShowLogWarning($"CScheduleManager.OnDestroy Exception: {oException.Message}");
		}
	}

	/** 상태를 갱신한다 */
	public virtual void Update() {
		this.UpdateComponentInfosState();
		float fMaxDeltaTime = Mathf.Clamp01(KCDefine.B_VAL_1_REAL / Application.targetFrameRate);

		m_oRealDict[EKey.DELTA_TIME] = Mathf.Clamp(Time.deltaTime, KCDefine.B_VAL_0_REAL, fMaxDeltaTime);
		m_oRealDict[EKey.UNSCALE_DELTA_TIME] = Mathf.Clamp(Time.unscaledDeltaTime, KCDefine.B_VAL_0_REAL, fMaxDeltaTime);

		for(int i = 0; i < m_oComponentInfoListWrapper.m_oList01.Count; ++i) {
			var oComponent = m_oComponentInfoListWrapper.m_oList01[i].m_oComponent as CComponent;

			// 상태 갱신이 가능 할 경우
			if(oComponent != null && (oComponent.enabled && !oComponent.IsDestroy && oComponent.gameObject.activeInHierarchy)) {
				oComponent.OnUpdate(m_oRealDict[EKey.DELTA_TIME]);
			}
		}
	}

	/** 상태를 갱신한다 */
	public virtual void LateUpdate() {
		m_oRealDict[EKey.UPDATE_SKIP_TIME] += m_oRealDict[EKey.UNSCALE_DELTA_TIME];

		for(int i = 0; i < m_oComponentInfoListWrapper.m_oList01.Count; ++i) {
			var oComponent = m_oComponentInfoListWrapper.m_oList01[i].m_oComponent as CComponent;

			// 제거 되었을 경우
			if(oComponent == null || oComponent.IsDestroy) {
				i -= KCDefine.B_VAL_1_INT;
				m_oComponentInfoListWrapper.m_oList01.ExRemoveValAt(i + KCDefine.B_VAL_1_INT);
			}
			// 상태 갱신이 가능 할 경우
			else if(oComponent.enabled && oComponent.gameObject.activeInHierarchy) {
				oComponent.OnLateUpdate(m_oRealDict[EKey.DELTA_TIME]);
			}
		}

		// 콜백 호출 주기가 지났을 경우
		if(m_oRealDict[EKey.UPDATE_SKIP_TIME].ExIsGreateEquals(KCDefine.U_DELTA_T_SCHEDULE_M_CALLBACK)) {
			m_oRealDict[EKey.UPDATE_SKIP_TIME] = KCDefine.B_VAL_0_REAL;

			lock(KCDefine.U_LOCK_OBJ_SCHEDULE_M_UPDATE) {
				this.UpdateCallbackInfosState();

				try {
					for(int i = 0; i < m_oCallbackInfoListWrapper.m_oList01.Count; ++i) {
						m_oCallbackInfoListWrapper.m_oList01[i].m_oCallback?.Invoke();
					}
				} finally {
					m_oCallbackInfoListWrapper.m_oList01.Clear();
				}
			}
		}
	}

	/** 콜백을 추가한다 */
	public void AddCallback(string a_oKey, System.Action a_oCallback) {
		lock(KCDefine.U_LOCK_OBJ_SCHEDULE_M_UPDATE) {
			int nIdx = m_oCallbackInfoListWrapper.m_oList01.FindIndex((a_stCallbackInfo) => a_stCallbackInfo.m_oKey.Equals(a_oKey));

			// 콜백이 없을 경우
			if(!m_oCallbackInfoListWrapper.m_oList01.ExIsValidIdx(nIdx)) {
				m_oCallbackInfoListWrapper.m_oList02.ExAddVal(new STCallbackInfo() {
					m_oKey = a_oKey,
					m_oCallback = a_oCallback
				});
			}
		}
	}

	/** 컴포넌트를 추가한다 */
	public void AddComponent(CComponent a_oComponent) {
		int nID = a_oComponent.GetInstanceID();
		int nIdx = m_oComponentInfoListWrapper.m_oList01.FindIndex((a_stComponentInfo) => nID == a_stComponentInfo.m_nID);

		// 컴포넌트가 없을 경우
		if(!m_oComponentInfoListWrapper.m_oList01.ExIsValidIdx(nIdx)) {
			m_oComponentInfoListWrapper.m_oList02.ExAddVal(new STComponentInfo() {
				m_nID = nID,
				m_oComponent = a_oComponent
			});
		}
	}

	/** 타이머를 추가한다 */
	public void AddTimer(CComponent a_oComponent, float a_fDeltaTime, uint a_nRepeatTimes, UnityAction a_oCallback, bool a_bIsRealtime = false) {
		var eTimerMode = a_bIsRealtime ? TimerMode.REALTIME : TimerMode.NORM;
		TimersManager.SetTimer(a_oComponent, new Timer(a_fDeltaTime, a_nRepeatTimes, a_oCallback, eTimerMode));
	}

	/** 반복 타이머를 추가한다 */
	public void AddRepeatTimer(CComponent a_oComponent, float a_fDeltaTime, UnityAction a_oCallback, bool a_bIsRealtime = false) {
		this.AddTimer(a_oComponent, a_fDeltaTime, Timer.INFINITE, a_oCallback, a_bIsRealtime);
	}

	/** 콜백을 제거한다 */
	public void RemoveCallback(string a_oKey) {
		lock(KCDefine.U_LOCK_OBJ_SCHEDULE_M_UPDATE) {
			int nIdx = m_oCallbackInfoListWrapper.m_oList01.FindIndex((a_stCallbackInfo) => a_stCallbackInfo.m_oKey.Equals(a_oKey));

			// 콜백이 존재 할 경우
			if(m_oCallbackInfoListWrapper.m_oList01.ExIsValidIdx(nIdx)) {
				m_oCallbackInfoListWrapper.m_oList03.ExAddVal(new STCallbackInfo() {
					m_oKey = a_oKey,
					m_oCallback = null
				});
			}
		}
	}

	/** 컴포넌트를 제거한다 */
	public void RemoveComponent(CComponent a_oComponent) {
		int nID = a_oComponent.GetInstanceID();
		int nIdx = m_oComponentInfoListWrapper.m_oList01.FindIndex((a_stComponentInfo) => nID == a_stComponentInfo.m_nID);

		// 컴포넌트가 존재 할 경우
		if(m_oComponentInfoListWrapper.m_oList01.ExIsValidIdx(nIdx)) {
			m_oComponentInfoListWrapper.m_oList03.ExAddVal(new STComponentInfo() {
				m_nID = nID,
				m_oComponent = a_oComponent
			});
		}
	}

	/** 타이머를 제거한다 */
	public void RemoveTimer(CComponent a_oComponent) {
		TimersManager.ClearTimer(new System.WeakReference(a_oComponent));
	}

	/** 타이머를 제거한다 */
	public void RemoveTimer(UnityAction a_oCallback) {
		TimersManager.ClearTimer(a_oCallback);
	}

	/** 컴포넌트 정보 상태를 갱신한다 */
	private void UpdateComponentInfosState() {
		for(int i = 0; i < m_oComponentInfoListWrapper.m_oList02.Count; ++i) {
			m_oComponentInfoListWrapper.m_oList01.ExAddVal(m_oComponentInfoListWrapper.m_oList02[i]);
			(m_oComponentInfoListWrapper.m_oList02[i].m_oComponent as CComponent).SetScheduleCallback(this.OnReceiveScheduleCallback);
		}

		for(int i = 0; i < m_oComponentInfoListWrapper.m_oList03.Count; ++i) {
			m_oComponentInfoListWrapper.m_oList01.ExRemoveVal((a_stComponentInfo) => a_stComponentInfo.m_nID == m_oComponentInfoListWrapper.m_oList03[i].m_nID);
			(m_oComponentInfoListWrapper.m_oList03[i].m_oComponent as CComponent).SetScheduleCallback(null);
		}

		m_oComponentInfoListWrapper.m_oList02.Clear();
		m_oComponentInfoListWrapper.m_oList03.Clear();
	}

	/** 콜백 정보 상태를 갱신한다 */
	private void UpdateCallbackInfosState() {
		for(int i = 0; i < m_oCallbackInfoListWrapper.m_oList02.Count; ++i) {
			m_oCallbackInfoListWrapper.m_oList01.ExAddVal(m_oCallbackInfoListWrapper.m_oList02[i]);
		}

		for(int i = 0; i < m_oCallbackInfoListWrapper.m_oList03.Count; ++i) {
			var stCallbackInfo = m_oCallbackInfoListWrapper.m_oList03[i];
			m_oCallbackInfoListWrapper.m_oList01.ExRemoveVal((a_stCallbackInfo) => a_stCallbackInfo.m_oKey.Equals(stCallbackInfo.m_oKey));
		}

		m_oCallbackInfoListWrapper.m_oList02.Clear();
		m_oCallbackInfoListWrapper.m_oList03.Clear();
	}

	/** 스케쥴 콜백을 수신했을 경우 */
	private void OnReceiveScheduleCallback(CComponent a_oSender) {
		this.RemoveComponent(a_oSender);
	}
	#endregion // 함수
}

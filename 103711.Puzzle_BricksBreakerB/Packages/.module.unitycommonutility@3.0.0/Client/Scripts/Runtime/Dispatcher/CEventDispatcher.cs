using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** 이벤트 전달자 */
public partial class CEventDispatcher : CComponent {
	/** 콜백 */
	private enum ECallback {
		NONE,
		ANI_EVENT,
		PARTICLE_EVENT,
		[HideInInspector] MAX_VAL
	}

	#region 변수
	private Dictionary<ECallback, System.Action<CEventDispatcher>> m_oCallbackDict01 = new Dictionary<ECallback, System.Action<CEventDispatcher>>();
	private Dictionary<ECallback, System.Action<CEventDispatcher, string>> m_oCallbackDict02 = new Dictionary<ECallback, System.Action<CEventDispatcher, string>>();
	#endregion // 변수

	#region 함수
	/** 애니메이션 이벤트를 수신했을 경우 */
	public void OnReceiveAniEvent(string a_oEvent) {
		m_oCallbackDict02.GetValueOrDefault(ECallback.ANI_EVENT)?.Invoke(this, a_oEvent);
	}

	/** 파티클 이벤트를 수신했을 경우 */
	public void OnParticleSystemStopped() {
		m_oCallbackDict01.GetValueOrDefault(ECallback.PARTICLE_EVENT)?.Invoke(this);
	}
	#endregion // 함수

	#region 접근자 함수
	/** 애니메이션 콜백을 변경한다 */
	public void SetAniCallback(System.Action<CEventDispatcher, string> a_oCallback) {
		m_oCallbackDict02.ExReplaceVal(ECallback.ANI_EVENT, a_oCallback);
	}

	/** 파티클 콜백을 변경한다 */
	public void SetParticleCallback(System.Action<CEventDispatcher> a_oCallback) {
		m_oCallbackDict01.ExReplaceVal(ECallback.PARTICLE_EVENT, a_oCallback);
	}
	#endregion // 접근자 함수
}

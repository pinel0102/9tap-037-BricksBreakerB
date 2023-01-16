using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** 접촉 전달자 */
public partial class CTriggerDispatcher : CComponent {
	/** 콜백 */
	private enum ECallback {
		NONE = -1,
		ENTER,
		STAY,
		EXIT,
		[HideInInspector] MAX_VAL
	}

	#region 변수
	private Dictionary<ECallback, System.Action<CTriggerDispatcher, Collider>> m_oCallbackDict = new Dictionary<ECallback, System.Action<CTriggerDispatcher, Collider>>();
	#endregion // 변수

	#region 함수
	/** 충돌을 시작했을 경우 */
	public void OnTriggerEnter(Collider a_oCollider) {
		m_oCallbackDict.GetValueOrDefault(ECallback.ENTER)?.Invoke(this, a_oCollider);
	}

	/** 충돌을 진행 중 일 경우 */
	public void OnTriggerStay(Collider a_oCollider) {
		m_oCallbackDict.GetValueOrDefault(ECallback.STAY)?.Invoke(this, a_oCollider);
	}

	/** 충돌을 종료했을 경우 */
	public void OnTriggerExit(Collider a_oCollider) {
		m_oCallbackDict.GetValueOrDefault(ECallback.EXIT)?.Invoke(this, a_oCollider);
	}
	#endregion // 함수
}

/** 접촉 전달자 - 접근 */
public partial class CTriggerDispatcher : CComponent {
	#region 함수
	/** 시작 콜백을 변경한다 */
	public void SetEnterCallback(System.Action<CTriggerDispatcher, Collider> a_oCallback) {
		m_oCallbackDict.ExReplaceVal(ECallback.ENTER, a_oCallback);
	}

	/** 진행 콜백을 변경한다 */
	public void SetStayCallback(System.Action<CTriggerDispatcher, Collider> a_oCallback) {
		m_oCallbackDict.ExReplaceVal(ECallback.STAY, a_oCallback);
	}

	/** 종료 콜백을 변경한다 */
	public void SetExitCallback(System.Action<CTriggerDispatcher, Collider> a_oCallback) {
		m_oCallbackDict.ExReplaceVal(ECallback.EXIT, a_oCallback);
	}
	#endregion // 함수
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** 충돌 전달자 */
public partial class CCollisionDispatcher : CComponent {
	/** 콜백 */
	private enum ECallback {
		NONE = -1,
		ENTER,
		STAY,
		EXIT,
		[HideInInspector] MAX_VAL
	}

	#region 변수
	private Dictionary<ECallback, System.Action<CCollisionDispatcher, Collision>> m_oCallbackDict = new Dictionary<ECallback, System.Action<CCollisionDispatcher, Collision>>();
	#endregion // 변수

	#region 함수
	/** 충돌을 시작했을 경우 */
	public void OnCollisionEnter(Collision a_oCollision) {
		m_oCallbackDict.GetValueOrDefault(ECallback.ENTER)?.Invoke(this, a_oCollision);
	}

	/** 충돌을 진행 중 일 경우 */
	public void OnCollisionStay(Collision a_oCollision) {
		m_oCallbackDict.GetValueOrDefault(ECallback.STAY)?.Invoke(this, a_oCollision);
	}

	/** 충돌을 종료했을 경우 */
	public void OnCollisionExit(Collision a_oCollision) {
		m_oCallbackDict.GetValueOrDefault(ECallback.EXIT)?.Invoke(this, a_oCollision);
	}
	#endregion // 함수
}

/** 충돌 전달자 - 접근 */
public partial class CCollisionDispatcher : CComponent {
	#region 함수
	/** 시작 콜백을 변경한다 */
	public void SetEnterCallback(System.Action<CCollisionDispatcher, Collision> a_oCallback) {
		m_oCallbackDict.ExReplaceVal(ECallback.ENTER, a_oCallback);
	}

	/** 진행 콜백을 변경한다 */
	public void SetStayCallback(System.Action<CCollisionDispatcher, Collision> a_oCallback) {
		m_oCallbackDict.ExReplaceVal(ECallback.STAY, a_oCallback);
	}

	/** 종료 콜백을 변경한다 */
	public void SetExitCallback(System.Action<CCollisionDispatcher, Collision> a_oCallback) {
		m_oCallbackDict.ExReplaceVal(ECallback.EXIT, a_oCallback);
	}
	#endregion // 함수
}

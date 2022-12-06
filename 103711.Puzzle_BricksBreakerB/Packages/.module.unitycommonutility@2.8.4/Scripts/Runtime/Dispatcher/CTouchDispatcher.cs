using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/** 터치 전달자 */
public partial class CTouchDispatcher : CComponent, IPointerDownHandler, IDragHandler, IPointerUpHandler {
	/** 콜백 */
	private enum ECallback {
		NONE = -1,
		BEGIN,
		MOVE,
		END,
		[HideInInspector] MAX_VAL
	}

	#region 변수
	private Dictionary<ECallback, System.Action<CTouchDispatcher, PointerEventData>> m_oCallbackDict = new Dictionary<ECallback, System.Action<CTouchDispatcher, PointerEventData>>();
	#endregion // 변수

	#region IPointerDownHandler
	/** 터치를 시작했을 경우 */
	public virtual void OnPointerDown(PointerEventData a_oEventData) {
		m_oCallbackDict.GetValueOrDefault(ECallback.BEGIN)?.Invoke(this, a_oEventData);
	}
	#endregion // IPointerDownHandler

	#region IDragHandler
	/** 터치를 이동했을 경우 */
	public virtual void OnDrag(PointerEventData a_oEventData) {
		m_oCallbackDict.GetValueOrDefault(ECallback.MOVE)?.Invoke(this, a_oEventData);
	}
	#endregion // IDragHandler

	#region IPointerUpHandler
	/** 터치를 종료했을 경우 */
	public virtual void OnPointerUp(PointerEventData a_oEventData) {
		m_oCallbackDict.GetValueOrDefault(ECallback.END)?.Invoke(this, a_oEventData);
	}
	#endregion // IPointerUpHandler

	#region 접근자 함수
	/** 시작 콜백을 변경한다 */
	public void SetBeginCallback(System.Action<CTouchDispatcher, PointerEventData> a_oCallback) {
		m_oCallbackDict.ExReplaceVal(ECallback.BEGIN, a_oCallback);
	}

	/** 이동 콜백을 변경한다 */
	public void SetMoveCallback(System.Action<CTouchDispatcher, PointerEventData> a_oCallback) {
		m_oCallbackDict.ExReplaceVal(ECallback.MOVE, a_oCallback);
	}

	/** 종료 콜백을 변경한다 */
	public void SetEndCallback(System.Action<CTouchDispatcher, PointerEventData> a_oCallback) {
		m_oCallbackDict.ExReplaceVal(ECallback.END, a_oCallback);
	}
	#endregion // 접근자 함수
}

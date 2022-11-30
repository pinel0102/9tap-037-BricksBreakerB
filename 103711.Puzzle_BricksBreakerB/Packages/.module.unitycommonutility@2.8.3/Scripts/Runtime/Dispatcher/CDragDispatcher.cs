using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/** 드래그 전달자 */
public partial class CDragDispatcher : CComponent, IBeginDragHandler, IDragHandler, IEndDragHandler, IScrollHandler {
	/** 콜백 */
	private enum ECallback {
		NONE = -1,
		BEGIN,
		DRAG,
		END,
		SCROLL,
		[HideInInspector] MAX_VAL
	}

	#region 변수
	private Dictionary<ECallback, System.Action<CDragDispatcher, PointerEventData>> m_oCallbackDict = new Dictionary<ECallback, System.Action<CDragDispatcher, PointerEventData>>();
	#endregion // 변수

	#region IBeginDragHandler
	/** 드래그를 시작했을 경우 */
	public virtual void OnBeginDrag(PointerEventData a_oEventData) {
		m_oCallbackDict.GetValueOrDefault(ECallback.BEGIN)?.Invoke(this, a_oEventData);
	}
	#endregion // IBeginDragHandler

	#region IDragHandler
	/** 드래그 중 일 경우 */
	public virtual void OnDrag(PointerEventData a_oEventData) {
		m_oCallbackDict.GetValueOrDefault(ECallback.DRAG)?.Invoke(this, a_oEventData);
	}
	#endregion // IDragHandler

	#region IEndDragHandler
	/** 드래그를 종료했을 경우 */
	public virtual void OnEndDrag(PointerEventData a_oEventData) {
		m_oCallbackDict.GetValueOrDefault(ECallback.END)?.Invoke(this, a_oEventData);
	}
	#endregion // IEndDragHandler

	#region IScrollHandler
	/** 스크롤 중 일 경우 */
	public virtual void OnScroll(PointerEventData a_oEventData) {
		m_oCallbackDict.GetValueOrDefault(ECallback.SCROLL)?.Invoke(this, a_oEventData);
	}
	#endregion // IScrollHandler

	#region 접근자 함수
	/** 시작 콜백을 변경한다 */
	public void SetBeginCallback(System.Action<CDragDispatcher, PointerEventData> a_oCallback) {
		m_oCallbackDict.ExReplaceVal(ECallback.BEGIN, a_oCallback);
	}

	/** 드래그 콜백을 변경한다 */
	public void SetDragCallback(System.Action<CDragDispatcher, PointerEventData> a_oCallback) {
		m_oCallbackDict.ExReplaceVal(ECallback.DRAG, a_oCallback);
	}

	/** 종료 콜백을 변경한다 */
	public void SetEndCallback(System.Action<CDragDispatcher, PointerEventData> a_oCallback) {
		m_oCallbackDict.ExReplaceVal(ECallback.END, a_oCallback);
	}

	/** 스크롤 콜백을 변경한다 */
	public void SetScrollCallback(System.Action<CDragDispatcher, PointerEventData> a_oCallback) {
		m_oCallbackDict.ExReplaceVal(ECallback.SCROLL, a_oCallback);
	}
	#endregion // 접근자 함수
}

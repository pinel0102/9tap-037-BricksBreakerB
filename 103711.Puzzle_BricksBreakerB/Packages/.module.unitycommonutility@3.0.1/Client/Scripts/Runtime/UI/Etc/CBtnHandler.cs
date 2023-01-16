using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/** 버튼 처리자 */
public class CBtnHandler : CComponent, IPointerClickHandler {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		BTN,
		[HideInInspector] MAX_VAL
	};

	#region 변수
	private Dictionary<EKey, Button> m_oBtnDict = new Dictionary<EKey, Button>();
	#endregion // 변수

	#region IPointerClickHandler
	/** 클릭했을 경우 */
	public void OnPointerClick(PointerEventData a_oEventData) {
		/** 마우스 오른쪽 버튼 일 경우 */
		if(a_oEventData.button == PointerEventData.InputButton.Right) {
			m_oBtnDict[EKey.BTN]?.onClick.Invoke();
		}
	}
	#endregion // IPointerClickHandler

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 버튼을 설정한다
		CFunc.SetupComponents(new List<(EKey, GameObject)>() {
			(EKey.BTN, this.gameObject)
		}, m_oBtnDict);
	}
	#endregion // 함수
}

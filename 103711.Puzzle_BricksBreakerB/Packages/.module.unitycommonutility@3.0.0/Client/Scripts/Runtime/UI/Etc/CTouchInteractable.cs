using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** 터치 상호 작용 처리자 */
public partial class CTouchInteractable : CComponent {
	#region 함수
	/** 상호 작용 여부를 변경한다 */
	public void SetInteractable(bool a_bIsEnable) {
		this.gameObject.ExSetInteractable<Button>(a_bIsEnable, false);
		this.gameObject.ExSetEnableComponent<CTouchScaler>(a_bIsEnable, false);
		this.gameObject.ExSetEnableComponent<CTouchSndPlayer>(a_bIsEnable, false);
	}
	#endregion // 함수
}

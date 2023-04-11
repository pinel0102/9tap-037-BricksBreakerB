using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 정지 팝업 */
public partial class CPreviewPopup : CSubPopup {
	#region 함수

	#endregion // 함수
}

/** 서브 정지 팝업 */
public partial class CPreviewPopup : CSubPopup {
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
	private void SubAwake() {
		// Do Something
	}

	/** 초기화한다 */
	private void SubInit() {
		// Do Something
	}

	/** UI 상태를 갱신한다 */
	private void SubUpdateUIsState() {
		// Do Something
	}
	#endregion // 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

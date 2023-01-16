using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 서브 엔진 열거형 값 */
	public static partial class KEngineEnumVal {
		#region 기본

		#endregion // 기본
	}

	#region 기본
	/** 엔진 이벤트 */
	public enum EEngineEvent {
		NONE,
		[HideInInspector] MAX_VAL
	}

	/** 엔진 객체 이벤트 */
	public enum EEngineObjEvent {
		NONE,
		AVOID,
		DAMAGE,
		CRITICAL_DAMAGE,

		DESTROY,
		MOVE_COMPLETE,
		[HideInInspector] MAX_VAL
	}
	#endregion // 기본
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 열거형 값 */
public static partial class KEnumVal {
#region 기본

#endregion // 기본
}

#region 기본
/** 플레이 모드 */
public enum EPlayMode {
	NONE = -1,
	NORM,
	TUTORIAL,
	TEST,
	[HideInInspector] MAX_VAL
}

/** 로그인 타입 */
public enum ELoginType {
	NONE = -1,
	GUEST,
	APPLE,
	FACEBOOK,
	[HideInInspector] MAX_VAL
}

/** 결제 타입 */
public enum EPurchaseType {
	NONE = -1,
	ADS,
	IN_APP_PURCHASE,
	TARGET,
	[HideInInspector] MAX_VAL
}
#endregion // 기본
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

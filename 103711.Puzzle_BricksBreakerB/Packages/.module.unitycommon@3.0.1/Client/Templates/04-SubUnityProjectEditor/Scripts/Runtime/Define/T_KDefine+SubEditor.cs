#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
/** 서브 에디터 씬 상수 */
public static partial class KDefine {
	#region 기본

	#endregion // 기본
}

/** 서브 레벨 에디터 씬 상수 */
public static partial class KDefine {
	#region 기본

	#endregion // 기본

	#region 런타임 상수
	// 객체 종류
	public static readonly Dictionary<int, List<EObjKinds>> LES_OBJ_KINDS_DICT_CONTAINER = new Dictionary<int, List<EObjKinds>>() {
		[KCDefine.B_VAL_0_INT] = new List<EObjKinds>() {
			EObjKinds.BG_EMPTY_01
		},

		[KCDefine.B_VAL_1_INT] = new List<EObjKinds>() {
			// Do Something
		},

		[KCDefine.B_VAL_2_INT] = new List<EObjKinds>() {
			// Do Something
		},

		[KCDefine.B_VAL_3_INT] = new List<EObjKinds>() {
			// Do Something
		},

		[KCDefine.B_VAL_4_INT] = new List<EObjKinds>() {
			// Do Something
		}
	};
	#endregion // 런타임 상수
}
#endif // #if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
#endif // #if SCRIPT_TEMPLATE_ONLY

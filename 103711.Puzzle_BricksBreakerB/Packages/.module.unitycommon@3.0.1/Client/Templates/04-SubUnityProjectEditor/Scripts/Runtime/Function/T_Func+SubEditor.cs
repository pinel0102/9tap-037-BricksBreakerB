#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
/** 서브 에디터 함수 */
public static partial class Func {
	#region 클래스 함수

	#endregion // 클래스 함수
}

/** 서브 레벨 에디터 씬 함수 */
public static partial class Func {
	#region 클래스 함수

	#endregion // 클래스 함수

	#region 조건부 클래스 함수
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
	/** 에디터 셀 정보를 설정한다 */
	private static void SetupEditorCellInfos(CLevelInfo a_oLevelInfo, CEditorCreateInfo a_oCreateInfo, Dictionary<int, List<Vector3Int>> a_oIdxVDictContainer, Dictionary<int, List<Vector3Int>> a_oIdxHDictContainer) {
		// Do Something
	}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
	#endregion // 조건부 클래스 함수

	#region 조건부 클래스 접근자 함수
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
	/** 에디터 셀 정보 설정 완료 여부를 검사한다 */
	private static bool IsSetupEditorCellInfos(CLevelInfo a_oLevelInfo, CEditorCreateInfo a_oCreateInfo) {
		return true;
	}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
	#endregion // 조건부 클래스 접근자 함수
}
#endif // #if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
#endif // #if SCRIPT_TEMPLATE_ONLY

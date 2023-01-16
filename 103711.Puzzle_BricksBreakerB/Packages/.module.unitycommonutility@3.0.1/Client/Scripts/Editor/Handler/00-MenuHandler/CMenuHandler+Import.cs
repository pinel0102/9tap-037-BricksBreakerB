using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;

/** 메뉴 처리자 - 임포트 */
public static partial class CMenuHandler {
	#region 클래스 함수
	/** DOTween Pro 패키지를 추가한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_IMPORT_BASE + "DOTween Pro Pkgs", false, KCEditorDefine.B_SORTING_O_IMPORT_MENU + 1)]
	public static void ImportDOTweenProPkgs() {
		AssetDatabase.ImportPackage(KCEditorDefine.B_ABS_PKGS_P_DOTWEEN_PRO, true);
	}

	/** Apple Sign In 패키지를 추가한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_IMPORT_BASE + "Apple SignIn Pkgs", false, KCEditorDefine.B_SORTING_O_IMPORT_MENU + 1)]
	public static void ImportAppleSignInPkgs() {
		AssetDatabase.ImportPackage(KCEditorDefine.B_ABS_PKGS_P_APPLE_SIGN_IN, true);
	}

	/** Build Report Tool 패키지를 추가한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_IMPORT_BASE + "Build Report Tool Pkgs", false, KCEditorDefine.B_SORTING_O_IMPORT_MENU + 1)]
	public static void ImportBuildReportToolPkgs() {
		AssetDatabase.ImportPackage(KCEditorDefine.B_ABS_PKGS_P_BUILD_REPORT_TOOL, true);
	}

	/** Odin Inspector 패키지를 추가한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_IMPORT_BASE + "Odin Inspector Pkgs", false, KCEditorDefine.B_SORTING_O_IMPORT_MENU + 1)]
	public static void ImportOdinInspectorPkgs() {
		AssetDatabase.ImportPackage(KCEditorDefine.B_ABS_PKGS_P_ODIN_INSPECTOR, true);
	}
	#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR

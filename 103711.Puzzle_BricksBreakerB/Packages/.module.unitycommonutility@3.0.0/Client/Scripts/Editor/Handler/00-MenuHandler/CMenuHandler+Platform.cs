using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;

/** 메뉴 처리자 - 플랫폼 */
public static partial class CMenuHandler {
	#region 클래스 함수
	/** iOS 로 전환한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_CHANGE_PLATFORM_BASE + "iOS", false, KCEditorDefine.B_SORTING_O_CHANGE_PLATFORM_MENU + 1)]
	public static void ChangeiOS() {
		CEditorFunc.ChangePlatform(BuildTargetGroup.iOS, BuildTarget.iOS);
	}

	/** 안드로이드로 전환한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_CHANGE_PLATFORM_BASE + "Android", false, KCEditorDefine.B_SORTING_O_CHANGE_PLATFORM_MENU + 1)]
	public static void ChangeAndroid() {
		CEditorFunc.ChangePlatform(BuildTargetGroup.Android, BuildTarget.Android);
	}

	/** 맥으로 전환한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_CHANGE_PLATFORM_BASE + "Mac", false, KCEditorDefine.B_SORTING_O_CHANGE_PLATFORM_MENU + 1)]
	public static void ChangeMac() {
		CEditorFunc.ChangePlatform(BuildTargetGroup.Standalone, BuildTarget.StandaloneOSX);
	}

	/** 윈도우즈로 전환한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_CHANGE_PLATFORM_BASE + "Windows", false, KCEditorDefine.B_SORTING_O_CHANGE_PLATFORM_MENU + 1)]
	public static void ChangeWnds() {
		CEditorFunc.ChangePlatform(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows64);
	}
	#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR

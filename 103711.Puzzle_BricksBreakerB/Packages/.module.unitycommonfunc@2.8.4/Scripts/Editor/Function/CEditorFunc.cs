using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using System.IO;
using System.Diagnostics;
using UnityEditor;
using UnityEditor.SceneManagement;
using TMPro;

/** 에디터 기본 함수 */
public static partial class CEditorFunc {
	#region 클래스 함수
	/** 에셋을 로드한다 */
	public static Object LoadAsset(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		var oAssets = AssetDatabase.LoadAllAssetsAtPath(a_oFilePath);

		return oAssets.ExIsValid() ? oAssets[KCDefine.B_VAL_0_INT] : null;
	}

	/** 에셋을 복사한다 */
	public static void CopyAsset(string a_oSrcPath, string a_oDestPath, bool a_bIsOverwrite = true, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oSrcPath.ExIsValid() && a_oDestPath.ExIsValid()));
		bool bIsValid = a_oSrcPath.ExIsValid() && a_oDestPath.ExIsValid();

		// 에셋 복사가 가능 할 경우
		if((bIsValid && CEditorAccess.IsExistsAsset(a_oSrcPath)) && (a_bIsOverwrite || !CEditorAccess.IsExistsAsset(a_oDestPath))) {
			CEditorFactory.MakeDirectories(Path.GetDirectoryName(a_oDestPath).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH), a_bIsEnableAssert);

			// 덮어쓰기 모드 일 경우
			if(a_bIsOverwrite && CEditorAccess.IsExistsAsset(a_oDestPath)) {
				AssetDatabase.DeleteAsset(a_oDestPath);
			}

			AssetDatabase.CopyAsset(a_oSrcPath, a_oDestPath);
		}
	}

	/** 에셋을 이동한다 */
	public static void MoveAsset(string a_oSrcPath, string a_oDestPath, bool a_bIsOverwrite = true, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oSrcPath.ExIsValid() && a_oDestPath.ExIsValid()));
		bool bIsValid = a_oSrcPath.ExIsValid() && a_oDestPath.ExIsValid();

		// 에셋 복사가 가능 할 경우
		if((bIsValid && CEditorAccess.IsExistsAsset(a_oSrcPath)) && (a_bIsOverwrite || !CEditorAccess.IsExistsAsset(a_oDestPath))) {
			CEditorFactory.MakeDirectories(Path.GetDirectoryName(a_oDestPath).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH), a_bIsEnableAssert);

			// 덮어쓰기 모드 일 경우
			if(a_bIsOverwrite && CEditorAccess.IsExistsAsset(a_oDestPath)) {
				AssetDatabase.DeleteAsset(a_oDestPath);
			}

			AssetDatabase.MoveAsset(a_oSrcPath, a_oDestPath);
		}
	}

	/** 경고 팝업을 출력한다 */
	public static bool ShowAlertPopup(string a_oTitle, string a_oMsg, string a_oOKBtnText = KCDefine.B_TEXT_EMPTY, string a_oCancelBtnText = KCDefine.B_TEXT_EMPTY) {
		// 취소 버튼 텍스트 존재 할 경우
		if(a_oCancelBtnText.ExIsValid()) {
			return EditorUtility.DisplayDialog(a_oTitle, a_oMsg, a_oOKBtnText.ExIsValid() ? a_oOKBtnText : KCEditorDefine.B_TEXT_ALERT_P_OK_BTN, a_oCancelBtnText);
		}

		return EditorUtility.DisplayDialog(a_oTitle, a_oMsg, a_oOKBtnText.ExIsValid() ? a_oOKBtnText : KCEditorDefine.B_TEXT_ALERT_P_OK_BTN);
	}

	/** 경고 팝업을 출력한다 */
	public static bool ShowOKCancelAlertPopup(string a_oTitle, string a_oMsg) {
		return CEditorFunc.ShowAlertPopup(a_oTitle, a_oMsg, KCEditorDefine.B_TEXT_ALERT_P_OK_BTN, KCEditorDefine.B_TEXT_ALERT_P_CANCEL_BTN);
	}

	/** 에셋 데이터 베이스 상태를 갱신한다 */
	public static void UpdateAssetDBState(bool a_bIsForceUpdate = false) {
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh(a_bIsForceUpdate ? ImportAssetOptions.ForceUpdate : ImportAssetOptions.Default);
	}

	/** 커맨드 라인을 실행한다 */
	public static void ExecuteCmdLine(string a_oParams, bool a_bIsAsync = true, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oParams.ExIsValid());

		// 매개 변수가 유효 할 경우
		if(a_oParams.ExIsValid()) {
#if UNITY_EDITOR_WIN
			CEditorFunc.ExecuteCmdLine(KCEditorDefine.B_TOOL_P_CMD_PROMPT, string.Format(KCEditorDefine.B_CMD_LINE_PARAMS_FMT_CMD_PROMPT, a_oParams), a_bIsAsync, a_bIsEnableAssert);
#else
			string oParams = string.Format(KCDefine.B_TEXT_FMT_2_SEMI_COLON_COMBINE, SystemInfo.processorType.ToUpper().Contains(KCEditorDefine.B_TOKEN_APPLE_M_SERIES) ? KCEditorDefine.B_BUILD_CMD_SILICON_EXPORT_PATH : KCEditorDefine.B_BUILD_CMD_INTEL_EXPORT_PATH, a_oParams);
			CEditorFunc.ExecuteCmdLine(KCEditorDefine.B_TOOL_P_SHELL, string.Format(KCEditorDefine.B_CMD_LINE_PARAMS_FMT_SHELL, oParams), a_bIsAsync, a_bIsEnableAssert);
#endif
		}
	}

	/** 커맨드 라인을 실행한다 */
	public static void ExecuteCmdLine(string a_oFilePath, string a_oParams, bool a_bIsAsync = true, bool a_bIsEnableAssert = true) {
		CFunc.ShowLog($"CEditorFunc.ExecuteCmdLine: {a_oFilePath}, {a_oParams}");
		CAccess.Assert(!a_bIsEnableAssert || (a_oFilePath.ExIsValid() && a_oParams.ExIsValid()));

		// 커맨드 라인 실행이 가능 할 경우
		if(a_oFilePath.ExIsValid() && a_oParams.ExIsValid()) {
			var oProcess = Process.Start(CEditorFactory.MakeProcessStartInfo(a_oFilePath, a_oParams));

			// 동기 모드 일 경우
			if(!a_bIsAsync) {
				oProcess?.WaitForExit();
			}
		}
	}

	/** 플랫폼을 변경한다 */
	public static void ChangePlatform(BuildTargetGroup a_eTargetGroup, BuildTarget a_eTarget) {
		EditorUserBuildSettings.SwitchActiveBuildTarget(a_eTargetGroup, a_eTarget);
	}

	/** 텍스트를 리셋한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_RESET_BASE + "Texts", false, KCEditorDefine.B_SORTING_O_RESET_MENU + 1)]
	public static void ResetTexts() {
		// 확인 버튼을 눌렀을 경우
		if(CEditorFunc.ShowOKCancelAlertPopup(KCEditorDefine.B_TEXT_ALERT_P_TITLE, KCEditorDefine.B_MSG_ALERT_P_RESET)) {
			var oTextList = CEditorFunc.FindComponents<Text>();
			var oTMPTextList = CEditorFunc.FindComponents<TMP_Text>();

			for(int i = 0; i < oTextList.Count; ++i) {
				oTextList[i].ExReset();

				// 에디터 모드 일 경우
				if(!Application.isPlaying) {
					EditorSceneManager.MarkSceneDirty(oTextList[i].gameObject.scene);
				}
			}

			for(int i = 0; i < oTMPTextList.Count; ++i) {
				oTMPTextList[i].ExReset();

				// 에디터 모드 일 경우
				if(!Application.isPlaying) {
					EditorSceneManager.MarkSceneDirty(oTMPTextList[i].gameObject.scene);
				}
			}
		}
	}

	/** 상호 작용자를 리셋한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_RESET_BASE + "Selectables", false, KCEditorDefine.B_SORTING_O_RESET_MENU + 1)]
	public static void ResetSelectables() {
		// 확인 버튼을 눌렀을 경우
		if(CEditorFunc.ShowOKCancelAlertPopup(KCEditorDefine.B_TEXT_ALERT_P_TITLE, KCEditorDefine.B_MSG_ALERT_P_RESET)) {
			var oSelectableList = CEditorFunc.FindComponents<Selectable>();

			for(int i = 0; i < oSelectableList.Count; ++i) {
				oSelectableList[i].ExReset();

				// 에디터 모드 일 경우
				if(!Application.isPlaying) {
					EditorSceneManager.MarkSceneDirty(oSelectableList[i].gameObject.scene);
				}
			}
		}
	}

	/** 스크롤 영역을 리셋한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_RESET_BASE + "Scroll Rects", false, KCEditorDefine.B_SORTING_O_RESET_MENU + 1)]
	public static void ResetScrollRects() {
		// 확인 버튼을 눌렀을 경우
		if(CEditorFunc.ShowOKCancelAlertPopup(KCEditorDefine.B_TEXT_ALERT_P_TITLE, KCEditorDefine.B_MSG_ALERT_P_RESET)) {
			var oScrollRectList = CEditorFunc.FindComponents<ScrollRect>();

			for(int i = 0; i < oScrollRectList.Count; ++i) {
				oScrollRectList[i].ExReset();

				// 에디터 모드 일 경우
				if(!Application.isPlaying) {
					EditorSceneManager.MarkSceneDirty(oScrollRectList[i].gameObject.scene);
				}
			}
		}
	}

	/** 캔버스 렌더러를 리셋한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_RESET_BASE + "Canvas Renderers", false, KCEditorDefine.B_SORTING_O_RESET_MENU + 1)]
	public static void ResetCanvasRenderers() {
		// 확인 버튼을 눌렀을 경우
		if(CEditorFunc.ShowOKCancelAlertPopup(KCEditorDefine.B_TEXT_ALERT_P_TITLE, KCEditorDefine.B_MSG_ALERT_P_RESET)) {
			var oCanvasRendererList = CEditorFunc.FindComponents<CanvasRenderer>();

			for(int i = 0; i < oCanvasRendererList.Count; ++i) {
				oCanvasRendererList[i].ExReset();

				// 에디터 모드 일 경우
				if(!Application.isPlaying) {
					EditorSceneManager.MarkSceneDirty(oCanvasRendererList[i].gameObject.scene);
				}
			}
		}
	}

	/** 레이아웃 그룹을 리셋한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_RESET_BASE + "Horizontal or Vertical LayoutGroups", false, KCEditorDefine.B_SORTING_O_RESET_MENU + 1)]
	public static void ResetLayoutGroups() {
		// 확인 버튼을 눌렀을 경우
		if(CEditorFunc.ShowOKCancelAlertPopup(KCEditorDefine.B_TEXT_ALERT_P_TITLE, KCEditorDefine.B_MSG_ALERT_P_RESET)) {
			var oLayoutGroupList = CEditorFunc.FindComponents<HorizontalOrVerticalLayoutGroup>();

			for(int i = 0; i < oLayoutGroupList.Count; ++i) {
				oLayoutGroupList[i].ExReset();

				// 에디터 모드 일 경우
				if(!Application.isPlaying) {
					EditorSceneManager.MarkSceneDirty(oLayoutGroupList[i].gameObject.scene);
				}
			}
		}
	}

	/** 카메라를 리셋한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_RESET_BASE + "Cameras", false, KCEditorDefine.B_SORTING_O_RESET_MENU + 1)]
	public static void ResetCameras() {
		// 확인 버튼을 눌렀을 경우
		if(CEditorFunc.ShowOKCancelAlertPopup(KCEditorDefine.B_TEXT_ALERT_P_TITLE, KCEditorDefine.B_MSG_ALERT_P_RESET)) {
			var oCameraList = CEditorFunc.FindComponents<Camera>();

			for(int i = 0; i < oCameraList.Count; ++i) {
				oCameraList[i].ExReset();

				// 에디터 모드 일 경우
				if(!Application.isPlaying) {
					EditorSceneManager.MarkSceneDirty(oCameraList[i].gameObject.scene);
				}
			}
		}
	}

	/** 렌더러를 리셋한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_RESET_BASE + "Renderers", false, KCEditorDefine.B_SORTING_O_RESET_MENU + 1)]
	public static void ResetRenderers() {
		// 확인 버튼을 눌렀을 경우
		if(CEditorFunc.ShowOKCancelAlertPopup(KCEditorDefine.B_TEXT_ALERT_P_TITLE, KCEditorDefine.B_MSG_ALERT_P_RESET)) {
			var oRendererList = CEditorFunc.FindComponents<Renderer>();

			for(int i = 0; i < oRendererList.Count; ++i) {
				oRendererList[i].ExReset();

				// 에디터 모드 일 경우
				if(!Application.isPlaying) {
					EditorSceneManager.MarkSceneDirty(oRendererList[i].gameObject.scene);
				}
			}
		}
	}

	/** 기준점을 리셋한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_RESET_BASE + "Pivots", false, KCEditorDefine.B_SORTING_O_RESET_MENU + 1)]
	public static void ResetPivots() {
		// 확인 버튼을 눌렀을 경우
		if(CEditorFunc.ShowOKCancelAlertPopup(KCEditorDefine.B_TEXT_ALERT_P_TITLE, KCEditorDefine.B_MSG_ALERT_P_RESET)) {
			var oBtnList = CEditorFunc.FindComponents<Button>();

			for(int i = 0; i < oBtnList.Count; ++i) {
				var stDelta = KCDefine.B_ANCHOR_MID_CENTER - (oBtnList[i].transform as RectTransform).pivot.ExTo3D();

				(oBtnList[i].transform as RectTransform).pivot = KCDefine.B_ANCHOR_MID_CENTER;
				(oBtnList[i].transform as RectTransform).localPosition += new Vector3(stDelta.x * (oBtnList[i].transform as RectTransform).rect.size.x, stDelta.y * (oBtnList[i].transform as RectTransform).rect.size.y, KCDefine.B_VAL_0_REAL);

				// 에디터 모드 일 경우
				if(!Application.isPlaying) {
					EditorSceneManager.MarkSceneDirty(oBtnList[i].gameObject.scene);
				}
			}
		}
	}

	/** DOTween Pro 패키지를 추가한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_IMPORT_BASE + "DOTweenPro Pkgs", false, KCEditorDefine.B_SORTING_O_IMPORT_MENU + 1)]
	public static void ImportDOTweenProPkgs() {
		AssetDatabase.ImportPackage(KCEditorDefine.B_ABS_PKGS_P_DOTWEEN_PRO, true);
	}

	/** Apple Sign In 패키지를 추가한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_IMPORT_BASE + "AppleSignIn Pkgs", false, KCEditorDefine.B_SORTING_O_IMPORT_MENU + 1)]
	public static void ImportAppleSignInPkgs() {
		AssetDatabase.ImportPackage(KCEditorDefine.B_ABS_PKGS_P_APPLE_SIGN_IN, true);
	}

	/** Build Report Tool 패키지를 추가한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_IMPORT_BASE + "BuildReportTool Pkgs", false, KCEditorDefine.B_SORTING_O_IMPORT_MENU + 1)]
	public static void ImportBuildReportToolPkgs() {
		AssetDatabase.ImportPackage(KCEditorDefine.B_ABS_PKGS_P_BUILD_REPORT_TOOL, true);
	}

	/** Odin Inspector 패키지를 추가한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_IMPORT_BASE + "OdinInspector Pkgs", false, KCEditorDefine.B_SORTING_O_IMPORT_MENU + 1)]
	public static void ImportOdinInspectorPkgs() {
		AssetDatabase.ImportPackage(KCEditorDefine.B_ABS_PKGS_P_ODIN_INSPECTOR, true);
	}

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

	#region 제네릭 클래스 함수
	/** 에셋을 탐색한다 */
	public static T FindAsset<T>(string a_oFilePath) where T : Object {
		CAccess.Assert(a_oFilePath.ExIsValid());
		return AssetDatabase.LoadAssetAtPath<T>(a_oFilePath);
	}

	/** 에셋을 탐색한다 */
	public static T FindAsset<T>(string a_oFilter, List<string> a_oSearchPathList) where T : Object {
		var oAssets = CEditorFunc.FindAssets<T>(a_oFilter, a_oSearchPathList);
		return oAssets.ExIsValid() ? oAssets[KCDefine.B_VAL_0_INT] : null;
	}

	/** 에셋을 탐색한다 */
	public static List<T> FindAssets<T>(string a_oFilter, List<string> a_oSearchPathList) where T : Object {
		var oAssetList = new List<T>();
		var oAssetGUIDs = AssetDatabase.FindAssets(a_oFilter, a_oSearchPathList.ToArray());

		// 에셋 GUID 가 존재 할 경우
		if(oAssetGUIDs.ExIsValid()) {
			for(int i = 0; i < oAssetGUIDs.Length; ++i) {
				var oAsset = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(oAssetGUIDs[i]));

				// 에셋이 존재 할 경우
				if(oAsset != null) {
					oAssetList.ExAddVal(oAsset);
				}
			}
		}

		return oAssetList;
	}

	/** 컴포넌트를 탐색한다 */
	public static List<T> FindComponents<T>(bool a_bIsIncludeInactive = false) where T : Component {
		var oPrefabStage = PrefabStageUtility.GetCurrentPrefabStage();
		var oComponentList = new List<T>();

		// 프리팹 모드 일 경우
		if(oPrefabStage != null) {
			oPrefabStage.prefabContentsRoot.GetComponentsInChildren<T>(a_bIsIncludeInactive, oComponentList);
		} else {
			CFunc.EnumerateComponents<T>((a_oComponent) => { oComponentList.Add(a_oComponent); return true; }, a_bIsIncludeInactive);
		}

		return oComponentList;
	}
	#endregion // 제네릭 클래스 함수
}
#endif // #if UNITY_EDITOR

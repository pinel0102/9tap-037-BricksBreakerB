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

	/** 에셋 상태를 갱신한다 */
	public static void UpdateAssetState(Object a_oObj) {
		AssetDatabase.SaveAssetIfDirty(a_oObj);
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

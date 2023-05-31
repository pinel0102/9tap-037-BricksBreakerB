using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;

#if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE
using UnityEngine.Rendering.Universal;
#endif // #if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE

/** 플랫폼 빌더 */
public static partial class CPlatformBuilder {
	/** 젠킨스 매개 변수 */
	private struct STJenkinsParams {
		public string m_oSrc;
		public string m_oPipeline;
		public string m_oProjName;
		public string m_oBuildOutputPath;
		public string m_oBuildFileExtension;
		public string m_oPlatform;
		public string m_oProjPlatform;
		public string m_oBuildVer;
		public string m_oBuildFunc;
		public string m_oPipelineName;

		public Dictionary<string, string> m_oDataDict;
	}

	#region 클래스 프로퍼티
	public static string BuildMethod { get; private set; } = string.Empty;
	public static EBuildMode BuildMode { get; private set; } = EBuildMode.NONE;

	public static EiOSType iOSType { get; private set; } = EiOSType.NONE;
	public static EAndroidType AndroidType { get; private set; } = EAndroidType.NONE;
	public static EStandaloneType StandaloneType { get; private set; } = EStandaloneType.NONE;
	#endregion // 클래스 프로퍼티

	#region 클래스 함수
	/** 빌드 플랫폼을 설정한다 */
	public static void SetupBuildPlatform() {
#if UNITY_IOS
		// Do Something
#elif UNITY_ANDROID
		// Do Something
#elif UNITY_STANDALONE
		// Do Something
#endif // #if UNITY_IOS
	}

	/** 플랫폼을 빌드한다 */
	private static void BuildPlatform(BuildPlayerOptions a_oPlayerOpts) {
		CFunc.ShowLog($"CPlatformBuilder.BuildPlatform: {a_oPlayerOpts.target}, {EditorUserBuildSettings.activeBuildTarget}");
		var oRestoreDefineSymbolList = new List<string>();

		try {
			// 현재 플랫폼 일 경우
			if(a_oPlayerOpts.target == EditorUserBuildSettings.activeBuildTarget) {
				CPlatformBuilder.DoBuildPlatform(a_oPlayerOpts, oRestoreDefineSymbolList);
			} else {
				CEditorFunc.ShowAlertPopup(KCEditorDefine.B_TEXT_ALERT_P_TITLE, KCEditorDefine.B_MSG_ALERT_P_PLATFORM_BUILD_FAIL);
			}
		} finally {
			// 빌드 메서드 파일이 없을 경우
			if(!Application.isBatchMode && !File.Exists(KCEditorDefine.B_DATA_P_BUILD_METHOD)) {
				CPlatformBuilder.BuildMode = EBuildMode.NONE;

				// 전처리기 심볼을 리셋한다 {
				var oRemoveDefineSymbolList = new List<string>() {
					KCEditorDefine.DS_DEFINE_S_STORE_DIST_BUILD,
					KCEditorDefine.DS_DEFINE_S_EDITOR_DIST_BUILD,
					KCEditorDefine.DS_DEFINE_S_CREATIVE_DIST_BUILD,

					KCEditorDefine.DS_DEFINE_S_ADS_TEST_ENABLE,
					KCEditorDefine.DS_DEFINE_S_ROBO_TEST_ENABLE,
					KCEditorDefine.DS_DEFINE_S_PLAY_TEST_ENABLE,

					KCEditorDefine.DS_DEFINE_S_IOS_APPLE_PLATFORM,

					KCEditorDefine.DS_DEFINE_S_ANDROID_GOOGLE_PLATFORM,
					KCEditorDefine.DS_DEFINE_S_ANDROID_AMAZON_PLATFORM,

					KCEditorDefine.DS_DEFINE_S_STANDALONE_MAC_STEAM_PLATFORM,
					KCEditorDefine.DS_DEFINE_S_STANDALONE_WNDS_STEAM_PLATFORM
				};

				CPlatformOptsSetter.AddDefineSymbols(a_oPlayerOpts.targetGroup, oRestoreDefineSymbolList);
				CPlatformOptsSetter.RemoveDefineSymbols(a_oPlayerOpts.targetGroup, oRemoveDefineSymbolList);

				CEditorFunc.SetupDefineSymbols(CPlatformOptsSetter.DefineSymbolDictContainer);
				// 전처리기 심볼을 리셋한다 }
			}
		}
	}

	/** 플랫폼을 빌드한다 */
	private static void DoBuildPlatform(BuildPlayerOptions a_oPlayerOpts, List<string> a_oOutRestoreDefineSymbolList) {
		// 빌드 메서드 파일이 없을 경우
		if(!File.Exists(KCEditorDefine.B_DATA_P_BUILD_METHOD)) {
			// 배포 빌드 모드 일 경우
			if(CPlatformBuilder.BuildMode == EBuildMode.STORE) {
				var oRemoveDefineSymbolList = new List<string>() {
					KCEditorDefine.DS_DEFINE_S_EDITOR_DIST_BUILD,
					KCEditorDefine.DS_DEFINE_S_CREATIVE_DIST_BUILD,
					KCEditorDefine.DS_DEFINE_S_ADS_TEST_ENABLE,
					KCEditorDefine.DS_DEFINE_S_ROBO_TEST_ENABLE,
					KCEditorDefine.DS_DEFINE_S_PLAY_TEST_ENABLE,
					KCEditorDefine.DS_DEFINE_S_LOCALIZE_TEST_ENABLE,
					KCEditorDefine.DS_DEFINE_S_ANALYTICS_TEST_ENABLE
				};

				for(int i = 0; i < oRemoveDefineSymbolList.Count; ++i) {
					// 전처리기 심볼이 포함 되었을 경우
					if(CPlatformOptsSetter.IsContainsDefineSymbol(a_oPlayerOpts.targetGroup, oRemoveDefineSymbolList[i])) {
						a_oOutRestoreDefineSymbolList.ExAddVal(oRemoveDefineSymbolList[i]);
						CPlatformOptsSetter.RemoveDefineSymbol(a_oPlayerOpts.targetGroup, oRemoveDefineSymbolList[i]);
					}
				}
			}

			CEditorFunc.SetupDefineSymbols(CPlatformOptsSetter.DefineSymbolDictContainer);
		}

		// 전처리기 심볼을 저장한다 {
		string oiOSName = CAccess.GetiOSName(CPlatformBuilder.iOSType);
		string oAndroidName = CAccess.GetAndroidName(CPlatformBuilder.AndroidType);
		string oStandaloneName = CAccess.GetStandaloneName(CPlatformBuilder.StandaloneType);

		switch(a_oPlayerOpts.targetGroup) {
			case BuildTargetGroup.iOS: CFunc.WriteStr(string.Format(KCEditorDefine.B_ASSET_P_FMT_DEFINE_S_OUTPUT, oiOSName), PlayerSettings.GetScriptingDefineSymbolsForGroup(a_oPlayerOpts.targetGroup), false); break;
			case BuildTargetGroup.Android: CFunc.WriteStr(string.Format(KCEditorDefine.B_ASSET_P_FMT_DEFINE_S_OUTPUT, oAndroidName), PlayerSettings.GetScriptingDefineSymbolsForGroup(a_oPlayerOpts.targetGroup), false); break;
			case BuildTargetGroup.Standalone: CFunc.WriteStr(string.Format(KCEditorDefine.B_ASSET_P_FMT_DEFINE_S_OUTPUT, oStandaloneName), PlayerSettings.GetScriptingDefineSymbolsForGroup(a_oPlayerOpts.targetGroup), false); break;
		}
		// 전처리기 심볼을 저장한다 }

		// 빌드 옵션을 설정한다 {
		a_oPlayerOpts.options &= ~(BuildOptions.SymlinkSources | BuildOptions.CleanBuildCache | BuildOptions.CompressWithLz4 | BuildOptions.CompressWithLz4HC);

#if DEBUG || DEVELOPMENT_BUILD
		a_oPlayerOpts.options |= a_oPlayerOpts.targetGroup == BuildTargetGroup.iOS ? BuildOptions.None : BuildOptions.CompressWithLz4;
#else
		a_oPlayerOpts.options |= BuildOptions.CleanBuildCache | a_oPlayerOpts.targetGroup == BuildTargetGroup.iOS ? BuildOptions.None : BuildOptions.CompressWithLz4HC;
#endif // #if DEBUG || DEVELOPMENT_BUILD

		CPlatformOptsSetter.SetupQuality();
		CPlatformOptsSetter.SetupEditorOpts();
		CPlatformOptsSetter.SetupPlayerOpts();
		// 빌드 옵션을 설정한다 }

		// 씬 경로를 설정한다 {
		var oScenePathList = new List<string>();

		for(int i = 0; i < EditorBuildSettings.scenes.Length; ++i) {
#if DEBUG || DEVELOPMENT_BUILD
			oScenePathList.ExAddVal(EditorBuildSettings.scenes[i].path);
#else
			bool bIsEditorScene01 = EditorBuildSettings.scenes[i].path.Contains(KCEditorDefine.B_EDITOR_SCENE_N_PATTERN_01);
			bool bIsEditorScene02 = EditorBuildSettings.scenes[i].path.Contains(KCEditorDefine.B_EDITOR_SCENE_N_PATTERN_02);

			// 테스트, 에디터 씬이 아닐 경우
			if(!bIsTestScene && !bIsEditorScene01 && !bIsEditorScene02 && !EditorBuildSettings.scenes[i].path.Contains(KCDefine.B_SCENE_N_TEST)) {
				oScenePathList.ExAddVal(oEditorScene.path);
			}
#endif // #if DEBUG || DEVELOPMENT_BUILD
		}

		a_oPlayerOpts.scenes = oScenePathList.ToArray();
		// 씬 경로를 설정한다 }

		// 빌드 가능 할 경우
		if(!EditorApplication.isCompiling && !BuildPipeline.isBuildingPlayer) {
			CPlatformBuilder.BuildMethod = string.Empty;
			CFunc.RemoveFile(KCEditorDefine.B_DATA_P_BUILD_METHOD);

			BuildPipeline.BuildPlayer(a_oPlayerOpts);
		} else {
			CEditorFunc.UpdateAssetDBState();
			CPlatformOptsSetter.SetupGraphicAPIs();

			CFunc.WriteStr(KCEditorDefine.B_DATA_P_BUILD_METHOD, CPlatformBuilder.BuildMethod, false);
		}
	}
	#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR

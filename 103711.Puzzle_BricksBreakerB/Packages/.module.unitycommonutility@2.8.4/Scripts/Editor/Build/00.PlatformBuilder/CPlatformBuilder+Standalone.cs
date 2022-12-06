using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;

/** 플랫폼 빌더 - 독립 플랫폼 */
public static partial class CPlatformBuilder {
	#region 클래스 함수
	/** 맥 스팀을 빌드한다 */
	public static void BuildStandaloneMacSteamStoreDist() {
#if !NINETAP_BUILD_PIPELINE_ENABLE
		CPlatformBuilder.BuildStandaloneMacSteamStoreA();
#endif // #if !NINETAP_BUILD_PIPELINE_ENABLE
	}

	/** 윈도우즈 스팀을 빌드한다 */
	public static void BuildStandaloneWndsSteamStoreDist() {
#if !NINETAP_BUILD_PIPELINE_ENABLE
		CPlatformBuilder.BuildStandaloneWndsSteamStoreA();
#endif // #if !NINETAP_BUILD_PIPELINE_ENABLE
	}

	/** 독립 플랫폼을 빌드한다 */
	private static void BuildStandaloneDebug(EStandaloneType a_eType, bool a_bIsAutoPlay, bool a_bIsEnableProfiler) {
		CPlatformBuilder.BuildMode = EBuildMode.DEBUG;

		// 빌드 옵션을 설정한다
		var oPlayerOpts = new BuildPlayerOptions();
		oPlayerOpts.options = BuildOptions.Development;
		oPlayerOpts.options |= a_bIsAutoPlay ? BuildOptions.AutoRunPlayer : BuildOptions.None;
		oPlayerOpts.options |= a_bIsEnableProfiler ? BuildOptions.ConnectWithProfiler : BuildOptions.None;

		CPlatformOptsSetter.AddDefineSymbol(BuildTargetGroup.Standalone, KCEditorDefine.DS_DEFINE_S_ADS_TEST_ENABLE);
		CPlatformBuilder.BuildStandalone(a_eType, oPlayerOpts);
	}

	/** 독립 플랫폼을 빌드한다 */
	private static void BuildStandaloneDebugWithAutoPlay(EStandaloneType a_eType) {
		CPlatformBuilder.BuildStandaloneDebug(a_eType, true, false);
	}

	/** 독립 플랫폼을 빌드한다 */
	private static void BuildStandaloneDebugWithProfiler(EStandaloneType a_eType) {
		CPlatformBuilder.BuildStandaloneDebug(a_eType, true, true);
	}

	/** 독립 플랫폼을 빌드한다 */
	private static void BuildStandaloneRelease(EStandaloneType a_eType, bool a_bIsAutoPlay) {
		CPlatformBuilder.BuildMode = EBuildMode.RELEASE;

		// 빌드 옵션을 설정한다
		var oPlayerOpts = new BuildPlayerOptions();
		oPlayerOpts.options = a_bIsAutoPlay ? BuildOptions.AutoRunPlayer : BuildOptions.None;

		CPlatformOptsSetter.AddDefineSymbol(BuildTargetGroup.Standalone, KCEditorDefine.DS_DEFINE_S_ADS_TEST_ENABLE);
		CPlatformBuilder.BuildStandalone(a_eType, oPlayerOpts);
	}

	/** 독립 플랫폼을 빌드한다 */
	private static void BuildStandaloneReleaseWithAutoPlay(EStandaloneType a_eType) {
		CPlatformOptsSetter.AddDefineSymbol(BuildTargetGroup.Standalone, KCEditorDefine.DS_DEFINE_S_PLAY_TEST_ENABLE);
		CPlatformBuilder.BuildStandaloneRelease(a_eType, true);
	}

	/** 독립 플랫픔을 빌드한다 */
	private static void BuildStandaloneStoreA(EStandaloneType a_eType) {
		CPlatformBuilder.BuildMode = EBuildMode.STORE;
		CPlatformOptsSetter.AddDefineSymbol(BuildTargetGroup.Standalone, KCEditorDefine.DS_DEFINE_S_STORE_DIST_BUILD);

		CPlatformBuilder.BuildStandalone(a_eType, new BuildPlayerOptions());
	}

	/** 독립 플랫폼을 빌드한다 */
	private static void BuildStandalone(EStandaloneType a_eType, BuildPlayerOptions a_oPlayerOpts) {
		CPlatformBuilder.StandaloneType = a_eType;

		// 빌드 옵션을 설정한다 {
		string oPlatform = CAccess.GetStandaloneName(a_eType);
		string oBuildFileExtension = (a_eType == EStandaloneType.WNDS_STEAM) ? KCEditorDefine.B_BUILD_FILE_EXTENSION_STANDALONE_EXE : KCEditorDefine.B_BUILD_FILE_EXTENSION_STANDALONE_APP;

		a_oPlayerOpts.target = (a_eType == EStandaloneType.WNDS_STEAM) ? BuildTarget.StandaloneWindows64 : BuildTarget.StandaloneOSX;
		a_oPlayerOpts.targetGroup = BuildTargetGroup.Standalone;
		a_oPlayerOpts.locationPathName = string.Format(KCEditorDefine.B_BUILD_P_FMT_STANDALONE, oPlatform, string.Format(KCEditorDefine.B_BUILD_FILE_N_FMT_STANDALONE, oPlatform, oBuildFileExtension));

		switch(a_eType) {
			case EStandaloneType.WNDS_STEAM: CPlatformOptsSetter.AddDefineSymbol(a_oPlayerOpts.targetGroup, KCEditorDefine.DS_DEFINE_S_STANDALONE_WNDS_STEAM_PLATFORM); break;
			default: CPlatformOptsSetter.AddDefineSymbol(a_oPlayerOpts.targetGroup, KCEditorDefine.DS_DEFINE_S_STANDALONE_MAC_STEAM_PLATFORM); break;
		}

		// 프로젝트 정보 테이블이 존재 할 경우
		if(CPlatformOptsSetter.ProjInfoTable != null) {
			switch(a_eType) {
				case EStandaloneType.WNDS_STEAM: PlayerSettings.bundleVersion = CPlatformOptsSetter.ProjInfoTable.StandaloneWndsSteamProjInfo.m_stBuildVerInfo.m_oVer; break;
				default: PlayerSettings.bundleVersion = CPlatformOptsSetter.ProjInfoTable.StandaloneMacSteamProjInfo.m_stBuildVerInfo.m_oVer; break;
			}
		}
		// 빌드 옵션을 설정한다 }

		// 플랫폼을 빌드한다
		CFactory.MakeDirectories(string.Format(KCEditorDefine.B_ABS_BUILD_P_FMT_STANDALONE, oPlatform));
		CPlatformBuilder.BuildPlatform(a_oPlayerOpts);
	}

	/** 독립 플랫폼을 원격 빌드한다 */
	private static void RemoteBuildStandaloneDebug(EStandaloneType a_eType) {
		CPlatformBuilder.ExecuteStandaloneJenkinsBuild(a_eType, KCEditorDefine.B_DEBUG_BUILD_FUNC_JENKINS, KCEditorDefine.B_STANDALONE_DEBUG_PIPELINE_N_JENKINS, KCEditorDefine.B_BUILD_FILE_EXTENSION_STANDALONE_ZIP);
	}

	/** 독립 플랫폼을 원격 빌드한다 */
	private static void RemoteBuildStandaloneRelease(EStandaloneType a_eType) {
		CPlatformBuilder.ExecuteStandaloneJenkinsBuild(a_eType, KCEditorDefine.B_RELEASE_BUILD_FUNC_JENKINS, KCEditorDefine.B_STANDALONE_RELEASE_PIPELINE_N_JENKINS, KCEditorDefine.B_BUILD_FILE_EXTENSION_STANDALONE_ZIP);
	}

	/** 독립 플랫폼을 원격 빌드한다 */
	private static void RemoteBuildStandaloneStoreA(EStandaloneType a_eType) {
		CPlatformBuilder.ExecuteStandaloneJenkinsBuild(a_eType, KCEditorDefine.B_STORE_A_BUILD_FUNC_JENKINS, KCEditorDefine.B_STANDALONE_STORE_PIPELINE_N_JENKINS, KCEditorDefine.B_BUILD_FILE_EXTENSION_STANDALONE_ZIP);
	}

	/** 독립 플랫폼을 원격 빌드한다 */
	private static void RemoteBuildStandaloneStoreDist(EStandaloneType a_eType) {
		CPlatformBuilder.ExecuteStandaloneJenkinsBuild(a_eType, KCEditorDefine.B_STORE_DIST_BUILD_FUNC_JENKINS, KCEditorDefine.B_STANDALONE_STORE_PIPELINE_N_JENKINS, KCEditorDefine.B_BUILD_FILE_EXTENSION_STANDALONE_ZIP);
	}
	#endregion // 클래스 함수
}

/** 플랫폼 빌더 - 에디터 */
public static partial class CPlatformBuilder {
	#region 클래스 함수
	/** 맥 에디터를 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Editor/Mac/Debug", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 1)]
	public static void BuildMacEditorDebug() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformOptsSetter.AddDefineSymbol(BuildTargetGroup.Standalone, KCEditorDefine.DS_DEFINE_S_EDITOR_DIST_BUILD);

		CPlatformBuilder.BuildStandaloneDebug(EStandaloneType.MAC_STEAM, false, false);
	}

	/** 맥 에디터를 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Editor/Mac/Debug With AutoPlay", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 1)]
	public static void BuildMacEditorDebugWithAutoPlay() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformOptsSetter.AddDefineSymbol(BuildTargetGroup.Standalone, KCEditorDefine.DS_DEFINE_S_EDITOR_DIST_BUILD);

		CPlatformBuilder.BuildStandaloneDebugWithAutoPlay(EStandaloneType.MAC_STEAM);
	}

	/** 윈도우즈 에디터를 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Editor/Windows/Debug", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 1)]
	public static void BuildWndsEditorDebug() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformOptsSetter.AddDefineSymbol(BuildTargetGroup.Standalone, KCEditorDefine.DS_DEFINE_S_EDITOR_DIST_BUILD);

		CPlatformBuilder.BuildStandaloneDebug(EStandaloneType.WNDS_STEAM, false, false);
	}

	/** 윈도우즈 에디터를 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Editor/Windows/Debug With AutoPlay", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 1)]
	public static void BuildWndsEditorDebugWithAutoPlay() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformOptsSetter.AddDefineSymbol(BuildTargetGroup.Standalone, KCEditorDefine.DS_DEFINE_S_EDITOR_DIST_BUILD);

		CPlatformBuilder.BuildStandaloneDebugWithAutoPlay(EStandaloneType.WNDS_STEAM);
	}
	#endregion // 클래스 함수
}

#if !NINETAP_BUILD_PIPELINE_ENABLE
/** 플랫폼 빌더 - 맥 스팀 */
public static partial class CPlatformBuilder {
	#region 클래스 함수
	/** 맥 스팀을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Standalone/Mac/Steam/Debug", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 101)]
	public static void BuildStandaloneMacSteamDebug() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildStandaloneDebug(EStandaloneType.MAC_STEAM, false, false);
	}

	/** 맥 스팀을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Standalone/Mac/Steam/Debug with AutoPlay", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 101)]
	public static void BuildStandaloneMacSteamDebugWithAutoPlay() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildStandaloneDebugWithAutoPlay(EStandaloneType.MAC_STEAM);
	}

	/** 맥 스팀을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Standalone/Mac/Steam/Debug with Profiler", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 101)]
	public static void BuildStandaloneMacSteamDebugWithProfiler() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildStandaloneDebugWithProfiler(EStandaloneType.MAC_STEAM);
	}

	/** 맥 스팀을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Standalone/Mac/Steam/Release", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 201)]
	public static void BuildStandaloneMacSteamRelease() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildStandaloneRelease(EStandaloneType.MAC_STEAM, false);
	}

	/** 맥 스팀을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Standalone/Mac/Steam/Release with AutoPlay", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 201)]
	public static void BuildStandaloneMacSteamReleaseWithAutoPlay() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildStandaloneReleaseWithAutoPlay(EStandaloneType.MAC_STEAM);
	}

	/** 맥 스팀을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Standalone/Mac/Steam/Distribution", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 301)]
	public static void BuildStandaloneMacSteamStoreA() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildStandaloneStoreA(EStandaloneType.MAC_STEAM);
	}

	/** 맥 스팀을 원격 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Remote (Jenkins)/Standalone/Mac/Steam/Debug", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 101)]
	public static void RemoteBuildStandaloneMacSteamDebug() {
		CPlatformBuilder.RemoteBuildStandaloneDebug(EStandaloneType.MAC_STEAM);
	}

	/** 맥 스팀을 원격 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Remote (Jenkins)/Standalone/Mac/Steam/Release", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 201)]
	public static void RemoteBuildStandaloneMacSteamRelease() {
		CPlatformBuilder.RemoteBuildStandaloneRelease(EStandaloneType.MAC_STEAM);
	}

	/** 맥 스팀을 원격 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Remote (Jenkins)/Standalone/Mac/Steam/Distribution", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 301)]
	public static void RemoteBuildStandaloneMacSteamStoreA() {
		CPlatformBuilder.RemoteBuildStandaloneStoreA(EStandaloneType.MAC_STEAM);
	}

	/** 맥 스팀을 원격 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Remote (Jenkins)/Standalone/Mac/Steam/Distribution (Store)", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 301)]
	public static void RemoteBuildStandaloneMacSteamStoreDist() {
		CPlatformBuilder.RemoteBuildStandaloneStoreDist(EStandaloneType.MAC_STEAM);
	}
	#endregion // 클래스 함수
}

/** 플랫폼 빌더 - 윈도우즈 스팀 */
public static partial class CPlatformBuilder {
	#region 클래스 함수
	/** 윈도우즈 스팀을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Standalone/Windows/Steam/Debug", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 101)]
	public static void BuildStandaloneWndsSteamDebug() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildStandaloneDebug(EStandaloneType.WNDS_STEAM, false, false);
	}

	/** 윈도우즈 스팀을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Standalone/Windows/Steam/Debug with AutoPlay", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 101)]
	public static void BuildStandaloneWndsSteamDebugWithAutoPlay() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildStandaloneDebugWithAutoPlay(EStandaloneType.WNDS_STEAM);
	}

	/** 윈도우즈 스팀을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Standalone/Windows/Steam/Debug with Profiler", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 101)]
	public static void BuildStandaloneWndsSteamDebugWithProfiler() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildStandaloneDebugWithProfiler(EStandaloneType.WNDS_STEAM);
	}

	/** 윈도우즈 스팀을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Standalone/Windows/Steam/Release", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 201)]
	public static void BuildStandaloneWndsSteamRelease() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildStandaloneRelease(EStandaloneType.WNDS_STEAM, false);
	}

	/** 윈도우즈 스팀을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Standalone/Windows/Steam/Release with AutoPlay", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 201)]
	public static void BuildStandaloneWndsSteamReleaseWithAutoPlay() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildStandaloneReleaseWithAutoPlay(EStandaloneType.WNDS_STEAM);
	}

	/** 윈도우즈 스팀을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Standalone/Windows/Steam/Distribution", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 301)]
	public static void BuildStandaloneWndsSteamStoreA() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildStandaloneStoreA(EStandaloneType.WNDS_STEAM);
	}

	/** 윈도우즈 스팀을 원격 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Remote (Jenkins)/Standalone/Windows/Steam/Debug", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 101)]
	public static void RemoteBuildStandaloneWndsSteamDebug() {
		CPlatformBuilder.RemoteBuildStandaloneDebug(EStandaloneType.WNDS_STEAM);
	}

	/** 윈도우즈 스팀을 원격 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Remote (Jenkins)/Standalone/Windows/Steam/Release", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 201)]
	public static void RemoteBuildStandaloneWndsSteamRelease() {
		CPlatformBuilder.RemoteBuildStandaloneRelease(EStandaloneType.WNDS_STEAM);
	}

	/** 윈도우즈 스팀을 원격 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Remote (Jenkins)/Standalone/Windows/Steam/Distribution", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 301)]
	public static void RemoteBuildStandaloneWndsSteamStoreA() {
		CPlatformBuilder.RemoteBuildStandaloneStoreA(EStandaloneType.WNDS_STEAM);
	}

	/** 윈도우즈 스팀을 원격 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Remote (Jenkins)/Standalone/Windows/Steam/Distribution (Store)", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 301)]
	public static void RemoteBuildStandaloneWndsSteamStoreDist() {
		CPlatformBuilder.RemoteBuildStandaloneStoreDist(EStandaloneType.WNDS_STEAM);
	}
	#endregion // 클래스 함수
}
#endif // #if !NINETAP_BUILD_PIPELINE_ENABLE
#endif // #if UNITY_EDITOR

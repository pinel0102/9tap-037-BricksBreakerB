using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;

/** 플랫폼 빌더 - iOS */
public static partial class CPlatformBuilder {
	#region 클래스 함수
	/** iOS 애플을 빌드한다 */
	public static void BuildiOSAppleStoreDist() {
		CPlatformBuilder.BuildiOSAppleStoreA();
	}

	/** iOS 를 빌드한다 */
	private static void BuildiOSDebug(EiOSType a_eType, bool a_bIsAutoPlay, bool a_bIsEnableProfiler) {
		CPlatformBuilder.BuildMode = EBuildMode.DEBUG;
		EditorUserBuildSettings.iOSXcodeBuildConfig = XcodeBuildConfig.Debug;

		// 빌드 옵션을 설정한다
		var oPlayerOpts = new BuildPlayerOptions();
		oPlayerOpts.options = BuildOptions.Development;
		oPlayerOpts.options |= a_bIsAutoPlay ? BuildOptions.AutoRunPlayer : BuildOptions.None;
		oPlayerOpts.options |= a_bIsEnableProfiler ? BuildOptions.ConnectWithProfiler : BuildOptions.None;

		// 프로비저닝 파일 정보를 설정한다
		PlayerSettings.iOS.iOSManualProvisioningProfileID = CPlatformOptsSetter.BuildInfoTable.iOSBuildInfo.m_oDevProfileID;
		PlayerSettings.iOS.iOSManualProvisioningProfileType = ProvisioningProfileType.Development;

		CPlatformOptsSetter.AddDefineSymbol(BuildTargetGroup.iOS, KCEditorDefine.DS_DEFINE_S_ADS_TEST_ENABLE);
		CPlatformBuilder.BuildiOS(a_eType, oPlayerOpts);
	}

	/** iOS 를 빌드한다 */
	private static void BuildiOSDebugWithAutoPlay(EiOSType a_eType) {
		CPlatformBuilder.BuildiOSDebug(a_eType, true, false);
	}

	/** iOS 를 빌드한다 */
	private static void BuildiOSDebugWithProfiler(EiOSType a_eType) {
		CPlatformBuilder.BuildiOSDebug(a_eType, true, true);
	}

	/** iOS 를 빌드한다 */
	private static void BuildiOSRelease(EiOSType a_eType, bool a_bIsAutoPlay) {
		CPlatformBuilder.BuildMode = EBuildMode.RELEASE;
		EditorUserBuildSettings.iOSXcodeBuildConfig = XcodeBuildConfig.Release;

		// 빌드 옵션을 설정한다
		var oPlayerOpts = new BuildPlayerOptions();
		oPlayerOpts.options = a_bIsAutoPlay ? BuildOptions.AutoRunPlayer : BuildOptions.None;

		// 프로비저닝 파일 정보를 설정한다
		PlayerSettings.iOS.iOSManualProvisioningProfileID = CPlatformOptsSetter.BuildInfoTable.iOSBuildInfo.m_oDevProfileID;
		PlayerSettings.iOS.iOSManualProvisioningProfileType = ProvisioningProfileType.Development;

		CPlatformOptsSetter.AddDefineSymbol(BuildTargetGroup.iOS, KCEditorDefine.DS_DEFINE_S_ADS_TEST_ENABLE);
		CPlatformBuilder.BuildiOS(a_eType, oPlayerOpts);
	}

	/** iOS 를 빌드한다 */
	private static void BuildiOSReleaseWithAutoPlay(EiOSType a_eType) {
		CPlatformOptsSetter.AddDefineSymbol(BuildTargetGroup.iOS, KCEditorDefine.DS_DEFINE_S_PLAY_TEST_ENABLE);
		CPlatformBuilder.BuildiOSRelease(a_eType, true);
	}

	/** iOS 를 빌드한다 */
	private static void BuildiOSStoreA(EiOSType a_eType) {
		CPlatformBuilder.BuildMode = EBuildMode.STORE;
		EditorUserBuildSettings.iOSXcodeBuildConfig = XcodeBuildConfig.Release;

		// 프로비저닝 파일 정보를 설정한다
		PlayerSettings.iOS.iOSManualProvisioningProfileID = CPlatformOptsSetter.BuildInfoTable.iOSBuildInfo.m_oStoreProfileID;
		PlayerSettings.iOS.iOSManualProvisioningProfileType = ProvisioningProfileType.Distribution;

		CPlatformOptsSetter.AddDefineSymbol(BuildTargetGroup.iOS, KCEditorDefine.DS_DEFINE_S_STORE_DIST_BUILD);
		CPlatformBuilder.BuildiOS(a_eType, new BuildPlayerOptions());
	}

	/** iOS 를 빌드한다 */
	private static void BuildiOS(EiOSType a_eType, BuildPlayerOptions a_oPlayerOpts) {
		CPlatformBuilder.iOSType = a_eType;

		// 플러그인 파일을 복사한다
		if(!Application.isBatchMode) {
			CFunc.CopyDir(KCEditorDefine.B_SRC_PLUGIN_P_IOS, KCEditorDefine.B_DEST_PLUGIN_P_IOS, false);
		}

		// 빌드 옵션을 설정한다 {
		a_oPlayerOpts.target = BuildTarget.iOS;
		a_oPlayerOpts.targetGroup = BuildTargetGroup.iOS;
		a_oPlayerOpts.locationPathName = string.Format(KCEditorDefine.B_BUILD_P_FMT_IOS, CAccess.GetiOSName(a_eType));

		CPlatformOptsSetter.AddDefineSymbol(a_oPlayerOpts.targetGroup, KCEditorDefine.DS_DEFINE_S_IOS_APPLE_PLATFORM);

		// 프로젝트 정보 테이블이 존재 할 경우
		if(CPlatformOptsSetter.ProjInfoTable != null) {
			PlayerSettings.bundleVersion = CPlatformOptsSetter.ProjInfoTable.iOSAppleProjInfo.m_stBuildVerInfo.m_oVer;
		}
		// 빌드 옵션을 설정한다 }

		// 플랫폼을 빌드한다
		CFactory.MakeDirectories(KCEditorDefine.B_ABS_BUILD_P_IOS);
		CPlatformBuilder.BuildPlatform(a_oPlayerOpts);
	}

	/** iOS 를 원격 빌드한다 */
	private static void RemoteBuildiOSDebug(EiOSType a_eType) {
		CPlatformBuilder.ExecuteiOSJenkinsBuild(a_eType, KCEditorDefine.B_DEBUG_BUILD_FUNC_JENKINS, KCEditorDefine.B_IOS_DEBUG_PIPELINE_N_JENKINS, KCEditorDefine.B_BUILD_FILE_EXTENSION_IOS_IPA, CPlatformOptsSetter.BuildInfoTable.iOSBuildInfo.m_oDevProfileID, KCEditorDefine.B_IPA_EXPORT_METHOD_IOS_DEV);
	}

	/** iOS 를 원격 빌드한다 */
	private static void RemoteBuildiOSRelease(EiOSType a_eType) {
		CPlatformBuilder.ExecuteiOSJenkinsBuild(a_eType, KCEditorDefine.B_RELEASE_BUILD_FUNC_JENKINS, KCEditorDefine.B_IOS_RELEASE_PIPELINE_N_JENKINS, KCEditorDefine.B_BUILD_FILE_EXTENSION_IOS_IPA, CPlatformOptsSetter.BuildInfoTable.iOSBuildInfo.m_oDevProfileID, KCEditorDefine.B_IPA_EXPORT_METHOD_IOS_DEV);
	}

	/** iOS 를 원격 빌드한다 */
	private static void RemoteBuildiOSStoreA(EiOSType a_eType) {
		CPlatformBuilder.ExecuteiOSJenkinsBuild(a_eType, KCEditorDefine.B_STORE_A_BUILD_FUNC_JENKINS, KCEditorDefine.B_IOS_STORE_PIPELINE_N_JENKINS, KCEditorDefine.B_BUILD_FILE_EXTENSION_IOS_IPA, CPlatformOptsSetter.BuildInfoTable.iOSBuildInfo.m_oStoreProfileID, KCEditorDefine.B_IPA_EXPORT_METHOD_IOS_STORE);
	}

	/** iOS 를 원격 빌드한다 */
	private static void RemoteBuildiOSStoreDist(EiOSType a_eType) {
		CPlatformBuilder.ExecuteiOSJenkinsBuild(a_eType, KCEditorDefine.B_STORE_DIST_BUILD_FUNC_JENKINS, KCEditorDefine.B_IOS_STORE_PIPELINE_N_JENKINS, KCEditorDefine.B_BUILD_FILE_EXTENSION_IOS_IPA, CPlatformOptsSetter.BuildInfoTable.iOSBuildInfo.m_oStoreProfileID, KCEditorDefine.B_IPA_EXPORT_METHOD_IOS_STORE);
	}
	#endregion // 클래스 함수
}

/** 플랫폼 빌더 - iOS 애플 */
public static partial class CPlatformBuilder {
	#region 클래스 함수
	/** iOS 애플을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/iOS/Apple/Debug", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 101)]
	public static void BuildiOSAppleDebug() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildiOSDebug(EiOSType.APPLE, false, false);
	}

	/** iOS 애플을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/iOS/Apple/Debug with AutoPlay", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 101)]
	public static void BuildiOSAppleDebugWithAutoPlay() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildiOSDebugWithAutoPlay(EiOSType.APPLE);
	}

	/** iOS 애플을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/iOS/Apple/Debug with Profiler", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 101)]
	public static void BuildiOSAppleDebugWithProfiler() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildiOSDebugWithProfiler(EiOSType.APPLE);
	}

	/** iOS 애플을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/iOS/Apple/Release", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 201)]
	public static void BuildiOSAppleRelease() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildiOSRelease(EiOSType.APPLE, false);
	}

	/** iOS 애플을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/iOS/Apple/Release with AutoPlay", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 201)]
	public static void BuildiOSAppleReleaseWithAutoPlay() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildiOSReleaseWithAutoPlay(EiOSType.APPLE);
	}

	/** iOS 애플을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/iOS/Apple/Distribution", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 301)]
	public static void BuildiOSAppleStoreA() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildiOSStoreA(EiOSType.APPLE);
	}

	/** iOS 애플을 원격 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Remote (Jenkins)/iOS/Apple/Debug", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 101)]
	public static void RemoteBuildiOSAppleDebug() {
		CPlatformBuilder.RemoteBuildiOSDebug(EiOSType.APPLE);
	}

	/** iOS 애플을 원격 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Remote (Jenkins)/iOS/Apple/Release", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 201)]
	public static void RemoteBuildiOSAppleRelease() {
		CPlatformBuilder.RemoteBuildiOSRelease(EiOSType.APPLE);
	}

	/** iOS 애플을 원격 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Remote (Jenkins)/iOS/Apple/Distribution", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 301)]
	public static void RemoteBuildiOSAppleStoreA() {
		CPlatformBuilder.RemoteBuildiOSStoreA(EiOSType.APPLE);
	}

	/** iOS 애플을 원격 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Remote (Jenkins)/iOS/Apple/Distribution (Store)", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 301)]
	public static void RemoteBuildiOSAppleStoreDist() {
		CPlatformBuilder.RemoteBuildiOSStoreDist(EiOSType.APPLE);
	}
	#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;

/** 플랫폼 빌더 - 안드로이드 */
public static partial class CPlatformBuilder {
	#region 클래스 함수
	/** 안드로이드 구글을 빌드한다 */
	public static void BuildAndroidGoogleStoreDist() {
		CPlatformBuilder.BuildAndroidGoogleStoreB();
	}

	/** 안드로이드 아마존을 빌드한다 */
	public static void BuildAndroidAmazonStoreDist() {
		CPlatformBuilder.BuildAndroidAmazonStoreA();
	}

	/** 안드로이드를 빌드한다 */
	private static void BuildAndroidDebug(EAndroidType a_eType, bool a_bIsAutoPlay, bool a_bIsEnableProfiler) {
		CPlatformBuilder.BuildMode = EBuildMode.DEBUG;
		EditorUserBuildSettings.buildAppBundle = false;

		// 빌드 옵션을 설정한다
		var oPlayerOpts = new BuildPlayerOptions();
		oPlayerOpts.options = BuildOptions.Development;
		oPlayerOpts.options |= a_bIsAutoPlay ? BuildOptions.AutoRunPlayer : BuildOptions.None;
		oPlayerOpts.options |= a_bIsEnableProfiler ? BuildOptions.ConnectWithProfiler : BuildOptions.None;

		CPlatformOptsSetter.AddDefineSymbol(BuildTargetGroup.Android, KCEditorDefine.DS_DEFINE_S_ADS_TEST_ENABLE);
		CPlatformBuilder.BuildAndroid(a_eType, oPlayerOpts, "Debug");
	}

	/** 안드로이드를 빌드한다 */
	private static void BuildAndroidDebugWithAutoPlay(EAndroidType a_eType) {
		CPlatformBuilder.BuildAndroidDebug(a_eType, true, false);
	}

	/** 안드로이드를 빌드한다 */
	private static void BuildAndroidDebugWithRoboTest(EAndroidType a_eType) {
		CPlatformOptsSetter.AddDefineSymbol(BuildTargetGroup.Android, KCEditorDefine.DS_DEFINE_S_ROBO_TEST_ENABLE);
		CPlatformBuilder.BuildAndroidDebug(a_eType, false, false);
	}

	/** 안드로이드를 빌드한다 */
	private static void BuildAndroidDebugWithProfiler(EAndroidType a_eType) {
		CPlatformBuilder.BuildAndroidDebug(a_eType, true, true);
	}

	/** 안드로이드를 빌드한다 */
	private static void BuildAndroidRelease(EAndroidType a_eType, bool a_bIsAutoPlay) {
		CPlatformBuilder.BuildMode = EBuildMode.RELEASE;
		EditorUserBuildSettings.buildAppBundle = false;

		// 빌드 옵션을 설정한다
		var oPlayerOpts = new BuildPlayerOptions();
		oPlayerOpts.options = a_bIsAutoPlay ? BuildOptions.AutoRunPlayer : BuildOptions.None;

		CPlatformOptsSetter.AddDefineSymbol(BuildTargetGroup.Android, KCEditorDefine.DS_DEFINE_S_ADS_TEST_ENABLE);
		CPlatformBuilder.BuildAndroid(a_eType, oPlayerOpts, "Release");
	}

	/** 안드로이드를 빌드한다 */
	private static void BuildAndroidReleaseWithAutoPlay(EAndroidType a_eType) {
		CPlatformOptsSetter.AddDefineSymbol(BuildTargetGroup.Android, KCEditorDefine.DS_DEFINE_S_PLAY_TEST_ENABLE);
		CPlatformBuilder.BuildAndroidRelease(a_eType, true);
	}

	/** 안드로이드를 빌드한다 */
	private static void BuildAndroidStoreA(EAndroidType a_eType) {
		CPlatformBuilder.BuildAndroidStore(a_eType, false);
	}

	/** 안드로이드를 빌드한다 */
	private static void BuildAndroidStoreB(EAndroidType a_eType) {
		CPlatformBuilder.BuildAndroidStore(a_eType, true);
	}

	/** 안드로이드를 빌드한다 */
	private static void BuildAndroidStore(EAndroidType a_eType, bool a_bIsBuildAppBundle) {
		CPlatformBuilder.BuildMode = EBuildMode.STORE;
		EditorUserBuildSettings.buildAppBundle = a_bIsBuildAppBundle;

		CPlatformOptsSetter.AddDefineSymbol(BuildTargetGroup.Android, KCEditorDefine.DS_DEFINE_S_STORE_DIST_BUILD);
		CPlatformBuilder.BuildAndroid(a_eType, new BuildPlayerOptions(), "Store");
	}

	/** 안드로이드를 빌드한다 */
	private static void BuildAndroid(EAndroidType a_eType, BuildPlayerOptions a_oPlayerOpts, string buildType = "Debug") {
		CPlatformBuilder.AndroidType = a_eType;

		// 플러그인 파일을 복사한다
		if(!Application.isBatchMode) {
			CFunc.CopyFile(KCEditorDefine.B_SRC_PLUGIN_P_ANDROID, KCEditorDefine.B_DEST_PLUGIN_P_ANDROID, false);
		}

		// 빌드 옵션을 설정한다 {

        // [끌올] 프로젝트 정보 테이블이 존재 할 경우
        if(CPlatformOptsSetter.ProjInfoTable != null) {
            switch(a_eType) {
                case EAndroidType.AMAZON: PlayerSettings.bundleVersion = CPlatformOptsSetter.ProjInfoTable.AndroidAmazonProjInfo.m_stBuildVerInfo.m_oVer; break;
                default: PlayerSettings.bundleVersion = CPlatformOptsSetter.ProjInfoTable.AndroidGoogleProjInfo.m_stBuildVerInfo.m_oVer; break;
            }
        }

        string oProjName = CPlatformOptsSetter.ProjInfoTable.CommonProjInfo.m_oProductName;
		string oPlatform = CAccess.GetAndroidName(a_eType);
        string oBuildMode = buildType;
        string oVersion = PlayerSettings.bundleVersion;
        int oBundleVersion = PlayerSettings.Android.bundleVersionCode;

		string oBuildFileExtension = EditorUserBuildSettings.buildAppBundle ? KCEditorDefine.B_BUILD_FILE_EXTENSION_ANDROID_AAB : KCEditorDefine.B_BUILD_FILE_EXTENSION_ANDROID_APK;

		a_oPlayerOpts.target = BuildTarget.Android;
		a_oPlayerOpts.targetGroup = BuildTargetGroup.Android;

        if (SystemInfo.deviceName.ToUpper().Contains("JENKINS"))
            a_oPlayerOpts.locationPathName = string.Format(KCEditorDefine.B_BUILD_P_FMT_ANDROID, oPlatform, string.Format(KCEditorDefine.B_BUILD_FILE_N_FMT_ANDROID, oPlatform, oBuildFileExtension));
        else
            a_oPlayerOpts.locationPathName = string.Format(KCEditorDefine.B_BUILD_P_FMT_ANDROID, oPlatform, string.Format("{0}_{1}_v{2}_{3}", oProjName, oBuildMode, oVersion, oBundleVersion));
		
		switch(a_eType) {
			case EAndroidType.AMAZON: CPlatformOptsSetter.AddDefineSymbol(a_oPlayerOpts.targetGroup, KCEditorDefine.DS_DEFINE_S_ANDROID_AMAZON_PLATFORM); break;
			default: CPlatformOptsSetter.AddDefineSymbol(a_oPlayerOpts.targetGroup, KCEditorDefine.DS_DEFINE_S_ANDROID_GOOGLE_PLATFORM); break;
		}

		// 프로젝트 정보 테이블이 존재 할 경우
		/*if(CPlatformOptsSetter.ProjInfoTable != null) {
			switch(a_eType) {
				case EAndroidType.AMAZON: PlayerSettings.bundleVersion = CPlatformOptsSetter.ProjInfoTable.AndroidAmazonProjInfo.m_stBuildVerInfo.m_oVer; break;
				default: PlayerSettings.bundleVersion = CPlatformOptsSetter.ProjInfoTable.AndroidGoogleProjInfo.m_stBuildVerInfo.m_oVer; break;
			}
		}*/
		// 빌드 옵션을 설정한다 }

		// 플랫폼을 빌드한다
		CFactory.MakeDirectories(string.Format(KCEditorDefine.B_ABS_BUILD_P_FMT_ANDROID, oPlatform));
		CPlatformBuilder.BuildPlatform(a_oPlayerOpts);
	}

	/** 안드로이드를 원격 빌드한다 */
	private static void RemoteBuildAndroidDebug(EAndroidType a_eType) {
		CPlatformBuilder.ExecuteAndroidJenkinsBuild(a_eType, KCEditorDefine.B_DEBUG_BUILD_FUNC_JENKINS, KCEditorDefine.B_ANDROID_DEBUG_PIPELINE_N_JENKINS, KCEditorDefine.B_BUILD_FILE_EXTENSION_ANDROID_APK);
	}

	/** 안드로이드를 원격 빌드한다 */
	private static void RemoteBuildAndroidRelease(EAndroidType a_eType) {
		CPlatformBuilder.ExecuteAndroidJenkinsBuild(a_eType, KCEditorDefine.B_RELEASE_BUILD_FUNC_JENKINS, KCEditorDefine.B_ANDROID_RELEASE_PIPELINE_N_JENKINS, KCEditorDefine.B_BUILD_FILE_EXTENSION_ANDROID_APK);
	}

	/** 안드로이드를 원격 빌드한다 */
	private static void RemoteBuildAndroidStoreA(EAndroidType a_eType) {
		CPlatformBuilder.ExecuteAndroidJenkinsBuild(a_eType, KCEditorDefine.B_STORE_A_BUILD_FUNC_JENKINS, KCEditorDefine.B_ANDROID_STORE_PIPELINE_N_JENKINS, KCEditorDefine.B_BUILD_FILE_EXTENSION_ANDROID_APK);
	}

	/** 안드로이드를 원격 빌드한다 */
	private static void RemoteBuildAndroidStoreB(EAndroidType a_eType) {
		CPlatformBuilder.ExecuteAndroidJenkinsBuild(a_eType, KCEditorDefine.B_STORE_B_BUILD_FUNC_JENKINS, KCEditorDefine.B_ANDROID_STORE_PIPELINE_N_JENKINS, KCEditorDefine.B_BUILD_FILE_EXTENSION_ANDROID_AAB);
	}

	/** 안드로이드를 원격 빌드한다 */
	private static void RemoteBuildAndroidStoreDistA(EAndroidType a_eType) {
		CPlatformBuilder.ExecuteAndroidJenkinsBuild(a_eType, KCEditorDefine.B_STORE_DIST_BUILD_FUNC_JENKINS, KCEditorDefine.B_ANDROID_STORE_PIPELINE_N_JENKINS, KCEditorDefine.B_BUILD_FILE_EXTENSION_ANDROID_APK);
	}

	/** 안드로이드를 원격 빌드한다 */
	private static void RemoteBuildAndroidStoreDistB(EAndroidType a_eType) {
		CPlatformBuilder.ExecuteAndroidJenkinsBuild(a_eType, KCEditorDefine.B_STORE_DIST_BUILD_FUNC_JENKINS, KCEditorDefine.B_ANDROID_STORE_PIPELINE_N_JENKINS, KCEditorDefine.B_BUILD_FILE_EXTENSION_ANDROID_AAB);
	}
	#endregion // 클래스 함수
}

/** 플랫폼 빌드 - 크리에이티브 */
public static partial class CPlatformBuilder {
	#region 클래스 함수
	/** 구글 크리에이티브를 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Creative/Google/Release", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 2)]
	public static void BuildGoogleCreativeRelease() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformOptsSetter.AddDefineSymbol(BuildTargetGroup.Standalone, KCEditorDefine.DS_DEFINE_S_CREATIVE_DIST_BUILD);

		CPlatformBuilder.BuildAndroidRelease(EAndroidType.GOOGLE, false);
	}

	/** 구글 크리에이티브를 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Creative/Google/Release With AutoPlay", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 2)]
	public static void BuildGoogleCreativeReleaseWithAutoPlay() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformOptsSetter.AddDefineSymbol(BuildTargetGroup.Standalone, KCEditorDefine.DS_DEFINE_S_CREATIVE_DIST_BUILD);

		CPlatformBuilder.BuildAndroidReleaseWithAutoPlay(EAndroidType.GOOGLE);
	}

	/** 아마존 크리에이티브를 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Creative/Amazon/Release", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 2)]
	public static void BuildAmazonCreativeRelease() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformOptsSetter.AddDefineSymbol(BuildTargetGroup.Standalone, KCEditorDefine.DS_DEFINE_S_CREATIVE_DIST_BUILD);

		CPlatformBuilder.BuildAndroidRelease(EAndroidType.AMAZON, false);
	}

	/** 아마존 크리에이티브를 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Creative/Amazon/Release With AutoPlay", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 2)]
	public static void BuildAmazonCreativeReleaseWithAutoPlay() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformOptsSetter.AddDefineSymbol(BuildTargetGroup.Standalone, KCEditorDefine.DS_DEFINE_S_CREATIVE_DIST_BUILD);

		CPlatformBuilder.BuildAndroidReleaseWithAutoPlay(EAndroidType.AMAZON);
	}
	#endregion // 클래스 함수
}

/** 플랫폼 빌더 - 안드로이드 구글 */
public static partial class CPlatformBuilder {
	#region 클래스 함수
	/** 안드로이드 구글을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Android/Google/Debug", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 101)]
	public static void BuildAndroidGoogleDebug() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildAndroidDebug(EAndroidType.GOOGLE, false, false);
	}

	/** 안드로이드 구글을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Android/Google/Debug with AutoPlay", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 101)]
	public static void BuildAndroidGoogleDebugWithAutoPlay() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildAndroidDebugWithAutoPlay(EAndroidType.GOOGLE);
	}

	/** 안드로이드 구글을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Android/Google/Debug with RoboTest", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 101)]
	public static void BuildAndroidGoogleDebugWithRoboTest() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildAndroidDebugWithRoboTest(EAndroidType.GOOGLE);
	}

	/** 안드로이드 구글을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Android/Google/Debug with Profiler", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 101)]
	public static void BuildAndroidGoogleDebugWithProfiler() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildAndroidDebugWithProfiler(EAndroidType.GOOGLE);
	}

	/** 안드로이드 구글을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Android/Google/Release", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 201)]
	public static void BuildAndroidGoogleRelease() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildAndroidRelease(EAndroidType.GOOGLE, false);
	}

	/** 안드로이드 구글을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Android/Google/Release with AutoPlay", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 201)]
	public static void BuildAndroidGoogleReleaseWithAutoPlay() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildAndroidReleaseWithAutoPlay(EAndroidType.GOOGLE);
	}

	/** 안드로이드 구글을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Android/Google/Distribution (APK)", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 301)]
	public static void BuildAndroidGoogleStoreA() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildAndroidStoreA(EAndroidType.GOOGLE);
	}

	/** 안드로이드 구글을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Android/Google/Distribution (AAB)", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 301)]
	public static void BuildAndroidGoogleStoreB() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildAndroidStoreB(EAndroidType.GOOGLE);
	}

	/** 안드로이드 구글을 원격 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Remote (Jenkins)/Android/Google/Debug", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 101)]
	public static void RemoteBuildAndroidGoogleDebug() {
		CPlatformBuilder.RemoteBuildAndroidDebug(EAndroidType.GOOGLE);
	}

	/** 안드로이드 구글을 원격 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Remote (Jenkins)/Android/Google/Release", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 201)]
	public static void RemoteBuildAndroidGoogleRelease() {
		CPlatformBuilder.RemoteBuildAndroidRelease(EAndroidType.GOOGLE);
	}

	/** 안드로이드 구글을 원격 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Remote (Jenkins)/Android/Google/Distribution (APK)", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 301)]
	public static void RemoteBuildAndroidGoogleStoreA() {
		CPlatformBuilder.RemoteBuildAndroidStoreA(EAndroidType.GOOGLE);
	}

	/** 안드로이드 구글을 원격 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Remote (Jenkins)/Android/Google/Distribution (AAB)", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 301)]
	public static void RemoteBuildAndroidGoogleStoreB() {
		CPlatformBuilder.RemoteBuildAndroidStoreB(EAndroidType.GOOGLE);
	}

	/** 안드로이드 구글을 원격 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Remote (Jenkins)/Android/Google/Distribution (Store)", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 301)]
	public static void RemoteBuildAndroidGoogleStoreDist() {
		CPlatformBuilder.RemoteBuildAndroidStoreDistB(EAndroidType.GOOGLE);
	}
	#endregion // 클래스 함수
}

/** 플랫폼 빌더 - 안드로이드 아마존 */
public static partial class CPlatformBuilder {
	#region 클래스 함수
	/** 안드로이드 아마존을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Android/Amazon/Debug", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 101)]
	public static void BuildAndroidAmazonDebug() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildAndroidDebug(EAndroidType.AMAZON, false, false);
	}

	/** 안드로이드 아마존을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Android/Amazon/Debug with AutoPlay", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 101)]
	public static void BuildAndroidAmazonDebugWithAutoPlay() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildAndroidDebugWithAutoPlay(EAndroidType.AMAZON);
	}

	/** 안드로이드 아마존을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Android/Amazon/Debug with RoboTest", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 101)]
	public static void BuildAndroidAmazonDebugWithRoboTest() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildAndroidDebugWithRoboTest(EAndroidType.AMAZON);
	}

	/** 안드로이드 아마존을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Android/Amazon/Debug with Profiler", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 101)]
	public static void BuildAndroidAmazonDebugWithProfiler() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildAndroidDebugWithProfiler(EAndroidType.AMAZON);
	}

	/** 안드로이드 아마존을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Android/Amazon/Release", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 201)]
	public static void BuildAndroidAmazonRelease() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildAndroidRelease(EAndroidType.AMAZON, false);
	}

	/** 안드로이드 아마존을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Android/Amazon/Release with AutoPlay", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 201)]
	public static void BuildAndroidAmazonReleaseWithAutoPlay() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildAndroidReleaseWithAutoPlay(EAndroidType.AMAZON);
	}

	/** 안드로이드 아마존을 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Local/Android/Amazon/Distribution (APK)", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 301)]
	public static void BuildAndroidAmazonStoreA() {
		CPlatformBuilder.BuildMethod = MethodBase.GetCurrentMethod().Name;
		CPlatformBuilder.BuildAndroidStoreA(EAndroidType.AMAZON);
	}

	/** 안드로이드 아마존을 원격 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Remote (Jenkins)/Android/Amazon/Debug", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 101)]
	public static void RemoteBuildAndroidAmazonDebug() {
		CPlatformBuilder.RemoteBuildAndroidDebug(EAndroidType.AMAZON);
	}

	/** 안드로이드 아마존을 원격 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Remote (Jenkins)/Android/Amazon/Release", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 201)]
	public static void RemoteBuildAndroidAmazonRelease() {
		CPlatformBuilder.RemoteBuildAndroidRelease(EAndroidType.AMAZON);
	}

	/** 안드로이드 아마존을 원격 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Remote (Jenkins)/Android/Amazon/Distribution (APK)", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 301)]
	public static void RemoteBuildAndroidAmazonStoreA() {
		CPlatformBuilder.RemoteBuildAndroidStoreA(EAndroidType.AMAZON);
	}

	/** 안드로이드 아마존을 원격 빌드한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_BUILD_BASE + "Remote (Jenkins)/Android/Amazon/Distribution (Store)", false, KCEditorDefine.B_SORTING_O_BUILD_MENU + 301)]
	public static void RemoteBuildAndroidAmazonStoreDist() {
		CPlatformBuilder.RemoteBuildAndroidStoreDistA(EAndroidType.AMAZON);
	}
	#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR

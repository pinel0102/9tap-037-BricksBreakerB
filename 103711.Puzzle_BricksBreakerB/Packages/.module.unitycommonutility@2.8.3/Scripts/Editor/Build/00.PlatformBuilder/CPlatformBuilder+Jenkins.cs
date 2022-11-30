using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;

/** 플랫폼 빌더 - 젠킨스 */
public static partial class CPlatformBuilder {
	#region 클래스 함수
	/** iOS 젠킨스 빌드를 실행한다 */
	public static void ExecuteiOSJenkinsBuild(EiOSType a_eType, string a_oBuildFunc, string a_oPipelineName, string a_oBuildFileExtension, string a_oProfileID, string a_oIPAExportMethod) {
		var oDataDict = new Dictionary<string, string>() {
			[KCEditorDefine.B_KEY_JENKINS_BUNDLE_ID] = CPlatformOptsSetter.ProjInfoTable.GetAppID(CPlatformOptsSetter.ProjInfoTable.iOSAppleProjInfo),
			[KCEditorDefine.B_KEY_JENKINS_PROFILE_ID] = a_oProfileID,
			[KCEditorDefine.B_KEY_JENKINS_IPA_EXPORT_METHOD] = a_oIPAExportMethod
		};

		CPlatformBuilder.ExecuteJenkinsBuild(new STJenkinsParams() {
			m_oSrc = string.Format(KCDefine.B_TEXT_FMT_3_SLASH_COMBINE, CPlatformOptsSetter.BuildInfoTable.JenkinsInfo.m_oSrc, CEditorAccess.GetiOSProjName(a_eType), KCEditorDefine.B_JENKINS_IOS_SOURCES[a_eType][a_oBuildFunc]),
			m_oPipeline = KCEditorDefine.B_JENKINS_IOS_PIPELINE,
			m_oProjName = CEditorAccess.GetiOSProjName(a_eType),
			m_oBuildOutputPath = CEditorAccess.GetiOSBuildOutputPath(a_eType, a_oBuildFileExtension),
			m_oBuildFileExtension = a_oBuildFileExtension,
			m_oPlatform = CAccess.GetiOSName(a_eType),
			m_oProjPlatform = KCEditorDefine.B_PROJ_PLATFORM_N_IOS,
			m_oBuildVer = string.Format(KCDefine.B_TEXT_FMT_2_UNDER_SCORE_COMBINE, CPlatformOptsSetter.ProjInfoTable.iOSAppleProjInfo.m_stBuildVerInfo.m_oVer, CPlatformOptsSetter.ProjInfoTable.iOSAppleProjInfo.m_stBuildVerInfo.m_nNum),
			m_oBuildFunc = a_oBuildFunc,
			m_oPipelineName = a_oPipelineName,
			m_oDataDict = oDataDict
		});
	}

	/** 안드로이드 젠킨스 빌드를 실행한다 */
	public static void ExecuteAndroidJenkinsBuild(EAndroidType a_eType, string a_oBuildFunc, string a_oPipelineName, string a_oBuildFileExtension) {
		var stProjInfo = (a_eType == EAndroidType.AMAZON) ? CPlatformOptsSetter.ProjInfoTable.AndroidGoogleProjInfo : CPlatformOptsSetter.ProjInfoTable.AndroidGoogleProjInfo;

		CPlatformBuilder.ExecuteJenkinsBuild(new STJenkinsParams() {
			m_oSrc = string.Format(KCDefine.B_TEXT_FMT_3_SLASH_COMBINE, CPlatformOptsSetter.BuildInfoTable.JenkinsInfo.m_oSrc, CEditorAccess.GetAndroidProjName(a_eType), KCEditorDefine.B_JENKINS_ANDROID_SOURCES[a_eType][a_oBuildFunc]),
			m_oPipeline = KCEditorDefine.B_JENKINS_ANDROID_PIPELINE,
			m_oProjName = CEditorAccess.GetAndroidProjName(a_eType),
			m_oBuildOutputPath = CEditorAccess.GetAndroidBuildOutputPath(a_eType, a_oBuildFileExtension),
			m_oBuildFileExtension = a_oBuildFileExtension,
			m_oPlatform = CAccess.GetAndroidName(a_eType),
			m_oProjPlatform = KCEditorDefine.B_PROJ_PLATFORM_N_ANDROID,
			m_oBuildVer = string.Format(KCDefine.B_TEXT_FMT_2_UNDER_SCORE_COMBINE, stProjInfo.m_stBuildVerInfo.m_oVer, stProjInfo.m_stBuildVerInfo.m_nNum),
			m_oBuildFunc = a_oBuildFunc,
			m_oPipelineName = a_oPipelineName,
			m_oDataDict = null
		});
	}

	/** 독립 플랫폼 젠킨스 빌드를 실행한다 */
	public static void ExecuteStandaloneJenkinsBuild(EStandaloneType a_eType, string a_oBuildFunc, string a_oPipelineName, string a_oBuildFileExtension) {
		var stProjInfo = (a_eType == EStandaloneType.WNDS_STEAM) ? CPlatformOptsSetter.ProjInfoTable.StandaloneWndsSteamProjInfo : CPlatformOptsSetter.ProjInfoTable.StandaloneMacSteamProjInfo;

		CPlatformBuilder.ExecuteJenkinsBuild(new STJenkinsParams() {
			m_oSrc = string.Format(KCDefine.B_TEXT_FMT_3_SLASH_COMBINE, CPlatformOptsSetter.BuildInfoTable.JenkinsInfo.m_oSrc, CEditorAccess.GetStandaloneProjName(a_eType), KCEditorDefine.B_JENKINS_STANDALONE_SOURCES[a_eType][a_oBuildFunc]),
			m_oPipeline = KCEditorDefine.B_JENKINS_STANDALONE_PIPELINE,
			m_oProjName = CEditorAccess.GetStandaloneProjName(a_eType),
			m_oBuildOutputPath = CEditorAccess.GetStandaloneBuildOutputPath(a_eType, a_oBuildFileExtension),
			m_oBuildFileExtension = a_oBuildFileExtension,
			m_oPlatform = CAccess.GetStandaloneName(a_eType),
			m_oProjPlatform = (a_eType == EStandaloneType.WNDS_STEAM) ? KCEditorDefine.B_PROJ_PLATFORM_N_STANDALONE_WNDS : KCEditorDefine.B_PROJ_PLATFORM_N_STANDALONE_MAC,
			m_oBuildVer = string.Format(KCDefine.B_TEXT_FMT_2_UNDER_SCORE_COMBINE, stProjInfo.m_stBuildVerInfo.m_oVer, stProjInfo.m_stBuildVerInfo.m_nNum),
			m_oBuildFunc = a_oBuildFunc,
			m_oPipelineName = a_oPipelineName,
			m_oDataDict = null
		});
	}

	/** 젠킨스 빌드를 실행한다 */
	private static void ExecuteJenkinsBuild(STJenkinsParams a_stParams) {
		string oURL = string.Format(CPlatformOptsSetter.BuildInfoTable.JenkinsInfo.m_oBuildURLFmt, a_stParams.m_oPipeline);
		string oUserID = CPlatformOptsSetter.BuildInfoTable.JenkinsInfo.m_oUserID;
		string oAccessToken = CPlatformOptsSetter.BuildInfoTable.JenkinsInfo.m_oAccessToken;
		string oBuildToken = CPlatformOptsSetter.BuildInfoTable.JenkinsInfo.m_oBuildToken;
		string oProjRoot = CPlatformOptsSetter.BuildInfoTable.JenkinsInfo.m_oProjRoot;

		// 매개 변수를 설정한다 {
		var oDataDict = a_stParams.m_oDataDict ?? new Dictionary<string, string>();
		oDataDict.TryAdd(KCEditorDefine.B_KEY_JENKINS_PROJ_PART, KCEditorDefine.B_TOKEN_CLIENT);
		oDataDict.TryAdd(KCEditorDefine.B_KEY_JENKINS_MODULE_VER, KCEditorDefine.B_VER_UNITY_MODULE);
		oDataDict.TryAdd(KCEditorDefine.B_KEY_JENKINS_BRANCH, string.Format(KCEditorDefine.B_BRANCH_FMT_JENKINS, CPlatformOptsSetter.BuildInfoTable.JenkinsInfo.m_oBranch));
		oDataDict.TryAdd(KCEditorDefine.B_KEY_JENKINS_SRC, a_stParams.m_oSrc);
		oDataDict.TryAdd(KCEditorDefine.B_KEY_JENKINS_ANALYTICS_SRC, string.Format(KCEditorDefine.B_ANALYTICS_FMT_JENKINS, CPlatformOptsSetter.BuildInfoTable.JenkinsInfo.m_oSrc));
		oDataDict.TryAdd(KCEditorDefine.B_KEY_JENKINS_PROJ_NAME, CPlatformOptsSetter.ProjInfoTable.CommonProjInfo.m_oProjName);
		oDataDict.TryAdd(KCEditorDefine.B_KEY_JENKINS_PROJ_PATH, oProjRoot.ExIsValid() ? string.Format(KCDefine.B_TEXT_FMT_4_SLASH_COMBINE, CPlatformOptsSetter.BuildInfoTable.JenkinsInfo.m_oWorkspace, a_stParams.m_oSrc, oProjRoot, CPlatformOptsSetter.ProjInfoTable.CommonProjInfo.m_oProjName) : string.Format(KCDefine.B_TEXT_FMT_3_SLASH_COMBINE, CPlatformOptsSetter.BuildInfoTable.JenkinsInfo.m_oWorkspace, a_stParams.m_oSrc, CPlatformOptsSetter.ProjInfoTable.CommonProjInfo.m_oProjName));
		oDataDict.TryAdd(KCEditorDefine.B_KEY_JENKINS_BUILD_OUTPUT_PATH, a_stParams.m_oBuildOutputPath);
		oDataDict.TryAdd(KCEditorDefine.B_KEY_JENKINS_PLATFORM, a_stParams.m_oPlatform);
		oDataDict.TryAdd(KCEditorDefine.B_KEY_JENKINS_PROJ_PLATFORM, a_stParams.m_oProjPlatform);
		oDataDict.TryAdd(KCEditorDefine.B_KEY_JENKINS_BUILD_VER, a_stParams.m_oBuildVer);
		oDataDict.TryAdd(KCEditorDefine.B_KEY_JENKINS_BUILD_FUNC, a_stParams.m_oBuildFunc);
		oDataDict.TryAdd(KCEditorDefine.B_KEY_JENKINS_PIPELINE_NAME, a_stParams.m_oPipelineName);
		oDataDict.TryAdd(KCEditorDefine.B_KEY_JENKINS_BUILD_FILE_EXTENSION, a_stParams.m_oBuildFileExtension);

		var oStrBuilder = new System.Text.StringBuilder();
		oStrBuilder.AppendFormat(KCEditorDefine.B_BUILD_CMD_FMT_JENKINS, oURL, oUserID, oAccessToken, oBuildToken);

		foreach(var stKeyVal in oDataDict) {
			oStrBuilder.AppendFormat(string.Format(KCDefine.B_TEXT_FMT_2_COMBINE, KCDefine.B_TOKEN_SPACE, KCEditorDefine.B_BUILD_DATA_FMT_JENKINS), stKeyVal.Key, stKeyVal.Value);
		}
		// 매개 변수를 설정한다 }

		CEditorFunc.ExecuteCmdLine(oStrBuilder.ToString());
	}
	#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR

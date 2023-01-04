using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif // #if UNITY_EDITOR

/** 젠킨스 정보 */
[System.Serializable]
public struct STJenkinsInfo {
	public string m_oUserID;
	public string m_oBranch;

	public string m_oSrc;
	public string m_oProjRoot;
	public string m_oWorkspace;

	public string m_oBuildToken;
	public string m_oAccessToken;
	public string m_oBuildURLFmt;
}

/** 공용 빌드 정보 */
[System.Serializable]
public struct STCommonBuildInfo {
	// Do Something
}

/** iOS 빌드 정보 */
[System.Serializable]
public struct STiOSBuildInfo {
	public string m_oTeamID;
	public string m_oTargetOSVer;

	public string m_oDevProfileID;
	public string m_oStoreProfileID;
}

/** 안드로이드 빌드 정보 */
[System.Serializable]
public struct STAndroidBuildInfo {
	public string m_oKeystorePath;
	public string m_oKeyaliasName;

	public string m_oKeystorePassword;
	public string m_oKeyaliasPassword;

#if UNITY_EDITOR
	public AndroidSdkVersions m_eMinSDKVer;
	public AndroidSdkVersions m_eTargetSDKVer;
#endif // #if UNITY_EDITOR
}

/** 독립 플랫폼 빌드 정보 */
[System.Serializable]
public struct STStandaloneBuildInfo {
	public string m_oCategory;
	public string m_oTargetOSVer;
}

/** 빌드 정보 테이블 */
public partial class CBuildInfoTable : CScriptableObj<CBuildInfoTable> {
	#region 변수
	[Header("=====> Jenkins Info <=====")]
	[SerializeField] private STJenkinsInfo m_stJenkinsInfo;

	[Header("=====> Platform Build Info <=====")]
	[SerializeField] private STCommonBuildInfo m_stCommonBuildInfo;
	[SerializeField] private STiOSBuildInfo m_stiOSBuildInfo;
	[SerializeField] private STAndroidBuildInfo m_stAndroidBuildInfo;
	[SerializeField] private STStandaloneBuildInfo m_stStandaloneBuildInfo;
	#endregion // 변수

	#region 프로퍼티
	public STJenkinsInfo JenkinsInfo => m_stJenkinsInfo;
	public STCommonBuildInfo CommonBuildInfo => m_stCommonBuildInfo;
	public STiOSBuildInfo iOSBuildInfo => m_stiOSBuildInfo;
	public STAndroidBuildInfo AndroidBuildInfo => m_stAndroidBuildInfo;
	public STStandaloneBuildInfo StandaloneBuildInfo => m_stStandaloneBuildInfo;
	#endregion // 프로퍼티

	#region 조건부 함수
#if UNITY_EDITOR
	/** 젠킨스 정보를 변경한다 */
	public void SetJenkinsInfo(STJenkinsInfo a_stJenkinsInfo) {
		m_stJenkinsInfo = a_stJenkinsInfo;
	}

	/** 공용 빌드 정보를 변경한다 */
	public void SetCommonBuildInfo(STCommonBuildInfo a_stBuildInfo) {
		m_stCommonBuildInfo = a_stBuildInfo;
	}

	/** iOS 빌드 정보를 변경한다 */
	public void SetiOSBuildInfo(STiOSBuildInfo a_stBuildInfo) {
		m_stiOSBuildInfo = a_stBuildInfo;
	}

	/** 안드로이드 빌드 정보를 변경한다 */
	public void SetAndroidBuildInfo(STAndroidBuildInfo a_stBuildInfo) {
		m_stAndroidBuildInfo = a_stBuildInfo;
	}

	/** 독립 플랫폼 빌드 정보를 변경한다 */
	public void SetStandaloneBuildInfo(STStandaloneBuildInfo a_stBuildInfo) {
		m_stStandaloneBuildInfo = a_stBuildInfo;
	}
#endif // #if UNITY_EDITOR
	#endregion // 조건부 함수
}

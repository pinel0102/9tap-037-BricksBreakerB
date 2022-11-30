using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** 프로젝트 정보 */
[System.Serializable]
public struct STProjInfo {
	public string m_oAppID;
	public string m_oStoreAppID;

	public STBuildVerInfo m_stBuildVerInfo;
}

/** 회사 정보 */
[System.Serializable]
public struct STCompanyInfo {
	public string m_oCompany;
	public string m_oPrivacyURL;
	public string m_oServicesURL;
	public string m_oSupportsMail;
}

/** 공용 프로젝트 정보 */
[System.Serializable]
public struct STCommonProjInfo {
	public string m_oAppID;
	public string m_oProjName;
	public string m_oProductName;
	public string m_oExtraProjDirName;
}

/** 프로젝터 정보 테이블 */
public partial class CProjInfoTable : CScriptableObj<CProjInfoTable> {
	#region 변수
	[Header("=====> Company Info <=====")]
	[SerializeField] private STCompanyInfo m_stCompanyInfo;

	[Header("=====> Common Proj Info <=====")]
	[SerializeField] private STCommonProjInfo m_stCommonProjInfo;

	[Header("=====> iOS Proj Info <=====")]
	[SerializeField] private STProjInfo m_stiOSAppleProjInfo;

	[Header("=====> Android Proj Info <=====")]
	[SerializeField] private STProjInfo m_stAndroidGoogleProjInfo;
	[SerializeField] private STProjInfo m_stAndroidAmazonProjInfo;

	[Header("=====> Standalone Proj Info <=====")]
	[SerializeField] private STProjInfo m_stStandaloneMacSteamProjInfo;
	[SerializeField] private STProjInfo m_stStandaloneWndsSteamProjInfo;
	#endregion // 변수

	#region 프로퍼티
	public STProjInfo ProjInfo { get; private set; }
	public STCompanyInfo CompanyInfo => m_stCompanyInfo;
	public STCommonProjInfo CommonProjInfo => m_stCommonProjInfo;

	public STProjInfo iOSAppleProjInfo => m_stiOSAppleProjInfo;
	public STProjInfo AndroidGoogleProjInfo => m_stAndroidGoogleProjInfo;
	public STProjInfo AndroidAmazonProjInfo => m_stAndroidAmazonProjInfo;

	public STProjInfo StandaloneMacSteamProjInfo => m_stStandaloneMacSteamProjInfo;
	public STProjInfo StandaloneWndsSteamProjInfo => m_stStandaloneWndsSteamProjInfo;
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

#if UNITY_IOS
		this.ProjInfo = m_stiOSAppleProjInfo;
#elif UNITY_ANDROID
#if ANDROID_AMAZON_PLATFORM
		this.ProjInfo = m_stAndroidAmazonProjInfo;
#else
		this.ProjInfo = m_stAndroidGoogleProjInfo;
#endif // #if ANDROID_AMAZON_PLATFORM
#elif UNITY_STANDALONE
#if STANDALONE_WNDS_STEAM_PLATFORM
		this.ProjInfo = m_stStandaloneWndsSteamProjInfo;
#else
		this.ProjInfo = m_stStandaloneMacSteamProjInfo;
#endif // #if STANDALONE_MAC_STEAM_PLATFORM
#endif // #if UNITY_IOS
	}

	/** 앱 식별자를 반환한다 */
	public string GetAppID(STProjInfo a_stProjInfo) {
		return a_stProjInfo.m_oAppID.ExIsValid() ? a_stProjInfo.m_oAppID : m_stCommonProjInfo.m_oAppID;
	}
	#endregion // 함수

	#region 조건부 함수
#if UNITY_EDITOR
	/** 회사 정보를 변경한다 */
	public void SetCompanyInfo(STCompanyInfo a_stCompanyInfo) {
		m_stCompanyInfo = a_stCompanyInfo;
	}

	/** 공용 프로젝트 정보를 변경한다 */
	public void SetCommonProjInfo(STCommonProjInfo a_stProjInfo) {
		m_stCommonProjInfo = a_stProjInfo;
	}

	/** iOS 애플 프로젝트 정보를 변경한다 */
	public void SetiOSAppleProjInfo(STProjInfo a_stProjInfo) {
		m_stiOSAppleProjInfo = a_stProjInfo;
	}

	/** 안드로이드 구글 프로젝트 정보를 변경한다 */
	public void SetAndroidGoogleProjInfo(STProjInfo a_stProjInfo) {
		m_stAndroidGoogleProjInfo = a_stProjInfo;
	}

	/** 안드로이드 아마존 프로젝트 정보를 변경한다 */
	public void SetAndroidAmazonProjInfo(STProjInfo a_stProjInfo) {
		m_stAndroidAmazonProjInfo = a_stProjInfo;
	}

	/** 맥 스팀 프로젝트 정보를 변경한다 */
	public void SetStandaloneMacSteamProjInfo(STProjInfo a_stProjInfo) {
		m_stStandaloneMacSteamProjInfo = a_stProjInfo;
	}

	/** 윈도우즈 스팀 프로젝트 정보를 변경한다 */
	public void SetStandaloneWndsSteamProjInfo(STProjInfo a_stProjInfo) {
		m_stStandaloneWndsSteamProjInfo = a_stProjInfo;
	}
#endif // #if UNITY_EDITOR
	#endregion // 조건부 함수
}

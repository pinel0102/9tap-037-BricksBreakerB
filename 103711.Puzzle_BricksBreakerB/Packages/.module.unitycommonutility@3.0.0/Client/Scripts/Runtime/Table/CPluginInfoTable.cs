using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if ADS_MODULE_ENABLE
#if ADMOB_ADS_ENABLE
/** 애드 몹 플러그인 정보 */
[System.Serializable]
public struct STAdmobPluginInfo {
	public string m_oBannerAdsID;
	public string m_oRewardAdsID;
	public string m_oFullscreenAdsID;
}
#endif // #if ADMOB_ADS_ENABLE

#if IRON_SRC_ADS_ENABLE
/** 아이언 소스 플러그인 정보 */
[System.Serializable]
public struct STIronSrcPluginInfo {
	public string m_oAppKey;
}
#endif // #if IRON_SRC_ADS_ENABLE
#endif // #if ADS_MODULE_ENABLE

#if APPS_FLYER_MODULE_ENABLE
/** 앱스 플라이어 플러그인 정보 */
[System.Serializable]
public struct STAppsFlyerPluginInfo {
	public string m_oDevKey;
}
#endif // #if APPS_FLYER_MODULE_ENABLE

/** 플러그인 정보 테이블 */
public partial class CPluginInfoTable : CScriptableObj<CPluginInfoTable> {
	#region 변수
#if ADS_MODULE_ENABLE
	[Header("=====> Ads Plugin Info <=====")]
	[SerializeField] private EAdsPlatform m_eAdsPlatform = EAdsPlatform.NONE;
	[SerializeField] private EBannerAdsPos m_eBannerAdsPos = EBannerAdsPos.NONE;

#if ADMOB_ADS_ENABLE
	[Header("Admob Plugin Info")]
	[SerializeField] private STAdmobPluginInfo m_stiOSAdmobPluginInfo;
	[SerializeField] private STAdmobPluginInfo m_stAndroidAdmobPluginInfo;
#endif // #if ADMOB_ADS_ENABLE

#if IRON_SRC_ADS_ENABLE
	[Header("Iron Src Plugin Info")]
	[SerializeField] private bool m_bIsEnableIronSrcBannerAds;
	[SerializeField] private bool m_bIsEnableIronSrcRewardAds;
	[SerializeField] private bool m_bIsEnableIronSrcFullscreenAds;

	[SerializeField] private STIronSrcPluginInfo m_stiOSIronSrcPluginInfo;
	[SerializeField] private STIronSrcPluginInfo m_stAndroidIronSrcPluginInfo;
#endif // #if IRON_SRC_ADS_ENABLE
#endif // #if ADS_MODULE_ENABLE

#if FLURRY_MODULE_ENABLE
	[Header("=====> Flurry Plugin Info <=====")]
	[SerializeField] private string m_oiOSFlurryAPIKey = string.Empty;
	[SerializeField] private string m_oAndroidFlurryAPIKey = string.Empty;
#endif // #if FLURRY_MODULE_ENABLE

#if APPS_FLYER_MODULE_ENABLE
	[Header("=====> Apps Flyer Plugin Info <=====")]
	[SerializeField] private STAppsFlyerPluginInfo m_stAppsFlyerPluginInfo;
#endif // #if APPS_FLYER_MODULE_ENABLE
	#endregion // 변수

	#region 프로퍼티
#if ADS_MODULE_ENABLE
	public EAdsPlatform AdsPlatform => m_eAdsPlatform;
	public EBannerAdsPos BannerAdsPos => m_eBannerAdsPos;

#if ADMOB_ADS_ENABLE
	public STAdmobPluginInfo AdmobPluginInfo { get; private set; }
#endif // #if ADMOB_ADS_ENABLE

#if IRON_SRC_ADS_ENABLE
	public STIronSrcPluginInfo IronSrcPluginInfo { get; private set; }
#endif // #if IRON_SRC_ADS_ENABLE
#endif // #if ADS_MODULE_ENABLE

#if FLURRY_MODULE_ENABLE
	public string FlurryAPIKey { get; private set; } = string.Empty;
#endif // #if FLURRY_MODULE_ENABLE

#if APPS_FLYER_MODULE_ENABLE
	public STAppsFlyerPluginInfo AppsFlyerPluginInfo => m_stAppsFlyerPluginInfo;
#endif // #if APPS_FLYER_MODULE_ENABLE
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

#if ADS_MODULE_ENABLE
#if ADMOB_ADS_ENABLE
		STAdmobPluginInfo stAdmobPluginInfo;

#if UNITY_IOS
		stAdmobPluginInfo = m_stiOSAdmobPluginInfo;
#else
		stAdmobPluginInfo = m_stAndroidAdmobPluginInfo;
#endif // #if UNITY_IOS

#if ADS_TEST_ENABLE
		stAdmobPluginInfo.m_oBannerAdsID = KCDefine.U_ADS_ID_ADMOB_TEST_BANNER_ADS;
		stAdmobPluginInfo.m_oRewardAdsID = KCDefine.U_ADS_ID_ADMOB_TEST_REWARD_ADS;
		stAdmobPluginInfo.m_oFullscreenAdsID = KCDefine.U_ADS_ID_ADMOB_TEST_FULLSCREEN_ADS;
#endif // #if ADS_TEST_ENABLE

		this.AdmobPluginInfo = stAdmobPluginInfo;
#endif // #if ADMOB_ADS_ENABLE

#if IRON_SRC_ADS_ENABLE
#if UNITY_IOS
		this.IronSrcPluginInfo = m_stiOSIronSrcPluginInfo;
#else
		this.IronSrcPluginInfo = m_stAndroidIronSrcPluginInfo;
#endif // #if UNITY_IOS
#endif // #if IRON_SRC_ADS_ENABLE
#endif // #if ADS_MODULE_ENABLE

#if FLURRY_MODULE_ENABLE
#if UNITY_IOS
		this.FlurryAPIKey = m_oiOSFlurryAPIKey;
#else
		this.FlurryAPIKey = m_oAndroidFlurryAPIKey;
#endif // #if UNITY_IOS
#endif // #if FLURRY_MODULE_ENABLE
	}
	#endregion // 함수

	#region 조건부 함수
#if ADS_MODULE_ENABLE
	/** 배너 광고 식별자를 반환한다 */
	public string GetBannerAdsID(EAdsPlatform a_eAdsPlatform) {
		CAccess.Assert(a_eAdsPlatform.ExIsValid());
		var oBannerAdsIDDict = CCollectionManager.Inst.SpawnDict<EAdsPlatform, string>();

		try {
#if ADMOB_ADS_ENABLE
			oBannerAdsIDDict.TryAdd(EAdsPlatform.ADMOB, this.AdmobPluginInfo.m_oBannerAdsID ?? string.Empty);
#endif // #if ADMOB_ADS_ENABLE

#if IRON_SRC_ADS_ENABLE
			oBannerAdsIDDict.TryAdd(EAdsPlatform.IRON_SRC, m_bIsEnableIronSrcBannerAds ? IronSourceAdUnits.BANNER : string.Empty);
#endif // #if IRON_SRC_ADS_ENABLE

			return oBannerAdsIDDict.GetValueOrDefault(a_eAdsPlatform, string.Empty);
		} finally {
			CCollectionManager.Inst.DespawnDict(oBannerAdsIDDict);
		}
	}

	/** 보상 광고 식별자를 반환한다 */
	public string GetRewardAdsID(EAdsPlatform a_eAdsPlatform) {
		CAccess.Assert(a_eAdsPlatform.ExIsValid());
		var oRewardAdsIDDict = CCollectionManager.Inst.SpawnDict<EAdsPlatform, string>();

		try {
#if ADMOB_ADS_ENABLE
			oRewardAdsIDDict.TryAdd(EAdsPlatform.ADMOB, this.AdmobPluginInfo.m_oRewardAdsID ?? string.Empty);
#endif // #if ADMOB_ADS_ENABLE

#if IRON_SRC_ADS_ENABLE
			oRewardAdsIDDict.TryAdd(EAdsPlatform.IRON_SRC, m_bIsEnableIronSrcRewardAds ? IronSourceAdUnits.REWARDED_VIDEO : string.Empty);
#endif // #if IRON_SRC_ADS_ENABLE

			return oRewardAdsIDDict.GetValueOrDefault(a_eAdsPlatform, string.Empty);
		} finally {
			CCollectionManager.Inst.DespawnDict(oRewardAdsIDDict);
		}
	}

	/** 전면 광고 식별자를 반환한다 */
	public string GetFullscreenAdsID(EAdsPlatform a_eAdsPlatform) {
		CAccess.Assert(a_eAdsPlatform.ExIsValid());
		var oFullscreenAdsIDDict = CCollectionManager.Inst.SpawnDict<EAdsPlatform, string>();

		try {
#if ADMOB_ADS_ENABLE
			oFullscreenAdsIDDict.TryAdd(EAdsPlatform.ADMOB, this.AdmobPluginInfo.m_oFullscreenAdsID ?? string.Empty);
#endif // #if ADMOB_ADS_ENABLE

#if IRON_SRC_ADS_ENABLE
			oFullscreenAdsIDDict.TryAdd(EAdsPlatform.IRON_SRC, m_bIsEnableIronSrcFullscreenAds ? IronSourceAdUnits.INTERSTITIAL : string.Empty);
#endif // #if IRON_SRC_ADS_ENABLE

			return oFullscreenAdsIDDict.GetValueOrDefault(a_eAdsPlatform, string.Empty);
		} finally {
			CCollectionManager.Inst.DespawnDict(oFullscreenAdsIDDict);
		}
	}
#endif // #if ADS_MODULE_ENABLE

#if UNITY_EDITOR
#if ADS_MODULE_ENABLE
	/** 광고 플랫폼을 변경한다 */
	public void SetAdsPlatform(EAdsPlatform a_eAdsPlatform) {
		m_eAdsPlatform = a_eAdsPlatform;
	}

	/** 배너 광고 위치를 변경한다 */
	public void SetBannerAdsPos(EBannerAdsPos a_eAdsPos) {
		m_eBannerAdsPos = a_eAdsPos;
	}

#if ADMOB_ADS_ENABLE
	/** iOS 애드 몹 플러그인 정보를 변경한다 */
	public void SetiOSAdmobPluginInfo(STAdmobPluginInfo a_stPluginInfo) {
		m_stiOSAdmobPluginInfo = a_stPluginInfo;
	}

	/** 안드로이드 애드 몹 플러그인 정보를 변경한다 */
	public void SetAndroidAdmobPluginInfo(STAdmobPluginInfo a_stPluginInfo) {
		m_stAndroidAdmobPluginInfo = a_stPluginInfo;
	}
#endif // #if ADMOB_ADS_ENABLE

#if IRON_SRC_ADS_ENABLE
	/** iOS 아이언 소스 배너 광고 여부를 변경한다 */
	public void SetEnableIronSrcBannerAds(bool a_bIsEnable) {
		m_bIsEnableIronSrcBannerAds = a_bIsEnable;
	}

	/** iOS 아이언 소스 보상 광고 여부를 변경한다 */
	public void SetEnableIronSrcRewardAds(bool a_bIsEnable) {
		m_bIsEnableIronSrcRewardAds = a_bIsEnable;
	}

	/** iOS 아이언 소스 전면 광고 여부를 변경한다 */
	public void SetEnableIronSrcFullscreenAds(bool a_bIsEnable) {
		m_bIsEnableIronSrcFullscreenAds = a_bIsEnable;
	}

	/** iOS 아이언 소스 플러그인 정보를 변경한다 */
	public void SetiOSIronSrcPluginInfo(STIronSrcPluginInfo a_stPluginInfo) {
		m_stiOSIronSrcPluginInfo = a_stPluginInfo;
	}

	/** 안드로이드 아이언 소스 플러그인 정보를 변경한다 */
	public void SetAndroidIronSrcPluginInfo(STIronSrcPluginInfo a_stPluginInfo) {
		m_stAndroidIronSrcPluginInfo = a_stPluginInfo;
	}
#endif // #if IRON_SRC_ADS_ENABLE
#endif // #if ADS_MODULE_ENABLE

#if FLURRY_MODULE_ENABLE
	/** iOS 플러리 API 키를 변경한다 */
	public void SetiOSFlurryAPIKey(string a_oKey) {
		m_oiOSFlurryAPIKey = a_oKey;
	}

	/** 안드로이드 플러리 API 키를 변경한다 */
	public void SetAndroidFlurryAPIKey(string a_oKey) {
		m_oAndroidFlurryAPIKey = a_oKey;
	}
#endif // #if FLURRY_MODULE_ENABLE

#if APPS_FLYER_MODULE_ENABLE
	/** 앱스 플라이어 플러그인 정보를 변경한다 */
	public void SetAppsFlyerPluginInfo(STAppsFlyerPluginInfo a_stPluginInfo) {
		m_stAppsFlyerPluginInfo = a_stPluginInfo;
	}
#endif // #if APPS_FLYER_MODULE_ENABLE
#endif // #if UNITY_EDITOR
	#endregion // 조건부 함수
}

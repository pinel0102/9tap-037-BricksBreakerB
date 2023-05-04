using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if ADS_MODULE_ENABLE
#if ADMOB_ADS_ENABLE
using GoogleMobileAds.Api;
#endif // #if ADMOB_ADS_ENABLE

/** 광고 관리자 - 애드 몹 */
public partial class CAdsManager : CSingleton<CAdsManager> {
#region 함수
	/** 애드 몹 배너 광고를 로드한다 */
	private void LoadAdmobBannerAds() {
		CAccess.Assert(this.IsValidAdmobBannerAdsID());

#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
		this.GetAdmobBannerAds().LoadAd(this.GetAdmobRequestBuilder().Build());
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
	}

	/** 애드 몹 보상 광고를 로드한다 */
	private void LoadAdmobRewardAds() {
		CAccess.Assert(this.IsValidAdmobRewardAdsID());

#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
		this.GetAdmobRewardAds().LoadAd(this.GetAdmobRequestBuilder().Build());
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
	}

	/** 애드 몹 전면 광고를 로드한다 */
	private void LoadAdmobFullscreenAds() {
		CAccess.Assert(this.IsValidAdmobFullscreenAdsID());

#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
		this.GetAdmobFullscreenAds().LoadAd(this.GetAdmobRequestBuilder().Build());
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
	}

	/** 애드 몹 배너 광고를 출력한다 */
	private void ShowAdmobBannerAds() {
		CAccess.Assert(this.IsValidAdmobBannerAdsID() && this.Params.m_eAdsPlatform == EAdsPlatform.ADMOB);

#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
		this.GetAdmobBannerAds().Show();
		this.SetupBannerAdsHeight(this.GetAdmobBannerAds().GetHeightInPixels().ExPixelsToDPIPixels());
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
	}

	/** 애드 몹 보상 광고를 출력한다 */
	private void ShowAdmobRewardAds() {
		CAccess.Assert(this.IsValidAdmobRewardAdsID());

#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
		this.GetAdmobRewardAds().Show();
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
	}

	/** 애드 몹 전면 광고를 출력한다 */
	private void ShowAdmobFullscreenAds() {
		CAccess.Assert(this.IsValidAdmobFullscreenAdsID());

#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
		this.GetAdmobFullscreenAds().Show();
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
	}

	/** 애드 몹 배너 광고를 닫는다 */
	private void CloseAdmobBannerAds() {
		CAccess.Assert(this.IsValidAdmobBannerAdsID());

#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
		this.GetAdmobBannerAds()?.Hide();
		this.GetAdmobBannerAds()?.Destroy();

		m_oAdmobBannerAds = null;

		m_oBoolDict[EKey.IS_LOAD_ADMOB_BANNER_ADS] = false;
		m_oRealDict[EKey.BANNER_ADS_HEIGHT] = KCDefine.B_VAL_0_REAL;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
	}
#endregion // 함수

#region 접근자 함수
	/** 애드 몹 초기화 여부를 검사한다 */
	private bool IsInitAdmob() {
#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
		return m_oBoolDict[EKey.IS_INIT_ADMOB];
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
	}

	/** 애드 몹 광고 식별자 유효 여부를 검사한다 */
	private bool IsValidAdmobAdsID(string a_oID) {
		CAccess.Assert(a_oID.ExIsValid());

#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
		return this.Params.m_oAdmobAdsIDDict.TryGetValue(a_oID, out string oAdsID) && oAdsID.ExIsValid();
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
	}

	/** 애드 몹 배너 광고 식별자 유효 여부를 검사한다 */
	private bool IsValidAdmobBannerAdsID() {
#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
		return this.IsValidAdmobAdsID(KCDefine.U_KEY_ADS_M_BANNER_ADS_ID);
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
	}

	/** 애드 몹 보상 광고 식별자 유효 여부를 검사한다 */
	private bool IsValidAdmobRewardAdsID() {
#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
		return this.IsValidAdmobAdsID(KCDefine.U_KEY_ADS_M_REWARD_ADS_ID);
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
	}

	/** 애드 몹 전면 광고 식별자 유효 여부를 검사한다 */
	private bool IsValidAdmobFullscreenAdsID() {
#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
		return this.IsValidAdmobAdsID(KCDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID);
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
	}

	/** 애드 몹 배너 광고 로드 여부를 검사한다 */
	private bool IsLoadAdmobBannerAds() {
#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
		return m_oAdmobBannerAds != null && m_oBoolDict[EKey.IS_LOAD_ADMOB_BANNER_ADS];
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
	}

	/** 애드 몹 보상 광고 로드 여부를 검사한다 */
	private bool IsLoadAdmobRewardAds() {
#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
		return m_oAdmobRewardAds != null && m_oAdmobRewardAds.IsLoaded();
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
	}

	/** 애드 몹 전면 광고 로드 여부를 검사한다 */
	private bool IsLoadAdmobFullscreenAds() {
#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
		return m_oAdmobFullscreenAds != null && m_oAdmobFullscreenAds.IsLoaded();
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
	}
#endregion // 접근자 함수

#region 조건부 함수
#if(UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
	/** 애드 몹이 초기화 되었을 경우 */
	private void OnInitAdmob(InitializationStatus a_oStatus) {
		string oStr = a_oStatus.getAdapterStatusMap().ExToStr(KCDefine.B_TOKEN_COMMA);
		CFunc.ShowLog($"CAdsManager.OnInitAdmob: {oStr}", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_ADMOB_INIT_CALLBACK, () => {
			m_oBoolDict[EKey.IS_INIT] = true;
			m_oBoolDict[EKey.IS_INIT_ADMOB] = true;

			var oBuilder = new RequestConfiguration.Builder();
			oBuilder.SetTestDeviceIds(this.Params.m_oAdmobTestDeviceIDList);

			MobileAds.SetRequestConfiguration(oBuilder.build());
			this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.INIT)?.Invoke(this, EAdsPlatform.ADMOB, m_oBoolDict[EKey.IS_INIT_ADMOB]);
		});
	}

	/** 애드 몹 배너 광고가 로드 되었을 경우 */
	private void OnLoadAdmobBannerAds(object a_oSender, System.EventArgs a_oArgs) {
		CFunc.ShowLog("CAdsManager.OnLoadAdmobBannerAds", KCDefine.B_LOG_COLOR_PLUGIN);
		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_ADMOB_BANNER_ADS_LOAD_CALLBACK, () => { m_oBoolDict[EKey.IS_LOAD_ADMOB_BANNER_ADS] = true; this.HandleLoadAdmobBannerAdsResult(); });
	}

	/** 애드 몹 배너 광고 로드에 실패했을 경우 */
	private void OnLoadFailAdmobBannerAds(object a_oSender, AdFailedToLoadEventArgs a_oArgs) {
		CFunc.ShowLog($"CAdsManager.OnLoadFailAdmobBannerAds: {a_oArgs.LoadAdError.GetMessage()}", KCDefine.B_LOG_COLOR_PLUGIN);
		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_ADMOB_BANNER_ADS_LOAD_FAIL_CALLBACK, () => this.AddLoadFailBannerAdsInfo(EAdsPlatform.ADMOB, this.LoadBannerAds));
	}

	/** 애드 몹 배너 광고가 닫혔을 경우 */
	private void OnCloseAdmobBannerAds(object a_oSender, System.EventArgs a_oArgs) {
		CFunc.ShowLog("CAdsManager.OnCloseAdmobBannerAds", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_ADMOB_BANNER_ADS_CLOSE_CALLBACK, () => {
			// Do Something
		});
	}

	/** 애드 몹 보상 광고가 로드 되었을 경우 */
	private void OnLoadAdmobRewardAds(object a_oSender, System.EventArgs a_oArgs) {
		CFunc.ShowLog("CAdsManager.OnLoadAdmobRewardAds", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_ADMOB_REWARD_ADS_LOAD_CALLBACK, () => {
			// Do Something
		});
	}

	/** 애드 몹 보상 광고 로드에 실패했을 경우 */
	private void OnLoadFailAdmobRewardAds(object a_oSender, AdFailedToLoadEventArgs a_oArgs) {
		CFunc.ShowLog($"CAdsManager.OnLoadFailAdmobRewardAds: {a_oArgs.LoadAdError.GetMessage()}", KCDefine.B_LOG_COLOR_PLUGIN);
		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_ADMOB_REWARD_ADS_LOAD_FAIL_CALLBACK, () => { m_oAdmobRewardAds = null; this.AddLoadFailRewardAdsInfo(EAdsPlatform.ADMOB, this.LoadRewardAds); });
	}

	/** 애드 몹 보상 광고가 닫혔을 경우 */
	private void OnCloseAdmobRewardAds(object a_oSender, System.EventArgs a_oArgs) {
		CFunc.ShowLog("CAdsManager.OnCloseAdmobRewardAds", KCDefine.B_LOG_COLOR_PLUGIN);
		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_ADMOB_REWARD_ADS_CLOSE_CALLBACK, () => { m_oAdmobRewardAds = null; this.HandleCloseRewardAdsResult(EAdsPlatform.ADMOB); this.LoadRewardAds(EAdsPlatform.ADMOB); });
	}

	/** 애드 몹 광고 보상을 수신했을 경우 */
	private void OnReceiveAdmobAdsReward(object a_oSender, Reward a_oReward) {
		CFunc.ShowLog($"CAdsManager.OnReceiveAdmobAdsReward: {a_oReward}", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_ADMOB_REWARD_ADS_RECEIVE_ADS_REWARD_CALLBACK, () => {
			this.HandleRewardAdsResult(EAdsPlatform.ADMOB, new STAdsRewardInfo() {
				m_oID = (a_oReward != null && a_oReward.Type.ExIsValid()) ? a_oReward.Type : string.Empty, m_oVal = (a_oReward != null) ? $"{a_oReward.Amount}" : KCDefine.B_STR_0_INT
			}, true);
		});
	}

	/** 애드 몹 전면 광고가 로드 되었을 경우 */
	private void OnLoadAdmobFullscreenAds(object a_oSender, System.EventArgs a_oArgs) {
		CFunc.ShowLog("CAdsManager.OnLoadAdmobFullscreenAds", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_ADMOB_FULLSCREEN_ADS_LOAD_CALLBACK, () => {
			// Do Something
		});
	}

	/** 애드 몹 전면 광고 로드에 실패했을 경우 */
	private void OnLoadFailAdmobFullscreenAds(object a_oSender, AdFailedToLoadEventArgs a_oArgs) {
		CFunc.ShowLog($"CAdsManager.OnLoadFailAdmobFullscreenAds: {a_oArgs.LoadAdError.GetMessage()}", KCDefine.B_LOG_COLOR_PLUGIN);
		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_ADMOB_FULLSCREEN_ADS_LOAD_FAIL_CALLBACK, () => this.AddLoadFailFullscreenAdsInfo(EAdsPlatform.ADMOB, this.LoadFullscreenAds));
	}

	/** 애드 몹 전면 광고가 닫혔을 경우 */
	private void OnCloseAdmobFullscreenAds(object a_oSender, System.EventArgs a_oArgs) {
		CFunc.ShowLog("CAdsManager.OnCloseAdmobFullscreenAds", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_ADMOB_FULLSCREEN_ADS_CLOSE_CALLBACK, () => {
			m_oAdmobFullscreenAds?.Destroy();
			m_oAdmobFullscreenAds = null;

			this.HandleCloseFullscreenAdsResult(EAdsPlatform.ADMOB);
			this.LoadFullscreenAds(EAdsPlatform.ADMOB);
		});
	}

	/** 애드 몹 배너 광고를 반환한다 */
	private BannerView GetAdmobBannerAds() {
#if UNITY_IOS || UNITY_ANDROID
		// 배너 공고 생성이 가능 할 경우
		if(m_oAdmobBannerAds == null && m_oBoolDict[EKey.IS_ENABLE_BANNER_ADS] && this.IsInitAdmob() && this.IsValidAdmobBannerAdsID()) {
			m_oAdmobBannerAds = new BannerView(this.Params.m_oAdmobAdsIDDict.GetValueOrDefault(KCDefine.U_KEY_ADS_M_BANNER_ADS_ID, string.Empty), KCDefine.U_SIZE_ADMOB_BANNER_ADS, (this.Params.m_eBannerAdsPos == EBannerAdsPos.UP) ? AdPosition.Top : AdPosition.Bottom);
			m_oAdmobBannerAds.OnAdLoaded += this.OnLoadAdmobBannerAds;
			m_oAdmobBannerAds.OnAdFailedToLoad += this.OnLoadFailAdmobBannerAds;
			m_oAdmobBannerAds.OnAdClosed += this.OnCloseAdmobBannerAds;
		}

		CAccess.Assert(m_oAdmobBannerAds != null);
		return m_oAdmobBannerAds;
#else
		return null;
#endif // #if UNITY_IOS || UNITY_ANDROID
	}

	/** 애드 몹 보상 광고를 반환한다 */
	private RewardedAd GetAdmobRewardAds() {
#if UNITY_IOS || UNITY_ANDROID
		// 보상 광고 생성이 가능 할 경우
		if(m_oAdmobRewardAds == null && m_oBoolDict[EKey.IS_ENABLE_REWARD_ADS] && this.IsInitAdmob() && this.IsValidAdmobRewardAdsID()) {
			m_oAdmobRewardAds = new RewardedAd(this.Params.m_oAdmobAdsIDDict.GetValueOrDefault(KCDefine.U_KEY_ADS_M_REWARD_ADS_ID, string.Empty));
			m_oAdmobRewardAds.OnAdLoaded += this.OnLoadAdmobRewardAds;
			m_oAdmobRewardAds.OnAdFailedToLoad += this.OnLoadFailAdmobRewardAds;
			m_oAdmobRewardAds.OnAdClosed += this.OnCloseAdmobRewardAds;
			m_oAdmobRewardAds.OnUserEarnedReward += this.OnReceiveAdmobAdsReward;
		}

		CAccess.Assert(m_oAdmobRewardAds != null);
		return m_oAdmobRewardAds;
#else
		return null;
#endif // #if UNITY_IOS || UNITY_ANDROID
	}

	/** 애드 몹 전면 광고를 반환한다 */
	private InterstitialAd GetAdmobFullscreenAds() {
#if UNITY_IOS || UNITY_ANDROID
		// 전면 광고 생성이 가능 할 경우
		if(m_oAdmobFullscreenAds == null && m_oBoolDict[EKey.IS_ENABLE_FULLSCREEN_ADS] && this.IsInitAdmob() && this.IsValidAdmobFullscreenAdsID()) {
			m_oAdmobFullscreenAds = new InterstitialAd(this.Params.m_oAdmobAdsIDDict.GetValueOrDefault(KCDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID, string.Empty));
			m_oAdmobFullscreenAds.OnAdLoaded += this.OnLoadAdmobFullscreenAds;
			m_oAdmobFullscreenAds.OnAdFailedToLoad += this.OnLoadFailAdmobFullscreenAds;
			m_oAdmobFullscreenAds.OnAdClosed += this.OnCloseAdmobFullscreenAds;
		}

		CAccess.Assert(m_oAdmobFullscreenAds != null);
		return m_oAdmobFullscreenAds;
#else
		return null;
#endif // #if UNITY_IOS || UNITY_ANDROID
	}

	/** 애드 몹 광고 요청 빌더를 반환한다 */
	private AdRequest.Builder GetAdmobRequestBuilder() {
#if UNITY_IOS || UNITY_ANDROID
		// 요청 빌더 생성이 가능 할 경우
		if(this.IsInitAdmob() && m_oAdmobRequestBuilder == null) {
			m_oAdmobRequestBuilder = new AdRequest.Builder();
		}

		CAccess.Assert(m_oAdmobRequestBuilder != null);
		return m_oAdmobRequestBuilder;
#else
		return null;
#endif // #if UNITY_IOS || UNITY_ANDROID
	}

	/** 애드 몹 배너 광고 로드 결과를 처리한다 */
	private void HandleLoadAdmobBannerAdsResult() {
		this.ExLateCallFunc((a_oSender) => this.ShowBannerAds(EAdsPlatform.ADMOB, null));
	}
#endif // #if (UNITY_IOS || UNITY_ANDROID) && ADMOB_ADS_ENABLE
#endregion // 조건부 함수
}
#endif // #if ADS_MODULE_ENABLE

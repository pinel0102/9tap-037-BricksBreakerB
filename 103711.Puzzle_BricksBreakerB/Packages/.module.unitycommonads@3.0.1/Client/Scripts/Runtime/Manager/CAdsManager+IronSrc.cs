using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if ADS_MODULE_ENABLE
/** 광고 관리자 - 아이언 소스 */
public partial class CAdsManager : CSingleton<CAdsManager> {
#region 함수
	/** 아이언 소스 배너 광고를 로드한다 */
	private void LoadIronSrcBannerAds() {
		CAccess.Assert(this.IsValidIronSrcBannerAdsID());

#if(UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
		IronSource.Agent.loadBanner(KCDefine.U_SIZE_IRON_SRC_BANNER_ADS, (this.Params.m_eBannerAdsPos == EBannerAdsPos.UP) ? IronSourceBannerPosition.TOP : IronSourceBannerPosition.BOTTOM);
#endif // #if (UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
	}

	/** 아이언 소스 보상 광고를 로드한다 */
	private void LoadIronSrcRewardAds() {
		CAccess.Assert(this.IsValidIronSrcRewardAdsID());

#if(UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
		IronSource.Agent.loadRewardedVideo();
#endif // #if (UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
	}

	/** 아이언 소스 전면 광고를 로드한다 */
	private void LoadIronSrcFullscreenAds() {
		CAccess.Assert(this.IsValidIronSrcFullscreenAdsID());

#if(UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
		IronSource.Agent.loadInterstitial();
#endif // #if (UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
	}

	/** 아이언 소스 배너 광고를 출력한다 */
	private void ShowIronSrcBannerAds() {
		CAccess.Assert(this.IsValidIronSrcBannerAdsID() && this.Params.m_eAdsPlatform == EAdsPlatform.IRON_SRC);

#if(UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
		IronSource.Agent.displayBanner();
		this.SetupBannerAdsHeight((float)KCDefine.U_SIZE_IRON_SRC_BANNER_ADS.Height);
#endif // #if (UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
	}

	/** 아이언 소스 보상 광고를 출력한다 */
	private void ShowIronSrcRewardAds() {
		CAccess.Assert(this.IsValidIronSrcRewardAdsID());

#if(UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
		IronSource.Agent.showRewardedVideo();
#endif // #if (UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
	}

	/** 아이언 소스 전면 광고를 출력한다 */
	private void ShowIronSrcFullscreenAds() {
		CAccess.Assert(this.IsValidIronSrcFullscreenAdsID());

#if(UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
		IronSource.Agent.showInterstitial();
#endif // #if (UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
	}

	/** 아이언 소스 배너 광고를 닫는다 */
	private void CloseIronSrcBannerAds() {
		CAccess.Assert(this.IsValidIronSrcBannerAdsID());

#if(UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
		IronSource.Agent.hideBanner();
		IronSource.Agent.destroyBanner();

		m_oBoolDict[EKey.IS_LOAD_IRON_SRC_BANNER_ADS] = false;
		m_oRealDict[EKey.BANNER_ADS_HEIGHT] = KCDefine.B_VAL_0_REAL;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
	}
#endregion // 함수

#region 접근자 함수
	/** 아이언 소스 초기화 여부를 검사한다 */
	private bool IsInitIronSrc() {
#if(UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
		return m_oBoolDict[EKey.IS_INIT_IRON_SRC];
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
	}

	/** 아이언 소스 광고 식별자 유효 여부를 검사한다 */
	private bool IsValidIronSrcAdsID(string a_oID) {
		CAccess.Assert(a_oID.ExIsValid());

#if(UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
		return this.Params.m_oIronSrcAdsIDDict.TryGetValue(a_oID, out string oAdsID) && oAdsID.ExIsValid();
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
	}

	/** 아이언 소스 배너 광고 식별자 유효 여부를 검사한다 */
	private bool IsValidIronSrcBannerAdsID() {
#if(UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
		return this.IsValidIronSrcAdsID(KCDefine.U_KEY_ADS_M_BANNER_ADS_ID);
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
	}

	/** 아이언 소스 보상 광고 식별자 유효 여부를 검사한다 */
	private bool IsValidIronSrcRewardAdsID() {
#if(UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
		return this.IsValidIronSrcAdsID(KCDefine.U_KEY_ADS_M_REWARD_ADS_ID);
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
	}

	/** 아이언 소스 전면 광고 식별자 유효 여부를 검사한다 */
	private bool IsValidIronSrcFullscreenAdsID() {
#if(UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
		return this.IsValidIronSrcAdsID(KCDefine.U_KEY_ADS_M_FULLSCREEN_ADS_ID);
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
	}

	/** 아이언 소스 배너 광고 로드 여부를 검사한다 */
	private bool IsLoadIronSrcBannerAds() {
#if(UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
		return this.IsValidIronSrcBannerAdsID() && m_oBoolDict[EKey.IS_LOAD_IRON_SRC_BANNER_ADS];
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
	}

	/** 아이언 소스 보상 광고 로드 여부를 검사한다 */
	private bool IsLoadIronSrcRewardAds() {
#if(UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
		return this.IsValidIronSrcRewardAdsID() && IronSource.Agent.isRewardedVideoAvailable();
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
	}

	/** 아이언 소스 전면 광고 로드 여부를 검사한다 */
	private bool IsLoadIronSrcFullscreenAds() {
#if(UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
		return this.IsValidIronSrcFullscreenAdsID() && IronSource.Agent.isInterstitialReady();
#else
		return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
	}
#endregion // 접근자 함수

#region 조건부 함수
#if(UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
	/** 아이언 소스이 초기화 되었을 경우 */
	private void OnInitIronSrc() {
		CFunc.ShowLog("CAdsManager.OnInitIronSrc", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_IRON_SRC_INIT_CALLBACK, () => {
#if DEBUG || DEVELOPMENT_BUILD
			IronSource.Agent.setAdaptersDebug(true);
			IronSource.Agent.validateIntegration();
#endif // #if DEBUG || DEVELOPMENT_BUILD
			
			m_oBoolDict[EKey.IS_INIT] = true;
			m_oBoolDict[EKey.IS_INIT_IRON_SRC] = true;
			
			this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.INIT)?.Invoke(this, EAdsPlatform.IRON_SRC, m_oBoolDict[EKey.IS_INIT_IRON_SRC]);
		});
	}

	/** 아이언 소스 배너 광고가 로드 되었을 경우 */
	private void OnLoadIronSrcBannerAds() {
		CFunc.ShowLog("CAdsManager.OnLoadIronSrcBannerAds", KCDefine.B_LOG_COLOR_PLUGIN);
		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_IRON_SRC_BANNER_ADS_LOAD_CALLBACK, () => { m_oBoolDict[EKey.IS_LOAD_IRON_SRC_BANNER_ADS] = true; this.HandleLoadIronSrcBannerAdsResult(); });
	}

	/** 아이언 소스 배너 광고 로드에 실패했을 경우 */
	private void OnLoadFailIronSrcBannerAds(IronSourceError a_oError) {
		CFunc.ShowLog($"CAdsManager.OnLoadFailIronSrcBannerAds: {a_oError}", KCDefine.B_LOG_COLOR_PLUGIN);
		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_IRON_SRC_BANNER_ADS_LOAD_FAIL_CALLBACK, () => this.AddLoadFailBannerAdsInfo(EAdsPlatform.IRON_SRC, this.LoadBannerAds));
	}

	/** 아이언 소스 보상 광고가 닫혔을 경우 */
	private void OnCloseIronSrcRewardAds() {
		CFunc.ShowLog("CAdsManager.OnCloseIronSrcRewardAds", KCDefine.B_LOG_COLOR_PLUGIN);
		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_IRON_SRC_REWARD_ADS_CLOSE_CALLBACK, () => { this.HandleCloseRewardAdsResult(EAdsPlatform.IRON_SRC); this.LoadRewardAds(EAdsPlatform.IRON_SRC); });
	}

	/** 아이언 소스 광고 보상을 수신했을 경우 */
	private void OnReceiveIronSrcAdsReward(IronSourcePlacement a_oPlacement) {
		CFunc.ShowLog($"CAdsManager.OnReceiveIronSrcAdsReward: {a_oPlacement}", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_IRON_SRC_REWARD_ADS_RECEIVE_ADS_REWARD_CALLBACK, () => {
			this.HandleRewardAdsResult(EAdsPlatform.IRON_SRC, new STAdsRewardInfo() {
				m_oID = (a_oPlacement != null && a_oPlacement.getRewardName().ExIsValid()) ? a_oPlacement.getRewardName() : string.Empty, m_oVal = (a_oPlacement != null) ? $"{a_oPlacement.getRewardAmount()}" : KCDefine.B_STR_0_INT
			}, true);
		});
	}

	/** 아이언 소스 보상 광고 상태가 변경 되었을 경우 */
	private void OnChangeIronSrcRewardAdsState(bool a_bIsActive) {
		CFunc.ShowLog($"CAdsManager.OnChangeIronSrcRewardAdsState: {a_bIsActive}", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_IRON_SRC_REWARD_ADS_CHANGE_STATE_CALLBACK, () => {
			// 비활성 상태 일 경우
			if(!a_bIsActive) {
				this.AddLoadFailRewardAdsInfo(EAdsPlatform.IRON_SRC, this.LoadRewardAds);
			}
		});
	}

	/** 아이언 소스 전면 광고가 로드 되었을 경우 */
	private void OnLoadIronSrcFullscreenAds() {
		CFunc.ShowLog("CAdsManager.OnLoadIronSrcFullscreenAds", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_IRON_SRC_FULLSCREEN_ADS_LOAD_CALLBACK, () => {
			// Do Something
		});
	}

	/** 아이언 소스 전면 광고 로드에 실패했을 경우 */
	private void OnLoadFailIronSrcFullscreenAds(IronSourceError a_oError) {
		CFunc.ShowLog($"CAdsManager.OnLoadFailIronSrcFullscreenAds: {a_oError}", KCDefine.B_LOG_COLOR_PLUGIN);
		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_IRON_SRC_FULLSCREEN_ADS_LOAD_FAIL_CALLBACK, () => this.AddLoadFailFullscreenAdsInfo(EAdsPlatform.IRON_SRC, this.LoadFullscreenAds));
	}

	/** 아이언 소스 전면 광고가 닫혔을 경우 */
	private void OnCloseIronSrcFullscreenAds() {
		CFunc.ShowLog("CAdsManager.OnCloseIronSrcFullscreenAds", KCDefine.B_LOG_COLOR_PLUGIN);
		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_ADS_M_IRON_SRC_FULLSCREEN_ADS_CLOSE_CALLBACK, () => { this.HandleCloseFullscreenAdsResult(EAdsPlatform.IRON_SRC); this.LoadFullscreenAds(EAdsPlatform.IRON_SRC); });
	}
	
	/** 아이언 소스 배너 광고 로드 결과를 처리한다 */
	private void HandleLoadIronSrcBannerAdsResult() {
		this.ExLateCallFunc((a_oSender) => this.ShowBannerAds(EAdsPlatform.IRON_SRC, null));
	}

	/** 광고 단위를 생성한다 */
	private List<string> MakeIronSrcAdsIDs(Dictionary<string, string> a_oIronSrcAdsIDDict) {
		var oIronSrcAdsIDList = new List<string>();

		foreach(var stKeyVal in a_oIronSrcAdsIDDict) {
			// 광고 식별자가 유효 할 경우
			if(stKeyVal.Value.ExIsValid()) {
				oIronSrcAdsIDList.ExAddVal(stKeyVal.Value);
			}
		}
		
		return oIronSrcAdsIDList;
	}
#endif // #if (UNITY_IOS || UNITY_ANDROID) && IRON_SRC_ADS_ENABLE
#endregion // 조건부 함수
}
#endif // #if ADS_MODULE_ENABLE

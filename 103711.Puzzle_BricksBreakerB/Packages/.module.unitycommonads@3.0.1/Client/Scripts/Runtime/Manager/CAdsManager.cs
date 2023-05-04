using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if ADS_MODULE_ENABLE
#if ADMOB_ADS_ENABLE
using GoogleMobileAds.Api;
#endif // #if ADMOB_ADS_ENABLE

/** 광고 관리자 */
public partial class CAdsManager : CSingleton<CAdsManager> {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		IS_INIT,
		IS_ENABLE_BANNER_ADS,
		IS_ENABLE_REWARD_ADS,
		IS_ENABLE_FULLSCREEN_ADS,

		UPDATE_SKIP_TIME,
		BANNER_ADS_HEIGHT,
		ORIGIN_BANNER_ADS_HEIGHT,

#if ADMOB_ADS_ENABLE
		IS_INIT_ADMOB,
		IS_LOAD_ADMOB_BANNER_ADS,
#endif // #if ADMOB_ADS_ENABLE

#if IRON_SRC_ADS_ENABLE
		IS_INIT_IRON_SRC,
		IS_LOAD_IRON_SRC_BANNER_ADS,
#endif // #if IRON_SRC_ADS_ENABLE

		[HideInInspector] MAX_VAL
	}

	/** 콜백 */
	public enum ECallback {
		NONE = -1,
		INIT,
		[HideInInspector] MAX_VAL
	}

	/** 광고 함수 */
	private enum EAdsFunc {
		NONE = -1,
		IS_INIT,

		IS_LOAD_BANNER_ADS,
		IS_LOAD_REWARD_ADS,
		IS_LOAD_FULLSCREEN_ADS,

		LOAD_BANNER_ADS,
		LOAD_REWARD_ADS,
		LOAD_FULLSCREEN_ADS,

		SHOW_BANNER_ADS,
		SHOW_REWARD_ADS,
		SHOW_FULLSCREEN_ADS,

		CLOSE_BANNER_ADS,
        HIDE_BANNER_ADS,
		[HideInInspector] MAX_VAL
	}

	/** 광고 콜백 */
	private enum EAdsCallback {
		NONE = -1,
		CLOSE_REWARD_ADS,
		CLOSE_FULLSCREEN_ADS,

		REWARD_ADS_REWARD,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public EAdsPlatform m_eAdsPlatform;
		public EBannerAdsPos m_eBannerAdsPos;

#if ADMOB_ADS_ENABLE
		public List<string> m_oAdmobTestDeviceIDList;
		public Dictionary<string, string> m_oAdmobAdsIDDict;
#endif // #if ADMOB_ADS_ENABLE

#if IRON_SRC_ADS_ENABLE
		public string m_oIronSrcAppKey;
		public Dictionary<string, string> m_oIronSrcAdsIDDict;
#endif // #if IRON_SRC_ADS_ENABLE

		public Dictionary<ECallback, System.Action<CAdsManager, EAdsPlatform, bool>> m_oCallbackDict;
	}

	/** 로드 실패 광고 정보 */
	private struct STLoadFailAdsInfo {
		public EAdsPlatform m_eAdsPlatform;
		public System.Action<EAdsPlatform> m_oCallback;
	}

#region 변수
	private Dictionary<EKey, bool> m_oBoolDict = new Dictionary<EKey, bool>() {
		[EKey.IS_INIT] = false,
		[EKey.IS_ENABLE_BANNER_ADS] = true,
		[EKey.IS_ENABLE_REWARD_ADS] = true,
		[EKey.IS_ENABLE_FULLSCREEN_ADS] = true,

#if ADMOB_ADS_ENABLE
		[EKey.IS_INIT_ADMOB] = false,
		[EKey.IS_LOAD_ADMOB_BANNER_ADS] = false,
#endif // #if ADMOB_ADS_ENABLE

#if IRON_SRC_ADS_ENABLE
		[EKey.IS_INIT_IRON_SRC] = false,
		[EKey.IS_LOAD_IRON_SRC_BANNER_ADS] = false
#endif // #if IRON_SRC_ADS_ENABLE
	};

	private Dictionary<EKey, float> m_oRealDict = new Dictionary<EKey, float>() {
		[EKey.UPDATE_SKIP_TIME] = KCDefine.B_VAL_0_REAL,
		[EKey.BANNER_ADS_HEIGHT] = KCDefine.B_VAL_0_REAL,
		[EKey.ORIGIN_BANNER_ADS_HEIGHT] = KCDefine.B_VAL_0_REAL
	};

	private Dictionary<EAdsFunc, Dictionary<EAdsPlatform, System.Action>> m_oFuncDictContainer01 = new Dictionary<EAdsFunc, Dictionary<EAdsPlatform, System.Action>>() {
		[EAdsFunc.LOAD_BANNER_ADS] = new Dictionary<EAdsPlatform, System.Action>(),
		[EAdsFunc.LOAD_REWARD_ADS] = new Dictionary<EAdsPlatform, System.Action>(),
		[EAdsFunc.LOAD_FULLSCREEN_ADS] = new Dictionary<EAdsPlatform, System.Action>(),

		[EAdsFunc.SHOW_BANNER_ADS] = new Dictionary<EAdsPlatform, System.Action>(),
		[EAdsFunc.SHOW_REWARD_ADS] = new Dictionary<EAdsPlatform, System.Action>(),
		[EAdsFunc.SHOW_FULLSCREEN_ADS] = new Dictionary<EAdsPlatform, System.Action>(),

		[EAdsFunc.CLOSE_BANNER_ADS] = new Dictionary<EAdsPlatform, System.Action>(),
        [EAdsFunc.HIDE_BANNER_ADS] = new Dictionary<EAdsPlatform, System.Action>()
	};

	private Dictionary<EAdsFunc, Dictionary<EAdsPlatform, System.Func<bool>>> m_oFuncDictContainer02 = new Dictionary<EAdsFunc, Dictionary<EAdsPlatform, System.Func<bool>>>() {
		[EAdsFunc.IS_INIT] = new Dictionary<EAdsPlatform, System.Func<bool>>(),
		[EAdsFunc.IS_LOAD_BANNER_ADS] = new Dictionary<EAdsPlatform, System.Func<bool>>(),
		[EAdsFunc.IS_LOAD_REWARD_ADS] = new Dictionary<EAdsPlatform, System.Func<bool>>(),
		[EAdsFunc.IS_LOAD_FULLSCREEN_ADS] = new Dictionary<EAdsPlatform, System.Func<bool>>()
	};

	private Dictionary<EAdsCallback, Dictionary<EAdsPlatform, System.Action<CAdsManager>>> m_oCallbackDictContainer01 = new Dictionary<EAdsCallback, Dictionary<EAdsPlatform, System.Action<CAdsManager>>>() {
		[EAdsCallback.CLOSE_REWARD_ADS] = new Dictionary<EAdsPlatform, System.Action<CAdsManager>>(),
		[EAdsCallback.CLOSE_FULLSCREEN_ADS] = new Dictionary<EAdsPlatform, System.Action<CAdsManager>>()
	};

	private Dictionary<EAdsCallback, Dictionary<EAdsPlatform, System.Action<CAdsManager, STAdsRewardInfo, bool>>> m_oCallbackDictContainer02 = new Dictionary<EAdsCallback, Dictionary<EAdsPlatform, System.Action<CAdsManager, STAdsRewardInfo, bool>>>() {
		[EAdsCallback.REWARD_ADS_REWARD] = new Dictionary<EAdsPlatform, System.Action<CAdsManager, STAdsRewardInfo, bool>>()
	};

	private Dictionary<string, STLoadFailAdsInfo> m_oLoadFailAdsInfoDict = new Dictionary<string, STLoadFailAdsInfo>();

#if ADMOB_ADS_ENABLE
	private BannerView m_oAdmobBannerAds = null;
	private RewardedAd m_oAdmobRewardAds = null;
	private InterstitialAd m_oAdmobFullscreenAds = null;
	private AdRequest.Builder m_oAdmobRequestBuilder = null;
#endif // #if ADMOB_ADS_ENABLE
#endregion // 변수

#region 프로퍼티
	public STParams Params { get; private set; }

	public bool IsEnableBannerAds {
		get { return m_oBoolDict[EKey.IS_ENABLE_BANNER_ADS]; }
		set { m_oBoolDict[EKey.IS_ENABLE_BANNER_ADS] = value; }
	}

	public bool IsEnableRewardAds {
		get { return m_oBoolDict[EKey.IS_ENABLE_REWARD_ADS]; }
		set { m_oBoolDict[EKey.IS_ENABLE_REWARD_ADS] = value; }
	}

	public bool IsEnableFullscreenAds {
		get { return m_oBoolDict[EKey.IS_ENABLE_FULLSCREEN_ADS]; }
		set { m_oBoolDict[EKey.IS_ENABLE_FULLSCREEN_ADS] = value; }
	}

	public bool IsInit => m_oBoolDict[EKey.IS_INIT];

	public float BannerAdsHeight => m_oRealDict[EKey.BANNER_ADS_HEIGHT];
	public float OriginBannerAdsHeight => m_oRealDict[EKey.ORIGIN_BANNER_ADS_HEIGHT];
#endregion // 프로퍼티

#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		CScheduleManager.Inst.AddComponent(this);

#if ADMOB_ADS_ENABLE
		m_oFuncDictContainer01[EAdsFunc.LOAD_BANNER_ADS].TryAdd(EAdsPlatform.ADMOB, this.LoadAdmobBannerAds);
		m_oFuncDictContainer01[EAdsFunc.LOAD_REWARD_ADS].TryAdd(EAdsPlatform.ADMOB, this.LoadAdmobRewardAds);
		m_oFuncDictContainer01[EAdsFunc.LOAD_FULLSCREEN_ADS].TryAdd(EAdsPlatform.ADMOB, this.LoadAdmobFullscreenAds);

		m_oFuncDictContainer01[EAdsFunc.SHOW_BANNER_ADS].TryAdd(EAdsPlatform.ADMOB, this.ShowAdmobBannerAds);
		m_oFuncDictContainer01[EAdsFunc.SHOW_REWARD_ADS].TryAdd(EAdsPlatform.ADMOB, this.ShowAdmobRewardAds);
		m_oFuncDictContainer01[EAdsFunc.SHOW_FULLSCREEN_ADS].TryAdd(EAdsPlatform.ADMOB, this.ShowAdmobFullscreenAds);

		m_oFuncDictContainer01[EAdsFunc.CLOSE_BANNER_ADS].TryAdd(EAdsPlatform.ADMOB, this.CloseAdmobBannerAds);
		m_oFuncDictContainer02[EAdsFunc.IS_INIT].TryAdd(EAdsPlatform.ADMOB, this.IsInitAdmob);

		m_oFuncDictContainer02[EAdsFunc.IS_LOAD_BANNER_ADS].TryAdd(EAdsPlatform.ADMOB, this.IsLoadAdmobBannerAds);
		m_oFuncDictContainer02[EAdsFunc.IS_LOAD_REWARD_ADS].TryAdd(EAdsPlatform.ADMOB, this.IsLoadAdmobRewardAds);
		m_oFuncDictContainer02[EAdsFunc.IS_LOAD_FULLSCREEN_ADS].TryAdd(EAdsPlatform.ADMOB, this.IsLoadAdmobFullscreenAds);
#endif // #if ADMOB_ADS_ENABLE

#if IRON_SRC_ADS_ENABLE
		m_oFuncDictContainer01[EAdsFunc.LOAD_BANNER_ADS].TryAdd(EAdsPlatform.IRON_SRC, this.LoadIronSrcBannerAds);
		m_oFuncDictContainer01[EAdsFunc.LOAD_REWARD_ADS].TryAdd(EAdsPlatform.IRON_SRC, this.LoadIronSrcRewardAds);
		m_oFuncDictContainer01[EAdsFunc.LOAD_FULLSCREEN_ADS].TryAdd(EAdsPlatform.IRON_SRC, this.LoadIronSrcFullscreenAds);

		m_oFuncDictContainer01[EAdsFunc.SHOW_BANNER_ADS].TryAdd(EAdsPlatform.IRON_SRC, this.ShowIronSrcBannerAds);
		m_oFuncDictContainer01[EAdsFunc.SHOW_REWARD_ADS].TryAdd(EAdsPlatform.IRON_SRC, this.ShowIronSrcRewardAds);
		m_oFuncDictContainer01[EAdsFunc.SHOW_FULLSCREEN_ADS].TryAdd(EAdsPlatform.IRON_SRC, this.ShowIronSrcFullscreenAds);

		m_oFuncDictContainer01[EAdsFunc.CLOSE_BANNER_ADS].TryAdd(EAdsPlatform.IRON_SRC, this.CloseIronSrcBannerAds);
        m_oFuncDictContainer01[EAdsFunc.HIDE_BANNER_ADS].TryAdd(EAdsPlatform.IRON_SRC, this.HideIronSrcBannerAds);
		m_oFuncDictContainer02[EAdsFunc.IS_INIT].TryAdd(EAdsPlatform.IRON_SRC, this.IsInitIronSrc);

		m_oFuncDictContainer02[EAdsFunc.IS_LOAD_BANNER_ADS].TryAdd(EAdsPlatform.IRON_SRC, this.IsLoadIronSrcBannerAds);
		m_oFuncDictContainer02[EAdsFunc.IS_LOAD_REWARD_ADS].TryAdd(EAdsPlatform.IRON_SRC, this.IsLoadIronSrcRewardAds);
		m_oFuncDictContainer02[EAdsFunc.IS_LOAD_FULLSCREEN_ADS].TryAdd(EAdsPlatform.IRON_SRC, this.IsLoadIronSrcFullscreenAds);
#endif // #if IRON_SRC_ADS_ENABLE
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		CFunc.ShowLog("CAdsManager.Init", KCDefine.B_LOG_COLOR_PLUGIN);

#if ADMOB_ADS_ENABLE
		CAccess.Assert(a_stParams.m_oAdmobAdsIDDict.ExIsValid());
#endif // #if ADMOB_ADS_ENABLE

#if IRON_SRC_ADS_ENABLE
		CAccess.Assert(a_stParams.m_oIronSrcAppKey.ExIsValid() && a_stParams.m_oIronSrcAdsIDDict.ExIsValid());
#endif // #if IRON_SRC_ADS_ENABLE

#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
		// 초기화 되었을 경우
		if(m_oBoolDict[EKey.IS_INIT]) {
			a_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.INIT)?.Invoke(this, EAdsPlatform.NONE, m_oBoolDict[EKey.IS_INIT]);
		} else {
			this.Params = a_stParams;

#if ADMOB_ADS_ENABLE
			MobileAds.Initialize(this.OnInitAdmob);
#endif // #if ADMOB_ADS_ENABLE

#if IRON_SRC_ADS_ENABLE
			IronSourceEvents.onBannerAdLoadedEvent += this.OnLoadIronSrcBannerAds;
			IronSourceEvents.onBannerAdLoadFailedEvent += this.OnLoadFailIronSrcBannerAds;

			IronSourceEvents.onRewardedVideoAdClosedEvent += this.OnCloseIronSrcRewardAds;
			IronSourceEvents.onRewardedVideoAdRewardedEvent += this.OnReceiveIronSrcAdsReward;
			IronSourceEvents.onRewardedVideoAvailabilityChangedEvent += this.OnChangeIronSrcRewardAdsState;

			IronSourceEvents.onInterstitialAdReadyEvent += this.OnLoadIronSrcFullscreenAds;
			IronSourceEvents.onInterstitialAdLoadFailedEvent += this.OnLoadFailIronSrcFullscreenAds;
			IronSourceEvents.onInterstitialAdClosedEvent += this.OnCloseIronSrcFullscreenAds;

			IronSourceEvents.onSdkInitializationCompletedEvent += this.OnInitIronSrc;
			IronSource.Agent.init(a_stParams.m_oIronSrcAppKey, this.MakeIronSrcAdsIDs(a_stParams.m_oIronSrcAdsIDDict).ToArray());
#endif // #if IRON_SRC_ADS_ENABLE
		}
#else
		a_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.INIT)?.Invoke(this, EAdsPlatform.NONE, false);
#endif // #if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
	}

	/** 제거 되었을 경우 */
	public override void OnDestroy() {
		base.OnDestroy();

#if ADMOB_ADS_ENABLE
		try {
			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				m_oAdmobBannerAds?.Destroy();
				m_oAdmobFullscreenAds?.Destroy();
			}
		} catch(System.Exception oException) {
			CFunc.ShowLogWarning($"CAdsManager.OnDestroy Exception: {oException.Message}");
		}
#endif // #if ADMOB_ADS_ENABLE
	}

	/** 앱이 정지 되었을 경우 */
	public virtual void OnApplicationPause(bool a_bIsPause) {
#if IRON_SRC_ADS_ENABLE
		// 초기화 되었을 경우
		if(m_oBoolDict[EKey.IS_INIT]) {
			IronSource.Agent.onApplicationPause(a_bIsPause);
		}
#endif // #if IRON_SRC_ADS_ENABLE
	}

	/** 상태를 갱신한다 */
	public override void OnLateUpdate(float a_fDeltaTime) {
		base.OnLateUpdate(a_fDeltaTime);

		// 앱이 실행 중 일 경우
		if(CSceneManager.IsAppRunning) {
			m_oRealDict[EKey.UPDATE_SKIP_TIME] += CScheduleManager.Inst.UnscaleDeltaTime;

			// 광고 로드 주기가 지났을 경우
			if(m_oRealDict[EKey.UPDATE_SKIP_TIME].ExIsGreateEquals(KCDefine.U_DELTA_T_ADS_M_ADS_LOAD)) {
				m_oRealDict[EKey.UPDATE_SKIP_TIME] = KCDefine.B_VAL_0_REAL;

				try {
					foreach(var stAdsInfo in m_oLoadFailAdsInfoDict) {
						stAdsInfo.Value.m_oCallback?.Invoke(stAdsInfo.Value.m_eAdsPlatform);
					}
				} finally {
					m_oLoadFailAdsInfoDict.Clear();
				}
			}
		}
	}

	/** 배너 광고를 로드한다 */
	public void LoadBannerAds(EAdsPlatform a_eAdsPlatform) {
		CFunc.ShowLog($"CAdsManager.LoadBannerAds: {a_eAdsPlatform}", KCDefine.B_LOG_COLOR_PLUGIN);
		CAccess.Assert(a_eAdsPlatform.ExIsValid());

		// 배너 광고 로드가 가능 할 경우
		if(m_oBoolDict[EKey.IS_ENABLE_BANNER_ADS] && this.IsInitAds(a_eAdsPlatform) && !this.IsLoadBannerAds(a_eAdsPlatform)) {
			m_oFuncDictContainer01[EAdsFunc.LOAD_BANNER_ADS][a_eAdsPlatform]();
		}
	}

	/** 보상 광고를 로드한다 */
	public void LoadRewardAds(EAdsPlatform a_eAdsPlatform) {
		CFunc.ShowLog($"CAdsManager.LoadRewardAds: {a_eAdsPlatform}", KCDefine.B_LOG_COLOR_PLUGIN);
		CAccess.Assert(a_eAdsPlatform.ExIsValid());

		// 보상 광고 로드가 가능 할 경우
		if(m_oBoolDict[EKey.IS_ENABLE_REWARD_ADS] && this.IsInitAds(a_eAdsPlatform) && !this.IsLoadRewardAds(a_eAdsPlatform)) {
			m_oFuncDictContainer01[EAdsFunc.LOAD_REWARD_ADS][a_eAdsPlatform]();
		}
	}

	/** 전면 광고를 로드한다 */
	public void LoadFullscreenAds(EAdsPlatform a_eAdsPlatform) {
		CFunc.ShowLog($"CAdsManager.LoadFullscreenAds: {a_eAdsPlatform}", KCDefine.B_LOG_COLOR_PLUGIN);
		CAccess.Assert(a_eAdsPlatform.ExIsValid());

		// 전면 광고 로드가 가능 할 경우
		if(m_oBoolDict[EKey.IS_ENABLE_FULLSCREEN_ADS] && this.IsInitAds(a_eAdsPlatform) && !this.IsLoadFullscreenAds(a_eAdsPlatform)) {
			m_oFuncDictContainer01[EAdsFunc.LOAD_FULLSCREEN_ADS][a_eAdsPlatform]();
		}
	}

	/** 배너 광고를 출력한다 */
	public void ShowBannerAds(EAdsPlatform a_eAdsPlatform, System.Action<CAdsManager, bool> a_oCallback) {
		CFunc.ShowLog($"CAdsManager.ShowBannerAds: {a_eAdsPlatform}", KCDefine.B_LOG_COLOR_PLUGIN);
		CAccess.Assert(a_eAdsPlatform.ExIsValid());

		try {
			CFunc.Invoke(ref a_oCallback, this, this.IsLoadBannerAds(a_eAdsPlatform));
		} finally {
			// 배너 광고가 로드 되었을 경우
			if(this.IsLoadBannerAds(a_eAdsPlatform)) {
				m_oFuncDictContainer01[EAdsFunc.SHOW_BANNER_ADS][a_eAdsPlatform]();
			}
		}
	}

	/** 보상 광고를 출력한다 */
	public void ShowRewardAds(EAdsPlatform a_eAdsPlatform, System.Action<CAdsManager, STAdsRewardInfo, bool> a_oCallback, System.Action<CAdsManager, bool> a_oShowCallback = null, System.Action<CAdsManager> a_oCloseCallback = null) {
		CFunc.ShowLog($"CAdsManager.ShowRewardAds: {a_eAdsPlatform}", KCDefine.B_LOG_COLOR_PLUGIN);
		CAccess.Assert(a_eAdsPlatform.ExIsValid());

		try {
			CFunc.Invoke(ref a_oShowCallback, this, this.IsLoadRewardAds(a_eAdsPlatform));
		} finally {
			// 보상 광고가 로드 되었을 경우
			if(this.IsLoadRewardAds(a_eAdsPlatform)) {
				m_oCallbackDictContainer01[EAdsCallback.CLOSE_REWARD_ADS].ExReplaceVal(a_eAdsPlatform, a_oCloseCallback);
				m_oCallbackDictContainer02[EAdsCallback.REWARD_ADS_REWARD].ExReplaceVal(a_eAdsPlatform, a_oCallback);

				m_oFuncDictContainer01[EAdsFunc.SHOW_REWARD_ADS][a_eAdsPlatform]();
			} else {
				CFunc.Invoke(ref a_oCallback, this, STAdsRewardInfo.INVALID, false);
			}
		}
	}

	/** 전면 광고를 출력한다 */
	public void ShowFullscreenAds(EAdsPlatform a_eAdsPlatform, System.Action<CAdsManager, bool> a_oCallback, System.Action<CAdsManager> a_oCloseCallback = null) {
		CFunc.ShowLog($"CAdsManager.ShowFullscreenAds: {a_eAdsPlatform}", KCDefine.B_LOG_COLOR_PLUGIN);
		CAccess.Assert(a_eAdsPlatform.ExIsValid());

		try {
			CFunc.Invoke(ref a_oCallback, this, this.IsLoadFullscreenAds(a_eAdsPlatform));
		} finally {
			// 전면 광고가 로드 되었을 경우
			if(this.IsLoadFullscreenAds(a_eAdsPlatform)) {
				m_oCallbackDictContainer01[EAdsCallback.CLOSE_FULLSCREEN_ADS].ExReplaceVal(a_eAdsPlatform, a_oCloseCallback);
				m_oFuncDictContainer01[EAdsFunc.SHOW_FULLSCREEN_ADS][a_eAdsPlatform]();
			}
		}
	}

	/** 배너 광고를 닫는다 */
	public void CloseBannerAds(EAdsPlatform a_eAdsPlatform, System.Action<CAdsManager, bool> a_oCallback) {
		CFunc.ShowLog($"CAdsManager.CloseBannerAds: {a_eAdsPlatform}", KCDefine.B_LOG_COLOR_PLUGIN);
		CAccess.Assert(a_eAdsPlatform.ExIsValid());

		try {
			CFunc.Invoke(ref a_oCallback, this, this.IsLoadBannerAds(a_eAdsPlatform));
		} finally {
			// 배너 광고 닫기가 가능 할 경우
			if(this.IsLoadBannerAds(a_eAdsPlatform)) {
				m_oFuncDictContainer01[EAdsFunc.CLOSE_BANNER_ADS][a_eAdsPlatform]();
			}
		}
	}

    public void HideBannerAds(EAdsPlatform a_eAdsPlatform, System.Action<CAdsManager, bool> a_oCallback) {
		CFunc.ShowLog($"CAdsManager.HideBannerAds: {a_eAdsPlatform}", KCDefine.B_LOG_COLOR_PLUGIN);
		CAccess.Assert(a_eAdsPlatform.ExIsValid());

		try {
			CFunc.Invoke(ref a_oCallback, this, this.IsLoadBannerAds(a_eAdsPlatform));
		} finally {
			// 배너 광고 닫기가 가능 할 경우
			if(this.IsLoadBannerAds(a_eAdsPlatform)) {
				m_oFuncDictContainer01[EAdsFunc.HIDE_BANNER_ADS][a_eAdsPlatform]();
			}
		}
	}

	/** 배너 광고 높이를 설정한다 */
	private void SetupBannerAdsHeight(float a_fHeight) {
		CAccess.Assert(a_fHeight.ExIsGreateEquals(KCDefine.B_VAL_0_REAL));

		m_oRealDict[EKey.BANNER_ADS_HEIGHT] = CAccess.GetBannerAdsHeight(a_fHeight);
		m_oRealDict[EKey.ORIGIN_BANNER_ADS_HEIGHT] = a_fHeight;
	}

	/** 로드 실패 광고 정보를 추가한다 */
	private void AddLoadFailAdsInfo(string a_oKey, STLoadFailAdsInfo a_stAdsInfo) {
		CAccess.Assert(a_oKey.ExIsValid() && a_stAdsInfo.m_eAdsPlatform.ExIsValid());
		m_oLoadFailAdsInfoDict.TryAdd(a_oKey, a_stAdsInfo);
	}

	/** 로드 실패 배너 광고를 추가한다 */
	private void AddLoadFailBannerAdsInfo(EAdsPlatform a_eAdsPlatform, System.Action<EAdsPlatform> a_oCallback) {
		CAccess.Assert(a_eAdsPlatform.ExIsValid());

		this.AddLoadFailAdsInfo(string.Format(KCDefine.U_KEY_FMT_ADS_M_LOAD_FAIL_BANNER_ADS_INFO, a_eAdsPlatform), new STLoadFailAdsInfo() {
			m_eAdsPlatform = a_eAdsPlatform,
			m_oCallback = a_oCallback
		});
	}

	/** 로드 실패 보상 광고를 추가한다 */
	private void AddLoadFailRewardAdsInfo(EAdsPlatform a_eAdsPlatform, System.Action<EAdsPlatform> a_oCallback) {
		CAccess.Assert(a_eAdsPlatform.ExIsValid());

		this.AddLoadFailAdsInfo(string.Format(KCDefine.U_KEY_FMT_ADS_M_LOAD_FAIL_REWARD_ADS_INFO, a_eAdsPlatform), new STLoadFailAdsInfo() {
			m_eAdsPlatform = a_eAdsPlatform,
			m_oCallback = a_oCallback
		});
	}

	/** 로드 실패 전면 광고 정보를 추가한다 */
	private void AddLoadFailFullscreenAdsInfo(EAdsPlatform a_eAdsPlatform, System.Action<EAdsPlatform> a_oCallback) {
		CAccess.Assert(a_eAdsPlatform.ExIsValid());

		this.AddLoadFailAdsInfo(string.Format(KCDefine.U_KEY_FMT_ADS_M_LOAD_FAIL_FULLSCREEN_ADS_INFO, a_eAdsPlatform), new STLoadFailAdsInfo() {
			m_eAdsPlatform = a_eAdsPlatform,
			m_oCallback = a_oCallback
		});
	}

	/** 보상 광고 닫힘 결과를 처리한다 */
	private void HandleCloseRewardAdsResult(EAdsPlatform a_eAdsPlatform) {
		CAccess.Assert(a_eAdsPlatform.ExIsValid());
		m_oCallbackDictContainer01[EAdsCallback.CLOSE_REWARD_ADS].GetValueOrDefault(a_eAdsPlatform)?.Invoke(this);
	}

	/** 보상 광고 결과를 처리한다 */
	private void HandleRewardAdsResult(EAdsPlatform a_eAdsPlatform, STAdsRewardInfo a_stAdsRewardInfo, bool a_bIsSuccess) {
		CFunc.ShowLog($"CAdsManager.HandleRewardAdsResult: {a_eAdsPlatform}, {a_stAdsRewardInfo}, {a_bIsSuccess}", KCDefine.B_LOG_COLOR_PLUGIN);
		CAccess.Assert(a_eAdsPlatform.ExIsValid());

		m_oCallbackDictContainer02[EAdsCallback.REWARD_ADS_REWARD].GetValueOrDefault(a_eAdsPlatform)?.Invoke(this, a_stAdsRewardInfo, a_bIsSuccess);
	}

	/** 전면 광고 닫힘 결과를 처리한다 */
	private void HandleCloseFullscreenAdsResult(EAdsPlatform a_eAdsPlatform) {
		CFunc.ShowLog($"CAdsManager.HandleCloseFullscreenAdsResult: {a_eAdsPlatform}", KCDefine.B_LOG_COLOR_PLUGIN);
		CAccess.Assert(a_eAdsPlatform.ExIsValid());

		m_oCallbackDictContainer01[EAdsCallback.CLOSE_FULLSCREEN_ADS].GetValueOrDefault(a_eAdsPlatform)?.Invoke(this);
	}
#endregion // 함수

#region 클래스 함수
	/** 매개 변수를 생성한다 */
	public static STParams MakeParams(EAdsPlatform a_eAdsPlatform, EBannerAdsPos a_eBannerAdsPos, Dictionary<ECallback, System.Action<CAdsManager, EAdsPlatform, bool>> a_oCallbackDict = null) {
		return new STParams() {
			m_eAdsPlatform = a_eAdsPlatform,
			m_eBannerAdsPos = a_eBannerAdsPos,
			m_oCallbackDict = a_oCallbackDict ?? new Dictionary<ECallback, System.Action<CAdsManager, EAdsPlatform, bool>>(),

#if ADMOB_ADS_ENABLE
			m_oAdmobTestDeviceIDList = new List<string>(),
			m_oAdmobAdsIDDict = new Dictionary<string, string>(),
#endif // #if ADMOB_ADS_ENABLE

#if IRON_SRC_ADS_ENABLE
			m_oIronSrcAdsIDDict = new Dictionary<string, string>()
#endif // #if IRON_SRC_ADS_ENABLE
		};
	}
#endregion // 클래스 함수
}

/** 광고 관리자 - 접근 */
public partial class CAdsManager : CSingleton<CAdsManager> {
#region 함수
	/** 광고 초기화 여부를 검사한다 */
	public bool IsInitAds(EAdsPlatform a_eAdsPlatform) {
		CAccess.Assert(a_eAdsPlatform.ExIsValid());
		return m_oBoolDict[EKey.IS_INIT] && m_oFuncDictContainer02[EAdsFunc.IS_INIT][a_eAdsPlatform]();
	}

	/** 배너 광고 로드 여부를 검사한다 */
	public bool IsLoadBannerAds(EAdsPlatform a_eAdsPlatform) {
		CAccess.Assert(a_eAdsPlatform.ExIsValid());
		return m_oBoolDict[EKey.IS_ENABLE_BANNER_ADS] && this.IsInitAds(a_eAdsPlatform) && m_oFuncDictContainer02[EAdsFunc.IS_LOAD_BANNER_ADS][a_eAdsPlatform]();
	}

	/** 보상 광고 로드 여부를 검사한다 */
	public bool IsLoadRewardAds(EAdsPlatform a_eAdsPlatform) {
		CAccess.Assert(a_eAdsPlatform.ExIsValid());
		return m_oBoolDict[EKey.IS_ENABLE_REWARD_ADS] && this.IsInitAds(a_eAdsPlatform) && m_oFuncDictContainer02[EAdsFunc.IS_LOAD_REWARD_ADS][a_eAdsPlatform]();
	}

	/** 전면 광고 로드 여부를 검사한다 */
	public bool IsLoadFullscreenAds(EAdsPlatform a_eAdsPlatform) {
		CAccess.Assert(a_eAdsPlatform.ExIsValid());
		return m_oBoolDict[EKey.IS_ENABLE_FULLSCREEN_ADS] && this.IsInitAds(a_eAdsPlatform) && m_oFuncDictContainer02[EAdsFunc.IS_LOAD_FULLSCREEN_ADS][a_eAdsPlatform]();
	}
#endregion // 함수
}
#endif // #if ADS_MODULE_ENABLE

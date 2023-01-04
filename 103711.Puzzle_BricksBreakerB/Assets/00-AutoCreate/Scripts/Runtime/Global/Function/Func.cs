using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.Linq;
using UnityEngine.EventSystems;

#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif // #if PURCHASE_MODULE_ENABLE

#if PLAYFAB_MODULE_ENABLE
using PlayFab.SharedModels;
#endif // #if PLAYFAB_MODULE_ENABLE

#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
using GoogleSheetsToUnity;
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)

/** 기본 함수 */
public static partial class Func {
	/** 식별자 */
	private enum EKey {
		NONE = -1,

#if ADS_MODULE_ENABLE
		IS_WATCH_REWARD_ADS,
		IS_WATCH_FULLSCREEN_ADS,
#endif // #if ADS_MODULE_ENABLE

		[HideInInspector] MAX_VAL
	}

	/** 콜백 */
	private enum ECallback {
		NONE = -1,

#if ADS_MODULE_ENABLE
		SHOW_BANNER_ADS,
		SHOW_REWARD_ADS,
		SHOW_FULLSCREEN_ADS,
#endif // #if ADS_MODULE_ENABLE

#if UNITY_IOS && APPLE_LOGIN_ENABLE
		APPLE_LOGIN,
		APPLE_LOGOUT,
#endif // #if UNITY_IOS && APPLE_LOGIN_ENABLE

#if FACEBOOK_MODULE_ENABLE
		FACEBOOK_LOGIN,
		FACEBOOK_LOGOUT,
#endif // #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
		FIREBASE_LOGIN,
		FIREBASE_LOGOUT,

		LOAD_USER_INFO,
		LOAD_TARGET_INFOS,
		LOAD_PURCHASE_INFOS,

		SAVE_USER_INFO,
		SAVE_TARGET_INFOS,
		SAVE_PURCHASE_INFOS,
#endif // #if FIREBASE_MODULE_ENABLE

#if GAME_CENTER_MODULE_ENABLE
		GAME_CENTER_LOGIN,
		GAME_CENTER_LOGOUT,

		UPDATE_RECORD,
		UPDATE_ACHIEVEMENT,
#endif // #if GAME_CENTER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
		PURCHASE,
		RESTORE,
#endif // #if PURCHASE_MODULE_ENABLE

#if PLAYFAB_MODULE_ENABLE
		PLAYFAB_LOGIN,
		PLAYFAB_LOGOUT,
#endif // #if PLAYFAB_MODULE_ENABLE

#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
		LOAD_GOOGLE_SHEET,
		LOAD_GOOGLE_SHEETS,
		LOAD_VER_INFO_GOOGLE_SHEET,

		SAVE_GOOGLE_SHEET,
		SAVE_GOOGLE_SHEETS,
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)

		[HideInInspector] MAX_VAL
	}

#region 클래스 변수
	private static Dictionary<EKey, bool> m_oBoolDict = new Dictionary<EKey, bool>();

#if ADS_MODULE_ENABLE
	private static STAdsRewardInfo m_stAdsRewardInfo;
	private static Dictionary<ECallback, System.Action<CAdsManager, bool>> m_oAdsCallbackDict01 = new Dictionary<ECallback, System.Action<CAdsManager, bool>>();
	private static Dictionary<ECallback, System.Action<CAdsManager, STAdsRewardInfo, bool>> m_oAdsCallbackDict02 = new Dictionary<ECallback, System.Action<CAdsManager, STAdsRewardInfo, bool>>();
#endif // #if ADS_MODULE_ENABLE

#if UNITY_IOS && APPLE_LOGIN_ENABLE
	private static Dictionary<ECallback, System.Action<CServicesManager>> m_oServicesCallbackDict01 = new Dictionary<ECallback, System.Action<CServicesManager>>();
	private static Dictionary<ECallback, System.Action<CServicesManager, bool>> m_oServicesCallbackDict02 = new Dictionary<ECallback, System.Action<CServicesManager, bool>>();
#endif // #if UNITY_IOS && APPLE_LOGIN_ENABLE

#if FACEBOOK_MODULE_ENABLE
	private static Dictionary<ECallback, System.Action<CFacebookManager>> m_oFacebookCallbackDict01 = new Dictionary<ECallback, System.Action<CFacebookManager>>();
	private static Dictionary<ECallback, System.Action<CFacebookManager, bool>> m_oFacebookCallbackDict02 = new Dictionary<ECallback, System.Action<CFacebookManager, bool>>();
#endif // #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
	private static Dictionary<ECallback, System.Action<CFirebaseManager>> m_oFirebaseCallbackDict01 = new Dictionary<ECallback, System.Action<CFirebaseManager>>();
	private static Dictionary<ECallback, System.Action<CFirebaseManager, bool>> m_oFirebaseCallbackDict02 = new Dictionary<ECallback, System.Action<CFirebaseManager, bool>>();
	private static Dictionary<ECallback, System.Action<CFirebaseManager, string, bool>> m_oFirebaseCallbackDict03 = new Dictionary<ECallback, System.Action<CFirebaseManager, string, bool>>();
#endif // #if FIREBASE_MODULE_ENABLE

#if GAME_CENTER_MODULE_ENABLE
	private static Dictionary<ECallback, System.Action<CGameCenterManager>> m_oGameCenterCallbackDict01 = new Dictionary<ECallback, System.Action<CGameCenterManager>>();
	private static Dictionary<ECallback, System.Action<CGameCenterManager, bool>> m_oGameCenterCallbackDict02 = new Dictionary<ECallback, System.Action<CGameCenterManager, bool>>();
#endif // #if GAME_CENTER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
	private static Dictionary<ECallback, System.Action<CPurchaseManager, string, bool>> m_oPurchaseCallbackDict01 = new Dictionary<ECallback, System.Action<CPurchaseManager, string, bool>>();
	private static Dictionary<ECallback, System.Action<CPurchaseManager, List<Product>, bool>> m_oPurchaseCallbackDict02 = new Dictionary<ECallback, System.Action<CPurchaseManager, List<Product>, bool>>();
#endif // #if PURCHASE_MODULE_ENABLE

#if PLAYFAB_MODULE_ENABLE
	private static Dictionary<ECallback, System.Action<CPlayfabManager>> m_oPlayfabCallbackDict01 = new Dictionary<ECallback, System.Action<CPlayfabManager>>();
	private static Dictionary<ECallback, System.Action<CPlayfabManager, bool>> m_oPlayfabCallbackDict02 = new Dictionary<ECallback, System.Action<CPlayfabManager, bool>>();
	private static Dictionary<ECallback, System.Action<CPlayfabManager, PlayFabResultCommon, bool>> m_oPlayfabCallbackDict03 = new Dictionary<ECallback, System.Action<CPlayfabManager, PlayFabResultCommon, bool>>();
#endif // #if PLAYFAB_MODULE_ENABLE

#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	private static List<STLoadGoogleSheetInfo> m_oLoadGoogleSheetInfoList = new List<STLoadGoogleSheetInfo>();
	private static List<STSaveGoogleSheetInfo> m_oSaveGoogleSheetInfoList = new List<STSaveGoogleSheetInfo>();

	private static List<(string, int, int)> m_oGoogleSheetLoadInfoList = new List<(string, int, int)>();
	private static List<(string, int, List<List<string>>)> m_oGoogleSheetSaveInfoListContainer = new List<(string, int, List<List<string>>)>();

	private static Dictionary<string, SimpleJSON.JSONNode> m_oGoogleSheetJSONNodeDict = new Dictionary<string, SimpleJSON.JSONNode>();
	private static Dictionary<string, System.Action<CServicesManager, STGoogleSheetLoadInfo, Dictionary<string, SimpleJSON.JSONNode>, bool>> m_oGoogleSheetLoadHandlerDict = new Dictionary<string, System.Action<CServicesManager, STGoogleSheetLoadInfo, Dictionary<string, SimpleJSON.JSONNode>, bool>>();
	private static Dictionary<string, System.Action<CServicesManager, STGoogleSheetSaveInfo, bool>> m_oGoogleSheetSaveHandlerDict = new Dictionary<string, System.Action<CServicesManager, STGoogleSheetSaveInfo, bool>>();
	private static Dictionary<string, System.Func<Dictionary<string, List<List<string>>>>> m_oGoogleSheetInfoValCreatorDict = new Dictionary<string, System.Func<Dictionary<string, List<List<string>>>>>();

	private static Dictionary<ECallback, System.Action<CServicesManager, bool>> m_oGoogleSheetCallbackDict01 = new Dictionary<ECallback, System.Action<CServicesManager, bool>>();
	private static Dictionary<ECallback, System.Action<CServicesManager, SimpleJSON.JSONNode, Dictionary<string, STLoadGoogleSheetInfo>, bool>> m_oGoogleSheetCallbackDict02 = new Dictionary<ECallback, System.Action<CServicesManager, SimpleJSON.JSONNode, Dictionary<string, STLoadGoogleSheetInfo>, bool>>();
	private static Dictionary<ECallback, System.Action<CServicesManager, STGoogleSheetLoadInfo, Dictionary<string, SimpleJSON.JSONNode>, bool>> m_oGoogleSheetCallbackDict03 = new Dictionary<ECallback, System.Action<CServicesManager, STGoogleSheetLoadInfo, Dictionary<string, SimpleJSON.JSONNode>, bool>>();
	private static Dictionary<ECallback, System.Action<CServicesManager, STGoogleSheetSaveInfo, bool>> m_oGoogleSheetCallbackDict04 = new Dictionary<ECallback, System.Action<CServicesManager, STGoogleSheetSaveInfo, bool>>();
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
#endregion // 클래스 변수

#region 클래스 함수
	/** 문자열 테이블을 설정한다 */
	public static void SetupStrTable() {
		Func.SetupStrTable(CCommonAppInfoStorage.Inst.CountryCode, CCommonAppInfoStorage.Inst.SystemLanguage);
	}

	/** 문자열 테이블을 설정한다 */
	public static void SetupStrTable(string a_oCountryCode, SystemLanguage a_eSystemLanguage, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oCountryCode.ExIsValid());

		// 국가 코드가 존재 할 경우
		if(a_oCountryCode.ExIsValid()) {
			CStrTable.Inst.LoadStrsFromRes(CFactory.MakeLocalizePath(KCDefine.U_BASE_TABLE_P_G_LOCALIZE_COMMON_STR, KCDefine.U_TABLE_P_G_ENGLISH_COMMON_STR, a_oCountryCode, a_eSystemLanguage.ToString()));
		}
	}

	/** 터치 전달자를 설정한다 */
	public static void SetupTouchDispatchers(List<(GameObject, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>)> a_oKeyInfoList, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oKeyInfoList.ExIsValid());

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid()) {
			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oKeyInfoList[i].Item1?.GetComponentInChildren<CTouchDispatcher>()?.SetBeginCallback(a_oKeyInfoList[i].Item2);
				a_oKeyInfoList[i].Item1?.GetComponentInChildren<CTouchDispatcher>()?.SetMoveCallback(a_oKeyInfoList[i].Item3);
				a_oKeyInfoList[i].Item1?.GetComponentInChildren<CTouchDispatcher>()?.SetEndCallback(a_oKeyInfoList[i].Item4);
			}
		}
	}

	/** 터치 전달자를 설정한다 */
	public static void SetupTouchDispatchers(List<(string, GameObject, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>)> a_oKeyInfoList, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oKeyInfoList.ExIsValid());

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid()) {
			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oKeyInfoList[i].Item2?.ExFindComponent<CTouchDispatcher>(a_oKeyInfoList[i].Item1)?.SetBeginCallback(a_oKeyInfoList[i].Item3);
				a_oKeyInfoList[i].Item2?.ExFindComponent<CTouchDispatcher>(a_oKeyInfoList[i].Item1)?.SetMoveCallback(a_oKeyInfoList[i].Item4);
				a_oKeyInfoList[i].Item2?.ExFindComponent<CTouchDispatcher>(a_oKeyInfoList[i].Item1)?.SetEndCallback(a_oKeyInfoList[i].Item5);
			}
		}
	}

	/** 배경음을 재생한다 */
	public static CSnd PlayBGSnd(EResKinds a_eResKinds, float a_fVolume = KCDefine.B_VAL_0_REAL, bool a_bIsLoop = true, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_eResKinds.ExIsValid());

		try {
			return Func.PlayBGSnd(a_eResKinds, CSceneManager.ActiveSceneMainCamera.transform.position, a_fVolume, a_bIsLoop, a_bIsEnableAssert);
		} catch(System.Exception oException) {
			CFunc.ShowLog($"Func.PlayBGSnd Exception: {oException.Message}");
		}

		return null;
	}

	/** 배경음을 재생한다 */
	public static CSnd PlayBGSnd(EResKinds a_eResKinds, Vector3 a_stPos, float a_fVolume = KCDefine.B_VAL_0_REAL, bool a_bIsLoop = true, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_eResKinds.ExIsValid());
		return CResInfoTable.Inst.TryGetResInfo(a_eResKinds, out STResInfo stResInfo) ? CSndManager.Inst.PlayBGSnd(stResInfo.m_oResPath, a_stPos, a_fVolume, a_bIsLoop, a_bIsEnableAssert) : null;
	}

	/** 효과음을 재생한다 */
	public static CSnd PlayFXSnds(EResKinds a_eResKinds, float a_fVolume = KCDefine.B_VAL_0_REAL, bool a_bIsLoop = false, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_eResKinds.ExIsValid());

		try {
			return Func.PlayFXSnds(a_eResKinds, CSceneManager.ActiveSceneMainCamera.transform.position, a_fVolume, a_bIsLoop, a_bIsEnableAssert);
		} catch(System.Exception oException) {
			CFunc.ShowLog($"Func.PlayFXSnds Exception: {oException.Message}");
		}

		return null;
	}

	/** 효과음을 재생한다 */
	public static CSnd PlayFXSnds(EResKinds a_eResKinds, Vector3 a_stPos, float a_fVolume = KCDefine.B_VAL_0_REAL, bool a_bIsLoop = false, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_eResKinds.ExIsValid());
		return CResInfoTable.Inst.TryGetResInfo(a_eResKinds, out STResInfo stResInfo) ? CSndManager.Inst.PlayFXSnds(stResInfo.m_oResPath, a_stPos, a_fVolume, a_bIsLoop, a_bIsEnableAssert) : null;
	}
#endregion // 클래스 함수

#region 제네릭 클래스 함수
	/** 터치 전달자를 설정한다 */
	public static void SetupTouchDispatchers<K>(List<(K, GameObject, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>)> a_oKeyInfoList, Dictionary<K, CTouchDispatcher> a_oOutTouchDispatcherDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutTouchDispatcherDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutTouchDispatcherDict != null) {
			CFunc.SetupComponents(Factory.MakeKeyInfos(a_oKeyInfoList), a_oOutTouchDispatcherDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutTouchDispatcherDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.SetBeginCallback(a_oKeyInfoList[i].Item3);
				a_oOutTouchDispatcherDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.SetMoveCallback(a_oKeyInfoList[i].Item4);
				a_oOutTouchDispatcherDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.SetEndCallback(a_oKeyInfoList[i].Item5);
			}
		}
	}

	/** 터치 전달자를 설정한다 */
	public static void SetupTouchDispatchers<K>(List<(K, string, GameObject, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>)> a_oKeyInfoList, Dictionary<K, CTouchDispatcher> a_oOutTouchDispatcherDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutTouchDispatcherDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutTouchDispatcherDict != null) {
			CFunc.SetupComponents(Factory.MakeKeyInfos(a_oKeyInfoList), a_oOutTouchDispatcherDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutTouchDispatcherDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.SetBeginCallback(a_oKeyInfoList[i].Item4);
				a_oOutTouchDispatcherDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.SetMoveCallback(a_oKeyInfoList[i].Item5);
				a_oOutTouchDispatcherDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.SetEndCallback(a_oKeyInfoList[i].Item6);
			}
		}
	}

	/** 터치 전달자를 설정한다 */
	public static void SetupTouchDispatchers<K>(List<(K, string, GameObject, GameObject, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>)> a_oKeyInfoList, Dictionary<K, CTouchDispatcher> a_oOutTouchDispatcherDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKeyInfoList.ExIsValid() && a_oOutTouchDispatcherDict != null));

		// 키 정보가 존재 할 경우
		if(a_oKeyInfoList.ExIsValid() && a_oOutTouchDispatcherDict != null) {
			CFunc.SetupComponents(Factory.MakeKeyInfos(a_oKeyInfoList), a_oOutTouchDispatcherDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oKeyInfoList.Count; ++i) {
				a_oOutTouchDispatcherDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.SetBeginCallback(a_oKeyInfoList[i].Item5);
				a_oOutTouchDispatcherDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.SetMoveCallback(a_oKeyInfoList[i].Item6);
				a_oOutTouchDispatcherDict.GetValueOrDefault(a_oKeyInfoList[i].Item1)?.SetEndCallback(a_oKeyInfoList[i].Item7);
			}
		}
	}
#endregion // 제네릭 클래스 함수

#region 조건부 클래스 함수
#if ADS_MODULE_ENABLE
	/** 배너 광고를 출력한다 */
	public static void ShowBannerAds(System.Action<CAdsManager, bool> a_oCallback) {
		Func.ShowBannerAds(CPluginInfoTable.Inst.AdsPlatform, a_oCallback);
	}

	/** 배너 광고를 출력한다 */
	public static void ShowBannerAds(EAdsPlatform a_eAdsPlatform, System.Action<CAdsManager, bool> a_oCallback) {
		// 배너 광고 출력이 가능 할 경우
		if(CAdsManager.Inst.IsLoadBannerAds(a_eAdsPlatform)) {
			Func.m_oAdsCallbackDict01.ExReplaceVal(ECallback.SHOW_BANNER_ADS, a_oCallback);
			CSceneManager.ActiveSceneManager.ExLateCallFunc((a_oSender) => CAdsManager.Inst.ShowBannerAds(a_eAdsPlatform, Func.OnShowBannerAds));
		} else {
			CFunc.Invoke(ref a_oCallback, CAdsManager.Inst, false);
		}
	}

	/** 배너 광고를 닫는다 */
	public static void CloseBannerAds(System.Action<CAdsManager, bool> a_oCallback) {
		Func.CloseBannerAds(CPluginInfoTable.Inst.AdsPlatform, a_oCallback);
	}

	/** 배너 광고를 닫는다 */
	public static void CloseBannerAds(EAdsPlatform a_eAdsPlatform, System.Action<CAdsManager, bool> a_oCallback) {
		CAdsManager.Inst.CloseBannerAds(a_eAdsPlatform, a_oCallback);
	}

	/** 보상 광고를 출력한다 */
	public static void ShowRewardAds(System.Action<CAdsManager, STAdsRewardInfo, bool> a_oCallback) {
		Func.ShowRewardAds(CPluginInfoTable.Inst.AdsPlatform, a_oCallback);
	}

	/** 보상 광고를 출력한다 */
	public static void ShowRewardAds(EAdsPlatform a_eAdsPlatform, System.Action<CAdsManager, STAdsRewardInfo, bool> a_oCallback) {
		// 보상 광고 출력이 가능 할 경우
		if(CAdsManager.Inst.IsLoadRewardAds(a_eAdsPlatform)) {
			CIndicatorManager.Inst.Show();

			CSceneManager.ActiveSceneManager.ExLateCallFunc((a_oSender) => {
				Func.m_stAdsRewardInfo = STAdsRewardInfo.INVALID;
				Func.m_oBoolDict.ExReplaceVal(EKey.IS_WATCH_REWARD_ADS, false);
				Func.m_oAdsCallbackDict02.ExReplaceVal(ECallback.SHOW_REWARD_ADS, a_oCallback);

				CAdsManager.Inst.ShowRewardAds(a_eAdsPlatform, Func.OnReceiveAdsReward, null, Func.OnCloseRewardAds);
			});
		} else {
			CFunc.Invoke(ref a_oCallback, CAdsManager.Inst, STAdsRewardInfo.INVALID, false);
		}
	}

	/** 전면 광고를 출력한다 */
	public static void ShowFullscreenAds(System.Action<CAdsManager, bool> a_oCallback) {
		Func.ShowFullscreenAds(CPluginInfoTable.Inst.AdsPlatform, a_oCallback);
	}

	/** 전면 광고를 출력한다 */
	public static void ShowFullscreenAds(EAdsPlatform a_eAdsPlatform, System.Action<CAdsManager, bool> a_oCallback) {
		// 전면 광고 출력이 가능 할 경우
		if(CAppInfoStorage.Inst.IsEnableShowFullscreenAds && CAdsManager.Inst.IsLoadFullscreenAds(a_eAdsPlatform)) {
			CIndicatorManager.Inst.Show();

			CSceneManager.ActiveSceneManager.ExLateCallFunc((a_oSender) => {
				CIndicatorManager.Inst.Close();

				// 전면 광고 출력이 가능 할 경우
				if(CAppInfoStorage.Inst.IsEnableShowFullscreenAds) {
					Func.m_oBoolDict.ExReplaceVal(EKey.IS_WATCH_FULLSCREEN_ADS, true);
					Func.m_oAdsCallbackDict01.ExReplaceVal(ECallback.SHOW_FULLSCREEN_ADS, a_oCallback);

					CAdsManager.Inst.ShowFullscreenAds(a_eAdsPlatform, null, Func.OnCloseFullscreenAds);
				} else {
					CFunc.Invoke(ref a_oCallback, CAdsManager.Inst, false);
				}
			}, KCDefine.B_VAL_2_REAL, true);
		} else {
			Func.IncrAdsSkipTimes(KCDefine.B_VAL_1_INT);
			CFunc.Invoke(ref a_oCallback, CAdsManager.Inst, false);
		}
	}

	/** 배너 광고가 출력 되었을 경우 */
	private static void OnShowBannerAds(CAdsManager a_oSender, bool a_bIsSuccess) {
		Func.m_oAdsCallbackDict01.GetValueOrDefault(ECallback.SHOW_BANNER_ADS)?.Invoke(a_oSender, a_bIsSuccess);
	}

	/** 보상 광고가 닫혔을 경우 */
	private static void OnCloseRewardAds(CAdsManager a_oSender) {
		CIndicatorManager.Inst.Close();
		CAppInfoStorage.Inst.SetPrevRewardAdsTime(System.DateTime.Now);

		Func.IncrRewardAdsWatchTimes(KCDefine.B_VAL_1_INT);
		CAppInfoStorage.Inst.SaveAppInfo();

		Func.m_oAdsCallbackDict02.GetValueOrDefault(ECallback.SHOW_REWARD_ADS)?.Invoke(a_oSender, Func.m_stAdsRewardInfo, Func.m_oBoolDict.GetValueOrDefault(EKey.IS_WATCH_REWARD_ADS));
	}

	/** 광고 보상을 수신했을 경우 */
	private static void OnReceiveAdsReward(CAdsManager a_oSender, STAdsRewardInfo a_stAdsRewardInfo, bool a_bIsSuccess) {
		Func.m_stAdsRewardInfo = a_stAdsRewardInfo;
		Func.m_oBoolDict.ExReplaceVal(EKey.IS_WATCH_REWARD_ADS, a_bIsSuccess);
	}

	/** 전면 광고가 닫혔을 경우 */
	private static void OnCloseFullscreenAds(CAdsManager a_oSender) {
		CAppInfoStorage.Inst.SetAdsSkipTimes(KCDefine.B_VAL_0_INT);
		CAppInfoStorage.Inst.SetPrevFullscreenAdsTime(System.DateTime.Now);

		Func.IncrFullscreenAdsWatchTimes(KCDefine.B_VAL_1_INT);

		CAppInfoStorage.Inst.SaveAppInfo();
		Func.m_oAdsCallbackDict01.GetValueOrDefault(ECallback.SHOW_FULLSCREEN_ADS)?.Invoke(a_oSender, Func.m_oBoolDict.GetValueOrDefault(EKey.IS_WATCH_FULLSCREEN_ADS));
	}
#endif // #if ADS_MODULE_ENABLE

#if UNITY_IOS && APPLE_LOGIN_ENABLE
	/** 애플 로그인을 처리한다 */
	public void AppleLogin(System.Action<CServicesManager, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oServicesCallbackDict02.ExReplaceVal(ECallback.APPLE_LOGIN, a_oCallback);

		CServicesManager.Inst.LoginWithApple(Func.OnAppleLogin);
	}

	/** 애플 로그아웃을 처리한다 */
	public static void AppleLogout(System.Action<CServicesManager> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oServicesCallbackDict01.ExReplaceVal(ECallback.APPLE_LOGOUT, a_oCallback);

		CServicesManager.Inst.LogoutWithApple(Func.OnAppleLogout);
	}

	/** 애플에 로그인 되었을 경우 */
	private static void OnAppleLogin(CServicesManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oServicesCallbackDict02.GetValueOrDefault(ECallback.APPLE_LOGIN)?.Invoke(a_oSender, a_bIsSuccess);
	}

	/** 애플에서 로그아웃 되었을 경우 */
	private static void OnAppleLogout(CServicesManager a_oSender) {
		CIndicatorManager.Inst.Close();
		Func.m_oServicesCallbackDict01.GetValueOrDefault(ECallback.APPLE_LOGOUT)?.Invoke(a_oSender);
	}
#endif // #if UNITY_IOS && APPLE_LOGIN_ENABLE

#if FACEBOOK_MODULE_ENABLE
	/** 페이스 북 로그인을 처리한다 */
	public static void FacebookLogin(System.Action<CFacebookManager, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oFacebookCallbackDict02.ExReplaceVal(ECallback.FACEBOOK_LOGIN, a_oCallback);

		CFacebookManager.Inst.Login(KCDefine.U_KEY_FACEBOOK_PERMISSION_LIST, Func.OnFacebookLogin);
	}

	/** 페이스 북 로그아웃을 처리한다 */
	public static void FacebookLogout(System.Action<CFacebookManager> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oFacebookCallbackDict01.ExReplaceVal(ECallback.FACEBOOK_LOGOUT, a_oCallback);

		CFacebookManager.Inst.Logout(Func.OnFacebookLogout);
	}

	/** 페이스 북에 로그인 되었을 경우 */
	private static void OnFacebookLogin(CFacebookManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oFacebookCallbackDict02.GetValueOrDefault(ECallback.FACEBOOK_LOGIN)?.Invoke(a_oSender, a_bIsSuccess);
	}

	/** 페이스 북에서 로그아웃 되었을 경우 */
	private static void OnFacebookLogout(CFacebookManager a_oSender) {
		CIndicatorManager.Inst.Close();
		Func.m_oFacebookCallbackDict01.GetValueOrDefault(ECallback.FACEBOOK_LOGOUT)?.Invoke(a_oSender);
	}
#endif // #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
	/** 파이어 베이스 로그인을 처리한다 */
	public static void FirebaseLogin(System.Action<CFirebaseManager, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oFirebaseCallbackDict02.ExReplaceVal(ECallback.FIREBASE_LOGIN, a_oCallback);

#if UNITY_IOS && APPLE_LOGIN_ENABLE
		Func.AppleLogin(Func.OnFirebaseAppleLogin);
#elif (UNITY_IOS || UNITY_ANDROID) && FACEBOOK_MODULE_ENABLE
		Func.FacebookLogin(Func.OnFirebaseFacebookLogin);
#else
		CFirebaseManager.Inst.Login(CCommonAppInfoStorage.Inst.AppInfo.DeviceID, Func.OnFirebaseLogin);
#endif // #if UNITY_IOS && APPLE_LOGIN_ENABLE
	}

	/** 파이어 베이스 로그아웃을 처리한다 */
	public static void FirebaseLogout(System.Action<CFirebaseManager> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oFirebaseCallbackDict01.ExReplaceVal(ECallback.FIREBASE_LOGOUT, a_oCallback);

#if UNITY_IOS && APPLE_LOGIN_ENABLE
		Func.AppleLogout(Func.OnFirebaseAppleLogout);
#elif (UNITY_IOS || UNITY_ANDROID) && FACEBOOK_MODULE_ENABLE
		Func.FacebookLogout(Func.OnFirebaseFacebookLogout);
#else
		CFirebaseManager.Inst.Logout(Func.OnFirebaseLogout);
#endif // #if UNITY_IOS && APPLE_LOGIN_ENABLE
	}

	/** 유저 정보를 로드한다 */
	public static void LoadUserInfo(System.Action<CFirebaseManager, string, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oFirebaseCallbackDict03.ExReplaceVal(ECallback.LOAD_USER_INFO, a_oCallback);

		// 로그인 되었을 경우
		if(CFirebaseManager.Inst.IsLogin) {
			CFirebaseManager.Inst.LoadDatas(Factory.MakeUserInfoNodes(), Func.OnLoadUserInfo);
		} else {
			Func.OnLoadUserInfo(CFirebaseManager.Inst, string.Empty, false);
		}
	}

	/** 타겟 정보를 로드한다 */
	public static void LoadTargetInfos(System.Action<CFirebaseManager, string, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oFirebaseCallbackDict03.ExReplaceVal(ECallback.LOAD_TARGET_INFOS, a_oCallback);

		// 로그인 되었을 경우
		if(CFirebaseManager.Inst.IsLogin) {
			CFirebaseManager.Inst.LoadDatas(Factory.MakeTargetInfoNodes(), Func.OnLoadTargetInfos);
		} else {
			Func.OnLoadTargetInfos(CFirebaseManager.Inst, string.Empty, false);
		}
	}

	/** 결제 정보를 로드한다 */
	public static void LoadPurchaseInfos(System.Action<CFirebaseManager, string, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oFirebaseCallbackDict03.ExReplaceVal(ECallback.LOAD_PURCHASE_INFOS, a_oCallback);

		// 로그인 되었을 경우
		if(CFirebaseManager.Inst.IsLogin) {
			CFirebaseManager.Inst.LoadDatas(Factory.MakePurchaseInfoNodes(), Func.OnLoadPurchaseInfos);
		} else {
			Func.OnLoadPurchaseInfos(CFirebaseManager.Inst, string.Empty, false);
		}
	}

	/** 유저 정보를 저장한다 */
	public static void SaveUserInfo(System.Action<CFirebaseManager, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oFirebaseCallbackDict02.ExReplaceVal(ECallback.SAVE_USER_INFO, a_oCallback);

		// 로그인 되었을 경우
		if(CFirebaseManager.Inst.IsLogin) {
			var oJSONNode = new SimpleJSON.JSONClass();
			oJSONNode.Add(KCDefine.B_KEY_JSON_USER_INFO_DATA, CUserInfoStorage.Inst.UserInfo.ExToMsgPackBase64Str());
			oJSONNode.Add(KCDefine.B_KEY_JSON_GAME_INFO_DATA, CGameInfoStorage.Inst.GameInfo.ExToMsgPackBase64Str());
			oJSONNode.Add(KCDefine.B_KEY_JSON_COMMON_APP_INFO_DATA, CCommonAppInfoStorage.Inst.AppInfo.ExToMsgPackBase64Str());
			oJSONNode.Add(KCDefine.B_KEY_JSON_COMMON_USER_INFO_DATA, CCommonUserInfoStorage.Inst.UserInfo.ExToMsgPackBase64Str());

			CFirebaseManager.Inst.SaveDatas(Factory.MakeUserInfoNodes(), oJSONNode.ToString(), Func.OnSaveUserInfo);
		} else {
			Func.OnSaveUserInfo(CFirebaseManager.Inst, false);
		}
	}

	/** 타겟 정보를 저장한다 */
	public static void SaveTargetInfos(Dictionary<ulong, STTargetInfo> a_oTargetInfoDict, System.Action<CFirebaseManager, bool> a_oCallback, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oTargetInfoDict != null);

		// 타겟 정보가 존재 할 경우
		if(a_oTargetInfoDict != null) {
			CIndicatorManager.Inst.Show();
			Func.m_oFirebaseCallbackDict02.ExReplaceVal(ECallback.SAVE_TARGET_INFOS, a_oCallback);

			// 로그인 되었을 경우
			if(CFirebaseManager.Inst.IsLogin) {
#if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
				CFirebaseManager.Inst.SaveDatas(Factory.MakeTargetInfoNodes(), a_oTargetInfoDict.ExToJSONStr(true), Func.OnSaveTargetInfos);
#else
				Func.OnSaveTargetInfos(CFirebaseManager.Inst, false);
#endif // #if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
			} else {
				Func.OnSaveTargetInfos(CFirebaseManager.Inst, false);
			}
		}
	}

	/** 결제 정보를 저장한다 */
	public static void SavePurchaseInfos(List<STPurchaseInfo> a_oPurchaseInfoList, System.Action<CFirebaseManager, bool> a_oCallback, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oPurchaseInfoList != null);

		// 결제 정보가 존재 할 경우
		if(a_oPurchaseInfoList != null) {
			CIndicatorManager.Inst.Show();
			Func.m_oFirebaseCallbackDict02.ExReplaceVal(ECallback.SAVE_PURCHASE_INFOS, a_oCallback);

			// 로그인 되었을 경우
			if(CFirebaseManager.Inst.IsLogin) {
#if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
				CFirebaseManager.Inst.SaveDatas(Factory.MakePurchaseInfoNodes(), a_oPurchaseInfoList.ExToJSONStr(true), Func.OnSavePurchaseInfos);
#else
				Func.OnSavePurchaseInfos(CFirebaseManager.Inst, false);
#endif // #if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
			} else {
				Func.OnSavePurchaseInfos(CFirebaseManager.Inst, false);
			}
		}
	}

	/** 파이어 베이스에 로그인 되었을 경우 */
	private static void OnFirebaseLogin(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oFirebaseCallbackDict02.GetValueOrDefault(ECallback.FIREBASE_LOGIN)?.Invoke(a_oSender, a_bIsSuccess);
	}

	/** 파이어 베이스에서 로그아웃 되었을 경우 */
	private static void OnFirebaseLogout(CFirebaseManager a_oSender) {
		CIndicatorManager.Inst.Close();
		Func.m_oFirebaseCallbackDict01.GetValueOrDefault(ECallback.FIREBASE_LOGOUT)?.Invoke(a_oSender);
	}

	/** 유저 정보가 로드 되었을 경우 */
	private static void OnLoadUserInfo(CFirebaseManager a_oSender, string a_oJSONStr, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oFirebaseCallbackDict03.GetValueOrDefault(ECallback.LOAD_USER_INFO)?.Invoke(a_oSender, a_oJSONStr, a_bIsSuccess);
	}

	/** 타겟 정보가 로드 되었을 경우 */
	private static void OnLoadTargetInfos(CFirebaseManager a_oSender, string a_oJSONStr, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oFirebaseCallbackDict03.GetValueOrDefault(ECallback.LOAD_TARGET_INFOS)?.Invoke(a_oSender, a_oJSONStr, a_bIsSuccess);
	}

	/** 결제 정보가 로드 되었을 경우 */
	private static void OnLoadPurchaseInfos(CFirebaseManager a_oSender, string a_oJSONStr, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oFirebaseCallbackDict03.GetValueOrDefault(ECallback.LOAD_PURCHASE_INFOS)?.Invoke(a_oSender, a_oJSONStr, a_bIsSuccess);
	}

	/** 유저 정보가 저장 되었을 경우 */
	private static void OnSaveUserInfo(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oFirebaseCallbackDict02.GetValueOrDefault(ECallback.SAVE_USER_INFO)?.Invoke(a_oSender, a_bIsSuccess);
	}

	/** 타겟 정보가 저장 되었을 경우 */
	private static void OnSaveTargetInfos(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oFirebaseCallbackDict02.GetValueOrDefault(ECallback.SAVE_TARGET_INFOS)?.Invoke(a_oSender, a_bIsSuccess);
	}

	/** 결제 정보가 저장 되었을 경우 */
	private static void OnSavePurchaseInfos(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oFirebaseCallbackDict02.GetValueOrDefault(ECallback.SAVE_PURCHASE_INFOS)?.Invoke(a_oSender, a_bIsSuccess);
	}

#if UNITY_IOS && APPLE_LOGIN_ENABLE
	/** 애플에 로그인 되었을 경우 */
	private static void OnFirebaseAppleLogin(CServicesManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();

		// 로그인 되었을 경우
		if(a_bIsSuccess) {
			CIndicatorManager.Inst.Show();
			CFirebaseManager.Inst.LoginWithApple(a_oSender.AppleUserID, a_oSender.AppleIDToken, Func.OnFirebaseLogin);
		} else {
			Func.OnFirebaseLogin(CFirebaseManager.Inst, false);
		}
	}

	/** 애플에서 로그아웃 되었을 경우 */
	private static void OnFirebaseAppleLogout(CServicesManager a_oSender) {
		CFirebaseManager.Inst.Logout(Func.OnFirebaseLogout);
	}
#endif // #if UNITY_IOS && APPLE_LOGIN_ENABLE

#if(UNITY_IOS || UNITY_ANDROID) && FACEBOOK_MODULE_ENABLE
	/** 페이스 북에 로그인 되었을 경우 */
	private static void OnFirebaseFacebookLogin(CFacebookManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();

		// 로그인 되었을 경우
		if(a_bIsSuccess) {
			CIndicatorManager.Inst.Show();
			CFirebaseManager.Inst.LoginWithFacebook(a_oSender.AccessToken, Func.OnFirebaseLogin);
		} else {
			Func.OnFirebaseLogin(CFirebaseManager.Inst, false);
		}
	}

	/** 페이스 북에서 로그아웃 되었을 경우 */
	private static void OnFirebaseFacebookLogout(CFacebookManager a_oSender) {
		CFirebaseManager.Inst.Logout(Func.OnFirebaseLogout);
	}
#endif // #if (UNITY_IOS || UNITY_ANDROID) && FACEBOOK_MODULE_ENABLE
#endif // #if FIREBASE_MODULE_ENABLE

#if GAME_CENTER_MODULE_ENABLE
	/** 게임 센터 로그인을 처리한다 */
	public static void GameCenterLogin(System.Action<CGameCenterManager, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oGameCenterCallbackDict02.ExReplaceVal(ECallback.GAME_CENTER_LOGIN, a_oCallback);

		CGameCenterManager.Inst.Login(Func.OnGameCenterLogin);
	}

	/** 게임 센터 로그아웃을 처리한다 */
	public static void GameCenterLogout(System.Action<CGameCenterManager> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oGameCenterCallbackDict01.ExReplaceVal(ECallback.GAME_CENTER_LOGOUT, a_oCallback);

		CGameCenterManager.Inst.Logout(Func.OnGameCenterLogout);
	}

	/** 기록을 갱신한다 */
	public static void UpdateRecord(string a_oLeaderboardID, long a_nRecord, System.Action<CGameCenterManager, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oGameCenterCallbackDict02.ExReplaceVal(ECallback.UPDATE_RECORD, a_oCallback);

		CGameCenterManager.Inst.UpdateRecord(a_oLeaderboardID, a_nRecord, Func.OnUpdateRecord);
	}

	/** 업적을 갱신한다 */
	public static void UpdateAchievement(string a_oAchievementID, double a_dblPercent, System.Action<CGameCenterManager, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oGameCenterCallbackDict02.ExReplaceVal(ECallback.UPDATE_ACHIEVEMENT, a_oCallback);

		CGameCenterManager.Inst.UpdateAchievement(a_oAchievementID, a_dblPercent * KCDefine.B_UNIT_NORM_VAL_TO_PERCENT, Func.OnUpdateAchievement);
	}

	/** 게임 센터에 로그인 되었을 경우 */
	private static void OnGameCenterLogin(CGameCenterManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oGameCenterCallbackDict02.GetValueOrDefault(ECallback.GAME_CENTER_LOGIN)?.Invoke(a_oSender, a_bIsSuccess);
	}

	/** 게임 센터에서 로그아웃 되었을 경우 */
	private static void OnGameCenterLogout(CGameCenterManager a_oSender) {
		CIndicatorManager.Inst.Close();
		Func.m_oGameCenterCallbackDict01.GetValueOrDefault(ECallback.GAME_CENTER_LOGOUT)?.Invoke(a_oSender);
	}
	
	/** 기록이 갱신 되었을 경우 */
	private static void OnUpdateRecord(CGameCenterManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oGameCenterCallbackDict02.GetValueOrDefault(ECallback.UPDATE_RECORD)?.Invoke(a_oSender, a_bIsSuccess);
	}

	/** 업적이 갱신 되었을 경우 */
	private static void OnUpdateAchievement(CGameCenterManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oGameCenterCallbackDict02.GetValueOrDefault(ECallback.UPDATE_ACHIEVEMENT)?.Invoke(a_oSender, a_bIsSuccess);
	}
#endif // #if GAME_CENTER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
	/** 상품을 결제한다 */
	public static void PurchaseProduct(int a_nProductIdx, System.Action<CPurchaseManager, string, bool> a_oCallback, bool a_bIsEnableAssert = true) {
		Func.PurchaseProduct(CProductInfoTable.Inst.GetProductInfo(a_nProductIdx).m_oID, a_oCallback, a_bIsEnableAssert);
	}

	/** 상품을 결제한다 */
	public static void PurchaseProduct(EProductKinds a_eProductKinds, System.Action<CPurchaseManager, string, bool> a_oCallback, bool a_bIsEnableAssert = true) {
		bool bIsValid = CProductTradeInfoTable.Inst.TryGetBuyProductTradeInfo(a_eProductKinds, out STProductTradeInfo stProductTradeInfo);
		CAccess.Assert(!a_bIsEnableAssert || bIsValid);

		// 상품이 존재 할 경우
		if(bIsValid) {
			Func.PurchaseProduct(stProductTradeInfo.m_nProductIdx, a_oCallback, a_bIsEnableAssert);
		}
	}

	/** 상품을 결제한다 */
	public static void PurchaseProduct(string a_oID, System.Action<CPurchaseManager, string, bool> a_oCallback, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oID.ExIsValid());

		// 식별자가 유효 할 경우
		if(a_oID.ExIsValid()) {
			CIndicatorManager.Inst.Show();
			Func.m_oPurchaseCallbackDict01.ExReplaceVal(ECallback.PURCHASE, a_oCallback);

			CPurchaseManager.Inst.PurchaseProduct(a_oID, Func.OnPurchaseProduct);
		}
	}

	/** 상품을 복원한다 */
	public static void RestoreProducts(System.Action<CPurchaseManager, List<Product>, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oPurchaseCallbackDict02.ExReplaceVal(ECallback.RESTORE, a_oCallback);

		CPurchaseManager.Inst.RestoreProducts(Func.OnRestoreProducts);
	}

	/** 상품이 결제 되었을 경우 */
	private static void OnPurchaseProduct(CPurchaseManager a_oSender, string a_oProductID, bool a_bIsSuccess) {
		CPurchaseManager.Inst.ConfirmPurchase(a_oProductID, (a_oSender, a_oConfirmProductID, a_bIsConfirmSuccess) => {
			CIndicatorManager.Inst.Close();
			Func.m_oPurchaseCallbackDict01.GetValueOrDefault(ECallback.PURCHASE)?.Invoke(a_oSender, a_oConfirmProductID, a_bIsConfirmSuccess);
		});
	}

	/** 상품이 복원 되었을 경우 */
	private static void OnRestoreProducts(CPurchaseManager a_oSender, List<Product> a_oProductList, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oPurchaseCallbackDict02.GetValueOrDefault(ECallback.RESTORE)?.Invoke(a_oSender, a_oProductList, a_bIsSuccess);
	}
#endif // #if PURCHASE_MODULE_ENABLE

#if PLAYFAB_MODULE_ENABLE
	/** 플레이 팹 로그인을 처리한다 */
	public static void PlayfabLogin(System.Action<CPlayfabManager, bool> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oPlayfabCallbackDict02.ExReplaceVal(ECallback.PLAYFAB_LOGIN, a_oCallback);

#if UNITY_IOS && APPLE_LOGIN_ENABLE
		Func.AppleLogin(Func.OnPlayfabAppleLogin);
#elif (UNITY_IOS || UNITY_ANDROID) && FACEBOOK_MODULE_ENABLE
		Func.FacebookLogin(Func.OnPlayfabFacebookLogin);
#else
		CPlayfabManager.Inst.Login(CCommonAppInfoStorage.Inst.AppInfo.DeviceID, Func.OnPlayfabLogin);
#endif // #if UNITY_IOS && APPLE_LOGIN_ENABLE
	}

	/** 플레이 팹 로그아웃을 처리한다 */
	public static void PlayfabLogout(System.Action<CPlayfabManager> a_oCallback) {
		CIndicatorManager.Inst.Show();
		Func.m_oPlayfabCallbackDict01.ExReplaceVal(ECallback.PLAYFAB_LOGOUT, a_oCallback);

#if UNITY_IOS && APPLE_LOGIN_ENABLE
		Func.AppleLogout(Func.OnPlayfabAppleLogout);
#elif (UNITY_IOS || UNITY_ANDROID) && FACEBOOK_MODULE_ENABLE
		Func.FacebookLogout(Func.OnPlayfabFacebookLogout);
#else
		CPlayfabManager.Inst.Logout(Func.OnPlayfabLogout);
#endif // #if UNITY_IOS && APPLE_LOGIN_ENABLE
	}

	/** 플레이 팹에 로그인 되었을 경우 */
	private static void OnPlayfabLogin(CPlayfabManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();
		Func.m_oPlayfabCallbackDict02.GetValueOrDefault(ECallback.PLAYFAB_LOGIN)?.Invoke(a_oSender, a_bIsSuccess);
	}

	/** 플레이 팹에서 로그아웃 되었을 경우 */
	private static void OnPlayfabLogout(CPlayfabManager a_oSender) {
		CIndicatorManager.Inst.Close();
		Func.m_oPlayfabCallbackDict01.GetValueOrDefault(ECallback.PLAYFAB_LOGOUT)?.Invoke(a_oSender);
	}

#if UNITY_IOS && APPLE_LOGIN_ENABLE
	/** 애플에 로그인 되었을 경우 */
	private static void OnPlayfabAppleLogin(CServicesManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();

		// 로그인 되었을 경우
		if(a_bIsSuccess) {
			CIndicatorManager.Inst.Show();
			CPlayfabManager.Inst.LoginWithApple(a_oSender.AppleUserID, a_oSender.AppleIDToken, Func.OnPlayfabLogin);
		} else {
			Func.OnPlayfabLogin(CPlayfabManager.Inst, false);
		}
	}

	/** 애플에서 로그아웃 되었을 경우 */
	private static void OnPlayfabAppleLogout(CServicesManager a_oSender) {
		CPlayfabManager.Inst.Logout(Func.OnPlayfabLogout);
	}
#endif // #if UNITY_IOS && APPLE_LOGIN_ENABLE

#if(UNITY_IOS || UNITY_ANDROID) && FACEBOOK_MODULE_ENABLE
	/** 페이스 북에 로그인 되었을 경우 */
	private static void OnPlayfabFacebookLogin(CFacebookManager a_oSender, bool a_bIsSuccess) {
		CIndicatorManager.Inst.Close();

		// 로그인 되었을 경우
		if(a_bIsSuccess) {
			CIndicatorManager.Inst.Show();
			CPlayfabManager.Inst.LoginWithFacebook(a_oSender.AccessToken, Func.OnPlayfabLogin);
		} else {
			Func.OnPlayfabLogin(CPlayfabManager.Inst, false);
		}
	}

	/** 페이스 북에서 로그아웃 되었을 경우 */
	private static void OnPlayfabFacebookLogout(CFacebookManager a_oSender) {
		CPlayfabManager.Inst.Logout(Func.OnPlayfabLogout);
	}
#endif // #if (UNITY_IOS || UNITY_ANDROID) && FACEBOOK_MODULE_ENABLE
#endif // #if PLAYFAB_MODULE_ENABLE

#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 구글 시트 정보 값 생성자를 설정한다 */
	public static void SetupGoogleSheetInfoValCreators() {
		m_oGoogleSheetInfoValCreatorDict.TryAdd(KDefine.G_TABLE_N_ETC_INFO, CEtcInfoTable.Inst.MakeEtcInfoVals);
		m_oGoogleSheetInfoValCreatorDict.TryAdd(KDefine.G_TABLE_N_MISSION_INFO, CMissionInfoTable.Inst.MakeMissionInfoVals);
		m_oGoogleSheetInfoValCreatorDict.TryAdd(KDefine.G_TABLE_N_REWARD_INFO, CRewardInfoTable.Inst.MakeRewardInfoVals);
		m_oGoogleSheetInfoValCreatorDict.TryAdd(KDefine.G_TABLE_N_RES_INFO, CResInfoTable.Inst.MakeResInfoVals);
		m_oGoogleSheetInfoValCreatorDict.TryAdd(KDefine.G_TABLE_N_ITEM_INFO, CItemInfoTable.Inst.MakeItemInfoVals);
		m_oGoogleSheetInfoValCreatorDict.TryAdd(KDefine.G_TABLE_N_SKILL_INFO, CSkillInfoTable.Inst.MakeSkillInfoVals);
		m_oGoogleSheetInfoValCreatorDict.TryAdd(KDefine.G_TABLE_N_OBJ_INFO, CObjInfoTable.Inst.MakeObjInfoVals);
		m_oGoogleSheetInfoValCreatorDict.TryAdd(KDefine.G_TABLE_N_ABILITY_INFO, CAbilityInfoTable.Inst.MakeAbilityInfoVals);
		m_oGoogleSheetInfoValCreatorDict.TryAdd(KDefine.G_TABLE_N_PRODUCT_INFO, CProductTradeInfoTable.Inst.MakeProductTradeInfoVals);
	}

	/** 로드 구글 시트 정보를 설정한다 */
	public static void SetupLoadGoogleSheetInfos(Dictionary<string, STLoadGoogleSheetInfo> a_oOutLoadGoogleSheetInfoDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oOutLoadGoogleSheetInfoDict != null);

		// 로드 구글 시트 정보 설정이 가능 할 경우
		if(a_oOutLoadGoogleSheetInfoDict != null) {
			foreach(var stKeyVal in KDefine.G_TABLE_INFO_GOOGLE_SHEET_DICT) {
				// 버전 정보 테이블이 아닐 경우
				if(!stKeyVal.Key.Equals(KDefine.G_TABLE_N_VER_INFO)) {
					Func.DoSetupLoadGoogleSheetInfos(stKeyVal.Value, a_oOutLoadGoogleSheetInfoDict, KDefine.G_TABLE_INFO_NUM_ROWS_DICT.GetValueOrDefault(stKeyVal.Key), a_bIsEnableAssert);
				}
			}
		}
	}

	/** 저장 구글 시트 정보를 설정한다 */
	public static void SetupSaveGoogleSheetInfos(Dictionary<string, STSaveGoogleSheetInfo> a_oOutSaveGoogleSheetInfoDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oOutSaveGoogleSheetInfoDict != null);

		// 저장 구글 시트 정보 설정이 가능 할 경우
		if(a_oOutSaveGoogleSheetInfoDict != null) {
			foreach(var stKeyVal in KDefine.G_TABLE_INFO_GOOGLE_SHEET_DICT) {
				// 버전 정보 테이블이 아닐 경우
				if(!stKeyVal.Key.Equals(KDefine.G_TABLE_N_VER_INFO)) {
					Func.DoSetupSaveGoogleSheetInfos(stKeyVal.Value, a_oOutSaveGoogleSheetInfoDict, a_bIsEnableAssert);
				}
			}
		}
	}

	/** 구글 시트를 로드한다 */
	public static void LoadGoogleSheet(string a_oID, List<(string, int)> a_oInfoList, System.Action<CServicesManager, STGoogleSheetLoadInfo, Dictionary<string, SimpleJSON.JSONNode>, bool> a_oCallback) {
		var stResult = KDefine.G_TABLE_INFO_GOOGLE_SHEET_DICT.ExFindVal((a_stKeyVal) => a_stKeyVal.Value.m_oID.Equals(a_oID));
		a_oInfoList.ExCopyTo(Func.m_oGoogleSheetLoadInfoList, (a_stInfo) => (a_stInfo.Item1, a_stInfo.Item2, a_stInfo.Item2));

		Func.m_oGoogleSheetJSONNodeDict.Clear();
		Func.m_oGoogleSheetCallbackDict03.ExReplaceVal(ECallback.LOAD_GOOGLE_SHEET, a_oCallback);

		// 정보가 존재 할 경우
		if(a_oInfoList.ExIsValid()) {
			CIndicatorManager.Inst.Show();
			CIndicatorManager.Inst.SetInfoText(stResult.Item1 ? stResult.Item2 : string.Empty);

			CServicesManager.Inst.LoadGoogleSheet(a_oID, a_oInfoList[KCDefine.B_VAL_0_INT].Item1, Func.OnLoadGoogleSheet, KCDefine.B_VAL_0_INT, a_oInfoList[KCDefine.B_VAL_0_INT].Item2);
		} else {
			CSceneManager.ActiveSceneManager.ExLateCallFunc((a_oSender) => a_oCallback?.Invoke(CServicesManager.Inst, STGoogleSheetLoadInfo.INVALID, null, false));
		}
	}

	/** 구글 시트를 로드한다 */
	public static void LoadGoogleSheets(Dictionary<string, System.Action<CServicesManager, STGoogleSheetLoadInfo, Dictionary<string, SimpleJSON.JSONNode>, bool>> a_oHandlerDict, System.Action<CServicesManager, bool> a_oCallback) {
		var oLoadGoogleSheetInfoDict = new Dictionary<string, STLoadGoogleSheetInfo>();

		Func.SetupLoadGoogleSheetInfos(oLoadGoogleSheetInfoDict);
		Func.LoadGoogleSheets(oLoadGoogleSheetInfoDict.Values.ToList(), a_oHandlerDict, a_oCallback);
	}

	/** 구글 시트를 로드한다 */
	public static void LoadGoogleSheets(List<STLoadGoogleSheetInfo> a_oLoadGoogleSheetInfoList, Dictionary<string, System.Action<CServicesManager, STGoogleSheetLoadInfo, Dictionary<string, SimpleJSON.JSONNode>, bool>> a_oHandlerDict, System.Action<CServicesManager, bool> a_oCallback) {
		a_oHandlerDict.ExCopyTo(Func.m_oGoogleSheetLoadHandlerDict, (a_oHandler) => a_oHandler);
		a_oLoadGoogleSheetInfoList.ExCopyTo(Func.m_oLoadGoogleSheetInfoList, (a_stLoadGoogleSheetInfo) => a_stLoadGoogleSheetInfo);

		Func.m_oGoogleSheetCallbackDict01.ExReplaceVal(ECallback.LOAD_GOOGLE_SHEETS, a_oCallback);

		// 로드 구글 시트 정보가 존재 할 경우
		if(a_oLoadGoogleSheetInfoList.ExIsValid()) {
			Func.LoadGoogleSheet(a_oLoadGoogleSheetInfoList[KCDefine.B_VAL_0_INT].m_oID, a_oLoadGoogleSheetInfoList[KCDefine.B_VAL_0_INT].m_oSheetInfoList, Func.OnLoadGoogleSheets);
		} else {
			CSceneManager.ActiveSceneManager.ExLateCallFunc((a_oSender) => a_oCallback?.Invoke(CServicesManager.Inst, true));
		}
	}

	/** 버전 정보 구글 시트를 로드한다 */
	public static void LoadVerInfoGoogleSheet(string a_oID, System.Action<CServicesManager, SimpleJSON.JSONNode, Dictionary<string, STLoadGoogleSheetInfo>, bool> a_oCallback) {
		CAccess.Assert(a_oID.ExIsValid());
		Func.m_oGoogleSheetCallbackDict02.ExReplaceVal(ECallback.LOAD_VER_INFO_GOOGLE_SHEET, a_oCallback);

		Func.LoadGoogleSheet(a_oID, new List<(string, int)>() {
			($"{EUserType.A}", KCDefine.U_MAX_NUM_GOOGLE_SHEET_ROWS), ($"{EUserType.B}", KCDefine.U_MAX_NUM_GOOGLE_SHEET_ROWS), (KCDefine.B_KEY_COMMON, KCDefine.U_MAX_NUM_GOOGLE_SHEET_ROWS)
		}, Func.OnLoadVerInfoGoogleSheet);
	}

	/** 구글 시트를 저장한다 */
	public static void SaveGoogleSheet(string a_oID, List<(string, List<List<string>>)> a_oInfoListContainer, System.Action<CServicesManager, STGoogleSheetSaveInfo, bool> a_oCallback) {
		var stResult = KDefine.G_TABLE_INFO_GOOGLE_SHEET_DICT.ExFindVal((a_stKeyVal) => a_stKeyVal.Value.m_oID.Equals(a_oID)); ;
		a_oInfoListContainer.ExCopyTo(Func.m_oGoogleSheetSaveInfoListContainer, (a_stInfo) => (a_stInfo.Item1, a_stInfo.Item2.Count, a_stInfo.Item2));

		Func.m_oGoogleSheetCallbackDict04.ExReplaceVal(ECallback.SAVE_GOOGLE_SHEET, a_oCallback);

		// 정보가 존재 할 경우
		if(a_oInfoListContainer.ExIsValid()) {
			CIndicatorManager.Inst.Show();
			CIndicatorManager.Inst.SetInfoText(stResult.Item1 ? stResult.Item2 : string.Empty);

			CServicesManager.Inst.SaveGoogleSheet(a_oID, a_oInfoListContainer[KCDefine.B_VAL_0_INT].Item1, a_oInfoListContainer[KCDefine.B_VAL_0_INT].Item2, Func.OnSaveGoogleSheet, KCDefine.B_VAL_0_INT, a_oInfoListContainer[KCDefine.B_VAL_0_INT].Item2.Count);
		} else {
			CSceneManager.ActiveSceneManager.ExLateCallFunc((a_oSender) => a_oCallback?.Invoke(CServicesManager.Inst, STGoogleSheetSaveInfo.INVALID, false));
		}
	}

	/** 구글 시트를 저장한다 */
	public static void SaveGoogleSheets(Dictionary<string, System.Action<CServicesManager, STGoogleSheetSaveInfo, bool>> a_oHandlerDict, System.Action<CServicesManager, bool> a_oCallback) {
		var oSaveGoogleSheetInfoDict = new Dictionary<string, STSaveGoogleSheetInfo>();

		Func.SetupSaveGoogleSheetInfos(oSaveGoogleSheetInfoDict);
		Func.SaveGoogleSheets(oSaveGoogleSheetInfoDict.Values.ToList(), a_oHandlerDict, a_oCallback);
	}

	/** 구글 시트를 저장한다 */
	public static void SaveGoogleSheets(List<STSaveGoogleSheetInfo> a_oSaveGoogleSheetInfoList, Dictionary<string, System.Action<CServicesManager, STGoogleSheetSaveInfo, bool>> a_oHandlerDict, System.Action<CServicesManager, bool> a_oCallback) {
		a_oHandlerDict.ExCopyTo(Func.m_oGoogleSheetSaveHandlerDict, (a_oHandler) => a_oHandler);
		a_oSaveGoogleSheetInfoList.ExCopyTo(Func.m_oSaveGoogleSheetInfoList, (a_stLoadGoogleSheetInfo) => a_stLoadGoogleSheetInfo);

		Func.m_oGoogleSheetCallbackDict01.ExReplaceVal(ECallback.SAVE_GOOGLE_SHEETS, a_oCallback);

		// 저장 구글 시트 정보가 존재 할 경우
		if(a_oSaveGoogleSheetInfoList.ExIsValid()) {
			Func.SaveGoogleSheet(a_oSaveGoogleSheetInfoList[KCDefine.B_VAL_0_INT].m_oID, a_oSaveGoogleSheetInfoList[KCDefine.B_VAL_0_INT].m_oSheetInfoListContainer, Func.OnSaveGoogleSheets);
		} else {
			CSceneManager.ActiveSceneManager.ExLateCallFunc((a_oSender) => a_oCallback?.Invoke(CServicesManager.Inst, true));
		}
	}

	/** JSON 노드를 설정한다 */
	private static void SetupJSONNode(KeyValuePair<int, List<GSTU_Cell>> a_stKeyVal, STGoogleSheetLoadInfo a_stGoogleSheetLoadInfo, SimpleJSON.JSONNode a_oOutJSONNode) {
		SimpleJSON.JSONNode oJSONNode = (a_stKeyVal.Key <= KCDefine.B_VAL_1_INT) ? new SimpleJSON.JSONArray() : new SimpleJSON.JSONClass();

		for(int i = 0; i < a_stGoogleSheetLoadInfo.m_oGoogleSheet.rows[a_stKeyVal.Key].Count; ++i) {
			// 키 데이터 일 경우
			if(a_stKeyVal.Key <= KCDefine.B_VAL_1_INT) {
				oJSONNode.Add(a_stGoogleSheetLoadInfo.m_oGoogleSheet.rows[a_stKeyVal.Key][i].value);
			} else {
				oJSONNode.Add(a_oOutJSONNode[KCDefine.B_VAL_0_INT][i], a_stGoogleSheetLoadInfo.m_oGoogleSheet.rows[a_stKeyVal.Key][i].value.Contains(KCDefine.B_TOKEN_COMMA) ? Func.MakeJSONArray(a_stGoogleSheetLoadInfo.m_oGoogleSheet.rows[a_stKeyVal.Key][i].value) : a_stGoogleSheetLoadInfo.m_oGoogleSheet.rows[a_stKeyVal.Key][i].value);
			}
		}

		a_oOutJSONNode.Add(oJSONNode);
	}

	/** 구글 시트를 로드했을 경우 */
	private static void OnLoadGoogleSheet(CServicesManager a_oSender, STGoogleSheetLoadInfo a_stGoogleSheetLoadInfo, bool a_bIsSuccess) {
		int nIdx = Func.m_oGoogleSheetLoadInfoList.FindIndex((a_oLoadGoogleSheetInfo) => a_oLoadGoogleSheetInfo.Item1.Equals(a_stGoogleSheetLoadInfo.m_oSheetName));
		CAccess.Assert(Func.m_oGoogleSheetLoadInfoList.ExIsValidIdx(nIdx));

		var oJSONNode = Func.m_oGoogleSheetJSONNodeDict.ContainsKey(a_stGoogleSheetLoadInfo.m_oSheetName) ? Func.m_oGoogleSheetJSONNodeDict[a_stGoogleSheetLoadInfo.m_oSheetName] : new SimpleJSON.JSONArray();
		Func.m_oGoogleSheetLoadInfoList[nIdx] = (Func.m_oGoogleSheetLoadInfoList[nIdx].Item1, Func.m_oGoogleSheetLoadInfoList[nIdx].Item2, Func.m_oGoogleSheetLoadInfoList[nIdx].Item3 - a_stGoogleSheetLoadInfo.m_nNumRows);

		// 데이터를 로드했을 경우
		if(a_bIsSuccess && !SpreadsheetManager.IsError && a_stGoogleSheetLoadInfo.m_oGoogleSheet.rows.primaryDictionary.Count > KCDefine.B_VAL_0_INT) {
			foreach(var stKeyVal in a_stGoogleSheetLoadInfo.m_oGoogleSheet.rows.primaryDictionary) {
				Func.SetupJSONNode(stKeyVal, a_stGoogleSheetLoadInfo, oJSONNode);
			}
		}

		int nOriginNumRows = Func.m_oGoogleSheetLoadInfoList[nIdx].Item2;
		Func.m_oGoogleSheetJSONNodeDict.ExReplaceVal(a_stGoogleSheetLoadInfo.m_oSheetName, oJSONNode);

		// 로드 할 데이터가 존재 할 경우
		if(a_bIsSuccess && !SpreadsheetManager.IsError && Func.m_oGoogleSheetLoadInfoList[nIdx].Item3 > KCDefine.B_VAL_0_INT) {
			CServicesManager.Inst.LoadGoogleSheet(a_stGoogleSheetLoadInfo.m_oID, Func.m_oGoogleSheetLoadInfoList[nIdx].Item1, Func.OnLoadGoogleSheet, Func.m_oGoogleSheetLoadInfoList[nIdx].Item2 - Func.m_oGoogleSheetLoadInfoList[nIdx].Item3, Func.m_oGoogleSheetLoadInfoList[nIdx].Item3);
		} else {
			Func.m_oGoogleSheetLoadInfoList.ExRemoveValAt(nIdx);
			Func.m_oGoogleSheetJSONNodeDict.GetValueOrDefault(a_stGoogleSheetLoadInfo.m_oSheetName).Remove(KCDefine.B_VAL_0_INT);

			// 구글 시트 로드 정보가 존재 할 경우
			if(!SpreadsheetManager.IsError && Func.m_oGoogleSheetLoadInfoList.ExIsValid()) {
				CServicesManager.Inst.LoadGoogleSheet(a_stGoogleSheetLoadInfo.m_oID, Func.m_oGoogleSheetLoadInfoList[KCDefine.B_VAL_0_INT].Item1, Func.OnLoadGoogleSheet, KCDefine.B_VAL_0_INT, Func.m_oGoogleSheetLoadInfoList[KCDefine.B_VAL_0_INT].Item2);
			} else {
				var stResult = KDefine.G_TABLE_INFO_GOOGLE_SHEET_DICT.ExFindVal((a_stKeyVal) => a_stKeyVal.Value.m_oID.Equals(a_stGoogleSheetLoadInfo.m_oID));
				var stGoogleSheetLoadInfo = new STGoogleSheetLoadInfo(a_stGoogleSheetLoadInfo.m_oID, stResult.Item1 ? stResult.Item2 : string.Empty, KCDefine.B_VAL_0_INT, nOriginNumRows, a_stGoogleSheetLoadInfo.m_oGoogleSheet);

				CIndicatorManager.Inst.Close();
				Func.m_oGoogleSheetCallbackDict03.GetValueOrDefault(ECallback.LOAD_GOOGLE_SHEET)?.Invoke(a_oSender, stGoogleSheetLoadInfo, Func.m_oGoogleSheetJSONNodeDict, !SpreadsheetManager.IsError);
			}
		}
	}

	/** 구글 시트를 로드했을 경우 */
	private static void OnLoadGoogleSheets(CServicesManager a_oSender, STGoogleSheetLoadInfo a_stGoogleSheetLoadInfo, Dictionary<string, SimpleJSON.JSONNode> a_oJSONNodeInfoDict, bool a_bIsSuccess) {
		var stResult = KDefine.G_TABLE_INFO_GOOGLE_SHEET_DICT.ExFindVal((a_stKeyVal) => a_stKeyVal.Value.m_oID.Equals(a_stGoogleSheetLoadInfo.m_oID));

		Func.m_oLoadGoogleSheetInfoList.ExRemoveValAt(KCDefine.B_VAL_0_INT);
		Func.m_oGoogleSheetLoadHandlerDict.GetValueOrDefault(stResult.Item2)?.Invoke(a_oSender, a_stGoogleSheetLoadInfo, a_oJSONNodeInfoDict, a_bIsSuccess);

		// 구글 시트 로드가 완료 되었을 경우 */
		if(!a_bIsSuccess || !Func.m_oLoadGoogleSheetInfoList.ExIsValid()) {
			Func.m_oGoogleSheetCallbackDict01.GetValueOrDefault(ECallback.LOAD_GOOGLE_SHEETS)?.Invoke(a_oSender, a_bIsSuccess && !Func.m_oLoadGoogleSheetInfoList.ExIsValid());
		} else {
			var oLoadGoogleSheetInfoList = new List<STLoadGoogleSheetInfo>(Func.m_oLoadGoogleSheetInfoList);
			var oGoogleSheetLoadHandlerDict = new Dictionary<string, System.Action<CServicesManager, STGoogleSheetLoadInfo, Dictionary<string, SimpleJSON.JSONNode>, bool>>(Func.m_oGoogleSheetLoadHandlerDict);

			Func.LoadGoogleSheets(oLoadGoogleSheetInfoList, oGoogleSheetLoadHandlerDict, Func.m_oGoogleSheetCallbackDict01.GetValueOrDefault(ECallback.LOAD_GOOGLE_SHEETS));
		}
	}

	/** 버전 정보 구글 시트를 로드했을 경우 */
	private static void OnLoadVerInfoGoogleSheet(CServicesManager a_oSender, STGoogleSheetLoadInfo a_stGoogleSheetLoadInfo, Dictionary<string, SimpleJSON.JSONNode> a_oJSONNodeInfoDict, bool a_bIsSuccess) {
		var oLoadGoogleSheetInfoDict = new Dictionary<string, STLoadGoogleSheetInfo>();
		SimpleJSON.JSONNode oVerInfos = null;

		// 로드 되었을 경우
		if(a_bIsSuccess) {
#if AB_TEST_ENABLE
			oVerInfos = a_oJSONNodeInfoDict.ExToJSONNode()[(CCommonUserInfoStorage.Inst.UserInfo.UserType == EUserType.B) ? $"{EUserType.B}" : $"{EUserType.A}"];
#else
			oVerInfos = a_oJSONNodeInfoDict.ExToJSONNode()[KCDefine.B_KEY_COMMON];
#endif // #if AB_TEST_ENABLE

			for(int i = 0; i < oVerInfos.Count; ++i) {
				var oVer = CAppInfoStorage.Inst.AppInfo.m_oTableSysVerDict.GetValueOrDefault(oVerInfos[i][KCDefine.U_KEY_NAME], KCDefine.U_VER_DEF);

				string oFlags01Key = string.Format(KCDefine.U_KEY_FMT_FLAGS, KCDefine.B_VAL_1_INT);
				string oFlags02Key = string.Format(KCDefine.U_KEY_FMT_FLAGS, KCDefine.B_VAL_2_INT);
				string oFlags03Key = string.Format(KCDefine.U_KEY_FMT_FLAGS, KCDefine.B_VAL_3_INT);

				// 구글 시트 로드가 가능 할 경우
				if(oVerInfos[i][oFlags01Key].AsInt != KCDefine.B_VAL_0_INT || oVer.CompareTo(System.Version.Parse(oVerInfos[i][KCDefine.U_KEY_VER])) < KCDefine.B_COMPARE_EQUALS) {
					Func.DoSetupLoadGoogleSheetInfos(KDefine.G_TABLE_INFO_GOOGLE_SHEET_DICT.GetValueOrDefault(oVerInfos[i][KCDefine.U_KEY_NAME]), oLoadGoogleSheetInfoDict, KDefine.G_TABLE_INFO_NUM_ROWS_DICT.GetValueOrDefault(oVerInfos[i][KCDefine.U_KEY_NAME]));
				}
			}
		}

		Func.m_oGoogleSheetCallbackDict02.GetValueOrDefault(ECallback.LOAD_VER_INFO_GOOGLE_SHEET)?.Invoke(a_oSender, oVerInfos, oLoadGoogleSheetInfoDict, a_bIsSuccess);
	}

	/** 구글 시트를 저장했을 경우 */
	private static void OnSaveGoogleSheet(CServicesManager a_oSender, STGoogleSheetSaveInfo a_stGoogleSheetSaveInfo, bool a_bIsSuccess) {
		int nIdx = Func.m_oGoogleSheetSaveInfoListContainer.FindIndex((a_oLoadGoogleSheetInfo) => a_oLoadGoogleSheetInfo.Item1.Equals(a_stGoogleSheetSaveInfo.m_oSheetName));
		CAccess.Assert(Func.m_oGoogleSheetSaveInfoListContainer.ExIsValidIdx(nIdx));

		int nOriginNumRows = Func.m_oGoogleSheetSaveInfoListContainer[nIdx].Item2;
		Func.m_oGoogleSheetSaveInfoListContainer[nIdx].Item3.RemoveRange(KCDefine.B_VAL_0_INT, a_stGoogleSheetSaveInfo.m_nNumRows);

		// 저장 할 데이터가 존재 할 경우
		if(a_bIsSuccess && !SpreadsheetManager.IsError && Func.m_oGoogleSheetSaveInfoListContainer[nIdx].Item3.Count > KCDefine.B_VAL_0_INT) {
			CServicesManager.Inst.SaveGoogleSheet(a_stGoogleSheetSaveInfo.m_oID, Func.m_oGoogleSheetSaveInfoListContainer[nIdx].Item1, Func.m_oGoogleSheetSaveInfoListContainer[nIdx].Item3, Func.OnSaveGoogleSheet, Func.m_oGoogleSheetSaveInfoListContainer[nIdx].Item2 - Func.m_oGoogleSheetSaveInfoListContainer[nIdx].Item3.Count, Func.m_oGoogleSheetSaveInfoListContainer[nIdx].Item3.Count);
		} else {
			Func.m_oGoogleSheetSaveInfoListContainer.ExRemoveValAt(nIdx);

			// 구글 시트 저장 정보가 존재 할 경우
			if(!SpreadsheetManager.IsError && Func.m_oGoogleSheetSaveInfoListContainer.ExIsValid()) {
				CServicesManager.Inst.SaveGoogleSheet(a_stGoogleSheetSaveInfo.m_oID, Func.m_oGoogleSheetSaveInfoListContainer[KCDefine.B_VAL_0_INT].Item1, Func.m_oGoogleSheetSaveInfoListContainer[KCDefine.B_VAL_0_INT].Item3, Func.OnSaveGoogleSheet, KCDefine.B_VAL_0_INT, Func.m_oGoogleSheetSaveInfoListContainer[KCDefine.B_VAL_0_INT].Item3.Count);
			} else {
				var stResult = KDefine.G_TABLE_INFO_GOOGLE_SHEET_DICT.ExFindVal((a_stKeyVal) => a_stKeyVal.Value.m_oID.Equals(a_stGoogleSheetSaveInfo.m_oID));
				var stGoogleSheetSaveInfo = new STGoogleSheetSaveInfo(a_stGoogleSheetSaveInfo.m_oID, stResult.Item1 ? stResult.Item2 : string.Empty, KCDefine.B_VAL_0_INT, nOriginNumRows);

				CIndicatorManager.Inst.Close();
				Func.m_oGoogleSheetCallbackDict04.GetValueOrDefault(ECallback.SAVE_GOOGLE_SHEET)?.Invoke(a_oSender, stGoogleSheetSaveInfo, !SpreadsheetManager.IsError);
			}
		}
	}

	/** 구글 시트를 저장했을 경우 */
	private static void OnSaveGoogleSheets(CServicesManager a_oSender, STGoogleSheetSaveInfo a_stGoogleSheetSaveInfo, bool a_bIsSuccess) {
		var stResult = KDefine.G_TABLE_INFO_GOOGLE_SHEET_DICT.ExFindVal((a_stKeyVal) => a_stKeyVal.Value.m_oID.Equals(a_stGoogleSheetSaveInfo.m_oID));

		Func.m_oSaveGoogleSheetInfoList.ExRemoveValAt(KCDefine.B_VAL_0_INT);
		Func.m_oGoogleSheetSaveHandlerDict.GetValueOrDefault(stResult.Item2)?.Invoke(a_oSender, a_stGoogleSheetSaveInfo, a_bIsSuccess);

		// 구글 시트 저장이 완료 되었을 경우 */
		if(!a_bIsSuccess || !Func.m_oSaveGoogleSheetInfoList.ExIsValid()) {
			Func.m_oGoogleSheetCallbackDict01.GetValueOrDefault(ECallback.SAVE_GOOGLE_SHEETS)?.Invoke(a_oSender, a_bIsSuccess && !Func.m_oSaveGoogleSheetInfoList.ExIsValid());
		} else {
			var oSaveGoogleSheetInfoList = new List<STSaveGoogleSheetInfo>(Func.m_oSaveGoogleSheetInfoList);
			var oGoogleSheetSaveHandlerDict = new Dictionary<string, System.Action<CServicesManager, STGoogleSheetSaveInfo, bool>>(Func.m_oGoogleSheetSaveHandlerDict);

			Func.SaveGoogleSheets(oSaveGoogleSheetInfoList, oGoogleSheetSaveHandlerDict, Func.m_oGoogleSheetCallbackDict01.GetValueOrDefault(ECallback.SAVE_GOOGLE_SHEETS));
		}
	}

	/** 로드 구글 시트 정보를 설정한다 */
	private static void DoSetupLoadGoogleSheetInfos(STTableInfo a_stTableInfo, Dictionary<string, STLoadGoogleSheetInfo> a_oOutLoadGoogleSheetInfoDict, int a_nMaxNumRows = KCDefine.U_MAX_NUM_GOOGLE_SHEET_ROWS, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_stTableInfo.m_oSheetNameDictContainer.ExIsValid());

		// 구글 시트 정보 설정이 가능 할 경우
		if(a_stTableInfo.m_oSheetNameDictContainer.ExIsValid()) {
			var stLoadGoogleSheetInfo = a_oOutLoadGoogleSheetInfoDict.ContainsKey(a_stTableInfo.m_oTableName) ? a_oOutLoadGoogleSheetInfoDict[a_stTableInfo.m_oTableName] : new STLoadGoogleSheetInfo(a_stTableInfo.m_oID, a_stTableInfo.m_oTableName);

			foreach(var stKeyVal in a_stTableInfo.m_oSheetNameDictContainer) {
				var oExtraSheetNameDictContainer = a_stTableInfo.m_oExtraSheetNameDictContainer.GetValueOrDefault(stKeyVal.Key);

				foreach(var stSheetNameKeyVal in stKeyVal.Value) {
					stLoadGoogleSheetInfo.m_oSheetInfoList.ExAddVal((stSheetNameKeyVal.Value, a_nMaxNumRows));

					// 추가 구글 시트 정보 설정이 가능 할 경우
					if(oExtraSheetNameDictContainer != null && oExtraSheetNameDictContainer.ContainsKey(stSheetNameKeyVal.Key)) {
						for(int i = 0; i < oExtraSheetNameDictContainer[stSheetNameKeyVal.Key].Count; ++i) {
							stLoadGoogleSheetInfo.m_oSheetInfoList.ExAddVal((oExtraSheetNameDictContainer[stSheetNameKeyVal.Key][i], a_nMaxNumRows));
						}
					}
				}
			}

			a_oOutLoadGoogleSheetInfoDict.ExReplaceVal(a_stTableInfo.m_oTableName, stLoadGoogleSheetInfo);
		}
	}

	/** 저장 구글 시트 정보를 설정한다 */
	private static void DoSetupSaveGoogleSheetInfos(STTableInfo a_stTableInfo, Dictionary<string, STSaveGoogleSheetInfo> a_oOutSaveGoogleSheetInfoDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_stTableInfo.m_oSheetNameDictContainer.ExIsValid());

		// 구글 시트 정보 설정이 가능 할 경우
		if(a_stTableInfo.m_oSheetNameDictContainer.ExIsValid()) {
			var oInfoValDictContainer = m_oGoogleSheetInfoValCreatorDict.GetValueOrDefault(a_stTableInfo.m_oTableName)?.Invoke();
			var stSaveGoogleSheetInfo = a_oOutSaveGoogleSheetInfoDict.ContainsKey(a_stTableInfo.m_oTableName) ? a_oOutSaveGoogleSheetInfoDict[a_stTableInfo.m_oTableName] : new STSaveGoogleSheetInfo(a_stTableInfo.m_oID, a_stTableInfo.m_oTableName);

			foreach(var stKeyVal in oInfoValDictContainer) {
				stSaveGoogleSheetInfo.m_oSheetInfoListContainer.ExAddVal((stKeyVal.Key, stKeyVal.Value));
			}

			a_oOutSaveGoogleSheetInfoDict.ExReplaceVal(a_stTableInfo.m_oTableName, stSaveGoogleSheetInfo);
		}
	}

	/** JSON 배열을 생성한다 */
	private static SimpleJSON.JSONNode MakeJSONArray(string a_oJSONStr) {
		var oTokens = Regex.Replace(a_oJSONStr, KCDefine.B_PATTERN_SPACE, string.Empty).Split(KCDefine.B_TOKEN_COMMA);
		var oJSONArray = new SimpleJSON.JSONArray();

		for(int i = 0; i < oTokens.Length; ++i) {
			oJSONArray.Add(oTokens[i]);
		}

		return oJSONArray;
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
#endregion // 조건부 클래스 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

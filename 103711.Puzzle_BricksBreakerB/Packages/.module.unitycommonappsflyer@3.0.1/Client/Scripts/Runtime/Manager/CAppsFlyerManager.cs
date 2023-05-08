using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if APPS_FLYER_MODULE_ENABLE
using AppsFlyerSDK;

/** 앱스 플라이어 관리자 */
public partial class CAppsFlyerManager : CSingleton<CAppsFlyerManager>, IAppsFlyerConversionData {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		IS_INIT,
		[HideInInspector] MAX_VAL
	}

	/** 콜백 */
	public enum ECallback {
		NONE = -1,
		INIT,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public string m_oAppID;
		public string m_oDevKey;
		public Dictionary<ECallback, System.Action<CAppsFlyerManager, bool>> m_oCallbackDict;
	}

#region 변수
	private Dictionary<EKey, bool> m_oBoolDict = new Dictionary<EKey, bool>() {
		[EKey.IS_INIT] = false
	};
#endregion // 변수

#region 프로퍼티
	public STParams Params { get; private set; }
	public bool IsInit => m_oBoolDict[EKey.IS_INIT];
#endregion // 프로퍼티

#region IAppsFlyerConversionData
	/** 데이터가 변환 되었을 경우 */
	public virtual void onConversionDataSuccess(string a_oConversion) {
		CFunc.ShowLog($"CAppsFlyerManager.onConversionDataSuccess: {a_oConversion}", KCDefine.B_LOG_COLOR_PLUGIN);
	}

	/** 데이터 변환에 실패했을 경우 */
	public virtual void onConversionDataFail(string a_oError) {
		CFunc.ShowLog($"CAppsFlyerManager.onConversionDataFail: {a_oError}", KCDefine.B_LOG_COLOR_PLUGIN);
	}

	/** 앱 실행 속성을 수신했을 경우 */
	public virtual void onAppOpenAttribution(string a_oAttribution) {
		CFunc.ShowLog($"CAppsFlyerManager.onAppOpenAttribution: {a_oAttribution}", KCDefine.B_LOG_COLOR_PLUGIN);
	}

	/** 앱 실행 속성 수신에 실패했을 경우 */
	public virtual void onAppOpenAttributionFailure(string a_oError) {
		CFunc.ShowLog($"CAppsFlyerManager.onAppOpenAttributionFailure: {a_oError}", KCDefine.B_LOG_COLOR_PLUGIN);
	}
#endregion // IAppsFlyerConversionData

#region 함수
	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		CFunc.ShowLog($"CAppsFlyerManager.Init: {a_stParams.m_oAppID}, {a_stParams.m_oDevKey}", KCDefine.B_LOG_COLOR_PLUGIN);

#if UNITY_IOS
		CAccess.Assert(a_stParams.m_oAppID.ExIsValid() && a_stParams.m_oDevKey.ExIsValid());
#elif UNITY_ANDROID
		CAccess.Assert(a_stParams.m_oDevKey.ExIsValid());
#endif // #if UNITY_IOS

#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
		// 초기화 되었을 경우
		if(m_oBoolDict[EKey.IS_INIT]) {
			a_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.INIT)?.Invoke(this, m_oBoolDict[EKey.IS_INIT]);
		} else {
			this.Params = a_stParams;
			
#if DEBUG || DEVELOPMENT_BUILD
			AppsFlyer.setIsDebug(true);
#else
			AppsFlyer.setIsDebug(false);
#endif // #if DEBUG || DEVELOPMENT_BUILD

#if UNITY_IOS
			AppsFlyer.waitForATTUserAuthorizationWithTimeoutInterval(KCDefine.U_TIMEOUT_APPS_FM_AGREE_TRACKING);
#endif // #if UNITY_IOS

			AppsFlyer.initSDK(a_stParams.m_oDevKey, a_stParams.m_oAppID, this);
			this.ExLateCallFunc((a_oSender) => this.OnInit());
		}
#else
		a_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.INIT)?.Invoke(this, false);
#endif // #if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
	}
#endregion // 함수

#region 클래스 함수
	/** 매개 변수를 생성한다 */
	public static STParams MakeParams(string a_oAppID, string a_oDevKey, Dictionary<ECallback, System.Action<CAppsFlyerManager, bool>> a_oCallbackDict = null) {
		return new STParams() {
			m_oAppID = a_oAppID, m_oDevKey = a_oDevKey, m_oCallbackDict = a_oCallbackDict ?? new Dictionary<ECallback, System.Action<CAppsFlyerManager, bool>>()
		};
	}
#endregion // 클래스 함수

#region 조건부 함수
#if UNITY_IOS || UNITY_ANDROID
	// 초기화 되었을 경우
	private void OnInit() {
		CFunc.ShowLog("CAppsFlyerManager.OnInit", KCDefine.B_LOG_COLOR_PLUGIN);
		
		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_APPS_FM_INIT_CALLBACK, () => {
#if APPS_FLYER_ANALYTICS_ENABLE && (ANALYTICS_TEST_ENABLE || STORE_DIST_BUILD)
			AppsFlyer.startSDK();
#else
			AppsFlyer.stopSDK(true);
#endif // #if APPS_FLYER_ANALYTICS_ENABLE && (ANALYTICS_TEST_ENABLE || STORE_DIST_BUILD)                                                                                           

			m_oBoolDict[EKey.IS_INIT] = true;
			this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.INIT)?.Invoke(this, m_oBoolDict[EKey.IS_INIT]);
		});
	}
#endif // #if UNITY_IOS || UNITY_ANDROID
#endregion // 조건부 함수
}
#endif // #if APPS_FLYER_MODULE_ENABLE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if FLURRY_MODULE_ENABLE
using FlurrySDK;

/** 플러리 관리자 */
public partial class CFlurryManager : CSingleton<CFlurryManager> {
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
		public string m_oAPIKey;
		public Dictionary<ECallback, System.Action<CFlurryManager, bool>> m_oCallbackDict;
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

#region 함수
	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		CFunc.ShowLog($"CFlurryManager.Init: {a_stParams.m_oAPIKey}", KCDefine.B_LOG_COLOR_PLUGIN);
		CAccess.Assert(a_stParams.m_oAPIKey.ExIsValid());

#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
		// 초기화 되었을 경우
		if(m_oBoolDict[EKey.IS_INIT]) {
			a_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.INIT)?.Invoke(this, m_oBoolDict[EKey.IS_INIT]);
		} else {
			this.Params = a_stParams;

			var oBuilder = new Flurry.Builder();
			oBuilder.WithMessaging(false);
			oBuilder.WithLogLevel(Flurry.LogLevel.VERBOSE);
			oBuilder.WithAppVersion(CProjInfoTable.Inst.ProjInfo.m_stBuildVerInfo.m_oVer);
			oBuilder.WithDataSaleOptOut(!CCommonAppInfoStorage.Inst.AppInfo.IsAgreeTracking);
			oBuilder.WithContinueSessionMillis(KCDefine.U_TIMEOUT_FLURRY_M_NETWORK_CONNECTION);

#if FLURRY_ANALYTICS_ENABLE && (ANALYTICS_TEST_ENABLE || STORE_DIST_BUILD)
			oBuilder.WithLogEnabled(true);
			oBuilder.WithCrashReporting(true);
			oBuilder.WithIncludeBackgroundSessionsInMetrics(true);
#else
			oBuilder.WithLogEnabled(false);
			oBuilder.WithCrashReporting(false);
			oBuilder.WithIncludeBackgroundSessionsInMetrics(false);
#endif // #if FLURRY_ANALYTICS_ENABLE && (ANALYTICS_TEST_ENABLE || STORE_DIST_BUILD)

			oBuilder.Build(a_stParams.m_oAPIKey);
			this.ExLateCallFunc((a_oSender) => this.OnInit());
		}
#else
		a_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.INIT)?.Invoke(this, false);
#endif // #if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
	}
#endregion // 함수

#region 클래스 함수
	/** 매개 변수를 생성한다 */
	public static STParams MakeParams(string a_oAPIKey, Dictionary<ECallback, System.Action<CFlurryManager, bool>> a_oCallbackDict = null) {
		return new STParams() {
			m_oAPIKey = a_oAPIKey, m_oCallbackDict = a_oCallbackDict ?? new Dictionary<ECallback, System.Action<CFlurryManager, bool>>()
		};
	}
#endregion // 클래스 함수

#region 조건부 함수
#if UNITY_IOS || UNITY_ANDROID
	// 초기화 되었을 경우
	private void OnInit() {
		CFunc.ShowLog("CFlurryManager.OnInit", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_FLURRY_M_INIT_CALLBACK, () => {
			m_oBoolDict[EKey.IS_INIT] = true;
			this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.INIT)?.Invoke(this, m_oBoolDict[EKey.IS_INIT]);
		});
	}
#endif // #if UNITY_IOS || UNITY_ANDROID
#endregion // 조건부 함수
}
#endif // #if FLURRY_MODULE_ENABLE

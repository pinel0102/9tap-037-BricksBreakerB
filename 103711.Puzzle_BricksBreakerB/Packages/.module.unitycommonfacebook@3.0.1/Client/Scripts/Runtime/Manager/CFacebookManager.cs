using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if FACEBOOK_MODULE_ENABLE
using Facebook.Unity;

/** 페이스 북 관리자 */
public partial class CFacebookManager : CSingleton<CFacebookManager> {
	/** 콜백 */
	public enum ECallback {
		NONE = -1,
		INIT,
		[HideInInspector] MAX_VAL
	}

	/** 페이스 북 콜백 */
	private enum EFacebookCallback {
		NONE = -1,
		LOGIN,
		CHANGE_VIEW_STATE,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public Dictionary<ECallback, System.Action<CFacebookManager, bool>> m_oCallbackDict;
	}

#region 프로퍼티
	public STParams Params { get; private set; }

	public bool IsInit {
		get {
#if UNITY_IOS || UNITY_ANDROID
			return FB.IsInitialized;
#else
			return false;
#endif // #if UNITY_IOS || UNITY_ANDROID
		}
	}

	public bool IsLogin {
		get {
#if UNITY_IOS || UNITY_ANDROID
			// 초기화 되었을 경우
			if(this.IsInit) {
				var oToken = Facebook.Unity.AccessToken.CurrentAccessToken;
				var stExpirationTime = (oToken != null) ? oToken.ExpirationTime : System.DateTime.Now;
				
				return stExpirationTime.ExGetDeltaTimePerDays(System.DateTime.Now).ExIsGreate(KCDefine.B_VAL_0_REAL);
			}

			return false;
#else
			return false;
#endif // #if UNITY_IOS || UNITY_ANDROID
		}
	}

	public string UserID {
		get {
#if UNITY_IOS || UNITY_ANDROID
			return this.IsLogin ? Facebook.Unity.AccessToken.CurrentAccessToken.UserId : string.Empty;
#else
			return string.Empty;
#endif // #if UNITY_IOS || UNITY_ANDROID
		}
	}

	public string AccessToken {
		get {
#if UNITY_IOS || UNITY_ANDROID
			return this.IsLogin ? Facebook.Unity.AccessToken.CurrentAccessToken.TokenString : string.Empty;
#else
			return string.Empty;
#endif // #if UNITY_IOS || UNITY_ANDROID
		}
	}

	private Dictionary<EFacebookCallback, System.Action<CFacebookManager, bool>> CallbackDict { get; } = new Dictionary<EFacebookCallback, System.Action<CFacebookManager, bool>>();
#endregion // 프로퍼티

#region 함수
	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		CFunc.ShowLog("CFacebookManager.Init", KCDefine.B_LOG_COLOR_PLUGIN);

#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
		// 초기화 되었을 경우
		if(this.IsInit) {
			a_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.INIT)?.Invoke(this, this.IsInit);
		} else {
			this.Params = a_stParams;
			FB.Init(this.OnInit, this.OnChangeViewState);
		}
#else
		a_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.INIT)?.Invoke(this, false);
#endif // #if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
	}
#endregion // 함수

#region 클래스 함수
	/** 매개 변수를 생성한다 */
	public static STParams MakeParams(Dictionary<ECallback, System.Action<CFacebookManager, bool>> a_oCallbackDict = null) {
		return new STParams() {
			m_oCallbackDict = a_oCallbackDict ?? new Dictionary<ECallback, System.Action<CFacebookManager, bool>>()
		};
	}
#endregion // 클래스 함수

#region 조건부 함수
#if UNITY_IOS || UNITY_ANDROID
	// 초기화 되었을 경우
	private void OnInit() {
		CFunc.ShowLog($"CFacebookManager.OnInit: {this.IsInit}", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_FACEBOOK_M_INIT_CALLBACK, () => {
			FB.Mobile.SetAutoLogAppEventsEnabled(false);

#if ANALYTICS_TEST_ENABLE || STORE_DIST_BUILD
			FB.Mobile.SetAdvertiserTrackingEnabled(true);
			FB.Mobile.SetAdvertiserIDCollectionEnabled(true);
#else
			FB.Mobile.SetAdvertiserTrackingEnabled(false);
			FB.Mobile.SetAdvertiserIDCollectionEnabled(false);
#endif // #if ANALYTICS_TEST_ENABLE || STORE_DIST_BUILD

			FB.ActivateApp();
			this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.INIT)?.Invoke(this, this.IsInit);
		});
	}
	
	/** 뷰 상태가 변경 되었을 경우 */
	private void OnChangeViewState(bool a_bIsShow) {
		CFunc.ShowLog($"CFacebookManager.OnChangeViewState: {a_bIsShow}", KCDefine.B_LOG_COLOR_PLUGIN);
		CScheduleManager.Inst.AddCallback(a_bIsShow ? KCDefine.U_KEY_FACEBOOK_M_VIEW_STATE_SHOW_CALLBACK : KCDefine.U_KEY_FACEBOOK_M_VIEW_STATE_CLOSE_CALLBACK, () => this.CallbackDict.GetValueOrDefault(EFacebookCallback.CHANGE_VIEW_STATE)?.Invoke(this, a_bIsShow));
	}
#endif // #if UNITY_IOS || UNITY_ANDROID
#endregion // 조건부 함수
}
#endif // #if FACEBOOK_MODULE_ENABLE

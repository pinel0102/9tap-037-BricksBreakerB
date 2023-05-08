using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if FIREBASE_MODULE_ENABLE
using System.Threading.Tasks;
using Firebase;

#if FIREBASE_AUTH_ENABLE
using Firebase.Auth;
#endif // #if FIREBASE_AUTH_ENABLE

#if FIREBASE_ANALYTICS_ENABLE
using Firebase.Analytics;
#endif // #if FIREBASE_ANALYTICS_ENABLE

#if FIREBASE_MSG_ENABLE
using Firebase.Messaging;
#endif // #if FIREBASE_MSG_ENABLE

/** 파이어 베이스 관리자 */
public partial class CFirebaseManager : CSingleton<CFirebaseManager> {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		IS_INIT,
		MSG_TOKEN,
		[HideInInspector] MAX_VAL
	}

	/** 콜백 */
	public enum ECallback {
		NONE = -1,
		INIT,
		[HideInInspector] MAX_VAL
	}

	/** 파이어 베이스 콜백 */
	private enum EFirebaseCallback {
		NONE = -1,

#if FIREBASE_AUTH_ENABLE
		LOGIN,
#endif // #if FIREBASE_AUTH_ENABLE

#if FIREBASE_DB_ENABLE
		LOAD_DATAS,
		SAVE_DATAS,
#endif // #if FIREBASE_DB_ENABLE

#if FIREBASE_MSG_ENABLE
		LOAD_MSG_TOKEN,
#endif // #if FIREBASE_MSG_ENABLE

#if FIREBASE_CONFIG_ENABLE
		SETUP_DEF_CONFIGS,
		LOAD_CONFIGS,
#endif // #if FIREBASE_CONFIG_ENABLE

#if FIREBASE_STORAGE_ENABLE
		LOAD_FILES,
#endif // #if FIREBASE_STORAGE_ENABLE

		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public Dictionary<ECallback, System.Action<CFirebaseManager, bool>> m_oCallbackDict;
	}

#region 변수
	private Dictionary<EKey, bool> m_oBoolDict = new Dictionary<EKey, bool>() {
		[EKey.IS_INIT] = false
	};

	private Dictionary<EKey, string> m_oStrDict = new Dictionary<EKey, string>() {
		[EKey.MSG_TOKEN] = string.Empty
	};

	private FirebaseApp m_oFirebaseApp = null;
	private List<string> m_oConfigKeyList = new List<string>();
	private Dictionary<EFirebaseCallback, System.Action<CFirebaseManager, bool>> m_oCallbackDict01 = new Dictionary<EFirebaseCallback, System.Action<CFirebaseManager, bool>>();
	private Dictionary<EFirebaseCallback, System.Action<CFirebaseManager, string, bool>> m_oCallbackDict02 = new Dictionary<EFirebaseCallback, System.Action<CFirebaseManager, string, bool>>();
	private Dictionary<EFirebaseCallback, System.Action<CFirebaseManager, Dictionary<string, string>, bool>> m_oCallbackDict03 = new Dictionary<EFirebaseCallback, System.Action<CFirebaseManager, Dictionary<string, string>, bool>>();
#endregion // 변수

#region 프로퍼티
	public STParams Params { get; private set; }

	public bool IsLogin {
		get {
#if(UNITY_IOS || UNITY_ANDROID) && FIREBASE_AUTH_ENABLE
			return m_oBoolDict[EKey.IS_INIT] && FirebaseAuth.DefaultInstance.CurrentUser != null;
#else
			return false;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && FIREBASE_AUTH_ENABLE
		}
	}

	public string UserID {
		get {
#if(UNITY_IOS || UNITY_ANDROID) && FIREBASE_AUTH_ENABLE
			return this.IsLogin ? FirebaseAuth.DefaultInstance.CurrentUser.UserId : string.Empty;
#else
			return string.Empty;
#endif // #if (UNITY_IOS || UNITY_ANDROID) && FIREBASE_AUTH_ENABLE
		}
	}

	public bool IsInit => m_oBoolDict[EKey.IS_INIT];
	public string MsgToken => m_oStrDict[EKey.MSG_TOKEN];
#endregion // 프로퍼티

#region 함수
	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		CFunc.ShowLog($"CFirebaseManager.Init", KCDefine.B_LOG_COLOR_PLUGIN);

#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
		// 초기화 되었을 경우
		if(m_oBoolDict[EKey.IS_INIT]) {
			a_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.INIT)?.Invoke(this, m_oBoolDict[EKey.IS_INIT]);
		} else {
			this.Params = a_stParams;
			CTaskManager.Inst.WaitAsyncTask(FirebaseApp.CheckAndFixDependenciesAsync(), this.OnInit);
		}
#else
		a_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.INIT)?.Invoke(this, false);
#endif // #if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
	}
#endregion // 함수

#region 클래스 함수
	/** 매개 변수를 생성한다 */
	public static STParams MakeParams(Dictionary<ECallback, System.Action<CFirebaseManager, bool>> a_oCallbackDict = null) {
		return new STParams() {
			m_oCallbackDict = a_oCallbackDict ?? new Dictionary<ECallback, System.Action<CFirebaseManager, bool>>()
		};
	}
#endregion // 클래스 함수

#region 조건부 함수
#if UNITY_IOS || UNITY_ANDROID
	// 초기화 되었을 경우
	private void OnInit(Task<DependencyStatus> a_oTask) {
		string oErrorMsg = (a_oTask.Exception != null) ? a_oTask.Exception.Message : string.Empty;
		m_oBoolDict[EKey.IS_INIT] = a_oTask.ExIsCompleteSuccess() && a_oTask.Result == DependencyStatus.Available;

		CFunc.ShowLog($"CFirebaseManager.OnInit: {m_oBoolDict[EKey.IS_INIT]}, {oErrorMsg}", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_FIREBASE_M_INIT_CALLBACK, () => {
			// 초기화 되었을 경우
			if(m_oBoolDict[EKey.IS_INIT]) {
				m_oFirebaseApp = FirebaseApp.DefaultInstance;

#if FIREBASE_ANALYTICS_ENABLE
				FirebaseAnalytics.SetSessionTimeoutDuration(KCDefine.U_TIMEOUT_FIREBASE_SESSION);

#if ANALYTICS_TEST_ENABLE || STORE_DIST_BUILD
				FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
#else
				FirebaseAnalytics.SetAnalyticsCollectionEnabled(false);
#endif // #if ANALYTICS_TEST_ENABLE || STORE_DIST_BUILD
#endif // #if FIREBASE_ANALYTICS_ENABLE

#if FIREBASE_MSG_ENABLE
				FirebaseMessaging.TokenReceived += this.OnReceiveMsgToken;
				FirebaseMessaging.MessageReceived += this.OnReceiveNotiMsg;

				FirebaseMessaging.TokenRegistrationOnInitEnabled = true;
#endif // #if FIREBASE_MSG_ENABLE
			}

			this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.INIT)?.Invoke(this, m_oBoolDict[EKey.IS_INIT]);
		});
	}
#endif // #if UNITY_IOS || UNITY_ANDROID
#endregion // 조건부 함수
}
#endif // #if FIREBASE_MODULE_ENABLE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_IOS && APPLE_LOGIN_ENABLE
using AppleAuth;
using AppleAuth.Native;
#endif // #if UNITY_IOS && APPLE_LOGIN_ENABLE

/** 서비스 관리자 */
public partial class CServicesManager : CSingleton<CServicesManager> {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		IS_INIT,
		IS_APPLE_LOGIN,
		[HideInInspector] MAX_VAL
	}

	/** 콜백 */
	public enum ECallback {
		NONE = -1,
		INIT,
		[HideInInspector] MAX_VAL
	}

	/** 서비스 콜백 */
	private enum EServicesCallback {
		NONE = -1,
		LOAD_GOOGLE_SHEET,
		SAVE_GOOGLE_SHEET,

#if UNITY_IOS && APPLE_LOGIN_ENABLE
		LOGIN_WITH_APPLE,
		UPDATE_APPLE_LOGIN_STATE,
#endif // #if UNITY_IOS && APPLE_LOGIN_ENABLE

		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public Dictionary<ECallback, System.Action<CServicesManager, bool>> m_oCallbackDict;
	}

	#region 변수
	private Dictionary<EKey, bool> m_oBoolDict = new Dictionary<EKey, bool>();
	private Dictionary<EServicesCallback, System.Action<CServicesManager, bool>> m_oCallbackDict01 = new Dictionary<EServicesCallback, System.Action<CServicesManager, bool>>();

#if UNITY_IOS && APPLE_LOGIN_ENABLE
	private IAppleAuthManager m_oAppleAuthManager = null;
#endif // #if UNITY_IOS && APPLE_LOGIN_ENABLE

#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	private Dictionary<EServicesCallback, System.Action<CServicesManager, STGoogleSheetLoadInfo, bool>> m_oCallbackDict02 = new Dictionary<EServicesCallback, System.Action<CServicesManager, STGoogleSheetLoadInfo, bool>>();
	private Dictionary<EServicesCallback, System.Action<CServicesManager, STGoogleSheetSaveInfo, bool>> m_oCallbackDict03 = new Dictionary<EServicesCallback, System.Action<CServicesManager, STGoogleSheetSaveInfo, bool>>();
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	#endregion // 변수

	#region 프로퍼티
	public STParams Params { get; private set; }

	public string AppleUserID {
		get {
#if UNITY_IOS && APPLE_LOGIN_ENABLE
			return m_oBoolDict.GetValueOrDefault(EKey.IS_INIT) ? CCommonAppInfoStorage.Inst.AppInfo.AppleUserID : string.Empty;
#else
			return string.Empty;
#endif // #if UNITY_IOS && APPLE_LOGIN_ENABLE
		}
	}

	public string AppleIDToken {
		get {
#if UNITY_IOS && APPLE_LOGIN_ENABLE
			return m_oBoolDict.GetValueOrDefault(EKey.IS_INIT) ? CCommonAppInfoStorage.Inst.AppInfo.AppleIDToken : string.Empty;
#else
			return string.Empty;
#endif // #if UNITY_IOS && APPLE_LOGIN_ENABLE
		}
	}

	public bool IsInit => m_oBoolDict.GetValueOrDefault(EKey.IS_INIT);
	public bool IsAppleLogin => m_oBoolDict.GetValueOrDefault(EKey.IS_APPLE_LOGIN);
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		CFunc.ShowLog("CServicesManager.Init", KCDefine.B_LOG_COLOR_PLUGIN);

#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
		// 초기화 되었을 경우
		if(m_oBoolDict.GetValueOrDefault(EKey.IS_INIT)) {
			a_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.INIT)?.Invoke(this, m_oBoolDict.GetValueOrDefault(EKey.IS_INIT));
		} else {
			this.Params = a_stParams;

#if UNITY_IOS && APPLE_LOGIN_ENABLE
			// 애플 로그인을 지원 할 경우
			if(AppleAuthManager.IsCurrentPlatformSupported) {
				var oPayloadDeserializer = new PayloadDeserializer();
				m_oAppleAuthManager = new AppleAuthManager(oPayloadDeserializer);
			}
#endif // #if UNITY_IOS && APPLE_LOGIN_ENABLE

			this.ExLateCallFunc((a_oSender) => this.OnInit());
		}
#else
		a_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.INIT)?.Invoke(this, false);
#endif // #if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
	}
	#endregion // 함수

	#region 클래스 함수
	/** 매개 변수를 생성한다 */
	public static STParams MakeParams(Dictionary<ECallback, System.Action<CServicesManager, bool>> a_oCallbackDict = null) {
		return new STParams() {
			m_oCallbackDict = a_oCallbackDict ?? new Dictionary<ECallback, System.Action<CServicesManager, bool>>()
		};
	}
	#endregion // 클래스 함수

	#region 조건부 함수
#if UNITY_IOS || UNITY_ANDROID
	// 초기화 되었을 경우
	private void OnInit() {
		CFunc.ShowLog("CServicesManager.OnInit", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_SERVICES_M_INIT_CALLBACK, () => {
			m_oBoolDict.ExReplaceVal(EKey.IS_INIT, true);
			this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.INIT)?.Invoke(this, m_oBoolDict.GetValueOrDefault(EKey.IS_INIT));
		});
	}

#if UNITY_IOS && APPLE_LOGIN_ENABLE
	//! 상태를 갱신한다
	public override void OnUpdate(float a_fDeltaTime) {
		base.OnUpdate(a_fDeltaTime);
		m_oAppleAuthManager?.Update();
	}
#endif // #if UNITY_IOS && APPLE_LOGIN_ENABLE
#endif // #if UNITY_IOS || UNITY_ANDROID
	#endregion // 조건부 함수
}

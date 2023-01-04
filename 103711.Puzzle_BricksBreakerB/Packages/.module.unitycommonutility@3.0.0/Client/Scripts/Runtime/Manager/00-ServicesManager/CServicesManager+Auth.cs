using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_IOS && APPLE_LOGIN_ENABLE
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Interfaces;
#endif // #if UNITY_IOS && APPLE_LOGIN_ENABLE

/** 서비스 관리자 - 인증 */
public partial class CServicesManager : CSingleton<CServicesManager> {
	#region 함수
	/** 애플 로그인 상태를 갱신한다 */
	public void UpdateAppleLoginState(System.Action<CServicesManager, bool> a_oCallback) {
		CFunc.ShowLog("CServicesManager.UpdateAppleLoginState", KCDefine.B_LOG_COLOR_PLUGIN);

#if UNITY_IOS && APPLE_LOGIN_ENABLE
		// 애플 로그인을 지원 할 경우
		if(m_oBoolDict.GetValueOrDefault(EKey.IS_INIT) && AppleAuthManager.IsCurrentPlatformSupported) {
			m_oCallbackDict01.ExReplaceVal(EServicesCallback.UPDATE_APPLE_LOGIN_STATE, a_oCallback);
			m_oAppleAuthManager.GetCredentialState(this.AppleUserID, this.OnUpdateAppleLoginState, this.OnUpdateFailAppleLoginState);
		} else {
			CFunc.Invoke(ref a_oCallback, this, false);
		}
#else
		CFunc.Invoke(ref a_oCallback, this, false);
#endif // #if UNITY_IOS && APPLE_LOGIN_ENABLE
	}

	/** 애플 로그인을 처리한다 */
	public void LoginWithApple(System.Action<CServicesManager, bool> a_oCallback) {
		CFunc.ShowLog("CServicesManager.LoginWithApple", KCDefine.B_LOG_COLOR_PLUGIN);

#if UNITY_IOS && APPLE_LOGIN_ENABLE
		// 로그인 되었을 경우
		if(!m_oBoolDict.GetValueOrDefault(EKey.IS_INIT) || m_oBoolDict.GetValueOrDefault(EKey.IS_APPLE_LOGIN)) {
			CFunc.Invoke(ref a_oCallback, this, m_oBoolDict.GetValueOrDefault(EKey.IS_APPLE_LOGIN));
		}
		// 애플 로그인을 지원 할 경우
		else if(m_oBoolDict.GetValueOrDefault(EKey.IS_INIT) && AppleAuthManager.IsCurrentPlatformSupported) {
			m_oCallbackDict01.ExReplaceVal(EServicesCallback.LOGIN_WITH_APPLE, a_oCallback);
			m_oAppleAuthManager.LoginWithAppleId(new AppleAuthLoginArgs(), this.OnLoginWithApple, this.OnLoginFailWithApple);
		} else {
			CFunc.Invoke(ref a_oCallback, this, false);
		}
#else
		CFunc.Invoke(ref a_oCallback, this, false);
#endif // #if UNITY_IOS && APPLE_LOGIN_ENABLE
	}

	/** 애플 로그아웃을 처리한다 */
	public void LogoutWithApple(System.Action<CServicesManager> a_oCallback) {
		CFunc.ShowLog("CServicesManager.LogoutWithApple", KCDefine.B_LOG_COLOR_PLUGIN);

		try {
#if UNITY_IOS && APPLE_LOGIN_ENABLE
			// 로그인 되었을 경우
			if(m_oBoolDict.GetValueOrDefault(EKey.IS_INIT) && m_oBoolDict.GetValueOrDefault(EKey.IS_APPLE_LOGIN)) {
				m_oBoolDict.ExReplaceVal(EKey.IS_APPLE_LOGIN, false);
			}
#endif // #if UNITY_IOS && APPLE_LOGIN_ENABLE
		} finally {
			CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_SERVICES_M_LOGOUT_WITH_APPLE_CALLBACK, () => CFunc.Invoke(ref a_oCallback, this));
		}
	}
	#endregion // 함수

	#region 조건부 함수
#if UNITY_IOS && APPLE_LOGIN_ENABLE
	/** 애플에 로그인 되었을 경우 */
	private void OnLoginWithApple(ICredential a_oCredential) {
		var oAppleIDCredential = a_oCredential as IAppleIDCredential;
		m_oBoolDict.ExReplaceVal(EKey.IS_APPLE_LOGIN, oAppleIDCredential != null);

		CFunc.ShowLog($"CServicesManager.OnLoginWithApple: {m_oBoolDict.GetValueOrDefault(EKey.IS_APPLE_LOGIN)}", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_SERVICES_M_LOGIN_WITH_APPLE_CALLBACK, () => {
			CCommonAppInfoStorage.Inst.AppInfo.AppleUserID = m_oBoolDict.GetValueOrDefault(EKey.IS_APPLE_LOGIN) ? oAppleIDCredential.User : string.Empty;
			CCommonAppInfoStorage.Inst.AppInfo.AppleIDToken = m_oBoolDict.GetValueOrDefault(EKey.IS_APPLE_LOGIN) ? System.Text.Encoding.Default.GetString(oAppleIDCredential.IdentityToken, KCDefine.B_VAL_0_INT, oAppleIDCredential.IdentityToken.Length) : string.Empty;

			CCommonAppInfoStorage.Inst.SaveAppInfo();
			m_oCallbackDict01.GetValueOrDefault(EServicesCallback.LOGIN_WITH_APPLE)?.Invoke(this, m_oBoolDict.GetValueOrDefault(EKey.IS_APPLE_LOGIN));
		});
	}

	/** 애플 로그인에 실패했을 경우 */
	private void OnLoginFailWithApple(IAppleError a_oError) {
		m_oBoolDict.ExReplaceVal(EKey.IS_APPLE_LOGIN, false);
		CFunc.ShowLog($"CServicesManager.OnLoginFailWithApple: {a_oError.LocalizedFailureReason}", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_SERVICES_M_LOGIN_FAIL_WITH_APPLE_CALLBACK, () => {
			CCommonAppInfoStorage.Inst.AppInfo.AppleUserID = string.Empty;
			CCommonAppInfoStorage.Inst.AppInfo.AppleIDToken = string.Empty;

			CCommonAppInfoStorage.Inst.SaveAppInfo();
			m_oCallbackDict01.GetValueOrDefault(EServicesCallback.LOGIN_WITH_APPLE)?.Invoke(this, false);
		});
	}

	/** 애플 로그인 상태가 갱신 되었을 경우 */
	private void OnUpdateAppleLoginState(CredentialState a_eState) {
		m_oBoolDict.ExReplaceVal(EKey.IS_APPLE_LOGIN, a_eState == CredentialState.Authorized);
		CFunc.ShowLog($"CServicesManager.OnUpdateAppleLoginState: {m_oBoolDict.GetValueOrDefault(EKey.IS_APPLE_LOGIN)}", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_SERVICES_M_UPDATE_APPLE_LOGIN_STATE_CALLBACK, () => {
			CCommonAppInfoStorage.Inst.AppInfo.AppleUserID = m_oBoolDict.GetValueOrDefault(EKey.IS_APPLE_LOGIN) ? this.AppleUserID : string.Empty;
			CCommonAppInfoStorage.Inst.AppInfo.AppleIDToken = m_oBoolDict.GetValueOrDefault(EKey.IS_APPLE_LOGIN) ? this.AppleIDToken : string.Empty;

			CCommonAppInfoStorage.Inst.SaveAppInfo();
			m_oCallbackDict01.GetValueOrDefault(EServicesCallback.UPDATE_APPLE_LOGIN_STATE)?.Invoke(this, m_oBoolDict.GetValueOrDefault(EKey.IS_APPLE_LOGIN));
		});
	}

	/** 애플 로그인 상태 갱신에 실패했을 경우 */
	private void OnUpdateFailAppleLoginState(IAppleError a_oError) {
		m_oBoolDict.ExReplaceVal(EKey.IS_APPLE_LOGIN, false);
		CFunc.ShowLog($"CServicesManager.OnUpdateFailAppleLoginState: {a_oError.LocalizedFailureReason}", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_SERVICES_M_UPDATE_FAIL_APPLE_LOGIN_STATE_CALLBACK, () => {
			CCommonAppInfoStorage.Inst.AppInfo.AppleUserID = string.Empty;
			CCommonAppInfoStorage.Inst.AppInfo.AppleIDToken = string.Empty;
			
			CCommonAppInfoStorage.Inst.SaveAppInfo();
			m_oCallbackDict01.GetValueOrDefault(EServicesCallback.UPDATE_APPLE_LOGIN_STATE)?.Invoke(this, false);
		});
	}
#endif // #if UNITY_IOS && APPLE_LOGIN_ENABLE
	#endregion // 조건부 함수
}

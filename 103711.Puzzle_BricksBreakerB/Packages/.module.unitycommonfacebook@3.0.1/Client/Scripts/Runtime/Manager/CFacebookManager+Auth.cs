using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if FACEBOOK_MODULE_ENABLE
using Facebook.Unity;

/** 페이스 북 관리자 - 인증 */
public partial class CFacebookManager : CSingleton<CFacebookManager> {
#region 함수
	/** 로그인을 처리한다 */
	public void Login(List<string> a_oPermissionList, System.Action<CFacebookManager, bool> a_oCallback, System.Action<CFacebookManager, bool> a_oChangeViewStateCallback = null) {
		CFunc.ShowLog($"CFacebookManager.Login: {a_oPermissionList}", KCDefine.B_LOG_COLOR_PLUGIN);
		CAccess.Assert(a_oPermissionList.ExIsValid());

#if UNITY_IOS || UNITY_ANDROID
		// 초기화 되었을 경우
		if(!this.IsInit || this.IsLogin) {
			CFunc.Invoke(ref a_oCallback, this, this.IsLogin);
		} else {
			this.CallbackDict.ExReplaceVal(EFacebookCallback.LOGIN, a_oCallback);
			this.CallbackDict.ExReplaceVal(EFacebookCallback.CHANGE_VIEW_STATE, a_oChangeViewStateCallback);

			FB.LogInWithReadPermissions(a_oPermissionList, this.OnLogin);
		}
#else
		CFunc.Invoke(ref a_oCallback, this, false);
#endif // #if UNITY_IOS || UNITY_ANDROID
	}

	/** 로그아웃을 처리한다 */
	public void Logout(System.Action<CFacebookManager> a_oCallback) {
		CFunc.ShowLog("CFacebookManager.Logout", KCDefine.B_LOG_COLOR_PLUGIN);

		try {
#if UNITY_IOS || UNITY_ANDROID
			// 로그인 되었을 경우
			if(this.IsInit && this.IsLogin) {
				FB.LogOut();
			}
#endif // #if UNITY_IOS || UNITY_ANDROID
		} finally {
			CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_FACEBOOK_M_LOGOUT_CALLBACK, () => CFunc.Invoke(ref a_oCallback, this));
		}
	}
#endregion // 함수

#region 조건부 함수
#if UNITY_IOS || UNITY_ANDROID
	/** 로그인 되었을 경우 */
	private void OnLogin(ILoginResult a_oResult) {
		CFunc.ShowLog($"CFacebookManager.OnLogin: {this.IsLogin}, {a_oResult}", KCDefine.B_LOG_COLOR_PLUGIN);
		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_FACEBOOK_M_LOGIN_CALLBACK, () => this.CallbackDict.GetValueOrDefault(EFacebookCallback.LOGIN)?.Invoke(this, this.IsLogin));
	}
#endif // #if UNITY_IOS || UNITY_ANDROID
#endregion // 조건부 함수
}
#endif // #if FACEBOOK_MODULE_ENABLE

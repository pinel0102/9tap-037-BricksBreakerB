using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_IOS
using UnityEngine.iOS;
#elif UNITY_ANDROID && GOOGLE_PLAY_UPDATE_ENABLE
using Google.Play.AppUpdate;
#endif // #if UNITY_IOS

/** 유니티 메세지 전송자 */
public partial class CUnityMsgSender : CSingleton<CUnityMsgSender> {
	/** 콜백 */
	public enum ECallback {
		NONE = -1,
		SHARE,
		[HideInInspector] MAX_VAL
	}

	#region 변수
	private Dictionary<ECallback, System.Action<NativeShare.ShareResult, string>> m_oCallbackDict = new Dictionary<ECallback, System.Action<NativeShare.ShareResult, string>>();

#if !UNITY_EDITOR && UNITY_ANDROID
	private AndroidJavaClass m_oAndroidPlugin = new AndroidJavaClass(KCDefine.U_CLS_N_UNITY_MS_UNITY_MSG_RECEIVER);
#endif // #if !UNITY_EDITOR && UNITY_ANDROID
	#endregion // 변수

	#region 함수
	/** 디바이스 식별자 반환 메세지를 전송한다 */
	public void SendGetDeviceIDMsg(System.Action<string, string> a_oCallback) {
		CFunc.ShowLog("CUnityMsgSender.SendGetDeviceIDMsg", KCDefine.B_LOG_COLOR_PLUGIN);

		// 디바이스 식별자를 지원하지 않을 경우
		if(!System.Guid.TryParse(SystemInfo.deviceUniqueIdentifier, out System.Guid stGUID) || SystemInfo.deviceUniqueIdentifier.Equals(SystemInfo.unsupportedIdentifier)) {
			this.SendUnityMsg(KCDefine.B_CMD_GET_DEVICE_ID, string.Empty, a_oCallback, System.Guid.NewGuid().ToString());
		} else {
			this.ExLateCallFunc((a_oSender) => CFunc.Invoke(ref a_oCallback, KCDefine.B_CMD_GET_DEVICE_ID, stGUID.ToString()));
		}
	}

	/** 국가 코드 반환 메세지를 전송한다 */
	public void SendGetCountryCodeMsg(System.Action<string, string> a_oCallback) {
		this.SendUnityMsg(KCDefine.B_CMD_GET_COUNTRY_CODE, string.Empty, a_oCallback);
	}

	/** 스토어 버전 반환 메세지를 전송한다 */
	public void SendGetStoreVerMsg(string a_oAppID, string a_oVer, float a_fTimeout, System.Action<string, string> a_oCallback) {
#if(UNITY_IOS || (UNITY_ANDROID && GOOGLE_PLAY_UPDATE_ENABLE)) && (STORE_VER_CHECK_ENABLE && NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE)
#if UNITY_IOS
		var oDataDict = new Dictionary<string, string>() {
			[KCDefine.U_KEY_UNITY_MS_APP_ID] = a_oAppID, [KCDefine.U_KEY_UNITY_MS_VER] = a_oVer, [KCDefine.U_KEY_UNITY_MS_TIMEOUT] = $"{a_fTimeout}"
		};

		this.SendUnityMsg(KCDefine.B_CMD_GET_STORE_VER, oDataDict.ExToJSONStr(), a_oCallback);
#else
		var oUpdateManager = new AppUpdateManager();
		StartCoroutine(CTaskManager.Inst.WaitUpdateOperation(oUpdateManager, this.OnReceiveGetStoreVerMsg));
#endif // #if UNITY_IOS
#else
		this.ExLateCallFunc((a_oSender) => {
			var oMsg = new SimpleJSON.JSONClass();

			try {
				oMsg.Add(KCDefine.U_KEY_DEVICE_MR_VER, a_oVer);
				oMsg.Add(KCDefine.U_KEY_DEVICE_MR_RESULT, KCDefine.B_TEXT_FALSE);
			} finally {
				CFunc.Invoke(ref a_oCallback, KCDefine.B_CMD_GET_STORE_VER, oMsg.ToString());
			}
		});
#endif // #if (UNITY_IOS || (UNITY_ANDROID && GOOGLE_PLAY_UPDATE_ENABLE)) && (STORE_VER_CHECK_ENABLE && NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE)
	}

	/** 광고 추적 여부 변경 메세지를 전송한다 */
	public void SendSetEnableAdsTrackingMsg(bool a_bIsEnable) {
		this.SendUnityMsg(KCDefine.B_CMD_SET_ENABLE_ADS_TRACKING, a_bIsEnable ? KCDefine.B_TEXT_TRUE : KCDefine.B_TEXT_FALSE, null);
	}

	/** 경고 창 출력 메세지를 전송한다 */
	public void SendShowAlertMsg(string a_oTitle, string a_oMsg, string a_oOKBtnText, string a_oCancelBtnText, System.Action<string, string> a_oCallback) {
		var oDataDict = new Dictionary<string, string>() {
			[KCDefine.U_KEY_UNITY_MS_ALERT_TITLE] = a_oTitle,
			[KCDefine.U_KEY_UNITY_MS_ALERT_MSG] = a_oMsg,
			[KCDefine.U_KEY_UNITY_MS_ALERT_OK_BTN_TEXT] = a_oOKBtnText,
			[KCDefine.U_KEY_UNITY_MS_ALERT_CANCEL_BTN_TEXT] = a_oCancelBtnText
		};

#if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
		this.SendUnityMsg(KCDefine.B_CMD_SHOW_ALERT, oDataDict.ExToJSONStr(), a_oCallback);
#endif // #if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
	}

	/** 토스트 출력 메세지를 출력한다 */
	public void SendShowToastMsg(string a_oMsg) {
		this.SendUnityMsg(KCDefine.B_CMD_SHOW_TOAST, a_oMsg, null);
	}

	/** 평가 창 출력 메세지를 전송한다 */
	public void SendShowReviewMsg() {
		CFunc.ShowLog("CUnityMsgSender.SendShowReviewMsg", KCDefine.B_LOG_COLOR_PLUGIN);

#if UNITY_IOS
		// 스토어 평가를 지원하지 않을 경우
		if(!Device.RequestStoreReview()) {
			Application.OpenURL(CCommonAppInfoStorage.Inst.StoreURL);
		}
#else
		Application.OpenURL(CCommonAppInfoStorage.Inst.StoreURL);
#endif // #if UNITY_IOS
	}

	/** 진동 메세지를 전송한다 */
	public void SendVibrateMsg(EVibrateType a_eType, EVibrateStyle a_eStyle, float a_fDuration, float a_fIntensity) {
		bool bIsEnableType = a_eType > EVibrateType.NONE && a_eType < EVibrateType.MAX_VAL;
		bool bIsEnableStyle = a_eStyle > EVibrateStyle.NONE && a_eStyle < EVibrateStyle.MAX_VAL;

		CAccess.Assert(bIsEnableType && bIsEnableStyle);
		CAccess.Assert(a_fDuration.ExIsGreateEquals(KCDefine.B_VAL_0_REAL));

		var oDataDict = new Dictionary<string, string>() {
			[KCDefine.U_KEY_UNITY_MS_VIBRATE_TYPE] = $"{(int)a_eType}",
			[KCDefine.U_KEY_UNITY_MS_VIBRATE_STYLE] = $"{(int)a_eStyle}",
			[KCDefine.U_KEY_UNITY_MS_VIBRATE_DURATION] = $"{a_fDuration}",
			[KCDefine.U_KEY_UNITY_MS_VIBRATE_INTENSITY] = $"{a_fIntensity}"
		};

#if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
		this.SendUnityMsg(KCDefine.B_CMD_VIBRATE, oDataDict.ExToJSONStr(), null);
#endif // #if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
	}

	/** 인디케이터 메세지를 전송한다 */
	public void SendIndicatorMsg(bool a_bIsShow) {
		this.SendUnityMsg(KCDefine.B_CMD_INDICATOR, a_bIsShow ? KCDefine.B_TEXT_TRUE : KCDefine.B_TEXT_FALSE, null);
	}

	/** 메일 메세지를 전송한다 */
	public void SendMailMsg(string a_oTitle, string a_oMsg, string a_oRecipient) {
#if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
		string oVer = CAccess.GetVerStr(CProjInfoTable.Inst.ProjInfo.m_stBuildVerInfo.m_oVer, CCommonUserInfoStorage.Inst.UserInfo.UserType);
		string oMailMsg = string.Format(KCDefine.B_MAIL_MSG_FMT, CProjInfoTable.Inst.CommonProjInfo.m_oProductName, oVer, CCommonAppInfoStorage.Inst.Platform, SystemInfo.processorType, SystemInfo.graphicsDeviceName, SystemInfo.graphicsDeviceType, SystemInfo.operatingSystem, CCommonAppInfoStorage.Inst.AppInfo.DeviceID, a_oMsg);

		var oDataDict = new Dictionary<string, string>() {
			[KCDefine.U_KEY_UNITY_MS_MAIL_TITLE] = a_oTitle,
			[KCDefine.U_KEY_UNITY_MS_MAIL_MSG] = oMailMsg,
			[KCDefine.U_KEY_UNITY_MS_MAIL_RECIPIENT] = a_oRecipient
		};

		this.SendUnityMsg(KCDefine.B_CMD_MAIL, oDataDict.ExToJSONStr(), null);
#endif // #if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
	}

	/** 공유 메세지를 전송한다 */
	public void SendShareMsg(string a_oMsg, System.Action<NativeShare.ShareResult, string> a_oCallback, string a_oFilePath = KCDefine.B_TEXT_EMPTY) {
		CFunc.ShowLog($"CUnityMsgSender.SendShareMsg: {a_oMsg}", KCDefine.B_LOG_COLOR_PLUGIN);

#if UNITY_IOS || UNITY_ANDROID
		m_oCallbackDict.ExReplaceVal(ECallback.SHARE, a_oCallback);

		var oShare = new NativeShare();
		oShare.SetText(a_oMsg);
		oShare.SetCallback(this.OnSendShareMsg);

		// 파일 경로가 유효 할 경우
		if(a_oFilePath.ExIsValid()) {
			oShare.AddFile(a_oFilePath);
		}

		oShare.Share();
#else
		this.ExLateCallFunc((a_oSender) => CFunc.Invoke(ref a_oCallback, NativeShare.ShareResult.NotShared, KCDefine.B_TEXT_UNKNOWN));
#endif // #if UNITY_IOS || UNITY_ANDROID
	}

	/** 공유 메세지를 전송했을 경우 */
	private void OnSendShareMsg(NativeShare.ShareResult a_eResult, string a_oTarget) {
		CFunc.ShowLog($"CUnityMsgSender.OnSendShareMsg: {a_eResult}, {a_oTarget}", KCDefine.B_LOG_COLOR_PLUGIN);
		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_UNITY_MS_SEND_SHARE_MSG_CALLBACK, () => m_oCallbackDict.GetValueOrDefault(ECallback.SHARE)?.Invoke(a_eResult, a_oTarget));
	}

	/** 유니티 메세지를 전송한다 */
	private void SendUnityMsg(string a_oCmd, string a_oMsg, System.Action<string, string> a_oCallback, string a_oDefMsg = KCDefine.B_TEXT_EMPTY) {
		CAccess.Assert(a_oCmd.ExIsValid());
		CDeviceMsgReceiver.Inst.AddCallback(a_oCmd, a_oCallback);

#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
#if UNITY_IOS
		CUnityMsgSender.OnReceiveUnityMsg(a_oCmd, a_oMsg);
#else
		m_oAndroidPlugin.CallStatic(KCDefine.U_FUNC_N_UNITY_MS_UNITY_MSG_HANDLER, a_oCmd, a_oMsg);
#endif // #if UNITY_IOS
#else
		this.ExLateCallFunc((a_oSender) => CFunc.Invoke(ref a_oCallback, a_oCmd, a_oDefMsg));
#endif // #if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
	}
	#endregion // 함수

	#region 조건부 함수
#if UNITY_ANDROID && GOOGLE_PLAY_UPDATE_ENABLE
	/** 스토어 버전 반환 메세지를 수신했을 경우 */
	private void OnReceiveGetStoreVerMsg(CTaskManager a_oSender, int a_nVer, bool a_bIsSuccess) {
		CFunc.ShowLog($"CUnityMsgSender.OnReceiveGetStoreVerMsg: {a_nVer}, {a_bIsSuccess}", KCDefine.B_LOG_COLOR_PLUGIN);
		
		var oMsg = new SimpleJSON.JSONClass();
		oMsg.Add(KCDefine.U_KEY_DEVICE_MR_VER, $"{a_nVer}");
		oMsg.Add(KCDefine.U_KEY_DEVICE_MR_RESULT, $"{a_bIsSuccess}");

		var oDeviceMsg = new SimpleJSON.JSONClass();
		oDeviceMsg.Add(KCDefine.U_KEY_DEVICE_CMD, KCDefine.B_CMD_GET_STORE_VER);
		oDeviceMsg.Add(KCDefine.U_KEY_DEVICE_MSG, oMsg.ToString());
		
		CDeviceMsgReceiver.Inst.SendMessage(KCDefine.U_FUNC_N_DEVICE_MR_MSG_HANDLER, oDeviceMsg.ToString(), SendMessageOptions.DontRequireReceiver);
	}
#endif // #if UNITY_ANDROID && GOOGLE_PLAY_UPDATE_ENABLE
	#endregion // 조건부 함수

	#region 조건부 클래스 함수
#if !UNITY_EDITOR && UNITY_IOS
	[DllImport("__Internal")]
	private static extern void OnReceiveUnityMsg(string a_oCmd, string a_oMsg);
#endif // #if !UNITY_EDITOR && UNITY_IOS
	#endregion // 조건부 클래스 함수
}

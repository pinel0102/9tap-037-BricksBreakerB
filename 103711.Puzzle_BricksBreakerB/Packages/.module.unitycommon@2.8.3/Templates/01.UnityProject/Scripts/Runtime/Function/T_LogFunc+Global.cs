#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 전역 로그 함수 */
public static partial class LogFunc {
#region 클래스 함수
	/** 앱 구동 로그를 전송한다 */
	public static void SendLaunchLog() {
		LogFunc.SendLog(KDefine.L_LOG_N_LAUNCH, LogFunc.MakeDefDatas());
	}

	/** 약관 동의 로그를 전송한다 */
	public static void SendAgreeLog() {
		LogFunc.SendOnceLog(KDefine.L_LOG_N_AGREE, LogFunc.MakeDefDatas());
	}
	
	/** 스플래시 로그를 전송한다 */
	public static void SendSplashLog() {
		LogFunc.SendLog(KDefine.L_LOG_N_SPLASH, LogFunc.MakeDefDatas());
	}
	
	/** 기본 데이터를 생성한다 */
	public static Dictionary<string, object> MakeDefDatas() {
		return new Dictionary<string, object>() {
			// Do Something
		};
	}
#endregion // 클래스 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if FIREBASE_MODULE_ENABLE
#if FIREBASE_CRASHLYTICS_ENABLE
using Firebase.Crashlytics;
#endif // #if FIREBASE_CRASHLYTICS_ENABLE

/** 파이어 베이스 관리자 - 크래시 */
public partial class CFirebaseManager : CSingleton<CFirebaseManager> {
#region 함수
	/** 크래시 유저 식별자를 변경한다 */
	public void SetCrashlyticsUserID(string a_oID) {
		CFunc.ShowLog($"CFirebaseManager.SetCrashlyticsUserID: {a_oID}", KCDefine.B_LOG_COLOR_PLUGIN);
		CAccess.Assert(a_oID.ExIsValid());

#if(UNITY_IOS || UNITY_ANDROID) && FIREBASE_CRASHLYTICS_ENABLE
		// 초기화 되었을 경우
		if(m_oBoolDict[EKey.IS_INIT]) {
			Crashlytics.SetUserId(a_oID);
		}
#endif // #if (UNITY_IOS || UNITY_ANDROID) && FIREBASE_CRASHLYTICS_ENABLE
	}

	/** 크래시 데이터를 변경한다 */
	public void SetCrashlyticsDatas(Dictionary<string, string> a_oDataDict) {
		CFunc.ShowLog($"CFirebaseManager.SetCrashlyticsDatas: {a_oDataDict}", KCDefine.B_LOG_COLOR_PLUGIN);
		CAccess.Assert(a_oDataDict.ExIsValid());

#if(UNITY_IOS || UNITY_ANDROID) && FIREBASE_CRASHLYTICS_ENABLE
		// 초기화 되었을 경우
		if(m_oBoolDict[EKey.IS_INIT]) {
			foreach(var stKeyVal in a_oDataDict) {
				Crashlytics.SetCustomKey(stKeyVal.Key, stKeyVal.Value);
			}
		}
#endif // #if (UNITY_IOS || UNITY_ANDROID) && FIREBASE_CRASHLYTICS_ENABLE
	}
#endregion // 함수
}
#endif // #if FIREBASE_MODULE_ENABLE

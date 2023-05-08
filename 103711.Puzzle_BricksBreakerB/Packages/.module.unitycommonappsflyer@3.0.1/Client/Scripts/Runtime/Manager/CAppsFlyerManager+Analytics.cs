using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if APPS_FLYER_MODULE_ENABLE
using AppsFlyerSDK;

#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif // #if PURCHASE_MODULE_ENABLE

/** 앱스 플라이어 - 분석 */
public partial class CAppsFlyerManager : CSingleton<CAppsFlyerManager> {
#region 함수
	/** 분석 유저 식별자를 변경한다 */
	public void SetAnalyticsUserID(string a_oID) {
		CFunc.ShowLog($"CAppsFlyerManager.SetAnalyticsUserID: {a_oID}", KCDefine.B_LOG_COLOR_PLUGIN);
		CAccess.Assert(a_oID.ExIsValid());

#if(UNITY_IOS || UNITY_ANDROID) && APPS_FLYER_ANALYTICS_ENABLE
		// 초기화 되었을 경우
		if(m_oBoolDict[EKey.IS_INIT]) {
			AppsFlyer.setCustomerUserId(a_oID);
		}
#endif // #if (UNITY_IOS || UNITY_ANDROID) && APPS_FLYER_ANALYTICS_ENABLE
	}

	/** 로그를 전송한다 */
	public void SendLog(string a_oName, Dictionary<string, string> a_oDataDict) {
		CFunc.ShowLog($"CAppsFlyerManager.SendLog: {a_oName}, {a_oDataDict}", KCDefine.B_LOG_COLOR_PLUGIN);
		CAccess.Assert(a_oName.ExIsValid());

#if((UNITY_IOS || UNITY_ANDROID) && APPS_FLYER_ANALYTICS_ENABLE) && (ANALYTICS_TEST_ENABLE || STORE_DIST_BUILD)
		// 초기화 되었을 경우
		if(m_oBoolDict[EKey.IS_INIT]) {
			AppsFlyer.sendEvent(a_oName, a_oDataDict ?? new Dictionary<string, string>());
		}
#endif // #if ((UNITY_IOS || UNITY_ANDROID) && APPS_FLYER_ANALYTICS_ENABLE) && (ANALYTICS_TEST_ENABLE || STORE_DIST_BUILD)                                                                                                                             
	}
#endregion // 함수

#region 조건부 함수
#if PURCHASE_MODULE_ENABLE
	/** 결제 로그를 전송한다 */
	public void SendPurchaseLog(Product a_oProduct, int a_nNumProducts, Dictionary<string, string> a_oDataDict) {
		CFunc.ShowLog($"CAppsFlyerManager.SendPurchaseLog: {a_oProduct}, {a_nNumProducts}", KCDefine.B_LOG_COLOR_PLUGIN);
		CAccess.Assert(a_oProduct != null && a_nNumProducts > KCDefine.B_VAL_0_INT);

#if((UNITY_IOS || UNITY_ANDROID) && APPS_FLYER_ANALYTICS_ENABLE) && (ANALYTICS_TEST_ENABLE || STORE_DIST_BUILD)
		// 초기화 되었을 경우
		if(m_oBoolDict[EKey.IS_INIT]) {
			AppsFlyer.sendEvent(AFInAppEvents.PURCHASE, new Dictionary<string, string>() {
				[AFInAppEvents.CONTENT_ID] = a_oProduct.definition.id, [AFInAppEvents.CURRENCY] = a_oProduct.metadata.isoCurrencyCode, [AFInAppEvents.QUANTITY] = $"{a_nNumProducts}", [AFInAppEvents.REVENUE] = $"{a_oProduct.metadata.localizedPrice}"
			});
		}
#endif // #if ((UNITY_IOS || UNITY_ANDROID) && APPS_FLYER_ANALYTICS_ENABLE) && (ANALYTICS_TEST_ENABLE || STORE_DIST_BUILD)                                                                                                                             
	}
#endif // #if PURCHASE_MODULE_ENABLE
#endregion // 조건부 함수
}
#endif // #if APPS_FLYER_MODULE_ENABLE

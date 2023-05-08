using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if FLURRY_MODULE_ENABLE
using FlurrySDK;

#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif // #if PURCHASE_MODULE_ENABLE

/** 플러리 관리자 - 분석 */
public partial class CFlurryManager : CSingleton<CFlurryManager> {
#region 함수
	/** 분석 유저 식별자를 변경한다 */
	public void SetAnalyticsUserID(string a_oID) {
		CFunc.ShowLog($"CFlurryManager.SetAnalyticsUserID: {a_oID}", KCDefine.B_LOG_COLOR_PLUGIN);
		CAccess.Assert(a_oID.ExIsValid());

#if(UNITY_IOS || UNITY_ANDROID) && FLURRY_ANALYTICS_ENABLE
		// 초기화 되었을 경우
		if(m_oBoolDict[EKey.IS_INIT]) {
			Flurry.SetUserId(a_oID);
		}
#endif // #if (UNITY_IOS || UNITY_ANDROID) && FLURRY_ANALYTICS_ENABLE
	}

	/** 로그를 전송한다 */
	public void SendLog(string a_oName, Dictionary<string, string> a_oDataDict) {
		CFunc.ShowLog($"CFlurryManager.SendLog: {a_oName}, {a_oDataDict}", KCDefine.B_LOG_COLOR_PLUGIN);
		CAccess.Assert(a_oName.ExIsValid());
				
#if((UNITY_IOS || UNITY_ANDROID) && FLURRY_ANALYTICS_ENABLE) && (ANALYTICS_TEST_ENABLE || STORE_DIST_BUILD)
		// 초기화 되었을 경우
		if(m_oBoolDict[EKey.IS_INIT]) {
			Flurry.LogEvent(a_oName, a_oDataDict ?? new Dictionary<string, string>());
		}
#endif // #if ((UNITY_IOS || UNITY_ANDROID) && FLURRY_ANALYTICS_ENABLE) && (ANALYTICS_TEST_ENABLE || STORE_DIST_BUILD)
	}
#endregion // 함수

#region 조건부 함수
#if PURCHASE_MODULE_ENABLE
	/** 결제 로그를 전송한다 */
	public void SendPurchaseLog(Product a_oProduct, int a_nNumProducts, Dictionary<string, string> a_oDataDict) {
		CFunc.ShowLog($"CFlurryManager.SendPurchaseLog: {a_oProduct}, {a_nNumProducts}", KCDefine.B_LOG_COLOR_PLUGIN);
		CAccess.Assert(a_oProduct != null && a_nNumProducts > KCDefine.B_VAL_0_INT);

#if((UNITY_IOS || UNITY_ANDROID) && FLURRY_ANALYTICS_ENABLE) && (ANALYTICS_TEST_ENABLE || STORE_DIST_BUILD)
		// 초기화 되었을 경우
		if(m_oBoolDict[EKey.IS_INIT]) {
			double dblPrice = decimal.ToDouble(a_oProduct.metadata.localizedPrice);
			Flurry.LogPayment(a_oProduct.metadata.localizedTitle, a_oProduct.definition.id, a_nNumProducts, dblPrice, a_oProduct.metadata.isoCurrencyCode, a_oProduct.transactionID, a_oDataDict);
		}
#endif // #if ((UNITY_IOS || UNITY_ANDROID) && FLURRY_ANALYTICS_ENABLE) && (ANALYTICS_TEST_ENABLE || STORE_DIST_BUILD)
	}
#endif // #if PURCHASE_MODULE_ENABLE
#endregion // 조건부 함수
}
#endif // #if FLURRY_MODULE_ENABLE

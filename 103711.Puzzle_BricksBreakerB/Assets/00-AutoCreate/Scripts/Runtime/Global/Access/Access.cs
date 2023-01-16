using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif // #if PURCHASE_MODULE_ENABLE

/** 기본 접근자 */
public static partial class Access {
	#region 클래스 프로퍼티
	public static float BannerAdsHeight {
		get {
#if ADS_MODULE_ENABLE
			return CAccess.GetBannerAdsHeight(KCDefine.U_SIZE_BANNER_ADS.y);
#else
			return KCDefine.B_VAL_0_REAL;
#endif // #if ADS_MODULE_ENABLE
		}
	}
	#endregion // 클래스 프로퍼티

	#region 조건부 클래스 함수
#if PURCHASE_MODULE_ENABLE
	/** 가격 문자열을 반환한다 */
	public static string GetPriceStr(int a_nProductIdx) {
		var oProduct = Access.GetProduct(a_nProductIdx);
		return (oProduct != null) ? CAccess.GetPriceStr(oProduct) : string.Empty;
	}

	/** 상품을 반환한다 */
	public static Product GetProduct(int a_nProductIdx) {
		bool bIsValid = CProductInfoTable.Inst.TryGetProductInfo(a_nProductIdx, out STProductInfo stProductInfo);
		CAccess.Assert(bIsValid);

		return CPurchaseManager.Inst.GetProduct(stProductInfo.m_oID);
	}
#endif // #if PURCHASE_MODULE_ENABLE
	#endregion // 조건부 클래스 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

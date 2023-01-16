#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 전역 접근자 확장 클래스 */
public static partial class AccessExtension {
#region 클래스 함수
	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EPlayMode a_eSender) {
		return a_eSender > EPlayMode.NONE && a_eSender < EPlayMode.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this ELoginType a_eSender) {
		return a_eSender > ELoginType.NONE && a_eSender < ELoginType.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EPurchaseType a_eSender) {
		return a_eSender > EPurchaseType.NONE && a_eSender < EPurchaseType.MAX_VAL;
	}

	/** 유효 여부를 검사한다 */
	public static bool ExIsValid(this EKindsGroupType a_eSender) {
		return a_eSender > EKindsGroupType.NONE && a_eSender < EKindsGroupType.MAX_VAL;
	}
	
	/** 어빌리티 값을 반환한다 */
	public static decimal ExGetAbilityVal(this Dictionary<EAbilityKinds, decimal> a_oSender, EAbilityKinds a_eAbilityKinds) {
		return a_oSender.ExGetAbilityVal(a_eAbilityKinds, a_oSender.GetValueOrDefault(a_eAbilityKinds));
	}

	/** 어빌리티 값을 반환한다 */
	public static decimal ExGetAbilityVal(this Dictionary<EAbilityKinds, decimal> a_oSender, EAbilityKinds a_eAbilityKinds, decimal a_dmVal) {
		decimal dmAddMultiplyVal = a_oSender.GetValueOrDefault(a_eAbilityKinds + ((int)EEnumVal.ST * (int)EAbilityValType.INCR));
		decimal dmSubMultiplyVal = a_oSender.GetValueOrDefault(a_eAbilityKinds + ((int)EEnumVal.ST * (int)EAbilityValType.DECR));

		return System.Math.Clamp(a_dmVal + (a_dmVal * dmAddMultiplyVal) - (a_dmVal * dmSubMultiplyVal), KCDefine.B_VAL_0_INT, decimal.MaxValue);
	}
	
	/** 타겟 값을 반환한다 */
	public static decimal ExGetTargetVal(this Dictionary<ulong, STTargetInfo> a_oSender, ETargetKinds a_eTargetKinds, int a_nKinds) {
		return a_oSender.ExTryGetTargetInfo(a_eTargetKinds, a_nKinds, out STTargetInfo stTargetInfo) ? stTargetInfo.m_stValInfo01.m_dmVal : KCDefine.B_VAL_0_INT;
	}

	/** 누적 타겟 정보 값을 반환한다 */
	public static (decimal, decimal, decimal) ExGetSkipTargetValInfo(this Dictionary<ulong, STTargetInfo> a_oSender, ETargetKinds a_eTargetKinds, int a_nKinds, int a_nSkipTimes, Dictionary<ulong, STTargetInfo> a_oSkipTargetInfoDict) {
		a_oSkipTargetInfoDict.ExTryGetTargetInfo(a_eTargetKinds, a_nKinds, out STTargetInfo stSkipTargetInfo);

		decimal dmMaxTargetVal = KCDefine.B_VAL_0_INT;
		decimal dmPrevMaxTargetVal = KCDefine.B_VAL_0_INT;

		for(int i = 0; i < a_nSkipTimes; ++i) {
			dmPrevMaxTargetVal = dmMaxTargetVal;
			dmMaxTargetVal = (i * stSkipTargetInfo.m_stValInfo01.m_dmVal) + ((dmPrevMaxTargetVal * stSkipTargetInfo.m_stValInfo02.m_dmVal) / KCDefine.B_UNIT_NORM_VAL_TO_PERCENT);
		}
		
		return (a_oSender.ExGetTargetVal(a_eTargetKinds, a_nKinds), dmPrevMaxTargetVal, dmMaxTargetVal);
	}

	/** 타겟 정보를 반환한다 */
	public static STTargetInfo ExGetTargetInfo(this Dictionary<ulong, STTargetInfo> a_oSender, ETargetKinds a_eTargetKinds, int a_nKinds) {
		bool bIsValid = a_oSender.ExTryGetTargetInfo(a_eTargetKinds, a_nKinds, out STTargetInfo stTargetInfo);
		CAccess.Assert(bIsValid);

		return stTargetInfo;
	}

	/** 타겟 정보를 반환한다 */
	public static CTargetInfo ExGetTargetInfo(this List<CTargetInfo> a_oSender, ETargetType a_eTargetType, int a_nKinds) {
		bool bIsValid = a_oSender.ExTryGetTargetInfo(a_eTargetType, a_nKinds, out CTargetInfo oTargetInfo);
		CAccess.Assert(bIsValid);

		return oTargetInfo;
	}

	/** 타겟 정보를 반환한다 */
	public static CTargetInfo ExGetTargetInfo(this List<CTargetInfo> a_oSender, ETargetType a_eTargetType, string a_oGUID) {
		bool bIsValid = a_oSender.ExTryGetTargetInfo(a_eTargetType, a_oGUID, out CTargetInfo oTargetInfo);
		CAccess.Assert(bIsValid);

		return oTargetInfo;
	}

	/** 타겟 정보를 반환한다 */
	public static bool ExTryGetTargetInfo(this Dictionary<ulong, STTargetInfo> a_oSender, ETargetKinds a_eTargetKinds, int a_nKinds, out STTargetInfo a_stOutTargetInfo) {
		a_stOutTargetInfo = a_oSender.GetValueOrDefault(Factory.MakeUTargetInfoID(a_eTargetKinds, a_nKinds), STTargetInfo.INVALID);
		return a_oSender.ContainsKey(Factory.MakeUTargetInfoID(a_eTargetKinds, a_nKinds));
	}

	/** 타겟 정보를 반환한다 */
	public static bool ExTryGetTargetInfo(this List<CTargetInfo> a_oSender, ETargetType a_eTargetType, int a_nKinds, out CTargetInfo a_oOutTargetInfo) {
		a_oOutTargetInfo = a_oSender.ExGetVal((a_oTargetInfo) => a_oTargetInfo.TargetType == a_eTargetType && a_oTargetInfo.Kinds == a_nKinds, null);
		return a_oOutTargetInfo != null;
	}

	/** 타겟 정보를 반환한다 */
	public static bool ExTryGetTargetInfo(this List<CTargetInfo> a_oSender, ETargetType a_eTargetType, string a_oGUID, out CTargetInfo a_oOutTargetInfo) {
		a_oOutTargetInfo = a_oSender.ExGetVal((a_oTargetInfo) => a_oTargetInfo.TargetType == a_eTargetType && a_oTargetInfo.GUID.Equals(a_oGUID), null);
		return a_oOutTargetInfo != null;
	}
	
	/** 소유자 타겟 정보를 변경한다 */
	public static void ExSetOwnerTargetInfo(this CTargetInfo a_oSender, CTargetInfo a_oOwnerTargetInfo, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oSender != null);

		// 타겟 정보가 존재 할 경우
		if(a_oSender != null) {
			a_oSender.OwnerGUID = (a_oOwnerTargetInfo != null) ? a_oOwnerTargetInfo.GUID : string.Empty;
			a_oSender.OwnerTargetType = (a_oOwnerTargetInfo != null) ? a_oOwnerTargetInfo.TargetType : ETargetType.NONE;
			a_oSender.m_oOwnerTargetInfo = a_oOwnerTargetInfo;
		}
	}
#endregion // 클래스 함수
}

/** 초기화 씬 접근자 확장 클래스 */
public static partial class AccessExtension {
#region 클래스 함수
	
#endregion // 클래스 함수
}

/** 시작 씬 접근자 확장 클래스 */
public static partial class AccessExtension {
#region 클래스 함수

#endregion // 클래스 함수
}

/** 설정 씬 접근자 확장 클래스 */
public static partial class AccessExtension {
#region 클래스 함수

#endregion // 클래스 함수
}

/** 약관 동의 씬 접근자 확장 클래스 */
public static partial class AccessExtension {
#region 클래스 함수

#endregion // 클래스 함수
}

/** 지연 설정 씬 접근자 확장 클래스 */
public static partial class AccessExtension {
#region 클래스 함수

#endregion // 클래스 함수
}

/** 타이틀 씬 접근자 확장 클래스 */
public static partial class AccessExtension {
#region 클래스 함수
	
#endregion // 클래스 함수
}

/** 메인 씬 접근자 확장 클래스 */
public static partial class AccessExtension {
#region 클래스 함수

#endregion // 클래스 함수
}

/** 게임 씬 접근자 확장 클래스 */
public static partial class AccessExtension {
#region 클래스 함수

#endregion // 클래스 함수
}

/** 로딩 씬 접근자 확장 클래스 */
public static partial class AccessExtension {
#region 클래스 함수

#endregion // 클래스 함수
}

/** 중첩 씬 접근자 확장 클래스 */
public static partial class AccessExtension {
#region 클래스 함수
	
#endregion // 클래스 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

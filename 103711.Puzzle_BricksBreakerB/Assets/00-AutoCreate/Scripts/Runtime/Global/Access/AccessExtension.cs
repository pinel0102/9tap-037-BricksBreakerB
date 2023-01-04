using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using UnityEngine.EventSystems;
using TMPro;

/** 기본 접근자 확장 클래스 */
public static partial class AccessExtension {
#region 클래스 함수
	/** 컴포넌트 상호 작용 여부를 변경한다 */
	public static void ExSetInteractable(this Button a_oSender, bool a_bIsEnable, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oSender != null);

		var oTouchInteractable = a_oSender?.GetComponentInChildren<CTouchInteractable>();
		oTouchInteractable?.SetInteractable(a_bIsEnable);
	}

	/** 텍스트를 변경한다 */
	public static void ExSetText(this Text a_oSender, string a_oStr, EFontSet a_eFontSet = EFontSet._1, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oSender != null && a_eFontSet.ExIsValid()));

		// 텍스트가 존재 할 경우
		if(a_oSender != null && a_eFontSet.ExIsValid()) {
			a_oSender.ExSetText(a_oStr, CLocalizeInfoTable.Inst.GetFontSetInfo(a_eFontSet), a_bIsEnableAssert);
		}
	}

	/** 텍스트를 변경한다 */
	public static void ExSetText(this TMP_Text a_oSender, string a_oStr, EFontSet a_eFontSet = EFontSet._1, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oSender != null && a_eFontSet.ExIsValid()));

		// 텍스트가 존재 할 경우
		if(a_oSender != null && a_eFontSet.ExIsValid()) {
			a_oSender.ExSetText(a_oStr, CLocalizeInfoTable.Inst.GetFontSetInfo(a_eFontSet), a_bIsEnableAssert);
		}
	}

	/** 텍스트를 변경한다 */
	public static void ExSetText(this InputField a_oSender, string a_oStr, EFontSet a_eFontSet = EFontSet._1, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oSender != null && a_eFontSet.ExIsValid()));

		// 텍스트가 존재 할 경우
		if(a_oSender != null && a_eFontSet.ExIsValid()) {
			a_oSender.ExSetText(a_oStr, CLocalizeInfoTable.Inst.GetFontSetInfo(a_eFontSet), a_bIsEnableAssert);
		}
	}

	/** 텍스트를 변경한다 */
	public static void ExSetText(this TMP_InputField a_oSender, string a_oStr, EFontSet a_eFontSet = EFontSet._1, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oSender != null && a_eFontSet.ExIsValid()));

		// 텍스트가 존재 할 경우
		if(a_oSender != null && a_eFontSet.ExIsValid()) {
			a_oSender.ExSetText(a_oStr, CLocalizeInfoTable.Inst.GetFontSetInfo(a_eFontSet), a_bIsEnableAssert);
		}
	}
#endregion // 클래스 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using System.Linq;
using UnityEditor;

/** 메뉴 처리자 - 익스포트 */
public static partial class CMenuHandler {
	#region 클래스 함수
	/** 텍스처 => PNG 로 추출한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_EXPORT_BASE + "Texture to PNG", false, KCEditorDefine.B_SORTING_O_EXPORT_MENU + 1)]
	public static void ExportTexToPNG() {
		var oTexList = Selection.objects.ExIsValid() ? Selection.objects.ExToTypes<Texture2D>() : null;

		// 선택 된 텍스처가 없을 경우
		if(!oTexList.ExIsValid()) {
			CEditorFunc.ShowAlertPopup(KCEditorDefine.B_TEXT_ALERT_P_TITLE, KCEditorDefine.B_MSG_ALERT_P_EXPORT_TEX_FAIL);
		} else {
			for(int i = 0; i < oTexList.Count; ++i) {
				string oFilePath = string.Format(KCEditorDefine.B_TEX_P_FMT_EXPORT, oTexList[i].name);
				CMenuHandler.SaveTex(oFilePath, oTexList[i]);
			}

			CEditorFunc.ShowAlertPopup(KCEditorDefine.B_TEXT_ALERT_P_TITLE, KCEditorDefine.B_MSG_ALERT_P_EXPORT_IMG_SUCCESS);
		}
	}

	/** 기본 텍스처 => PNG 로 추출한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_EXPORT_BASE + "White Texture to PNG", false, KCEditorDefine.B_SORTING_O_EXPORT_MENU + 1)]
	public static void ExportDefTexToPNGImg() {
		string oFilePath = string.Format(KCEditorDefine.B_TEX_P_FMT_EXPORT, Texture2D.whiteTexture.name);

		CMenuHandler.SaveTex(oFilePath, Texture2D.whiteTexture);
		CEditorFunc.ShowAlertPopup(KCEditorDefine.B_TEXT_ALERT_P_TITLE, KCEditorDefine.B_MSG_ALERT_P_EXPORT_IMG_SUCCESS);
	}

	/** 스프라이트 => PNG 로 추출한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_EXPORT_BASE + "Sprite to PNG", false, KCEditorDefine.B_SORTING_O_EXPORT_MENU + 1)]
	public static void ExportSpriteToPNGImg() {
		var oSpriteList = Selection.objects.ExIsValid() ? Selection.objects.ExToTypes<Sprite>() : null;

		// 선택 된 스프라이트가 없을 경우
		if(!oSpriteList.ExIsValid()) {
			CEditorFunc.ShowAlertPopup(KCEditorDefine.B_TEXT_ALERT_P_TITLE, KCEditorDefine.B_MSG_ALERT_P_EXPORT_TEX_FAIL);
		} else {
			for(int i = 0; i < oSpriteList.Count; ++i) {
				var oTex = new Texture2D((int)oSpriteList[i].textureRect.width, (int)oSpriteList[i].textureRect.height, oSpriteList[i].texture.format, KCDefine.B_VAL_1_INT, true);
				oTex.ExSetPixels(oSpriteList[i].texture.GetPixels((int)oSpriteList[i].textureRect.x, (int)oSpriteList[i].textureRect.y, (int)oSpriteList[i].textureRect.width, (int)oSpriteList[i].textureRect.height, KCDefine.B_VAL_0_INT).ToList());

				CFunc.WriteBytes(string.Format(KCEditorDefine.B_IMG_P_FMT_EXPORT, oSpriteList[i].name), oTex.EncodeToPNG(), false);
			}

			CEditorFunc.ShowAlertPopup(KCEditorDefine.B_TEXT_ALERT_P_TITLE, KCEditorDefine.B_MSG_ALERT_P_EXPORT_IMG_SUCCESS);
		}
	}

	/** 텍스처를 저장한다 */
	private static void SaveTex(string a_oFilePath, Texture2D a_oTex) {
		var oTex = new Texture2D(a_oTex.width, a_oTex.height, a_oTex.format, KCDefine.B_VAL_1_INT, true);
		oTex.ExSetPixels(a_oTex.GetPixels(KCDefine.B_VAL_0_INT).ToList());

		CFunc.WriteBytes(a_oFilePath, oTex.EncodeToPNG(), false);
	}
	#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR

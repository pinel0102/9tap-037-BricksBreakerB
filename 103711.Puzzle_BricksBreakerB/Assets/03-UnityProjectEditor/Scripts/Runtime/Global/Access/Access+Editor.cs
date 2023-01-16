using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
/** 에디터 씬 접근자 */
public static partial class Access {
	#region 클래스 함수

	#endregion // 클래스 함수

	#region 조건부 클래스 함수
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
	/** 스프라이트를 반환한다 */
	public static Sprite GetEditorObjSprite(EObjKinds a_eObjKinds, string a_oPrefix) {
		string oImgPath = NSEngine.KDefine.E_IMG_P_OBJ_DICT.GetValueOrDefault((EObjKinds)((int)a_eObjKinds).ExKindsToDetailSubKindsType(), string.Empty);
		string oEditorImgPath = string.Format(KCDefine.B_TEXT_FMT_2_COMBINE, a_oPrefix, oImgPath);

		return CResManager.Inst.GetRes<Sprite>(oEditorImgPath) ?? NSEngine.Access.GetSprite(a_eObjKinds);
	}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
	#endregion // 조건부 클래스 함수
}

/** 레벨 에디터 씬 접근자 */
public static partial class Access {
	#region 클래스 함수

	#endregion // 클래스 함수
}
#endif // #if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)

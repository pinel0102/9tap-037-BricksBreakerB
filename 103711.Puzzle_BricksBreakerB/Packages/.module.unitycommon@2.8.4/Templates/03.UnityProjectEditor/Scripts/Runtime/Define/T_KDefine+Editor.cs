#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
/** 에디터 상수 */
public static partial class KDefine {
#region 기본
	
#endregion // 기본
}

/** 레벨 에디터 씬 상수 */
public static partial class KDefine {
#region 기본
	// 횟수
	public const int LES_MAX_TRY_TIMES_SETUP_CELL_INFOS = byte.MaxValue;
	
	// 식별자
	public const string LES_KEY_SPRITE_OBJS_POOL = "SpriteObjsPool";

	// 이름 {
	public const string LES_OBJ_N_OBJ_SPRITE = "OBJ_SPRITE";
	public const string LES_OBJ_N_FMT_RE_UIS_PAGE_UIS = "PAGE_UIS_{0:00}";

	public const string LES_FUNC_N_FMT_SETUP_RE_UIS_PAGE_UIS = "SetupREUIsPageUIs{0:00}";
	public const string LES_FUNC_N_FMT_UPDATE_RE_UIS_PAGE_UIS = "UpdateREUIsPageUIs{0:00}";
	// 이름 }
#endregion // 기본
}
#endif // #if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
#endif // #if SCRIPT_TEMPLATE_ONLY

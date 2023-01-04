using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR || UNITY_STANDALONE
/** 에디터 씬 상수 */
public static partial class KCDefine {
	#region 기본
	// 이름
	public const string ES_OBJ_N_EDITOR_INPUT_POPUP = "EDITOR_INPUT_POPUP";
	public const string ES_OBJ_N_EDITOR_CREATE_POPUP = "EDITOR_CREATE_POPUP";
	#endregion // 기본

	#region 런타임 상수
	// 경로
	public static readonly string ES_OBJ_P_EDITOR_INPUT_POPUP = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_GLOBAL}{KCDefine.B_DIR_P_POPUP}G_EditorInputPopup";
	public static readonly string ES_OBJ_P_EDITOR_CREATE_POPUP = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_GLOBAL}{KCDefine.B_DIR_P_POPUP}G_EditorCreatePopup";
	public static readonly string ES_OBJ_P_EDITOR_SCROLLER_CELL_VIEW = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_GLOBAL}UI/ScrollView/G_EditorScrollerCellView";
	#endregion // 런타임 상수
}

/** 레벨 에디터 씬 상수 */
public static partial class KCDefine {
	#region 기본
	// 형식
	public const string LES_TEXT_FMT_LEVEL = "레벨 {0:0000}";
	public const string LES_TEXT_FMT_STAGE = "스테이지 {0:000}";
	public const string LES_TEXT_FMT_CHAPTER = "챕터 {0:00}";

	// 중앙 에디터 UI
	public const string LES_OBJ_N_ME_UIS_SAVE_BTN = "ME_UIS_SAVE_BTN";
	public const string LES_OBJ_N_ME_UIS_RESET_BTN = "ME_UIS_RESET_BTN";
	public const string LES_OBJ_N_ME_UIS_TEST_BTN = "ME_UIS_TEST_BTN";
	public const string LES_OBJ_N_ME_UIS_COPY_LEVEL_BTN = "ME_UIS_COPY_LEVEL_BTN";

	// 왼쪽 에디터 UI {
	public const string LES_OBJ_N_LE_UIS_ADD_LEVEL_BTN = "LE_UIS_ADD_LEVEL_BTN";

	public const string LES_OBJ_N_LE_UIS_LEVEL_SCROLL_VIEW = "LE_UIS_LEVEL_SCROLL_VIEW";
	public const string LES_OBJ_N_LE_UIS_CHAPTER_SCROLL_VIEW = "LE_UIS_CHAPTER_SCROLL_VIEW";

	public const string LES_OBJ_N_LE_UIS_STAGE_SCROLL_VIEW_01 = "LE_UIS_STAGE_SCROLL_VIEW_01";
	public const string LES_OBJ_N_LE_UIS_STAGE_SCROLL_VIEW_02 = "LE_UIS_STAGE_SCROLL_VIEW_02";
	// 왼쪽 에디터 UI }

	// 오른쪽 에디터 UI {
	public const string LES_OBJ_N_RE_UIS_PAGE_UIS_01_APPLY_BTN = "RE_UIS_PAGE_UIS_01_APPLY_BTN";
	public const string LES_OBJ_N_RE_UIS_PAGE_UIS_01_FILL_ALL_CELLS_BTN = "RE_UIS_PAGE_UIS_01_FILL_ALL_CELLS_BTN";
	public const string LES_OBJ_N_RE_UIS_PAGE_UIS_01_CLEAR_ALL_CELLS_BTN = "RE_UIS_PAGE_UIS_01_CLEAR_ALL_CELLS_BTN";
	public const string LES_OBJ_N_RE_UIS_PAGE_UIS_01_CLEAR_SEL_CELLS_BTN = "RE_UIS_PAGE_UIS_01_CLEAR_SEL_CELLS_BTN";

	public const string LES_OBJ_N_RE_UIS_PAGE_UIS_02_SCROLLER_CELL_VIEW = "RE_UIS_PAGE_UIS_02_SCROLLER_CELL_VIEW";
	// 오른쪽 에디터 UI }
	#endregion // 기본

	#region 런타임 상수
	// 경로
	public static readonly string LES_OBJ_P_RE_UIS_PAGE_UIS_02_SCROLLER_CELL_VIEW = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_LEVEL_EDITOR_SCENE}UI/ScrollView/G_REUIsPageUIs02ScrollCellView";
	#endregion // 런타임 상수
}
#endif // #if UNITY_EDITOR || UNITY_STANDALONE

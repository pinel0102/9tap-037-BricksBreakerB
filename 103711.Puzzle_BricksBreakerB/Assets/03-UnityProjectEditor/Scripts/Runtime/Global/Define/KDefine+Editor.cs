using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
/** 에디터 씬 상수 */
public static partial class KDefine {
	#region 기본
	// 알림
	public const string ES_MSG_NOTI_SAVE = "레벨 데이터를 저장합니다.";

	// 알림 팝업 {
	public const string ES_MSG_ALERT_P_QUIT = "에디터를 종료하시겠습니까?";
	public const string ES_MSG_ALERT_P_RESET = "에디터를 리셋하시겠습니까?";

	public const string ES_MSG_ALERT_P_REMOVE_LEVEL = "레벨을 제거하시겠습니까?";
	public const string ES_MSG_ALERT_P_REMOVE_STAGE = "스테이지를 제거하시겠습니까?";
	public const string ES_MSG_ALERT_P_REMOVE_CHAPTER = "챕터를 제거하시겠습니까?";

	public const string ES_MSG_ALERT_P_LOAD_LOCAL_TABLE = "로컬 테이블을 로드하시겠습니까?";
	public const string ES_MSG_ALERT_P_LOAD_REMOTE_TABLE = "원격 테이블을 로드하시겠습니까?";
	// 알림 팝업 }
	#endregion // 기본
}

/** 레벨 에디터 씬 상수 */
public static partial class KDefine {
	#region 기본
	// 개수
	public const int LES_MAX_NUM_OBJ_KINDS_IN_ROW = 4;

	// 횟수
	public const int LES_MAX_TIMES_TRY_SETUP_CELL_INFOS = byte.MaxValue;

	// 식별자 {
	public const string LES_KEY_SPRITE_OBJS_POOL = "SpriteObjsPool";
	public const string LES_KEY_LINE_FX_OBJS_POOL = "LineFXObjsPool";

	public const string LES_KEY_BTN_OBJS_POOL = "BtnObjsPool";
	// 식별자 }

	// 이름 {
	public const string LES_OBJ_N_OBJ_SPRITE = "OBJ_SPRITE";
    public const string LES_OBJ_N_UPPER_SPRITE = "UPPER_SPRITE";
	public const string LES_OBJ_N_GRID_LINE_FX = "GRID_LINE_FX";

	public const string LES_OBJ_N_GRID_LINE_BTN = "GRID_LINE_BTN";
	public const string LES_OBJ_N_FMT_RE_UIS_PAGE_UIS = "RE_UIS_PAGE_UIS_{0:00}";

	public const string LES_FUNC_N_FMT_SETUP_RE_UIS_PAGE_UIS = "SetupREUIsPageUIs{0:00}";
	public const string LES_FUNC_N_FMT_UPDATE_RE_UIS_PAGE_UIS = "UpdateREUIsPageUIs{0:00}";

	public const string LES_FUNC_N_FMT_SUB_SETUP_RE_UIS_PAGE_UIS = "SubSetupREUIsPageUIs{0:00}";
	public const string LES_FUNC_N_FMT_SUB_UPDATE_RE_UIS_PAGE_UIS = "SubUpdateREUIsPageUIs{0:00}";
	// 이름 }
	#endregion // 기본

	#region 런타임 상수
	// 색상
	public static readonly Color LES_COLOR_GRID_LINE = new Color(0.0f, 0.35f, 0.35f, 1.0f);

	// 비율
	public static readonly Vector3 LES_SCALE_EDITOR_OBJ_ROOT = new Vector3(1.5f, 1.5f, 1.5f);

	// 간격
	public static readonly Vector3 LES_OFFSET_H_GRID_LINE_BTN = new Vector3(0.0f, 20.0f, 0.0f);
	public static readonly Vector3 LES_OFFSET_V_GRID_LINE_BTN = new Vector3(-30.0f, 0.0f, 0.0f);

    public static readonly string U_OBJ_P_TEXT_BTN_GRID_NUM = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_LEVEL_EDITOR_SCENE}UI/Button/U_TextBtn_GridNum";
	#endregion // 런타임 상수
}
#endif // #if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)

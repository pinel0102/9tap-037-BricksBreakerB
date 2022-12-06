#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE
/** 서브 전역 상수 */
public static partial class KDefine {
#region 기본
	// 개수
	public const int G_MAX_NUM_COINS_BOX_COINS = 0;
	public const int G_MAX_NUM_ADS_SKIP_CLEAR_INFOS = 0;

	// 횟수
	public const int G_MAX_TIMES_ADS_SKIP = 0;
	public const int G_MAX_TIMES_ACQUIRE_FREE_REWARDS = 0;

	// 시간 {
	public const float G_DELAY_SCALE_01 = 1.0f;
	public const float G_DELAY_SCALE_02 = 1.0f;
	public const float G_DELAY_SCALE_03 = 1.0f;
	public const float G_DELAY_SCALE_04 = 1.0f;
	public const float G_DELAY_SCALE_05 = 1.0f;
	public const float G_DELAY_SCALE_06 = 1.0f;
	public const float G_DELAY_SCALE_07 = 1.0f;
	public const float G_DELAY_SCALE_08 = 1.0f;
	public const float G_DELAY_SCALE_09 = 1.0f;

	public const float G_DELTA_T_SCALE_01 = 1.0f;
	public const float G_DELTA_T_SCALE_02 = 1.0f;
	public const float G_DELTA_T_SCALE_03 = 1.0f;
	public const float G_DELTA_T_SCALE_04 = 1.0f;
	public const float G_DELTA_T_SCALE_05 = 1.0f;
	public const float G_DELTA_T_SCALE_06 = 1.0f;
	public const float G_DELTA_T_SCALE_07 = 1.0f;
	public const float G_DELTA_T_SCALE_08 = 1.0f;
	public const float G_DELTA_T_SCALE_09 = 1.0f;

	public const float G_DURATION_SCALE_01 = 1.0f;
	public const float G_DURATION_SCALE_02 = 1.0f;
	public const float G_DURATION_SCALE_03 = 1.0f;
	public const float G_DURATION_SCALE_04 = 1.0f;
	public const float G_DURATION_SCALE_05 = 1.0f;
	public const float G_DURATION_SCALE_06 = 1.0f;
	public const float G_DURATION_SCALE_07 = 1.0f;
	public const float G_DURATION_SCALE_08 = 1.0f;
	public const float G_DURATION_SCALE_09 = 1.0f;
	// 시간 }

	// 이름
	public const string G_OBJ_N_STORE_POPUP = "STORE_POPUP";
	public const string G_OBJ_N_SETTINGS_POPUP = "SETTINGS_POPUP";
	public const string G_OBJ_N_SYNC_POPUP = "SYNC_POPUP";
	public const string G_OBJ_N_DAILY_MISSION_POPUP = "DAILY_MISSION_POPUP";
	public const string G_OBJ_N_FREE_REWARD_POPUP = "FREE_REWARD_POPUP";
	public const string G_OBJ_N_DAILY_REWARD_POPUP = "DAILY_REWARD_POPUP";
	public const string G_OBJ_N_COINS_BOX_POPUP = "COINS_BOX_POPUP";
	public const string G_OBJ_N_REWARD_ACQUIRE_POPUP = "REWARD_ACQUIRE_POPUP";
	public const string G_OBJ_N_COINS_BOX_ACQUIRE_POPUP = "COINS_BOX_ACQUIRE_POPUP";
	public const string G_OBJ_N_CONTINUE_POPUP = "CONTINUE_POPUP";
	public const string G_OBJ_N_RESULT_POPUP = "RESULT_POPUP";
	public const string G_OBJ_N_PAUSE_POPUP = "PAUSE_POPUP";
	public const string G_OBJ_N_RESUME_POPUP = "RESUME_POPUP";
	public const string G_OBJ_N_PRODUCT_BUY_POPUP = "PRODUCT_BUY_POPUP";
	public const string G_OBJ_N_FOCUS_POPUP = "FOCUS_POPUP";
	public const string G_OBJ_N_TUTORIAL_POPUP = "TUTORIAL_POPUP";

	// 설정 팝업 {
	public const string G_IMG_P_SETTINGS_P_SND_ON = "G_SndOn";
	public const string G_IMG_P_SETTINGS_P_SND_OFF = "G_SndOff";

	public const string G_IMG_P_SETTINGS_P_BG_SND_ON = "G_BGSndOn";
	public const string G_IMG_P_SETTINGS_P_BG_SND_OFF = "G_BGSndOff";

	public const string G_IMG_P_SETTINGS_P_FX_SNDS_ON = "G_FXSndsOn";
	public const string G_IMG_P_SETTINGS_P_FX_SNDS_OFF = "G_FXSndsOff";

	public const string G_IMG_P_SETTINGS_P_VIBRATE_ON = "G_VibrateOn";
	public const string G_IMG_P_SETTINGS_P_VIBRATE_OFF = "G_VibrateOff";

	public const string G_IMG_P_SETTINGS_P_NOTI_ON = "G_NotiOn";
	public const string G_IMG_P_SETTINGS_P_NOTI_OFF = "G_NotiOff";
	// 설정 팝업 }

	// 경로
	public const string G_IMG_P_FMT_ITEM = "G_Item_{0}";
	public const string G_IMG_P_FMT_SKILL = "G_Skill_{0}";
	public const string G_IMG_P_FMT_OBJ = "G_Obj_{0}";
	public const string G_IMG_P_FMT_ABILITY = "G_Ability_{0}";

	// 씬 이름 {
	public const string G_SCENE_N_A_ADMOB = "01.A_AdmobScene";
	public const string G_SCENE_N_A_IRON_SRC = "02.A_IronSrcScene";
	public const string G_SCENE_N_G_GOOGLE_SHEET = "01.G_GoogleSheetScene";

	public const string G_SCENE_N_F_AUTH = "01.F_AuthScene";
	public const string G_SCENE_N_F_ANALYTICS = "02.F_AnalyticsScene";
	public const string G_SCENE_N_F_CRASHLYTICS = "03.F_CrashlyticsScene";
	public const string G_SCENE_N_F_DB = "04.F_DBScene";
	public const string G_SCENE_N_F_MSG = "05.F_MsgScene";
	public const string G_SCENE_N_F_CONFIG = "06.F_ConfigScene";
	public const string G_SCENE_N_F_STORAGE = "07.F_StorageScene";
	// 씬 이름 }
#endregion // 기본

#region 런타임 상수
	// 일일 보상
	public static readonly List<ERewardKinds> G_REWARDS_KINDS_DAILY_REWARD_LIST = new List<ERewardKinds>() {
		// Do Something
	};

	// 상점 상품 종류
	public static readonly List<EProductKinds> G_PRODUCT_KINDS_STORE_LIST = new List<EProductKinds>() {
		// Do Something
	};

	// 특수 패키지 상품 종류
	public static readonly List<EProductKinds> G_PRODUCT_KINDS_SPECIAL_PKGS_LIST = new List<EProductKinds>() {
		// Do Something
	};
#endregion // 런타임 상수
}

/** 서브 타이틀 씬 상수 */
public static partial class KDefine {
#region 기본

#endregion // 기본
}

/** 서브 메인 씬 상수 */
public static partial class KDefine {
#region 기본
	// 개수
	public const int MS_MAX_NUM_LEVELS_IN_ROW = 1;
	public const int MS_MAX_NUM_STAGES_IN_ROW = 1;
	public const int MS_MAX_NUM_CHAPTERS_IN_ROW = 1;
#endregion // 기본
}

/** 서브 게임 씬 상수 */
public static partial class KDefine {
#region 기본
	// 단위
	public const int GS_MIN_LEVEL_ENABLE_REWARD_ADS_WATCH = 0;
#endregion // 기본

#region 런타임 상수
	// 경로
	public static readonly string GS_TEX_P_FMT_BG = $"{KCDefine.B_DIR_P_TEXTURES}{KCDefine.B_DIR_P_GAME_SCENE}BG_{"{0:00}"}_{"{0:000}"}_{"{0:0000}"}";
	public static readonly string GS_TEX_P_FMT_UP_BG = $"{KCDefine.B_DIR_P_TEXTURES}{KCDefine.B_DIR_P_GAME_SCENE}UpBG_{"{0:00}"}_{"{0:000}"}_{"{0:0000}"}";
	public static readonly string GS_TEX_P_FMT_DOWN_BG = $"{KCDefine.B_DIR_P_TEXTURES}{KCDefine.B_DIR_P_GAME_SCENE}DownBG_{"{0:00}"}_{"{0:000}"}_{"{0:0000}"}";
	public static readonly string GS_TEX_P_FMT_LEFT_BG = $"{KCDefine.B_DIR_P_TEXTURES}{KCDefine.B_DIR_P_GAME_SCENE}LeftBG_{"{0:00}"}_{"{0:000}"}_{"{0:0000}"}";
	public static readonly string GS_TEX_P_FMT_RIGHT_BG = $"{KCDefine.B_DIR_P_TEXTURES}{KCDefine.B_DIR_P_GAME_SCENE}RightBG_{"{0:00}"}_{"{0:000}"}_{"{0:0000}"}";

	// 정렬 순서 {
	public static readonly STSortingOrderInfo GS_SORTING_OI_BG = new STSortingOrderInfo() {
		m_nOrder = sbyte.MaxValue * 0, m_oLayer = KCDefine.U_SORTING_L_UNDERGROUND
	};

	public static readonly STSortingOrderInfo GS_SORTING_OI_UP_BG = new STSortingOrderInfo() {
		m_nOrder = sbyte.MaxValue * 2, m_oLayer = KCDefine.U_SORTING_L_UNDERGROUND
	};

	public static readonly STSortingOrderInfo GS_SORTING_OI_DOWN_BG = new STSortingOrderInfo() {
		m_nOrder = sbyte.MaxValue * 2, m_oLayer = KCDefine.U_SORTING_L_UNDERGROUND
	};

	public static readonly STSortingOrderInfo GS_SORTING_OI_LEFT_BG = new STSortingOrderInfo() {
		m_nOrder = sbyte.MaxValue * 1, m_oLayer = KCDefine.U_SORTING_L_UNDERGROUND
	};

	public static readonly STSortingOrderInfo GS_SORTING_OI_RIGHT_BG = new STSortingOrderInfo() {
		m_nOrder = sbyte.MaxValue * 1, m_oLayer = KCDefine.U_SORTING_L_UNDERGROUND
	};
	// 정렬 순서 }
#endregion // 런타임 상수
}

/** 서브 로딩 씬 상수 */
public static partial class KDefine {
#region 기본

#endregion // 기본
}

/** 서브 중첩 씬 상수 */
public static partial class KDefine {
#region 기본

#endregion // 기본
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

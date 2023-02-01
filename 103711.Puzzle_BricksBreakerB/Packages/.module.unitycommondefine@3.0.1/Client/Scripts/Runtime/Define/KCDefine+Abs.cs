using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** 기본 상수 */
public static partial class KCDefine {
	#region 기본
	// 값 {
	public const int B_VAL_0_INT = 0;
	public const int B_VAL_1_INT = 1;
	public const int B_VAL_2_INT = 2;
	public const int B_VAL_3_INT = 3;
	public const int B_VAL_4_INT = 4;
	public const int B_VAL_5_INT = 5;
	public const int B_VAL_6_INT = 6;
	public const int B_VAL_7_INT = 7;
	public const int B_VAL_8_INT = 8;
	public const int B_VAL_9_INT = 9;

	public const float B_VAL_0_REAL = 0.0f;
	public const float B_VAL_1_REAL = 1.0f;
	public const float B_VAL_2_REAL = 2.0f;
	public const float B_VAL_3_REAL = 3.0f;
	public const float B_VAL_4_REAL = 4.0f;
	public const float B_VAL_5_REAL = 5.0f;
	public const float B_VAL_6_REAL = 6.0f;
	public const float B_VAL_7_REAL = 7.0f;
	public const float B_VAL_8_REAL = 8.0f;
	public const float B_VAL_9_REAL = 9.0f;

	public const float B_VAL_0_1_REAL = 0.1f;
	public const float B_VAL_0_2_REAL = 0.2f;
	public const float B_VAL_0_3_REAL = 0.3f;
	public const float B_VAL_0_4_REAL = 0.4f;
	public const float B_VAL_0_5_REAL = 0.5f;
	public const float B_VAL_0_6_REAL = 0.6f;
	public const float B_VAL_0_7_REAL = 0.7f;
	public const float B_VAL_0_8_REAL = 0.8f;
	public const float B_VAL_0_9_REAL = 0.9f;

	public const float B_VAL_0_0_1_REAL = 0.01f;
	public const float B_VAL_0_0_2_REAL = 0.02f;
	public const float B_VAL_0_0_3_REAL = 0.03f;
	public const float B_VAL_0_0_4_REAL = 0.04f;
	public const float B_VAL_0_0_5_REAL = 0.05f;
	public const float B_VAL_0_0_6_REAL = 0.06f;
	public const float B_VAL_0_0_7_REAL = 0.07f;
	public const float B_VAL_0_0_8_REAL = 0.08f;
	public const float B_VAL_0_0_9_REAL = 0.09f;
	// 값 }

	// 회전 {
	public const float B_ANGLE_0_DEG = 0.0f;
	public const float B_ANGLE_45_DEG = 45.0f;
	public const float B_ANGLE_90_DEG = 90.0f;
	public const float B_ANGLE_180_DEG = 180.0f;
	public const float B_ANGLE_360_DEG = 360.0f;

	public const float B_ANGLE_0_RAD = KCDefine.B_ANGLE_0_DEG * Mathf.Deg2Rad;
	public const float B_ANGLE_45_RAD = KCDefine.B_ANGLE_45_DEG * Mathf.Deg2Rad;
	public const float B_ANGLE_90_RAD = KCDefine.B_ANGLE_90_DEG * Mathf.Deg2Rad;
	public const float B_ANGLE_180_RAD = KCDefine.B_ANGLE_180_DEG * Mathf.Deg2Rad;
	public const float B_ANGLE_360_RAD = KCDefine.B_ANGLE_360_DEG * Mathf.Deg2Rad;
	// 회전 }

	// 단위 {
	public const int B_UNIT_KINDS_PER_TYPE = 100000000;
	public const int B_UNIT_KINDS_PER_SUB_TYPE = 10000000;
	public const int B_UNIT_KINDS_PER_KINDS_TYPE = 100000;
	public const int B_UNIT_KINDS_PER_SUB_KINDS_TYPE = 100;

	public const int B_UNIT_IDS_PER_IDS_01 = 1;
	public const int B_UNIT_IDS_PER_IDS_02 = 10000;
	public const int B_UNIT_IDS_PER_IDS_03 = 10000000;

	public const int B_UNIT_NORM_VAL_TO_BYTE = 255;
	public const int B_UNIT_NORM_VAL_TO_PERCENT = 100;

	public const int B_UNIT_BITS_PER_BYTE = 8;
	public const int B_UNIT_BYTES_PER_KILO_BYTE = 1024;
	public const int B_UNIT_BYTES_PER_MEGA_BYTE = 1024 * 1024;

	public const int B_UNIT_SECS_PER_HOUR = 360;
	public const int B_UNIT_SECS_PER_MINUTE = 60;

	public const int B_UNIT_MILLI_SECS_PER_SEC = 1000;
	public const int B_UNIT_MICRO_SECS_PER_SEC = 1000000;

	public const int B_UNIT_DIGITS_PER_TEN = 10;
	public const int B_UNIT_DIGITS_PER_HUNDRED = 100;
	public const int B_UNIT_DIGITS_PER_THOUSAND = 1000;
	public const int B_UNIT_DIGITS_PER_TEN_THOUSAND = 10000;
	public const int B_UNIT_DIGITS_PER_HUNDRED_THOUSAND = 100000;

	public const float B_UNIT_PIXELS_PER_UNIT = 1.0f;
	public const float B_UNIT_REF_PIXELS_PER_UNIT = KCDefine.B_UNIT_PIXELS_PER_UNIT;

#if UNIT_SCALE_CORRECT_ENABLE
	public const float B_UNIT_SCALE = 0.01f;
	public const float B_UNIT_LIGHTMAP_RESOLUTION = 1.0f;
#else
	public const float B_UNIT_SCALE = 1.0f;
	public const float B_UNIT_LIGHTMAP_RESOLUTION = 0.01f;
#endif // #if UNIT_SCALE_CORRECT_ENABLE
	// 단위 }

	// 빌드 번호
	public const int B_MIN_BUILD_NUM = 1;

	// 비교
	public const int B_COMPARE_LESS = -1;
	public const int B_COMPARE_EQUALS = 0;
	public const int B_COMPARE_GREATE = 1;

	// 횟수 {
	public const int B_TIMES_INT_ONCE = 1;
	public const int B_TIMES_INT_INFINITE = -1;

	public const uint B_TIMES_UINT_ONCE = 1;
	public const uint B_TIMES_UINT_INFINITE = uint.MaxValue;
	// 횟수 }

	// 시간 {
	public const float B_DELTA_T_INFINITE = float.MaxValue;
	public const float B_DELTA_T_INTERMEDIATE = float.Epsilon;

	public const float B_DELTA_T_ASYNC_TASK = 0.15f;
	public const float B_DELTA_T_ASYNC_OPERATION = 0.15f;
	public const float B_MAX_DELTA_T_TRACKING_CONSENT_VIEW = 2.5f;

	public const double B_DELTA_T_UTC_TO_PST = -8.0;
	// 시간 }

	// 플래그 {
	public const BindingFlags B_BINDING_F_PUBLIC_STATIC = BindingFlags.Public | BindingFlags.Static;
	public const BindingFlags B_BINDING_F_NON_PUBLIC_STATIC = BindingFlags.NonPublic | BindingFlags.Static;

	public const BindingFlags B_BINDING_F_PUBLIC_INSTANCE = BindingFlags.Public | BindingFlags.Instance;
	public const BindingFlags B_BINDING_F_NON_PUBLIC_INSTANCE = BindingFlags.NonPublic | BindingFlags.Instance;
	// 플래그 }

	// 인덱스
	public const int B_IDX_INVALID = -1;

	// 디바이스 {
	public const int B_MIN_TARGET_FRAME_RATE = 30;

	public const int B_PORTRAIT_SCREEN_WIDTH = 720;
	public const int B_PORTRAIT_SCREEN_HEIGHT = 1280;
	public const int B_PORTRAIT_DPI_SCREEN_HEIGHT = 1920;

	public const int B_LANDSCAPE_SCREEN_WIDTH = 1280;
	public const int B_LANDSCAPE_SCREEN_HEIGHT = 720;
	public const int B_LANDSCAPE_DPI_SCREEN_HEIGHT = 1080;

#if MODE_PORTRAIT_ENABLE
	public const int B_SCREEN_WIDTH = KCDefine.B_PORTRAIT_SCREEN_WIDTH;
	public const int B_SCREEN_HEIGHT = KCDefine.B_PORTRAIT_SCREEN_HEIGHT;
	public const int B_DPI_SCREEN_HEIGHT = KCDefine.B_PORTRAIT_DPI_SCREEN_HEIGHT;
#else
	public const int B_SCREEN_WIDTH = 2560;
	public const int B_SCREEN_HEIGHT = 1440;
	public const int B_DPI_SCREEN_HEIGHT = KCDefine.B_LANDSCAPE_DPI_SCREEN_HEIGHT;
#endif // #if MODE_PORTRAIT_ENABLE

	public const float B_DEF_SCREEN_DPI = 160.0f;
	public const float B_PLATFORM_SCREEN_DPI = 480.0f;

	public const float B_FIXED_DELTA_TIME = 0.02f;
	public const float B_DESKTOP_SCREEN_RATE = 0.9f;
	public const float B_ADDITIONAL_LIGHT_INTENSITY = 0.99999f;

	public const string B_TEXT_NULL = "null";
	public const string B_TEXT_TRUE = "True";
	public const string B_TEXT_FALSE = "False";

	public const string B_TEXT_EMPTY = "";
	public const string B_TEXT_UNKNOWN = "Unknown";
	public const string B_TEXT_NEW_LINE = "\n";

	public const string B_STR_0_INT = "0";
	public const string B_STR_1_INT = "1";
	public const string B_STR_2_INT = "2";
	public const string B_STR_3_INT = "3";
	public const string B_STR_4_INT = "4";
	public const string B_STR_5_INT = "5";
	public const string B_STR_6_INT = "6";
	public const string B_STR_7_INT = "7";
	public const string B_STR_8_INT = "8";
	public const string B_STR_9_INT = "9";

	public const string B_STR_0_REAL = "0.0";
	public const string B_STR_1_REAL = "1.0";
	public const string B_STR_2_REAL = "2.0";
	public const string B_STR_3_REAL = "3.0";
	public const string B_STR_4_REAL = "4.0";
	public const string B_STR_5_REAL = "5.0";
	public const string B_STR_6_REAL = "6.0";
	public const string B_STR_7_REAL = "7.0";
	public const string B_STR_8_REAL = "8.0";
	public const string B_STR_9_REAL = "9.0";

	public const string B_FILE_EXTENSION_CSV = ".csv";
	public const string B_FILE_EXTENSION_TXT = ".txt";
	public const string B_FILE_EXTENSION_MAT = ".mat";
	public const string B_FILE_EXTENSION_HLSL = ".hlsl";
	public const string B_FILE_EXTENSION_JSON = ".json";
	public const string B_FILE_EXTENSION_ASSET = ".asset";
	public const string B_FILE_EXTENSION_UNITY = ".unity";
	public const string B_FILE_EXTENSION_BYTES = ".bytes";
	public const string B_FILE_EXTENSION_SHADER = ".shader";
	public const string B_FILE_EXTENSION_PREFAB = ".prefab";
	public const string B_FILE_EXTENSION_DS_STORE = ".DS_Store";
	public const string B_FILE_EXTENSION_SPRITE_ATLAS = ".spriteatlas";
	public const string B_FILE_EXTENSION_TMP_FONT_ASSET = KCDefine.B_FILE_EXTENSION_ASSET;

	public const SystemLanguage B_DEF_LANGUAGE = SystemLanguage.English;
	public static readonly Vector3 B_SCREEN_SIZE = new Vector3(KCDefine.B_SCREEN_WIDTH, KCDefine.B_SCREEN_HEIGHT, 0.0f);
	// 디바이스 }

	// 국가 코드
	public const string B_KOREA_COUNTRY_CODE = "KR";
	public const string B_AMERICA_COUNTRY_CODE = "US";

	// 이름 {
	public const string B_PLATFORM_N_IOS_APPLE = "iOSApple";

	public const string B_PLATFORM_N_ANDROID_GOOGLE = "AndroidGoogle";
	public const string B_PLATFORM_N_ANDROID_AMAZON = "AndroidAmazon";

	public const string B_PLATFORM_N_STANDALONE_MAC_STEAM = "StandaloneMacSteam";
	public const string B_PLATFORM_N_STANDALONE_WNDS_STEAM = "StandaloneWndsSteam";

	public const string B_DIR_N_EXTERNAL_DATAS = "ExternalDatas";

	public const string B_NAME_PATTERN_TEX_FIX_REPEAT_WRAP = "__FIX_RW__";
	public const string B_NAME_PATTERN_TEX_FIX_POINT_FILTER = "__FIX_PF__";
	public const string B_NAME_PATTERN_AUDIO_FIX_COMPRESS_IN_MEMORY = "__FIX_CIM__";

	public const string B_NAME_PATTERN_ENABLE_READABLE = "__ENABLE_R__";
	public const string B_NAME_PATTERN_IGNORE_LINEAR_PIPELINE = "__IGNORE_LP__";
	public const string B_NAME_PATTERN_IGNORE_SETUP_CULLING_MASK = "__IGNORE_SCM__";
	public const string B_NAME_PATTERN_IGNORE_TEX_COMPRESS = "__IGNORE_TC__";
	// 이름 }

	// 씬 이름 {
	public const string B_SCENE_N_SAMPLE = "00-SampleScene";
	public const string B_SCENE_N_MENU_SAMPLE = "10-MenuSampleScene";
	public const string B_SCENE_N_STUDY_SAMPLE = "11-StudySampleScene";
	public const string B_SCENE_N_EDITOR_SAMPLE = "01-EditorSampleScene";

	public const string B_SCENE_N_INIT = "00-InitScene";
	public const string B_SCENE_N_START = "00-StartScene";
	public const string B_SCENE_N_SETUP = "01-SetupScene";
	public const string B_SCENE_N_AGREE = "01-AgreeScene";
	public const string B_SCENE_N_LATE_SETUP = "01-LateSetupScene";

	public const string B_SCENE_N_TITLE = "01-TitleScene";
	public const string B_SCENE_N_MAIN = "02-MainScene";
	public const string B_SCENE_N_GAME = "03-GameScene";
	public const string B_SCENE_N_LOADING = "91-LoadingScene";
	public const string B_SCENE_N_OVERLAY = "92-OverlayScene";

	public const string B_SCENE_N_MENU = "02-MenuScene";
	public const string B_SCENE_N_TEST = "99-TestScene";
	public const string B_SCENE_N_LEVEL_EDITOR = "91-LevelEditorScene";
	// 씬 이름 }

	// 식별자 {
	public const string B_KEY_ROOT = "Root";
	public const string B_KEY_COMMON = "Common";
	public const string B_KEY_BUY_TRADE = "BuyTrade";
	public const string B_KEY_SALE_TRADE = "SaleTrade";
	public const string B_KEY_ENHANCE_TRADE = "EnhanceTrade";

	public const string B_KEY_JSON_USER_INFO_DATA = "UserInfo";
	public const string B_KEY_JSON_GAME_INFO_DATA = "GameInfo";
	public const string B_KEY_JSON_COMMON_APP_INFO_DATA = "CommonAppInfo";
	public const string B_KEY_JSON_COMMON_USER_INFO_DATA = "CommonUserInfo";
	// 식별자 }

	// 토큰 {
	public const string B_TOKEN_USER_A = "A";
	public const string B_TOKEN_USER_B = "B";
	public const string B_TOKEN_USER_UNKNOWN = "U";
	public const string B_TOKEN_TITLE = "=====";

	public const string B_TOKEN_DOT = ".";
	public const string B_TOKEN_COMMA = ",";

	public const string B_TOKEN_SPACE = " ";
	public const string B_TOKEN_CROSS = "x";
	public const string B_TOKEN_SLASH = "/";
	public const string B_TOKEN_R_SLASH = "\\";
	public const string B_TOKEN_D_QUOTES = "\"";
	public const string B_TOKEN_O_BRACKETS = "[";
	public const string B_TOKEN_C_BRACKETS = "]";

	public const string B_TOKEN_INCR = "+";
	public const string B_TOKEN_DECR = "-";
	public const string B_TOKEN_DASH = "-";

	public const string B_TOKEN_COLON = ":";
	public const string B_TOKEN_SEMI_COLON = ";";

	public const string B_TOKEN_AMPERSAND = "&";
	public const string B_TOKEN_UNDER_SCORE = "_";
	// 토큰 }

	// 버전
	public const string B_DEF_VER = "1.0.0";

	// 이벤트
	public const string B_EVENT_START = "Start";
	public const string B_EVENT_CANCEL = "Cancel";

	// 명령어 {
	public const string B_CMD_GET_DEVICE_ID = "GetDeviceID";
	public const string B_CMD_GET_DEVICE_TYPE = "GetDeviceType";
	public const string B_CMD_GET_COUNTRY_CODE = "GetCountryCode";
	public const string B_CMD_GET_STORE_VER = "GetStoreVer";

	public const string B_CMD_SET_ENABLE_ADS_TRACKING = "SetEnableAdsTracking";

	public const string B_CMD_SHOW_ALERT = "ShowAlert";
	public const string B_CMD_SHOW_TOAST = "ShowToast";

	public const string B_CMD_MAIL = "Mail";
	public const string B_CMD_VIBRATE = "Vibrate";
	public const string B_CMD_INDICATOR = "Indicator";
	// 명령어 }

	// 경로 {
	public const string B_DIR_P_FXS = "FXs/";
	public const string B_DIR_P_ICONS = "Icons/";
	public const string B_DIR_P_FONTS = "Fonts/";
	public const string B_DIR_P_DATAS = "Datas/";
	public const string B_DIR_P_SOUNDS = "Sounds/";

	public const string B_DIR_P_TABLES = "Tables/";
	public const string B_DIR_P_SHADERS = "Shaders/";
	public const string B_DIR_P_PREFABS = "Prefabs/";
	public const string B_DIR_P_TEXTURES = "Textures/";
	public const string B_DIR_P_MATERIALS = "Materials/";

	public const string B_DIR_P_PIPELINES = "Pipelines/";
	public const string B_DIR_P_SCRIPTABLES = "Scriptables/";
	public const string B_DIR_P_SPRITE_ATLASES = "SpriteAtlases/";
	public const string B_DIR_P_SETTINGS = "Settings/";

	public const string B_DIR_P_GLOBAL = "Global/";
	public const string B_DIR_P_UTILITY = "Utility/";
	public const string B_DIR_P_TUTORIAL = "Tutorial/";
	public const string B_DIR_P_ENGINE = "Engine/";
	public const string B_DIR_P_POPUP = "Popup/";

	public const string B_DIR_P_INIT_SCENE = "InitScene/";
	public const string B_DIR_P_START_SCENE = "StartScene/";
	public const string B_DIR_P_SETUP_SCENE = "SetupScene/";
	public const string B_DIR_P_AGREE_SCENE = "AgreeScene/";
	public const string B_DIR_P_LATE_SETUP_SCENE = "LateSetupScene/";

	public const string B_DIR_P_TITLE_SCENE = "TitleScene/";
	public const string B_DIR_P_MAIN_SCENE = "MainScene/";
	public const string B_DIR_P_GAME_SCENE = "GameScene/";
	public const string B_DIR_P_LOADING_SCENE = "LoadingScene/";
	public const string B_DIR_P_OVERLAY_SCENE = "OverlayScene/";

	public const string B_DIR_P_MENU_SCENE = "MenuScene/";
	public const string B_DIR_P_EDITOR_SCENE = "EditorScene/";
	public const string B_DIR_P_LEVEL_EDITOR_SCENE = "LevelEditorScene/";
	// 경로 }

	// 형식 {
	public const string B_PATTERN_SPACE = @"\s+";

	public const string B_TEXT_FMT_INCR = "+{0}";
	public const string B_TEXT_FMT_DECR = "-{0}";
	public const string B_TEXT_FMT_CROSS = "x{0}";
	public const string B_TEXT_FMT_PERCENT = "{0}%";
	public const string B_TEXT_FMT_BRACKET = "({0})";

	public const string B_TEXT_FMT_VER = "Ver.{0}{1}";
	public const string B_TEXT_FMT_DICT = "[{0}]{1}";
	public const string B_TEXT_FMT_ENHANCE_01 = "{0} +{1}";
	public const string B_TEXT_FMT_ENHANCE_02 = "{0} -> {1}";

	public const string B_TEXT_FMT_SIZE = "<size={0}>{1}</size>";
	public const string B_TEXT_FMT_COLOR = "<color=#{0}>{1}</color>";

	public const string B_TEXT_FMT_NUM = "{0}.{1}{2}";
	public const string B_TEXT_FMT_CURRENCY = "{0:N0}";
	public const string B_TEXT_FMT_USD_PRICE = "USD {0}";
	public const string B_TEXT_FMT_USD_CURRENCY = "USD {0:N2}";

	public const string B_TEXT_FMT_MM_SS = "{0:00}:{1:00}";
	public const string B_TEXT_FMT_HH_MM_SS = "{0:00}:{1:00}:{2:00}";

	public const string B_TEXT_FMT_1_INT = "{0:0}";
	public const string B_TEXT_FMT_2_INT = "{0:00}";
	public const string B_TEXT_FMT_3_INT = "{0:000}";
	public const string B_TEXT_FMT_4_INT = "{0:0000}";
	public const string B_TEXT_FMT_5_INT = "{0:00000}";
	public const string B_TEXT_FMT_6_INT = "{0:000000}";
	public const string B_TEXT_FMT_7_INT = "{0:0000000}";
	public const string B_TEXT_FMT_8_INT = "{0:00000000}";
	public const string B_TEXT_FMT_9_INT = "{0:000000000}";

	public const string B_TEXT_FMT_1_REAL = "{0:0.0}";
	public const string B_TEXT_FMT_2_REAL = "{0:0.00}";
	public const string B_TEXT_FMT_3_REAL = "{0:0.000}";
	public const string B_TEXT_FMT_4_REAL = "{0:0.0000}";
	public const string B_TEXT_FMT_5_REAL = "{0:0.00000}";
	public const string B_TEXT_FMT_6_REAL = "{0:0.000000}";
	public const string B_TEXT_FMT_7_REAL = "{0:0.0000000}";
	public const string B_TEXT_FMT_8_REAL = "{0:0.00000000}";
	public const string B_TEXT_FMT_9_REAL = "{0:0.000000000}";

	public const string B_TEXT_FMT_2_COMBINE = "{0}{1}";
	public const string B_TEXT_FMT_3_COMBINE = "{0}{1}{2}";
	public const string B_TEXT_FMT_4_COMBINE = "{0}{1}{2}{3}";
	public const string B_TEXT_FMT_5_COMBINE = "{0}{1}{2}{3}{4}";
	public const string B_TEXT_FMT_6_COMBINE = "{0}{1}{2}{3}{4}{5}";
	public const string B_TEXT_FMT_7_COMBINE = "{0}{1}{2}{3}{4}{5}{6}";
	public const string B_TEXT_FMT_8_COMBINE = "{0}{1}{2}{3}{4}{5}{6}{7}";
	public const string B_TEXT_FMT_9_COMBINE = "{0}{1}{2}{3}{4}{5}{6}{7}{8}";

	public const string B_TEXT_FMT_2_DOT_COMBINE = "{0}.{1}";
	public const string B_TEXT_FMT_3_DOT_COMBINE = "{0}.{1}.{2}";
	public const string B_TEXT_FMT_4_DOT_COMBINE = "{0}.{1}.{2}.{3}";
	public const string B_TEXT_FMT_5_DOT_COMBINE = "{0}.{1}.{2}.{3}.{4}";
	public const string B_TEXT_FMT_6_DOT_COMBINE = "{0}.{1}.{2}.{3}.{4}.{5}";
	public const string B_TEXT_FMT_7_DOT_COMBINE = "{0}.{1}.{2}.{3}.{4}.{5}.{6}";
	public const string B_TEXT_FMT_8_DOT_COMBINE = "{0}.{1}.{2}.{3}.{4}.{5}.{6}.{7}";
	public const string B_TEXT_FMT_9_DOT_COMBINE = "{0}.{1}.{2}.{3}.{4}.{5}.{6}.{7}.{8}";

	public const string B_TEXT_FMT_2_COMMA_COMBINE = "{0},{1}";
	public const string B_TEXT_FMT_3_COMMA_COMBINE = "{0},{1},{2}";
	public const string B_TEXT_FMT_4_COMMA_COMBINE = "{0},{1},{2},{3}";
	public const string B_TEXT_FMT_5_COMMA_COMBINE = "{0},{1},{2},{3},{4}";
	public const string B_TEXT_FMT_6_COMMA_COMBINE = "{0},{1},{2},{3},{4},{5}";
	public const string B_TEXT_FMT_7_COMMA_COMBINE = "{0},{1},{2},{3},{4},{5},{6}";
	public const string B_TEXT_FMT_8_COMMA_COMBINE = "{0},{1},{2},{3},{4},{5},{6},{7}";
	public const string B_TEXT_FMT_9_COMMA_COMBINE = "{0},{1},{2},{3},{4},{5},{6},{7},{8}";

	public const string B_TEXT_FMT_2_SPACE_COMBINE = "{0} {1}";
	public const string B_TEXT_FMT_3_SPACE_COMBINE = "{0} {1} {2}";
	public const string B_TEXT_FMT_4_SPACE_COMBINE = "{0} {1} {2} {3}";
	public const string B_TEXT_FMT_5_SPACE_COMBINE = "{0} {1} {2} {3} {4}";
	public const string B_TEXT_FMT_6_SPACE_COMBINE = "{0} {1} {2} {3} {4} {5}";
	public const string B_TEXT_FMT_7_SPACE_COMBINE = "{0} {1} {2} {3} {4} {5} {6}";
	public const string B_TEXT_FMT_8_SPACE_COMBINE = "{0} {1} {2} {3} {4} {5} {6} {7}";
	public const string B_TEXT_FMT_9_SPACE_COMBINE = "{0} {1} {2} {3} {4} {5} {6} {7} {8}";

	public const string B_TEXT_FMT_2_CROSS_COMBINE = "{0}x{1}";
	public const string B_TEXT_FMT_3_CROSS_COMBINE = "{0}x{1}x{2}";
	public const string B_TEXT_FMT_4_CROSS_COMBINE = "{0}x{1}x{2}x{3}";
	public const string B_TEXT_FMT_5_CROSS_COMBINE = "{0}x{1}x{2}x{3}x{4}";
	public const string B_TEXT_FMT_6_CROSS_COMBINE = "{0}x{1}x{2}x{3}x{4}x{5}";
	public const string B_TEXT_FMT_7_CROSS_COMBINE = "{0}x{1}x{2}x{3}x{4}x{5}x{6}";
	public const string B_TEXT_FMT_8_CROSS_COMBINE = "{0}x{1}x{2}x{3}x{4}x{5}x{6}x{7}";
	public const string B_TEXT_FMT_9_CROSS_COMBINE = "{0}x{1}x{2}x{3}x{4}x{5}x{6}x{7}x{8}";

	public const string B_TEXT_FMT_2_SLASH_COMBINE = "{0}/{1}";
	public const string B_TEXT_FMT_3_SLASH_COMBINE = "{0}/{1}/{2}";
	public const string B_TEXT_FMT_4_SLASH_COMBINE = "{0}/{1}/{2}/{3}";
	public const string B_TEXT_FMT_5_SLASH_COMBINE = "{0}/{1}/{2}/{3}/{4}";
	public const string B_TEXT_FMT_6_SLASH_COMBINE = "{0}/{1}/{2}/{3}/{4}/{5}";
	public const string B_TEXT_FMT_7_SLASH_COMBINE = "{0}/{1}/{2}/{3}/{4}/{5}/{6}";
	public const string B_TEXT_FMT_8_SLASH_COMBINE = "{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}";
	public const string B_TEXT_FMT_9_SLASH_COMBINE = "{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}";

	public const string B_TEXT_FMT_2_COLON_COMBINE = "{0}:{1}";
	public const string B_TEXT_FMT_3_COLON_COMBINE = "{0}:{1}:{2}";
	public const string B_TEXT_FMT_4_COLON_COMBINE = "{0}:{1}:{2}:{3}";
	public const string B_TEXT_FMT_5_COLON_COMBINE = "{0}:{1}:{2}:{3}:{4}";
	public const string B_TEXT_FMT_6_COLON_COMBINE = "{0}:{1}:{2}:{3}:{4}:{5}";
	public const string B_TEXT_FMT_7_COLON_COMBINE = "{0}:{1}:{2}:{3}:{4}:{5}:{6}";
	public const string B_TEXT_FMT_8_COLON_COMBINE = "{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}";
	public const string B_TEXT_FMT_9_COLON_COMBINE = "{0}:{1}:{2}:{3}:{4}:{5}:{6}:{7}:{8}";

	public const string B_TEXT_FMT_2_SEMI_COLON_COMBINE = "{0};{1}";
	public const string B_TEXT_FMT_3_SEMI_COLON_COMBINE = "{0};{1};{2}";
	public const string B_TEXT_FMT_4_SEMI_COLON_COMBINE = "{0};{1};{2};{3}";
	public const string B_TEXT_FMT_5_SEMI_COLON_COMBINE = "{0};{1};{2};{3};{4}";
	public const string B_TEXT_FMT_6_SEMI_COLON_COMBINE = "{0};{1};{2};{3};{4};{5}";
	public const string B_TEXT_FMT_7_SEMI_COLON_COMBINE = "{0};{1};{2};{3};{4};{5};{6}";
	public const string B_TEXT_FMT_8_SEMI_COLON_COMBINE = "{0};{1};{2};{3};{4};{5};{6};{7}";
	public const string B_TEXT_FMT_9_SEMI_COLON_COMBINE = "{0};{1};{2};{3};{4};{5};{6};{7};{8}";

	public const string B_TEXT_FMT_2_UNDER_SCORE_COMBINE = "{0}_{1}";
	public const string B_TEXT_FMT_3_UNDER_SCORE_COMBINE = "{0}_{1}_{2}";
	public const string B_TEXT_FMT_4_UNDER_SCORE_COMBINE = "{0}_{1}_{2}_{3}";
	public const string B_TEXT_FMT_5_UNDER_SCORE_COMBINE = "{0}_{1}_{2}_{3}_{4}";
	public const string B_TEXT_FMT_6_UNDER_SCORE_COMBINE = "{0}_{1}_{2}_{3}_{4}_{5}";
	public const string B_TEXT_FMT_7_UNDER_SCORE_COMBINE = "{0}_{1}_{2}_{3}_{4}_{5}_{6}";
	public const string B_TEXT_FMT_8_UNDER_SCORE_COMBINE = "{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}";
	public const string B_TEXT_FMT_9_UNDER_SCORE_COMBINE = "{0}_{1}_{2}_{3}_{4}_{5}_{6}_{7}_{8}";

	public const string B_PATH_FMT_1_COMBINE = "{0}/";
	public const string B_PATH_FMT_2_COMBINE = "{0}/{1}/";
	public const string B_PATH_FMT_3_COMBINE = "{0}/{1}/{2}/";
	public const string B_PATH_FMT_4_COMBINE = "{0}/{1}/{2}/{3}/";
	public const string B_PATH_FMT_5_COMBINE = "{0}/{1}/{2}/{3}/{4}/";
	public const string B_PATH_FMT_6_COMBINE = "{0}/{1}/{2}/{3}/{4}/{5}/";
	public const string B_PATH_FMT_7_COMBINE = "{0}/{1}/{2}/{3}/{4}/{5}/{6}/";
	public const string B_PATH_FMT_8_COMBINE = "{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/";
	public const string B_PATH_FMT_9_COMBINE = "{0}/{1}/{2}/{3}/{4}/{5}/{6}/{7}/{8}/";

	public const string B_DATE_T_FMT_HH_MM_SS = "HH:mm:ss";
	public const string B_DATE_T_FMT_YYYY_MM_DD = "yyyyMMdd";
	public const string B_DATE_T_FMT_YYYY_MM_DD_HH_MM_SS = "yyyyMMdd HH:mm:ss";

	public const string B_DATE_T_FMT_SLASH_YYYY_MM_DD = "yyyy/MM/dd";
	public const string B_DATE_T_FMT_SLASH_YYYY_MM_DD_HH_MM_SS = "yyyy/MM/dd HH:mm:ss";
	// 형식 }

	// 접두어 {
	public const string B_PREFIX_EDITOR_SCENE = "ES_";
	public const string B_PREFIX_LEVEL_EDITOR_SCENE = "LES_";

	public const string B_PREFIX_G_SPRITE_ATLAS_01 = "G_SA_01_";
	public const string B_PREFIX_G_SPRITE_ATLAS_02 = "G_SA_02_";
	public const string B_PREFIX_G_SPRITE_ATLAS_03 = "G_SA_03_";
	public const string B_PREFIX_G_SPRITE_ATLAS_04 = "G_SA_04_";
	public const string B_PREFIX_G_SPRITE_ATLAS_05 = "G_SA_05_";
	public const string B_PREFIX_G_SPRITE_ATLAS_06 = "G_SA_06_";
	public const string B_PREFIX_G_SPRITE_ATLAS_07 = "G_SA_07_";
	public const string B_PREFIX_G_SPRITE_ATLAS_08 = "G_SA_08_";
	public const string B_PREFIX_G_SPRITE_ATLAS_09 = "G_SA_09_";

	public const string B_PREFIX_U_SPRITE_ATLAS_01 = "U_SA_01_";
	public const string B_PREFIX_U_SPRITE_ATLAS_02 = "U_SA_02_";
	public const string B_PREFIX_U_SPRITE_ATLAS_03 = "U_SA_03_";
	public const string B_PREFIX_U_SPRITE_ATLAS_04 = "U_SA_04_";
	public const string B_PREFIX_U_SPRITE_ATLAS_05 = "U_SA_05_";
	public const string B_PREFIX_U_SPRITE_ATLAS_06 = "U_SA_06_";
	public const string B_PREFIX_U_SPRITE_ATLAS_07 = "U_SA_07_";
	public const string B_PREFIX_U_SPRITE_ATLAS_08 = "U_SA_08_";
	public const string B_PREFIX_U_SPRITE_ATLAS_09 = "U_SA_09_";

	public const string B_PREFIX_ES_SPRITE_ATLAS_01 = "ES_SA_01_";
	public const string B_PREFIX_ES_SPRITE_ATLAS_02 = "ES_SA_02_";
	public const string B_PREFIX_ES_SPRITE_ATLAS_03 = "ES_SA_03_";
	public const string B_PREFIX_ES_SPRITE_ATLAS_04 = "ES_SA_04_";
	public const string B_PREFIX_ES_SPRITE_ATLAS_05 = "ES_SA_05_";
	public const string B_PREFIX_ES_SPRITE_ATLAS_06 = "ES_SA_06_";
	public const string B_PREFIX_ES_SPRITE_ATLAS_07 = "ES_SA_07_";
	public const string B_PREFIX_ES_SPRITE_ATLAS_08 = "ES_SA_08_";
	public const string B_PREFIX_ES_SPRITE_ATLAS_09 = "ES_SA_09_";
	// 접두어 }

	// 메일
	public const string B_MAIL_MSG_FMT = "App: {0}\nVersion: {1}\nPlatform: {2}\nProcessor: {3}\nGraphics: {4} [{5}]\nOS: {6}\nDeviceID: {7}\n\nPlease enter your inquiry:\n{8}";
	#endregion // 기본

	#region 런타임 상수
	// 씬 이름 {
	public static readonly List<string> B_DEF_SCENE_NAME_LIST_01 = new List<string>() {
		KCDefine.B_SCENE_N_INIT, KCDefine.B_SCENE_N_START, KCDefine.B_SCENE_N_SETUP, KCDefine.B_SCENE_N_AGREE, KCDefine.B_SCENE_N_LATE_SETUP, KCDefine.B_SCENE_N_LOADING, KCDefine.B_SCENE_N_OVERLAY
	};

	public static readonly List<string> B_DEF_SCENE_NAME_LIST_02 = new List<string>() {
		KCDefine.B_SCENE_N_INIT, KCDefine.B_SCENE_N_START, KCDefine.B_SCENE_N_SETUP, KCDefine.B_SCENE_N_AGREE, KCDefine.B_SCENE_N_LATE_SETUP, KCDefine.B_SCENE_N_LOADING, KCDefine.B_SCENE_N_OVERLAY, KCDefine.B_SCENE_N_TITLE, KCDefine.B_SCENE_N_MAIN, KCDefine.B_SCENE_N_GAME
	};
	// 씬 이름 }

	// 기타 {
	public static readonly List<Vector2> B_EMPTY_2D_VEC_LIST = new List<Vector2>();
	public static readonly List<Vector3> B_EMPTY_3D_VEC_LIST = new List<Vector3>();

	public static readonly List<Vector2Int> B_EMPTY_2D_INT_VEC_LIST = new List<Vector2Int>();
	public static readonly List<Vector3Int> B_EMPTY_3D_INT_VEC_LIST = new List<Vector3Int>();

	public static readonly SimpleJSON.JSONArray B_EMPTY_JSON_ARRAY = new SimpleJSON.JSONArray();
	public static readonly SimpleJSON.JSONClass B_EMPTY_JSON_CLASS = new SimpleJSON.JSONClass();
	// 기타 }

	// 색상 {
	public static readonly Color B_LOG_COLOR_INFO = Color.cyan;
	public static readonly Color B_LOG_COLOR_WARNING = new Color(0xff / (float)KCDefine.B_UNIT_NORM_VAL_TO_BYTE, 0xa5 / (float)KCDefine.B_UNIT_NORM_VAL_TO_BYTE, 0.0f, 1.0f);
	public static readonly Color B_LOG_COLOR_ERROR = Color.red;

	public static readonly Color B_LOG_COLOR_PLUGIN = Color.yellow;
	public static readonly Color B_LOG_COLOR_PLATFORM_INFO = Color.magenta;
	// 색상 }

	// 앵커 {
	public static readonly Vector3 B_ANCHOR_UP_LEFT = new Vector3(0.0f, 1.0f, 0.0f);
	public static readonly Vector3 B_ANCHOR_UP_CENTER = new Vector3(0.5f, 1.0f, 0.0f);
	public static readonly Vector3 B_ANCHOR_UP_RIGHT = new Vector3(1.0f, 1.0f, 0.0f);

	public static readonly Vector3 B_ANCHOR_DOWN_LEFT = new Vector3(0.0f, 0.0f, 0.0f);
	public static readonly Vector3 B_ANCHOR_DOWN_CENTER = new Vector3(0.5f, 0.0f, 0.0f);
	public static readonly Vector3 B_ANCHOR_DOWN_RIGHT = new Vector3(1.0f, 0.0f, 0.0f);

	public static readonly Vector3 B_ANCHOR_MID_LEFT = new Vector3(0.0f, 0.5f, 0.0f);
	public static readonly Vector3 B_ANCHOR_MID_CENTER = new Vector3(0.5f, 0.5f, 0.0f);
	public static readonly Vector3 B_ANCHOR_MID_RIGHT = new Vector3(1.0f, 0.5f, 0.0f);
	// 앵커 }

	// 위치 {
	public static readonly Vector3 B_POS_UP_LEFT = new Vector3(0.0f, KCDefine.B_SCREEN_HEIGHT, 0.0f);
	public static readonly Vector3 B_POS_UP_CENTER = new Vector3(KCDefine.B_SCREEN_WIDTH / 2.0f, KCDefine.B_SCREEN_HEIGHT, 0.0f);
	public static readonly Vector3 B_POS_UP_RIGHT = new Vector3(KCDefine.B_SCREEN_WIDTH, KCDefine.B_SCREEN_HEIGHT, 0.0f);

	public static readonly Vector3 B_POS_DOWN_LEFT = Vector3.zero;
	public static readonly Vector3 B_POS_DOWN_CENTER = new Vector3(KCDefine.B_SCREEN_WIDTH / 2.0f, 0.0f, 0.0f);
	public static readonly Vector3 B_POS_DOWN_RIGHT = new Vector3(KCDefine.B_SCREEN_WIDTH, 0.0f, 0.0f);

	public static readonly Vector3 B_POS_MID_LEFT = new Vector3(0.0f, KCDefine.B_SCREEN_HEIGHT / 2.0f, 0.0f);
	public static readonly Vector3 B_POS_MID_CENTER = new Vector3(KCDefine.B_SCREEN_WIDTH / 2.0f, KCDefine.B_SCREEN_HEIGHT / 2.0f, 0.0f);
	public static readonly Vector3 B_POS_MID_RIGHT = new Vector3(KCDefine.B_SCREEN_WIDTH, KCDefine.B_SCREEN_HEIGHT / 2.0f, 0.0f);

	public static readonly Vector3 B_POS_POPUP = Vector3.zero;
	public static readonly Vector3 B_POS_TOUCH_RESPONDER = Vector3.zero;
	// 위치 }

	// 인덱스 {
	public static readonly Vector2Int B_IDX_INVALID_2D = new Vector2Int(KCDefine.B_IDX_INVALID, KCDefine.B_IDX_INVALID);
	public static readonly Vector3Int B_IDX_INVALID_3D = new Vector3Int(KCDefine.B_IDX_INVALID, KCDefine.B_IDX_INVALID, KCDefine.B_IDX_INVALID);

	public static readonly List<(Vector2Int, Vector2Int)> B_IDX_OFFSET_INFO_LIST_2D = new List<(Vector2Int, Vector2Int)>() {
		(Vector2Int.down, Vector2Int.up), (Vector2Int.up, Vector2Int.down), (Vector2Int.right, Vector2Int.left), (Vector2Int.left, Vector2Int.right), (Vector2Int.down, Vector2Int.left), (Vector2Int.down, Vector2Int.right), (Vector2Int.up, Vector2Int.left), (Vector2Int.up, Vector2Int.right), (Vector2Int.right, Vector2Int.up), (Vector2Int.right, Vector2Int.down), (Vector2Int.left, Vector2Int.up), (Vector2Int.left, Vector2Int.down)
	};
	// 인덱스 }

	// 토큰 {
	public static readonly Dictionary<string, decimal> B_NUM_TOKEN_DICT_KOREAN = new Dictionary<string, decimal>() {
		["만"] = 10000m,
		["억"] = 100000000m,
		["조"] = 1000000000000m,
		["경"] = 10000000000000000m,
		["해"] = 100000000000000000000m,
		["자"] = 1000000000000000000000000m,
		["양"] = 10000000000000000000000000000m
	};

	public static readonly Dictionary<string, decimal> B_NUM_TOKEN_DICT_ENGLISH = new Dictionary<string, decimal>() {
		["K"] = 1000m,
		["M"] = 1000000m,
		["B"] = 1000000000m,
		["T"] = 1000000000000m,
		["q"] = 1000000000000000m,
		["Q"] = 1000000000000000000m,
		["s"] = 1000000000000000000000m,
		["S"] = 1000000000000000000000000m,
		["O"] = 1000000000000000000000000000m
	};
	// 토큰 }

	// 국가 코드
	public static readonly List<string> B_EU_COUNTRY_CODE_LIST = new List<string>() {
		"BE", "BG", "CZ", "DK", "DE", "EE", "IE", "GR", "ES", "FR", "HR", "IT", "CY", "LV", "LT", "LU", "HU", "MT", "NL", "AT", "PL", "PT", "RO", "SI", "SK", "FI", "SE"
	};

	// 경로 {
#if UNITY_IOS || UNITY_ANDROID
	public static readonly string B_DIR_P_WRITABLE = $"{Application.persistentDataPath}/{Application.identifier}/";
#else
	public static readonly string B_DIR_P_WRITABLE = $"{Application.persistentDataPath}/PersistentDatas/";
#endif // #if UNITY_IOS || UNITY_ANDROID

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
	public static readonly string B_ABS_DIR_P_EXTERNAL_DATAS = $"{Application.dataPath}/../{KCDefine.B_DIR_N_EXTERNAL_DATAS}/";

#if UNITY_STANDALONE_WIN
	public static readonly string B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS = $"{Application.dataPath}/../{KCDefine.B_DIR_N_EXTERNAL_DATAS}/";
#else
	public static readonly string B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS = $"{Application.dataPath}/../../{KCDefine.B_DIR_N_EXTERNAL_DATAS}/";
#endif // #if UNITY_STANDALONE_WIN
#else
	public static readonly string B_ABS_DIR_P_EXTERNAL_DATAS = KCDefine.B_DIR_P_WRITABLE;
	public static readonly string B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS = KCDefine.B_DIR_P_WRITABLE;
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
	// 경로 }
	#endregion // 런타임 상수
}

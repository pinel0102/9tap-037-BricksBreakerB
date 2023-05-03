using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

#if UNITY_IOS
using UnityEngine.iOS;
#endif // #if UNITY_IOS

#if ADS_MODULE_ENABLE && ADMOB_ADS_ENABLE
using GoogleMobileAds.Api;
#endif // #if ADS_MODULE_ENABLE && ADMOB_ADS_ENABLE

#if NOTI_MODULE_ENABLE
#if UNITY_IOS
using Unity.Notifications.iOS;
#elif UNITY_ANDROID
using Unity.Notifications.Android;
#endif // #if UNITY_IOS
#endif // #if NOTI_MODULE_ENABLE

/** 유틸리티 상수 */
public static partial class KCDefine {
	#region 기본
	// 개수 {
	public const int U_MAX_NUM_LAYERS = 32;
	public const int U_MAX_NUM_FX_SNDS = 128;
	public const int U_MAX_NUM_GOOGLE_SHEET_ROWS = 1000;

	public const int U_MAX_NUM_LEVEL_INFOS = 9999;
	public const int U_MAX_NUM_STAGE_INFOS = 999;
	public const int U_MAX_NUM_CHAPTER_INFOS = 99;
	// 개수 }

	// 크기 {
	public const int U_SIZE_OBJS_POOL_01 = 50;
	public const int U_SIZE_OBJS_POOL_02 = 150;
	public const int U_SIZE_OBJS_POOL_03 = 250;

	public const int U_SIZE_DOTWEEN_ANI = byte.MaxValue;
	public const int U_SIZE_DOTWEEN_SEQUENCE_ANI = sbyte.MaxValue;

	public const int U_DEF_SIZE_FONT = 28;
	public const int U_DEF_MIN_SIZE_FONT = 14;
	public const int U_DEF_MAX_SIZE_FONT = 18;
	// 크기 }

	// 길이
	public const float U_MAX_PERCENT_ASYNC_OPERATION = 0.9f;

	// 단위
	public const float U_UNIT_TABLET_INCHES = 6.5f;
	public const float U_UNIT_SCROLL_SENSITIVITY = 250.0f;

	// 세기
	public const float U_INTENSITY_VIBRATE = 1.0f;

	// 깊이
	public const float U_DEPTH_MAIN_CAMERA = 0.0f;

	// 거리
	public const float U_DISTANCE_CAMERA_FAR_PLANE = 25000.0f * KCDefine.B_UNIT_SCALE;
	public const float U_DISTANCE_CAMERA_NEAR_PLANE = 0.1f;

	// 비율 {
	public const float U_SCALE_POPUP = 1.0f;
	public const float U_MIN_SCALE_POPUP = 0.001f;

	public const float U_SCALE_TOUCH = 1.05f;
	public const float U_SCALE_PAGE_SCROLL = 0.35f;
	public const float U_SCALE_INDICATOR_IMG = 0.25f;
	// 비율 }

	// 시간 {
	public const float U_DELAY_DEF = 0.15f;
	public const float U_DELAY_INIT = 0.15f;
	public const float U_DELAY_POPUP_SHOW_ANI = KCDefine.B_DELTA_T_INTERMEDIATE;
	public const float U_MIN_DELAY_FX_SNDS = 0.05f;

	public const float U_DURATION_ANI = 0.15f;
	public const float U_DURATION_POPUP_ANI = 0.25f;
	public const float U_DURATION_LIGHT_VIBRATE = 0.05f;
	public const float U_DURATION_MEDIUM_VIBRATE = 0.1f;
	public const float U_DURATION_HEAVY_VIBRATE = 0.15f;

	public const float U_TIMEOUT_ASYNC_TASK = 30.0f;
	public const float U_TIMEOUT_NETWORK_CONNECTION = 30.0f;
	public const float U_DELTA_T_SCHEDULE_M_CALLBACK = 0.15f;
	// 시간 }

	// 레이어
	public const int U_LAYER_DEF = 0;
	public const int U_LAYER_TRANSPARENT_FX = 1;
	public const int U_LAYER_IGNORE_RAYCAST = 2;
	public const int U_LAYER_WATER = 4;
	public const int U_LAYER_UIS = 5;
	public const int U_LAYER_CUSTOM = 11;

	// 정렬 순서 {
	public const int U_SORTING_O_ABS = sbyte.MaxValue * 50;
	public const int U_SORTING_O_DEF = sbyte.MaxValue * 0;

	public const int U_SORTING_O_TOP = sbyte.MaxValue * 30;
	public const int U_SORTING_O_TOPMOST = sbyte.MaxValue * 40;

	public const int U_SORTING_O_FOREGROUND = sbyte.MaxValue * 10;
	public const int U_SORTING_O_OVERGROUND = sbyte.MaxValue * 20;

	public const int U_SORTING_O_BACKGROUND = sbyte.MaxValue * -10;
	public const int U_SORTING_O_UNDERGROUND = sbyte.MaxValue * -20;

	public const int U_SORTING_O_UIS = sbyte.MaxValue * 0;
	public const int U_SORTING_O_OVERLAY_UIS = sbyte.MaxValue * 1;

	public const int U_SORTING_O_SCREEN_POPUP_UIS = sbyte.MaxValue * 20;
	public const int U_SORTING_O_SCREEN_TOPMOST_UIS = sbyte.MaxValue * 30;
	public const int U_SORTING_O_SCREEN_ABS_UIS = sbyte.MaxValue * 40;
	public const int U_SORTING_O_SCREEN_BLIND_UIS = sbyte.MaxValue * 50;
	public const int U_SORTING_O_SCREEN_DEBUG_UIS = sbyte.MaxValue * 60;
	// 정렬 순서 }

	// 애니메이션
	public const Ease U_EASE_DEF = Ease.OutQuad;
	public const Ease U_EASE_UNSET = Ease.Unset;
	public const Ease U_EASE_LINEAR = Ease.Linear;

	// 형식 {
#if UNITY_IOS
	public const string U_FMT_STORE_URL = "https://itunes.apple.com/app/id{0}";
	public const string U_FMT_MORE_APPS_URL = "https://apps.apple.com/us/developer/ninetap/id{0}#see-all/i-phonei-pad-apps";
#else
	public const string U_FMT_STORE_URL = "https://play.google.com/store/apps/details?id={0}";
	public const string U_FMT_MORE_APPS_URL = "https://play.google.com/store/apps/developer?id={0}";
#endif // #if UNITY_IOS
	// 형식 }

	// 식별자 {
	public const string U_ADS_ID_TEST_DEVICE = "TestDevice";

	public const string U_KEY_DEVICE_CMD = "Cmd";
	public const string U_KEY_DEVICE_MSG = "Msg";

	public const string U_KEY_VER = "Ver";
	public const string U_KEY_NAME = "Name";
	public const string U_KEY_DESC = "Desc";
	public const string U_KEY_RATE = "Rate";
	public const string U_KEY_REPLACE = "Replace";
	public const string U_KEY_RES_PATH = "ResPath";
	public const string U_KEY_REWARD_QUALITY = "RewardQuality";

	public const string U_KEY_NOEX_T = "NOEX_T";
	public const string U_KEY_NOEX_ST = "NOEX_ST";
	public const string U_KEY_NOEX_KT = "NOEX_KT";
	public const string U_KEY_NOEX_SKT = "NOEX_SKT";
	public const string U_KEY_NOEX_DSKT = "NOEX_DSKT";

	public const string U_KEY_FX = "FX";
	public const string U_KEY_CALC = "Calc";
	public const string U_KEY_TUTORIAL = "Tutorial";

	public const string U_KEY_LEVEL_EPISODE = "LevelEpisode";
	public const string U_KEY_STAGE_EPISODE = "StageEpisode";
	public const string U_KEY_CHAPTER_EPISODE = "ChapterEpisode";

	public const string U_KEY_ID = "ID";
	public const string U_KEY_PREV_ID = "PrevID";
	public const string U_KEY_NEXT_ID = "NextID";

	public const string U_KEY_SND = "Snd";
	public const string U_KEY_FONT = "Font";
	public const string U_KEY_SIZE = "Size";
	public const string U_KEY_IMG = "Image";
	public const string U_KEY_TEX = "Texture";
	public const string U_KEY_SPRITE = "Sprite";

	public const string U_KEY_PRODUCT_IDX = "ProductIdx";
	public const string U_KEY_MAX_APPLY_TIMES = "MaxApplyTimes";
	public const string U_KEY_NUM_SUB_EPISODES = "NumSubEpisodes";
	public const string U_KEY_MAX_NUM_ENEMY_OBJS = "MaxNumEnemyObjs";

	public const string U_KEY_VAL_INFO = "ValInfo";
	public const string U_KEY_TIME_INFO = "TimeInfo";

	public const string U_KEY_DIFFICULTY = "Difficulty";
	public const string U_KEY_PRODUCT_TYPE = "ProductType";
	public const string U_KEY_PURCHASE_TYPE = "PurchaseType";
	public const string U_KEY_EPISODE_KINDS = "EpisodeKinds";
	public const string U_KEY_ABILITY_VAL_TYPE = "AbilityValType";

	public const string U_KEY_CALC_KINDS = "CalcKinds";
	public const string U_KEY_PREV_CALC_KINDS = "PrevCalcKinds";
	public const string U_KEY_NEXT_CALC_KINDS = "NextCalcKinds";

	public const string U_KEY_REWARD_KINDS = "RewardKinds";
	public const string U_KEY_PREV_REWARD_KINDS = "PrevRewardKinds";
	public const string U_KEY_NEXT_REWARD_KINDS = "NextRewardKinds";

	public const string U_KEY_ITEM_KINDS = "ItemKinds";
	public const string U_KEY_PREV_ITEM_KINDS = "PrevItemKinds";
	public const string U_KEY_NEXT_ITEM_KINDS = "NextItemKinds";

	public const string U_KEY_MISSION_KINDS = "MissionKinds";
	public const string U_KEY_PREV_MISSION_KINDS = "PrevMissionKinds";
	public const string U_KEY_NEXT_MISSION_KINDS = "NextMissionKinds";

	public const string U_KEY_FX_KINDS = "FXKinds";
	public const string U_KEY_PREV_FX_KINDS = "PrevFXKinds";
	public const string U_KEY_NEXT_FX_KINDS = "NextFXKinds";

	public const string U_KEY_SKILL_KINDS = "SkillKinds";
	public const string U_KEY_PREV_SKILL_KINDS = "PrevSkillKinds";
	public const string U_KEY_NEXT_SKILL_KINDS = "NextSkillKinds";
	public const string U_KEY_SKILL_APPLY_KINDS = "SkillApplyKinds";
	public const string U_KEY_ACTION_SKILL_KINDS = "ActionSkillKinds";

	public const string U_KEY_ABILITY_KINDS = "AbilityKinds";
	public const string U_KEY_PREV_ABILITY_KINDS = "PrevAbilityKinds";
	public const string U_KEY_NEXT_ABILITY_KINDS = "NextAbilityKinds";

	public const string U_KEY_OBJ_KINDS = "ObjKinds";
	public const string U_KEY_PREV_OBJ_KINDS = "PrevObjKinds";
	public const string U_KEY_NEXT_OBJ_KINDS = "NextObjKinds";

	public const string U_KEY_TUTORIAL_KINDS = "TutorialKinds";
	public const string U_KEY_PREV_TUTORIAL_KINDS = "PrevTutorialKinds";
	public const string U_KEY_NEXT_TUTORIAL_KINDS = "NextTutorialKinds";

	public const string U_KEY_RES_KINDS = "ResKinds";
	public const string U_KEY_PREV_RES_KINDS = "PrevResKinds";
	public const string U_KEY_NEXT_RES_KINDS = "NextResKinds";

	public const string U_KEY_PRODUCT_KINDS = "ProductKinds";
	public const string U_KEY_PREV_PRODUCT_KINDS = "PrevProductKinds";
	public const string U_KEY_NEXT_PRODUCT_KINDS = "NextProductKinds";

	public const string U_KEY_FMT_ID = "ID_{0:00}";
	public const string U_KEY_FMT_PREV_ID = "PrevID_{0:00}";
	public const string U_KEY_FMT_NEXT_ID = "NextID_{0:00}";

	public const string U_KEY_FMT_FLAGS = "Flags_{0:00}";
	public const string U_KEY_FMT_PRICE = "Price_{0:00}";
	public const string U_KEY_FMT_FX_KINDS = "FXKinds_{0:00}";
	public const string U_KEY_FMT_OBJ_KINDS = "ObjKinds_{0:00}";
    public const string U_KEY_FMT_EXTRA_OBJ_KINDS = "ExtraObjKinds_{0:00}";
	public const string U_KEY_FMT_RES_KINDS = "ResKinds_{0:00}";
	public const string U_KEY_FMT_ITEM_KINDS = "ItemKinds_{0:00}";
	public const string U_KEY_FMT_SKILL_KINDS = "SkillKinds_{0:00}";
	public const string U_KEY_FMT_REWARD_KINDS = "RewardKinds_{0:00}";

	public const string U_KEY_FMT_STRS = "Str_{0:00}";
	public const string U_KEY_FMT_TUTORIAL_MSG = "TUTORIAL_MSG_{0:00}_{1:00}";
	public const string U_KEY_FMT_RECORD_VAL_INFO = "RecordValInfo_{0:00}";

	public const string U_KEY_FMT_PAY_TARGET_INFO = "PayTargetInfo_{0:00}";
	public const string U_KEY_FMT_ACQUIRE_TARGET_INFO = "AcquireTargetInfo_{0:00}";

	public const string U_KEY_FMT_CLEAR_TARGET_INFO = "ClearTargetInfo_{0:00}";
	public const string U_KEY_FMT_UNLOCK_TARGET_INFO = "UnlockTargetInfo_{0:00}";

	public const string U_KEY_FMT_DROP_ITEM_TARGET_INFO = "DropItemTargetInfo_{0:00}";
	public const string U_KEY_FMT_EQUIP_ITEM_TARGET_INFO = "EquipItemTargetInfo_{0:00}";
	public const string U_KEY_FMT_ATTACH_ITEM_TARGET_INFO = "AttachItemTargetInfo_{0:00}";

	public const string U_KEY_FMT_SKILL_TARGET_INFO = "SkillTargetInfo_{0:00}";
	public const string U_KEY_FMT_PLAYER_OBJ_TARGET_INFO = "PlayerObjTargetInfo_{0:00}";
	public const string U_KEY_FMT_ENEMY_OBJ_TARGET_INFO = "EnemyObjTargetInfo_{0:00}";

	public const string U_KEY_FMT_ABILITY_TARGET_INFO = "AbilityTargetInfo_{0:00}";
	public const string U_KEY_FMT_EXTRA_ABILITY_TARGET_INFO = "ExtraAbilityTargetInfo_{0:00}";

	public const string U_KEY_SERVICES_M_UPDATE_APPLE_LOGIN_STATE_CALLBACK = "ServicesMUpdateAppleLoginStateCallback";
	public const string U_KEY_SERVICES_M_UPDATE_FAIL_APPLE_LOGIN_STATE_CALLBACK = "ServicesMUpdateFailAppleLoginStateCallback";

	public const string U_KEY_SERVICES_M_LOGIN_WITH_APPLE_CALLBACK = "ServicesMLoginWithAppleCallback";
	public const string U_KEY_SERVICES_M_LOGIN_FAIL_WITH_APPLE_CALLBACK = "ServicesMLoginFailWithAppleCallback";
	public const string U_KEY_SERVICES_M_LOGOUT_WITH_APPLE_CALLBACK = "ServicesMLogoutWithAppleCallback";
	// 식별자 }

	// 이름 {
	public const string U_OBJ_N_SCENE_UIS_ROOT = "UIsRoot";
	public const string U_OBJ_N_SCENE_UIS_BASE = "Canvas";
	public const string U_OBJ_N_SCENE_UIS = "UIs";
	public const string U_OBJ_N_SCENE_TEST_UIS = "TestUIs";
	public const string U_OBJ_N_SCENE_PIVOT_UIS = "PivotUIs";
	public const string U_OBJ_N_SCENE_ANCHOR_UIS = "AnchorUIs";
	public const string U_OBJ_N_SCENE_MID_EDITOR_UIS = "MID_EDITOR_UIS";
	public const string U_OBJ_N_SCENE_CORNER_ANCHOR_UIS = "CornerAnchorUIs";
	public const string U_OBJ_N_SCENE_DESIGN_RESOLUTION_GUIDE_UIS = "DesignResolutionGuideUIs";

	public const string U_OBJ_N_SCENE_UP_UIS = "UpUIs";
	public const string U_OBJ_N_SCENE_DOWN_UIS = "DownUIs";
	public const string U_OBJ_N_SCENE_LEFT_UIS = "LeftUIs";
	public const string U_OBJ_N_SCENE_RIGHT_UIS = "RightUIs";

	public const string U_OBJ_N_SCENE_UP_LEFT_UIS = "UpLeftUIs";
	public const string U_OBJ_N_SCENE_UP_RIGHT_UIS = "UpRightUIs";
	public const string U_OBJ_N_SCENE_DOWN_LEFT_UIS = "DownLeftUIs";
	public const string U_OBJ_N_SCENE_DOWN_RIGHT_UIS = "DownRightUIs";

	public const string U_OBJ_N_SCENE_POPUP_UIS = "PopupUIs";
	public const string U_OBJ_N_SCENE_TOPMOST_UIS = "TopmostUIs";
	public const string U_OBJ_N_SCENE_ABS_UIS = "AbsUIs";

	public const string U_OBJ_N_SCENE_BASE = "Base";
	public const string U_OBJ_N_SCENE_OBJS_ROOT = "ObjsRoot";
	public const string U_OBJ_N_SCENE_OBJS = "Objs";
	public const string U_OBJ_N_SCENE_PIVOT_OBJS = "PivotObjs";
	public const string U_OBJ_N_SCENE_ANCHOR_OBJS = "AnchorObjs";
	public const string U_OBJ_N_SCENE_STATIC_OBJS = "StaticObjs";

	public const string U_OBJ_N_SCENE_EVENT_SYSTEM = "EventSystem";
	public const string U_OBJ_N_SCENE_ADDITIONAL_LIGHTS = "AdditionalLights";
	public const string U_OBJ_N_SCENE_ADDITIONAL_CAMERAS = "AdditionalCameras";
	public const string U_OBJ_N_SCENE_REFLECTION_PROBES = "ReflectionProbes";
	public const string U_OBJ_N_SCENE_LIGHT_PROBE_GROUPS = "LightProbeGroups";

	public const string U_OBJ_N_SCENE_UP_OBJS = "UpObjs";
	public const string U_OBJ_N_SCENE_DOWN_OBJS = "DownObjs";
	public const string U_OBJ_N_SCENE_LEFT_OBJS = "LeftObjs";
	public const string U_OBJ_N_SCENE_RIGHT_OBJS = "RightObjs";

	public const string U_OBJ_N_SCENE_MAIN_LIGHT = "MainLight";
	public const string U_OBJ_N_SCENE_MAIN_CAMERA = "MainCamera";
	public const string U_OBJ_N_SCENE_MANAGER = "SceneManager";

	public const string U_OBJ_N_SCREEN_DEBUG_UIS = "ScreenDebugUIs";
	public const string U_OBJ_N_SCREEN_BLIND_UIS = "ScreenBlindUIs";
	public const string U_OBJ_N_SCREEN_POPUP_UIS = "ScreenPopupUIs";
	public const string U_OBJ_N_SCREEN_TOPMOST_UIS = "ScreenTopmostUIs";
	public const string U_OBJ_N_SCREEN_ABS_UIS = "ScreenAbsUIs";

	public const string U_OBJ_N_VIEWPORT = "VIEWPORT";
	public const string U_OBJ_N_CONTENTS = "CONTENTS";
	public const string U_OBJ_N_CONTENTS_UIS = "CONTENTS_UIS";
	public const string U_OBJ_N_AB_T_UIS_SET_UIS = "SET_UIS";

	public const string U_OBJ_N_UP_BLIND_IMG = "UP_BLIND_IMG";
	public const string U_OBJ_N_DOWN_BLIND_IMG = "DOWN_BLIND_IMG";
	public const string U_OBJ_N_LEFT_BLIND_IMG = "LEFT_BLIND_IMG";
	public const string U_OBJ_N_RIGHT_BLIND_IMG = "RIGHT_BLIND_IMG";

	public const string U_OBJ_N_DESC_UIS = "DESC_UIS";
	public const string U_OBJ_N_PAGE_UIS = "PAGE_UIS";
	public const string U_OBJ_N_PAGINATION = "PAGINATION";

	public const string U_OBJ_N_BLIND_UIS = "BLIND_UIS";
	public const string U_OBJ_N_CLEAR_UIS = "CLEAR_UIS";
	public const string U_OBJ_N_CLEAR_FAIL_UIS = "CLEAR_FAIL_UIS";

	public const string U_OBJ_N_LOCK_UIS = "LOCK_UIS";
	public const string U_OBJ_N_OPEN_UIS = "OPEN_UIS";

	public const string U_OBJ_N_GAUGE_UIS = "GAUGE_UIS";
	public const string U_OBJ_N_HP_GAUGE_UIS = "HP_GAUGE_UIS";
	public const string U_OBJ_N_MP_GAUGE_UIS = "MP_GAUGE_UIS";
	public const string U_OBJ_N_EXP_GAUGE_UIS = "EXP_GAUGE_UIS";

	public const string U_OBJ_N_LOGIN_UIS = "LOGIN_UIS";
	public const string U_OBJ_N_LOGOUT_UIS = "LOGOUT_UIS";

	public const string U_OBJ_N_ADS_PRICE_UIS = "ADS_PRICE_UIS";
	public const string U_OBJ_N_PURCHASE_PRICE_UIS = "PURCHASE_PRICE_UIS";

	public const string U_OBJ_N_TOP_MENU_UIS = "TOP_MENU_UIS";
	public const string U_OBJ_N_BOTTOM_MENU_UIS = "BOTTOM_MENU_UIS";

	public const string U_OBJ_N_MSG_TEXT = "MSG_TEXT";
	public const string U_OBJ_N_VAL_TEXT = "VAL_TEXT";
	public const string U_OBJ_N_PAGE_TEXT = "PAGE_TEXT";
	public const string U_OBJ_N_TITLE_TEXT = "TITLE_TEXT";

	public const string U_OBJ_N_NAME_TEXT = "NAME_TEXT";
	public const string U_OBJ_N_DESC_TEXT = "DESC_TEXT";

	public const string U_OBJ_N_LV_TEXT = "LV_TEXT";
	public const string U_OBJ_N_HP_TEXT = "HP_TEXT";
	public const string U_OBJ_N_MP_TEXT = "MP_TEXT";
	public const string U_OBJ_N_EXP_TEXT = "EXP_TEXT";

	public const string U_OBJ_N_LEVEL_TEXT = "LEVEL_TEXT";
	public const string U_OBJ_N_STAGE_TEXT = "STAGE_TEXT";
	public const string U_OBJ_N_CHAPTER_TEXT = "CHAPTER_TEXT";

	public const string U_OBJ_N_VER_TEXT = "VER_TEXT";
	public const string U_OBJ_N_NUM_TEXT = "NUM_TEXT";
	public const string U_OBJ_N_TIME_TEXT = "TIME_TEXT";
	public const string U_OBJ_N_TIMES_TEXT = "TIMES_TEXT";
	public const string U_OBJ_N_PRICE_TEXT = "PRICE_TEXT";
	public const string U_OBJ_N_STATE_TEXT = "STATE_TEXT";
	public const string U_OBJ_N_LOADING_TEXT = "LOADING_TEXT";
	public const string U_OBJ_N_COUNTDOWN_TEXT = "COUNTDOWN_TEXT";

	public const string U_OBJ_N_RECORD_TEXT = "RECORD_TEXT";
	public const string U_OBJ_N_BEST_RECORD_TEXT = "BEST_RECORD_TEXT";

	public const string U_OBJ_N_NUM_COINS_TEXT = "NUM_COINS_TEXT";
	public const string U_OBJ_N_NUM_MARKS_TEXT = "NUM_MARKS_TEXT";
	public const string U_OBJ_N_NUM_MARKS_STATE_TEXT = "NUM_MARKS_STATE_TEXT";

	public const string U_OBJ_N_BG_IMG = "BG_IMG";
    public const string U_OBJ_N_GLOW_IMG = "GLOW_IMG";
	public const string U_OBJ_N_COINS_IMG = "COINS_IMG";
	public const string U_OBJ_N_BLIND_IMG = "BLIND_IMG";
	public const string U_OBJ_N_SPLASH_IMG = "SPLASH_IMG";
	public const string U_OBJ_N_FOREGROUND_IMG = "FOREGROUND_IMG";

	public const string U_OBJ_N_MARK_IMG = "MARK_IMG";
	public const string U_OBJ_N_CHECK_IMG = "CHECK_IMG";
	public const string U_OBJ_N_RIBBON_IMG = "RIBBON_IMG";
	public const string U_OBJ_N_PERCENT_IMG = "PERCENT_IMG";
	public const string U_OBJ_N_COMPLETE_IMG = "COMPLETE_IMG";

	public const string U_OBJ_N_LOCK_IMG = "LOCK_IMG";
	public const string U_OBJ_N_ICON_IMG = "ICON_IMG";
	public const string U_OBJ_N_ITEM_IMG = "ITEM_IMG";
	public const string U_OBJ_N_REWARD_IMG = "REWARD_IMG";

	public const string U_OBJ_N_OK_BTN = "OK_BTN";
	public const string U_OBJ_N_BACK_BTN = "BACK_BTN";
	public const string U_OBJ_N_CANCEL_BTN = "CANCEL_BTN";
	public const string U_OBJ_N_OPEN_BTN = "OPEN_BTN";
	public const string U_OBJ_N_CLOSE_BTN = "CLOSE_BTN";

	public const string U_OBJ_N_A_SET_BTN = "A_SET_BTN";
	public const string U_OBJ_N_B_SET_BTN = "B_SET_BTN";

	public const string U_OBJ_N_AGREE_BTN = "AGREE_BTN";
	public const string U_OBJ_N_PRIVACY_BTN = "PRIVACY_BTN";
	public const string U_OBJ_N_SERVICES_BTN = "SERVICES_BTN";

	public const string U_OBJ_N_LOGIN_BTN = "LOGIN_BTN";
	public const string U_OBJ_N_LOGOUT_BTN = "LOGOUT_BTN";

	public const string U_OBJ_N_LOAD_BTN = "LOAD_BTN";
	public const string U_OBJ_N_SAVE_BTN = "SAVE_BTN";
	public const string U_OBJ_N_PLAY_BTN = "PLAY_BTN";
	public const string U_OBJ_N_COPY_BTN = "COPY_BTN";
	public const string U_OBJ_N_STORE_BTN = "STORE_BTN";
	public const string U_OBJ_N_SETTINGS_BTN = "SETTINGS_BTN";

	public const string U_OBJ_N_PREV_BTN = "PREV_BTN";
	public const string U_OBJ_N_NEXT_BTN = "NEXT_BTN";
	public const string U_OBJ_N_RETRY_BTN = "RETRY_BTN";
	public const string U_OBJ_N_LEAVE_BTN = "LEAVE_BTN";
	public const string U_OBJ_N_CONTINUE_BTN = "CONTINUE_BTN";

	public const string U_OBJ_N_ADS_BTN = "ADS_BTN";
	public const string U_OBJ_N_PAUSE_BTN = "PAUSE_BTN";
	public const string U_OBJ_N_SHARE_BTN = "SHARE_BTN";
	public const string U_OBJ_N_ACQUIRE_BTN = "ACQUIRE_BTN";
	public const string U_OBJ_N_RANKING_BTN = "RANKING_BTN";
	public const string U_OBJ_N_PURCHASE_BTN = "PURCHASE_BTN";
	public const string U_OBJ_N_RESTORE_BTN = "RESTORE_BTN";
	public const string U_OBJ_N_REMOVE_ADS_BTN = "REMOVE_ADS_BTN";

	public const string U_OBJ_N_SND_BTN = "SND_BTN";
	public const string U_OBJ_N_BG_SND_BTN = "BG_SND_BTN";
	public const string U_OBJ_N_FX_SNDS_BTN = "FX_SNDS_BTN";
	public const string U_OBJ_N_VIBRATE_BTN = "VIBRATE_BTN";
	public const string U_OBJ_N_NOTI_BTN = "NOTI_BTN";
	public const string U_OBJ_N_REVIEW_BTN = "REVIEW_BTN";
	public const string U_OBJ_N_SUPPORTS_BTN = "SUPPORTS_BTN";
    public const string U_OBJ_N_MORE_APPS_BTN = "MORE_APPS_BTN";
	public const string U_OBJ_N_SYNC_BTN = "SYNC_BTN";
	public const string U_OBJ_N_LOCALIZE_BTN = "LOCALIZE_BTN";

	public const string U_OBJ_N_POPUP = "POPUP";
	public const string U_OBJ_N_GAME_OBJ = "GAME_OBJ";
	public const string U_OBJ_N_ALERT_POPUP = "ALERT_POPUP";
	public const string U_OBJ_N_FOCUS_POPUP = "FOCUS_POPUP";

	public const string U_OBJ_N_TEXT = "TEXT";
	public const string U_OBJ_N_LOCALIZE_TEXT = "LOCALIZE_TEXT";

	public const string U_OBJ_N_TMP_TEXT = "TMP_TEXT";
	public const string U_OBJ_N_TMP_TEXT_MESH = "TMP_TEXT_MESH";
	public const string U_OBJ_N_TMP_LOCALIZE_TEXT = "TMP_LOCALIZE_TEXT";

	public const string U_OBJ_N_IMG = "IMG";
	public const string U_OBJ_N_MASK_IMG = "MASK_IMG";
	public const string U_OBJ_N_FOCUS_IMG = "FOCUS_IMG";
	public const string U_OBJ_N_GAUGE_IMG = "GAUGE_IMG";

	public const string U_OBJ_N_TEXT_BTN = "TEXT_BTN";
	public const string U_OBJ_N_TEXT_SCALE_BTN = "TEXT_SCALE_BTN";

	public const string U_OBJ_N_TMP_TEXT_BTN = "TMP_TEXT_BTN";
	public const string U_OBJ_N_TMP_TEXT_SCALE_BTN = "TMP_TEXT_SCALE_BTN";

	public const string U_OBJ_N_LOCALIZE_TEXT_BTN = "LOCALIZE_TEXT_BTN";
	public const string U_OBJ_N_LOCALIZE_TEXT_SCALE_BTN = "LOCALIZE_TEXT_SCALE_BTN";

	public const string U_OBJ_N_TMP_LOCALIZE_TEXT_BTN = "TMP_LOCALIZE_TEXT_BTN";
	public const string U_OBJ_N_TMP_LOCALIZE_TEXT_SCALE_BTN = "TMP_LOCALIZE_TEXT_SCALE_BTN";

	public const string U_OBJ_N_IMG_BTN = "IMG_BTN";
	public const string U_OBJ_N_IMG_SCALE_BTN = "IMG_SCALE_BTN";

	public const string U_OBJ_N_IMG_TEXT_BTN = "IMG_TEXT_BTN";
	public const string U_OBJ_N_IMG_TEXT_SCALE_BTN = "IMG_TEXT_SCALE_BTN";

	public const string U_OBJ_N_TMP_IMG_TEXT_BTN = "TMP_IMG_TEXT_BTN";
	public const string U_OBJ_N_TMP_IMG_TEXT_SCALE_BTN = "TMP_IMG_TEXT_SCALE_BTN";

	public const string U_OBJ_N_IMG_LOCALIZE_TEXT_BTN = "IMG_LOCALIZE_TEXT_BTN";
	public const string U_OBJ_N_IMG_LOCALIZE_TEXT_SCALE_BTN = "IMG_LOCALIZE_TEXT_SCALE_BTN";

	public const string U_OBJ_N_TMP_IMG_LOCALIZE_TEXT_BTN = "TMP_IMG_LOCALIZE_TEXT_BTN";
	public const string U_OBJ_N_TMP_IMG_LOCALIZE_TEXT_SCALE_BTN = "TMP_IMG_LOCALIZE_TEXT_SCALE_BTN";

	public const string U_OBJ_N_DROP = "DROP";
	public const string U_OBJ_N_INPUT = "INPUT";

	public const string U_OBJ_N_DROPDOWN = "DROPDOWN";
	public const string U_OBJ_N_INPUT_FIELD = "INPUT_FIELD";

	public const string U_OBJ_N_TMP_DROPDOWN = "TMP_DROPDOWN";
	public const string U_OBJ_N_TMP_INPUT_FIELD = "TMP_INPUT_FIELD";

	public const string U_OBJ_N_PAGE_VIEW = "PAGE_VIEW";
	public const string U_OBJ_N_SCROLL_VIEW = "SCROLL_VIEW";
	public const string U_OBJ_N_RECYCLE_VIEW = "RECYCLE_VIEW";

	public const string U_OBJ_N_LEVEL_SCROLL_VIEW = "LEVEL_SCROLL_VIEW";
	public const string U_OBJ_N_STAGE_SCROLL_VIEW = "STAGE_SCROLL_VIEW";
	public const string U_OBJ_N_CHAPTER_SCROLL_VIEW = "CHAPTER_SCROLL_VIEW";

	public const string U_OBJ_N_OBJ = "OBJ";
	public const string U_OBJ_N_SPRITE = "SPRITE";
	public const string U_OBJ_N_LINE_FX = "LINE_FX";
	public const string U_OBJ_N_PARTICLE_FX = "PARTICLE_FX";
	public const string U_OBJ_N_REFLECTION_PROBE = "REFLECTION_PROBE";
	public const string U_OBJ_N_LIGHT_PROBE_GROUP = "LIGHT_PROBE_GROUP";

	public const string U_OBJ_N_DRAG_RESPONDER = "DRAG_RESPONDER";
	public const string U_OBJ_N_TOUCH_RESPONDER = "TOUCH_RESPONDER";
	public const string U_OBJ_N_BG_TOUCH_RESPONDER = "BG_TOUCH_RESPONDER";

	public const string U_OBJ_N_INDICATOR_TOUCH_RESPONDER = "INDICATOR_TOUCH_RESPONDER";
	public const string U_OBJ_N_SCREEN_FADE_TOUCH_RESPONDER = "SCREEN_FADE_TOUCH_RESPONDER";

	public const string U_OBJ_N_FMT_TOGGLE = "TOGGLE_{0:00}";
	public const string U_OBJ_N_FMT_NUM_TEXT = "NUM_TEXT_{0:00}";
	public const string U_OBJ_N_FMT_MARK_IMG = "MARK_IMG_{0:00}";
	public const string U_OBJ_N_FMT_PAGE_UIS = "PAGE_UIS_{0:00}";
	public const string U_OBJ_N_FMT_POPUP_TOUCH_RESPONDER = "POPUP_TOUCH_RESPONDER_{0}";

	public const string U_IMG_N_TEX = "Tex";
	public const string U_IMG_N_SPRITE = "Sprite";
	public const string U_IMG_N_CLONE_SPRITE = "(Clone)";

	public const string U_FUNC_N_ON_DRAG = "OnDrag";
	public const string U_FUNC_N_ON_POINTER_UP = "OnPointerUp";
	public const string U_FUNC_N_ON_POINTER_DOWN = "OnPointerDown";
	public const string U_FUNC_N_ON_POINTER_ENTER = "OnPointerEnter";
	public const string U_FUNC_N_ON_POINTER_EXIT = "OnPointerExit";

	public const string U_FUNC_N_INIT = "Init";
	public const string U_FUNC_N_RESET_ANI = "ResetAni";
	public const string U_FUNC_N_RESET_LOCALIZE = "ResetLocalize";
	public const string U_FUNC_N_RESET_DIFFICULTY = "ResetDifficulty";
	public const string U_FUNC_N_UPDATE_UIS_STATE = "UpdateUIsState";

	public const string U_INPUT_N_HORIZONTAL = "Horizontal";
	public const string U_INPUT_N_VERTICAL = "Vertical";

	public const string U_PROPERTY_N_TEXT = "text";
	public const string U_PROPERTY_N_FONT = "font";
	public const string U_PROPERTY_N_COLOR = "color";
	public const string U_PROPERTY_N_SPRITE = "sprite";
	public const string U_PROPERTY_N_TMP_TEXT = "text";
	public const string U_PROPERTY_N_TMP_FONT = "font";

	public const string U_MAT_N_DEF_SKYBOX = "Default-Skybox";
	public const string U_ICON_N_ANDROID_NOTI_SMALL = "smallnotiicon";
	public const string U_ICON_N_ANDROID_NOTI_LARGE = "largenotiicon";
	// 이름 }

	// 경로
	public const string U_IMG_P_DEF = "DefImg";
	public const string U_MESH_P_DEF = "DefMesh";

	// 태그 {
	public const string U_TAG_PLAYER = "Player";
	public const string U_TAG_FINISH = "Finish";
	public const string U_TAG_RESPAWN = "Respawn";
	public const string U_TAG_UNTAGGED = "Untagged";
	public const string U_TAG_EDITOR_ONLY = "EditorOnly";
	public const string U_TAG_MAIN_CAMERA = "MainCamera";
	public const string U_TAG_GAME_CONTROLLER = "GameController";

	public const string U_TAG_ENEMY = "Enemy";
	public const string U_TAG_OBSTACLE = "Obstacle";
	public const string U_TAG_MAIN_LIGHT = "MainLight";
	public const string U_TAG_ADDITIONAL_LIGHT = "AdditionalLight";
	public const string U_TAG_ADDITIONAL_CAMERA = "AdditionalCamera";
	public const string U_TAG_SCENE_MANAGER = "SceneManager";
	// 태그 }

	// 정렬 레이어 {
	public const string U_SORTING_L_ABS = "Abs";
	public const string U_SORTING_L_DEF = "Default";

	public const string U_SORTING_L_TOP = "Top";
	public const string U_SORTING_L_TOPMOST = "Topmost";

	public const string U_SORTING_L_FOREGROUND = "Foreground";
	public const string U_SORTING_L_OVERGROUND = "Overground";

	public const string U_SORTING_L_BACKGROUND = "Background";
	public const string U_SORTING_L_UNDERGROUND = "Underground";

	public const string U_SORTING_L_OVERLAY_ABS = "OverlayAbs";
	public const string U_SORTING_L_OVERLAY_DEF = "OverlayDefault";

	public const string U_SORTING_L_OVERLAY_TOP = "OverlayTop";
	public const string U_SORTING_L_OVERLAY_TOPMOST = "OverlayTopmost";

	public const string U_SORTING_L_OVERLAY_FOREGROUND = "OverlayForeground";
	public const string U_SORTING_L_OVERLAY_OVERGROUND = "OverlayOverground";

	public const string U_SORTING_L_OVERLAY_BACKGROUND = "OverlayBackground";
	public const string U_SORTING_L_OVERLAY_UNDERGROUND = "OverlayUnderground";

    public const string U_SORTING_L_AIMING = "Aiming";
    public const string U_SORTING_L_CELL = "Cell";
    public const string U_SORTING_L_BALL = "Ball";
    public const string U_SORTING_L_EFFECT = "Effect";
	// 정렬 레이어 }

	// 씬 관리자
	public const string U_KEY_FMT_SCENE_M_TOUCH_RESPONDER = "SceneMTouchResponder_{0}";

	// 사운드 관리자
	public const string U_OBJ_N_SND_M_FX_SNDS = "FXSnds";

	// 입력 모듈 {
	public const int U_THRESHOLD_INPUT_M_MOVE = 5;

	public const float U_RATE_INPUT_M_MOVE_REPEAT = 0.1f;
	public const float U_DELAY_INPUT_M_MOVE_REPEAT = 0.5f;
	public const float U_UNIT_INPUT_M_INPUT_ACTIONS_PER_SEC = 10.0f;
	// 입력 모듈 }

	// 문자열 테이블
	public const string U_KEY_STR_T_ID = "ID";
	public const string U_KEY_STR_T_STR = "Str";

	// 값 테이블
	public const string U_KEY_VAL_T_ID = "ID";
	public const string U_KEY_VAL_T_VAL = "Val";
	public const string U_KEY_VAL_T_VAL_TYPE = "ValType";

	// 서비스 관리자 {
	public const string U_KEY_SERVICES_M_RECEIPT = "json";
	public const string U_KEY_SERVICES_M_PAYLOAD = "Payload";
	public const string U_KEY_SERVICES_M_SIGNATURE = "signature";

	public const string U_KEY_SERVICES_M_INIT_CALLBACK = "ServicesMInitCallback";
	public const string U_KEY_SERVICES_M_LOAD_GOOGLE_SHEET_CALLBACK = "ServicesMLoadGoogleSheetCallback";
	public const string U_KEY_SERVICES_M_SAVE_GOOGLE_SHEET_CALLBACK = "ServicesMSaveGoogleSheetCallback";
	// 서비스 관리자 }

	// 유니티 메세지 전송자 {
	public const string U_KEY_UNITY_MS_APP_ID = "AppID";
	public const string U_KEY_UNITY_MS_VER = "Ver";
	public const string U_KEY_UNITY_MS_TIMEOUT = "Timeout";

	public const string U_KEY_UNITY_MS_ALERT_TITLE = "Title";
	public const string U_KEY_UNITY_MS_ALERT_MSG = "Msg";
	public const string U_KEY_UNITY_MS_ALERT_OK_BTN_TEXT = "OKBtnText";
	public const string U_KEY_UNITY_MS_ALERT_CANCEL_BTN_TEXT = "CancelBtnText";

	public const string U_KEY_UNITY_MS_MAIL_TITLE = "Title";
	public const string U_KEY_UNITY_MS_MAIL_MSG = "Msg";
	public const string U_KEY_UNITY_MS_MAIL_RECIPIENT = "Recipient";

	public const string U_KEY_UNITY_MS_VIBRATE_TYPE = "Type";
	public const string U_KEY_UNITY_MS_VIBRATE_STYLE = "Style";
	public const string U_KEY_UNITY_MS_VIBRATE_DURATION = "Duration";
	public const string U_KEY_UNITY_MS_VIBRATE_INTENSITY = "Intensity";

	public const string U_KEY_UNITY_MS_SEND_SHARE_MSG_CALLBACK = "UnityMSSendShareMsgCallback";
	public const string U_CLS_N_UNITY_MS_UNITY_MSG_RECEIVER = "lkstudio.dante.android.CAndroidPlugin";
	public const string U_FUNC_N_UNITY_MS_UNITY_MSG_HANDLER = "onReceiveUnityMsg";
	// 유니티 메세지 전송자 }

	// 디바이스 메세지 수신자 {
	public const string U_KEY_DEVICE_MR_VER = KCDefine.U_KEY_UNITY_MS_VER;
	public const string U_KEY_DEVICE_MR_RESULT = "Result";
	public const string U_KEY_FMT_DEVICE_MR_HANDLE_MSG_CALLBACK = "DeviceMRHandleMsgCallback_{0}";

	public const string U_FUNC_N_DEVICE_MR_MSG_HANDLER = "OnReceiveDeviceMsg";
	// 디바이스 메세지 수신자 }
	#endregion // 기본

	#region 런타임 상수
	// 영역
	public static readonly Rect U_RECT_CAMERA = new Rect(0.0f, 0.0f, 1.0f, 1.0f);

	// 색상 {
	public static readonly Color U_COLOR_NORM = Color.white;
	public static readonly Color U_COLOR_PRESS = new Color(0.75f, 0.75f, 0.75f, 1.0f);
	public static readonly Color U_COLOR_SEL = Color.white;
	public static readonly Color U_COLOR_HIGHLIGHT = Color.white;
	public static readonly Color U_COLOR_DISABLE = new Color(0.35f, 0.35f, 0.35f, 1.0f);
	public static readonly Color U_COLOR_TRANSPARENT = Color.clear;

	public static readonly Color U_COLOR_BLIND_UIS = Color.black;
	public static readonly Color U_COLOR_SCREEN_FADE_IN = Color.black;
	public static readonly Color U_COLOR_SCREEN_FADE_OUT = KCDefine.U_COLOR_TRANSPARENT;

	public static readonly Color U_COLOR_CLEAR = Color.black;
	public static readonly Color U_COLOR_POPUP_BLIND = new Color(0.0f, 0.0f, 0.0f, 0.95f);
	public static readonly Color U_COLOR_INDICATOR_BLIND = new Color(0.0f, 0.0f, 0.0f, 0.75f);
	// 색상 }

	// 버전
	public static readonly System.Version U_VER_DEF = new System.Version(0, 0, 0);
	public static readonly System.Version U_VER_COMMON_APP_INFO = new System.Version(1, 0, 0);
	public static readonly System.Version U_VER_COMMON_GAME_INFO = new System.Version(1, 0, 0);
	public static readonly System.Version U_VER_COMMON_USER_INFO = new System.Version(1, 0, 0);

	// 크기
	public static readonly Vector3 U_SIZE_BANNER_ADS = new Vector3(320.0f, 50.0f, 0.0f);
	public static readonly Vector3 U_MIN_SIZE_ALERT_POPUP = new Vector3(580.0f, 300.0f, 0.0f);

	// 태그
	public static readonly List<string> U_TAG_LIST = new List<string>() {
		KCDefine.U_TAG_ENEMY, KCDefine.U_TAG_OBSTACLE, KCDefine.U_TAG_MAIN_LIGHT, KCDefine.U_TAG_ADDITIONAL_LIGHT, KCDefine.U_TAG_ADDITIONAL_CAMERA, KCDefine.U_TAG_SCENE_MANAGER
	};

	// 정렬 레이어
	public static readonly List<string> U_SORTING_LAYER_LIST = new List<string>() {
		KCDefine.U_SORTING_L_UNDERGROUND, KCDefine.U_SORTING_L_BACKGROUND, KCDefine.U_SORTING_L_DEF, KCDefine.U_SORTING_L_FOREGROUND, KCDefine.U_SORTING_L_OVERGROUND, KCDefine.U_SORTING_L_TOP, KCDefine.U_SORTING_L_TOPMOST, KCDefine.U_SORTING_L_ABS, KCDefine.U_SORTING_L_OVERLAY_UNDERGROUND, KCDefine.U_SORTING_L_OVERLAY_BACKGROUND, KCDefine.U_SORTING_L_OVERLAY_DEF, KCDefine.U_SORTING_L_OVERLAY_FOREGROUND, KCDefine.U_SORTING_L_OVERLAY_OVERGROUND, KCDefine.U_SORTING_L_OVERLAY_TOP, KCDefine.U_SORTING_L_OVERLAY_TOPMOST, KCDefine.U_SORTING_L_OVERLAY_ABS
	};

	// 정렬 순서 {
	public static readonly STSortingOrderInfo U_SORTING_OI_ABS = new STSortingOrderInfo() {
		m_nOrder = KCDefine.U_SORTING_O_ABS, m_oLayer = KCDefine.U_SORTING_L_ABS
	};

	public static readonly STSortingOrderInfo U_SORTING_OI_DEF = new STSortingOrderInfo() {
		m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_DEF
	};

    public static readonly STSortingOrderInfo U_SORTING_OI_CELL = new STSortingOrderInfo() {
		m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL
	};

	public static readonly STSortingOrderInfo U_SORTING_OI_TOP = new STSortingOrderInfo() {
		m_nOrder = KCDefine.U_SORTING_O_TOP, m_oLayer = KCDefine.U_SORTING_L_TOP
	};

	public static readonly STSortingOrderInfo U_SORTING_OI_TOPMOST = new STSortingOrderInfo() {
		m_nOrder = KCDefine.U_SORTING_O_TOPMOST, m_oLayer = KCDefine.U_SORTING_L_TOPMOST
	};

	public static readonly STSortingOrderInfo U_SORTING_OI_FOREGROUND = new STSortingOrderInfo() {
		m_nOrder = KCDefine.U_SORTING_O_FOREGROUND, m_oLayer = KCDefine.U_SORTING_L_FOREGROUND
	};

	public static readonly STSortingOrderInfo U_SORTING_OI_OVERGROUND = new STSortingOrderInfo() {
		m_nOrder = KCDefine.U_SORTING_O_OVERGROUND, m_oLayer = KCDefine.U_SORTING_L_OVERGROUND
	};

	public static readonly STSortingOrderInfo U_SORTING_OI_BACKGROUND = new STSortingOrderInfo() {
		m_nOrder = KCDefine.U_SORTING_O_BACKGROUND, m_oLayer = KCDefine.U_SORTING_L_BACKGROUND
	};

	public static readonly STSortingOrderInfo U_SORTING_OI_UNDERGROUND = new STSortingOrderInfo() {
		m_nOrder = KCDefine.U_SORTING_O_UNDERGROUND, m_oLayer = KCDefine.U_SORTING_L_UNDERGROUND
	};

	public static readonly STSortingOrderInfo U_SORTING_OI_UIS_CANVAS = new STSortingOrderInfo() {
		m_nOrder = KCDefine.U_SORTING_O_UIS, m_oLayer = KCDefine.U_SORTING_L_DEF
	};

	public static readonly STSortingOrderInfo U_SORTING_OI_OVERLAY_UIS_CANVAS = new STSortingOrderInfo() {
		m_nOrder = KCDefine.U_SORTING_O_OVERLAY_UIS, m_oLayer = KCDefine.U_SORTING_L_DEF
	};
	// 정렬 순서 }

	// 동기화 객체
	public static readonly object U_LOCK_OBJ_COMMON = new object();
	public static readonly object U_LOCK_OBJ_TASK_M_UPDATE = new object();
	public static readonly object U_LOCK_OBJ_SCHEDULE_M_UPDATE = new object();

	// 경로 {
	public static readonly string U_DATA_P_FMT_G_LEVEL_INFO = $"{KCDefine.B_DIR_P_TABLES}{KCDefine.B_DIR_P_GLOBAL}LevelInfo/G_LevelInfo_{"{0:000000000}"}";
	public static readonly string U_DATA_P_FMT_G_LEVEL_INFO_SET_A = $"{KCDefine.B_DIR_P_TABLES}{KCDefine.B_DIR_P_GLOBAL}A/LevelInfo/G_LevelInfo_{"{0:000000000}"}";
	public static readonly string U_DATA_P_FMT_G_LEVEL_INFO_SET_B = $"{KCDefine.B_DIR_P_TABLES}{KCDefine.B_DIR_P_GLOBAL}B/LevelInfo/G_LevelInfo_{"{0:000000000}"}";

	public static readonly string U_OBJ_P_TEXT = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Text/U_Text";
	public static readonly string U_OBJ_P_TEXT_BTN = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Button/Text/U_TextBtn";
	public static readonly string U_OBJ_P_TEXT_SCALE_BTN = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Button/Text/U_TextScaleBtn";

	public static readonly string U_OBJ_P_TMP_TEXT = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Text/U_TMPText";
	public static readonly string U_OBJ_P_TMP_TEXT_BTN = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Button/TextMeshPro/U_TMPTextBtn";
	public static readonly string U_OBJ_P_TMP_TEXT_MESH = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Text/U_TMPTextMesh";
	public static readonly string U_OBJ_P_TMP_TEXT_SCALE_BTN = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Button/TextMeshPro/U_TMPTextScaleBtn";

	public static readonly string U_OBJ_P_LOCALIZE_TEXT = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Text/U_LocalizeText";
	public static readonly string U_OBJ_P_LOCALIZE_TEXT_BTN = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Button/Text/U_LocalizeTextBtn";
	public static readonly string U_OBJ_P_LOCALIZE_TEXT_SCALE_BTN = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Button/Text/U_LocalizeTextScaleBtn";

	public static readonly string U_OBJ_P_TMP_LOCALIZE_TEXT = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Text/U_TMPLocalizeText";
	public static readonly string U_OBJ_P_TMP_LOCALIZE_TEXT_BTN = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Button/TextMeshPro/U_TMPLocalizeTextBtn";
	public static readonly string U_OBJ_P_TMP_LOCALIZE_TEXT_SCALE_BTN = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Button/TextMeshPro/U_TMPLocalizeTextScaleBtn";

	public static readonly string U_OBJ_P_IMG = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Image/U_Img";
	public static readonly string U_OBJ_P_MASK_IMG = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Image/U_MaskImg";
	public static readonly string U_OBJ_P_FOCUS_IMG = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Image/U_FocusImg";
	public static readonly string U_OBJ_P_GAUGE_IMG = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Image/U_GaugeImg";

	public static readonly string U_OBJ_P_IMG_BTN = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Button/U_ImgBtn";
	public static readonly string U_OBJ_P_IMG_SCALE_BTN = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Button/U_ImgScaleBtn";

	public static readonly string U_OBJ_P_IMG_TEXT_BTN = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Button/Text/U_ImgTextBtn";
	public static readonly string U_OBJ_P_IMG_TEXT_SCALE_BTN = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Button/Text/U_ImgTextScaleBtn";

	public static readonly string U_OBJ_P_TMP_IMG_TEXT_BTN = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Button/TextMeshPro/U_TMPImgTextBtn";
	public static readonly string U_OBJ_P_TMP_IMG_TEXT_SCALE_BTN = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Button/TextMeshPro/U_TMPImgTextScaleBtn";

	public static readonly string U_OBJ_P_IMG_LOCALIZE_TEXT_BTN = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Button/Text/U_ImgLocalizeTextBtn";
	public static readonly string U_OBJ_P_IMG_LOCALIZE_TEXT_SCALE_BTN = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Button/Text/U_ImgLocalizeTextScaleBtn";

	public static readonly string U_OBJ_P_TMP_IMG_LOCALIZE_TEXT_BTN = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Button/TextMeshPro/U_TMPImgLocalizeTextBtn";
	public static readonly string U_OBJ_P_TMP_IMG_LOCALIZE_TEXT_SCALE_BTN = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Button/TextMeshPro/U_TMPImgLocalizeTextScaleBtn";

	public static readonly string U_OBJ_P_DROPDOWN = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Input/U_Dropdown";
	public static readonly string U_OBJ_P_INPUT_FIELD = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Input/U_InputField";

	public static readonly string U_OBJ_P_TMP_DROPDOWN = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Input/U_TMPDropdown";
	public static readonly string U_OBJ_P_TMP_INPUT_FIELD = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Input/U_TMPInputField";

	public static readonly string U_OBJ_P_PAGE_VIEW = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/ScrollView/U_PageView";
	public static readonly string U_OBJ_P_SCROLL_VIEW = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/ScrollView/U_ScrollView";
	public static readonly string U_OBJ_P_RECYCLE_VIEW = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/ScrollView/U_RecycleView";

	public static readonly string U_OBJ_P_DRAG_RESPONDER = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Responder/U_DragResponder";
	public static readonly string U_OBJ_P_TOUCH_RESPONDER = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Responder/U_TouchResponder";
	public static readonly string U_OBJ_P_INDICATOR_TOUCH_RESPONDER = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Responder/U_IndicatorTouchResponder";
	public static readonly string U_OBJ_P_SCREEN_FADE_TOUCH_RESPONDER = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}UI/Responder/U_ScreenFadeTouchResponder";

	public static readonly string U_OBJ_P_OBJ = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}2D/U_Obj";
	public static readonly string U_OBJ_P_SPRITE = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}2D/U_Sprite";
	public static readonly string U_OBJ_P_LINE_FX = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}FX/U_LineFX";
	public static readonly string U_OBJ_P_PARTICLE_FX = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}FX/U_ParticleFX";
	public static readonly string U_OBJ_P_TIMER_MANAGER = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}External/U_TimerManager";
	public static readonly string U_OBJ_P_REFLECTION_PROBE = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}FX/U_ReflectionProbe";
	public static readonly string U_OBJ_P_LIGHT_PROBE_GROUP = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}FX/U_LightProbeGroup";

	public static readonly string U_OBJ_P_BG_SND = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}Sound/U_BGSnd";
	public static readonly string U_OBJ_P_FX_SNDS = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_UTILITY}Sound/U_FXSnds";

	public static readonly string U_OBJ_P_G_INFO_TEXT = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_GLOBAL}G_InfoText";
	public static readonly string U_OBJ_P_G_BACK_BTN = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_GLOBAL}G_BackBtn";
	public static readonly string U_OBJ_P_G_MESH = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_GLOBAL}G_Mesh";

	public static readonly string U_OBJ_P_G_POPUP = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_GLOBAL}{KCDefine.B_DIR_P_POPUP}G_Popup";
	public static readonly string U_OBJ_P_G_ALERT_POPUP = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_GLOBAL}{KCDefine.B_DIR_P_POPUP}G_AlertPopup";
	public static readonly string U_OBJ_P_G_STORE_POPUP = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_GLOBAL}{KCDefine.B_DIR_P_POPUP}G_StorePopup";
	public static readonly string U_OBJ_P_G_SETTINGS_POPUP = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_GLOBAL}{KCDefine.B_DIR_P_POPUP}G_SettingsPopup";
	public static readonly string U_OBJ_P_G_SYNC_POPUP = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_GLOBAL}{KCDefine.B_DIR_P_POPUP}G_SyncPopup";
	public static readonly string U_OBJ_P_G_DAILY_MISSION_POPUP = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_GLOBAL}{KCDefine.B_DIR_P_POPUP}G_DailyMissionPopup";
	public static readonly string U_OBJ_P_G_FREE_REWARD_POPUP = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_GLOBAL}{KCDefine.B_DIR_P_POPUP}G_FreeRewardPopup";
	public static readonly string U_OBJ_P_G_DAILY_REWARD_POPUP = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_GLOBAL}{KCDefine.B_DIR_P_POPUP}G_DailyRewardPopup";
	public static readonly string U_OBJ_P_G_COINS_BOX_POPUP = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_GLOBAL}{KCDefine.B_DIR_P_POPUP}G_CoinsBoxBuyPopup";
	public static readonly string U_OBJ_P_G_REWARD_ACQUIRE_POPUP = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_GLOBAL}{KCDefine.B_DIR_P_POPUP}G_RewardAcquirePopup";
	public static readonly string U_OBJ_P_G_COINS_BOX_ACQUIRE_POPUP = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_GLOBAL}{KCDefine.B_DIR_P_POPUP}G_CoinsBoxAcquirePopup";
	public static readonly string U_OBJ_P_G_CONTINUE_POPUP = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_GLOBAL}{KCDefine.B_DIR_P_POPUP}G_ContinuePopup";
	public static readonly string U_OBJ_P_G_RESULT_POPUP = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_GLOBAL}{KCDefine.B_DIR_P_POPUP}G_ResultPopup";
	public static readonly string U_OBJ_P_G_RESUME_POPUP = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_GLOBAL}{KCDefine.B_DIR_P_POPUP}G_ResumePopup";
	public static readonly string U_OBJ_P_G_PAUSE_POPUP = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_GLOBAL}{KCDefine.B_DIR_P_POPUP}G_PausePopup";
	public static readonly string U_OBJ_P_G_PRODUCT_BUY_POPUP = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_GLOBAL}{KCDefine.B_DIR_P_POPUP}G_ProductBuyPopup";
	public static readonly string U_OBJ_P_G_FOCUS_POPUP = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_GLOBAL}{KCDefine.B_DIR_P_POPUP}G_FocusPopup";
	public static readonly string U_OBJ_P_G_TUTORIAL_POPUP = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_GLOBAL}{KCDefine.B_DIR_P_POPUP}G_TutorialPopup";
    public static readonly string U_OBJ_P_G_PREVIEW_POPUP = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_GLOBAL}{KCDefine.B_DIR_P_POPUP}G_LevelPreviewPopup";
    public static readonly string U_OBJ_P_G_SKIPLEVEL_POPUP = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_GLOBAL}{KCDefine.B_DIR_P_POPUP}G_SkipLevelPopup";
    public static readonly string U_OBJ_P_G_PIGGYBANK_POPUP = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_GLOBAL}{KCDefine.B_DIR_P_POPUP}G_PiggyBankPopup";
    public static readonly string U_OBJ_P_G_STARTERPACK_POPUP = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_GLOBAL}{KCDefine.B_DIR_P_POPUP}G_StarterPackPopup";
    public static readonly string U_OBJ_P_G_MEMBERSHIP_POPUP = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_GLOBAL}{KCDefine.B_DIR_P_POPUP}G_MembershipPopup";
    public static readonly string U_OBJ_P_G_RATEUS_POPUP = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_GLOBAL}{KCDefine.B_DIR_P_POPUP}G_RateUsPopup";

	public static readonly string U_ASSET_P_G_NORM_QUALITY_UNIVERSAL_RP = $"{KCDefine.B_DIR_P_PIPELINES}{KCDefine.B_DIR_P_GLOBAL}G_NormQualityUniversalRPAsset";
	public static readonly string U_ASSET_P_G_HIGH_QUALITY_UNIVERSAL_RP = $"{KCDefine.B_DIR_P_PIPELINES}{KCDefine.B_DIR_P_GLOBAL}G_HighQualityUniversalRPAsset";
	public static readonly string U_ASSET_P_G_ULTRA_QUALITY_UNIVERSAL_RP = $"{KCDefine.B_DIR_P_PIPELINES}{KCDefine.B_DIR_P_GLOBAL}G_UltraQualityUniversalRPAsset";

	public static readonly string U_ASSET_P_G_NORM_QUALITY_LIGHTING_SETTINGS = $"{KCDefine.B_DIR_P_SETTINGS}{KCDefine.B_DIR_P_GLOBAL}G_NormQualityLightingSettings";
	public static readonly string U_ASSET_P_G_HIGH_QUALITY_LIGHTING_SETTINGS = $"{KCDefine.B_DIR_P_SETTINGS}{KCDefine.B_DIR_P_GLOBAL}G_HighQualityLightingSettings";
	public static readonly string U_ASSET_P_G_ULTRA_QUALITY_LIGHTING_SETTINGS = $"{KCDefine.B_DIR_P_SETTINGS}{KCDefine.B_DIR_P_GLOBAL}G_UltraQualityLightingSettings";

	public static readonly string U_ASSET_P_G_NORM_QUALITY_POST_PROCESSING_SETTINGS = $"{KCDefine.B_DIR_P_SETTINGS}{KCDefine.B_DIR_P_GLOBAL}G_NormQualityPostProcessingSettings";
	public static readonly string U_ASSET_P_G_HIGH_QUALITY_POST_PROCESSING_SETTINGS = $"{KCDefine.B_DIR_P_SETTINGS}{KCDefine.B_DIR_P_GLOBAL}G_HighQualityPostProcessingSettings";
	public static readonly string U_ASSET_P_G_ULTRA_QUALITY_POST_PROCESSING_SETTINGS = $"{KCDefine.B_DIR_P_SETTINGS}{KCDefine.B_DIR_P_GLOBAL}G_UltraQualityPostProcessingSettings";

	public static readonly string U_ASSET_P_G_UNIVERSAL_RP_2D_DATA = $"{KCDefine.B_DIR_P_PIPELINES}{KCDefine.B_DIR_P_GLOBAL}G_UniversalRP2DData";
	public static readonly string U_ASSET_P_G_UNIVERSAL_RP_2D_SSAO_DATA = $"{KCDefine.B_DIR_P_PIPELINES}{KCDefine.B_DIR_P_GLOBAL}G_UniversalRP2DSSAOData";

	public static readonly string U_ASSET_P_G_UNIVERSAL_RP_RENDERER_DATA = $"{KCDefine.B_DIR_P_PIPELINES}{KCDefine.B_DIR_P_GLOBAL}G_UniversalRPRendererData";
	public static readonly string U_ASSET_P_G_UNIVERSAL_RP_SSAO_RENDERER_DATA = $"{KCDefine.B_DIR_P_PIPELINES}{KCDefine.B_DIR_P_GLOBAL}G_UniversalRPSSAORendererData";

	public static readonly string U_ASSET_P_G_OPTS_INFO_TABLE = $"{KCDefine.B_DIR_P_SCRIPTABLES}{KCDefine.B_DIR_P_GLOBAL}G_OptsInfoTable";
	public static readonly string U_ASSET_P_G_BUILD_INFO_TABLE = $"{KCDefine.B_DIR_P_SCRIPTABLES}{KCDefine.B_DIR_P_GLOBAL}G_BuildInfoTable";
	public static readonly string U_ASSET_P_G_PROJ_INFO_TABLE = $"{KCDefine.B_DIR_P_SCRIPTABLES}{KCDefine.B_DIR_P_GLOBAL}G_ProjInfoTable";
	public static readonly string U_ASSET_P_G_LOCALIZE_INFO_TABLE = $"{KCDefine.B_DIR_P_SCRIPTABLES}{KCDefine.B_DIR_P_GLOBAL}G_LocalizeInfoTable";
	public static readonly string U_ASSET_P_G_DEFINE_SYMBOL_INFO_TABLE = $"{KCDefine.B_DIR_P_SCRIPTABLES}{KCDefine.B_DIR_P_GLOBAL}G_DefineSymbolInfoTable";
	public static readonly string U_ASSET_P_G_DEVICE_INFO_TABLE = $"{KCDefine.B_DIR_P_SCRIPTABLES}{KCDefine.B_DIR_P_GLOBAL}G_DeviceInfoTable";
	public static readonly string U_ASSET_P_G_PRODUCT_INFO_TABLE = $"{KCDefine.B_DIR_P_SCRIPTABLES}{KCDefine.B_DIR_P_GLOBAL}G_ProductInfoTable";

	public static readonly string U_ASSET_P_SPRITE_ATLAS_01 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_UTILITY}U_SpriteAtlas_01";
	public static readonly string U_ASSET_P_SPRITE_ATLAS_02 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_UTILITY}U_SpriteAtlas_02";
	public static readonly string U_ASSET_P_SPRITE_ATLAS_03 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_UTILITY}U_SpriteAtlas_03";
	public static readonly string U_ASSET_P_SPRITE_ATLAS_04 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_UTILITY}U_SpriteAtlas_04";
	public static readonly string U_ASSET_P_SPRITE_ATLAS_05 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_UTILITY}U_SpriteAtlas_05";
	public static readonly string U_ASSET_P_SPRITE_ATLAS_06 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_UTILITY}U_SpriteAtlas_06";
	public static readonly string U_ASSET_P_SPRITE_ATLAS_07 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_UTILITY}U_SpriteAtlas_07";
	public static readonly string U_ASSET_P_SPRITE_ATLAS_08 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_UTILITY}U_SpriteAtlas_08";
	public static readonly string U_ASSET_P_SPRITE_ATLAS_09 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_UTILITY}U_SpriteAtlas_09";

	public static readonly string U_ASSET_P_G_SPRITE_ATLAS_01 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_GLOBAL}G_SpriteAtlas_01";
	public static readonly string U_ASSET_P_G_SPRITE_ATLAS_02 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_GLOBAL}G_SpriteAtlas_02";
	public static readonly string U_ASSET_P_G_SPRITE_ATLAS_03 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_GLOBAL}G_SpriteAtlas_03";
	public static readonly string U_ASSET_P_G_SPRITE_ATLAS_04 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_GLOBAL}G_SpriteAtlas_04";
	public static readonly string U_ASSET_P_G_SPRITE_ATLAS_05 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_GLOBAL}G_SpriteAtlas_05";
	public static readonly string U_ASSET_P_G_SPRITE_ATLAS_06 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_GLOBAL}G_SpriteAtlas_06";
	public static readonly string U_ASSET_P_G_SPRITE_ATLAS_07 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_GLOBAL}G_SpriteAtlas_07";
	public static readonly string U_ASSET_P_G_SPRITE_ATLAS_08 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_GLOBAL}G_SpriteAtlas_08";
	public static readonly string U_ASSET_P_G_SPRITE_ATLAS_09 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_GLOBAL}G_SpriteAtlas_09";

	public static readonly string U_ASSET_P_ES_SPRITE_ATLAS_01 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_GLOBAL}ES_SpriteAtlas_01";
	public static readonly string U_ASSET_P_ES_SPRITE_ATLAS_02 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_GLOBAL}ES_SpriteAtlas_02";
	public static readonly string U_ASSET_P_ES_SPRITE_ATLAS_03 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_GLOBAL}ES_SpriteAtlas_03";
	public static readonly string U_ASSET_P_ES_SPRITE_ATLAS_04 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_GLOBAL}ES_SpriteAtlas_04";
	public static readonly string U_ASSET_P_ES_SPRITE_ATLAS_05 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_GLOBAL}ES_SpriteAtlas_05";
	public static readonly string U_ASSET_P_ES_SPRITE_ATLAS_06 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_GLOBAL}ES_SpriteAtlas_06";
	public static readonly string U_ASSET_P_ES_SPRITE_ATLAS_07 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_GLOBAL}ES_SpriteAtlas_07";
	public static readonly string U_ASSET_P_ES_SPRITE_ATLAS_08 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_GLOBAL}ES_SpriteAtlas_08";
	public static readonly string U_ASSET_P_ES_SPRITE_ATLAS_09 = $"{KCDefine.B_DIR_P_SPRITE_ATLASES}{KCDefine.B_DIR_P_GLOBAL}ES_SpriteAtlas_09";

	public static readonly string U_TABLE_P_G_LEVEL_INFO = $"{KCDefine.B_DIR_P_TABLES}{KCDefine.B_DIR_P_GLOBAL}LevelInfo/G_LevelInfoTable";
	public static readonly string U_TABLE_P_G_LEVEL_INFO_SET_A = $"{KCDefine.B_DIR_P_TABLES}{KCDefine.B_DIR_P_GLOBAL}A/LevelInfo/G_LevelInfoTable";
	public static readonly string U_TABLE_P_G_LEVEL_INFO_SET_B = $"{KCDefine.B_DIR_P_TABLES}{KCDefine.B_DIR_P_GLOBAL}B/LevelInfo/G_LevelInfoTable";

	public static readonly string U_TABLE_P_G_ETC_INFO = $"{KCDefine.B_DIR_P_TABLES}{KCDefine.B_DIR_P_GLOBAL}G_EtcInfoTable";
	public static readonly string U_TABLE_P_G_ETC_INFO_SET_A = $"{KCDefine.B_DIR_P_TABLES}{KCDefine.B_DIR_P_GLOBAL}A/G_EtcInfoTable";
	public static readonly string U_TABLE_P_G_ETC_INFO_SET_B = $"{KCDefine.B_DIR_P_TABLES}{KCDefine.B_DIR_P_GLOBAL}B/G_EtcInfoTable";

	public static readonly string U_TABLE_P_G_VER_INFO = $"{KCDefine.B_DIR_P_TABLES}{KCDefine.B_DIR_P_GLOBAL}G_VerInfoTable";
	public static readonly string U_TABLE_P_G_MISSION_INFO = $"{KCDefine.B_DIR_P_TABLES}{KCDefine.B_DIR_P_GLOBAL}G_MissionInfoTable";
	public static readonly string U_TABLE_P_G_REWARD_INFO = $"{KCDefine.B_DIR_P_TABLES}{KCDefine.B_DIR_P_GLOBAL}G_RewardInfoTable";
	public static readonly string U_TABLE_P_G_RES_INFO = $"{KCDefine.B_DIR_P_TABLES}{KCDefine.B_DIR_P_GLOBAL}G_ResInfoTable";

	public static readonly string U_TABLE_P_G_ITEM_INFO = $"{KCDefine.B_DIR_P_TABLES}{KCDefine.B_DIR_P_GLOBAL}G_ItemInfoTable";
	public static readonly string U_TABLE_P_G_SKILL_INFO = $"{KCDefine.B_DIR_P_TABLES}{KCDefine.B_DIR_P_GLOBAL}G_SkillInfoTable";
	public static readonly string U_TABLE_P_G_OBJ_INFO = $"{KCDefine.B_DIR_P_TABLES}{KCDefine.B_DIR_P_GLOBAL}G_ObjInfoTable";
	public static readonly string U_TABLE_P_G_ABILITY_INFO = $"{KCDefine.B_DIR_P_TABLES}{KCDefine.B_DIR_P_GLOBAL}G_AbilityInfoTable";
	public static readonly string U_TABLE_P_G_PRODUCT_INFO = $"{KCDefine.B_DIR_P_TABLES}{KCDefine.B_DIR_P_GLOBAL}G_ProductInfoTable";

	public static readonly string U_TABLE_P_G_COMMON_VAL = $"{KCDefine.B_DIR_P_TABLES}{KCDefine.B_DIR_P_GLOBAL}ValueInfo/G_ValTable_Common";
	public static readonly string U_TABLE_P_G_COMMON_STR = $"{KCDefine.B_DIR_P_TABLES}{KCDefine.B_DIR_P_GLOBAL}StringInfo/G_StrTable_Common";

	public static readonly string U_TABLE_P_FMT_G_COMMON_VAL = $"{KCDefine.B_DIR_P_TABLES}{KCDefine.B_DIR_P_GLOBAL}ValueInfo/{KCDefine.B_TEXT_FMT_2_UNDER_SCORE_COMBINE}";
	public static readonly string U_TABLE_P_FMT_G_COMMON_STR = $"{KCDefine.B_DIR_P_TABLES}{KCDefine.B_DIR_P_GLOBAL}StringInfo/{KCDefine.B_TEXT_FMT_2_UNDER_SCORE_COMBINE}";

	public static readonly string U_TABLE_P_FMT_G_LOCALIZE_COMMON_VAL = string.Format(KCDefine.U_TABLE_P_FMT_G_COMMON_VAL, "G_ValTable_Common", "{0}");
	public static readonly string U_TABLE_P_FMT_G_LOCALIZE_COMMON_STR = string.Format(KCDefine.U_TABLE_P_FMT_G_COMMON_STR, "G_StrTable_Common", "{0}");

	public static readonly string U_TABLE_P_G_KOREAN_COMMON_STR = string.Format(KCDefine.U_TABLE_P_FMT_G_LOCALIZE_COMMON_STR, SystemLanguage.Korean);
	public static readonly string U_TABLE_P_G_ENGLISH_COMMON_STR = string.Format(KCDefine.U_TABLE_P_FMT_G_LOCALIZE_COMMON_STR, SystemLanguage.English);

	public static readonly string U_BASE_TABLE_P_G_LOCALIZE_COMMON_VAL = KCDefine.U_TABLE_P_G_COMMON_VAL;
	public static readonly string U_BASE_TABLE_P_G_LOCALIZE_COMMON_STR = KCDefine.U_TABLE_P_G_COMMON_STR;

	public static readonly string U_FONT_P_G_DEF = $"Fonts & Materials/LiberationSans SDF";
	public static readonly string U_FONT_P_G_KOREAN = $"{KCDefine.B_DIR_P_FONTS}{KCDefine.B_DIR_P_GLOBAL}G_Korean";
	public static readonly string U_FONT_P_G_ENGLISH = $"{KCDefine.B_DIR_P_FONTS}{KCDefine.B_DIR_P_GLOBAL}G_English";

	public static readonly string U_SND_P_G_SFX_TOUCH_BEGIN = $"{KCDefine.B_DIR_P_SOUNDS}{KCDefine.B_DIR_P_GLOBAL}G_SFX_TouchBegin";
	public static readonly string U_SND_P_G_SFX_TOUCH_END = $"{KCDefine.B_DIR_P_SOUNDS}{KCDefine.B_DIR_P_GLOBAL}G_SFX_TouchEnd";

	public static readonly string U_SND_P_G_SFX_POPUP_SHOW = $"{KCDefine.B_DIR_P_SOUNDS}{KCDefine.B_DIR_P_GLOBAL}G_SFX_PopupShow";
	public static readonly string U_SND_P_G_SFX_POPUP_CLOSE = $"{KCDefine.B_DIR_P_SOUNDS}{KCDefine.B_DIR_P_GLOBAL}G_SFX_PopupClose";

	public static readonly string U_MAT_P_G_SKYBOX = $"{KCDefine.B_DIR_P_MATERIALS}{KCDefine.B_DIR_P_GLOBAL}G_Skybox";
	public static readonly string U_MAT_P_G_UNIVERSAL_RP_LIT = $"{KCDefine.B_DIR_P_MATERIALS}{KCDefine.B_DIR_P_GLOBAL}G_UniversalRPLit";
	public static readonly string U_MAT_P_G_UNIVERSAL_RP_UNLIT = $"{KCDefine.B_DIR_P_MATERIALS}{KCDefine.B_DIR_P_GLOBAL}G_UniversalRPUnlit";

	public static readonly string U_IMG_P_ARROW = $"{KCDefine.B_PREFIX_U_SPRITE_ATLAS_01}Arrow";
	public static readonly string U_IMG_P_WHITE = $"{KCDefine.B_PREFIX_U_SPRITE_ATLAS_01}White";
	public static readonly string U_IMG_P_SPLASH = $"{KCDefine.B_PREFIX_U_SPRITE_ATLAS_01}Splash";
	public static readonly string U_IMG_P_INDICATOR = $"{KCDefine.B_PREFIX_U_SPRITE_ATLAS_01}Indicator";

	public static readonly string U_TEX_P_ARROW = $"{KCDefine.B_DIR_P_TEXTURES}U_SpriteAtlas_01/{KCDefine.B_PREFIX_U_SPRITE_ATLAS_01}Arrow";
	public static readonly string U_TEX_P_WHITE = $"{KCDefine.B_DIR_P_TEXTURES}U_SpriteAtlas_01/{KCDefine.B_PREFIX_U_SPRITE_ATLAS_01}White";
	public static readonly string U_TEX_P_SPLASH = $"{KCDefine.B_DIR_P_TEXTURES}U_SpriteAtlas_01/{KCDefine.B_PREFIX_U_SPRITE_ATLAS_01}Splash";
	public static readonly string U_TEX_P_INDICATOR = $"{KCDefine.B_DIR_P_TEXTURES}U_SpriteAtlas_01/{KCDefine.B_PREFIX_U_SPRITE_ATLAS_01}Indicator";

	public static readonly string U_DATA_P_COMMON_APP_INFO = $"{KCDefine.B_DIR_P_WRITABLE}CommonAppInfo.bytes";
	public static readonly string U_DATA_P_COMMON_USER_INFO = $"{KCDefine.B_DIR_P_WRITABLE}CommonUserInfo.bytes";
	public static readonly string U_DATA_P_COMMON_GAME_INFO = $"{KCDefine.B_DIR_P_WRITABLE}CommonGameInfo.bytes";

#if UNITY_EDITOR
	public static readonly string U_RUNTIME_TABLE_P_G_LEVEL_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_LEVEL_INFO}.json";
	public static readonly string U_RUNTIME_TABLE_P_G_LEVEL_INFO_SET_A = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_LEVEL_INFO_SET_A}.json";
	public static readonly string U_RUNTIME_TABLE_P_G_LEVEL_INFO_SET_B = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_LEVEL_INFO_SET_B}.json";

	public static readonly string U_RUNTIME_TABLE_P_G_ETC_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_ETC_INFO}.json";
	public static readonly string U_RUNTIME_TABLE_P_G_ETC_INFO_SET_A = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_ETC_INFO_SET_A}.json";
	public static readonly string U_RUNTIME_TABLE_P_G_ETC_INFO_SET_B = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_ETC_INFO_SET_B}.json";

	public static readonly string U_RUNTIME_TABLE_P_G_MISSION_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_MISSION_INFO}.json";
	public static readonly string U_RUNTIME_TABLE_P_G_REWARD_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_REWARD_INFO}.json";
	public static readonly string U_RUNTIME_TABLE_P_G_RES_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_RES_INFO}.json";

	public static readonly string U_RUNTIME_TABLE_P_G_ITEM_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_ITEM_INFO}.json";
	public static readonly string U_RUNTIME_TABLE_P_G_SKILL_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_SKILL_INFO}.json";
	public static readonly string U_RUNTIME_TABLE_P_G_OBJ_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_OBJ_INFO}.json";
	public static readonly string U_RUNTIME_TABLE_P_G_ABILITY_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_ABILITY_INFO}.json";
	public static readonly string U_RUNTIME_TABLE_P_G_PRODUCT_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_PRODUCT_INFO}.json";

	public static readonly string U_RUNTIME_DATA_P_FMT_G_LEVEL_INFO = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_DATA_P_FMT_G_LEVEL_INFO}.bytes";
	public static readonly string U_RUNTIME_DATA_P_FMT_G_LEVEL_INFO_SET_A = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_DATA_P_FMT_G_LEVEL_INFO_SET_A}.bytes";
	public static readonly string U_RUNTIME_DATA_P_FMT_G_LEVEL_INFO_SET_B = $"{KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_DATA_P_FMT_G_LEVEL_INFO_SET_B}.bytes";
#else
	public static readonly string U_RUNTIME_TABLE_P_G_LEVEL_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_LEVEL_INFO}.json";
	public static readonly string U_RUNTIME_TABLE_P_G_LEVEL_INFO_SET_A = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_LEVEL_INFO_SET_A}.json";
	public static readonly string U_RUNTIME_TABLE_P_G_LEVEL_INFO_SET_B = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_LEVEL_INFO_SET_B}.json";
	
	public static readonly string U_RUNTIME_TABLE_P_G_ETC_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_ETC_INFO}.json";
	public static readonly string U_RUNTIME_TABLE_P_G_ETC_INFO_SET_A = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_ETC_INFO_SET_A}.json";
	public static readonly string U_RUNTIME_TABLE_P_G_ETC_INFO_SET_B = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_ETC_INFO_SET_B}.json";

	public static readonly string U_RUNTIME_TABLE_P_G_MISSION_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_MISSION_INFO}.json";
	public static readonly string U_RUNTIME_TABLE_P_G_REWARD_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_REWARD_INFO}.json";
	public static readonly string U_RUNTIME_TABLE_P_G_RES_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_RES_INFO}.json";

	public static readonly string U_RUNTIME_TABLE_P_G_ITEM_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_ITEM_INFO}.json";
	public static readonly string U_RUNTIME_TABLE_P_G_SKILL_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_SKILL_INFO}.json";
	public static readonly string U_RUNTIME_TABLE_P_G_OBJ_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_OBJ_INFO}.json";
	public static readonly string U_RUNTIME_TABLE_P_G_ABILITY_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_ABILITY_INFO}.json";
	public static readonly string U_RUNTIME_TABLE_P_G_PRODUCT_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_PRODUCT_INFO}.json";

	public static readonly string U_RUNTIME_DATA_P_FMT_G_LEVEL_INFO = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_DATA_P_FMT_G_LEVEL_INFO}.bytes";
	public static readonly string U_RUNTIME_DATA_P_FMT_G_LEVEL_INFO_SET_A = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_DATA_P_FMT_G_LEVEL_INFO_SET_A}.bytes";
	public static readonly string U_RUNTIME_DATA_P_FMT_G_LEVEL_INFO_SET_B = $"{KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}{KCDefine.U_DATA_P_FMT_G_LEVEL_INFO_SET_B}.bytes";
#endif // #if UNITY_EDITOR

#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
	public static readonly string U_IMG_P_SCREENSHOT = $"{Application.identifier}/Screenshot.png";
#else
	public static readonly string U_IMG_P_SCREENSHOT = $"{KCDefine.B_DIR_P_WRITABLE}Screenshot.png";
#endif // #if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)

	public static readonly List<string> U_ASSET_P_SPRITE_ATLAS_LIST = new List<string>() {
		KCDefine.U_ASSET_P_SPRITE_ATLAS_01,
		KCDefine.U_ASSET_P_SPRITE_ATLAS_02,
		KCDefine.U_ASSET_P_SPRITE_ATLAS_03,
		KCDefine.U_ASSET_P_SPRITE_ATLAS_04,
		KCDefine.U_ASSET_P_SPRITE_ATLAS_05,
		KCDefine.U_ASSET_P_SPRITE_ATLAS_06,
		KCDefine.U_ASSET_P_SPRITE_ATLAS_07,
		KCDefine.U_ASSET_P_SPRITE_ATLAS_08,
		KCDefine.U_ASSET_P_SPRITE_ATLAS_09,

		KCDefine.U_ASSET_P_G_SPRITE_ATLAS_01,
		KCDefine.U_ASSET_P_G_SPRITE_ATLAS_02,
		KCDefine.U_ASSET_P_G_SPRITE_ATLAS_03,
		KCDefine.U_ASSET_P_G_SPRITE_ATLAS_04,
		KCDefine.U_ASSET_P_G_SPRITE_ATLAS_05,
		KCDefine.U_ASSET_P_G_SPRITE_ATLAS_06,
		KCDefine.U_ASSET_P_G_SPRITE_ATLAS_07,
		KCDefine.U_ASSET_P_G_SPRITE_ATLAS_08,
		KCDefine.U_ASSET_P_G_SPRITE_ATLAS_09
	};

	public static readonly List<string> U_ASSET_P_ES_SPRITE_ATLAS_LIST = new List<string>() {
		KCDefine.U_ASSET_P_ES_SPRITE_ATLAS_01,
		KCDefine.U_ASSET_P_ES_SPRITE_ATLAS_02,
		KCDefine.U_ASSET_P_ES_SPRITE_ATLAS_03,
		KCDefine.U_ASSET_P_ES_SPRITE_ATLAS_04,
		KCDefine.U_ASSET_P_ES_SPRITE_ATLAS_05,
		KCDefine.U_ASSET_P_ES_SPRITE_ATLAS_06,
		KCDefine.U_ASSET_P_ES_SPRITE_ATLAS_07,
		KCDefine.U_ASSET_P_ES_SPRITE_ATLAS_08,
		KCDefine.U_ASSET_P_ES_SPRITE_ATLAS_09
	};
	// 경로 }
	#endregion // 런타임 상수

	#region 조건부 상수
#if UNITY_EDITOR
	// 퀄리티
	public const int U_QUALITY_ASYNC_UPLOAD_TIME_SLICE = 2;
	public const int U_QUALITY_ASYNC_UPLOAD_BUFFER_SIZE = 16;

	// 스크립트 순서 {
	public const int U_SCRIPT_O_SINGLETON = sbyte.MaxValue;
	public const int U_SCRIPT_O_ADS_CORRECTOR = byte.MaxValue;
	public const int U_SCRIPT_O_ADS_INTERACTABLE = byte.MaxValue;

	public const int U_SCRIPT_O_INIT_SCENE_MANAGER = sbyte.MaxValue / 2;
	public const int U_SCRIPT_O_START_SCENE_MANAGER = KCDefine.U_SCRIPT_O_INIT_SCENE_MANAGER + 1;
	public const int U_SCRIPT_O_SETUP_SCENE_MANAGER = KCDefine.U_SCRIPT_O_INIT_SCENE_MANAGER + 1;
	public const int U_SCRIPT_O_AGREE_SCENE_MANAGER = KCDefine.U_SCRIPT_O_INIT_SCENE_MANAGER + 1;
	public const int U_SCRIPT_O_LATE_SETUP_SCENE_MANAGER = KCDefine.U_SCRIPT_O_INIT_SCENE_MANAGER + 1;

	public const int U_SCRIPT_O_SCENE_MANAGER = KCDefine.U_SCRIPT_O_INIT_SCENE_MANAGER + 2;
	public const int U_SCRIPT_O_LOADING_SCENE_MANAGER = KCDefine.U_SCRIPT_O_INIT_SCENE_MANAGER + 3;
	public const int U_SCRIPT_O_OVERLAY_SCENE_MANAGER = KCDefine.U_SCRIPT_O_INIT_SCENE_MANAGER + 3;
	// 스크립트 순서 }
#endif // #if UNITY_EDITOR

#if UNITY_IOS
	// 이름
	public const string U_MODEL_N_IPAD = "iPad";
	public const string U_MODEL_N_IPHONE = "iPhone";
#endif // #if UNITY_IOS

#if UNITY_ANDROID
	// 시간
	public const float U_DELTA_T_PERMISSION_M_REQUEST_CHECK = 0.25f;
	public const float U_MAX_DELTA_T_PERMISSION_M_REQUEST_CHECK = 1.0f;
#endif // #if UNITY_ANDROID

#if DEBUG || DEVELOPMENT_BUILD
	// 형식 {
	public const string U_TEXT_FMT_STATIC_DEBUG_MSG = "{0}\n\n{1}";
	public const string U_TEXT_FMT_DYNAMIC_DEBUG_MSG = "{0}\n\n{1}";

	public const string U_TEXT_FMT_FPS = "FPS: <color=orange>{0:0.0}</color> <color=green>[{1}]</color>";
	public const string U_TEXT_FMT_FRAME_TIME = "Frame Time: <color=orange>{0:0.0}</color> ms";
	public const string U_TEXT_FMT_DEVICE_INFO = "Graphics Device: <color=orange>{0}</color> <color=green>[{1}]</color>";

	public const string U_TEXT_FMT_STATIC_DEBUG_INFO_01 = "Screen Size: <color=orange>{0:0.0}</color>, <color=orange>{1:0.0}</color>\n";
	public const string U_TEXT_FMT_STATIC_DEBUG_INFO_02 = "Canvas Size: <color=orange>{0:0.0}</color>, <color=orange>{1:0.0}</color>\n";
	public const string U_TEXT_FMT_STATIC_DEBUG_INFO_03 = "SafeArea Offset: <color=orange>{0:0.0}</color>, <color=orange>{1:0.0}</color>, <color=orange>{2:0.0}</color>, <color=orange>{3:0.0}</color>\n";
	public const string U_TEXT_FMT_STATIC_DEBUG_INFO_04 = "Quality Level: <color=orange>{0}</color>, Target Frame Rate: <color=orange>{1}</color>\n";
	public const string U_TEXT_FMT_STATIC_DEBUG_INFO_05 = "Persistent Data Path: <color=orange>{0}</color>";
	public const string U_TEXT_FMT_STATIC_DEBUG_INFO_06 = "\nScreen DPI: <color=orange>{0:0.0}</color>, Banner Ads Height: <color=orange>{1:0.0}</color>";

	public const string U_TEXT_FMT_DYNAMIC_DEBUG_INFO_01 = "Used Heap: <color=orange>{0:0.0}</color> MB, GPU Alloc: <color=orange>{1:0.0}</color> MB\n";
	public const string U_TEXT_FMT_DYNAMIC_DEBUG_INFO_02 = "Mono Heap: <color=orange>{0:0.0}</color> MB, Mono Used: <color=orange>{1:0.0}</color> MB\n";
	public const string U_TEXT_FMT_DYNAMIC_DEBUG_INFO_03 = "Temp Alloc: <color=orange>{0:0.0}</color> MB, Total Alloc: <color=orange>{1:0.0}</color> MB\n";
	public const string U_TEXT_FMT_DYNAMIC_DEBUG_INFO_04 = "Reserved: <color=orange>{0:0.0}</color> MB, Unused Reserved: <color=orange>{1:0.0}</color> MB\n";
	public const string U_TEXT_FMT_DYNAMIC_DEBUG_INFO_05 = "Time Scale: <color=orange>{0:0.00}</color>";
	// 형식 }

	// 이름 {
	public const string U_OBJ_N_SCREEN_FPS_INFO_UIS = "FPSInfoUIs";
	public const string U_OBJ_N_SCREEN_DEBUG_INFO_UIS = "DebugInfoUIs";

	public const string U_OBJ_N_SCREEN_FPS_TEXT = "FPSText";
	public const string U_OBJ_N_SCREEN_FRAME_TIME_TEXT = "FrameTimeText";
	public const string U_OBJ_N_SCREEN_DEVICE_INFO_TEXT = "DeviceInfoText";

	public const string U_OBJ_N_SCREEN_STATIC_DEBUG_TEXT = "StaticDebugText";
	public const string U_OBJ_N_SCREEN_DYNAMIC_DEBUG_TEXT = "DynamicDebugText";

	public const string U_OBJ_N_SCREEN_FPS_INFO_BTN = "FPSInfoBtn";
	public const string U_OBJ_N_SCREEN_DEBUG_INFO_BTN = "DebugInfoBtn";

	public const string U_OBJ_N_SCREEN_TIME_SCALE_UP_BTN = "TimeScaleUpBtn";
	public const string U_OBJ_N_SCREEN_TIME_SCALE_DOWN_BTN = "TimeScaleDownBtn";
	// 이름 }
#endif // #if DEBUG || DEVELOPMENT_BUILD

#if ADS_MODULE_ENABLE
	// 시간
	public const float U_DELTA_T_ADS_M_ADS_LOAD = 5.0f;
	public const float U_DELTA_T_REWARD_ATI_UPDATE = 0.5f;

	// 식별자 {
	public const string U_KEY_ADS_M_BANNER_ADS_ID = "AdsMBannerAdsID";
	public const string U_KEY_ADS_M_REWARD_ADS_ID = "AdsMRewardAdsID";
	public const string U_KEY_ADS_M_FULLSCREEN_ADS_ID = "AdsMFullscreenAdsID";

	public const string U_KEY_FMT_ADS_M_LOAD_FAIL_BANNER_ADS_INFO = "AdsMLoadFailBannerAdsInfo_{0}";
	public const string U_KEY_FMT_ADS_M_LOAD_FAIL_REWARD_ADS_INFO = "AdsMLoadFailRewardAdsInfo_{0}";
	public const string U_KEY_FMT_ADS_M_LOAD_FAIL_FULLSCREEN_ADS_INFO = "AdsMLoadFailFullscreenAdsInfo_{0}";
	// 식별자 }

#if ADMOB_ADS_ENABLE
	// 식별자 {
	public const string U_ADS_ID_ADMOB_TEST_BANNER_ADS = "ca-app-pub-3940256099942544/6300978111";
	public const string U_ADS_ID_ADMOB_TEST_REWARD_ADS = "ca-app-pub-3940256099942544/5224354917";
	public const string U_ADS_ID_ADMOB_TEST_FULLSCREEN_ADS = "ca-app-pub-3940256099942544/1033173712";

	public const string U_KEY_ADS_M_ADMOB_INIT_CALLBACK = "AdsMAdmobInitCallback";

	public const string U_KEY_ADS_M_ADMOB_BANNER_ADS_LOAD_CALLBACK = "AdsMAdmobBannerAdsLoadCallback";
	public const string U_KEY_ADS_M_ADMOB_BANNER_ADS_LOAD_FAIL_CALLBACK = "AdsMAdmobBannerAdsLoadFailCallback";
	public const string U_KEY_ADS_M_ADMOB_BANNER_ADS_CLOSE_CALLBACK = "AdsMAdmobBannerAdsCloseCallback";

	public const string U_KEY_ADS_M_ADMOB_REWARD_ADS_LOAD_CALLBACK = "AdsMAdmobRewardAdsLoadCallback";
	public const string U_KEY_ADS_M_ADMOB_REWARD_ADS_LOAD_FAIL_CALLBACK = "AdsMAdmobRewardAdsLoadFailCallback";
	public const string U_KEY_ADS_M_ADMOB_REWARD_ADS_CLOSE_CALLBACK = "AdsMAdmobRewardAdsCloseCallback";
	public const string U_KEY_ADS_M_ADMOB_REWARD_ADS_RECEIVE_ADS_REWARD_CALLBACK = "AdsMAdmobRewardAdsReceiveAdsRewardCallback";

	public const string U_KEY_ADS_M_ADMOB_FULLSCREEN_ADS_LOAD_CALLBACK = "AdsMAdmobFullscreenAdsLoadCallback";
	public const string U_KEY_ADS_M_ADMOB_FULLSCREEN_ADS_LOAD_FAIL_CALLBACK = "AdsMAdmobFullscreenAdsLoadFailCallback";
	public const string U_KEY_ADS_M_ADMOB_FULLSCREEN_ADS_CLOSE_CALLBACK = "AdsMAdmobFullscreenAdsCloseCallback";
	// 식별자 }
#endif // #if ADMOB_ADS_ENABLE

#if IRON_SRC_ADS_ENABLE
	// 식별자 {
	public const string U_KEY_ADS_M_IRON_SRC_INIT_CALLBACK = "AdsMIronSrcInitCallback";

	public const string U_KEY_ADS_M_IRON_SRC_BANNER_ADS_LOAD_CALLBACK = "AdsMIronSrcBannerAdsLoadCallback";
	public const string U_KEY_ADS_M_IRON_SRC_BANNER_ADS_LOAD_FAIL_CALLBACK = "AdsMIronSrcBannerAdsLoadFailCallback";

	public const string U_KEY_ADS_M_IRON_SRC_REWARD_ADS_CLOSE_CALLBACK = "AdsMIronSrcRewardAdsCloseCallback";
	public const string U_KEY_ADS_M_IRON_SRC_REWARD_ADS_RECEIVE_ADS_REWARD_CALLBACK = "AdsMIronSrcRewardAdsReceiveAdsRewardCallback";
	public const string U_KEY_ADS_M_IRON_SRC_REWARD_ADS_CHANGE_STATE_CALLBACK = "AdsMIronSrcRewardAdsChangeStateCallback";

	public const string U_KEY_ADS_M_IRON_SRC_FULLSCREEN_ADS_LOAD_CALLBACK = "AdsMIronSrcFullscreenAdsLoadCallback";
	public const string U_KEY_ADS_M_IRON_SRC_FULLSCREEN_ADS_LOAD_FAIL_CALLBACK = "AdsMIronSrcFullscreenAdsLoadFailCallback";
	public const string U_KEY_ADS_M_IRON_SRC_FULLSCREEN_ADS_CLOSE_CALLBACK = "AdsMIronSrcFullscreenAdsCloseCallback";
	// 식별자 }
#endif // #if IRON_SRC_ADS_ENABLE
#endif // #if ADS_MODULE_ENABLE

#if FLURRY_MODULE_ENABLE
	// 시간
	public const long U_TIMEOUT_FLURRY_M_NETWORK_CONNECTION = 60 * KCDefine.B_UNIT_MILLI_SECS_PER_SEC;

	// 식별자
	public const string U_KEY_FLURRY_M_INIT_CALLBACK = "FlurryMInitCallback";
#endif // #if FLURRY_MODULE_ENABLE

#if FACEBOOK_MODULE_ENABLE
	// 식별자
	public const string U_KEY_FACEBOOK_M_INIT_CALLBACK = "FacebookMInitCallback";
	public const string U_KEY_FACEBOOK_M_LOGIN_CALLBACK = "FacebookMLoginCallback";
	public const string U_KEY_FACEBOOK_M_LOGOUT_CALLBACK = "FacebookMLogoutCallback";
	public const string U_KEY_FACEBOOK_M_VIEW_STATE_SHOW_CALLBACK = "FacebookMViewStateShowCallback";
	public const string U_KEY_FACEBOOK_M_VIEW_STATE_CLOSE_CALLBACK = "FacebookMViewStateCloseCallback";
#endif // #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
	// 식별자
	public const string U_KEY_FIREBASE_M_INIT_CALLBACK = "FirebaseMInitCallback";
	public const string U_KEY_FIREBASE_M_LOGOUT_CALLBACK = "FirebaseMLogoutCallback";

	// 노드
	public const string U_NODE_FIREBASE_USER_INFOS = "UserInfos";
	public const string U_NODE_FIREBASE_TARGET_INFOS = "TargetInfos";
	public const string U_NODE_FIREBASE_PURCHASE_INFOS = "PurchaseInfos";

#if FIREBASE_AUTH_ENABLE
	// 식별자 {
	public const string U_KEY_FIREBASE_M_LOGIN_CALLBACK = "FirebaseMLoginCallback";

#if UNITY_IOS
	public const string U_PROVIDER_ID_FIREBASE_M_APPLE_LOGIN = "apple.com";
	public const string U_KEY_FIREBASE_M_RECEIVE_GAME_CENTER_CREDENTIAL_CALLBACK = "FirebaseMReceiveGameCenterCredentialCallback";
#endif // #if UNITY_IOS
	// 식별자 }
#endif // #if FIREBASE_AUTH_ENABLE

#if FIREBASE_DB_ENABLE
	// 식별자
	public const string U_KEY_FIREBASE_M_LOAD_DATAS_CALLBACK = "FirebaseMLoadDatasCallback";
	public const string U_KEY_FIREBASE_M_SAVE_DATAS_CALLBACK = "FirebaseMSaveDatasCallback";
#endif // #if FIREBASE_DB_ENABLE

#if FIREBASE_MSG_ENABLE
	// 식별자 {
	public const string U_KEY_FIREBASE_M_TOKEN_CALLBACK = "FirebaseMTokenCallback";
	public const string U_KEY_FIREBASE_M_NOTI_MSG_CALLBACK = "FirebaseMNotiMsgCallback";

	public const string U_KEY_FIREBASE_M_LOAD_MSG_TOKEN_CALLBACK = "FirebaseMLoadMsgTokenCallback";
	// 식별자 }
#endif // #if FIREBASE_MSG_ENABLE

#if FIREBASE_CONFIG_ENABLE
	// 식별자
	public const string U_KEY_FIREBASE_M_SETUP_DEF_CONFIGS_CALLBACK = "FirebaseMSetupDefConfigsCallback";
	public const string U_KEY_FIREBASE_M_LOAD_CONFIGS_CALLBACK = "FirebaseMLoadConfigsCallback";
#endif // #if FIREBASE_CONFIG_ENABLE

#if FIREBASE_STORAGE_ENABLE
	// 식별자
	public const string U_KEY_FIREBASE_M_LOAD_FILES_CALLBACK = "FirebaseMLoadFilesCallback";
#endif // #if FIREBASE_STORAGE_ENABLE
#endif // #if FIREBASE_MODULE_ENABLE

#if APPS_FLYER_MODULE_ENABLE
	// 시간
	public const int U_TIMEOUT_APPS_FM_AGREE_TRACKING = 60;

	// 식별자
	public const string U_KEY_APPS_FM_INIT_CALLBACK = "AppsFMInitCallback";
#endif // #if APPS_FLYER_MODULE_ENABLE

#if GAME_CENTER_MODULE_ENABLE
	// 식별자
	public const string U_KEY_GAME_CM_INIT_CALLBACK = "GameCMInitCallback";
	public const string U_KEY_GAME_CM_LOGIN_CALLBACK = "GameCMLoginCallback";
	public const string U_KEY_GAME_CM_LOGOUT_CALLBACK = "GameCMLogoutCallback";
	public const string U_KEY_GAME_CM_UPDATE_RECORD_CALLBACK = "GameCMUpdateRecordCallback";
	public const string U_KEY_GAME_CM_UPDATE_ACHIEVEMENT_CALLBACK = "GameCMUpdateAchievementCallback";
	public const string U_KEY_GAME_CM_RECEIVE_SERVER_SIDE_ACCESS_RESULT_CALLBACK = "GameCMReceiveServerSideAccessResultCallback";
#endif // #if GAME_CENTER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
	// 기타
    public const string U_ENVIRONMENT_N_DEV = "development";
	public const string U_ENVIRONMENT_N_PRODUCTION = "production";
	public const string U_PAYLOAD_PURCHASE_M_PURCHASE = "PurchaseMPurchase";

	// 식별자 {
	public const string U_KEY_PURCHASE_M_INIT_CALLBACK = "PurchaseMInitCallback";
	public const string U_KEY_PURCHASE_M_INIT_FAIL_CALLBACK = "PurchaseMInitFailCallback";
	public const string U_KEY_PURCHASE_M_PURCHASE_FAIL_CALLBACK = "PurchaseMPurchaseFailCallback";

	public const string U_KEY_PURCHASE_M_CONFIRM_CALLBACK = "PurchaseMConfirmCallback";
	public const string U_KEY_PURCHASE_M_RESTORE_CALLBACK = "PurchaseMRestoreCallback";
	public const string U_KEY_PURCHASE_M_HANDLE_PURCHASE_RESULT_CALLBACK = "PurchaseMHandlePurchaseResultCallback";
	// 식별자 }
#endif // #if PURCHASE_MODULE_ENABLE

#if NOTI_MODULE_ENABLE
	// 시간
	public const float U_DELTA_T_NOTI_M_REQUEST_CHECK = 0.15f;
	public const float U_MAX_DELTA_T_NOTI_M_REQUEST_CHECK = 0.5f;

	// 식별자
	public const string U_KEY_NOTI_M_INIT_CALLBACK = "NotiMInitCallback";

#if UNITY_IOS
	// 옵션
	public const PresentationOption B_OPTS_PRESENTATION = PresentationOption.Alert | PresentationOption.Sound;
	public const AuthorizationOption B_OPTS_AUTHORIZATION = AuthorizationOption.Alert | AuthorizationOption.Badge | AuthorizationOption.Sound;
#elif UNITY_ANDROID
	// 그룹 정보
	public const string U_GROUP_N_NOTI = "NotiMNotiGroup";
	public const string U_GROUP_DESC_NOTI = KCDefine.U_GROUP_N_NOTI;
#endif // #if UNITY_IOS
#endif // #if NOTI_ENABLE

#if PLAYFAB_MODULE_ENABLE
	// 개수
	public const int U_MAX_NUM_PLAYFAB_M_NOTICES = 10;
	public const int U_MAX_NUM_PLAYFAB_M_STATISTICS = 100;

	// 식별자
	public const string U_KEY_PLAYFAB_M_INIT_CALLBACK = "PlayfabMInitCallback";
	public const string U_KEY_PLAYFAB_M_LOGIN_CALLBACK = "PlayfabMLoginCallback";
	public const string U_KEY_PLAYFAB_M_LOGOUT_CALLBACK = "PlayfabMLogoutCallback";
	public const string U_KEY_PLAYFAB_M_LOAD_SERVER_TIME_CALLBACK = "PlayfabMLoadServerTimeCallback";
#endif // #if PLAYFAB_MODULE_ENABLE

#if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE
	// 이름
	public const string U_FIELD_N_CLEAR_DEPTH = "m_ClearDepth";
#endif // #if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE

#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	// 이름
	public const string U_COL_N_GOOGLE_SHEET_SRC = "B";
	public const string U_COL_N_GOOGLE_SHEET_DEST = "BZ";
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	#endregion // 조건부 상수

	#region 조건부 런타임 상수
#if UNITY_IOS
	// 버전
	public static readonly System.Version U_MIN_VER_HAPTIC_FEEDBACK = new System.Version(10, 0, 0);
	public static readonly System.Version U_MIN_VER_TRACKING_CONSENT_VIEW = new System.Version(14, 0, 0);

	// 햅틱 피드백 지원 모델
	public static readonly List<DeviceGeneration> U_HAPTIC_FEEDBACK_SUPPORTS_MODEL_LIST = new List<DeviceGeneration>() {
		DeviceGeneration.iPhone7, DeviceGeneration.iPhone7Plus, DeviceGeneration.iPhone8, DeviceGeneration.iPhone8Plus, DeviceGeneration.iPhoneX, DeviceGeneration.iPhoneXR, DeviceGeneration.iPhoneXS, DeviceGeneration.iPhoneXSMax, DeviceGeneration.iPhone11, DeviceGeneration.iPhone11Pro, DeviceGeneration.iPhone11ProMax, DeviceGeneration.iPhoneUnknown
	};
#endif // #if UNITY_IOS

#if ADS_MODULE_ENABLE
#if ADMOB_ADS_ENABLE
	// 크기
	public static readonly AdSize U_SIZE_ADMOB_BANNER_ADS = new AdSize((int)KCDefine.U_SIZE_BANNER_ADS.x, (int)KCDefine.U_SIZE_BANNER_ADS.y);
#endif // #if ADMOB_ADS_ENABLE

#if IRON_SRC_ADS_ENABLE
	// 크기
	public static readonly IronSourceBannerSize U_SIZE_IRON_SRC_BANNER_ADS = new IronSourceBannerSize((int)KCDefine.U_SIZE_BANNER_ADS.x, (int)KCDefine.U_SIZE_BANNER_ADS.y);
#endif // #if IRON_SRC_ADS_ENABLE
#endif // #if ADS_MODULE_ENABLE

#if ADS_MODULE_ENABLE || FLURRY_MODULE_ENABLE || APPS_FLYER_MODULE_ENABLE
	// 경로
	public static readonly string U_ASSET_P_G_PLUGIN_INFO_TABLE = $"{KCDefine.B_DIR_P_SCRIPTABLES}{KCDefine.B_DIR_P_GLOBAL}G_PluginInfoTable";
#endif // #if ADS_MODULE_ENABLE || FLURRY_MODULE_ENABLE || APPS_FLYER_MODULE_ENABLE

#if FACEBOOK_MODULE_ENABLE
	// 식별자
	public static readonly List<string> U_KEY_FACEBOOK_PERMISSION_LIST = new List<string>() {
		"public_profile"
	};
#endif // #if FACEBOOK_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
	// 시간
	public static readonly System.TimeSpan U_TIMEOUT_FIREBASE_SESSION = new System.TimeSpan(0, 0, 60);
#endif // #if FIREBASE_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
	// 경로
	public static readonly string U_DATA_P_PURCHASE_PRODUCT_IDS = $"{KCDefine.B_DIR_P_WRITABLE}PurchaseProductIDs.bytes";
#endif // #if PURCHASE_MODULE_ENABLE

#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	// 이름
	public static readonly string U_CELL_N_FMT_GOOGLE_SHEET_SRC = $"{KCDefine.U_COL_N_GOOGLE_SHEET_SRC}{"{0}"}";
	public static readonly string U_CELL_N_FMT_GOOGLE_SHEET_DEST = $"{KCDefine.U_COL_N_GOOGLE_SHEET_DEST}{"{0}"}";
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	#endregion // 조건부 런타임 상수
}

/** 초기화 씬 상수 */
public static partial class KCDefine {
	#region 기본

	#endregion // 기본

	#region 런타임 상수
	// 경로
	public static readonly string IS_OBJ_P_SCREEN_BLIND_UIS = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_INIT_SCENE}IS_ScreenBlindUIs";
	public static readonly string IS_OBJ_P_SCREEN_BLIND_IMG = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_INIT_SCENE}IS_ScreenBlindImg";
	#endregion // 런타임 상수
}

/** 시작 씬 상수 */
public static partial class KCDefine {
	#region 기본
	// 이름
	public const string SS_FUNC_N_START_SCENE_EVENT = "OnReceiveStartSceneEvent";
	#endregion // 기본

	#region 런타임 상수
	// 경로
	public static readonly string SS_OBJ_P_LOADING_TEXT = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_START_SCENE}SS_LoadingText";
	public static readonly string SS_OBJ_P_LOADING_GAUGE = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_START_SCENE}SS_LoadingGauge";
	#endregion // 런타임 상수
}

/** 설정 씬 상수 */
public static partial class KCDefine {
	#region 기본
	// 이름
	public const string SS_OBJ_N_TIMER_MANAGER = "TimerManager";
	#endregion // 기본

	#region 런타임 상수
	// 경로
	public static readonly string SS_OBJ_P_SCREEN_DEBUG_UIS = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_SETUP_SCENE}SS_ScreenDebugUIs";
	public static readonly string SS_OBJ_P_SCREEN_POPUP_UIS = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_SETUP_SCENE}SS_ScreenPopupUIs";
	public static readonly string SS_OBJ_P_SCREEN_TOPMOST_UIS = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_SETUP_SCENE}SS_ScreenTopmostUIs";
	public static readonly string SS_OBJ_P_SCREEN_ABS_UIS = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_SETUP_SCENE}SS_ScreenAbsUIs";
	#endregion // 런타임 상수
}

/** 약관 동의 씬 상수 */
public static partial class KCDefine {
	#region 기본
	// 약관 동의 팝업
	public const string AS_OBJ_N_AGREE_POPUP = "AgreePopup";
	#endregion // 기본

	#region 런타임 상수
	// 경로 {
	public static readonly string AS_DATA_P_PRIVACY = $"{KCDefine.B_DIR_P_DATAS}{KCDefine.B_DIR_P_AGREE_SCENE}AS_Privacy_{SystemLanguage.Korean}";
	public static readonly string AS_DATA_P_SERVICES = $"{KCDefine.B_DIR_P_DATAS}{KCDefine.B_DIR_P_AGREE_SCENE}AS_Services_{SystemLanguage.Korean}";

	public static readonly string AS_OBJ_P_PORTRAIT_AGREE_POPUP = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_AGREE_SCENE}AS_PortraitAgreePopup";
	public static readonly string AS_OBJ_P_LANDSCAPE_AGREE_POPUP = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_AGREE_SCENE}AS_LandscapeAgreePopup";
	// 경로 }
	#endregion // 런타임 상수
}

/** 지연 설정 씬 상수 */
public static partial class KCDefine {
	#region 기본
	// 추적 설명 팝업
	public const string LSS_OBJ_N_TRACKING_DESC_POPUP = "TrackingDescPopup";
	#endregion // 기본

	#region 런타임 상수
	// 경로
	public static readonly string LSS_OBJ_P_TRACKING_DESC_POPUP = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_LATE_SETUP_SCENE}LSS_TrackingDescPopup";
	#endregion // 런타임 상수
}

/** 타이틀 씬 상수 */
public static partial class KCDefine {
	#region 기본

	#endregion // 기본
}

/** 메인 씬 상수 */
public static partial class KCDefine {
	#region 기본

	#endregion // 기본

	#region 런타임 상수
	// 경로
	public static readonly string MS_OBJ_P_LEVEL_SCROLLER_CELL_VIEW = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_MAIN_SCENE}MS_LevelScrollerCellView";
	public static readonly string MS_OBJ_P_STAGE_SCROLLER_CELL_VIEW = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_MAIN_SCENE}MS_StageScrollerCellView";
	public static readonly string MS_OBJ_P_CHAPTER_SCROLLER_CELL_VIEW = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_MAIN_SCENE}MS_ChapterScrollerCellView";
	#endregion // 런타임 상수
}

/** 게임 씬 상수 */
public static partial class KCDefine {
	#region 기본

	#endregion // 기본
}

/** 로딩 씬 상수 */
public static partial class KCDefine {
	#region 기본

	#endregion // 기본
}

/** 중첩 씬 상수 */
public static partial class KCDefine {
	#region 기본

	#endregion // 기본
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE
/** 서브 열거형 값 */
public static partial class KEnumVal {
	#region 기본
	// 타겟 종류
	public const int TK_LV_SUB_KINDS_TYPE_VAL = 1;
	public const int TK_EXP_SUB_KINDS_TYPE_VAL = 2;
	public const int TK_NUMS_SUB_KINDS_TYPE_VAL = 3;
	public const int TK_ENHANCE_SUB_KINDS_TYPE_VAL = 4;
	#endregion // 기본

	#region 클래스 함수
	/** 열거형 값을 생성한다 */
	public static int MakeEnumVal(int a_nType, int a_nSubType, int a_nKindsType, int a_nSubKindsType) {
		return ((int)EEnumVal.T * a_nType) + ((int)EEnumVal.ST * a_nSubType) + ((int)EEnumVal.KT * a_nKindsType) + ((int)EEnumVal.SKT * a_nSubKindsType);
	}
	#endregion // 클래스 함수
}

#region 기본
/** 보상 수준 */
public enum ERewardQuality {
	NONE = -1,
	NORM,
	HIGH,
	ULTRA,
	[HideInInspector] MAX_VAL
}

/** 어빌리티 값 타입 */
public enum EAbilityValType {
	NONE = -1,
	NORM,
	INCR,
	DECR,
	[HideInInspector] MAX_VAL
}

/** 보상 획득 팝업 타입 */
public enum ERewardAcquirePopupType {
	NONE = -1,
	FREE,
	DAILY,
	EVENT,
	CLEAR,
	[HideInInspector] MAX_VAL
}

/** 수식 타입 */
public enum ECalcType {
	NONE = -1,
	ABILITY,
	[HideInInspector] MAX_VAL
}

/** 수식 종류 */
public enum ECalcKinds {
	NONE = -1,

	#region 어빌리티
	// 0
	ABILITY_CALC_SAMPLE = (EEnumVal.T * ECalcType.ABILITY) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 어빌리티

	[HideInInspector] MAX_VAL
}

/** 미션 타입 */
public enum EMissionType {
	NONE = -1,
	MAIN,
	FREE,
	DAILY,
	EVENT,
	[HideInInspector] MAX_VAL
}

/** 미션 종류 */
public enum EMissionKinds {
	NONE = -1,

	#region 메인
	// 0
	MAIN_MISSION_SAMPLE = (EEnumVal.T * EMissionType.MAIN) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 메인

	#region 자유
	// 100,000,000
	FREE_MISSION_SAMPLE = (EEnumVal.T * EMissionType.FREE) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 자유

	#region 일일
	// 200,000,000
	DAILY_MISSION_SAMPLE = (EEnumVal.T * EMissionType.DAILY) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 일일

	#region 이벤트
	// 300,000,000
	EVENT_MISSION_SAMPLE = (EEnumVal.T * EMissionType.EVENT) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 이벤트

	[HideInInspector] MAX_VAL
}

/** 보상 타입 */
public enum ERewardType {
	NONE = -1,
	FREE,
	DAILY,
	EVENT,
	CLEAR,
	MISSION,
	TUTORIAL,

    EXTRA_DAILY = 10,
	EXTRA_CLEAR,
	[HideInInspector] MAX_VAL
}

/** 보상 종류 */
public enum ERewardKinds {
	NONE = -1,

	#region 무료
	// 0
	FREE_COINS = (EEnumVal.T * ERewardType.FREE) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 무료

	#region 일일
	// 100,000,000
	ADS_REWARD_DAILY_RUBY = (EEnumVal.T * ERewardType.DAILY) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
    ADS_REWARD_FAIL_RUBY,
	#endregion // 일일

	#region 이벤트
	// 200,000,000
	EVENT_REWARD_SAMPLE = (EEnumVal.T * ERewardType.EVENT) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 이벤트

	#region 클리어
	// 300,000,000
	CLEAR_REWARD_SAMPLE = (EEnumVal.T * ERewardType.CLEAR) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 클리어

	#region 미션
	// 400,000,000
	MISSION_REWARD_SAMPLE = (EEnumVal.T * ERewardType.MISSION) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 미션

	#region 튜토리얼
	// 50,000,000
	TUTORIAL_REWARD_SAMPLE = (EEnumVal.T * ERewardType.TUTORIAL) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 튜토리얼

    #region 추가 일일
	// 1,000,000,000
	EXTRA_DAILY_REWARD_01 = (EEnumVal.T * ERewardType.EXTRA_DAILY) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	EXTRA_DAILY_REWARD_02,
	EXTRA_DAILY_REWARD_03,
	EXTRA_DAILY_REWARD_04,
	EXTRA_DAILY_REWARD_05,
	EXTRA_DAILY_REWARD_06,
	EXTRA_DAILY_REWARD_07,
	#endregion // 추가 일일

	#region 추가 클리어
	EXTRA_CLEAR_REWARD_01 = (EEnumVal.T * ERewardType.EXTRA_CLEAR) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
    #endregion // 추가 클리어

	[HideInInspector] MAX_VAL
}

/** 에피소드 타입 */
public enum EEpisodeType {
	NONE = -1,
	LEVEL,
	STAGE,
	CHAPTER,
	[HideInInspector] MAX_VAL
}

/** 에피소드 종류 */
public enum EEpisodeKinds {
	NONE = -1,

	#region 레벨
	// 0
	LEVEL_NORM = (EEnumVal.T * EEpisodeType.LEVEL) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),

	// 10,000,000
	LEVEL_BOSS = (EEnumVal.T * EEpisodeType.LEVEL) + (EEnumVal.ST * 1) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 레벨

	#region 스테이지
	// 100,000,000
	STAGE_NORM = (EEnumVal.T * EEpisodeType.STAGE) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),

	// 110,000,000
	STAGE_BOSS = (EEnumVal.T * EEpisodeType.STAGE) + (EEnumVal.ST * 1) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 스테이지

	#region 챕터
	// 200,000,000
	CHAPTER_NORM = (EEnumVal.T * EEpisodeType.CHAPTER) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),

	// 210,000,000
	CHAPTER_BOSS = (EEnumVal.T * EEpisodeType.CHAPTER) + (EEnumVal.ST * 1) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 챕터

	[HideInInspector] MAX_VAL
}

/** 튜토리얼 타입 */
public enum ETutorialType {
	NONE = -1,
	PLAY,
	HELP,
	[HideInInspector] MAX_VAL
}

/** 튜토리얼 종류 */
public enum ETutorialKinds {
	NONE = -1,

	#region 플레이
	// 0
	PLAY_TUTORIAL_SAMPLE = (EEnumVal.T * ETutorialType.PLAY) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 플레이

	#region 도움말
	// 100,000,000
	HELP_TUTORIAL_SAMPLE = (EEnumVal.T * ETutorialType.HELP) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 도움말

	[HideInInspector] MAX_VAL
}

/** 리소스 타입 */
public enum EResType {
	NONE = -1,
	FONT,
	SND,
	IMG,
	TEX,
	[HideInInspector] MAX_VAL
}

/** 리소스 종류 */
public enum EResKinds {
	NONE = -1,

	#region 폰트
	// 0
	FONT_KOREAN_01 = (EEnumVal.T * EResType.FONT) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	FONT_ENGLISH_01 = (EEnumVal.T * EResType.FONT) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 1),
	#endregion // 폰트

	#region 사운드
	// 100,000,000
	SND_BG_SCENE_TITLE_01 = (EEnumVal.T * EResType.SND) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	SND_BG_SCENE_MAIN_01 = (EEnumVal.T * EResType.SND) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 1),
	SND_BG_SCENE_GAME_01 = (EEnumVal.T * EResType.SND) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 2),

	// 100,100,000
	SND_FX_TOUCH_BEGIN_01 = (EEnumVal.T * EResType.SND) + (EEnumVal.ST * 0) + (EEnumVal.KT * 1) + (EEnumVal.SKT * 0),
	SND_FX_TOUCH_END_01 = (EEnumVal.T * EResType.SND) + (EEnumVal.ST * 0) + (EEnumVal.KT * 1) + (EEnumVal.SKT * 1),

	// 100,200,000
	SND_FX_POPUP_SHOW_01 = (EEnumVal.T * EResType.SND) + (EEnumVal.ST * 0) + (EEnumVal.KT * 2) + (EEnumVal.SKT * 0),
	SND_FX_POPUP_CLOSE_01 = (EEnumVal.T * EResType.SND) + (EEnumVal.ST * 0) + (EEnumVal.KT * 2) + (EEnumVal.SKT * 1),
	#endregion // 사운드

	#region 이미지
	// 200,000,000
	IMG_WHITE = (EEnumVal.T * EResType.IMG) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	IMG_SPLASH,
	IMG_INDICATOR,
	#endregion // 이미지

	#region 텍스처
	// 300,000,000
	TEX_WHITE = (EEnumVal.T * EResType.TEX) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	TEX_SPLASH,
	TEX_INDICATOR,
	#endregion // 텍스처

	[HideInInspector] MAX_VAL
}

/** 아이템 타입 */
public enum EItemType {
	NONE = -1,
	GOODS,
	CONSUMABLE,
	NON_CONSUMABLE,
	WEAPON,
	ARMOR,
	ACCESSORY,

    EXTRA_CONSUMABLE = 10,
	[HideInInspector] MAX_VAL
}

/** 아이템 종류 */
public enum EItemKinds {
	NONE = -1,

	#region 재화
	// 0
	GOODS_RUBY = (EEnumVal.T * EItemType.GOODS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),

	// 100,000
	GOODS_BOX_COINS = (EEnumVal.T * EItemType.GOODS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 1) + (EEnumVal.SKT * 0),
	#endregion // 재화

	#region 소모
	// 100,000,000
	CONSUMABLE_BOOSTER_SAMPLE = (EEnumVal.T * EItemType.CONSUMABLE) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),

	// 100,100,000
	CONSUMABLE_GAME_ITEM_HINT = (EEnumVal.T * EItemType.CONSUMABLE) + (EEnumVal.ST * 0) + (EEnumVal.KT * 1) + (EEnumVal.SKT * 0),
	CONSUMABLE_GAME_ITEM_CONTINUE = (EEnumVal.T * EItemType.CONSUMABLE) + (EEnumVal.ST * 0) + (EEnumVal.KT * 1) + (EEnumVal.SKT * 1),
	CONSUMABLE_GAME_ITEM_SHUFFLE = (EEnumVal.T * EItemType.CONSUMABLE) + (EEnumVal.ST * 0) + (EEnumVal.KT * 1) + (EEnumVal.SKT * 2),
	#endregion // 소모

	#region 비소모
	// 200,000,000
	NON_CONSUMABLE_REMOVE_ADS = (EEnumVal.T * EItemType.NON_CONSUMABLE) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
    NON_CONSUMABLE_GOLDEN_AIM = (EEnumVal.T * EItemType.NON_CONSUMABLE) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 1),
	#endregion // 비소모

	#region 무기
	// 300,000,000
	WEAPON_ITEM_SAMPLE = (EEnumVal.T * EItemType.WEAPON) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 무기

	#region 방어구
	// 400,000,000
	ARMOR_ITEM_SAMPLE = (EEnumVal.T * EItemType.ARMOR) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 방어구

	#region 악세서리
	// 50,000,000
	ACCESSORY_ITEM_SAMPLE = (EEnumVal.T * EItemType.ACCESSORY) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 악세서리

    #region 추가 소모
	// 1,000,000,000 {
	BOOSTER_ITEM_01_MISSILE = (EEnumVal.T * EItemType.EXTRA_CONSUMABLE) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	BOOSTER_ITEM_02_LIGHTNING,
	BOOSTER_ITEM_03_BOMB,
	BOOSTER_ITEM_04,

	GAME_ITEM_01_EARTHQUAKE = (EEnumVal.T * EItemType.EXTRA_CONSUMABLE) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 1),
	GAME_ITEM_02_ADD_BALLS,
	GAME_ITEM_03_BRICKS_DELETE,
	GAME_ITEM_04_ADD_LASER_BRICKS,
	GAME_ITEM_05_ADD_STEEL_BRICKS,
	GAME_ITEM_06,
	GAME_ITEM_MAX_VAL,
	// 1,000,000,000 }
	#endregion // 추가 소모

	[HideInInspector] MAX_VAL
}

/** 스킬 타입 */
public enum ESkillType {
	NONE = -1,
	ACTION,
	ACTIVE,
	PASSIVE,
	[HideInInspector] MAX_VAL
}

/** 스킬 종류 */
public enum ESkillKinds {
	NONE = -1,

	#region 액션
	// 0
	ACTION_PLAYER_ATK_01_01 = (EEnumVal.T * ESkillType.ACTION) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),

	// 10,000,000
	ACTION_NORM_ENEMY_ATK_01_01 = (EEnumVal.T * ESkillType.ACTION) + (EEnumVal.ST * 1) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),

	// 20,000,000
	ACTION_BOSS_ENEMY_ATK_01_01 = (EEnumVal.T * ESkillType.ACTION) + (EEnumVal.ST * 2) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),

	// 30,000,000
	ACTION_NAMED_ENEMY_ATK_01_01 = (EEnumVal.T * ESkillType.ACTION) + (EEnumVal.ST * 3) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 액션

	#region 액티브
	// 100,000,000
	ACTIVE_SKILL_SAMPLE = (EEnumVal.T * ESkillType.ACTIVE) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 액티브

	#region 패시브
	// 200,000,000
	PASSIVE_SKILL_SAMPLE = (EEnumVal.T * ESkillType.PASSIVE) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 패시브

	[HideInInspector] MAX_VAL
}

/** 객체 타입 */
public enum EObjType {
	NONE = -1,
	BG,
	GROUND,
	OVERLAY,
	PLAYABLE,
	NON_PLAYABLE,

	NORM_BRICKS = 10,
	ITEM_BRICKS,
	SPECIAL_BRICKS,
	OBSTACLE_BRICKS,

	BALL = 20,
	[HideInInspector] MAX_VAL
}

/** 객체 종류 */
public enum EObjKinds {
	NONE = -1,

	#region 배경
	// 0
	BG_EMPTY_01 = (EEnumVal.T * EObjType.BG) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	BG_PLACEHOLDER_01 = (EEnumVal.T * EObjType.BG) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 1),
	#endregion // 배경

	#region 바닥
	// 100,000,000
	NORM_OBJ_SAMPLE = (EEnumVal.T * EObjType.GROUND) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 바닥

	#region 중첩
	// 200,000,000
	OVERLAY_OBJ_SAMPLE = (EEnumVal.T * EObjType.OVERLAY) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 중첩

	#region 플레이 가능
	// 390,000,000
	PLAYABLE_COMMON_CHARACTER_01 = (EEnumVal.T * EObjType.PLAYABLE) + (EEnumVal.ST * 9) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 플레이 가능

	#region 플레이 불가능
	// 410,000,000
	NON_PLAYABLE_NORM_ENEMY_01_01 = (EEnumVal.T * EObjType.NON_PLAYABLE) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),

	// 420,000,000
	NON_PLAYABLE_BOSS_ENEMY_01_01 = (EEnumVal.T * EObjType.NON_PLAYABLE) + (EEnumVal.ST * 1) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),

	// 430,000,000
	NON_PLAYABLE_NAMED_ENEMY_01_01 = (EEnumVal.T * EObjType.NON_PLAYABLE) + (EEnumVal.ST * 2) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),

	// 490,000,000
	NON_PLAYABLE_COMMON_CHARACTER_01 = (EEnumVal.T * EObjType.NON_PLAYABLE) + (EEnumVal.ST * 9) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 플레이 불가능

	#region 일반 블럭
	// 1,000,000,000
	NORM_BRICKS_SQUARE_01 = (EEnumVal.T * EObjType.NORM_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),

	// 1,010,000,000
	NORM_BRICKS_TRIANGLE_01 = (EEnumVal.T * EObjType.NORM_BRICKS) + (EEnumVal.ST * 1) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	NORM_BRICKS_TRIANGLE_02,
	NORM_BRICKS_TRIANGLE_03,
	NORM_BRICKS_TRIANGLE_04,

	// 1,020,000,000
	NORM_BRICKS_RIGHT_TRIANGLE_01 = (EEnumVal.T * EObjType.NORM_BRICKS) + (EEnumVal.ST * 2) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	NORM_BRICKS_RIGHT_TRIANGLE_02,
	NORM_BRICKS_RIGHT_TRIANGLE_03,
	NORM_BRICKS_RIGHT_TRIANGLE_04,

    // 1,030,000,000
    NORM_BRICKS_DIAMOND_01 = (EEnumVal.T * EObjType.NORM_BRICKS) + (EEnumVal.ST * 3) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 일반 블럭

	#region 아이템 블럭
	// 1,100,000,000
	ITEM_BRICKS_BALL_01 = (EEnumVal.T * EObjType.ITEM_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	ITEM_BRICKS_BALL_02,
	ITEM_BRICKS_BALL_03,
    ITEM_BRICKS_BALL_04,

	// 1,100,100,000
	ITEM_BRICKS_BOOSTER_01 = (EEnumVal.T * EObjType.ITEM_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 1) + (EEnumVal.SKT * 0),

    // 1,100,200,000
    ITEM_BRICKS_COINS_01 = (EEnumVal.T * EObjType.ITEM_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 2) + (EEnumVal.SKT * 0), 
	#endregion // 아이템 블럭

	#region 특수 블럭
	// 1,200,000,000
	SPECIAL_BRICKS_LASER_HORIZONTAL_01 = (EEnumVal.T * EObjType.SPECIAL_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
    SPECIAL_BRICKS_LASER_HORIZONTAL_02,
    // 1,200,000,100
	SPECIAL_BRICKS_LASER_VERTICAL_01 = (EEnumVal.T * EObjType.SPECIAL_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 1),
    SPECIAL_BRICKS_LASER_VERTICAL_02,
    // 1,200,000,200
	SPECIAL_BRICKS_LASER_CROSS_01 = (EEnumVal.T * EObjType.SPECIAL_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 2),
    SPECIAL_BRICKS_LASER_CROSS_02,

    // 1,200,100,000
    SPECIAL_BRICKS_MISSILE_01 = (EEnumVal.T * EObjType.SPECIAL_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 1) + (EEnumVal.SKT * 0),
    SPECIAL_BRICKS_MISSILE_02,

    // 1,200,200,000
    SPECIAL_BRICKS_EXPLOSION_HORIZONTAL_01 = (EEnumVal.T * EObjType.SPECIAL_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 2) + (EEnumVal.SKT * 0),
    SPECIAL_BRICKS_EXPLOSION_VERTICAL_01 = (EEnumVal.T * EObjType.SPECIAL_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 2) + (EEnumVal.SKT * 1),
    SPECIAL_BRICKS_EXPLOSION_CROSS_01 = (EEnumVal.T * EObjType.SPECIAL_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 2) + (EEnumVal.SKT * 2),
    SPECIAL_BRICKS_EXPLOSION_AROUND_01 = (EEnumVal.T * EObjType.SPECIAL_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 2) + (EEnumVal.SKT * 3),
    SPECIAL_BRICKS_EXPLOSION_ALL_01 = (EEnumVal.T * EObjType.SPECIAL_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 2) + (EEnumVal.SKT * 4), 

    // 1,200,300,000
    SPECIAL_BRICKS_BALL_DIFFUSION_01 = (EEnumVal.T * EObjType.SPECIAL_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 3) + (EEnumVal.SKT * 0),

    // 1,200,400,000
    SPECIAL_BRICKS_BALL_AMPLIFICATION_01 = (EEnumVal.T * EObjType.SPECIAL_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 4) + (EEnumVal.SKT * 0),

    // 1,200,500,000
    SPECIAL_BRICKS_POWERBALL_01 = (EEnumVal.T * EObjType.SPECIAL_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 5) + (EEnumVal.SKT * 0),
    
    // 1,200,600,000
    SPECIAL_BRICKS_ADD_BALL_01 = (EEnumVal.T * EObjType.SPECIAL_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 6) + (EEnumVal.SKT * 0),
    SPECIAL_BRICKS_ADD_BALL_02,
    SPECIAL_BRICKS_ADD_BALL_03,

    // 1,200,700,000
    SPECIAL_BRICKS_LIGHTNING_01 = (EEnumVal.T * EObjType.SPECIAL_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 7) + (EEnumVal.SKT * 0),

    // 1,200,800,000
    SPECIAL_BRICKS_ARROW_01 = (EEnumVal.T * EObjType.SPECIAL_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 8) + (EEnumVal.SKT * 0),
    SPECIAL_BRICKS_ARROW_02,
    SPECIAL_BRICKS_ARROW_03,
    SPECIAL_BRICKS_ARROW_04,
    SPECIAL_BRICKS_ARROW_05,
    SPECIAL_BRICKS_ARROW_06,
    SPECIAL_BRICKS_ARROW_07,
    SPECIAL_BRICKS_ARROW_08,

    // 1,200,900,000
    SPECIAL_BRICKS_EARTHQUAKE_01 = (EEnumVal.T * EObjType.SPECIAL_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 9) + (EEnumVal.SKT * 0),

    // 1,201,000,000
    SPECIAL_BRICKS_MORPH_01 = (EEnumVal.T * EObjType.SPECIAL_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 10) + (EEnumVal.SKT * 0),
    SPECIAL_BRICKS_MORPH_02,
    SPECIAL_BRICKS_MORPH_03,
    SPECIAL_BRICKS_MORPH_04,

	#endregion // 특수 블럭

	#region 장애물 블럭
	// 1,300,000,000
	OBSTACLE_BRICKS_KEY_01 = (EEnumVal.T * EObjType.OBSTACLE_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	OBSTACLE_BRICKS_LOCK_01 = (EEnumVal.T * EObjType.OBSTACLE_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 1),

	// 1,300,100,000
	OBSTACLE_BRICKS_FADE_IN_01 = (EEnumVal.T * EObjType.OBSTACLE_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 1) + (EEnumVal.SKT * 0),
	OBSTACLE_BRICKS_FADE_OUT_01 = (EEnumVal.T * EObjType.OBSTACLE_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 1) + (EEnumVal.SKT * 1),

	// 1,300,200,000 {
	OBSTACLE_BRICKS_WARP_IN_01 = (EEnumVal.T * EObjType.OBSTACLE_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 2) + (EEnumVal.SKT * 0),
	OBSTACLE_BRICKS_WARP_IN_02,
	OBSTACLE_BRICKS_WARP_IN_03,
	OBSTACLE_BRICKS_WARP_IN_04,
	OBSTACLE_BRICKS_WARP_IN_05,

	OBSTACLE_BRICKS_WARP_OUT_01 = (EEnumVal.T * EObjType.OBSTACLE_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 2) + (EEnumVal.SKT * 1),
	OBSTACLE_BRICKS_WARP_OUT_02,
	OBSTACLE_BRICKS_WARP_OUT_03,
	OBSTACLE_BRICKS_WARP_OUT_04,
	OBSTACLE_BRICKS_WARP_OUT_05,
	// 1,300,200,000 }

    // 1,300,300,000
    OBSTACLE_BRICKS_FIX_01 = (EEnumVal.T * EObjType.OBSTACLE_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 3) + (EEnumVal.SKT * 0),
    OBSTACLE_BRICKS_FIX_02,
    OBSTACLE_BRICKS_FIX_03,
    
    // 1,300,400,000
    OBSTACLE_BRICKS_OPEN_01 = (EEnumVal.T * EObjType.OBSTACLE_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 4) + (EEnumVal.SKT * 0),
    OBSTACLE_BRICKS_CLOSE_01 = (EEnumVal.T * EObjType.OBSTACLE_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 4) + (EEnumVal.SKT * 1),

    // 1,300,500,000
    OBSTACLE_BRICKS_RAND_01 = (EEnumVal.T * EObjType.OBSTACLE_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 5) + (EEnumVal.SKT * 0),
    OBSTACLE_BRICKS_RAND_02,
    OBSTACLE_BRICKS_RAND_03,
    OBSTACLE_BRICKS_RAND_04,

    // 1,300,600,000
    OBSTACLE_BRICKS_REFRACT_01 = (EEnumVal.T * EObjType.OBSTACLE_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 6) + (EEnumVal.SKT * 0),

    // 1,300,700,000
    OBSTACLE_BRICKS_WOODBOX_01 = (EEnumVal.T * EObjType.OBSTACLE_BRICKS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 7) + (EEnumVal.SKT * 0),
    OBSTACLE_BRICKS_WOODBOX_02,

    #endregion // 장애물 블럭

	#region 공
	// 2,000,000,000
	BALL_NORM_01 = (EEnumVal.T * EObjType.BALL) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
    BALL_NORM_02, // 유지되는 추가볼.
    BALL_NORM_03, // 사라지는 추가볼.

    // 2,010,000,000 {
	BALL_BOOSTER_SHIELD_01 = (EEnumVal.T * EObjType.BALL) + (EEnumVal.ST * 1) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	BALL_BOOSTER_POWER_01 = (EEnumVal.T * EObjType.BALL) + (EEnumVal.ST * 1) + (EEnumVal.KT * 1) + (EEnumVal.SKT * 0),

	BALL_BOOSTER_SHOT_01 = (EEnumVal.T * EObjType.BALL) + (EEnumVal.ST * 1) + (EEnumVal.KT * 2) + (EEnumVal.SKT * 0),
	BALL_BOOSTER_SHOT_02,
	// 2,010,000,000 }
	#endregion // 공

	[HideInInspector] MAX_VAL
}

/** 효과 타입 */
public enum EFXType {
	NONE = -1,
	HIT,
	BUFF,
	DEBUFF,
	[HideInInspector] MAX_VAL
}

/** 효과 종류 */
public enum EFXKinds {
	NONE = -1,

	#region 타격
	// 0
	HIT_FX_SAMPLE = (EEnumVal.T * EFXType.HIT) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 타격

	#region 버프
	// 100,000,000
	BUFF_FX_SAMPLE = (EEnumVal.T * EFXType.BUFF) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 버프

	#region 디버프
	// 200,000,000
	DEBUFF_FX_SAMPLE = (EEnumVal.T * EFXType.DEBUFF) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 디버프

	[HideInInspector] MAX_VAL
}

/** 어빌리티 타입 */
public enum EAbilityType {
	NONE = -1,
	STAT,
	BUFF,
	DEBUFF,
	UPGRADE,
	[HideInInspector] MAX_VAL
}

/** 어빌리티 종류 */
public enum EAbilityKinds {
	NONE = -1,

	#region 스탯
	// 0 {
	STAT_LV = (EEnumVal.T * EAbilityType.STAT) + (EEnumVal.ST * EAbilityValType.NORM) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	STAT_EXP = (EEnumVal.T * EAbilityType.STAT) + (EEnumVal.ST * EAbilityValType.NORM) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 1),
	STAT_NUMS = (EEnumVal.T * EAbilityType.STAT) + (EEnumVal.ST * EAbilityValType.NORM) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 2),
	STAT_ENHANCE = (EEnumVal.T * EAbilityType.STAT) + (EEnumVal.ST * EAbilityValType.NORM) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 3),

	STAT_HP_01 = (EEnumVal.T * EAbilityType.STAT) + (EEnumVal.ST * EAbilityValType.NORM) + (EEnumVal.KT * 1) + (EEnumVal.SKT * 0), STAT_HP_02,
	STAT_MP_01 = (EEnumVal.T * EAbilityType.STAT) + (EEnumVal.ST * EAbilityValType.NORM) + (EEnumVal.KT * 1) + (EEnumVal.SKT * 1), STAT_MP_02,
	STAT_SP_01 = (EEnumVal.T * EAbilityType.STAT) + (EEnumVal.ST * EAbilityValType.NORM) + (EEnumVal.KT * 1) + (EEnumVal.SKT * 2), STAT_SP_02,

	STAT_RECOVERY_01 = (EEnumVal.T * EAbilityType.STAT) + (EEnumVal.ST * EAbilityValType.NORM) + (EEnumVal.KT * 2) + (EEnumVal.SKT * 0), STAT_RECOVERY_02,
	STAT_HP_RECOVERY_01 = (EEnumVal.T * EAbilityType.STAT) + (EEnumVal.ST * EAbilityValType.NORM) + (EEnumVal.KT * 2) + (EEnumVal.SKT * 1), STAT_HP_RECOVERY_02,
	STAT_MP_RECOVERY_01 = (EEnumVal.T * EAbilityType.STAT) + (EEnumVal.ST * EAbilityValType.NORM) + (EEnumVal.KT * 2) + (EEnumVal.SKT * 2), STAT_MP_RECOVERY_02,
	STAT_SP_RECOVERY_01 = (EEnumVal.T * EAbilityType.STAT) + (EEnumVal.ST * EAbilityValType.NORM) + (EEnumVal.KT * 2) + (EEnumVal.SKT * 3), STAT_SP_RECOVERY_02,

	STAT_ATK_01 = (EEnumVal.T * EAbilityType.STAT) + (EEnumVal.ST * EAbilityValType.NORM) + (EEnumVal.KT * 3) + (EEnumVal.SKT * 0), STAT_ATK_02,
	STAT_P_ATK_01 = (EEnumVal.T * EAbilityType.STAT) + (EEnumVal.ST * EAbilityValType.NORM) + (EEnumVal.KT * 3) + (EEnumVal.SKT * 1), STAT_P_ATK_02,
	STAT_M_ATK_01 = (EEnumVal.T * EAbilityType.STAT) + (EEnumVal.ST * EAbilityValType.NORM) + (EEnumVal.KT * 3) + (EEnumVal.SKT * 2), STAT_M_ATK_02,

	STAT_DEF_01 = (EEnumVal.T * EAbilityType.STAT) + (EEnumVal.ST * EAbilityValType.NORM) + (EEnumVal.KT * 4) + (EEnumVal.SKT * 0), STAT_DEF_02,
	STAT_P_DEF_01 = (EEnumVal.T * EAbilityType.STAT) + (EEnumVal.ST * EAbilityValType.NORM) + (EEnumVal.KT * 4) + (EEnumVal.SKT * 1), STAT_P_DEF_02,
	STAT_M_DEF_01 = (EEnumVal.T * EAbilityType.STAT) + (EEnumVal.ST * EAbilityValType.NORM) + (EEnumVal.KT * 4) + (EEnumVal.SKT * 2), STAT_M_DEF_02,

	STAT_RANGE_01 = (EEnumVal.T * EAbilityType.STAT) + (EEnumVal.ST * EAbilityValType.NORM) + (EEnumVal.KT * 5) + (EEnumVal.SKT * 0), STAT_RANGE_02,
	STAT_ATK_RANGE_01 = (EEnumVal.T * EAbilityType.STAT) + (EEnumVal.ST * EAbilityValType.NORM) + (EEnumVal.KT * 5) + (EEnumVal.SKT * 1), STAT_ATK_RANGE_02,
	STAT_VIEW_RANGE_01 = (EEnumVal.T * EAbilityType.STAT) + (EEnumVal.ST * EAbilityValType.NORM) + (EEnumVal.KT * 5) + (EEnumVal.SKT * 2), STAT_VIEW_RANGE_02,

	STAT_SPEED_01 = (EEnumVal.T * EAbilityType.STAT) + (EEnumVal.ST * EAbilityValType.NORM) + (EEnumVal.KT * 6) + (EEnumVal.SKT * 0), STAT_SPEED_02,
	STAT_ATK_SPEED_01 = (EEnumVal.T * EAbilityType.STAT) + (EEnumVal.ST * EAbilityValType.NORM) + (EEnumVal.KT * 6) + (EEnumVal.SKT * 1), STAT_ATK_SPEED_02,
	STAT_MOVE_SPEED_01 = (EEnumVal.T * EAbilityType.STAT) + (EEnumVal.ST * EAbilityValType.NORM) + (EEnumVal.KT * 6) + (EEnumVal.SKT * 2), STAT_MOVE_SPEED_02,

	STAT_DELAY_01 = (EEnumVal.T * EAbilityType.STAT) + (EEnumVal.ST * EAbilityValType.NORM) + (EEnumVal.KT * 7) + (EEnumVal.SKT * 0), STAT_DELAY_02,
	STAT_ATK_DELAY_01 = (EEnumVal.T * EAbilityType.STAT) + (EEnumVal.ST * EAbilityValType.NORM) + (EEnumVal.KT * 7) + (EEnumVal.SKT * 1), STAT_ATK_DELAY_02,
	STAT_SKILL_DELAY_01 = (EEnumVal.T * EAbilityType.STAT) + (EEnumVal.ST * EAbilityValType.NORM) + (EEnumVal.KT * 7) + (EEnumVal.SKT * 2), STAT_SKILL_DELAY_02,

	STAT_RATE_01 = (EEnumVal.T * EAbilityType.STAT) + (EEnumVal.ST * EAbilityValType.NORM) + (EEnumVal.KT * 8) + (EEnumVal.SKT * 0), STAT_RATE_02,
	STAT_HIT_RATE_01 = (EEnumVal.T * EAbilityType.STAT) + (EEnumVal.ST * EAbilityValType.NORM) + (EEnumVal.KT * 8) + (EEnumVal.SKT * 1), STAT_HIT_RATE_02,
	STAT_AVOID_RATE_01 = (EEnumVal.T * EAbilityType.STAT) + (EEnumVal.ST * EAbilityValType.NORM) + (EEnumVal.KT * 8) + (EEnumVal.SKT * 2), STAT_AVOID_RATE_02,
	STAT_CRITICAL_RATE_01 = (EEnumVal.T * EAbilityType.STAT) + (EEnumVal.ST * EAbilityValType.NORM) + (EEnumVal.KT * 8) + (EEnumVal.SKT * 3), STAT_CRITICAL_RATE_02,
	// 0 }

	// 10,000,000 {
	[System.Obsolete] STAT_LV_INCR = EAbilityKinds.STAT_LV + (int)(EEnumVal.ST * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_EXP_INCR = EAbilityKinds.STAT_EXP + (int)(EEnumVal.ST * (EAbilityValType.INCR - EAbilityValType.NORM)),
	[System.Obsolete] STAT_NUMS_INCR = EAbilityKinds.STAT_NUMS + (int)(EEnumVal.ST * (EAbilityValType.INCR - EAbilityValType.NORM)),
	[System.Obsolete] STAT_ENHANCE_INCR = EAbilityKinds.STAT_ENHANCE + (int)(EEnumVal.ST * (EAbilityValType.INCR - EAbilityValType.NORM)),

	STAT_HP_INCR_01 = EAbilityKinds.STAT_HP_01 + (int)(EEnumVal.ST * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_MP_INCR_01 = EAbilityKinds.STAT_MP_01 + (int)(EEnumVal.ST * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_SP_INCR_01 = EAbilityKinds.STAT_SP_01 + (int)(EEnumVal.ST * (EAbilityValType.INCR - EAbilityValType.NORM)),

	STAT_RECOVERY_INCR_01 = EAbilityKinds.STAT_RECOVERY_01 + (int)(EEnumVal.ST * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_HP_RECOVERY_INCR_01 = EAbilityKinds.STAT_HP_RECOVERY_01 + (int)(EEnumVal.ST * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_MP_RECOVERY_INCR_01 = EAbilityKinds.STAT_MP_RECOVERY_01 + (int)(EEnumVal.ST * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_SP_RECOVERY_INCR_01 = EAbilityKinds.STAT_SP_RECOVERY_01 + (int)(EEnumVal.ST * (EAbilityValType.INCR - EAbilityValType.NORM)),

	STAT_ATK_INCR_01 = EAbilityKinds.STAT_ATK_01 + (int)(EEnumVal.ST * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_P_ATK_INCR_01 = EAbilityKinds.STAT_P_ATK_01 + (int)(EEnumVal.ST * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_M_ATK_INCR_01 = EAbilityKinds.STAT_M_ATK_01 + (int)(EEnumVal.ST * (EAbilityValType.INCR - EAbilityValType.NORM)),

	STAT_DEF_INCR_01 = EAbilityKinds.STAT_DEF_01 + (int)(EEnumVal.ST * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_P_DEF_INCR_01 = EAbilityKinds.STAT_P_DEF_01 + (int)(EEnumVal.ST * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_M_DEF_INCR_01 = EAbilityKinds.STAT_M_DEF_01 + (int)(EEnumVal.ST * (EAbilityValType.INCR - EAbilityValType.NORM)),

	STAT_RANGE_INCR_01 = EAbilityKinds.STAT_RANGE_01 + (int)(EEnumVal.ST * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_ATK_RANGE_INCR_01 = EAbilityKinds.STAT_ATK_RANGE_01 + (int)(EEnumVal.ST * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_VIEW_RANGE_INCR_01 = EAbilityKinds.STAT_VIEW_RANGE_01 + (int)(EEnumVal.ST * (EAbilityValType.INCR - EAbilityValType.NORM)),

	STAT_SPEED_INCR_01 = EAbilityKinds.STAT_SPEED_01 + (int)(EEnumVal.ST * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_ATK_SPEED_INCR_01 = EAbilityKinds.STAT_ATK_SPEED_01 + (int)(EEnumVal.ST * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_MOVE_SPEED_INCR_01 = EAbilityKinds.STAT_MOVE_SPEED_01 + (int)(EEnumVal.ST * (EAbilityValType.INCR - EAbilityValType.NORM)),

	STAT_DELAY_INCR_01 = EAbilityKinds.STAT_DELAY_01 + (int)(EEnumVal.ST * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_ATK_DELAY_INCR_01 = EAbilityKinds.STAT_ATK_DELAY_01 + (int)(EEnumVal.ST * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_SKILL_DELAY_INCR_01 = EAbilityKinds.STAT_SKILL_DELAY_01 + (int)(EEnumVal.ST * (EAbilityValType.INCR - EAbilityValType.NORM)),

	STAT_RATE_INCR_01 = EAbilityKinds.STAT_RATE_01 + (int)(EEnumVal.ST * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_HIT_RATE_INCR_01 = EAbilityKinds.STAT_HIT_RATE_01 + (int)(EEnumVal.ST * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_AVOID_RATE_INCR_01 = EAbilityKinds.STAT_AVOID_RATE_01 + (int)(EEnumVal.ST * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_CRITICAL_RATE_INCR_01 = EAbilityKinds.STAT_CRITICAL_RATE_01 + (int)(EEnumVal.ST * (EAbilityValType.INCR - EAbilityValType.NORM)),
	// 10,000,000 }

	// 10,000,000 {
	[System.Obsolete] STAT_LV_DECR = EAbilityKinds.STAT_LV + (int)(EEnumVal.ST * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_EXP_DECR = EAbilityKinds.STAT_EXP + (int)(EEnumVal.ST * (EAbilityValType.DECR - EAbilityValType.NORM)),
	[System.Obsolete] STAT_NUMS_DECR = EAbilityKinds.STAT_NUMS + (int)(EEnumVal.ST * (EAbilityValType.DECR - EAbilityValType.NORM)),
	[System.Obsolete] STAT_ENHANCE_DECR = EAbilityKinds.STAT_ENHANCE + (int)(EEnumVal.ST * (EAbilityValType.DECR - EAbilityValType.NORM)),

	STAT_HP_DECR_01 = EAbilityKinds.STAT_HP_01 + (int)(EEnumVal.ST * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_MP_DECR_01 = EAbilityKinds.STAT_MP_01 + (int)(EEnumVal.ST * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_SP_DECR_01 = EAbilityKinds.STAT_SP_01 + (int)(EEnumVal.ST * (EAbilityValType.DECR - EAbilityValType.NORM)),

	STAT_RECOVERY_DECR_01 = EAbilityKinds.STAT_RECOVERY_01 + (int)(EEnumVal.ST * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_HP_RECOVERY_DECR_01 = EAbilityKinds.STAT_HP_RECOVERY_01 + (int)(EEnumVal.ST * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_MP_RECOVERY_DECR_01 = EAbilityKinds.STAT_MP_RECOVERY_01 + (int)(EEnumVal.ST * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_SP_RECOVERY_DECR_01 = EAbilityKinds.STAT_SP_RECOVERY_01 + (int)(EEnumVal.ST * (EAbilityValType.DECR - EAbilityValType.NORM)),

	STAT_ATK_DECR_01 = EAbilityKinds.STAT_ATK_01 + (int)(EEnumVal.ST * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_P_ATK_DECR_01 = EAbilityKinds.STAT_P_ATK_01 + (int)(EEnumVal.ST * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_M_ATK_DECR_01 = EAbilityKinds.STAT_M_ATK_01 + (int)(EEnumVal.ST * (EAbilityValType.DECR - EAbilityValType.NORM)),

	STAT_DEF_DECR_01 = EAbilityKinds.STAT_DEF_01 + (int)(EEnumVal.ST * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_P_DEF_DECR_01 = EAbilityKinds.STAT_P_DEF_01 + (int)(EEnumVal.ST * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_M_DEF_DECR_01 = EAbilityKinds.STAT_M_DEF_01 + (int)(EEnumVal.ST * (EAbilityValType.DECR - EAbilityValType.NORM)),

	STAT_RANGE_DECR_01 = EAbilityKinds.STAT_RANGE_01 + (int)(EEnumVal.ST * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_ATK_RANGE_DECR_01 = EAbilityKinds.STAT_ATK_RANGE_01 + (int)(EEnumVal.ST * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_VIEW_RANGE_DECR_01 = EAbilityKinds.STAT_VIEW_RANGE_01 + (int)(EEnumVal.ST * (EAbilityValType.DECR - EAbilityValType.NORM)),

	STAT_SPEED_DECR_01 = EAbilityKinds.STAT_SPEED_01 + (int)(EEnumVal.ST * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_ATK_SPEED_DECR_01 = EAbilityKinds.STAT_ATK_SPEED_01 + (int)(EEnumVal.ST * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_MOVE_SPEED_DECR_01 = EAbilityKinds.STAT_MOVE_SPEED_01 + (int)(EEnumVal.ST * (EAbilityValType.DECR - EAbilityValType.NORM)),

	STAT_DELAY_DECR_01 = EAbilityKinds.STAT_DELAY_01 + (int)(EEnumVal.ST * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_ATK_DELAY_DECR_01 = EAbilityKinds.STAT_ATK_DELAY_01 + (int)(EEnumVal.ST * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_SKILL_DELAY_DECR_01 = EAbilityKinds.STAT_SKILL_DELAY_01 + (int)(EEnumVal.ST * (EAbilityValType.DECR - EAbilityValType.NORM)),

	STAT_RATE_DECR_01 = EAbilityKinds.STAT_RATE_01 + (int)(EEnumVal.ST * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_HIT_RATE_DECR_01 = EAbilityKinds.STAT_HIT_RATE_01 + (int)(EEnumVal.ST * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_AVOID_RATE_DECR_01 = EAbilityKinds.STAT_AVOID_RATE_01 + (int)(EEnumVal.ST * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_CRITICAL_RATE_DECR_01 = EAbilityKinds.STAT_CRITICAL_RATE_01 + (int)(EEnumVal.ST * (EAbilityValType.DECR - EAbilityValType.NORM)),
	// 10,000,000 }
	#endregion // 스탯

	#region 버프
	// 100,000,000
	BUFF_ABILITY_SAMPLE = (EEnumVal.T * EAbilityType.BUFF) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 버프

	#region 디버프
	// 200,000,000
	DEBUFF_ABILITY_SAMPLE = (EEnumVal.T * EAbilityType.DEBUFF) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 디버프

	#region 업그레이드
	// 300,000,000
	UPGRADE_ABILITY_SAMPLE = (EEnumVal.T * EAbilityType.UPGRADE) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 업그레이드

	[HideInInspector] MAX_VAL
}

/** 상품 타입 */
public enum EProductType {
	NONE = -1,
	PKGS,
	SINGLE,
	[HideInInspector] MAX_VAL
}

/** 상품 종류 */
public enum EProductKinds {
	NONE = -1,

	#region 패키지
	// 0
	PKGS_PRODUCT_POWER_PACKAGE_1 = (EEnumVal.T * EProductType.PKGS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
    PKGS_PRODUCT_POWER_PACKAGE_2,
    PKGS_PRODUCT_POWER_PACKAGE_3,
    // 100
	PKGS_PRODUCT_SPECIAL_PACKAGE_1 = (EEnumVal.T * EProductType.PKGS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 1),
    PKGS_PRODUCT_SPECIAL_PACKAGE_2,
    PKGS_PRODUCT_SPECIAL_PACKAGE_3,
    // 200
	//PKGS_PRODUCT_SMASH_01 = (EEnumVal.T * EProductType.PKGS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 2),
	//PKGS_PRODUCT_SHATTER_01 = (EEnumVal.T * EProductType.PKGS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 3),
	//PKGS_PRODUCT_SPLINTER_01 = (EEnumVal.T * EProductType.PKGS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 4),
	//PKGS_PRODUCT_BURST_01 = (EEnumVal.T * EProductType.PKGS) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 5),

	// 10,000,000
	//PKGS_PRODUCT_SPECIAL_STARTER_01 = (EEnumVal.T * EProductType.PKGS) + (EEnumVal.ST * 1) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 패키지

	#region 단일
	// 100,000,000 {
	SINGLE_PRODUCT_GEMS_01 = (EEnumVal.T * EProductType.SINGLE) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	SINGLE_PRODUCT_GEMS_02,
	SINGLE_PRODUCT_GEMS_03,
	SINGLE_PRODUCT_GEMS_04,
	SINGLE_PRODUCT_GEMS_05,
	SINGLE_PRODUCT_GEMS_06,

	//SINGLE_PRODUCT_COINS_BOX_01 = (EEnumVal.T * EProductType.SINGLE) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 1),
    // 100,000,000 }

	// 110,000,000
	SINGLE_PRODUCT_REMOVE_ADS = (EEnumVal.T * EProductType.SINGLE) + (EEnumVal.ST * 1) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
    SINGLE_PRODUCT_GOLDEN_AIM = (EEnumVal.T * EProductType.SINGLE) + (EEnumVal.ST * 1) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 1),
	#endregion // 단일

	[HideInInspector] MAX_VAL
}

/** 타겟 타입 */
public enum ETargetType {
	NONE = -1,
	ITEM,
	SKILL,
	OBJ,
	ABILITY,
	[HideInInspector] MAX_VAL
}

/** 타겟 종류 */
public enum ETargetKinds {
	NONE = -1,

	#region 아이템
	// 0
	[System.Obsolete] ITEM = (EEnumVal.T * ETargetType.ITEM) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	ITEM_LV = (EEnumVal.T * ETargetType.ITEM) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 1),
	ITEM_EXP = (EEnumVal.T * ETargetType.ITEM) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 2),
	ITEM_NUMS = (EEnumVal.T * ETargetType.ITEM) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 3),
	ITEM_ENHANCE = (EEnumVal.T * ETargetType.ITEM) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 4),
	#endregion // 아이템

	#region 스킬
	// 100,000,000
	[System.Obsolete] SKILL = (EEnumVal.T * ETargetType.SKILL) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	SKILL_LV = (EEnumVal.T * ETargetType.SKILL) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 1),
	SKILL_EXP = (EEnumVal.T * ETargetType.SKILL) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 2),
	SKILL_NUMS = (EEnumVal.T * ETargetType.SKILL) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 3),
	SKILL_ENHANCE = (EEnumVal.T * ETargetType.SKILL) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 4),
	#endregion // 스킬

	#region 객체
	// 200,000,000
	[System.Obsolete] OBJ = (EEnumVal.T * ETargetType.OBJ) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	OBJ_LV = (EEnumVal.T * ETargetType.OBJ) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 1),
	OBJ_EXP = (EEnumVal.T * ETargetType.OBJ) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 2),
	OBJ_NUMS = (EEnumVal.T * ETargetType.OBJ) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 3),
	OBJ_ENHANCE = (EEnumVal.T * ETargetType.OBJ) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 4),
	#endregion // 객체

	#region 어빌리티
	// 300,000,000
	ABILITY = (EEnumVal.T * ETargetType.ABILITY) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	ABILITY_LV = (EEnumVal.T * ETargetType.ABILITY) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 1),
	[System.Obsolete] ABILITY_EXP = (EEnumVal.T * ETargetType.ABILITY) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 2),
	[System.Obsolete] ABILITY_NUMS = (EEnumVal.T * ETargetType.ABILITY) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 3),
	[System.Obsolete] ABILITY_ENHANCE = (EEnumVal.T * ETargetType.ABILITY) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 4),
	#endregion // 어빌리티

	[HideInInspector] MAX_VAL
}

/** 스킬 적용 타입 */
public enum ESkillApplyType {
	NONE = -1,
	MULTI,
	SINGLE,
	[HideInInspector] MAX_VAL
}

/** 스킬 적용 종류 */
public enum ESkillApplyKinds {
	NONE = -1,

	#region 다중
	// 0
	MULTI_SKILL_APPLY_SAMPLE = (EEnumVal.T * ESkillApplyType.MULTI) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 다중

	#region 단일
	// 100,000,000
	SINGLE_SKILL_APPLY_SAMPLE = (EEnumVal.T * ESkillApplyType.SINGLE) + (EEnumVal.ST * 0) + (EEnumVal.KT * 0) + (EEnumVal.SKT * 0),
	#endregion // 단일

	[HideInInspector] MAX_VAL
}
#endregion // 기본
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE

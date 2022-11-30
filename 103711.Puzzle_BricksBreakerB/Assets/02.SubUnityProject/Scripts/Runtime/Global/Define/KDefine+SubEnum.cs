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
		return ((int)EEnumVal.TYPE * a_nType) + ((int)EEnumVal.SUB_TYPE * a_nSubType) + ((int)EEnumVal.KINDS_TYPE * a_nKindsType) + ((int)EEnumVal.SUB_KINDS_TYPE * a_nSubKindsType);
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
	ABILITY_CALC_SAMPLE = (EEnumVal.TYPE * ECalcType.ABILITY) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
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
	MAIN_MISSION_SAMPLE = (EEnumVal.TYPE * EMissionType.MAIN) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion // 메인

	#region 자유
	// 100,000,000
	FREE_MISSION_SAMPLE = (EEnumVal.TYPE * EMissionType.FREE) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion // 자유

	#region 일일
	// 200,000,000
	DAILY_MISSION_SAMPLE = (EEnumVal.TYPE * EMissionType.DAILY) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion // 일일

	#region 이벤트
	// 300,000,000
	EVENT_MISSION_SAMPLE = (EEnumVal.TYPE * EMissionType.EVENT) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
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
	[HideInInspector] MAX_VAL
}

/** 보상 종류 */
public enum ERewardKinds {
	NONE = -1,

	#region 무료
	// 0
	FREE_COINS = (EEnumVal.TYPE * ERewardType.FREE) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion // 무료

	#region 일일
	// 100,000,000
	DAILY_REWARD_SAMPLE = (EEnumVal.TYPE * ERewardType.DAILY) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion // 일일

	#region 이벤트
	// 200,000,000
	EVENT_REWARD_SAMPLE = (EEnumVal.TYPE * ERewardType.EVENT) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion // 이벤트

	#region 클리어
	// 300,000,000
	CLEAR_REWARD_SAMPLE = (EEnumVal.TYPE * ERewardType.CLEAR) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion // 클리어

	#region 미션
	// 400,000,000
	MISSION_REWARD_SAMPLE = (EEnumVal.TYPE * ERewardType.MISSION) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion // 미션

	#region 튜토리얼
	// 50,000,000
	TUTORIAL_REWARD_SAMPLE = (EEnumVal.TYPE * ERewardType.TUTORIAL) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion // 튜토리얼

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
	LEVEL_NORM = (EEnumVal.TYPE * EEpisodeType.LEVEL) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0) + (EEnumVal.SUB_TYPE * 0),

	// 10,000,000
	LEVEL_BOSS = (EEnumVal.TYPE * EEpisodeType.LEVEL) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0) + (EEnumVal.SUB_TYPE * 1),
	#endregion // 레벨

	#region 스테이지
	// 100,000,000
	STAGE_NORM = (EEnumVal.TYPE * EEpisodeType.STAGE) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0) + (EEnumVal.SUB_TYPE * 0),

	// 110,000,000
	STAGE_BOSS = (EEnumVal.TYPE * EEpisodeType.STAGE) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0) + (EEnumVal.SUB_TYPE * 1),
	#endregion // 스테이지

	#region 챕터
	// 200,000,000
	CHAPTER_NORM = (EEnumVal.TYPE * EEpisodeType.CHAPTER) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0) + (EEnumVal.SUB_TYPE * 0),

	// 210,000,000
	CHAPTER_BOSS = (EEnumVal.TYPE * EEpisodeType.CHAPTER) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0) + (EEnumVal.SUB_TYPE * 1),
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
	PLAY_TUTORIAL_SAMPLE = (EEnumVal.TYPE * ETutorialType.PLAY) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion // 플레이

	#region 도움말
	// 100,000,000
	HELP_TUTORIAL_SAMPLE = (EEnumVal.TYPE * ETutorialType.HELP) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
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
	FONT_KOREAN_01 = (EEnumVal.TYPE * EResType.FONT) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	FONT_ENGLISH_01 = (EEnumVal.TYPE * EResType.FONT) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 1),
	#endregion // 폰트

	#region 사운드
	// 100,000,000
	SND_BG_SCENE_TITLE_01 = (EEnumVal.TYPE * EResType.SND) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	SND_BG_SCENE_MAIN_01 = (EEnumVal.TYPE * EResType.SND) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 1),
	SND_BG_SCENE_GAME_01 = (EEnumVal.TYPE * EResType.SND) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 2),

	// 100,100,000
	SND_FX_TOUCH_BEGIN_01 = (EEnumVal.TYPE * EResType.SND) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 0),
	SND_FX_TOUCH_END_01 = (EEnumVal.TYPE * EResType.SND) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 1),

	// 100,200,000
	SND_FX_POPUP_SHOW_01 = (EEnumVal.TYPE * EResType.SND) + (EEnumVal.KINDS_TYPE * 2) + (EEnumVal.SUB_KINDS_TYPE * 0),
	SND_FX_POPUP_CLOSE_01 = (EEnumVal.TYPE * EResType.SND) + (EEnumVal.KINDS_TYPE * 2) + (EEnumVal.SUB_KINDS_TYPE * 1),
	#endregion // 사운드

	#region 이미지
	// 200,000,000
	IMG_WHITE = (EEnumVal.TYPE * EResType.IMG) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	IMG_SPLASH,
	IMG_INDICATOR,
	#endregion // 이미지

	#region 텍스처
	// 300,000,000
	TEX_WHITE = (EEnumVal.TYPE * EResType.TEX) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
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
	[HideInInspector] MAX_VAL
}

/** 아이템 종류 */
public enum EItemKinds {
	NONE = -1,

	#region 재화
	// 0
	GOODS_NORM_COINS = (EEnumVal.TYPE * EItemType.GOODS) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 100,000
	GOODS_COINS_BOX_COINS = (EEnumVal.TYPE * EItemType.GOODS) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion // 재화

	#region 소모
	// 100,000,000
	CONSUMABLE_BOOSTER_SAMPLE = (EEnumVal.TYPE * EItemType.CONSUMABLE) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),

	// 100,100,000
	CONSUMABLE_GAME_ITEM_HINT = (EEnumVal.TYPE * EItemType.CONSUMABLE) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 0),
	CONSUMABLE_GAME_ITEM_CONTINUE = (EEnumVal.TYPE * EItemType.CONSUMABLE) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 1),
	CONSUMABLE_GAME_ITEM_SHUFFLE = (EEnumVal.TYPE * EItemType.CONSUMABLE) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 2),
	#endregion // 소모

	#region 비소모
	// 200,000,000
	NON_CONSUMABLE_REMOVE_ADS = (EEnumVal.TYPE * EItemType.NON_CONSUMABLE) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion // 비소모

	#region 무기
	// 300,000,000
	WEAPON_ITEM_SAMPLE = (EEnumVal.TYPE * EItemType.WEAPON) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion // 무기

	#region 방어구
	// 400,000,000
	ARMOR_ITEM_SAMPLE = (EEnumVal.TYPE * EItemType.ARMOR) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion // 방어구

	#region 악세서리
	// 50,000,000
	ACCESSORY_ITEM_SAMPLE = (EEnumVal.TYPE * EItemType.ACCESSORY) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion // 악세서리

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
	ACTION_PLAYER_ATK_01_01 = (EEnumVal.TYPE * ESkillType.ACTION) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0) + (EEnumVal.SUB_TYPE * 0),

	// 10,000,000
	ACTION_NORM_ENEMY_ATK_01_01 = (EEnumVal.TYPE * ESkillType.ACTION) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0) + (EEnumVal.SUB_TYPE * 1),

	// 20,000,000
	ACTION_BOSS_ENEMY_ATK_01_01 = (EEnumVal.TYPE * ESkillType.ACTION) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0) + (EEnumVal.SUB_TYPE * 2),
	#endregion // 액션

	#region 액티브
	// 100,000,000
	ACTIVE_SKILL_SAMPLE = (EEnumVal.TYPE * ESkillType.ACTIVE) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion // 액티브

	#region 패시브
	// 200,000,000
	PASSIVE_SKILL_SAMPLE = (EEnumVal.TYPE * ESkillType.PASSIVE) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion // 패시브

	[HideInInspector] MAX_VAL
}

/** 객체 타입 */
public enum EObjType {
	NONE = -1,
	BG,
	NORM,
	OVERLAY,
	PLAYABLE,
	NON_PLAYABLE,
	ENEMY,
	[HideInInspector] MAX_VAL
}

/** 객체 종류 */
public enum EObjKinds {
	NONE = -1,

	#region 배경
	// 0
	BG_EMPTY_01 = (EEnumVal.TYPE * EObjType.BG) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion // 배경

	#region 일반
	// 100,000,000
	NORM_OBJ_SAMPLE = (EEnumVal.TYPE * EObjType.NORM) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion // 일반

	#region 중첩
	// 200,000,000
	OVERLAY_OBJ_SAMPLE = (EEnumVal.TYPE * EObjType.OVERLAY) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion // 중첩

	#region 플레이 가능
	// 300,000,000
	PLAYABLE_COMMON_CHARACTER_01 = (EEnumVal.TYPE * EObjType.PLAYABLE) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion // 플레이 가능

	#region 플레이 불가능
	// 400,000,000
	NON_PLAYABLE_OBJ_SAMPLE = (EEnumVal.TYPE * EObjType.NON_PLAYABLE) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion // 플레이 불가능

	#region 적
	// 500,000,000
	ENEMY_NORM_01_01 = (EEnumVal.TYPE * EObjType.ENEMY) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0) + (EEnumVal.SUB_TYPE * 0),

	// 510,000,000
	ENEMY_BOSS_01_01 = (EEnumVal.TYPE * EObjType.ENEMY) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0) + (EEnumVal.SUB_TYPE * 1),
	#endregion // 적

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
	HIT_FX_SAMPLE = (EEnumVal.TYPE * EFXType.HIT) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion // 타격

	#region 버프
	// 100,000,000
	BUFF_FX_SAMPLE = (EEnumVal.TYPE * EFXType.BUFF) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion // 버프

	#region 디버프
	// 200,000,000
	DEBUFF_FX_SAMPLE = (EEnumVal.TYPE * EFXType.DEBUFF) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
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
	STAT_LV = (EEnumVal.TYPE * EAbilityType.STAT) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0) + (EEnumVal.SUB_TYPE * EAbilityValType.NORM),
	STAT_EXP = (EEnumVal.TYPE * EAbilityType.STAT) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 1) + (EEnumVal.SUB_TYPE * EAbilityValType.NORM),
	STAT_NUMS = (EEnumVal.TYPE * EAbilityType.STAT) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 2) + (EEnumVal.SUB_TYPE * EAbilityValType.NORM),
	STAT_ENHANCE = (EEnumVal.TYPE * EAbilityType.STAT) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 3) + (EEnumVal.SUB_TYPE * EAbilityValType.NORM),

	STAT_HP_01 = (EEnumVal.TYPE * EAbilityType.STAT) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 0) + (EEnumVal.SUB_TYPE * EAbilityValType.NORM), STAT_HP_02,
	STAT_MP_01 = (EEnumVal.TYPE * EAbilityType.STAT) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 1) + (EEnumVal.SUB_TYPE * EAbilityValType.NORM), STAT_MP_02,
	STAT_SP_01 = (EEnumVal.TYPE * EAbilityType.STAT) + (EEnumVal.KINDS_TYPE * 1) + (EEnumVal.SUB_KINDS_TYPE * 2) + (EEnumVal.SUB_TYPE * EAbilityValType.NORM), STAT_SP_02,

	STAT_RECOVERY_01 = (EEnumVal.TYPE * EAbilityType.STAT) + (EEnumVal.KINDS_TYPE * 2) + (EEnumVal.SUB_KINDS_TYPE * 0) + (EEnumVal.SUB_TYPE * EAbilityValType.NORM), STAT_RECOVERY_02,
	STAT_HP_RECOVERY_01 = (EEnumVal.TYPE * EAbilityType.STAT) + (EEnumVal.KINDS_TYPE * 2) + (EEnumVal.SUB_KINDS_TYPE * 1) + (EEnumVal.SUB_TYPE * EAbilityValType.NORM), STAT_HP_RECOVERY_02,
	STAT_MP_RECOVERY_01 = (EEnumVal.TYPE * EAbilityType.STAT) + (EEnumVal.KINDS_TYPE * 2) + (EEnumVal.SUB_KINDS_TYPE * 2) + (EEnumVal.SUB_TYPE * EAbilityValType.NORM), STAT_MP_RECOVERY_02,
	STAT_SP_RECOVERY_01 = (EEnumVal.TYPE * EAbilityType.STAT) + (EEnumVal.KINDS_TYPE * 2) + (EEnumVal.SUB_KINDS_TYPE * 3) + (EEnumVal.SUB_TYPE * EAbilityValType.NORM), STAT_SP_RECOVERY_02,

	STAT_ATK_01 = (EEnumVal.TYPE * EAbilityType.STAT) + (EEnumVal.KINDS_TYPE * 3) + (EEnumVal.SUB_KINDS_TYPE * 0) + (EEnumVal.SUB_TYPE * EAbilityValType.NORM), STAT_ATK_02,
	STAT_P_ATK_01 = (EEnumVal.TYPE * EAbilityType.STAT) + (EEnumVal.KINDS_TYPE * 3) + (EEnumVal.SUB_KINDS_TYPE * 1) + (EEnumVal.SUB_TYPE * EAbilityValType.NORM), STAT_P_ATK_02,
	STAT_M_ATK_01 = (EEnumVal.TYPE * EAbilityType.STAT) + (EEnumVal.KINDS_TYPE * 3) + (EEnumVal.SUB_KINDS_TYPE * 2) + (EEnumVal.SUB_TYPE * EAbilityValType.NORM), STAT_M_ATK_02,

	STAT_DEF_01 = (EEnumVal.TYPE * EAbilityType.STAT) + (EEnumVal.KINDS_TYPE * 4) + (EEnumVal.SUB_KINDS_TYPE * 0) + (EEnumVal.SUB_TYPE * EAbilityValType.NORM), STAT_DEF_02,
	STAT_P_DEF_01 = (EEnumVal.TYPE * EAbilityType.STAT) + (EEnumVal.KINDS_TYPE * 4) + (EEnumVal.SUB_KINDS_TYPE * 1) + (EEnumVal.SUB_TYPE * EAbilityValType.NORM), STAT_P_DEF_02,
	STAT_M_DEF_01 = (EEnumVal.TYPE * EAbilityType.STAT) + (EEnumVal.KINDS_TYPE * 4) + (EEnumVal.SUB_KINDS_TYPE * 2) + (EEnumVal.SUB_TYPE * EAbilityValType.NORM), STAT_M_DEF_02,

	STAT_RANGE_01 = (EEnumVal.TYPE * EAbilityType.STAT) + (EEnumVal.KINDS_TYPE * 5) + (EEnumVal.SUB_KINDS_TYPE * 0) + (EEnumVal.SUB_TYPE * EAbilityValType.NORM), STAT_RANGE_02,
	STAT_ATK_RANGE_01 = (EEnumVal.TYPE * EAbilityType.STAT) + (EEnumVal.KINDS_TYPE * 5) + (EEnumVal.SUB_KINDS_TYPE * 1) + (EEnumVal.SUB_TYPE * EAbilityValType.NORM), STAT_ATK_RANGE_02,
	STAT_VIEW_RANGE_01 = (EEnumVal.TYPE * EAbilityType.STAT) + (EEnumVal.KINDS_TYPE * 5) + (EEnumVal.SUB_KINDS_TYPE * 2) + (EEnumVal.SUB_TYPE * EAbilityValType.NORM), STAT_VIEW_RANGE_02,

	STAT_SPEED_01 = (EEnumVal.TYPE * EAbilityType.STAT) + (EEnumVal.KINDS_TYPE * 6) + (EEnumVal.SUB_KINDS_TYPE * 0) + (EEnumVal.SUB_TYPE * EAbilityValType.NORM), STAT_SPEED_02,
	STAT_ATK_SPEED_01 = (EEnumVal.TYPE * EAbilityType.STAT) + (EEnumVal.KINDS_TYPE * 6) + (EEnumVal.SUB_KINDS_TYPE * 1) + (EEnumVal.SUB_TYPE * EAbilityValType.NORM), STAT_ATK_SPEED_02,
	STAT_MOVE_SPEED_01 = (EEnumVal.TYPE * EAbilityType.STAT) + (EEnumVal.KINDS_TYPE * 6) + (EEnumVal.SUB_KINDS_TYPE * 2) + (EEnumVal.SUB_TYPE * EAbilityValType.NORM), STAT_MOVE_SPEED_02,

	STAT_DELAY_01 = (EEnumVal.TYPE * EAbilityType.STAT) + (EEnumVal.KINDS_TYPE * 7) + (EEnumVal.SUB_KINDS_TYPE * 0) + (EEnumVal.SUB_TYPE * EAbilityValType.NORM), STAT_DELAY_02,
	STAT_ATK_DELAY_01 = (EEnumVal.TYPE * EAbilityType.STAT) + (EEnumVal.KINDS_TYPE * 7) + (EEnumVal.SUB_KINDS_TYPE * 1) + (EEnumVal.SUB_TYPE * EAbilityValType.NORM), STAT_ATK_DELAY_02,
	STAT_SKILL_DELAY_01 = (EEnumVal.TYPE * EAbilityType.STAT) + (EEnumVal.KINDS_TYPE * 7) + (EEnumVal.SUB_KINDS_TYPE * 2) + (EEnumVal.SUB_TYPE * EAbilityValType.NORM), STAT_SKILL_DELAY_02,

	STAT_RATE_01 = (EEnumVal.TYPE * EAbilityType.STAT) + (EEnumVal.KINDS_TYPE * 8) + (EEnumVal.SUB_KINDS_TYPE * 0) + (EEnumVal.SUB_TYPE * EAbilityValType.NORM), STAT_RATE_02,
	STAT_HIT_RATE_01 = (EEnumVal.TYPE * EAbilityType.STAT) + (EEnumVal.KINDS_TYPE * 8) + (EEnumVal.SUB_KINDS_TYPE * 1) + (EEnumVal.SUB_TYPE * EAbilityValType.NORM), STAT_HIT_RATE_02,
	STAT_AVOID_RATE_01 = (EEnumVal.TYPE * EAbilityType.STAT) + (EEnumVal.KINDS_TYPE * 8) + (EEnumVal.SUB_KINDS_TYPE * 2) + (EEnumVal.SUB_TYPE * EAbilityValType.NORM), STAT_AVOID_RATE_02,
	STAT_CRITICAL_RATE_01 = (EEnumVal.TYPE * EAbilityType.STAT) + (EEnumVal.KINDS_TYPE * 8) + (EEnumVal.SUB_KINDS_TYPE * 3) + (EEnumVal.SUB_TYPE * EAbilityValType.NORM), STAT_CRITICAL_RATE_02,
	// 0 }

	// 10,000,000 {
	[System.Obsolete] STAT_LV_INCR = EAbilityKinds.STAT_LV + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_EXP_INCR = EAbilityKinds.STAT_EXP + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.INCR - EAbilityValType.NORM)),
	[System.Obsolete] STAT_NUMS_INCR = EAbilityKinds.STAT_NUMS + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.INCR - EAbilityValType.NORM)),
	[System.Obsolete] STAT_ENHANCE_INCR = EAbilityKinds.STAT_ENHANCE + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.INCR - EAbilityValType.NORM)),

	STAT_HP_INCR_01 = EAbilityKinds.STAT_HP_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_MP_INCR_01 = EAbilityKinds.STAT_MP_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_SP_INCR_01 = EAbilityKinds.STAT_SP_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.INCR - EAbilityValType.NORM)),

	STAT_RECOVERY_INCR_01 = EAbilityKinds.STAT_RECOVERY_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_HP_RECOVERY_INCR_01 = EAbilityKinds.STAT_HP_RECOVERY_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_MP_RECOVERY_INCR_01 = EAbilityKinds.STAT_MP_RECOVERY_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_SP_RECOVERY_INCR_01 = EAbilityKinds.STAT_SP_RECOVERY_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.INCR - EAbilityValType.NORM)),

	STAT_ATK_INCR_01 = EAbilityKinds.STAT_ATK_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_P_ATK_INCR_01 = EAbilityKinds.STAT_P_ATK_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_M_ATK_INCR_01 = EAbilityKinds.STAT_M_ATK_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.INCR - EAbilityValType.NORM)),

	STAT_DEF_INCR_01 = EAbilityKinds.STAT_DEF_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_P_DEF_INCR_01 = EAbilityKinds.STAT_P_DEF_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_M_DEF_INCR_01 = EAbilityKinds.STAT_M_DEF_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.INCR - EAbilityValType.NORM)),

	STAT_RANGE_INCR_01 = EAbilityKinds.STAT_RANGE_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_ATK_RANGE_INCR_01 = EAbilityKinds.STAT_ATK_RANGE_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_VIEW_RANGE_INCR_01 = EAbilityKinds.STAT_VIEW_RANGE_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.INCR - EAbilityValType.NORM)),

	STAT_SPEED_INCR_01 = EAbilityKinds.STAT_SPEED_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_ATK_SPEED_INCR_01 = EAbilityKinds.STAT_ATK_SPEED_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_MOVE_SPEED_INCR_01 = EAbilityKinds.STAT_MOVE_SPEED_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.INCR - EAbilityValType.NORM)),

	STAT_DELAY_INCR_01 = EAbilityKinds.STAT_DELAY_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_ATK_DELAY_INCR_01 = EAbilityKinds.STAT_ATK_DELAY_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_SKILL_DELAY_INCR_01 = EAbilityKinds.STAT_SKILL_DELAY_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.INCR - EAbilityValType.NORM)),

	STAT_RATE_INCR_01 = EAbilityKinds.STAT_RATE_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_HIT_RATE_INCR_01 = EAbilityKinds.STAT_HIT_RATE_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_AVOID_RATE_INCR_01 = EAbilityKinds.STAT_AVOID_RATE_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.INCR - EAbilityValType.NORM)),
	STAT_CRITICAL_RATE_INCR_01 = EAbilityKinds.STAT_CRITICAL_RATE_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.INCR - EAbilityValType.NORM)),
	// 10,000,000 }

	// 10,000,000 {
	[System.Obsolete] STAT_LV_DECR = EAbilityKinds.STAT_LV + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_EXP_DECR = EAbilityKinds.STAT_EXP + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.DECR - EAbilityValType.NORM)),
	[System.Obsolete] STAT_NUMS_DECR = EAbilityKinds.STAT_NUMS + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.DECR - EAbilityValType.NORM)),
	[System.Obsolete] STAT_ENHANCE_DECR = EAbilityKinds.STAT_ENHANCE + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.DECR - EAbilityValType.NORM)),

	STAT_HP_DECR_01 = EAbilityKinds.STAT_HP_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_MP_DECR_01 = EAbilityKinds.STAT_MP_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_SP_DECR_01 = EAbilityKinds.STAT_SP_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.DECR - EAbilityValType.NORM)),

	STAT_RECOVERY_DECR_01 = EAbilityKinds.STAT_RECOVERY_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_HP_RECOVERY_DECR_01 = EAbilityKinds.STAT_HP_RECOVERY_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_MP_RECOVERY_DECR_01 = EAbilityKinds.STAT_MP_RECOVERY_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_SP_RECOVERY_DECR_01 = EAbilityKinds.STAT_SP_RECOVERY_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.DECR - EAbilityValType.NORM)),

	STAT_ATK_DECR_01 = EAbilityKinds.STAT_ATK_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_P_ATK_DECR_01 = EAbilityKinds.STAT_P_ATK_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_M_ATK_DECR_01 = EAbilityKinds.STAT_M_ATK_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.DECR - EAbilityValType.NORM)),

	STAT_DEF_DECR_01 = EAbilityKinds.STAT_DEF_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_P_DEF_DECR_01 = EAbilityKinds.STAT_P_DEF_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_M_DEF_DECR_01 = EAbilityKinds.STAT_M_DEF_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.DECR - EAbilityValType.NORM)),

	STAT_RANGE_DECR_01 = EAbilityKinds.STAT_RANGE_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_ATK_RANGE_DECR_01 = EAbilityKinds.STAT_ATK_RANGE_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_VIEW_RANGE_DECR_01 = EAbilityKinds.STAT_VIEW_RANGE_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.DECR - EAbilityValType.NORM)),

	STAT_SPEED_DECR_01 = EAbilityKinds.STAT_SPEED_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_ATK_SPEED_DECR_01 = EAbilityKinds.STAT_ATK_SPEED_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_MOVE_SPEED_DECR_01 = EAbilityKinds.STAT_MOVE_SPEED_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.DECR - EAbilityValType.NORM)),

	STAT_DELAY_DECR_01 = EAbilityKinds.STAT_DELAY_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_ATK_DELAY_DECR_01 = EAbilityKinds.STAT_ATK_DELAY_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_SKILL_DELAY_DECR_01 = EAbilityKinds.STAT_SKILL_DELAY_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.DECR - EAbilityValType.NORM)),

	STAT_RATE_DECR_01 = EAbilityKinds.STAT_RATE_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_HIT_RATE_DECR_01 = EAbilityKinds.STAT_HIT_RATE_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_AVOID_RATE_DECR_01 = EAbilityKinds.STAT_AVOID_RATE_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.DECR - EAbilityValType.NORM)),
	STAT_CRITICAL_RATE_DECR_01 = EAbilityKinds.STAT_CRITICAL_RATE_01 + (int)(EEnumVal.SUB_TYPE * (EAbilityValType.DECR - EAbilityValType.NORM)),
	// 10,000,000 }
	#endregion // 스탯

	#region 버프
	// 100,000,000
	BUFF_ABILITY_SAMPLE = (EEnumVal.TYPE * EAbilityType.BUFF) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion // 버프

	#region 디버프
	// 200,000,000
	DEBUFF_ABILITY_SAMPLE = (EEnumVal.TYPE * EAbilityType.DEBUFF) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion // 디버프

	#region 업그레이드
	// 300,000,000
	UPGRADE_ABILITY_SAMPLE = (EEnumVal.TYPE * EAbilityType.UPGRADE) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
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
	PKGS_SPECIAL_BEGINNER = (EEnumVal.TYPE * EProductType.PKGS) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	PKGS_SPECIAL_EXPERT = (EEnumVal.TYPE * EProductType.PKGS) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 1),
	PKGS_SPECIAL_PRO = (EEnumVal.TYPE * EProductType.PKGS) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 2),
	#endregion // 패키지

	#region 단일
	// 100,000,000
	SINGLE_COINS_BOX = (EEnumVal.TYPE * EProductType.SINGLE) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	SINGLE_REMOVE_ADS = (EEnumVal.TYPE * EProductType.SINGLE) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 1),
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
	[System.Obsolete] ITEM = (EEnumVal.TYPE * ETargetType.ITEM) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	ITEM_LV = (EEnumVal.TYPE * ETargetType.ITEM) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 1),
	ITEM_EXP = (EEnumVal.TYPE * ETargetType.ITEM) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 2),
	ITEM_NUMS = (EEnumVal.TYPE * ETargetType.ITEM) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 3),
	ITEM_ENHANCE = (EEnumVal.TYPE * ETargetType.ITEM) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 4),
	#endregion // 아이템

	#region 스킬
	// 100,000,000
	[System.Obsolete] SKILL = (EEnumVal.TYPE * ETargetType.SKILL) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	SKILL_LV = (EEnumVal.TYPE * ETargetType.SKILL) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 1),
	SKILL_EXP = (EEnumVal.TYPE * ETargetType.SKILL) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 2),
	SKILL_NUMS = (EEnumVal.TYPE * ETargetType.SKILL) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 3),
	SKILL_ENHANCE = (EEnumVal.TYPE * ETargetType.SKILL) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 4),
	#endregion // 스킬

	#region 객체
	// 200,000,000
	[System.Obsolete] OBJ = (EEnumVal.TYPE * ETargetType.OBJ) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	OBJ_LV = (EEnumVal.TYPE * ETargetType.OBJ) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 1),
	OBJ_EXP = (EEnumVal.TYPE * ETargetType.OBJ) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 2),
	OBJ_NUMS = (EEnumVal.TYPE * ETargetType.OBJ) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 3),
	OBJ_ENHANCE = (EEnumVal.TYPE * ETargetType.OBJ) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 4),
	#endregion // 객체

	#region 어빌리티
	// 300,000,000
	ABILITY = (EEnumVal.TYPE * ETargetType.ABILITY) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	ABILITY_LV = (EEnumVal.TYPE * ETargetType.ABILITY) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 1),
	[System.Obsolete] ABILITY_EXP = (EEnumVal.TYPE * ETargetType.ABILITY) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 2),
	[System.Obsolete] ABILITY_NUMS = (EEnumVal.TYPE * ETargetType.ABILITY) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 3),
	[System.Obsolete] ABILITY_ENHANCE = (EEnumVal.TYPE * ETargetType.ABILITY) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 4),
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
	MULTI_SKILL_APPLY_SAMPLE = (EEnumVal.TYPE * ESkillApplyType.MULTI) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion // 다중

	#region 단일
	// 100,000,000
	SINGLE_SKILL_APPLY_SAMPLE = (EEnumVal.TYPE * ESkillApplyType.SINGLE) + (EEnumVal.KINDS_TYPE * 0) + (EEnumVal.SUB_KINDS_TYPE * 0),
	#endregion // 단일

	[HideInInspector] MAX_VAL
}
#endregion // 기본
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE

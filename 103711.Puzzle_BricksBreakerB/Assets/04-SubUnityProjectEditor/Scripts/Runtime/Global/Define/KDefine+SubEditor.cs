using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
/** 서브 에디터 씬 상수 */
public static partial class KDefine {
	#region 기본

	#endregion // 기본
}

/** 서브 레벨 에디터 씬 상수 */
public static partial class KDefine {
	#region 기본
	// 형식
	public const string LES_TEXT_FMT_HP_INFO = "체력 {0}";
    public const string LES_TEXT_FMT_SHIELD_INFO = "실드 {0}";
	public const string LES_TEXT_FMT_ATK_INFO = "공격력 {0}";
    public const string LES_TEXT_FMT_COLOR_INFO = "색상 {0}";
	#endregion // 기본

	#region 런타임 상수
	// 경로
	public static readonly string LES_OBJ_P_TMP_TEXT = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_LEVEL_EDITOR_SCENE}LES_TMPText";

	// 객체 종류
	public static readonly Dictionary<int, List<EObjKinds>> LES_OBJ_KINDS_DICT_CONTAINER = new Dictionary<int, List<EObjKinds>>() {
		[KCDefine.B_VAL_0_INT] = new List<EObjKinds>() {
			EObjKinds.NORM_BRICKS_SQUARE_01,
			EObjKinds.NORM_BRICKS_DIAMOND_01,
			EObjKinds.NONE,
			EObjKinds.NONE,

			EObjKinds.NORM_BRICKS_TRIANGLE_01,
			EObjKinds.NORM_BRICKS_TRIANGLE_02,
			EObjKinds.NORM_BRICKS_TRIANGLE_03,
			EObjKinds.NORM_BRICKS_TRIANGLE_04,

			EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_01,
			EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_02,
			EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_03,
			EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_04,
		},

		[KCDefine.B_VAL_1_INT] = new List<EObjKinds>() {
			EObjKinds.ITEM_BRICKS_BALL_01,
			EObjKinds.ITEM_BRICKS_BALL_02,
			EObjKinds.ITEM_BRICKS_BALL_03,
            EObjKinds.ITEM_BRICKS_BALL_04,

            EObjKinds.ITEM_BRICKS_COINS_01,
			EObjKinds.NONE,
		},

		[KCDefine.B_VAL_2_INT] = new List<EObjKinds>() {
			EObjKinds.SPECIAL_BRICKS_LASER_HORIZONTAL_01,
			EObjKinds.SPECIAL_BRICKS_LASER_VERTICAL_01,
			EObjKinds.SPECIAL_BRICKS_LASER_CROSS_01,
            EObjKinds.NONE,

            EObjKinds.SPECIAL_BRICKS_BALL_DIFFUSION_01,
            EObjKinds.SPECIAL_BRICKS_BALL_AMPLIFICATION_01,
            EObjKinds.SPECIAL_BRICKS_POWERBALL_01,
            EObjKinds.NONE,

            EObjKinds.SPECIAL_BRICKS_ADD_BALL_01,
            EObjKinds.SPECIAL_BRICKS_ADD_BALL_02,
            EObjKinds.SPECIAL_BRICKS_ADD_BALL_03,
            EObjKinds.NONE,     
		},

		[KCDefine.B_VAL_3_INT] = new List<EObjKinds>() {

            EObjKinds.SPECIAL_BRICKS_EXPLOSION_HORIZONTAL_01,
            EObjKinds.SPECIAL_BRICKS_EXPLOSION_VERTICAL_01,
            EObjKinds.SPECIAL_BRICKS_EXPLOSION_CROSS_01,
            EObjKinds.SPECIAL_BRICKS_EXPLOSION_AROUND_01,
			
            EObjKinds.SPECIAL_BRICKS_MISSILE_01,
            EObjKinds.SPECIAL_BRICKS_MISSILE_02,
            EObjKinds.SPECIAL_BRICKS_LIGHTNING_01,
            EObjKinds.SPECIAL_BRICKS_EARTHQUAKE_01,

            EObjKinds.SPECIAL_BRICKS_ARROW_01,
            EObjKinds.SPECIAL_BRICKS_ARROW_02,
            EObjKinds.SPECIAL_BRICKS_ARROW_03,
            EObjKinds.SPECIAL_BRICKS_ARROW_04,

            EObjKinds.SPECIAL_BRICKS_ARROW_05,
            EObjKinds.SPECIAL_BRICKS_ARROW_06,
            EObjKinds.SPECIAL_BRICKS_ARROW_07,
            EObjKinds.SPECIAL_BRICKS_ARROW_08,            

            //EObjKinds.SPECIAL_BRICKS_MORPH_01,
            //EObjKinds.SPECIAL_BRICKS_MORPH_02,
            //EObjKinds.SPECIAL_BRICKS_MORPH_03,
            //EObjKinds.SPECIAL_BRICKS_MORPH_04,
		},

		[KCDefine.B_VAL_4_INT] = new List<EObjKinds>() {

            EObjKinds.OBSTACLE_BRICKS_KEY_01,
			EObjKinds.OBSTACLE_BRICKS_LOCK_01,
			EObjKinds.OBSTACLE_BRICKS_WARP_IN_01,
			EObjKinds.OBSTACLE_BRICKS_WARP_OUT_01,

            EObjKinds.OBSTACLE_BRICKS_FIX_01,
            EObjKinds.OBSTACLE_BRICKS_FIX_02,
            EObjKinds.OBSTACLE_BRICKS_OPEN_01,
            EObjKinds.OBSTACLE_BRICKS_CLOSE_01,

            EObjKinds.OBSTACLE_BRICKS_WOODBOX_01,
            EObjKinds.OBSTACLE_BRICKS_WOODBOX_02,
		}
	};
	#endregion // 런타임 상수
}
#endif // #if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)

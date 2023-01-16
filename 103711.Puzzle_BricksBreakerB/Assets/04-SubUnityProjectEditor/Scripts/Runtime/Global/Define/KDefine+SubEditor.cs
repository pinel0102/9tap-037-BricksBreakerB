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
	public const string LES_TEXT_FMT_HP_INFO = "히트 {0}";
	public const string LES_TEXT_FMT_ATK_INFO = "공격력 {0}";
	#endregion // 기본

	#region 런타임 상수
	// 경로
	public static readonly string LES_OBJ_P_TMP_TEXT = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_LEVEL_EDITOR_SCENE}LES_TMPText";

	// 객체 종류
	public static readonly Dictionary<int, List<EObjKinds>> LES_OBJ_KINDS_DICT_CONTAINER = new Dictionary<int, List<EObjKinds>>() {
		[KCDefine.B_VAL_0_INT] = new List<EObjKinds>() {
			EObjKinds.NORM_BRICKS_SQUARE_01,
			EObjKinds.NONE,
			EObjKinds.NONE,
			EObjKinds.NONE,

			EObjKinds.NORM_BRICKS_TRIANGLE_01,
			EObjKinds.NORM_BRICKS_TRIANGLE_02,
			EObjKinds.NORM_BRICKS_TRIANGLE_03,
			EObjKinds.NORM_BRICKS_TRIANGLE_04,

			EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_01,
			EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_02,
			EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_03,
			EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_04
		},

		[KCDefine.B_VAL_1_INT] = new List<EObjKinds>() {
			// EObjKinds.ITEM_BRICKS_BALL_01,
			// EObjKinds.ITEM_BRICKS_BALL_02,
			// EObjKinds.ITEM_BRICKS_BALL_03,
			// EObjKinds.NONE,

			// EObjKinds.ITEM_BRICKS_BOOSTER_01,
			// EObjKinds.NONE,
			// EObjKinds.NONE,
			// EObjKinds.NONE
		},

		[KCDefine.B_VAL_2_INT] = new List<EObjKinds>() {
			// EObjKinds.SPECIAL_BRICKS_LAZER_HORIZONTAL_01,
			// EObjKinds.SPECIAL_BRICKS_LAZER_VERTICAL_01,
			// EObjKinds.SPECIAL_BRICKS_LAZER_CROSS_01,
			// EObjKinds.NONE
		},

		[KCDefine.B_VAL_3_INT] = new List<EObjKinds>() {
			// EObjKinds.OBSTACLE_BRICKS_KEY_01,
			// EObjKinds.OBSTACLE_BRICKS_LOCK_01,

			// EObjKinds.OBSTACLE_BRICKS_FADE_IN_01,
			// EObjKinds.OBSTACLE_BRICKS_FADE_OUT_01,

			// EObjKinds.OBSTACLE_BRICKS_WARP_IN_01,
			// EObjKinds.OBSTACLE_BRICKS_WARP_IN_02,
			// EObjKinds.OBSTACLE_BRICKS_WARP_IN_03,
			// EObjKinds.OBSTACLE_BRICKS_WARP_IN_04,
			// EObjKinds.OBSTACLE_BRICKS_WARP_IN_05,

			// EObjKinds.OBSTACLE_BRICKS_WARP_OUT_01,
			// EObjKinds.OBSTACLE_BRICKS_WARP_OUT_02,
			// EObjKinds.OBSTACLE_BRICKS_WARP_OUT_03,
			// EObjKinds.OBSTACLE_BRICKS_WARP_OUT_04,
			// EObjKinds.OBSTACLE_BRICKS_WARP_OUT_05
		},

		[KCDefine.B_VAL_4_INT] = new List<EObjKinds>() {
			// Do Something
		}
	};
	#endregion // 런타임 상수
}
#endif // #if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)

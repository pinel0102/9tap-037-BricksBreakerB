using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 서브 엔진 상수 */
	public static partial class KDefine {
		#region 기본
		public const string E_KEY_BALL_OBJ_OBJS_POOL = "BallObjObjsPool";
        public const string E_KEY_AIM_DOT_OBJS_POOL = "AimDotObjsPool";
        public const string E_OBJ_N_BOUNDS = "BOUNDS";
        public const string E_OBJ_N_BALL_OBJ = "BALL_OBJ";
		#endregion // 기본

		#region 런타임 상수
		// 정렬 순서 {
		public static readonly Dictionary<EItemKinds, STSortingOrderInfo> E_SORTING_OI_ITEM_DICT = new Dictionary<EItemKinds, STSortingOrderInfo>() {
			// Do Something
		};

		public static readonly Dictionary<ESkillKinds, STSortingOrderInfo> E_SORTING_OI_SKILL_DICT = new Dictionary<ESkillKinds, STSortingOrderInfo>() {
			// Do Something
		};

		public static readonly Dictionary<EObjKinds, STSortingOrderInfo> E_SORTING_OI_OBJ_DICT = new Dictionary<EObjKinds, STSortingOrderInfo>() {
			[EObjKinds.BG_EMPTY_01] = new STSortingOrderInfo() { m_nOrder = KCDefine.U_SORTING_O_BACKGROUND, m_oLayer = KCDefine.U_SORTING_L_BACKGROUND },

			[EObjKinds.NORM_BRICKS_SQUARE_01] = new STSortingOrderInfo() { m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL },
			[EObjKinds.NORM_BRICKS_TRIANGLE_01] = new STSortingOrderInfo() { m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL },
			[EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_01] = new STSortingOrderInfo() { m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL },
            [EObjKinds.NORM_BRICKS_DIAMOND_01] = new STSortingOrderInfo() { m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL },

            [EObjKinds.ITEM_BRICKS_BALL_01] = new STSortingOrderInfo() { m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL },
            [EObjKinds.ITEM_BRICKS_COINS_01] = new STSortingOrderInfo() { m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL },
            
            [EObjKinds.SPECIAL_BRICKS_LASER_HORIZONTAL_01] = new STSortingOrderInfo() { m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL },
            [EObjKinds.SPECIAL_BRICKS_LASER_VERTICAL_01] = new STSortingOrderInfo() { m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL },
            [EObjKinds.SPECIAL_BRICKS_LASER_CROSS_01] = new STSortingOrderInfo() { m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL },
            [EObjKinds.SPECIAL_BRICKS_MISSILE_01] = new STSortingOrderInfo() { m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL },
            [EObjKinds.SPECIAL_BRICKS_EXPLOSION_HORIZONTAL_01] = new STSortingOrderInfo() { m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL },
            [EObjKinds.SPECIAL_BRICKS_EXPLOSION_VERTICAL_01] = new STSortingOrderInfo() { m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL },
            [EObjKinds.SPECIAL_BRICKS_EXPLOSION_CROSS_01] = new STSortingOrderInfo() { m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL },
            [EObjKinds.SPECIAL_BRICKS_EXPLOSION_AROUND_01] = new STSortingOrderInfo() { m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL },
            [EObjKinds.SPECIAL_BRICKS_EXPLOSION_ALL_01] = new STSortingOrderInfo() { m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL },
            [EObjKinds.SPECIAL_BRICKS_BALL_DIFFUSION_01] = new STSortingOrderInfo() { m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL },
            [EObjKinds.SPECIAL_BRICKS_BALL_AMPLIFICATION_01] = new STSortingOrderInfo() { m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL },
            [EObjKinds.SPECIAL_BRICKS_POWERBALL_01] = new STSortingOrderInfo() { m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL },
            [EObjKinds.SPECIAL_BRICKS_ADD_BALL_01] = new STSortingOrderInfo() { m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL },
            [EObjKinds.SPECIAL_BRICKS_LIGHTNING_01] = new STSortingOrderInfo() { m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL },
            [EObjKinds.SPECIAL_BRICKS_ARROW_01] = new STSortingOrderInfo() { m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL },
            [EObjKinds.SPECIAL_BRICKS_EARTHQUAKE_01] = new STSortingOrderInfo() { m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL },
            [EObjKinds.SPECIAL_BRICKS_MORPH_01] = new STSortingOrderInfo() { m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL },
            
            [EObjKinds.OBSTACLE_BRICKS_KEY_01] = new STSortingOrderInfo() { m_nOrder = 10, m_oLayer = KCDefine.U_SORTING_L_CELL },
            [EObjKinds.OBSTACLE_BRICKS_LOCK_01] = new STSortingOrderInfo() { m_nOrder = 10, m_oLayer = KCDefine.U_SORTING_L_CELL },
            [EObjKinds.OBSTACLE_BRICKS_WARP_IN_01] = new STSortingOrderInfo() { m_nOrder = 10, m_oLayer = KCDefine.U_SORTING_L_CELL },
            [EObjKinds.OBSTACLE_BRICKS_WARP_OUT_01] = new STSortingOrderInfo() { m_nOrder = 10, m_oLayer = KCDefine.U_SORTING_L_CELL },
            [EObjKinds.OBSTACLE_BRICKS_FIX_01] = new STSortingOrderInfo() { m_nOrder = 10, m_oLayer = KCDefine.U_SORTING_L_CELL },
            [EObjKinds.OBSTACLE_BRICKS_OPEN_01] = new STSortingOrderInfo() { m_nOrder = 10, m_oLayer = KCDefine.U_SORTING_L_CELL },
            [EObjKinds.OBSTACLE_BRICKS_CLOSE_01] = new STSortingOrderInfo() { m_nOrder = 10, m_oLayer = KCDefine.U_SORTING_L_CELL },
            [EObjKinds.OBSTACLE_BRICKS_WOODBOX_01] = new STSortingOrderInfo() { m_nOrder = 10, m_oLayer = KCDefine.U_SORTING_L_CELL },

			[EObjKinds.BALL_NORM_01] = new STSortingOrderInfo() { m_nOrder = 10, m_oLayer = KCDefine.U_SORTING_L_BALL },
		};

		public static readonly Dictionary<EFXKinds, STSortingOrderInfo> E_SORTING_OI_FX_DICT = new Dictionary<EFXKinds, STSortingOrderInfo>() {
			// Do Something
		};
		// 정렬 순서 }

		// 경로 {
		public static readonly string E_OBJ_P_BOUNDS = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_ENGINE}Etc/E_Bounds";
		public static readonly string E_OBJ_P_AIMING_DOT = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_ENGINE}Etc/E_AimingDot";
        public static readonly string E_OBJ_P_BALL_OBJ = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_ENGINE}Object/E_BallObj";

		public static readonly Dictionary<EItemKinds, string> E_IMG_P_ITEM_DICT = new Dictionary<EItemKinds, string>() {
			// Do Something
		};

		public static readonly Dictionary<ESkillKinds, string> E_IMG_P_SKILL_DICT = new Dictionary<ESkillKinds, string>() {
			// Do Something
		};

		public static readonly Dictionary<EObjKinds, string> E_IMG_P_OBJ_DICT = new Dictionary<EObjKinds, string>() {
			[EObjKinds.BG_PLACEHOLDER_01] = EObjKinds.BG_PLACEHOLDER_01.ToString(),
			
            [EObjKinds.NORM_BRICKS_SQUARE_01] = EObjKinds.NORM_BRICKS_SQUARE_01.ToString(),
			[EObjKinds.NORM_BRICKS_TRIANGLE_01] = EObjKinds.NORM_BRICKS_TRIANGLE_01.ToString(),
			[EObjKinds.NORM_BRICKS_TRIANGLE_02] = EObjKinds.NORM_BRICKS_TRIANGLE_02.ToString(),
			[EObjKinds.NORM_BRICKS_TRIANGLE_03] = EObjKinds.NORM_BRICKS_TRIANGLE_03.ToString(),
			[EObjKinds.NORM_BRICKS_TRIANGLE_04] = EObjKinds.NORM_BRICKS_TRIANGLE_04.ToString(),
			[EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_01] = EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_01.ToString(),
			[EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_02] = EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_02.ToString(),
			[EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_03] = EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_03.ToString(),
			[EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_04] = EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_04.ToString(),
            [EObjKinds.NORM_BRICKS_DIAMOND_01] = EObjKinds.NORM_BRICKS_DIAMOND_01.ToString(),

            [EObjKinds.ITEM_BRICKS_BALL_01] = EObjKinds.ITEM_BRICKS_BALL_01.ToString(),
            [EObjKinds.ITEM_BRICKS_BALL_02] = EObjKinds.ITEM_BRICKS_BALL_02.ToString(),
            [EObjKinds.ITEM_BRICKS_BALL_03] = EObjKinds.ITEM_BRICKS_BALL_03.ToString(),
            [EObjKinds.ITEM_BRICKS_BALL_04] = EObjKinds.ITEM_BRICKS_BALL_04.ToString(),
            [EObjKinds.ITEM_BRICKS_COINS_01] = EObjKinds.ITEM_BRICKS_COINS_01.ToString(),

            [EObjKinds.SPECIAL_BRICKS_LASER_HORIZONTAL_01] = EObjKinds.SPECIAL_BRICKS_LASER_HORIZONTAL_01.ToString(),
            [EObjKinds.SPECIAL_BRICKS_LASER_HORIZONTAL_02] = EObjKinds.SPECIAL_BRICKS_LASER_HORIZONTAL_01.ToString(),
            [EObjKinds.SPECIAL_BRICKS_LASER_VERTICAL_01] = EObjKinds.SPECIAL_BRICKS_LASER_VERTICAL_01.ToString(),
            [EObjKinds.SPECIAL_BRICKS_LASER_VERTICAL_02] = EObjKinds.SPECIAL_BRICKS_LASER_VERTICAL_01.ToString(),
            [EObjKinds.SPECIAL_BRICKS_LASER_CROSS_01] = EObjKinds.SPECIAL_BRICKS_LASER_CROSS_01.ToString(),
            [EObjKinds.SPECIAL_BRICKS_LASER_CROSS_02] = EObjKinds.SPECIAL_BRICKS_LASER_CROSS_01.ToString(),
            [EObjKinds.SPECIAL_BRICKS_BALL_DIFFUSION_01] = EObjKinds.SPECIAL_BRICKS_BALL_DIFFUSION_01.ToString(),
            [EObjKinds.SPECIAL_BRICKS_BALL_AMPLIFICATION_01] = EObjKinds.SPECIAL_BRICKS_BALL_AMPLIFICATION_01.ToString(),
            [EObjKinds.SPECIAL_BRICKS_POWERBALL_01] = EObjKinds.SPECIAL_BRICKS_POWERBALL_01.ToString(),
            [EObjKinds.SPECIAL_BRICKS_ADD_BALL_01] = EObjKinds.SPECIAL_BRICKS_ADD_BALL_01.ToString(),
            [EObjKinds.SPECIAL_BRICKS_ADD_BALL_02] = EObjKinds.SPECIAL_BRICKS_ADD_BALL_02.ToString(),
            [EObjKinds.SPECIAL_BRICKS_ADD_BALL_03] = EObjKinds.SPECIAL_BRICKS_ADD_BALL_03.ToString(),
            [EObjKinds.SPECIAL_BRICKS_MISSILE_01] = EObjKinds.SPECIAL_BRICKS_MISSILE_01.ToString(),
            [EObjKinds.SPECIAL_BRICKS_MISSILE_02] = EObjKinds.SPECIAL_BRICKS_MISSILE_02.ToString(),
            [EObjKinds.SPECIAL_BRICKS_LIGHTNING_01] = EObjKinds.SPECIAL_BRICKS_LIGHTNING_01.ToString(),
            [EObjKinds.SPECIAL_BRICKS_EXPLOSION_HORIZONTAL_01] = EObjKinds.SPECIAL_BRICKS_EXPLOSION_HORIZONTAL_01.ToString(),
            [EObjKinds.SPECIAL_BRICKS_EXPLOSION_VERTICAL_01] = EObjKinds.SPECIAL_BRICKS_EXPLOSION_VERTICAL_01.ToString(),
            [EObjKinds.SPECIAL_BRICKS_EXPLOSION_CROSS_01] = EObjKinds.SPECIAL_BRICKS_EXPLOSION_CROSS_01.ToString(),
            [EObjKinds.SPECIAL_BRICKS_EXPLOSION_AROUND_01] = EObjKinds.SPECIAL_BRICKS_EXPLOSION_AROUND_01.ToString(),
            [EObjKinds.SPECIAL_BRICKS_EXPLOSION_ALL_01] = EObjKinds.SPECIAL_BRICKS_EXPLOSION_ALL_01.ToString(),
            [EObjKinds.SPECIAL_BRICKS_ARROW_01] = EObjKinds.SPECIAL_BRICKS_ARROW_01.ToString(),
            [EObjKinds.SPECIAL_BRICKS_ARROW_02] = EObjKinds.SPECIAL_BRICKS_ARROW_02.ToString(),
            [EObjKinds.SPECIAL_BRICKS_ARROW_03] = EObjKinds.SPECIAL_BRICKS_ARROW_03.ToString(),
            [EObjKinds.SPECIAL_BRICKS_ARROW_04] = EObjKinds.SPECIAL_BRICKS_ARROW_04.ToString(),
            [EObjKinds.SPECIAL_BRICKS_ARROW_05] = EObjKinds.SPECIAL_BRICKS_ARROW_05.ToString(),
            [EObjKinds.SPECIAL_BRICKS_ARROW_06] = EObjKinds.SPECIAL_BRICKS_ARROW_06.ToString(),
            [EObjKinds.SPECIAL_BRICKS_ARROW_07] = EObjKinds.SPECIAL_BRICKS_ARROW_07.ToString(),
            [EObjKinds.SPECIAL_BRICKS_ARROW_08] = EObjKinds.SPECIAL_BRICKS_ARROW_08.ToString(),
            [EObjKinds.SPECIAL_BRICKS_EARTHQUAKE_01] = EObjKinds.SPECIAL_BRICKS_EARTHQUAKE_01.ToString(),
            [EObjKinds.SPECIAL_BRICKS_MORPH_01] = EObjKinds.SPECIAL_BRICKS_MORPH_01.ToString(),
            [EObjKinds.SPECIAL_BRICKS_MORPH_02] = EObjKinds.SPECIAL_BRICKS_MORPH_02.ToString(),
            [EObjKinds.SPECIAL_BRICKS_MORPH_03] = EObjKinds.SPECIAL_BRICKS_MORPH_03.ToString(),
            [EObjKinds.SPECIAL_BRICKS_MORPH_04] = EObjKinds.SPECIAL_BRICKS_MORPH_04.ToString(),

            [EObjKinds.OBSTACLE_BRICKS_KEY_01] = EObjKinds.OBSTACLE_BRICKS_KEY_01.ToString(),
            [EObjKinds.OBSTACLE_BRICKS_LOCK_01] = EObjKinds.OBSTACLE_BRICKS_LOCK_01.ToString(),
            [EObjKinds.OBSTACLE_BRICKS_FIX_01] = EObjKinds.OBSTACLE_BRICKS_FIX_01.ToString(),
            [EObjKinds.OBSTACLE_BRICKS_FIX_02] = EObjKinds.OBSTACLE_BRICKS_FIX_02.ToString(),
            [EObjKinds.OBSTACLE_BRICKS_FIX_03] = EObjKinds.OBSTACLE_BRICKS_FIX_02.ToString(),
            [EObjKinds.OBSTACLE_BRICKS_OPEN_01] = EObjKinds.OBSTACLE_BRICKS_OPEN_01.ToString(),
            [EObjKinds.OBSTACLE_BRICKS_CLOSE_01] = EObjKinds.OBSTACLE_BRICKS_CLOSE_01.ToString(),
            [EObjKinds.OBSTACLE_BRICKS_WOODBOX_01] = EObjKinds.OBSTACLE_BRICKS_WOODBOX_01.ToString(),
            [EObjKinds.OBSTACLE_BRICKS_WOODBOX_02] = EObjKinds.OBSTACLE_BRICKS_WOODBOX_02.ToString(),
            [EObjKinds.OBSTACLE_BRICKS_WARP_IN_01] = EObjKinds.OBSTACLE_BRICKS_WARP_IN_01.ToString(),
            [EObjKinds.OBSTACLE_BRICKS_WARP_IN_02] = EObjKinds.OBSTACLE_BRICKS_WARP_IN_01.ToString(),
            [EObjKinds.OBSTACLE_BRICKS_WARP_IN_03] = EObjKinds.OBSTACLE_BRICKS_WARP_IN_01.ToString(),
            [EObjKinds.OBSTACLE_BRICKS_WARP_IN_04] = EObjKinds.OBSTACLE_BRICKS_WARP_IN_01.ToString(),
            [EObjKinds.OBSTACLE_BRICKS_WARP_OUT_01] = EObjKinds.OBSTACLE_BRICKS_WARP_OUT_01.ToString(),
            [EObjKinds.OBSTACLE_BRICKS_WARP_OUT_02] = EObjKinds.OBSTACLE_BRICKS_WARP_OUT_01.ToString(),
            [EObjKinds.OBSTACLE_BRICKS_WARP_OUT_03] = EObjKinds.OBSTACLE_BRICKS_WARP_OUT_01.ToString(),
            [EObjKinds.OBSTACLE_BRICKS_WARP_OUT_04] = EObjKinds.OBSTACLE_BRICKS_WARP_OUT_01.ToString(),

			[EObjKinds.BALL_NORM_01] = EObjKinds.BALL_NORM_01.ToString(),
            [EObjKinds.BALL_NORM_02] = EObjKinds.BALL_NORM_01.ToString(),
            [EObjKinds.BALL_NORM_03] = EObjKinds.BALL_NORM_01.ToString(),
		};
		// 경로 }
		#endregion // 런타임 상수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

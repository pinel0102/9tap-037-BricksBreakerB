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
		// 속도
		public const float E_SPEED_SHOOT = 1250.0f;

		// 길이
		public const float E_LENGTH_LINE = 100.0f;
		public const float E_MAX_LENGTH_LINE = 450.0f;

		// 각도
		public const float E_MIN_ANGLE_AIMING = 5.0f;

		// 식별자
		public const string E_KEY_BALL_OBJ_OBJS_POOL = "BallObjObjsPool";

		// 이름
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
			[EObjKinds.BG_EMPTY_01] = new STSortingOrderInfo() {
				m_nOrder = KCDefine.U_SORTING_O_BACKGROUND, m_oLayer = KCDefine.U_SORTING_L_BACKGROUND
			},

			[EObjKinds.NORM_BRICKS_SQUARE_01] = new STSortingOrderInfo() {
				m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL
			},

			[EObjKinds.NORM_BRICKS_TRIANGLE_01] = new STSortingOrderInfo() {
				m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL
			},

			[EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_01] = new STSortingOrderInfo() {
				m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL
			},

            [EObjKinds.NORM_BRICKS_DIAMOND_01] = new STSortingOrderInfo() {
				m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL
			},

            [EObjKinds.ITEM_BRICKS_BALL_01] = new STSortingOrderInfo() {
				m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL
			},

            [EObjKinds.SPECIAL_BRICKS_LASER_HORIZONTAL_01] = new STSortingOrderInfo() {
				m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL
			},

            [EObjKinds.SPECIAL_BRICKS_LASER_VERTICAL_01] = new STSortingOrderInfo() {
				m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL
			},

            [EObjKinds.SPECIAL_BRICKS_LASER_CROSS_01] = new STSortingOrderInfo() {
				m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_CELL
			},

			[EObjKinds.BALL_NORM_01] = new STSortingOrderInfo() {
				m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_BALL
			}
		};

		public static readonly Dictionary<EFXKinds, STSortingOrderInfo> E_SORTING_OI_FX_DICT = new Dictionary<EFXKinds, STSortingOrderInfo>() {
			// Do Something
		};
		// 정렬 순서 }

		// 경로 {
		public static readonly string E_OBJ_P_BOUNDS = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_ENGINE}Etc/E_Bounds";
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

            [EObjKinds.SPECIAL_BRICKS_LASER_HORIZONTAL_01] = EObjKinds.SPECIAL_BRICKS_LASER_HORIZONTAL_01.ToString(),
            [EObjKinds.SPECIAL_BRICKS_LASER_VERTICAL_01] = EObjKinds.SPECIAL_BRICKS_LASER_VERTICAL_01.ToString(),
            [EObjKinds.SPECIAL_BRICKS_LASER_CROSS_01] = EObjKinds.SPECIAL_BRICKS_LASER_CROSS_01.ToString(),

			[EObjKinds.BALL_NORM_01] = EObjKinds.BALL_NORM_01.ToString(),
		};
		// 경로 }
		#endregion // 런타임 상수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

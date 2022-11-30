#if SCRIPT_TEMPLATE_ONLY
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
		// 간격
		public const float E_OFFSET_BOTTOM = 150.0f;
		public const float E_OFFSET_MAIN_CAMERA = -50.0f;

		// 식별자
		public const string E_KEY_CELL_OBJ_OBJS_POOL = "CellObjObjsPool";
		public const string E_KEY_PLAYER_OBJ_OBJS_POOL = "PlayerObjObjsPool";
		public const string E_KEY_ENEMY_OBJ_OBJS_POOL = "EnemyObjObjsPool";

		// 이름
		public const string E_OBJ_N_CELL_OBJ = "CELL_OBJ";
		public const string E_OBJ_N_PLAYER_OBJ = "PLAYER_OBJ";
		public const string E_OBJ_N_ENEMY_OBJ = "ENEMY_OBJ";
#endregion // 기본

#region 런타임 상수
		// 크기
		public static readonly Vector3 E_SIZE_CELL = new Vector3(0.0f, 0.0f, 0.0f);
		public static readonly Vector3 E_MAX_SIZE_GRID = new Vector3(KCDefine.B_SCREEN_WIDTH - 20.0f, KCDefine.B_SCREEN_WIDTH - 20.0f, 0.0f);

		// 간격
		public static readonly Vector3 E_OFFSET_CELL = new Vector3(KDefine.E_SIZE_CELL.x / 2.0f, KDefine.E_SIZE_CELL.y / -2.0f, 0.0f);

		// 개수
		public static readonly Vector3Int E_MIN_NUM_CELLS = new Vector3Int(1, 1, 1);
		public static readonly Vector3Int E_MAX_NUM_CELLS = new Vector3Int(15, 15, 15);

		// 정렬 순서 {
		public static readonly STSortingOrderInfo E_SORTING_OI_DEF = new STSortingOrderInfo() {
			m_nOrder = sbyte.MaxValue * 0, m_oLayer = KCDefine.U_SORTING_L_DEF
		};

		public static readonly Dictionary<EObjKinds, STSortingOrderInfo> E_SORTING_OI_OBJ_DICT = new Dictionary<EObjKinds, STSortingOrderInfo>() {
			[EObjKinds.BG_EMPTY_01] = new STSortingOrderInfo() { m_nOrder = sbyte.MaxValue * -1, m_oLayer = KCDefine.U_SORTING_L_DEF }
		};
		// 정렬 순서 }

		// 경로 {
		public static readonly string E_OBJ_P_CELL_OBJ = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_ENGINE}Object/E_CellObj";
		public static readonly string E_OBJ_P_PLAYER_OBJ = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_ENGINE}Object/E_PlayerObj";
		public static readonly string E_OBJ_P_ENEMY_OBJ = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_ENGINE}Object/E_EnemyObj";

		public static readonly Dictionary<EObjKinds, string> E_IMG_P_OBJ_DICT = new Dictionary<EObjKinds, string>() {
			[EObjKinds.BG_EMPTY_01] = EObjKinds.BG_EMPTY_01.ToString()
		};
		// 경로 }
#endregion // 런타임 상수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

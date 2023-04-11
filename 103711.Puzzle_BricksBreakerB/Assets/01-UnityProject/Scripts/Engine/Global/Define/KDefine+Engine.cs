using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 엔진 상수 */
	public static partial class KDefine {
		#region 기본
		// 식별자 {
		public const string E_KEY_ITEM_OBJS_POOL = "ItemObjsPool";
		public const string E_KEY_SKILL_OBJS_POOL = "SkillObjsPool";
		public const string E_KEY_OBJ_OBJS_POOL = "ObjObjsPool";
		public const string E_KEY_FX_OBJS_POOL = "FXObjsPool";

		public const string E_KEY_CELL_OBJ_OBJS_POOL = "CellObjObjsPool";
		public const string E_KEY_PLAYER_OBJ_OBJS_POOL = "PlayerObjObjsPool";
		public const string E_KEY_ENEMY_OBJ_OBJS_POOL = "EnemyObjObjsPool";
		// 식별자 }

		// 이름 {
		public const string E_OBJ_N_ITEM = "ITEM";
		public const string E_OBJ_N_SKILL = "SKILL";
		public const string E_OBJ_N_OBJ = "OBJ";
		public const string E_OBJ_N_FX = "FX";

		public const string E_OBJ_N_CELL_OBJ = "CELL_OBJ";
		public const string E_OBJ_N_PLAYER_OBJ = "PLAYER_OBJ";
		public const string E_OBJ_N_ENEMY_OBJ = "ENEMY_OBJ";

        // 이름 }
		#endregion // 기본

		#region 런타임 상수
		// 개수
		public static readonly Vector3Int E_MIN_NUM_CELLS = new Vector3Int(5, 6, 1);
		public static readonly Vector3Int E_MAX_NUM_CELLS = new Vector3Int(20, 40, 1);
		public static readonly Vector3Int E_DEF_NUM_CELLS = new Vector3Int(11, 11, 1);

		// 간격 {
		public static readonly Vector3 E_OFFSET_BOTTOM = new Vector3(0.0f, 150.0f, 0.0f);
		public static readonly Vector3 E_OFFSET_MAIN_CAMERA = new Vector3(0.0f, -50.0f, 0.0f);

		public static readonly Vector3 E_PADDING_GRID = new Vector3(0.0f, 0.0f, 0.0f);
		// 간격 }

		// 크기
		public static readonly Vector3 E_MAX_GRID_SIZE_PORTRAIT = new Vector3(KCDefine.B_PORTRAIT_SCREEN_WIDTH - KDefine.E_PADDING_GRID.x, KCDefine.B_PORTRAIT_SCREEN_WIDTH - KDefine.E_PADDING_GRID.y, KCDefine.B_VAL_0_REAL);
		public static readonly Vector3 E_MAX_GRID_SIZE_LANDSCAPE = new Vector3(KCDefine.B_LANDSCAPE_SCREEN_HEIGHT - KDefine.E_PADDING_GRID.x, KCDefine.B_LANDSCAPE_SCREEN_HEIGHT - KDefine.E_PADDING_GRID.y, KCDefine.B_VAL_0_REAL);

		// 경로 {
		public static readonly string E_OBJ_P_ITEM = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_ENGINE}Object/E_Item";
		public static readonly string E_OBJ_P_SKILL = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_ENGINE}Object/E_Skill";
		public static readonly string E_OBJ_P_OBJ = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_ENGINE}Object/E_Obj";
		public static readonly string E_OBJ_P_FX = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_ENGINE}Object/E_FX";

		public static readonly string E_OBJ_P_CELL_OBJ = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_ENGINE}Object/E_CellObj";
		public static readonly string E_OBJ_P_PLAYER_OBJ = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_ENGINE}Object/E_PlayerObj";
		public static readonly string E_OBJ_P_ENEMY_OBJ = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_ENGINE}Object/E_EnemyObj";

        // 경로 }
		#endregion // 런타임 상수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

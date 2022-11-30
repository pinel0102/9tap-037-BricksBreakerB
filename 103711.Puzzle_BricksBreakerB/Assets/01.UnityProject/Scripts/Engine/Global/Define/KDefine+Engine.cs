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
		// 식별자
		public const string E_KEY_ITEM_OBJS_POOL = "ItemObjsPool";
		public const string E_KEY_SKILL_OBJS_POOL = "SkillObjsPool";
		public const string E_KEY_OBJ_OBJS_POOL = "ObjObjsPool";
		public const string E_KEY_FX_OBJS_POOL = "FXObjsPool";

		// 이름
		public const string E_OBJ_N_ITEM = "ITEM";
		public const string E_OBJ_N_SKILL = "SKILL";
		public const string E_OBJ_N_OBJ = "OBJ";
		public const string E_OBJ_N_FX = "FX";
#endregion // 기본

#region 런타임 상수
		// 경로
		public static readonly string E_OBJ_P_ITEM = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_ENGINE}Object/E_Item";
		public static readonly string E_OBJ_P_SKILL = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_ENGINE}Object/E_Skill";
		public static readonly string E_OBJ_P_OBJ = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_ENGINE}Object/E_Obj";
		public static readonly string E_OBJ_P_FX = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_ENGINE}Object/E_FX";
#endregion // 런타임 상수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

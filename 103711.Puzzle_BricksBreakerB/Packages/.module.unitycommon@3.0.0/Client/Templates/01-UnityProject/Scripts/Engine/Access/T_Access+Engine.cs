#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 엔진 접근자 */
	public static partial class Access {
#region 프로퍼티
		public static Vector3 MaxGridSize => CSceneManager.ActiveSceneManager.IsPortrait ? new Vector3(CSceneManager.ActiveSceneManager.ScreenWidth - KDefine.E_PADDING_GRID.x, CSceneManager.ActiveSceneManager.ScreenWidth - KDefine.E_PADDING_GRID.y, KCDefine.B_VAL_0_REAL) : new Vector3(CSceneManager.ActiveSceneManager.ScreenHeight - KDefine.E_PADDING_GRID.x, CSceneManager.ActiveSceneManager.ScreenHeight - KDefine.E_PADDING_GRID.y, KCDefine.B_VAL_0_REAL);
		public static Vector3 CellSize => CSceneManager.ActiveSceneManager.IsPortrait ? new Vector3(Access.MaxGridSize.x / (float)KDefine.E_DEF_NUM_CELLS.x, Access.MaxGridSize.x / (float)KDefine.E_DEF_NUM_CELLS.y, KCDefine.B_VAL_0_REAL) : new Vector3(Access.MaxGridSize.y / (float)KDefine.E_DEF_NUM_CELLS.x, Access.MaxGridSize.y / (float)KDefine.E_DEF_NUM_CELLS.y, KCDefine.B_VAL_0_REAL);
		public static Vector3 CellCenterOffset => new Vector3(Access.CellSize.x / KCDefine.B_VAL_2_REAL, Access.CellSize.y / -KCDefine.B_VAL_2_REAL, KCDefine.B_VAL_0_REAL);
#endregion // 프로퍼티

#region 클래스 함수
		/** 객체 스프라이트를 반환한다 */
		public static Sprite GetObjSprite(EObjKinds a_eObjKinds) {
			string oImgPath = KDefine.E_IMG_P_OBJ_DICT.GetValueOrDefault((EObjKinds)((int)a_eObjKinds).ExKindsToSubKindsType(), string.Empty);
			return oImgPath.ExIsValid() ? CResManager.Inst.GetRes<Sprite>(oImgPath) : null;
		}

		/** 정렬 순서 정보를 반환한다 */
		public static STSortingOrderInfo GetSortingOrderInfo(EObjKinds a_eObjKinds, int a_nExtraOrder = KCDefine.B_VAL_0_INT) {
			var stSortingOrderInfo = KDefine.E_SORTING_OI_OBJ_DICT.GetValueOrDefault((EObjKinds)((int)a_eObjKinds).ExKindsToSubKindsType(), STSortingOrderInfo.INVALID);
			stSortingOrderInfo.m_nOrder += a_nExtraOrder;

			return stSortingOrderInfo;
		}
#endregion // 클래스 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

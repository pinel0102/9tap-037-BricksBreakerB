using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 엔진 접근자 */
	public static partial class Access {
#region 클래스 함수
		/** 객체 스프라이트를 반환한다 */
		public static Sprite GetObjSprite(EObjKinds a_eObjKinds) {
			string oImgPath = KDefine.E_IMG_P_OBJ_DICT.GetValueOrDefault((EObjKinds)((int)a_eObjKinds).ExKindsToSubKindsType(), KCDefine.U_IMG_P_WHITE);
			return oImgPath.ExIsValid() ? CResManager.Inst.GetRes<Sprite>(oImgPath) : null;
		}
		
		/** 정렬 순서 정보를 반환한다 */
		public static STSortingOrderInfo GetSortingOrderInfo(EObjKinds a_eObjKinds) {
			return KDefine.E_SORTING_OI_OBJ_DICT.GetValueOrDefault((EObjKinds)((int)a_eObjKinds).ExKindsToSubKindsType(), KDefine.E_SORTING_OI_DEF);
		}
#endregion // 클래스 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

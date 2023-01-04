#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.Linq;

namespace NSEngine {
	/** 엔진 - 접근 */
	public partial class CEngine : CComponent {
#region 함수
		/** 셀 객체를 탐색한다 */
		public CEObj FindCellObj(EObjKinds a_eObjKinds, Vector3Int a_stIdx) {
			return m_oCellObjLists.ExGetVal(a_stIdx, null)?.ExGetVal((a_oCellObj) => a_oCellObj.Params.m_stObjInfo.m_eObjKinds == a_eObjKinds, null);
		}

		/** 셀 객체를 탐색한다 */
		public List<CEObj> FindCellObjs(EObjKinds a_eObjKinds, Vector3Int a_stIdx) {
			return m_oCellObjLists.ExGetVal(a_stIdx, null)?.ExGetVals((a_oCellObj) => a_oCellObj.Params.m_stObjInfo.m_eObjKinds == a_eObjKinds);
		}

		/** 최상단 셀 객체를 탐색한다 */
		public CEObj FindTopCellObj(EObjKinds a_eObjKinds, Vector3Int a_stIdx) {
			var oCellObjList = this.FindCellObjs(a_eObjKinds, a_stIdx);
			return oCellObjList.ExIsValid() ? oCellObjList.Last() : null;
		}
#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

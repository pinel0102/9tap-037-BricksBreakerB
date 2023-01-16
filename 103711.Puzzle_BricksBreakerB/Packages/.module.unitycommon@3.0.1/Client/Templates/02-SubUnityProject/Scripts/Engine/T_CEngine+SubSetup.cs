#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 서브 엔진 - 설정 */
	public partial class CEngine : CComponent {
		#region 함수
		/** 셀을 설정한다 */
		private void SubSetupCell(STCellInfo a_stCellInfo, STGridInfo a_stGridInfo) {
			for(int i = 0; i < a_stCellInfo.m_oCellObjInfoList.Count; ++i) {
				// 객체 종류가 유효 할 경우
				if(a_stCellInfo.m_oCellObjInfoList[i].ObjKinds.ExIsValid() && a_stCellInfo.m_oCellObjInfoList[i].ObjKinds != EObjKinds.BG_PLACEHOLDER_01) {
					var stPos = a_stGridInfo.m_stPivotPos + a_stCellInfo.m_stIdx.ExToPos(Access.CellCenterOffset, Access.CellSize);
				}
			}
		}

		/** 그리드 라인을 설정한다 */
		private void SubSetupGridLine() {
			// Do Something
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

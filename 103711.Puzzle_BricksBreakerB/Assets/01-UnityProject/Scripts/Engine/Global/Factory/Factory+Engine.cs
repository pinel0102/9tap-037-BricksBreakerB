using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 엔진 팩토리 */
	public static partial class Factory {
		#region 클래스 함수
		/** 그리드 정보를 생성한다 */
		public static STGridInfo MakeGridInfo(Vector3 a_stPivot, Vector3 a_stPos, Vector3 a_stOffset, Vector3Int a_stNumCells, bool a_bIsEnableOverflow = false) {
			var stGridInfo = new STGridInfo() {
				m_stBounds = new Bounds(Vector3.zero, new Vector3(a_stNumCells.x * Access.CellSize.x, a_stNumCells.y * Access.CellSize.y, KCDefine.B_VAL_0_REAL)),
                aimAdjustHeight = Access.CellSize.y * GlobalDefine.GRID_BLANK_CELL
			};

			var stPos = a_stPos + a_stOffset;
			var stBoundsPos = stPos.ExGetPivotPos(KCDefine.B_ANCHOR_MID_CENTER, a_stPivot, stGridInfo.m_stBounds.size);
			var stViewBoundsPos = a_stPos.ExGetPivotPos(KCDefine.B_ANCHOR_MID_CENTER, a_stPivot, a_bIsEnableOverflow ? new Vector3(stGridInfo.m_stBounds.size.x, stGridInfo.m_stBounds.size.x * (Access.MaxGridSize.y / Access.MaxGridSize.x), KCDefine.B_VAL_0_REAL) : new Vector3(Mathf.Max(stGridInfo.m_stBounds.size.x, stGridInfo.m_stBounds.size.y), Mathf.Max(stGridInfo.m_stBounds.size.x, stGridInfo.m_stBounds.size.y), KCDefine.B_VAL_0_REAL));

			try {
				stGridInfo.m_stBounds = new Bounds(stBoundsPos, stGridInfo.m_stBounds.size);
				stGridInfo.m_stViewBounds = new Bounds(stViewBoundsPos, a_bIsEnableOverflow ? new Vector3(stGridInfo.m_stBounds.size.x, stGridInfo.m_stBounds.size.x * (Access.MaxGridSize.y / Access.MaxGridSize.x), KCDefine.B_VAL_0_REAL) : new Vector3(Mathf.Max(stGridInfo.m_stBounds.size.x, stGridInfo.m_stBounds.size.y), Mathf.Max(stGridInfo.m_stBounds.size.x, stGridInfo.m_stBounds.size.y), KCDefine.B_VAL_0_REAL));
                stGridInfo.m_stAimBounds = new Bounds(stViewBoundsPos, a_bIsEnableOverflow ? new Vector3(stGridInfo.m_stBounds.size.x, stGridInfo.m_stBounds.size.x * (Access.MaxGridSize.y / Access.MaxGridSize.x) + stGridInfo.aimAdjustHeight, KCDefine.B_VAL_0_REAL) : new Vector3(Mathf.Max(stGridInfo.m_stBounds.size.x, stGridInfo.m_stBounds.size.y), Mathf.Max(stGridInfo.m_stBounds.size.x, stGridInfo.m_stBounds.size.y + stGridInfo.aimAdjustHeight), KCDefine.B_VAL_0_REAL));
                
				stGridInfo.m_stPivotPos = new Vector3(stGridInfo.m_stBounds.min.x, stGridInfo.m_stBounds.max.y, KCDefine.B_VAL_0_REAL);
				stGridInfo.m_stViewPivotPos = new Vector3(stGridInfo.m_stViewBounds.min.x, stGridInfo.m_stViewBounds.max.y, KCDefine.B_VAL_0_REAL);

				// 오버 플로우 모드 일 경우
				if(a_bIsEnableOverflow) {
					stGridInfo.m_stScale = Vector3.one * (Access.MaxGridSize.x / stGridInfo.m_stBounds.size.x);
				} else {
					stGridInfo.m_stScale = Vector3.one * Mathf.Min(Access.MaxGridSize.x / stGridInfo.m_stBounds.size.x, Access.MaxGridSize.y / stGridInfo.m_stBounds.size.y);
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"Factory.MakeGridInfo Exception: {oException.Message}");
				stGridInfo.m_stScale = Vector3.one;
			}

			return stGridInfo;
		}
		#endregion // 클래스 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

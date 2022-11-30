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
		public static STGridInfo MakeGridInfo(CLevelInfo a_oLevelInfo, Vector3 a_stPos) {
			var stGridInfo = new STGridInfo() {
				m_stSize = new Vector3(a_oLevelInfo.NumCells.x * KDefine.E_SIZE_CELL.x, a_oLevelInfo.NumCells.y * KDefine.E_SIZE_CELL.y, KCDefine.B_VAL_0_REAL)
			};

			stGridInfo.m_stBounds = new Bounds(a_stPos, stGridInfo.m_stSize);
			stGridInfo.m_stPivotPos = new Vector3(stGridInfo.m_stBounds.min.x, stGridInfo.m_stBounds.max.y, KCDefine.B_VAL_0_REAL);

			try {
				stGridInfo.m_stScale = Vector3.one * Mathf.Min(KDefine.E_MAX_SIZE_GRID.x / stGridInfo.m_stBounds.size.x, KDefine.E_MAX_SIZE_GRID.y / stGridInfo.m_stBounds.size.y);
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

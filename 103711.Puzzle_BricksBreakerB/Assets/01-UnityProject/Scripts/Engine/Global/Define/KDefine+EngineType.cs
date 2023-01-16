using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using MessagePack;

namespace NSEngine {
	#region 기본
	/** 그리드 정보 */
	public struct STGridInfo {
		public Bounds m_stBounds;
		public Bounds m_stViewBounds;

		public Vector3 m_stScale;
		public Vector3 m_stPivotPos;
		public Vector3 m_stViewPivotPos;

		#region 상수
		public static STGridInfo INVALID = new STGridInfo() {
			m_stScale = Vector3.one
		};
		#endregion // 상수
	}

	/** 엔진 타입 랩퍼 */
	[MessagePackObject]
	public struct STEngineTypeWrapper {
		// Do Something
	}
	#endregion // 기본
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 엔진 접근자 확장 클래스 */
	public static partial class AccessExtension {
#region 클래스 함수
		/** 인덱스 유효 여부를 검사한다 */
		public static bool ExIsValidIdx(this Dictionary<EObjType, List<CEObj>>[,] a_oSender, Vector3Int a_stIdx) {
			return a_oSender.ExIsValidIdx<Dictionary<EObjType, List<CEObj>>>(a_stIdx) && a_oSender[a_stIdx.y, a_stIdx.x] != null;
		}
#endregion // 클래스 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

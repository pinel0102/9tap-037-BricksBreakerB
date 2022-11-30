#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 셀 객체 제어자 */
	public partial class CECellObjController : CEObjController {
		/** 매개 변수 */
		public new struct STParams {
			public CEObjController.STParams m_stBaseParams;
		}

#region 변수

#endregion // 변수

#region 프로퍼티
		public new STParams Params { get; private set; }
#endregion // 프로퍼티

#region 함수
		
#endregion // 함수

#region 클래스 함수
		/** 매개 변수를 생성한다 */
		public new static STParams MakeParams(CEngine a_oEngine) {
			return new STParams() {
				m_stBaseParams = CEObjController.MakeParams(a_oEngine)
			};
		}
#endregion // 클래스 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

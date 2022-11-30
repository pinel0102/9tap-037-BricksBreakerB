using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace GameScene {
	/** 게임 씬 컴포넌트 */
	public partial class CGSComponent : CComponent {
		/** 매개 변수 */
		public struct STParams {
			public NSEngine.CEngine m_oEngine;
		}

#region 변수
		
#endregion // 변수

#region 프로퍼티
		public STParams Params { get; private set; }
#endregion // 프로퍼티

#region 함수

#endregion // 함수

#region 클래스 함수
		/** 매개 변수를 생성한다 */
		public static STParams MakeParams(NSEngine.CEngine a_oEngine) {
			return new STParams() {
				m_oEngine = a_oEngine
			};
		}
#endregion // 클래스 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

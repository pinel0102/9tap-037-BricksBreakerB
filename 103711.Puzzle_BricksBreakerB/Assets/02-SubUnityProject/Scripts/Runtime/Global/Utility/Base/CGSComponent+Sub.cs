using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace GameScene {
	/** 게임 씬 컴포넌트 */
	public abstract partial class CGSComponent : CComponent {
		#region 함수

		#endregion // 함수
	}

	/** 서브 게임 씬 컴포넌트 */
	public abstract partial class CGSComponent : CComponent {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

		#region 변수

		#endregion // 변수

		#region 프로퍼티

		#endregion // 프로퍼티

		#region 함수
		/** 컴포넌트를 설정한다 */
		private void SubAwake() {
			// Do Something
		}

		/** 초기화한다 */
		private void SubInit() {
			// Do Something
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

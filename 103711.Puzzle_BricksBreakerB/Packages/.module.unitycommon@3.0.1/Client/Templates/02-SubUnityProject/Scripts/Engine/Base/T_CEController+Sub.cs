#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 서브 제어자 */
	public abstract partial class CEController : CEComponent {
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
		/** 대기 상태를 처리한다 */
		protected virtual void HandleIdleState(float a_fDeltaTime) {
			// Do Something
		}

		/** 이동 상태를 처리한다 */
		protected virtual void HandleMoveState(float a_fDeltaTime) {
			// Do Something
		}

		/** 스킬 상태를 처리한다 */
		protected virtual void HandleSkillState(float a_fDeltaTime) {
			// Do Something
		}

		/** 등장 상태를 처리한다 */
		protected virtual void HandleAppearState(float a_fDeltaTime) {
			// Do Something
		}

		/** 사라짐 상태를 처리한다 */
		protected virtual void HandleDisappearState(float a_fDeltaTime) {
			// Do Something
		}

		/** 초기화 */
		private void SubAwake() {
			// Do Something
		}

		/** 초기화 */
		private void SubInit() {
			// Do Something
		}

		/** 상태를 갱신한다 */
		private void SubOnUpdate(float a_fDeltaTime) {
			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				// Do Something
			}
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

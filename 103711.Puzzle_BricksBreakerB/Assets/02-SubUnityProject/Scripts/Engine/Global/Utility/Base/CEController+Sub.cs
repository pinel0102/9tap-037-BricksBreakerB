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

		/** 상태 */
		public enum EState {
			NONE = -1,
			IDLE,
			MOVE,
			SKILL,
			APPEAR,
			DISAPPEAR,
			[HideInInspector] MAX_VAL
		}

		/** 서브 상태 */
		public enum ESubState {
			NONE = -1,
			APPLY,
			[HideInInspector] MAX_VAL
		}

		#region 변수

		#endregion // 변수

		#region 프로퍼티
		public EState State { get; private set; } = EState.NONE;
		public ESubState SubState { get; private set; } = ESubState.NONE;
		protected Dictionary<EState, System.Func<bool>> StateCheckerDict { get; } = new Dictionary<EState, System.Func<bool>>();
		protected Dictionary<ESubState, System.Func<bool>> SubStateCheckerDict { get; } = new Dictionary<ESubState, System.Func<bool>>();

		public virtual bool IsActive => this.State != EState.NONE && this.State != EState.DISAPPEAR;
		#endregion // 프로퍼티

		#region 함수
		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				switch(this.State) {
					case EState.IDLE: this.HandleIdleState(a_fDeltaTime); break;
					case EState.MOVE: this.HandleMoveState(a_fDeltaTime); break;
					case EState.SKILL: this.HandleSkillState(a_fDeltaTime); break;
					case EState.APPEAR: this.HandleAppearState(a_fDeltaTime); break;
					case EState.DISAPPEAR: this.HandleDisappearState(a_fDeltaTime); break;
				}
			}
		}

		/** 상태를 변경한다 */
		public void SetState(EState a_eState, bool a_bIsForce = false) {
			// 강제 변경 모드 일 경우
			if(a_bIsForce) {
				this.State = a_eState;
			} else {
				this.State = (!this.StateCheckerDict.TryGetValue(a_eState, out System.Func<bool> oStateChecker) || oStateChecker()) ? a_eState : this.State;
			}
		}

		/** 서브 상태를 변경한다 */
		public void SetSubState(ESubState a_eSubState, bool a_bIsForce = false) {
			// 강제 변경 모드 일 경우
			if(a_bIsForce) {
				this.SubState = a_eSubState;
			} else {
				this.SubState = (!this.SubStateCheckerDict.TryGetValue(a_eSubState, out System.Func<bool> oSubStateChecker) || oSubStateChecker()) ? a_eSubState : this.SubState;
			}
		}

		/** 이동 상태 가능 여부를 검사한다 */
		protected virtual bool IsEnableMoveState() {
			return this.State == EState.NONE || this.State == EState.IDLE || this.State == EState.MOVE;
		}

		/** 스킬 상태 가능 여부를 검사한다 */
		protected virtual bool IsEnableSkillState() {
			return this.State == EState.NONE || this.State == EState.IDLE || this.State == EState.MOVE;
		}

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

		/** 제어자를 설정한다 */
		private void SubAwake() {
			this.StateCheckerDict.TryAdd(EState.MOVE, this.IsEnableMoveState);
			this.StateCheckerDict.TryAdd(EState.SKILL, this.IsEnableSkillState);
		}

		/** 초기화한다 */
		private void SubInit() {
			this.TargetObjList.Clear();

			this.SetState(EState.NONE);
			this.SetSubState(ESubState.NONE);
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

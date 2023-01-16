#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 제어자 */
	public abstract partial class CEController : CEComponent {
		/** 식별자 */
		private enum EKey {
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

		/** 매개 변수 */
		public new struct STParams {
			public CEComponent.STParams m_stBaseParams;
		}

		#region 변수

		#endregion // 변수

		#region 프로퍼티
		public new STParams Params { get; private set; }

		public EState State { get; private set; } = EState.NONE;
		public ESubState SubState { get; private set; } = ESubState.NONE;

		public List<CEObjComponent> TargetObjList { get; } = new List<CEObjComponent>();
		protected Dictionary<EState, System.Func<bool>> StateCheckerDict { get; } = new Dictionary<EState, System.Func<bool>>();
		protected Dictionary<ESubState, System.Func<bool>> SubStateCheckerDict { get; } = new Dictionary<ESubState, System.Func<bool>>();

		public virtual bool IsActive => this.State != EState.NONE && this.State != EState.DISAPPEAR;
		#endregion // 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 상태 검사자를 설정한다
			this.StateCheckerDict.TryAdd(EState.MOVE, this.IsEnableMoveState);
			this.StateCheckerDict.TryAdd(EState.SKILL, this.IsEnableSkillState);

			this.SubAwake();
		}

		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			base.Init(a_stParams.m_stBaseParams);
			this.Params = a_stParams;

			this.Reset();
			this.SubInit();
		}

		/** 상태를 리셋한다 */
		public override void Reset() {
			base.Reset();
			this.TargetObjList.Clear();

			this.SetState(EState.NONE, true);
			this.SetSubState(ESubState.NONE, true);
		}

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

				this.SubOnUpdate(a_fDeltaTime);
			}
		}
		#endregion // 함수

		#region 클래스 함수
		/** 매개 변수를 생성한다 */
		public static STParams MakeParams(CEngine a_oEngine) {
			return new STParams() {
				m_stBaseParams = CEComponent.MakeParams(a_oEngine, string.Empty)
			};
		}
		#endregion // 클래스 함수
	}

	/** 제어자 - 접근 */
	public abstract partial class CEController : CEComponent {
		#region 함수
		/** 이동 상태 가능 여부를 검사한다 */
		protected virtual bool IsEnableMoveState() {
			return this.State == EState.NONE || this.State == EState.IDLE || this.State == EState.MOVE;
		}

		/** 스킬 상태 가능 여부를 검사한다 */
		protected virtual bool IsEnableSkillState() {
			return this.State == EState.NONE || this.State == EState.IDLE || this.State == EState.MOVE;
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
		#endregion // 함수

		#region 제네릭 함수
		/** 타겟을 반환한다 */
		public T GetTarget<T>(int a_nIdx) where T : CEObjComponent {
			return this.TargetObjList.ExGetVal(a_nIdx, null) as T;
		}
		#endregion // 제네릭 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

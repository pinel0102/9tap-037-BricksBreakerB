#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 서브 적 객체 제어자 */
	public partial class CEEnemyObjController : CEObjController {
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
		protected override void HandleIdleState(float a_fDeltaTime) {
			base.HandleIdleState(a_fDeltaTime);

			// 플레이어 객체 공격이 가능 할 경우
			if(this.IsEnableAttackPlayerObj()) {
				this.ApplySkill(CSkillInfoTable.Inst.GetSkillInfo(this.GetOwner<CEObj>().Params.m_stObjInfo.m_eActionSkillKinds), null);
			} else {
				this.Move(this.Engine.SelPlayerObj.transform.localPosition - this.GetOwner<CEObj>().transform.localPosition);
			}
		}

		/** 이동 상태를 처리한다 */
		protected override void HandleMoveState(float a_fDeltaTime) {
			base.HandleMoveState(a_fDeltaTime);

			// 플레이어 객체 공격이 가능 할 경우
			if(this.IsEnableAttackPlayerObj()) {
				this.SetState(EState.IDLE);
				this.ApplySkill(CSkillInfoTable.Inst.GetSkillInfo(this.GetOwner<CEObj>().Params.m_stObjInfo.m_eActionSkillKinds), null);
			} else {
				this.SetMoveDirection(this.Engine.SelPlayerObj.transform.localPosition - this.GetOwner<CEObj>().transform.localPosition);
			}
		}

		/** 스킬 상태를 처리한다 */
		protected override void HandleSkillState(float a_fDeltaTime) {
			base.HandleSkillState(a_fDeltaTime);
		}

		/** 등장 상태를 처리한다 */
		protected override void HandleAppearState(float a_fDeltaTime) {
			base.HandleAppearState(a_fDeltaTime);
			m_oRealDict[EKey.UPDATE_SKIP_TIME] += a_fDeltaTime;

			// 일정 시간이 지났을 경우
			if(m_oRealDict[EKey.UPDATE_SKIP_TIME].ExIsGreateEquals(KCDefine.B_VAL_1_REAL)) {
				m_oRealDict[EKey.UPDATE_SKIP_TIME] = KCDefine.B_VAL_0_REAL;
				this.SetState(EState.IDLE);
			}
		}

		/** 초기화 */
		private void SubAwake() {
			// Do Something
		}

		/** 초기화 */
		private void SubInit() {
			// Do Something
		}

		/** 제거 되었을 경우 */
		private void SubOnDestroy() {
			try {
				// 앱이 실행 중 일 경우
				if(CSceneManager.IsAppRunning) {
					// Do Something
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CEEnemyObjController.SubOnDestroy Exception: {oException.Message}");
			}
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

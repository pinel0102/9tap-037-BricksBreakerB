#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 서브 플레이어 객체 제어자 */
	public partial class CEPlayerObjController : CEObjController {
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

			// 자동 제어 모드 일 경우
			if(this.IsAutoControl) {
				var oEnemyObj = this.Engine.FindEnemyObj(this.GetOwner<CEObj>().transform.localPosition);

				// 적 객체 공격이 가능 할 경우
				if(this.IsEnableAttackEnemyObj(oEnemyObj)) {
					this.ApplySkill(CSkillInfoTable.Inst.GetSkillInfo(this.GetOwner<CEObj>().Params.m_stObjInfo.m_eActionSkillKinds), null);
				} else {
					this.Move((oEnemyObj != null) ? oEnemyObj.transform.localPosition - this.GetOwner<CEObj>().transform.localPosition : Vector3.zero);
				}
			}
		}

		/** 이동 상태를 처리한다 */
		protected override void HandleMoveState(float a_fDeltaTime) {
			base.HandleMoveState(a_fDeltaTime);

			// 자동 제어 모드 일 경우
			if(this.IsAutoControl) {
				var oEnemyObj = this.Engine.FindEnemyObj(this.GetOwner<CEObj>().transform.localPosition);

				// 적 객체 공격이 가능 할 경우
				if(this.IsEnableAttackEnemyObj(oEnemyObj)) {
					this.SetState(EState.IDLE);
					this.ApplySkill(CSkillInfoTable.Inst.GetSkillInfo(this.GetOwner<CEObj>().Params.m_stObjInfo.m_eActionSkillKinds), null);
				} else {
					this.Move((oEnemyObj != null) ? oEnemyObj.transform.localPosition - this.GetOwner<CEObj>().transform.localPosition : Vector3.zero);
				}
			}
		}

		/** 스킬 상태를 처리한다 */
		protected override void HandleSkillState(float a_fDeltaTime) {
			base.HandleSkillState(a_fDeltaTime);
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
				CFunc.ShowLogWarning($"CEPlayerObjController.SubOnDestroy Exception: {oException.Message}");
			}
		}

		/** 상태를 갱신한다 */
		private void SubOnUpdate(float a_fDeltaTime) {
			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				// Do Something
			}
		}

		/** 다중 스킬 타겟을 설정한다 */
		private void SetupMultiSkillTargets(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo, List<CEObjComponent> a_oOutTargetObjList) {
			// Do Something
		}

		/** 단일 스킬 타겟을 설정한다 */
		private void SetupSingleSkillTargets(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo, List<CEObjComponent> a_oOutTargetObjList) {
			// Do Something
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

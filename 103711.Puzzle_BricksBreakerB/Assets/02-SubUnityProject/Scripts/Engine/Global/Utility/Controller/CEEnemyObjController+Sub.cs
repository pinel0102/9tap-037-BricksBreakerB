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
			UPDATE_SKIP_TIME,
			[HideInInspector] MAX_VAL
		}

		#region 변수
		private Dictionary<ESubKey, float> m_oRealDict = new Dictionary<ESubKey, float>();
		#endregion // 변수

		#region 프로퍼티

		#endregion // 프로퍼티

		#region 함수
		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				// Do Something
			}
		}

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
			float fUpdateSkipTime = m_oRealDict.GetValueOrDefault(ESubKey.UPDATE_SKIP_TIME);

			m_oRealDict.ExReplaceVal(ESubKey.UPDATE_SKIP_TIME, fUpdateSkipTime + a_fDeltaTime);

			// 일정 시간이 지났을 경우
			if(m_oRealDict.GetValueOrDefault(ESubKey.UPDATE_SKIP_TIME).ExIsGreateEquals(KCDefine.B_VAL_1_REAL)) {
				this.SetState(EState.IDLE);
				m_oRealDict.ExReplaceVal(ESubKey.UPDATE_SKIP_TIME, KCDefine.B_VAL_0_REAL);
			}
		}

		/** 스킬을 적용시킨다 */
		protected override void DoApplySkill(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo) {
			base.DoApplySkill(a_stSkillInfo, a_oSkillTargetInfo);

			var oSkill = this.Engine.CreateSkill(a_stSkillInfo, a_oSkillTargetInfo, this.GetOwner<CEObj>());
			oSkill.transform.localPosition = this.GetOwner<CEObj>().transform.localPosition;
			oSkill.GetController<CESkillController>().TargetObjList.ExAddVal(this.Engine.SelPlayerObj);

			this.Engine.SkillList.ExAddVal(oSkill);
			oSkill.GetController<CESkillController>().Apply();
		}

		/** 제어자를 설정한다 */
		private void SubAwake() {
			// Do Something
		}

		/** 초기화한다 */
		private void SubInit() {
			// Do Something
		}

		/** 플레이어 객체 공격 가능 여부를 검사한다 */
		private bool IsEnableAttackPlayerObj() {
			var stDelta = this.Engine.SelPlayerObj.transform.localPosition - this.GetOwner<CEObj>().transform.localPosition;
			return stDelta.sqrMagnitude.ExIsLessEquals(Mathf.Pow((float)this.GetOwner<CEObj>().AbilityValDictWrapper.m_oDict01.ExGetAbilityVal(EAbilityKinds.STAT_ATK_RANGE_01), KCDefine.B_VAL_2_REAL));
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

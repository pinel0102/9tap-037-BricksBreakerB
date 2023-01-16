using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 서브 스킬 제어자 */
	public partial class CESkillController : CEController {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			APPLY_TIMES,
			UPDATE_SKIP_TIME,
			[HideInInspector] MAX_VAL
		}

		#region 변수
		private Dictionary<ESubKey, int> m_oIntDict = new Dictionary<ESubKey, int>();
		private Dictionary<ESubKey, float> m_oRealDict = new Dictionary<ESubKey, float>();
		#endregion // 변수

		#region 프로퍼티

		#endregion // 프로퍼티

		#region 함수
		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(this.SubState != ESubState.NONE && CSceneManager.IsAppRunning) {
				switch(this.SubState) {
					case ESubState.APPLY: this.HandleApplySubState(a_fDeltaTime); break;
				}
			}
		}

		/** 스킬을 적용한다 */
		public void Apply() {
			this.SetState(EState.IDLE);
			this.Owner.GetOwner<CEObj>().GetController<CEObjController>().ApplySkillTimeDict.ExReplaceVal(this.GetOwner<CESkill>().Params.m_stSkillInfo.m_eSkillKinds, System.DateTime.Now);
		}

		/** 대기 상태를 처리한다 */
		protected override void HandleIdleState(float a_fDeltaTime) {
			base.HandleIdleState(a_fDeltaTime);
			float fUpdateSkipTime = m_oRealDict.GetValueOrDefault(ESubKey.UPDATE_SKIP_TIME);

			m_oRealDict.ExReplaceVal(ESubKey.UPDATE_SKIP_TIME, fUpdateSkipTime + a_fDeltaTime);

			// 딜레이 시간이 지났을 경우
			if(m_oRealDict.GetValueOrDefault(ESubKey.UPDATE_SKIP_TIME).ExIsGreateEquals(this.GetOwner<CESkill>().Params.m_stSkillInfo.m_stTimeInfo.m_fDelay)) {
				this.SetState(EState.SKILL);
				this.SetSubState(ESubState.APPLY);

				m_oRealDict.ExReplaceVal(ESubKey.UPDATE_SKIP_TIME, KCDefine.B_VAL_0_REAL);
			}
		}

		/** 효과를 설정한다 */
		private void SubAwake() {
			// Do Something
		}

		/** 초기화한다 */
		private void SubInit() {
			m_oIntDict.Clear();
			m_oRealDict.Clear();
		}

		/** 적용 서브 상태를 처리한다 */
		private void HandleApplySubState(float a_fDeltaTime) {
			int nApplyTimes = m_oIntDict.GetValueOrDefault(ESubKey.APPLY_TIMES);
			float fUpdateSkipTime = m_oRealDict.GetValueOrDefault(ESubKey.UPDATE_SKIP_TIME);

			m_oRealDict.ExReplaceVal(ESubKey.UPDATE_SKIP_TIME, fUpdateSkipTime + a_fDeltaTime);

			// 적용 간격이 지났을 경우
			if(nApplyTimes < this.GetOwner<CESkill>().Params.m_stSkillInfo.m_nMaxApplyTimes && m_oRealDict.GetValueOrDefault(ESubKey.UPDATE_SKIP_TIME).ExIsGreateEquals(this.GetOwner<CESkill>().Params.m_stSkillInfo.m_stTimeInfo.m_fDeltaTime * (nApplyTimes - KCDefine.B_VAL_1_INT))) {
				switch((ESkillApplyType)((int)this.GetOwner<CESkill>().Params.m_stSkillInfo.m_eSkillApplyKinds).ExKindsToType()) {
					case ESkillApplyType.MULTI: this.ApplyMultiSkill(); break;
					case ESkillApplyType.SINGLE: this.ApplySingleSkill(); break;
				}

				m_oIntDict.ExReplaceVal(ESubKey.APPLY_TIMES, nApplyTimes + KCDefine.B_VAL_1_INT);
			}

			// 적용 시간이 지났을 경우
			if(m_oRealDict.GetValueOrDefault(ESubKey.UPDATE_SKIP_TIME).ExIsGreateEquals(this.GetOwner<CESkill>().Params.m_stSkillInfo.m_stTimeInfo.m_fDuration)) {
				this.Owner.GetOwner<CEObj>().GetController<CEObjController>().SetState(EState.IDLE);
				this.Engine.RemoveSkill(this.GetOwner<CESkill>());
			}
		}

		/** 다중 스킬을 적용한다 */
		private void ApplyMultiSkill() {
			// Do Something
		}

		/** 단일 스킬을 적용한다 */
		private void ApplySingleSkill() {
			// Do Something
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

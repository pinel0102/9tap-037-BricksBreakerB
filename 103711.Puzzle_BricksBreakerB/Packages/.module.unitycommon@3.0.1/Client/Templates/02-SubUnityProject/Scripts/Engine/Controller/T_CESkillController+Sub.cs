#if SCRIPT_TEMPLATE_ONLY
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
		private Dictionary<ESubKey, int> m_oSubIntDict = new Dictionary<ESubKey, int>() {
			[ESubKey.APPLY_TIMES] = KCDefine.B_VAL_0_INT
		};

		private Dictionary<ESubKey, float> m_oSubRealDict = new Dictionary<ESubKey, float>() {
			[ESubKey.UPDATE_SKIP_TIME] = KCDefine.B_VAL_0_REAL
		};
		#endregion // 변수

		#region 프로퍼티

		#endregion // 프로퍼티

		#region 함수
		/** 스킬을 적용한다 */
		public void Apply() {
			this.SetState(EState.IDLE);
			this.Owner.GetOwner<CEObj>().GetController<CEObjController>().ApplySkillTimeDict.ExReplaceVal(this.GetOwner<CESkill>().Params.m_stSkillInfo.m_eSkillKinds, System.DateTime.Now);
		}

		/** 대기 상태를 처리한다 */
		protected override void HandleIdleState(float a_fDeltaTime) {
			base.HandleIdleState(a_fDeltaTime);
			m_oSubRealDict[ESubKey.UPDATE_SKIP_TIME] += a_fDeltaTime;

			// 딜레이 시간이 지났을 경우
			if(m_oSubRealDict[ESubKey.UPDATE_SKIP_TIME].ExIsGreateEquals(this.GetOwner<CESkill>().Params.m_stSkillInfo.m_stTimeInfo.m_fDelay)) {
				m_oSubRealDict[ESubKey.UPDATE_SKIP_TIME] = KCDefine.B_VAL_0_REAL;

				this.SetState(EState.SKILL);
				this.SetSubState(ESubState.APPLY);
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
				CFunc.ShowLogWarning($"CESkillController.SubOnDestroy Exception: {oException.Message}");
			}
		}

		/** 상태를 갱신한다 */
		private void SubOnUpdate(float a_fDeltaTime) {
			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				// Do Something
			}
		}

		/** 적용 서브 상태를 처리한다 */
		private void HandleApplySubState(float a_fDeltaTime) {
			m_oSubRealDict[ESubKey.UPDATE_SKIP_TIME] += a_fDeltaTime;

			// 적용 간격이 지났을 경우
			if(m_oSubIntDict[ESubKey.APPLY_TIMES] < this.GetOwner<CESkill>().Params.m_stSkillInfo.m_nMaxApplyTimes && m_oSubRealDict[ESubKey.UPDATE_SKIP_TIME].ExIsGreateEquals(this.GetOwner<CESkill>().Params.m_stSkillInfo.m_stTimeInfo.m_fDeltaTime * (m_oSubIntDict[ESubKey.APPLY_TIMES] - KCDefine.B_VAL_1_INT))) {
				switch((ESkillApplyType)((int)this.GetOwner<CESkill>().Params.m_stSkillInfo.m_eSkillApplyKinds).ExKindsToType()) {
					case ESkillApplyType.MULTI: this.ApplyMultiSkill(); break;
					case ESkillApplyType.SINGLE: this.ApplySingleSkill(); break;
				}

				m_oSubIntDict[ESubKey.APPLY_TIMES] += KCDefine.B_VAL_1_INT;
			}

			// 적용 시간이 지났을 경우
			if(m_oSubRealDict[ESubKey.UPDATE_SKIP_TIME].ExIsGreateEquals(this.GetOwner<CESkill>().Params.m_stSkillInfo.m_stTimeInfo.m_fDuration)) {
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
#endif // #if SCRIPT_TEMPLATE_ONLY

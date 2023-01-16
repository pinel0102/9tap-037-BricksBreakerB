#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 적 객체 제어자 */
	public partial class CEEnemyObjController : CEObjController {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			UPDATE_SKIP_TIME,
			[HideInInspector] MAX_VAL
		}

		/** 매개 변수 */
		public new struct STParams {
			public CEObjController.STParams m_stBaseParams;
		}

		#region 변수
		private Dictionary<EKey, float> m_oRealDict = new Dictionary<EKey, float>() {
			[EKey.UPDATE_SKIP_TIME] = KCDefine.B_VAL_0_REAL
		};
		#endregion // 변수

		#region 프로퍼티
		public new STParams Params { get; private set; }
		#endregion // 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();
			this.SubAwake();
		}

		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			base.Init(a_stParams.m_stBaseParams);
			this.Params = a_stParams;

			this.SetEnableAutoControl(true);
			this.SubInit();
		}

		/** 제거 되었을 경우 */
		public override void OnDestroy() {
			base.OnDestroy();

			try {
				// 앱이 실행 중 일 경우
				if(CSceneManager.IsAppRunning) {
					this.SubOnDestroy();
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CEEnemyObjController.OnDestroy Exception: {oException.Message}");
			}
		}

		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				this.SubOnUpdate(a_fDeltaTime);
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
		#endregion // 함수

		#region 클래스 함수
		/** 매개 변수를 생성한다 */
		public new static STParams MakeParams(CEngine a_oEngine) {
			return new STParams() {
				m_stBaseParams = CEObjController.MakeParams(a_oEngine)
			};
		}
		#endregion // 클래스 함수
	}

	/** 적 객체 제어자 - 접근 */
	public partial class CEEnemyObjController : CEObjController {
		#region 함수
		/** 플레이어 객체 공격 가능 여부를 검사한다 */
		private bool IsEnableAttackPlayerObj() {
			var stDelta = this.Engine.SelPlayerObj.transform.localPosition - this.GetOwner<CEObj>().transform.localPosition;
			return stDelta.sqrMagnitude.ExIsLessEquals(Mathf.Pow((float)this.GetOwner<CEObj>().AbilityValDictWrapper.m_oDict01.ExGetAbilityVal(EAbilityKinds.STAT_ATK_RANGE_01), KCDefine.B_VAL_2_REAL));
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 플레이어 객체 제어자 */
	public partial class CEPlayerObjController : CEObjController {
#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

#region 추가
			this.SubSetupAwake();
#endregion // 추가
		}

		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			base.Init(a_stParams.m_stBaseParams);
			this.Params = a_stParams;

#region 추가
			this.SubInit();
#endregion // 추가
		}
#endregion // 함수
	}

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
		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				// Do Something
			}
		}

		/** 이동을 처리한다 */
		public override void Move(Vector3 a_stDirection, EVecType a_eVecType = EVecType.DIRECTION) {
			base.Move(a_stDirection);

			// 방향 모드 일 경우
			if(a_eVecType == EVecType.DIRECTION) {
				this.SetState((this.State == EState.MOVE && a_stDirection.ExIsEquals(Vector3.zero)) ? EState.IDLE : this.State);
			}
		}

		/** 스킬을 적용한다 */
		public override void ApplySkill(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo) {
			base.ApplySkill(a_stSkillInfo, a_oSkillTargetInfo);
			CSceneManager.GetSceneManager<GameScene.CSubGameSceneManager>(KCDefine.B_SCENE_N_GAME).SetEnableUpdateUIsState(true);
		}

		/** 대기 상태를 처리한다 */
		protected override void HandleIdleState(float a_fDeltaTime) {
			base.HandleIdleState(a_fDeltaTime);

			// 자동 제어 모드 일 경우
			if(this.IsAutoControl) {
				var oEnemyObj = base.Params.m_stBaseParams.m_stBaseParams.m_oEngine.FindEnemyObj(this.GetOwner<CEObj>().transform.localPosition);

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
				var oEnemyObj = base.Params.m_stBaseParams.m_stBaseParams.m_oEngine.FindEnemyObj(this.GetOwner<CEObj>().transform.localPosition);

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

		/** 스킬을 적용시킨다 */
		protected override void DoApplySkill(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo) {
			base.DoApplySkill(a_stSkillInfo, a_oSkillTargetInfo);
			var oTargetObjList = CCollectionManager.Inst.SpawnList<CEObjComponent>();

			try {
				switch(a_stSkillInfo.SkillApplyType) {
					case ESkillApplyType.MULTI: this.SetupMultiSkillTargets(a_stSkillInfo, a_oSkillTargetInfo, oTargetObjList); break;
					case ESkillApplyType.SINGLE: this.SetupSingleSkillTargets(a_stSkillInfo, a_oSkillTargetInfo, oTargetObjList); break;
				}

				var oSkill = base.Params.m_stBaseParams.m_stBaseParams.m_oEngine.CreateSkill(a_stSkillInfo, a_oSkillTargetInfo, this.GetOwner<CEObj>());
				oSkill.transform.localPosition = this.GetOwner<CEObj>().transform.localPosition;
				oTargetObjList.ExCopyTo(oSkill.GetController<CESkillController>().TargetObjList, (a_oTargetObj) => a_oTargetObj);

				oSkill.GetController<CESkillController>().Apply();
				base.Params.m_stBaseParams.m_stBaseParams.m_oEngine.SkillList.ExAddVal(oSkill);
			} finally {
				CCollectionManager.Inst.DespawnList(oTargetObjList);
			}
		}

		/** 제어자를 설정한다 */
		private void SubSetupAwake() {
			// Do Something
		}

		/** 초기화한다 */
		private void SubInit() {
			this.SetState(EState.IDLE, true);
		}

		/** 다중 스킬 타겟을 설정한다 */
		private void SetupMultiSkillTargets(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo, List<CEObjComponent> a_oOutTargetObjList) {
			// Do Something
		}

		/** 단일 스킬 타겟을 설정한다 */
		private void SetupSingleSkillTargets(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo, List<CEObjComponent> a_oOutTargetObjList) {
			// Do Something
		}

		/** 적 객체 공격 가능 여부를 검사한다 */
		private bool IsEnableAttackEnemyObj(CEObj a_oEnemyObj) {
			// 적 객체가 존재 할 경우
			if(a_oEnemyObj != null) {
				var stDelta = a_oEnemyObj.transform.localPosition - this.GetOwner<CEObj>().transform.localPosition;
				return stDelta.sqrMagnitude.ExIsLessEquals(Mathf.Pow((float)this.GetOwner<CEObj>().AbilityValDictWrapper.m_oDict01.ExGetAbilityVal(EAbilityKinds.STAT_ATK_RANGE_01), KCDefine.B_VAL_2_REAL));
			}

			return false;
		}
#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

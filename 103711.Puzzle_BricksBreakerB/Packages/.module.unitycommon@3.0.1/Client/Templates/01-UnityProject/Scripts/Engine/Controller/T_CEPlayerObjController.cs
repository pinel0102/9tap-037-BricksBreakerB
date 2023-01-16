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
		/** 매개 변수 */
		public new struct STParams {
			public CEObjController.STParams m_stBaseParams;
		}

		#region 변수

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

			this.SetState(EState.IDLE, true);
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
				CFunc.ShowLogWarning($"CEPlayerObjController.OnDestroy Exception: {oException.Message}");
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

		/** 스킬을 적용시킨다 */
		protected override void DoApplySkill(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo) {
			base.DoApplySkill(a_stSkillInfo, a_oSkillTargetInfo);
			var oTargetObjList = CCollectionManager.Inst.SpawnList<CEObjComponent>();

			try {
				switch(a_stSkillInfo.SkillApplyType) {
					case ESkillApplyType.MULTI: this.SetupMultiSkillTargets(a_stSkillInfo, a_oSkillTargetInfo, oTargetObjList); break;
					case ESkillApplyType.SINGLE: this.SetupSingleSkillTargets(a_stSkillInfo, a_oSkillTargetInfo, oTargetObjList); break;
				}

				var oSkill = this.Engine.CreateSkill(a_stSkillInfo, a_oSkillTargetInfo, this.GetOwner<CEObj>());
				oSkill.transform.localPosition = this.GetOwner<CEObj>().transform.localPosition;
				oTargetObjList.ExCopyTo(oSkill.GetController<CESkillController>().TargetObjList, (a_oTargetObj) => a_oTargetObj);

				oSkill.GetController<CESkillController>().Apply();
				this.Engine.SkillList.ExAddVal(oSkill);
			} finally {
				CCollectionManager.Inst.DespawnList(oTargetObjList);
			}
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

	/** 플레이어 객체 제어자 - 접근 */
	public partial class CEPlayerObjController : CEObjController {
		#region 함수
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

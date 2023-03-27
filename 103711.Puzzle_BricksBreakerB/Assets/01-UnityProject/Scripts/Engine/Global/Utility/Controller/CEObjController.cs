using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 객체 제어자 */
	public partial class CEObjController : CEController {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			IS_AUTO_CONTROL,
			UPDATE_SKIP_TIME,
			MOVE_POS,
			MOVE_DIRECTION,
			APPLY_SKILL_INFO,
			APPLY_SKILL_TARGET_INFO,
			[HideInInspector] MAX_VAL
		}

		/** 매개 변수 */
		public new struct STParams {
			public CEController.STParams m_stBaseParams;
		}

		#region 변수
		private Dictionary<EKey, bool> m_oBoolDict = new Dictionary<EKey, bool>() {
			[EKey.IS_AUTO_CONTROL] = false
		};

		private Dictionary<EKey, float> m_oRealDict = new Dictionary<EKey, float>() {
			[EKey.UPDATE_SKIP_TIME] = KCDefine.B_VAL_0_REAL
		};

		private Dictionary<EKey, Vector3> m_oVec3Dict = new Dictionary<EKey, Vector3>() {
			[EKey.MOVE_POS] = Vector3.zero,
			[EKey.MOVE_DIRECTION] = Vector3.zero
		};

		private Dictionary<EKey, STSkillInfo> m_oSkillInfoDict = new Dictionary<EKey, STSkillInfo>() {
			[EKey.APPLY_SKILL_INFO] = STSkillInfo.INVALID
		};

		private Dictionary<EKey, CSkillTargetInfo> m_oSkillTargetInfoDict = new Dictionary<EKey, CSkillTargetInfo>() {
			[EKey.APPLY_SKILL_TARGET_INFO] = null
		};
		#endregion // 변수

		#region 프로퍼티
		public new STParams Params { get; private set; }
		public Dictionary<ESkillKinds, System.DateTime> ApplySkillTimeDict { get; } = new Dictionary<ESkillKinds, System.DateTime>();

		public bool IsAutoControl => m_oBoolDict[EKey.IS_AUTO_CONTROL];

		public Vector3 MovePos => m_oVec3Dict[EKey.MOVE_POS];
		public Vector3 MoveDirection => m_oVec3Dict[EKey.MOVE_DIRECTION];
		public CSkillTargetInfo ApplySkillTargetInfo => m_oSkillTargetInfoDict[EKey.APPLY_SKILL_TARGET_INFO];
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

			this.SubInit();
		}

		/** 상태를 리셋한다 */
		public override void Reset() {
			base.Reset();
			this.ResetApplySkillInfo();
		}

        /** 객체 정보를 리셋한다 */
		public virtual void ResetObjInfo(STObjInfo a_stObjInfo) {
            // 리셋 가능 할 경우
			if(a_stObjInfo.m_eObjKinds != this.GetOwner<CEObj>().Params.m_stObjInfo.m_eObjKinds) {
				var stParams = this.GetOwner<CEObj>().Params;

                stParams.m_stObjInfo = a_stObjInfo;

                this.GetOwner<CEObj>().Init(stParams);
                this.GetOwner<CEObj>().RefreshText(a_stObjInfo.m_eObjKinds);                
			}

			this.SubResetObjInfo(a_stObjInfo);
		}

		/** 적용 스킬 정보를 리셋한다 */
		public virtual void ResetApplySkillInfo() {
			m_oSkillInfoDict[EKey.APPLY_SKILL_INFO] = STSkillInfo.INVALID;
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
				CFunc.ShowLogWarning($"CEObjController.OnDestroy Exception: {oException.Message}");
			}
		}

		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning && this.IsActive) {
				m_oRealDict[EKey.UPDATE_SKIP_TIME] += a_fDeltaTime;

				// 일정 시간이 지났을 경우
				if(m_oRealDict[EKey.UPDATE_SKIP_TIME].ExIsGreateEquals(KCDefine.U_DELAY_DEF)) {
					m_oRealDict[EKey.UPDATE_SKIP_TIME] = KCDefine.B_VAL_0_REAL;
					var oAbilityKindsInfoList = CCollectionManager.Inst.SpawnList<(EAbilityKinds, EAbilityKinds)>();

					try {
						oAbilityKindsInfoList.ExAddVal((EAbilityKinds.STAT_HP_01, EAbilityKinds.STAT_HP_RECOVERY_01));
						oAbilityKindsInfoList.ExAddVal((EAbilityKinds.STAT_MP_01, EAbilityKinds.STAT_MP_RECOVERY_01));
						oAbilityKindsInfoList.ExAddVal((EAbilityKinds.STAT_SP_01, EAbilityKinds.STAT_SP_RECOVERY_01));

						for(int i = 0; i < oAbilityKindsInfoList.Count; ++i) {
							decimal dmVal = this.GetOwner<CEObj>().AbilityValDictWrapper.m_oDict01.ExGetAbilityVal(oAbilityKindsInfoList[i].Item1);
							decimal dmMaxVal = this.GetOwner<CEObj>().AbilityValDictWrapper.m_oDict02.ExGetAbilityVal(oAbilityKindsInfoList[i].Item1);
							decimal dmRecoveryVal = this.GetOwner<CEObj>().AbilityValDictWrapper.m_oDict01.ExGetAbilityVal(oAbilityKindsInfoList[i].Item2);

							this.GetOwner<CEObj>().AbilityValDictWrapper.m_oDict01.ExReplaceVal(oAbilityKindsInfoList[i].Item1, System.Math.Clamp(dmVal + (dmRecoveryVal * (decimal)m_oRealDict[EKey.UPDATE_SKIP_TIME]), KCDefine.B_VAL_0_INT, dmMaxVal));
						}
					} finally {
						CCollectionManager.Inst.DespawnList(oAbilityKindsInfoList);
					}
				}

				this.SubOnUpdate(a_fDeltaTime);
			}
		}

		/** 공격을 처리한다 */
		public virtual void Attack(CEObj a_oTargetObj, CESkill a_oSkill) {
			var oAbilityValDict = CCollectionManager.Inst.SpawnDict<EAbilityKinds, decimal>();

			try {
				float fPercent = Random.Range(KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_1_REAL);
				float fCriticalRate = (float)a_oTargetObj.AbilityValDictWrapper.m_oDict01.ExGetAbilityVal(EAbilityKinds.STAT_CRITICAL_RATE_01);

				this.GetOwner<CEObj>().AbilityValDictWrapper.m_oDict01.ExCopyTo(oAbilityValDict, (a_dmAbilityVal) => a_dmAbilityVal);
				global::Func.SetupAbilityVals(a_oSkill.Params.m_stSkillInfo, a_oSkill.Params.m_oSkillTargetInfo, oAbilityValDict);

				// 공격을 회피했을 경우
				if(fPercent.ExIsLessEquals((float)a_oTargetObj.AbilityValDictWrapper.m_oDict01.ExGetAbilityVal(EAbilityKinds.STAT_AVOID_RATE_01))) {
					this.GetOwner<CEObj>().Params.m_stBaseParams.m_oCallbackDict.GetValueOrDefault(CEObj.ECallback.ENGINE_OBJ_EVENT)?.Invoke(a_oTargetObj, EEngineObjEvent.AVOID, string.Empty);
				} else {
					decimal dmDamage = System.Math.Clamp(oAbilityValDict.ExGetAbilityVal(EAbilityKinds.STAT_ATK_01) - a_oTargetObj.AbilityValDictWrapper.m_oDict01.ExGetAbilityVal(EAbilityKinds.STAT_DEF_01), KCDefine.B_VAL_0_INT, decimal.MaxValue);
					decimal dmPDamage = System.Math.Clamp(oAbilityValDict.ExGetAbilityVal(EAbilityKinds.STAT_P_ATK_01) - a_oTargetObj.AbilityValDictWrapper.m_oDict01.ExGetAbilityVal(EAbilityKinds.STAT_P_DEF_01), KCDefine.B_VAL_0_INT, decimal.MaxValue);
					decimal dmMDamage = System.Math.Clamp(oAbilityValDict.ExGetAbilityVal(EAbilityKinds.STAT_M_ATK_01) - a_oTargetObj.AbilityValDictWrapper.m_oDict01.ExGetAbilityVal(EAbilityKinds.STAT_M_DEF_01), KCDefine.B_VAL_0_INT, decimal.MaxValue);

					decimal dmTotalDamage = dmDamage + dmPDamage + dmMDamage;
					dmTotalDamage = fPercent.ExIsLessEquals(fCriticalRate) ? dmTotalDamage * KCDefine.B_VAL_2_INT : dmTotalDamage;

					a_oTargetObj.AbilityValDictWrapper.m_oDict01.ExIncrAbilityVal(EAbilityKinds.STAT_HP_01, -dmTotalDamage);
					this.GetOwner<CEObj>().Params.m_stBaseParams.m_oCallbackDict.GetValueOrDefault(CEObj.ECallback.ENGINE_OBJ_EVENT)?.Invoke(a_oTargetObj, fPercent.ExIsLessEquals(fCriticalRate) ? EEngineObjEvent.CRITICAL_DAMAGE : EEngineObjEvent.DAMAGE, $"{dmTotalDamage:0}");
				}
			} finally {
				CCollectionManager.Inst.DespawnDict(oAbilityValDict);
			}
		}

		/** 이동을 처리한다 */
		public virtual void Move(Vector3 a_stVal, EVecType a_eVecType = EVecType.DIRECTION) {
			this.SetState(EState.MOVE);
			this.SetMovePos((a_eVecType == EVecType.POS) ? a_stVal : Vector3.zero);
			this.SetMoveDirection((a_eVecType == EVecType.DIRECTION) ? a_stVal : Vector3.zero);
		}

		/** 스킬을 적용시킨다 */
		public virtual void ApplySkill(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo) {
			// 스킬 적용이 가능 할 경우
			if(this.IsEnableSkillState() && this.IsEnableApplySkill(a_stSkillInfo, a_oSkillTargetInfo)) {
				this.SetState(EState.SKILL);
				this.DoApplySkill(a_stSkillInfo, a_oSkillTargetInfo);
			}
		}

		/** 스킬을 적용시킨다 */
		protected virtual void DoApplySkill(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo) {
			m_oSkillInfoDict[EKey.APPLY_SKILL_INFO] = a_stSkillInfo;
			m_oSkillTargetInfoDict[EKey.APPLY_SKILL_TARGET_INFO] = a_oSkillTargetInfo;
		}
		#endregion // 함수

		#region 클래스 함수
		/** 매개 변수를 생성한다 */
		public new static STParams MakeParams(CEngine a_oEngine) {
			return new STParams() {
				m_stBaseParams = CEController.MakeParams(a_oEngine)
			};
		}
		#endregion // 클래스 함수
	}

	/** 객체 제어자 - 접근 */
	public partial class CEObjController : CEController {
		#region 함수
		/** 자동 제어 여부를 변경한다 */
		public void SetEnableAutoControl(bool a_bIsAutoControl) {
			// 수동 제어 모드 일 경우
			if(!a_bIsAutoControl && this.State == EState.MOVE) {
				this.SetState(EState.IDLE);
			}

			m_oBoolDict[EKey.IS_AUTO_CONTROL] = a_bIsAutoControl;
		}

		/** 이동 위치를 변경한다 */
		public void SetMovePos(Vector3 a_stPos) {
			m_oVec3Dict[EKey.MOVE_POS] = a_stPos;
		}

		/** 이동 방향을 변경한다 */
		public void SetMoveDirection(Vector3 a_stDirection) {
			m_oVec3Dict[EKey.MOVE_DIRECTION] = a_stDirection.normalized;
		}

		/** 스킬 적용 가능 여부를 검사한다 */
		protected virtual bool IsEnableApplySkill(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo) {
			// 적용 스킬 타겟 정보가 없을 경우
			if(m_oSkillInfoDict[EKey.APPLY_SKILL_INFO].m_eSkillKinds == ESkillKinds.NONE) {
				return true;
			}

			var oAbilityValDict = CCollectionManager.Inst.SpawnDict<EAbilityKinds, decimal>();

			try {
				global::Func.SetupAbilityVals(a_stSkillInfo, a_oSkillTargetInfo, oAbilityValDict);
				double dblDeltaTime = System.DateTime.Now.ExGetDeltaTime(this.ApplySkillTimeDict.GetValueOrDefault(a_stSkillInfo.m_eSkillKinds, System.DateTime.Today.AddDays(-KCDefine.B_VAL_1_INT)));

				return (decimal)dblDeltaTime >= this.GetOwner<CEObj>().AbilityValDictWrapper.m_oDict01.ExGetAbilityVal((a_stSkillInfo.SkillType == ESkillType.ACTION) ? EAbilityKinds.STAT_ATK_DELAY_01 : EAbilityKinds.STAT_SKILL_DELAY_01, (a_stSkillInfo.SkillType == ESkillType.ACTION) ? this.GetOwner<CEObj>().AbilityValDictWrapper.m_oDict01.GetValueOrDefault(EAbilityKinds.STAT_ATK_DELAY_01) : oAbilityValDict.GetValueOrDefault(EAbilityKinds.STAT_SKILL_DELAY_01));
			} finally {
				CCollectionManager.Inst.DespawnDict(oAbilityValDict);
			}
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

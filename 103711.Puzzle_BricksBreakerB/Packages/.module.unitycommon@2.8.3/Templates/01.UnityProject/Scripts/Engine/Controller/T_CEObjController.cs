#if SCRIPT_TEMPLATE_ONLY
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
		private Dictionary<EKey, STSkillInfo> m_oSkillInfoDict = new Dictionary<EKey, STSkillInfo>() {
			[EKey.APPLY_SKILL_INFO] = STSkillInfo.INVALID
		};

		private Dictionary<EKey, bool> m_oBoolDict = new Dictionary<EKey, bool>();
		private Dictionary<EKey, Vector3> m_oVec3Dict = new Dictionary<EKey, Vector3>();
		private Dictionary<EKey, CSkillTargetInfo> m_oSkillTargetInfoDict = new Dictionary<EKey, CSkillTargetInfo>();
#endregion // 변수

#region 프로퍼티
		public new STParams Params { get; private set; }
		public Dictionary<ESkillKinds, System.DateTime> ApplySkillTimeDict { get; } = new Dictionary<ESkillKinds, System.DateTime>();

		public bool IsAutoControl => m_oBoolDict.GetValueOrDefault(EKey.IS_AUTO_CONTROL);
		public Vector3 MovePos => m_oVec3Dict.GetValueOrDefault(EKey.MOVE_POS);
		public Vector3 MoveDirection => m_oVec3Dict.GetValueOrDefault(EKey.MOVE_DIRECTION);
		public CSkillTargetInfo ApplySkillTargetInfo => m_oSkillTargetInfoDict.GetValueOrDefault(EKey.APPLY_SKILL_TARGET_INFO);
#endregion // 프로퍼티

#region 함수
		/** 적용 스킬 정보를 리셋한다 */
		public virtual void ResetApplySkillInfo() {
			m_oSkillInfoDict.ExReplaceVal(EKey.APPLY_SKILL_INFO, STSkillInfo.INVALID);
		}

		/** 자동 제어 여부를 변경한다 */
		public void SetIsAutoControl(bool a_bIsAutoControl) {
			// 수동 제어 모드 일 경우
			if(!a_bIsAutoControl && this.State == EState.MOVE) {
				this.SetState(EState.IDLE);
			}

			m_oBoolDict.ExReplaceVal(EKey.IS_AUTO_CONTROL, a_bIsAutoControl);
		}

		/** 이동 위치를 변경한다 */
		public void SetMovePos(Vector3 a_stPos) {
			m_oVec3Dict.ExReplaceVal(EKey.MOVE_POS, a_stPos);
		}

		/** 이동 방향을 변경한다 */
		public void SetMoveDirection(Vector3 a_stDirection) {
			m_oVec3Dict.ExReplaceVal(EKey.MOVE_DIRECTION, a_stDirection.normalized);
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
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

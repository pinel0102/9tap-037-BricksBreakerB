using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 스킬 */
	public partial class CESkill : CEObjComponent {
		/** 매개 변수 */
		public new struct STParams {
			public CEObjComponent.STParams m_stBaseParams;
			public STSkillInfo m_stSkillInfo;
			public CSkillTargetInfo m_oSkillTargetInfo;
		}

#region 프로퍼티
		public new STParams Params { get; private set; }
		public List<CEFX> FXList { get; } = new List<CEFX>();
#endregion // 프로퍼티

#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();
			this.SubSetupAwake();
		}

		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			base.Init(a_stParams.m_stBaseParams);
			this.Params = a_stParams;

			this.SubInit();
		}

		/** 어빌리티 값을 설정한다 */
		protected override void DoSetupAbilityVals(bool a_bIsReset = true) {
			base.DoSetupAbilityVals(a_bIsReset);

			// 스킬 정보가 존재 할 경우
			if(this.Params.m_stSkillInfo.m_eSkillKinds.ExIsValid()) {
				global::Func.SetupAbilityVals(this.Params.m_stSkillInfo, this.Params.m_oSkillTargetInfo, this.AbilityValDictWrapper.m_oDict02);
			}
		}
#endregion // 함수

#region 클래스 함수
		/** 스킬 매개 변수를 생성한다 */
		public static STParams MakeParams(CEngine a_oEngine, STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo, CEController a_oController = null, string a_oObjsPoolKey = KCDefine.B_TEXT_EMPTY) {
			return new STParams() {
				m_stBaseParams = CEObjComponent.MakeParams(a_oEngine, a_oController, a_oObjsPoolKey), m_stSkillInfo = a_stSkillInfo, m_oSkillTargetInfo = a_oSkillTargetInfo
			};
		}
#endregion // 클래스 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

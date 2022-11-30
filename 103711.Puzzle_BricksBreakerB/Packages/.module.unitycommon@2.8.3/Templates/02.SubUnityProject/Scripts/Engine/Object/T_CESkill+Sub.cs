#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 스킬 */
	public partial class CESkill : CEObjComponent {
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

	/** 서브 스킬 */
	public partial class CESkill : CEObjComponent {
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
		/** 컴포넌트를 설정한다 */
		private void SubSetupAwake() {
			// Do Something
		}

		/** 초기화한다 */
		private void SubInit() {
			this.SetupAbilityVals();
		}
#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

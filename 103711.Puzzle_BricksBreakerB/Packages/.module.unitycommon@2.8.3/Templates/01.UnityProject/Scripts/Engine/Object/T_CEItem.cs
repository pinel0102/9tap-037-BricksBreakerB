#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 아이템 */
	public partial class CEItem : CEObjComponent {
		/** 매개 변수 */
		public new struct STParams {
			public CEObjComponent.STParams m_stBaseParams;
			public STItemInfo m_stItemInfo;
			public CItemTargetInfo m_oItemTargetInfo;
		}

#region 변수

#endregion // 변수

#region 프로퍼티
		public new STParams Params { get; private set; }
#endregion // 프로퍼티

#region 함수
		/** 어빌리티 값을 설정한다 */
		protected override void DoSetupAbilityVals(bool a_bIsReset = true) {
			base.DoSetupAbilityVals(a_bIsReset);

			// 아이템 정보가 존재 할 경우
			if(this.Params.m_stItemInfo.m_eItemKinds.ExIsValid()) {
				global::Func.SetupAbilityVals(this.Params.m_stItemInfo, this.Params.m_oItemTargetInfo, this.AbilityValDictWrapper.m_oDict02);
			}
		}
#endregion // 함수

#region 클래스 함수
		/** 매개 변수를 생성한다 */
		public static STParams MakeParams(CEngine a_oEngine, STItemInfo a_stItemInfo, CItemTargetInfo a_oItemTargetInfo, CEController a_oController = null, string a_oObjsPoolKey = KCDefine.B_TEXT_EMPTY) {
			return new STParams() {
				m_stBaseParams = CEObjComponent.MakeParams(a_oEngine, a_oController, a_oObjsPoolKey), m_stItemInfo = a_stItemInfo, m_oItemTargetInfo = a_oItemTargetInfo
			};
		}
#endregion // 클래스 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

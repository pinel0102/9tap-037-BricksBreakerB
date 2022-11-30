#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 튜토리얼 팝업 */
public partial class CTutorialPopup : CFocusPopup {
	/** 매개 변수 */
	public new struct STParams {
		public CFocusPopup.STParams m_stBaseParams;
		public ETutorialKinds m_eTutorialKinds;
	}

#region 변수

#endregion // 변수

#region 프로퍼티
	public new STParams Params { get; private set; }
#endregion // 프로퍼티

#region 함수
	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
	}
#endregion // 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

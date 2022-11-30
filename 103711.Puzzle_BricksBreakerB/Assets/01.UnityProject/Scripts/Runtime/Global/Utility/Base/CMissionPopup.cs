using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 미션 팝업 */
public abstract partial class CMissionPopup : CSubPopup {
	/** 매개 변수 */
	public struct STParams {
		public List<STMissionInfo> m_oMissionInfoList;
	}

#region 변수
	/** =====> 객체 <===== */
	[SerializeField] private List<GameObject> m_oMissionUIsList = new List<GameObject>();
#endregion // 변수

#region 프로퍼티
	public STParams Params { get; private set; }
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

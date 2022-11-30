#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using UnityEngine.EventSystems;

/** 포커스 팝업 */
public partial class CFocusPopup : CSubPopup {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		FOCUS_BLIND_IMG,
		[HideInInspector] MAX_VAL
	}

	/** 콜백 */
	public enum ECallback {
		NONE = -1,
		BEGIN,
		MOVE,
		END,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public List<GameObject> m_oContentsUIsList;
		public Dictionary<ECallback, System.Action<CFocusPopup, PointerEventData>> m_oCallbackDict;
	}

#region 변수
	/** =====> UI <===== */
	private Dictionary<EKey, Image> m_oImgDict = new Dictionary<EKey, Image>();
#endregion // 변수
	
#region 프로퍼티
	public STParams Params { get; private set; }
	public override bool IsIgnoreBlindAni => true;
	public override EAniType AniType => EAniType.NONE;
	public override Color BlindColor => KCDefine.U_COLOR_TRANSPARENT;
#endregion // 프로퍼티

#region 함수
	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents() {
		base.SetupContents();

		// 포커스 UI 가 존재 할 경우
		if(this.Params.m_oContentsUIsList.ExIsValid()) {
			for(int i = 0; i < this.Params.m_oContentsUIsList.Count; ++i) {
				this.Params.m_oContentsUIsList[i].SetActive(true);
				this.Params.m_oContentsUIsList[i].ExSetParent(this.ContentsUIs);
			}
		}

		this.UpdateUIsState();
	}
#endregion // 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

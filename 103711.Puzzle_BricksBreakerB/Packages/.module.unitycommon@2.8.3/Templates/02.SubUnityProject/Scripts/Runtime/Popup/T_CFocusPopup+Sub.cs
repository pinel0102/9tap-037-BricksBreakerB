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
#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.SetIgnoreAni(true);

		// 이미지를 설정한다
		CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
			(EKey.FOCUS_BLIND_IMG, $"{EKey.FOCUS_BLIND_IMG}", this.Contents)
		}, m_oImgDict);

#region 추가
		this.SubSetupAwake();
#endregion // 추가
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init();
		this.Params = a_stParams;

		// 터치 전달자를 설정한다
		Func.SetupTouchDispatchers(new List<(GameObject, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>)>() {
			(m_oImgDict.GetValueOrDefault(EKey.FOCUS_BLIND_IMG)?.gameObject, (a_oSender, a_oEventData) => a_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.BEGIN)?.Invoke(this, a_oEventData), (a_oSender, a_oEventData) => a_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.MOVE)?.Invoke(this, a_oEventData), (a_oSender, a_oEventData) => a_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.END)?.Invoke(this, a_oEventData))
		}, false);

#region 추가
		this.SubInit();
#endregion // 추가
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {
		// 이미지를 갱신한다
		m_oImgDict.GetValueOrDefault(EKey.FOCUS_BLIND_IMG)?.ExSetColor<Image>(KCDefine.U_COLOR_POPUP_BLIND, false);

#region 추가
		this.SubUpdateUIsState();
#endregion // 추가
	}
#endregion // 함수
}

/** 서브 포커스 팝업 */
public partial class CFocusPopup : CSubPopup {
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
	/** 팝업을 설정한다 */
	private void SubSetupAwake() {
		// Do Something
	}

	/** 초기화한다 */
	private void SubInit() {
		// Do Something
	}

	/** UI 상태를 갱신한다 */
	private void SubUpdateUIsState() {
		// Do Something
	}
#endregion // 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

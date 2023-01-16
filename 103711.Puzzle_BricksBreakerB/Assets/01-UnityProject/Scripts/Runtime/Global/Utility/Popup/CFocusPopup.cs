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
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.SetIgnoreAni(true);

		// 이미지를 설정한다
		CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
			(EKey.FOCUS_BLIND_IMG, $"{EKey.FOCUS_BLIND_IMG}", this.Contents)
		}, m_oImgDict);

		this.SubAwake();
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init();
		this.Params = a_stParams;

		// 터치 전달자를 설정한다
		Func.SetupTouchDispatchers(new List<(GameObject, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>, System.Action<CTouchDispatcher, PointerEventData>)>() {
			(m_oImgDict.GetValueOrDefault(EKey.FOCUS_BLIND_IMG)?.gameObject, (a_oSender, a_oEventData) => a_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.BEGIN)?.Invoke(this, a_oEventData), (a_oSender, a_oEventData) => a_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.MOVE)?.Invoke(this, a_oEventData), (a_oSender, a_oEventData) => a_stParams.m_oCallbackDict?.GetValueOrDefault(ECallback.END)?.Invoke(this, a_oEventData))
		});

		this.SubInit();
	}

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

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {
		// 이미지를 갱신한다
		m_oImgDict.GetValueOrDefault(EKey.FOCUS_BLIND_IMG)?.ExSetColor<Image>(KCDefine.U_COLOR_POPUP_BLIND, false);

		this.SubUpdateUIsState();
	}
	#endregion // 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

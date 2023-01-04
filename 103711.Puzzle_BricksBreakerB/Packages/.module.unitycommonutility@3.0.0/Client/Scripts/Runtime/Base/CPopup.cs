using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

/** 팝업 */
public abstract partial class CPopup : CComponent {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		IS_SHOW,
		IS_CLOSE,

		SHOW_ANI,
		CLOSE_ANI,
		BLIND_ANI,

		BG_IMG,
		CLOSE_BTN,

		CONTENTS,
		CONTENTS_UIS,
		BLIND_TOUCH_RESPONDER,
		[HideInInspector] MAX_VAL
	}

	/** 콜백 */
	private enum ECallback {
		NONE = -1,
		SHOW,
		CLOSE,
		[HideInInspector] MAX_VAL
	}

	#region 변수
	private Dictionary<EKey, bool> m_oBoolDict = new Dictionary<EKey, bool>();
	private Dictionary<EKey, Tween> m_oAniDict = new Dictionary<EKey, Tween>();
	private Dictionary<ECallback, System.Action<CPopup>> m_oCallbackDict = new Dictionary<ECallback, System.Action<CPopup>>();

	/** =====> UI <===== */
	private Dictionary<EKey, Image> m_oImgDict = new Dictionary<EKey, Image>();
	private Dictionary<EKey, Button> m_oBtnDict = new Dictionary<EKey, Button>();

	/** =====> 객체 <===== */
	private Dictionary<EKey, GameObject> m_oUIsDict = new Dictionary<EKey, GameObject>();
	#endregion // 변수

	#region 프로퍼티
	public virtual bool IsIgnoreBlindAni => false;
	public virtual bool IsIgnoreCloseBtn => false;
	public virtual bool IsIgnoreRebuildLayout => false;

	public virtual float ShowTimeScale => KCDefine.B_VAL_0_REAL;
	public virtual float CloseTimeScale => KCDefine.B_VAL_1_REAL;
	public virtual float ShowAniDelay => KCDefine.U_DELAY_POPUP_SHOW_ANI;

	public virtual EAniType AniType => EAniType.DROPDOWN;
	public virtual Color BlindColor => KCDefine.U_COLOR_POPUP_BLIND;

	protected virtual string ShowSndPath => KCDefine.U_SND_P_G_SFX_POPUP_SHOW;
	protected virtual string CloseSndPath => KCDefine.U_SND_P_G_SFX_POPUP_CLOSE;

	/** =====> UI <===== */
	protected virtual Image BGImg => m_oImgDict.GetValueOrDefault(EKey.BG_IMG);
	protected virtual Image BlindImg => m_oUIsDict.GetValueOrDefault(EKey.BLIND_TOUCH_RESPONDER)?.GetComponentInChildren<Image>();
	protected virtual Button CloseBtn => m_oBtnDict.GetValueOrDefault(EKey.CLOSE_BTN);

	/** =====> 객체 <===== */
	protected GameObject Contents => m_oUIsDict.GetValueOrDefault(EKey.CONTENTS);
	protected GameObject ContentsUIs => m_oUIsDict.GetValueOrDefault(EKey.CONTENTS_UIS);
	protected GameObject BlindTouchResponder => m_oUIsDict.GetValueOrDefault(EKey.BLIND_TOUCH_RESPONDER);
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		CNavStackManager.Inst.AddComponent(this);

		// 객체를 설정한다
		CFunc.SetupObjs(new List<(EKey, string, GameObject)>() {
			(EKey.CONTENTS, $"{EKey.CONTENTS}", this.gameObject),
			(EKey.CONTENTS_UIS, $"{EKey.CONTENTS_UIS}", this.gameObject)
		}, m_oUIsDict);

		// 이미지를 설정한다
		CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
			(EKey.BG_IMG, $"{EKey.BG_IMG}", this.gameObject)
		}, m_oImgDict);

		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
			(EKey.CLOSE_BTN, $"{EKey.CLOSE_BTN}", m_oUIsDict.GetValueOrDefault(EKey.CONTENTS), this.OnTouchCloseBtn)
		}, m_oBtnDict);

		// 터치 응답자를 설정한다
		m_oUIsDict.ExReplaceVal(EKey.BLIND_TOUCH_RESPONDER, this.CreateTouchResponder());
		m_oUIsDict.GetValueOrDefault(EKey.BLIND_TOUCH_RESPONDER)?.transform.SetAsFirstSibling();
	}

	/** 초기화 */
	public virtual void Init() {
		switch(this.AniType) {
			case EAniType.SCALE: {
				m_oUIsDict.GetValueOrDefault(EKey.CONTENTS).transform.localScale = new Vector3(KCDefine.U_MIN_SCALE_POPUP, KCDefine.U_MIN_SCALE_POPUP, KCDefine.U_MIN_SCALE_POPUP);
				m_oUIsDict.GetValueOrDefault(EKey.CONTENTS).transform.localPosition = Vector3.zero;

				break;
			}
			case EAniType.DROPDOWN:
			case EAniType.SLIDE_LEFT:
			case EAniType.SLIDE_RIGHT: {
				// 낙하 애니메이션 일 경우
				if(this.AniType == EAniType.DROPDOWN) {
					m_oUIsDict.GetValueOrDefault(EKey.CONTENTS).transform.localPosition = new Vector3(KCDefine.B_VAL_0_REAL, CSceneManager.CanvasSize.y, KCDefine.B_VAL_0_REAL);
				} else {
					m_oUIsDict.GetValueOrDefault(EKey.CONTENTS).transform.localPosition = new Vector3((this.AniType == EAniType.SLIDE_LEFT) ? CSceneManager.CanvasSize.x : -CSceneManager.CanvasSize.x, KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_0_REAL);
				}

				m_oUIsDict.GetValueOrDefault(EKey.CONTENTS).transform.localScale = Vector3.one;
				break;
			}
		}

		this.BlindImg.color = this.BlindColor.ExGetAlphaColor(KCDefine.B_VAL_0_REAL);
	}

	/** 제거 되었을 경우 */
	public override void OnDestroy() {
		base.OnDestroy();

		try {
			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				this.ResetAni();
			}
		} catch(System.Exception oException) {
			CFunc.ShowLogWarning($"CPopup.OnDestroy Exception: {oException.Message}");
		}
	}

	/** 애니메이션을 리셋한다 */
	public virtual void ResetAni() {
		foreach(var stKeyVal in m_oAniDict) {
			stKeyVal.Value?.Kill();
		}
	}

	/** 블라인드 색상을 변경한다 */
	public void SetBlindColor(Color a_stColor, bool a_bIsAni = true, float a_fDuration = KCDefine.U_DURATION_POPUP_ANI) {
		m_oAniDict.GetValueOrDefault(EKey.BLIND_ANI)?.Kill();

		// 애니메이션 모드 일 경우
		if(a_bIsAni && !this.IsIgnoreAni) {
#if DOTWEEN_MODULE_ENABLE || EXTRA_SCRIPT_MODULE_ENABLE
			m_oAniDict.ExReplaceVal(EKey.BLIND_ANI, this.BlindImg.DOColor(a_stColor, a_fDuration).SetUpdate(true));
#endif // #if DOTWEEN_MODULE_ENABLE || EXTRA_SCRIPT_MODULE_ENABLE
		} else {
			this.BlindImg.color = a_stColor;
		}
	}

	/** 내비게이션 스택 이벤트를 수신했을 경우 */
	public override void OnReceiveNavStackEvent(ENavStackEvent a_eEvent) {
		base.OnReceiveNavStackEvent(a_eEvent);

		switch(a_eEvent) {
			case ENavStackEvent.TOP: {
				Time.timeScale = this.ShowTimeScale;
				break;
			}
			case ENavStackEvent.REMOVE: {
				// 이벤트 처리가 가능 할 경우
				if(!this.IsIgnoreNavStackEvent) {
					this.Close();
				}

				break;
			}
		}
	}

	/** 팝업을 출력한다 */
	public virtual void Show(System.Action<CPopup> a_oShowCallback, System.Action<CPopup> a_oCloseCallback) {
		// 객체가 존재 할 경우
		if(!this.IsDestroy && (!m_oBoolDict.GetValueOrDefault(EKey.IS_SHOW) && !m_oBoolDict.GetValueOrDefault(EKey.IS_CLOSE))) {
			m_oBoolDict.ExReplaceVal(EKey.IS_SHOW, true);
			CSndManager.Inst.PlayFXSnds(this.ShowSndPath);

			m_oCallbackDict.ExReplaceVal(ECallback.SHOW, a_oShowCallback);
			m_oCallbackDict.ExReplaceVal(ECallback.CLOSE, a_oCloseCallback);

			this.ExLateCallFunc(this.DoShow, Mathf.Max(this.ShowAniDelay, KCDefine.B_DELTA_T_INTERMEDIATE), true);
		}
	}

	/** 팝업을 닫는다 */
	public virtual void Close() {
		// 출력 상태 일 경우
		if(!this.IsDestroy && (m_oBoolDict.GetValueOrDefault(EKey.IS_SHOW) && !m_oBoolDict.GetValueOrDefault(EKey.IS_CLOSE))) {
			m_oBoolDict.ExReplaceVal(EKey.IS_CLOSE, true);
			CSndManager.Inst.PlayFXSnds(this.CloseSndPath);

			this.StartCloseAni();

			// 내비게이션 스택 콜백이 유효 할 경우
			if(this.NavStackCallback != null) {
				this.SetNavStackCallback(null);
				CNavStackManager.Inst.RemoveComponent(this);
			}
		}
	}

	/** 팝업 컨텐츠를 설정한다 */
	protected virtual void SetupContents() {
		// 레이아웃 보정 모드 일 경우
		if(!this.IsIgnoreRebuildLayout) {
			this.RebuildLayout(m_oUIsDict.GetValueOrDefault(EKey.CONTENTS));
		}

		this.UpdateUIsState();
	}

	/** 팝업이 출력 되었을 경우 */
	protected virtual void OnShow() {
		// Do Something
	}

	/** 팝업이 닫혔을 경우 */
	protected virtual void OnClose() {
		// Do Something
	}

	/** 출력 애니메이션이 완료 되었을 경우 */
	protected virtual void OnCompleteShowAni() {
		this.OnShow();
		m_oCallbackDict.GetValueOrDefault(ECallback.SHOW)?.Invoke(this);

		m_oUIsDict.GetValueOrDefault(EKey.CONTENTS).transform.localPosition = Vector3.zero;
	}

	/** 닫기 애니메이션이 완료 되었을 경우 */
	protected virtual void OnCompleteCloseAni() {
		this.OnClose();
		m_oCallbackDict.GetValueOrDefault(ECallback.CLOSE)?.Invoke(this);

		Destroy(this.gameObject);
	}

	/** 닫기 버튼을 눌렀을 경우 */
	protected virtual void OnTouchCloseBtn() {
		// 닫기 버튼 처리가 가능 할 경우
		if(!this.IsIgnoreCloseBtn) {
			this.Close();
		}
	}

	/** 레이아웃을 재배치한다 */
	protected void RebuildLayout(GameObject a_oObj) {
		a_oObj.ExEnumerateComponents<RectTransform>((a_oTrans) => { LayoutRebuilder.ForceRebuildLayoutImmediate(a_oTrans); return true; });
	}

	/** 출력 애니메이션을 생성한다 */
	protected virtual Tween MakeShowAni() {
		switch(this.AniType) {
			case EAniType.SCALE: return m_oUIsDict.GetValueOrDefault(EKey.CONTENTS).transform.DOScale(KCDefine.U_SCALE_POPUP, KCDefine.U_DURATION_POPUP_ANI).SetEase(Ease.OutBack);
			case EAniType.DROPDOWN: return m_oUIsDict.GetValueOrDefault(EKey.CONTENTS).transform.DOLocalMoveY(KCDefine.B_VAL_0_REAL, KCDefine.U_DURATION_POPUP_ANI).SetEase(Ease.OutBack);
			case EAniType.SLIDE_LEFT:
			case EAniType.SLIDE_RIGHT: return m_oUIsDict.GetValueOrDefault(EKey.CONTENTS).transform.DOLocalMoveX(KCDefine.B_VAL_0_REAL, KCDefine.U_DURATION_POPUP_ANI).SetEase(Ease.OutBack);
		}

		return null;
	}

	/** 닫기 애니메이션을 생성한다 */
	protected virtual Tween MakeCloseAni() {
		switch(this.AniType) {
			case EAniType.SCALE: return m_oUIsDict.GetValueOrDefault(EKey.CONTENTS).transform.DOScale(KCDefine.U_MIN_SCALE_POPUP, KCDefine.U_DURATION_POPUP_ANI).SetEase(Ease.InBack);
			case EAniType.DROPDOWN: return m_oUIsDict.GetValueOrDefault(EKey.CONTENTS).transform.DOLocalMoveY(CSceneManager.CanvasSize.y, KCDefine.U_DURATION_POPUP_ANI).SetEase(Ease.InBack);
			case EAniType.SLIDE_LEFT:
			case EAniType.SLIDE_RIGHT: return m_oUIsDict.GetValueOrDefault(EKey.CONTENTS).transform.DOLocalMoveX((this.AniType == EAniType.SLIDE_LEFT) ? -CSceneManager.CanvasSize.x : CSceneManager.CanvasSize.x, KCDefine.U_DURATION_POPUP_ANI).SetEase(Ease.InBack);
		}

		return null;
	}

	/** 터치 응답자를 생성한다 */
	protected virtual GameObject CreateTouchResponder() {
		string oName = string.Format(KCDefine.U_OBJ_N_FMT_POPUP_TOUCH_RESPONDER, this.gameObject.name);
		return CFactory.CreateTouchResponder(oName, KCDefine.U_OBJ_P_TOUCH_RESPONDER, this.gameObject, CSceneManager.CanvasSize, Vector3.zero, KCDefine.U_COLOR_TRANSPARENT);
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {
		// Do Something
	}

	/** 출력 애니메이션을 시작한다 */
	private void StartShowAni() {
		this.ResetAni();
		Time.timeScale = this.ShowTimeScale;

		// 애니메이션 모드 일 경우
		if(!this.IsIgnoreAni && this.AniType != EAniType.NONE) {
			m_oAniDict.ExReplaceVal(EKey.SHOW_ANI, CFactory.MakeSequence(this.MakeShowAni(), (a_oSender) => this.OnCompleteShowAni(), KCDefine.B_VAL_0_REAL, false, true));
		} else {
			this.ExLateCallFunc((a_oSender) => this.OnCompleteShowAni());
		}
	}

	/** 닫기 애니메이션을 시작한다 */
	private void StartCloseAni() {
		this.ResetAni();
		Time.timeScale = this.CloseTimeScale;

		// 애니메이션 모드 일 경우
		if(!this.IsIgnoreAni && this.AniType != EAniType.NONE) {
			m_oAniDict.ExReplaceVal(EKey.CLOSE_ANI, CFactory.MakeSequence(this.MakeCloseAni(), (a_oSender) => this.OnCompleteCloseAni(), KCDefine.B_VAL_0_REAL, false, true));
		} else {
			this.ExLateCallFunc((a_oSender) => this.OnCompleteCloseAni());
		}
	}

	/** 팝업을 출력한다 */
	private void DoShow(MonoBehaviour a_oSender) {
		// 출력 가능 할 경우
		if(!this.IsDestroy && !m_oBoolDict.GetValueOrDefault(EKey.IS_CLOSE)) {
			this.SetupContents();
			this.StartShowAni();
			this.SetBlindColor(this.BlindColor, !this.IsIgnoreBlindAni);
		}
	}
	#endregion // 함수

	#region 제네릭 클래스 함수
	/** 팝업을 생성한다 */
	public static T Create<T>(string a_oName, string a_oObjPath, GameObject a_oParent) where T : CPopup {
		CAccess.Assert(a_oName.ExIsValid() && a_oObjPath.ExIsValid());
		return CPopup.Create<T>(a_oName, a_oObjPath, a_oParent, KCDefine.B_POS_POPUP);
	}

	/** 팝업을 생성한다 */
	public static T Create<T>(string a_oName, string a_oObjPath, GameObject a_oParent, Vector3 a_stPos) where T : CPopup {
		CAccess.Assert(a_oName.ExIsValid() && a_oObjPath.ExIsValid());
		return CPopup.Create<T>(a_oName, CResManager.Inst.GetRes<GameObject>(a_oObjPath), a_oParent, a_stPos);
	}

	/** 팝업을 생성한다 */
	public static T Create<T>(string a_oName, GameObject a_oOrigin, GameObject a_oParent) where T : CPopup {
		CAccess.Assert(a_oOrigin != null && a_oName.ExIsValid());
		return CPopup.Create<T>(a_oName, a_oOrigin, a_oParent, KCDefine.B_POS_POPUP);
	}

	/** 팝업을 생성한다 */
	public static T Create<T>(string a_oName, GameObject a_oOrigin, GameObject a_oParent, Vector3 a_stPos) where T : CPopup {
		CAccess.Assert(a_oOrigin != null && a_oName.ExIsValid());
		return CFactory.CreateCloneObj<T>(a_oName, a_oOrigin, a_oParent, a_stPos);
	}
	#endregion // 제네릭 클래스 함수
}

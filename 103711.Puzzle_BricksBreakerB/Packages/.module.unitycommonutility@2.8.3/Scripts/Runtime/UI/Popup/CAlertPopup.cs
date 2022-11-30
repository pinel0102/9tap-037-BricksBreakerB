using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

/** 경고 팝업 */
public partial class CAlertPopup : CPopup {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		TITLE_TEXT,
		MSG_TEXT,
		OK_BTN,
		CANCEL_BTN,
		BM_UIS_LAYOUT_GROUP,
		[HideInInspector] MAX_VAL
	}

	/** 콜백 */
	public enum ECallback {
		NONE = -1,
		OK_CANCEL,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public string m_oTitle;
		public string m_oMsg;
		public string m_oOKBtnText;
		public string m_oCancelBtnText;

		public Dictionary<ECallback, System.Action<CAlertPopup, bool>> m_oCallbackDict;
	}

	#region 변수
	/** =====> UI <===== */
	private Dictionary<EKey, TMP_Text> m_oTextDict = new Dictionary<EKey, TMP_Text>();
	private Dictionary<EKey, Button> m_oBtnDict = new Dictionary<EKey, Button>();
	private Dictionary<EKey, LayoutGroup> m_oLayoutGroupDict = new Dictionary<EKey, LayoutGroup>();
	#endregion // 변수

	#region 프로퍼티
	public STParams Params { get; private set; }
	public TMP_Text TitleText => m_oTextDict.GetValueOrDefault(EKey.TITLE_TEXT);
	public TMP_Text MsgText => m_oTextDict.GetValueOrDefault(EKey.MSG_TEXT);

	public Button OKBtn => m_oBtnDict.GetValueOrDefault(EKey.OK_BTN);
	public Button CancelBtn => m_oBtnDict.GetValueOrDefault(EKey.CANCEL_BTN);
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 텍스트를 설정한다
		CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
			(EKey.TITLE_TEXT, $"{EKey.TITLE_TEXT}", this.Contents),
			(EKey.MSG_TEXT, $"{EKey.MSG_TEXT}", this.Contents)
		}, m_oTextDict);

		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
			(EKey.OK_BTN, $"{EKey.OK_BTN}", this.Contents, this.OnTouchOKBtn),
			(EKey.CANCEL_BTN, $"{EKey.CANCEL_BTN}", this.Contents, this.OnTouchCancelBtn)
		}, m_oBtnDict);

		// 레이아웃을 설정한다
		CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
			(EKey.BM_UIS_LAYOUT_GROUP, KCDefine.U_OBJ_N_BOTTOM_MENU_UIS, this.Contents)
		}, m_oLayoutGroupDict);
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init();
		this.Params = a_stParams;
	}

	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents() {
		base.SetupContents();

		float fMsgTextHeight = m_oTextDict.GetValueOrDefault(EKey.MSG_TEXT).rectTransform.sizeDelta.y;
		m_oBtnDict.GetValueOrDefault(EKey.CANCEL_BTN).gameObject.SetActive(this.Params.m_oCancelBtnText.ExIsValid());

		// 컨텐츠 크기를 설정한다 {
		float fContentsWidth = m_oTextDict.GetValueOrDefault(EKey.TITLE_TEXT).rectTransform.sizeDelta.x;
		fContentsWidth = Mathf.Max(fContentsWidth, m_oTextDict.GetValueOrDefault(EKey.MSG_TEXT).rectTransform.sizeDelta.x);
		fContentsWidth = Mathf.Max(fContentsWidth, (m_oLayoutGroupDict.GetValueOrDefault(EKey.BM_UIS_LAYOUT_GROUP).transform as RectTransform).sizeDelta.x);

		float fContentsHeight = Mathf.Abs((this.ContentsUIs.transform as RectTransform).offsetMin.y);
		fContentsHeight += Mathf.Abs((this.ContentsUIs.transform as RectTransform).offsetMax.y);
		fContentsHeight += Mathf.Abs(m_oTextDict.GetValueOrDefault(EKey.TITLE_TEXT).transform.localPosition.y);
		fContentsHeight += Mathf.Abs(m_oLayoutGroupDict.GetValueOrDefault(EKey.BM_UIS_LAYOUT_GROUP).transform.localPosition.y);
		// 컨텐츠 크기를 설정한다 }

		this.UpdateUIsState();
		this.ExLateCallFunc((a_oSender) => this.DoSetupContents(fMsgTextHeight, new Vector3(fContentsWidth, fContentsHeight, KCDefine.B_VAL_0_REAL)));
	}

	/** 확인 버튼을 눌렀을 경우 */
	protected virtual void OnTouchOKBtn() {
		this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.OK_CANCEL)?.Invoke(this, true);
		this.OnTouchCloseBtn();
	}

	/** 취소 버튼을 눌렀을 경우 */
	protected virtual void OnTouchCancelBtn() {
		this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.OK_CANCEL)?.Invoke(this, false);
		this.OnTouchCloseBtn();
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {
		// 텍스트를 갱신한다 {
		m_oTextDict.GetValueOrDefault(EKey.TITLE_TEXT).ExSetText(this.Params.m_oTitle, CLocalizeInfoTable.Inst.GetFontSetInfo(EFontSet._1));
		m_oTextDict.GetValueOrDefault(EKey.MSG_TEXT).ExSetText(this.Params.m_oMsg, CLocalizeInfoTable.Inst.GetFontSetInfo(EFontSet._1));

		m_oBtnDict.GetValueOrDefault(EKey.OK_BTN).GetComponentInChildren<TMP_Text>().ExSetText(this.Params.m_oOKBtnText, CLocalizeInfoTable.Inst.GetFontSetInfo(EFontSet._1));
		m_oBtnDict.GetValueOrDefault(EKey.CANCEL_BTN).GetComponentInChildren<TMP_Text>().ExSetText(this.Params.m_oCancelBtnText.ExIsValid() ? this.Params.m_oCancelBtnText : string.Empty, CLocalizeInfoTable.Inst.GetFontSetInfo(EFontSet._1));

		LayoutRebuilder.ForceRebuildLayoutImmediate(m_oTextDict.GetValueOrDefault(EKey.TITLE_TEXT).rectTransform);
		LayoutRebuilder.ForceRebuildLayoutImmediate(m_oTextDict.GetValueOrDefault(EKey.MSG_TEXT).rectTransform);
		// 텍스트를 갱신한다 }
	}

	/** 팝업 컨텐츠를 설정한다 */
	private void DoSetupContents(float a_fMsgTextHeight, Vector3 a_stContentsSize) {
		float fContentsOffsetV = m_oTextDict.GetValueOrDefault(EKey.MSG_TEXT).rectTransform.sizeDelta.y - a_fMsgTextHeight;
		float fContentsOffsetH = this.BGImg.rectTransform.sizeDelta.x - a_stContentsSize.x;

		// 이미지를 설정한다 {
		var stContentsSize = new Vector3(m_oTextDict.GetValueOrDefault(EKey.TITLE_TEXT).rectTransform.sizeDelta.x, m_oTextDict.GetValueOrDefault(EKey.TITLE_TEXT).rectTransform.sizeDelta.y, KCDefine.B_VAL_0_REAL);
		stContentsSize.x = Mathf.Max(stContentsSize.x, m_oTextDict.GetValueOrDefault(EKey.MSG_TEXT).rectTransform.sizeDelta.x);
		stContentsSize.x = Mathf.Max(stContentsSize.x, (m_oLayoutGroupDict.GetValueOrDefault(EKey.BM_UIS_LAYOUT_GROUP).transform as RectTransform).sizeDelta.x);

		this.BGImg.gameObject.ExSetSizeDeltaX(Mathf.Max(stContentsSize.x + fContentsOffsetH, KCDefine.U_MIN_SIZE_ALERT_POPUP.x));
		this.BGImg.gameObject.ExSetSizeDeltaY(Mathf.Max(a_stContentsSize.y + fContentsOffsetV, KCDefine.U_MIN_SIZE_ALERT_POPUP.y));
		// 이미지를 설정한다 }
	}
	#endregion // 함수

	#region 클래스 함수
	/** 매개 변수를 생성한다 */
	public static STParams MakeParams(string a_oTitle, string a_oMsg, string a_oOKBtnText, string a_oCancelBtnText, Dictionary<CAlertPopup.ECallback, System.Action<CAlertPopup, bool>> a_oCallbackDict = null) {
		return new STParams() {
			m_oTitle = a_oTitle,
			m_oMsg = a_oMsg,
			m_oOKBtnText = a_oOKBtnText,
			m_oCancelBtnText = a_oCancelBtnText,
			m_oCallbackDict = a_oCallbackDict ?? new Dictionary<ECallback, System.Action<CAlertPopup, bool>>()
		};
	}
	#endregion // 클래스 함수

	#region 제네릭 클래스 함수
	/** 경고 팝업을 생성한다 */
	public static T Create<T>(string a_oName, string a_oObjPath, GameObject a_oParent, STParams a_stParams) where T : CAlertPopup {
		CAccess.Assert(a_oName.ExIsValid() && a_oObjPath.ExIsValid());
		return CAlertPopup.Create<T>(a_oName, a_oObjPath, a_oParent, a_stParams, KCDefine.B_POS_POPUP);
	}

	/** 경고 팝업을 생성한다 */
	public static T Create<T>(string a_oName, string a_oObjPath, GameObject a_oParent, STParams a_stParams, Vector3 a_stPos) where T : CAlertPopup {
		CAccess.Assert(a_oName.ExIsValid() && a_oObjPath.ExIsValid());
		return CAlertPopup.Create<T>(a_oName, CResManager.Inst.GetRes<GameObject>(a_oObjPath), a_oParent, a_stParams, a_stPos);
	}

	/** 경고 팝업을 생성한다 */
	public static T Create<T>(string a_oName, GameObject a_oOrigin, GameObject a_oParent, STParams a_stParams) where T : CAlertPopup {
		CAccess.Assert(a_oOrigin != null && a_oName.ExIsValid());
		return CAlertPopup.Create<T>(a_oName, a_oOrigin, a_oParent, a_stParams, KCDefine.B_POS_POPUP);
	}

	/** 경고 팝업을 생성한다 */
	public static T Create<T>(string a_oName, GameObject a_oOrigin, GameObject a_oParent, STParams a_stParams, Vector3 a_stPos) where T : CAlertPopup {
		CAccess.Assert(a_oOrigin != null && a_oName.ExIsValid());

		var oAlertPopup = CPopup.Create<T>(a_oName, a_oOrigin, a_oParent, a_stPos);
		oAlertPopup?.Init(a_stParams);

		return oAlertPopup;
	}
	#endregion // 제네릭 클래스 함수
}

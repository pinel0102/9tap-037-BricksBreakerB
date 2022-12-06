using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR || UNITY_STANDALONE
using EnhancedUI.EnhancedScroller;

/** 에디터 스크롤러 셀 뷰 */
public partial class CEditorScrollerCellView : CScrollerCellView {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		NAME_TEXT,
		MOVE_BTN,
		REMOVE_BTN,
		[HideInInspector] MAX_VAL
	}

	/** 콜백 */
	public new enum ECallback {
		NONE = -1,
		COPY,
		MOVE,
		REMOVE,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public new struct STParams {
		public CScrollerCellView.STParams m_stBaseParams;
		public Dictionary<ECallback, System.Action<CEditorScrollerCellView, ulong>> m_oCallbackDict;
	}

	#region 변수
	/** =====> UI <===== */
	private Dictionary<EKey, Text> m_oTextDict = new Dictionary<EKey, Text>();
	private Dictionary<EKey, Button> m_oBtnDict = new Dictionary<EKey, Button>();
	#endregion // 변수

	#region 프로퍼티
	public new STParams Params { get; private set; }
	public Text NameText => m_oTextDict.GetValueOrDefault(EKey.NAME_TEXT);
	public Button MoveBtn => m_oBtnDict.GetValueOrDefault(EKey.MOVE_BTN);
	public Button RemoveBtn => m_oBtnDict.GetValueOrDefault(EKey.REMOVE_BTN);
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 텍스트를 설정한다
		CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
			(EKey.NAME_TEXT, $"{EKey.NAME_TEXT}", this.gameObject)
		}, m_oTextDict);

		// 버튼을 설정한다 {
		CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
			(EKey.MOVE_BTN, $"{EKey.MOVE_BTN}", this.gameObject, this.OnTouchMoveBtn),
			(EKey.REMOVE_BTN, $"{EKey.REMOVE_BTN}", this.gameObject, this.OnTouchRemoveBtn)
		}, m_oBtnDict);

		CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
			(KCDefine.U_OBJ_N_COPY_BTN, this.gameObject, this.OnTouchCopyBtn)
		}, false);
		// 버튼을 설정한다 }
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init(a_stParams.m_stBaseParams);
		this.Params = a_stParams;

		this.UpdateUIsState();
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {
		// Do Something
	}

	/** 복사 버튼을 눌렀을 경우 */
	private void OnTouchCopyBtn() {
		this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.COPY)?.Invoke(this, base.Params.m_nID);
	}

	/** 이동 버튼을 눌렀을 경우 */
	private void OnTouchMoveBtn() {
		this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.MOVE)?.Invoke(this, base.Params.m_nID);
	}

	/** 제거 버튼을 눌렀을 경우 */
	private void OnTouchRemoveBtn() {
		this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.REMOVE)?.Invoke(this, base.Params.m_nID);
	}
	#endregion // 함수

	#region 클래스 함수
	/** 매개 변수를 생성한다 */
	public static STParams MakeParams(ulong a_nID, EnhancedScroller a_oScroller, Dictionary<CScrollerCellView.ECallback, System.Action<CScrollerCellView, ulong>> a_oCallbackDict01 = null, Dictionary<ECallback, System.Action<CEditorScrollerCellView, ulong>> a_oCallbackDict02 = null) {
		return new STParams() {
			m_stBaseParams = CScrollerCellView.MakeParams(a_nID, a_oScroller, a_oCallbackDict01 ?? new Dictionary<CScrollerCellView.ECallback, System.Action<CScrollerCellView, ulong>>()),
			m_oCallbackDict = a_oCallbackDict02 ?? new Dictionary<ECallback, System.Action<CEditorScrollerCellView, ulong>>()
		};
	}
	#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR || UNITY_STANDALONE

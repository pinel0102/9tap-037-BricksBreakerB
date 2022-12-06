using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR || UNITY_STANDALONE
/** 에디터 입력 팝업 */
public partial class CEditorInputPopup : CPopup {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		INPUT,
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
		public Dictionary<ECallback, System.Action<CEditorInputPopup, string, bool>> m_oCallbackDict;
	}

	#region 변수
	/** =====> UI <===== */
	private Dictionary<EKey, InputField> m_oInputDict = new Dictionary<EKey, InputField>();
	#endregion // 변수

	#region 프로퍼티
	public STParams Params { get; private set; }
	public InputField Input => m_oInputDict.GetValueOrDefault(EKey.INPUT);
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.SetIgnoreAni(true);

		// 입력 필드를 설정한다
		CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
			(EKey.INPUT, $"{EKey.INPUT}", this.Contents)
		}, m_oInputDict);

		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
			(KCDefine.U_OBJ_N_OK_BTN, this.Contents, this.OnTouchOKBtn),
			(KCDefine.U_OBJ_N_CANCEL_BTN, this.Contents, this.OnTouchCancelBtn)
		}, false);
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init();
		this.Params = a_stParams;
	}

	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {
		// Do Something
	}

	/** 확인 버튼을 눌렀을 경우 */
	private void OnTouchOKBtn() {
		this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.OK_CANCEL)?.Invoke(this, (m_oInputDict.GetValueOrDefault(EKey.INPUT) != null) ? m_oInputDict.GetValueOrDefault(EKey.INPUT).text : string.Empty, true);
		this.OnTouchCloseBtn();
	}

	/** 취소 버튼을 눌렀을 경우 */
	private void OnTouchCancelBtn() {
		this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.OK_CANCEL)?.Invoke(this, string.Empty, false);
		this.OnTouchCloseBtn();
	}
	#endregion // 함수

	#region 클래스 함수
	/** 매개 변수를 생성한다 */
	public static STParams MakeParams(Dictionary<ECallback, System.Action<CEditorInputPopup, string, bool>> a_oCallbackDict = null) {
		return new STParams() {
			m_oCallbackDict = a_oCallbackDict ?? new Dictionary<ECallback, System.Action<CEditorInputPopup, string, bool>>()
		};
	}
	#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR || UNITY_STANDALONE

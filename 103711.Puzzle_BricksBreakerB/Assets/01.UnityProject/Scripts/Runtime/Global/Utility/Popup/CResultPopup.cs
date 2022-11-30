using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using TMPro;

/** 결과 팝업 */
public partial class CResultPopup : CSubPopup {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		RECORD_TEXT,
		BEST_RECORD_TEXT,
		CLEAR_UIS,
		CLEAR_FAIL_UIS,
		[HideInInspector] MAX_VAL
	}

	/** 콜백 */
	public enum ECallback {
		NONE = -1,
		NEXT,
		RETRY,
		LEAVE,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public STRecordInfo m_stRecordInfo;
		public Dictionary<ECallback, System.Action<CResultPopup>> m_oCallbackDict;
	}

#region 변수
	/** =====> UI <===== */
	private Dictionary<EKey, TMP_Text> m_oTextDict = new Dictionary<EKey, TMP_Text>();

	/** =====> 객체 <===== */
	private Dictionary<EKey, GameObject> m_oUIsDict = new Dictionary<EKey, GameObject>();
#endregion // 변수

#region 프로퍼티
	public STParams Params { get; private set; }
	public override bool IsIgnoreCloseBtn => true;
#endregion // 프로퍼티

#region 함수
	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
	}

	/** 닫기 버튼을 눌렀을 경우 */
	protected override void OnTouchCloseBtn() {
		base.OnTouchCloseBtn();
		this.OnTouchLeaveBtn();
	}
	
	/** 다음 버튼을 눌렀을 경우 */
	private void OnTouchNextBtn() {
		this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.NEXT)?.Invoke(this);
	}

	/** 재시도 버튼을 눌렀을 경우 */
	private void OnTouchRetryBtn() {
		this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.RETRY)?.Invoke(this);
	}

	/** 나가기 버튼을 눌렀을 경우 */
	private void OnTouchLeaveBtn() {
		this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.LEAVE)?.Invoke(this);
	}
#endregion // 함수

#region 클래스 함수
	/** 매개 변수를 생성한다 */
	public static STParams MakeParams(STRecordInfo a_stRecordInfo, Dictionary<ECallback, System.Action<CResultPopup>> a_oCallbackDict = null) {
		return new STParams() {
			m_stRecordInfo = a_stRecordInfo, m_oCallbackDict = a_oCallbackDict ?? new Dictionary<ECallback, System.Action<CResultPopup>>()
		};
	}
#endregion // 클래스 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

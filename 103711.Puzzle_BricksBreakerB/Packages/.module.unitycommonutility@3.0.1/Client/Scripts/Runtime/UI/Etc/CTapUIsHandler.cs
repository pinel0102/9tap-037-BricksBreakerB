using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** 탭 UI 처리자 */
public partial class CTapUIsHandler : CComponent {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		SEL_TAP_BTN_IDX,
		[HideInInspector] MAX_VAL
	}

	/** 콜백 */
	public enum ECallback {
		NONE = -1,
		TAP,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public Dictionary<ECallback, System.Action<CTapUIsHandler, int>> m_oCallbackDict;
	}

	#region 변수
	private Dictionary<EKey, int> m_oIntDict = new Dictionary<EKey, int>() {
		[EKey.SEL_TAP_BTN_IDX] = KCDefine.B_VAL_0_INT
	};

	/** =====> UI <===== */
	[SerializeField] private List<Button> m_oTapBtnList = new List<Button>();

	/** =====> 객체 <===== */
	[SerializeField] private List<GameObject> m_oContentsUIsList = new List<GameObject>();
	#endregion // 변수

	#region 프로퍼티
	public STParams Params { get; private set; }
	public int SelTapBtnIdx => m_oIntDict[EKey.SEL_TAP_BTN_IDX];

	/** =====> UI <===== */
	public List<Button> TapBtnList => m_oTapBtnList;

	/** =====> 객체 <===== */
	public List<GameObject> ContentsUIsList => m_oContentsUIsList;
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 버튼을 설정한다
		for(int i = 0; i < m_oTapBtnList.Count; ++i) {
			int nIdx = i;
			m_oTapBtnList[i].onClick.AddListener(() => this.OnTouchTapBtn(nIdx));
		}
	}

	/** 초기화 */
	public override void Start() {
		base.Start();
		this.UpdateUIsState();
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		this.Params = a_stParams;
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {
		for(int i = 0; i < m_oTapBtnList.Count; ++i) {
			m_oContentsUIsList[i].SetActive(i == m_oIntDict[EKey.SEL_TAP_BTN_IDX]);
		}
	}

	/** 탭 버튼을 눌렀을 경우 */
	protected virtual void OnTouchTapBtn(int a_nIdx) {
		m_oIntDict[EKey.SEL_TAP_BTN_IDX] = a_nIdx;
		this.Params.m_oCallbackDict.GetValueOrDefault(ECallback.TAP)?.Invoke(this, a_nIdx);

		this.UpdateUIsState();
	}
	#endregion // 함수

	#region 클래스 함수
	/** 매개 변수를 생성한다 */
	public static STParams MakeParams(Dictionary<ECallback, System.Action<CTapUIsHandler, int>> a_oCallbackDict = null) {
		return new STParams() {
			m_oCallbackDict = a_oCallbackDict ?? new Dictionary<ECallback, System.Action<CTapUIsHandler, int>>()
		};
	}
	#endregion // 클래스 함수
}

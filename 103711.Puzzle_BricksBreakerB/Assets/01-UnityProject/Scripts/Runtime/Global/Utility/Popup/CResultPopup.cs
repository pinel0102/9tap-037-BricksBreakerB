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
        public NSEngine.CEngine Engine;
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

    public TMP_Text[] levelText;
    public GameObject[] starObject;
    public SpriteMask previewMask;
    public RectTransform previewArea;
    
    private const string formatLevel = "Level {0}";
    private const string U_OBJ_N_LEAVE_BTN_2 = "LEAVE_BTN_2";    
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.SetIgnoreNavStackEvent(true);

		// 객체를 설정한다
		CFunc.SetupObjs(new List<(EKey, string, GameObject)>() {
			(EKey.CLEAR_UIS, $"{EKey.CLEAR_UIS}", this.Contents),
			(EKey.CLEAR_FAIL_UIS, $"{EKey.CLEAR_FAIL_UIS}", this.Contents)
		}, m_oUIsDict);

		// 텍스트를 설정한다
		CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
			(EKey.RECORD_TEXT, $"{EKey.RECORD_TEXT}", this.Contents),
			(EKey.BEST_RECORD_TEXT, $"{EKey.BEST_RECORD_TEXT}", this.Contents)
		}, m_oTextDict);

		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
			(KCDefine.U_OBJ_N_NEXT_BTN, this.Contents, this.OnTouchNextBtn),
			(KCDefine.U_OBJ_N_RETRY_BTN, this.Contents, this.OnTouchRetryBtn),
            (KCDefine.U_OBJ_N_LEAVE_BTN, this.Contents, this.OnTouchLeaveBtn),
            (U_OBJ_N_LEAVE_BTN_2, this.Contents, this.OnTouchLeaveBtn)
		});

		this.SubAwake();
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init();
		this.Params = a_stParams;

        this.SubInit();
	}

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

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {

        Debug.Log(CodeManager.GetMethodName());

        Params.Engine.SetupPreview(previewArea, previewMask);

        levelText[0].text = levelText[1].text = string.Format(formatLevel, Params.Engine.currentLevel);
        
		var oClearLevelInfo = Access.GetLevelClearInfo(CGameInfoStorage.Inst.PlayCharacterID, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID01, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03, false);

		// 객체를 갱신한다
		m_oUIsDict.GetValueOrDefault(EKey.CLEAR_UIS)?.SetActive(this.Params.m_stRecordInfo.m_bIsSuccess);
		m_oUIsDict.GetValueOrDefault(EKey.CLEAR_FAIL_UIS)?.SetActive(!this.Params.m_stRecordInfo.m_bIsSuccess);

		// 텍스트를 갱신한다
		m_oTextDict.GetValueOrDefault(EKey.RECORD_TEXT)?.ExSetText($"{this.Params.m_stRecordInfo.m_nIntRecord}", EFontSet._1, false);
		m_oTextDict.GetValueOrDefault(EKey.BEST_RECORD_TEXT)?.ExSetText((oClearLevelInfo != null) ? $"{oClearLevelInfo.m_stBestRecordInfo.m_nIntRecord}" : string.Empty, EFontSet._1, false);

        for (int i=0; i < starObject.Length; i++)
        {
            starObject[i].SetActive(this.Params.m_stRecordInfo.m_bIsSuccess && this.Params.m_stRecordInfo.m_starCount > i);
        }

        if (this.Params.m_stRecordInfo.m_bIsSuccess)
        {
            GlobalDefine.PlaySoundFX(ESoundSet.SOUND_LEVEL_CLEAR);
        }
        else
        {
            GlobalDefine.PlaySoundFX(ESoundSet.SOUND_LEVEL_FAIL);
        }
        
		this.SubUpdateUIsState();
	}

	/** 다음 버튼을 눌렀을 경우 */
	public void OnTouchNextBtn() {
		this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.NEXT)?.Invoke(this);
	}

	/** 재시도 버튼을 눌렀을 경우 */
	public void OnTouchRetryBtn() {
		this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.RETRY)?.Invoke(this);
	}

	/** 나가기 버튼을 눌렀을 경우 */
	public void OnTouchLeaveBtn() {
		this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.LEAVE)?.Invoke(this);
	}
	#endregion // 함수

	#region 클래스 함수
	/** 매개 변수를 생성한다 */
	public static STParams MakeParams(STRecordInfo a_stRecordInfo, Dictionary<ECallback, System.Action<CResultPopup>> a_oCallbackDict = null, NSEngine.CEngine _engine = null) {
		return new STParams() {
			m_stRecordInfo = a_stRecordInfo, m_oCallbackDict = a_oCallbackDict ?? new Dictionary<ECallback, System.Action<CResultPopup>>(),
            Engine = _engine
		};
	}
	#endregion // 클래스 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

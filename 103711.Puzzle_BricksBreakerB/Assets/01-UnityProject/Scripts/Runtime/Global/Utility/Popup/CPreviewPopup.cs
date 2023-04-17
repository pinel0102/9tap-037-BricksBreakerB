using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using TMPro;

/** 정지 팝업 */
public partial class CPreviewPopup : CSubPopup {
	/** 식별자 */
	public enum EKey {
		NONE = -1,
        RETRY_BTN,
		LEAVE_BTN,
		[HideInInspector] MAX_VAL
	}

	/** 콜백 */
	public enum ECallback {
		NONE = -1,
        RETRY,
		LEAVE,
        RESUME,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public Dictionary<ECallback, System.Action<CPreviewPopup>> m_oCallbackDict;
        public NSEngine.CEngine Engine;
	}

	#region 변수

    public List<Button> buttonPlay = new List<Button>();
    public Button buttonPlayAD;

	#endregion // 변수

	#region 프로퍼티
	public STParams Params { get; private set; }

    public TMP_Text levelText;
    public SpriteMask previewMask;
    public RectTransform previewArea;

    private const string formatLevel = "Level {0}";
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
            ($"{EKey.RETRY_BTN}", this.gameObject, this.OnTouchRetryBtn),
			($"{EKey.LEAVE_BTN}", this.gameObject, this.OnTouchLeaveBtn)
		});

        for (int i=0; i < buttonPlay.Count; i++)
        {
            buttonPlay[i].ExAddListener(this.OnTouchPlayButton);
        }

        buttonPlayAD.ExAddListener(this.OnTouchPlayButtonAD);

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

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {

        Params.Engine.SetupPreview(previewArea, previewMask);

        levelText.text = string.Format(formatLevel, Params.Engine.currentLevel);
        
        GlobalDefine.PlaySoundFX(ESoundSet.SOUND_LEVEL_READY);

		this.SubUpdateUIsState();
	}

    private void OnTouchPlayButton()
    {
        this.OnTouchResumeBtn();
    }

    private void OnTouchPlayButtonAD()
    {
        this.OnTouchResumeBtn();
    }

    /** 재시도 버튼을 눌렀을 경우 */
	public void OnTouchRetryBtn() {
		this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.RETRY)?.Invoke(this);
	}

    private void OnTouchLeaveBtn() {
		this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.LEAVE)?.Invoke(this);
	}

	/** Play 버튼을 눌렀을 경우 */
	private void OnTouchResumeBtn() {
		this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.RESUME)?.Invoke(this);
	}
	#endregion // 함수

	#region 클래스 함수
	/** 매개 변수를 생성한다 */
	public static STParams MakeParams(Dictionary<ECallback, System.Action<CPreviewPopup>> a_oCallbackDict = null, NSEngine.CEngine _engine = null) {
		return new STParams() {
			m_oCallbackDict = a_oCallbackDict ?? new Dictionary<ECallback, System.Action<CPreviewPopup>>(),
            Engine = _engine
		};
	}
	#endregion // 클래스 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

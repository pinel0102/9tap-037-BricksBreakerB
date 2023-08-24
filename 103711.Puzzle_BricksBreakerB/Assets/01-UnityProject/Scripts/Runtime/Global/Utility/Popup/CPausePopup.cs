using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using TMPro;

/** 정지 팝업 */
public partial class CPausePopup : CSubPopup {
	/** 식별자 */
	public enum EKey {
		NONE = -1,
        RETRY_BTN,
		LEAVE_BTN,
        BG_SND_BTN,
		FX_SNDS_BTN,
		[HideInInspector] MAX_VAL
	}

	/** 콜백 */
	public enum ECallback {
		NONE = -1,
        RETRY,
		LEAVE,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public Dictionary<ECallback, System.Action<CPausePopup>> m_oCallbackDict;
        public NSEngine.CEngine Engine;
	}

	#region 변수

	#endregion // 변수

	#region 프로퍼티
	public STParams Params { get; private set; }

    public TMP_Text levelText;
    public SpriteMask previewMask;
    public RectTransform previewArea;
    public Button ADBlockButton;

    private Dictionary<EKey, Button> m_oBtnDict = new Dictionary<EKey, Button>();
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

        CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
			(EKey.BG_SND_BTN, $"{EKey.BG_SND_BTN}", this.Contents, this.OnTouchBGSndBtn),
			(EKey.FX_SNDS_BTN, $"{EKey.FX_SNDS_BTN}", this.Contents, this.OnTouchFXSndsBtn)
		}, m_oBtnDict);

        ADBlockButton.ExAddListener(OnTouchADBlockButton);

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
        
        levelText.text = string.Format(formatLevel, CSceneManager.GetSceneManager<GameScene.CSubGameSceneManager>(KCDefine.B_SCENE_N_GAME).Engine.currentLevel);
        ADBlockButton.gameObject.SetActive(!CUserInfoStorage.Inst.IsPurchaseRemoveAds && !CUserInfoStorage.Inst.subscriptionActivated);

        var oBtnKeyInfoList = CCollectionManager.Inst.SpawnList<(EKey, string, string, string, bool)>();

		try {
			CSndManager.Inst.SetIsMuteBGSnd(CCommonGameInfoStorage.Inst.GameInfo.IsMuteBGSnd);
			CSndManager.Inst.SetIsMuteFXSnds(CCommonGameInfoStorage.Inst.GameInfo.IsMuteFXSnds);

			// 버튼을 갱신한다 {
			oBtnKeyInfoList.ExAddVal((EKey.BG_SND_BTN, KCDefine.U_OBJ_N_ICON_IMG, KDefine.G_IMG_P_SETTINGS_P_BG_SND_ON, KDefine.G_IMG_P_SETTINGS_P_BG_SND_OFF, !CCommonGameInfoStorage.Inst.GameInfo.IsMuteBGSnd));
			oBtnKeyInfoList.ExAddVal((EKey.FX_SNDS_BTN, KCDefine.U_OBJ_N_ICON_IMG, KDefine.G_IMG_P_SETTINGS_P_FX_SNDS_ON, KDefine.G_IMG_P_SETTINGS_P_FX_SNDS_OFF, !CCommonGameInfoStorage.Inst.GameInfo.IsMuteFXSnds));
			
			for(int i = 0; i < oBtnKeyInfoList.Count; ++i) {
				string oImgPath = oBtnKeyInfoList[i].Item5 ? oBtnKeyInfoList[i].Item3 : oBtnKeyInfoList[i].Item4;
				m_oBtnDict.GetValueOrDefault(oBtnKeyInfoList[i].Item1)?.gameObject.ExFindComponent<Image>(oBtnKeyInfoList[i].Item2)?.ExSetSprite<Image>(CResManager.Inst.GetRes<Sprite>(oImgPath));
			}
            // 버튼을 갱신한다 }
            
		} finally {
			CCollectionManager.Inst.DespawnList(oBtnKeyInfoList);
		}

		this.SubUpdateUIsState();
	}

    /** 재시도 버튼을 눌렀을 경우 */
	public void OnTouchRetryBtn() {
		this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.RETRY)?.Invoke(this);
	}

	/** 나가기 버튼을 눌렀을 경우 */
	private void OnTouchLeaveBtn() {
		this.Params.m_oCallbackDict?.GetValueOrDefault(ECallback.LEAVE)?.Invoke(this);
	}

    /** 배경음 버튼을 눌렀을 경우 */
	private void OnTouchBGSndBtn() {
		CCommonGameInfoStorage.Inst.GameInfo.IsMuteBGSnd = !CCommonGameInfoStorage.Inst.GameInfo.IsMuteBGSnd;
		CCommonGameInfoStorage.Inst.SaveGameInfo();

		this.UpdateUIsState();
	}

	/** 효과음 버튼을 눌렀을 경우 */
	private void OnTouchFXSndsBtn() {
		CCommonGameInfoStorage.Inst.GameInfo.IsMuteFXSnds = !CCommonGameInfoStorage.Inst.GameInfo.IsMuteFXSnds;
		CCommonGameInfoStorage.Inst.SaveGameInfo();

		this.UpdateUIsState();
	}

    private void OnTouchADBlockButton()
    {
        CSceneManager.GetSceneManager<OverlayScene.CSubOverlaySceneManager>(KCDefine.B_SCENE_N_OVERLAY)?.PurchaseProduct(EProductKinds.SINGLE_PRODUCT_REMOVE_ADS, this.OnPurchaseProduct);
    }

    private void OnPurchaseProduct(CPurchaseManager a_oSender, string a_oProductID, bool a_bIsSuccess) 
    {
        // 결제 되었을 경우
        if(a_bIsSuccess) {
            ADBlockButton.gameObject.SetActive(!CUserInfoStorage.Inst.IsPurchaseRemoveAds && !CUserInfoStorage.Inst.subscriptionActivated);
        }
    }
	#endregion // 함수

	#region 클래스 함수
	/** 매개 변수를 생성한다 */
	public static STParams MakeParams(Dictionary<ECallback, System.Action<CPausePopup>> a_oCallbackDict = null, NSEngine.CEngine _engine = null) {
		return new STParams() {
			m_oCallbackDict = a_oCallbackDict ?? new Dictionary<ECallback, System.Action<CPausePopup>>(),
            Engine = _engine
		};
	}
	#endregion // 클래스 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

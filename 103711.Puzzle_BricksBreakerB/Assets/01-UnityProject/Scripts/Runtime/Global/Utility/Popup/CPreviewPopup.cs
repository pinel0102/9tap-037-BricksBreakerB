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

    public RectTransform preview;
    public SpriteMask mask;
    public TMP_Text levelText;

    private bool isfirstScreenOnly;
    public int firstRow;
    public int lastRow;
    public int activeLine;

    private Vector3 CELL_SPRITE_ADJUSTMENT;
    private Vector3 MaxGridSize;
    private Vector3 CellSize;
	private Vector3 CellCenterOffset;
    private Vector3 GridOffset;
    private STSortingOrderInfo sortingOrderInfoCell;
    private STSortingOrderInfo sortingOrderInfoSub;
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

        int level = Params.Engine.currentLevel;

        levelText.text = string.Format(formatLevel, level);
        SetupPreview(level);

        GlobalDefine.PlaySoundFX(ESoundSet.SOUND_LEVEL_READY);

		this.SubUpdateUIsState();
	}


#region Preview

    private void SetupPreview(int level)
    {
        sortingOrderInfoCell = new STSortingOrderInfo(){ m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_FOREGROUND };
        sortingOrderInfoSub = new STSortingOrderInfo(){ m_nOrder = KCDefine.B_VAL_1_INT, m_oLayer = KCDefine.U_SORTING_L_FOREGROUND };

        isfirstScreenOnly = false;
        firstRow = -1;
        lastRow = -1;

        mask.ExSetSortingOrder(sortingOrderInfoCell);

        // 레벨 정보가 존재 할 경우
        if(CGameInfoStorage.Inst.PlayLevelInfo != null) {

            if (isfirstScreenOnly)
            {
                CELL_SPRITE_ADJUSTMENT = new Vector3(-1f, -1f, 0);
                MaxGridSize = new Vector3(preview.sizeDelta.x, preview.sizeDelta.y, 0);
                CellSize = new Vector3(MaxGridSize.x / (float)NSEngine.KDefine.E_DEF_NUM_CELLS.x, MaxGridSize.x / (float)NSEngine.KDefine.E_DEF_NUM_CELLS.x, KCDefine.B_VAL_0_REAL);
                CellCenterOffset = new Vector3(CellSize.x / KCDefine.B_VAL_2_REAL, CellSize.y / -KCDefine.B_VAL_2_REAL, KCDefine.B_VAL_0_REAL);
                GridOffset = new Vector3(-preview.sizeDelta.x * 0.5f, (preview.sizeDelta.y * 0.5f) + (Mathf.Max(0, CGameInfoStorage.Inst.PlayLevelInfo.m_oCellInfoDictContainer.Count - NSEngine.KDefine.E_DEF_NUM_CELLS.y) * CellSize.y) - CellSize.y, 0);
                
                for(int i = Mathf.Max(0, CGameInfoStorage.Inst.PlayLevelInfo.m_oCellInfoDictContainer.Count - NSEngine.KDefine.E_DEF_NUM_CELLS.y); i < CGameInfoStorage.Inst.PlayLevelInfo.m_oCellInfoDictContainer.Count; ++i) {
                    for(int j = 0; j < CGameInfoStorage.Inst.PlayLevelInfo.m_oCellInfoDictContainer[i].Count; ++j) {
                        this.SetupCell(CGameInfoStorage.Inst.PlayLevelInfo.m_oCellInfoDictContainer[i][j]);
                    }
                }
            }
            else
            {
                CELL_SPRITE_ADJUSTMENT = new Vector3(-1f, -1f, 0);
                MaxGridSize = new Vector3(preview.sizeDelta.x, preview.sizeDelta.y, 0);
                CellSize = new Vector3(MaxGridSize.x / (float)NSEngine.KDefine.E_DEF_NUM_CELLS.x, MaxGridSize.x / (float)NSEngine.KDefine.E_DEF_NUM_CELLS.x, KCDefine.B_VAL_0_REAL);
                CellSize *= Params.Engine.SelGridInfo.m_stScale.x;
                CellCenterOffset = new Vector3(CellSize.x / KCDefine.B_VAL_2_REAL, CellSize.y / -KCDefine.B_VAL_2_REAL, KCDefine.B_VAL_0_REAL);
                GridOffset = new Vector3(-preview.sizeDelta.x * 0.5f, 0, 0);
                
                for(int i = 0; i < CGameInfoStorage.Inst.PlayLevelInfo.m_oCellInfoDictContainer.Count; ++i) {
                    for(int j = 0; j < CGameInfoStorage.Inst.PlayLevelInfo.m_oCellInfoDictContainer[i].Count; ++j) {
                        
                        if (CGameInfoStorage.Inst.PlayLevelInfo.m_oCellInfoDictContainer[i][j].m_oCellObjInfoList.Count > 0)
                        {
                            if (firstRow == -1) 
                                firstRow = i;                        
                            lastRow = i;
                        }

                        this.SetupCell(CGameInfoStorage.Inst.PlayLevelInfo.m_oCellInfoDictContainer[i][j]);
                    }
                }

                activeLine = lastRow - firstRow + 1;
                preview.localPosition = new Vector3(0, ((activeLine * 0.5f) + firstRow) * CellSize.y, 0);
            }
        }
    }

    /** 셀을 설정한다 */
    private void SetupCell(STCellInfo a_stCellInfo) {
        
        for(int i = 0; i < a_stCellInfo.m_oCellObjInfoList.Count; ++i) {
            EObjKinds kinds = a_stCellInfo.m_oCellObjInfoList[i].ObjKinds;
            //EObjKinds kindsType = (EObjKinds)((int)kinds).ExKindsToCorrectKinds(EKindsGroupType.SUB_KINDS_TYPE);
            STObjInfo stObjInfo = CObjInfoTable.Inst.GetObjInfo(kinds);
            STCellObjInfo stCellObjInfo = (STCellObjInfo)a_stCellInfo.m_oCellObjInfoList[i].Clone();

            var oCellObj = SpawnObj(stObjInfo);
            
            oCellObj.transform.localPosition = GridOffset + a_stCellInfo.m_stIdx.ExToPos(CellCenterOffset, CellSize);
            oCellObj.GetController<NSEngine.CECellObjController>().ResetObjInfo(stObjInfo, stCellObjInfo);
            
            oCellObj.SetCellObjInfo(stCellObjInfo);
            //oCellObj.AddCellEffect(kindsType);
            oCellObj.SetCellActive(true);

            oCellObj.TargetSprite.size = CellSize + CELL_SPRITE_ADJUSTMENT;            
            oCellObj.TargetSprite.ExSetSortingOrder(sortingOrderInfoCell);
            oCellObj.TargetSprite.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

            oCellObj.UpperSprite.size = CellSize + CELL_SPRITE_ADJUSTMENT;
            oCellObj.UpperSprite.ExSetSortingOrder(sortingOrderInfoSub);
            oCellObj.UpperSprite.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

            oCellObj.HPText.text = string.Empty;
        }
    }

    private NSEngine.CEObj SpawnObj(STObjInfo a_stObjInfo) {		
        GameObject oObj = (GameObject)Instantiate(Resources.Load(NSEngine.KDefine.E_OBJ_P_CELL_OBJ), preview);
        oObj.transform.localScale = Vector3.one;
		oObj.transform.localEulerAngles = Vector3.zero;
        oObj.transform.localPosition = Vector3.zero;

        var ceObj = oObj.GetComponent<NSEngine.CEObj>();
        var oController = oObj.ExAddComponent<NSEngine.CECellObjController>();

        ceObj.Init(NSEngine.CEObj.MakeParams(this.Params.Engine, a_stObjInfo, null, oController));        
        oController?.Init(NSEngine.CECellObjController.MakeParams(this.Params.Engine), a_stObjInfo.m_eObjKinds);

        ceObj.SetOwner(null);
        oController?.SetOwner(ceObj);

        return ceObj;
	}

#endregion Preview


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

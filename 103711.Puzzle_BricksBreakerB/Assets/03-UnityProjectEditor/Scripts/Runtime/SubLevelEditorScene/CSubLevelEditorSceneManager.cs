using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
using System.Linq;
using System.Globalization;
using UnityEngine.EventSystems;
using EnhancedUI.EnhancedScroller;
using DanielLochner.Assets.SimpleScrollSnap;

namespace LevelEditorScene {
	/** 서브 레벨 에디터 씬 관리자 */
	public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			PREV_CELL_IDX,
			UPDATE_SKIP_TIME,
			GRID_SCROLL_DELTA_X,
			GRID_SCROLL_DELTA_Y,

			SEL_GRID_IDX,
			SEL_USER_TYPE,
			SEL_TABLE_SRC,
			SEL_OBJ_KINDS,
			SEL_INPUT_POPUP,
			SEL_EDITOR_MODE,

			SEL_SCROLLER,
			SEL_OBJ_SPRITE,
			SEL_LEVEL_INFO,

			ME_UIS_MSG_TEXT,
			ME_UIS_LEVEL_TEXT,
			ME_UIS_OBJ_SIZE_TEXT,

			ME_UIS_SEL_OBJ_IMG,

			ME_UIS_PREV_GRID_BTN,
			ME_UIS_NEXT_GRID_BTN,
			ME_UIS_PREV_LEVEL_BTN,
			ME_UIS_NEXT_LEVEL_BTN,
			ME_UIS_MOVE_LEVEL_BTN,
			ME_UIS_REMOVE_LEVEL_BTN,

			ME_UIS_GRID_SCROLL_BAR_H,
			ME_UIS_GRID_SCROLL_BAR_V,

			ME_UIS_DRAW_MODE_TOGGLE,
			ME_UIS_PAINT_MODE_TOGGLE,

			LE_UIS_A_SET_BTN,
			LE_UIS_B_SET_BTN,
			LE_UIS_ADD_STAGE_BTN,
			LE_UIS_ADD_CHAPTER_BTN,

			LE_UIS_LEVEL_SCROLLER_INFO,
			LE_UIS_STAGE_SCROLLER_INFO,
			LE_UIS_CHAPTER_SCROLLER_INFO,

			LE_UIS_STAGE_SCROLLER_INFO_01,
			LE_UIS_STAGE_SCROLLER_INFO_02,

			RE_UIS_PAGE_TEXT,
			RE_UIS_TITLE_TEXT,

			RE_UIS_PREV_BTN,
			RE_UIS_NEXT_BTN,
			RE_UIS_PAGE_SCROLL_SNAP,

			RE_UIS_PAGE_UIS_01,
			RE_UIS_PAGE_UIS_02,

			RE_UIS_PAGE_UIS_01_LOAD_LEVEL_BTN,
			RE_UIS_PAGE_UIS_01_LOAD_LOCAL_TABLE_BTN,
			RE_UIS_PAGE_UIS_01_LOAD_REMOTE_TABLE_BTN,
			RE_UIS_PAGE_UIS_01_REMOVE_ALL_LEVELS_BTN,

			RE_UIS_PAGE_UIS_01_LEVEL_INPUT,
			RE_UIS_PAGE_UIS_01_NUM_CELLS_X_INPUT,
			RE_UIS_PAGE_UIS_01_NUM_CELLS_Y_INPUT,

			RE_UIS_PAGE_UIS_02_OBJ_SIZE_X_INPUT,
			RE_UIS_PAGE_UIS_02_OBJ_SIZE_Y_INPUT,
			[HideInInspector] MAX_VAL
		}

		/** 테이블 소스 */
		private enum ETableSrc {
			NONE = -1,
			LOCAL,
			REMOTE,
			[HideInInspector] MAX_VAL
		}

		/** 입력 팝업 */
		private enum EInputPopup {
			NONE = -1,
			MOVE_LEVEL,
			REMOVE_LEVEL,
			[HideInInspector] MAX_VAL
		}

		/** 에디터 모드 */
		private enum EEditorMode {
			NONE = -1,
			DRAW,
			PAINT,
			[HideInInspector] MAX_VAL
		}

		/** 콜백 */
		private enum ECallback {
			NONE = -1,
			SETUP_RE_UIS_PAGE_UIS_01,
			SETUP_RE_UIS_PAGE_UIS_02,
			UPDATE_RE_UIS_PAGE_UIS_01,
			UPDATE_RE_UIS_PAGE_UIS_02,
			[HideInInspector] MAX_VAL
		}

		/** 객체 스프라이트 정보 */
		private struct STObjSpriteInfo {
			public EObjKinds m_eObjKinds;
			public SpriteRenderer m_oSprite;
		}

		#region 변수
		private Dictionary<EKey, int> m_oIntDict = new Dictionary<EKey, int>() {
			[EKey.SEL_GRID_IDX] = KCDefine.B_VAL_0_INT
		};

		private Dictionary<EKey, float> m_oRealDict = new Dictionary<EKey, float>() {
			[EKey.UPDATE_SKIP_TIME] = KCDefine.B_VAL_0_REAL,
			[EKey.GRID_SCROLL_DELTA_X] = KCDefine.B_VAL_0_REAL,
			[EKey.GRID_SCROLL_DELTA_Y] = KCDefine.B_VAL_0_REAL
		};

		private Dictionary<EKey, EUserType> m_oUserTypeDict = new Dictionary<EKey, EUserType>() {
			[EKey.SEL_USER_TYPE] = EUserType.NONE
		};

		private Dictionary<EKey, ETableSrc> m_oTableSrcDict = new Dictionary<EKey, ETableSrc>() {
			[EKey.SEL_TABLE_SRC] = ETableSrc.NONE
		};

		private Dictionary<EKey, EObjKinds> m_oObjKindsDict = new Dictionary<EKey, EObjKinds>() {
			[EKey.SEL_OBJ_KINDS] = EObjKinds.NONE
		};

		private Dictionary<EKey, EInputPopup> m_oInputPopupDict = new Dictionary<EKey, EInputPopup>() {
			[EKey.SEL_INPUT_POPUP] = EInputPopup.NONE
		};

		private Dictionary<EKey, EEditorMode> m_oEditorModeDict = new Dictionary<EKey, EEditorMode>() {
			[EKey.SEL_EDITOR_MODE] = EEditorMode.NONE
		};

		private Dictionary<EKey, Vector3Int> m_oVec3IntDict = new Dictionary<EKey, Vector3Int>() {
			[EKey.PREV_CELL_IDX] = new Vector3Int(KCDefine.B_IDX_INVALID, KCDefine.B_IDX_INVALID, KCDefine.B_IDX_INVALID)
		};

		private Sprite m_oGridBoundsImg = null;
		private Texture2D m_oGridBoundsTex2D = null;
		private List<LineRenderer> m_oGridLineFXList = new List<LineRenderer>();

		private Dictionary<EKey, SpriteRenderer> m_oSpriteDict = new Dictionary<EKey, SpriteRenderer>();
		private Dictionary<ECallback, System.Reflection.MethodInfo> m_oMethodInfoDict = new Dictionary<ECallback, System.Reflection.MethodInfo>();
		private Dictionary<ECallback, System.Reflection.MethodInfo> m_oSubMethodInfoDict = new Dictionary<ECallback, System.Reflection.MethodInfo>();

#if GOOGLE_SHEET_ENABLE
		private SimpleJSON.JSONNode m_oVerInfos = null;
		private Dictionary<string, System.Action<CServicesManager, STGoogleSheetLoadInfo, Dictionary<string, SimpleJSON.JSONNode>, bool>> m_oGoogleSheetLoadHandlerDict = new Dictionary<string, System.Action<CServicesManager, STGoogleSheetLoadInfo, Dictionary<string, SimpleJSON.JSONNode>, bool>>();
#endif // #if GOOGLE_SHEET_ENABLE

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		private Dictionary<EKey, CLevelInfo> m_oLevelInfoDict = new Dictionary<EKey, CLevelInfo>() {
			[EKey.SEL_LEVEL_INFO] = null
		};

		private List<STObjSpriteInfo>[,] m_oObjSpriteInfoLists = null;
		private List<NSEngine.STGridInfo> m_oGridInfoList = new List<NSEngine.STGridInfo>();
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

		/** =====> UI <===== */
		private Dictionary<EKey, EnhancedScroller> m_oScrollerDict = new Dictionary<EKey, EnhancedScroller>() {
			[EKey.SEL_SCROLLER] = null
		};

		private List<InputField> m_oInputList01 = new List<InputField>();
		private List<InputField> m_oInputList02 = new List<InputField>();

		private List<Button> m_oGridLineBtnHList = new List<Button>();
		private List<Button> m_oGridLineBtnVList = new List<Button>();

		private Dictionary<EKey, Text> m_oTextDict = new Dictionary<EKey, Text>();
		private Dictionary<EKey, InputField> m_oInputDict = new Dictionary<EKey, InputField>();
		private Dictionary<EKey, Image> m_oImgDict = new Dictionary<EKey, Image>();
		private Dictionary<EKey, Button> m_oBtnDict = new Dictionary<EKey, Button>();
		private Dictionary<EKey, Toggle> m_oToggleDict = new Dictionary<EKey, Toggle>();
		private Dictionary<EKey, Scrollbar> m_oScrollBarDict = new Dictionary<EKey, Scrollbar>();
		private Dictionary<EKey, SimpleScrollSnap> m_oScrollSnapDict = new Dictionary<EKey, SimpleScrollSnap>();
		private Dictionary<EKey, STScrollerInfo> m_oScrollerInfoDict = new Dictionary<EKey, STScrollerInfo>();

		/** =====> 객체 <===== */
		private Dictionary<EKey, GameObject> m_oUIsDict = new Dictionary<EKey, GameObject>();
		#endregion // 변수

		#region 프로퍼티
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		public int SelGridInfoIdx => m_oIntDict[EKey.SEL_GRID_IDX];
		public CLevelInfo SelLevelInfo => m_oLevelInfoDict[EKey.SEL_LEVEL_INFO];
		public NSEngine.STGridInfo SelGridInfo => m_oGridInfoList[this.SelGridInfoIdx];
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		#endregion // 프로퍼티

		#region IEnhancedScrollerDelegate
		/** 셀 개수를 반환한다 */
		public int GetNumberOfCells(EnhancedScroller a_oSender) {
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
			// 레벨 스크롤러 일 경우
			if(m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].m_oScroller == a_oSender) {
				return CLevelInfoTable.Inst.GetNumLevelInfos(this.SelLevelInfo.m_stIDInfo.m_nID02, this.SelLevelInfo.m_stIDInfo.m_nID03);
			}

			return (m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].m_oScroller == a_oSender) ? CLevelInfoTable.Inst.GetNumStageInfos(this.SelLevelInfo.m_stIDInfo.m_nID03) : CLevelInfoTable.Inst.NumChapterInfos;
#else
			return KCDefine.B_VAL_0_INT;
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		}

		/** 셀 뷰 크기를 반환한다 */
		public float GetCellViewSize(EnhancedScroller a_oSender, int a_nDataIdx) {
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
			// 레벨 스크롤러 일 경우
			if(m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].m_oScroller == a_oSender) {
				return (m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].m_oScrollerCellView.transform as RectTransform).sizeDelta.y;
			}

			return (m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].m_oScroller == a_oSender) ? (m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].m_oScrollerCellView.transform as RectTransform).sizeDelta.y : (m_oScrollerInfoDict[EKey.LE_UIS_CHAPTER_SCROLLER_INFO].m_oScrollerCellView.transform as RectTransform).sizeDelta.y;
#else
			return KCDefine.B_VAL_0_REAL;
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		}

		/** 셀 뷰를 반환한다 */
		public EnhancedScrollerCellView GetCellView(EnhancedScroller a_oSender, int a_nDataIdx, int a_nCellIdx) {
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
			var oCallbackDict01 = new Dictionary<CScrollerCellView.ECallback, System.Action<CScrollerCellView, ulong>>() {
				[CScrollerCellView.ECallback.SEL] = this.OnReceiveSelCallback
			};

			var oCallbackDict02 = new Dictionary<CEditorScrollerCellView.ECallback, System.Action<CEditorScrollerCellView, ulong>>() {
				[CEditorScrollerCellView.ECallback.COPY] = this.OnReceiveCopyCallback,
				[CEditorScrollerCellView.ECallback.MOVE] = this.OnReceiveMoveCallback,
				[CEditorScrollerCellView.ECallback.REMOVE] = this.OnReceiveRemoveCallback
			};

			// 레벨 스크롤러 일 경우
			if(m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].m_oScroller == a_oSender) {
				return this.CreateLevelScrollerCellView(a_oSender, a_nDataIdx, a_nCellIdx, oCallbackDict01, oCallbackDict02);
			}

			return (m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].m_oScroller == a_oSender) ? this.CreateStageScrollerCellView(a_oSender, a_nDataIdx, a_nCellIdx, oCallbackDict01, oCallbackDict02) : this.CreateChapterScrollerCellView(a_oSender, a_nDataIdx, a_nCellIdx, oCallbackDict01, oCallbackDict02);
#else
			return null;
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		}
		#endregion // IEnhancedScrollerDelegate

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
				// 레벨 정보가 없을 경우
				if(!CLevelInfoTable.Inst.LevelInfoDictContainer.ExIsValid()) {
					var oLevelInfo = Factory.MakeEditorLevelInfo(KCDefine.B_VAL_0_INT);
					CLevelInfoTable.Inst.AddLevelInfo(oLevelInfo);

					Func.SetupEditorLevelInfo(oLevelInfo, new CSubEditorCreateInfo() {
						m_nNumLevels = KCDefine.B_VAL_0_INT, m_stMinNumCells = NSEngine.KDefine.E_MIN_NUM_CELLS, m_stMaxNumCells = NSEngine.KDefine.E_MIN_NUM_CELLS
					});

					CLevelInfoTable.Inst.SaveLevelInfos();
				}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

				// 객체 풀을 설정한다 {
				this.AddObjsPool(KDefine.LES_KEY_SPRITE_OBJS_POOL, CFactory.CreateObjsPool(KCDefine.U_OBJ_P_SPRITE, this.ObjRoot));
				this.AddObjsPool(KDefine.LES_KEY_LINE_FX_OBJS_POOL, CFactory.CreateObjsPool(KCDefine.U_OBJ_P_LINE_FX, this.ObjRoot));

				this.AddObjsPool(KDefine.LES_KEY_BTN_OBJS_POOL, CFactory.CreateObjsPool(KCDefine.U_OBJ_P_TEXT_BTN, this.MidEditorUIs));
				// 객체 풀을 설정한다 }

				// 텍스처를 설정한다
				m_oGridBoundsTex2D = CFactory.MakeTex2D(KCDefine.U_IMG_N_TEX, new Vector3Int((int)(this.ScreenWidth * KCDefine.B_VAL_2_REAL), (int)(this.ScreenHeight * KCDefine.B_VAL_2_REAL), KCDefine.B_VAL_0_INT));
				m_oGridBoundsTex2D?.ExSetPixels(Color.white);

				// 스프라이트를 설정한다 {
				CFunc.SetupComponents(new List<(EKey, string, GameObject, GameObject)>() {
					(EKey.SEL_OBJ_SPRITE, $"{EKey.SEL_OBJ_SPRITE}", this.ObjRoot, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_SPRITE))
				}, m_oSpriteDict);

				m_oSpriteDict[EKey.SEL_OBJ_SPRITE]?.gameObject.SetActive(false);
				m_oSpriteDict[EKey.SEL_OBJ_SPRITE]?.ExSetSortingOrder(KCDefine.U_SORTING_OI_OVERGROUND);
				// 스프라이트를 설정한다 }

#if GOOGLE_SHEET_ENABLE
				// 구글 시트 로드 처리자를 설정한다
				m_oGoogleSheetLoadHandlerDict.TryAdd(KCDefine.U_TABLE_P_G_ETC_INFO.ExGetFileName(false), this.OnLoadGoogleSheet);
				m_oGoogleSheetLoadHandlerDict.TryAdd(KCDefine.U_TABLE_P_G_MISSION_INFO.ExGetFileName(false), this.OnLoadGoogleSheet);
				m_oGoogleSheetLoadHandlerDict.TryAdd(KCDefine.U_TABLE_P_G_REWARD_INFO.ExGetFileName(false), this.OnLoadGoogleSheet);
				m_oGoogleSheetLoadHandlerDict.TryAdd(KCDefine.U_TABLE_P_G_RES_INFO.ExGetFileName(false), this.OnLoadGoogleSheet);
				m_oGoogleSheetLoadHandlerDict.TryAdd(KCDefine.U_TABLE_P_G_ITEM_INFO.ExGetFileName(false), this.OnLoadGoogleSheet);
				m_oGoogleSheetLoadHandlerDict.TryAdd(KCDefine.U_TABLE_P_G_SKILL_INFO.ExGetFileName(false), this.OnLoadGoogleSheet);
				m_oGoogleSheetLoadHandlerDict.TryAdd(KCDefine.U_TABLE_P_G_OBJ_INFO.ExGetFileName(false), this.OnLoadGoogleSheet);
				m_oGoogleSheetLoadHandlerDict.TryAdd(KCDefine.U_TABLE_P_G_ABILITY_INFO.ExGetFileName(false), this.OnLoadGoogleSheet);
				m_oGoogleSheetLoadHandlerDict.TryAdd(KCDefine.U_TABLE_P_G_PRODUCT_INFO.ExGetFileName(false), this.OnLoadGoogleSheet);
#endif // #if GOOGLE_SHEET_ENABLE

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
				this.SetupMidEditorUIs();
				this.SetupLeftEditorUIs();
				this.SetupRightEditorUIs();

				this.SetSelLevelInfo(CGameInfoStorage.Inst.PlayLevelInfo ?? CLevelInfoTable.Inst.GetLevelInfo(KCDefine.B_VAL_0_INT));
				this.SubAwake();
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
			}
		}

		/** 초기화 */
		public override void Start() {
			base.Start();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				// 스크롤 뷰를 설정한다
				m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP]?.gameObject.SetActive(true);

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
				this.SubStart();

				this.ExLateCallFunc((a_oSender) => {
					bool bIsValid = KDefine.LES_OBJ_KINDS_DICT_CONTAINER.TryGetValue(KCDefine.B_VAL_0_INT, out List<EObjKinds> oObjKindsList);

					this.SetMEUIsEditorMode(true, false);
					this.OnTouchREUIsPageUIs02ScrollerCellViewBtn(bIsValid ? oObjKindsList.ExGetVal(KCDefine.B_VAL_0_INT, EObjKinds.NONE) : EObjKinds.NONE);
				}, KCDefine.U_DELAY_INIT);
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

				CSndManager.Inst.StopBGSnd();
				CCommonAppInfoStorage.Inst.SetEnableEditor(false);
			}
		}

		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				m_oRealDict[EKey.UPDATE_SKIP_TIME] += a_fDeltaTime;

				// 단축키를 눌렀을 경우
				if(Input.GetKey(KeyCode.LeftShift)) {
					this.HandleHotKeys();
					CSceneManager.ActiveSceneEventSystem.SetSelectedGameObject(null);
				}

				// 메인 카메라가 존재 할 경우
				if(CSceneManager.IsExistsMainCamera) {
					var stMousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, this.PlaneDistance);
					var stCursorPos = CSceneManager.ActiveSceneMainCamera.ScreenToWorldPoint(stMousePos).ExToLocal(this.ObjRoot);

					// 그리드 영역 일 경우
					if(m_oGridInfoList.ExIsValidIdx(this.SelGridInfoIdx) && this.SelGridInfo.m_stViewBounds.Contains(stCursorPos)) {
						var stIdx = stCursorPos.ExToIdx(this.SelGridInfo.m_stPivotPos, NSEngine.Access.CellSize);

						// 인덱스가 유효 할 경우
						if(this.SelLevelInfo.m_oCellInfoDictContainer.ExIsValidIdx(stIdx)) {
							// Do Something
						}

						// 스크롤이 가능 할 경우
						if(!Input.mouseScrollDelta.ExIsEquals(Vector3.zero)) {
							float fScrollDeltaX = (Input.mouseScrollDelta.x * NSEngine.Access.CellSize.x) * this.EditorObjRoot.transform.localScale.x;
							float fScrollDeltaY = (Input.mouseScrollDelta.y * NSEngine.Access.CellSize.y) * this.EditorObjRoot.transform.localScale.y;

							this.SetMEUIsGridScrollDelta(m_oRealDict[EKey.GRID_SCROLL_DELTA_X] + fScrollDeltaX, m_oRealDict[EKey.GRID_SCROLL_DELTA_Y] - fScrollDeltaY);
						}

						bool bIsValid01 = m_oObjKindsDict[EKey.SEL_OBJ_KINDS].ExIsValid() && this.SelLevelInfo.m_oCellInfoDictContainer.ExIsValidIdx(stIdx);
						bool bIsValid02 = m_oGridInfoList.ExIsValidIdx(this.SelGridInfoIdx) && this.SelGridInfo.m_stViewBounds.Contains(stCursorPos);

						Color stColor = this.IsEnableAddCellObjInfo(stIdx, this.GetEditorObjSize(), m_oObjKindsDict[EKey.SEL_OBJ_KINDS]) ? Color.white : Color.red;

						m_oSpriteDict[EKey.SEL_OBJ_SPRITE]?.gameObject.SetActive(bIsValid01 && bIsValid02);
						m_oSpriteDict[EKey.SEL_OBJ_SPRITE]?.gameObject.ExSetLocalPos(this.SelGridInfo.m_stPivotPos + stIdx.ExToPos(NSEngine.Access.CellCenterOffset, NSEngine.Access.CellSize));
						m_oSpriteDict[EKey.SEL_OBJ_SPRITE]?.ExSetColor<SpriteRenderer>(stColor.ExGetAlphaColor(KCDefine.B_VAL_1_REAL / KCDefine.B_VAL_2_REAL));
					} else {
						m_oSpriteDict[EKey.SEL_OBJ_SPRITE]?.gameObject.SetActive(false);
					}
				}

				// 일정 시간이 지났을 경우
				if(m_oRealDict[EKey.UPDATE_SKIP_TIME].ExIsGreateEquals(KCDefine.B_UNIT_SECS_PER_MINUTE * KCDefine.B_VAL_5_REAL)) {
					m_oRealDict[EKey.UPDATE_SKIP_TIME] = KCDefine.B_VAL_0_REAL;
					this.OnTouchMEUIsSaveBtn();
				}

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
				// 저장 키를 눌렀을 경우
				if(Input.GetKey(CAccess.CmdKeyCode) && Input.GetKeyDown(KeyCode.S)) {
					this.OnTouchMEUIsSaveBtn();
				}

				// 모든 셀 채우기 키를 눌렀을 경우
				if(Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.F)) {
					this.OnTouchREUIsPageUIs01FillAllCellsBtn();
				}
				// 모든 셀 지우기 키를 눌렀을 경우
				else if(Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.C)) {
					this.OnTouchREUIsPageUIs01ClearAllCellsBtn();
				}
				// 선택 셀 지우기 키를 눌렀을 경우
				else if(Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.Q)) {
					this.OnTouchREUIsPageUIs01ClearSelCellsBtn();
				}

				// 로컬 테이블 로드 키를 눌렀을 경우
				if(Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.L)) {
					this.OnTouchREUIsPageUIs01LoadTableBtn(ETableSrc.LOCAL);
				}
				// 원격 테이블 로드 키를 눌렀을 경우
				else if(Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.R)) {
					this.OnTouchREUIsPageUIs01LoadTableBtn(ETableSrc.REMOTE);
				}

				this.SubOnUpdate(a_fDeltaTime);
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
			}
		}

		/** 상태를 갱신한다 */
		public override void OnLateUpdate(float a_fDeltaTime) {
			base.OnLateUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning && CNavStackManager.Inst.TopComponent == this) {
				// 탭 키를 눌렀을 경우
				if(UnityEngine.Input.GetKeyDown(KeyCode.Tab)) {
					var oInputListContainer = CCollectionManager.Inst.SpawnList<List<InputField>>();

					try {
						oInputListContainer.ExAddVal(m_oInputList01);
						oInputListContainer.ExAddVal(m_oInputList02);

						// 페이지 스크롤 스냅이 존재 할 경우
						if(m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP] != null) {
							int nIdx = oInputListContainer[m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP].SelectedPanel].FindIndex((a_oInput) => a_oInput.gameObject == CSceneManager.ActiveSceneEventSystem.currentSelectedGameObject);
							CSceneManager.ActiveSceneEventSystem.SetSelectedGameObject(oInputListContainer[m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP].SelectedPanel].ExGetVal((nIdx + KCDefine.B_VAL_1_INT) % oInputListContainer[m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP].SelectedPanel].Count, null)?.gameObject);
						}
					} finally {
						CCollectionManager.Inst.DespawnList(oInputListContainer);
					}
				}
			}
		}

		/** 제거 되었을 경우 */
		public override void OnDestroy() {
			base.OnDestroy();

			try {
				// 앱이 실행 중 일 경우
				if(CSceneManager.IsAppRunning) {
					GameObject.DestroyImmediate(m_oGridBoundsImg);
					GameObject.DestroyImmediate(m_oGridBoundsTex2D);
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CSubLevelEditorSceneManager.OnDestroy Exception: {oException.Message}");
			}
		}

		/** 내비게이션 스택 이벤트를 수신했을 경우 */
		public override void OnReceiveNavStackEvent(ENavStackEvent a_eEvent) {
			base.OnReceiveNavStackEvent(a_eEvent);

			// 백 키 눌림 이벤트 일 경우
			if(a_eEvent == ENavStackEvent.BACK_KEY_DOWN) {
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
				Func.ShowAlertPopup(KDefine.ES_MSG_ALERT_P_QUIT, this.OnReceiveEditorQuitPopupResult);
#else
				this.OnReceiveEditorQuitPopupResult(null, true);
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
			}
		}

		/** 화면 크기가 갱신 되었을 경우 */
		protected override void OnUpdateScreenSize(Vector3 a_stScreenSize) {
			base.OnUpdateScreenSize(a_stScreenSize);
			this.ExLateCallFunc((a_oSender) => CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_LEVEL_EDITOR), KCDefine.B_VAL_0_1_REAL);
		}

		/** 에디터 종료 팝업 결과를 수신했을 경우 */
		private void OnReceiveEditorQuitPopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
				CLevelInfoTable.Inst.SaveLevelInfos();
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

#if STUDY_MODULE_ENABLE && SCENE_TEMPLATES_MODULE_ENABLE
				CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MENU);
#elif EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
				CSceneLoader.Inst.LoadScene(COptsInfoTable.Inst.EtcOptsInfo.m_bIsEnableTitleScene ? KCDefine.B_SCENE_N_TITLE : KCDefine.B_SCENE_N_MAIN);
#endif // #if STUDY_MODULE_ENABLE && SCENE_TEMPLATES_MODULE_ENABLE
			}
		}

		/** 단축키를 처리한다 */
		private void HandleHotKeys() {
			// 이전 페이지 키를 눌렀을 경우
			if(Input.GetKeyDown(KeyCode.A)) {
				m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP]?.GoToPreviousPanel();
			}
			// 다음 페이지 키를 눌렀을 경우
			else if(Input.GetKeyDown(KeyCode.D)) {
				m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP]?.GoToNextPanel();
			}

			// 페이지 스크롤 스냅이 존재 할 경우
			if(m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP] != null) {
				for(int i = 0; i <= (int)(KeyCode.Alpha9 - KeyCode.Alpha1); ++i) {
					// 숫자 키를 눌렀을 경우
					if(Input.GetKeyDown(KeyCode.Alpha1 + i)) {
						int nNumPanels = m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP].NumberOfPanels;
						m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP].GoToPanel(Mathf.Clamp(i, KCDefine.B_VAL_0_INT, nNumPanels - KCDefine.B_VAL_1_INT));
					}
				}
			}

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
			// 에디터 모드 키를 눌렀을 경우
			if(Input.GetKeyDown(KeyCode.E)) {
				this.SetMEUIsEditorMode(m_oEditorModeDict[EKey.SEL_EDITOR_MODE] != EEditorMode.DRAW, m_oEditorModeDict[EKey.SEL_EDITOR_MODE] != EEditorMode.PAINT);
			}

			// 리셋 키를 눌렀을 경우
			if(Input.GetKeyDown(KeyCode.R)) {
				this.OnTouchMEUIsResetBtn();
			}
			// 테스트 키를 눌렀을 경우
			else if(Input.GetKeyDown(KeyCode.T)) {
				this.OnTouchMEUIsTestBtn();
			}

			// 이전 레벨 키를 눌렀을 경우
			if(Input.GetKeyDown(KeyCode.W)) {
				this.OnTouchMEUIsPrevLevelBtn();
			}
			// 다음 레벨 키를 눌렀을 경우
			else if(Input.GetKeyDown(KeyCode.S)) {
				this.OnTouchMEUIsNextLevelBtn();
			}

			// 위쪽 셀 이동 키를 눌렀을 경우
			if(Input.GetKeyDown(KeyCode.UpArrow)) {
				this.OnTouchREUIsPageUIs01MoveAllCellsBtn(EDirection.UP);
			}
			// 아래쪽 셀 이동 키를 눌렀을 경우
			else if(Input.GetKeyDown(KeyCode.DownArrow)) {
				this.OnTouchREUIsPageUIs01MoveAllCellsBtn(EDirection.DOWN);
			}

			// 왼쪽 셀 이동 키를 눌렀을 경우
			if(Input.GetKeyDown(KeyCode.LeftArrow)) {
				this.OnTouchREUIsPageUIs01MoveAllCellsBtn(EDirection.LEFT);
			}
			// 오른쪽 셀 이동 키를 눌렀을 경우
			else if(Input.GetKeyDown(KeyCode.RightArrow)) {
				this.OnTouchREUIsPageUIs01MoveAllCellsBtn(EDirection.RIGHT);
			}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		}
		#endregion // 함수

		#region 조건부 함수
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		/** 터치 이벤트를 처리한다 */
		protected override void HandleTouchEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData, ETouchEvent a_eTouchEvent) {
			base.HandleTouchEvent(a_oSender, a_oEventData, a_eTouchEvent);
			var stPos = a_oEventData.ExGetLocalPos(this.ObjRoot, this.ScreenSize);

			// 그리드 영역 일 경우
			if(this.BGTouchDispatcher == a_oSender && (m_oGridInfoList.ExIsValidIdx(this.SelGridInfoIdx) && this.SelGridInfo.m_stViewBounds.Contains(stPos))) {
				switch(a_eTouchEvent) {
					case ETouchEvent.BEGIN: this.HandleTouchBeginEvent(a_oSender, a_oEventData); break;
					case ETouchEvent.MOVE: this.HandleTouchMoveEvent(a_oSender, a_oEventData); break;
					case ETouchEvent.END: this.HandleTouchEndEvent(a_oSender, a_oEventData); break;
				}
			}
		}

		/** 객체 스프라이트를 리셋한다 */
		private void ResetObjSprites() {
			// 객체를 제거한다 {
			for(int i = 0; i < m_oGridLineBtnHList.Count; ++i) {
				this.DespawnObj(KDefine.LES_KEY_BTN_OBJS_POOL, m_oGridLineBtnHList[i].gameObject);
			}

			for(int i = 0; i < m_oGridLineBtnVList.Count; ++i) {
				this.DespawnObj(KDefine.LES_KEY_BTN_OBJS_POOL, m_oGridLineBtnVList[i].gameObject);
			}

			for(int i = 0; i < m_oGridLineFXList.Count; ++i) {
				this.DespawnObj(KDefine.LES_KEY_LINE_FX_OBJS_POOL, m_oGridLineFXList[i].gameObject);
			}

			m_oObjSpriteInfoLists?.ExEnumerate((oObjSpriteInfoList, a_stIdx) => {
				this.ResetObjSprites(oObjSpriteInfoList);
				return true;
			});
			// 객체를 제거한다 }

			// 그리드 정보를 설정한다 {
			m_oGridInfoList.Clear();

			// TODO: 다중 그리드 처리 구현 예정
			for(int i = 0; i < KCDefine.B_VAL_1_INT; ++i) {
				switch(this.SelLevelInfo.GridPivot) {
					case EGridPivot.DOWN: {
						var stGridInfo = NSEngine.Factory.MakeGridInfo(KCDefine.B_ANCHOR_DOWN_CENTER, Vector3.zero, Vector3.zero, this.SelLevelInfo.NumCells, true);
						var stGridScale = stGridInfo.m_stScale.ExIsValid() ? stGridInfo.m_stScale : Vector3.one;
						var stGridOffset = new Vector3(KCDefine.B_VAL_0_REAL, m_oRealDict[EKey.GRID_SCROLL_DELTA_Y], KCDefine.B_VAL_0_REAL);

						m_oGridInfoList.ExAddVal(NSEngine.Factory.MakeGridInfo(KCDefine.B_ANCHOR_DOWN_CENTER, new Vector3(KCDefine.B_VAL_0_REAL, (NSEngine.Access.MaxGridSize.y / -KCDefine.B_VAL_2_REAL) * (KCDefine.B_VAL_1_REAL / stGridScale.y), KCDefine.B_VAL_0_REAL), stGridOffset, this.SelLevelInfo.NumCells, true));
						break;
					}
					default: {
						m_oGridInfoList.ExAddVal(NSEngine.Factory.MakeGridInfo(KCDefine.B_ANCHOR_MID_CENTER, Vector3.zero, Vector3.zero, this.SelLevelInfo.NumCells));
						break;
					}
				}
			}
			// 그리드 정보를 설정한다 }

			// 객체를 설정한다 {
			this.ObjRoot.transform.localScale = this.SelGridInfo.m_stScale.ExIsValid() ? this.SelGridInfo.m_stScale : Vector3.one;

			this.EditorObjRoot.transform.localScale = KDefine.LES_SCALE_EDITOR_OBJ_ROOT;
			this.EditorObjRoot.transform.localPosition = this.ObjRootPivotPos;
			// 객체를 설정한다 }

			// 그리드 라인 효과를 설정한다 {
			m_oGridLineFXList.Clear();

			for(int i = 0; i < this.SelLevelInfo.NumCells.y; ++i) {
				for(int j = 0; j < this.SelLevelInfo.NumCells.x; ++j) {
					var oLineFX = this.SpawnObj<LineRenderer>(KDefine.LES_OBJ_N_GRID_LINE_FX, KDefine.LES_KEY_LINE_FX_OBJS_POOL);
					oLineFX.loop = true;

					oLineFX.ExSetWidth(KCDefine.B_VAL_5_REAL / this.ObjRoot.transform.localScale.x, KCDefine.B_VAL_5_REAL / this.ObjRoot.transform.localScale.y);
					oLineFX.ExSetColor(KDefine.LES_COLOR_GRID_LINE, KDefine.LES_COLOR_GRID_LINE);
					oLineFX.ExSetSortingOrder(KCDefine.U_SORTING_OI_UNDERGROUND);

					oLineFX.ExSetPositions(new List<Vector3>() {
						this.SelGridInfo.m_stPivotPos + new Vector3(j * NSEngine.Access.CellSize.x, (i + KCDefine.B_VAL_1_INT) * -NSEngine.Access.CellSize.y, KCDefine.B_VAL_0_REAL),
						this.SelGridInfo.m_stPivotPos + new Vector3(j * NSEngine.Access.CellSize.x, i * -NSEngine.Access.CellSize.y, KCDefine.B_VAL_0_REAL),
						this.SelGridInfo.m_stPivotPos + new Vector3((j + KCDefine.B_VAL_1_INT) * NSEngine.Access.CellSize.x, i * -NSEngine.Access.CellSize.y, KCDefine.B_VAL_0_REAL),
						this.SelGridInfo.m_stPivotPos + new Vector3((j + KCDefine.B_VAL_1_INT) * NSEngine.Access.CellSize.x, (i + KCDefine.B_VAL_1_INT) * -NSEngine.Access.CellSize.y, KCDefine.B_VAL_0_REAL)
					});

					m_oGridLineFXList.ExAddVal(oLineFX);
				}
			}
			// 그리드 라인 효과를 설정한다 }

			// 객체 스프라이트를 설정한다 {
			m_oObjSpriteInfoLists = new List<STObjSpriteInfo>[this.SelLevelInfo.NumCells.y, this.SelLevelInfo.NumCells.x];

			for(int i = 0; i < this.SelLevelInfo.m_oCellInfoDictContainer.Count; ++i) {
				for(int j = 0; j < this.SelLevelInfo.m_oCellInfoDictContainer[i].Count; ++j) {
					this.SetupObjSprites(this.SelLevelInfo.m_oCellInfoDictContainer[i][j], out List<STObjSpriteInfo> oObjSpriteInfoList);
					m_oObjSpriteInfoLists[this.SelLevelInfo.m_oCellInfoDictContainer[i][j].m_stIdx.y, this.SelLevelInfo.m_oCellInfoDictContainer[i][j].m_stIdx.x] = oObjSpriteInfoList;
				}
			}

            this.SetupSpriteGrid(EObjKinds.NONE, m_oSpriteDict[EKey.SEL_OBJ_SPRITE], Access.GetEditorObjSprite(m_oObjKindsDict[EKey.SEL_OBJ_KINDS], KCDefine.B_PREFIX_LEVEL_EDITOR_SCENE));
			//m_oSpriteDict[EKey.SEL_OBJ_SPRITE]?.ExSetSprite<SpriteRenderer>(Access.GetEditorObjSprite(m_oObjKindsDict[EKey.SEL_OBJ_KINDS], KCDefine.B_PREFIX_LEVEL_EDITOR_SCENE));
            
			// 객체 스프라이트를 설정한다 }

			// 에디터 객체 스프라이트를 설정한다 {
			GameObject.DestroyImmediate(m_oGridBoundsImg);

			var stRect = new Rect(KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_0_REAL, NSEngine.Access.MaxGridSize.x, NSEngine.Access.MaxGridSize.y);
			m_oGridBoundsImg = CFactory.MakeSprite(KCDefine.U_IMG_N_SPRITE, m_oGridBoundsTex2D, stRect, KCDefine.B_ANCHOR_MID_CENTER);

			var oSpriteRenderer = this.EditorObjRoot.GetComponentInChildren<SpriteRenderer>();
			oSpriteRenderer.color = Color.white.ExGetAlphaColor(KCDefine.B_VAL_0_1_REAL);
			oSpriteRenderer.sprite = m_oGridBoundsImg;

			oSpriteRenderer.ExSetSortingOrder(KCDefine.U_SORTING_OI_UNDERGROUND.ExGetExtraOrder(-KCDefine.B_VAL_1_INT));
			// 에디터 객체 스프라이트를 설정한다 }

			// 그리드 라인 버튼을 설정한다 {
			m_oGridLineBtnHList.Clear();
			m_oGridLineBtnVList.Clear();

			for(int i = 0; i < this.SelLevelInfo.NumCells.x; ++i) {
				var stIdx = new Vector3Int(i, KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT);
				var stPos = this.SelGridInfo.m_stPivotPos + stIdx.ExToPos(new Vector3(NSEngine.Access.CellCenterOffset.x, KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_0_REAL), NSEngine.Access.CellSize);
				var stWorldPos = stPos.ExToWorld(this.ObjRoot) + new Vector3(KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_0_REAL, this.PlaneDistance);

				var oBtn = this.SpawnObj<Button>(KDefine.LES_OBJ_N_GRID_LINE_BTN, KDefine.LES_KEY_BTN_OBJS_POOL);
				oBtn.gameObject.ExAddComponent<CBtnHandler>();
				oBtn.ExAddListener(() => this.OnTouchMEUIsGridLineBtnH(stIdx.x));

				(oBtn.transform as RectTransform).pivot = KCDefine.B_ANCHOR_DOWN_CENTER;
				(oBtn.transform as RectTransform).SetAsFirstSibling();

				// 2 차원 투영 일 경우
				if(this.MainCameraProjection == EProjection._2D) {
					(oBtn.transform as RectTransform).anchoredPosition = stWorldPos.ExToCanvas(CSceneManager.ActiveSceneMainCamera, this.UIsCanvas, this.MidEditorUIs) + KDefine.LES_OFFSET_H_GRID_LINE_BTN;
				} else {
					(oBtn.transform as RectTransform).anchoredPosition = stWorldPos.ExToCanvas(CSceneManager.ActiveSceneMainCamera, this.UIsCanvas) + KDefine.LES_OFFSET_H_GRID_LINE_BTN;
				}

				// 텍스트가 존재 할 경우
				if(oBtn.TryGetComponent<Text>(out Text oText)) {
					oText.text = $"{i + KCDefine.B_VAL_1_INT}";
					oText.fontSize = KCDefine.B_VAL_4_INT * KCDefine.B_VAL_7_INT;
				}

				m_oGridLineBtnHList.ExAddVal(oBtn);
			}

			for(int i = 0; i < this.SelLevelInfo.NumCells.y; ++i) {
				var stIdx = new Vector3Int(KCDefine.B_VAL_0_INT, i, KCDefine.B_VAL_0_INT);
				var stPos = this.SelGridInfo.m_stPivotPos + stIdx.ExToPos(new Vector3(KCDefine.B_VAL_0_REAL, NSEngine.Access.CellCenterOffset.y, KCDefine.B_VAL_0_REAL), NSEngine.Access.CellSize);
				var stWorldPos = stPos.ExToWorld(this.ObjRoot) + new Vector3(KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_0_REAL, this.PlaneDistance);

				var oBtn = this.SpawnObj<Button>(KDefine.LES_OBJ_N_GRID_LINE_BTN, KDefine.LES_KEY_BTN_OBJS_POOL);
				oBtn.gameObject.ExAddComponent<CBtnHandler>();
				oBtn.ExAddListener(() => this.OnTouchMEUIsGridLineBtnV(stIdx.y));

				(oBtn.transform as RectTransform).pivot = KCDefine.B_ANCHOR_MID_RIGHT;
				(oBtn.transform as RectTransform).SetAsFirstSibling();

				// 2 차원 투영 일 경우
				if(this.MainCameraProjection == EProjection._2D) {
					(oBtn.transform as RectTransform).anchoredPosition = stWorldPos.ExToCanvas(CSceneManager.ActiveSceneMainCamera, this.UIsCanvas, this.MidEditorUIs) + KDefine.LES_OFFSET_V_GRID_LINE_BTN;
				} else {
					(oBtn.transform as RectTransform).anchoredPosition = stWorldPos.ExToCanvas(CSceneManager.ActiveSceneMainCamera, this.UIsCanvas) + KDefine.LES_OFFSET_V_GRID_LINE_BTN;
				}

				// 텍스트가 존재 할 경우
				if(oBtn.TryGetComponent<Text>(out Text oText)) {
					oText.text = $"{i + KCDefine.B_VAL_1_INT}";
					oText.fontSize = KCDefine.B_VAL_4_INT * KCDefine.B_VAL_7_INT;
				}

				m_oGridLineBtnVList.ExAddVal(oBtn);
			}
			// 그리드 라인 버튼을 설정한다 }

			// 그리드 스크롤 바를 설정한다 {
			m_oScrollBarDict[EKey.ME_UIS_GRID_SCROLL_BAR_H]?.gameObject.SetActive(false);
			m_oScrollBarDict[EKey.ME_UIS_GRID_SCROLL_BAR_V]?.gameObject.SetActive(this.SelLevelInfo.GridPivot == EGridPivot.DOWN);

			// 수평 그리드 스크롤 바가 존재 할 경우
			if(m_oScrollBarDict.TryGetValue(EKey.ME_UIS_GRID_SCROLL_BAR_H, out Scrollbar oGridScrollBarH)) {
				oGridScrollBarH.size = this.SelGridInfo.m_stViewBounds.size.x / this.SelGridInfo.m_stBounds.size.x;

				(oGridScrollBarH.transform as RectTransform).pivot = KCDefine.B_ANCHOR_UP_CENTER;
				(oGridScrollBarH.transform as RectTransform).sizeDelta = new Vector3(m_oGridBoundsImg.bounds.size.x * this.EditorObjRoot.transform.localScale.x, (oGridScrollBarH.transform as RectTransform).sizeDelta.y, KCDefine.B_VAL_0_REAL);
				(oGridScrollBarH.transform as RectTransform).anchoredPosition = new Vector3(KCDefine.B_VAL_0_REAL, ((m_oGridBoundsImg.bounds.size.y / -KCDefine.B_VAL_2_REAL) * this.EditorObjRoot.transform.localScale.y) - ((oGridScrollBarH.transform as RectTransform).sizeDelta.y * KCDefine.B_VAL_2_REAL), KCDefine.B_VAL_0_REAL);
			}

			// 수직 그리드 스크롤 바가 존재 할 경우
			if(m_oScrollBarDict.TryGetValue(EKey.ME_UIS_GRID_SCROLL_BAR_V, out Scrollbar oGridScrollBarV)) {
				oGridScrollBarV.size = this.SelGridInfo.m_stViewBounds.size.y / this.SelGridInfo.m_stBounds.size.y;

				(oGridScrollBarV.transform as RectTransform).pivot = KCDefine.B_ANCHOR_MID_LEFT;
				(oGridScrollBarV.transform as RectTransform).sizeDelta = new Vector3((oGridScrollBarV.transform as RectTransform).sizeDelta.x, m_oGridBoundsImg.bounds.size.y * this.EditorObjRoot.transform.localScale.y, KCDefine.B_VAL_0_REAL);
				(oGridScrollBarV.transform as RectTransform).anchoredPosition = new Vector3(((m_oGridBoundsImg.bounds.size.x / KCDefine.B_VAL_2_REAL) * this.EditorObjRoot.transform.localScale.x) + ((oGridScrollBarV.transform as RectTransform).sizeDelta.x * KCDefine.B_VAL_2_REAL), KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_0_REAL);
			}
			// 그리드 스크롤 바를 설정한다 }
		}

		/** 객체 스프라이트를 리셋한다 */
		private void ResetObjSprites(List<STObjSpriteInfo> a_oObjSpriteInfoList) {
			for(int i = 0; i < a_oObjSpriteInfoList.Count; ++i) {
				a_oObjSpriteInfoList[i].m_oSprite.sprite = null;
				this.DespawnObj(KDefine.LES_KEY_SPRITE_OBJS_POOL, a_oObjSpriteInfoList[i].m_oSprite.gameObject);
			}
		}

		/** 객체 스프라이트를 설정한다 */
		private void SetupObjSprites(STCellInfo a_stCellInfo, out List<STObjSpriteInfo> a_oOutObjSpriteInfoList) {
			a_oOutObjSpriteInfoList = new List<STObjSpriteInfo>();

			for(int i = 0; i < a_stCellInfo.m_oCellObjInfoList.Count; ++i) {
				var oObjSprite = this.SpawnObj<SpriteRenderer>(KDefine.LES_OBJ_N_OBJ_SPRITE, KDefine.LES_KEY_SPRITE_OBJS_POOL);
				this.SetupObjSprite(a_stCellInfo, a_stCellInfo.m_oCellObjInfoList[i], oObjSprite);

				a_oOutObjSpriteInfoList.ExAddVal(new STObjSpriteInfo() {
					m_eObjKinds = a_stCellInfo.m_oCellObjInfoList[i].ObjKinds, m_oSprite = oObjSprite
				});
			}
		}

		/** 객체 스프라이트를 설정한다 */
		private void SetupObjSprite(STCellInfo a_stCellInfo, STCellObjInfo a_stCellObjInfo, SpriteRenderer a_oOutObjSprite) {
			this.SetupSpriteGrid(a_stCellObjInfo.ObjKinds, a_oOutObjSprite, Access.GetEditorObjSprite(a_stCellObjInfo.ObjKinds, KCDefine.B_PREFIX_LEVEL_EDITOR_SCENE));
            //a_oOutObjSprite.sprite = Access.GetEditorObjSprite(a_stCellObjInfo.ObjKinds, KCDefine.B_PREFIX_LEVEL_EDITOR_SCENE);
			a_oOutObjSprite.transform.localPosition = this.SelGridInfo.m_stPivotPos + a_stCellInfo.m_stIdx.ExToPos(NSEngine.Access.CellCenterOffset, NSEngine.Access.CellSize);

			a_oOutObjSprite.ExSetSortingOrder(NSEngine.Access.GetSortingOrderInfo(a_stCellObjInfo.ObjKinds));
			this.SubSetupObjSprite(a_stCellInfo, a_stCellObjInfo, a_oOutObjSprite);
		}

		/** UI 상태를 갱신한다 */
		private void UpdateUIsState() {
			this.ResetObjSprites();

			this.UpdateMidEditorUIsState();
			this.UpdateLeftEditorUIsState();
			this.UpdateRightEditorUIsState();

			this.SubUpdateUIsState();

			// 레이아웃을 재배치한다
			this.RebuildLayouts(this.MEUIsInfoUIs);
			this.RebuildLayouts(this.MEUIsEditorModeUIs);
		}

		/** 에디터 리셋 팝업 결과를 수신했을 경우 */
		private void OnReceiveEditorResetPopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				CLevelInfoTable.Inst.LevelInfoDictContainer.Clear();
				CLevelInfoTable.Inst.LoadLevelInfos();

				// 레벨 정보가 없을 경우
				if(!CLevelInfoTable.Inst.LevelInfoDictContainer.ExIsValid()) {
					var oLevelInfo = Factory.MakeEditorLevelInfo(KCDefine.B_VAL_0_INT);
					CLevelInfoTable.Inst.AddLevelInfo(oLevelInfo);

					Func.SetupEditorLevelInfo(oLevelInfo, new CSubEditorCreateInfo() {
						m_nNumLevels = KCDefine.B_VAL_0_INT,
						m_stMinNumCells = NSEngine.KDefine.E_MIN_NUM_CELLS,
						m_stMaxNumCells = NSEngine.KDefine.E_MIN_NUM_CELLS
					});
				}

				this.SetSelLevelInfo(CLevelInfoTable.Inst.GetLevelInfo(KCDefine.B_VAL_0_INT)); ;
				this.UpdateUIsState();
			}
		}

		/** 에디터 세트 팝업 결과를 수신했을 경우 */
		private void OnReceiveEditorSetPopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				CCommonUserInfoStorage.Inst.UserInfo.UserType = m_oUserTypeDict[EKey.SEL_USER_TYPE];
				CCommonUserInfoStorage.Inst.SaveUserInfo();

#if GOOGLE_SHEET_ENABLE
				m_oTableSrcDict[EKey.SEL_TABLE_SRC] = ETableSrc.REMOTE;
#else
				m_oTableSrcDict[EKey.SEL_TABLE_SRC] = ETableSrc.LOCAL;
#endif // #if GOOGLE_SHEET_ENABLE

				this.OnReceiveEditorResetPopupResult(null, true);
				this.OnReceiveEditorTableLoadPopupResult(null, true);
			}
		}

		/** 에디터 세트 테이블 로드 팝업 결과를 수신했을 경우 */
		private void OnReceiveEditorTableLoadPopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				switch(m_oTableSrcDict[EKey.SEL_TABLE_SRC]) {
					case ETableSrc.LOCAL: {
						CObjInfoTable.Inst.LoadObjInfos();
						CEpisodeInfoTable.Inst.LoadEpisodeInfos();

						this.UpdateUIsState();
						break;
					}
					case ETableSrc.REMOTE: {
#if GOOGLE_SHEET_ENABLE
						string oKey = KCDefine.U_TABLE_P_G_VER_INFO.ExGetFileName(false);
						Func.LoadVerInfoGoogleSheet(KDefine.G_TABLE_INFO_GOOGLE_SHEET_DICT.GetValueOrDefault(oKey).m_oID, this.OnLoadVerInfoGoogleSheet);
#endif // #if GOOGLE_SHEET_ENABLE

						break;
					}
				}
			}
		}

		/** 에디터 제거 팝업 결과를 수신했을 경우 */
		private void OnReceiveEditorRemovePopupResult(CAlertPopup a_oSender, STIDInfo a_stIDInfo, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				this.RemoveLevelInfos(m_oScrollerDict[EKey.SEL_SCROLLER], a_stIDInfo);
				this.UpdateUIsState();
			}
		}

		/** 에디터 입력 팝업 결과를 수신했을 경우 */
		private void OnReceiveEditorInputPopupResult(CEditorInputPopup a_oSender, string a_oStr, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				switch(m_oInputPopupDict[EKey.SEL_INPUT_POPUP]) {
					case EInputPopup.MOVE_LEVEL: this.HandleMoveLevelEditorInputPopupResult(a_oSender, a_oStr); break;
					case EInputPopup.REMOVE_LEVEL: this.HandleRemoveLevelEditorInputPopupResult(a_oSender, a_oStr); break;
				}
			}

			this.UpdateUIsState();
		}

		/** 에디터 생성 팝업 결과를 수신했을 경우 */
		private void OnReceiveEditorCreatePopupResult(CEditorCreatePopup a_oSender, CEditorCreateInfo a_oCreateInfo, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(this.SelLevelInfo.m_stIDInfo.m_nID02, this.SelLevelInfo.m_stIDInfo.m_nID03);
				int nNumCreateLevelInfos = (nNumLevelInfos + a_oCreateInfo.m_nNumLevels < KCDefine.U_MAX_NUM_LEVEL_INFOS) ? a_oCreateInfo.m_nNumLevels : KCDefine.U_MAX_NUM_LEVEL_INFOS - nNumLevelInfos;

				for(int i = 0; i < nNumCreateLevelInfos; ++i) {
					this.SetSelLevelInfo(Factory.MakeEditorLevelInfo(i + nNumLevelInfos, this.SelLevelInfo.m_stIDInfo.m_nID02, this.SelLevelInfo.m_stIDInfo.m_nID03));
					CLevelInfoTable.Inst.AddLevelInfo(this.SelLevelInfo);

					Func.SetupEditorLevelInfo(this.SelLevelInfo, a_oCreateInfo);
				}

				this.UpdateUIsState();
			}
		}

		/** 셀 객체 정보를 추가한다 */
		private void AddCellObjInfo(STCellObjInfo a_stCellObjInfo, Vector3Int a_stIdx, bool a_bIsReplace = true, bool a_bIsEnableOverlay = false) {
			// 셀 객체 정보 추가가 가능 할 경우
			if(this.IsEnableAddCellObjInfo(a_stIdx, a_stCellObjInfo.m_stSize, a_stCellObjInfo.ObjKinds, a_bIsReplace, a_bIsEnableOverlay)) {
				for(int i = 0; i < a_stCellObjInfo.m_stSize.y; ++i) {
					for(int j = 0; j < a_stCellObjInfo.m_stSize.x; ++j) {
						var stIdx = new Vector3Int(a_stIdx.x + j, a_stIdx.y + i, a_stIdx.z);
						var stCellInfo = this.SelLevelInfo.GetCellInfo(stIdx);

						// 기본 인덱스 일 경우
						if(stIdx.Equals(a_stIdx)) {
							int nIdx = stCellInfo.m_oCellObjInfoList.FindIndex((a_stFindCellObjInfo) => a_stFindCellObjInfo.ObjKinds == a_stCellObjInfo.ObjKinds);

							// 셀 객체 정보가 존재 할 경우
							if(stCellInfo.m_oCellObjInfoList.ExIsValidIdx(nIdx) || (!a_bIsEnableOverlay && Input.GetKey(KeyCode.LeftShift))) {
								this.RemoveAllCellObjInfos(stCellInfo.m_oCellObjInfoList.ExIsValidIdx(nIdx) ? a_stCellObjInfo.ObjKinds : EObjKinds.NONE, a_stIdx);
							}

							#region 추가
							a_stCellObjInfo.HP = int.Parse(m_oSubInputDict[ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_HP_INPUT].text);
							a_stCellObjInfo.ATK = int.Parse(m_oSubInputDict[ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_ATK_INPUT].text);
							#endregion // 추가

							stCellInfo.m_oCellObjInfoList.ExAddVal(a_stCellObjInfo);
						} else {
							stCellInfo.m_oCellObjInfoList.ExAddVal(Factory.MakeEditorCellObjInfo(EObjKinds.BG_PLACEHOLDER_01, Vector3Int.one, a_stIdx, drawCellColor));
						}
					}
				}
			}
		}

		/** 레벨 정보를 추가한다 */
		private void AddLevelInfo(int a_nLevelID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
			this.SetSelLevelInfo(Factory.MakeEditorLevelInfo(a_nLevelID, a_nStageID, a_nChapterID));
			CLevelInfoTable.Inst.AddLevelInfo(this.SelLevelInfo);

			Func.SetupEditorLevelInfo(this.SelLevelInfo, new CSubEditorCreateInfo() {
				m_nNumLevels = KCDefine.B_VAL_0_INT,
				m_stMinNumCells = NSEngine.KDefine.E_MIN_NUM_CELLS,
				m_stMaxNumCells = NSEngine.KDefine.E_MIN_NUM_CELLS
			});

			this.UpdateUIsState();
		}

		/** 셀 객체 정보를 제거한다 */
		private void RemoveCellObjInfo(EObjKinds a_eObjKinds, Vector3Int a_stIdx) {
			// 셀 객체 정보 제거가 가능 할 경우
			if(this.IsEnableRemoveCellObjInfo(a_eObjKinds, a_stIdx) && this.TryGetCellObjInfo(a_stIdx, a_eObjKinds, out STCellObjInfo stCellObjInfo)) {
				for(int i = 0; i < stCellObjInfo.m_stSize.y; ++i) {
					for(int j = 0; j < stCellObjInfo.m_stSize.x; ++j) {
						var stIdx = new Vector3Int(a_stIdx.x + j, a_stIdx.y + i, a_stIdx.z);

						// 기본 인덱스 일 경우
						if(stIdx.Equals(a_stIdx)) {
							this.SelLevelInfo.GetCellInfo(stIdx).m_oCellObjInfoList.ExRemoveVal((a_stCellObjInfo) => a_stCellObjInfo.ObjKinds == stCellObjInfo.ObjKinds);
						} else {
							this.SelLevelInfo.GetCellInfo(stIdx).m_oCellObjInfoList.ExRemoveVal((a_stCellObjInfo) => a_stCellObjInfo.ObjKinds == EObjKinds.BG_PLACEHOLDER_01);
						}
					}
				}
			}
		}

		/** 모든 셀 객체 정보를 제거한다 */
		private void RemoveAllCellObjInfos(EObjKinds a_eObjKinds, Vector3Int a_stIdx) {
			while(this.IsEnableRemoveCellObjInfo(a_eObjKinds, a_stIdx)) {
				this.RemoveCellObjInfo(a_eObjKinds, a_stIdx);
			}
		}

		/** 레벨 정보를 제거한다 */
		private void RemoveLevelInfos(EnhancedScroller a_oScroller, STIDInfo a_stIDInfo) {
			// 레벨 스크롤러 일 경우
			if(m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].m_oScroller == a_oScroller) {
				CLevelInfoTable.Inst.RemoveLevelInfo(a_stIDInfo.m_nID01, a_stIDInfo.m_nID02, a_stIDInfo.m_nID03);
			}
			// 스테이지 스크롤러 일 경우
			else if(m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].m_oScroller == a_oScroller) {
				CLevelInfoTable.Inst.RemoveStageLevelInfos(a_stIDInfo.m_nID02, a_stIDInfo.m_nID03);
			}
			// 챕터 스크롤러 일 경우
			else if(m_oScrollerInfoDict[EKey.LE_UIS_CHAPTER_SCROLLER_INFO].m_oScroller == a_oScroller) {
				CLevelInfoTable.Inst.RemoveChapterLevelInfos(a_stIDInfo.m_nID03);
			}

			// 레벨 정보가 없을 경우
			if(!CLevelInfoTable.Inst.LevelInfoDictContainer.ExIsValid()) {
				this.SetSelLevelInfo(Factory.MakeEditorLevelInfo(KCDefine.B_VAL_0_INT));
				CLevelInfoTable.Inst.AddLevelInfo(this.SelLevelInfo);

				Func.SetupEditorLevelInfo(this.SelLevelInfo, new CSubEditorCreateInfo() {
					m_nNumLevels = KCDefine.B_VAL_0_INT,
					m_stMinNumCells = NSEngine.KDefine.E_MIN_NUM_CELLS,
					m_stMaxNumCells = NSEngine.KDefine.E_MIN_NUM_CELLS
				});
			} else {
				CLevelInfo oLevelInfo = null;

				// 레벨 스크롤러 일 경우
				if(m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].m_oScroller == a_oScroller) {
					var stPrevIDInfo = new STIDInfo(a_stIDInfo.m_nID01 - KCDefine.B_VAL_1_INT, a_stIDInfo.m_nID02, a_stIDInfo.m_nID03);
					var stNextIDInfo = new STIDInfo(a_stIDInfo.m_nID01, a_stIDInfo.m_nID02, a_stIDInfo.m_nID03);

					this.TryGetLevelInfo(stPrevIDInfo, stNextIDInfo, out oLevelInfo);
				}

				// 스테이지 스크롤러 일 경우
				if(oLevelInfo == null || m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].m_oScroller == a_oScroller) {
					var stPrevIDInfo = new STIDInfo(KCDefine.B_VAL_0_INT, a_stIDInfo.m_nID02 - KCDefine.B_VAL_1_INT, a_stIDInfo.m_nID03);
					var stNextIDInfo = new STIDInfo(KCDefine.B_VAL_0_INT, a_stIDInfo.m_nID02, a_stIDInfo.m_nID03);

					this.TryGetLevelInfo(stPrevIDInfo, stNextIDInfo, out oLevelInfo);
				}

				// 챕터 스크롤러 일 경우
				if(oLevelInfo == null || m_oScrollerInfoDict[EKey.LE_UIS_CHAPTER_SCROLLER_INFO].m_oScroller == a_oScroller) {
					var stPrevIDInfo = new STIDInfo(KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, a_stIDInfo.m_nID03 - KCDefine.B_VAL_1_INT);
					var stNextIDInfo = new STIDInfo(KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, a_stIDInfo.m_nID03);

					this.TryGetLevelInfo(stPrevIDInfo, stNextIDInfo, out oLevelInfo);
				}

				this.SetSelLevelInfo(oLevelInfo);
			}

			this.UpdateUIsState();
		}

		/** 레벨 정보를 복사한다 */
		private void CopyLevelInfos(EnhancedScroller a_oScroller, STIDInfo a_stIDInfo) {
			// 레벨 스크롤러 일 경우
			if(m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].m_oScroller == a_oScroller) {
				this.SetSelLevelInfo(CLevelInfoTable.Inst.GetLevelInfo(a_stIDInfo.m_nID01, a_stIDInfo.m_nID02, a_stIDInfo.m_nID03).Clone() as CLevelInfo);
				m_oLevelInfoDict[EKey.SEL_LEVEL_INFO].m_stIDInfo.m_nID01 = CLevelInfoTable.Inst.GetNumLevelInfos(a_stIDInfo.m_nID02, a_stIDInfo.m_nID03);

				CLevelInfoTable.Inst.AddLevelInfo(this.SelLevelInfo);
			} else {
				// 스테이지 스크롤러 일 경우
				if(m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].m_oScroller == a_oScroller) {
					int nNumStageInfos = CLevelInfoTable.Inst.GetNumStageInfos(a_stIDInfo.m_nID03);
					var oStageLevelInfoDict = CLevelInfoTable.Inst.GetStageLevelInfos(a_stIDInfo.m_nID02, a_stIDInfo.m_nID03);

					for(int i = 0; i < oStageLevelInfoDict.Count; ++i) {
						var oCloneLevelInfo = oStageLevelInfoDict[i].Clone() as CLevelInfo;
						oCloneLevelInfo.m_stIDInfo.m_nID02 = nNumStageInfos;

						CLevelInfoTable.Inst.AddLevelInfo(oCloneLevelInfo);
					}
				}
				// 챕터 스크롤러 일 경우
				else if(m_oScrollerInfoDict[EKey.LE_UIS_CHAPTER_SCROLLER_INFO].m_oScroller == a_oScroller) {
					int nNumChapterInfos = CLevelInfoTable.Inst.NumChapterInfos;
					var oChapterLevelInfoDictContainer = CLevelInfoTable.Inst.GetChapterLevelInfos(a_stIDInfo.m_nID03);

					for(int i = 0; i < oChapterLevelInfoDictContainer.Count; ++i) {
						for(int j = 0; j < oChapterLevelInfoDictContainer[i].Count; ++j) {
							var oCloneLevelInfo = oChapterLevelInfoDictContainer[i][j].Clone() as CLevelInfo;
							oCloneLevelInfo.m_stIDInfo.m_nID03 = nNumChapterInfos;

							CLevelInfoTable.Inst.AddLevelInfo(oCloneLevelInfo);
						}
					}
				}

				int nID = KCDefine.B_VAL_0_INT;
				int nStageID = (m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].m_oScroller == a_oScroller) ? CLevelInfoTable.Inst.GetNumStageInfos(a_stIDInfo.m_nID03) - KCDefine.B_VAL_1_INT : KCDefine.B_VAL_0_INT;
				int nChapterID = (m_oScrollerInfoDict[EKey.LE_UIS_CHAPTER_SCROLLER_INFO].m_oScroller == a_oScroller) ? CLevelInfoTable.Inst.NumChapterInfos - KCDefine.B_VAL_1_INT : a_stIDInfo.m_nID03;

				this.SetSelLevelInfo(CLevelInfoTable.Inst.GetLevelInfo(nID, nStageID, nChapterID));
			}

			this.SetSelLevelInfo(this.SelLevelInfo ?? CLevelInfoTable.Inst.GetLevelInfo(KCDefine.B_VAL_0_INT));
			this.UpdateUIsState();
		}

		/** 레벨 정보를 이동한다 */
		private void MoveLevelInfos(EnhancedScroller a_oScroller, STIDInfo a_stIDInfo, int a_nDestID) {
			// 레벨 스크롤러 일 경우
			if(m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].m_oScroller == a_oScroller) {
				int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(a_stIDInfo.m_nID02, a_stIDInfo.m_nID03);
				CLevelInfoTable.Inst.MoveLevelInfo(a_stIDInfo.m_nID01, Mathf.Clamp(a_nDestID, KCDefine.B_VAL_1_INT, nNumLevelInfos) - KCDefine.B_VAL_1_INT, a_stIDInfo.m_nID02, a_stIDInfo.m_nID03);
			}
			// 스테이지 스크롤러 일 경우
			else if(m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].m_oScroller == a_oScroller) {
				int nNumStageInfos = CLevelInfoTable.Inst.GetNumStageInfos(a_stIDInfo.m_nID03);
				CLevelInfoTable.Inst.MoveStageLevelInfos(a_stIDInfo.m_nID02, Mathf.Clamp(a_nDestID, KCDefine.B_VAL_1_INT, nNumStageInfos) - KCDefine.B_VAL_1_INT, a_stIDInfo.m_nID03);
			}
			// 챕터 스크롤러 일 경우
			else if(m_oScrollerInfoDict[EKey.LE_UIS_CHAPTER_SCROLLER_INFO].m_oScroller == a_oScroller) {
				int nNumChapterInfos = CLevelInfoTable.Inst.NumChapterInfos;
				CLevelInfoTable.Inst.MoveChapterLevelInfos(a_stIDInfo.m_nID03, Mathf.Clamp(a_nDestID, KCDefine.B_VAL_1_INT, nNumChapterInfos) - KCDefine.B_VAL_1_INT);
			}
		}

		/** 알림을 출력한다 */
		private void ShowNoti(string a_oMsg) {
			this.CloseNoti();
			this.MEUIsMsgUIs?.SetActive(true);

			m_oTextDict[EKey.ME_UIS_MSG_TEXT]?.ExSetText<Text>(a_oMsg, false);
			CScheduleManager.Inst.AddTimer(this, KCDefine.B_VAL_5_REAL, KCDefine.B_VAL_1_INT, () => this.CloseNoti());
		}

		/** 알림을 닫는다 */
		private void CloseNoti() {
			this.MEUIsMsgUIs?.SetActive(false);
			CScheduleManager.Inst.RemoveTimer(this);
		}

		/** 레벨 정보를 저장한다 */
		private void SaveLevelInfos() {
			this.ShowNoti(KDefine.ES_MSG_NOTI_SAVE);

			this.ExLateCallFunc((a_oSender) => {
				this.CloseNoti();
				CLevelInfoTable.Inst.SaveLevelInfos();
			}, KCDefine.B_VAL_0_REAL, true);
		}

		/** 에디터 레벨 이동 에디터 입력 팝업 결과를 처리한다 */
		private void HandleMoveLevelEditorInputPopupResult(CEditorInputPopup a_oSender, string a_oStr) {
			// 식별자가 유효 할 경우
			if(int.TryParse(a_oStr, NumberStyles.Any, null, out int nID)) {
				this.MoveLevelInfos(m_oScrollerDict[EKey.SEL_SCROLLER], this.SelLevelInfo.m_stIDInfo, nID);
			}
		}

		/** 에디터 레벨 제거 에디터 입력 팝업 결과를 처리한다 */
		private void HandleRemoveLevelEditorInputPopupResult(CEditorInputPopup a_oSender, string a_oStr) {
			var oTokenList = a_oStr.Split(KCDefine.B_TOKEN_DASH).ToList();
			int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(this.SelLevelInfo.m_stIDInfo.m_nID02, this.SelLevelInfo.m_stIDInfo.m_nID03);

			// 식별자가 유효 할 경우
			if(oTokenList.Count > KCDefine.B_VAL_1_INT && (int.TryParse(oTokenList[KCDefine.B_VAL_0_INT], NumberStyles.Any, null, out int nMinID) && int.TryParse(oTokenList[KCDefine.B_VAL_1_INT], NumberStyles.Any, null, out int nMaxID))) {
				nMinID = Mathf.Clamp(nMinID, KCDefine.B_VAL_1_INT, nNumLevelInfos);
				nMaxID = Mathf.Clamp(nMaxID, KCDefine.B_VAL_1_INT, nNumLevelInfos);

				CFunc.LessCorrectSwap(ref nMinID, ref nMaxID);
				var stIDInfo = new STIDInfo(nMinID - KCDefine.B_VAL_1_INT, this.SelLevelInfo.m_stIDInfo.m_nID02, this.SelLevelInfo.m_stIDInfo.m_nID03);

				for(int i = 0; i <= nMaxID - nMinID; ++i) {
					// 레벨 정보가 존재 할 경우
					if(CLevelInfoTable.Inst.TryGetLevelInfo(stIDInfo.m_nID01, out CLevelInfo oLevelInfo, stIDInfo.m_nID02, stIDInfo.m_nID03)) {
						this.RemoveLevelInfos(m_oScrollerDict[EKey.SEL_SCROLLER], stIDInfo);
					}
				}
			}
		}

#if GOOGLE_SHEET_ENABLE
		/** 구글 시트가 로드 되었을 경우 */
		private void OnLoadGoogleSheet(CServicesManager a_oSender, STGoogleSheetLoadInfo a_stGoogleSheetLoadInfo, Dictionary<string, SimpleJSON.JSONNode> a_oJSONNodeInfoDict, bool a_bIsSuccess) {
			// 로드 되었을 경우
			if(a_bIsSuccess) {
				var oHandlerDict = new Dictionary<string, System.Action>() {
					[KCDefine.U_TABLE_P_G_ETC_INFO.ExGetFileName(false)] = () => CEtcInfoTable.Inst.SaveEtcInfos(a_oJSONNodeInfoDict.ExToJSONNode().ToString()),
					[KCDefine.U_TABLE_P_G_MISSION_INFO.ExGetFileName(false)] = () => CMissionInfoTable.Inst.SaveMissionInfos(a_oJSONNodeInfoDict.ExToJSONNode().ToString()),
					[KCDefine.U_TABLE_P_G_REWARD_INFO.ExGetFileName(false)] = () => CRewardInfoTable.Inst.SaveRewardInfos(a_oJSONNodeInfoDict.ExToJSONNode().ToString()),
					[KCDefine.U_TABLE_P_G_RES_INFO.ExGetFileName(false)] = () => CResInfoTable.Inst.SaveResInfos(a_oJSONNodeInfoDict.ExToJSONNode().ToString()),
					[KCDefine.U_TABLE_P_G_ITEM_INFO.ExGetFileName(false)] = () => CItemInfoTable.Inst.SaveItemInfos(a_oJSONNodeInfoDict.ExToJSONNode().ToString()),
					[KCDefine.U_TABLE_P_G_SKILL_INFO.ExGetFileName(false)] = () => CSkillInfoTable.Inst.SaveSkillInfos(a_oJSONNodeInfoDict.ExToJSONNode().ToString()),
					[KCDefine.U_TABLE_P_G_OBJ_INFO.ExGetFileName(false)] = () => CObjInfoTable.Inst.SaveObjInfos(a_oJSONNodeInfoDict.ExToJSONNode().ToString()),
					[KCDefine.U_TABLE_P_G_ABILITY_INFO.ExGetFileName(false)] = () => CAbilityInfoTable.Inst.SaveAbilityInfos(a_oJSONNodeInfoDict.ExToJSONNode().ToString()),
					[KCDefine.U_TABLE_P_G_PRODUCT_INFO.ExGetFileName(false)] = () => CProductTradeInfoTable.Inst.SaveProductTradeInfos(a_oJSONNodeInfoDict.ExToJSONNode().ToString())
				};

				oHandlerDict.GetValueOrDefault(a_stGoogleSheetLoadInfo.m_oSheetName)?.Invoke();
			}
		}

		/** 버전 정보 구글 시트를 로드했을 경우 */
		private void OnLoadVerInfoGoogleSheet(CServicesManager a_oSender, SimpleJSON.JSONNode a_oVerInfos, Dictionary<string, STLoadGoogleSheetInfo> a_oLoadGoogleSheetInfoDict, bool a_bIsSuccess) {
			// 로드 되었을 경우
			if(a_bIsSuccess) {
				m_oVerInfos = a_oVerInfos;
				Func.LoadGoogleSheets(a_oLoadGoogleSheetInfoDict.Values.ToList(), m_oGoogleSheetLoadHandlerDict, this.OnLoadGoogleSheets);
			} else {
				Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_C_ON_TABLE_LOAD_FAIL_MSG), null, false);
			}
		}

		/** 구글 시트를 로드했을 경우 */
		private void OnLoadGoogleSheets(CServicesManager a_oSender, bool a_bIsSuccess) {
			// 로드 되었을 경우
			if(a_bIsSuccess) {
				this.UpdateUIsState();
				Func.OnLoadGoogleSheets(m_oVerInfos);

				Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_C_ON_TABLE_LOAD_MSG), null, false);
			} else {
				Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_C_ON_TABLE_LOAD_FAIL_MSG), null, false);
			}
		}
#endif // #if GOOGLE_SHEET_ENABLE
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		#endregion // 조건부 함수

		#region 조건부 접근자 함수
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		/** 셀 정보 포함 여부를 검사한다 */
		private bool IsContainsCellInfo(Vector3Int a_stIdx, Vector3Int a_stSize) {
			for(int i = 0; i < a_stSize.y; ++i) {
				for(int j = 0; j < a_stSize.x; ++j) {
					var stIdx = new Vector3Int(a_stIdx.x + j, a_stIdx.y + i, a_stIdx.z);

					// 인덱스가 유효 할 경우
					if(!this.SelLevelInfo.m_oCellInfoDictContainer.ExIsValidIdx(stIdx)) {
						return false;
					}
				}
			}

			return true;
		}

		/** 모든 셀 이동 가능 여부를 검사한다 */
		private bool IsEnableMoveAllCells(EDirection a_eDirection) {
			var stIdx = this.GetGridBaseIdx(a_eDirection);
			int nNumCells = a_eDirection.ExIsVertical() ? this.SelLevelInfo.NumCells.x : this.SelLevelInfo.NumCells.y;

			for(int i = 0; i < nNumCells; ++i) {
				var stCellIdx = a_eDirection.ExIsVertical() ? new Vector3Int(stIdx.x + i, stIdx.y, KCDefine.B_VAL_0_INT) : new Vector3Int(stIdx.x, stIdx.y + i, KCDefine.B_VAL_0_INT);

				// 셀 객체 정보가 존재 할 경우
				if(this.SelLevelInfo.m_oCellInfoDictContainer[stCellIdx.y][stCellIdx.x].m_oCellObjInfoList.ExIsValid()) {
					return false;
				}
			}

			return true;
		}

		/** 셀 객체 추가 가능 여부를 검사한다 */
		private bool IsEnableAddCellObjInfo(Vector3Int a_stIdx, Vector3Int a_stSize, EObjKinds a_eObjKinds, bool a_bIsReplace = true, bool a_bIsEnableOverlay = false) {
			// 셀 정보가 존재 할 경우
			if(this.IsContainsCellInfo(a_stIdx, a_stSize)) {
				for(int i = 0; i < a_stSize.y; ++i) {
					for(int j = 0; j < a_stSize.x; ++j) {
						var stIdx = new Vector3Int(a_stIdx.x + j, a_stIdx.y + i, a_stIdx.z);
						this.SelLevelInfo.TryGetCellInfo(stIdx, out STCellInfo stExtraCellInfo);

						int nIdx = stExtraCellInfo.m_oCellObjInfoList.FindIndex((a_stCellObjInfo) => !stIdx.Equals(a_stIdx) && !a_stCellObjInfo.m_stBaseIdx.Equals(a_stIdx));

						// 셀 객체 추가가 불가능 할 경우
						if(!a_bIsEnableOverlay && stExtraCellInfo.m_oCellObjInfoList.ExIsValidIdx(nIdx)) {
							return false;
						}
					}
				}

				this.SelLevelInfo.TryGetCellInfo(a_stIdx, out STCellInfo stCellInfo);

				int nIdx01 = stCellInfo.m_oCellObjInfoList.FindIndex((a_stCellObjInfo) => a_stCellObjInfo.ObjKinds == EObjKinds.BG_PLACEHOLDER_01);
				int nIdx02 = stCellInfo.m_oCellObjInfoList.FindIndex((a_stCellObjInfo) => a_stCellObjInfo.ObjKinds == a_eObjKinds && !a_bIsReplace);
				int nIdx03 = stCellInfo.m_oCellObjInfoList.FindIndex((a_stCellObjInfo) => a_stCellObjInfo.ObjKinds != a_eObjKinds && !Input.GetKey(KeyCode.LeftShift));

				return a_bIsEnableOverlay || (!stCellInfo.m_oCellObjInfoList.ExIsValidIdx(nIdx01) && !stCellInfo.m_oCellObjInfoList.ExIsValidIdx(nIdx02) && !stCellInfo.m_oCellObjInfoList.ExIsValidIdx(nIdx03));
			}

			return false;
		}

		/** 셀 객체 제거 가능 여부를 검사한다 */
		private bool IsEnableRemoveCellObjInfo(EObjKinds a_eObjKinds, Vector3Int a_stIdx) {
			bool bIsValid = this.TryGetCellObjInfo(a_stIdx, a_eObjKinds, out STCellObjInfo stCellObjInfo);
			return this.IsContainsCellInfo(a_stIdx, stCellObjInfo.m_stSize) && (stCellObjInfo.ObjKinds != EObjKinds.NONE && stCellObjInfo.ObjKinds != EObjKinds.BG_PLACEHOLDER_01);
		}

		/** 에디터 객체 크기를 반환한다 */
		private Vector3Int GetEditorObjSize() {
			bool bIsValid01 = int.TryParse(m_oInputDict[EKey.RE_UIS_PAGE_UIS_02_OBJ_SIZE_X_INPUT]?.text, NumberStyles.Any, null, out int nSizeX);
			bool bIsValid02 = int.TryParse(m_oInputDict[EKey.RE_UIS_PAGE_UIS_02_OBJ_SIZE_Y_INPUT]?.text, NumberStyles.Any, null, out int nSizeY);

			return new Vector3Int(bIsValid01 ? nSizeX : KCDefine.B_VAL_1_INT, bIsValid02 ? nSizeY : KCDefine.B_VAL_1_INT, KCDefine.B_VAL_1_INT);
		}

		/** 셀 객체 정보를 반환한다 */
		private bool TryGetCellObjInfo(Vector3Int a_stIdx, EObjKinds a_eObjKinds, out STCellObjInfo a_stOutCellObjInfo) {
			this.SelLevelInfo.TryGetCellInfo(a_stIdx, out STCellInfo stCellInfo);
			a_stOutCellObjInfo = (a_eObjKinds != EObjKinds.NONE) ? stCellInfo.m_oCellObjInfoList.ExGetVal((a_stCellObjInfo) => a_stCellObjInfo.ObjKinds == a_eObjKinds, STCellObjInfo.INVALID) : stCellInfo.m_oCellObjInfoList.ExGetVal(stCellInfo.m_oCellObjInfoList.Count - KCDefine.B_VAL_1_INT, STCellObjInfo.INVALID);

			return !a_stOutCellObjInfo.Equals(STCellObjInfo.INVALID);
		}

		/** 레벨 정보를 반환한다 */
		private bool TryGetLevelInfo(STIDInfo a_stPrevIDInfo, STIDInfo a_stNextIDInfo, out CLevelInfo a_oOutLevelInfo) {
			CLevelInfoTable.Inst.TryGetLevelInfo(a_stPrevIDInfo.m_nID01, out CLevelInfo oPrevLevelInfo, a_stPrevIDInfo.m_nID02, a_stPrevIDInfo.m_nID03);
			CLevelInfoTable.Inst.TryGetLevelInfo(a_stNextIDInfo.m_nID01, out CLevelInfo oNextLevelInfo, a_stNextIDInfo.m_nID02, a_stNextIDInfo.m_nID03);

			a_oOutLevelInfo = oPrevLevelInfo ?? oNextLevelInfo;
			return oPrevLevelInfo != null || oNextLevelInfo != null;
		}

		/** 선택 레벨 정보를 변경한다 */
		private void SetSelLevelInfo(CLevelInfo a_oLevelInfo) {
			m_oLevelInfoDict[EKey.SEL_LEVEL_INFO] = a_oLevelInfo;
			CGameInfoStorage.Inst.SetPlayLevelInfo(a_oLevelInfo);
		}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		#endregion // 조건부 접근자 함수
	}

	/** 서브 레벨 에디터 씬 관리자 - 중앙 에디터 UI */
	public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
		#region 조건부 함수
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		/** 중앙 에디터 UI 를 설정한다 */
		private void SetupMidEditorUIs() {
			// 텍스트를 설정한다
			CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
				(EKey.ME_UIS_MSG_TEXT, $"{EKey.ME_UIS_MSG_TEXT}", this.MidEditorUIs),
				(EKey.ME_UIS_LEVEL_TEXT, $"{EKey.ME_UIS_LEVEL_TEXT}", this.MidEditorUIs),
				(EKey.ME_UIS_OBJ_SIZE_TEXT, $"{EKey.ME_UIS_OBJ_SIZE_TEXT}", this.MidEditorUIs)
			}, m_oTextDict);

			// 이미지를 설정한다
			CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
				(EKey.ME_UIS_SEL_OBJ_IMG, $"{EKey.ME_UIS_SEL_OBJ_IMG}", this.MidEditorUIs)
			}, m_oImgDict);

			// 버튼을 설정한다 {
			CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
				(KCDefine.LES_OBJ_N_ME_UIS_SAVE_BTN, this.MidEditorUIs, this.OnTouchMEUIsSaveBtn),
				(KCDefine.LES_OBJ_N_ME_UIS_RESET_BTN, this.MidEditorUIs, this.OnTouchMEUIsResetBtn),
				(KCDefine.LES_OBJ_N_ME_UIS_TEST_BTN, this.MidEditorUIs, this.OnTouchMEUIsTestBtn),
				(KCDefine.LES_OBJ_N_ME_UIS_COPY_LEVEL_BTN, this.MidEditorUIs, this.OnTouchMEUIsCopyLevelBtn)
			});

			CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
				(EKey.ME_UIS_PREV_GRID_BTN, $"{EKey.ME_UIS_PREV_GRID_BTN}", this.MidEditorUIs, this.OnTouchMEUIsPrevGridBtn),
				(EKey.ME_UIS_NEXT_GRID_BTN, $"{EKey.ME_UIS_NEXT_GRID_BTN}", this.MidEditorUIs, this.OnTouchMEUIsNextGridBtn),
				(EKey.ME_UIS_PREV_LEVEL_BTN, $"{EKey.ME_UIS_PREV_LEVEL_BTN}", this.MidEditorUIs, this.OnTouchMEUIsPrevLevelBtn),
				(EKey.ME_UIS_NEXT_LEVEL_BTN, $"{EKey.ME_UIS_NEXT_LEVEL_BTN}", this.MidEditorUIs, this.OnTouchMEUIsNextLevelBtn),
				(EKey.ME_UIS_MOVE_LEVEL_BTN, $"{EKey.ME_UIS_MOVE_LEVEL_BTN}", this.MidEditorUIs, this.OnTouchMEUIsMoveLevelBtn),
				(EKey.ME_UIS_REMOVE_LEVEL_BTN, $"{EKey.ME_UIS_REMOVE_LEVEL_BTN}", this.MidEditorUIs, this.OnTouchMEUIsRemoveLevelBtn)
			}, m_oBtnDict);
			// 버튼을 설정한다 }

			// 토글을 설정한다 {
			CFunc.SetupToggles(new List<(EKey, string, GameObject, UnityAction<bool>)>() {
				(EKey.ME_UIS_DRAW_MODE_TOGGLE, $"{EKey.ME_UIS_DRAW_MODE_TOGGLE}", this.MidEditorUIs, this.OnTouchMEUIsEditorModeToggle),
				(EKey.ME_UIS_PAINT_MODE_TOGGLE, $"{EKey.ME_UIS_PAINT_MODE_TOGGLE}", this.MidEditorUIs, this.OnTouchMEUIsEditorModeToggle)
			}, m_oToggleDict);

			m_oToggleDict[EKey.ME_UIS_DRAW_MODE_TOGGLE]?.SetIsOnWithoutNotify(true);
			m_oToggleDict[EKey.ME_UIS_PAINT_MODE_TOGGLE]?.SetIsOnWithoutNotify(false);
			// 토글을 설정한다 }

			// 스크롤 바를 설정한다
			CFunc.SetupScrollBars(new List<(EKey, string, GameObject, UnityAction<float>)>() {
				(EKey.ME_UIS_GRID_SCROLL_BAR_V, $"{EKey.ME_UIS_GRID_SCROLL_BAR_V}", this.MidEditorUIs, this.OnChangeMEUIsGridScrollBarVal),
				(EKey.ME_UIS_GRID_SCROLL_BAR_H, $"{EKey.ME_UIS_GRID_SCROLL_BAR_H}", this.MidEditorUIs, this.OnChangeMEUIsGridScrollBarVal)
			}, m_oScrollBarDict);
		}

		/** 중앙 에디터 UI 상태를 갱신한다 */
		private void UpdateMidEditorUIsState() {
			int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(this.SelLevelInfo.m_stIDInfo.m_nID02, this.SelLevelInfo.m_stIDInfo.m_nID03);

			// 텍스트를 갱신한다
			m_oTextDict[EKey.ME_UIS_LEVEL_TEXT]?.ExSetText<Text>(string.Format(KCDefine.LES_TEXT_FMT_LEVEL, this.SelLevelInfo.m_stIDInfo.m_nID01 + KCDefine.B_VAL_1_INT), false);
			m_oTextDict[EKey.ME_UIS_OBJ_SIZE_TEXT]?.ExSetText<Text>(string.Format(KCDefine.LES_TEXT_FMT_SIZE, m_oInputDict[EKey.RE_UIS_PAGE_UIS_02_OBJ_SIZE_X_INPUT]?.text, m_oInputDict[EKey.RE_UIS_PAGE_UIS_02_OBJ_SIZE_Y_INPUT]?.text), false);

			// 이미지를 갱신한다
			m_oImgDict[EKey.ME_UIS_SEL_OBJ_IMG]?.gameObject.SetActive(m_oObjKindsDict[EKey.SEL_OBJ_KINDS].ExIsValid());
			m_oImgDict[EKey.ME_UIS_SEL_OBJ_IMG]?.ExSetSprite<Image>(Access.GetEditorObjSprite(m_oObjKindsDict[EKey.SEL_OBJ_KINDS], KCDefine.B_PREFIX_LEVEL_EDITOR_SCENE));

            SetupSpriteREUIs(m_oObjKindsDict[EKey.SEL_OBJ_KINDS], m_oImgDict[EKey.ME_UIS_SEL_OBJ_IMG]);

			// 버튼을 갱신한다 {
			m_oBtnDict[EKey.ME_UIS_PREV_GRID_BTN]?.ExSetInteractable(m_oGridInfoList.ExIsValidIdx(this.SelGridInfoIdx - KCDefine.B_VAL_1_INT));
			m_oBtnDict[EKey.ME_UIS_NEXT_GRID_BTN]?.ExSetInteractable(m_oGridInfoList.ExIsValidIdx(this.SelGridInfoIdx + KCDefine.B_VAL_1_INT));

			m_oBtnDict[EKey.ME_UIS_PREV_LEVEL_BTN]?.ExSetInteractable(CLevelInfoTable.Inst.TryGetLevelInfo(this.SelLevelInfo.m_stIDInfo.m_nID01 - KCDefine.B_VAL_1_INT, out CLevelInfo oPrevLevelInfo, this.SelLevelInfo.m_stIDInfo.m_nID02, this.SelLevelInfo.m_stIDInfo.m_nID03), false);
			m_oBtnDict[EKey.ME_UIS_NEXT_LEVEL_BTN]?.ExSetInteractable(CLevelInfoTable.Inst.TryGetLevelInfo(this.SelLevelInfo.m_stIDInfo.m_nID01 + KCDefine.B_VAL_1_INT, out CLevelInfo oNextLevelInfo, this.SelLevelInfo.m_stIDInfo.m_nID02, this.SelLevelInfo.m_stIDInfo.m_nID03), false);

			m_oBtnDict[EKey.ME_UIS_MOVE_LEVEL_BTN]?.ExSetInteractable(nNumLevelInfos > KCDefine.B_VAL_1_INT);
			m_oBtnDict[EKey.ME_UIS_REMOVE_LEVEL_BTN]?.ExSetInteractable(nNumLevelInfos > KCDefine.B_VAL_1_INT);
			// 버튼을 갱신한다 }
		}

		/** 중앙 에디터 UI 저장 버튼을 눌렀을 경우 */
		private void OnTouchMEUIsSaveBtn() {
			this.SaveLevelInfos();
		}

		/** 중앙 에디터 UI 리셋 버튼을 눌렀을 경우 */
		private void OnTouchMEUIsResetBtn() {
			Func.ShowAlertPopup(KDefine.ES_MSG_ALERT_P_RESET, this.OnReceiveEditorResetPopupResult);
		}

		/** 중앙 에디터 UI 테스트 버튼을 눌렀을 경우 */
		private void OnTouchMEUIsTestBtn() {
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
			Func.SetupPlayEpisodeInfo(CGameInfoStorage.Inst.PlayCharacterID, this.SelLevelInfo.m_stIDInfo.m_nID01, EPlayMode.TEST, this.SelLevelInfo.m_stIDInfo.m_nID02, this.SelLevelInfo.m_stIDInfo.m_nID03);
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME);
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		}

		/** 중앙 에디터 UI 레벨 복사 버튼을 눌렀을 경우 */
		private void OnTouchMEUIsCopyLevelBtn() {
			int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(this.SelLevelInfo.m_stIDInfo.m_nID02, this.SelLevelInfo.m_stIDInfo.m_nID03);

			// 레벨 추가가 가능 할 경우
			if(nNumLevelInfos < KCDefine.U_MAX_NUM_LEVEL_INFOS) {
				var stIDInfo = new STIDInfo(this.SelLevelInfo.m_stIDInfo.m_nID01, this.SelLevelInfo.m_stIDInfo.m_nID02, this.SelLevelInfo.m_stIDInfo.m_nID03);
				this.CopyLevelInfos(m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].m_oScroller, stIDInfo);
			}
		}

		/** 중앙 에디터 UI 이전 레벨 버튼을 눌렀을 경우 */
		private void OnTouchMEUIsPrevGridBtn() {
			// 이전 그리드 정보가 존재 할 경우
			if(m_oGridInfoList.ExIsValidIdx(this.SelGridInfoIdx - KCDefine.B_VAL_1_INT)) {
				m_oIntDict[EKey.SEL_GRID_IDX] = this.SelGridInfoIdx - KCDefine.B_VAL_1_INT;
				this.UpdateUIsState();
			}
		}

		/** 중앙 에디터 UI 다음 레벨 버튼을 눌렀을 경우 */
		private void OnTouchMEUIsNextGridBtn() {
			// 다음 그리드 정보가 존재 할 경우
			if(m_oGridInfoList.ExIsValidIdx(this.SelGridInfoIdx + KCDefine.B_VAL_1_INT)) {
				m_oIntDict[EKey.SEL_GRID_IDX] = this.SelGridInfoIdx + KCDefine.B_VAL_1_INT;
				this.UpdateUIsState();
			}
		}

		/** 중앙 에디터 UI 이전 레벨 버튼을 눌렀을 경우 */
		private void OnTouchMEUIsPrevLevelBtn() {
			// 이전 레벨 정보가 존재 할 경우
			if(CLevelInfoTable.Inst.TryGetLevelInfo(this.SelLevelInfo.m_stIDInfo.m_nID01 - KCDefine.B_VAL_1_INT, out CLevelInfo oPrevLevelInfo, this.SelLevelInfo.m_stIDInfo.m_nID02, this.SelLevelInfo.m_stIDInfo.m_nID03)) {
				this.SetSelLevelInfo(oPrevLevelInfo);
				this.UpdateUIsState();
			}
		}

		/** 중앙 에디터 UI 다음 레벨 버튼을 눌렀을 경우 */
		private void OnTouchMEUIsNextLevelBtn() {
			// 다음 레벨 정보가 존재 할 경우
			if(CLevelInfoTable.Inst.TryGetLevelInfo(this.SelLevelInfo.m_stIDInfo.m_nID01 + KCDefine.B_VAL_1_INT, out CLevelInfo oNextLevelInfo, this.SelLevelInfo.m_stIDInfo.m_nID02, this.SelLevelInfo.m_stIDInfo.m_nID03)) {
				this.SetSelLevelInfo(oNextLevelInfo);
				this.UpdateUIsState();
			}
		}

		/** 중앙 에디터 UI 레벨 이동 버튼을 눌렀을 경우 */
		private void OnTouchMEUIsMoveLevelBtn() {
			m_oScrollerDict[EKey.SEL_SCROLLER] = m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].m_oScroller;
			m_oInputPopupDict[EKey.SEL_INPUT_POPUP] = EInputPopup.MOVE_LEVEL;

			Func.ShowEditorInputPopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CEditorInputPopup).Init(CEditorInputPopup.MakeParams(new Dictionary<CEditorInputPopup.ECallback, System.Action<CEditorInputPopup, string, bool>>() {
					[CEditorInputPopup.ECallback.OK_CANCEL] = this.OnReceiveEditorInputPopupResult
				}));
			});
		}

		/** 중앙 에디터 UI 레벨 제거 버튼을 눌렀을 경우 */
		private void OnTouchMEUIsRemoveLevelBtn() {
			m_oScrollerDict[EKey.SEL_SCROLLER] = m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].m_oScroller;
			Func.ShowAlertPopup(KDefine.ES_MSG_ALERT_P_REMOVE_LEVEL, (a_oSender, a_bIsOK) => this.OnReceiveEditorRemovePopupResult(a_oSender, this.SelLevelInfo.m_stIDInfo, a_bIsOK));
		}

		/** 중앙 에디터 UI 그리드 라인 수평 버튼을 눌렀을 경우 */
		private void OnTouchMEUIsGridLineBtnH(int a_nIdx) {
			for(int i = 0; i < this.SelLevelInfo.m_oCellInfoDictContainer.Count; ++i) {
				// 셀 정보가 존재 할 경우
				if(this.SelLevelInfo.m_oCellInfoDictContainer[i].TryGetValue(a_nIdx, out STCellInfo stCellInfo)) {
					var stIdx = new Vector3Int(a_nIdx, i, KCDefine.B_VAL_0_INT);

					// 객체 추가가 가능 할 경우
					if(Input.GetMouseButtonUp((int)EMouseBtn.LEFT) && m_oObjKindsDict[EKey.SEL_OBJ_KINDS].ExIsValid()) {
						this.AddCellObjInfo(Factory.MakeEditorCellObjInfo(m_oObjKindsDict[EKey.SEL_OBJ_KINDS], this.GetEditorObjSize(), stIdx, drawCellColor), stIdx, Input.GetKey(KeyCode.LeftShift));
					}
					// 객체 제거가 가능 할 경우
					else if(Input.GetMouseButtonUp((int)EMouseBtn.RIGHT) && stCellInfo.m_oCellObjInfoList.ExIsValid()) {
						this.RemoveAllCellObjInfos(Input.GetKey(KeyCode.LeftShift) ? m_oObjKindsDict[EKey.SEL_OBJ_KINDS] : EObjKinds.NONE, stIdx);
					}
				}
			}

			this.UpdateUIsState();
		}

		/** 중앙 에디터 UI 그리드 라인 수직 버튼을 눌렀을 경우 */
		private void OnTouchMEUIsGridLineBtnV(int a_nIdx) {
			// 셀 정보가 존재 할 경우
			if(this.SelLevelInfo.m_oCellInfoDictContainer.TryGetValue(a_nIdx, out Dictionary<int, STCellInfo> oCellInfoDict)) {
				for(int i = 0; i < oCellInfoDict.Count; ++i) {
					var stIdx = new Vector3Int(i, a_nIdx, KCDefine.B_VAL_0_INT);

					// 객체 추가가 가능 할 경우
					if(Input.GetMouseButtonUp((int)EMouseBtn.LEFT) && m_oObjKindsDict[EKey.SEL_OBJ_KINDS].ExIsValid()) {
						this.AddCellObjInfo(Factory.MakeEditorCellObjInfo(m_oObjKindsDict[EKey.SEL_OBJ_KINDS], this.GetEditorObjSize(), stIdx, drawCellColor), stIdx, Input.GetKey(KeyCode.LeftShift));
					}
					// 객체 제거가 가능 할 경우
					else if(Input.GetMouseButtonUp((int)EMouseBtn.RIGHT) && oCellInfoDict[i].m_oCellObjInfoList.ExIsValid()) {
						this.RemoveAllCellObjInfos(Input.GetKey(KeyCode.LeftShift) ? m_oObjKindsDict[EKey.SEL_OBJ_KINDS] : EObjKinds.NONE, stIdx);
					}
				}
			}

			this.UpdateUIsState();
		}

		/** 중앙 에디터 UI 에디터 모드 토글을 눌렀을 경우 */
		private void OnTouchMEUIsEditorModeToggle(bool a_bIsTrue) {
			bool bIsDraw = m_oToggleDict[EKey.ME_UIS_DRAW_MODE_TOGGLE] != null && m_oToggleDict[EKey.ME_UIS_DRAW_MODE_TOGGLE].isOn;
			bool bIsPaint = m_oToggleDict[EKey.ME_UIS_PAINT_MODE_TOGGLE] != null && m_oToggleDict[EKey.ME_UIS_PAINT_MODE_TOGGLE].isOn;

			this.SetMEUIsEditorMode(bIsDraw, bIsPaint);
		}

		/** 중앙 에디터 UI 그리드 스크롤 바 값이 변경 되었을 경우 */
		private void OnChangeMEUIsGridScrollBarVal(float a_fVal) {
			float fWidth = Mathf.Max(KCDefine.B_VAL_0_INT, this.SelGridInfo.m_stBounds.size.x - this.SelGridInfo.m_stViewBounds.size.x);
			float fHeight = Mathf.Max(KCDefine.B_VAL_0_INT, this.SelGridInfo.m_stBounds.size.y - this.SelGridInfo.m_stViewBounds.size.y);

			this.SetMEUIsGridScrollDelta(a_fVal * -fWidth, a_fVal * -fHeight);
		}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		#endregion // 조건부 함수

		#region 조건부 접근자 함수
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		/** 중앙 에디터 UI 에디터 모드를 변경한다 */
		private void SetMEUIsEditorMode(bool a_bIsDraw, bool a_bIsPaint) {
			m_oEditorModeDict[EKey.SEL_EDITOR_MODE] = a_bIsDraw ? EEditorMode.DRAW : EEditorMode.PAINT;

			m_oToggleDict[EKey.ME_UIS_DRAW_MODE_TOGGLE]?.SetIsOnWithoutNotify(a_bIsDraw);
			m_oToggleDict[EKey.ME_UIS_PAINT_MODE_TOGGLE]?.SetIsOnWithoutNotify(a_bIsPaint);

			this.UpdateUIsState();
		}

		/** 중앙 에디터 UI 그리드 스크롤 간격을 변경한다 */
		private void SetMEUIsGridScrollDelta(float a_fDeltaX, float a_fDeltaY) {
			float fWidth = Mathf.Max(KCDefine.B_VAL_0_REAL, this.SelGridInfo.m_stBounds.size.x - this.SelGridInfo.m_stViewBounds.size.x);
			float fHeight = Mathf.Max(KCDefine.B_VAL_0_REAL, this.SelGridInfo.m_stBounds.size.y - this.SelGridInfo.m_stViewBounds.size.y);

			m_oRealDict[EKey.GRID_SCROLL_DELTA_X] = Mathf.Clamp(a_fDeltaX, -fWidth, KCDefine.B_VAL_0_REAL);
			m_oRealDict[EKey.GRID_SCROLL_DELTA_Y] = Mathf.Clamp(a_fDeltaY, -fHeight, KCDefine.B_VAL_0_REAL);

			// 수평 그리드 스크롤 바가 존재 할 경우
			if(fWidth.ExIsGreate(KCDefine.B_VAL_0_REAL) && m_oScrollBarDict.TryGetValue(EKey.ME_UIS_GRID_SCROLL_BAR_H, out Scrollbar oScrollbarH)) {
				oScrollbarH.SetValueWithoutNotify(Mathf.Abs(m_oRealDict[EKey.GRID_SCROLL_DELTA_X] / fWidth));
			}

			// 수직 그리드 스크롤 바가 존재 할 경우
			if(fHeight.ExIsGreate(KCDefine.B_VAL_0_REAL) && m_oScrollBarDict.TryGetValue(EKey.ME_UIS_GRID_SCROLL_BAR_V, out Scrollbar oScrollbarV)) {
				oScrollbarV.SetValueWithoutNotify(Mathf.Abs(m_oRealDict[EKey.GRID_SCROLL_DELTA_Y] / fHeight));
			}

			this.UpdateUIsState();
		}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		#endregion // 조건부 접근자 함수
	}

	/** 서브 레벨 에디터 씬 관리자 - 왼쪽 에디터 UI */
	public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
		#region 조건부 함수
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		/** 왼쪽 에디터 UI 를 설정한다 */
		private void SetupLeftEditorUIs() {
			var oScrollViewDict = CCollectionManager.Inst.SpawnDict<string, GameObject>();

			try {
				// 스크롤 뷰를 설정한다 {
				CFunc.SetupObjs(new List<(string, string, GameObject)>() {
					(KCDefine.LES_OBJ_N_LE_UIS_STAGE_SCROLL_VIEW_01, KCDefine.LES_OBJ_N_LE_UIS_STAGE_SCROLL_VIEW_01, this.LeftEditorUIs),
					(KCDefine.LES_OBJ_N_LE_UIS_STAGE_SCROLL_VIEW_02, KCDefine.LES_OBJ_N_LE_UIS_STAGE_SCROLL_VIEW_02, this.LeftEditorUIs)
				}, oScrollViewDict);

				CFunc.SetupScrollerInfos(new List<(EKey, string, GameObject, EnhancedScrollerCellView, IEnhancedScrollerDelegate)>() {
					(EKey.LE_UIS_LEVEL_SCROLLER_INFO, KCDefine.LES_OBJ_N_LE_UIS_LEVEL_SCROLL_VIEW, this.LeftEditorUIs, CResManager.Inst.GetRes<GameObject>(KCDefine.ES_OBJ_P_EDITOR_SCROLLER_CELL_VIEW)?.GetComponentInChildren<EnhancedScrollerCellView>(), this),
					(EKey.LE_UIS_CHAPTER_SCROLLER_INFO, KCDefine.LES_OBJ_N_LE_UIS_CHAPTER_SCROLL_VIEW, this.LeftEditorUIs, CResManager.Inst.GetRes<GameObject>(KCDefine.ES_OBJ_P_EDITOR_SCROLLER_CELL_VIEW)?.GetComponentInChildren<EnhancedScrollerCellView>(), this),
					(EKey.LE_UIS_STAGE_SCROLLER_INFO_01, KCDefine.LES_OBJ_N_LE_UIS_STAGE_SCROLL_VIEW_01, this.LeftEditorUIs, CResManager.Inst.GetRes<GameObject>(KCDefine.ES_OBJ_P_EDITOR_SCROLLER_CELL_VIEW)?.GetComponentInChildren<EnhancedScrollerCellView>(), this),
					(EKey.LE_UIS_STAGE_SCROLLER_INFO_02, KCDefine.LES_OBJ_N_LE_UIS_STAGE_SCROLL_VIEW_02, this.LeftEditorUIs, CResManager.Inst.GetRes<GameObject>(KCDefine.ES_OBJ_P_EDITOR_SCROLLER_CELL_VIEW)?.GetComponentInChildren<EnhancedScrollerCellView>(), this)
				}, m_oScrollerInfoDict);

				m_oScrollerInfoDict.ExReplaceVal(EKey.LE_UIS_STAGE_SCROLLER_INFO, m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO_01]);

				foreach(var stKeyVal in oScrollViewDict) {
					stKeyVal.Value?.gameObject.SetActive(false);
				}

				m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].m_oScroller?.gameObject.SetActive(true);
				m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].m_oScroller?.gameObject.SetActive(false);
				m_oScrollerInfoDict[EKey.LE_UIS_CHAPTER_SCROLLER_INFO].m_oScroller?.gameObject.SetActive(false);
				// 스크롤 뷰를 설정한다 }

				// 버튼을 설정한다 {
				CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
					(KCDefine.LES_OBJ_N_LE_UIS_ADD_LEVEL_BTN, this.LeftEditorUIs, this.OnTouchLEUIsAddLevelBtn)
				});

				CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
					(EKey.LE_UIS_ADD_STAGE_BTN, $"{EKey.LE_UIS_ADD_STAGE_BTN}", this.LeftEditorUIs, this.OnTouchLEUIsAddStageBtn),
					(EKey.LE_UIS_ADD_CHAPTER_BTN, $"{EKey.LE_UIS_ADD_CHAPTER_BTN}", this.LeftEditorUIs, this.OnTouchLEUIsAddChapterBtn)
				}, m_oBtnDict);

#if AB_TEST_ENABLE
				CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
					(EKey.LE_UIS_A_SET_BTN, $"{EKey.LE_UIS_A_SET_BTN}", this.LeftEditorUIs, () => this.OnTouchLEUIsSetBtn(this.LeftEditorUIs.ExFindComponent<Button>($"{EKey.LE_UIS_A_SET_BTN}"))),
					(EKey.LE_UIS_B_SET_BTN, $"{EKey.LE_UIS_B_SET_BTN}", this.LeftEditorUIs, () => this.OnTouchLEUIsSetBtn(this.LeftEditorUIs.ExFindComponent<Button>($"{EKey.LE_UIS_B_SET_BTN}")))
				}, m_oBtnDict);
#endif // #if AB_TEST_ENABLE

				this.ExLateCallFunc((a_oSender) => {
#if AB_TEST_ENABLE
					this.LEUIsABSetUIs?.SetActive(true);
#else
					this.LEUIsABSetUIs?.SetActive(false);
#endif // #if AB_TEST_ENABLE

					m_oBtnDict[EKey.LE_UIS_ADD_STAGE_BTN]?.ExSetInteractable(m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].m_oScroller != null && m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].m_oScroller.gameObject.activeSelf);
					m_oBtnDict[EKey.LE_UIS_ADD_CHAPTER_BTN]?.ExSetInteractable(m_oScrollerInfoDict[EKey.LE_UIS_CHAPTER_SCROLLER_INFO].m_oScroller != null && m_oScrollerInfoDict[EKey.LE_UIS_CHAPTER_SCROLLER_INFO].m_oScroller.gameObject.activeSelf);
				});
				// 버튼을 설정한다 }
			} finally {
				CCollectionManager.Inst.DespawnDict(oScrollViewDict);
			}
		}

		/** 왼쪽 에디터 UI 상태를 갱신한다 */
		private void UpdateLeftEditorUIsState() {
			// 스크롤 뷰를 갱신한다
			m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].m_oScroller?.ExReloadData(this.SelLevelInfo.m_stIDInfo.m_nID01 - KCDefine.B_VAL_1_INT, false);
			m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].m_oScroller?.ExReloadData(this.SelLevelInfo.m_stIDInfo.m_nID02 - KCDefine.B_VAL_1_INT, false);
			m_oScrollerInfoDict[EKey.LE_UIS_CHAPTER_SCROLLER_INFO].m_oScroller?.ExReloadData(this.SelLevelInfo.m_stIDInfo.m_nID03 - KCDefine.B_VAL_1_INT, false);

			// 버튼을 설정한다 {
#if AB_TEST_ENABLE
			m_oBtnDict[EKey.LE_UIS_A_SET_BTN]?.image.ExSetColor<Image>((CCommonUserInfoStorage.Inst.UserInfo.UserType == EUserType.A) ? Color.yellow : Color.white, false);
			m_oBtnDict[EKey.LE_UIS_B_SET_BTN]?.image.ExSetColor<Image>((CCommonUserInfoStorage.Inst.UserInfo.UserType == EUserType.B) ? Color.yellow : Color.white, false);
#endif // #if AB_TEST_ENABLE
			// 버튼을 설정한다 }
		}

		/** 왼쪽 에디터 UI 레벨 추가 버튼을 눌렀을 경우 */
		private void OnTouchLEUIsAddLevelBtn() {
			Func.ShowEditorCreatePopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CEditorCreatePopup).Init(CEditorCreatePopup.MakeParams(new Dictionary<CEditorCreatePopup.ECallback, System.Action<CEditorCreatePopup, CEditorCreateInfo, bool>>() {
					[CEditorCreatePopup.ECallback.OK_CANCEL] = this.OnReceiveEditorCreatePopupResult
				}));
			});
		}

		/** 왼쪽 에디터 UI 스테이지 추가 버튼을 눌렀을 경우 */
		private void OnTouchLEUIsAddStageBtn() {
			int nNumStageInfos = CLevelInfoTable.Inst.GetNumStageInfos(this.SelLevelInfo.m_stIDInfo.m_nID03);

			// 스테이지 추가가 가능 할 경우
			if(nNumStageInfos < KCDefine.U_MAX_NUM_STAGE_INFOS) {
				this.AddLevelInfo(KCDefine.B_VAL_0_INT, nNumStageInfos, this.SelLevelInfo.m_stIDInfo.m_nID03);
			}
		}

		/** 왼쪽 에디터 UI 챕터 추가 버튼을 눌렀을 경우 */
		private void OnTouchLEUIsAddChapterBtn() {
			int nNumChapterInfos = CLevelInfoTable.Inst.NumChapterInfos;

			// 챕터 추가가 가능 할 경우
			if(nNumChapterInfos < KCDefine.U_MAX_NUM_CHAPTER_INFOS) {
				this.AddLevelInfo(KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, nNumChapterInfos);
			}
		}

#if AB_TEST_ENABLE
		/** 왼쪽 에디터 UI 세트 버튼을 눌렀을 경우 */
		private void OnTouchLEUIsSetBtn(Button a_oSender) {
			var eUserType = (a_oSender == m_oBtnDict[EKey.LE_UIS_A_SET_BTN]) ? EUserType.A : EUserType.B;

			// 유저 타입이 다를 경우
			if(CCommonUserInfoStorage.Inst.UserInfo.UserType != eUserType) {
				string oKey = (a_oSender == m_oBtnDict[EKey.LE_UIS_A_SET_BTN]) ? KCDefine.ST_KEY_C_SETUP_A_SET_MSG : KCDefine.ST_KEY_C_SETUP_B_SET_MSG;
				m_oUserTypeDict[EKey.SEL_USER_TYPE] = eUserType;

				Func.ShowAlertPopup(CStrTable.Inst.GetStr(oKey), this.OnReceiveEditorSetPopupResult);
			}
		}
#endif // #if AB_TEST_ENABLE
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		#endregion // 조건부 함수
	}

	/** 서브 레벨 에디터 씬 관리자 - 오른쪽 에디터 UI */
	public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
		#region 조건부 함수
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		/** 오른족 에디터 UI 를 설정한다 */
		private void SetupRightEditorUIs() {
			// 텍스트를 설정한다
			CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
				(EKey.RE_UIS_PAGE_TEXT, $"{EKey.RE_UIS_PAGE_TEXT}", this.RightEditorUIs),
				(EKey.RE_UIS_TITLE_TEXT, $"{EKey.RE_UIS_TITLE_TEXT}", this.RightEditorUIs)
			}, m_oTextDict);

			// 버튼을 설정한다
			CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
				(EKey.RE_UIS_PREV_BTN, $"{EKey.RE_UIS_PREV_BTN}", this.RightEditorUIs),
				(EKey.RE_UIS_NEXT_BTN, $"{EKey.RE_UIS_NEXT_BTN}", this.RightEditorUIs)
			}, m_oBtnDict);

			// 스크롤 뷰를 설정한다 {
			CFunc.SetupScrollSnaps(new List<(EKey, string, GameObject, UnityAction<int, int>)>() {
				(EKey.RE_UIS_PAGE_SCROLL_SNAP, KCDefine.U_OBJ_N_PAGE_VIEW, this.RightEditorUIs, (a_nCenterIdx, a_nSelIdx) => this.UpdateUIsState())
			}, m_oScrollSnapDict);

			// 페이지 스크롤 스냅이 존재 할 경우
			if(m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP] != null) {
				m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP].gameObject.SetActive(false);

				for(int i = 0; i < m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP].NumberOfPanels; ++i) {
					string oSetupFuncName = string.Format(KDefine.LES_FUNC_N_FMT_SETUP_RE_UIS_PAGE_UIS, i + KCDefine.B_VAL_1_INT);
					string oUpdateFuncName = string.Format(KDefine.LES_FUNC_N_FMT_UPDATE_RE_UIS_PAGE_UIS, i + KCDefine.B_VAL_1_INT);

					m_oMethodInfoDict.TryAdd(ECallback.SETUP_RE_UIS_PAGE_UIS_01 + i, this.GetType().GetMethod(oSetupFuncName, KCDefine.B_BINDING_F_NON_PUBLIC_INSTANCE));
					m_oMethodInfoDict.TryAdd(ECallback.UPDATE_RE_UIS_PAGE_UIS_01 + i, this.GetType().GetMethod(oUpdateFuncName, KCDefine.B_BINDING_F_NON_PUBLIC_INSTANCE));
				}

				for(int i = 0; i < m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP].NumberOfPanels; ++i) {
					string oPageUIsName = string.Format(KDefine.LES_OBJ_N_FMT_RE_UIS_PAGE_UIS, i + KCDefine.B_VAL_1_INT);
					m_oUIsDict.TryAdd(EKey.RE_UIS_PAGE_UIS_01 + i, m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP].gameObject.ExFindChild(oPageUIsName));
				}

				for(int i = 0; i < m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP].NumberOfPanels; ++i) {
					m_oMethodInfoDict.GetValueOrDefault(ECallback.SETUP_RE_UIS_PAGE_UIS_01 + i)?.Invoke(this, new object[] {
						m_oUIsDict.GetValueOrDefault(EKey.RE_UIS_PAGE_UIS_01 + i)
					});
				}
			}
			// 스크롤 뷰를 설정한다 }
		}

		/** 오른쪽 에디터 UI 페이지 UI 1 를 설정한다 */
		private void SetupREUIsPageUIs01(GameObject a_oPageUIs) {
			// 입력 필드를 설정한다 {
			CFunc.SetupInputs(new List<(EKey, string, GameObject, UnityAction<string>)>() {
				(EKey.RE_UIS_PAGE_UIS_01_LEVEL_INPUT, $"{EKey.RE_UIS_PAGE_UIS_01_LEVEL_INPUT}", a_oPageUIs, this.OnChangeREUIsPageUIs01LevelInputStr),
				(EKey.RE_UIS_PAGE_UIS_01_NUM_CELLS_X_INPUT, $"{EKey.RE_UIS_PAGE_UIS_01_NUM_CELLS_X_INPUT}", a_oPageUIs, this.OnChangeREUIsPageUIs01NumCellsInputStr),
				(EKey.RE_UIS_PAGE_UIS_01_NUM_CELLS_Y_INPUT, $"{EKey.RE_UIS_PAGE_UIS_01_NUM_CELLS_Y_INPUT}", a_oPageUIs, this.OnChangeREUIsPageUIs01NumCellsInputStr)
			}, m_oInputDict);

			m_oInputList01.ExAddVal(m_oInputDict[EKey.RE_UIS_PAGE_UIS_01_LEVEL_INPUT]);
			m_oInputList01.ExAddVal(m_oInputDict[EKey.RE_UIS_PAGE_UIS_01_NUM_CELLS_X_INPUT]);
			m_oInputList01.ExAddVal(m_oInputDict[EKey.RE_UIS_PAGE_UIS_01_NUM_CELLS_Y_INPUT]);
			// 입력 필드를 설정한다 }

			// 버튼을 설정한다 {
			CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
				(KCDefine.LES_OBJ_N_RE_UIS_PAGE_UIS_01_APPLY_BTN, a_oPageUIs, this.OnTouchREUIsPageUIs01ApplyBtn),
				(KCDefine.LES_OBJ_N_RE_UIS_PAGE_UIS_01_FILL_ALL_CELLS_BTN, a_oPageUIs, this.OnTouchREUIsPageUIs01FillAllCellsBtn),
				(KCDefine.LES_OBJ_N_RE_UIS_PAGE_UIS_01_CLEAR_ALL_CELLS_BTN, a_oPageUIs, this.OnTouchREUIsPageUIs01ClearAllCellsBtn),
				(KCDefine.LES_OBJ_N_RE_UIS_PAGE_UIS_01_CLEAR_SEL_CELLS_BTN, a_oPageUIs, this.OnTouchREUIsPageUIs01ClearSelCellsBtn)
			});

			CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
				(EKey.RE_UIS_PAGE_UIS_01_LOAD_LEVEL_BTN, $"{EKey.RE_UIS_PAGE_UIS_01_LOAD_LEVEL_BTN}", a_oPageUIs, this.OnTouchREUIsPageUIs01LoadLevelBtn),
				(EKey.RE_UIS_PAGE_UIS_01_LOAD_LOCAL_TABLE_BTN, $"{EKey.RE_UIS_PAGE_UIS_01_LOAD_LOCAL_TABLE_BTN}", a_oPageUIs, () => this.OnTouchREUIsPageUIs01LoadTableBtn(ETableSrc.LOCAL)),
				(EKey.RE_UIS_PAGE_UIS_01_LOAD_REMOTE_TABLE_BTN, $"{EKey.RE_UIS_PAGE_UIS_01_LOAD_REMOTE_TABLE_BTN}", a_oPageUIs, () => this.OnTouchREUIsPageUIs01LoadTableBtn(ETableSrc.REMOTE)),
				(EKey.RE_UIS_PAGE_UIS_01_REMOVE_ALL_LEVELS_BTN, $"{EKey.RE_UIS_PAGE_UIS_01_REMOVE_ALL_LEVELS_BTN}", a_oPageUIs, this.OnTouchREUIsPageUIs01RemoveAllLevelsBtn)
			}, m_oBtnDict);
			// 버튼을 설정한다 }
		}

		/** 오른쪽 에디터 UI 페이지 UI 2 를 설정한다 */
		private void SetupREUIsPageUIs02(GameObject a_oPageUIs) {
			// 입력 필드를 설정한다 {
			CFunc.SetupInputs(new List<(EKey, string, GameObject, UnityAction<string>)>() {
				(EKey.RE_UIS_PAGE_UIS_02_OBJ_SIZE_X_INPUT, $"{EKey.RE_UIS_PAGE_UIS_02_OBJ_SIZE_X_INPUT}", a_oPageUIs, this.OnChangeREUIsPageUIs02ObjSizeInputStr),
				(EKey.RE_UIS_PAGE_UIS_02_OBJ_SIZE_Y_INPUT, $"{EKey.RE_UIS_PAGE_UIS_02_OBJ_SIZE_Y_INPUT}", a_oPageUIs, this.OnChangeREUIsPageUIs02ObjSizeInputStr)
			}, m_oInputDict);

			m_oInputList02.ExAddVal(m_oInputDict[EKey.RE_UIS_PAGE_UIS_02_OBJ_SIZE_X_INPUT]);
			m_oInputList02.ExAddVal(m_oInputDict[EKey.RE_UIS_PAGE_UIS_02_OBJ_SIZE_Y_INPUT]);

			m_oInputDict[EKey.RE_UIS_PAGE_UIS_02_OBJ_SIZE_X_INPUT]?.SetTextWithoutNotify(KCDefine.B_STR_1_INT);
			m_oInputDict[EKey.RE_UIS_PAGE_UIS_02_OBJ_SIZE_Y_INPUT]?.SetTextWithoutNotify(KCDefine.B_STR_1_INT);
			// 입력 필드를 설정한다 }

			// 탭 UI 를 설정한다 {
			var oTapUIsHandler = a_oPageUIs.GetComponentInChildren<CTapUIsHandler>();

			// 탭 UI 가 존재 할 경우
			if(oTapUIsHandler != null) {
				oTapUIsHandler.Init(CTapUIsHandler.MakeParams(new Dictionary<CTapUIsHandler.ECallback, System.Action<CTapUIsHandler, int>>() {
					[CTapUIsHandler.ECallback.TAP] = this.OnReceiveREUIsPageUIs02TapCallback
				}));

				for(int i = 0; i < oTapUIsHandler.ContentsUIsList.Count; ++i) {
					this.SetupREUIsPageUIs02TapContentsUIs(oTapUIsHandler.ContentsUIsList[i], i);
				}
			}
			// 탭 UI 를 설정한다 }
		}

		/** 오른쪽 에디터 UI 페이지 UI 2 탭 컨텐츠 UI 를 설정한다 */
		private void SetupREUIsPageUIs02TapContentsUIs(GameObject a_oContents, int a_nIdx) {
			var oScrollRect = a_oContents?.GetComponentInChildren<ScrollRect>();
			var oScrollViewContents = a_oContents?.ExFindChild(KCDefine.U_OBJ_N_CONTENTS);

			// 스크롤 영역이 존재 할 경우
			if(oScrollRect != null && oScrollViewContents != null) {
				// 스크롤러 셀 뷰가 존재 할 경우
				if(KDefine.LES_OBJ_KINDS_DICT_CONTAINER.TryGetValue(a_nIdx, out List<EObjKinds> oObjKindsList)) {
					int nMaxNumCells = oObjKindsList.Count / KDefine.LES_MAX_NUM_OBJ_KINDS_IN_ROW;
					int nNumExtraCells = (oObjKindsList.Count % KDefine.LES_MAX_NUM_OBJ_KINDS_IN_ROW > KCDefine.B_VAL_0_INT) ? KCDefine.B_VAL_1_INT : KCDefine.B_VAL_0_INT;

					for(int i = 0; i < nMaxNumCells + nNumExtraCells; ++i) {
						var oScrollerCellView = CFactory.CreateCloneObj(KCDefine.LES_OBJ_N_RE_UIS_PAGE_UIS_02_SCROLLER_CELL_VIEW, CResManager.Inst.GetRes<GameObject>(KCDefine.LES_OBJ_P_RE_UIS_PAGE_UIS_02_SCROLLER_CELL_VIEW), oScrollViewContents);
						this.SetupREUIsPageUIs02ScrollerCellView(oScrollerCellView, oObjKindsList, i * KDefine.LES_MAX_NUM_OBJ_KINDS_IN_ROW);
					}
				}
			}
		}

		/** 오른쪽 에디터 UI 페이지 UI 2 스크롤러 셀 뷰를 설정한다 */
		private void SetupREUIsPageUIs02ScrollerCellView(GameObject oScrollerCellView, List<EObjKinds> a_oObjKindsList, int a_nIdx) {
			for(int i = 0; i < KDefine.LES_MAX_NUM_OBJ_KINDS_IN_ROW; ++i) {
				// 버튼이 존재 할 경우
				if(oScrollerCellView.transform.GetChild(i).TryGetComponent<Button>(out Button oBtn)) {
					var eObjKinds = a_oObjKindsList.ExGetVal(i + a_nIdx, EObjKinds.NONE);

					oBtn.gameObject.ExAddComponent<CBtnHandler>();
					oBtn.ExSetInteractable(eObjKinds != EObjKinds.NONE);

					oBtn.image.ExSetEnable(eObjKinds != EObjKinds.NONE);
					oBtn.image.sprite = Access.GetEditorObjSprite(eObjKinds, KCDefine.B_PREFIX_LEVEL_EDITOR_SCENE);

                    SetupSpriteREUIs(eObjKinds, oBtn.image);

					oBtn.onClick.AddListener(() => this.OnTouchREUIsPageUIs02ScrollerCellViewBtn(eObjKinds));
				}
			}
		}

		/** 오른쪽 에디터 UI 상태를 갱신한다 */
		private void UpdateRightEditorUIsState() {
			// 텍스트를 설정한다
			int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(this.SelLevelInfo.m_stIDInfo.m_nID02, this.SelLevelInfo.m_stIDInfo.m_nID03);
			m_oTextDict[EKey.RE_UIS_TITLE_TEXT]?.ExSetText<Text>(string.Format(CStrTable.Inst.GetStr(KCDefine.ST_KEY_C_LEVEL_PAGE_TEXT_FMT), this.SelLevelInfo.m_stIDInfo.m_nID01 + KCDefine.B_VAL_1_INT, nNumLevelInfos), false);

			// 버튼을 설정한다 {
			m_oBtnDict[EKey.RE_UIS_PAGE_UIS_01_REMOVE_ALL_LEVELS_BTN]?.ExSetInteractable(nNumLevelInfos > KCDefine.B_VAL_1_INT, false);

#if GOOGLE_SHEET_ENABLE
			m_oBtnDict[EKey.RE_UIS_PAGE_UIS_01_LOAD_REMOTE_TABLE_BTN]?.ExSetInteractable(true, false);
#else
			m_oBtnDict[EKey.RE_UIS_PAGE_UIS_01_LOAD_REMOTE_TABLE_BTN]?.ExSetInteractable(false, false);
#endif // #if GOOGLE_SHEET_ENABLE
			// 버튼을 설정한다 }

			// 페이지 스크롤 스냅이 존재 할 경우
			if(m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP] != null) {
				// 텍스트를 설정한다
				m_oTextDict[EKey.RE_UIS_PAGE_TEXT]?.ExSetText<Text>(string.Format(KCDefine.B_TEXT_FMT_2_SLASH_COMBINE, m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP].CenteredPanel + KCDefine.B_VAL_1_INT, m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP].NumberOfPanels), false);

				// 버튼 상태를 갱신한다
				m_oBtnDict[EKey.RE_UIS_PREV_BTN]?.ExSetInteractable(m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP].CenteredPanel > KCDefine.B_VAL_0_INT, false);
				m_oBtnDict[EKey.RE_UIS_NEXT_BTN]?.ExSetInteractable(m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP].CenteredPanel < m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP].NumberOfPanels - KCDefine.B_VAL_1_INT, false);

				// 페이지 UI 상태를 갱신한다
				for(int i = 0; i < m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP].NumberOfPanels; ++i) {
					m_oMethodInfoDict.GetValueOrDefault(ECallback.UPDATE_RE_UIS_PAGE_UIS_01 + i)?.Invoke(this, new object[] {
						m_oUIsDict.GetValueOrDefault(EKey.RE_UIS_PAGE_UIS_01 + i)
					});
				}
			}
		}

		/** 오른쪽 에디터 UI 페이지 UI 1 상태를 갱신한다 */
		private void UpdateREUIsPageUIs01(GameObject a_oPageUIs) {
			// 입력 필드를 갱신한다 {
			m_oInputDict[EKey.RE_UIS_PAGE_UIS_01_LEVEL_INPUT]?.ExSetText<InputField>($"{this.SelLevelInfo.m_stIDInfo.m_nID01 + KCDefine.B_VAL_1_INT}", false);

			m_oInputDict[EKey.RE_UIS_PAGE_UIS_01_NUM_CELLS_X_INPUT]?.ExSetText<InputField>((this.SelLevelInfo.NumCells.x <= KCDefine.B_VAL_0_INT) ? string.Empty : $"{this.SelLevelInfo.NumCells.x}", false);
			m_oInputDict[EKey.RE_UIS_PAGE_UIS_01_NUM_CELLS_Y_INPUT]?.ExSetText<InputField>((this.SelLevelInfo.NumCells.y <= KCDefine.B_VAL_0_INT) ? string.Empty : $"{this.SelLevelInfo.NumCells.y}", false);
			// 입력 필드를 갱신한다 }
		}

		/** 오른쪽 에디터 UI 페이지 UI 2 상태를 갱신한다 */
		private void UpdateREUIsPageUIs02(GameObject a_oPageUIs) {
			// 탭 UI 를 갱신한다 {
			var oTapUIsHandler = a_oPageUIs.GetComponentInChildren<CTapUIsHandler>();

			// 탭 UI 가 존재 할 경우
			if(oTapUIsHandler != null) {
				for(int i = 0; i < oTapUIsHandler.TapBtnList.Count; ++i) {
					oTapUIsHandler.TapBtnList[i].image.color = (oTapUIsHandler.SelTapBtnIdx == i) ? KCDefine.U_COLOR_NORM : KCDefine.U_COLOR_DISABLE;
				}
			}
			// 탭 UI 를 갱신한다 }
		}

		/** 오른쪽 에디터 UI 페이지 UI 2 탭 콜백을 수신했을 경우 */
		private void OnReceiveREUIsPageUIs02TapCallback(CTapUIsHandler a_oSender, int a_nIdx) {
			this.UpdateREUIsPageUIs02(m_oUIsDict.GetValueOrDefault(EKey.RE_UIS_PAGE_UIS_02));
		}

		/** 오른쪽 에디터 UI 페이지 UI 1 적용 버튼을 눌렀을 경우 */
		private void OnTouchREUIsPageUIs01ApplyBtn() {
			bool bIsValid01 = int.TryParse(m_oInputDict[EKey.RE_UIS_PAGE_UIS_01_NUM_CELLS_X_INPUT]?.text, NumberStyles.Any, null, out int nNumCellsX);
			bool bIsValid02 = int.TryParse(m_oInputDict[EKey.RE_UIS_PAGE_UIS_01_NUM_CELLS_Y_INPUT]?.text, NumberStyles.Any, null, out int nNumCellsY);

			bool bIsValidNumCellsX = Mathf.Max(nNumCellsX, NSEngine.KDefine.E_MIN_NUM_CELLS.x) != this.SelLevelInfo.NumCells.x;
			bool bIsValidNumCellsY = Mathf.Max(nNumCellsY, NSEngine.KDefine.E_MIN_NUM_CELLS.y) != this.SelLevelInfo.NumCells.y;

			// 셀 개수가 유효 할 경우
			if(bIsValid01 && bIsValid02 && (bIsValidNumCellsX || bIsValidNumCellsY)) {
				Func.SetupEditorLevelInfo(this.SelLevelInfo, new CSubEditorCreateInfo() {
					m_nNumLevels = KCDefine.B_VAL_0_INT,
					m_stMinNumCells = new Vector3Int(nNumCellsX, nNumCellsY, NSEngine.KDefine.E_MIN_NUM_CELLS.z),
					m_stMaxNumCells = new Vector3Int(nNumCellsX, nNumCellsY, NSEngine.KDefine.E_MIN_NUM_CELLS.z)
				}, false);

				this.UpdateUIsState();
				this.SetMEUIsGridScrollDelta(m_oRealDict[EKey.GRID_SCROLL_DELTA_X], m_oRealDict[EKey.GRID_SCROLL_DELTA_Y]);
			}
		}

		/** 오른쪽 에디터 UI 페이지 UI 1 레벨 로드 버튼을 눌렀을 경우 */
		private void OnTouchREUIsPageUIs01LoadLevelBtn() {
			// 식별자가 유효 할 경우
			if(int.TryParse(m_oInputDict[EKey.RE_UIS_PAGE_UIS_01_LEVEL_INPUT]?.text, NumberStyles.Any, null, out int nID)) {
				int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(this.SelLevelInfo.m_stIDInfo.m_nID02, this.SelLevelInfo.m_stIDInfo.m_nID03);
				this.SetSelLevelInfo(CLevelInfoTable.Inst.GetLevelInfo(Mathf.Clamp(nID, KCDefine.B_VAL_1_INT, nNumLevelInfos) - KCDefine.B_VAL_1_INT, this.SelLevelInfo.m_stIDInfo.m_nID02, this.SelLevelInfo.m_stIDInfo.m_nID03));

				this.UpdateUIsState();
			}
		}

		/** 오른쪽 에디터 UI 페이지 UI 1 모든 레벨 제거 버튼을 눌렀을 경우 */
		private void OnTouchREUIsPageUIs01RemoveAllLevelsBtn() {
			m_oScrollerDict[EKey.SEL_SCROLLER] = m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].m_oScroller;
			m_oInputPopupDict[EKey.SEL_INPUT_POPUP] = EInputPopup.REMOVE_LEVEL;

			Func.ShowEditorInputPopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CEditorInputPopup).Init(CEditorInputPopup.MakeParams(new Dictionary<CEditorInputPopup.ECallback, System.Action<CEditorInputPopup, string, bool>>() {
					[CEditorInputPopup.ECallback.OK_CANCEL] = this.OnReceiveEditorInputPopupResult
				}));
			});
		}

		/** 오른쪽 에디터 UI 페이지 UI 1 모든 셀 채우기 버튼을 눌렀을 경우 */
		private void OnTouchREUIsPageUIs01FillAllCellsBtn() {
			// 그리드 정보가 존재 할 경우
			if(m_oObjKindsDict[EKey.SEL_OBJ_KINDS].ExIsValid() && m_oGridInfoList.ExIsValidIdx(this.SelGridInfoIdx)) {
				for(int i = 0; i < this.SelLevelInfo.m_oCellInfoDictContainer.Count; ++i) {
					for(int j = 0; j < this.SelLevelInfo.m_oCellInfoDictContainer[i].Count; ++j) {
						// 객체 추가가 가능 할 경우
						if(m_oObjKindsDict[EKey.SEL_OBJ_KINDS].ExIsValid()) {
							var stIdx = new Vector3Int(j, i, KCDefine.B_VAL_0_INT);
							this.AddCellObjInfo(Factory.MakeEditorCellObjInfo(m_oObjKindsDict[EKey.SEL_OBJ_KINDS], this.GetEditorObjSize(), stIdx, drawCellColor), stIdx, Input.GetKey(KeyCode.LeftShift));
						}
					}
				}

				this.UpdateUIsState();
			}
		}

		/** 오른쪽 에디터 UI 페이지 UI 1 모든 셀 지우기 버튼을 눌렀을 경우 */
		private void OnTouchREUIsPageUIs01ClearAllCellsBtn() {
			// 그리드 정보가 존재 할 경우
			if(m_oGridInfoList.ExIsValidIdx(this.SelGridInfoIdx)) {
				for(int i = 0; i < this.SelLevelInfo.m_oCellInfoDictContainer.Count; ++i) {
					for(int j = 0; j < this.SelLevelInfo.m_oCellInfoDictContainer[i].Count; ++j) {
						this.RemoveAllCellObjInfos(EObjKinds.NONE, new Vector3Int(j, i, KCDefine.B_VAL_0_INT));
					}
				}

				this.UpdateUIsState();
			}
		}

		/** 오른쪽 에디터 UI 페이지 UI 1 선택 셀 지우기 버튼을 눌렀을 경우 */
		private void OnTouchREUIsPageUIs01ClearSelCellsBtn() {
			// 그리드 정보가 존재 할 경우
			if(m_oObjKindsDict[EKey.SEL_OBJ_KINDS].ExIsValid() && m_oGridInfoList.ExIsValidIdx(this.SelGridInfoIdx)) {
				for(int i = 0; i < this.SelLevelInfo.m_oCellInfoDictContainer.Count; ++i) {
					for(int j = 0; j < this.SelLevelInfo.m_oCellInfoDictContainer[i].Count; ++j) {
						this.RemoveAllCellObjInfos(m_oObjKindsDict[EKey.SEL_OBJ_KINDS], new Vector3Int(j, i, KCDefine.B_VAL_0_INT));
					}
				}

				this.UpdateUIsState();
			}
		}

		/** 오른쪽 에디터 UI 페이지 UI 1 모든 셀 이동 버튼을 눌렀을 경우 */
		private void OnTouchREUIsPageUIs01MoveAllCellsBtn(EDirection a_eDirection) {
			// 모든 셀 이동이 가능 할 경우
			if(this.IsEnableMoveAllCells(a_eDirection)) {
				var stIdx = this.GetGridBaseIdx(a_eDirection);
				var stOffset = stIdx.ExGetNextIdx(a_eDirection) - stIdx;

				for(int i = 0; i < this.SelLevelInfo.NumCells.y; ++i) {
					for(int j = 0; j < this.SelLevelInfo.NumCells.x; ++j) {
						var stCellIdx = a_eDirection.ExIsVertical() ? new Vector3Int(stIdx.x + j, stIdx.y + (i * stOffset.y), KCDefine.B_VAL_0_INT) : new Vector3Int(stIdx.x + (j * -stOffset.x), stIdx.y + i, KCDefine.B_VAL_0_INT);
						var stNextCellIdx = a_eDirection.ExIsVertical() ? new Vector3Int(stIdx.x + j, (stIdx.y + stOffset.y) + (i * stOffset.y), KCDefine.B_VAL_0_INT) : new Vector3Int((stIdx.x - stOffset.x) - (j * stOffset.x), stIdx.y + i, KCDefine.B_VAL_0_INT);

						this.SelLevelInfo.m_oCellInfoDictContainer[stCellIdx.y][stCellIdx.x].m_oCellObjInfoList.Clear();

						// 셀 정보가 존재 할 경우
						if((a_eDirection.ExIsVertical() && i < this.SelLevelInfo.NumCells.y - KCDefine.B_VAL_1_INT) || (a_eDirection.ExIsHorizontal() && j < this.SelLevelInfo.NumCells.x - KCDefine.B_VAL_1_INT)) {
							this.SelLevelInfo.m_oCellInfoDictContainer[stNextCellIdx.y][stNextCellIdx.x].m_oCellObjInfoList.ExCopyTo(this.SelLevelInfo.m_oCellInfoDictContainer[stCellIdx.y][stCellIdx.x].m_oCellObjInfoList, (a_stCellObjInfo) => a_stCellObjInfo);
							this.SelLevelInfo.m_oCellInfoDictContainer[stCellIdx.y][stCellIdx.x].OnAfterDeserialize();
						}
					}
				}

				this.SetMEUIsGridScrollDelta(m_oRealDict[EKey.GRID_SCROLL_DELTA_X] + (stOffset.x * -NSEngine.Access.CellSize.x), m_oRealDict[EKey.GRID_SCROLL_DELTA_Y] + (stOffset.y * -NSEngine.Access.CellSize.y));
			}
		}

		/** 오른쪽 에디터 UI 페이지 UI 1 테이블 로드 버튼을 눌렀을 경우 */
		private void OnTouchREUIsPageUIs01LoadTableBtn(ETableSrc a_eTableSrc) {
			m_oTableSrcDict[EKey.SEL_TABLE_SRC] = a_eTableSrc;
			Func.ShowAlertPopup((a_eTableSrc == ETableSrc.LOCAL) ? KDefine.ES_MSG_ALERT_P_LOAD_LOCAL_TABLE : KDefine.ES_MSG_ALERT_P_LOAD_REMOTE_TABLE, this.OnReceiveEditorTableLoadPopupResult);
		}

		/** 오른쪽 에디터 UI 페이지 UI 2 스크롤러 셀 뷰 버튼을 눌렀을 경우 */
		private void OnTouchREUIsPageUIs02ScrollerCellViewBtn(EObjKinds a_eObjKinds) {
			m_oObjKindsDict[EKey.SEL_OBJ_KINDS] = a_eObjKinds;

			// 객체 정보가 존재 할 경우
			if(Input.GetMouseButtonUp((int)EMouseBtn.LEFT) && CObjInfoTable.Inst.TryGetObjInfo(a_eObjKinds, out STObjInfo stObjInfo)) {
				this.SetREUIsPageUIs02ObjSize((int)stObjInfo.m_stSize.x, (int)stObjInfo.m_stSize.y);

				#region 추가
				int nHP = int.Parse(m_oSubInputDict[ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_HP_INPUT].text, NumberStyles.Any, null);
				decimal dmATK = stObjInfo.m_oAbilityTargetInfoDict.ExGetTargetVal(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_ATK_01);

				this.SetREUIsPageUIs02CellObjInfo(nHP, (int)dmATK);
				#endregion // 추가
			}

			this.UpdateUIsState();
		}

		/** 오른쪽 에디터 UI 페이지 UI 1 레벨 입력 문자열을 변경했을 경우 */
		private void OnChangeREUIsPageUIs01LevelInputStr(string a_oStr) {
			bool bIsValid = int.TryParse(m_oInputDict[EKey.RE_UIS_PAGE_UIS_01_LEVEL_INPUT]?.text, NumberStyles.Any, null, out int nID);
			m_oInputDict[EKey.RE_UIS_PAGE_UIS_01_LEVEL_INPUT]?.SetTextWithoutNotify($"{Mathf.Max(nID, KCDefine.B_VAL_1_INT)}");
		}

		/** 오른쪽 에디터 UI 페이지 UI 1 셀 개수 입력 문자열을 변경했을 경우 */
		private void OnChangeREUIsPageUIs01NumCellsInputStr(string a_oStr) {
			bool bIsValid01 = int.TryParse(m_oInputDict[EKey.RE_UIS_PAGE_UIS_01_NUM_CELLS_X_INPUT]?.text, NumberStyles.Any, null, out int nNumCellsX);
			bool bIsValid02 = int.TryParse(m_oInputDict[EKey.RE_UIS_PAGE_UIS_01_NUM_CELLS_Y_INPUT]?.text, NumberStyles.Any, null, out int nNumCellsY);

			this.SetREUIsPageUIs01NumCells(bIsValid01 ? nNumCellsX : KCDefine.B_VAL_1_INT, bIsValid02 ? nNumCellsY : KCDefine.B_VAL_1_INT);
		}

		/** 오른쪽 에디터 UI 페이지 UI 2 객체 크기 입력 문자열을 변경했을 경우 */
		private void OnChangeREUIsPageUIs02ObjSizeInputStr(string a_oStr) {
			bool bIsValid01 = int.TryParse(m_oInputDict[EKey.RE_UIS_PAGE_UIS_02_OBJ_SIZE_X_INPUT]?.text, NumberStyles.Any, null, out int nSizeX);
			bool bIsValid02 = int.TryParse(m_oInputDict[EKey.RE_UIS_PAGE_UIS_02_OBJ_SIZE_Y_INPUT]?.text, NumberStyles.Any, null, out int nSizeY);

			this.SetREUIsPageUIs02ObjSize(bIsValid01 ? nSizeX : KCDefine.B_VAL_1_INT, bIsValid02 ? nSizeY : KCDefine.B_VAL_1_INT);
		}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		#endregion // 조건부 함수

		#region 조건부 접근자 함수
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		/** 그리드 기본 인덱스를 반환한다 */
		private Vector3Int GetGridBaseIdx(EDirection a_eDirection) {
			switch(a_eDirection) {
				case EDirection.UP: return new Vector3Int(KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT);
				case EDirection.DOWN: return new Vector3Int(KCDefine.B_VAL_0_INT, this.SelLevelInfo.NumCells.y - KCDefine.B_VAL_1_INT, KCDefine.B_VAL_0_INT);
				case EDirection.LEFT: return new Vector3Int(KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT);
				case EDirection.RIGHT: return new Vector3Int(this.SelLevelInfo.NumCells.x - KCDefine.B_VAL_1_INT, KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT);
			}

			return KCDefine.B_IDX_INVALID_3D;
		}

		/** 오른쪽 에디터 UI 페이지 UI 1 셀 개수를 변경한다 */
		private void SetREUIsPageUIs01NumCells(int a_nNumCellsX, int a_nNumCellsY) {
			m_oInputDict[EKey.RE_UIS_PAGE_UIS_01_NUM_CELLS_X_INPUT]?.SetTextWithoutNotify($"{Mathf.Clamp(a_nNumCellsX, NSEngine.KDefine.E_MIN_NUM_CELLS.x, NSEngine.KDefine.E_MAX_NUM_CELLS.x)}");
			m_oInputDict[EKey.RE_UIS_PAGE_UIS_01_NUM_CELLS_Y_INPUT]?.SetTextWithoutNotify($"{Mathf.Clamp(a_nNumCellsY, NSEngine.KDefine.E_MIN_NUM_CELLS.y, NSEngine.KDefine.E_MAX_NUM_CELLS.y)}");
		}

		/** 오른쪽 에디터 UI 페이지 UI 2 객체 크기를 변경한다 */
		private void SetREUIsPageUIs02ObjSize(int a_nSizeX, int a_nSizeY) {
			m_oInputDict[EKey.RE_UIS_PAGE_UIS_02_OBJ_SIZE_X_INPUT]?.SetTextWithoutNotify($"{Mathf.Clamp(a_nSizeX, KCDefine.B_VAL_1_INT, this.SelLevelInfo.NumCells.x)}");
			m_oInputDict[EKey.RE_UIS_PAGE_UIS_02_OBJ_SIZE_Y_INPUT]?.SetTextWithoutNotify($"{Mathf.Clamp(a_nSizeY, KCDefine.B_VAL_1_INT, this.SelLevelInfo.NumCells.y)}");

			this.UpdateUIsState();
		}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		#endregion // 조건부 접근자 함수
	}

	/** 서브 레벨 에디터 씬 관리자 - 스크롤러 셀 뷰 */
	public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
		#region 조건부 함수
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		/** 선택 콜백을 수신했을 경우 */
		private void OnReceiveSelCallback(CScrollerCellView a_oSender, ulong a_nID) {
			m_oRealDict[EKey.GRID_SCROLL_DELTA_X] = KCDefine.B_VAL_0_REAL;
			m_oRealDict[EKey.GRID_SCROLL_DELTA_Y] = KCDefine.B_VAL_0_REAL;

			this.SetSelLevelInfo(CLevelInfoTable.Inst.GetLevelInfo(a_nID.ExULevelIDToLevelID(), a_nID.ExULevelIDToStageID(), a_nID.ExULevelIDToChapterID()));
			this.UpdateUIsState();
		}

		/** 복사 콜백을 수신했을 경우 */
		private void OnReceiveCopyCallback(CScrollerCellView a_oSender, ulong a_nID) {
			int nNumInfos = CLevelInfoTable.Inst.GetNumLevelInfos(a_nID.ExULevelIDToStageID(), a_nID.ExULevelIDToChapterID());
			int nMaxNumInfos = KCDefine.U_MAX_NUM_LEVEL_INFOS;

			// 레벨 스크롤러가 아닐 경우
			if(m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].m_oScroller != a_oSender.Params.m_oScroller) {
				nNumInfos = (m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].m_oScroller == a_oSender.Params.m_oScroller) ? CLevelInfoTable.Inst.GetNumStageInfos(a_nID.ExULevelIDToChapterID()) : CLevelInfoTable.Inst.NumChapterInfos;
				nMaxNumInfos = (m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].m_oScroller == a_oSender.Params.m_oScroller) ? KCDefine.U_MAX_NUM_STAGE_INFOS : KCDefine.U_MAX_NUM_CHAPTER_INFOS;
			}

			// 복사가 가능 할 경우
			if(nNumInfos < nMaxNumInfos) {
				this.CopyLevelInfos(a_oSender.Params.m_oScroller, new STIDInfo(a_nID.ExULevelIDToLevelID(), a_nID.ExULevelIDToStageID(), a_nID.ExULevelIDToChapterID()));
			}
		}

		/** 이동 콜백을 수신했을 경우 */
		private void OnReceiveMoveCallback(CScrollerCellView a_oSender, ulong a_nID) {
			m_oScrollerDict[EKey.SEL_SCROLLER] = a_oSender.Params.m_oScroller;
			m_oInputPopupDict[EKey.SEL_INPUT_POPUP] = EInputPopup.MOVE_LEVEL;

			Func.ShowEditorInputPopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CEditorInputPopup).Init(CEditorInputPopup.MakeParams(new Dictionary<CEditorInputPopup.ECallback, System.Action<CEditorInputPopup, string, bool>>() {
					[CEditorInputPopup.ECallback.OK_CANCEL] = this.OnReceiveEditorInputPopupResult
				}));
			});
		}

		/** 제거 콜백을 수신했을 경우 */
		private void OnReceiveRemoveCallback(CScrollerCellView a_oSender, ulong a_nID) {
			m_oScrollerDict[EKey.SEL_SCROLLER] = a_oSender.Params.m_oScroller;

			// 레벨 스크롤러 일 경우
			if(m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].m_oScroller == a_oSender.Params.m_oScroller) {
				Func.ShowAlertPopup(KDefine.ES_MSG_ALERT_P_REMOVE_LEVEL, (a_oPopupSender, a_bIsOK) => this.OnReceiveEditorRemovePopupResult(a_oPopupSender, a_nID.ExULevelIDToIDInfo(), a_bIsOK));
			}
			// 스테이지 스크롤러 일 경우
			else if(m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].m_oScroller == a_oSender.Params.m_oScroller) {
				Func.ShowAlertPopup(KDefine.ES_MSG_ALERT_P_REMOVE_STAGE, (a_oPopupSender, a_bIsOK) => this.OnReceiveEditorRemovePopupResult(a_oPopupSender, a_nID.ExULevelIDToIDInfo(), a_bIsOK));
			}
			// 챕터 스크롤러 일 경우
			else if(m_oScrollerInfoDict[EKey.LE_UIS_CHAPTER_SCROLLER_INFO].m_oScroller == a_oSender.Params.m_oScroller) {
				Func.ShowAlertPopup(KDefine.ES_MSG_ALERT_P_REMOVE_CHAPTER, (a_oPopupSender, a_bIsOK) => this.OnReceiveEditorRemovePopupResult(a_oPopupSender, a_nID.ExULevelIDToIDInfo(), a_bIsOK));
			}
		}

		/** 레벨 스크롤러 셀 뷰를 생성한다 */
		private EnhancedScrollerCellView CreateLevelScrollerCellView(EnhancedScroller a_oSender, int a_nDataIdx, int a_nCellIdx, Dictionary<CScrollerCellView.ECallback, System.Action<CScrollerCellView, ulong>> a_oCallbackDict01, Dictionary<CEditorScrollerCellView.ECallback, System.Action<CEditorScrollerCellView, ulong>> a_oCallbackDict02) {
			string oName = string.Format(KCDefine.LES_TEXT_FMT_LEVEL, a_nDataIdx + KCDefine.B_VAL_1_INT);
			string oScrollerCellViewName = string.Format(KCDefine.B_TEXT_FMT_2_SPACE_COMBINE, oName, string.Empty);

			var oScrollerCellView = a_oSender.GetCellView(m_oScrollerInfoDict[EKey.LE_UIS_LEVEL_SCROLLER_INFO].m_oScrollerCellView) as CEditorScrollerCellView;
			oScrollerCellView.Init(CEditorScrollerCellView.MakeParams(CFactory.MakeULevelID(a_nDataIdx, this.SelLevelInfo.m_stIDInfo.m_nID02, this.SelLevelInfo.m_stIDInfo.m_nID03), a_oSender, a_oCallbackDict01, a_oCallbackDict02));

			oScrollerCellView.NameText?.ExSetText<Text>(oScrollerCellViewName, false);
			oScrollerCellView.SelBtn?.image.ExSetColor<Image>((this.SelLevelInfo.m_stIDInfo.m_nID01 == a_nDataIdx) ? KCDefine.U_COLOR_NORM : KCDefine.U_COLOR_DISABLE, false);

			oScrollerCellView.MoveBtn?.ExSetInteractable(CLevelInfoTable.Inst.GetNumLevelInfos(this.SelLevelInfo.m_stIDInfo.m_nID02, this.SelLevelInfo.m_stIDInfo.m_nID03) > KCDefine.B_VAL_1_INT, false);
			oScrollerCellView.RemoveBtn?.ExSetInteractable(CLevelInfoTable.Inst.GetNumLevelInfos(this.SelLevelInfo.m_stIDInfo.m_nID02, this.SelLevelInfo.m_stIDInfo.m_nID03) > KCDefine.B_VAL_1_INT, false);

			return oScrollerCellView;
		}

		/** 스테이지 스크롤러 셀 뷰를 생성한다 */
		private EnhancedScrollerCellView CreateStageScrollerCellView(EnhancedScroller a_oSender, int a_nDataIdx, int a_nCellIdx, Dictionary<CScrollerCellView.ECallback, System.Action<CScrollerCellView, ulong>> a_oCallbackDict01, Dictionary<CEditorScrollerCellView.ECallback, System.Action<CEditorScrollerCellView, ulong>> a_oCallbackDict02) {
			string oName = string.Format(KCDefine.LES_TEXT_FMT_STAGE, a_nDataIdx + KCDefine.B_VAL_1_INT);
			string oExtraName = string.Format(KCDefine.B_TEXT_FMT_BRACKET, CLevelInfoTable.Inst.GetNumLevelInfos(a_nDataIdx, this.SelLevelInfo.m_stIDInfo.m_nID03));
			string oScrollerCellViewName = string.Format(KCDefine.B_TEXT_FMT_2_COMBINE, oName, KCDefine.B_TEXT_NEW_LINE, oExtraName.ExGetColorFmtStr(Color.red));

			var oScrollerCellView = a_oSender.GetCellView(m_oScrollerInfoDict[EKey.LE_UIS_STAGE_SCROLLER_INFO].m_oScrollerCellView) as CEditorScrollerCellView;
			oScrollerCellView.Init(CEditorScrollerCellView.MakeParams(CFactory.MakeUStageID(a_nDataIdx, this.SelLevelInfo.m_stIDInfo.m_nID03), a_oSender, a_oCallbackDict01, a_oCallbackDict02));

			oScrollerCellView.NameText?.ExSetText<Text>(oScrollerCellViewName, false);
			oScrollerCellView.SelBtn?.image.ExSetColor<Image>((this.SelLevelInfo.m_stIDInfo.m_nID02 == a_nDataIdx) ? KCDefine.U_COLOR_NORM : KCDefine.U_COLOR_DISABLE, false);

			oScrollerCellView.MoveBtn?.ExSetInteractable(CLevelInfoTable.Inst.GetNumStageInfos(this.SelLevelInfo.m_stIDInfo.m_nID03) > KCDefine.B_VAL_1_INT, false);
			oScrollerCellView.RemoveBtn?.ExSetInteractable(CLevelInfoTable.Inst.GetNumStageInfos(this.SelLevelInfo.m_stIDInfo.m_nID03) > KCDefine.B_VAL_1_INT, false);

			return oScrollerCellView;
		}

		/** 챕터 스크롤러 셀 뷰를 생성한다 */
		private EnhancedScrollerCellView CreateChapterScrollerCellView(EnhancedScroller a_oSender, int a_nDataIdx, int a_nCellIdx, Dictionary<CScrollerCellView.ECallback, System.Action<CScrollerCellView, ulong>> a_oCallbackDict01, Dictionary<CEditorScrollerCellView.ECallback, System.Action<CEditorScrollerCellView, ulong>> a_oCallbackDict02) {
			string oName = string.Format(KCDefine.LES_TEXT_FMT_CHAPTER, a_nDataIdx + KCDefine.B_VAL_1_INT);
			string oExtraName = string.Format(KCDefine.B_TEXT_FMT_BRACKET, CLevelInfoTable.Inst.GetNumStageInfos(a_nDataIdx));
			string oScrollerCellViewName = string.Format(KCDefine.B_TEXT_FMT_2_COMBINE, oName, KCDefine.B_TEXT_NEW_LINE, oExtraName.ExGetColorFmtStr(Color.red));

			var oScrollerCellView = a_oSender.GetCellView(m_oScrollerInfoDict[EKey.LE_UIS_CHAPTER_SCROLLER_INFO].m_oScrollerCellView) as CEditorScrollerCellView;
			oScrollerCellView.Init(CEditorScrollerCellView.MakeParams(CFactory.MakeUChapterID(a_nDataIdx), a_oSender, a_oCallbackDict01, a_oCallbackDict02));

			oScrollerCellView.NameText?.ExSetText<Text>(oScrollerCellViewName, false);
			oScrollerCellView.SelBtn?.image.ExSetColor<Image>((this.SelLevelInfo.m_stIDInfo.m_nID03 == a_nDataIdx) ? KCDefine.U_COLOR_NORM : KCDefine.U_COLOR_DISABLE, false);

			oScrollerCellView.MoveBtn?.ExSetInteractable(CLevelInfoTable.Inst.NumChapterInfos > KCDefine.B_VAL_1_INT, false);
			oScrollerCellView.RemoveBtn?.ExSetInteractable(CLevelInfoTable.Inst.NumChapterInfos > KCDefine.B_VAL_1_INT, false);

			return oScrollerCellView;
		}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		#endregion // 조건부 함수
	}
}
#endif // #if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Linq;

#if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
using System.Globalization;
using UnityEngine.EventSystems;
using TMPro;
using EnhancedUI.EnhancedScroller;

namespace LevelEditorScene {
	/** 서브 레벨 에디터 씬 관리자 - 서브 */
	public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
            ME_UIS_CELL_OBJ_HP_TEXT,
            ME_UIS_CELL_OBJ_SHIELD_TEXT,
			ME_UIS_CELL_OBJ_ATK_TEXT,
            ME_UIS_CELL_OBJ_COLOR_TEXT,
            ME_UIS_CELL_OBJ_COUNT_TEXT,
            ME_UIS_CELL_OBJ_SCORE_1_TEXT,
            ME_UIS_CELL_OBJ_SCORE_2_TEXT,
            ME_UIS_CELL_OBJ_SCORE_3_TEXT,
            RE_UIS_PAGE_UIS_02_CELL_OBJ_COLOR_DROP,
			RE_UIS_PAGE_UIS_02_CELL_OBJ_HP_INPUT,
            RE_UIS_PAGE_UIS_02_CELL_OBJ_SHIELD_INPUT,
			RE_UIS_PAGE_UIS_02_CELL_OBJ_ATK_INPUT,
            
			[HideInInspector] MAX_VAL
		}

		#region 변수
		/** =====> UI <===== */
		private Dictionary<ESubKey, Text> m_oSubTextDict = new Dictionary<ESubKey, Text>();
        private Dictionary<ESubKey, Dropdown> m_oSubDropDict = new Dictionary<ESubKey, Dropdown>();
		private Dictionary<ESubKey, InputField> m_oSubInputDict = new Dictionary<ESubKey, InputField>();

		/** =====> 객체 <===== */
		[SerializeField] private GameObject m_oSubCellObjHPBtnUIs = null;
        [SerializeField] private GameObject m_oSubCellObjShieldBtnUIs = null;
		[SerializeField] private GameObject m_oSubCellObjATKBtnUIs = null;
		#endregion // 변수

		#region 프로퍼티

		#endregion // 프로퍼티

		#region 함수

		#endregion // 함수

		#region 조건부 함수
#if UNITY_EDITOR
		/** 기즈모를 그린다 */
		public override void OnDrawGizmos() {
			base.OnDrawGizmos();

			// 메인 카메라가 존재 할 경우
			if(CSceneManager.IsExistsMainCamera) {
				var stPrevColor = Gizmos.color;
				var stMainCameraPos = CSceneManager.ActiveSceneMainCamera.transform.position;
				var stPivotPos = stMainCameraPos + new Vector3(KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_0_REAL, this.PlaneDistance);

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
				try {
					var oGridPosList = new List<Vector3>() {
						stPivotPos + this.ObjRootPivotPos + new Vector3(NSEngine.Access.MaxGridSize.x / -KCDefine.B_VAL_2_REAL, NSEngine.Access.MaxGridSize.y / -KCDefine.B_VAL_2_REAL, 0.0f) * CAccess.ResolutionUnitScale,
						stPivotPos + this.ObjRootPivotPos + new Vector3(NSEngine.Access.MaxGridSize.x / -KCDefine.B_VAL_2_REAL, NSEngine.Access.MaxGridSize.y / KCDefine.B_VAL_2_REAL, 0.0f) * CAccess.ResolutionUnitScale,
						stPivotPos + this.ObjRootPivotPos + new Vector3(NSEngine.Access.MaxGridSize.x / KCDefine.B_VAL_2_REAL, NSEngine.Access.MaxGridSize.y / KCDefine.B_VAL_2_REAL, 0.0f) * CAccess.ResolutionUnitScale,
						stPivotPos + this.ObjRootPivotPos + new Vector3(NSEngine.Access.MaxGridSize.x / KCDefine.B_VAL_2_REAL, NSEngine.Access.MaxGridSize.y / -KCDefine.B_VAL_2_REAL, 0.0f) * CAccess.ResolutionUnitScale
					};

					for(int i = 0; i < oGridPosList.Count; ++i) {
						int nIdx01 = (i + KCDefine.B_VAL_0_INT) % oGridPosList.Count;
						int nIdx02 = (i + KCDefine.B_VAL_1_INT) % oGridPosList.Count;

						Gizmos.color = Color.magenta;
						Gizmos.DrawLine(oGridPosList[nIdx01], oGridPosList[nIdx02]);
					}
				} finally {
					Gizmos.color = stPrevColor;
				}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
			}
		}
#endif // #if UNITY_EDITOR

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		/** 초기화 */
		private void SubAwake() {
			this.SubSetupMidEditorUIs();
			this.SubSetupLeftEditorUIs();
			this.SubSetupRightEditorUIs();
		}

		/** 초기화 */
		private void SubStart() {
			ExtraStart();
		}

		/** 상태를 갱신한다 */
		private void SubOnUpdate(float a_fDeltaTime) {
			// Do Something
		}

		/** UI 상태를 갱신한다 */
		private void SubUpdateUIsState() {
			this.SubUpdateMidEditorUIsState();
			this.SubUpdateLeftEditorUIsState();
			this.SubUpdateRightEditorUIsState();
		}

		/** 객체 스프라이트를 설정한다 */
		private void SubSetupObjSprite(STCellInfo a_stCellInfo, STCellObjInfo a_stCellObjInfo, SpriteRenderer a_oOutObjSprite) {
            CObjInfoTable.Inst.TryGetObjInfo(a_stCellObjInfo.ObjKinds, out STObjInfo stObjInfo);

            var subSprite = a_oOutObjSprite.gameObject.GetComponentsInChildren<SpriteRenderer>(true).FirstOrDefault(x => x.gameObject != a_oOutObjSprite.gameObject);
		    subSprite = subSprite ?? CFactory.CreateCloneObj<SpriteRenderer>(GlobalDefine.OBJECTNAME_SUB_SPRITE, CResManager.Inst.GetRes<GameObject>(GlobalDefine.PREFAB_SUB_SPRITE), a_oOutObjSprite.gameObject);
            
            if (GlobalDefine.IsNeedSubSprite(a_stCellObjInfo.ObjKinds))
            {
                this.SetupSpriteGrid(a_stCellObjInfo, a_stCellObjInfo.ObjKinds, subSprite, Access.GetEditorObjSprite(a_stCellObjInfo.ObjKinds, KCDefine.B_PREFIX_LEVEL_EDITOR_SCENE));
                subSprite.color = GlobalDefine.COLOR_WHITE;
                subSprite.gameObject.SetActive(true);

                this.SetupSpriteGrid(a_stCellObjInfo, EObjKinds.NORM_BRICKS_SQUARE_01, a_oOutObjSprite, Access.GetEditorObjSprite(EObjKinds.NORM_BRICKS_SQUARE_01, KCDefine.B_PREFIX_LEVEL_EDITOR_SCENE));
            }
            else
            {
                this.SetupSpriteGrid(a_stCellObjInfo, EObjKinds.NORM_BRICKS_SQUARE_01, subSprite, Access.GetEditorObjSprite(EObjKinds.NORM_BRICKS_SQUARE_01, KCDefine.B_PREFIX_LEVEL_EDITOR_SCENE));
                subSprite.color = GlobalDefine.COLOR_WHITE;
                subSprite.gameObject.SetActive(false);
            }

			var oInfoText = a_oOutObjSprite.gameObject.GetComponentInChildren<TMP_Text>();
			oInfoText = oInfoText ?? CFactory.CreateCloneObj<TMP_Text>(KCDefine.U_OBJ_N_TMP_TEXT, CResManager.Inst.GetRes<GameObject>(KDefine.LES_OBJ_P_TMP_TEXT), a_oOutObjSprite.gameObject);

            RefreshText(a_stCellObjInfo, stObjInfo, oInfoText);

			oInfoText.gameObject.SetActive(a_stCellObjInfo.ObjKinds != EObjKinds.BG_PLACEHOLDER_01 && stObjInfo.m_eColliderType != EColliderType.NONE);

            (subSprite as SpriteRenderer).ExSetSortingOrder(new STSortingOrderInfo() {
				m_nOrder = a_oOutObjSprite.sortingOrder + KCDefine.B_VAL_1_INT, m_oLayer = a_oOutObjSprite.sortingLayerName
			});

			(oInfoText as TextMeshPro).ExSetSortingOrder(new STSortingOrderInfo() {
				m_nOrder = a_oOutObjSprite.sortingOrder + KCDefine.B_VAL_2_INT, m_oLayer = a_oOutObjSprite.sortingLayerName
			});
		}

		/** 터치 시작 이벤트를 처리한다 */
		private void HandleTouchBeginEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			var stPos = a_oEventData.ExGetLocalPos(this.ObjRoot, this.ScreenSize);
			var stIdx = stPos.ExToIdx(this.SelGridInfo.m_stPivotPos, NSEngine.Access.CellSize);

			// 인덱스가 유효 할 경우
			if(this.SelLevelInfo.m_oCellInfoDictContainer.ExIsValidIdx(stIdx) && !stIdx.Equals(m_oVec3IntDict[EKey.PREV_CELL_IDX])) {
				bool bIsValid01 = this.TryGetCellObjInfo(stIdx, EObjKinds.NONE, out STCellObjInfo stCellObjInfo);
                bool bIsValid02 = Input.GetKey(CAccess.CmdKeyCode) && Input.GetMouseButtonDown((int)EMouseBtn.LEFT);
                bool bIsValid03 = Input.GetKey(KeyCode.M) && Input.GetMouseButtonDown((int)EMouseBtn.LEFT);
				
                // 셀 객체 정보가 존재 할 경우
				if(bIsValid01 && stCellObjInfo.ObjKinds != EObjKinds.BG_PLACEHOLDER_01) 
                {
                    if (bIsValid02)
                    {
                        this.CopyCurrentCellInfo(a_oSender, a_oEventData);
                        return;
                    }
                    else if (bIsValid03)
                    {
                        this.ModifyCurrentCellInfo(a_oSender, a_oEventData);
                        return;
                    }
				}

                // 특수 명령 단축키 사용이 아니면 통상 그리기.
                switch(m_oEditorModeDict[EKey.SEL_EDITOR_MODE]) {
                    case EEditorMode.DRAW: this.HandleDrawEditorModeTouchMoveEvent(a_oSender, a_oEventData); break;
                    case EEditorMode.PAINT: this.HandlePaintEditorModeTouchMoveEvent(a_oSender, a_oEventData); break;
                }

                this.UpdateUIsState();
                m_oVec3IntDict[EKey.PREV_CELL_IDX] = stIdx;
			}

			this.HandleTouchMoveEvent(a_oSender, a_oEventData);
		}

		/** 터치 이동 이벤트를 처리한다 */
		private void HandleTouchMoveEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			var stPos = a_oEventData.ExGetLocalPos(this.ObjRoot, this.ScreenSize);
			var stIdx = stPos.ExToIdx(this.SelGridInfo.m_stPivotPos, NSEngine.Access.CellSize);

			// 인덱스가 유효 할 경우
			if(this.SelLevelInfo.m_oCellInfoDictContainer.ExIsValidIdx(stIdx) && !stIdx.Equals(m_oVec3IntDict[EKey.PREV_CELL_IDX])) {
				switch(m_oEditorModeDict[EKey.SEL_EDITOR_MODE]) {
					case EEditorMode.DRAW: this.HandleDrawEditorModeTouchMoveEvent(a_oSender, a_oEventData); break;
					case EEditorMode.PAINT: this.HandlePaintEditorModeTouchMoveEvent(a_oSender, a_oEventData); break;
				}

				this.UpdateUIsState();
				m_oVec3IntDict[EKey.PREV_CELL_IDX] = stIdx;
			}
		}

		/** 터치 종료 이벤트를 처리한다 */
		private void HandleTouchEndEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			var stPos = a_oEventData.ExGetLocalPos(this.ObjRoot, this.ScreenSize);
			var stIdx = stPos.ExToIdx(this.SelGridInfo.m_stPivotPos, NSEngine.Access.CellSize);

			// 인덱스가 유효 할 경우
			if(this.SelLevelInfo.m_oCellInfoDictContainer.ExIsValidIdx(stIdx) && !stIdx.Equals(m_oVec3IntDict[EKey.PREV_CELL_IDX])) {
				// Do Something
			}

			m_oVec3IntDict[EKey.PREV_CELL_IDX] = KCDefine.B_IDX_INVALID_3D;
		}

		/** 그리기 에디터 모드 터치 이동 이벤트를 처리한다 */
		private void HandleDrawEditorModeTouchMoveEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			var stPos = a_oEventData.ExGetLocalPos(this.ObjRoot, this.ScreenSize);
			var stIdx = stPos.ExToIdx(this.SelGridInfo.m_stPivotPos, NSEngine.Access.CellSize);

			var stCellInfo = this.SelLevelInfo.GetCellInfo(stIdx);
            
            // 객체 추가가 가능 할 경우
            if(Input.GetMouseButton((int)EMouseBtn.LEFT) && m_oObjKindsDict[EKey.SEL_OBJ_KINDS].ExIsValid()) {
                this.AddCellObjInfo(Factory.MakeEditorCellObjInfo(m_oObjKindsDict[EKey.SEL_OBJ_KINDS], this.GetEditorObjSize(), stIdx, currentColorID), stIdx);
            }
            // 객체 제거가 가능 할 경우
            else if(Input.GetMouseButton((int)EMouseBtn.RIGHT) && stCellInfo.m_oCellObjInfoList.ExIsValid()) {
                this.RemoveCellObjInfo(Input.GetKey(KeyCode.LeftShift) ? m_oObjKindsDict[EKey.SEL_OBJ_KINDS] : EObjKinds.NONE, stIdx);
            }
		}

		/** 페인트 에디터 모드 터치 이동 이벤트를 처리한다 */
		private void HandlePaintEditorModeTouchMoveEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			var oIdxList = CCollectionManager.Inst.SpawnList<Vector3Int>();

			try {
				var stPos = a_oEventData.ExGetLocalPos(this.ObjRoot, this.ScreenSize);
				var stIdx = stPos.ExToIdx(this.SelGridInfo.m_stPivotPos, NSEngine.Access.CellSize);

				oIdxList.ExAddVal(stIdx);
				this.TryGetCellObjInfo(stIdx, EObjKinds.NONE, out STCellObjInfo stCellObjInfo);

				while(oIdxList.ExIsValid()) {
					stIdx = oIdxList[KCDefine.B_VAL_0_INT];
					oIdxList.ExRemoveValAt(KCDefine.B_VAL_0_INT);

					var stSize = this.GetEditorObjSize();
					var eObjKinds = Input.GetKey(KeyCode.LeftShift) ? m_oObjKindsDict[EKey.SEL_OBJ_KINDS] : stCellObjInfo.ObjKinds;

					bool bIsValid01 = this.IsEnableAddCellObjInfo(stIdx, stSize, m_oObjKindsDict[EKey.SEL_OBJ_KINDS], false);
					bool bIsValid02 = this.IsEnableRemoveCellObjInfo(eObjKinds, stIdx);

					// 객체 추가가 가능 할 경우
					if(Input.GetMouseButton((int)EMouseBtn.LEFT) && bIsValid01 && m_oObjKindsDict[EKey.SEL_OBJ_KINDS].ExIsValid()) {
						this.AddCellObjInfo(Factory.MakeEditorCellObjInfo(m_oObjKindsDict[EKey.SEL_OBJ_KINDS], stSize, stIdx, currentColorID), stIdx, false);

						oIdxList.ExAddVal(new Vector3Int(stIdx.x, stIdx.y - stSize.y, stIdx.z));
						oIdxList.ExAddVal(new Vector3Int(stIdx.x, stIdx.y + stSize.y, stIdx.z));
						oIdxList.ExAddVal(new Vector3Int(stIdx.x - stSize.x, stIdx.y, stIdx.z));
						oIdxList.ExAddVal(new Vector3Int(stIdx.x + stSize.x, stIdx.y, stIdx.z));
					}
					// 객체 제거가 가능 할 경우
					else if(Input.GetMouseButton((int)EMouseBtn.RIGHT) && bIsValid02 && this.TryGetCellObjInfo(stIdx, eObjKinds, out stCellObjInfo)) {
						this.RemoveCellObjInfo(eObjKinds, stIdx);

						oIdxList.ExAddVal(new Vector3Int(stIdx.x, stIdx.y - stCellObjInfo.m_stSize.y, stIdx.z));
						oIdxList.ExAddVal(new Vector3Int(stIdx.x, stIdx.y + stCellObjInfo.m_stSize.y, stIdx.z));
						oIdxList.ExAddVal(new Vector3Int(stIdx.x - stCellObjInfo.m_stSize.x, stIdx.y, stIdx.z));
						oIdxList.ExAddVal(new Vector3Int(stIdx.x + stCellObjInfo.m_stSize.x, stIdx.y, stIdx.z));
					}
				}
			} finally {
				CCollectionManager.Inst.DespawnList(oIdxList);
			}
		}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		#endregion // 조건부 함수
	}

	/** 서브 레벨 에디터 씬 관리자 - 중앙 에디터 UI */
	public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
		#region 조건부 함수
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		/** 중앙 에디터 UI 를 설정한다 */
		private void SubSetupMidEditorUIs() {
			// 텍스트를 설정한다
			CFunc.SetupComponents(new List<(ESubKey, string, GameObject)>() {
				(ESubKey.ME_UIS_CELL_OBJ_HP_TEXT, $"{ESubKey.ME_UIS_CELL_OBJ_HP_TEXT}", this.MEUIsInfoUIs),
                (ESubKey.ME_UIS_CELL_OBJ_SHIELD_TEXT, $"{ESubKey.ME_UIS_CELL_OBJ_SHIELD_TEXT}", this.MEUIsInfoUIs),
				(ESubKey.ME_UIS_CELL_OBJ_ATK_TEXT, $"{ESubKey.ME_UIS_CELL_OBJ_ATK_TEXT}", this.MEUIsInfoUIs),
                (ESubKey.ME_UIS_CELL_OBJ_COLOR_TEXT, $"{ESubKey.ME_UIS_CELL_OBJ_COLOR_TEXT}", this.MEUIsInfoUIs),
                (ESubKey.ME_UIS_CELL_OBJ_COUNT_TEXT, $"{ESubKey.ME_UIS_CELL_OBJ_COUNT_TEXT}", this.MEUIsInfoUIs),
                (ESubKey.ME_UIS_CELL_OBJ_SCORE_1_TEXT, $"{ESubKey.ME_UIS_CELL_OBJ_SCORE_1_TEXT}", this.MEUIsInfoUIs),
                (ESubKey.ME_UIS_CELL_OBJ_SCORE_2_TEXT, $"{ESubKey.ME_UIS_CELL_OBJ_SCORE_2_TEXT}", this.MEUIsInfoUIs),
                (ESubKey.ME_UIS_CELL_OBJ_SCORE_3_TEXT, $"{ESubKey.ME_UIS_CELL_OBJ_SCORE_3_TEXT}", this.MEUIsInfoUIs),
			}, m_oSubTextDict);
		}

		/** 중앙 에디터 UI 상태를 갱신한다 */
		private void SubUpdateMidEditorUIsState() {
            Color stColor = GlobalDefine.colorList[m_oSubDropDict[ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_COLOR_DROP].value][0];
            scoreList = GlobalDefine.GetLevelScoreList(this.SelLevelInfo);
            
			m_oSubTextDict[ESubKey.ME_UIS_CELL_OBJ_HP_TEXT].text = string.Format(KDefine.LES_TEXT_FMT_HP_INFO, m_oSubInputDict[ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_HP_INPUT].text);
            m_oSubTextDict[ESubKey.ME_UIS_CELL_OBJ_SHIELD_TEXT].text = string.Format(KDefine.LES_TEXT_FMT_SHIELD_INFO, m_oSubInputDict[ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_SHIELD_INPUT].text);
			m_oSubTextDict[ESubKey.ME_UIS_CELL_OBJ_ATK_TEXT].text = string.Format(KDefine.LES_TEXT_FMT_ATK_INFO, m_oSubInputDict[ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_ATK_INPUT].text);
            m_oSubTextDict[ESubKey.ME_UIS_CELL_OBJ_COLOR_TEXT].text = string.Format(KDefine.LES_TEXT_FMT_COLOR_INFO, m_oSubDropDict[ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_COLOR_DROP].captionText.text.ExGetColorFmtStr(stColor));
            m_oSubTextDict[ESubKey.ME_UIS_CELL_OBJ_COUNT_TEXT].text = string.Format(KDefine.LES_TEXT_FMT_COUNT_INFO, GlobalDefine.GetTargetCellCount(this.SelLevelInfo));
            m_oSubTextDict[ESubKey.ME_UIS_CELL_OBJ_SCORE_1_TEXT].text = string.Format(KDefine.LES_TEXT_FMT_SCORE_1_INFO, scoreList[0]);
            m_oSubTextDict[ESubKey.ME_UIS_CELL_OBJ_SCORE_2_TEXT].text = string.Format(KDefine.LES_TEXT_FMT_SCORE_2_INFO, scoreList[1]);
            m_oSubTextDict[ESubKey.ME_UIS_CELL_OBJ_SCORE_3_TEXT].text = string.Format(KDefine.LES_TEXT_FMT_SCORE_3_INFO, scoreList[2]);

            UpdateRightUIsColor();
		}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		#endregion // 조건부 함수
	}

	/** 서브 레벨 에디터 씬 관리자 - 왼쪽 에디터 UI */
	public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
		#region 조건부 함수
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		/** 왼쪽 에디터 UI 를 설정한다 */
		private void SubSetupLeftEditorUIs() {
			// Do Something
		}

		/** 왼쪽 에디터 UI 상태를 갱신한다 */
		private void SubUpdateLeftEditorUIsState() {
			// Do Something
		}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		#endregion // 조건부 함수
	}

	/** 서브 레벨 에디터 씬 관리자 - 오른쪽 에디터 UI */
	public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
		#region 조건부 함수
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		/** 오른쪽 에디터 UI 를 설정한다 */
		private void SubSetupRightEditorUIs() {
			// 페이지 스크롤 스냅이 존재 할 경우
			if(m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP] != null) {
				for(int i = 0; i < m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP].NumberOfPanels; ++i) {
					string oSetupFuncName = string.Format(KDefine.LES_FUNC_N_FMT_SUB_SETUP_RE_UIS_PAGE_UIS, i + KCDefine.B_VAL_1_INT);
					string oUpdateFuncName = string.Format(KDefine.LES_FUNC_N_FMT_SUB_UPDATE_RE_UIS_PAGE_UIS, i + KCDefine.B_VAL_1_INT);

					m_oSubMethodInfoDict.TryAdd(ECallback.SETUP_RE_UIS_PAGE_UIS_01 + i, this.GetType().GetMethod(oSetupFuncName, KCDefine.B_BINDING_F_NON_PUBLIC_INSTANCE));
					m_oSubMethodInfoDict.TryAdd(ECallback.UPDATE_RE_UIS_PAGE_UIS_01 + i, this.GetType().GetMethod(oUpdateFuncName, KCDefine.B_BINDING_F_NON_PUBLIC_INSTANCE));
				}

				for(int i = 0; i < m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP].NumberOfPanels; ++i) {
					m_oSubMethodInfoDict.GetValueOrDefault(ECallback.SETUP_RE_UIS_PAGE_UIS_01 + i)?.Invoke(this, new object[] {
						m_oUIsDict.GetValueOrDefault(EKey.RE_UIS_PAGE_UIS_01 + i)
					});
				}
			}
		}

		/** 오른쪽 에디터 UI 페이지 UI 1 를 설정한다 */
		private void SubSetupREUIsPageUIs01(GameObject a_oPageUIs) {
			// Do Something
		}

		/** 오른쪽 에디터 UI 페이지 UI 2 를 설정한다 */
		private void SubSetupREUIsPageUIs02(GameObject a_oPageUIs) {
            // 드롭을 설정한다
			CFunc.SetupDrops(new List<(ESubKey, string, GameObject, UnityAction<int>)>() {
				(ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_COLOR_DROP, $"{ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_COLOR_DROP}", a_oPageUIs, this.OnChangeREUIsPageUIs02CellObjColorVal)
			}, m_oSubDropDict);

			// 입력을 설정한다 {
			CFunc.SetupInputs(new List<(ESubKey, string, GameObject, UnityAction<string>)>() {
				(ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_HP_INPUT, $"{ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_HP_INPUT}", a_oPageUIs, this.OnChangeREUIsPageUIs02CellObjInfoInputStr),
                (ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_SHIELD_INPUT, $"{ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_SHIELD_INPUT}", a_oPageUIs, this.OnChangeREUIsPageUIs02CellObjInfoInputStr),
				(ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_ATK_INPUT, $"{ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_ATK_INPUT}", a_oPageUIs, this.OnChangeREUIsPageUIs02CellObjInfoInputStr),
			}, m_oSubInputDict);

			m_oInputList02.ExAddVal(m_oSubInputDict[ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_HP_INPUT]);
            m_oInputList02.ExAddVal(m_oSubInputDict[ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_SHIELD_INPUT]);
			m_oInputList02.ExAddVal(m_oSubInputDict[ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_ATK_INPUT]);

			m_oSubInputDict[ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_HP_INPUT]?.SetTextWithoutNotify(10.ToString());
            m_oSubInputDict[ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_SHIELD_INPUT]?.SetTextWithoutNotify(10.ToString());
			m_oSubInputDict[ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_ATK_INPUT]?.SetTextWithoutNotify(KCDefine.B_STR_0_INT);
			// 입력을 설정한다 }

			// 버튼을 설정한다 {
			for(int i = 0; i < m_oSubCellObjHPBtnUIs.transform.childCount; ++i) {
				var oBtn = m_oSubCellObjHPBtnUIs.transform.GetChild(i).GetComponentInChildren<Button>();
				oBtn.onClick.AddListener(() => this.OnTouchREUIsPageUIs02CellObjInfoBtn(oBtn));
			}

            for(int i = 0; i < m_oSubCellObjShieldBtnUIs.transform.childCount; ++i) {
				var oBtn = m_oSubCellObjShieldBtnUIs.transform.GetChild(i).GetComponentInChildren<Button>();
				oBtn.onClick.AddListener(() => this.OnTouchREUIsPageUIs02CellObjInfoBtn(oBtn));
			}

			for(int i = 0; i < m_oSubCellObjATKBtnUIs.transform.childCount; ++i) {
				var oBtn = m_oSubCellObjATKBtnUIs.transform.GetChild(i).GetComponentInChildren<Button>();
				oBtn.onClick.AddListener(() => this.OnTouchREUIsPageUIs02CellObjInfoBtn(oBtn));
			}
			// 버튼을 설정한다 }
		}

		/** 오른쪽 에디터 UI 상태를 갱신한다 */
		private void SubUpdateRightEditorUIsState() {
			// 페이지 스크롤 스냅이 존재 할 경우
			if(m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP] != null) {
				for(int i = 0; i < m_oScrollSnapDict[EKey.RE_UIS_PAGE_SCROLL_SNAP].NumberOfPanels; ++i) {
					m_oSubMethodInfoDict.GetValueOrDefault(ECallback.UPDATE_RE_UIS_PAGE_UIS_01 + i)?.Invoke(this, new object[] {
						m_oUIsDict.GetValueOrDefault(EKey.RE_UIS_PAGE_UIS_01 + i)
					});
				}
			}
		}

		/** 오른쪽 에디터 UI 페이지 UI 1 상태를 갱신한다 */
		private void SubUpdateREUIsPageUIs01(GameObject a_oPageUIs) {
			// Do Something
		}

		/** 오른쪽 에디터 UI 페이지 UI 2 상태를 갱신한다 */
		private void SubUpdateREUIsPageUIs02(GameObject a_oPageUIs) {
			// Do Something
		}

		/** 오른쪽 에디터 UI 페이지 UI 2 셀 객체 정보 버튼을 눌렀을 경우 */
		private void OnTouchREUIsPageUIs02CellObjInfoBtn(Button a_oBtn) {
			var oText = a_oBtn.GetComponentInChildren<Text>();

			bool bIsValid01 = int.TryParse(oText.text, NumberStyles.Any, null, out int nExtraVal);
			bool bIsValid02 = int.TryParse(m_oSubInputDict[ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_HP_INPUT].text, NumberStyles.Any, null, out int nHP);
            bool bIsValid03 = int.TryParse(m_oSubInputDict[ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_SHIELD_INPUT].text, NumberStyles.Any, null, out int nShield);
			bool bIsValid04 = int.TryParse(m_oSubInputDict[ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_ATK_INPUT].text, NumberStyles.Any, null, out int nATK);

			// 정보 갱신이 가능 할 경우
			if(bIsValid01 && (bIsValid02 || bIsValid03 || bIsValid04)) {
				this.SetREUIsPageUIs02CellObjInfo((bIsValid02 && a_oBtn.transform.parent.gameObject == m_oSubCellObjHPBtnUIs) ? nHP + nExtraVal : nHP, 
                                                    (bIsValid03 && a_oBtn.transform.parent.gameObject == m_oSubCellObjShieldBtnUIs) ? nShield + nExtraVal : nShield, 
                                                    (bIsValid04 && a_oBtn.transform.parent.gameObject == m_oSubCellObjATKBtnUIs) ? nATK + nExtraVal : nATK);

                if (a_oBtn.transform.parent.gameObject == m_oSubCellObjHPBtnUIs)
                {
                    m_oSubInputDict[ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_HP_INPUT].onValueChanged.Invoke($"{Mathf.Max(nHP + nExtraVal, KCDefine.B_VAL_0_INT)}");
                }

                if (a_oBtn.transform.parent.gameObject == m_oSubCellObjShieldBtnUIs)
                {
                    m_oSubInputDict[ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_SHIELD_INPUT].onValueChanged.Invoke($"{Mathf.Max(nShield + nExtraVal, KCDefine.B_VAL_0_INT)}");
                }
			}
		}

        /** 오른쪽 에디터 UI 페이지 UI 2 셀 객체 색상 값이 변경 되었을 경우 */
		private void OnChangeREUIsPageUIs02CellObjColorVal(int a_nIdx) {
			this.SetREUIsPageUIs02CellObjColor(a_nIdx);
		}

		/** 오른쪽 에디터 UI 페이지 UI 2 셀 객체 정보 문자열을 변경했을 경우 */
		private void OnChangeREUIsPageUIs02CellObjInfoInputStr(string a_oStr) {
			bool bIsValid01 = int.TryParse(m_oSubInputDict[ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_HP_INPUT].text, NumberStyles.Any, null, out int nHP);
            bool bIsValid02 = int.TryParse(m_oSubInputDict[ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_SHIELD_INPUT].text, NumberStyles.Any, null, out int nShield);
			bool bIsValid03 = int.TryParse(m_oSubInputDict[ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_ATK_INPUT].text, NumberStyles.Any, null, out int nATK);

			this.SetREUIsPageUIs02CellObjInfo(bIsValid01 ? nHP : KCDefine.B_VAL_0_INT, bIsValid02 ? nShield : KCDefine.B_VAL_0_INT, bIsValid03 ? nATK : KCDefine.B_VAL_0_INT);
		}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		#endregion // 조건부 함수

		#region 조건부 접근자 함수
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
        /** 오른쪽 에디터 UI 페이지 UI 2 셀 객체 색상을 변경한다 */
		private void SetREUIsPageUIs02CellObjColor(int a_nIdx) {
			m_oSubDropDict[ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_COLOR_DROP].SetValueWithoutNotify(Mathf.Clamp(a_nIdx, KCDefine.B_VAL_0_INT, GlobalDefine.colorList.Count - KCDefine.B_VAL_1_INT));
			this.UpdateUIsState();
		}
        
		/** 오른쪽 에디터 UI 페이지 UI 2 셀 객체 정보를 변경한다 */
		private void SetREUIsPageUIs02CellObjInfo(int a_nHP, int a_nShield, int a_nATK) {
			m_oSubInputDict[ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_HP_INPUT].SetTextWithoutNotify($"{Mathf.Max(a_nHP, KCDefine.B_VAL_0_INT)}");
            m_oSubInputDict[ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_SHIELD_INPUT].SetTextWithoutNotify($"{Mathf.Max(a_nShield, KCDefine.B_VAL_0_INT)}");
			m_oSubInputDict[ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_ATK_INPUT].SetTextWithoutNotify($"{Mathf.Max(a_nATK, KCDefine.B_VAL_0_INT)}");

			this.UpdateUIsState();
		}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		#endregion // 조건부 접근자 함수
	}
}
#endif // #if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)

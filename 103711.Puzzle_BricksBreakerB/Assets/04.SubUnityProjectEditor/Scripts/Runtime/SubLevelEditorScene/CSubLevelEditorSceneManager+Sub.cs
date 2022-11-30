using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
using UnityEngine.EventSystems;
using EnhancedUI.EnhancedScroller;

namespace LevelEditorScene {
	/** 서브 레벨 에디터 씬 관리자 */
	public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();
			
			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
				// 레벨 정보가 없을 경우
				if(!CLevelInfoTable.Inst.LevelInfoDictContainer.ExIsValid()) {
					var oLevelInfo = Factory.MakeLevelInfo(KCDefine.B_VAL_0_INT);

					Func.SetupEditorLevelInfo(oLevelInfo, new CSubEditorLevelCreateInfo() {
						m_nNumLevels = KCDefine.B_VAL_0_INT, m_stMinNumCells = NSEngine.KDefine.E_MIN_NUM_CELLS, m_stMaxNumCells = NSEngine.KDefine.E_MIN_NUM_CELLS
					});

					CLevelInfoTable.Inst.AddLevelInfo(oLevelInfo);
					CLevelInfoTable.Inst.SaveLevelInfos();
				}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

				this.SetupAwake();
			}
		}
		
		/** 초기화 */
		public override void Start() {
			base.Start();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
				this.ExLateCallFunc((a_oSender) => this.UpdateUIsState(), KCDefine.U_DELAY_INIT);
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

				this.SetupStart();
				CSndManager.Inst.StopBGSnd();
			}
		}

		/** 씬을 설정한다 */
		private void SetupAwake() {
			this.AddObjsPool(KDefine.LES_KEY_SPRITE_OBJS_POOL, CFactory.CreateObjsPool(KCDefine.U_OBJ_P_SPRITE, this.ObjRoot));

			// 스프라이트를 설정한다
			CFunc.SetupSprites(new List<(EKey, string, GameObject)>() {
				(EKey.SEL_OBJ_SPRITE, $"{EKey.SEL_OBJ_SPRITE}", this.Objs)
			}, m_oSpriteDict);

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
			this.SetupMidEditorUIs();
			this.SetupLeftEditorUIs();
			this.SetupRightEditorUIs();

			// 레벨 정보를 설정한다
			m_oLevelInfoDict.ExReplaceVal(EKey.SEL_LEVEL_INFO, CGameInfoStorage.Inst.PlayLevelInfo ?? CLevelInfoTable.Inst.GetLevelInfo(KCDefine.B_VAL_0_INT));
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

#region 추가
			this.SubSetupAwake();
#endregion // 추가
		}

		/** 씬을 설정한다 */
		private void SetupStart() {
			// 스크롤 뷰를 설정한다
			m_oScrollSnapDict.GetValueOrDefault(EKey.RE_UIS_PAGE_SCROLL_SNAP)?.gameObject.SetActive(true);

#region 추가
			this.SubSetupStart();
#endregion // 추가
		}
#endregion // 함수

#region 조건부 함수
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		/** UI 상태를 갱신한다 */
		private void UpdateUIsState() {
			this.ResetObjSprites();

			this.UpdateMidEditorUIsState();
			this.UpdateLeftEditorUIsState();
			this.UpdateRightEditorUIsState();

#region 추가
			this.SubUpdateUIsState();
#endregion // 추가
		}

		/** 객체 스프라이트를 리셋한다 */
		private void ResetObjSprites() {
			// 객체 스프라이트가 존재 할 경우
			if(m_oObjSpriteInfoDictContainers.ExIsValid()) {
				for(int i = 0; i < m_oObjSpriteInfoDictContainers.GetLength(KCDefine.B_VAL_0_INT); ++i) {
					for(int j = 0; j < m_oObjSpriteInfoDictContainers.GetLength(KCDefine.B_VAL_1_INT); ++j) {
						this.ResetObjSprites(m_oObjSpriteInfoDictContainers[i, j]);
					}
				}
			}

			m_oGridInfoList.Clear();
			m_oGridInfoList.ExAddVal(NSEngine.Factory.MakeGridInfo(m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO), Vector3.zero));

			// 비율을 설정한다 {
			bool bIsValid01 = !float.IsNaN(m_oGridInfoList[this.SelGridInfoIdx].m_stScale.x) && !float.IsInfinity(m_oGridInfoList[this.SelGridInfoIdx].m_stScale.x);
			bool bIsValid02 = !float.IsNaN(m_oGridInfoList[this.SelGridInfoIdx].m_stScale.y) && !float.IsInfinity(m_oGridInfoList[this.SelGridInfoIdx].m_stScale.y);
			bool bIsValid03 = !float.IsNaN(m_oGridInfoList[this.SelGridInfoIdx].m_stScale.z) && !float.IsInfinity(m_oGridInfoList[this.SelGridInfoIdx].m_stScale.z);

			this.ObjRoot.transform.localScale = (bIsValid01 && bIsValid02 && bIsValid03) ? m_oGridInfoList[this.SelGridInfoIdx].m_stScale : Vector3.one;
			this.ObjRoot.transform.localPosition = Vector3.zero.ExToWorld(this.MidEditorUIs).ExToLocal(this.UIs);
			// 비율을 설정한다 }

			// 객체 스프라이트를 설정한다 {
			m_oObjSpriteInfoDictContainers = new Dictionary<EObjType, List<(EObjKinds, SpriteRenderer)>>[m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).NumCells.y, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).NumCells.x];

			for(int i = 0; i < m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_oCellInfoDictContainer.Count; ++i) {
				for(int j = 0; j < m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_oCellInfoDictContainer[i].Count; ++j) {
					this.SetupObjSprites(m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_oCellInfoDictContainer[i][j], out Dictionary<EObjType, List<(EObjKinds, SpriteRenderer)>> oObjSpriteInfoDictContainer);
					m_oObjSpriteInfoDictContainers[m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_oCellInfoDictContainer[i][j].m_stIdx.y, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_oCellInfoDictContainer[i][j].m_stIdx.x] = oObjSpriteInfoDictContainer;
				}
			}
			// 객체 스프라이트를 설정한다 }
		}

		/** 객체 스프라이트를 리셋한다 */
		private void ResetObjSprites(Dictionary<EObjType, List<(EObjKinds, SpriteRenderer)>> a_oObjSpriteInfoDictContainer) {
			foreach(var stKeyVal in a_oObjSpriteInfoDictContainer) {
				for(int i = 0; i < stKeyVal.Value.Count; ++i) {
					this.DespawnObj(KDefine.LES_KEY_SPRITE_OBJS_POOL, stKeyVal.Value[i].Item2.gameObject);
				}
			}
		}

		/** 객체 스프라이트를 설정한다 */
		private void SetupObjSprites(STCellInfo a_stCellInfo, out Dictionary<EObjType, List<(EObjKinds, SpriteRenderer)>> a_oOutObjSpriteInfoDictContainer) {
			a_oOutObjSpriteInfoDictContainer = new Dictionary<EObjType, List<(EObjKinds, SpriteRenderer)>>();

			foreach(var stKeyVal in a_stCellInfo.m_oObjKindsDictContainer) {
				var oObjSpriteInfoList = new List<(EObjKinds, SpriteRenderer)>();

				for(int i = 0; i < stKeyVal.Value.Count; ++i) {
					var oObjSprite = this.SpawnObj<SpriteRenderer>(KDefine.LES_OBJ_N_OBJ_SPRITE, KDefine.LES_KEY_SPRITE_OBJS_POOL);
					oObjSprite.sprite = NSEngine.Access.GetObjSprite(stKeyVal.Value[i]);
					oObjSprite.transform.localPosition = m_oGridInfoList[this.SelGridInfoIdx].m_stPivotPos + a_stCellInfo.m_stIdx.ExToPos(NSEngine.KDefine.E_OFFSET_CELL, NSEngine.KDefine.E_SIZE_CELL);

					oObjSprite.ExSetSortingOrder(NSEngine.Access.GetSortingOrderInfo(stKeyVal.Value[i]));
					oObjSpriteInfoList.ExAddVal((stKeyVal.Value[i], oObjSprite));
				}

				a_oOutObjSpriteInfoDictContainer.TryAdd(stKeyVal.Key, oObjSpriteInfoList);
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
		private void SetupMidEditorUIs() {
			// 텍스트를 설정한다
			CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
				(EKey.ME_UIS_MSG_TEXT, $"{EKey.ME_UIS_MSG_TEXT}", this.MidEditorUIs),
				(EKey.ME_UIS_LEVEL_TEXT, $"{EKey.ME_UIS_LEVEL_TEXT}", this.MidEditorUIs)
			}, m_oTextDict);

			// 버튼을 설정한다 {
			CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
				(EKey.ME_UIS_PREV_BTN, $"{EKey.ME_UIS_PREV_BTN}", this.MidEditorUIs, this.OnTouchMEUIsPrevBtn),
				(EKey.ME_UIS_NEXT_BTN, $"{EKey.ME_UIS_NEXT_BTN}", this.MidEditorUIs, this.OnTouchMEUIsNextBtn),
				(EKey.ME_UIS_MOVE_LEVEL_BTN, $"{EKey.ME_UIS_MOVE_LEVEL_BTN}", this.MidEditorUIs, this.OnTouchMEUIsMoveLevelBtn),
				(EKey.ME_UIS_REMOVE_LEVEL_BTN, $"{EKey.ME_UIS_REMOVE_LEVEL_BTN}", this.MidEditorUIs, this.OnTouchMEUIsRemoveLevelBtn)
			}, m_oBtnDict);

			CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
				(KCDefine.LES_OBJ_N_ME_UIS_SAVE_BTN, this.MidEditorUIs, this.OnTouchMEUIsSaveBtn),
				(KCDefine.LES_OBJ_N_ME_UIS_RESET_BTN, this.MidEditorUIs, this.OnTouchMEUIsResetBtn),
				(KCDefine.LES_OBJ_N_ME_UIS_TEST_BTN, this.MidEditorUIs, this.OnTouchMEUIsTestBtn),
				(KCDefine.LES_OBJ_N_ME_UIS_COPY_LEVEL_BTN, this.MidEditorUIs, this.OnTouchMEUIsCopyLevelBtn)
			}, false);
			// 버튼을 설정한다 }
		}

		/** 중앙 에디터 UI 상태를 갱신한다 */
		private void UpdateMidEditorUIsState() {
			int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID02, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID03);

			// 텍스트를 갱신한다
			m_oTextDict.GetValueOrDefault(EKey.ME_UIS_LEVEL_TEXT)?.ExSetText<Text>(string.Format(KCDefine.LES_TEXT_FMT_LEVEL, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID01 + KCDefine.B_VAL_1_INT), false);

			// 버튼을 갱신한다 {
			m_oBtnDict.GetValueOrDefault(EKey.ME_UIS_PREV_BTN)?.ExSetInteractable(CLevelInfoTable.Inst.TryGetLevelInfo(m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID01 - KCDefine.B_VAL_1_INT, out CLevelInfo oPrevLevelInfo, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID02, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID03), false);
			m_oBtnDict.GetValueOrDefault(EKey.ME_UIS_NEXT_BTN)?.ExSetInteractable(CLevelInfoTable.Inst.TryGetLevelInfo(m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID01 + KCDefine.B_VAL_1_INT, out CLevelInfo oNextLevelInfo, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID02, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID03), false);

			m_oBtnDict.GetValueOrDefault(EKey.ME_UIS_MOVE_LEVEL_BTN)?.ExSetInteractable(nNumLevelInfos > KCDefine.B_VAL_1_INT);
			m_oBtnDict.GetValueOrDefault(EKey.ME_UIS_REMOVE_LEVEL_BTN)?.ExSetInteractable(nNumLevelInfos > KCDefine.B_VAL_1_INT);
			// 버튼을 갱신한다 }
		}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endregion // 조건부 함수
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
					(EKey.LE_UIS_LEVEL_SCROLLER_INFO, KCDefine.U_OBJ_N_LEVEL_SCROLL_VIEW, this.LeftEditorUIs, CResManager.Inst.GetRes<GameObject>(KCDefine.LES_OBJ_P_LEVEL_EDITOR_SCROLLER_CELL_VIEW)?.GetComponentInChildren<EnhancedScrollerCellView>(), this),
					(EKey.LE_UIS_STAGE_SCROLLER_INFO_01, KCDefine.LES_OBJ_N_LE_UIS_STAGE_SCROLL_VIEW_01, this.LeftEditorUIs, CResManager.Inst.GetRes<GameObject>(KCDefine.LES_OBJ_P_STAGE_EDITOR_SCROLLER_CELL_VIEW)?.GetComponentInChildren<EnhancedScrollerCellView>(), this),
					(EKey.LE_UIS_STAGE_SCROLLER_INFO_02, KCDefine.LES_OBJ_N_LE_UIS_STAGE_SCROLL_VIEW_02, this.LeftEditorUIs, CResManager.Inst.GetRes<GameObject>(KCDefine.LES_OBJ_P_STAGE_EDITOR_SCROLLER_CELL_VIEW)?.GetComponentInChildren<EnhancedScrollerCellView>(), this),
					(EKey.LE_UIS_CHAPTER_SCROLLER_INFO, KCDefine.U_OBJ_N_CHAPTER_SCROLL_VIEW, this.LeftEditorUIs, CResManager.Inst.GetRes<GameObject>(KCDefine.LES_OBJ_P_CHAPTER_EDITOR_SCROLLER_CELL_VIEW)?.GetComponentInChildren<EnhancedScrollerCellView>(), this)
				}, m_oScrollerInfoDict);

				foreach(var stKeyVal in oScrollViewDict) {
					stKeyVal.Value?.gameObject.SetActive(false);
				}
				
				m_oScrollerInfoDict.ExReplaceVal(EKey.LE_UIS_STAGE_SCROLLER_INFO, m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_STAGE_SCROLLER_INFO_01));

				m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_LEVEL_SCROLLER_INFO).Item1?.gameObject.SetActive(true);
				m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_STAGE_SCROLLER_INFO).Item1?.gameObject.SetActive(false);
				m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_CHAPTER_SCROLLER_INFO).Item1?.gameObject.SetActive(false);
				// 스크롤 뷰를 설정한다 }

				// 버튼을 설정한다 {
				CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
					(EKey.LE_UIS_ADD_STAGE_BTN, $"{EKey.LE_UIS_ADD_STAGE_BTN}", this.LeftEditorUIs, this.OnTouchLEUIsAddStageBtn),
					(EKey.LE_UIS_ADD_CHAPTER_BTN, $"{EKey.LE_UIS_ADD_CHAPTER_BTN}", this.LeftEditorUIs, this.OnTouchLEUIsAddChapterBtn)
				}, m_oBtnDict);

				CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
					(KCDefine.LES_OBJ_N_LE_UIS_ADD_LEVEL_BTN, this.LeftEditorUIs, this.OnTouchLEUIsAddLevelBtn)
				}, false);

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

					m_oBtnDict.GetValueOrDefault(EKey.LE_UIS_ADD_STAGE_BTN)?.ExSetInteractable(m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_STAGE_SCROLLER_INFO).Item1 != null && m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_STAGE_SCROLLER_INFO).Item1.gameObject.activeSelf);
					m_oBtnDict.GetValueOrDefault(EKey.LE_UIS_ADD_CHAPTER_BTN)?.ExSetInteractable(m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_CHAPTER_SCROLLER_INFO).Item1 != null && m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_CHAPTER_SCROLLER_INFO).Item1.gameObject.activeSelf);
				});
				// 버튼을 설정한다 }
			} finally {
				CCollectionManager.Inst.DespawnDict(oScrollViewDict);
			}
		}

		/** 왼쪽 에디터 UI 상태를 갱신한다 */
		private void UpdateLeftEditorUIsState() {
			// 버튼을 설정한다
			m_oBtnDict.GetValueOrDefault(EKey.LE_UIS_A_SET_BTN)?.image.ExSetColor<Image>((CCommonUserInfoStorage.Inst.UserInfo.UserType == EUserType.A) ? Color.yellow : Color.white, false);
			m_oBtnDict.GetValueOrDefault(EKey.LE_UIS_B_SET_BTN)?.image.ExSetColor<Image>((CCommonUserInfoStorage.Inst.UserInfo.UserType == EUserType.B) ? Color.yellow : Color.white, false);

			// 스크롤 뷰를 갱신한다
			m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_LEVEL_SCROLLER_INFO).Item1?.ExReloadData(m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID01 - KCDefine.B_VAL_1_INT, false);
			m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_STAGE_SCROLLER_INFO).Item1?.ExReloadData(m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID02 - KCDefine.B_VAL_1_INT, false);
			m_oScrollerInfoDict.GetValueOrDefault(EKey.LE_UIS_CHAPTER_SCROLLER_INFO).Item1?.ExReloadData(m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID03 - KCDefine.B_VAL_1_INT, false);
		}
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

			// 버튼을 설정한다 {
			CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
				(EKey.RE_UIS_PREV_BTN, $"{EKey.RE_UIS_PREV_BTN}", this.RightEditorUIs),
				(EKey.RE_UIS_NEXT_BTN, $"{EKey.RE_UIS_NEXT_BTN}", this.RightEditorUIs)
			}, m_oBtnDict);

			CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
				(EKey.RE_UIS_REMOVE_ALL_LEVELS_BTN, $"{EKey.RE_UIS_REMOVE_ALL_LEVELS_BTN}", this.RightEditorUIs, this.OnTouchREUIsRemoveAllLevelsBtn),
				(EKey.RE_UIS_LOAD_REMOTE_TABLE_BTN, $"{EKey.RE_UIS_LOAD_REMOTE_TABLE_BTN}", this.RightEditorUIs, () => this.OnTouchREUIsLoadTableBtn(ETableSrc.REMOTE))
			}, m_oBtnDict);

			CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
				(KCDefine.LES_OBJ_N_RE_UIS_APPLY_BTN, this.RightEditorUIs, this.OnTouchREUIsApplyBtn),
				(KCDefine.LES_OBJ_N_RE_UIS_LOAD_LEVEL_BTN, this.RightEditorUIs, this.OnTouchREUIsLoadLevelBtn),
				(KCDefine.LES_OBJ_N_RE_UIS_LOAD_LOCAL_TABLE_BTN, this.RightEditorUIs, () => this.OnTouchREUIsLoadTableBtn(ETableSrc.LOCAL))
			}, false);
			// 버튼을 설정한다 }

			// 스크롤 뷰를 설정한다 {
			CFunc.SetupScrollSnaps(new List<(EKey, string, GameObject, UnityAction<int, int>)>() {
				(EKey.RE_UIS_PAGE_SCROLL_SNAP, KCDefine.U_OBJ_N_PAGE_VIEW, this.RightEditorUIs, (a_nCenterIdx, a_nSelIdx) => this.UpdateUIsState())
			}, m_oScrollSnapDict);

			m_oScrollSnapDict.GetValueOrDefault(EKey.RE_UIS_PAGE_SCROLL_SNAP)?.gameObject.SetActive(false);

			for(int i = 0; i < m_oScrollSnapDict.GetValueOrDefault(EKey.RE_UIS_PAGE_SCROLL_SNAP).NumberOfPanels; ++i) {
				string oSetupFuncName = string.Format(KDefine.LES_FUNC_N_FMT_SETUP_RE_UIS_PAGE_UIS, i + KCDefine.B_VAL_1_INT);
				string oUpdateFuncName = string.Format(KDefine.LES_FUNC_N_FMT_UPDATE_RE_UIS_PAGE_UIS, i + KCDefine.B_VAL_1_INT);

				m_oMethodInfoDict.TryAdd(ECallback.SETUP_RE_UIS_PAGE_UIS_01 + i, this.GetType().GetMethod(oSetupFuncName, KCDefine.B_BINDING_F_NON_PUBLIC_INSTANCE));
				m_oMethodInfoDict.TryAdd(ECallback.UPDATE_RE_UIS_PAGE_UIS_01 + i, this.GetType().GetMethod(oUpdateFuncName, KCDefine.B_BINDING_F_NON_PUBLIC_INSTANCE));
			}

			for(int i = 0; i < m_oScrollSnapDict.GetValueOrDefault(EKey.RE_UIS_PAGE_SCROLL_SNAP).NumberOfPanels; ++i) {
				string oPageUIsName = string.Format(KDefine.LES_OBJ_N_FMT_RE_UIS_PAGE_UIS, i + KCDefine.B_VAL_1_INT);
				m_oUIsDict.TryAdd(EKey.RE_UIS_PAGE_UIS_01 + i, m_oScrollSnapDict.GetValueOrDefault(EKey.RE_UIS_PAGE_SCROLL_SNAP).gameObject.ExFindChild(oPageUIsName));
			}

			for(int i = 0; i < m_oScrollSnapDict.GetValueOrDefault(EKey.RE_UIS_PAGE_SCROLL_SNAP).NumberOfPanels; ++i) {
				m_oMethodInfoDict.GetValueOrDefault(ECallback.SETUP_RE_UIS_PAGE_UIS_01 + i)?.Invoke(this, null);
			}
			// 스크롤 뷰를 설정한다 }
		}

		/** 오른쪽 에디터 UI 페이지 UI 1 를 설정한다 */
		private void SetupREUIsPageUIs01() {
			// 입력 필드를 설정한다
			CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
				(EKey.RE_UIS_PAGE_UIS_01_LEVEL_INPUT, $"{EKey.RE_UIS_PAGE_UIS_01_LEVEL_INPUT}", this.RightEditorUIs),
				(EKey.RE_UIS_PAGE_UIS_01_NUM_CELLS_X_INPUT, $"{EKey.RE_UIS_PAGE_UIS_01_NUM_CELLS_X_INPUT}", this.RightEditorUIs),
				(EKey.RE_UIS_PAGE_UIS_01_NUM_CELLS_Y_INPUT, $"{EKey.RE_UIS_PAGE_UIS_01_NUM_CELLS_Y_INPUT}", this.RightEditorUIs)
			}, m_oInputDict);
		}

		/** 오른쪽 에디터 UI 상태를 갱신한다 */
		private void UpdateRightEditorUIsState() {
			// 텍스트를 설정한다
			int nNumLevelInfos = CLevelInfoTable.Inst.GetNumLevelInfos(m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID02, m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID03);
			m_oTextDict.GetValueOrDefault(EKey.RE_UIS_TITLE_TEXT)?.ExSetText<Text>(string.Format(CStrTable.Inst.GetStr(KCDefine.ST_KEY_C_LEVEL_PAGE_TEXT_FMT), m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID01 + KCDefine.B_VAL_1_INT, nNumLevelInfos), false);

			// 버튼을 설정한다 {
			m_oBtnDict.GetValueOrDefault(EKey.RE_UIS_REMOVE_ALL_LEVELS_BTN)?.ExSetInteractable(nNumLevelInfos > KCDefine.B_VAL_1_INT, false);

#if GOOGLE_SHEET_ENABLE
			m_oBtnDict.GetValueOrDefault(EKey.RE_UIS_LOAD_REMOTE_TABLE_BTN)?.ExSetInteractable(true, false);
#else
			m_oBtnDict.GetValueOrDefault(EKey.RE_UIS_LOAD_REMOTE_TABLE_BTN)?.ExSetInteractable(false, false);
#endif // #if GOOGLE_SHEET_ENABLE
			// 버튼을 설정한다 }

			// 스크롤 스냅이 존재 할 경우
			if(m_oScrollSnapDict.GetValueOrDefault(EKey.RE_UIS_PAGE_SCROLL_SNAP) != null) {
				// 텍스트를 설정한다
				m_oTextDict.GetValueOrDefault(EKey.RE_UIS_PAGE_TEXT)?.ExSetText<Text>(string.Format(KCDefine.B_TEXT_FMT_2_SLASH_COMBINE, m_oScrollSnapDict.GetValueOrDefault(EKey.RE_UIS_PAGE_SCROLL_SNAP).CenteredPanel + KCDefine.B_VAL_1_INT, m_oScrollSnapDict.GetValueOrDefault(EKey.RE_UIS_PAGE_SCROLL_SNAP).NumberOfPanels), false);

				// 버튼 상태를 갱신한다
				m_oBtnDict.GetValueOrDefault(EKey.RE_UIS_PREV_BTN)?.ExSetInteractable(m_oScrollSnapDict.GetValueOrDefault(EKey.RE_UIS_PAGE_SCROLL_SNAP).CenteredPanel > KCDefine.B_VAL_0_INT, false);
				m_oBtnDict.GetValueOrDefault(EKey.RE_UIS_NEXT_BTN)?.ExSetInteractable(m_oScrollSnapDict.GetValueOrDefault(EKey.RE_UIS_PAGE_SCROLL_SNAP).CenteredPanel < m_oScrollSnapDict.GetValueOrDefault(EKey.RE_UIS_PAGE_SCROLL_SNAP).NumberOfPanels - KCDefine.B_VAL_1_INT, false);
			}

			// 페이지 UI 상태를 갱신한다
			for(int i = 0; i < m_oScrollSnapDict.GetValueOrDefault(EKey.RE_UIS_PAGE_SCROLL_SNAP).NumberOfPanels; ++i) {
				m_oMethodInfoDict.GetValueOrDefault(ECallback.UPDATE_RE_UIS_PAGE_UIS_01 + i)?.Invoke(this, null);
			}
		}

		/** 오른쪽 에디터 UI 페이지 UI 1 상태를 갱신한다 */
		private void UpdateREUIsPageUIs01() {
			// 입력 필드를 갱신한다 {
			m_oInputDict.GetValueOrDefault(EKey.RE_UIS_PAGE_UIS_01_LEVEL_INPUT)?.ExSetText<InputField>($"{m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).m_stIDInfo.m_nID01 + KCDefine.B_VAL_1_INT}", false);

			m_oInputDict.GetValueOrDefault(EKey.RE_UIS_PAGE_UIS_01_NUM_CELLS_X_INPUT)?.ExSetText<InputField>((m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).NumCells.x <= KCDefine.B_VAL_0_INT) ? string.Empty : $"{m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).NumCells.x}", false);
			m_oInputDict.GetValueOrDefault(EKey.RE_UIS_PAGE_UIS_01_NUM_CELLS_Y_INPUT)?.ExSetText<InputField>((m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).NumCells.y <= KCDefine.B_VAL_0_INT) ? string.Empty : $"{m_oLevelInfoDict.GetValueOrDefault(EKey.SEL_LEVEL_INFO).NumCells.y}", false);
			// 입력 필드를 갱신한다 }
		}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endregion // 조건부 함수
	}

	/** 서브 레벨 에디터 씬 관리자 - 서브 */
	public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

#region 변수

#endregion // 변수

#region 프로퍼티

#endregion // 프로퍼티

#region 함수
		/** 씬을 설정한다 */
		private void SubSetupAwake() {
			// Do Something
		}

		/** 씬을 설정한다 */
		private void SubSetupStart() {
			// Do Something
		}
#endregion // 함수
		
#region 조건부 함수
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		/** UI 상태를 갱신한다 */
		private void SubUpdateUIsState() {
			// Do Something
		}

		/** 터치 시작 이벤트를 처리한다 */
		private void HandleTouchBeginEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			var stIdx = a_oEventData.ExGetLocalPos(this.ObjRoot).ExToIdx(m_oGridInfoList[this.SelGridInfoIdx].m_stPivotPos, NSEngine.KDefine.E_SIZE_CELL);
		}

		/** 터치 이동 이벤트를 처리한다 */
		private void HandleTouchMoveEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			var stIdx = a_oEventData.ExGetLocalPos(this.ObjRoot).ExToIdx(m_oGridInfoList[this.SelGridInfoIdx].m_stPivotPos, NSEngine.KDefine.E_SIZE_CELL);
		}

		/** 터치 종료 이벤트를 처리한다 */
		private void HandleTouchEndEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			var stIdx = a_oEventData.ExGetLocalPos(this.ObjRoot).ExToIdx(m_oGridInfoList[this.SelGridInfoIdx].m_stPivotPos, NSEngine.KDefine.E_SIZE_CELL);
		}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endregion // 조건부 함수
	}
}
#endif // #if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)

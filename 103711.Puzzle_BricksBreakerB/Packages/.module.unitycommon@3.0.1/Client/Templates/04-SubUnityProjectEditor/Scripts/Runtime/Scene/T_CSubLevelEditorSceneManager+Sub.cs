#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
using UnityEngine.EventSystems;
using EnhancedUI.EnhancedScroller;

namespace LevelEditorScene {
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
			// Do Something
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
			// Do Something
		}

		/** 터치 시작 이벤트를 처리한다 */
		private void HandleTouchBeginEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			var stPos = a_oEventData.ExGetLocalPos(this.ObjRoot, this.ScreenSize);
			var stIdx = stPos.ExToIdx(this.SelGridInfo.m_stPivotPos, NSEngine.Access.CellSize);

			// 인덱스가 유효 할 경우
			if(this.SelLevelInfo.m_oCellInfoDictContainer.ExIsValidIdx(stIdx) && !stIdx.Equals(m_oVec3IntDict[EKey.PREV_CELL_IDX])) {
				// Do Something
			}

			this.HandleTouchMoveEvent(a_oSender, a_oEventData);
		}

		/** 터치 이동 이벤트를 처리한다 */
		private void HandleTouchMoveEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			var stPos = a_oEventData.ExGetLocalPos(this.ObjRoot, this.ScreenSize);
			var stIdx = stPos.ExToIdx(this.SelGridInfo.m_stPivotPos, NSEngine.Access.CellSize);

			// 인덱스가 유효 할 경우
			if(this.SelLevelInfo.m_oCellInfoDictContainer.ExIsValidIdx(stIdx) && !stIdx.Equals(m_oVec3IntDict[EKey.PREV_CELL_IDX])) {
				var stCellInfo = this.SelLevelInfo.GetCellInfo(stIdx);

				// 객체 추가가 가능 할 경우
				if(Input.GetMouseButton((int)EMouseBtn.LEFT) && m_oObjKindsDict[EKey.SEL_OBJ_KINDS].ExIsValid()) {
					this.AddCellObjInfo(Factory.MakeEditorCellObjInfo(m_oObjKindsDict[EKey.SEL_OBJ_KINDS], this.GetEditorObjSize(), stIdx), stIdx);
				}
				// 객체 제거가 가능 할 경우
				else if(Input.GetMouseButton((int)EMouseBtn.RIGHT) && stCellInfo.m_oCellObjInfoList.ExIsValid()) {
					this.RemoveCellObjInfo(Input.GetKey(KeyCode.LeftShift) ? m_oObjKindsDict[EKey.SEL_OBJ_KINDS] : EObjKinds.NONE, stIdx);
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
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		#endregion // 조건부 함수
	}

	/** 서브 레벨 에디터 씬 관리자 - 중앙 에디터 UI */
	public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate {
		#region 조건부 함수
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		/** 중앙 에디터 UI 를 설정한다 */
		private void SubSetupMidEditorUIs() {
			// Do Something
		}

		/** 중앙 에디터 UI 상태를 갱신한다 */
		private void SubUpdateMidEditorUIsState() {
			// Do Something
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
			// Do Something
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
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		#endregion // 조건부 함수
	}
}
#endif // #if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
#endif // #if SCRIPT_TEMPLATE_ONLY

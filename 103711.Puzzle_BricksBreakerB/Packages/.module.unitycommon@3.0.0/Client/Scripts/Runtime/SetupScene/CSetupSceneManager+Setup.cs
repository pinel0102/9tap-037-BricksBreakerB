using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace SetupScene {
	/** 설정 씬 관리자 - 설정 */
	public abstract partial class CSetupSceneManager : CSceneManager {
		#region 함수
		/** 팝업 UI 를 설정한다 */
		private void SetupPopupUIs() {
			// 팝업 UI 가 없을 경우
			if(CSetupSceneManager.m_oPopupUIs == null) {
				CSetupSceneManager.m_oPopupUIs = CFactory.CreateCloneObj(KCDefine.U_OBJ_N_SCREEN_POPUP_UIS, CResManager.Inst.GetRes<GameObject>(KCDefine.SS_OBJ_P_SCREEN_POPUP_UIS), null);

				try {
					CSceneManager.SetScreenPopupUIs(CSetupSceneManager.m_oPopupUIs.ExFindChild(KCDefine.U_OBJ_N_SCREEN_POPUP_UIS, false));
				} finally {
					DontDestroyOnLoad(CSetupSceneManager.m_oPopupUIs);
					CFunc.SetupScreenUIs(CSetupSceneManager.m_oPopupUIs, KCDefine.U_SORTING_O_SCREEN_POPUP_UIS);
				}
			}
		}

		/** 최상위 UI 를 설정한다 */
		private void SetupTopmostUIs() {
			// 최상위 UI 가 없을 경우
			if(CSetupSceneManager.m_oTopmostUIs == null) {
				CSetupSceneManager.m_oTopmostUIs = CFactory.CreateCloneObj(KCDefine.U_OBJ_N_SCREEN_TOPMOST_UIS, CResManager.Inst.GetRes<GameObject>(KCDefine.SS_OBJ_P_SCREEN_TOPMOST_UIS), null);

				try {
					CSceneManager.SetScreenTopmostUIs(CSetupSceneManager.m_oTopmostUIs.ExFindChild(KCDefine.U_OBJ_N_SCREEN_TOPMOST_UIS, false));
				} finally {
					DontDestroyOnLoad(CSetupSceneManager.m_oTopmostUIs);
					CFunc.SetupScreenUIs(CSetupSceneManager.m_oTopmostUIs, KCDefine.U_SORTING_O_SCREEN_TOPMOST_UIS);
				}
			}
		}

		/** 절대 UI 를 설정한다 */
		private void SetupAbsUIs() {
			// 절대 UI 가 없을 경우
			if(CSetupSceneManager.m_oAbsUIs == null) {
				CSetupSceneManager.m_oAbsUIs = CFactory.CreateCloneObj(KCDefine.U_OBJ_N_SCREEN_ABS_UIS, CResManager.Inst.GetRes<GameObject>(KCDefine.SS_OBJ_P_SCREEN_ABS_UIS), null);

				try {
					CSceneManager.SetScreenAbsUIs(CSetupSceneManager.m_oAbsUIs.ExFindChild(KCDefine.U_OBJ_N_SCREEN_ABS_UIS, false));
				} finally {
					DontDestroyOnLoad(CSetupSceneManager.m_oAbsUIs);
					CFunc.SetupScreenUIs(CSetupSceneManager.m_oAbsUIs, KCDefine.U_SORTING_O_SCREEN_ABS_UIS);
				}
			}
		}

		/** 타이머 관리자를 설정한다 */
		private void SetupTimerManager() {
			// 타이머 관리자가 없을 경우
			if(CSetupSceneManager.m_oTimerManager == null) {
				CSetupSceneManager.m_oTimerManager = CFactory.CreateCloneObj(KCDefine.SS_OBJ_N_TIMER_MANAGER, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_TIMER_MANAGER), null);
			}
		}
		#endregion // 함수

		#region 조건부 함수
#if DEBUG || DEVELOPMENT_BUILD
		/** 디버그 UI 를 설정한다 */
		private void SetupDebugUIs() {
			// 디버그 UI 가 없을 경우
			if(CSetupSceneManager.m_oDebugUIs == null) {
				CSetupSceneManager.m_oDebugUIs = CFactory.CreateCloneObj(KCDefine.U_OBJ_N_SCREEN_DEBUG_UIS, CResManager.Inst.GetRes<GameObject>(KCDefine.SS_OBJ_P_SCREEN_DEBUG_UIS), null);

				try {
					CSceneManager.SetScreenDebugUIs(CSetupSceneManager.m_oDebugUIs.ExFindChild(KCDefine.U_OBJ_N_SCREEN_DEBUG_UIS, false));
					CSceneManager.SetScreenFPSInfoUIs(CSetupSceneManager.m_oDebugUIs.ExFindChild(KCDefine.U_OBJ_N_SCREEN_FPS_INFO_UIS));
					CSceneManager.SetScreenDebugInfoUIs(CSetupSceneManager.m_oDebugUIs.ExFindChild(KCDefine.U_OBJ_N_SCREEN_DEBUG_INFO_UIS));

					// 객체를 설정한다
					CSceneManager.ScreenFPSInfoUIs.SetActive(false);
					CSceneManager.ScreenDebugInfoUIs.SetActive(false);
				} finally {
					DontDestroyOnLoad(CSetupSceneManager.m_oDebugUIs);
					CFunc.SetupScreenUIs(CSetupSceneManager.m_oDebugUIs, KCDefine.U_SORTING_O_SCREEN_DEBUG_UIS);
				}
			}
		}
#endif // #if DEBUG || DEVELOPMENT_BUILD
		#endregion // 조건부 함수
	}
}

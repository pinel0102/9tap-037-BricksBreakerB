using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.Linq;
using UnityEngine.EventSystems;

namespace TitleScene {
	/** 서브 타이틀 씬 관리자 */
	public partial class CSubTitleSceneManager : CTitleSceneManager {
#region 함수
		
#endregion // 함수
	}

	/** 서브 타이틀 씬 관리자 - 서브 */
	public partial class CSubTitleSceneManager : CTitleSceneManager {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

#if DEBUG || DEVELOPMENT_BUILD
		/** 서브 테스트 UI */
		[System.Serializable]
		private struct STSubTestUIs {
			// Do Something
		}
#endif // #if DEBUG || DEVELOPMENT_BUILD

#region 변수
		/** =====> UI <===== */
#if DEBUG || DEVELOPMENT_BUILD
		[SerializeField] private STSubTestUIs m_stSubTestUIs;
#endif // #if DEBUG || DEVELOPMENT_BUILD
#endregion // 변수

#region 프로퍼티

#endregion // 프로퍼티

#region 함수
		/** 씬을 설정한다 */
		private void SubSetupAwake() {
#if DEBUG || DEVELOPMENT_BUILD
			this.SubSetupTestUIs();
#endif // #if DEBUG || DEVELOPMENT_BUILD
		}

		/** 씬을 설정한다 */
		private void SubSetupStart() {
			// Do Something
		}

		/** UI 상태를 갱신한다 */
		private void SubUpdateUIsState() {
#if DEBUG || DEVELOPMENT_BUILD
			this.SubUpdateTestUIsState();
#endif // #if DEBUG || DEVELOPMENT_BUILD
		}

		/** 터치 시작 이벤트를 처리한다 */
		private void HandleTouchBeginEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// Do Something
		}

		/** 터치 이동 이벤트를 처리한다 */
		private void HandleTouchMoveEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// Do Something
		}

		/** 터치 종료 이벤트를 처리한다 */
		private void HandleTouchEndEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 터치 모드가 아닐 경우
			if(!m_oBoolDict.GetValueOrDefault(EKey.IS_TOUCH) && CUserInfoStorage.Inst.UserInfo.LoginType != ELoginType.NONE) {
				m_oBoolDict.ExReplaceVal(EKey.IS_TOUCH, true);

#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
				string oKey = KCDefine.U_TABLE_P_G_VER_INFO.ExGetFileName(false);
				Func.LoadVerInfoGoogleSheet(KDefine.G_TABLE_INFO_GOOGLE_SHEET_DICT.GetValueOrDefault(oKey).m_oID, this.OnLoadVerInfoGoogleSheet);
#else
				CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MAIN);
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
			}
		}
#endregion // 함수

#region 조건부 함수
#if DEBUG || DEVELOPMENT_BUILD
		/** 테스트 UI 를 설정한다 */
		private void SubSetupTestUIs() {
			// Do Something
		}

		/** 테스트 UI 상태를 갱신한다 */
		private void SubUpdateTestUIsState() {
			// Do Something
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

			m_oBoolDict.ExReplaceVal(EKey.IS_TOUCH, a_bIsSuccess);
		}

		/** 구글 시트가 로드 되었을 경우 */
		private void OnLoadGoogleSheets(CServicesManager a_oSender, bool a_bIsSuccess) {
			// 로드 되었을 경우
			if(a_bIsSuccess) {
				Func.OnLoadGoogleSheets(m_oVerInfos);
				CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MAIN);
			} else {
				Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_C_ON_TABLE_LOAD_FAIL_MSG), null, false);
			}

			m_oBoolDict.ExReplaceVal(EKey.IS_TOUCH, a_bIsSuccess);
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

			m_oBoolDict.ExReplaceVal(EKey.IS_TOUCH, a_bIsSuccess);
		}
#endif // #if GOOGLE_SHEET_ENABLE
#endif // #if DEBUG || DEVELOPMENT_BUILD
#endregion // 조건부 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

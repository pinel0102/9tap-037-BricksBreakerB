using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
using GoogleSheetsToUnity;
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)

namespace TitleScene {
	/** 서브 타이틀 씬 관리자 */
	public partial class CSubTitleSceneManager : CTitleSceneManager {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			IS_TOUCH,
			TOUCH_ANI,
			TOUCH_TEXT,
			PLAY_BTN,
			GUEST_LOGIN_BTN,
			APPLE_LOGIN_BTN,
			FACEBOOK_LOGIN_BTN,
			[HideInInspector] MAX_VAL
		}

#region 변수
		private static List<EKey> m_oLoginBtnKeyList = new List<EKey>() {
			EKey.PLAY_BTN, EKey.GUEST_LOGIN_BTN, EKey.APPLE_LOGIN_BTN, EKey.FACEBOOK_LOGIN_BTN,
		};

		private Dictionary<EKey, bool> m_oBoolDict = new Dictionary<EKey, bool>();
		private Dictionary<EKey, Tween> m_oAniDict = new Dictionary<EKey, Tween>();

		/** =====> UI <===== */
		private Dictionary<EKey, TMP_Text> m_oTextDict = new Dictionary<EKey, TMP_Text>();
		private Dictionary<EKey, Button> m_oBtnDict = new Dictionary<EKey, Button>();

#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
		private SimpleJSON.JSONNode m_oVerInfos = null;
		private Dictionary<string, System.Action<CServicesManager, STGoogleSheetLoadInfo, Dictionary<string, SimpleJSON.JSONNode>, bool>> m_oGoogleSheetLoadHandlerDict = new Dictionary<string, System.Action<CServicesManager, STGoogleSheetLoadInfo, Dictionary<string, SimpleJSON.JSONNode>, bool>>();
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
#endregion // 변수

#region 함수
		/** 내비게이션 스택 이벤트를 수신했을 경우 */
		public override void OnReceiveNavStackEvent(ENavStackEvent a_eEvent) {
			base.OnReceiveNavStackEvent(a_eEvent);

			// 백 키 눌림 이벤트 일 경우
			if(a_eEvent == ENavStackEvent.BACK_KEY_DOWN) {
				Func.ShowQuitPopup(this.OnReceiveQuitPopupResult);
			}
		}

		/** 터치 이벤트를 처리한다 */
		protected override void HandleTouchEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData, ETouchEvent a_eTouchEvent) {
			base.HandleTouchEvent(a_oSender, a_oEventData, a_eTouchEvent);
			double dblDeltaTime = System.DateTime.Now.ExGetDeltaTime(CSceneManager.ActiveSceneAwakeTime);

			// 배경 터치 전달자 일 경우
			if(this.BGTouchDispatcher == a_oSender && dblDeltaTime.ExIsGreate(KCDefine.B_VAL_1_REAL)) {
				switch(a_eTouchEvent) {
					case ETouchEvent.BEGIN: this.HandleTouchBeginEvent(a_oSender, a_oEventData); break;
					case ETouchEvent.MOVE: this.HandleTouchMoveEvent(a_oSender, a_oEventData); break;
					case ETouchEvent.END: this.HandleTouchEndEvent(a_oSender, a_oEventData); break;
				}
			}
		}

		/** 최초 시작 상태를 갱신한다 */
		private void UpdateFirstStartState() {
			LogFunc.SendLaunchLog();
			LogFunc.SendSplashLog();

			CCommonAppInfoStorage.Inst.SetFirstStart(false);

#if(!UNITY_EDITOR && UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_LEVEL_EDITOR);
#endif // #if (!UNITY_EDITOR && UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
		}

		/** 최초 플레이 상태를 갱신한다 */
		private void UpdateFirstPlayState() {
			CCommonAppInfoStorage.Inst.AppInfo.IsFirstPlay = false;
			CCommonAppInfoStorage.Inst.SaveAppInfo();

			// 약관 동의 팝업이 닫혔을 경우
			if(CAppInfoStorage.Inst.IsCloseAgreePopup) {
				LogFunc.SendAgreeLog();
			}
		}

		/** 종료 팝업 결과를 수신했을 경우 */
		private void OnReceiveQuitPopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				a_oSender.SetIgnoreAni(true);
				this.ExLateCallFunc((a_oSender) => this.QuitApp());
			}
		}

		/** 업데이트 팝업 결과를 수신했을 경우 */
		private void OnReceiveUpdatePopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				Application.OpenURL(Access.StoreURL);
			}
		}

		/** 플레이 버튼을 눌렀을 경우 */
		private void OnTouchPlayBtn() {
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MAIN);
		}

		/** 게스트 로그인 버튼을 눌렀을 경우 */
		private void OnTouchGuestLoginBtn() {
			this.OnLogin(ELoginType.GUEST, true);
		}

		/** 애플 로그인 버튼을 눌렀을 경우 */
		private void OnTouchAppleLoginBtn() {
#if UNITY_IOS && APPLE_LOGIN_ENABLE
			Func.AppleLogin((a_oSender, a_bIsSuccess) => this.OnLogin(ELoginType.APPLE, a_bIsSuccess));
#endif // #if UNITY_IOS && APPLE_LOGIN_ENABLE
		}

		/** 페이스 북 로그인 버튼을 눌렀을 경우 */
		private void OnTouchFacebookLoginBtn() {
#if FACEBOOK_MODULE_ENABLE
			Func.FacebookLogin((a_oSender, a_bIsSuccess) => this.OnLogin(ELoginType.FACEBOOK, a_bIsSuccess));
#endif // #if FACEBOOK_MODULE_ENABLE
		}

		/** 로그인 되었을 경우 */
		private void OnLogin(ELoginType a_eLoginType, bool a_bIsSuccess) {
			// 로그인 되었을 경우
			if(a_bIsSuccess) {
				CUserInfoStorage.Inst.UserInfo.LoginType = a_eLoginType;
				CUserInfoStorage.Inst.SaveUserInfo();

				this.UpdateUIsState();

				m_oTextDict.GetValueOrDefault(EKey.TOUCH_TEXT)?.gameObject.SetActive(true);
				m_oAniDict.ExAssignVal(EKey.TOUCH_ANI, m_oTextDict.GetValueOrDefault(EKey.TOUCH_TEXT)?.DOFaceFade(KCDefine.B_VAL_1_REAL / KCDefine.B_VAL_2_REAL, KCDefine.B_VAL_1_REAL).SetAutoKill().SetEase(KCDefine.U_EASE_DEF).SetLoops(KCDefine.B_TIMES_INT_INFINITE, LoopType.Yoyo).SetUpdate(true));
			}
		}
#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

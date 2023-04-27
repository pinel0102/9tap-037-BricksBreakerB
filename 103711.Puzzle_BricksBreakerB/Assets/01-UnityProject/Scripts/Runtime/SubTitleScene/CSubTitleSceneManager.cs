using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.Linq;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

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
		private Dictionary<EKey, bool> m_oBoolDict = new Dictionary<EKey, bool>() {
			[EKey.IS_TOUCH] = false
		};

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
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				// 텍스트를 설정한다 {
				CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
					(EKey.TOUCH_TEXT, $"{EKey.TOUCH_TEXT}", this.UIsBase)
				}, m_oTextDict);

				m_oTextDict[EKey.TOUCH_TEXT]?.gameObject.SetActive(false);
				// 텍스트를 설정한다 }

				// 버튼을 설정한다
				CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
					(EKey.PLAY_BTN, $"{EKey.PLAY_BTN}", this.UIsBase, this.OnTouchPlayBtn),
					(EKey.GUEST_LOGIN_BTN, $"{EKey.GUEST_LOGIN_BTN}", this.UIsBase, this.OnTouchGuestLoginBtn),
					(EKey.APPLE_LOGIN_BTN, $"{EKey.APPLE_LOGIN_BTN}", this.UIsBase, this.OnTouchAppleLoginBtn),
					(EKey.FACEBOOK_LOGIN_BTN, $"{EKey.FACEBOOK_LOGIN_BTN}", this.UIsBase, this.OnTouchFacebookLoginBtn)
				}, m_oBtnDict);

#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
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
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)

				this.SubAwake();
			}
		}

		/** 초기화 */
		public override void Start() {
			base.Start();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				// 업데이트가 가능 할 경우
				if(!CAppInfoStorage.Inst.IsIgnoreUpdate && COptsInfoTable.Inst.EtcOptsInfo.m_bIsEnableTitleScene && CCommonAppInfoStorage.Inst.IsEnableUpdate()) {
					CAppInfoStorage.Inst.SetIgnoreUpdate(true);
					this.ExLateCallFunc((a_oSender) => Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_UPDATE_P_MSG), this.OnReceiveUpdatePopupResult));
				}

				this.SubStart();
				this.UpdateUIsState();

				// 최초 시작 일 경우
				if(CCommonAppInfoStorage.Inst.IsFirstStart) {
					this.UpdateFirstStartState();
				}

				// 최초 실행 일 경우
				if(CCommonAppInfoStorage.Inst.AppInfo.IsFirstRunning) {
					this.UpdateFirstRunningState();
				}

				// 타이틀 씬이 유효 할 경우
				if(COptsInfoTable.Inst.EtcOptsInfo.m_bIsEnableTitleScene) {
					Func.PlayBGSnd(EResKinds.SND_BG_SCENE_TITLE_01);

					// 로그인 되었을 경우
					if(GlobalDefine.UserInfo.LoginType != ELoginType.NONE) {
						this.OnLogin(GlobalDefine.UserInfo.LoginType, true);
					}
				} else {
					CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MAIN);
				}
			}
		}

		/** 제거 되었을 경우 */
		public override void OnDestroy() {
			base.OnDestroy();

			try {
				// 앱이 실행 중 일 경우
				if(CSceneManager.IsAppRunning) {
					foreach(var stKeyVal in m_oAniDict) {
						stKeyVal.Value?.Kill();
					}

					this.SubOnDestroy();
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CTitleGameSceneManager.OnDestroy Exception: {oException.Message}");
			}
		}

		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				this.SubOnUpdate(a_fDeltaTime);
			}
		}

		/** 내비게이션 스택 이벤트를 수신했을 경우 */
		public override void OnReceiveNavStackEvent(ENavStackEvent a_eEvent) {
			base.OnReceiveNavStackEvent(a_eEvent);

			// 백 키 눌림 이벤트 일 경우
			if(a_eEvent == ENavStackEvent.BACK_KEY_DOWN) {
				Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_QUIT_P_MSG), this.OnReceiveQuitPopupResult);
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

		/** UI 상태를 갱신한다 */
		private void UpdateUIsState() {
			// 버튼을 갱신한다 {
#if UNITY_IOS && APPLE_LOGIN_ENABLE
			m_oBtnDict[EKey.APPLE_LOGIN_BTN]?.gameObject.SetActive(true);
#else
			m_oBtnDict[EKey.APPLE_LOGIN_BTN]?.gameObject.SetActive(false);
#endif // #if UNITY_IOS && APPLE_LOGIN_ENABLE

			var oLoginBtnKeyList = new List<EKey>() {
				EKey.PLAY_BTN, EKey.GUEST_LOGIN_BTN, EKey.APPLE_LOGIN_BTN, EKey.FACEBOOK_LOGIN_BTN,
			};

			for(int i = 0; i < oLoginBtnKeyList.Count; ++i) {
				// 로그인 되었을 경우
				if(GlobalDefine.UserInfo.LoginType != ELoginType.NONE) {
					m_oBtnDict.GetValueOrDefault(oLoginBtnKeyList[i])?.gameObject.SetActive(false);
				}
			}
			// 버튼을 갱신한다 }

			this.SubUpdateUIsState();
		}

		/** 최초 시작 상태를 갱신한다 */
		private void UpdateFirstStartState() {
			LogFunc.SendLaunchLog();
			LogFunc.SendSplashLog();

			CCommonAppInfoStorage.Inst.SetFirstStart(false);
		}

		/** 최초 실행 상태를 갱신한다 */
		private void UpdateFirstRunningState() {
			CCommonAppInfoStorage.Inst.AppInfo.IsFirstRunning = false;
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
				GlobalDefine.UserInfo.LoginType = a_eLoginType;
				GlobalDefine.SaveUserData();

				this.UpdateUIsState();

				m_oTextDict[EKey.TOUCH_TEXT]?.gameObject.SetActive(true);
				m_oAniDict.ExAssignVal(EKey.TOUCH_ANI, m_oTextDict[EKey.TOUCH_TEXT]?.DOFaceFade(KCDefine.B_VAL_1_REAL / KCDefine.B_VAL_2_REAL, KCDefine.B_VAL_1_REAL).SetAutoKill().SetEase(KCDefine.U_EASE_DEF).SetLoops(KCDefine.B_TIMES_INT_INFINITE, LoopType.Yoyo).SetUpdate(true));
			}
		}
		#endregion // 함수

		#region 조건부 함수
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
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

			m_oBoolDict[EKey.IS_TOUCH] = a_bIsSuccess;
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

			m_oBoolDict[EKey.IS_TOUCH] = a_bIsSuccess;
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

			m_oBoolDict[EKey.IS_TOUCH] = a_bIsSuccess;
		}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
		#endregion // 조건부 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

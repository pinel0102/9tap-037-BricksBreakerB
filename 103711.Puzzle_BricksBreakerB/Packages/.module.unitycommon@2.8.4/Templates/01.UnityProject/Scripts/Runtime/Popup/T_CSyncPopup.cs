#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 동기화 팝업 */
public partial class CSyncPopup : CSubPopup {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		IS_LOAD_USER_INFO,
		LOGIN_UIS,
		LOGOUT_UIS,
		[HideInInspector] MAX_VAL
	}

	#region 변수
	private Dictionary<EKey, bool> m_oBoolDict = new Dictionary<EKey, bool>();

	/** =====> 객체 <===== */
	private Dictionary<EKey, GameObject> m_oUIsDict = new Dictionary<EKey, GameObject>();
	#endregion // 변수

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 객체를 설정한다
		CFunc.SetupObjs(new List<(EKey, string, GameObject)>() {
			(EKey.LOGIN_UIS, $"{EKey.LOGIN_UIS}", this.Contents),
			(EKey.LOGOUT_UIS, $"{EKey.LOGIN_UIS}", this.Contents)
		}, m_oUIsDict);

		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
			(KCDefine.U_OBJ_N_LOGIN_BTN, this.Contents, this.OnTouchLoginBtn),
			(KCDefine.U_OBJ_N_LOGOUT_BTN, this.Contents, this.OnTouchLogoutBtn),
			(KCDefine.U_OBJ_N_LOAD_BTN, this.Contents, this.OnTouchLoadBtn),
			(KCDefine.U_OBJ_N_SAVE_BTN, this.Contents, this.OnTouchSaveBtn)
		}, false);

		this.SubSetupAwake();
	}

	/** 초기화 */
	public override void Init() {
		base.Init();
		this.SubInit();
	}

	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {
		// 객체를 갱신한다 {
#if FIREBASE_MODULE_ENABLE
		m_oUIsDict.GetValueOrDefault(EKey.LOGIN_UIS)?.SetActive(CFirebaseManager.Inst.IsLogin);
		m_oUIsDict.GetValueOrDefault(EKey.LOGOUT_UIS)?.SetActive(!CFirebaseManager.Inst.IsLogin);
#endif // #if FIREBASE_MODULE_ENABLE
		// 객체를 갱신한다 }

		this.SubUpdateUIsState();
	}

	/** 로그인 버튼을 눌렀을 경우 */
	private void OnTouchLoginBtn() {
#if FIREBASE_MODULE_ENABLE
		Func.FirebaseLogin(this.OnLogin);
#endif // #if FIREBASE_MODULE_ENABLE
	}

	/** 로그아웃 버튼을 눌렀을 경우 */
	private void OnTouchLogoutBtn() {
#if FIREBASE_MODULE_ENABLE
		Func.FirebaseLogout(this.OnLogout);
#endif // #if FIREBASE_MODULE_ENABLE
	}

	/** 로드 버튼을 눌렀을 경우 */
	private void OnTouchLoadBtn() {
		Func.ShowLoadPopup((a_oSender, a_bIsOK) => {
#if FIREBASE_MODULE_ENABLE
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				a_oSender.SetIgnoreAni(true);
				Func.LoadUserInfo(this.OnLoadUserInfo);
			}
#endif // #if FIREBASE_MODULE_ENABLE
		});
	}

	/** 저장 버튼을 눌렀을 경우 */
	private void OnTouchSaveBtn() {
		Func.ShowSavePopup((a_oSender, a_bIsOK) => {
#if FIREBASE_MODULE_ENABLE
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				a_oSender.SetIgnoreAni(true);
				Func.SaveUserInfo(this.OnSaveUserInfo);
			}
#endif // #if FIREBASE_MODULE_ENABLE
		});
	}
	#endregion // 함수

	#region 조건부 함수
#if FIREBASE_MODULE_ENABLE
	/** 로그인 되었을 경우 */
	private void OnLogin(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		// 로그인 되었을 경우
		if(a_bIsSuccess) {
			// Do Something
		}

		this.UpdateUIsState();
	}

	/** 로그아웃 되었을 경우 */
	private void OnLogout(CFirebaseManager a_oSender) {
		this.UpdateUIsState();
	}

	/** 유저 정보가 로드 되었을 경우 */
	private void OnLoadUserInfo(CFirebaseManager a_oSender, string a_oJSONStr, bool a_bIsSuccess) {
		// 로드 되었을 경우
		if(a_bIsSuccess && a_oJSONStr.ExIsValid()) {
			var oJSONNode = SimpleJSON.JSON.Parse(a_oJSONStr);

			string oUserInfoStr = oJSONNode[KCDefine.B_KEY_JSON_USER_INFO_DATA];
			string oGameInfoStr = oJSONNode[KCDefine.B_KEY_JSON_GAME_INFO_DATA];
			string oCommonAppInfoStr = oJSONNode[KCDefine.B_KEY_JSON_COMMON_APP_INFO_DATA];
			string oCommonUserInfoStr = oJSONNode[KCDefine.B_KEY_JSON_COMMON_USER_INFO_DATA];

			CUserInfoStorage.Inst.ResetUserInfo(oUserInfoStr);
			CUserInfoStorage.Inst.SaveUserInfo();

			CGameInfoStorage.Inst.ResetGameInfo(oGameInfoStr);
			CGameInfoStorage.Inst.SaveGameInfo();

			CCommonAppInfoStorage.Inst.ResetAppInfo(oCommonAppInfoStr);
			CCommonAppInfoStorage.Inst.SaveAppInfo();

			CCommonUserInfoStorage.Inst.ResetUserInfo(oCommonUserInfoStr);
			CCommonUserInfoStorage.Inst.SaveUserInfo();

#if ADS_MODULE_ENABLE
			// 광고 제거 상품을 결제했을 경우
			if(CUserInfoStorage.Inst.IsPurchaseRemoveAds) {
				Func.CloseBannerAds(null);

				CAdsManager.Inst.IsEnableBannerAds = false;
				CAdsManager.Inst.IsEnableFullscreenAds = false;
			}
#endif // #if ADS_MODULE_ENABLE
		}

		m_oBoolDict.ExReplaceVal(EKey.IS_LOAD_USER_INFO, a_bIsSuccess && a_oJSONStr.ExIsValid());

		Func.OnLoadUserInfo(a_oSender, a_oJSONStr, m_oBoolDict.GetValueOrDefault(EKey.IS_LOAD_USER_INFO), this.OnReceiveLoadSuccessPopupResult);
		CSceneManager.ScreenPopupUIs.ExEnumerateComponents<CAlertPopup>((a_oPopupSender) => { a_oPopupSender.SetIgnoreNavStackEvent(m_oBoolDict.GetValueOrDefault(EKey.IS_LOAD_USER_INFO)); return true; });
	}

	/** 유저 정보가 저장 되었을 경우 */
	private void OnSaveUserInfo(CFirebaseManager a_oSender, bool a_bIsSuccess) {
		// 저장 되었을 경우
		if(a_bIsSuccess) {
			// Do Something
		}

		this.UpdateUIsState();
		Func.OnSaveUserInfo(a_oSender, a_bIsSuccess, null);
	}

	/** 로드 팝업 결과를 수신했을 경우 */
	private void OnReceiveLoadSuccessPopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
		// 유저 정보를 로드했을 경우
		if(a_bIsOK && m_oBoolDict.GetValueOrDefault(EKey.IS_LOAD_USER_INFO)) {
			this.ExLateCallFunc((a_oSender) => {
				CScheduleManager.Inst.Reset();
				CNavStackManager.Inst.Reset();

				CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_TITLE);
			});
		}
	}
#endif // #if FIREBASE_MODULE_ENABLE
	#endregion // 조건부 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

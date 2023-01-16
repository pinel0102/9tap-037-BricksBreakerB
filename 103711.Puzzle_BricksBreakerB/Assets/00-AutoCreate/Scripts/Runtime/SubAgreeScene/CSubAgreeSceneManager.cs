using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCENE_TEMPLATES_MODULE_ENABLE
namespace AgreeScene {
	/** 서브 약관 동의 씬 관리자 */
	public partial class CSubAgreeSceneManager : CAgreeSceneManager {
		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 초기화 되었을 경우
			if(CSceneManager.IsInit) {
				CFunc.ShowLog($"Country Code: {CCommonAppInfoStorage.Inst.CountryCode}", KCDefine.B_LOG_COLOR_PLATFORM_INFO);
				CFunc.ShowLog($"System Language: {CCommonAppInfoStorage.Inst.SystemLanguage}", KCDefine.B_LOG_COLOR_PLATFORM_INFO);

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
				Func.SetupStrTable();

#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
				Func.SetupGoogleSheetInfoValCreators();
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
			}
		}

		/** 한국 약관 동의 팝업을 출력한다 */
		protected override void ShowKRAgreePopup(string a_oPrivacy, string a_oServices) {
			this.ShowAgreePopup(a_oPrivacy, a_oServices, EAgreePopup.KR);
		}

		/** 유럽 연합 약관 동의 팝업을 출력한다 */
		protected override void ShowEUAgreePopup(string a_oPrivacyURL, string a_oServicesURL) {
			this.ShowAgreePopup(a_oPrivacyURL, a_oServicesURL, EAgreePopup.EU);
		}

		/** 약관 동의 팝업을 출력한다 */
		private void ShowAgreePopup(string a_oPrivacy, string a_oServices, EAgreePopup a_eAgreePopup) {
#if MODE_PORTRAIT_ENABLE
			string oObjPath = KCDefine.AS_OBJ_P_PORTRAIT_AGREE_POPUP;
#else
			string oObjPath = KCDefine.AS_OBJ_P_LANDSCAPE_AGREE_POPUP;
#endif // #if MODE_PORTRAIT_ENABLE

			var oAgreePopup = CPopup.Create<CAgreePopup>(KCDefine.AS_OBJ_N_AGREE_POPUP, oObjPath, this.PopupUIs);
			oAgreePopup.Init(CAgreePopup.MakeParams(a_oPrivacy, a_oServices, a_eAgreePopup));
			oAgreePopup.Show(null, this.OnCloseAgreePopup);
		}

		/** 약관 동의 팝업이 닫혔을 경우 */
		private void OnCloseAgreePopup(CPopup a_oSender) {
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
			CAppInfoStorage.Inst.SetCloseAgreePopup(true);
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

			this.LoadNextScene();
		}
		#endregion // 함수
	}
}
#endif // #if SCENE_TEMPLATES_MODULE_ENABLE

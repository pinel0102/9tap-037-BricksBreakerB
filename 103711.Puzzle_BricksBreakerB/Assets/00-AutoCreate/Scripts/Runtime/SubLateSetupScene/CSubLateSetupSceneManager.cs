using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCENE_TEMPLATES_MODULE_ENABLE
#if UNITY_ANDROID
using UnityEngine.Android;
#endif // #if UNITY_ANDROID

namespace LateSetupScene {
	/** 서브 지연 설정 씬 관리자 */
	public partial class CSubLateSetupSceneManager : CLateSetupSceneManager {
		#region 변수
		[SerializeField] private EUserType m_eUserType = EUserType.NONE;
		#endregion // 변수

		#region 프로퍼티
		public override bool IsAutoInitManager => true;
		#endregion // 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 초기화 되었을 경우
			if(CSceneManager.IsInit) {
#if UNITY_EDITOR
				// 유저 타입이 유효 할 경우
				if(m_eUserType.ExIsValid()) {
					CCommonUserInfoStorage.Inst.UserInfo.UserType = m_eUserType;
				} else {
					CCommonUserInfoStorage.Inst.UserInfo.UserType = CCommonUserInfoStorage.Inst.UserInfo.UserType.ExIsValid() ? CCommonUserInfoStorage.Inst.UserInfo.UserType : EUserType.A;
				}
#else
				// 유저 타입이 유효하지 않을 경우
				if(!CCommonUserInfoStorage.Inst.UserInfo.UserType.ExIsValid()) {
#if AB_TEST_ENABLE
					int nSumVal = CCommonAppInfoStorage.Inst.AppInfo.DeviceID.Sum((a_chLetter) => a_chLetter);
					CCommonUserInfoStorage.Inst.UserInfo.UserType = (nSumVal % KCDefine.B_VAL_2_INT <= KCDefine.B_VAL_0_INT) ? EUserType.A : EUserType.B;
#else
					CCommonUserInfoStorage.Inst.UserInfo.UserType = EUserType.A;
#endif // #if AB_TEST_ENABLE
				}
#endif // #if UNITY_EDITOR

#if UNITY_ANDROID && EXTERNAL_STORAGE_ENABLE
				this.UserPermissionList.ExAddVal(Permission.ExternalStorageRead);
				this.UserPermissionList.ExAddVal(Permission.ExternalStorageWrite);
#endif // #if UNITY_ANDROID && EXTERNAL_STORAGE_ENABLE

                CLateSetupSceneManager.SetPurchaseGoldenAim(CUserInfoStorage.Inst.IsPurchaseGoldenAim);
#if ADS_MODULE_ENABLE
#if(EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE)
				CLateSetupSceneManager.SetPurchaseRemoveAds(CUserInfoStorage.Inst.IsPurchaseRemoveAds);
#endif // #if(EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE)

#if(!SAMPLE_PROJ && !EDITOR_DIST_BUILD && !CREATIVE_DIST_BUILD && !STUDY_MODULE_ENABLE)
				CLateSetupSceneManager.SetAutoLoadBannerAds(true);
				CLateSetupSceneManager.SetAutoLoadRewardAds(true);
				CLateSetupSceneManager.SetAutoLoadFullscreenAds(true);
#endif // #if(!SAMPLE_PROJ && !EDITOR_DIST_BUILD && !CREATIVE_DIST_BUILD && !STUDY_MODULE_ENABLE)
#endif // #if ADS_MODULE_ENABLE
			}
		}

		/** 씬을 설정한다 */
		protected override void Setup() {
			base.Setup();

            GlobalDefine.SetGoldenAim(CUserInfoStorage.Inst.IsPurchaseGoldenAim);

#if ADS_MODULE_ENABLE && (EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE)
			CAdsManager.Inst.IsEnableBannerAds = !CUserInfoStorage.Inst.IsPurchaseRemoveAds;
			CAdsManager.Inst.IsEnableFullscreenAds = !CUserInfoStorage.Inst.IsPurchaseRemoveAds;
#endif // #if ADS_MODULE_ENABLE && (EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE)
		}

		/** 추적 설명 팝업을 출력한다 */
		protected override void ShowTrackingDescPopup() {
			// 추적 설명 팝업 출력이 가능 할 경우
			if(CCommonAppInfoStorage.Inst.AppInfo.IsEnableShowTrackingDescPopup) {
				var oTrackingDescPopup = CPopup.Create<CTrackingDescPopup>(KCDefine.LSS_OBJ_N_TRACKING_DESC_POPUP, KCDefine.LSS_OBJ_P_TRACKING_DESC_POPUP, this.PopupUIs);

				oTrackingDescPopup.Init(CTrackingDescPopup.MakeParams(new Dictionary<CTrackingDescPopup.ECallback, System.Action<CTrackingDescPopup>>() {
					[CTrackingDescPopup.ECallback.NEXT] = this.OnReceiveTrackingDescPopupResult
				}));

				oTrackingDescPopup.Show(null, null);
			} else {
				this.OnReceiveTrackingDescPopupResult(null);
			}
		}

		/** 추적 설명 팝업 결과를 수신했을 경우 */
		private void OnReceiveTrackingDescPopupResult(CTrackingDescPopup a_oSender) {
			a_oSender?.Close();
			this.ExLateCallFunc((a_oSender) => this.ShowTrackingConsentView(), KCDefine.U_DELAY_INIT);
		}
		#endregion // 함수

		#region 조건부 함수
#if UNITY_ANDROID
		/** 유저 권한을 요청한다 */
		protected override void RequestUserPermission(string a_oPermission, System.Action<string, bool> a_oCallback) {
			this.ExRequestUserPermission(a_oPermission, a_oCallback, true);
		}
#endif // #if UNITY_ANDROID
		#endregion // 조건부 함수
	}
}
#endif // #if SCENE_TEMPLATES_MODULE_ENABLE

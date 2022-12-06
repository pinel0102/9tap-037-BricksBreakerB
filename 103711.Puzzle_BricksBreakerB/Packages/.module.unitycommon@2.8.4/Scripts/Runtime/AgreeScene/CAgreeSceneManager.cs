using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace AgreeScene {
	/** 약관 동의 씬 관리자 */
	public abstract partial class CAgreeSceneManager : CSceneManager {
		#region 프로퍼티
		public override string SceneName => KCDefine.B_SCENE_N_AGREE;

#if UNITY_EDITOR
		public override int ScriptOrder => KCDefine.U_SCRIPT_O_AGREE_SCENE_MANAGER;
#endif // #if UNITY_EDITOR
		#endregion // 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 초기화 되었을 경우
			if(CSceneManager.IsInit) {
				CSceneManager.GetSceneManager<StartScene.CStartSceneManager>(KCDefine.B_SCENE_N_START)?.gameObject.ExSendMsg(string.Empty, KCDefine.SS_FUNC_N_START_SCENE_EVENT, EStartSceneEvent.LOAD_AGREE_SCENE, false);
			}
		}

		/** 초기화 */
		public sealed override void Start() {
			base.Start();

			// 초기화 되었을 경우
			if(CSceneManager.IsInit) {
				StartCoroutine(this.CoStart());
			}
		}

		/** 다음 씬을 로드한다 */
		protected void LoadNextScene() {
			CCommonAppInfoStorage.Inst.AppInfo.IsAgree = true;
			CCommonAppInfoStorage.Inst.SaveAppInfo();

			CSceneLoader.Inst.LoadAdditiveScene(KCDefine.B_SCENE_N_LATE_SETUP);
		}

		/** 한국 약관 동의 팝업을 출력한다 */
		protected abstract void ShowKRAgreePopup(string a_oPrivacy, string a_oServices);

		/** 유럽 연합 약관 동의 팝업을 출력한다 */
		protected abstract void ShowEUAgreePopup(string a_oPrivacyURL, string a_oServicesURL);

		/** 초기화 */
		private IEnumerator CoStart() {
			yield return CFactory.CoCreateWaitForSecs(KCDefine.U_DELAY_INIT);
			this.SetupActiveScene();

#if ROBO_TEST_ENABLE
			this.LoadNextScene();
#else
			bool bIsAgree = CCommonAppInfoStorage.Inst.AppInfo.IsAgree;

			// 약관 동의 상태 일 경우
			if(bIsAgree || !CCommonAppInfoStorage.Inst.IsNeedsAgree(CCommonAppInfoStorage.Inst.CountryCode)) {
				this.LoadNextScene();
			} else {
				// 한국 일 경우
				if(CCommonAppInfoStorage.Inst.CountryCode.Equals(KCDefine.B_KOREA_COUNTRY_CODE)) {
					var oPrivacy = CResManager.Inst.GetRes<TextAsset>(KCDefine.AS_DATA_P_PRIVACY);
					var oServices = CResManager.Inst.GetRes<TextAsset>(KCDefine.AS_DATA_P_SERVICES);

					this.ShowKRAgreePopup(oPrivacy.text, oServices.text);

					CResManager.Inst.RemoveRes<TextAsset>(KCDefine.AS_DATA_P_PRIVACY, true);
					CResManager.Inst.RemoveRes<TextAsset>(KCDefine.AS_DATA_P_SERVICES, true);
				} else {
					this.ShowEUAgreePopup(CProjInfoTable.Inst.CompanyInfo.m_oPrivacyURL, CProjInfoTable.Inst.CompanyInfo.m_oServicesURL);
				}
			}
#endif // #if ROBO_TEST_ENABLE
		}
		#endregion // 함수
	}
}

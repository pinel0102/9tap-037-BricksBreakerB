using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace SetupScene {
	/** 설정 씬 관리자 */
	public abstract partial class CSetupSceneManager : CSceneManager {
		#region 클래스 변수
		/** =====> 객체 <===== */
		private static GameObject m_oPopupUIs = null;
		private static GameObject m_oTopmostUIs = null;
		private static GameObject m_oAbsUIs = null;
		private static GameObject m_oTimerManager = null;
		private static GameObject m_oDebugUIs = null;
		#endregion // 클래스 변수

		#region 프로퍼티
		public override string SceneName => KCDefine.B_SCENE_N_SETUP;

		/** =====> 기타 <===== */
		private Dictionary<string, System.Action<string>> DeviceMsgHandlerDict { get; } = new Dictionary<string, System.Action<string>>();

#if UNITY_EDITOR
		public override int ScriptOrder => KCDefine.U_SCRIPT_O_SETUP_SCENE_MANAGER;
#endif // #if UNITY_EDITOR
		#endregion // 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 초기화 되었을 경우
			if(CSceneManager.IsInit) {
				this.DeviceMsgHandlerDict.TryAdd(KCDefine.B_CMD_GET_DEVICE_ID, this.OnReceiveGetDeviceIDMsg);
				this.DeviceMsgHandlerDict.TryAdd(KCDefine.B_CMD_GET_COUNTRY_CODE, this.OnReceiveGetCountryCodeMsg);

				CSceneManager.GetSceneManager<StartScene.CStartSceneManager>(KCDefine.B_SCENE_N_START)?.gameObject.ExSendMsg(string.Empty, KCDefine.SS_FUNC_N_START_SCENE_EVENT, EStartSceneEvent.LOAD_SETUP_SCENE, false);
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

		/** 디바이스 메세지를 수신했을 경우 */
		private void OnReceiveDeviceMsg(string a_oCmd, string a_oMsg) {
			this.DeviceMsgHandlerDict.GetValueOrDefault(a_oCmd)?.Invoke(a_oMsg);
		}

		/** 씬을 설정한다 */
		protected virtual void Setup() {
			this.SetupPopupUIs();
			this.SetupTopmostUIs();
			this.SetupAbsUIs();
			this.SetupTimerManager();

#if DEBUG || DEVELOPMENT_BUILD
			this.SetupDebugUIs();
#endif // #if DEBUG || DEVELOPMENT_BUILD

#if PURCHASE_MODULE_ENABLE
			CProductInfoTable.Inst.LoadProductInfos();
#endif // #if PURCHASE_MODULE_ENABLE
		}

		/** 디바이스 식별자 반환 메세지를 수신했을 경우 */
		private void OnReceiveGetDeviceIDMsg(string a_oMsg) {
			CCommonAppInfoStorage.Inst.SetDeviceType(CAccess.DeviceType);

			// 디바이스 식별자 설정이 필요 할 경우
			if(!CCommonAppInfoStorage.Inst.AppInfo.DeviceID.ExIsValid() || CCommonAppInfoStorage.Inst.AppInfo.DeviceID.Equals(KCDefine.B_TEXT_UNKNOWN)) {
				CCommonAppInfoStorage.Inst.AppInfo.DeviceID = a_oMsg.ExIsValid() ? a_oMsg : KCDefine.B_TEXT_UNKNOWN;
			}

			CCommonAppInfoStorage.Inst.SaveAppInfo();
			CUnityMsgSender.Inst.SendGetCountryCodeMsg(this.OnReceiveDeviceMsg);
		}

		/** 국가 코드 반환 메세지를 수신했을 경우 */
		private void OnReceiveGetCountryCodeMsg(string a_oMsg) {
#if UNITY_EDITOR
			CCommonAppInfoStorage.Inst.SetCountryCode(a_oMsg.ExIsValid() ? a_oMsg.ToUpper() : KCDefine.B_KOREA_COUNTRY_CODE);
#else
			CCommonAppInfoStorage.Inst.SetCountryCode(a_oMsg.ExIsValid() ? a_oMsg.ToUpper() : KCDefine.B_AMERICA_COUNTRY_CODE);
#endif // #if UNITY_EDITOR

			CSceneManager.SetSetup(true);
			CCommonAppInfoStorage.Inst.SaveAppInfo();

			CSceneLoader.Inst.LoadAdditiveScene(KCDefine.B_SCENE_N_AGREE);
		}

		/** 초기화 */
		private IEnumerator CoStart() {
			yield return CFactory.CoCreateWaitForSecs(KCDefine.U_DELAY_INIT);
			this.Setup();

			CUnityMsgSender.Inst.SendGetDeviceIDMsg(this.OnReceiveDeviceMsg);
		}
		#endregion // 함수
	}
}

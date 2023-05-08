using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if FIREBASE_MODULE_ENABLE
using System.Threading.Tasks;

#if FIREBASE_CONFIG_ENABLE
using Firebase.RemoteConfig;
#endif // #if FIREBASE_CONFIG_ENABLE

/** 파이어 베이스 관리자 - 구성 */
public partial class CFirebaseManager : CSingleton<CFirebaseManager> {
#region 함수
	/** 기본 구성을 설정한다 */
	public void SetupDefConfigs(Dictionary<string, object> a_oDataDict, System.Action<CFirebaseManager, bool> a_oCallback) {
		CFunc.ShowLog($"CFirebaseManager.SetupDefConfigs: {a_oDataDict}", KCDefine.B_LOG_COLOR_PLUGIN);
		CAccess.Assert(a_oDataDict != null);

#if(UNITY_IOS || UNITY_ANDROID) && FIREBASE_CONFIG_ENABLE
		// 초기화 되었을 경우
		if(m_oBoolDict[EKey.IS_INIT]) {
			m_oCallbackDict01.ExReplaceVal(EFirebaseCallback.SETUP_DEF_CONFIGS, a_oCallback);
			CTaskManager.Inst.WaitAsyncTask(FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(a_oDataDict), this.OnSetupDefConfigs);
		} else {
			CFunc.Invoke(ref a_oCallback, this, false);
		}
#else
		CFunc.Invoke(ref a_oCallback, this, false);
#endif // #if (UNITY_IOS || UNITY_ANDROID) && FIREBASE_CONFIG_ENABLE
	}

	/** 구성을 로드한다 */
	public void LoadConfigs(List<string> a_oKeyList, System.Action<CFirebaseManager, Dictionary<string, string>, bool> a_oCallback) {
		CFunc.ShowLog($"CFirebaseManager.LoadConfigs: {a_oKeyList}", KCDefine.B_LOG_COLOR_PLUGIN);
		CAccess.Assert(a_oKeyList != null);

#if(UNITY_IOS || UNITY_ANDROID) && FIREBASE_CONFIG_ENABLE
		// 초기화 되었을 경우
		if(m_oBoolDict[EKey.IS_INIT]) {
			a_oKeyList.ExCopyTo(m_oConfigKeyList, (a_oKey) => a_oKey);
			m_oCallbackDict03.ExReplaceVal(EFirebaseCallback.LOAD_CONFIGS, a_oCallback);
			CTaskManager.Inst.WaitAsyncTask(FirebaseRemoteConfig.DefaultInstance.FetchAndActivateAsync(), this.OnLoadConfigs);
		} else {
			CFunc.Invoke(ref a_oCallback, this, null, false);
		}
#else
		CFunc.Invoke(ref a_oCallback, this, null, false);
#endif // #if (UNITY_IOS || UNITY_ANDROID) && FIREBASE_CONFIG_ENABLE
	}
#endregion // 함수

#region 조건부 함수
#if(UNITY_IOS || UNITY_ANDROID) && FIREBASE_CONFIG_ENABLE
	/** 기본 구성을 설정했을 경우 */
	public void OnSetupDefConfigs(Task a_oTask) {
		string oErrorMsg = (a_oTask.Exception != null) ? a_oTask.Exception.Message : string.Empty;
		CFunc.ShowLog($"CFirebaseManager.OnSetupDefConfigs: {oErrorMsg}", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_FIREBASE_M_SETUP_DEF_CONFIGS_CALLBACK, () => {
			m_oCallbackDict01.GetValueOrDefault(EFirebaseCallback.SETUP_DEF_CONFIGS)?.Invoke(this, a_oTask.ExIsCompleteSuccess());
		});
	}

	/** 구성을 로드했을 경우 */
	private void OnLoadConfigs(Task<bool> a_oTask) {
		string oErrorMsg = (a_oTask.Exception != null) ? a_oTask.Exception.Message : string.Empty;
		CFunc.ShowLog($"CFirebaseManager.OnLoadConfigs: {oErrorMsg}", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_FIREBASE_M_LOAD_CONFIGS_CALLBACK, () => {
			m_oCallbackDict03.GetValueOrDefault(EFirebaseCallback.LOAD_CONFIGS)?.Invoke(this, a_oTask.ExIsCompleteSuccess() ? m_oConfigKeyList.ExToDict((a_nIdx) => (m_oConfigKeyList[a_nIdx], FirebaseRemoteConfig.DefaultInstance.GetValue(m_oConfigKeyList[a_nIdx]).StringValue)) : null, a_oTask.ExIsCompleteSuccess());
		});
	}
#endif // #if (UNITY_IOS || UNITY_ANDROID) && FIREBASE_CONFIG_ENABLE
#endregion // 조건부 함수
}
#endif // #if FIREBASE_MODULE_ENABLE

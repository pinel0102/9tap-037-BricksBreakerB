using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if FIREBASE_MODULE_ENABLE
using System.IO;
using System.Threading.Tasks;

#if FIREBASE_STORAGE_ENABLE
using Firebase.Storage;
#endif // #if FIREBASE_STORAGE_ENABLE

/** 파이어 베이스 관리자 - 저장소 */
public partial class CFirebaseManager : CSingleton<CFirebaseManager> {
#region 함수
	/** 파일을 로드한다 */
	public void LoadFiles(string a_oFilePath, System.Action<CFirebaseManager, string, bool> a_oCallback) {
		CFunc.ShowLog($"CFirebaseManager.LoadDatas: {a_oFilePath}", KCDefine.B_LOG_COLOR_PLUGIN);
		CAccess.Assert(a_oFilePath.ExIsValid());

#if(UNITY_IOS || UNITY_ANDROID) && FIREBASE_STORAGE_ENABLE
		// 초기화 되었을 경우
		if(m_oBoolDict[EKey.IS_INIT]) {
			m_oCallbackDict02.ExReplaceVal(EFirebaseCallback.LOAD_FILES, a_oCallback);
			CTaskManager.Inst.WaitAsyncTask(FirebaseStorage.DefaultInstance.GetReference(a_oFilePath).GetStreamAsync(), this.OnLoadFiles);
		} else {
			CFunc.Invoke(ref a_oCallback, this, string.Empty, false);
		}
#else
		CFunc.Invoke(ref a_oCallback, this, string.Empty, false);
#endif // #if (UNITY_IOS || UNITY_ANDROID) && FIREBASE_STORAGE_ENABLE
	}
#endregion // 함수

#region 조건부 함수
#if FIREBASE_STORAGE_ENABLE
	/** 파일이 로드 되었을 경우 */
	public void OnLoadFiles(Task<Stream> a_oTask) {
		string oErrorMsg = (a_oTask.Exception != null) ? a_oTask.Exception.Message : string.Empty;
		CFunc.ShowLog($"CFirebaseManager.OnLoadFiles: {oErrorMsg}", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_FIREBASE_M_LOAD_FILES_CALLBACK, () => {
			m_oCallbackDict02.GetValueOrDefault(EFirebaseCallback.LOAD_FILES)?.Invoke(this, a_oTask.ExIsCompleteSuccess() ? CFunc.ReadStr(a_oTask.Result, true) : string.Empty, a_oTask.ExIsCompleteSuccess());
		});
	}
#endif // #if FIREBASE_STORAGE_ENABLE
#endregion // 조건부 함수
}
#endif // #if FIREBASE_MODULE_ENABLE

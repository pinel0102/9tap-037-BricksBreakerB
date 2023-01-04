using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if FIREBASE_MODULE_ENABLE
using Firebase.Extensions;
#endif // #if FIREBASE_MODULE_ENABLE

#if UNITY_ANDROID && GOOGLE_PLAY_UPDATE_ENABLE
using Google.Play.AppUpdate;
#endif // #if UNITY_ANDROID && GOOGLE_PLAY_UPDATE_ENABLE

/** 작업 관리자 */
public partial class CTaskManager : CSingleton<CTaskManager> {
	#region 변수
	private Dictionary<int, STTaskInfo> m_oTaskInfoDict = new Dictionary<int, STTaskInfo>();
	#endregion // 변수

	#region 함수
	/** 비동기 작업을 대기한다 */
	public void WaitAsyncTask(Task a_oTask, System.Action<Task> a_oCallback) {
		CAccess.Assert(a_oTask != null && !m_oTaskInfoDict.ContainsKey(a_oTask.Id));

#if FIREBASE_MODULE_ENABLE
		m_oTaskInfoDict.TryAdd(a_oTask.Id, new STTaskInfo() {
			m_oTask = a_oTask, m_oCallback = a_oCallback
		});

		a_oTask.ContinueWithOnMainThread(this.OnCompleteAsyncTask, CancellationToken.None);
#else
		StartCoroutine(this.CoWaitAsyncTask(a_oTask, a_oCallback));
#endif // #if FIREBASE_MODULE_ENABLE
	}

	/** 비동기 작업을 대기한다 */
	public IEnumerator CoWaitAsyncOperation(AsyncOperation a_oAsyncOperation, System.Action<AsyncOperation, bool> a_oCallback, bool a_bIsRealtime = false) {
		CAccess.Assert(a_oAsyncOperation != null);

		try {
			do {
				yield return CFactory.CoCreateWaitForSecs(KCDefine.B_DELTA_T_ASYNC_OPERATION, a_bIsRealtime);
				a_oCallback?.Invoke(a_oAsyncOperation, false);
			} while(!a_oAsyncOperation.isDone);

			yield return CFactory.CoCreateWaitForSecs(KCDefine.B_DELTA_T_ASYNC_OPERATION, a_bIsRealtime);
		} finally {
			CFunc.Invoke(ref a_oCallback, a_oAsyncOperation, true);
		}
	}

	/** 비동기 작업이 완료 되었을 경우 */
	private void OnCompleteAsyncTask(Task a_oTask) {
		// 비동기 작업 정보가 존재 할 경우
		if(m_oTaskInfoDict.TryGetValue(a_oTask.Id, out STTaskInfo stTaskInfo)) {
			try {
				m_oTaskInfoDict.ExRemoveVal(a_oTask.Id);
			} finally {
				CFunc.Invoke(ref stTaskInfo.m_oCallback, stTaskInfo.m_oTask);
			}
		}
	}

	/** 비동기 작업을 대기한다 */
	private IEnumerator CoWaitAsyncTask(Task a_oTask, System.Action<Task> a_oCallback, bool a_bIsRealtime = false) {
		CAccess.Assert(a_oTask != null);
		float fUpdateSkipTime = KCDefine.B_VAL_0_REAL;

		try {
			do {
				yield return CFactory.CoCreateWaitForSecs(KCDefine.B_DELTA_T_ASYNC_TASK, a_bIsRealtime);
				fUpdateSkipTime += CScheduleManager.Inst.UnscaleDeltaTime;
			} while(!a_oTask.ExIsComplete() && !a_oTask.IsFaulted && !a_oTask.IsCanceled && fUpdateSkipTime.ExIsLess(KCDefine.U_TIMEOUT_ASYNC_TASK));

			yield return CFactory.CoCreateWaitForSecs(KCDefine.B_DELTA_T_ASYNC_TASK, a_bIsRealtime);
		} finally {
			CFunc.Invoke(ref a_oCallback, a_oTask);
		}
	}
	#endregion // 함수

	#region 제네릭 함수
	/** 비동기 작업을 대기한다 */
	public void WaitAsyncTask<T>(Task<T> a_oTask, System.Action<Task<T>> a_oCallback) {
		this.WaitAsyncTask(a_oTask as Task, (a_oAsyncTask) => CFunc.Invoke(ref a_oCallback, a_oAsyncTask as Task<T>));
	}
	#endregion // 제네릭 함수

	#region 조건부 함수
#if UNITY_ANDROID && GOOGLE_PLAY_UPDATE_ENABLE
	/** 업데이트 작업을 대기한다 */
	public IEnumerator WaitUpdateOperation(AppUpdateManager a_oUpdateManager, System.Action<CTaskManager, int, bool> a_oCallback) {
		var oUpdateOperation = a_oUpdateManager.GetAppUpdateInfo();
		CAccess.Assert(oUpdateOperation != null);

		yield return oUpdateOperation;

		// 업데이트 정보를 갱신했을 경우
		if(oUpdateOperation.IsSuccessful) {
			CFunc.Invoke(ref a_oCallback, this, oUpdateOperation.GetResult().AvailableVersionCode, true);
		} else {
			CFunc.Invoke(ref a_oCallback, this, CProjInfoTable.Inst.ProjInfo.m_stBuildVerInfo.m_nNum, false);
		}
	}
#endif // #if UNITY_ANDROID && GOOGLE_PLAY_UPDATE_ENABLE
	#endregion // 조건부 함수
}

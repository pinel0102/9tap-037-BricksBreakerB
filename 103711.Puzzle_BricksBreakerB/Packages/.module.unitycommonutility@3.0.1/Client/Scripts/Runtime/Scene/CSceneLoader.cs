using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

/** 씬 로더 */
public partial class CSceneLoader : CSingleton<CSceneLoader> {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		PREV_ACTIVE_SCENE_NAME,
		AWAKE_ACTIVE_SCENE_NANE,
		[HideInInspector] MAX_VAL
	}

	#region 변수
	private Dictionary<EKey, string> m_oStrDict = new Dictionary<EKey, string>() {
		[EKey.PREV_ACTIVE_SCENE_NAME] = string.Empty,
		[EKey.AWAKE_ACTIVE_SCENE_NANE] = string.Empty
	};
	#endregion // 변수

	#region 프로퍼티
	public string PrevActiveSceneName => m_oStrDict[EKey.PREV_ACTIVE_SCENE_NAME];
	public string AwakeActiveSceneName => m_oStrDict[EKey.AWAKE_ACTIVE_SCENE_NANE];
	#endregion // 프로퍼티

	#region 함수
	/** 이전 액티브 씬 이름을 변경한다 */
	public void SetPrevActiveSceneName(string a_oSceneName) {
		m_oStrDict[EKey.PREV_ACTIVE_SCENE_NAME] = a_oSceneName;
	}

	/** 최초 액티브 씬 이름을 변경한다 */
	public void SetAwakeActiveSceneName(string a_oSceneName) {
		m_oStrDict[EKey.AWAKE_ACTIVE_SCENE_NANE] = a_oSceneName;
	}

	/** 씬을 로드한다 */
	public void LoadScene(string a_oName, bool a_bIsAni = true, float a_fDuration = KCDefine.U_DURATION_ANI) {
		this.LoadScene(a_oName, a_bIsAni, false, false, a_fDuration, LoadSceneMode.Single);
	}

	/** 씬을 로드한다 */
	public void LoadSceneAsync(string a_oName, System.Action<AsyncOperation, bool> a_oCallback, float a_fDelay = KCDefine.B_VAL_0_REAL, bool a_bIsAni = true, float a_fDuration = KCDefine.U_DURATION_ANI) {
		StartCoroutine(this.DoLoadSceneAsync(a_oName, a_oCallback, a_fDelay, a_bIsAni, false, a_fDuration, LoadSceneMode.Single));
	}

	/** 씬을 로드한다 */
	public void LoadSceneByLoadingScene(string a_oName, bool a_bIsAni = true, float a_fDuration = KCDefine.U_DURATION_ANI) {
		this.LoadScene(a_oName, a_bIsAni, false, true, a_fDuration, LoadSceneMode.Single);
	}

	/** 씬을 로드한다 */
	public void LoadSceneAsyncByLoadingScene(string a_oName, float a_fDelay = KCDefine.B_VAL_0_REAL, bool a_bIsAni = true, float a_fDuration = KCDefine.U_DURATION_ANI) {
		StartCoroutine(this.DoLoadSceneAsync(a_oName, null, a_fDelay, a_bIsAni, true, a_fDuration, LoadSceneMode.Single));
	}

	/** 씬을 로드한다 */
	public void LoadAdditiveScene(string a_oName) {
		this.LoadScene(a_oName, false, false, false, KCDefine.B_VAL_0_REAL, LoadSceneMode.Additive);
	}

	/** 씬을 로드한다 */
	public void LoadAdditiveSceneAsync(string a_oName, System.Action<AsyncOperation, bool> a_oCallback, float a_fDelay = KCDefine.B_VAL_0_REAL) {
		StartCoroutine(this.DoLoadSceneAsync(a_oName, a_oCallback, a_fDelay, false, false, KCDefine.B_VAL_0_REAL, LoadSceneMode.Additive));
	}

	/** 씬을 제거한다 */
	public void UnloadSceneAsync(string a_oName, System.Action<AsyncOperation, bool> a_oCallback, float a_fDelay = KCDefine.B_VAL_0_REAL, UnloadSceneOptions a_eUnloadSceneOpts = UnloadSceneOptions.UnloadAllEmbeddedSceneObjects) {
		StartCoroutine(this.DoUnloadSceneAsync(a_oName, a_oCallback, a_fDelay, a_eUnloadSceneOpts));
	}

	/** 로딩 씬을 설정한다 */
	private void SetupLoadingScene(string a_oName, bool a_bIsAni, bool a_bIsAsync, float a_fDuration) {
		LoadingScene.CLoadingSceneManager.SetIsAni(a_bIsAni);
		LoadingScene.CLoadingSceneManager.SetIsAsync(a_bIsAsync);
		LoadingScene.CLoadingSceneManager.SetDuration(a_fDuration);
		LoadingScene.CLoadingSceneManager.SetNextSceneName(a_oName);
	}

	/** 비동기 씬 로드가 완료 되었을 경우 */
	private void OnCompleteAsyncSceneLoading(AsyncOperation a_oAsyncOperation, bool a_bIsAni, float a_fDuration) {
		// 애니메이션 모드 일 경우
		if(!this.IsIgnoreAni && a_bIsAni) {
			CSceneManager.ActiveSceneManager.StartScreenFadeInAni((a_oSender) => a_oAsyncOperation.allowSceneActivation = true, a_fDuration);
		} else {
			a_oAsyncOperation.allowSceneActivation = true;
		}
	}

	/** 씬을 로드한다 */
	private void LoadScene(string a_oName, bool a_bIsAni, bool a_bIsAsync, bool a_bIsShowLoadingScene, float a_fDuration, LoadSceneMode a_eLoadSceneMode) {
		// 애니메이션 모드 일 경우
		if(!this.IsIgnoreAni && a_bIsAni) {
			CSceneManager.ActiveSceneManager.StartScreenFadeInAni((a_oSender) => { this.DoLoadScene(a_oName, a_bIsAni, a_bIsAsync, a_bIsShowLoadingScene, a_fDuration, a_eLoadSceneMode); }, a_fDuration);
		} else {
			this.DoLoadScene(a_oName, a_bIsAni, a_bIsAsync, a_bIsShowLoadingScene, a_fDuration, a_eLoadSceneMode);
		}
	}

	/** 씬을 로드한다 */
	private void DoLoadScene(string a_oName, bool a_bIsAni, bool a_bIsAsync, bool a_bIsShowLoadingScene, float a_fDuration, LoadSceneMode a_eLoadSceneMode) {
		CAccess.Assert(!a_bIsShowLoadingScene || a_eLoadSceneMode != LoadSceneMode.Additive);

		// 로딩 씬 출력 모드 일 경우
		if(a_bIsShowLoadingScene) {
			this.SetupLoadingScene(a_oName, a_bIsAni, a_bIsAsync, a_fDuration);
			SceneManager.LoadScene(KCDefine.B_SCENE_N_LOADING, a_eLoadSceneMode);
		} else {
			SceneManager.LoadScene(a_oName, a_eLoadSceneMode);
		}
	}

	/** 씬을 로드한다 */
	private IEnumerator DoLoadSceneAsync(string a_oName, System.Action<AsyncOperation, bool> a_oCallback, float a_fDelay, bool a_bIsAni, bool a_bIsShowLoadingScene, float a_fDuration, LoadSceneMode a_eLoadSceneMode) {
		CAccess.Assert(!a_bIsShowLoadingScene || a_eLoadSceneMode != LoadSceneMode.Additive);
		yield return CFactory.CoCreateWaitForSecs(a_fDelay);

		// 로딩 씬 출력 모드 일 경우
		if(a_bIsShowLoadingScene) {
			this.LoadScene(a_oName, a_bIsAni, true, a_bIsShowLoadingScene, a_fDuration, a_eLoadSceneMode);
		} else {
			bool bIsEnableActiveScene = false;

			var oAsyncOperation = SceneManager.LoadSceneAsync(a_oName, a_eLoadSceneMode);
			oAsyncOperation.allowSceneActivation = false;

			yield return CTaskManager.Inst.CoWaitAsyncOperation(oAsyncOperation, (a_oAsyncOperation, a_bIsComplete) => {
				a_oCallback?.Invoke(a_oAsyncOperation, a_bIsComplete);

				// 비동기 로드가 완료 되었을 경우
				if(!bIsEnableActiveScene && a_oAsyncOperation.progress.ExIsGreateEquals(KCDefine.U_MAX_PERCENT_ASYNC_OPERATION)) {
					bIsEnableActiveScene = true;
					CSceneManager.ActiveSceneManager.ExLateCallFunc((a_oSender) => this.OnCompleteAsyncSceneLoading(a_oAsyncOperation, a_bIsAni, a_fDuration));
				}
			});
		}
	}

	/** 씬을 제거한다 */
	private IEnumerator DoUnloadSceneAsync(string a_oName, System.Action<AsyncOperation, bool> a_oCallback, float a_fDelay, UnloadSceneOptions a_eUnloadSceneOpts = UnloadSceneOptions.UnloadAllEmbeddedSceneObjects) {
		CAccess.Assert(!CSceneManager.ActiveSceneName.Equals(a_oName));

		yield return CFactory.CoCreateWaitForSecs(a_fDelay);
		yield return CTaskManager.Inst.CoWaitAsyncOperation(SceneManager.UnloadSceneAsync(a_oName, a_eUnloadSceneOpts), a_oCallback);
	}
	#endregion // 함수
}

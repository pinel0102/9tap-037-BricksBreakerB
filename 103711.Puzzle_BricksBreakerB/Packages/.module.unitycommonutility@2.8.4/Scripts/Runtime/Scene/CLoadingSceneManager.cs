using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace LoadingScene {
	/** 로딩 씬 관리자 */
	public abstract partial class CLoadingSceneManager : CSceneManager {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			IS_ANI,
			IS_ASYNC,
			DURATION,
			NEXT_SCENE_NANE,
			[HideInInspector] MAX_VAL
		}

		#region 클래스 변수
		private static Dictionary<EKey, bool> m_oBoolDict = new Dictionary<EKey, bool>();
		private static Dictionary<EKey, float> m_oRealDict = new Dictionary<EKey, float>();
		private static Dictionary<EKey, string> m_oStrDict = new Dictionary<EKey, string>();
		#endregion // 클래스 변수

		#region 프로퍼티
		public override string SceneName => KCDefine.B_SCENE_N_LOADING;

#if UNITY_EDITOR
		public override int ScriptOrder => KCDefine.U_SCRIPT_O_LOADING_SCENE_MANAGER;
#endif // #if UNITY_EDITOR
		#endregion // 프로퍼티

		#region 클래스 프로퍼티
		public static bool IsAni => CLoadingSceneManager.m_oBoolDict.GetValueOrDefault(EKey.IS_ANI);
		public static bool IsAsync => CLoadingSceneManager.m_oBoolDict.GetValueOrDefault(EKey.IS_ASYNC);
		public static float Duration => CLoadingSceneManager.m_oRealDict.GetValueOrDefault(EKey.DURATION);
		public static string NextSceneName => CLoadingSceneManager.m_oStrDict.GetValueOrDefault(EKey.NEXT_SCENE_NANE, string.Empty);
		#endregion // 클래스 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Start() {
			base.Start();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				// 비동기 모드 일 경우
				if(CLoadingSceneManager.m_oBoolDict.GetValueOrDefault(EKey.IS_ASYNC)) {
					CSceneLoader.Inst.LoadSceneAsync(CLoadingSceneManager.m_oStrDict.GetValueOrDefault(EKey.NEXT_SCENE_NANE, string.Empty), this.OnUpdateAsyncSceneLoadingState, KCDefine.B_VAL_0_REAL, CLoadingSceneManager.m_oBoolDict.GetValueOrDefault(EKey.IS_ANI), CLoadingSceneManager.m_oRealDict.GetValueOrDefault(EKey.DURATION));
				} else {
					CSceneLoader.Inst.LoadScene(CLoadingSceneManager.m_oStrDict.GetValueOrDefault(EKey.NEXT_SCENE_NANE, string.Empty), CLoadingSceneManager.m_oBoolDict.GetValueOrDefault(EKey.IS_ANI), CLoadingSceneManager.m_oRealDict.GetValueOrDefault(EKey.DURATION));
				}
			}
		}
		#endregion // 함수

		#region 클래스 변수
		/** 애니메이션 여부를 변경한다 */
		public static void SetIsAni(bool a_bIsAni) {
			CLoadingSceneManager.m_oBoolDict.ExReplaceVal(EKey.IS_ANI, a_bIsAni);
		}

		/** 비동기 여부를 변경한다 */
		public static void SetIsAsync(bool a_bIsAsync) {
			CLoadingSceneManager.m_oBoolDict.ExReplaceVal(EKey.IS_ASYNC, a_bIsAsync);
		}

		/** 지속 시간을 변경한다 */
		public static void SetDuration(float a_fDuration) {
			CLoadingSceneManager.m_oRealDict.ExReplaceVal(EKey.DURATION, a_fDuration);
		}

		/** 다음 씬 이름을 변경한다 */
		public static void SetNextSceneName(string a_oSceneName) {
			CLoadingSceneManager.m_oStrDict.ExReplaceVal(EKey.NEXT_SCENE_NANE, a_oSceneName);
		}
		#endregion // 클래스 변수
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using TMPro;

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

			LOADING_TEXT,
			LOADING_GAUGE_HANDLER,

			LOADING_GAUGE,
			[HideInInspector] MAX_VAL
		}

		#region 변수
		/** =====> UI <===== */
		private Dictionary<EKey, TMP_Text> m_oTextDict = new Dictionary<EKey, TMP_Text>();
		private Dictionary<EKey, CGaugeHandler> m_oGaugeHandlerDict = new Dictionary<EKey, CGaugeHandler>();

		/** =====> 객체 <===== */
		private Dictionary<EKey, GameObject> m_oUIsDict = new Dictionary<EKey, GameObject>();
		#endregion // 변수

		#region 클래스 변수
		private static Dictionary<EKey, bool> m_oBoolDict = new Dictionary<EKey, bool>() {
			[EKey.IS_ANI] = false,
			[EKey.IS_ASYNC] = false
		};

		private static Dictionary<EKey, float> m_oRealDict = new Dictionary<EKey, float>() {
			[EKey.DURATION] = KCDefine.B_VAL_0_REAL
		};

		private static Dictionary<EKey, string> m_oStrDict = new Dictionary<EKey, string>() {
			[EKey.NEXT_SCENE_NANE] = string.Empty
		};
		#endregion // 클래스 변수

		#region 프로퍼티
		public override string SceneName => KCDefine.B_SCENE_N_LOADING;
		public virtual bool IsIgnoreLoadingGauge => true;

		public virtual Vector3 LoadingTextPos => Vector3.zero;
		public virtual Vector3 LoadingGaugePos => Vector3.zero;

#if UNITY_EDITOR
		public override int ScriptOrder => KCDefine.U_SCRIPT_O_LOADING_SCENE_MANAGER;
#endif // #if UNITY_EDITOR

		/** =====> UI <===== */
		public TMP_Text LoadingText => m_oTextDict[EKey.LOADING_TEXT];
		public CGaugeHandler LoadingGaugeHandler => m_oGaugeHandlerDict[EKey.LOADING_GAUGE_HANDLER];

		/** =====> 객체 <===== */
		public GameObject LoadingGauge => m_oUIsDict[EKey.LOADING_GAUGE];
		#endregion // 프로퍼티

		#region 클래스 프로퍼티
		public static bool IsAni => CLoadingSceneManager.m_oBoolDict[EKey.IS_ANI];
		public static bool IsAsync => CLoadingSceneManager.m_oBoolDict[EKey.IS_ASYNC];
		public static float Duration => CLoadingSceneManager.m_oRealDict[EKey.DURATION];
		public static string NextSceneName => CLoadingSceneManager.m_oStrDict[EKey.NEXT_SCENE_NANE];
		#endregion // 클래스 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				// 객체를 설정한다 {
				CFunc.SetupObjs(new List<(EKey, string, GameObject, GameObject)>() {
					(EKey.LOADING_GAUGE, $"{EKey.LOADING_GAUGE}", this.UIs, CResManager.Inst.GetRes<GameObject>(KCDefine.SS_OBJ_P_LOADING_GAUGE))
				}, m_oUIsDict);

				m_oUIsDict[EKey.LOADING_GAUGE].SetActive(!this.IsIgnoreLoadingGauge);
				m_oUIsDict[EKey.LOADING_GAUGE].transform.localPosition = this.LoadingGaugePos;
				// 객체를 설정한다 }

				// 텍스트를 설정한다 {
				CFunc.SetupComponents(new List<(EKey, string, GameObject, GameObject)>() {
					(EKey.LOADING_TEXT, $"{EKey.LOADING_TEXT}", m_oUIsDict[EKey.LOADING_GAUGE], CResManager.Inst.GetRes<GameObject>(KCDefine.SS_OBJ_P_LOADING_TEXT))
				}, m_oTextDict);

				m_oTextDict[EKey.LOADING_TEXT].transform.localPosition = this.LoadingTextPos;
				// 텍스트를 설정한다 }

				// 게이지 처리자를 설정한다
				CFunc.SetupComponents(new List<(EKey, GameObject)>() {
					(EKey.LOADING_GAUGE_HANDLER, m_oUIsDict[EKey.LOADING_GAUGE])
				}, m_oGaugeHandlerDict);
			}
		}

		/** 초기화 */
		public override void Start() {
			base.Start();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				// 비동기 모드 일 경우
				if(CLoadingSceneManager.m_oBoolDict[EKey.IS_ASYNC]) {
					CSceneLoader.Inst.LoadSceneAsync(CLoadingSceneManager.m_oStrDict[EKey.NEXT_SCENE_NANE], this.OnUpdateAsyncSceneLoadingState, KCDefine.B_VAL_0_REAL, CLoadingSceneManager.m_oBoolDict[EKey.IS_ANI], CLoadingSceneManager.m_oRealDict[EKey.DURATION]);
				} else {
					CSceneLoader.Inst.LoadScene(CLoadingSceneManager.m_oStrDict[EKey.NEXT_SCENE_NANE], CLoadingSceneManager.m_oBoolDict[EKey.IS_ANI], CLoadingSceneManager.m_oRealDict[EKey.DURATION]);
				}
			}
		}

		/** 비동기 씬 로딩 상태가 갱신 되었을 경우 */
		protected override void OnUpdateAsyncSceneLoadingState(AsyncOperation a_oAsyncOperation, bool a_bIsComplete) {
			// 완료 상태가 아닐 경우
			if(!this.IsDestroy && !a_bIsComplete) {
				m_oGaugeHandlerDict[EKey.LOADING_GAUGE_HANDLER].SetPercent(a_oAsyncOperation.progress);
			}
		}
		#endregion // 함수

		#region 클래스 변수
		/** 애니메이션 여부를 변경한다 */
		public static void SetIsAni(bool a_bIsAni) {
			CLoadingSceneManager.m_oBoolDict[EKey.IS_ANI] = a_bIsAni;
		}

		/** 비동기 여부를 변경한다 */
		public static void SetIsAsync(bool a_bIsAsync) {
			CLoadingSceneManager.m_oBoolDict[EKey.IS_ASYNC] = a_bIsAsync;
		}

		/** 지속 시간을 변경한다 */
		public static void SetDuration(float a_fDuration) {
			CLoadingSceneManager.m_oRealDict[EKey.DURATION] = a_fDuration;
		}

		/** 다음 씬 이름을 변경한다 */
		public static void SetNextSceneName(string a_oSceneName) {
			CLoadingSceneManager.m_oStrDict[EKey.NEXT_SCENE_NANE] = a_oSceneName;
		}
		#endregion // 클래스 변수
	}
}

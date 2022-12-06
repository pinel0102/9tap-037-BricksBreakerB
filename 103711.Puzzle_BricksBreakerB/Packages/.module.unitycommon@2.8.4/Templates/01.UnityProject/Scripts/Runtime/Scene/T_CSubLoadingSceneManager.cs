#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using TMPro;

namespace LoadingScene {
	/** 서브 로딩 씬 관리자 */
	public partial class CSubLoadingSceneManager : CLoadingSceneManager {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
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

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				this.SetupAwake();
			}
		}

		/** 초기화 */
		public override void Start() {
			base.Start();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				this.SetupStart();
				this.UpdateUIsState();
			}
		}

		/** 씬을 설정한다 */
		private void SetupAwake() {
			// 객체를 설정한다 {
			CFunc.SetupObjs(new List<(EKey, string, GameObject, GameObject)>() {
				(EKey.LOADING_GAUGE, $"{EKey.LOADING_GAUGE}", this.UIs, CResManager.Inst.GetRes<GameObject>(KCDefine.SS_OBJ_P_LOADING_GAUGE))
			}, m_oUIsDict);

			m_oUIsDict.GetValueOrDefault(EKey.LOADING_GAUGE).transform.localPosition = KDefine.LS_POS_LOADING_GAUGE;
			// 객체를 설정한다 }

			// 텍스트를 설정한다 {
			CFunc.SetupComponents(new List<(EKey, string, GameObject, GameObject)>() {
				(EKey.LOADING_TEXT, $"{EKey.LOADING_TEXT}", this.UIs, CResManager.Inst.GetRes<GameObject>(KCDefine.SS_OBJ_P_LOADING_TEXT))
			}, m_oTextDict);

			m_oTextDict.GetValueOrDefault(EKey.LOADING_TEXT).transform.localPosition = KDefine.LS_POS_LOADING_TEXT;
			// 텍스트를 설정한다 }

			// 게이지 처리자를 설정한다
			CFunc.SetupComponents(new List<(EKey, GameObject)>() {
				(EKey.LOADING_GAUGE_HANDLER, m_oUIsDict.GetValueOrDefault(EKey.LOADING_GAUGE))
			}, m_oGaugeHandlerDict);

			this.SubSetupAwake();
		}

		/** 씬을 설정한다 */
		private void SetupStart() {
			this.SubSetupStart();
		}

		/** UI 상태를 갱신한다 */
		private void UpdateUIsState() {
			this.SubUpdateUIsState();
		}

		/** 비동기 씬 로딩 상태가 갱신 되었을 경우 */
		protected override void OnUpdateAsyncSceneLoadingState(AsyncOperation a_oAsyncOperation, bool a_bIsComplete) {
			m_oGaugeHandlerDict.GetValueOrDefault(EKey.LOADING_GAUGE_HANDLER).SetPercent(a_oAsyncOperation.progress);
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

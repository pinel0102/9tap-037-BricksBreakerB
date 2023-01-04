#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace LoadingScene {
	/** 서브 로딩 씬 관리자 */
	public partial class CSubLoadingSceneManager : CLoadingSceneManager {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

#region 변수

#endregion // 변수

#region 프로퍼티
		public override Vector3 LoadingTextPos => KDefine.LS_POS_LOADING_TEXT;
		public override Vector3 LoadingGaugePos => KDefine.LS_POS_LOADING_GAUGE;
#endregion // 프로퍼티

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
			base.OnUpdateAsyncSceneLoadingState(a_oAsyncOperation, a_bIsComplete);
		}
#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

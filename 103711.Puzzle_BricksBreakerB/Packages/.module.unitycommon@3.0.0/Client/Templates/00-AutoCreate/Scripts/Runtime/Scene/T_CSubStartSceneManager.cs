#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCENE_TEMPLATES_MODULE_ENABLE
namespace StartScene {
	/** 서브 시작 씬 관리자 */
	public partial class CSubStartSceneManager : CStartSceneManager {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			LOADING_GAUGE_ANI,

			STR_BUILDER_01,
			STR_BUILDER_02,

			LOADING_TEXT,
			SCENE_INFO_TEXT,
			LOADING_GAUGE_HANDLER,

			LOADING_GAUGE,
			[HideInInspector] MAX_VAL
		}

#region 변수

#endregion // 변수

#region 프로퍼티
		public override bool IsIgnoreLoadingGauge => false;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		public override Vector3 LoadingTextPos => KDefine.SS_POS_LOADING_TEXT;
		public override Vector3 LoadingGaugePos => KDefine.SS_POS_LOADING_GAUGE;
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endregion // 프로퍼티

#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 초기화 되었을 경우
			if(CSceneManager.IsInit) {
				this.SetupAwake();
			}
		}

		/** 씬을 설정한다 */
		protected override void Setup() {
			base.Setup();
			this.UpdateUIsState();
		}

		/** 시작 씬 이벤트를 수신했을 경우 */
		protected override void OnReceiveStartSceneEvent(EStartSceneEvent a_eEvent) {
			base.OnReceiveStartSceneEvent(a_eEvent);
		}

		/** 씬을 설정한다 */
		private void SetupAwake() {
			// Do Something
		}

		/** 텍스트 상태를 갱신한다 */
		private void UpdateUIsState() {
			// Do Something
		}
#endregion // 함수
	}
}
#endif // #if SCENE_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

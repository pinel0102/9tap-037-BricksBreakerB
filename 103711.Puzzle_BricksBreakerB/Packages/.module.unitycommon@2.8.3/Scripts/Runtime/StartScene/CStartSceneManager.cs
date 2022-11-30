using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace StartScene {
	/** 시작 씬 관리자 */
	public abstract partial class CStartSceneManager : CSceneManager {
		#region 변수
		protected Dictionary<string, int> m_oMaxNumFXSndsDict = new Dictionary<string, int>();
		#endregion // 변수

		#region 프로퍼티
		public override string SceneName => KCDefine.B_SCENE_N_START;

#if UNITY_EDITOR
		public override int ScriptOrder => KCDefine.U_SCRIPT_O_START_SCENE_MANAGER;
#endif // #if UNITY_EDITOR
		#endregion // 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 초기화 되었을 경우
			if(CSceneManager.IsInit) {
				CCommonAppInfoStorage.Inst.IncrAppRunningTimes(KCDefine.B_VAL_1_INT);
				CCommonAppInfoStorage.Inst.SaveAppInfo();
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

		/** 씬을 설정한다 */
		protected virtual void Setup() {
			foreach(var stKeyVal in this.m_oMaxNumFXSndsDict) {
				CSndManager.Inst.SetMaxNumFXSnds(stKeyVal.Key, stKeyVal.Value);
			}
		}

		/** 시작 씬 이벤트를 수신했을 경우 */
		protected abstract void OnReceiveStartSceneEvent(EStartSceneEvent a_eEvent);

		/** 초기화 */
		private IEnumerator CoStart() {
			yield return CFactory.CoCreateWaitForSecs(KCDefine.U_DELAY_INIT);
			this.Setup();

			CSceneLoader.Inst.LoadAdditiveScene(KCDefine.B_SCENE_N_SETUP);
		}
		#endregion // 함수
	}
}

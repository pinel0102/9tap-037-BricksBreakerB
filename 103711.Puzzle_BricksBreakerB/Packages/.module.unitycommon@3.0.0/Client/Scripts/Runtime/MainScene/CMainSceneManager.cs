using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace MainScene {
	/** 메인 씬 관리자 */
	public partial class CMainSceneManager : CSceneManager {
		/** 식별자 */
		private enum EKey {
			NONE,
			VER_TEXT,
			[HideInInspector] MAX_VAL
		}

		#region 변수
		/** =====> UI <===== */
		private Dictionary<EKey, TMP_Text> m_oTextDict = new Dictionary<EKey, TMP_Text>();
		#endregion // 변수

		#region 프로퍼티
		public override bool IsIgnoreTestUIs => false;
		public override bool IsIgnoreABTestUIs => false;
		public override bool IsIgnoreOverlayScene => false;
		public override string SceneName => KCDefine.B_SCENE_N_MAIN;

		/** =====> UI <===== */
		public TMP_Text VerText => m_oTextDict.GetValueOrDefault(EKey.VER_TEXT);
		#endregion // 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				// 텍스트를 설정한다 {
				CFunc.SetupComponents(new List<(EKey, string, GameObject, GameObject)>() {
					(EKey.VER_TEXT, $"{EKey.VER_TEXT}", this.UpUIs, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_G_INFO_TEXT))
				}, m_oTextDict);

				m_oTextDict.GetValueOrDefault(EKey.VER_TEXT).rectTransform.pivot = KCDefine.B_ANCHOR_UP_LEFT;
				m_oTextDict.GetValueOrDefault(EKey.VER_TEXT).rectTransform.anchorMin = KCDefine.B_ANCHOR_UP_LEFT;
				m_oTextDict.GetValueOrDefault(EKey.VER_TEXT).rectTransform.anchorMax = KCDefine.B_ANCHOR_UP_LEFT;
				m_oTextDict.GetValueOrDefault(EKey.VER_TEXT).rectTransform.anchoredPosition = Vector3.zero;
				// 텍스트를 설정한다 }
			}
		}

		/** 초기화 */
		public override void Start() {
			base.Start();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				m_oTextDict.GetValueOrDefault(EKey.VER_TEXT)?.ExSetText(CAccess.GetVerStr(CProjInfoTable.Inst.ProjInfo.m_stBuildVerInfo.m_oVer, CCommonUserInfoStorage.Inst.UserInfo.UserType), CLocalizeInfoTable.Inst.GetFontSetInfo(EFontSet._1), false);
				m_oTextDict.GetValueOrDefault(EKey.VER_TEXT)?.transform.SetAsLastSibling();
			}
		}

		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
				// 단축키를 눌렀을 경우
				if(Input.GetKey(KeyCode.LeftShift)) {
					this.HandleHotKeys();
				}
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
			}
		}
		#endregion // 함수

		#region 조건부 함수
#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
		/** 단축키를 처리한다 */
		private void HandleHotKeys() {
#if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE
			// 에디터 키를 눌렀을 경우
			if(Input.GetKeyDown(KeyCode.E)) {
				CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_LEVEL_EDITOR);
			}
#endif // #if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE
		}
#endif // #if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
		#endregion // 조건부 함수
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif // #if UNITY_EDITOR

/** 샘플 씬 관리자 */
public partial class CSampleSceneManager : CSceneManager {
	#region 프로퍼티
	public override float ScreenWidth => CSceneManager.ActiveSceneName.Equals(KCDefine.B_SCENE_N_EDITOR_SAMPLE) ? KCDefine.B_PORTRAIT_SCREEN_WIDTH : base.ScreenWidth;
	public override float ScreenHeight => CSceneManager.ActiveSceneName.Equals(KCDefine.B_SCENE_N_EDITOR_SAMPLE) ? KCDefine.B_PORTRAIT_SCREEN_HEIGHT : base.ScreenHeight;

	public override string SceneName => KCDefine.B_SCENE_N_SAMPLE;
	#endregion // 프로퍼티

	#region 조건부 클래스 함수
#if UNITY_EDITOR
	/** 씬 관리자를 설정한다 */
	public static void SetupSceneManager(Scene a_stScene, Dictionary<string, System.Type> a_oSceneManagerTypeDict) {
		foreach(var stKeyVal in a_oSceneManagerTypeDict) {
			// 씬 관리자 타입과 동일 할 경우
			if(a_stScene.name.Equals(stKeyVal.Key)) {
				var oSceneManager = a_stScene.ExFindChild(KCDefine.U_OBJ_N_SCENE_MANAGER);

				// 씬 관리자 추가가 필요 할 경우
				if(oSceneManager != null && oSceneManager.GetComponentInChildren(stKeyVal.Value) == null) {
					EditorSceneManager.MarkSceneDirty(a_stScene);

					oSceneManager.ExRemoveComponent<CSceneManager>(false);
					oSceneManager.AddComponent(stKeyVal.Value);
				}
			}
		}
	}

	/** 내비게이션 스택 이벤트를 수신했을 경우 */
	public override void OnReceiveNavStackEvent(ENavStackEvent a_eEvent) {
		base.OnReceiveNavStackEvent(a_eEvent);

		// 백 키 눌림 이벤트 일 경우
		if(a_eEvent == ENavStackEvent.BACK_KEY_DOWN) {
#if STUDY_MODULE_ENABLE
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MENU);
#else
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_TITLE);
#endif // #if STUDY_MODULE_ENABLE
		}
	}
#endif // #if UNITY_EDITOR
	#endregion // 조건부 클래스 함수
}

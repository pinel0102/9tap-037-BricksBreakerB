using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using TMPro;

/** 씬 에디터 윈도우 */
public partial class CSceneEditorWnd : CEditorWnd<CSceneEditorWnd> {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		TARGET_OBJ_NAME,
		REPLACE_OBJ_NAME,
		TARGET_FONT_NAME,
		REPLACE_FONT_PATH,
		[HideInInspector] MAX_VAL
	}

	#region 변수
	private Dictionary<EKey, string> m_oStrDict = new Dictionary<EKey, string>() {
		[EKey.TARGET_OBJ_NAME] = string.Empty,
		[EKey.REPLACE_OBJ_NAME] = string.Empty,
		[EKey.TARGET_FONT_NAME] = string.Empty,
		[EKey.REPLACE_FONT_PATH] = string.Empty
	};
	#endregion // 변수

	#region 함수
	/** GUI 를 그린다 */
	protected override void OnDrawGUI() {
		base.OnDrawGUI();

		this.DrawFontEditorGUI();
		this.DrawObjNameEditorGUI();
	}

	/** 폰트 에디터 GUI 를 그린다 */
	private void DrawFontEditorGUI() {
		EditorGUILayout.LabelField(KCEditorDefine.B_TEXT_FONT_REPLACE, GUILayout.Width(this.WndWidth));

		m_oStrDict[EKey.TARGET_FONT_NAME] = EditorGUILayout.TextField(KCEditorDefine.B_TEXT_SEARCH, m_oStrDict[EKey.TARGET_FONT_NAME], GUILayout.Width(this.WndWidth));
		m_oStrDict[EKey.REPLACE_FONT_PATH] = EditorGUILayout.TextField(KCEditorDefine.B_TEXT_REPLACE, m_oStrDict[EKey.REPLACE_FONT_PATH], GUILayout.Width(this.WndWidth));

		// 적용 버튼을 눌렀을 경우
		if(GUILayout.Button(KCEditorDefine.B_TEXT_APPLY, GUILayout.Width(this.WndWidth)) && m_oStrDict[EKey.REPLACE_FONT_PATH].ExIsValid()) {
			var oFont = Resources.Load<Font>(m_oStrDict[EKey.REPLACE_FONT_PATH]);
			var oFontAsset = Resources.Load<TMP_FontAsset>(m_oStrDict[EKey.REPLACE_FONT_PATH]);

			var oTextList = CEditorFunc.FindComponents<Text>();
			var oTMPTextList = CEditorFunc.FindComponents<TMP_Text>();

			for(int i = 0; i < oTextList.Count; ++i) {
				EditorSceneManager.MarkSceneDirty(oTextList[i].gameObject.scene);
				oTextList[i].font = (oTextList[i].font == null || (m_oStrDict[EKey.TARGET_FONT_NAME].Length <= KCDefine.B_VAL_0_INT || m_oStrDict[EKey.TARGET_FONT_NAME].Equals(oTextList[i].font.name))) ? oFont : oTextList[i].font;
			}

			for(int i = 0; i < oTMPTextList.Count; ++i) {
				EditorSceneManager.MarkSceneDirty(oTMPTextList[i].gameObject.scene);
				oTMPTextList[i].font = (oTMPTextList[i].font == null || (m_oStrDict[EKey.TARGET_FONT_NAME].Length <= KCDefine.B_VAL_0_INT || m_oStrDict[EKey.TARGET_FONT_NAME].Equals(oTMPTextList[i].font.name))) ? oFontAsset : oTMPTextList[i].font;
			}
		}
	}

	/** 객체 이름 에디터 GUI 를 그린다 */
	private void DrawObjNameEditorGUI() {
		EditorGUILayout.Space();
		EditorGUILayout.LabelField(KCEditorDefine.B_TEXT_OBJ_NAME_REPLACE, GUILayout.Width(this.WndWidth));

		m_oStrDict[EKey.TARGET_OBJ_NAME] = EditorGUILayout.TextField(KCEditorDefine.B_TEXT_SEARCH, m_oStrDict[EKey.TARGET_OBJ_NAME], GUILayout.Width(this.WndWidth));
		m_oStrDict[EKey.REPLACE_OBJ_NAME] = EditorGUILayout.TextField(KCEditorDefine.B_TEXT_REPLACE, m_oStrDict[EKey.REPLACE_OBJ_NAME], GUILayout.Width(this.WndWidth));

		// 적용 버튼을 눌렀을 경우
		if(GUILayout.Button(KCEditorDefine.B_TEXT_APPLY, GUILayout.Width(this.WndWidth)) && (m_oStrDict[EKey.TARGET_OBJ_NAME].ExIsValid() && m_oStrDict[EKey.REPLACE_OBJ_NAME].ExIsValid())) {
			CFunc.EnumerateScenes((a_stScene) => {
				EditorSceneManager.MarkSceneDirty(a_stScene);
				this.ReplaceSceneObjsName(a_stScene, m_oStrDict[EKey.TARGET_OBJ_NAME], m_oStrDict[EKey.REPLACE_OBJ_NAME]);

				return true;
			});
		}
	}

	/** 씬 객체 이름을 변경한다 */
	private void ReplaceSceneObjsName(Scene a_stScene, string a_oTargetName, string a_oReplaceName) {
		var oObjList = a_stScene.ExGetChildren();

		for(int i = 0; i < oObjList.Count; ++i) {
			oObjList[i].name = oObjList[i].name.Replace(a_oTargetName, a_oReplaceName);
		}
	}
	#endregion // 함수

	#region 클래스 함수
	/** 씬 에디터 윈도우를 출력한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_EDITOR_WND_BASE + "Show Scene Editor Window", false, KCEditorDefine.B_SORTING_O_EDITOR_WND_MENU + 1)]
	public static void ShowSceneEditorWnd() {
		CSceneEditorWnd.ShowEditorWnd(KCEditorDefine.B_OBJ_N_SCENE_EDITOR_POPUP, KCEditorDefine.B_MIN_SIZE_EDITOR_WND);
	}
	#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR

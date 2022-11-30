using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;

/** 에디터 윈도우 */
public abstract partial class CEditorWnd<T> : EditorWindow where T : CEditorWnd<T> {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		SCROLL_VIEW_POS,
		[HideInInspector] MAX_VAL
	}

	#region 변수
	private Dictionary<EKey, Vector3> m_oVec3Dict = new Dictionary<EKey, Vector3>();
	#endregion // 변수

	#region 클래스 변수
	private static T m_tInst = null;
	#endregion // 클래스 변수

	#region 프로퍼티
	public float WndWidth => Mathf.Clamp(EditorGUIUtility.currentViewWidth, KCDefine.B_VAL_0_REAL, float.MaxValue);
	#endregion // 프로퍼티

	#region 함수
	/** 제거 되었을 경우 */
	public virtual void OnDestroy() {
		CEditorWnd<T>.m_tInst = null;
	}

	/** GUI 를 그린다 */
	public virtual void OnGUI() {
		var stScrollViewPos = EditorGUILayout.BeginScrollView(m_oVec3Dict.GetValueOrDefault(EKey.SCROLL_VIEW_POS), true, true);
		m_oVec3Dict.ExReplaceVal(EKey.SCROLL_VIEW_POS, stScrollViewPos);

		this.OnDrawGUI();
		EditorGUILayout.EndScrollView();
	}

	/** GUI 를 그린다 */
	protected virtual void OnDrawGUI() {
		// Do Something
	}
	#endregion // 함수

	#region 클래스 함수
	/** 에디터 윈도우를 출력한다 */
	public static void ShowEditorWnd(string a_oName, Vector3 a_stMinSize, bool a_bIsImmediate = true) {
		// 인스턴스가 없을 경우
		if(CEditorWnd<T>.m_tInst == null) {
			CEditorWnd<T>.m_tInst = CEditorFactory.CreateEditorWnd<T>(a_oName);
			CEditorWnd<T>.m_tInst.minSize = a_stMinSize;

			CEditorWnd<T>.m_tInst.Show(a_bIsImmediate);
		}

		CEditorWnd<T>.m_tInst.Focus();
	}
	#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR

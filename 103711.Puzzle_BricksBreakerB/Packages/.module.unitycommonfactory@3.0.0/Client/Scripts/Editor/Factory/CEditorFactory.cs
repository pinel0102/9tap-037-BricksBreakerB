using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using System.IO;
using System.Diagnostics;
using UnityEditor;

/** 에디터 기본 팩토리 */
public static class CEditorFactory {
	#region 클래스 함수
	/** 디렉토리를 생성한다 */
	public static void MakeDirectories(string a_oDirPath, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oDirPath.ExIsValid() && a_oDirPath.Contains(KCEditorDefine.B_DIR_P_ASSETS)));

		// 경로가 유효 할 경우
		if(a_oDirPath.ExIsValid() && a_oDirPath.Contains(KCEditorDefine.B_DIR_P_ASSETS)) {
			var oTokens = a_oDirPath.Split(KCDefine.B_TOKEN_SLASH);
			var oStrBuilder = new System.Text.StringBuilder();

			for(int i = 1; i < oTokens.Length; ++i) {
				oStrBuilder.AppendFormat(KCDefine.B_TEXT_FMT_2_COMBINE, (i > KCDefine.B_VAL_1_INT) ? KCDefine.B_TOKEN_SLASH : string.Empty, oTokens[i - KCDefine.B_VAL_1_INT]);
				string oDirPath = string.Format(KCDefine.B_TEXT_FMT_2_SLASH_COMBINE, oStrBuilder.ToString(), oTokens[i]);

				// 디렉토리가 없을 경우
				if(!AssetDatabase.IsValidFolder(oDirPath)) {
					AssetDatabase.CreateFolder(oStrBuilder.ToString(), oTokens[i]);
				}
			}
		}
	}

	/** 프로세스 시작 정보를 생성한다 */
	public static ProcessStartInfo MakeProcessStartInfo(string a_oFilePath, string a_oParams, bool a_bIsUseShellExecute = true) {
		var oStartInfo = new ProcessStartInfo(a_oFilePath, a_oParams);
		oStartInfo.UseShellExecute = true;

		return oStartInfo;
	}

	/** 스크립트 객체를 생성한다 */
	public static ScriptableObject CreateScriptableObj(System.Type a_oType, string a_oFilePath = KCDefine.B_TEXT_EMPTY) {
		string oFilePath = a_oFilePath.ExIsValid() ? a_oFilePath : string.Format(KCEditorDefine.B_ASSET_P_FMT_SCRIPTABLE_OBJ, a_oType.ToString());
		CEditorFactory.MakeDirectories(Path.GetDirectoryName(oFilePath).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH), false);

		return CEditorFactory.CreateAsset<ScriptableObject>(ScriptableObject.CreateInstance(a_oType), oFilePath);
	}

	/** 직렬화 객체를 생성한다 */
	public static SerializedObject CreateSerializeObj(string a_oAssetPath) {
		CAccess.Assert(a_oAssetPath.ExIsValid() && CEditorAccess.IsExistsAsset(a_oAssetPath));
		return new SerializedObject(AssetDatabase.LoadAllAssetsAtPath(a_oAssetPath)[KCDefine.B_VAL_0_INT]);
	}

	/** 프리팹 인스턴스를 생성한다 */
	public static GameObject CreatePrefabInstance(string a_oName, string a_oObjPath, GameObject a_oParent) {
		CAccess.Assert(a_oName.ExIsValid() && a_oObjPath.ExIsValid());
		return CEditorFactory.CreatePrefabInstance(a_oName, Resources.Load<GameObject>(a_oObjPath), a_oParent);
	}

	/** 프리팹 인스턴스를 생성한다 */
	public static GameObject CreatePrefabInstance(string a_oName, GameObject a_oOrigin, GameObject a_oParent) {
		CAccess.Assert(a_oOrigin != null && a_oName.ExIsValid());

		var oObj = PrefabUtility.InstantiatePrefab(a_oOrigin, a_oParent?.transform) as GameObject;
		oObj.name = a_oName;
		oObj.transform.localScale = a_oOrigin.transform.localScale;
		oObj.transform.localEulerAngles = Vector3.zero;
		oObj.transform.localPosition = Vector3.zero;

		oObj.transform.SetAsLastSibling();
		return oObj;
	}

	/** 에셋을 제거한다 */
	public static void RemoveAsset(string a_oFilePath, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oFilePath.ExIsValid());

		AssetDatabase.DeleteAsset(a_oFilePath);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh();
	}
	#endregion // 클래스 함수

	#region 제네릭 클래스 함수
	/** 에디터 윈도우를 생성한다 */
	public static T CreateEditorWnd<T>(string a_oName) where T : EditorWindow {
		CAccess.Assert(a_oName.ExIsValid());
		return EditorWindow.GetWindow<T>() ?? EditorWindow.CreateWindow<T>(a_oName);
	}

	/** 에셋을 생성한다 */
	public static T CreateAsset<T>(T a_tAsset, string a_oFilePath, bool a_bIsFocus = true) where T : Object {
		CAccess.Assert(a_tAsset != null && a_oFilePath.ExIsValid());

		// 에셋 생성이 가능 할 경우
		if(AssetDatabase.LoadAssetAtPath<T>(a_oFilePath) == null) {
			AssetDatabase.CreateAsset(a_tAsset, a_oFilePath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();

			// 포커스 모드 일 경우
			if(a_bIsFocus) {
				Selection.activeObject = a_tAsset;
				EditorUtility.FocusProjectWindow();
			}
		}

		return a_tAsset;
	}

	/** 스크립트 객체를 생성한다 */
	public static T CreateScriptableObj<T>(string a_oFilePath = KCDefine.B_TEXT_EMPTY) where T : ScriptableObject {
		return CEditorFactory.CreateScriptableObj(typeof(T), a_oFilePath) as T;
	}
	#endregion // 제네릭 클래스 함수
}
#endif // #if UNITY_EDITOR

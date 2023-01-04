using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;

/** 씬 임포터 */
[InitializeOnLoad]
public static class CSceneImporter {
	#region 클래스 함수
	/** 클래스 생성자 */
	static CSceneImporter() {
		// 플레이 모드가 아닐 경우
		if(!EditorApplication.isPlaying) {
			EditorApplication.projectChanged -= CSceneImporter.ImportAllScenes;
			EditorApplication.projectChanged += CSceneImporter.ImportAllScenes;
		}
	}

	/** 모든 씬을 추가한다 */
	public static void ImportAllScenes() {
		var oSearchPathList = new List<string>(KCEditorDefine.B_SEARCH_P_SCENE_LIST);
		var oEditorBuildSettingsSceneList = new List<EditorBuildSettingsScene>();

		// 프로젝트 정보 테이블이 존재 할 경우
		if(CPlatformOptsSetter.ProjInfoTable != null) {
			string oDirName = string.Format(KCDefine.B_PATH_FMT_1_COMBINE, CPlatformOptsSetter.ProjInfoTable.CommonProjInfo.m_oExtraProjDirName);
			oSearchPathList.ExAddVal(string.Format(KCDefine.B_TEXT_FMT_3_COMBINE, KCEditorDefine.B_DIR_P_ASSETS, oDirName, KCEditorDefine.B_DIR_N_SCENES));
		}

		for(int i = 0; i < oSearchPathList.Count; ++i) {
			// 디렉토리가 존재 할 경우
			if(AssetDatabase.IsValidFolder(oSearchPathList[i])) {
				var oAssetGUIDs = AssetDatabase.FindAssets(KCEditorDefine.B_SCENE_N_PATTERN, new string[] {
					oSearchPathList[i]
				});

				for(int j = 0; j < oAssetGUIDs.Length; ++j) {
					string oFilePath = AssetDatabase.GUIDToAssetPath(oAssetGUIDs[j]);

					// 씬 파일 일 경우
					if(Path.GetExtension(oFilePath).Equals(KCDefine.B_FILE_EXTENSION_UNITY)) {
						oEditorBuildSettingsSceneList.ExAddVal(new EditorBuildSettingsScene(oFilePath, true));
					}
				}
			}
		}

		EditorBuildSettings.scenes = oEditorBuildSettingsSceneList.ToArray();
	}
	#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR

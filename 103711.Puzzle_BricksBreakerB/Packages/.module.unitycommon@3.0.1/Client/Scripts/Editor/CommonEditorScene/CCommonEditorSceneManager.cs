using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;

#if EDITOR_COROUTINE_ENABLE
using Unity.EditorCoroutines.Editor;
#endif // #if EDITOR_COROUTINE_ENABLE

/** 공용 에디터 씬 관리자 */
[InitializeOnLoad]
public static partial class CCommonEditorSceneManager {
	#region 클래스 변수
	private static bool m_bIsEnableSetup = false;
	private static bool m_bIsEnableBuild = false;

	private static double m_dblUpdateSkipTime = 0.0;
	private static System.Text.StringBuilder m_oStrBuilder = new System.Text.StringBuilder();

	private static GUIStyle m_oTextGUIStyle = new GUIStyle() {
		alignment = TextAnchor.MiddleRight,
		fontStyle = FontStyle.Bold
	};

	private static GUIStyle m_oOutlineGUIStyle = new GUIStyle() {
		alignment = TextAnchor.MiddleRight,
		fontStyle = FontStyle.Bold
	};

	private static List<string> m_oSampleSceneNameList = new List<string>();
	private static List<GameObject> m_oPrefabMissingObjList = new List<GameObject>();

	private static Dictionary<string, string> m_oSortingLayerDict = new Dictionary<string, string>() {
		[KCDefine.U_SORTING_L_ABS] = "A",
		[KCDefine.U_SORTING_L_DEF] = "D",

		[KCDefine.U_SORTING_L_TOP] = "T",
		[KCDefine.U_SORTING_L_TOPMOST] = "TM",

		[KCDefine.U_SORTING_L_FOREGROUND] = "F",
		[KCDefine.U_SORTING_L_BACKGROUND] = "B",

		[KCDefine.U_SORTING_L_OVERGROUND] = "O",
		[KCDefine.U_SORTING_L_UNDERGROUND] = "U",

		[KCDefine.U_SORTING_L_OVERLAY_ABS] = "OA",
		[KCDefine.U_SORTING_L_OVERLAY_DEF] = "OD",

		[KCDefine.U_SORTING_L_OVERLAY_TOP] = "OT",
		[KCDefine.U_SORTING_L_OVERLAY_TOPMOST] = "OTM",

		[KCDefine.U_SORTING_L_OVERLAY_FOREGROUND] = "OF",
		[KCDefine.U_SORTING_L_OVERLAY_BACKGROUND] = "OB",

		[KCDefine.U_SORTING_L_OVERLAY_OVERGROUND] = "OO",
		[KCDefine.U_SORTING_L_OVERLAY_UNDERGROUND] = "OU",

        [KCDefine.U_SORTING_L_AIMING] = "AM",
        [KCDefine.U_SORTING_L_CELL] = "CE",
        [KCDefine.U_SORTING_L_BALL] = "BA",
        [KCDefine.U_SORTING_L_EFFECT] = "EF",
	};
	#endregion // 클래스 변수

	#region 클래스 함수
	/** 생성자 */
	static CCommonEditorSceneManager() {
		// 플레이 모드가 아닐 경우
		if(!EditorApplication.isPlaying) {
			CCommonEditorSceneManager.m_oTextGUIStyle.normal = new GUIStyleState() {
				textColor = KCEditorDefine.B_COLOR_HIERARCHY_TEXT
			};

			CCommonEditorSceneManager.m_oOutlineGUIStyle.normal = new GUIStyleState() {
				textColor = KCEditorDefine.B_COLOR_HIERARCHY_OUTLINE
			};

			CCommonEditorSceneManager.m_dblUpdateSkipTime = EditorApplication.timeSinceStartup;

			CCommonEditorSceneManager.m_oSampleSceneNameList.ExAddVal(KCDefine.B_SCENE_N_SAMPLE);
			CCommonEditorSceneManager.m_oSampleSceneNameList.ExAddVal(KCDefine.B_SCENE_N_MENU_SAMPLE);
			CCommonEditorSceneManager.m_oSampleSceneNameList.ExAddVal(KCDefine.B_SCENE_N_STUDY_SAMPLE);
			CCommonEditorSceneManager.m_oSampleSceneNameList.ExAddVal(KCDefine.B_SCENE_N_EDITOR_SAMPLE);
		}

		CCommonEditorSceneManager.SetupCallbacks();
	}

	/** 스크립트가 로드 되었을 경우 */
	[UnityEditor.Callbacks.DidReloadScripts]
	public static void OnLoadScript() {
#if EDITOR_COROUTINE_ENABLE
		EditorCoroutineUtility.StartCoroutineOwnerless(CCommonEditorSceneManager.CoSetupEditorSceneManager());
#else
		CCommonEditorSceneManager.m_bIsEnableSetup = true;
		CCommonEditorSceneManager.m_bIsEnableBuild = true;
#endif // #if EDITOR_COROUTINE_ENABLE
	}

	/** 상태를 갱신한다 */
	private static void Update() {
		// 상태 갱신이 가능 할 경우
		if(CEditorAccess.IsEnableUpdateState) {
			// 설정 가능 할 경우
			if(CCommonEditorSceneManager.m_bIsEnableSetup) {
				CCommonEditorSceneManager.m_bIsEnableSetup = false;
				CSceneImporter.ImportAllScenes();

				CPlatformOptsSetter.SetupPlayerOpts();
				CPlatformOptsSetter.SetupEditorOpts();
				CPlatformOptsSetter.SetupProjOpts();
				CPlatformOptsSetter.SetupPluginProjs();
			}

			// 갱신 주기가 지났을 경우
			if((EditorApplication.timeSinceStartup - CCommonEditorSceneManager.m_dblUpdateSkipTime).ExIsGreateEquals(KCDefine.B_VAL_1_REAL)) {
				CCommonEditorSceneManager.m_dblUpdateSkipTime = EditorApplication.timeSinceStartup;
				CFunc.EnumerateComponents<CSceneManager>((a_oSceneManager) => { a_oSceneManager.EditorSetupScene(); return true; });

				CCommonEditorSceneManager.SetupTags();
				CCommonEditorSceneManager.SetupRaycasters();
				//CCommonEditorSceneManager.SetupLightOpts();
				CCommonEditorSceneManager.SetupLocalizeInfos();

#if ADAPTIVE_PERFORMANCE_ENABLE
				CCommonEditorSceneManager.SetupAdaptivePerformance();
#endif // #if ADAPTIVE_PERFORMANCE_ENABLE

#if LOCALIZE_MODULE_ENABLE
				CCommonEditorSceneManager.SetupLocalize();
#endif // #if LOCALIZE_MODULE_ENABLE

#if ML_AGENTS_MODULE_ENABLE
				CCommonEditorSceneManager.SetupMLAgents();
#endif // #if ML_AGENTS_MODULE_ENABLE

#if INPUT_SYSTEM_MODULE_ENABLE
				CCommonEditorSceneManager.SetupInputSystem();
#endif // #if INPUT_SYSTEM_MODULE_ENABLE

#if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE
				CCommonEditorSceneManager.SetupRenderingPipeline();
#endif // #if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE

#if BURST_COMPILER_MODULE_ENABLE && NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
				CCommonEditorSceneManager.SetupBurstCompiler();
#endif // #if BURST_COMPILER_MODULE_ENABLE && NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE

				CFunc.EnumerateRootObjs((a_oObj) => {
					// 최상단 UI 일 경우
					if(a_oObj.name.Equals(KCDefine.U_OBJ_N_SCENE_UIS_ROOT)) {
						CCommonEditorSceneManager.SetupLayers(a_oObj);
					}

					// 최상단 객체 일 경우
					if(KCEditorDefine.B_OBJ_N_ROOT_OBJ_LIST.Contains(a_oObj.name)) {
						CCommonEditorSceneManager.SetupStaticObjs(a_oObj);
					}

					CCommonEditorSceneManager.UpdateMissingScriptState(a_oObj);
					return true;
				});

				// 유니티 패키지 정보를 설정한다 {
				CCommonEditorSceneManager.m_oStrBuilder.Clear();

				CFunc.EnumerateDirectories(KCEditorDefine.B_ABS_DIR_P_UNITY_PACKAGES, (a_oDirList, a_oFileList) => {
					for(int i = 0; i < a_oFileList.Count; ++i) {
						// DS Store 파일이 아닐 경우
						if(!a_oFileList[i].EndsWith(KCDefine.B_FILE_EXTENSION_DS_STORE)) {
							string oDirPath = Path.GetRelativePath(KCEditorDefine.B_ABS_DIR_P_UNITY_PACKAGES, a_oFileList[i]);
							CCommonEditorSceneManager.m_oStrBuilder.AppendLine(oDirPath.Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH));
						}
					}

					return true;
				});
				// 유니티 패키지 정보를 설정한다 }

				// 디렉토리가 존재 할 경우
				if(Directory.Exists(KCEditorDefine.B_ABS_DIR_P_UNITY_PACKAGES)) {
					string oDirPath = Path.GetDirectoryName(KCEditorDefine.B_ABS_DIR_P_UNITY_PACKAGES).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH);
					CFunc.WriteStr(string.Format(KCDefine.B_TEXT_FMT_2_COMBINE, oDirPath, KCDefine.B_FILE_EXTENSION_TXT), CCommonEditorSceneManager.m_oStrBuilder.ToString(), false);
				}

				// 빌드 가능 할 경우
				if(CCommonEditorSceneManager.m_bIsEnableBuild) {
					string oBuildMethod = CFunc.ReadStr(KCEditorDefine.B_DATA_P_BUILD_METHOD, false);
					CCommonEditorSceneManager.m_bIsEnableBuild = false;

					// 빌드 메서드가 존재 할 경우
					if(oBuildMethod.ExIsValid()) {
						typeof(CPlatformBuilder).GetMethod(oBuildMethod, KCDefine.B_BINDING_F_PUBLIC_STATIC)?.Invoke(null, null);
					} else {
						CFunc.RemoveFile(KCEditorDefine.B_DATA_P_BUILD_METHOD);
					}
				}
			}
		}
	}

	/** 계층 뷰 UI 상태를 갱신한다 */
	private static void UpdateHierarchyUIState(int a_nInstanceID, Rect a_stRect) {
		var oObj = EditorUtility.InstanceIDToObject(a_nInstanceID) as GameObject;

		// 객체가 존재 할 경우
		if(oObj != null) {
			var oComponents = oObj.GetComponents<Component>();

			for(int i = 0; i < oComponents.Length; ++i) {
				// 컴포넌트가 존재 할 경우
				if(oComponents[i] != null) {
					var oSortingLayerProperty = oComponents[i].GetType().GetProperty(KCEditorDefine.B_PROPERTY_N_SORTING_LAYER, KCDefine.B_BINDING_F_PUBLIC_INSTANCE);
					var oSortingOrderProperty = oComponents[i].GetType().GetProperty(KCEditorDefine.B_PROPERTY_N_SORTING_ORDER, KCDefine.B_BINDING_F_PUBLIC_INSTANCE);

					string oSortingLayer = (string)oSortingLayerProperty?.GetValue(oComponents[i]);
					oSortingLayer = oSortingLayer.ExIsValid() ? CCommonEditorSceneManager.m_oSortingLayerDict.GetValueOrDefault(oSortingLayer, string.Empty) : string.Empty;

					// 프로퍼티가 존재 할 경우
					if(oSortingOrderProperty != null && oSortingLayer.ExIsValid()) {
						a_stRect.position += new Vector2((a_stRect.size.x + KCEditorDefine.B_OFFSET_HIERARCHY_TEXT) * -1.0f, KCDefine.B_VAL_0_REAL);
						string oStr = string.Format(KCEditorDefine.B_SORTING_OI_FMT, oSortingLayer, oSortingOrderProperty.GetValue(oComponents[i]));

						var oRectList = new List<Rect>() {
							new Rect(a_stRect.x + KCDefine.B_VAL_1_REAL, a_stRect.y, a_stRect.width, a_stRect.height),
							new Rect(a_stRect.x - KCDefine.B_VAL_1_REAL, a_stRect.y, a_stRect.width, a_stRect.height),
							new Rect(a_stRect.x, a_stRect.y + KCDefine.B_VAL_1_REAL, a_stRect.width, a_stRect.height),
							new Rect(a_stRect.x, a_stRect.y - KCDefine.B_VAL_1_REAL, a_stRect.width, a_stRect.height)
						};

						for(int j = 0; j < oRectList.Count; ++j) {
							GUI.Label(oRectList[j], oStr, CCommonEditorSceneManager.m_oOutlineGUIStyle);
						}

						GUI.Label(a_stRect, oStr, CCommonEditorSceneManager.m_oTextGUIStyle);
					}
				}
			}
		}
	}

	/** 플레이 모드 상태가 갱신 되었을 경우 */
	private static void OnUpdatePlayModeState(PlayModeStateChange a_ePlayMode) {
		// 에디터 모드 일 경우
		if(a_ePlayMode == PlayModeStateChange.EnteredEditMode) {
#if EDITOR_COROUTINE_ENABLE
			EditorCoroutineUtility.StartCoroutineOwnerless(CCommonEditorSceneManager.CoUpdateEditorModeState());
#else
			Time.timeScale = KCDefine.B_VAL_1_REAL;
#endif // #if EDITOR_COROUTINE_ENABLE
		}
	}

	/** 프로젝트 상태가 갱신 되었을 경우 */
	private static void OnUpdateProjectState() {
		CCommonEditorSceneManager.SetupPreloadAssets();
		CCommonEditorSceneManager.SetupSpriteAtlases();
		CCommonEditorSceneManager.SetupSceneTemplates();
	}

	/** 에디터 모드 상태를 갱신한다 */
	private static IEnumerator CoUpdateEditorModeState() {
		yield return CFactory.CoCreateWaitForSecs(KCDefine.B_DELTA_T_ASYNC_TASK, true);
		Time.timeScale = KCDefine.B_VAL_1_REAL;
	}
	#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

/** 에디터 기본 접근 */
[InitializeOnLoad]
public static partial class CEditorAccess {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		IS_IMPORT_ASSETS,
		[HideInInspector] MAX_VAL
	}

	#region 클래스 변수
	private static Dictionary<EKey, bool> m_oBoolDict = new Dictionary<EKey, bool>() {
		[EKey.IS_IMPORT_ASSETS] = false
	};
	#endregion // 클래스 변수

	#region 클래스 프로퍼티
	public static bool IsEnableUpdateState => !BuildPipeline.isBuildingPlayer && !EditorApplication.isUpdating && !EditorApplication.isCompiling && !EditorApplication.isPlayingOrWillChangePlaymode && !CEditorAccess.m_oBoolDict[EKey.IS_IMPORT_ASSETS];
	#endregion // 클래스 프로퍼티

	#region 클래스 함수
	/** 생성자 */
	static CEditorAccess() {
		AssetDatabase.importPackageStarted -= CEditorAccess.OnStartAssetImport;
		AssetDatabase.importPackageStarted += CEditorAccess.OnStartAssetImport;

		AssetDatabase.importPackageCompleted -= CEditorAccess.OnCompleteAssetImport;
		AssetDatabase.importPackageCompleted += CEditorAccess.OnCompleteAssetImport;

		AssetDatabase.importPackageCancelled -= CEditorAccess.OnCompleteAssetImport;
		AssetDatabase.importPackageCancelled += CEditorAccess.OnCompleteAssetImport;

		AssetDatabase.importPackageFailed -= CEditorAccess.OnFailAssetImport;
		AssetDatabase.importPackageFailed += CEditorAccess.OnFailAssetImport;
	}

	/** 에셋 존재 여부를 검사한다 */
	public static bool IsExistsAsset(string a_oFilePath) {
		return AssetDatabase.GetMainAssetTypeAtPath(a_oFilePath) != null;
	}

	/** 전처리기 심볼 포함 여부를 검사한다 */
	public static bool IsContainsDefineSymbol(BuildTargetGroup a_eTargetGroup, string a_oSymbol) {
		PlayerSettings.GetScriptingDefineSymbolsForGroup(a_eTargetGroup, out string[] oDefineSymbols);
		return oDefineSymbols.ExIsValid() && oDefineSymbols.ExIsContains(a_oSymbol);
	}

	/** 최상단 객체 프리팹 경로를 반환한다 */
	public static string GetRootObjPrefabPath(Scene a_stScene, string a_oName) {
		string oDirPath = (!a_stScene.name.Contains(KCEditorDefine.B_EDITOR_SCENE_N_PATTERN_01) && !a_stScene.name.Contains(KCEditorDefine.B_EDITOR_SCENE_N_PATTERN_02)) ? KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_PREFABS : KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR_PREFABS;
		string oPrefabPath = string.Format(KCDefine.B_TEXT_FMT_3_SLASH_COMBINE, Path.GetDirectoryName(oDirPath).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH), CEditorAccess.GetSceneDirName(a_stScene), string.Format(KCDefine.B_TEXT_FMT_2_UNDER_SCORE_COMBINE, a_stScene.name, a_oName));

		return string.Format(KCDefine.B_TEXT_FMT_2_COMBINE, oPrefabPath, KCDefine.B_FILE_EXTENSION_PREFAB);
	}

	/** 활성 객체를 반환한다 */
	public static GameObject GetActiveObj(bool a_bIsInHierarchy = true) {
		return (a_bIsInHierarchy && (Selection.activeGameObject != null && !Selection.activeGameObject.activeInHierarchy)) ? null : Selection.activeGameObject;
	}

	/** 씬 디렉토리 이름을 반환한다 */
	public static string GetSceneDirName(Scene a_stScene) {
		return KCEditorDefine.B_DIR_N_SCENE_DICT.GetValueOrDefault(a_stScene.name, (!a_stScene.name.Contains(KCEditorDefine.B_EDITOR_SCENE_N_PATTERN_01) && !a_stScene.name.Contains(KCEditorDefine.B_EDITOR_SCENE_N_PATTERN_02)) ? Path.GetDirectoryName(KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH) : Path.GetDirectoryName(KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH));
	}

	/** iOS 프로젝트 이름을 반환한다 */
	public static string GetiOSProjName(EiOSType a_eType) {
		return KCEditorDefine.B_IOS_APPLE_BUILD_PROJ_N_JENKINS;
	}

	/** iOS 빌드 결과 경로를 반환한다 */
	public static string GetiOSBuildOutputPath(EiOSType a_eType, string a_oBuildFileExtension) {
		return string.Format(KCEditorDefine.B_BUILD_OUTPUT_P_FMT_IOS, CAccess.GetiOSName(a_eType), a_oBuildFileExtension);
	}

	/** 안드로이드 프로젝트 이름을 반환한다 */
	public static string GetAndroidProjName(EAndroidType a_eType) {
		switch(a_eType) {
			case EAndroidType.AMAZON: return KCEditorDefine.B_ANDROID_AMAZON_BUILD_PROJ_N_JENKINS;
		}

		return KCEditorDefine.B_ANDROID_GOOGLE_BUILD_PROJ_N_JENKINS;
	}

	/** 안드로이드 빌드 결과 경로를 반환한다 */
	public static string GetAndroidBuildOutputPath(EAndroidType a_eType, string a_oBuildFileExtension) {
		return string.Format(KCEditorDefine.B_BUILD_OUTPUT_P_FMT_ANDROID, CAccess.GetAndroidName(a_eType), a_oBuildFileExtension);
	}

	/** 독립 플랫폼 프로젝트 이름을 반환한다 */
	public static string GetStandaloneProjName(EStandaloneType a_eType) {
		switch(a_eType) {
			case EStandaloneType.WNDS_STEAM:
				return KCEditorDefine.B_STANDALONE_WNDS_STEAM_BUILD_PROJ_N_JENKINS;
		}

		return KCEditorDefine.B_STANDALONE_MAC_STEAM_BUILD_PROJ_N_JENKINS;
	}

	/** 독립 플랫폼 빌드 결과 경로를 반환한다 */
	public static string GetStandaloneBuildOutputPath(EStandaloneType a_eType, string a_oBuildFileExtension) {
		return string.Format(KCEditorDefine.B_BUILD_OUTPUT_P_FMT_STANDALONE, CAccess.GetStandaloneName(a_eType), a_oBuildFileExtension);
	}

	/** 그래픽 API 를 변경한다 */
	public static void SetGraphicAPI(BuildTarget a_eTarget, List<GraphicsDeviceType> a_oDeviceTypeList, bool a_bIsEnableAuto = true, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oDeviceTypeList.ExIsValid());

		// 디바이스 타입이 존재할 경우
		if(a_oDeviceTypeList.ExIsValid()) {
			PlayerSettings.SetGraphicsAPIs(a_eTarget, a_oDeviceTypeList.ToArray());
			PlayerSettings.SetUseDefaultGraphicsAPIs(a_eTarget, a_bIsEnableAuto);
		}
	}

	/** 에셋 임포트가 시작했을 경우 */
	private static void OnStartAssetImport(string a_oAssetName) {
		CEditorAccess.m_oBoolDict[EKey.IS_IMPORT_ASSETS] = true;
	}

	/** 에셋 임포트가 완료 되었을 경우 */
	private static void OnCompleteAssetImport(string a_oAssetName) {
		CEditorAccess.m_oBoolDict[EKey.IS_IMPORT_ASSETS] = false;
	}

	/** 에셋 임포트가 실패했을 경우 */
	private static void OnFailAssetImport(string a_oAssetName, string a_oErrorMsg) {
		CEditorAccess.m_oBoolDict[EKey.IS_IMPORT_ASSETS] = false;
	}
	#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR

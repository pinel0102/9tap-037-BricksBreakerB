using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;

/** 플랫폼 옵션 설정자 */
[InitializeOnLoad]
public static partial class CPlatformOptsSetter {
	#region 클래스 변수
	private static COptsInfoTable m_oOptsInfoTable = null;
	private static CBuildInfoTable m_oBuildInfoTable = null;
	private static CProjInfoTable m_oProjInfoTable = null;
	private static CLocalizeInfoTable m_oLocalizeInfoTable = null;
	private static CDefineSymbolInfoTable m_oDefineSymbolInfoTable = null;
	#endregion // 클래스 변수

	#region 클래스 프로퍼티
	public static Dictionary<BuildTargetGroup, List<string>> DefineSymbolDictContainer { get; private set; } = new Dictionary<BuildTargetGroup, List<string>>();

	public static COptsInfoTable OptsInfoTable {
		get {
			// 옵션 정보 테이블이 없을 경우
			if(CPlatformOptsSetter.m_oOptsInfoTable == null) {
				CPlatformOptsSetter.m_oOptsInfoTable = CEditorFunc.FindAsset<COptsInfoTable>(KCEditorDefine.B_ASSET_P_OPTS_INFO_TABLE);
				CPlatformOptsSetter.m_oOptsInfoTable?.Awake();
			}

			return CPlatformOptsSetter.m_oOptsInfoTable;
		}
	}

	public static CBuildInfoTable BuildInfoTable {
		get {
			// 빌드 정보 테이블이 없을 경우
			if(CPlatformOptsSetter.m_oBuildInfoTable == null) {
				CPlatformOptsSetter.m_oBuildInfoTable = CEditorFunc.FindAsset<CBuildInfoTable>(KCEditorDefine.B_ASSET_P_BUILD_INFO_TABLE);
				CPlatformOptsSetter.m_oBuildInfoTable?.Awake();
			}

			return CPlatformOptsSetter.m_oBuildInfoTable;
		}
	}

	public static CProjInfoTable ProjInfoTable {
		get {
			// 프로젝트 정보 테이블이 없을 경우
			if(CPlatformOptsSetter.m_oProjInfoTable == null) {
				CPlatformOptsSetter.m_oProjInfoTable = CEditorFunc.FindAsset<CProjInfoTable>(KCEditorDefine.B_ASSET_P_PROJ_INFO_TABLE);
				CPlatformOptsSetter.m_oProjInfoTable?.Awake();
			}

			return CPlatformOptsSetter.m_oProjInfoTable;
		}
	}

	public static CLocalizeInfoTable LocalizeInfoTable {
		get {
			// 지역화 정보 테이블이 없을 경우
			if(CPlatformOptsSetter.m_oLocalizeInfoTable == null) {
				CPlatformOptsSetter.m_oLocalizeInfoTable = CEditorFunc.FindAsset<CLocalizeInfoTable>(KCEditorDefine.B_ASSET_P_LOCALIZE_INFO_TABLE);
				CPlatformOptsSetter.m_oLocalizeInfoTable?.Awake();
			}

			return CPlatformOptsSetter.m_oLocalizeInfoTable;
		}
	}

	public static CDefineSymbolInfoTable DefineSymbolInfoTable {
		get {
			// 전처리기 심볼 정보 테이블이 없을 경우
			if(CPlatformOptsSetter.m_oDefineSymbolInfoTable == null) {
				CPlatformOptsSetter.m_oDefineSymbolInfoTable = CEditorFunc.FindAsset<CDefineSymbolInfoTable>(KCEditorDefine.B_ASSET_P_DEFINE_SYMBOL_INFO_TABLE);
				CPlatformOptsSetter.m_oDefineSymbolInfoTable?.Awake();
			}

			return CPlatformOptsSetter.m_oDefineSymbolInfoTable;
		}
	}
	#endregion // 클래스 프로퍼티

	#region 클래스 함수
	/** 생성자 */
	static CPlatformOptsSetter() {
		// 플레이 모드가 아닐 경우
		if(!EditorApplication.isPlaying) {
			CPlatformOptsSetter.EditorInitialize();
		}
	}

	/** 초기화 */
	public static void EditorInitialize() {
		// 전처리기 심볼 정보 테이블이 존재 할 경우
		if(CPlatformOptsSetter.DefineSymbolInfoTable != null) {
			var oExtraDefineSymbolInfoList01 = new List<(string, string)>() {
				(KCEditorDefine.DS_DEFINE_S_SAMPLE_PROJ, KCEditorDefine.DS_DEFINE_S_AB_TEST_ENABLE),
				(KCEditorDefine.DS_DEFINE_S_DOTWEEN_ENABLE, KCEditorDefine.DS_DEFINE_S_DOTWEEN_MODULE_ENABLE),
				(KCEditorDefine.DS_DEFINE_S_EXTRA_SCRIPT_ENABLE, KCEditorDefine.DS_DEFINE_S_EXTRA_SCRIPT_MODULE_ENABLE),
				(KCEditorDefine.DS_DEFINE_S_NEWTON_SOFT_JSON_ENABLE, KCEditorDefine.DS_DEFINE_S_NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE),
				(KCEditorDefine.DS_DEFINE_S_INPUT_SYSTEM_ENABLE, KCEditorDefine.DS_DEFINE_S_INPUT_SYSTEM_MODULE_ENABLE),
				(KCEditorDefine.DS_DEFINE_S_INPUT_SYSTEM_MODULE_ENABLE, KCEditorDefine.DS_DEFINE_S_ENABLE_INPUT_SYSTEM),
				(KCEditorDefine.DS_DEFINE_S_EXTRA_SCRIPT_MODULE_ENABLE, KCEditorDefine.DS_DEFINE_S_ADAPTIVE_PERFORMANCE_ENABLE),
				(KCEditorDefine.DS_DEFINE_S_EXTRA_SCRIPT_MODULE_ENABLE, KCEditorDefine.DS_DEFINE_S_EDITOR_COROUTINE_ENABLE),
				(KCEditorDefine.DS_DEFINE_S_PLAYFAB_MODULE_ENABLE, KCEditorDefine.DS_DEFINE_S_PLAYFAB_SERVER_API_ENABLE)
			};

			CPlatformOptsSetter.DefineSymbolDictContainer = CPlatformOptsSetter.DefineSymbolDictContainer ?? new Dictionary<BuildTargetGroup, List<string>>();
			CPlatformOptsSetter.DefineSymbolDictContainer.Clear();
			CPlatformOptsSetter.DefineSymbolDictContainer.TryAdd(BuildTargetGroup.iOS, CPlatformOptsSetter.DefineSymbolInfoTable.iOSDefineSymbolList);
			CPlatformOptsSetter.DefineSymbolDictContainer.TryAdd(BuildTargetGroup.Android, CPlatformOptsSetter.DefineSymbolInfoTable.AndroidDefineSymbolList);
			CPlatformOptsSetter.DefineSymbolDictContainer.TryAdd(BuildTargetGroup.Standalone, CPlatformOptsSetter.DefineSymbolInfoTable.StandaloneDefineSymbolList);

			// 배치 모드 일 경우
			if(Application.isBatchMode) {
				switch(CPlatformBuilder.iOSType) {
					default: CPlatformOptsSetter.DefineSymbolDictContainer[BuildTargetGroup.iOS].ExAddVals(CPlatformOptsSetter.DefineSymbolInfoTable.EditoriOSAppleDefineSymbolList); break;
				}

				switch(CPlatformBuilder.AndroidType) {
					case EAndroidType.AMAZON: CPlatformOptsSetter.DefineSymbolDictContainer[BuildTargetGroup.Android].ExAddVals(CPlatformOptsSetter.DefineSymbolInfoTable.EditorAndroidAmazonDefineSymbolList); break;
					default: CPlatformOptsSetter.DefineSymbolDictContainer[BuildTargetGroup.Android].ExAddVals(CPlatformOptsSetter.DefineSymbolInfoTable.EditorAndroidGoogleDefineSymbolList); break;
				}

				switch(CPlatformBuilder.StandaloneType) {
					case EStandaloneType.WNDS_STEAM: CPlatformOptsSetter.DefineSymbolDictContainer[BuildTargetGroup.Standalone].ExAddVals(CPlatformOptsSetter.DefineSymbolInfoTable.EditorStandaloneWndsSteamDefineSymbolList); break;
					default: CPlatformOptsSetter.DefineSymbolDictContainer[BuildTargetGroup.Standalone].ExAddVals(CPlatformOptsSetter.DefineSymbolInfoTable.EditorStandaloneMacSteamDefineSymbolList); break;
				}
			}

			foreach(var stKeyVal in CPlatformOptsSetter.DefineSymbolDictContainer) {
				stKeyVal.Value.ExAddVal(KCEditorDefine.DS_DEFINE_S_IL2CPP_ENABLE);
				stKeyVal.Value.ExAddVal(KCEditorDefine.DS_DEFINE_S_ODIN_INSPECTOR);
				stKeyVal.Value.ExAddVal(KCEditorDefine.DS_DEFINE_S_DOTWEEN_ENABLE);
				stKeyVal.Value.ExAddVal(KCEditorDefine.DS_DEFINE_S_MSG_PACK_ENABLE);
				stKeyVal.Value.ExAddVal(KCEditorDefine.DS_DEFINE_S_ODIN_INSPECTOR_3);
				stKeyVal.Value.ExAddVal(KCEditorDefine.DS_DEFINE_S_ODIN_INSPECTOR_3_1);
				stKeyVal.Value.ExAddVal(KCEditorDefine.DS_DEFINE_S_EXTRA_SCRIPT_ENABLE);
				stKeyVal.Value.ExAddVal(KCEditorDefine.DS_DEFINE_S_INPUT_SYSTEM_ENABLE);
				stKeyVal.Value.ExAddVal(KCEditorDefine.DS_DEFINE_S_RECEIPT_CHECK_ENABLE);
				stKeyVal.Value.ExAddVal(KCEditorDefine.DS_DEFINE_S_NEWTON_SOFT_JSON_ENABLE);
				stKeyVal.Value.ExAddVal(KCEditorDefine.DS_DEFINE_S_UNITY_POST_PROCESSING_STACK_V2);

				for(int i = 0; i < oExtraDefineSymbolInfoList01.Count; ++i) {
					// 추가 전처리기 심볼이 존재 할 경우
					if(CEditorAccess.IsContainsDefineSymbol(stKeyVal.Key, oExtraDefineSymbolInfoList01[i].Item1)) {
						stKeyVal.Value.ExAddVal(oExtraDefineSymbolInfoList01[i].Item2);
					}
				}

				// 전처리기 심볼 추가가 가능 할 경우
				if(stKeyVal.Value.Contains(KCEditorDefine.DS_DEFINE_S_SAMPLE_PROJ) || (stKeyVal.Value.Contains(KCEditorDefine.DS_DEFINE_S_STUDY_ENABLE) || stKeyVal.Value.Contains(KCEditorDefine.DS_DEFINE_S_STUDY_MODULE_ENABLE))) {
					var oExtraDefineSymbolInfoList02 = new List<(string, string)>() {
						(KCEditorDefine.DS_DEFINE_S_ML_AGENTS_ENABLE, KCEditorDefine.DS_DEFINE_S_ML_AGENTS_MODULE_ENABLE),
						(KCEditorDefine.DS_DEFINE_S_CINEMACHINE_ENABLE, KCEditorDefine.DS_DEFINE_S_CINEMACHINE_MODULE_ENABLE),
						(KCEditorDefine.DS_DEFINE_S_POST_PROCESSING_ENABLE, KCEditorDefine.DS_DEFINE_S_POST_PROCESSING_MODULE_ENABLE),
						(KCEditorDefine.DS_DEFINE_S_SKELETON_2D_ANI_ENABLE, KCEditorDefine.DS_DEFINE_S_SKELETON_2D_ANI_MODULE_ENABLE),
						(KCEditorDefine.DS_DEFINE_S_UNIVERSAL_RENDERING_PIPELINE_ENABLE, KCEditorDefine.DS_DEFINE_S_UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE)
					};

					var oExtraDefineSymbolInfoList03 = new List<(string, string)>() {
						(KCEditorDefine.DS_DEFINE_S_NOTI_ENABLE, KCEditorDefine.DS_DEFINE_S_NOTI_MODULE_ENABLE)
					};

					for(int i = 0; i < oExtraDefineSymbolInfoList02.Count; ++i) {
						// 추가 전처리기 심볼이 존재 할 경우
						if(CEditorAccess.IsContainsDefineSymbol(stKeyVal.Key, oExtraDefineSymbolInfoList02[i].Item1)) {
							stKeyVal.Value.ExAddVal(oExtraDefineSymbolInfoList02[i].Item2);
						}

						stKeyVal.Value.ExAddVal(oExtraDefineSymbolInfoList02[i].Item1);
					}

					// 샘플 프로젝트 심볼이 존재 할 경우
					if(stKeyVal.Value.Contains(KCEditorDefine.DS_DEFINE_S_SAMPLE_PROJ)) {
						for(int i = 0; i < oExtraDefineSymbolInfoList03.Count; ++i) {
							// 추가 전처리기 심볼이 존재 할 경우
							if(CEditorAccess.IsContainsDefineSymbol(stKeyVal.Key, oExtraDefineSymbolInfoList03[i].Item1)) {
								stKeyVal.Value.ExAddVal(oExtraDefineSymbolInfoList03[i].Item2);
							}

							stKeyVal.Value.ExAddVal(oExtraDefineSymbolInfoList03[i].Item1);
						}
					}

					stKeyVal.Value.ExAddVal(KCEditorDefine.DS_DEFINE_S_LIGHT_ENABLE);
					stKeyVal.Value.ExAddVal(KCEditorDefine.DS_DEFINE_S_CAMERA_STACKING_ENABLE);
					stKeyVal.Value.ExAddVal(KCEditorDefine.DS_DEFINE_S_PHYSICS_RAYCASTER_ENABLE);
				}
			}
		}
	}

	/** 전처리기 심볼 포함 여부를 검사한다 */
	public static bool IsContainsDefineSymbol(BuildTargetGroup a_eTargetGroup, string a_oDefineSymbol) {
		return CPlatformOptsSetter.DefineSymbolDictContainer.TryGetValue(a_eTargetGroup, out List<string> oDefineSymbolList) && oDefineSymbolList.Contains(a_oDefineSymbol);
	}

	/** 전처리기 심볼을 추가한다 */
	public static void AddDefineSymbol(BuildTargetGroup a_eTargetGroup, string a_oDefineSymbol) {
		// 전처리기 심볼이 유효하지 않을 경우
		if(!CPlatformOptsSetter.DefineSymbolDictContainer.ExIsValid()) {
			CPlatformOptsSetter.EditorInitialize();
		}

		// 전처리기 심볼 추가가 가능 할 경우
		if(CPlatformOptsSetter.DefineSymbolDictContainer.TryGetValue(a_eTargetGroup, out List<string> oDefineSymbolList)) {
			oDefineSymbolList.ExAddVal(a_oDefineSymbol);
		}
	}

	/** 전처리기 심볼을 추가한다 */
	public static void AddDefineSymbols(BuildTargetGroup a_eTargetGroup, List<string> a_oDefineSymbolList) {
		for(int i = 0; i < a_oDefineSymbolList.Count; ++i) {
			CPlatformOptsSetter.AddDefineSymbol(a_eTargetGroup, a_oDefineSymbolList[i]);
		}
	}

	/** 전처리기 심볼을 제거한다 */
	public static void RemoveDefineSymbol(BuildTargetGroup a_eTargetGroup, string a_oDefineSymbol) {
		// 전처리기 심볼 제거가 가능 할 경우
		if(CPlatformOptsSetter.DefineSymbolDictContainer.TryGetValue(a_eTargetGroup, out List<string> oDefineSymbolList)) {
			oDefineSymbolList.ExRemoveVal(a_oDefineSymbol);
		}
	}

	/** 전처리기 심볼을 제거한다 */
	public static void RemoveDefineSymbols(BuildTargetGroup a_eTargetGroup, List<string> a_oDefineSymbolList) {
		for(int i = 0; i < a_oDefineSymbolList.Count; ++i) {
			CPlatformOptsSetter.RemoveDefineSymbol(a_eTargetGroup, a_oDefineSymbolList[i]);
		}
	}
	#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR

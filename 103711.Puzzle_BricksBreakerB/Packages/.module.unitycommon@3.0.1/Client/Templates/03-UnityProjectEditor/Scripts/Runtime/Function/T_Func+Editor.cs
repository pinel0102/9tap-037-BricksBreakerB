#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
/** 에디터 씬 함수 */
public static partial class Func {
#region 클래스 함수

#endregion // 클래스 함수

#region 조건부 클래스 함수
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
	/** 에디터 입력 팝업을 출력한다 */
	public static void ShowEditorInputPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CEditorInputPopup>(KCDefine.ES_OBJ_N_EDITOR_INPUT_POPUP, KCDefine.ES_OBJ_P_EDITOR_INPUT_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 에디터 생성 팝업을 출력한다 */
	public static void ShowEditorCreatePopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CEditorCreatePopup>(KCDefine.ES_OBJ_N_EDITOR_CREATE_POPUP, KCDefine.ES_OBJ_P_EDITOR_CREATE_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endregion // 조건부 클래스 함수
}

/** 레벨 에디터 씬 함수 */
public static partial class Func {
#region 클래스 함수

#endregion // 클래스 함수

#region 조건부 클래스 함수
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
	/** 에디터 레벨 정보를 설정한다 */
	public static void SetupEditorLevelInfo(CLevelInfo a_oLevelInfo, CEditorCreateInfo a_oCreateInfo, bool a_bIsReset = true) {
		int nNumCellsX = Random.Range(a_oCreateInfo.m_stMinNumCells.x, a_oCreateInfo.m_stMaxNumCells.x + KCDefine.B_VAL_1_INT);
		int nNumCellsY = Random.Range(a_oCreateInfo.m_stMinNumCells.y, a_oCreateInfo.m_stMaxNumCells.y + KCDefine.B_VAL_1_INT);

		var oCloneLevelInfo = a_bIsReset ? null : (CLevelInfo)a_oLevelInfo.Clone();
		a_oLevelInfo.m_oCellInfoDictContainer.Clear();

		for(int i = 0; i < Mathf.Clamp(nNumCellsY, NSEngine.KDefine.E_MIN_NUM_CELLS.y, NSEngine.KDefine.E_MAX_NUM_CELLS.y); ++i) {
			var oCellInfoDict = new Dictionary<int, STCellInfo>();

			for(int j = 0; j < Mathf.Clamp(nNumCellsX, NSEngine.KDefine.E_MIN_NUM_CELLS.x, NSEngine.KDefine.E_MAX_NUM_CELLS.x); ++j) {
				oCellInfoDict.TryAdd(j, Factory.MakeEditorCellInfo(new Vector3Int(j, i, KCDefine.B_VAL_0_INT)));
			}

			a_oLevelInfo.m_oCellInfoDictContainer.TryAdd(i, oCellInfoDict);
		}

		a_oLevelInfo.OnAfterDeserialize();
		Func.SetupEditorCellInfos(a_oLevelInfo, oCloneLevelInfo, a_oCreateInfo);
	}

	/** 에디터 셀 정보를 설정한다 */
	private static void SetupEditorCellInfos(CLevelInfo a_oLevelInfo, CLevelInfo a_oCloneLevelInfo, CEditorCreateInfo a_oCreateInfo) {
		int nTryTimes = KCDefine.B_VAL_0_INT;

		do {
			var oIdxVDictContainer = CCollectionManager.Inst.SpawnDict<int, List<Vector3Int>>();
			var oIdxHDictContainer = CCollectionManager.Inst.SpawnDict<int, List<Vector3Int>>();

			try {
				for(int i = 0; i < a_oLevelInfo.m_oCellInfoDictContainer.Count; ++i) {
					for(int j = 0; j < a_oLevelInfo.m_oCellInfoDictContainer[i].Count; ++j) {
						var oIdxVList = oIdxVDictContainer.GetValueOrDefault(j) ?? new List<Vector3Int>();
						var oIdxHList = oIdxHDictContainer.GetValueOrDefault(i) ?? new List<Vector3Int>();

						oIdxVDictContainer.TryAdd(j, oIdxVList);
						oIdxHDictContainer.TryAdd(i, oIdxHList);

						oIdxVList.Add(a_oLevelInfo.m_oCellInfoDictContainer[i][j].m_stIdx);
						oIdxHList.Add(a_oLevelInfo.m_oCellInfoDictContainer[i][j].m_stIdx);

						a_oLevelInfo.m_oCellInfoDictContainer[i][j].m_oCellObjInfoList.Clear();
					}
				}

				// 사본 레벨 정보가 존재 할 경우
				if(a_oCloneLevelInfo != null) {
					for(int i = a_oLevelInfo.m_oCellInfoDictContainer.Count - KCDefine.B_VAL_1_INT; i >= KCDefine.B_VAL_0_INT; --i) {
						for(int j = 0; j < a_oLevelInfo.m_oCellInfoDictContainer[i].Count; ++j) {
							int nIdxX = j;
							int nIdxY = a_oCloneLevelInfo.m_oCellInfoDictContainer.Count - (((a_oLevelInfo.m_oCellInfoDictContainer.Count - KCDefine.B_VAL_1_INT) - i) + KCDefine.B_VAL_1_INT);

							// 셀 정보가 존재 할 경우
							if(a_oCloneLevelInfo.m_oCellInfoDictContainer.TryGetValue(nIdxY, out Dictionary<int, STCellInfo> oCellInfoDict) && oCellInfoDict.TryGetValue(nIdxX, out STCellInfo stCellInfo)) {
								a_oLevelInfo.m_oCellInfoDictContainer[i][j] = (STCellInfo)stCellInfo.Clone();
							}
						}
					}
				}

				oIdxVDictContainer.ExShuffle();
				oIdxHDictContainer.ExShuffle();

				Func.SetupEditorCellInfos(a_oLevelInfo, a_oCreateInfo, oIdxVDictContainer, oIdxHDictContainer);
			} finally {
				CCollectionManager.Inst.DespawnDict(oIdxVDictContainer);
				CCollectionManager.Inst.DespawnDict(oIdxHDictContainer);
			}
		} while(nTryTimes++ < KDefine.LES_MAX_TIMES_TRY_SETUP_CELL_INFOS && !Func.IsSetupEditorCellInfos(a_oLevelInfo, a_oCreateInfo));

		a_oLevelInfo.OnAfterDeserialize();
	}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endregion // 조건부 클래스 함수
}
#endif // #if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
#endif // #if SCRIPT_TEMPLATE_ONLY

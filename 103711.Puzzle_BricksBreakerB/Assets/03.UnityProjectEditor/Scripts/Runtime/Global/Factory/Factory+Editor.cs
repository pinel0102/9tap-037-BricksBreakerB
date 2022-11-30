using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
/** 에디터 팩토리 */
public static partial class Factory {
#region 클래스 함수
	
#endregion // 클래스 함수

#region 조건부 클래스 함수
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
	/** 셀 정보를 생성한다 */
	public static STCellInfo MakeCellInfo(Vector3Int a_stIdx) {
		var stCellInfo = new STCellInfo() {
			m_stIdx = a_stIdx, m_oObjKindsDictContainer = new Dictionary<EObjType, List<EObjKinds>>()
		};

		stCellInfo.OnAfterDeserialize();
		return stCellInfo;
	}

	/** 레벨 정보를 생성한다 */
	public static CLevelInfo MakeLevelInfo(int a_nLevelID, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		var stLevelInfo = new CLevelInfo() {
			m_stIDInfo = new STIDInfo(a_nLevelID, a_nStageID, a_nChapterID)
		};

		stLevelInfo.OnAfterDeserialize();
		return stLevelInfo;
	}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endregion // 조건부 클래스 함수
}

/** 레벨 에디터 씬 팩토리 */
public static partial class Factory {
#region 클래스 함수

#endregion // 클래스 함수
}
#endif // #if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)

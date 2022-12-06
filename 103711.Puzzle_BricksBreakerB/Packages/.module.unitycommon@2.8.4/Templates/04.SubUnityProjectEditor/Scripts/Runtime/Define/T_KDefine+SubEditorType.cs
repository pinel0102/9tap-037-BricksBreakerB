#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
using MessagePack;

#region 기본
/** 서브 에디터 레벨 생성 정보 */
public partial class CSubEditorLevelCreateInfo : CEditorLevelCreateInfo {
	// Do Something
}

/** 서브 에디터 타입 랩퍼 */
[MessagePackObject]
public struct STSubEditorTypeWrapper {
	// Do Something
}
#endregion // 기본
#endif // #if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
#endif // #if SCRIPT_TEMPLATE_ONLY

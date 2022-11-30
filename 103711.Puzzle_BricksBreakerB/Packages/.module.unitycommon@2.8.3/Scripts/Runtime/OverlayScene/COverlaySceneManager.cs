using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace OverlayScene {
	/** 중첩 씬 관리자 */
	public partial class COverlaySceneManager : CSceneManager {
		#region 프로퍼티
		public override string SceneName => KCDefine.B_SCENE_N_OVERLAY;

#if UNITY_EDITOR
		public override int ScriptOrder => KCDefine.U_SCRIPT_O_OVERLAY_SCENE_MANAGER;
#endif // #if UNITY_EDITOR
		#endregion // 프로퍼티
	}
}

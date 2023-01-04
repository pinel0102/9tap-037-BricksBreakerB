using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
namespace LevelEditorScene {
	/** 레벨 에디터 씬 관리자 */
	public partial class CLevelEditorSceneManager : CSceneManager {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			MID_EDITOR_UIS,
			LEFT_EDITOR_UIS,
			RIGHT_EDITOR_UIS,

			ME_UIS_MSG_UIS,
			LE_UIS_AB_SET_UIS,

			OBJ_ROOT,
			MASK_OBJ_ROOT,
			[HideInInspector] MAX_VAL
		}

		#region 변수
		/** =====> 객체 <===== */
		private Dictionary<EKey, GameObject> m_oUIsDict = new Dictionary<EKey, GameObject>();
		private Dictionary<EKey, GameObject> m_oObjDict = new Dictionary<EKey, GameObject>();
		#endregion // 변수

		#region 프로퍼티
		public override bool IsIgnoreBlindH => true;
		public override bool IsIgnoreBlindV => true;
		public override bool IsIgnoreBGTouchResponder => false;

		public override float ScreenWidth => KCDefine.B_PORTRAIT_SCREEN_WIDTH;
		public override float ScreenHeight => KCDefine.B_PORTRAIT_SCREEN_HEIGHT;

		public override string SceneName => KCDefine.B_SCENE_N_LEVEL_EDITOR;
		public override Vector3 ObjRootPivotPos => (this.UIsBase != null && this.UIsBase.ExFindChild(KCDefine.U_OBJ_N_SCENE_MID_EDITOR_UIS) != null) ? Vector3.zero.ExToWorld(this.UIsBase.ExFindChild(KCDefine.U_OBJ_N_SCENE_MID_EDITOR_UIS)).ExToLocal(this.UIs) : Vector3.zero;

		/** =====> 객체 <===== */
		protected GameObject MidEditorUIs => m_oUIsDict.GetValueOrDefault(EKey.MID_EDITOR_UIS);
		protected GameObject LeftEditorUIs => m_oUIsDict.GetValueOrDefault(EKey.LEFT_EDITOR_UIS);
		protected GameObject RightEditorUIs => m_oUIsDict.GetValueOrDefault(EKey.RIGHT_EDITOR_UIS);

		protected GameObject MEUIsMsgUIs => m_oUIsDict.GetValueOrDefault(EKey.ME_UIS_MSG_UIS);
		protected GameObject LEUIsABSetUIs => m_oUIsDict.GetValueOrDefault(EKey.LE_UIS_AB_SET_UIS);

		protected GameObject ObjRoot => m_oObjDict.GetValueOrDefault(EKey.OBJ_ROOT);
		protected GameObject MaskObjRoot => m_oObjDict.GetValueOrDefault(EKey.MASK_OBJ_ROOT);
		#endregion // 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				// 객체를 설정한다 {
				CFunc.SetupObjs(new List<(EKey, string, GameObject)>() {
					(EKey.MID_EDITOR_UIS, $"{EKey.MID_EDITOR_UIS}", this.UIsBase),
					(EKey.LEFT_EDITOR_UIS, $"{EKey.LEFT_EDITOR_UIS}", this.UIsBase),
					(EKey.RIGHT_EDITOR_UIS, $"{EKey.RIGHT_EDITOR_UIS}", this.UIsBase)
				}, m_oUIsDict);

				CFunc.SetupObjs(new List<(EKey, string, GameObject)>() {
					(EKey.ME_UIS_MSG_UIS, $"{EKey.ME_UIS_MSG_UIS}", m_oUIsDict.GetValueOrDefault(EKey.MID_EDITOR_UIS)),
					(EKey.LE_UIS_AB_SET_UIS, $"{EKey.LE_UIS_AB_SET_UIS}", m_oUIsDict.GetValueOrDefault(EKey.LEFT_EDITOR_UIS))
				}, m_oUIsDict);

				CFunc.SetupObjs(new List<(EKey, string, GameObject, GameObject)>() {
					(EKey.MASK_OBJ_ROOT, $"{EKey.MASK_OBJ_ROOT}", this.Objs, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_SPRITE)),
				}, m_oObjDict);

				CFunc.SetupObjs(new List<(EKey, string, GameObject, GameObject)>() {
					(EKey.OBJ_ROOT, $"{EKey.OBJ_ROOT}", m_oObjDict.GetValueOrDefault(EKey.MASK_OBJ_ROOT), null)
				}, m_oObjDict);

				CSceneManager.ScreenDebugUIs?.SetActive(false);
				m_oUIsDict.GetValueOrDefault(EKey.ME_UIS_MSG_UIS)?.SetActive(false);

				m_oObjDict.GetValueOrDefault(EKey.MASK_OBJ_ROOT)?.ExAddComponent<SpriteMask>();
				m_oObjDict.GetValueOrDefault(EKey.MASK_OBJ_ROOT)?.ExAddComponent<SpriteRenderer>();
				// 객체를 설정한다 }
			}
		}
		#endregion // 함수
	}
}
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)

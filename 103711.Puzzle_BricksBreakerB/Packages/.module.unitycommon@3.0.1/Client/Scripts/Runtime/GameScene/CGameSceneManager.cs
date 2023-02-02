using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GameScene {
	/** 게임 씬 관리자 */
	public partial class CGameSceneManager : CSceneManager {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
            CELL_ROOT,
			ITEM_ROOT,
			SKILL_ROOT,
			OBJ_ROOT,
			FX_ROOT,
			[HideInInspector] MAX_VAL
		}

		#region 변수
		/** =====> 객체 <===== */
		private Dictionary<EKey, GameObject> m_oObjDict = new Dictionary<EKey, GameObject>();
		#endregion // 변수

		#region 프로퍼티
		public override bool IsIgnoreTestUIs => false;
		public override bool IsIgnoreOverlayScene => false;
		public override bool IsIgnoreBGTouchResponder => false;

		public override string SceneName => KCDefine.B_SCENE_N_GAME;

		/** =====> 객체 <===== */
        protected GameObject CellRoot => m_oObjDict[EKey.CELL_ROOT];
		protected GameObject ItemRoot => m_oObjDict[EKey.ITEM_ROOT];
		protected GameObject SkillRoot => m_oObjDict[EKey.SKILL_ROOT];
		protected GameObject ObjRoot => m_oObjDict[EKey.OBJ_ROOT];
		protected GameObject FXRoot => m_oObjDict[EKey.FX_ROOT];
		#endregion // 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				// 객체를 설정한다
				CFunc.SetupObjs(new List<(EKey, string, GameObject, GameObject)>() {
                    (EKey.CELL_ROOT, $"{EKey.CELL_ROOT}", this.Objs, null),
					(EKey.ITEM_ROOT, $"{EKey.ITEM_ROOT}", this.Objs, null),
					(EKey.SKILL_ROOT, $"{EKey.SKILL_ROOT}", this.Objs, null),
					(EKey.OBJ_ROOT, $"{EKey.OBJ_ROOT}", this.Objs, null),
					(EKey.FX_ROOT, $"{EKey.FX_ROOT}", this.Objs, null)
				}, m_oObjDict);
			}
		}
		#endregion // 함수
	}
}

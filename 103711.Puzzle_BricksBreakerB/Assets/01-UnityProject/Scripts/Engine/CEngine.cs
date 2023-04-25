using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using UnityEngine.EventSystems;

namespace NSEngine {
	/** 엔진 */
	public partial class CEngine : CComponent {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			IS_RUNNING,
			IS_SAVE_USER_INFO,
			SEL_GRID_IDX,
			[HideInInspector] MAX_VAL
		}

		/** 콜백 */
		public enum ECallback {
			NONE = -1,
			CLEAR,
			CLEAR_FAIL,
			ACQUIRE,
			[HideInInspector] MAX_VAL
		}

		/** 매개 변수 */
		public struct STParams {
			public GameObject m_oCellRoot;
            public GameObject m_oAimRoot;
            public GameObject m_oItemRoot;
			public GameObject m_oSkillRoot;
			public GameObject m_oObjRoot;
            public GameObject m_oWallRoot;
			public GameObject m_oFXRoot;

			public Dictionary<ECallback, System.Action<CEngine>> m_oCallbackDict01;
			public Dictionary<ECallback, System.Action<CEngine, Dictionary<ulong, STTargetInfo>>> m_oCallbackDict02;
		}

		#region 변수
		private Dictionary<EKey, int> m_oIntDict = new Dictionary<EKey, int>();
		private Dictionary<EKey, bool> m_oBoolDict = new Dictionary<EKey, bool>();

		private List<STGridInfo> m_oGridInfoList = new List<STGridInfo>();
		private List<LineRenderer> m_oGridLineFXList = new List<LineRenderer>();
		private Dictionary<ulong, STTargetInfo> m_oClearTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
		#endregion // 변수

		#region 프로퍼티
		public STParams Params { get; private set; }
		public STRecordInfo RecordInfo { get; private set; }
		public List<CEObj>[,] CellObjLists { get; private set; } = null;

		public List<CEItem> ItemList { get; } = new List<CEItem>();
		public List<CESkill> SkillList { get; } = new List<CESkill>();
		public List<CEObj> ObjList { get; } = new List<CEObj>();
		public List<CEFX> FXList { get; } = new List<CEFX>();

		public bool IsRunning => m_oBoolDict.GetValueOrDefault(EKey.IS_RUNNING);
		public int SelGridInfoIdx => m_oIntDict.GetValueOrDefault(EKey.SEL_GRID_IDX);
		public Vector3 EpisodeSize => new Vector3(Mathf.Max(CSceneManager.ActiveSceneManager.ScreenWidth, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.x), Mathf.Max(CSceneManager.ActiveSceneManager.ScreenHeight, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.y), CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.z);
		public Vector3 CameraEpisodeSize => new Vector3(Mathf.Max(CSceneManager.ActiveSceneManager.ScreenWidth, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.x - CSceneManager.ActiveSceneManager.ScreenWidth), Mathf.Max(CSceneManager.ActiveSceneManager.ScreenHeight, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.y - CSceneManager.ActiveSceneManager.ScreenHeight), CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.z);
		public STGridInfo SelGridInfo => m_oGridInfoList[this.SelGridInfoIdx];
		#endregion // 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();
			this.SubAwake();
		}

		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			this.Params = a_stParams;

            this.isShooting = false;
            this.isLevelClear = false;
            this.isLevelFail = false;
            this.isAddSteelBricks = false;
            this.isExplosionAll = false;

			this.SetupEngine();
			this.SetupLevel();
			this.SetupGridLine();

			this.SubInit();
		}

		/** 상태를 리셋한다 */
		public override void Reset() {
			base.Reset();
			m_oBoolDict.ExReplaceVal(EKey.IS_RUNNING, false);

			this.SubReset();
		}

		/** 구동 여부를 변경한다 */
		public void SetEnableRunning(bool a_bIsRunning) {
			m_oBoolDict.ExReplaceVal(EKey.IS_RUNNING, a_bIsRunning);
		}

		/** 터치 이벤트를 처리한다 */
		public void HandleTouchEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData, ETouchEvent a_eTouchEvent) {
			var stTouchPos = a_oEventData.ExGetLocalPos(this.Params.m_oObjRoot, CSceneManager.ActiveSceneManager.ScreenSize);

            // 그리드가 존재 할 경우
			if(m_oGridInfoList.ExIsValidIdx(this.SelGridInfoIdx)) {
				switch(a_eTouchEvent) {
					case ETouchEvent.BEGIN: this.HandleTouchBeginEvent(a_oSender, a_oEventData); break;
					case ETouchEvent.MOVE: this.HandleTouchMoveEvent(a_oSender, a_oEventData); break;
					case ETouchEvent.END: this.HandleTouchEndEvent(a_oSender, a_oEventData); break;
				}
			}
		}
		#endregion // 함수

		#region 클래스 함수
		/** 매개 변수를 생성한다 */
		public static STParams MakeParams(GameObject a_oCellRoot, GameObject a_oAimRoot, GameObject a_oItemRoot, GameObject a_oSkillRoot, GameObject a_oObjRoot, GameObject a_oWallRoot, GameObject a_oFXRoot, Dictionary<ECallback, System.Action<CEngine>> a_oCallbackDict01 = null, Dictionary<ECallback, System.Action<CEngine, Dictionary<ulong, STTargetInfo>>> a_oCallbackDict02 = null) {
			return new STParams() {
                m_oCellRoot = a_oCellRoot,
                m_oAimRoot = a_oAimRoot,
				m_oItemRoot = a_oItemRoot,
				m_oSkillRoot = a_oSkillRoot,
				m_oObjRoot = a_oObjRoot,
				m_oWallRoot = a_oWallRoot,
                m_oFXRoot = a_oFXRoot,
				m_oCallbackDict01 = a_oCallbackDict01 ?? new Dictionary<ECallback, System.Action<CEngine>>(),
				m_oCallbackDict02 = a_oCallbackDict02 ?? new Dictionary<ECallback, System.Action<CEngine, Dictionary<ulong, STTargetInfo>>>()
			};
		}
		#endregion // 클래스 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

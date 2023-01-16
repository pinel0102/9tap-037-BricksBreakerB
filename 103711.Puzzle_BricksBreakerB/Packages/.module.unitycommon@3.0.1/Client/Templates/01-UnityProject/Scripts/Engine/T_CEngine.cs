#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.Linq;
using UnityEngine.EventSystems;

namespace NSEngine {
	/** 엔진 */
	public partial class CEngine : CComponent {
		///** 식별자 */
		private enum EKey {
			NONE = -1,
			IS_RUNNING,
			IS_SAVE_USER_INFO,
			SEL_GRID_IDX,
			SEL_PLAYER_OBJ_IDX,
			[HideInInspector] MAX_VAL
		}

		/** 상태 */
		public enum EState {
			NONE = -1,
			PLAY,
			PAUSE,
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
			public GameObject m_oItemRoot;
			public GameObject m_oSkillRoot;
			public GameObject m_oObjRoot;
			public GameObject m_oFXRoot;

			public Dictionary<ECallback, System.Action<CEngine>> m_oCallbackDict01;
			public Dictionary<ECallback, System.Action<CEngine, Dictionary<ulong, STTargetInfo>>> m_oCallbackDict02;
		}

		#region 변수
		private Dictionary<EKey, int> m_oIntDict = new Dictionary<EKey, int>() {
			[EKey.SEL_GRID_IDX] = KCDefine.B_VAL_0_INT,
			[EKey.SEL_PLAYER_OBJ_IDX] = KCDefine.B_VAL_0_INT
		};

		private Dictionary<EKey, bool> m_oBoolDict = new Dictionary<EKey, bool>() {
			[EKey.IS_RUNNING] = false,
			[EKey.IS_SAVE_USER_INFO] = false
		};

		private List<STGridInfo> m_oGridInfoList = new List<STGridInfo>();
		private Dictionary<ulong, STTargetInfo> m_oClearTargetInfoDict = new Dictionary<ulong, STTargetInfo>();
		private Dictionary<EState, System.Func<bool>> m_oStateCheckerDict = new Dictionary<EState, System.Func<bool>>();
		#endregion // 변수

		#region 프로퍼티
		public STParams Params { get; private set; }
		public STRecordInfo RecordInfo { get; private set; }

		public EState State { get; private set; } = EState.NONE;
		public List<CEObj>[,] CellObjLists { get; private set; } = null;

		public List<CEItem> ItemList { get; } = new List<CEItem>();
		public List<CESkill> SkillList { get; } = new List<CESkill>();
		public List<CEObj> ObjList { get; } = new List<CEObj>();
		public List<CEFX> FXList { get; } = new List<CEFX>();

		public List<CEObj> PlayerObjList { get; } = new List<CEObj>();
		public List<CEObj> EnemyObjList { get; } = new List<CEObj>();

		public bool IsRunning => m_oBoolDict[EKey.IS_RUNNING];

		public int SelGridInfoIdx => m_oIntDict[EKey.SEL_GRID_IDX];
		public int SelPlayerObjIdx => m_oIntDict[EKey.SEL_PLAYER_OBJ_IDX];

		public Vector3 EpisodeSize => new Vector3(Mathf.Max(CSceneManager.ActiveSceneManager.ScreenWidth, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.x), Mathf.Max(CSceneManager.ActiveSceneManager.ScreenHeight, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.y), CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.z);
		public Vector3 CameraEpisodeSize => new Vector3(Mathf.Max(CSceneManager.ActiveSceneManager.ScreenWidth, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.x - CSceneManager.ActiveSceneManager.ScreenWidth), Mathf.Max(CSceneManager.ActiveSceneManager.ScreenHeight, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.y - CSceneManager.ActiveSceneManager.ScreenHeight), CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.z);
		public STGridInfo SelGridInfo => m_oGridInfoList[this.SelGridInfoIdx];

		public CEObj SelPlayerObj => this.PlayerObjList[this.SelPlayerObjIdx];
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

			this.Setup();
			this.SetupLevel();
			this.SetupGridLine();

#if NEVER_USE_THIS
			// FIXME: dante (비활성 처리 - 필요 시 활성 및 사용 가능) {
			var stObjInfo = CObjInfoTable.Inst.GetObjInfo(EObjKinds.PLAYABLE_COMMON_CHARACTER_01);
			this.PlayerObjList.ExAddVal(this.CreatePlayerObj(stObjInfo, CUserInfoStorage.Inst.GetCharacterUserInfo(CGameInfoStorage.Inst.PlayCharacterID), null));

			CSceneManager.ActiveSceneMainCamera.transform.position = new Vector3(this.SelPlayerObj.transform.position.x, this.SelPlayerObj.transform.position.y + (KDefine.E_OFFSET_MAIN_CAMERA * CAccess.ResolutionUnitScale), CSceneManager.ActiveSceneMainCamera.transform.position.z);
			// FIXME: dante (비활성 처리 - 필요 시 활성 및 사용 가능) }
#endif // #if NEVER_USE_THIS

			this.SubInit();
		}

		/** 상태를 리셋한다 */
		public override void Reset() {
			base.Reset();
			this.SetState(EState.NONE, true);

			m_oBoolDict[EKey.IS_RUNNING] = false;
		}

		/** 제거 되었을 경우 */
		public override void OnDestroy() {
			base.OnDestroy();

			try {
				// 앱이 실행 중 일 경우
				if(CSceneManager.IsAppRunning) {
					this.SubOnDestroy();
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CEngine.OnDestroy Exception: {oException.Message}");
			}
		}

		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				// 실행 중 일 경우
				if(m_oBoolDict[EKey.IS_RUNNING]) {
					switch(this.State) {
						case EState.PLAY: this.HandlePlayState(a_fDeltaTime); break;
						case EState.PAUSE: this.HandlePauseState(a_fDeltaTime); break;
					}

					// 플레이어 객체가 존재 할 경우
					if(this.PlayerObjList.ExIsValid()) {
						var stMainCameraPos = this.GetMainCameraPos();
						CSceneManager.ActiveSceneMainCamera.transform.position = Vector3.Lerp(CSceneManager.ActiveSceneMainCamera.transform.position, stMainCameraPos.ExToWorld(this.Params.m_oObjRoot), a_fDeltaTime * KCDefine.B_VAL_9_REAL);
					}
				}

				// 유저 정보 저장이 필요 할 경우
				if(m_oBoolDict[EKey.IS_SAVE_USER_INFO]) {
					CUserInfoStorage.Inst.SaveUserInfo();
					m_oBoolDict[EKey.IS_SAVE_USER_INFO] = false;
				}

				this.SubOnUpdate(a_fDeltaTime);
			}
		}

		/** 플레이어 객체 이동을 처리한다 */
		public void MovePlayerObj(Vector3 a_stVal, EVecType a_eVecType = EVecType.DIRECTION) {
			this.SelPlayerObj.GetController<CEPlayerObjController>().Move(a_stVal, a_eVecType);
		}

		/** 플레이어 객체 스킬을 적용한다 */
		public void ApplyPlayerObjSkill(CSkillTargetInfo a_oSkillTargetInfo) {
			var stSkillInfo = CSkillInfoTable.Inst.GetSkillInfo(a_oSkillTargetInfo.SkillKinds);
			this.SelPlayerObj.GetController<CEPlayerObjController>().ApplySkill(stSkillInfo, a_oSkillTargetInfo);
		}

		/** 터치 이벤트를 처리한다 */
		public void HandleTouchEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData, ETouchEvent a_eTouchEvent) {
			var stPos = a_oEventData.ExGetLocalPos(this.Params.m_oObjRoot, CSceneManager.ActiveSceneManager.ScreenSize);

			// 그리드 영역 일 경우
			if(m_oGridInfoList.ExIsValidIdx(this.SelGridInfoIdx) && this.SelGridInfo.m_stViewBounds.Contains(stPos)) {
				switch(a_eTouchEvent) {
					case ETouchEvent.BEGIN: this.HandleTouchBeginEvent(a_oSender, a_oEventData); break;
					case ETouchEvent.MOVE: this.HandleTouchMoveEvent(a_oSender, a_oEventData); break;
					case ETouchEvent.END: this.HandleTouchEndEvent(a_oSender, a_oEventData); break;
				}
			}
		}

		/** 엔진을 설정한다 */
		private void Setup() {
			// 그리드 정보를 설정한다 {
			m_oGridInfoList.Clear();

			for(int i = 0; i < KCDefine.B_VAL_1_INT; ++i) {
				switch(CGameInfoStorage.Inst.PlayLevelInfo.GridPivot) {
					case EGridPivot.DOWN: {
						var stGridInfo = Factory.MakeGridInfo(KCDefine.B_ANCHOR_DOWN_CENTER, Vector3.zero, Vector3.zero, CGameInfoStorage.Inst.PlayLevelInfo.NumCells, true);
						var stOffset = new Vector3(KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_0_REAL);
						var stPos = new Vector3(KCDefine.B_VAL_0_REAL, (Access.MaxGridSize.y / -KCDefine.B_VAL_2_REAL) * (KCDefine.B_VAL_1_REAL / stGridInfo.m_stScale.y), KCDefine.B_VAL_0_REAL);

						m_oGridInfoList.ExAddVal(Factory.MakeGridInfo(KCDefine.B_ANCHOR_DOWN_CENTER, stPos, stOffset, CGameInfoStorage.Inst.PlayLevelInfo.NumCells, true));
						break;
					}
					default: {
						m_oGridInfoList.ExAddVal(Factory.MakeGridInfo(KCDefine.B_ANCHOR_MID_CENTER, Vector3.zero, Vector3.zero, CGameInfoStorage.Inst.PlayLevelInfo.NumCells));
						break;
					}
				}
			}
			// 그리드 정보를 설정한다 }

			this.CellObjLists = new List<CEObj>[CGameInfoStorage.Inst.PlayLevelInfo.NumCells.y, CGameInfoStorage.Inst.PlayLevelInfo.NumCells.x];
			CGameInfoStorage.Inst.PlayEpisodeInfo.m_oClearTargetInfoDict.ExCopyTo(m_oClearTargetInfoDict, (a_stTargetInfo) => a_stTargetInfo);

			// 객체 풀을 설정한다 {
			CSceneManager.ActiveSceneManager.AddObjsPool(KDefine.E_KEY_ITEM_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_ITEM), this.Params.m_oItemRoot, KCDefine.U_SIZE_OBJS_POOL_01, false);
			CSceneManager.ActiveSceneManager.AddObjsPool(KDefine.E_KEY_SKILL_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_SKILL), this.Params.m_oSkillRoot, KCDefine.U_SIZE_OBJS_POOL_01, false);
			CSceneManager.ActiveSceneManager.AddObjsPool(KDefine.E_KEY_OBJ_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_OBJ), this.Params.m_oObjRoot, KCDefine.U_SIZE_OBJS_POOL_01, false);
			CSceneManager.ActiveSceneManager.AddObjsPool(KDefine.E_KEY_FX_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_FX), this.Params.m_oFXRoot, KCDefine.U_SIZE_OBJS_POOL_01, false);

			CSceneManager.ActiveSceneManager.AddObjsPool(KDefine.E_KEY_CELL_OBJ_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_CELL_OBJ), this.Params.m_oObjRoot, KCDefine.U_SIZE_OBJS_POOL_01, false);
			CSceneManager.ActiveSceneManager.AddObjsPool(KDefine.E_KEY_PLAYER_OBJ_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_PLAYER_OBJ), this.Params.m_oObjRoot, KCDefine.B_VAL_1_INT, false);
			CSceneManager.ActiveSceneManager.AddObjsPool(KDefine.E_KEY_ENEMY_OBJ_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_ENEMY_OBJ), this.Params.m_oObjRoot, KCDefine.U_SIZE_OBJS_POOL_01, false);
			// 객체 풀을 설정한다 }
		}

		/** 엔진 객체 이벤트를 수신했을 경우 */
		private void OnReceiveEObjEvent(CEObjComponent a_oSender, EEngineObjEvent a_eEvent, string a_oParams) {
			switch(a_eEvent) {
				case EEngineObjEvent.AVOID: this.HandleAvoidEObjEvent(a_oSender, a_oParams); break;
				case EEngineObjEvent.DAMAGE: this.HandleDamageEObjEvent(a_oSender, a_oParams); break;
				case EEngineObjEvent.CRITICAL_DAMAGE: this.HandleCriticalDamageEObjEvent(a_oSender, a_oParams); break;
			}

			// 체력이 없을 경우
			if(a_oSender.AbilityValDictWrapper.m_oDict01.ExGetAbilityVal(EAbilityKinds.STAT_HP_01) <= KCDefine.B_VAL_0_INT) {
				// 플레이어 객체 일 경우
				if(a_oSender.Params.m_stBaseParams.m_oObjsPoolKey.Equals(KDefine.E_KEY_PLAYER_OBJ_OBJS_POOL)) {
					this.Params.m_oCallbackDict01.GetValueOrDefault(ECallback.CLEAR_FAIL)?.Invoke(this);
				} else {
					var oAcquireTargetInfoDict = CCollectionManager.Inst.SpawnDict<ulong, STTargetInfo>();

					try {
						this.SetupAcquireTargetInfos(a_oSender, oAcquireTargetInfoDict);
						this.Params.m_oCallbackDict02.GetValueOrDefault(ECallback.ACQUIRE)?.Invoke(this, oAcquireTargetInfoDict);
						global::Func.Acquire(CGameInfoStorage.Inst.PlayCharacterID, oAcquireTargetInfoDict, this.SelPlayerObj.Params.m_oObjTargetInfo, true);

						var stObjTradeInfo = CObjInfoTable.Inst.GetEnhanceObjTradeInfo(this.SelPlayerObj.Params.m_stObjInfo.m_eObjKinds);
						var stSkipTargetValInfo = this.SelPlayerObj.Params.m_oObjTargetInfo.m_oAbilityTargetInfoDict.ExGetSkipTargetValInfo(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_EXP, (int)this.SelPlayerObj.Params.m_oObjTargetInfo.m_oAbilityTargetInfoDict.ExGetTargetVal(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_LV), stObjTradeInfo.m_oPayTargetInfoDict);

						foreach(var stKeyVal in CGameInfoStorage.Inst.PlayEpisodeInfo.m_oClearTargetInfoDict) {
							bool bIsValid = stKeyVal.Value.TargetType == ETargetType.ITEM && (a_oSender as CEItem != null) && stKeyVal.Value.Kinds == ((int)(a_oSender as CEItem).Params.m_stItemInfo.m_eItemKinds).ExKindsToCorrectKinds(stKeyVal.Value.m_eKindsGroupType);

							// 클리어 타겟 정보가 존재 할 경우
							if(bIsValid || (stKeyVal.Value.TargetType == ETargetType.OBJ && (a_oSender as CEObj != null) && stKeyVal.Value.Kinds == ((int)(a_oSender as CEObj).Params.m_stObjInfo.m_eObjKinds).ExKindsToCorrectKinds(stKeyVal.Value.m_eKindsGroupType))) {
								m_oClearTargetInfoDict.ExIncrTargetVal(stKeyVal.Value.m_eTargetKinds, stKeyVal.Value.m_nKinds, -KCDefine.B_VAL_1_INT);
							}
						}

						// 플레이어 객체 레벨 강화가 가능 할 경우
						if(stSkipTargetValInfo.Item1 >= stSkipTargetValInfo.Item3) {
							global::Func.Pay(CGameInfoStorage.Inst.PlayCharacterID, stObjTradeInfo.m_oPayTargetInfoDict, this.PlayerObjList[KCDefine.B_VAL_0_INT].Params.m_oObjTargetInfo);
							global::Func.Acquire(CGameInfoStorage.Inst.PlayCharacterID, stObjTradeInfo.m_oAcquireTargetInfoDict, this.PlayerObjList[KCDefine.B_VAL_0_INT].Params.m_oObjTargetInfo, true);

							this.SelPlayerObj.SetupAbilityVals();
						}
						
						m_oBoolDict[EKey.IS_SAVE_USER_INFO] = oAcquireTargetInfoDict.ExIsValid() ? true : m_oBoolDict[EKey.IS_SAVE_USER_INFO];
					} finally {
						this.RemoveEObjComponent(a_oSender);
						CCollectionManager.Inst.DespawnDict(oAcquireTargetInfoDict);
					}
				}
			}

			// 클리어 타겟을 완료했을 경우
			if(m_oClearTargetInfoDict.All((a_stKeyVal) => a_stKeyVal.Value.m_stValInfo01.m_dmVal <= KCDefine.B_VAL_0_INT)) {
				this.Params.m_oCallbackDict01.GetValueOrDefault(ECallback.CLEAR)?.Invoke(this);
			}

			CSceneManager.GetSceneManager<GameScene.CSubGameSceneManager>(KCDefine.B_SCENE_N_GAME).SetEnableUpdateUIsState(true);
		}
		#endregion // 함수

		#region 클래스 함수
		/** 매개 변수를 생성한다 */
		public static STParams MakeParams(GameObject a_oItemRoot, GameObject a_oSkillRoot, GameObject a_oObjRoot, GameObject a_oFXRoot, Dictionary<ECallback, System.Action<CEngine>> a_oCallbackDict01 = null, Dictionary<ECallback, System.Action<CEngine, Dictionary<ulong, STTargetInfo>>> a_oCallbackDict02 = null) {
			return new STParams() {
				m_oItemRoot = a_oItemRoot,
				m_oSkillRoot = a_oSkillRoot,
				m_oObjRoot = a_oObjRoot,
				m_oFXRoot = a_oFXRoot,
				m_oCallbackDict01 = a_oCallbackDict01 ?? new Dictionary<ECallback, System.Action<CEngine>>(),
				m_oCallbackDict02 = a_oCallbackDict02 ?? new Dictionary<ECallback, System.Action<CEngine, Dictionary<ulong, STTargetInfo>>>()
			};
		}
		#endregion // 클래스 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

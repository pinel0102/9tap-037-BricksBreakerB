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
#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

#region 추가
			this.SubSetupAwake();
#endregion // 추가
		}

		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			this.Params = a_stParams;

			this.SetupEngine();
			this.SetupLevel();
			this.SetupGridLine();

#region 추가
			this.SubInit();
#endregion // 추가
		}

		/** 상태를 리셋한다 */
		public override void Reset() {
			base.Reset();
			m_oBoolDict.ExReplaceVal(EKey.IS_RUNNING, false);

#region 추가
			this.SubReset();
#endregion // 추가
		}
#endregion // 함수
	}

	/** 서브 엔진 */
	public partial class CEngine : CComponent {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
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

#region 변수
		private Dictionary<ESubKey, int> m_oSubIntDict = new Dictionary<ESubKey, int>();
		private Dictionary<EState, System.Func<bool>> m_oStateCheckerDict = new Dictionary<EState, System.Func<bool>>();
#endregion // 변수

#region 프로퍼티
		public EState State { get; private set; } = EState.NONE;
		public List<CEObj> PlayerObjList { get; } = new List<CEObj>();
		public List<CEObj> EnemyObjList { get; } = new List<CEObj>();

		public int SelPlayerObjIdx => m_oSubIntDict.GetValueOrDefault(ESubKey.SEL_PLAYER_OBJ_IDX);
		public CEObj SelPlayerObj => this.PlayerObjList[this.SelPlayerObjIdx];
#endregion // 프로퍼티

#region 함수
		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				// 실행 중 일 경우
				if(m_oBoolDict.GetValueOrDefault(EKey.IS_RUNNING)) {
					switch(this.State) {
						case EState.PLAY: this.HandlePlayState(a_fDeltaTime); break;
						case EState.PAUSE: this.HandlePauseState(a_fDeltaTime); break;
					}

					// 플레이어 객체가 존재 할 경우
					if(this.PlayerObjList.ExIsValid()) {
						var stEpisodeSize = this.CameraEpisodeSize * CAccess.ResolutionUnitScale;
						var stMainCameraPos = new Vector3(Mathf.Clamp(this.SelPlayerObj.transform.position.x, stEpisodeSize.x / -KCDefine.B_VAL_2_REAL, stEpisodeSize.x / KCDefine.B_VAL_2_REAL), Mathf.Clamp(this.SelPlayerObj.transform.position.y + (KDefine.E_OFFSET_MAIN_CAMERA * CAccess.ResolutionUnitScale), (stEpisodeSize.y / -KCDefine.B_VAL_2_REAL) - ((CSceneManager.ActiveSceneManager.ScreenHeight / KCDefine.B_VAL_3_REAL) * CAccess.ResolutionUnitScale), stEpisodeSize.y / KCDefine.B_VAL_2_REAL), CSceneManager.ActiveSceneMainCamera.transform.position.z);

						CSceneManager.ActiveSceneMainCamera.transform.position = Vector3.Lerp(CSceneManager.ActiveSceneMainCamera.transform.position, stMainCameraPos, a_fDeltaTime * KCDefine.B_VAL_9_REAL);
					}
				}

				// 유저 정보 저장이 필요 할 경우
				if(m_oBoolDict.GetValueOrDefault(EKey.IS_SAVE_USER_INFO)) {
					CUserInfoStorage.Inst.SaveUserInfo();
					m_oBoolDict.ExReplaceVal(EKey.IS_SAVE_USER_INFO, false);
				}
			}
		}

		/** 제거 되었을 경우 */
		public override void OnDestroy() {
			base.OnDestroy();

			try {
				// 앱이 실행 중 일 경우
				if(CSceneManager.IsAppRunning) {
					// Do Something
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CEngine.OnDestroy Exception: {oException.Message}");
			}
		}

		/** 플레이어 객체 자동 제어 여부를 변경한다 */
		public void SetIsPlayerObjAutoControl(bool a_bIsAutoControl) {
			this.SelPlayerObj.GetController<CEPlayerObjController>().SetIsAutoControl(a_bIsAutoControl);
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

		/** 초기화한다 */
		private void SubInit() {
			var stObjInfo = CObjInfoTable.Inst.GetObjInfo(EObjKinds.PLAYABLE_COMMON_CHARACTER_01);
			this.PlayerObjList.ExAddVal(this.CreatePlayerObj(stObjInfo, CUserInfoStorage.Inst.GetCharacterUserInfo(CGameInfoStorage.Inst.PlayCharacterID), null));

			CSceneManager.ActiveSceneMainCamera.transform.position = new Vector3(this.SelPlayerObj.transform.position.x, this.SelPlayerObj.transform.position.y + (KDefine.E_OFFSET_MAIN_CAMERA * CAccess.ResolutionUnitScale), CSceneManager.ActiveSceneMainCamera.transform.position.z);
		}

		/** 상태를 리셋한다 */
		private void SubReset() {
			this.SetState(EState.NONE);
		}

		/** 획득 타겟 정보를 설정한다 */
		private void SetupAcquireTargetInfos(CEObjComponent a_oEObjComponent, Dictionary<ulong, STTargetInfo> a_oOutAcquireTargetInfos) {
			// 아이템 일 경우
			if(a_oEObjComponent.Params.m_stBaseParams.m_oObjsPoolKey.Equals(KDefine.E_KEY_ITEM_OBJS_POOL)) {
				(a_oEObjComponent as CEItem).Params.m_stItemInfo.m_oAcquireTargetInfoDict.ExCopyTo(a_oOutAcquireTargetInfos, (a_stTargetInfo) => a_stTargetInfo);
			}
			// 적 객체 일 경우
			else if(a_oEObjComponent.Params.m_stBaseParams.m_oObjsPoolKey.Equals(KDefine.E_KEY_ENEMY_OBJ_OBJS_POOL)) {
				(a_oEObjComponent as CEObj).Params.m_stObjInfo.m_oAcquireTargetInfoDict.ExCopyTo(a_oOutAcquireTargetInfos, (a_stTargetInfo) => a_stTargetInfo);
			}
		}

		/** 엔진 객체 이벤트를 수신했을 경우 */
		private void OnReceiveEObjEvent(CEObjComponent a_oSender, EEngineObjEvent a_eEvent, string a_oParams) {
			switch(a_eEvent) {
				case EEngineObjEvent.AVOID:
				case EEngineObjEvent.DAMAGE:
				case EEngineObjEvent.CRITICAL_DAMAGE: {
					break;
				}
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

						bool bIsSaveUserInfo = m_oBoolDict.GetValueOrDefault(EKey.IS_SAVE_USER_INFO);
						m_oBoolDict.ExReplaceVal(EKey.IS_SAVE_USER_INFO, oAcquireTargetInfoDict.ExIsValid() ? true : bIsSaveUserInfo);
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

		/** 플레이 상태를 처리한다 */
		private void HandlePlayState(float a_fDeltaTime) {
			CFunc.UpdateComponents(this.ItemList, a_fDeltaTime);
			CFunc.UpdateComponents(this.SkillList, a_fDeltaTime);
			CFunc.UpdateComponents(this.FXList, a_fDeltaTime);
			CFunc.UpdateComponents(this.PlayerObjList, a_fDeltaTime);
			CFunc.UpdateComponents(this.EnemyObjList, a_fDeltaTime);

			// 실행 중 일 경우
			if(m_oBoolDict.GetValueOrDefault(EKey.IS_RUNNING)) {
				var oNumEnemyObjsDict = CCollectionManager.Inst.SpawnDict<EObjKinds, int>();

				try {
					for(int i = 0; i < this.EnemyObjList.Count; ++i) {
						int nNumEnemyObjs = oNumEnemyObjsDict.GetValueOrDefault(this.EnemyObjList[i].Params.m_stObjInfo.m_eObjKinds);
						oNumEnemyObjsDict.ExReplaceVal(this.EnemyObjList[i].Params.m_stObjInfo.m_eObjKinds, nNumEnemyObjs + KCDefine.B_VAL_1_INT);
					}

					foreach(var stKeyVal in CGameInfoStorage.Inst.PlayEpisodeInfo.m_oEnemyObjTargetInfoDict) {
						// 적 객체 생성이 가능 할 경우
						if(oNumEnemyObjsDict.GetValueOrDefault((EObjKinds)stKeyVal.Value.Kinds) < stKeyVal.Value.m_stValInfo01.m_dmVal && this.EnemyObjList.Count < CGameInfoStorage.Inst.PlayEpisodeInfo.m_nMaxNumEnemyObjs) {
							float fPosX = Random.Range(this.EpisodeSize.x / -KCDefine.B_VAL_2_REAL, this.EpisodeSize.x / KCDefine.B_VAL_2_REAL);
							float fPosY = Random.Range(this.EpisodeSize.y / -KCDefine.B_VAL_2_REAL, this.EpisodeSize.y / KCDefine.B_VAL_2_REAL);

							var oEnemyObj = this.CreateEnemyObj(CObjInfoTable.Inst.GetObjInfo((EObjKinds)stKeyVal.Value.Kinds), null);
							oEnemyObj.transform.localPosition = new Vector3(fPosX, fPosY, fPosY / this.EpisodeSize.y);

							this.EnemyObjList.ExAddVal(oEnemyObj);
						}
					}
				} finally {
					CCollectionManager.Inst.DespawnDict(oNumEnemyObjsDict);
				}
			}
		}

		/** 정지 상태를 처리한다 */
		private void HandlePauseState(float a_fDeltaTime) {
			// Do Something
		}

		/** 터치 시작 이벤트를 처리한다 */
		private void HandleTouchBeginEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 구동 모드 일 경우
			if(m_oBoolDict.GetValueOrDefault(EKey.IS_RUNNING)) {
				var stIdx = a_oEventData.ExGetLocalPos(this.Params.m_oObjRoot).ExToIdx(m_oGridInfoList[this.SelGridInfoIdx].m_stPivotPos, KDefine.E_SIZE_CELL);
			}
		}

		/** 터치 이동 이벤트를 처리한다 */
		private void HandleTouchMoveEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 구동 모드 일 경우
			if(m_oBoolDict.GetValueOrDefault(EKey.IS_RUNNING)) {
				var stIdx = a_oEventData.ExGetLocalPos(this.Params.m_oObjRoot).ExToIdx(m_oGridInfoList[this.SelGridInfoIdx].m_stPivotPos, KDefine.E_SIZE_CELL);
			}
		}

		/** 터치 종료 이벤트를 처리한다 */
		private void HandleTouchEndEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 구동 모드 일 경우
			if(m_oBoolDict.GetValueOrDefault(EKey.IS_RUNNING)) {
				var stIdx = a_oEventData.ExGetLocalPos(this.Params.m_oObjRoot).ExToIdx(m_oGridInfoList[this.SelGridInfoIdx].m_stPivotPos, KDefine.E_SIZE_CELL);
			}
		}
#endregion // 함수

#region 조건부 함수
#if UNITY_EDITOR
		/** 기즈모를 그린다 */
		public virtual void OnDrawGizmos() {
			// 메인 카메라가 존재 할 경우
			if(CSceneManager.IsExistsMainCamera) {
				// Do Something
			}
		}
#endif // #if UNITY_EDITOR
#endregion // 조건부 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

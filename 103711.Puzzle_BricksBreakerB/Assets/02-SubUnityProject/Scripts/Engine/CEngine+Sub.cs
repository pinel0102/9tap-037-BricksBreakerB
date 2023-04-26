using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using Timers;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.Linq;
using UnityEngine.EventSystems;

namespace NSEngine {
	/** 서브 엔진 */
	public partial class CEngine : CComponent {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			SKIP_TIME,
			TIME_SCALE,
			SEL_PLAYER_OBJ_IDX,
			SHOOT_START_POS,
			
			UP_BOUNDS_SPRITE,
			DOWN_BOUNDS_SPRITE,
			LEFT_BOUNDS_SPRITE,
			RIGHT_BOUNDS_SPRITE,
			[HideInInspector] MAX_VAL
		}

		/** 상태 */
		public enum EState {
			NONE = -1,
			PLAY,
			PAUSE,
			[HideInInspector] MAX_VAL
		}

		/** 플레이 상태 */
		public enum EPlayState {
			NONE = -1,
			IDLE,
			SHOOT,
			[HideInInspector] MAX_VAL
		}

		#region 변수
		private Dictionary<ESubKey, float> m_oSubRealDict = new Dictionary<ESubKey, float>() {
			[ESubKey.SKIP_TIME] = KCDefine.B_VAL_0_REAL,
			[ESubKey.TIME_SCALE] = KCDefine.B_VAL_1_REAL
		};

		private Dictionary<ESubKey, Vector3> m_oSubVec3Dict = new Dictionary<ESubKey, Vector3>() {
			[ESubKey.SHOOT_START_POS] = Vector3.zero
		};

		private Dictionary<ESubKey, int> m_oSubIntDict = new Dictionary<ESubKey, int>();
		private Dictionary<ESubKey, LineRenderer> m_oSubLineFXDict = new Dictionary<ESubKey, LineRenderer>();
		private Dictionary<ESubKey, SpriteRenderer> m_oSubSpriteDict = new Dictionary<ESubKey, SpriteRenderer>();

		private List<Tween> m_oAniList = new List<Tween>();
		private List<CEObj> m_oMoveCompleteBallObjList = new List<CEObj>();
        private List<GameObject> m_oAimDotList = new List<GameObject>();
		private Dictionary<EState, System.Func<bool>> m_oStateCheckerDict = new Dictionary<EState, System.Func<bool>>();
		#endregion // 변수

		#region 프로퍼티
		public EState State { get; private set; } = EState.NONE;		
        public List<CEObj> PlayerObjList { get; } = new List<CEObj>();
		public List<CEObj> EnemyObjList { get; } = new List<CEObj>();
		public int SelPlayerObjIdx => m_oSubIntDict.GetValueOrDefault(ESubKey.SEL_PLAYER_OBJ_IDX);
		public SpriteRenderer DownBoundsSprite => m_oSubSpriteDict[ESubKey.DOWN_BOUNDS_SPRITE];
		public CEObj SelPlayerObj => this.PlayerObjList[this.SelPlayerObjIdx];
		public CEObj SelBallObj => this.BallObjList[KCDefine.B_VAL_0_INT];
		#endregion // 프로퍼티

		#region 함수
		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				float fSkipTime = m_oSubRealDict[ESubKey.SKIP_TIME];
				m_oSubRealDict.ExReplaceVal(ESubKey.SKIP_TIME, (this.PlayState == EPlayState.IDLE) ? KCDefine.B_VAL_0_REAL : fSkipTime + a_fDeltaTime);

				// 일정 시간이 지났을 경우
				if(m_oSubRealDict[ESubKey.SKIP_TIME].ExIsGreateEquals(15.0f)) {
					float fTimeScale = m_oSubRealDict[ESubKey.TIME_SCALE];
					m_oSubRealDict.ExReplaceVal(ESubKey.TIME_SCALE, Mathf.Min(3.0f, fTimeScale + 1.0f));

					m_oSubRealDict.ExReplaceVal(ESubKey.SKIP_TIME, KCDefine.B_VAL_0_REAL);
				}

				// 실행 중 일 경우
				if(m_oBoolDict.GetValueOrDefault(EKey.IS_RUNNING)) {
					switch(this.State) {
						case EState.PLAY: this.HandlePlayState(a_fDeltaTime * m_oSubRealDict[ESubKey.TIME_SCALE]); break;
						case EState.PAUSE: this.HandlePauseState(a_fDeltaTime * m_oSubRealDict[ESubKey.TIME_SCALE]); break;
					}

					// 플레이어 객체가 존재 할 경우
					if(this.PlayerObjList.ExIsValid()) {
						var stMainCameraPos = this.GetMainCameraPos();
						CSceneManager.ActiveSceneMainCamera.transform.position = Vector3.Lerp(CSceneManager.ActiveSceneMainCamera.transform.position, stMainCameraPos.ExToWorld(this.Params.m_oObjRoot), a_fDeltaTime * KCDefine.B_VAL_9_REAL);
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
					for(int i = 0; i < m_oAniList.Count; ++i) {
						m_oAniList[i]?.Kill();
					}
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CEngine.OnDestroy Exception: {oException.Message}");
			}
		}

		/** 플레이어 객체 자동 제어 여부를 변경한다 */
		public void SetIsPlayerObjAutoControl(bool a_bIsAutoControl) {
			this.SelPlayerObj.GetController<CEPlayerObjController>().SetEnableAutoControl(a_bIsAutoControl);
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

		/** 모든 공을 떨어뜨린다 */
		public void DropAllBalls() {
			// 발사 상태 일 경우
			if(this.PlayState == EPlayState.SHOOT) {                
                
                StopAllCoroutines();
                isShooting = false;
                currentShootCount = 0;

				var oAniList = CCollectionManager.Inst.SpawnList<Tween>();

				try {
					var stPos = m_oMoveCompleteBallObjList.ExIsValid() ? m_oMoveCompleteBallObjList[KCDefine.B_VAL_0_INT].transform.localPosition : m_oSubVec3Dict[ESubKey.SHOOT_START_POS];
					CScheduleManager.Inst.RemoveTimer(this);

                    for(int i = 0; i < this.BallObjList.Count; ++i) {
						this.BallObjList[i].GetController<CEBallObjController>().Initialize();
                        oAniList.ExAddVal(this.BallObjList[i].transform.DOLocalMove(stPos, KCDefine.B_VAL_0_3_REAL));
					}

                    for(int i = 0; i < this.ExtraBallObjList.Count; ++i) {
						this.ExtraBallObjList[i].GetController<CEBallObjController>().Initialize();
                        oAniList.ExAddVal(this.ExtraBallObjList[i].transform.DOLocalMove(stPos, KCDefine.B_VAL_0_3_REAL));
					}

					m_oAniList.ExAddVal(CFactory.MakeSequence(oAniList, (a_oSender) => {
						a_oSender?.Kill();
						oAniList.ExRemoveVal(a_oSender);

                        TurnEnd(true);

					}, KCDefine.B_VAL_0_REAL, true));

				} finally {
					CCollectionManager.Inst.DespawnList(oAniList);
				}
			}
		}

		/** 초기화한다 */
		private void SubInit() {
            
            InitResoulution();
            InitPreview();
            InitCellRoot();
            InitLayerMask();
            
            InitScoreList(CGameInfoStorage.Inst.PlayLevelInfo);
            InitCombo();
            InitScore();
            InitBooster();

            currentLevel = (int)CGameInfoStorage.Inst.PlayLevelInfo.ULevelID + 1;
            isTutorial = GlobalDefine.TUTORIAL_LEVEL_BOTTOM_ITEM.Contains(currentLevel);
            isWarning = false;

#if NEVER_USE_THIS
			// FIXME: dante (비활성 처리 - 필요 시 활성 및 사용 가능) {
			var stObjInfo = CObjInfoTable.Inst.GetObjInfo(EObjKinds.PLAYABLE_COMMON_CHARACTER_01);
			this.PlayerObjList.ExAddVal(this.CreatePlayerObj(stObjInfo, CUserInfoStorage.Inst.GetCharacterUserInfo(CGameInfoStorage.Inst.PlayCharacterID), null));

			CSceneManager.ActiveSceneMainCamera.transform.position = new Vector3(this.SelPlayerObj.transform.position.x, this.SelPlayerObj.transform.position.y + (KDefine.E_OFFSET_MAIN_CAMERA * CAccess.ResolutionUnitScale), CSceneManager.ActiveSceneMainCamera.transform.position.z);
			// FIXME: dante (비활성 처리 - 필요 시 활성 및 사용 가능) }
#endif // #if NEVER_USE_THIS

			for(int i = 0; i < CGameInfoStorage.Inst.PlayEpisodeInfo.m_nNumBalls; ++i) {
                CreateBall(i);
			}

            startPosition = SelBallObj.transform.localPosition;
            shootDirection = Vector3.zero;

            // 스프라이트를 설정한다 {
			CFunc.SetupComponents(new List<(ESubKey, string, GameObject, GameObject)>() {
				(ESubKey.UP_BOUNDS_SPRITE, $"{ESubKey.UP_BOUNDS_SPRITE}", this.Params.m_oWallRoot, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_BOUNDS)),
				(ESubKey.DOWN_BOUNDS_SPRITE, $"{ESubKey.DOWN_BOUNDS_SPRITE}", this.Params.m_oWallRoot, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_BOUNDS)),
				(ESubKey.LEFT_BOUNDS_SPRITE, $"{ESubKey.LEFT_BOUNDS_SPRITE}", this.Params.m_oWallRoot, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_BOUNDS)),
				(ESubKey.RIGHT_BOUNDS_SPRITE, $"{ESubKey.RIGHT_BOUNDS_SPRITE}", this.Params.m_oWallRoot, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_BOUNDS))
			}, m_oSubSpriteDict);

			float texWidth = m_oSubSpriteDict[ESubKey.UP_BOUNDS_SPRITE].sprite.textureRect.width;
			float texHeight = m_oSubSpriteDict[ESubKey.UP_BOUNDS_SPRITE].sprite.textureRect.height;
            
            float horizontalWidth = reWidth / texWidth;
            float veticalHeight = ((reHeight - uiAreaTop - uiAreaBottom) / texHeight);
            float objScale = CSceneManager.ObjsRootScale.x;

			m_oSubSpriteDict[ESubKey.UP_BOUNDS_SPRITE].transform.localScale = new Vector3(horizontalWidth / objScale, KCDefine.B_VAL_1_REAL, KCDefine.B_VAL_1_REAL);
			m_oSubSpriteDict[ESubKey.UP_BOUNDS_SPRITE].transform.localPosition = new Vector3(0, (reHeight * 0.5f) - uiAreaTop + (m_oSubSpriteDict[ESubKey.UP_BOUNDS_SPRITE].sprite.textureRect.height / KCDefine.B_VAL_2_REAL), 0);

			m_oSubSpriteDict[ESubKey.DOWN_BOUNDS_SPRITE].transform.localScale = new Vector3(horizontalWidth / objScale, KCDefine.B_VAL_1_REAL, KCDefine.B_VAL_1_REAL);
			m_oSubSpriteDict[ESubKey.DOWN_BOUNDS_SPRITE].transform.localPosition = new Vector3(0, -((reHeight * 0.5f) - uiAreaBottom) - (m_oSubSpriteDict[ESubKey.DOWN_BOUNDS_SPRITE].sprite.textureRect.height / KCDefine.B_VAL_2_REAL), 0);

			m_oSubSpriteDict[ESubKey.LEFT_BOUNDS_SPRITE].transform.localScale = new Vector3(KCDefine.B_VAL_1_REAL, veticalHeight, KCDefine.B_VAL_1_REAL);
			m_oSubSpriteDict[ESubKey.LEFT_BOUNDS_SPRITE].transform.localPosition = new Vector3(((-reWidth * 0.5f) - (m_oSubSpriteDict[ESubKey.LEFT_BOUNDS_SPRITE].sprite.textureRect.width / KCDefine.B_VAL_2_REAL)) / objScale, 0, 0);

			m_oSubSpriteDict[ESubKey.RIGHT_BOUNDS_SPRITE].transform.localScale = new Vector3(KCDefine.B_VAL_1_REAL, veticalHeight, KCDefine.B_VAL_1_REAL);
			m_oSubSpriteDict[ESubKey.RIGHT_BOUNDS_SPRITE].transform.localPosition = new Vector3(((reWidth * 0.5f) + (m_oSubSpriteDict[ESubKey.RIGHT_BOUNDS_SPRITE].sprite.textureRect.width / KCDefine.B_VAL_2_REAL)) / objScale, 0, 0);
			// 스프라이트를 설정한다 }
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
#if NEVER_USE_THIS
				case EEngineObjEvent.AVOID:
				case EEngineObjEvent.DAMAGE:
				case EEngineObjEvent.CRITICAL_DAMAGE: {
					break;
				}
#endif // #if NEVER_USE_THIS

				case EEngineObjEvent.DESTROY: {
					CEObj _ceObj = a_oSender as CEObj;
                    _ceObj.SetCellActive(false);
					this.RemoveCellObj(_ceObj);
                    
                    // 클리어했을 경우
					if(this.IsClear()) {
						m_oSubRealDict[ESubKey.TIME_SCALE] = KCDefine.B_VAL_3_REAL;
					}

					break;
				}
				case EEngineObjEvent.MOVE_COMPLETE: {
                    m_oMoveCompleteBallObjList.ExAddVal(a_oSender as CEObj);

                    for (int i=1; i < m_oMoveCompleteBallObjList.Count; i++)
                    {
                        m_oMoveCompleteBallObjList[i].NumText.text = string.Empty;
                    }

                    int excludeCount = m_oMoveCompleteBallObjList.FindAll(item => ExtraBallObjList.Contains(item) || (BallObjList.Contains(item) && DeleteBallList.Contains(item))).Count;
                    int completeCount = m_oMoveCompleteBallObjList.Count - excludeCount;
                    
                    m_oMoveCompleteBallObjList[KCDefine.B_VAL_0_INT].NumText.text = completeCount > 0 ? GlobalDefine.GetBallText(completeCount, 0) : string.Empty;

					var oSequence = CFactory.MakeSequence(a_oSender.transform.DOLocalMove(m_oMoveCompleteBallObjList[KCDefine.B_VAL_0_INT].transform.localPosition, KCDefine.U_DURATION_ANI), (a_oSequenceSender) => {
						a_oSequenceSender?.Kill();

                        // 모든 공이 이동을 완료했을 경우
                        if(m_oMoveCompleteBallObjList.Count >= this.BallObjList.Count + this.ExtraBallObjList.Count) {

                            TurnEnd();
                        }
					});

					m_oAniList.ExAddVal(oSequence);

					break;
				}
			}

#if NEVER_USE_THIS
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
#endif // #if NEVER_USE_THIS

			CSceneManager.GetSceneManager<GameScene.CSubGameSceneManager>(KCDefine.B_SCENE_N_GAME).SetEnableUpdateUIsState(true);
		}

		/** 플레이 상태를 처리한다 */
		private void HandlePlayState(float a_fDeltaTime) {
#if NEVER_USE_THIS
			CFunc.UpdateComponents(this.ItemList, a_fDeltaTime);
			CFunc.UpdateComponents(this.SkillList, a_fDeltaTime);
			CFunc.UpdateComponents(this.FXList, a_fDeltaTime);
			CFunc.UpdateComponents(this.PlayerObjList, a_fDeltaTime);
			CFunc.UpdateComponents(this.EnemyObjList, a_fDeltaTime);
#endif // #if NEVER_USE_THIS

			// 실행 중 일 경우
			if(m_oBoolDict.GetValueOrDefault(EKey.IS_RUNNING)) {
#if NEVER_USE_THIS
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
#endif // #if NEVER_USE_THIS

				CFunc.UpdateComponents(this.BallObjList, a_fDeltaTime);
                CFunc.UpdateComponents(this.ExtraBallObjList, a_fDeltaTime);
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
                var stPos = a_oEventData.ExGetLocalPos(this.Params.m_oObjRoot, CSceneManager.ActiveSceneManager.ScreenSize);
				var stIdx = stPos.ExToIdx(this.SelGridInfo.m_stPivotPos, Access.CellSize);

#if NEVER_USE_THIS
				// 인덱스가 유효 할 경우
				if(this.CellObjLists.ExIsValidIdx(stIdx)) {
					// Do Something
				}
#endif // #if NEVER_USE_THIS

				// 조준 가능 할 경우
				if(this.IsEnableAiming(stPos)) {
                    this.HandleTouchMoveEvent(a_oSender, a_oEventData);
				}
			}
		}

		/** 터치 이동 이벤트를 처리한다 */
		private void HandleTouchMoveEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 구동 모드 일 경우
			if(m_oBoolDict.GetValueOrDefault(EKey.IS_RUNNING)) {
				var stPos = a_oEventData.ExGetLocalPos(this.Params.m_oObjRoot, CSceneManager.ActiveSceneManager.ScreenSize);
				//var stIdx = stPos.ExToIdx(this.SelGridInfo.m_stPivotPos, Access.CellSize);

                this.ResetGuideLine();
                this.DrawGuideLine(stPos);
			}
		}

		/** 터치 종료 이벤트를 처리한다 */
		private void HandleTouchEndEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 구동 모드 일 경우
			if(m_oBoolDict.GetValueOrDefault(EKey.IS_RUNNING)) {
                
                var stPos = a_oEventData.ExGetLocalPos(this.Params.m_oObjRoot, CSceneManager.ActiveSceneManager.ScreenSize);
				var stIdx = a_oEventData.ExGetLocalPos(this.Params.m_oObjRoot, CSceneManager.ActiveSceneManager.ScreenSize).ExToIdx(this.SelGridInfo.m_stPivotPos, Access.CellSize);

#if NEVER_USE_THIS
				// 인덱스가 유효 할 경우
				if(this.CellObjLists.ExIsValidIdx(stIdx)) {
					// Do Something
				}
#endif // #if NEVER_USE_THIS
                
                this.CallShoot(stPos);
			}
		}

        public void CallShoot(Vector3 stPos)
        {
            // 조준 가능 할 경우
            if(this.IsEnableAiming(stPos)) {     

                startPosition = SelBallObj.transform.localPosition;

                this.SetPlayState(EPlayState.SHOOT);
                var stDirection = stPos - this.SelBallObj.transform.localPosition;

                float fAngle = Vector3.Angle(stDirection, Vector3.right * Mathf.Sign(stDirection.x));
                fAngle = fAngle.ExIsLess(KDefine.E_MIN_ANGLE_AIMING) ? KDefine.E_MIN_ANGLE_AIMING : fAngle;

                int nNumShootBalls = KCDefine.B_VAL_0_INT;
                m_oMoveCompleteBallObjList.Clear();

                m_oSubRealDict.ExReplaceVal(ESubKey.TIME_SCALE, KCDefine.B_VAL_1_REAL);
                m_oSubVec3Dict.ExReplaceVal(ESubKey.SHOOT_START_POS, this.SelBallObj.transform.localPosition);

                for(int i = 0; i < this.BallObjList.Count; ++i) {
                    this.BallObjList[i].NumText.text = string.Empty;
                }

                shootDirection = new Vector3(Mathf.Cos(fAngle * Mathf.Deg2Rad) * Mathf.Sign(stDirection.x), Mathf.Sin(fAngle * Mathf.Deg2Rad), KCDefine.B_VAL_0_REAL) * KDefine.E_SPEED_SHOOT;
                
                currentShootCount = 0;
                isWarning = false;

                ShootBalls(nNumShootBalls, this.BallObjList.Count);
            }
            else
            {
                subGameSceneManager.warningObject.SetActive(isWarning);
            }

            ResetGuideLine();
        }

        public void LevelClear()
        {
            Debug.Log(CodeManager.GetMethodName() + string.Format(GlobalDefine.FORMAT_INT, currentLevel));

            CUserInfoStorage.Inst.UserInfo.LevelCurrent = Mathf.Min(Mathf.Max(CUserInfoStorage.Inst.UserInfo.LevelCurrent, currentLevel + 1), CLevelInfoTable.Inst.levelCount);
            CUserInfoStorage.Inst.SaveUserInfo();

            Params.m_oCallbackDict01[NSEngine.CEngine.ECallback.CLEAR].Invoke(this);
        }

        public void LevelFail()
        {
            Debug.Log(CodeManager.GetMethodName() + string.Format(GlobalDefine.FORMAT_INT, currentLevel));

            this.InitCombo();
            isLevelFail = true;

            Params.m_oCallbackDict01[NSEngine.CEngine.ECallback.CLEAR_FAIL].Invoke(this);
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

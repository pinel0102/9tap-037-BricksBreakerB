using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using UnityEngine.EventSystems;

namespace GameScene {
	/** 서브 게임 씬 관리자 */
	public partial class CSubGameSceneManager : CGameSceneManager {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			IS_UPDATE_UIS_STATE,
			CONTINUE_TIMES,
			BG_SPRITE,
			UP_BG_SPRITE,
			DOWN_BG_SPRITE,
			LEFT_BG_SPRITE,
			RIGHT_BG_SPRITE,
			SEL_REWARD_ADS_UIS,
			[HideInInspector] MAX_VAL
		}

		/** 팝업 결과 */
		private enum EPopupResult {
			NONE = -1,
			NEXT,
			RETRY,
			RESUME,
			CONTINUE,
			LEAVE,
			[HideInInspector] MAX_VAL
		}

		/** 보상 광고 UI */
		private enum ERewardAdsUIs {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

		#region 변수
		private NSEngine.CEngine m_oEngine = null;
		private Dictionary<EKey, int> m_oIntDict = new Dictionary<EKey, int>();
		private Dictionary<EKey, bool> m_oBoolDict = new Dictionary<EKey, bool>();
		private Dictionary<EKey, ERewardAdsUIs> m_oRewardAdsUIsDict = new Dictionary<EKey, ERewardAdsUIs>();
		private Dictionary<EKey, SpriteRenderer> m_oSpriteDict = new Dictionary<EKey, SpriteRenderer>();

		/** =====> 객체 <===== */
        [Header("★ [Reference] SubGameSceneManager")]
		[SerializeField] private List<GameObject> m_oRewardAdsUIsList = new List<GameObject>();
        #endregion // 변수

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

            // 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
#if DEBUG || DEVELOPMENT_BUILD
				// 플레이 레벨 정보가 없을 경우
				if(CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID01 <= KCDefine.B_IDX_INVALID) {
					Func.SetupPlayEpisodeInfo(CGameInfoStorage.Inst.PlayCharacterID, KCDefine.B_VAL_0_INT, EPlayMode.NORM);
				}
#endif // #if DEBUG || DEVELOPMENT_BUILD

				this.SetupAwake();
			}
		}

		/** 초기화 */
		public override void Start() {
			base.Start();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				this.SetupStart();
				this.UpdateUIsState();

				Func.PlayBGSnd(EResKinds.SND_BG_SCENE_GAME_01);
			}
		}

		/** 앱이 정지 되었을 경우 */
		public override void OnApplicationPause(bool a_bIsPause) {
			base.OnApplicationPause(a_bIsPause);

			// 재개 되었을 경우
			if(!a_bIsPause && CSceneManager.IsAppRunning) {
#if ADS_MODULE_ENABLE
				// 광고 출력이 가능 할 경우
				if(CAppInfoStorage.Inst.IsEnableShowFullscreenAds && CAdsManager.Inst.IsLoadFullscreenAds(CPluginInfoTable.Inst.AdsPlatform)) {
					Func.ShowFullscreenAds(null);
				}
#endif // #if ADS_MODULE_ENABLE

				Func.ShowResumePopup(this.PopupUIs, (a_oSender) => {
					(a_oSender as CResumePopup).Init(CResumePopup.MakeParams(new Dictionary<CResumePopup.ECallback, System.Action<CResumePopup>>() {
						[CResumePopup.ECallback.RESUME] = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.RESUME),
						[CResumePopup.ECallback.LEAVE] = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.LEAVE)
					}));
				});
			}
		}

		/** UI 상태 갱신 여부를 변경한다 */
		public void SetEnableUpdateUIsState(bool a_bIsEnable) {
			m_oBoolDict.ExReplaceVal(EKey.IS_UPDATE_UIS_STATE, a_bIsEnable);
		}

		/** 내비게이션 스택 이벤트를 수신했을 경우 */
		public override void OnReceiveNavStackEvent(ENavStackEvent a_eEvent) {
			base.OnReceiveNavStackEvent(a_eEvent);

			// 백 키 눌림 이벤트 일 경우
			if(a_eEvent == ENavStackEvent.BACK_KEY_DOWN) {
                // 이전 씬이 레벨 에디터 씬 일 경우
                if(CSceneLoader.Inst.PrevActiveSceneName.Equals(KCDefine.B_SCENE_N_LEVEL_EDITOR) || GlobalDefine.isLevelEditor) {
					Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_LEAVE_P_MSG), this.OnReceiveLeavePopupResult);
				} else {
                    if(IsTabMoving()) 
                        return;
                
                    if(currentTab > -1)
                        this.CloseTabs();
                    else
                        this.OnTouchPauseBtn();
				}
			}
		}

		/** 터치 이벤트를 처리한다 */
		protected override void HandleTouchEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData, ETouchEvent a_eTouchEvent) {
			base.HandleTouchEvent(a_oSender, a_oEventData, a_eTouchEvent);

			// 배경 터치 전달자 일 경우
			if(this.BGTouchDispatcher == a_oSender) {
				switch(a_eTouchEvent) {
					case ETouchEvent.BEGIN: this.HandleTouchBeginEvent(a_oSender, a_oEventData); break;
					case ETouchEvent.MOVE: this.HandleTouchMoveEvent(a_oSender, a_oEventData); break;
					case ETouchEvent.END: this.HandleTouchEndEvent(a_oSender, a_oEventData); break;
				}

				m_oEngine.HandleTouchEvent(a_oSender, a_oEventData, a_eTouchEvent);
			}
		}

		/** 씬을 설정한다 */
		private void SetupAwake() {
			this.SetupEngine();
			this.SetupRewardAdsUIs();

			// 버튼을 설정한다
			/*CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
				(KCDefine.U_OBJ_N_PAUSE_BTN, this.UIsBase, this.OnTouchPauseBtn),
				(KCDefine.U_OBJ_N_SETTINGS_BTN, this.UIsBase, this.OnTouchSettingsBtn)
			});*/

			// 비율을 설정한다 {
			bool bIsValid01 = !float.IsNaN(m_oEngine.SelGridInfo.m_stScale.x) && !float.IsInfinity(m_oEngine.SelGridInfo.m_stScale.x);
			bool bIsValid02 = !float.IsNaN(m_oEngine.SelGridInfo.m_stScale.y) && !float.IsInfinity(m_oEngine.SelGridInfo.m_stScale.y);
			bool bIsValid03 = !float.IsNaN(m_oEngine.SelGridInfo.m_stScale.z) && !float.IsInfinity(m_oEngine.SelGridInfo.m_stScale.z);

			this.CellRoot.transform.localScale = (bIsValid01 && bIsValid02 && bIsValid03) ? m_oEngine.SelGridInfo.m_stScale : Vector3.one;
            this.ObjRoot.transform.localScale = (bIsValid01 && bIsValid02 && bIsValid03) ? m_oEngine.SelGridInfo.m_stScale : Vector3.one;
            this.FXRoot.transform.localScale = (bIsValid01 && bIsValid02 && bIsValid03) ? m_oEngine.SelGridInfo.m_stScale : Vector3.one;
            // 비율을 설정한다 }

			// 스프라이트를 설정한다 {
			var oSpriteInfoDict = new Dictionary<EKey, (Sprite, STSortingOrderInfo)>() {
				[EKey.BG_SPRITE] = (Access.GetBGSprite(KDefine.GS_TEX_P_FMT_BG, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID01, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03), KDefine.GS_SORTING_OI_BG),
				[EKey.UP_BG_SPRITE] = (Access.GetBGSprite(KDefine.GS_TEX_P_FMT_UP_BG, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID01, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03), KDefine.GS_SORTING_OI_UP_BG),
				[EKey.DOWN_BG_SPRITE] = (Access.GetBGSprite(KDefine.GS_TEX_P_FMT_DOWN_BG, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID01, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03), KDefine.GS_SORTING_OI_DOWN_BG),
				[EKey.LEFT_BG_SPRITE] = (Access.GetBGSprite(KDefine.GS_TEX_P_FMT_LEFT_BG, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID01, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03), KDefine.GS_SORTING_OI_LEFT_BG),
				[EKey.RIGHT_BG_SPRITE] = (Access.GetBGSprite(KDefine.GS_TEX_P_FMT_RIGHT_BG, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID01, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03), KDefine.GS_SORTING_OI_RIGHT_BG)
			};

			CFunc.SetupComponents(new List<(EKey, string, GameObject, GameObject)>() {
				(EKey.BG_SPRITE, $"{EKey.BG_SPRITE}", this.Objs, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_SPRITE)),
				(EKey.UP_BG_SPRITE, $"{EKey.UP_BG_SPRITE}", this.Objs, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_SPRITE)),
				(EKey.DOWN_BG_SPRITE, $"{EKey.DOWN_BG_SPRITE}", this.Objs, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_SPRITE)),
				(EKey.LEFT_BG_SPRITE, $"{EKey.LEFT_BG_SPRITE}", this.Objs, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_SPRITE)),
				(EKey.RIGHT_BG_SPRITE, $"{EKey.RIGHT_BG_SPRITE}", this.Objs, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_SPRITE))
			}, m_oSpriteDict);

			foreach(var stKeyVal in m_oSpriteDict) {
				stKeyVal.Value.drawMode = SpriteDrawMode.Tiled;
				stKeyVal.Value.tileMode = SpriteTileMode.Continuous;
				stKeyVal.Value.sprite = oSpriteInfoDict.GetValueOrDefault(stKeyVal.Key).Item1;
				stKeyVal.Value.ExSetSortingOrder(oSpriteInfoDict.GetValueOrDefault(stKeyVal.Key).Item2);
			}

#if NEVER_USE_THIS
			// FIXME: dante (비활성 처리 - 필요 시 활성 및 사용 가능) {
			var stSize = new Vector3(Mathf.Max(this.ScreenWidth, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.x), Mathf.Max(this.ScreenHeight, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.y), KCDefine.B_VAL_0_REAL);

			m_oSpriteDict.GetValueOrDefault(EKey.BG_SPRITE).size = stSize;
			m_oSpriteDict.GetValueOrDefault(EKey.BG_SPRITE).transform.localScale = Vector3.one;
			m_oSpriteDict.GetValueOrDefault(EKey.BG_SPRITE).transform.localPosition = Vector3.zero;

			m_oSpriteDict.GetValueOrDefault(EKey.UP_BG_SPRITE).size = new Vector3(stSize.x + (this.ScreenWidth * KCDefine.B_VAL_2_REAL), m_oSpriteDict.GetValueOrDefault(EKey.UP_BG_SPRITE).sprite.rect.height, KCDefine.B_VAL_0_REAL);
			m_oSpriteDict.GetValueOrDefault(EKey.UP_BG_SPRITE).transform.localScale = Vector3.one;
			m_oSpriteDict.GetValueOrDefault(EKey.UP_BG_SPRITE).transform.localPosition = new Vector3(KCDefine.B_VAL_0_REAL, (stSize.y / KCDefine.B_VAL_2_REAL) + (m_oSpriteDict.GetValueOrDefault(EKey.UP_BG_SPRITE).sprite.rect.height / KCDefine.B_VAL_2_REAL), KCDefine.B_VAL_0_REAL);

			m_oSpriteDict.GetValueOrDefault(EKey.DOWN_BG_SPRITE).size = m_oSpriteDict.GetValueOrDefault(EKey.UP_BG_SPRITE).size;
			m_oSpriteDict.GetValueOrDefault(EKey.DOWN_BG_SPRITE).transform.localScale = Vector3.one;
			m_oSpriteDict.GetValueOrDefault(EKey.DOWN_BG_SPRITE).transform.localPosition = new Vector3(KCDefine.B_VAL_0_REAL, -((stSize.y / KCDefine.B_VAL_2_REAL) + (m_oSpriteDict.GetValueOrDefault(EKey.DOWN_BG_SPRITE).sprite.rect.height / KCDefine.B_VAL_2_REAL) - NSEngine.KDefine.E_OFFSET_BOTTOM), KCDefine.B_VAL_0_REAL);

			m_oSpriteDict.GetValueOrDefault(EKey.LEFT_BG_SPRITE).size = new Vector3(m_oSpriteDict.GetValueOrDefault(EKey.LEFT_BG_SPRITE).sprite.rect.width, Mathf.Max(this.ScreenHeight, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.y), KCDefine.B_VAL_0_REAL);
			m_oSpriteDict.GetValueOrDefault(EKey.LEFT_BG_SPRITE).transform.localScale = Vector3.one;
			m_oSpriteDict.GetValueOrDefault(EKey.LEFT_BG_SPRITE).transform.localPosition = new Vector3(-((stSize.y / KCDefine.B_VAL_2_REAL) + (m_oSpriteDict.GetValueOrDefault(EKey.LEFT_BG_SPRITE).sprite.rect.height / KCDefine.B_VAL_2_REAL)), KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_0_REAL);

			m_oSpriteDict.GetValueOrDefault(EKey.RIGHT_BG_SPRITE).size = m_oSpriteDict.GetValueOrDefault(EKey.LEFT_BG_SPRITE).size;
			m_oSpriteDict.GetValueOrDefault(EKey.RIGHT_BG_SPRITE).transform.localScale = Vector3.one;
			m_oSpriteDict.GetValueOrDefault(EKey.RIGHT_BG_SPRITE).transform.localPosition = new Vector3((stSize.y / KCDefine.B_VAL_2_REAL) + (m_oSpriteDict.GetValueOrDefault(EKey.RIGHT_BG_SPRITE).sprite.rect.height / KCDefine.B_VAL_2_REAL), KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_0_REAL);
			// FIXME: dante (비활성 처리 - 필요 시 활성 및 사용 가능) }
#endif // #if NEVER_USE_THIS
			// 스프라이트를 설정한다 }

            this.SubAwake();
		}

		/** 씬을 설정한다 */
		private void SetupStart() {
			this.ApplySelItems();
			CGameInfoStorage.Inst.ResetSelItems();

			this.SubStart();
		}

		/** 엔진을 설정한다 */
		private void SetupEngine() {
			var oCallbackDict01 = new Dictionary<NSEngine.CEngine.ECallback, System.Action<NSEngine.CEngine>>() {
				[NSEngine.CEngine.ECallback.CLEAR] = this.OnReceiveClearCallback,
				[NSEngine.CEngine.ECallback.CLEAR_FAIL] = this.OnReceiveClearFailCallback
			};

			var oCallbackDict02 = new Dictionary<NSEngine.CEngine.ECallback, System.Action<NSEngine.CEngine, Dictionary<ulong, STTargetInfo>>>() {
				[NSEngine.CEngine.ECallback.ACQUIRE] = this.OnReceiveAcquireCallback
			};

			m_oEngine = CFactory.CreateObj<NSEngine.CEngine>(KDefine.GS_OBJ_N_ENGINE, this.gameObject);
			m_oEngine.Init(NSEngine.CEngine.MakeParams(this.CellRoot, this.ItemRoot, this.SkillRoot, this.ObjRoot, this.WallRoot, this.FXRoot, oCallbackDict01, oCallbackDict02));

            AssignEngine();

            m_oEngine.SetPlayState(NSEngine.CEngine.EPlayState.IDLE);
		}

		/** 보상 광고 UI 를 설정한다 */
		private void SetupRewardAdsUIs() {
			for(int i = 0; i < m_oRewardAdsUIsList.Count; ++i) {
				var eRewardAdsUIs = (ERewardAdsUIs)i;
				m_oRewardAdsUIsList[i]?.GetComponentInChildren<Button>()?.onClick.AddListener(() => this.OnTouchAdsBtn(eRewardAdsUIs));
			}
		}

		/** UI 상태를 갱신한다 */
		private void UpdateUIsState() {
			this.UpdateRewardAdsUIsState();
			this.SubUpdateUIsState();
		}

		/** 보상 광고 UI 상태를 갱신한다 */
		private void UpdateRewardAdsUIsState() {
			for(int i = 0; i < m_oRewardAdsUIsList.Count; ++i) {
				m_oRewardAdsUIsList[i]?.SetActive(CGameInfoStorage.Inst.PlayEpisodeInfo.ULevelID + KCDefine.B_VAL_1_INT >= KDefine.GS_MIN_LEVEL_ENABLE_REWARD_ADS_WATCH);
			}
		}

		/** 그만두기 팝업 결과를 수신했을 경우 */
		private void OnReceiveLeavePopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
#if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE
				CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_LEVEL_EDITOR);
#endif // #if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE
			}
		}

		/** 팝업 결과를 수신했을 경우 */
		private void OnReceivePopupResult(CPopup a_oSender, EPopupResult a_eResult) {
			// 팝업이 존재 할 경우
			if(a_oSender != null) {
				a_oSender.SetIgnoreAni(a_eResult != EPopupResult.CONTINUE);
				a_oSender.Close();
			}

			switch(a_eResult) {
				case EPopupResult.NEXT: this.LoadNextLevel(a_oSender); break;
				case EPopupResult.RETRY: this.RetryPlayLevel(a_oSender); break;
				case EPopupResult.RESUME: this.ResumePlayLevel(a_oSender); break;
				case EPopupResult.CONTINUE: this.ContinuePlayLevel(a_oSender); break;
				case EPopupResult.LEAVE: this.LeavePlayLevel(a_oSender); break;
			}
		}

		/** 클리어 콜백을 수신했을 경우 */
		private void OnReceiveClearCallback(NSEngine.CEngine a_oSender) {
			var oLevelClearInfo = Access.GetLevelClearInfo(CGameInfoStorage.Inst.PlayCharacterID, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID01, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03, true);
			oLevelClearInfo.m_stRecordInfo.m_nIntRecord = a_oSender.RecordInfo.m_nIntRecord;
			oLevelClearInfo.m_stRecordInfo.m_dblRealRecord = a_oSender.RecordInfo.m_dblRealRecord;
			oLevelClearInfo.m_stBestRecordInfo.m_nIntRecord = System.Math.Max(a_oSender.RecordInfo.m_nIntRecord, oLevelClearInfo.m_stBestRecordInfo.m_nIntRecord);
			oLevelClearInfo.m_stBestRecordInfo.m_dblRealRecord = a_oSender.RecordInfo.m_dblRealRecord.ExIsGreate(oLevelClearInfo.m_stBestRecordInfo.m_dblRealRecord) ? a_oSender.RecordInfo.m_dblRealRecord : oLevelClearInfo.m_stBestRecordInfo.m_dblRealRecord;

			CGameInfoStorage.Inst.SaveGameInfo();
			this.ShowResultPopup(true);
		}

		/** 클리어 실패 콜백을 수신했을 경우 */
		private void OnReceiveClearFailCallback(NSEngine.CEngine a_oSender) {
            this.ShowContinuePopup();
		}

        /** 선택 아이템을 적용한다 */
		private void ApplySelItems() {
			for(int i = 0; i < CGameInfoStorage.Inst.FreeSelItemKindsList.Count; ++i) {
				this.ApplySelItem(CGameInfoStorage.Inst.FreeSelItemKindsList[i]);
				CGameInfoStorage.Inst.FreeSelItemKindsList.ExRemoveVal(CGameInfoStorage.Inst.FreeSelItemKindsList[i]);
			}

			for(int i = 0; i < CGameInfoStorage.Inst.SelItemKindsList.Count; ++i) {
				var stValInfo = new STValInfo(KCDefine.B_VAL_1_INT, EValType.INT);
				var stTargetInfo = new STTargetInfo(ETargetKinds.ITEM_NUMS, (int)CGameInfoStorage.Inst.SelItemKindsList[i], stValInfo);

				this.ApplySelItem(CGameInfoStorage.Inst.SelItemKindsList[i]);
				Func.Pay(CGameInfoStorage.Inst.PlayCharacterID, stTargetInfo);
			}
		}

		/** 레벨을 로드한다 */
		private void LoadLevel(CPopup a_oPopup, STEpisodeInfo a_stEpisodeInfo) {
            switch(CGameInfoStorage.Inst.PlayMode) {
				case EPlayMode.NORM: {
                    //Debug.Log(CodeManager.GetMethodName() + string.Format("{0} <? {1}", m_oEngine.currentLevel, CLevelInfoTable.Inst.levelCount));
                    //Debug.Log(CodeManager.GetMethodName() + string.Format("a_stEpisodeInfo.m_stIDInfo.m_nID01 : {0}", a_stEpisodeInfo.m_stIDInfo.m_nID01));
                    //Debug.Log(CodeManager.GetMethodName() + string.Format("Access.GetNumLevelClearInfos() : {0}", Access.GetNumLevelClearInfos(CGameInfoStorage.Inst.PlayCharacterID, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03)));
                    //Debug.Log(CodeManager.GetMethodName() + string.Format("PlayCharacterID : {0} / m_stIDInfo.m_nID02 : {1} / m_stIDInfo.m_nID03 : {2}", CGameInfoStorage.Inst.PlayCharacterID, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03));
					
                    // 레벨 로드가 가능 할 경우
					//if(a_stEpisodeInfo.m_stIDInfo.m_nID01 > KCDefine.B_IDX_INVALID && a_stEpisodeInfo.m_stIDInfo.m_nID01 <= Access.GetNumLevelClearInfos(CGameInfoStorage.Inst.PlayCharacterID, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03)) {
                    //  Func.SetupPlayEpisodeInfo(CGameInfoStorage.Inst.PlayCharacterID, a_stEpisodeInfo.m_stIDInfo.m_nID01, CGameInfoStorage.Inst.PlayMode, a_stEpisodeInfo.m_stIDInfo.m_nID02, a_stEpisodeInfo.m_stIDInfo.m_nID03);
                    
                    if (m_oEngine.currentLevel < 15)//TODO: CLevelInfoTable.Inst.levelCount
                    {
                        Debug.Log(CodeManager.GetMethodName() + string.Format("{0}", m_oEngine.currentLevel + 1));

                        Func.SetupPlayEpisodeInfo(CGameInfoStorage.Inst.PlayCharacterID, (int)CGameInfoStorage.Inst.PlayEpisodeInfo.ULevelID + 1, CGameInfoStorage.Inst.PlayMode);

#if ADS_MODULE_ENABLE
						Func.ShowFullscreenAds((a_oSender, a_bIsSuccess) => CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME));
#else
						CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME);
#endif // #if ADS_MODULE_ENABLE
					} else {
						this.LeavePlayLevel(a_oPopup);
					}

					break;
				}
				case EPlayMode.TUTORIAL: {
					break;
				}
				case EPlayMode.TEST: {
#if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE
					CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_LEVEL_EDITOR);
#endif // #if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE
					break;
				}
			}
		}

		/** 이전 레벨을 로드한다 */
		private void LoadPrevLevel(CPopup a_oPopup) {
			this.LoadLevel(a_oPopup, Access.GetPrevLevelEpisodeInfo(CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID01, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03));
		}

		/** 다음 레벨을 로드한다 */
		private void LoadNextLevel(CPopup a_oPopup) {
			this.LoadLevel(a_oPopup, Access.GetNextLevelEpisodeInfo(CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID01, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03));
		}

		/** 플레이 레벨을 재시도한다 */
		private void RetryPlayLevel(CPopup a_oPopup) {
#if ADS_MODULE_ENABLE
			Func.ShowFullscreenAds((a_oSender, a_bIsSuccess) => CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME));
#else
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME);
#endif // #if ADS_MODULE_ENABLE
		}

		/** 플레이 레벨을 제개한다 */
		private void ResumePlayLevel(CPopup a_oPopup) {
			a_oPopup?.Close();
		}

		/** 플레이 레벨을 이어한다 */
		private void ContinuePlayLevel(CPopup a_oPopup) {
			int nContinueTimes = m_oIntDict.GetValueOrDefault(EKey.CONTINUE_TIMES);
			m_oIntDict.ExReplaceVal(EKey.CONTINUE_TIMES, nContinueTimes + KCDefine.B_VAL_1_INT);
		}

		/** 플레이 레벨을 떠난다 */
		private void LeavePlayLevel(CPopup a_oPopup) {
            switch(CGameInfoStorage.Inst.PlayMode) {
				case EPlayMode.NORM:
                case EPlayMode.TUTORIAL: {
#if ADS_MODULE_ENABLE
			Func.ShowFullscreenAds((a_oSender, a_bIsSuccess) => CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MAIN));
#else
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MAIN);
#endif // #if ADS_MODULE_ENABLE
					break;
				}
				case EPlayMode.TEST: {
#if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE
					CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_LEVEL_EDITOR);
#endif // #if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE
					break;
				}
            }
		}

		/** 이어하기 팝업을 출력한다 */
		private void ShowContinuePopup() {
			Func.ShowContinuePopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CContinuePopup).Init(CContinuePopup.MakeParams(m_oIntDict.GetValueOrDefault(EKey.CONTINUE_TIMES), new Dictionary<CContinuePopup.ECallback, System.Action<CContinuePopup>>() {
					[CContinuePopup.ECallback.RETRY] = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.RETRY),
					[CContinuePopup.ECallback.CONTINUE] = (a_oPopupSender) => {this.OnReceivePopupResult(a_oPopupSender, EPopupResult.CONTINUE); GetContinueBonus();},
					[CContinuePopup.ECallback.LEAVE] = (a_oPopupSender) => this.ShowResultPopup(false)
				}));
			});
		}

		/** 결과 팝업을 출력한다 */
		private void ShowResultPopup(bool a_bIsClear) {
			Func.ShowResultPopup(this.PopupUIs, (a_oSender) => {
				var stRecordInfo = new STRecordInfo {
					m_bIsSuccess = a_bIsClear,
					m_nIntRecord = m_oEngine.RecordInfo.m_nIntRecord,
					m_dblRealRecord = m_oEngine.RecordInfo.m_dblRealRecord,
                    m_starCount = Engine.starCount
				};

                (a_oSender as CResultPopup).Init(CResultPopup.MakeParams(stRecordInfo, new Dictionary<CResultPopup.ECallback, System.Action<CResultPopup>>() {
                        [CResultPopup.ECallback.NEXT] = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.NEXT),
                        [CResultPopup.ECallback.RETRY] = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.RETRY),
                        [CResultPopup.ECallback.LEAVE] = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.LEAVE)
                    }));
			});
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

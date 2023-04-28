using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using UnityEngine.EventSystems;
using EnhancedUI.EnhancedScroller;

namespace MainScene {
	/** 서브 메인 씬 관리자 */
	public partial class CSubMainSceneManager : CMainSceneManager, IEnhancedScrollerDelegate {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			SEL_ID_INFO,
			LEVEL_SCROLLER_INFO,
			STAGE_SCROLLER_INFO,
			CHAPTER_SCROLLER_INFO,
			[HideInInspector] MAX_VAL
		}

		#region 변수
		private Dictionary<EKey, STIDInfo> m_oIDInfoDict = new Dictionary<EKey, STIDInfo>() {
			[EKey.SEL_ID_INFO] = STIDInfo.INVALID
		};

		/** =====> UI <===== */
		private Dictionary<EKey, STScrollerInfo> m_oScrollerInfoDict = new Dictionary<EKey, STScrollerInfo>();
		#endregion // 변수

		#region IEnhancedScrollerDelegate
		/** 셀 개수를 반환한다 */
		public int GetNumberOfCells(EnhancedScroller a_oSender) {
			// 레벨 스크롤러 일 경우
			if(m_oScrollerInfoDict[EKey.LEVEL_SCROLLER_INFO].m_oScroller == a_oSender) {
				return this.GetNumLevelScrollerCells(a_oSender);
			}

			return (m_oScrollerInfoDict[EKey.STAGE_SCROLLER_INFO].m_oScroller == a_oSender) ? this.GetNumStageScrollerCells(a_oSender) : this.GetNumChapterScrollerCells(a_oSender);
		}

		/** 셀 뷰 크기를 반환한다 */
		public float GetCellViewSize(EnhancedScroller a_oSender, int a_nDataIdx) {
			// 레벨 스크롤러 일 경우
			if(m_oScrollerInfoDict[EKey.LEVEL_SCROLLER_INFO].m_oScroller == a_oSender) {
				return (m_oScrollerInfoDict[EKey.LEVEL_SCROLLER_INFO].m_oScrollerCellView.transform as RectTransform).sizeDelta.y;
			}

			return (m_oScrollerInfoDict[EKey.STAGE_SCROLLER_INFO].m_oScroller == a_oSender) ? (m_oScrollerInfoDict[EKey.STAGE_SCROLLER_INFO].m_oScrollerCellView.transform as RectTransform).sizeDelta.y : (m_oScrollerInfoDict[EKey.CHAPTER_SCROLLER_INFO].m_oScrollerCellView.transform as RectTransform).sizeDelta.y;
		}

		/** 셀 뷰를 반환한다 */
		public EnhancedScrollerCellView GetCellView(EnhancedScroller a_oSender, int a_nDataIdx, int a_nCellIdx) {
			var oCallbackDict = new Dictionary<CScrollerCellView.ECallback, System.Action<CScrollerCellView, ulong>>() {
				[CScrollerCellView.ECallback.SEL] = this.OnReceiveSelCallback
			};

			/** 레벨 스크롤러 일 경우 */
			if(m_oScrollerInfoDict[EKey.LEVEL_SCROLLER_INFO].m_oScroller == a_oSender) {
				return this.CreateLevelScrollerCellView(a_oSender, a_nDataIdx, a_nCellIdx, oCallbackDict);
			}

			return (m_oScrollerInfoDict[EKey.STAGE_SCROLLER_INFO].m_oScroller == a_oSender) ? this.CreateStageScrollerCellView(a_oSender, a_nDataIdx, a_nCellIdx, oCallbackDict) : this.CreateChapterScrollerCellView(a_oSender, a_nDataIdx, a_nCellIdx, oCallbackDict);
		}
		#endregion // IEnhancedScrollerDelegate

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
#if CREATIVE_DIST_BUILD
				for(int i = 0; i < Access.NumChapterEpisodes; ++i) {
					for(int j = 0; j < Access.GetNumStageEpisodes(i); ++i) {
						for(int k = 0; k < Access.GetNumLevelEpisodes(j, i); ++k) {
							// 클리어 정보가 없을 경우
							if(Access.IsClearLevel(CGameInfoStorage.Inst.PlayCharacterID, k, j, i)) {
								CGameInfoStorage.Inst.AddLevelClearInfo(CGameInfoStorage.Inst.PlayCharacterID, Factory.MakeClearInfo(k, j, i));
							}

							var oLevelClearInfo = CGameInfoStorage.Inst.GetLevelClearInfo(k, j, i);
							oLevelClearInfo.NumSymbols = KCDefine.B_VAL_1_INT;
						}
					}
				}

				Access.SetItemTargetVal(CGameInfoStorage.Inst.PlayCharacterID, EItemKinds.GOODS_NORM_COINS, ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_NUMS, KCDefine.B_UNIT_DIGITS_PER_HUNDRED_THOUSAND);
				CGameInfoStorage.Inst.SaveGameInfo();
#endif // #if CREATIVE_DIST_BUILD

				this.SetupAwake();
                this.SubSetupAwake();
				CGameInfoStorage.Inst.ResetSelItems();
			}
		}

		/** 초기화 */
		public override void Start() {
			base.Start();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				this.SetupStart();
				this.UpdateUIsState();

				Func.PlayBGSnd(EResKinds.SND_BG_SCENE_MAIN_01);

#if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
				// 에디터가 유효 할 경우
				if(CCommonAppInfoStorage.Inst.IsEnableEditor) {
					//CCommonAppInfoStorage.Inst.SetEnableEditor(false);
					CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_LEVEL_EDITOR);
				}
#endif // #if UNITY_STANDALONE && (DEBUG || DEVELOPMENT_BUILD)
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
			}
		}

		/** 내비게이션 스택 이벤트를 수신했을 경우 */
		public override void OnReceiveNavStackEvent(ENavStackEvent a_eEvent) {
			base.OnReceiveNavStackEvent(a_eEvent);

			// 백 키 눌림 이벤트 일 경우
			if(a_eEvent == ENavStackEvent.BACK_KEY_DOWN) {
                if(IsTabMoving()) 
                    return;
                
                if(currentTab > -1)
                    this.CloseTabs();
                else
				    Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_QUIT_P_MSG), this.OnReceiveQuitPopupResult);
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
			}
		}

		/** 씬을 설정한다 */
		private void SetupAwake() {
			var ePlayMode = CGameInfoStorage.Inst.PlayMode;
			m_oIDInfoDict[EKey.SEL_ID_INFO] = (ePlayMode == EPlayMode.NORM && CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID01 > KCDefine.B_IDX_INVALID) ? CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo : new STIDInfo(KCDefine.B_VAL_0_INT);

			// 버튼을 설정한다
			/*CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
				(KCDefine.U_OBJ_N_PLAY_BTN, this.UIsBase, this.OnTouchPlayBtn),
				(KCDefine.U_OBJ_N_STORE_BTN, this.UIsBase, this.OnTouchStoreBtn),
				(KCDefine.U_OBJ_N_REVIEW_BTN, this.UIsBase, this.OnTouchReviewBtn),
				(KCDefine.U_OBJ_N_SETTINGS_BTN, this.UIsBase, this.OnTouchSettingsBtn)
			});*/

			// 스크롤 뷰를 설정한다
			CFunc.SetupScrollerInfos(new List<(EKey, string, GameObject, EnhancedScrollerCellView, IEnhancedScrollerDelegate)>() {
				(EKey.LEVEL_SCROLLER_INFO, KCDefine.U_OBJ_N_LEVEL_SCROLL_VIEW, this.UIsBase, CResManager.Inst.GetRes<GameObject>(KCDefine.MS_OBJ_P_LEVEL_SCROLLER_CELL_VIEW)?.GetComponentInChildren<EnhancedScrollerCellView>(), this),
				(EKey.STAGE_SCROLLER_INFO, KCDefine.U_OBJ_N_STAGE_SCROLL_VIEW, this.UIsBase, CResManager.Inst.GetRes<GameObject>(KCDefine.MS_OBJ_P_STAGE_SCROLLER_CELL_VIEW)?.GetComponentInChildren<EnhancedScrollerCellView>(), this),
				(EKey.CHAPTER_SCROLLER_INFO, KCDefine.U_OBJ_N_CHAPTER_SCROLL_VIEW, this.UIsBase, CResManager.Inst.GetRes<GameObject>(KCDefine.MS_OBJ_P_CHAPTER_SCROLLER_CELL_VIEW)?.GetComponentInChildren<EnhancedScrollerCellView>(), this)
			}, m_oScrollerInfoDict);

			this.SubAwake();
		}

		/** 씬을 설정한다 */
		private void SetupStart() {
			// 캐릭터 게임 정보가 존재 할 경우
			if(CGameInfoStorage.Inst.TryGetCharacterGameInfo(CGameInfoStorage.Inst.PlayCharacterID, out CCharacterGameInfo oCharacterGameInfo)) {
				// 일일 미션 리셋이 가능 할 경우
				if(Access.IsEnableResetDailyMission(CGameInfoStorage.Inst.PlayCharacterID)) {
					oCharacterGameInfo.PrevDailyMissionTime = System.DateTime.Today;
					oCharacterGameInfo.m_oCompleteDailyMissionKindsList.Clear();
				}

				// 무료 보상 획득이 가능 할 경우
				if(Access.IsEnableGetFreeReward(CGameInfoStorage.Inst.PlayCharacterID)) {
					oCharacterGameInfo.FreeRewardAcquireTimes = KCDefine.B_VAL_0_INT;
					oCharacterGameInfo.PrevFreeRewardTime = System.DateTime.Today.AddDays(-KCDefine.B_VAL_1_REAL);
				}

				CGameInfoStorage.Inst.SaveGameInfo();
			}

			// 업데이트가 가능 할 경우
			if(!CAppInfoStorage.Inst.IsIgnoreUpdate && !COptsInfoTable.Inst.EtcOptsInfo.m_bIsEnableTitleScene && CCommonAppInfoStorage.Inst.IsEnableUpdate()) {
				CAppInfoStorage.Inst.SetIgnoreUpdate(true);
				this.ExLateCallFunc((a_oSender) => Func.ShowAlertPopup(CStrTable.Inst.GetStr(KCDefine.ST_KEY_UPDATE_P_MSG), this.OnReceiveUpdatePopupResult));
			}

#if DAILY_REWARD_ENABLE
			// 일일 보상 획득이 가능 할 경우
			if(CGameInfoStorage.Inst.IsEnableGetDailyReward) {
				Func.ShowDailyRewardPopup(this.PopupUIs, (a_oSender) => (a_oSender as CDailyRewardPopup).Init());
			}
#endif // #if DAILY_REWARD_ENABLE

			this.SubStart();
		}

		/** UI 상태를 갱신한다 */
		private void UpdateUIsState() {
			this.SubUpdateUIsState();
		}

		/** 종료 팝업 결과를 수신했을 경우 */
		private void OnReceiveQuitPopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				a_oSender.SetIgnoreAni(true);
				this.ExLateCallFunc((a_oSender) => this.QuitApp());
			}
		}

		/** 업데이트 팝업 결과를 수신했을 경우 */
		private void OnReceiveUpdatePopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				Application.OpenURL(Access.StoreURL);
			}
		}

		/** 플레이 버튼을 눌렀을 경우 */
		private void OnTouchPlayBtn() {
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME);
		}

		/** 상점 버튼을 눌렀을 경우 */
		private void OnTouchStoreBtn() {
			GlobalDefine.OpenShop();
		}

		/** 평가 버튼을 눌렀을 경우 */
		private void OnTouchReviewBtn() {
			CUnityMsgSender.Inst.SendShowReviewMsg();
		}

		/** 설정 버튼을 눌렀을 경우 */
		private void OnTouchSettingsBtn() {
			Func.ShowSettingsPopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CSettingsPopup).Init();
			});
		}
		#endregion // 함수

		#region 조건부 함수
#if AB_TEST_ENABLE && (DEBUG || DEVELOPMENT_BUILD || PLAY_TEST_ENABLE)
		/** AB 테스트 UI 세트 버튼을 눌렀을 경우 */
		protected override void OnTouchABTUIsSetBtn(EUserType a_eUserType) {
			base.OnTouchABTUIsSetBtn(a_eUserType);

			// 유저 타입이 다를 경우
			if(CCommonUserInfoStorage.Inst.UserInfo.UserType != a_eUserType) {
				string oKey = (a_eUserType == EUserType.A) ? KCDefine.ST_KEY_C_SETUP_A_SET_MSG : KCDefine.ST_KEY_C_SETUP_B_SET_MSG;
				Func.ShowAlertPopup(CStrTable.Inst.GetStr(oKey), (a_oSender, a_bIsOK) => this.OnReceiveABSetPopupResult(a_oSender, a_bIsOK, a_eUserType));
			}
		}

		/** AB 세트 팝업 결과를 수신했을 경우 */
		protected override void OnReceiveABSetPopupResult(CAlertPopup a_oSender, bool a_bIsOK, EUserType a_eUserType) {
			base.OnReceiveABSetPopupResult(a_oSender, a_bIsOK, a_eUserType);

			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				// 에피소드 정보 테이블을 리셋한다 {
				CEpisodeInfoTable.Inst.LevelEpisodeInfoDict.Clear();
				CEpisodeInfoTable.Inst.StageEpisodeInfoDict.Clear();
				CEpisodeInfoTable.Inst.ChapterEpisodeInfoDict.Clear();

				CEpisodeInfoTable.Inst.LoadEpisodeInfos();
				// 에피소드 정보 테이블을 리셋한다 }

				// 레벨 정보 테이블을 리셋한다
				CLevelInfoTable.Inst.LevelInfoDictContainer.Clear();
				CLevelInfoTable.Inst.LoadLevelInfos();

				CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MAIN);
			}
		}
#endif // #if AB_TEST_ENABLE && (DEBUG || DEVELOPMENT_BUILD || PLAY_TEST_ENABLE)
		#endregion // 조건부 함수
	}

	/** 서브 메인 씬 관리자 - 스크롤러 셀 뷰 */
	public partial class CSubMainSceneManager : CMainSceneManager, IEnhancedScrollerDelegate {
		#region 함수
		/** 레벨 스크롤러 셀 개수를 반환한다 */
		private int GetNumLevelScrollerCells(EnhancedScroller a_oSender) {
			int nNumExtraCells = (Access.GetNumLevelEpisodes(m_oIDInfoDict[EKey.SEL_ID_INFO].m_nID02, m_oIDInfoDict[EKey.SEL_ID_INFO].m_nID03) % KDefine.MS_MAX_NUM_LEVELS_IN_ROW > KCDefine.B_VAL_0_INT) ? KCDefine.B_VAL_1_INT : KCDefine.B_VAL_0_INT;
			return (Access.GetNumLevelEpisodes(m_oIDInfoDict[EKey.SEL_ID_INFO].m_nID02, m_oIDInfoDict[EKey.SEL_ID_INFO].m_nID03) / KDefine.MS_MAX_NUM_LEVELS_IN_ROW) + nNumExtraCells;
		}

		/** 스테이지 스크롤러 셀 개수를 반환한다 */
		private int GetNumStageScrollerCells(EnhancedScroller a_oSender) {
			int nNumExtraCells = (Access.GetNumStageEpisodes(m_oIDInfoDict[EKey.SEL_ID_INFO].m_nID03) % KDefine.MS_MAX_NUM_STAGES_IN_ROW > KCDefine.B_VAL_0_INT) ? KCDefine.B_VAL_1_INT : KCDefine.B_VAL_0_INT;
			return (Access.GetNumStageEpisodes(m_oIDInfoDict[EKey.SEL_ID_INFO].m_nID03) / KDefine.MS_MAX_NUM_STAGES_IN_ROW) + nNumExtraCells;
		}

		/** 챕터 스크롤러 셀 개수를 반환한다 */
		private int GetNumChapterScrollerCells(EnhancedScroller a_oSender) {
			int nNumExtraCells = (Access.NumChapterEpisodes % KDefine.MS_MAX_NUM_CHAPTERS_IN_ROW > KCDefine.B_VAL_0_INT) ? KCDefine.B_VAL_1_INT : KCDefine.B_VAL_0_INT;
			return (Access.NumChapterEpisodes / KDefine.MS_MAX_NUM_CHAPTERS_IN_ROW) + nNumExtraCells;
		}

		/** 레벨 스크롤러 셀 뷰를 생성한다 */
		private EnhancedScrollerCellView CreateLevelScrollerCellView(EnhancedScroller a_oSender, int a_nDataIdx, int a_nCellIdx, Dictionary<CScrollerCellView.ECallback, System.Action<CScrollerCellView, ulong>> a_oCallbackDict) {
			var oScrollerCellView = a_oSender.GetCellView(m_oScrollerInfoDict[EKey.LEVEL_SCROLLER_INFO].m_oScrollerCellView) as CLevelScrollerCellView;
			oScrollerCellView.Init(CLevelScrollerCellView.MakeParams(CFactory.MakeULevelID(a_nDataIdx * KDefine.MS_MAX_NUM_LEVELS_IN_ROW, m_oIDInfoDict[EKey.SEL_ID_INFO].m_nID02, m_oIDInfoDict[EKey.SEL_ID_INFO].m_nID03), a_oSender, a_oCallbackDict));

			return oScrollerCellView;
		}

		/** 스테이지 스크롤러 셀 뷰를 생성한다 */
		private EnhancedScrollerCellView CreateStageScrollerCellView(EnhancedScroller a_oSender, int a_nDataIdx, int a_nCellIdx, Dictionary<CScrollerCellView.ECallback, System.Action<CScrollerCellView, ulong>> a_oCallbackDict) {
			var oScrollerCellView = a_oSender.GetCellView(m_oScrollerInfoDict[EKey.STAGE_SCROLLER_INFO].m_oScrollerCellView) as CStageScrollerCellView;
			oScrollerCellView.Init(CStageScrollerCellView.MakeParams(CFactory.MakeUStageID(a_nDataIdx * KDefine.MS_MAX_NUM_STAGES_IN_ROW, m_oIDInfoDict[EKey.SEL_ID_INFO].m_nID03), a_oSender, a_oCallbackDict));

			return oScrollerCellView;
		}

		/** 챕터 스크롤러 셀 뷰를 생성한다 */
		private EnhancedScrollerCellView CreateChapterScrollerCellView(EnhancedScroller a_oSender, int a_nDataIdx, int a_nCellIdx, Dictionary<CScrollerCellView.ECallback, System.Action<CScrollerCellView, ulong>> a_oCallbackDict) {
			var oScrollerCellView = a_oSender.GetCellView(m_oScrollerInfoDict[EKey.CHAPTER_SCROLLER_INFO].m_oScrollerCellView) as CChapterScrollerCellView;
			oScrollerCellView.Init(CChapterScrollerCellView.MakeParams(CFactory.MakeUChapterID(a_nDataIdx * KDefine.MS_MAX_NUM_CHAPTERS_IN_ROW), a_oSender, a_oCallbackDict));

			return oScrollerCellView;
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

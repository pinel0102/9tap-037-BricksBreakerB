#if SCRIPT_TEMPLATE_ONLY
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
		private Dictionary<EKey, bool> m_oBoolDict = new Dictionary<EKey, bool>();
		private Dictionary<EKey, int> m_oIntDict = new Dictionary<EKey, int>();
		private Dictionary<EKey, ERewardAdsUIs> m_oRewardAdsUIsDict = new Dictionary<EKey, ERewardAdsUIs>();
		private Dictionary<EKey, SpriteRenderer> m_oSpriteDict = new Dictionary<EKey, SpriteRenderer>();

		/** =====> 객체 <===== */
		[SerializeField] private List<GameObject> m_oRewardAdsUIsList = new List<GameObject>();
#endregion // 변수

#region 함수
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
				if(CSceneLoader.Inst.PrevActiveSceneName.Equals(KCDefine.B_SCENE_N_LEVEL_EDITOR)) {
					Func.ShowLeavePopup(this.OnReceiveLeavePopupResult);
				} else {
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

		/** 그만두기 팝업 결과를 수신했을 경우 */
		private void OnReceiveLeavePopupResult(CAlertPopup a_oSender, bool a_bIsOK) {
			// 확인 버튼을 눌렀을 경우
			if(a_bIsOK) {
				CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_LEVEL_EDITOR);
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
			this.ShowResultPopup(false);
		}

		/** 정지 버튼을 눌렀을 경우 */
		private void OnTouchPauseBtn() {
			Func.ShowPausePopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CPausePopup).Init(CPausePopup.MakeParams(new Dictionary<CPausePopup.ECallback, System.Action<CPausePopup>>() {
					[CPausePopup.ECallback.LEAVE] = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.LEAVE)
				}));
			});
		}

		/** 설정 버튼을 눌렀을 경웅 */
		private void OnTouchSettingsBtn() {
			Func.ShowSettingsPopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CSettingsPopup).Init();
			});
		}

		/** 광고 버튼을 눌렀을 경우 */
		private void OnTouchAdsBtn(ERewardAdsUIs a_eRewardAdsUIs) {
			m_oRewardAdsUIsDict.ExReplaceVal(EKey.SEL_REWARD_ADS_UIS, a_eRewardAdsUIs);

#if ADS_MODULE_ENABLE
			Func.ShowRewardAds(this.OnCloseRewardAds);
#endif // #if ADS_MODULE_ENABLE
		}

		/** 선택 아이템을 적용한다 */
		private void ApplySelItems() {
			for(int i = 0; i < CGameInfoStorage.Inst.FreeSelItemKindsList.Count; ++i) {
				this.ApplySelItem(CGameInfoStorage.Inst.FreeSelItemKindsList[i]);
				CGameInfoStorage.Inst.FreeSelItemKindsList.ExRemoveVal(CGameInfoStorage.Inst.FreeSelItemKindsList[i]);
			}

			for(int i = 0; i < CGameInfoStorage.Inst.SelItemKindsList.Count; ++i) {
				var stValInfo = new STValInfo(KCDefine.B_VAL_1_INT, EValType.INT);

				this.ApplySelItem(CGameInfoStorage.Inst.SelItemKindsList[i]);
				Func.Pay(CGameInfoStorage.Inst.PlayCharacterID, new STTargetInfo(ETargetKinds.ITEM_NUMS, (int)CGameInfoStorage.Inst.SelItemKindsList[i], stValInfo));
			}
		}

		/** 레벨을 로드한다 */
		private void LoadLevel(CPopup a_oPopup, STEpisodeInfo a_stEpisodeInfo) {
			switch(CGameInfoStorage.Inst.PlayMode) {
				case EPlayMode.NORM: {
					// 레벨 로드가 가능 할 경우
					if(a_stEpisodeInfo.m_stIDInfo.m_nID01 > KCDefine.B_IDX_INVALID && a_stEpisodeInfo.m_stIDInfo.m_nID01 <= Access.GetNumLevelClearInfos(CGameInfoStorage.Inst.PlayCharacterID, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03)) {
						Func.SetupPlayEpisodeInfo(CGameInfoStorage.Inst.PlayCharacterID, a_stEpisodeInfo.m_stIDInfo.m_nID01, CGameInfoStorage.Inst.PlayMode, a_stEpisodeInfo.m_stIDInfo.m_nID02, a_stEpisodeInfo.m_stIDInfo.m_nID03);

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
					CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_LEVEL_EDITOR);
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
#if ADS_MODULE_ENABLE
			Func.ShowFullscreenAds((a_oSender, a_bIsSuccess) => CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MAIN));
#else
			CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MAIN);
#endif // #if ADS_MODULE_ENABLE
		}

		/** 이어하기 팝업을 출력한다 */
		private void ShowContinuePopup() {
			Func.ShowContinuePopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CContinuePopup).Init(CContinuePopup.MakeParams(m_oIntDict.GetValueOrDefault(EKey.CONTINUE_TIMES), new Dictionary<CContinuePopup.ECallback, System.Action<CContinuePopup>>() {
					[CContinuePopup.ECallback.RETRY] = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.RETRY),
					[CContinuePopup.ECallback.CONTINUE] = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.CONTINUE),
					[CContinuePopup.ECallback.LEAVE] = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.LEAVE)
				}));
			});
		}

		/** 결과 팝업을 출력한다 */
		private void ShowResultPopup(bool a_bIsClear) {
			Func.ShowResultPopup(this.PopupUIs, (a_oSender) => {
				var stRecordInfo = new STRecordInfo {
					m_bIsSuccess = a_bIsClear,
					m_nIntRecord = m_oEngine.RecordInfo.m_nIntRecord,
					m_dblRealRecord = m_oEngine.RecordInfo.m_dblRealRecord
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
#endif // #if SCRIPT_TEMPLATE_ONLY

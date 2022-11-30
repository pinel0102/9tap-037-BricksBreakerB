using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using EnhancedUI.EnhancedScroller;

namespace MainScene {
	/** 서브 메인 씬 관리자 */
	public partial class CSubMainSceneManager : CMainSceneManager, IEnhancedScrollerDelegate {
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
			}
		}

		/** 씬을 설정한다 */
		private void SetupAwake() {
			var ePlayMode = CGameInfoStorage.Inst.PlayMode;
			m_oIDInfoDict.ExReplaceVal(EKey.SEL_ID_INFO, (ePlayMode == EPlayMode.NORM && CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID01 > KCDefine.B_IDX_INVALID) ? CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo : new STIDInfo(KCDefine.B_VAL_0_INT));

			// 버튼을 설정한다
			CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
				(KCDefine.U_OBJ_N_PLAY_BTN, this.UIsBase, this.OnTouchPlayBtn),
				(KCDefine.U_OBJ_N_STORE_BTN, this.UIsBase, this.OnTouchStoreBtn),
				(KCDefine.U_OBJ_N_REVIEW_BTN, this.UIsBase, this.OnTouchReviewBtn),
				(KCDefine.U_OBJ_N_SETTINGS_BTN, this.UIsBase, this.OnTouchSettingsBtn)
			}, false);

			// 스크롤 뷰를 설정한다
			CFunc.SetupScrollerInfos(new List<(EKey, string, GameObject, EnhancedScrollerCellView, IEnhancedScrollerDelegate)>() {
				(EKey.LEVEL_SCROLLER_INFO, KCDefine.U_OBJ_N_LEVEL_SCROLL_VIEW, this.UIsBase, CResManager.Inst.GetRes<GameObject>(KCDefine.MS_OBJ_P_LEVEL_SCROLLER_CELL_VIEW)?.GetComponentInChildren<EnhancedScrollerCellView>(), this),
				(EKey.STAGE_SCROLLER_INFO, KCDefine.U_OBJ_N_STAGE_SCROLL_VIEW, this.UIsBase, CResManager.Inst.GetRes<GameObject>(KCDefine.MS_OBJ_P_STAGE_SCROLLER_CELL_VIEW)?.GetComponentInChildren<EnhancedScrollerCellView>(), this),
				(EKey.CHAPTER_SCROLLER_INFO, KCDefine.U_OBJ_N_CHAPTER_SCROLL_VIEW, this.UIsBase, CResManager.Inst.GetRes<GameObject>(KCDefine.MS_OBJ_P_CHAPTER_SCROLLER_CELL_VIEW)?.GetComponentInChildren<EnhancedScrollerCellView>(), this)
			}, m_oScrollerInfoDict);

#if AB_TEST_ENABLE && (DEBUG || DEVELOPMENT_BUILD || PLAY_TEST_ENABLE)
			this.SetupABTestUIs();
#endif // #if AB_TEST_ENABLE && (DEBUG || DEVELOPMENT_BUILD || PLAY_TEST_ENABLE)

			#region 추가
			this.SubSetupAwake();
			#endregion // 추가
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
					oCharacterGameInfo.PrevFreeRewardTime = System.DateTime.Today;
				}

				CGameInfoStorage.Inst.SaveGameInfo();
			}

			// 업데이트가 가능 할 경우
			if(!CAppInfoStorage.Inst.IsIgnoreUpdate && !COptsInfoTable.Inst.EtcOptsInfo.m_bIsEnableTitleScene && CCommonAppInfoStorage.Inst.IsEnableUpdate()) {
				CAppInfoStorage.Inst.SetIgnoreUpdate(true);
				this.ExLateCallFunc((a_oSender) => Func.ShowUpdatePopup(this.OnReceiveUpdatePopupResult));
			}

#if DAILY_REWARD_ENABLE
			// 일일 보상 획득이 가능 할 경우
			if(CGameInfoStorage.Inst.IsEnableGetDailyReward) {
				Func.ShowDailyRewardPopup(this.PopupUIs, (a_oSender) => (a_oSender as CDailyRewardPopup).Init());
			}
#endif // #if DAILY_REWARD_ENABLE

			#region 추가
			this.SubSetupStart();
			#endregion // 추가
		}

		/** UI 상태를 갱신한다 */
		private void UpdateUIsState() {
			#region 추가
			this.SubUpdateUIsState();
			#endregion // 추가
		}
		#endregion // 함수
	}

	/** 서브 메인 씬 관리자 - 서브 */
	public partial class CSubMainSceneManager : CMainSceneManager, IEnhancedScrollerDelegate {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

#if DEBUG || DEVELOPMENT_BUILD
		/** 서브 테스트 UI */
		[System.Serializable]
		private struct STSubTestUIs {
			// Do Something
		}
#endif // #if DEBUG || DEVELOPMENT_BUILD

		#region 변수
		/** =====> UI <===== */
#if DEBUG || DEVELOPMENT_BUILD
		[SerializeField] private STSubTestUIs m_stSubTestUIs;
#endif // #if DEBUG || DEVELOPMENT_BUILD
		#endregion // 변수

		#region 프로퍼티

		#endregion // 프로퍼티

		#region 함수
		/** 제거 되었을 경우 */
		public override void OnDestroy() {
			base.OnDestroy();

			try {
				// 앱이 실행 중 일 경우
				if(CSceneManager.IsAppRunning) {
					// Do Something
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CSubMainSceneManager.OnDestroy Exception: {oException.Message}");
			}
		}

		/** 씬을 설정한다 */
		private void SubSetupAwake() {
#if DEBUG || DEVELOPMENT_BUILD
			this.SubSetupTestUIs();
#endif // #if DEBUG || DEVELOPMENT_BUILD
		}

		/** 씬을 설정한다 */
		private void SubSetupStart() {
			// Do Something
		}

		/** UI 상태를 갱신한다 */
		private void SubUpdateUIsState() {
#if DEBUG || DEVELOPMENT_BUILD
			this.SubUpdateTestUIsState();
#endif // #if DEBUG || DEVELOPMENT_BUILD
		}
		#endregion // 함수

		#region 조건부 함수
#if DEBUG || DEVELOPMENT_BUILD
		/** 테스트 UI 를 설정한다 */
		private void SubSetupTestUIs() {
			// Do Something
		}

		/** 테스트 UI 상태를 갱신한다 */
		private void SubUpdateTestUIsState() {
			// Do Something
		}
#endif // #if DEBUG || DEVELOPMENT_BUILD
		#endregion // 조건부 함수
	}

	/** 서브 메인 씬 관리자 - 스크롤러 셀 뷰 */
	public partial class CSubMainSceneManager : CMainSceneManager, IEnhancedScrollerDelegate {
		#region 함수
		/** 선택 콜백을 수신했을 경우 */
		private void OnReceiveSelCallback(CScrollerCellView a_oSender, ulong a_nID) {
			// Do Something
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

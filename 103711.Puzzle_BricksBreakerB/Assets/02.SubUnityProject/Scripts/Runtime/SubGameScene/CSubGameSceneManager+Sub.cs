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

		/** 씬을 설정한다 */
		private void SetupAwake() {
			this.SetupEngine();
			this.SetupRewardAdsUIs();

			// 버튼을 설정한다
			CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
				(KCDefine.U_OBJ_N_PAUSE_BTN, this.UIsBase, this.OnTouchPauseBtn),
				(KCDefine.U_OBJ_N_SETTINGS_BTN, this.UIsBase, this.OnTouchSettingsBtn)
			}, false);

			// 비율을 설정한다 {
			bool bIsValid01 = !float.IsNaN(m_oEngine.SelGridInfo.m_stScale.x) && !float.IsInfinity(m_oEngine.SelGridInfo.m_stScale.x);
			bool bIsValid02 = !float.IsNaN(m_oEngine.SelGridInfo.m_stScale.y) && !float.IsInfinity(m_oEngine.SelGridInfo.m_stScale.y);
			bool bIsValid03 = !float.IsNaN(m_oEngine.SelGridInfo.m_stScale.z) && !float.IsInfinity(m_oEngine.SelGridInfo.m_stScale.z);

			this.ObjRoot.transform.localScale = (bIsValid01 && bIsValid02 && bIsValid03) ? m_oEngine.SelGridInfo.m_stScale : Vector3.one;
			// 비율을 설정한다 }

			// 스프라이트를 설정한다 {
			var oSpriteInfoDict = new Dictionary<EKey, (Sprite, STSortingOrderInfo)>() {
				[EKey.BG_SPRITE] = (Access.GetBGSprite(KDefine.GS_TEX_P_FMT_BG, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID01, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03), KDefine.GS_SORTING_OI_BG),
				[EKey.UP_BG_SPRITE] = (Access.GetBGSprite(KDefine.GS_TEX_P_FMT_TOP_BG, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID01, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03), KDefine.GS_SORTING_OI_TOP_BG),
				[EKey.DOWN_BG_SPRITE] = (Access.GetBGSprite(KDefine.GS_TEX_P_FMT_BOTTOM_BG, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID01, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03), KDefine.GS_SORTING_OI_BOTTOM_BG)
			};

			CFunc.SetupComponents(new List<(EKey, string, GameObject, GameObject)>() {
				(EKey.BG_SPRITE, $"{EKey.BG_SPRITE}", this.Objs, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_SPRITE)),
				(EKey.UP_BG_SPRITE, $"{EKey.UP_BG_SPRITE}", this.Objs, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_SPRITE)),
				(EKey.DOWN_BG_SPRITE, $"{EKey.DOWN_BG_SPRITE}", this.Objs, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_SPRITE))
			}, m_oSpriteDict);

			foreach(var stKeyVal in m_oSpriteDict) {
				stKeyVal.Value.drawMode = SpriteDrawMode.Tiled;
				stKeyVal.Value.tileMode = SpriteTileMode.Continuous;
				stKeyVal.Value.sprite = oSpriteInfoDict.GetValueOrDefault(stKeyVal.Key).Item1;
				stKeyVal.Value.ExSetSortingOrder(oSpriteInfoDict.GetValueOrDefault(stKeyVal.Key).Item2);
			}

			m_oSpriteDict.GetValueOrDefault(EKey.BG_SPRITE).size = new Vector3(Mathf.Max(this.ScreenWidth, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.x), Mathf.Max(CSceneManager.CanvasSize.y, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.y), KCDefine.B_VAL_0_REAL);
			m_oSpriteDict.GetValueOrDefault(EKey.BG_SPRITE).transform.localScale = Vector3.one;
			m_oSpriteDict.GetValueOrDefault(EKey.BG_SPRITE).transform.localPosition = Vector3.zero;

			m_oSpriteDict.GetValueOrDefault(EKey.UP_BG_SPRITE).size = new Vector3(Mathf.Max(this.ScreenWidth, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.x), m_oSpriteDict.GetValueOrDefault(EKey.UP_BG_SPRITE).sprite.rect.height, KCDefine.B_VAL_0_REAL);
			m_oSpriteDict.GetValueOrDefault(EKey.UP_BG_SPRITE).transform.localScale = Vector3.one;
			m_oSpriteDict.GetValueOrDefault(EKey.UP_BG_SPRITE).transform.localPosition = new Vector3(KCDefine.B_VAL_0_REAL, Mathf.Max(this.ScreenHeight / KCDefine.B_VAL_2_REAL, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.y / KCDefine.B_VAL_2_REAL) + (m_oSpriteDict.GetValueOrDefault(EKey.UP_BG_SPRITE).sprite.rect.height / KCDefine.B_VAL_2_REAL), KCDefine.B_VAL_0_REAL);

			m_oSpriteDict.GetValueOrDefault(EKey.DOWN_BG_SPRITE).size = new Vector3(Mathf.Max(this.ScreenWidth, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.x), m_oSpriteDict.GetValueOrDefault(EKey.DOWN_BG_SPRITE).sprite.rect.height, KCDefine.B_VAL_0_REAL);
			m_oSpriteDict.GetValueOrDefault(EKey.DOWN_BG_SPRITE).transform.localScale = Vector3.one;
			m_oSpriteDict.GetValueOrDefault(EKey.DOWN_BG_SPRITE).transform.localPosition = new Vector3(KCDefine.B_VAL_0_REAL, -(Mathf.Max((this.ScreenHeight / KCDefine.B_VAL_2_REAL) - NSEngine.KDefine.E_OFFSET_BOTTOM, (CGameInfoStorage.Inst.PlayEpisodeInfo.m_stSize.y / KCDefine.B_VAL_2_REAL) - NSEngine.KDefine.E_OFFSET_BOTTOM) + (m_oSpriteDict.GetValueOrDefault(EKey.DOWN_BG_SPRITE).sprite.rect.height / KCDefine.B_VAL_2_REAL)), KCDefine.B_VAL_0_REAL);
			// 스프라이트를 설정한다 }

#region 추가
			this.SubSetupAwake();
#endregion // 추가
		}

		/** 씬을 설정한다 */
		private void SetupStart() {
			this.ApplySelItems();
			CGameInfoStorage.Inst.ResetSelItems();

#region 추가
			this.SubSetupStart();
#endregion // 추가
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
			m_oEngine.Init(NSEngine.CEngine.MakeParams(this.ItemRoot, this.SkillRoot, this.ObjRoot, this.FXRoot, oCallbackDict01, oCallbackDict02));
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

#region 추가
			this.SubUpdateUIsState();
#endregion // 추가
		}

		/** 보상 광고 UI 상태를 갱신한다 */
		private void UpdateRewardAdsUIsState() {
			for(int i = 0; i < m_oRewardAdsUIsList.Count; ++i) {
				m_oRewardAdsUIsList[i]?.SetActive(CGameInfoStorage.Inst.PlayEpisodeInfo.ULevelID + KCDefine.B_VAL_1_INT >= KDefine.GS_MIN_LEVEL_ENABLE_REWARD_ADS_WATCH);
			}
		}
#endregion // 함수
	}

	/** 서브 게임 씬 관리자 - 서브 */
	public partial class CSubGameSceneManager : CGameSceneManager {
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
		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				m_oEngine.OnUpdate(a_fDeltaTime);

				// UI 갱신이 필요 할 경우
				if(m_oBoolDict.GetValueOrDefault(EKey.IS_UPDATE_UIS_STATE)) {
					m_oBoolDict.ExReplaceVal(EKey.IS_UPDATE_UIS_STATE, false);
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
				CFunc.ShowLogWarning($"CSubGameSceneManager.OnDestroy Exception: {oException.Message}");
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
			this.ExLateCallFunc((a_oSender) => {
				m_oEngine.SetEnableRunning(true);
				m_oEngine.SetState(NSEngine.CEngine.EState.PLAY);
				m_oEngine.SelPlayerObj.GetController<NSEngine.CEController>().SetState(NSEngine.CEController.EState.IDLE, true);
			}, KCDefine.B_VAL_1_REAL / KCDefine.B_VAL_2_REAL);
		}

		/** UI 상태를 갱신한다 */
		private void SubUpdateUIsState() {
#if DEBUG || DEVELOPMENT_BUILD
			this.SubUpdateTestUIsState();
#endif // #if DEBUG || DEVELOPMENT_BUILD
		}

		/** 획득했을 경우 */
		private void OnReceiveAcquireCallback(NSEngine.CEngine a_oSender, Dictionary<ulong, STTargetInfo> a_oAcquireTargetInfoDict) {
			// Do Something
		}
		
		/** 선택 아이템을 적용한다 */
		private void ApplySelItem(EItemKinds a_eItemKinds) {
			// Do Something
		}

		/** 터치 시작 이벤트를 처리한다 */
		private void HandleTouchBeginEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// Do Something
		}

		/** 터치 이동 이벤트를 처리한다 */
		private void HandleTouchMoveEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// Do Something
		}

		/** 터치 종료 이벤트를 처리한다 */
		private void HandleTouchEndEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// Do Something
		}
#endregion // 함수

#region 조건부 함수
#if UNITY_EDITOR
		/** 기즈모를 그린다 */
		public override void OnDrawGizmos() {
			base.OnDrawGizmos();

			// 메인 카메라가 존재 할 경우
			if(CSceneManager.IsExistsMainCamera) {
				// Do Something
			}
		}
#endif // #if UNITY_EDITOR

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

#if ADS_MODULE_ENABLE
		/** 보상 광고가 닫혔을 경우 */
		private void OnCloseRewardAds(CAdsManager a_oSender, STAdsRewardInfo a_stAdsRewardInfo, bool a_bIsSuccess) {
			// 광고를 시청했을 경우
			if(a_bIsSuccess) {
				// Do Something
			}
		}
#endif // #if ADS_MODULE_ENABLE
#endregion // 조건부 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

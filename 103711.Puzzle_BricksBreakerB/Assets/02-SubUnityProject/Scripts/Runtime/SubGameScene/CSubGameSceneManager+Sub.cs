using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using UnityEngine.EventSystems;

namespace GameScene {
	/** 서브 게임 씬 관리자 - 서브 */
	public partial class CSubGameSceneManager : CGameSceneManager {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			DROP_ALL_BALLS_BTN,
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
        [Header("★ [Reference] SubGameSceneManager Sub")]
		[SerializeField] private STSubTestUIs m_stSubTestUIs;
#endif // #if DEBUG || DEVELOPMENT_BUILD

		[SerializeField] private List<GameObject> m_oIdleUIsList = new List<GameObject>();
		[SerializeField] private List<GameObject> m_oShootUIsList = new List<GameObject>();

        public TMPro.TMP_Text levelText;
        private const string formatLevelText = "Level {0}";
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
					this.UpdateUIsState();
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
		private void SubAwake() {
#if DEBUG || DEVELOPMENT_BUILD
			this.SubSetupTestUIs();
#endif // #if DEBUG || DEVELOPMENT_BUILD

			// 버튼을 설정한다
			CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
				($"{ESubKey.DROP_ALL_BALLS_BTN}", this.UIsBase, this.OnTouchDropAllBallsBtn)
			});

            levelText.text = string.Format(formatLevelText, m_oEngine.currentLevel);
            
            m_oEngine.RefreshActiveCells();
            m_oEngine.CheckDeadLine(true);
		}

		/** 씬을 설정한다 */
		private void SubStart() {
			this.ExLateCallFunc((a_oSender) => {
				m_oEngine.SetEnableRunning(true);
				m_oEngine.SetState(NSEngine.CEngine.EState.PLAY);

#if NEVER_USE_THIS
				// FIXME: dante (비활성 처리 - 필요 시 활성 및 사용 가능)
				m_oEngine.SelPlayerObj.GetController<NSEngine.CEController>().SetState(NSEngine.CEController.EState.IDLE, true);
#endif // #if NEVER_USE_THIS

				//m_oEngine.SetPlayState(NSEngine.CEngine.EPlayState.IDLE);
			}, KCDefine.B_VAL_1_REAL / KCDefine.B_VAL_2_REAL);
		}

		/** UI 상태를 갱신한다 */
		private void SubUpdateUIsState() {
#if DEBUG || DEVELOPMENT_BUILD
			this.SubUpdateTestUIsState();
#endif // #if DEBUG || DEVELOPMENT_BUILD

			for(int i = 0; i < m_oIdleUIsList.Count; ++i) {
				m_oIdleUIsList[i].SetActive(m_oEngine.PlayState == NSEngine.CEngine.EPlayState.IDLE);
			}

			for(int i = 0; i < m_oShootUIsList.Count; ++i) {
				m_oShootUIsList[i].SetActive(m_oEngine.PlayState == NSEngine.CEngine.EPlayState.SHOOT);
			}
		}

		/** 획득했을 경우 */
		private void OnReceiveAcquireCallback(NSEngine.CEngine a_oSender, Dictionary<ulong, STTargetInfo> a_oAcquireTargetInfoDict) {
			// Do Something
		}

		/** 모든 볼 떨어뜨리기 버튼을 눌렀을 경우 */
		public void OnTouchDropAllBallsBtn() {
            HideShootUIs();

			m_oEngine.DropAllBalls();
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
				var stPrevColor = Gizmos.color;
				var stMainCameraPos = CSceneManager.ActiveSceneMainCamera.transform.position;
				var stPivotPos = stMainCameraPos + new Vector3(KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_0_REAL, this.PlaneDistance);

				try {
					var oGridPosList = new List<Vector3>() {
						stPivotPos + this.ObjRootPivotPos + new Vector3(NSEngine.Access.MaxGridSize.x / -KCDefine.B_VAL_2_REAL, NSEngine.Access.MaxGridSize.y / -KCDefine.B_VAL_2_REAL, 0.0f) * CAccess.ResolutionUnitScale,
						stPivotPos + this.ObjRootPivotPos + new Vector3(NSEngine.Access.MaxGridSize.x / -KCDefine.B_VAL_2_REAL, NSEngine.Access.MaxGridSize.y / KCDefine.B_VAL_2_REAL, 0.0f) * CAccess.ResolutionUnitScale,
						stPivotPos + this.ObjRootPivotPos + new Vector3(NSEngine.Access.MaxGridSize.x / KCDefine.B_VAL_2_REAL, NSEngine.Access.MaxGridSize.y / KCDefine.B_VAL_2_REAL, 0.0f) * CAccess.ResolutionUnitScale,
						stPivotPos + this.ObjRootPivotPos + new Vector3(NSEngine.Access.MaxGridSize.x / KCDefine.B_VAL_2_REAL, NSEngine.Access.MaxGridSize.y / -KCDefine.B_VAL_2_REAL, 0.0f) * CAccess.ResolutionUnitScale
					};

					for(int i = 0; i < oGridPosList.Count; ++i) {
						int nIdx01 = (i + KCDefine.B_VAL_0_INT) % oGridPosList.Count;
						int nIdx02 = (i + KCDefine.B_VAL_1_INT) % oGridPosList.Count;

						Gizmos.color = Color.magenta;
						Gizmos.DrawLine(oGridPosList[nIdx01], oGridPosList[nIdx02]);
					}
				} finally {
					Gizmos.color = stPrevColor;
				}
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

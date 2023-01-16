#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using UnityEngine.EventSystems;

namespace NSEngine {
	/** 서브 엔진 */
	public partial class CEngine : CComponent {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

		#region 변수
		
		#endregion // 변수

		#region 프로퍼티

		#endregion // 프로퍼티

		#region 함수
		/** 초기화 */
		private void SubAwake() {
			// Do Something
		}

		/** 초기화 */
		private void SubInit() {
			// Do Something
		}
		
		/** 제거 되었을 경우 */
		private void SubOnDestroy() {
			try {
				// 앱이 실행 중 일 경우
				if(CSceneManager.IsAppRunning) {
					// Do Something
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CEngine.SubOnDestroy Exception: {oException.Message}");
			}
		}

		/** 상태를 갱신한다 */
		private void SubOnUpdate(float a_fDeltaTime) {
			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				// Do Something
			}
		}

		/** 플레이 상태를 처리한다 */
		private void HandlePlayState(float a_fDeltaTime) {
			CFunc.UpdateComponents(this.ItemList, a_fDeltaTime);
			CFunc.UpdateComponents(this.SkillList, a_fDeltaTime);
			CFunc.UpdateComponents(this.FXList, a_fDeltaTime);
			CFunc.UpdateComponents(this.PlayerObjList, a_fDeltaTime);
			CFunc.UpdateComponents(this.EnemyObjList, a_fDeltaTime);

			// 실행 중 일 경우
			if(m_oBoolDict[EKey.IS_RUNNING]) {
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

		/** 회피 엔진 객체 이벤트를 처리한다 */
		private void HandleAvoidEObjEvent(CEObjComponent a_oSender, string a_oParams) {
			// Do Something
		}

		/** 피해 엔진 객체 이벤트를 처리한다 */
		private void HandleDamageEObjEvent(CEObjComponent a_oSender, string a_oParams) {
			// Do Something
		}

		/** 치명 피해 엔진 객체 이벤트를 처리한다 */
		private void HandleCriticalDamageEObjEvent(CEObjComponent a_oSender, string a_oParams) {
			// Do Something
		}

		/** 터치 시작 이벤트를 처리한다 */
		private void HandleTouchBeginEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 구동 모드 일 경우
			if(m_oBoolDict[EKey.IS_RUNNING]) {
				var stPos = a_oEventData.ExGetLocalPos(this.Params.m_oObjRoot, CSceneManager.ActiveSceneManager.ScreenSize);
				var stIdx = stPos.ExToIdx(this.SelGridInfo.m_stPivotPos, Access.CellSize);

				// 인덱스가 유효 할 경우
				if(this.CellObjLists.ExIsValidIdx(stIdx)) {
					// Do Something
				}
			}
		}

		/** 터치 이동 이벤트를 처리한다 */
		private void HandleTouchMoveEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 구동 모드 일 경우
			if(m_oBoolDict[EKey.IS_RUNNING]) {
				var stPos = a_oEventData.ExGetLocalPos(this.Params.m_oObjRoot, CSceneManager.ActiveSceneManager.ScreenSize);
				var stIdx = stPos.ExToIdx(this.SelGridInfo.m_stPivotPos, Access.CellSize);

				// 인덱스가 유효 할 경우
				if(this.CellObjLists.ExIsValidIdx(stIdx)) {
					// Do Something
				}
			}
		}

		/** 터치 종료 이벤트를 처리한다 */
		private void HandleTouchEndEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			// 구동 모드 일 경우
			if(m_oBoolDict[EKey.IS_RUNNING]) {
				var stPos = a_oEventData.ExGetLocalPos(this.Params.m_oObjRoot, CSceneManager.ActiveSceneManager.ScreenSize);
				var stIdx = stPos.ExToIdx(this.SelGridInfo.m_stPivotPos, Access.CellSize);

				// 인덱스가 유효 할 경우
				if(this.CellObjLists.ExIsValidIdx(stIdx)) {
					// Do Something
				}
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
#endif // #if SCRIPT_TEMPLATE_ONLY

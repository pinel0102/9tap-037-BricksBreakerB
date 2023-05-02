using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 서브 엔진 - 접근 */
	public partial class CEngine : CComponent {
		#region 함수
		/** 클리어 여부를 검사한다 */
		public bool IsClear() {
			bool bIsClear = true;

			for(int i = 0; i < this.CellObjLists.GetLength(KCDefine.B_VAL_0_INT); ++i) {
				for(int j = 0; j < this.CellObjLists.GetLength(KCDefine.B_VAL_1_INT); ++j) {
					for(int k = 0; k < this.CellObjLists[i, j].Count; ++k) {
						// 셀이 존재 할 경우
                        CEObj target = this.CellObjLists[i, j][k];							
                        if(target != null && target.Params.m_stObjInfo.m_bIsClearTarget)
                        {
                            bIsClear = false;
                            goto EXIT_FOR;
                        }
					}
				}
			}

EXIT_FOR:
			return bIsClear;
		}

		/** 조준 가능 여부를 검사한다 */
		public bool IsEnableAiming(Vector3 a_stPos) {
			var stIdx = a_stPos.ExToIdx(this.SelGridInfo.m_stPivotPos, Access.CellSize);
			var stDirection = a_stPos - this.SelBallObj.transform.localPosition;

			return this.PlayState == EPlayState.IDLE && !isShooting && !isLevelClear && !isGridMoving && !isExplosionAll && 
                    (this.SelGridInfo.m_stAimBounds.Contains(a_stPos) && stDirection.y.ExIsGreate(KCDefine.B_VAL_0_REAL) && a_stPos.y.ExIsGreateEquals(GlobalDefine.aimYPositionMin / SelGridInfo.m_stScale.x) || 
                    (!this.SelGridInfo.m_stAimBounds.Contains(a_stPos) && subGameSceneManager.isAimLayerOn && subGameSceneManager.isAimDragOn));
		}

		/** 발사 가능 여부를 검사한다 */
		public bool IsEnableShoot(Vector3 a_stPos) {
			var stDirection01 = a_stPos - this.SelBallObj.transform.localPosition;
			var stDirection02 = Vector3.right * Mathf.Sign(stDirection01.x);

			return this.IsEnableAiming(a_stPos) && Vector3.Angle(stDirection01, stDirection02).ExIsGreateEquals(KDefine.E_MIN_ANGLE_AIMING);
		}

		/** 상태를 변경한다 */
		public void SetState(EState a_eState, bool a_bIsForce = false) {
			// 강제 변경 모드 일 경우
			if(a_bIsForce) {
				this.State = a_eState;
			} else {
				this.State = (!m_oStateCheckerDict.TryGetValue(a_eState, out System.Func<bool> oStateChecker) || oStateChecker()) ? a_eState : this.State;
			}
		}

		/** 플레이 상태를 변경한다 */
		public void SetPlayState(EPlayState a_eState) {
			this.PlayState = a_eState;
			CSceneManager.GetSceneManager<GameScene.CSubGameSceneManager>(KCDefine.B_SCENE_N_GAME).SetEnableUpdateUIsState(true);

			// 대기 상태 일 경우
			if(this.PlayState == EPlayState.IDLE) {
				this.SelBallObj.NumText.text = GlobalDefine.GetBallText(BallObjList.Count - DeleteBallList.Count, DeleteBallList.Count);
                this.currentShootCount = 0;
			}
		}

		/** 적 객체를 탐색한다 */
		public CEObj FindEnemyObj(Vector3 a_stPos, float a_fDistance = float.MaxValue) {
			var oEnemyObj = this.EnemyObjList.ExGetVal(KCDefine.B_VAL_0_INT, null);

			for(int i = 1; i < this.EnemyObjList.Count; ++i) {
				float fDistance = (a_stPos - oEnemyObj.transform.localPosition).sqrMagnitude;
				oEnemyObj = fDistance.ExIsLessEquals((a_stPos - this.EnemyObjList[i].transform.localPosition).sqrMagnitude) ? oEnemyObj : this.EnemyObjList[i];
			}

			return (oEnemyObj != null && (a_stPos - oEnemyObj.transform.localPosition).sqrMagnitude.ExIsLessEquals(Mathf.Pow(a_fDistance, KCDefine.B_VAL_2_REAL))) ? oEnemyObj : null;
		}

		/** 적 객체를 탐색한다 */
		public List<CEObj> FindEnemyObjs(Vector3 a_stPos, List<CEObj> a_oOutEnemyObjList, float a_fDistance = float.MaxValue) {
			a_oOutEnemyObjList = a_oOutEnemyObjList ?? new List<CEObj>();

			for(int i = 0; i < this.EnemyObjList.Count; ++i) {
				float fDistance = (a_stPos - this.EnemyObjList[i].transform.localPosition).sqrMagnitude;

				// 범위 안에 존재 할 경우
				if(fDistance.ExIsLessEquals(Mathf.Pow(a_fDistance, KCDefine.B_VAL_2_REAL))) {
					a_oOutEnemyObjList.ExAddVal(this.EnemyObjList[i]);
				}
			}

			return a_oOutEnemyObjList;
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

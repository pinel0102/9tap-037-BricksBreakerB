#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.Linq;

namespace NSEngine {
	/** 엔진 - 접근 */
	public partial class CEngine : CComponent {
		#region 함수
		/** 메인 카메라 위치를 반환한다 */
		public Vector3 GetMainCameraPos() {
			var stPos = this.SelPlayerObj.transform.localPosition;
			var stSize = this.CameraEpisodeSize.ExToLocal(this.Params.m_oObjRoot, false);
			var stOffset = KDefine.E_OFFSET_MAIN_CAMERA.ExToLocal(this.Params.m_oObjRoot, false);
			var stScreenSize = CSceneManager.ActiveSceneManager.ScreenSize.ExToLocal(this.Params.m_oObjRoot, false);

			float fMainCameraPosX = Mathf.Clamp(stPos.x, stSize.x / -KCDefine.B_VAL_2_REAL, stSize.x / KCDefine.B_VAL_2_REAL);
			float fMainCameraPosY = Mathf.Clamp(stPos.y + stOffset.y, (stSize.y / -KCDefine.B_VAL_2_REAL) - (stScreenSize.y / KCDefine.B_VAL_3_REAL), stSize.y / KCDefine.B_VAL_2_REAL);

			return new Vector3(fMainCameraPosX, fMainCameraPosY, CSceneManager.ActiveSceneMainCamera.transform.position.ExToLocal(this.Params.m_oObjRoot).z);
		}

		/** 구동 여부를 변경한다 */
		public void SetEnableRunning(bool a_bIsRunning) {
			m_oBoolDict[EKey.IS_RUNNING] = a_bIsRunning;
		}

		/** 플레이어 객체 자동 제어 여부를 변경한다 */
		public void SetEnablePlayerObjAutoControl(bool a_bIsAutoControl) {
			this.SelPlayerObj.GetController<CEPlayerObjController>().SetEnableAutoControl(a_bIsAutoControl);
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

		/** 셀 객체를 탐색한다 */
		public CEObj FindCellObj(EObjKinds a_eObjKinds, Vector3Int a_stIdx) {
			return this.CellObjLists.ExGetVal(a_stIdx, null)?.ExGetVal((a_oCellObj) => a_oCellObj.Params.m_stObjInfo.m_eObjKinds == a_eObjKinds, null);
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

		/** 셀 객체를 탐색한다 */
		public List<CEObj> FindCellObjs(EObjKinds a_eObjKinds, Vector3Int a_stIdx) {
			return this.CellObjLists.ExGetVal(a_stIdx, null)?.ExGetVals((a_oCellObj) => a_oCellObj.Params.m_stObjInfo.m_eObjKinds == a_eObjKinds);
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

		/** 최상단 셀 객체를 탐색한다 */
		public CEObj FindTopCellObj(EObjKinds a_eObjKinds, Vector3Int a_stIdx) {
			var oCellObjList = this.FindCellObjs(a_eObjKinds, a_stIdx);
			return oCellObjList.ExIsValid() ? oCellObjList.Last() : null;
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

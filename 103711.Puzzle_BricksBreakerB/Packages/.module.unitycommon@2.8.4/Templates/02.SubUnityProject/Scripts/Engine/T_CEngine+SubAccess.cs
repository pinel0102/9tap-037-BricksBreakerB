#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 엔진 - 접근 */
	public partial class CEngine : CComponent {
#region 함수
		
#endregion // 함수
	}

	/** 서브 엔진 - 접근 */
	public partial class CEngine : CComponent {
#region 함수
		/** 상태를 변경한다 */
		public void SetState(EState a_eState, bool a_bIsForce = false) {
			// 강제 변경 모드 일 경우
			if(a_bIsForce) {
				this.State = a_eState;
			} else {
				this.State = (!m_oStateCheckerDict.TryGetValue(a_eState, out System.Func<bool> oStateChecker) || oStateChecker()) ? a_eState : this.State;
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
#endif // #if SCRIPT_TEMPLATE_ONLY

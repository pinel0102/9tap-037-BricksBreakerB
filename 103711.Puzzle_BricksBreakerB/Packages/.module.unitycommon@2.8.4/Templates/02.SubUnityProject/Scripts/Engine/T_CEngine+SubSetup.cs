#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 엔진 - 설정 */
	public partial class CEngine : CComponent {
		#region 함수
		
		#endregion // 함수
	}

	/** 서브 엔진 - 설정 */
	public partial class CEngine : CComponent {
		#region 함수
		/** 엔진을 설정한다 */
		private void SubSetupAwake() {
			// Do Something
		}

		/** 엔진을 설정한다 */
		private void SubSetupEngine() {
			// 객체 풀을 설정한다
			CSceneManager.ActiveSceneManager.AddObjsPool(KDefine.E_KEY_CELL_OBJ_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_CELL_OBJ), this.Params.m_oObjRoot, KCDefine.U_SIZE_OBJS_POOL_01, false);
			CSceneManager.ActiveSceneManager.AddObjsPool(KDefine.E_KEY_PLAYER_OBJ_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_PLAYER_OBJ), this.Params.m_oObjRoot, KCDefine.B_VAL_1_INT, false);
			CSceneManager.ActiveSceneManager.AddObjsPool(KDefine.E_KEY_ENEMY_OBJ_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_ENEMY_OBJ), this.Params.m_oObjRoot, KCDefine.U_SIZE_OBJS_POOL_01, false);
		}

		/** 셀을 설정한다 */
		private void SetupCell(STCellInfo a_stCellInfo, STGridInfo a_stGridInfo) {
			var oCellObjDictContainer = new Dictionary<EObjType, List<CEObj>>();

			foreach(var stKeyVal in a_stCellInfo.m_oObjKindsDictContainer) {
				var oCellObjList = new List<CEObj>();

				for(int i = 0; i < stKeyVal.Value.Count; ++i) {
					// Do Something
				}

				oCellObjDictContainer.TryAdd(stKeyVal.Key, oCellObjList);
			}

			m_oCellObjDictContainers[a_stCellInfo.m_stIdx.y, a_stCellInfo.m_stIdx.x] = oCellObjDictContainer;
		}

		/** 그리드 라인을 설정한다 */
		private void SetupGridLine() {
			// Do Something
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

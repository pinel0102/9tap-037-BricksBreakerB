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

		/** 셀 객체를 탐색한다 */
		public CEObj FindCellObj(EObjKinds a_eObjKinds, Vector3Int a_stIdx) {
			return this.CellObjLists.ExGetVal(a_stIdx, null)?.ExGetVal((a_oCellObj) => a_oCellObj.Params.m_stObjInfo.m_eObjKinds == a_eObjKinds, null);
		}

		/** 셀 객체를 탐색한다 */
		public List<CEObj> FindCellObjs(EObjKinds a_eObjKinds, Vector3Int a_stIdx) {
			return this.CellObjLists.ExGetVal(a_stIdx, null)?.ExGetVals((a_oCellObj) => a_oCellObj.Params.m_stObjInfo.m_eObjKinds == a_eObjKinds);
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

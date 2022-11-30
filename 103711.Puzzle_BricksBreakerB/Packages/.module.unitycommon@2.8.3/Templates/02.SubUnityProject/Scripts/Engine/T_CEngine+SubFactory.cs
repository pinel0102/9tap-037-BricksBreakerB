#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 엔진 - 팩토리 */
	public partial class CEngine : CComponent {
#region 함수
		
#endregion // 함수
	}

	/** 서브 엔진 - 팩토리 */
	public partial class CEngine : CComponent {
#region 함수
		/** 셀 객체를 생성한다 */
		public CEObj CreateCellObj(STObjInfo a_stObjInfo, CObjTargetInfo a_oObjTargetInfo, CEObjComponent a_oOwner = null, bool a_bIsEnableController = true) {
			var oObj = CSceneManager.ActiveSceneManager.SpawnObj<CEObj>(KDefine.E_OBJ_N_CELL_OBJ, KDefine.E_KEY_CELL_OBJ_OBJS_POOL);
			var oController = a_bIsEnableController ? oObj.gameObject.ExAddComponent<CECellObjController>() : null;

			oObj.Init(CEObj.MakeParams(this, a_stObjInfo, a_oObjTargetInfo, oController, KDefine.E_KEY_CELL_OBJ_OBJS_POOL));
			oController?.Init(CECellObjController.MakeParams(this));

			this.SetupEObjComponent(oObj, a_oOwner, oController);
			return oObj;
		}

		/** 플레이어 객체를 생성한다 */
		public CEObj CreatePlayerObj(STObjInfo a_stObjInfo, CObjTargetInfo a_oObjTargetInfo, CEObjComponent a_oOwner = null, bool a_bIsEnableController = true) {
			var oObj = CSceneManager.ActiveSceneManager.SpawnObj<CEObj>(KDefine.E_OBJ_N_PLAYER_OBJ, KDefine.E_KEY_PLAYER_OBJ_OBJS_POOL);
			var oController = a_bIsEnableController ? oObj.gameObject.ExAddComponent<CEPlayerObjController>() : null;

			oObj.Init(CEObj.MakeParams(this, a_stObjInfo, a_oObjTargetInfo, oController, KDefine.E_KEY_PLAYER_OBJ_OBJS_POOL));
			oController?.Init(CEPlayerObjController.MakeParams(this));

			this.SetupEObjComponent(oObj, a_oOwner, oController);
			return oObj;
		}

		/** 적 객체를 생성한다 */
		public CEObj CreateEnemyObj(STObjInfo a_stObjInfo, CObjTargetInfo a_oObjTargetInfo, CEObjComponent a_oOwner = null, bool a_bIsEnableController = true) {
			var oObj = CSceneManager.ActiveSceneManager.SpawnObj<CEObj>(KDefine.E_OBJ_N_ENEMY_OBJ, KDefine.E_KEY_ENEMY_OBJ_OBJS_POOL);
			var oController = a_bIsEnableController ? oObj.gameObject.ExAddComponent<CEEnemyObjController>() : null;

			oObj.Init(CEObj.MakeParams(this, a_stObjInfo, a_oObjTargetInfo, oController, KDefine.E_KEY_ENEMY_OBJ_OBJS_POOL));
			oController?.Init(CEEnemyObjController.MakeParams(this));

			this.SetupEObjComponent(oObj, a_oOwner, oController);
			return oObj;
		}

		/** 셀 객체를 제거한다 */
		public void RemoveCellObj(CEObj a_oObj, float a_fDelay = KCDefine.B_VAL_0_REAL, bool a_bIsEnableAssert = true) {
			var oCellObjList = (a_oObj != null) ? this.FindCellObjs(a_oObj.Params.m_stObjInfo.ObjType, a_oObj.CellIdx) : null;
			CAccess.Assert(!a_bIsEnableAssert || (a_oObj != null && oCellObjList != null && a_oObj.CellIdx.ExIsValidIdx() && a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey.ExIsValid()));

			// 셀 객체가 존재 할 경우
			if(a_oObj != null && oCellObjList != null && a_oObj.CellIdx.ExIsValidIdx() && a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey.ExIsValid()) {
				oCellObjList.ExRemoveVal(a_oObj);
				CFactory.RemoveObj(a_oObj.Params.m_stBaseParams.m_oController, false, false);
				CSceneManager.ActiveSceneManager.DespawnObj(a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey, a_oObj.gameObject, a_fDelay);
			}
		}

		/** 플레이어 객체를 제거한다 */
		public void RemovePlayerObj(CEObj a_oObj, float a_fDelay = KCDefine.B_VAL_0_REAL, bool a_bIsEnableAssert = true) {
			CAccess.Assert(!a_bIsEnableAssert || (a_oObj != null && a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey.ExIsValid()));

			// 플레이어 객체가 존재 할 경우
			if(a_oObj != null && a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey.ExIsValid()) {
				this.PlayerObjList.ExRemoveVal(a_oObj);
				CFactory.RemoveObj(a_oObj.Params.m_stBaseParams.m_oController, false, false);
				CSceneManager.ActiveSceneManager.DespawnObj(a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey, a_oObj.gameObject, a_fDelay);
			}
		}

		/** 적 객체를 제거한다 */
		public void RemoveEnemyObj(CEObj a_oObj, float a_fDelay = KCDefine.B_VAL_0_REAL, bool a_bIsEnableAssert = true) {
			CAccess.Assert(!a_bIsEnableAssert || (a_oObj != null && a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey.ExIsValid()));

			// 적 객체가 존재 할 경우
			if(a_oObj != null && a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey.ExIsValid()) {
				this.EnemyObjList.ExRemoveVal(a_oObj);
				CFactory.RemoveObj(a_oObj.Params.m_stBaseParams.m_oController, false, false);
				CSceneManager.ActiveSceneManager.DespawnObj(a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey, a_oObj.gameObject, a_fDelay);
			}
		}
#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

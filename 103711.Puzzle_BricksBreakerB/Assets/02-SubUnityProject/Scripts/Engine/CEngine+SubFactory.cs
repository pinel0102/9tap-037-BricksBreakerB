using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 서브 엔진 - 팩토리 */
	public partial class CEngine : CComponent {
		#region 함수
        /** 셀 객체를 생성한다 */
		public CEObj CreateCellObj(STObjInfo a_stObjInfo, CObjTargetInfo a_oObjTargetInfo, CEObjComponent a_oOwner = null, bool a_bIsEnableController = true) {
			var oObj = CSceneManager.ActiveSceneManager.SpawnObj<CEObj>(KDefine.E_OBJ_N_CELL_OBJ, KDefine.E_KEY_CELL_OBJ_OBJS_POOL);
			var oController = a_bIsEnableController ? oObj.gameObject.ExAddComponent<CECellObjController>() : null;

			oObj.Init(CEObj.MakeParams(this, a_stObjInfo, a_oObjTargetInfo, oController, KDefine.E_KEY_CELL_OBJ_OBJS_POOL));
			oController?.Init(CECellObjController.MakeParams(this), a_stObjInfo.m_eObjKinds);

			this.SetupEObjComponent(oObj, a_oOwner, oController);
			return oObj;
		}

        /** 아이템을 생성한다 */
		public CEItem CreateItemObj(STItemInfo a_stItemInfo, CItemTargetInfo a_oItemTargetInfo, CEObjComponent a_oOwner = null, bool a_bIsEnableController = true) {
			var oItem = CSceneManager.ActiveSceneManager.SpawnObj<CEItem>(KDefine.E_OBJ_N_ITEM, KDefine.E_KEY_ITEM_OBJS_POOL);
			var oController = a_bIsEnableController ? oItem.gameObject.ExAddComponent<CEItemController>() : null;

			oItem.Init(CEItem.MakeParams(this, a_stItemInfo, a_oItemTargetInfo, oController, KDefine.E_KEY_ITEM_OBJS_POOL));
			oController?.Init(CEItemController.MakeParams(this));

			this.SetupEObjComponent(oItem, a_oOwner, oController);
			return oItem;
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
			var oCellObjList = (a_oObj != null) ? this.CellObjLists.ExGetVal(a_oObj.CellIdx, null) : null;
			CAccess.Assert(!a_bIsEnableAssert || (a_oObj != null && oCellObjList != null && a_oObj.CellIdx.ExIsValidIdx() && a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey.ExIsValid()));

			// 셀 객체가 존재 할 경우
			if(a_oObj != null && oCellObjList != null && a_oObj.CellIdx.ExIsValidIdx() && a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey.ExIsValid()) {
				oCellObjList.ExRemoveVal(a_oObj);
				CFunc.RemoveObj(a_oObj.Params.m_stBaseParams.m_oController, false, false);
				CSceneManager.ActiveSceneManager.DespawnObj(a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey, a_oObj.gameObject, a_fDelay);
			}
		}

        /** 아이템을 제거한다 */
		public void RemoveItemObj(CEItem a_oItem, float a_fDelay = KCDefine.B_VAL_0_REAL, bool a_bIsEnableAssert = true) {
			CAccess.Assert(!a_bIsEnableAssert || (a_oItem != null && a_oItem.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey.ExIsValid()));

			// 아이템이 존재 할 경우
			if(a_oItem != null && a_oItem.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey.ExIsValid()) {
				this.ItemList.ExRemoveVal(a_oItem);
				CFunc.RemoveObj(a_oItem.Params.m_stBaseParams.m_oController, false, false);
				CSceneManager.ActiveSceneManager.DespawnObj(a_oItem.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey, a_oItem.gameObject, a_fDelay);
			}
		}

		/** 플레이어 객체를 제거한다 */
		public void RemovePlayerObj(CEObj a_oObj, float a_fDelay = KCDefine.B_VAL_0_REAL, bool a_bIsEnableAssert = true) {
			CAccess.Assert(!a_bIsEnableAssert || (a_oObj != null && a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey.ExIsValid()));

			// 플레이어 객체가 존재 할 경우
			if(a_oObj != null && a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey.ExIsValid()) {
				this.PlayerObjList.ExRemoveVal(a_oObj);
				CFunc.RemoveObj(a_oObj.Params.m_stBaseParams.m_oController, false, false);
				CSceneManager.ActiveSceneManager.DespawnObj(a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey, a_oObj.gameObject, a_fDelay);
			}
		}

		/** 적 객체를 제거한다 */
		public void RemoveEnemyObj(CEObj a_oObj, float a_fDelay = KCDefine.B_VAL_0_REAL, bool a_bIsEnableAssert = true) {
			CAccess.Assert(!a_bIsEnableAssert || (a_oObj != null && a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey.ExIsValid()));

			// 적 객체가 존재 할 경우
			if(a_oObj != null && a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey.ExIsValid()) {
				this.EnemyObjList.ExRemoveVal(a_oObj);
				CFunc.RemoveObj(a_oObj.Params.m_stBaseParams.m_oController, false, false);
				CSceneManager.ActiveSceneManager.DespawnObj(a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey, a_oObj.gameObject, a_fDelay);
			}
		}
		#endregion // 함수
	}

	/** 서브 엔진 - 추가 팩토리 */
	public partial class CEngine : CComponent {
		#region 함수
		/** 공 객체를 생성한다 */
		public CEObj CreateBallObj(int _index, STObjInfo a_stObjInfo, CObjTargetInfo a_oObjTargetInfo, CEObjComponent a_oOwner = null, bool a_bIsEnableController = true) {
			var oObj = CSceneManager.ActiveSceneManager.SpawnObj<CEObj>(KDefine.E_OBJ_N_BALL_OBJ, KDefine.E_KEY_BALL_OBJ_OBJS_POOL);
			var oController = a_bIsEnableController ? oObj.gameObject.ExAddComponent<CEBallObjController>() : null;

			oObj.Init(CEObj.MakeParams(this, a_stObjInfo, a_oObjTargetInfo, oController, KDefine.E_KEY_BALL_OBJ_OBJS_POOL));
			oObj.SetSpriteColor(a_stObjInfo.m_eObjKinds);

            oController?.Init(oObj, CEBallObjController.MakeParams(this), _index);

			this.SetupEObjComponent(oObj, a_oOwner, oController);
			return oObj;
		}

		/** 공 객체를 제거한다 */
		public void RemoveBallObj(CEObj a_oObj, float a_fDelay = KCDefine.B_VAL_0_REAL, bool a_bIsEnableAssert = true) {
			CAccess.Assert(!a_bIsEnableAssert || (a_oObj != null && a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey.ExIsValid()));

			// 공 객체가 존재 할 경우
			if(a_oObj != null && a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey.ExIsValid()) {
				this.BallObjList.ExRemoveVal(a_oObj);
				CFunc.RemoveObj(a_oObj.Params.m_stBaseParams.m_oController, false, false);
				CSceneManager.ActiveSceneManager.DespawnObj(a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey, a_oObj.gameObject, a_fDelay);
			}
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

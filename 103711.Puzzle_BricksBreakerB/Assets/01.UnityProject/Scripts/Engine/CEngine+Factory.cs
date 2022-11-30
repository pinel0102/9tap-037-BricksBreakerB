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
		/** 아이템을 생성한다 */
		public CEItem CreateItem(STItemInfo a_stItemInfo, CItemTargetInfo a_oItemTargetInfo, CEObjComponent a_oOwner = null, bool a_bIsEnableController = true) {
			var oItem = CSceneManager.ActiveSceneManager.SpawnObj<CEItem>(KDefine.E_OBJ_N_ITEM, KDefine.E_KEY_ITEM_OBJS_POOL);
			var oController = a_bIsEnableController ? oItem.gameObject.ExAddComponent<CEItemController>() : null;

			oItem.Init(CEItem.MakeParams(this, a_stItemInfo, a_oItemTargetInfo, oController, KDefine.E_KEY_ITEM_OBJS_POOL));
			oController?.Init(CEItemController.MakeParams(this));

			this.SetupEObjComponent(oItem, a_oOwner, oController);
			return oItem;
		}

		/** 스킬을 생성한다 */
		public CESkill CreateSkill(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo, CEObjComponent a_oOwner = null, bool a_bIsEnableController = true) {
			var oSkill = CSceneManager.ActiveSceneManager.SpawnObj<CESkill>(KDefine.E_OBJ_N_SKILL, KDefine.E_KEY_SKILL_OBJS_POOL);
			var oController = a_bIsEnableController ? oSkill.gameObject.ExAddComponent<CESkillController>() : null;

			oSkill.Init(CESkill.MakeParams(this, a_stSkillInfo, a_oSkillTargetInfo, oController, KDefine.E_KEY_SKILL_OBJS_POOL));
			oController?.Init(CESkillController.MakeParams(this));

			this.SetupEObjComponent(oSkill, a_oOwner, oController);
			return oSkill;
		}

		/** 객체를 생성한다 */
		public CEObj CreateObj(STObjInfo a_stObjInfo, CObjTargetInfo a_oObjTargetInfo, CEObjComponent a_oOwner = null, bool a_bIsEnableController = true) {
			var oObj = CSceneManager.ActiveSceneManager.SpawnObj<CEObj>(KDefine.E_OBJ_N_OBJ, KDefine.E_KEY_OBJ_OBJS_POOL);
			var oController = a_bIsEnableController ? oObj.gameObject.ExAddComponent<CEObjController>() : null;

			oObj.Init(CEObj.MakeParams(this, a_stObjInfo, a_oObjTargetInfo, oController, KDefine.E_KEY_OBJ_OBJS_POOL));
			oController?.Init(CEObjController.MakeParams(this));

			this.SetupEObjComponent(oObj, a_oOwner, oController);
			return oObj;
		}

		/** 효과를 생성한다 */
		public CEFX CreateFX(STFXInfo a_stFXInfo, CEObjComponent a_oOwner = null, bool a_bIsEnableController = true) {
			var oFX = CSceneManager.ActiveSceneManager.SpawnObj<CEFX>(KDefine.E_OBJ_N_FX, KDefine.E_KEY_FX_OBJS_POOL);
			var oController = a_bIsEnableController ? oFX.gameObject.ExAddComponent<CEFXController>() : null;

			oFX.Init(CEFX.MakeParams(this, a_stFXInfo, oController, KDefine.E_KEY_FX_OBJS_POOL));
			oController?.Init(CEFXController.MakeParams(this));

			this.SetupEObjComponent(oFX, a_oOwner, oController);
			return oFX;
		}

		/** 아이템을 제거한다 */
		public void RemoveItem(CEItem a_oItem, float a_fDelay = KCDefine.B_VAL_0_REAL, bool a_bIsEnableAssert = true) {
			CAccess.Assert(!a_bIsEnableAssert || (a_oItem != null && a_oItem.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey.ExIsValid()));

			// 아이템이 존재 할 경우
			if(a_oItem != null && a_oItem.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey.ExIsValid()) {
				this.ItemList.ExRemoveVal(a_oItem);
				CFactory.RemoveObj(a_oItem.Params.m_stBaseParams.m_oController, false, false);
				CSceneManager.ActiveSceneManager.DespawnObj(a_oItem.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey, a_oItem.gameObject, a_fDelay);
			}
		}

		/** 스킬을 제거한다 */
		public void RemoveSkill(CESkill a_oSkill, float a_fDelay = KCDefine.B_VAL_0_REAL, bool a_bIsEnableAssert = true) {
			CAccess.Assert(!a_bIsEnableAssert || (a_oSkill != null && a_oSkill.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey.ExIsValid()));

			// 스킬이 존재 할 경우
			if(a_oSkill != null && a_oSkill.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey.ExIsValid()) {
				this.SkillList.ExRemoveVal(a_oSkill);
				CFactory.RemoveObj(a_oSkill.Params.m_stBaseParams.m_oController, false, false);
				CSceneManager.ActiveSceneManager.DespawnObj(a_oSkill.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey, a_oSkill.gameObject, a_fDelay);
			}
		}

		/** 객체를 제거한다 */
		public void RemoveObj(CEObj a_oObj, float a_fDelay = KCDefine.B_VAL_0_REAL, bool a_bIsEnableAssert = true) {
			CAccess.Assert(!a_bIsEnableAssert || (a_oObj != null && a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey.ExIsValid()));

			// 객체가 존재 할 경우
			if(a_oObj != null && a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey.ExIsValid()) {
				this.ObjList.ExRemoveVal(a_oObj);
				CFactory.RemoveObj(a_oObj.Params.m_stBaseParams.m_oController, false, false);
				CSceneManager.ActiveSceneManager.DespawnObj(a_oObj.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey, a_oObj.gameObject, a_fDelay);
			}
		}

		/** 효과를 제거한다 */
		public void RemoveFX(CEFX a_oFX, float a_fDelay = KCDefine.B_VAL_0_REAL, bool a_bIsEnableAssert = true) {
			CAccess.Assert(!a_bIsEnableAssert || (a_oFX != null && a_oFX.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey.ExIsValid()));

			// 효과가 존재 할 경우
			if(a_oFX != null && a_oFX.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey.ExIsValid()) {
				this.FXList.ExRemoveVal(a_oFX);
				CFactory.RemoveObj(a_oFX.Params.m_stBaseParams.m_oController, false, false);
				CSceneManager.ActiveSceneManager.DespawnObj(a_oFX.Params.m_stBaseParams.m_stBaseParams.m_oObjsPoolKey, a_oFX.gameObject, a_fDelay);
			}
		}

		/** 엔진 객체 컴포넌트를 설정한다 */
		private void SetupEObjComponent(CEObjComponent a_oEObjComponent, CComponent a_oOwner, CEController a_oController) {
			a_oEObjComponent.SetOwner(a_oOwner);
			a_oEObjComponent.Params.m_oCallbackDict.TryAdd(CEObjComponent.ECallback.ENGINE_OBJ_EVENT, this.OnReceiveEObjEvent);

			a_oController?.SetOwner(a_oEObjComponent);
		}

		/** 엔진 객체 컴포넌트를 제거한다 */
		private void RemoveEObjComponent(CEObjComponent a_oEObjComponent) {
			switch(a_oEObjComponent.Params.m_stBaseParams.m_oObjsPoolKey) {
				case KDefine.E_KEY_ITEM_OBJS_POOL: this.RemoveItem(a_oEObjComponent as CEItem); break;
				case KDefine.E_KEY_SKILL_OBJS_POOL: this.RemoveSkill(a_oEObjComponent as CESkill); break;
				case KDefine.E_KEY_OBJ_OBJS_POOL: this.RemoveObj(a_oEObjComponent as CEObj); break;
				case KDefine.E_KEY_FX_OBJS_POOL: this.RemoveFX(a_oEObjComponent as CEFX); break;
				case KDefine.E_KEY_CELL_OBJ_OBJS_POOL: this.RemoveCellObj(a_oEObjComponent as CEObj); break;
				case KDefine.E_KEY_PLAYER_OBJ_OBJS_POOL: this.RemovePlayerObj(a_oEObjComponent as CEObj); break;
				case KDefine.E_KEY_ENEMY_OBJ_OBJS_POOL: this.RemoveEnemyObj(a_oEObjComponent as CEObj); break;
			}
		}
#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

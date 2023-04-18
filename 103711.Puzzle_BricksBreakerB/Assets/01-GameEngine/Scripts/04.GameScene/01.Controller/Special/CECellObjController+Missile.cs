using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
	/** 서브 셀 객체 제어자 */
	public partial class CECellObjController : CEObjController {

        private void GetSpecial_Missile(EObjKinds kindsType, EObjKinds kinds, int _ATK = KCDefine.B_VAL_1_INT)
        {
            //Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>{0}</color>", kinds));

            List<CEObj> excludeList = new List<CEObj>();
            excludeList.Add(this.GetOwner<CEObj>());
            
            switch(kinds)
            {
                case EObjKinds.SPECIAL_BRICKS_MISSILE_01: Missile(KCDefine.B_VAL_1_INT, excludeList); break;
                case EObjKinds.SPECIAL_BRICKS_MISSILE_02: Missile(KCDefine.B_VAL_4_INT, excludeList); break;
                default: break;
            }

            this.SetHideReserved();
        }

        ///<Summary>미사일.</Summary>
        private void Missile(int targetCount, List<CEObj> excludeList)
        {
            List<CEObj> targetList = Engine.GetRandomCells_SkillTarget(targetCount, excludeList);
            CEObj myCell = this.GetOwner<CEObj>();

            for(int i=0; i < targetList.Count; i++)
            {
                ShowEffect_Missile(myCell.centerPosition, targetList[i]);
            }

            //GlobalDefine.PlaySoundFX(ESoundSet.SOUND_SPECIAL_MISSILE);
        }

        private void ShowEffect_Missile(Vector3 centerPosition, CEObj target)
        {
            if(target != null && target.TryGetComponent<CECellObjController>(out CECellObjController oController))
            {
                Vector3 fromPosition = centerPosition;
                Vector3 toPosition = target.centerPosition;

                float fxAngle = GlobalDefine.GetAngle(fromPosition, toPosition) + GlobalDefine.FXMissile_AngleOffset;
                float distance = Vector2.Distance(fromPosition, toPosition);
                
                GlobalDefine.ShowEffect_Missile(EFXSet.FX_MISSILE_BULLET, fromPosition, fxAngle, target, Engine.CellDestroy_SkillTarget_Missile, GlobalDefine.FXMissile_Time);
                GlobalDefine.ShowEffect(EFXSet.FX_MISSILE_HEAD, toPosition);
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
	/** 서브 셀 객체 제어자 */
	public partial class CECellObjController : CEObjController {

        private void GetSpecial_Missile(CEBallObjController ballController, EObjKinds kindsType, EObjKinds kinds, int _ATK = KCDefine.B_VAL_1_INT)
        {
            //Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>{0}</color>", kinds));

            List<CEObj> excludeList = new List<CEObj>();
            excludeList.Add(this.GetOwner<CEObj>());
            
            switch(kinds)
            {
                case EObjKinds.SPECIAL_BRICKS_MISSILE_01: Missile(ballController, KCDefine.B_VAL_1_INT, excludeList); break;
                case EObjKinds.SPECIAL_BRICKS_MISSILE_02: Missile(ballController, KCDefine.B_VAL_4_INT, excludeList); break;
                default: break;
            }

            this.SetHideReserved();
        }

        ///<Summary>미사일.</Summary>
        private void Missile(CEBallObjController ballController, int targetCount, List<CEObj> excludeList)
        {
            List<CEObj> targetList = Engine.GetRandomCells_SkillTarget(targetCount, excludeList);

            for(int i=0; i < targetList.Count; i++)
            {
                ShowEffect_Missile(targetList[i]);
            }
        }

        private void ShowEffect_Missile(CEObj target)
        {
            if(target != null && target.TryGetComponent<CECellObjController>(out CECellObjController oController))
            {
                float fxAngle = GlobalDefine.GetAngle(this.transform.position, target.transform.position) + GlobalDefine.FXMissile_AngleOffset;
                float distance = Vector2.Distance(this.transform.position, target.transform.position);
                
                GlobalDefine.ShowEffect_Missile(EFXSet.FX_MISSILE_BULLET, this.transform.position, fxAngle, target, Engine.CellDestroy_SkillTarget, GlobalDefine.FXMissile_Time);
                GlobalDefine.ShowEffect(EFXSet.FX_MISSILE_HEAD, target.transform.position);

                //oController.ReserveMissileDestroy();
            }
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
	/** 서브 셀 객체 제어자 */
	public partial class CECellObjController : CEObjController {

        private void GetObstacle_Wormhole(CEBallObjController ballController, EObjKinds kindsType, EObjKinds kinds, int _ATK = KCDefine.B_VAL_1_INT)
        {
            //Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>{0}</color>", kindsType));

            if (ballController.isOn_Wormhole)
                return;
            
            ballController.transform.position = transform.position;

            switch(kindsType)
            {
                case EObjKinds.OBSTACLE_BRICKS_WARP_IN_01:  Wormhole(ballController, EObjKinds.OBSTACLE_BRICKS_WARP_OUT_01); break;
                default: break;
            }

            this.SetHideReserved();
        }

        private void Wormhole(CEBallObjController ballController, EObjKinds targetType)
        {
            List<CEObj> targetList = Engine.GetAllCells(targetType);
            
            if (targetList.Count < 1) 
                return;
            
            CEObj target = Engine.GetRandomCell(targetList);

            ballController.transform.position = target.transform.position;
            ballController.isOn_Wormhole = true;
            
            ShowEffect_Wormhole(target.transform);
        }

        private void ShowEffect_Wormhole(Transform target)
        {
            //GlobalDefine.ShowEffect(EFXSet.FX_LASER, this.transform.position, _rotation, Vector3.one);
        }
    }
}

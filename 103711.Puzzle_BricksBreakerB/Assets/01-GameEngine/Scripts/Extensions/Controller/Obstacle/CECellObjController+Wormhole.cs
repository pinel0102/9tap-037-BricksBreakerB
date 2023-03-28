using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
	/** 서브 셀 객체 제어자 */
	public partial class CECellObjController : CEObjController {

        private void GetObstacle_Wormhole(CEBallObjController ballController, EObjKinds kindsType, EObjKinds kinds, int _ATK = KCDefine.B_VAL_1_INT)
        {
            if (ballController.usedWormholes.Contains(this))
                return;

            //Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>{0}</color>", kindsType));
            
            switch(kindsType)
            {
                case EObjKinds.OBSTACLE_BRICKS_WARP_IN_01:  Wormhole(ballController, ExtraObjKindsList[m_oSubIntDict[ESubKey.EXTRA_OBJ_KINDS_IDX]]); break;
                default: break;
            }

            this.SetHideReserved();
        }

        private void Wormhole(CEBallObjController ballController, EObjKinds targetKinds)
        {
            List<CEObj> targetList = Engine.GetAllCells(targetKinds);
            
            if (targetList.Count < 1) 
                return;
            
            CEObj target = Engine.GetRandomCell(targetList);

            ballController.transform.position = target.transform.position;
            ballController.usedWormholes.Add(this);
            
            ShowEffect_Wormhole(target.transform);
        }

        private void ShowEffect_Wormhole(Transform target)
        {
            //GlobalDefine.ShowEffect(EFXSet.FX_LASER, this.transform.position, _rotation, Vector3.one);
        }
    }
}

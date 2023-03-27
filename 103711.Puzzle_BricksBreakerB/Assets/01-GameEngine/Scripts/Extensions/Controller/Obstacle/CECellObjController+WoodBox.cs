using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
	/** 서브 셀 객체 제어자 */
	public partial class CECellObjController : CEObjController {

        private void GetObstacle_WoodBox(CEBallObjController ballController, EObjKinds kindsType, EObjKinds kinds, int _ATK = KCDefine.B_VAL_1_INT)
        {
            //Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>{0}</color>", kindsType));
            
            switch(kindsType)
            {
                case EObjKinds.OBSTACLE_BRICKS_WOODBOX_01:  WoodBox(); break;
                case EObjKinds.OBSTACLE_BRICKS_WOODBOX_02:  WoodBox(); break;
                default: break;
            }

            this.SetHideReserved();
        }

        private void WoodBox()
        {
            //
        }

        private void ShowEffect_WoodBox()
        {
            //GlobalDefine.ShowEffect(EFXSet.FX_LASER, this.transform.position, _rotation, Vector3.one);
        }
    }
}

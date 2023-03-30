using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
	/** 서브 셀 객체 제어자 */
	public partial class CECellObjController : CEObjController {

        private void GetSpecial_Refract(CEBallObjController ballController, EObjKinds kindsType, EObjKinds kinds, int _ATK = KCDefine.B_VAL_1_INT)
        {
            //Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>{0}</color>", kindsType));

            switch(kindsType)
            {
                case EObjKinds.SPECIAL_BRICKS_BALL_DIFFUSION_01:
                    //Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>{0}</color>", kindsType));
                    ballController.transform.position = transform.position;
                    Refract_Diffusion(ballController); 
                    break;
                case EObjKinds.SPECIAL_BRICKS_BALL_AMPLIFICATION_01:
                    if (ballController.isOn_Amplification)
                        return;
                    //Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>{0}</color>", kindsType));
                    ballController.transform.position = transform.position;
                    Refract_Amplification(ballController); 
                    break;
                default: break;
            }

            this.SetHideReserved();
        }

        ///<Summary>볼 확산.</Summary>
        private void Refract_Diffusion(CEBallObjController ballController)
        {
            var stDirection = Quaternion.Euler(KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_0_REAL, Random.Range(-KDefine.E_MAX_ANGLE_REFRACT, KDefine.E_MAX_ANGLE_REFRACT)) * ballController.MoveDirection;
            stDirection = stDirection.y < 0 ? -stDirection : stDirection;

            ballController.SetMoveDirection(stDirection.normalized);
        }

        ///<Summary>볼 증폭. (2개가 되어 확산.)</Summary>
        private void Refract_Amplification(CEBallObjController ballController)
        {
            CEBallObjController ballController2 = Engine.AddExtraBall(ballController.transform.position);

            ballController.isOn_Amplification = true;
            ballController2.isOn_Amplification = true;

            Refract_Diffusion(ballController);
            Refract_Diffusion(ballController2);
        }
    }
}
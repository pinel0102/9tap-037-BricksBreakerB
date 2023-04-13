using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
	/** 서브 셀 객체 제어자 */
	public partial class CECellObjController : CEObjController {

        private void GetSpecial_PowerBall(CEBallObjController ballController, EObjKinds kindsType, EObjKinds kinds, int _ATK = KCDefine.B_VAL_1_INT)
        {
            //Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>{0}</color>", kindsType));
            
            switch(kindsType)
            {
                case EObjKinds.SPECIAL_BRICKS_POWERBALL_01: PowerBall(ballController, KCDefine.B_VAL_1_INT); break;
                default: break;
            }

            this.SetHideReserved();
        }

        ///<Summary>파워 볼.</Summary>
        private void PowerBall(CEBallObjController ballController, int _extraATK = KCDefine.B_VAL_1_INT)
        {
            ballController.extraATK = _extraATK;
            ballController.isOn_PowerBall = true;
            ballController.FXToggle_PowerBall(true);
            //ballController.SetBallSize(2f);

            GlobalDefine.PlaySoundFX(ESoundSet.SOUND_SPECIAL_POWERBALL);
        }
    }
}
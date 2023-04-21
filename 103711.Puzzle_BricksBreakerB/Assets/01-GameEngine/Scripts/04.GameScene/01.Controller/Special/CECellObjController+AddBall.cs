using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
	/** 서브 셀 객체 제어자 */
	public partial class CECellObjController : CEObjController {

        private void GetSpecial_AddBall(EObjKinds kindsType, EObjKinds kinds, int _ATK = KCDefine.B_VAL_1_INT)
        {
            //Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>{0}</color>", kindsType));

            int _addBallIndex = ((int)kinds).ExKindsToDetailSubKindsTypeVal();
            int _addCount = GlobalDefine.GetSpecial_AddBall[_addBallIndex];

            switch(kindsType)
            {
                case EObjKinds.SPECIAL_BRICKS_ADD_BALL_01:    AddBall(_addCount); break;
                default: break;
            }

            this.SetHideReserved();
        }

        private void AddBall(int _addCount)
        {
            Engine.AddNormalBalls(Engine.startPosition, _addCount);
            GlobalDefine.PlaySoundFX(ESoundSet.SOUND_SPECIAL_ADD_BALL);
        }
    }
}
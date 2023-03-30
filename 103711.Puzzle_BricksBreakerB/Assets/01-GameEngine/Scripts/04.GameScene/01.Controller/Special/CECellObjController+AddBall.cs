using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
	/** 서브 셀 객체 제어자 */
	public partial class CECellObjController : CEObjController {

        private void GetSpecial_AddBall(CEBallObjController ballController, EObjKinds kindsType, EObjKinds kinds, int _ATK = KCDefine.B_VAL_1_INT)
        {
            //Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>{0}</color>", kindsType));

            int _index = ((int)kinds).ExKindsToDetailSubKindsTypeVal();
            int _addCount = GlobalDefine.GetSpecial_AddBall[_index];

            switch(kindsType)
            {
                case EObjKinds.SPECIAL_BRICKS_ADD_BALL_01:    AddBall(_addCount); break;
                case EObjKinds.SPECIAL_BRICKS_ADD_BALL_02:    AddBall(_addCount); break;
                case EObjKinds.SPECIAL_BRICKS_ADD_BALL_03:    AddBall(_addCount); break;
                default: break;
            }

            this.SetHideReserved();
        }

        private void AddBall(int _addCount)
        {
            int _oldCount = Engine.BallObjList.Count;
            
            for (int i=0; i < _addCount; i++)
            {
                Engine.AddBall(_oldCount + i);
                Engine.BallObjList[_oldCount+i].NumText.text = string.Empty;
            }

            Engine.AddShootBalls(_oldCount, _addCount);
        }
    }
}
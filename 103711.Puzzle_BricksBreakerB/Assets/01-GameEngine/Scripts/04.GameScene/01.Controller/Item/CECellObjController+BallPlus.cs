using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
	/** 서브 셀 객체 제어자 */
	public partial class CECellObjController : CEObjController {

        private void GetItem_BallPlus(CEBallObjController ballController, EObjKinds kindsType, EObjKinds kinds)
        {
            //Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>{0}</color>", kindsType));

            int _ballPlusIndex = ((int)kinds).ExKindsToDetailSubKindsTypeVal();
            int _addCount = GlobalDefine.GetItem_BallPlus[_ballPlusIndex];

            Engine.AddNormalBalls(Engine.startPosition, _addCount);

            GlobalDefine.PlaySoundFX(ESoundSet.SOUND_GET_ITEM);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
	/** 서브 셀 객체 제어자 */
	public partial class CECellObjController : CEObjController {

        private void GetItem_BallPlus(EObjKinds kindsType, EObjKinds kinds)
        {
            int _oldCount = this.Engine.BallObjList.Count;
            int _index = ((int)kinds).ExKindsToDetailSubKindsTypeVal();            
            int _addCount = GlobalDefine.GetItem_BallPlus[_index];

            Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>{0} : +{1}</color>", kinds, _addCount));
            
            for (int i=0; i < _addCount; i++)
            {
                this.Engine.AddBall(_oldCount + i);
                this.Engine.BallObjList[_oldCount+i].NumText.text = string.Empty;
            }

            this.Engine.AddShootBalls(_oldCount, _addCount);
        }
    }
}

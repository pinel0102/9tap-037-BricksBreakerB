using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
	/** 서브 셀 객체 제어자 */
	public partial class CECellObjController : CEObjController {

        private void GetItem_BallPlus(EObjKinds kinds)
        {
            int _oldCount = this.Engine.BallObjList.Count;
            int _index = ((int)kinds).ExKindsToDetailSubKindsTypeVal();            
            int _addCount = ItemInfo.GetItem_BallPlus[_index];

            Debug.Log(CodeManager.GetMethodName() + string.Format("[{0}] : +{1}", _index, _addCount));
            
            for (int i=0; i < _addCount; i++)
            {
                this.Engine.CreateBall();
                this.Engine.BallObjList[_oldCount+i].NumText.text = string.Empty;
            }

            this.Engine.ShootBalls(_oldCount, _addCount);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
	/** 서브 셀 객체 제어자 */
	public partial class CECellObjController : CEObjController {

        private void GetItem_Ruby(CEBallObjController ballController, EObjKinds kindsType, EObjKinds kinds)
        {
            int _addCount = GlobalDefine.GetItem_Ruby;

            Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>{0} : +{1}</color>", kinds, _addCount));

            switch(kinds)
            {
                case EObjKinds.ITEM_BRICKS_COINS_01: AddRuby(_addCount); break;
                default: break;
            }
        }

        ///<Summary>루비 획득.</Summary>
        private void AddRuby(int count)
        {
            CEObj myCell = this.GetOwner<CEObj>();
            
            ShowEffect_AddRuby(myCell);
        }

        private void ShowEffect_AddRuby(CEObj target)
        {
            GlobalDefine.PlaySoundFX(ESoundSet.SOUND_GET_COIN);
        }
    }
}

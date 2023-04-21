using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
	/** 서브 셀 객체 제어자 */
	public partial class CECellObjController : CEObjController {

        private void GetSpecial_Earthquake(EObjKinds kindsType, EObjKinds kinds, int _ATK = KCDefine.B_VAL_1_INT)
        {
            //Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>{0}</color>", kinds));

            List<CEObj> excludeList = new List<CEObj>();
            excludeList.Add(this.GetOwner<CEObj>());
            
            switch(kinds)
            {
                case EObjKinds.SPECIAL_BRICKS_EARTHQUAKE_01: Earthquake(50, excludeList); break;
                default: break;
            }


            this.SetHideReserved();
        }

        ///<Summary>지진.</Summary>
        private void Earthquake(int _ATK, List<CEObj> excludeList)
        {
            Engine.subGameSceneManager.ShakeCamera(() => {

                List<CEObj> targetList = Engine.GetAllCells_EnableHit(excludeList);

                for(int i=0; i < targetList.Count; i++)
                {
                    Engine.CellDamage_EnableHit(targetList[i], null, _ATK);
                }

            });

            GlobalDefine.PlaySoundFX(ESoundSet.SOUND_SPECIAL_EARTHQUAKE);
        }
    }
}
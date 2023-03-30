using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
	/** 서브 셀 객체 제어자 */
	public partial class CECellObjController : CEObjController {

        private void GetObstacle_Key(EObjKinds kindsType)
        {
            //Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>{0}</color>", kindsType));
            
            switch(kindsType)
            {
                case EObjKinds.OBSTACLE_BRICKS_KEY_01:  UnLock(ExtraObjKindsList[m_oSubIntDict[ESubKey.EXTRA_OBJ_KINDS_IDX]]); break;
                default: break;
            }

            this.SetHideReserved();
        }

        private void UnLock(EObjKinds targetKinds)
        {
            List<CEObj> targetList = Engine.GetAllCells(targetKinds);            
            for (int i=0; i < targetList.Count; i++)
            {
                CECellObjController target = targetList[i].GetComponent<CECellObjController>();
                if (target.ExtraObjKindsList.ExIsValid())
                {
                    EObjKinds toKinds = target.ExtraObjKindsList[m_oSubIntDict[ESubKey.EXTRA_OBJ_KINDS_IDX]];
                    Engine.ChangeCell(target, toKinds);
                }
            }
        }

        private void ShowEffect_Key()
        {
            //GlobalDefine.ShowEffect(EFXSet.FX_LASER, this.transform.position, _rotation, Vector3.one);
        }
    }
}

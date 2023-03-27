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
                case EObjKinds.OBSTACLE_BRICKS_KEY_01:  UnLock(EObjKinds.OBSTACLE_BRICKS_LOCK_01); break;
                default: break;
            }

            this.SetHideReserved();
        }

        private void UnLock(EObjKinds targetType)
        {
            List<CEObj> targetList = Engine.GetAllCells(targetType);
            
            for (int i=0; i < targetList.Count; i++)
            {
                CECellObjController target = targetList[i].GetComponent<CECellObjController>();

                if(target.ExtraObjKindsList.ExIsValid())
                {
                    var oObjInfoList = CCollectionManager.Inst.SpawnList<STObjInfo>();

                    try {
                        m_oSubIntDict[ESubKey.EXTRA_OBJ_KINDS_IDX] = (m_oSubIntDict[ESubKey.EXTRA_OBJ_KINDS_IDX] + KCDefine.B_VAL_1_INT) % target.ExtraObjKindsList.Count;
                        var stObjInfo = CObjInfoTable.Inst.GetObjInfo(target.ExtraObjKindsList[m_oSubIntDict[ESubKey.EXTRA_OBJ_KINDS_IDX]]);

                        //Debug.Log(CodeManager.GetMethodName() + string.Format("TO <color=green>{0}</color>", target.ExtraObjKindsList[m_oSubIntDict[ESubKey.EXTRA_OBJ_KINDS_IDX]]));

                        oObjInfoList.Add(stObjInfo);
                        
                        target.ResetObjInfo(stObjInfo, target.CellObjInfo);

                    } finally {
                        CCollectionManager.Inst.DespawnList(oObjInfoList);
                    }
                }
            }
        }

        private void ShowEffect_Key()
        {
            //GlobalDefine.ShowEffect(EFXSet.FX_LASER, this.transform.position, _rotation, Vector3.one);
        }
    }
}

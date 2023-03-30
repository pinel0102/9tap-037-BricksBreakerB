using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
	/** 서브 셀 객체 제어자 */
	public partial class CECellObjController : CEObjController {

        private void GetObstacle_WoodBox(EObjKinds kindsType, EObjKinds kinds, int _ATK = KCDefine.B_VAL_1_INT)
        {
            //Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>{0}</color>", kinds));
            
            switch(kinds)
            {
                case EObjKinds.OBSTACLE_BRICKS_WOODBOX_01:  BreakWoodBox(); break;
                case EObjKinds.OBSTACLE_BRICKS_WOODBOX_02:  BreakWoodBox(); break;
                default: break;
            }

            this.SetHideReserved();
        }

        private void BreakWoodBox()
        {
            EObjKinds toKinds = ExtraObjKindsList[m_oSubIntDict[ESubKey.EXTRA_OBJ_KINDS_IDX]];
            
            CEObj myCell = this.GetOwner<CEObj>();
            STCellObjInfo stCellObjInfo = myCell.CellObjInfo;
            stCellObjInfo.SHIELD = 0;
            
            Engine.ChangeCell(this, toKinds, stCellObjInfo);
        }

        private void ShowEffect_WoodBox()
        {
            //GlobalDefine.ShowEffect(EFXSet.FX_LASER, this.transform.position, _rotation, Vector3.one);
        }
    }
}

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
            isShieldCell = false;

            CEObj myCell = this.GetOwner<CEObj>();
            EObjKinds toKinds = ExtraObjKindsList[m_oSubIntDict[ESubKey.EXTRA_OBJ_KINDS_IDX]];
            
            var stParams = myCell.Params;            
            stParams.m_stObjInfo = CObjInfoTable.Inst.GetObjInfo(toKinds);
            myCell.Init(stParams);

            STCellObjInfo stCellObjInfo = myCell.CellObjInfo;
            stCellObjInfo.ObjKinds = toKinds;
            stCellObjInfo.SHIELD = 0;

            myCell.SetCellObjInfo(stCellObjInfo);            
            myCell.InitSprite(GlobalDefine.IsNeedSubSprite(toKinds));
            myCell.RefreshText(toKinds);

            //Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>{0}</color>", myCell.CellObjInfo.ObjKinds));
        }

        private void ShowEffect_WoodBox()
        {
            //GlobalDefine.ShowEffect(EFXSet.FX_LASER, this.transform.position, _rotation, Vector3.one);
        }
    }
}

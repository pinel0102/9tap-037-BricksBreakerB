using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
	/** 서브 셀 객체 제어자 */
	public partial class CECellObjController : CEObjController {

        private void GetSpecial_Lazer(EObjKinds kinds)
        {
            EObjKinds kindsType = (EObjKinds)((int)kinds).ExKindsToCorrectKinds(EKindsGroupType.SUB_KINDS_TYPE);

            switch(kindsType)
            {
                case EObjKinds.SPECIAL_BRICKS_LAZER_HORIZONTAL_01:  Lazer_Horizontal(); break;
                case EObjKinds.SPECIAL_BRICKS_LAZER_VERTICAL_01:    Lazer_Vertical(); break;
                case EObjKinds.SPECIAL_BRICKS_LAZER_CROSS_01:       Lazer_Cross(); break;
                default: break;
            }
        }

        private void Lazer_Horizontal()
        {
            CEObj myCell = this.GetOwner<CEObj>();
            int _cRow = myCell.row;

            //Debug.Log(CodeManager.GetMethodName() + string.Format("Row:{0}, Col:{1}, {2}", myCell.row, myCell.column, myCell.layer));

            for(int i = 0; i < this.Engine.CellObjLists.GetLength(KCDefine.B_VAL_1_INT); ++i) 
            {
                int _count = this.Engine.CellObjLists[_cRow, i].Count;
                if (_count > 0)
                {
                    int _cLastLayer = _count - 1;
                    if(this.Engine.CellObjLists[_cRow, i][_cLastLayer].gameObject.activeSelf) 
                    {
                        CEObj target = this.Engine.CellObjLists[_cRow, i][_cLastLayer];
                        if (target != null)
                        {
                            EObjType cellType = (EObjType)((int)target.CellObjInfo.ObjKinds).ExKindsToType();
                            switch(cellType) 
                            {
                                case EObjType.NORM_BRICKS: 
                                    //Debug.Log(CodeManager.GetMethodName() + string.Format("CellObjLists[Row:{0}, Col:{1}][{2}]", _cRow, i, _cLastLayer));
                                    target.GetComponent<CECellObjController>().GetDamage(KCDefine.B_VAL_1_INT); 
                                    break;
                            }
                        }
                    }
                }
            }

            ShowEffect_Lazer_Horizontal();
        }

        private void Lazer_Vertical()
        {
            CEObj myCell = this.GetOwner<CEObj>();
            int _cCol = myCell.column;

            //Debug.Log(CodeManager.GetMethodName() + string.Format("Row:{0}, Col:{1}, {2}", myCell.row, myCell.column, myCell.layer));

            for(int i = 0; i < this.Engine.CellObjLists.GetLength(KCDefine.B_VAL_0_INT); ++i) 
            {
                int _count = this.Engine.CellObjLists[i, _cCol].Count;
                if (_count > 0)
                {
                    int _cLastLayer = _count - 1;
                    if(this.Engine.CellObjLists[i, _cCol][_cLastLayer].gameObject.activeSelf) 
                    {
                        CEObj target = this.Engine.CellObjLists[i, _cCol][_cLastLayer];
                        if (target != null)
                        {
                            EObjType cellType = (EObjType)((int)target.CellObjInfo.ObjKinds).ExKindsToType();
                            switch(cellType) 
                            {
                                case EObjType.NORM_BRICKS: 
                                    //Debug.Log(CodeManager.GetMethodName() + string.Format("CellObjLists[Row:{0}, Col:{1}][{2}]", i, _cCol, _cLastLayer));
                                    target.GetComponent<CECellObjController>().GetDamage(KCDefine.B_VAL_1_INT); 
                                    break;
                            }
                        }
                    }
                }
			}

            ShowEffect_Lazer_Vertical();
        }

        private void Lazer_Cross()
        {
            Lazer_Horizontal();
            Lazer_Vertical();
        }

        private void ShowEffect_Lazer_Horizontal()
        {
            //
        }

        private void ShowEffect_Lazer_Vertical()
        {
            //
        }
    }
}

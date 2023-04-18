using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
	/** 서브 셀 객체 제어자 */
	public partial class CECellObjController : CEObjController {

        private void GetSpecial_Laser(CEBallObjController ballController, EObjKinds kindsType, EObjKinds kinds, int _ATK = KCDefine.B_VAL_1_INT)
        {
            //Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>{0}</color>", kindsType));
            
            switch(kindsType)
            {
                case EObjKinds.SPECIAL_BRICKS_LASER_HORIZONTAL_01:  Laser_Horizontal(ballController, _ATK); break;
                case EObjKinds.SPECIAL_BRICKS_LASER_VERTICAL_01:    Laser_Vertical(ballController, _ATK); break;
                case EObjKinds.SPECIAL_BRICKS_LASER_CROSS_01:       Laser_Cross(ballController, _ATK); break;
                default: break;
            }

            this.SetHideReserved();
        }

        private void Laser_Horizontal(CEBallObjController ballController, int _ATK = KCDefine.B_VAL_1_INT)
        {
            CEObj myCell = this.GetOwner<CEObj>();
            int _cRow = myCell.row;

            //Debug.Log(CodeManager.GetMethodName() + string.Format("Row:{0}, Col:{1}, {2}", myCell.row, myCell.column, myCell.layer));
            List<CEObj> targetList = new List<CEObj>();

            for(int i = 0; i < Engine.CellObjLists.GetLength(KCDefine.B_VAL_1_INT); ++i) 
            {
                int row = _cRow;
                int col = i;

                int _count = Engine.CellObjLists[row, col].Count;
                if (_count > 0)
                {
                    int _cLastLayer = _count - 1;
                    CEObj target = Engine.CellObjLists[row, col][_cLastLayer];
                    if(target != null && target.IsActiveCell()) 
                    {
                        if (target.kinds == EObjKinds.BG_PLACEHOLDER_01 && target.parentCell != null)
                        {
                            if(!targetList.Contains(target.parentCell))
                                targetList.Add(target.parentCell);
                        }
                        else
                        {
                            if(!targetList.Contains(target))
                                targetList.Add(target);
                        }
                    }
                }
            }

            for(int i=0; i < targetList.Count; i++)
            {
                Engine.CellDamage_EnableHit(targetList[i], ballController, _ATK);
            }

            ShowEffect_Laser(myCell.centerPosition, GlobalDefine.FXLaser_Rotation_Horizontal);
        }

        private void Laser_Vertical(CEBallObjController ballController, int _ATK = KCDefine.B_VAL_1_INT)
        {
            CEObj myCell = this.GetOwner<CEObj>();
            int _cCol = myCell.col;

            //Debug.Log(CodeManager.GetMethodName() + string.Format("Row:{0}, Col:{1}, {2}", myCell.row, myCell.column, myCell.layer));
            List<CEObj> targetList = new List<CEObj>();

            for(int i = 0; i < Engine.CellObjLists.GetLength(KCDefine.B_VAL_0_INT); ++i) 
            {
                int row = i;
                int col = _cCol;

                int _count = Engine.CellObjLists[row, col].Count;
                if (_count > 0)
                {
                    int _cLastLayer = _count - 1;
                    CEObj target = Engine.CellObjLists[row, col][_cLastLayer];
                    if(target != null && target.IsActiveCell()) 
                    {
                        if (target.kinds == EObjKinds.BG_PLACEHOLDER_01 && target.parentCell != null)
                        {
                            if(!targetList.Contains(target.parentCell))
                                targetList.Add(target.parentCell);
                        }
                        else
                        {
                            if(!targetList.Contains(target))
                                targetList.Add(target);
                        }
                    }
                }
			}

            for(int i=0; i < targetList.Count; i++)
            {
                Engine.CellDamage_EnableHit(targetList[i], ballController, _ATK);
            }

            ShowEffect_Laser(myCell.centerPosition, GlobalDefine.FXLaser_Rotation_Vertictal);
        }

        private void Laser_Cross(CEBallObjController ballController, int _ATK = KCDefine.B_VAL_1_INT)
        {
            CEObj myCell = this.GetOwner<CEObj>();
            int _cRow = myCell.row;
            int _cCol = myCell.col;

            //Debug.Log(CodeManager.GetMethodName() + string.Format("Row:{0}, Col:{1}, {2}", myCell.row, myCell.column, myCell.layer));
            List<CEObj> targetList = new List<CEObj>();

            for(int i = 0; i < Engine.CellObjLists.GetLength(KCDefine.B_VAL_1_INT); ++i) 
            {
                int row = _cRow;
                int col = i;

                int _count = Engine.CellObjLists[row, col].Count;
                if (_count > 0)
                {
                    int _cLastLayer = _count - 1;
                    CEObj target = Engine.CellObjLists[row, col][_cLastLayer];
                    if(target != null && target.IsActiveCell()) 
                    {
                        if (target.kinds == EObjKinds.BG_PLACEHOLDER_01 && target.parentCell != null)
                        {
                            if(!targetList.Contains(target.parentCell))
                                targetList.Add(target.parentCell);
                        }
                        else
                        {
                            if(!targetList.Contains(target))
                                targetList.Add(target);
                        }
                    }
                }
            }

            for(int i = 0; i < Engine.CellObjLists.GetLength(KCDefine.B_VAL_0_INT); ++i) 
            {
                int row = i;
                int col = _cCol;

                int _count = Engine.CellObjLists[row, col].Count;
                if (_count > 0)
                {
                    int _cLastLayer = _count - 1;
                    CEObj target = Engine.CellObjLists[row, col][_cLastLayer];
                    if(target != null && target.IsActiveCell()) 
                    {
                        if (target.kinds == EObjKinds.BG_PLACEHOLDER_01 && target.parentCell != null)
                        {
                            if(!targetList.Contains(target.parentCell))
                                targetList.Add(target.parentCell);
                        }
                        else
                        {
                            if(!targetList.Contains(target))
                                targetList.Add(target);
                        }
                    }
                }
            }

            for(int i=0; i < targetList.Count; i++)
            {
                Engine.CellDamage_EnableHit(targetList[i], ballController, _ATK);
            }

            ShowEffect_Laser(myCell.centerPosition, GlobalDefine.FXLaser_Rotation_Horizontal);
            ShowEffect_Laser(myCell.centerPosition, GlobalDefine.FXLaser_Rotation_Vertictal);
        }

        private void ShowEffect_Laser(Vector3 centerPosition, Vector3 _rotation)
        {
            GlobalDefine.ShowEffect_Laser(EFXSet.FX_LASER, centerPosition, _rotation, Vector3.one);
            GlobalDefine.PlaySoundFX(ESoundSet.SOUND_SPECIAL_LASER);
        }
    }
}

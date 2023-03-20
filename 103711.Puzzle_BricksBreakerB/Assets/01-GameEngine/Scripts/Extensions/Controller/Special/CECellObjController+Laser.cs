using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
	/** 서브 셀 객체 제어자 */
	public partial class CECellObjController : CEObjController {

        private void GetSpecial_Laser(CEBallObjController ballController, EObjKinds kindsType, EObjKinds kinds, int _ATK = KCDefine.B_VAL_1_INT)
        {
            Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>{0}</color>", kindsType));
            
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
                                    target.GetComponent<CECellObjController>().GetDamage(ballController, _ATK); 
                                    break;
                            }
                        }
                    }
                }
            }

            ShowEffect_Laser(GlobalDefine.Rotation_Horizontal);
        }

        private void Laser_Vertical(CEBallObjController ballController, int _ATK = KCDefine.B_VAL_1_INT)
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
                                    target.GetComponent<CECellObjController>().GetDamage(ballController, _ATK); 
                                    break;
                            }
                        }
                    }
                }
			}

            ShowEffect_Laser(GlobalDefine.Rotation_Vertictal);
        }

        private void Laser_Cross(CEBallObjController ballController, int _ATK = KCDefine.B_VAL_1_INT)
        {
            Laser_Horizontal(ballController, _ATK);
            Laser_Vertical(ballController, _ATK);
        }

        private void ShowEffect_Laser(Vector3 _rotation)
        {
            Transform effect = CSceneManager.ActiveSceneManager.SpawnObj<Transform>(KDefine.E_OBJ_N_FX_LASER_OBJ, KDefine.E_KEY_FX_OBJS_POOL, Vector3.one, _rotation, this.transform.position, true);
            CSceneManager.ActiveSceneManager.DespawnObj(KDefine.E_KEY_FX_OBJS_POOL, effect.gameObject, GlobalDefine.EffectTime_Laser);
        }
    }
}

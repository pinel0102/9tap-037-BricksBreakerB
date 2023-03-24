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

            for(int i = 0; i < this.Engine.CellObjLists.GetLength(KCDefine.B_VAL_1_INT); ++i) 
            {
                CellDamage_SkillTarget(_cRow, i, ballController, _ATK);
            }

            ShowEffect_Laser(GlobalDefine.FXLaser_Rotation_Horizontal);
        }

        private void Laser_Vertical(CEBallObjController ballController, int _ATK = KCDefine.B_VAL_1_INT)
        {
            CEObj myCell = this.GetOwner<CEObj>();
            int _cCol = myCell.col;

            //Debug.Log(CodeManager.GetMethodName() + string.Format("Row:{0}, Col:{1}, {2}", myCell.row, myCell.column, myCell.layer));

            for(int i = 0; i < this.Engine.CellObjLists.GetLength(KCDefine.B_VAL_0_INT); ++i) 
            {
                CellDamage_SkillTarget(i, _cCol, ballController, _ATK);
			}

            ShowEffect_Laser(GlobalDefine.FXLaser_Rotation_Vertictal);
        }

        private void Laser_Cross(CEBallObjController ballController, int _ATK = KCDefine.B_VAL_1_INT)
        {
            Laser_Horizontal(ballController, _ATK);
            Laser_Vertical(ballController, _ATK);
        }

        private void ShowEffect_Laser(Vector3 _rotation)
        {
            GlobalDefine.ShowEffect(EFXSet.FX_LASER, this.transform.position, _rotation, Vector3.one);
        }
    }
}

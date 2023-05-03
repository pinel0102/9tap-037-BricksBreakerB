using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
	/** 서브 셀 객체 제어자 */
	public partial class CECellObjController : CEObjController {

        private void GetSpecial_Explosion(EObjKinds kindsType, EObjKinds kinds, int _ATK = KCDefine.B_VAL_1_INT)
        {
            //Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>{0}</color>", kindsType));
            
            switch(kindsType)
            {
                case EObjKinds.SPECIAL_BRICKS_EXPLOSION_HORIZONTAL_01: Explosion_Horizontal(kindsType, kinds); break;
                case EObjKinds.SPECIAL_BRICKS_EXPLOSION_VERTICAL_01: Explosion_Vertical(kindsType, kinds); break;
                case EObjKinds.SPECIAL_BRICKS_EXPLOSION_CROSS_01: Explosion_Cross(kindsType, kinds); break;
                case EObjKinds.SPECIAL_BRICKS_EXPLOSION_AROUND_01: Explosion_Around(kindsType, kinds); break;
                case EObjKinds.SPECIAL_BRICKS_EXPLOSION_ALL_01: Explosion_All(kindsType, kinds); break;
                default: break;
            }

            this.SetHideReserved();
        }

        ///<Summary>가로 폭탄.</Summary>
        private void Explosion_Horizontal(EObjKinds kindsType, EObjKinds kinds)
        {
            CEObj myCell = this.GetOwner<CEObj>();
            int _cRow = myCell.row;
            int _cCol = myCell.col;

            //Debug.Log(CodeManager.GetMethodName() + string.Format("Row:{0}, Col:{1}, {2}", myCell.row, myCell.column, myCell.layer));

            for(int i = 0; i < Engine.CellObjLists.GetLength(KCDefine.B_VAL_1_INT); ++i) 
            {
                if (i != _cCol)
                    Engine.CellDestroy_SkillTarget(_cRow, i);
            }

            ShowEffect_Explosion(myCell.centerPosition, GlobalDefine.FXLaser_Rotation_Horizontal);
        }

        ///<Summary>세로 폭탄.</Summary>
        private void Explosion_Vertical(EObjKinds kindsType, EObjKinds kinds)
        {
            CEObj myCell = this.GetOwner<CEObj>();
            int _cRow = myCell.row;
            int _cCol = myCell.col;

            //Debug.Log(CodeManager.GetMethodName() + string.Format("Row:{0}, Col:{1}, {2}", myCell.row, myCell.column, myCell.layer));

            for(int i = 0; i < Engine.CellObjLists.GetLength(KCDefine.B_VAL_0_INT); ++i) 
            {
                if (i != _cRow)
                    Engine.CellDestroy_SkillTarget(i, _cCol);
			}

            ShowEffect_Explosion(myCell.centerPosition, GlobalDefine.FXLaser_Rotation_Vertictal);
        }

        ///<Summary>십자 폭탄.</Summary>
        private void Explosion_Cross(EObjKinds kindsType, EObjKinds kinds)
        {
            Explosion_Horizontal(kindsType, kinds);
            Explosion_Vertical(kindsType, kinds);
        }

        ///<Summary>3x3 폭탄.</Summary>
        private void Explosion_Around(EObjKinds kindsType, EObjKinds kinds)
        {
            CEObj myCell = this.GetOwner<CEObj>();
            int _cRow = myCell.row;
            int _cCol = myCell.col;

            Debug.Log(CodeManager.GetMethodName() + string.Format("Row:{0}, Col:{1}, {2}", myCell.row, myCell.col, myCell.layer));

            for (int i = Mathf.Max(0, _cRow - 1); i < Mathf.Min(_cRow + 2, Engine.CellObjLists.GetLength(KCDefine.B_VAL_0_INT)); i++)
            {
                for (int j = Mathf.Max(0, _cCol - 1); j < Mathf.Min(_cCol + 2, Engine.CellObjLists.GetLength(KCDefine.B_VAL_1_INT)); j++)
                {
                    if (i != _cRow || j != _cCol)
                        Engine.CellDestroy_SkillTarget(i, j);
                }
            }

            ShowEffect_Explosion_Around(myCell.centerPosition);
        }

        ///<Summary>대형 폭탄.</Summary>
        private void Explosion_All(EObjKinds kindsType, EObjKinds kinds)
        {
            CEObj myCell = this.GetOwner<CEObj>();
            
            Debug.Log(CodeManager.GetMethodName() + string.Format("Row:{0}, Col:{1}, {2}", myCell.row, myCell.col, myCell.layer));

            List<CEObj> excludeList = new List<CEObj>();
            excludeList.Add(myCell);

            ShowEffect_Explosion_All(myCell.centerPosition + (GlobalDefine.CenterOffset_EXPLOSION_ALL * Engine.SelGridInfo.m_stScale.x));
            
            Engine.ExLateCallFunc((sender) => {

                List<CEObj> targetList = Engine.GetAllCells_SkillTarget(excludeList, true, true);

                for(int i=0; i < targetList.Count; i++)
                {
                    Engine.CellDestroy_SkillTarget(targetList[i], true, true);
                }

                if (Engine.PlayState == NSEngine.CEngine.EPlayState.IDLE)
                {
                    Engine.CheckClear(true, true);
                }

            }, KCDefine.B_VAL_0_5_REAL);
        }

        private void ShowEffect_Explosion(Vector3 centerPosition, Vector3 _rotation)
        {
            GlobalDefine.ShowEffect_Laser_Red(EFXSet.FX_LASER_RED, centerPosition, _rotation, Vector3.one, GlobalDefine.FXLaserRed_Time);
            GlobalDefine.PlaySoundFX(ESoundSet.SOUND_SPECIAL_EXPLOSION);
        }

        private void ShowEffect_Explosion_Around(Vector3 centerPosition)
        {
            GlobalDefine.ShowEffect(EFXSet.FX_EXPLOSION_3x3, centerPosition);
            GlobalDefine.PlaySoundFX(ESoundSet.SOUND_SPECIAL_EXPLOSION_3x3);
        }

        private void ShowEffect_Explosion_All(Vector3 centerPosition)
        {
            GlobalDefine.ShowEffect(EFXSet.FX_EXPLOSION_ALL, centerPosition);
            GlobalDefine.PlaySoundFX(ESoundSet.SOUND_SPECIAL_EXPLOSION_ALL);
        }
    }
}
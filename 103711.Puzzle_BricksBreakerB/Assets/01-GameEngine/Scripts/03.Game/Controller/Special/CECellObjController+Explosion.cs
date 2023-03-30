using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
	/** 서브 셀 객체 제어자 */
	public partial class CECellObjController : CEObjController {

        private void GetSpecial_Explosion(CEBallObjController ballController, EObjKinds kindsType, EObjKinds kinds, int _ATK = KCDefine.B_VAL_1_INT)
        {
            //Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>{0}</color>", kindsType));
            
            switch(kindsType)
            {
                case EObjKinds.SPECIAL_BRICKS_EXPLOSION_HORIZONTAL_01: Explosion_Horizontal(ballController, kindsType, kinds); break;
                case EObjKinds.SPECIAL_BRICKS_EXPLOSION_VERTICAL_01: Explosion_Vertical(ballController, kindsType, kinds); break;
                case EObjKinds.SPECIAL_BRICKS_EXPLOSION_CROSS_01: Explosion_Cross(ballController, kindsType, kinds); break;
                case EObjKinds.SPECIAL_BRICKS_EXPLOSION_AROUND_01: Explosion_Around(ballController, kindsType, kinds); break;
                default: break;
            }

            this.SetHideReserved();
        }

        ///<Summary>가로 폭탄.</Summary>
        private void Explosion_Horizontal(CEBallObjController ballController, EObjKinds kindsType, EObjKinds kinds)
        {
            CEObj myCell = this.GetOwner<CEObj>();
            int _cRow = myCell.row;

            //Debug.Log(CodeManager.GetMethodName() + string.Format("Row:{0}, Col:{1}, {2}", myCell.row, myCell.column, myCell.layer));

            for(int i = 0; i < Engine.CellObjLists.GetLength(KCDefine.B_VAL_1_INT); ++i) 
            {
                Engine.CellDestroy_SkillTarget(_cRow, i);
            }

            ShowEffect_Explosion(GlobalDefine.FXLaser_Rotation_Horizontal);
        }

        ///<Summary>세로 폭탄.</Summary>
        private void Explosion_Vertical(CEBallObjController ballController, EObjKinds kindsType, EObjKinds kinds)
        {
            CEObj myCell = this.GetOwner<CEObj>();
            int _cCol = myCell.col;

            //Debug.Log(CodeManager.GetMethodName() + string.Format("Row:{0}, Col:{1}, {2}", myCell.row, myCell.column, myCell.layer));

            for(int i = 0; i < Engine.CellObjLists.GetLength(KCDefine.B_VAL_0_INT); ++i) 
            {
                Engine.CellDestroy_SkillTarget(i, _cCol);
			}

            ShowEffect_Explosion(GlobalDefine.FXLaser_Rotation_Vertictal);
        }

        ///<Summary>십자 폭탄.</Summary>
        private void Explosion_Cross(CEBallObjController ballController, EObjKinds kindsType, EObjKinds kinds)
        {
            Explosion_Horizontal(ballController, kindsType, kinds);
            Explosion_Vertical(ballController, kindsType, kinds);
        }

        ///<Summary>3x3 폭탄.</Summary>
        private void Explosion_Around(CEBallObjController ballController, EObjKinds kindsType, EObjKinds kinds)
        {
            CEObj myCell = this.GetOwner<CEObj>();
            int _cRow = myCell.row;
            int _cCol = myCell.col;

            Debug.Log(CodeManager.GetMethodName() + string.Format("Row:{0}, Col:{1}, {2}", myCell.row, myCell.col, myCell.layer));

            for (int i = Mathf.Max(0, _cRow - 1); i < Mathf.Min(_cRow + 2, Engine.CellObjLists.GetLength(KCDefine.B_VAL_0_INT)); i++)
            {
                for (int j = Mathf.Max(0, _cCol - 1); j < Mathf.Min(_cCol + 2, Engine.CellObjLists.GetLength(KCDefine.B_VAL_1_INT)); j++)
                {
                    Engine.CellDestroy_SkillTarget(i, j);
                }
            }

            ShowEffect_Explosion_Around();
        }

        private void ShowEffect_Explosion(Vector3 _rotation)
        {
            //Transform effect = CSceneManager.ActiveSceneManager.SpawnObj<Transform>(GlobalDefine.FX_LASER, KDefine.E_KEY_FX_OBJS_POOL, Vector3.one, _rotation, this.transform.position, true);
            //CSceneManager.ActiveSceneManager.DespawnObj(KDefine.E_KEY_FX_OBJS_POOL, effect.gameObject, GlobalDefine.FXTime_LASER);
        }

        private void ShowEffect_Explosion_Around()
        {
            GlobalDefine.ShowEffect(EFXSet.FX_EXPLOSION_3x3, this.transform.position);
        }
    }
}
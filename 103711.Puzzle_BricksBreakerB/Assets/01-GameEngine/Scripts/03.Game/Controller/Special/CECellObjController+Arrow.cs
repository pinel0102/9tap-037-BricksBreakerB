using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
	/** 서브 셀 객체 제어자 */
	public partial class CECellObjController : CEObjController {

        private void GetSpecial_Arrow(CEBallObjController ballController, EObjKinds kindsType, EObjKinds kinds, int _ATK = KCDefine.B_VAL_1_INT)
        {
            //Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>{0}</color>", kinds));

            CEObj myCell = this.GetOwner<CEObj>();
            int _cCol = myCell.col;
            int _cRow = myCell.row;
            
            switch(kinds)
            {
                case EObjKinds.SPECIAL_BRICKS_ARROW_01: Arrow_Up(_cCol, _cRow); break;
                case EObjKinds.SPECIAL_BRICKS_ARROW_02: Arrow_RightUp(_cCol, _cRow); break;
                case EObjKinds.SPECIAL_BRICKS_ARROW_03: Arrow_Right(_cCol, _cRow); break;
                case EObjKinds.SPECIAL_BRICKS_ARROW_04: Arrow_RightDown(_cCol, _cRow); break;
                case EObjKinds.SPECIAL_BRICKS_ARROW_05: Arrow_Down(_cCol, _cRow); break;
                case EObjKinds.SPECIAL_BRICKS_ARROW_06: Arrow_LeftDown(_cCol, _cRow); break;
                case EObjKinds.SPECIAL_BRICKS_ARROW_07: Arrow_Left(_cCol, _cRow); break;
                case EObjKinds.SPECIAL_BRICKS_ARROW_08: Arrow_LeftUp(_cCol, _cRow); break;
                default: break;
            }

            this.SetHideReserved();
        }

        private void Arrow_Left(int _cCol, int _cRow)
        {
            int startIndex = 0;
            int endIndex = Mathf.Max(_cCol, 0);
            
            Arrow_Horizontal(_cCol, _cRow, startIndex, endIndex);
            ShowEffect_Arrow(-1, 0);
        }

        private void Arrow_Right(int _cCol, int _cRow)
        {
            int startIndex = Mathf.Min(_cCol + 1, this.Engine.CellObjLists.GetLength(KCDefine.B_VAL_1_INT) - 1);
            int endIndex = this.Engine.CellObjLists.GetLength(KCDefine.B_VAL_1_INT);

            Arrow_Horizontal(_cCol, _cRow, startIndex, endIndex);
            ShowEffect_Arrow(1, 0);
        }        

        private void Arrow_Up(int _cCol, int _cRow)
        {
            int startIndex = 0;
            int endIndex = Mathf.Max(_cRow, 0);

            Arrow_Vertical(_cCol, _cRow, startIndex, endIndex);
            ShowEffect_Arrow(0, 1);
        }

        private void Arrow_Down(int _cCol, int _cRow)
        {
            int startIndex = Mathf.Min(_cRow + 1, this.Engine.CellObjLists.GetLength(KCDefine.B_VAL_0_INT) - 1);
            int endIndex = this.Engine.CellObjLists.GetLength(KCDefine.B_VAL_0_INT);

            Arrow_Vertical(_cCol, _cRow, startIndex, endIndex);
            ShowEffect_Arrow(0, -1);
        }

        private void Arrow_RightDown(int _cCol, int _cRow)
        {
            int startRow = Mathf.Min(_cRow + 1, this.Engine.CellObjLists.GetLength(KCDefine.B_VAL_0_INT) - 1);
            int startCol = Mathf.Min(_cCol + 1, this.Engine.CellObjLists.GetLength(KCDefine.B_VAL_1_INT) - 1);

            Arrow_Diagonal(_cCol, _cRow, startCol, startRow, 1, 1);
            ShowEffect_Arrow(1, 1);
        }

        private void Arrow_RightUp(int _cCol, int _cRow)
        {
            int startRow = Mathf.Max(_cRow - 1, 0);
            int startCol = Mathf.Min(_cCol + 1, this.Engine.CellObjLists.GetLength(KCDefine.B_VAL_1_INT) - 1);

            Arrow_Diagonal(_cCol, _cRow, startCol, startRow, 1, -1);
            ShowEffect_Arrow(1, -1);
        }

        private void Arrow_LeftDown(int _cCol, int _cRow)
        {
            int startRow = Mathf.Min(_cRow + 1, this.Engine.CellObjLists.GetLength(KCDefine.B_VAL_0_INT) - 1);
            int startCol = Mathf.Max(_cCol - 1, 0);

            Arrow_Diagonal(_cCol, _cRow, startCol, startRow, -1, 1);
            ShowEffect_Arrow(-1, 1);
        }

        private void Arrow_LeftUp(int _cCol, int _cRow)
        {
            int startRow = Mathf.Max(_cRow - 1, 0);
            int startCol = Mathf.Max(_cCol - 1, 0);

            Arrow_Diagonal(_cCol, _cRow, startCol, startRow, -1, -1);
            ShowEffect_Arrow(-1, -1);
        }



        ///<Summary>가로 화살.</Summary>
        private void Arrow_Horizontal(int _cCol, int _cRow, int startIndex, int endIndex)
        {
            for(int i = startIndex; i < endIndex; ++i) 
            {
                Engine.CellDestroy_SkillTarget(_cRow, i);
            }
        }

        ///<Summary>세로 화살.</Summary>
        private void Arrow_Vertical(int _cCol, int _cRow, int startIndex, int endIndex)
        {
            for(int i = startIndex; i < endIndex; ++i) 
            {
                Engine.CellDestroy_SkillTarget(i, _cCol);
			}
        }

        ///<Summary>대각선 화살.</Summary>
        private void Arrow_Diagonal(int _cCol, int _cRow, int startCol, int startRow, int directionX, int directionY)
        {
            for (int i = startRow; i >= 0 && i < this.Engine.CellObjLists.GetLength(KCDefine.B_VAL_0_INT); i += directionY)
            {
                for (int j = startCol; j >= 0 && j < this.Engine.CellObjLists.GetLength(KCDefine.B_VAL_1_INT); )
                {
                    Engine.CellDestroy_SkillTarget(i, j);

                    startCol += directionX;
                    break;
                }
            }
        }

        private void ShowEffect_Arrow(int directionX, int directionY)
        {
            //Transform effect = CSceneManager.ActiveSceneManager.SpawnObj<Transform>(GlobalDefine.FX_LASER, KDefine.E_KEY_FX_OBJS_POOL, Vector3.one, _rotation, this.transform.position, true);
            //CSceneManager.ActiveSceneManager.DespawnObj(KDefine.E_KEY_FX_OBJS_POOL, effect.gameObject, GlobalDefine.FXTime_LASER);
        }
    }
}
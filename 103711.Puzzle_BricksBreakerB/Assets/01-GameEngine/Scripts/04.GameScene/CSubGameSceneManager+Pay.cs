using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GameScene {
    public partial class CSubGameSceneManager : CGameSceneManager
    {
        public void GetContinueBonus()
        {
            int deleteCount = 3;

            this.ExLateCallFunc((a_oFuncSender) => 
            {
                var lastClearTarget = Engine.GetLastClearTarget();
                if (lastClearTarget != null)
                {
                    int lastRow = lastClearTarget.row;

                    for (int line = 0; line < deleteCount; line++)
                    {
                        GlobalDefine.ShowEffect_Laser_Red(EFXSet.FX_LASER_RED, new Vector3(0, lastClearTarget.transform.position.y + (NSEngine.Access.CellSize.y * line), lastClearTarget.transform.position.z), GlobalDefine.FXLaser_Rotation_Horizontal, Vector3.one, GlobalDefine.FXLaserRed_Time);

                        for(int i = 0; i < Engine.CellObjLists.GetLength(KCDefine.B_VAL_1_INT); ++i) 
                        {
                            Engine.CellDestroy_SkillTarget(Mathf.Max(0, lastClearTarget.row - line), i, true);
                        }
                    }
                }

                Engine.RefreshActiveCells();
                Engine.CheckDeadLine();

                Engine.CheckClear(true, true);
                Engine.isLevelFail = false;
                    
            }, KCDefine.B_VAL_0_5_REAL);
        }
    }
}
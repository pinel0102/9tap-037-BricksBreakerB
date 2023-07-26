using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
    public partial class CEngine : CComponent
    {
        /*public void RequestVideo(RewardVideoType type, CPopup popup)
        {
            Debug.Log(CodeManager.GetMethodName());

            if (GlobalDefine.isLevelEditor)
            {
                GetReward(type, popup);
            }
            else
            {
                GlobalDefine.RequestRewardVideo(type, popup);
            }
        }*/

        public void GetReward(RewardVideoType type, CPopup popup, bool isRewardVideo = true)
        {
            Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>{0}</color>", type));

            switch(type)
            {
                case RewardVideoType.CONTINUE_3LINE:

                    if (popup != null)
                    {
                        var _popup = popup as CContinuePopup;
                        _popup.Params.m_oCallbackDict?.GetValueOrDefault(CContinuePopup.ECallback.CONTINUE)?.Invoke(_popup);
                        _popup.Close();
                    }

                    int deleteCount = 3;

                    this.ExLateCallFunc((a_oFuncSender) => 
                    {
                        var lastClearTarget = this.GetLastClearTarget();
                        if (lastClearTarget != null)
                        {
                            int lastRow = lastClearTarget.row;

                            for (int line = 0; line < deleteCount; line++)
                            {
                                GlobalDefine.ShowEffect_Laser_Red(EFXSet.FX_LASER_RED, new Vector3(0, lastClearTarget.transform.position.y + ((this.cellsizeY * CSceneManager.ObjsRootScale.y) * line), lastClearTarget.transform.position.z), GlobalDefine.FXLaser_Rotation_Horizontal, Vector3.one, GlobalDefine.FXLaserRed_Time);

                                for(int i = 0; i < this.CellObjLists.GetLength(KCDefine.B_VAL_1_INT); ++i) 
                                {
                                    this.CellDestroy_SkillTarget(Mathf.Max(0, lastRow - line), i, true);
                                }
                            }
                        }

                        this.RefreshActiveCells();
                        this.CheckDeadLine();
                        this.CheckClear(true, true);
                        
                        this.isLevelFail = false;
                            
                    }, KCDefine.B_VAL_0_5_REAL);

                    break;

                case RewardVideoType.READY_BOOSTER:
                    break;

                case RewardVideoType.INGAME_ADDBALLS:
                    break;
            }
        }
    }
}
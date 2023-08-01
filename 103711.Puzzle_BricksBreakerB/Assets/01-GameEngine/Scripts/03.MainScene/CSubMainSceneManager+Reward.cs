using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MainScene {
    public partial class CSubMainSceneManager
    {
        [Header("★ [Live] Reward Video")]
        public RewardVideoType rewardVideoType = RewardVideoType.NONE;
        public GameObject rewardBalloon;

#region Reward Balloon

        private void InitRewardButtons()
        {
            //
        }

        public void RefreshLimitedItems()
        {
            //TODO: RefreshLimitedItems
        }

        public void RefreshRewardBalloon(bool isShow)
        {
            rewardBalloon.SetActive(isShow);
        }

        public void RewardBalloon_Hide()
        {
            rewardBalloon.SetActive(false);
            
            DateTime now = DateTime.Now;
            GlobalDefine.UserInfo.LimitedStartTime_Balloon = now.ExToLongStr();
            GlobalDefine.UserInfo.LimitedItemCount_Balloon++;
            CGameInfoStorage.Inst.InitCooltime();

            GlobalDefine.SaveUserData();
        }

        public void OnClick_Reward_BalloonBoosterButton() 
        {
            rewardVideoType = RewardVideoType.BALLOON_BOOSTER;

            Func.ShowRewardVideoAlertPopup(this.PopupUIs, (a_oSender) => {
                var oTargetInfoDict = CCollectionManager.Inst.SpawnDict<ulong, STTargetInfo>();

                try {
                    switch(rewardVideoType)
                    {
                        case RewardVideoType.BALLOON_BOOSTER:
                            (a_oSender as CRewardVideoAlertPopup).Init(CRewardVideoAlertPopup.MakeParams(KDefine.L_SCENE_N_MAIN, KCDefine.B_VAL_0_INT, rewardVideoType, ERewardKinds.ADS_REWARD_BALLOON_BOOSTER, EItemKinds.BOOSTER_ITEM_04_RANDOM, KCDefine.B_VAL_0_INT, 
                            GlobalDefine.RewardVideoDesc_RandomBooster, 
                            () => {  }, null));
                            break;
                    }				
                } finally {
                    CCollectionManager.Inst.DespawnDict(oTargetInfoDict);
                }
            }, null, null);
        }

#endregion Reward Balloon

    }
}

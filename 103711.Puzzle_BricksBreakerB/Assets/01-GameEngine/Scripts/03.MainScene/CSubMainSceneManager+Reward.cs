using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainScene {
    public partial class CSubMainSceneManager
    {
        [Header("★ [Live] Reward Video")]
        public RewardVideoType rewardVideoType = RewardVideoType.NONE;
        public GameObject rewardBalloon;
        public bool isBalloonEnable;
        public bool isLoadRewardAds;

        private WaitForSecondsRealtime wBalloonDelay = new WaitForSecondsRealtime(1f);

#region Reward Balloon

        private void InitRewardButtons()
        {
            CheckBalloonCooltime();
        }

        private void CheckBalloonCooltime()
        {
            RewardBalloon_Show();
            //StartCoroutine(CO_BalloonCooltime());
        }

        public void RewardBalloon_Show()
        {
            rewardBalloon.SetActive(true);
        }

        public void RewardBalloon_Hide()
        {
            rewardBalloon.SetActive(false);
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

        private IEnumerator CO_BalloonCooltime()
        {
            yield return wBalloonDelay;

            RewardBalloon_Show();
        }

#endregion Reward Balloon

    }
}

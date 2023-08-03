using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace MainScene {
    public partial class CSubMainSceneManager
    {
        [Header("★ [Reference] Reward Video")]
        public RewardVideoType rewardVideoType = RewardVideoType.NONE;
        public GameObject rewardBalloon;
        public Button rewardBalloonButton;

        [Header("★ [Reference] Limited Items")]
        public List<GameObject> limitedItem = new List<GameObject>();
        public List<TMP_Text> limitedItem_Count = new List<TMP_Text>();
        public List<TMP_Text> limitedItem_Time = new List<TMP_Text>();

#region Reward Balloon

        private void InitRewardButtons()
        {
            //Debug.Log(CodeManager.GetMethodName());

            for(int i=0; i < limitedItem.Count; i++)
            {
                limitedItem[i].SetActive(false);
            }

            rewardBalloonButton?.ExAddListener(this.OnClick_Reward_BalloonBoosterButton);
        }

        public void RefreshLimitedItems()
        {
            //Debug.Log(CodeManager.GetMethodName());
            
            for(int i=0; i < limitedItem.Count; i++)
            {
                LimitedItem currentItem = CGameInfoStorage.Inst.limitedItems[i+1];
                limitedItem[i].SetActive(currentItem.count > 0);
                limitedItem_Count[i].text = currentItem.count.ToString();
                limitedItem_Time[i].text = GlobalDefine.SecondsToTimeText(currentItem.cooltime);
            }
        }

        public void RefreshRewardBalloon(bool isShow)
        {
            rewardBalloon.SetActive(isShow);
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

#endregion Reward Balloon

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class CGameInfoStorage
{
    public bool rewardBooster_balloon;
    public bool rewardBooster_ready;

    public void Initialize()
    {
        InitRewardBooster();
    }

    public void InitRewardBooster()
    {
        rewardBooster_balloon = false;
        rewardBooster_ready = false;
    }

    public void GetRewardBooster(RewardVideoType type)
    {
        switch(type)
        {
            case RewardVideoType.BALLOON_BOOSTER:
                rewardBooster_balloon = true;
                break;
            case RewardVideoType.READY_BOOSTER:
                rewardBooster_ready = true;
                break;
        }
    }
}

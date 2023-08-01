using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class CGameInfoStorage
{
    public bool isLoadRewardAds;
    public bool rewardBooster_balloon;
    public bool rewardBooster_ready;
    public List<int> limitedCooltime = new List<int>();
    public List<int> limitedItemCount = new List<int>();
    
    private Coroutine coCheckCooltime;
    private WaitForSecondsRealtime wOneSecond = new WaitForSecondsRealtime(1f);

    public void Initialize()
    {
        InitCooltime();
        InitRewardBooster();
    }

#region Reward Cooltime

    public void InitCooltime()
    {
        DateTime now = DateTime.Now;

        List<KeyValuePair<int, int>> kv = new List<KeyValuePair<int, int>>();
        kv.Add(CalulateCooltime(now, GlobalDefine.UserInfo.LimitedStartTime_Balloon, GlobalDefine.UserInfo.LimitedItemCount_Balloon, GlobalDefine.cooltime_balloon));
        kv.Add(CalulateCooltime(now, GlobalDefine.UserInfo.LimitedStartTime_Earthquake, GlobalDefine.UserInfo.LimitedItemCount_Earthquake, GlobalDefine.cooltime_item));
        kv.Add(CalulateCooltime(now, GlobalDefine.UserInfo.LimitedStartTime_AddBall, GlobalDefine.UserInfo.LimitedItemCount_AddBall, GlobalDefine.cooltime_item));
        kv.Add(CalulateCooltime(now, GlobalDefine.UserInfo.LimitedStartTime_BricksDelete, GlobalDefine.UserInfo.LimitedItemCount_BricksDelete, GlobalDefine.cooltime_item));
        kv.Add(CalulateCooltime(now, GlobalDefine.UserInfo.LimitedStartTime_AddLaserBricks, GlobalDefine.UserInfo.LimitedItemCount_AddLaserBricks, GlobalDefine.cooltime_item));
        kv.Add(CalulateCooltime(now, GlobalDefine.UserInfo.LimitedStartTime_AddSteelBricks, GlobalDefine.UserInfo.LimitedItemCount_AddSteelBricks, GlobalDefine.cooltime_item));

        GlobalDefine.UserInfo.LimitedItemCount_Balloon = kv[0].Key;
        GlobalDefine.UserInfo.LimitedItemCount_Earthquake = kv[1].Key;
        GlobalDefine.UserInfo.LimitedItemCount_AddBall = kv[2].Key;
        GlobalDefine.UserInfo.LimitedItemCount_BricksDelete = kv[3].Key;
        GlobalDefine.UserInfo.LimitedItemCount_AddLaserBricks = kv[4].Key;
        GlobalDefine.UserInfo.LimitedItemCount_AddSteelBricks = kv[5].Key;
        GlobalDefine.SaveUserData();

        limitedCooltime.Clear();        
        limitedCooltime.Add(kv[0].Value);
        limitedCooltime.Add(kv[1].Value);
        limitedCooltime.Add(kv[2].Value);
        limitedCooltime.Add(kv[3].Value);
        limitedCooltime.Add(kv[4].Value);
        limitedCooltime.Add(kv[5].Value);
        
        limitedItemCount.Clear();
        limitedItemCount.Add(GlobalDefine.UserInfo.LimitedItemCount_Balloon);
        limitedItemCount.Add(GlobalDefine.UserInfo.LimitedItemCount_Earthquake);
        limitedItemCount.Add(GlobalDefine.UserInfo.LimitedItemCount_AddBall);
        limitedItemCount.Add(GlobalDefine.UserInfo.LimitedItemCount_BricksDelete);
        limitedItemCount.Add(GlobalDefine.UserInfo.LimitedItemCount_AddLaserBricks);
        limitedItemCount.Add(GlobalDefine.UserInfo.LimitedItemCount_AddSteelBricks);

        CheckCooltime();
    }

    private KeyValuePair<int, int> CalulateCooltime(DateTime now, string startTime, int itemCount, int cooltimeMax)
    {
        int totalSeconds = (int)now.Subtract(startTime.ExToTime(KCDefine.B_DATE_T_FMT_SLASH_YYYY_MM_DD_HH_MM_SS)).TotalSeconds;
        int newCount = Mathf.Max(0, itemCount - (totalSeconds / cooltimeMax));
        int newCooltime = newCount > 0 ? Mathf.Max(GlobalDefine.cooltime_none, cooltimeMax - totalSeconds) : GlobalDefine.cooltime_none;
        
        return new KeyValuePair<int, int>(newCount, newCooltime);
    }

    private void CheckCooltime()
    {
        if (coCheckCooltime != null) StopCoroutine(coCheckCooltime);        
        coCheckCooltime = StartCoroutine(CO_CheckCooltime());
    }

    private IEnumerator CO_CheckCooltime()
    {
        while(true)
        {
            yield return wOneSecond;

            DateTime now = DateTime.Now;
            bool isChanged = false;

            isChanged = AfterOneSecond(now, 0, GlobalDefine.cooltime_balloon) || isChanged;
            isChanged = AfterOneSecond(now, 1, GlobalDefine.cooltime_item) || isChanged;
            isChanged = AfterOneSecond(now, 2, GlobalDefine.cooltime_item) || isChanged;
            isChanged = AfterOneSecond(now, 3, GlobalDefine.cooltime_item) || isChanged;
            isChanged = AfterOneSecond(now, 4, GlobalDefine.cooltime_item) || isChanged;
            isChanged = AfterOneSecond(now, 5, GlobalDefine.cooltime_item) || isChanged;

            if (isChanged)
            {
                Debug.Log(CodeManager.GetMethodName() + string.Format("isChanged : {0}", isChanged));

                GlobalDefine.UserInfo.LimitedItemCount_Balloon = limitedItemCount[0];
                GlobalDefine.UserInfo.LimitedItemCount_Earthquake = limitedItemCount[1];
                GlobalDefine.UserInfo.LimitedItemCount_AddBall = limitedItemCount[2];
                GlobalDefine.UserInfo.LimitedItemCount_BricksDelete = limitedItemCount[3];
                GlobalDefine.UserInfo.LimitedItemCount_AddLaserBricks = limitedItemCount[4];
                GlobalDefine.UserInfo.LimitedItemCount_AddSteelBricks = limitedItemCount[5];

                GlobalDefine.SaveUserData();
            }

            RefreshCooltimeUI();
        }
    }

    private bool AfterOneSecond(DateTime now, int index, int cooltimeMax)
    {
        bool isChanged = false;

        if (limitedCooltime[index] > GlobalDefine.cooltime_none)
        {
            limitedCooltime[index]--;
        }

        if (limitedCooltime[index] == GlobalDefine.cooltime_none)
        {
            if(limitedItemCount[index] > 0)
            {
                isChanged = true;
                limitedItemCount[index] = Mathf.Max(0, limitedItemCount[index] - 1);

                Debug.Log(CodeManager.GetMethodName() + string.Format("[{0}] currentCooltime : {1} / limitedItem_Count[{2}]: {3}", index, now.ExToLongStr(), index, limitedItemCount[index]));

                if (limitedItemCount[index] > 0)
                    limitedCooltime[index] = CalulateCooltime(now, now.ExToLongStr(), limitedItemCount[index], cooltimeMax).Value;
            }
        }

        return isChanged;
    }

    private void RefreshCooltimeUI()
    {
        //Debug.Log(CodeManager.GetMethodName());

        isLoadRewardAds = GlobalDefine.IsEnableRewardVideo();

        RefreshRewardBalloon(isLoadRewardAds && limitedCooltime[0] == GlobalDefine.cooltime_none);
        RefreshLimitedItems();
    }

    private void RefreshRewardBalloon(bool isShow)
    {
        CSceneManager.GetSceneManager<MainScene.CSubMainSceneManager>(KCDefine.B_SCENE_N_MAIN)?.RefreshRewardBalloon(isShow);
    }

    private void RefreshLimitedItems()
    {
        CSceneManager.GetSceneManager<MainScene.CSubMainSceneManager>(KCDefine.B_SCENE_N_MAIN)?.RefreshLimitedItems();
        CSceneManager.GetSceneManager<GameScene.CSubGameSceneManager>(KCDefine.B_SCENE_N_GAME)?.RefreshLimitedItems();
    }

#endregion Reward Cooltime


#region Reward Booster

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

#endregion Reward Booster

}

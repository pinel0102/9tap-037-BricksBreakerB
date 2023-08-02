using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class CGameInfoStorage
{
    [Header("★ [Reference] Limited Items")]
    public bool isLoadRewardAds;
    public bool rewardBooster_balloon;
    public bool rewardBooster_ready;
    public List<LimitedItem> limitedItems = new List<LimitedItem>();
    
    private const float checkCooltimeDelay = 0.5f;
    private const int checkCooltimeTick = (int)(1f/checkCooltimeDelay);
    private WaitForSecondsRealtime wCheckCooltimeDelay = new WaitForSecondsRealtime(checkCooltimeDelay);
    private Coroutine coCheckCooltime;

    public void Initialize()
    {
        Debug.Log(CodeManager.GetMethodName());

        InitRewardBooster();
        InitCooltime(DateTime.Now);
        CheckCooltime();
    }

#region Reward Cooltime

    public void InitCooltime(DateTime _now)
    {
        limitedItems.Clear();
        limitedItems.Add(new LimitedItem(_now, GlobalDefine.UserInfo.LimitedStartTime_Balloon, GlobalDefine.UserInfo.LimitedItemCount_Balloon, GlobalDefine.cooltime_balloon));
        limitedItems.Add(new LimitedItem(_now, GlobalDefine.UserInfo.LimitedStartTime_Earthquake, GlobalDefine.UserInfo.LimitedItemCount_Earthquake, GlobalDefine.cooltime_item));
        limitedItems.Add(new LimitedItem(_now, GlobalDefine.UserInfo.LimitedStartTime_AddBall, GlobalDefine.UserInfo.LimitedItemCount_AddBall, GlobalDefine.cooltime_item));
        limitedItems.Add(new LimitedItem(_now, GlobalDefine.UserInfo.LimitedStartTime_BricksDelete, GlobalDefine.UserInfo.LimitedItemCount_BricksDelete, GlobalDefine.cooltime_item));
        limitedItems.Add(new LimitedItem(_now, GlobalDefine.UserInfo.LimitedStartTime_AddLaserBricks, GlobalDefine.UserInfo.LimitedItemCount_AddLaserBricks, GlobalDefine.cooltime_item));
        limitedItems.Add(new LimitedItem(_now, GlobalDefine.UserInfo.LimitedStartTime_AddSteelBricks, GlobalDefine.UserInfo.LimitedItemCount_AddSteelBricks, GlobalDefine.cooltime_item));

        SaveUserData_LimitedItems();
    }

    private void SaveUserData_LimitedItems()
    {
        Debug.Log(CodeManager.GetMethodName());

        GlobalDefine.UserInfo.LimitedItemCount_Balloon = limitedItems[0].count;
        GlobalDefine.UserInfo.LimitedItemCount_Earthquake = limitedItems[1].count;
        GlobalDefine.UserInfo.LimitedItemCount_AddBall = limitedItems[2].count;
        GlobalDefine.UserInfo.LimitedItemCount_BricksDelete = limitedItems[3].count;
        GlobalDefine.UserInfo.LimitedItemCount_AddLaserBricks = limitedItems[4].count;
        GlobalDefine.UserInfo.LimitedItemCount_AddSteelBricks = limitedItems[5].count;
        GlobalDefine.SaveUserData();
    }

    private void CheckCooltime()
    {
        if (coCheckCooltime != null) StopCoroutine(coCheckCooltime);        
        coCheckCooltime = StartCoroutine(CO_CheckCooltime());
    }

    private IEnumerator CO_CheckCooltime()
    {
        RefreshCooltimeUI();

        int currentTick = 0;

        while(true)
        {
            yield return wCheckCooltimeDelay;

            currentTick++;
            if(currentTick % checkCooltimeTick == 0)
            {
                currentTick = 0;
                DateTime now = DateTime.Now;
                bool isChanged = false;

                for(int i=0; i<limitedItems.Count; i++)
                {
                    isChanged = AfterOneSecond(now, i) || isChanged;
                }

                if (isChanged)
                {
                    SaveUserData_LimitedItems();
                }
            }

            RefreshCooltimeUI();
        }
    }

    private bool AfterOneSecond(DateTime now, int index)
    {
        bool isChanged = false;
        LimitedItem currentItem = limitedItems[index];

        currentItem.DecreaseCoolTime();

        if (currentItem.cooltime == GlobalDefine.cooltime_none)
        {
            if (currentItem.count > 0)
            {
                isChanged = true;
                currentItem.DecreaseCount();

                Debug.Log(CodeManager.GetMethodName() + string.Format("limitedItems[{0}] / count : {1} / startTime : {2}", index, currentItem.count, now.ExToLongStr()));

                if (currentItem.count > 0)
                {
                    currentItem.SetCooltime(now, now);
                }
            }
        }

        limitedItems[index] = currentItem;

        return isChanged;
    }

    private void RefreshCooltimeUI()
    {
        //Debug.Log(CodeManager.GetMethodName());

        isLoadRewardAds = GlobalDefine.IsEnableRewardVideo();

        RefreshRewardBalloon(isLoadRewardAds && limitedItems[0].cooltime == GlobalDefine.cooltime_none);
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

[System.Serializable]
public struct LimitedItem
{
    public string startTime;
    public int cooltimeMax;
    public int cooltime;
    public int count;

    public LimitedItem(DateTime _now, string _startTime, int _count, int _cooltimeMax)
    {
        startTime = _startTime;
        cooltimeMax = _cooltimeMax;
        cooltime = GlobalDefine.cooltime_none;
        count = _count;

        SetCooltime(_now, _startTime);
    }

    public void DecreaseCoolTime()
    {
        cooltime = Mathf.Max(GlobalDefine.cooltime_none, cooltime - 1);
    }

    public void DecreaseCount()
    {
        count = Mathf.Max(0, count - 1);
    }

    public void SetCooltime(DateTime _now, DateTime _startTime)
    {
        SetCooltime(_now, _startTime.ExToLongStr());
    }

    private void SetCooltime(DateTime _now, string _startTime)
    {
        if (cooltimeMax == 0) return;

        int totalSeconds = (int)_now.Subtract(_startTime.ExToTime(KCDefine.B_DATE_T_FMT_SLASH_YYYY_MM_DD_HH_MM_SS)).TotalSeconds;
        count = Mathf.Max(0, count - (totalSeconds / cooltimeMax));
        cooltime = count > 0 ? Mathf.Max(GlobalDefine.cooltime_none, (cooltimeMax - 1) - totalSeconds) : GlobalDefine.cooltime_none;
    }
}
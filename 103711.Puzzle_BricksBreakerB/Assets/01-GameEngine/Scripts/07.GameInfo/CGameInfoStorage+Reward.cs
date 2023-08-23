using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public partial class CGameInfoStorage
{
    [Header("★ [Reference] Limited Items")]
    public bool isLoadRewardAds;
    public bool rewardBooster_balloon;
    public bool rewardBooster_ready;
    public List<LimitedItem> limitedItems = new List<LimitedItem>();
    public List<int> rewardBoosterIndex = new List<int>();
    public int rewardBoosterIndex_balloon = -1;
    
    private const float checkCooltimeDelay = 0.5f;
    private const int checkCooltimeTick = (int)(1f/checkCooltimeDelay);
    private WaitForSecondsRealtime wCheckCooltimeDelay = new WaitForSecondsRealtime(checkCooltimeDelay);
    private Coroutine coCheckCooltime;

    public void Initialize()
    {
        Debug.Log(CodeManager.GetMethodName());

        InitRewardBooster();
        InitCooltime(DateTime.Now);
    }

#region Cooltime

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
        CheckCooltime();
    }

    private void SaveUserData_LimitedItems()
    {
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

                Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>(-) Limited Item : {0}</color>", index));
            }
        }

        limitedItems[index] = currentItem;

        return isChanged;
    }

    private void RefreshCooltimeUI()
    {
        //Debug.Log(CodeManager.GetMethodName());

        isLoadRewardAds = GlobalDefine.IsEnableRewardVideo();

        RefreshRewardBalloon(isLoadRewardAds && GlobalDefine.UserInfo.LevelCurrent >= GlobalDefine.BOOSTER_LEVEL[0] && limitedItems[0].cooltime == GlobalDefine.cooltime_none);
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

#endregion Cooltime


#region Limited Items

    public void UseLimitedItem(int index)
    {
        Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>(use) Limited Item : {0}</color>", index));

        DateTime now = DateTime.Now;

        switch(index)
        {
            case 1:
                GlobalDefine.UserInfo.LimitedItemCount_Earthquake--;
                if (GlobalDefine.UserInfo.LimitedItemCount_Earthquake > 0)
                    GlobalDefine.UserInfo.LimitedStartTime_Earthquake = now.ExToLongStr();
                break;
            case 2:
                GlobalDefine.UserInfo.LimitedItemCount_AddBall--;
                if (GlobalDefine.UserInfo.LimitedItemCount_AddBall > 0)
                    GlobalDefine.UserInfo.LimitedStartTime_AddBall = now.ExToLongStr();
                break;
            case 3:
                GlobalDefine.UserInfo.LimitedItemCount_BricksDelete--;
                if (GlobalDefine.UserInfo.LimitedItemCount_BricksDelete > 0)
                    GlobalDefine.UserInfo.LimitedStartTime_BricksDelete = now.ExToLongStr();
                break;
            case 4:
                GlobalDefine.UserInfo.LimitedItemCount_AddLaserBricks--;
                if (GlobalDefine.UserInfo.LimitedItemCount_AddLaserBricks > 0)
                    GlobalDefine.UserInfo.LimitedStartTime_AddLaserBricks = now.ExToLongStr();
                break;
            case 5:
                GlobalDefine.UserInfo.LimitedItemCount_AddSteelBricks--;
                if (GlobalDefine.UserInfo.LimitedItemCount_AddSteelBricks > 0)
                    GlobalDefine.UserInfo.LimitedStartTime_AddSteelBricks = now.ExToLongStr();
                break;
            default:
                break;
        }

        GlobalDefine.SaveUserData();

        LimitedItem currentItem = limitedItems[index];
        currentItem.DecreaseCount();
        limitedItems[index] = currentItem;
    }

    public void GetLimitedItem(int index)
    {
        Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>(+) Limited Item : {0}</color>", index));

        DateTime now = DateTime.Now;
        
        switch(index)
        {
            case 0:
                if (GlobalDefine.UserInfo.LimitedItemCount_Balloon == 0)
                    GlobalDefine.UserInfo.LimitedStartTime_Balloon = now.ExToLongStr();
                GlobalDefine.UserInfo.LimitedItemCount_Balloon++;
                break;
            case 1:
                if (GlobalDefine.UserInfo.LimitedItemCount_Earthquake == 0)
                    GlobalDefine.UserInfo.LimitedStartTime_Earthquake = now.ExToLongStr();
                GlobalDefine.UserInfo.LimitedItemCount_Earthquake++;
                break;
            case 2:
                if (GlobalDefine.UserInfo.LimitedItemCount_AddBall == 0)
                    GlobalDefine.UserInfo.LimitedStartTime_AddBall = now.ExToLongStr();
                GlobalDefine.UserInfo.LimitedItemCount_AddBall++;
                break;
            case 3:
                if (GlobalDefine.UserInfo.LimitedItemCount_BricksDelete == 0)
                    GlobalDefine.UserInfo.LimitedStartTime_BricksDelete = now.ExToLongStr();
                GlobalDefine.UserInfo.LimitedItemCount_BricksDelete++;
                break;
            case 4:
                if (GlobalDefine.UserInfo.LimitedItemCount_AddLaserBricks == 0)
                    GlobalDefine.UserInfo.LimitedStartTime_AddLaserBricks = now.ExToLongStr();
                GlobalDefine.UserInfo.LimitedItemCount_AddLaserBricks++;
                break;
            case 5:
                if (GlobalDefine.UserInfo.LimitedItemCount_AddSteelBricks == 0)
                    GlobalDefine.UserInfo.LimitedStartTime_AddSteelBricks = now.ExToLongStr();
                GlobalDefine.UserInfo.LimitedItemCount_AddSteelBricks++;
                break;
            default:
                break;
        }

        GlobalDefine.SaveUserData();

        LimitedItem currentItem = limitedItems[index];
        currentItem.IncreaseCount();
        limitedItems[index] = currentItem;

        switch(index)
        {
            case 1: case 2: case 3: case 4: case 5: 

            var mainSceneManager = CSceneManager.GetSceneManager<MainScene.CSubMainSceneManager>(KCDefine.B_SCENE_N_MAIN);
            var PopupUIs = mainSceneManager != null ? mainSceneManager?.PopupUIs : CSceneManager.GetSceneManager<GameScene.CSubGameSceneManager>(KCDefine.B_SCENE_N_GAME)?.PopupUIs;
            EItemKinds kinds = EItemKinds.GAME_ITEM_01_EARTHQUAKE + (index - 1);

            Func.ShowRewardAcquirePopup(PopupUIs, (a_oSender) => {
            try {
                (a_oSender as CRewardAcquirePopup).Init(CRewardAcquirePopup.MakeParams(
                    KDefine.L_SCENE_N_MAIN, KCDefine.B_VAL_0_INT, 
                    kinds, KCDefine.B_VAL_0_INT, false, 
                    () => {  }, GlobalDefine.SecondsToTimeText(GlobalDefine.cooltime_item)));
                } 
                finally { }
            }, null, null);            
            break;
        }
    }


#endregion Limited Items


#region Reward Booster

    public void InitRewardBooster()
    {
        rewardBooster_balloon = false;
        rewardBooster_ready = false;
        rewardBoosterIndex.Clear();
        rewardBoosterIndex_balloon = -1;
    }

    public void GetRewardBooster(RewardVideoType type)
    {
        switch(type)
        {
            case RewardVideoType.BALLOON_BOOSTER:
                rewardBooster_balloon = true;
                rewardBoosterIndex_balloon = GetRandomBoosterIndex();
                break;
            case RewardVideoType.READY_BOOSTER:
                rewardBooster_ready = true;
                break;
        }
    }

    public int GetRandomBoosterIndex()
    {
        int boosterEnableCount = GlobalDefine.GetEnableBoosterCount();
        int _index = 0;

        List<int> enableList = new List<int>();
        for(int i=0; i < boosterEnableCount; i++)
        {
            if(!rewardBoosterIndex.Contains(i))
                enableList.Add(i);
        }
        
        if (enableList.Count > 0)
            _index = enableList.OrderBy(g => System.Guid.NewGuid())
                                .Take(1).ToList()[0];

        if (_index > -1)
        {
            Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>enableCount : {0} / index : {1}</color>", boosterEnableCount, _index));

            rewardBoosterIndex.Add(_index);
        }

        return _index;
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

        InitCooltime(_now, _startTime);
    }

    public void IncreaseCount()
    {
        count++;        
        if (count == 1)
            StartCooltime();
    }

    public void DecreaseCount()
    {
        count = Mathf.Max(0, count - 1);
        ResetCooltime();
    }

    public void DecreaseCoolTime()
    {
        cooltime = Mathf.Max(GlobalDefine.cooltime_none, cooltime - 1);
    }

    private void StartCooltime()
    {
        cooltime = Mathf.Max(GlobalDefine.cooltime_none, cooltimeMax - 1);
    }

    private void ResetCooltime()
    {
        cooltime = CalculateCooltime(0);
    }

    private void InitCooltime(DateTime _now, string _startTime)
    {
        if (cooltimeMax == 0) return;
        int _totalSeconds = (int)_now.Subtract(_startTime.ExToTime(KCDefine.B_DATE_T_FMT_SLASH_YYYY_MM_DD_HH_MM_SS)).TotalSeconds;
        
        count = Mathf.Max(0, count - (_totalSeconds / cooltimeMax));
        cooltime = CalculateCooltime(_totalSeconds);
    }

    private int CalculateCooltime(int _elapsedSeconds)
    {
        return count > 0 ? Mathf.Max(GlobalDefine.cooltime_none, (cooltimeMax - 1) - _elapsedSeconds) : GlobalDefine.cooltime_none;
    }
}
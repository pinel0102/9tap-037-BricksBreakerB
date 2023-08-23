using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GlobalDefine
{
    public const int cooltime_balloon = 180; // 180
    public const int cooltime_item = 1800; // 1800
    public const int cooltime_none = -1;

    public static readonly List<KeyValuePair<EItemKinds, int>> dailyReward = new List<KeyValuePair<EItemKinds, int>>()
    {
        new KeyValuePair<EItemKinds, int>(EItemKinds.GAME_ITEM_01_EARTHQUAKE, 1),
        new KeyValuePair<EItemKinds, int>(EItemKinds.GAME_ITEM_02_ADD_BALLS, 1),
        new KeyValuePair<EItemKinds, int>(EItemKinds.GOODS_RUBY, 50),
        new KeyValuePair<EItemKinds, int>(EItemKinds.GAME_ITEM_03_BRICKS_DELETE, 1),
        new KeyValuePair<EItemKinds, int>(EItemKinds.GOODS_RUBY, 100),
        new KeyValuePair<EItemKinds, int>(EItemKinds.GAME_ITEM_05_ADD_STEEL_BRICKS, 1),
        new KeyValuePair<EItemKinds, int>(EItemKinds.GOODS_RUBY, 150),
    };

    public static void AddRuby(int addCount)
    {
        Debug.Log(CodeManager.GetMethodName() + string.Format("{0} + {1} = {2}", UserInfo.Ruby, addCount, UserInfo.Ruby + addCount));

        UserInfo.Ruby = Mathf.Max(0, UserInfo.Ruby + addCount);

        SaveUserData();
    }

    public static void AddStar(int addCount)
    {
        Debug.Log(CodeManager.GetMethodName() + string.Format("{0} + {1} = {2}", UserInfo.Star, addCount, UserInfo.Star + addCount));

        UserInfo.Star = Mathf.Max(0, UserInfo.Star + addCount);

        SaveUserData();
    }

    public static void AddItem(EItemKinds kinds, int addCount)
    {
        Debug.Log(CodeManager.GetMethodName() + string.Format("[{0}] x {1}", kinds, addCount));

        switch(kinds)
        {
            case EItemKinds.GOODS_RUBY:
                AddRuby(addCount);
                break;
            case EItemKinds.GAME_ITEM_01_EARTHQUAKE:
                UserInfo.Item_Earthquake = Mathf.Max(0, UserInfo.Item_Earthquake + addCount);
                break;
            case EItemKinds.GAME_ITEM_02_ADD_BALLS:
                UserInfo.Item_AddBall = Mathf.Max(0, UserInfo.Item_AddBall + addCount);
                break;
            case EItemKinds.GAME_ITEM_03_BRICKS_DELETE:
                UserInfo.Item_BricksDelete = Mathf.Max(0, UserInfo.Item_BricksDelete + addCount);
                break;
            case EItemKinds.GAME_ITEM_04_ADD_LASER_BRICKS:
                UserInfo.Item_AddLaserBricks = Mathf.Max(0, UserInfo.Item_AddLaserBricks + addCount);
                break;
            case EItemKinds.GAME_ITEM_05_ADD_STEEL_BRICKS:
                UserInfo.Item_AddSteelBricks = Mathf.Max(0, UserInfo.Item_AddSteelBricks + addCount);
                break;
            case EItemKinds.BOOSTER_ITEM_01_MISSILE:
                UserInfo.Booster_Missile = Mathf.Max(0, UserInfo.Booster_Missile + addCount);
                break;
            case EItemKinds.BOOSTER_ITEM_02_LIGHTNING:
                UserInfo.Booster_Lightning = Mathf.Max(0, UserInfo.Booster_Lightning + addCount);
                break;
            case EItemKinds.BOOSTER_ITEM_03_BOMB:
                UserInfo.Booster_Bomb = Mathf.Max(0, UserInfo.Booster_Bomb + addCount);
                break;
        }

        SaveUserData();
    }

    public static void SetRuby(int newCount)
    {
        Debug.Log(CodeManager.GetMethodName() + newCount);

        UserInfo.Ruby = Mathf.Max(0, newCount);

        SaveUserData();
    }

    public static void SetItem(int newCount)
    {
        Debug.Log(CodeManager.GetMethodName() + newCount);

        UserInfo.Item_Earthquake = Mathf.Max(0, newCount);
        UserInfo.Item_AddBall = Mathf.Max(0, newCount);
        UserInfo.Item_BricksDelete = Mathf.Max(0, newCount);
        UserInfo.Item_AddLaserBricks = Mathf.Max(0, newCount);
        UserInfo.Item_AddSteelBricks = Mathf.Max(0, newCount);

        SaveUserData();
    }

    public static void SetBooster(int newCount)
    {
        Debug.Log(CodeManager.GetMethodName() + newCount);

        UserInfo.Booster_Missile = Mathf.Max(0, newCount);
        UserInfo.Booster_Lightning = Mathf.Max(0, newCount);
        UserInfo.Booster_Bomb = Mathf.Max(0, newCount);

        SaveUserData();
    }

    public static void SetClear(int level)
    {
        Debug.Log(CodeManager.GetMethodName() + level);

        UserInfo.LevelCurrent = Mathf.Min(CLevelInfoTable.Inst.levelCount, level + 1);
        
        SaveUserData();
    }

    public static int GetEnableBoosterCount()
    {
        int enableCount = 0;
        for(int i=0; i < BOOSTER_LEVEL.Count; i++)
        {
            if(UserInfo.LevelCurrent >= BOOSTER_LEVEL[i])
                enableCount++;
        }
        return enableCount;
    }
}
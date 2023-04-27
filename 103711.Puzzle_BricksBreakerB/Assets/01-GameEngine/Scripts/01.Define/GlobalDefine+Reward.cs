using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GlobalDefine
{
    public static void AddRuby(int addCount)
    {
        Debug.Log(CodeManager.GetMethodName() + string.Format("{0} + {1} = {2}", GlobalDefine.UserInfo.Ruby, addCount, GlobalDefine.UserInfo.Ruby + addCount));

        GlobalDefine.UserInfo.Ruby = Mathf.Max(0, GlobalDefine.UserInfo.Ruby + addCount);

        SaveUserData();
    }

    public static void AddStar(int addCount)
    {
        Debug.Log(CodeManager.GetMethodName() + string.Format("{0} + {1} = {2}", GlobalDefine.UserInfo.Star, addCount, GlobalDefine.UserInfo.Star + addCount));

        GlobalDefine.UserInfo.Star = Mathf.Max(0, GlobalDefine.UserInfo.Star + addCount);

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
                GlobalDefine.UserInfo.Item_Earthquake = Mathf.Max(0, GlobalDefine.UserInfo.Item_Earthquake + addCount);
                break;
            case EItemKinds.GAME_ITEM_02_ADD_BALLS:
                GlobalDefine.UserInfo.Item_AddBall = Mathf.Max(0, GlobalDefine.UserInfo.Item_AddBall + addCount);
                break;
            case EItemKinds.GAME_ITEM_03_BRICKS_DELETE:
                GlobalDefine.UserInfo.Item_BricksDelete = Mathf.Max(0, GlobalDefine.UserInfo.Item_BricksDelete + addCount);
                break;
            case EItemKinds.GAME_ITEM_04_ADD_LASER_BRICKS:
                GlobalDefine.UserInfo.Item_AddLaserBricks = Mathf.Max(0, GlobalDefine.UserInfo.Item_AddLaserBricks + addCount);
                break;
            case EItemKinds.GAME_ITEM_05_ADD_STEEL_BRICKS:
                GlobalDefine.UserInfo.Item_AddSteelBricks = Mathf.Max(0, GlobalDefine.UserInfo.Item_AddSteelBricks + addCount);
                break;
            case EItemKinds.BOOSTER_ITEM_01_MISSILE:
                GlobalDefine.UserInfo.Booster_Missile = Mathf.Max(0, GlobalDefine.UserInfo.Booster_Missile + addCount);
                break;
            case EItemKinds.BOOSTER_ITEM_02_LIGHTNING:
                GlobalDefine.UserInfo.Booster_Lightning = Mathf.Max(0, GlobalDefine.UserInfo.Booster_Lightning + addCount);
                break;
            case EItemKinds.BOOSTER_ITEM_03_BOMB:
                GlobalDefine.UserInfo.Booster_Bomb = Mathf.Max(0, GlobalDefine.UserInfo.Booster_Bomb + addCount);
                break;
        }

        SaveUserData();
    }

    public static void SetRuby(int newCount)
    {
        Debug.Log(CodeManager.GetMethodName() + newCount);

        GlobalDefine.UserInfo.Ruby = Mathf.Max(0, newCount);

        SaveUserData();
    }

    public static void SetStar(int newCount)
    {
        Debug.Log(CodeManager.GetMethodName() + newCount);

        GlobalDefine.UserInfo.Star = Mathf.Max(0, newCount);

        SaveUserData();
    }

    public static void SetItem(int newCount)
    {
        Debug.Log(CodeManager.GetMethodName() + newCount);

        GlobalDefine.UserInfo.Item_Earthquake = Mathf.Max(0, newCount);
        GlobalDefine.UserInfo.Item_AddBall = Mathf.Max(0, newCount);
        GlobalDefine.UserInfo.Item_BricksDelete = Mathf.Max(0, newCount);
        GlobalDefine.UserInfo.Item_AddLaserBricks = Mathf.Max(0, newCount);
        GlobalDefine.UserInfo.Item_AddSteelBricks = Mathf.Max(0, newCount);

        SaveUserData();
    }

    public static void SetBooster(int newCount)
    {
        Debug.Log(CodeManager.GetMethodName() + newCount);

        GlobalDefine.UserInfo.Booster_Missile = Mathf.Max(0, newCount);
        GlobalDefine.UserInfo.Booster_Lightning = Mathf.Max(0, newCount);
        GlobalDefine.UserInfo.Booster_Bomb = Mathf.Max(0, newCount);

        SaveUserData();
    }

    public static void SetClear(int level)
    {
        Debug.Log(CodeManager.GetMethodName() + level);

        GlobalDefine.UserInfo.LevelCurrent = Mathf.Min(CLevelInfoTable.Inst.levelCount, level + 1);
        
        SaveUserData();
    }
}
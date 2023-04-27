using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GlobalDefine
{
    ///<Summary>[인게임] 하단 아이템 구매 가격.</Summary>
    public const int CostRuby_BottomItem = 100;
    ///<Summary>[레벨 준비] 부스터 구매 가격.</Summary>
    public const int CostRuby_Booster = 100;
    ///<Summary>[레벨 준비] 골든 에임 구매 가격.</Summary>
    public const int CostRuby_GoldenAim = 50;
    ///<Summary>[레벨 실패 - 광고 시청으로 대체 가능] 3줄 제거 및 컨티뉴 가격.</Summary>
    public const int CostRuby_Continue_Remove3Lines = 200;

    ///<Summary>[레벨 실패 - 광고 시청] 루비+20 (3회).</Summary>
    public const int RewardRuby_Continue = 20;

    public static void OpenShop()
    {
        CSceneManager.GetSceneManager<OverlayScene.CSubOverlaySceneManager>(KCDefine.B_SCENE_N_OVERLAY)?.ShowStorePopup();
    }

    public static void AddRuby(int addCount)
    {
        Debug.Log(CodeManager.GetMethodName() + addCount);

        GlobalDefine.UserInfo.Ruby = Mathf.Max(0, GlobalDefine.UserInfo.Ruby + addCount);

        SaveUserData();
    }

    public static void AddStar(int addCount)
    {
        Debug.Log(CodeManager.GetMethodName() + addCount);

        GlobalDefine.UserInfo.Star = Mathf.Max(0, GlobalDefine.UserInfo.Star + addCount);

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

    public static void RefreshShopText(TMPro.TMP_Text rubyText)
    {
        if (rubyText != null)
            rubyText.text = string.Format(GlobalDefine.FORMAT_INT, GlobalDefine.UserInfo.Ruby);
    }

    public static void RefreshShopText(UnityEngine.UI.Text rubyText)
    {
        if (rubyText != null)
            rubyText.text = string.Format(GlobalDefine.FORMAT_INT, GlobalDefine.UserInfo.Ruby);
    }

    public static void RefreshStarText(TMPro.TMP_Text rubyText)
    {
        if (rubyText != null)
            rubyText.text = string.Format(GlobalDefine.FORMAT_INT, GlobalDefine.UserInfo.Star);
    }

    public static void RefreshStarText(UnityEngine.UI.Text rubyText)
    {
        if (rubyText != null)
            rubyText.text = string.Format(GlobalDefine.FORMAT_INT, GlobalDefine.UserInfo.Star);
    }
}
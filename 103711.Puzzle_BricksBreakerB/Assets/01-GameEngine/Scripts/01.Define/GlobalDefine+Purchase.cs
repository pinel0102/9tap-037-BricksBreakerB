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

    ///<Summary>[레벨 실패 - 광고 시청] 루비+20.</Summary>
    public const int RewardRuby_Continue = 20;
    ///<Summary>[출석 체크 - 광고 시청] 루비+50 (1회).</Summary>
    public const int RewardRuby_Daily = 50;
    public const int RewardRuby_Daily_Store_Min = 5;
    public const int RewardRuby_Daily_Store_Max = 40;

    public static void OpenShop()
    {
        CSceneManager.GetSceneManager<OverlayScene.CSubOverlaySceneManager>(KCDefine.B_SCENE_N_OVERLAY)?.ShowStorePopup();
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
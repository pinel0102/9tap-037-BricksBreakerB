using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GlobalDefine
{
    public const float Time_FullscreenAD = 90f;

    public static void CloseBannerAD()
    {
        Debug.Log(CodeManager.GetMethodName());
#if ADS_MODULE_ENABLE
        Func.CloseBannerAds(null);
#endif
    }

    public static void HideBannerAD()
    {
        Debug.Log(CodeManager.GetMethodName());
#if ADS_MODULE_ENABLE
        Func.HideBannerAds(null);
#endif
    }

    public static void RequestBannerAD()
    {
        if(IsEnableAD())
        {
            Debug.Log(CodeManager.GetMethodName());
            Func.ShowBannerAds(null);
        }
        else
        {
            CloseBannerAD();
        }
    }

    public static bool IsEnableRewardVideo()
    {
        if(isLevelEditor)
            return true;

#if ADS_MODULE_ENABLE && !UNITY_EDITOR && !UNITY_STANDALONE
        return CAdsManager.Inst.IsLoadRewardAds(CCommonAppInfoStorage.Inst.AdsPlatform);
#else
        return true;
#endif
    }

    public static bool IsEnableAD()
    {
        return !CUserInfoStorage.Inst.IsPurchaseRemoveAds;
    }
}

///<Summary>리워드 광고 특수 보상 (1회성).</Summary>
public enum RewardVideoType
{
    NONE,
    CONTINUE_3LINE,
    LEVELMAP_BOOSTER,
    READY_BOOSTER,
    INGAME_BALLPLUS,
}
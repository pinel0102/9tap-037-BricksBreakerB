using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GlobalDefine
{
    public const float Time_FullscreenAD = 90f;

    public static void HideBannerAD()
    {
        Debug.Log(CodeManager.GetMethodName());
#if ADS_MODULE_ENABLE
        Func.CloseBannerAds(null);
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
            HideBannerAD();
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

public enum RewardVideoType
{
    CONTINUE_3LINE,
    CONTINUE_RUBY,
    DAILY_FREE_REWARD,
}
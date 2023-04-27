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

        CAdsManager.Inst.IsEnableBannerAds = false;
        CAdsManager.Inst.IsEnableFullscreenAds = false;
#endif
    }

    public static void RequestBannerAD()
    {
        if(IsEnableAD())
        {
            Debug.Log(CodeManager.GetMethodName());
            //
        }
        else
        {
            HideBannerAD();
        }
    }

    public static void RequestFullscreenAD()
    {
        if(IsEnableAD())
        {
            Debug.Log(CodeManager.GetMethodName());
            //
        }
    }

    public static void RequestRewardVideo(RewardVideoType type, CPopup popup)
    {
        if(IsEnableRewardVideo())
        {
            Debug.Log(CodeManager.GetMethodName());
            //
        }
    }

    public static bool IsEnableRewardVideo()
    {
        if(isLevelEditor)
            return true;

        return true;
    }

    public static bool IsEnableAD()
    {
        return !GlobalDefine.UserInfo.Item_ADBlock;
    }
}

public enum RewardVideoType
{
    CONTINUE_3LINE,
    CONTINUE_RUBY,
    DAILY_REWARD,
}
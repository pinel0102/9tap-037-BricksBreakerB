using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GlobalDefine
{
    public static void HideBannerAD()
    {
        Debug.Log(CodeManager.GetMethodName());
        //
    }

    public static void RequestBannerAD()
    {
        if(IsEnableAD())
        {
            Debug.Log(CodeManager.GetMethodName());
            //
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

    public static void RequestRewardVideo(NSEngine.RewardVideoType type, CPopup popup)
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
        return !CUserInfoStorage.Inst.UserInfo.Item_ADBlock;
    }
}
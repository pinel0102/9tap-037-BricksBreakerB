using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GlobalDefine
{
    public static CUserInfo UserInfo => CUserInfoStorage.Inst.UserInfo;

    public static void LoadUserData()
    {
        Debug.Log(CodeManager.GetMethodName());
        
        CUserInfoStorage.Inst.LoadUserInfo();
    }

    public static void SaveUserData()
    {
        //Debug.Log(CodeManager.GetMethodName());

        CUserInfoStorage.Inst.SaveUserInfo();
    }

    public static void ResetUserData()
    {
        Debug.Log(CodeManager.GetMethodName());

        CUserInfo userInfo = new CUserInfo
        {
            Settings_PlayerName = GlobalDefine.PLAYER_NAME_DEFAULT,
            Settings_Avatar = 0,
            Settings_Frame = 0,
            Settings_DarkMode = false,
            Ruby = 0,
            Star = 0,
            Item_ADBlock = false, 
            Item_Earthquake = 0,
            Item_AddBall = 0,
            Item_BricksDelete = 0,
            Item_AddLaserBricks = 0,
            Item_AddSteelBricks = 0,
            Booster_Missile = 0,
            Booster_Lightning = 0,
            Booster_Bomb = 0,
            LevelCurrent = 1,
            LevelStar = string.Empty,
            LevelScore = string.Empty,
            LevelSkip = string.Empty,
        };

        CUserInfoStorage.Inst.ResetUserInfo(CExtension.ExToMsgPackBase64Str(userInfo));
    }
}
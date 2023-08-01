using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GlobalDefine
{
    public static CUserInfo UserInfo => CUserInfoStorage.Inst.UserInfo;
    public static bool hasGoldenAim { get; private set; }
    public static List<int> UserStarList;
    public static List<int> UserScoreList;

    public const string ArrayDefault = "0";

    public static void LoadUserData()
    {
        Debug.Log(CodeManager.GetMethodName());
        
        CUserInfoStorage.Inst.LoadUserInfo();
    }

    public static void SetGoldenAim(bool value)
    {
        hasGoldenAim = value;
    }

    public static void InitUserDataList()
    {
        UserStarList = CSVToList(UserInfo.LevelStar);
        UserScoreList = CSVToList(UserInfo.LevelScore);

        int levelCount = CLevelInfoTable.Inst.levelCount;

        if (UserStarList.Count < levelCount)
        {
            int oldCount = UserStarList.Count;
            for (int i=0; i < levelCount - oldCount; i++)
            {
                UserStarList.Add(0);
            }
            
            UserInfo.LevelStar = ListToCSV(UserStarList);
            SaveUserData();
        }

        if (UserScoreList.Count < levelCount)
        {
            int oldCount = UserScoreList.Count;
            for (int i=0; i < levelCount - oldCount; i++)
            {
                UserScoreList.Add(0);
            }

            UserInfo.LevelScore = ListToCSV(UserScoreList);
            SaveUserData();
        }
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
            Item_Earthquake = 0,
            Item_AddBall = 0,
            Item_BricksDelete = 0,
            Item_AddLaserBricks = 0,
            Item_AddSteelBricks = 0,
            LimitedItemCount_Balloon = 0,
            LimitedItemCount_Earthquake = 0,
            LimitedItemCount_AddBall = 0,
            LimitedItemCount_BricksDelete = 0,
            LimitedItemCount_AddLaserBricks = 0,
            LimitedItemCount_AddSteelBricks = 0,
            LimitedStartTime_Balloon = GlobalDefine.STRING_DATE_EMPTY,
            LimitedStartTime_Earthquake = GlobalDefine.STRING_DATE_EMPTY,
            LimitedStartTime_AddBall = GlobalDefine.STRING_DATE_EMPTY,
            LimitedStartTime_BricksDelete = GlobalDefine.STRING_DATE_EMPTY,
            LimitedStartTime_AddLaserBricks = GlobalDefine.STRING_DATE_EMPTY,
            LimitedStartTime_AddSteelBricks = GlobalDefine.STRING_DATE_EMPTY,
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

    public static List<int> CreateEmptyList()
    {
        List<int> list = new List<int>();
        int levelCount = CLevelInfoTable.Inst.levelCount;
        
        for(int i=0; i < levelCount; i++)
        {
            list.Add(0);
        }

        return list;
    }        
}
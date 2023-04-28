using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GlobalDefine
{
    public static void ResultCalculate(NSEngine.CEngine Engine)
    {
        int level = Engine.currentLevel - 1;

        Debug.Log(CodeManager.GetMethodName() + level);

        int starIncrease = 0;

        if (UserStarList[level] < Engine.starCount)
        {
            starIncrease = Engine.starCount - UserStarList[level];
            UserStarList[level] = Engine.starCount;

            UserInfo.LevelStar = ListToCSV(UserStarList);
        }

        if (UserScoreList[level] < Engine.currentScore)
        {
            UserScoreList[level] = Engine.currentScore;

            UserInfo.LevelScore = ListToCSV(UserScoreList);
        }

        if (starIncrease > 0)
        {
            UserInfo.Star += starIncrease;
        }

        SaveUserData();
    }
}
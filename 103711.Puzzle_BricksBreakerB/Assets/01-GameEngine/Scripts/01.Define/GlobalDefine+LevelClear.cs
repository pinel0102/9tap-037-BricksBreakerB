using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GlobalDefine
{
    public static readonly List<List<KeyValuePair<EItemKinds, int>>> starReward = new List<List<KeyValuePair<EItemKinds, int>>>()
    {
        new List<KeyValuePair<EItemKinds, int>>()
        {
            new KeyValuePair<EItemKinds, int>(EItemKinds.GAME_ITEM_02_ADD_BALLS, 1),
            new KeyValuePair<EItemKinds, int>(EItemKinds.GOODS_RUBY, 20),
            new KeyValuePair<EItemKinds, int>(EItemKinds.GAME_ITEM_01_EARTHQUAKE, 1)
        },
        new List<KeyValuePair<EItemKinds, int>>()
        {
            new KeyValuePair<EItemKinds, int>(EItemKinds.GAME_ITEM_02_ADD_BALLS, 1),
            new KeyValuePair<EItemKinds, int>(EItemKinds.GOODS_RUBY, 20),
            new KeyValuePair<EItemKinds, int>(EItemKinds.GAME_ITEM_03_BRICKS_DELETE, 1)
        },
        new List<KeyValuePair<EItemKinds, int>>()
        {
            new KeyValuePair<EItemKinds, int>(EItemKinds.GAME_ITEM_02_ADD_BALLS, 1),
            new KeyValuePair<EItemKinds, int>(EItemKinds.GOODS_RUBY, 20),
            new KeyValuePair<EItemKinds, int>(EItemKinds.GAME_ITEM_04_ADD_LASER_BRICKS, 1)
        },
        new List<KeyValuePair<EItemKinds, int>>()
        {
            new KeyValuePair<EItemKinds, int>(EItemKinds.GAME_ITEM_02_ADD_BALLS, 1),
            new KeyValuePair<EItemKinds, int>(EItemKinds.GOODS_RUBY, 20),
            new KeyValuePair<EItemKinds, int>(EItemKinds.GAME_ITEM_05_ADD_STEEL_BRICKS, 1)
        },
        new List<KeyValuePair<EItemKinds, int>>()
        {
            new KeyValuePair<EItemKinds, int>(EItemKinds.GAME_ITEM_02_ADD_BALLS, 1),
            new KeyValuePair<EItemKinds, int>(EItemKinds.GOODS_RUBY, 20),
            new KeyValuePair<EItemKinds, int>(EItemKinds.BOOSTER_ITEM_01_MISSILE, 1)
        },
        new List<KeyValuePair<EItemKinds, int>>()
        {
            new KeyValuePair<EItemKinds, int>(EItemKinds.GAME_ITEM_02_ADD_BALLS, 1),
            new KeyValuePair<EItemKinds, int>(EItemKinds.GOODS_RUBY, 20),
            new KeyValuePair<EItemKinds, int>(EItemKinds.BOOSTER_ITEM_02_LIGHTNING, 1)
        },
        new List<KeyValuePair<EItemKinds, int>>()
        {
            new KeyValuePair<EItemKinds, int>(EItemKinds.GAME_ITEM_02_ADD_BALLS, 1),
            new KeyValuePair<EItemKinds, int>(EItemKinds.GOODS_RUBY, 20),
            new KeyValuePair<EItemKinds, int>(EItemKinds.BOOSTER_ITEM_03_BOMB, 1)
        }
    };

    public static readonly List<int> starRewardPoint = new List<int>(){3, 13, 28};
    public static int starIncrease;

    public static void ResultCalculate(NSEngine.CEngine Engine)
    {
        int level = Engine.currentLevel - 1;

        Debug.Log(CodeManager.GetMethodName() + Engine.currentLevel);

        starIncrease = 0;

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

        UserInfo.Star = GetStarSum();

        SaveUserData();
    }

    public static int GetStarSum()
    {
        int count = 0;

        for(int i=0; i < UserStarList.Count; i++)
        {
            count += UserStarList[i];
        }

        return count;
    }
}
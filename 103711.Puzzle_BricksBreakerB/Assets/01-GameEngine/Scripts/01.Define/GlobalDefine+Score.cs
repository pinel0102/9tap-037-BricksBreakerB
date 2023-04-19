using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static partial class GlobalDefine
{
    public const int SCORE_DEFAULT = 10;
    public const int SCORE_COMBO_BONUS = 10;
    
    public const float SCORE_TEXT_DURATION = 0.5f;
    public const float SCORE_GAGE_DURATION = 0.5f;
    public const string FORMAT_SCORE = "Score: {0}";

    private const int STAR_SCORE_1 = 100;
    private const int STAR_SCORE_2_MULTIPLIER = 50;
    private const int STAR_SCORE_3_MULTIPLIER = 80;

    public static List<int> GetLevelScoreList(CLevelInfo levelInfo)
    {
        int targetCellCount = GetTargetCellCount(levelInfo);
        List<int> scoreList = new List<int>();

        scoreList.Add(STAR_SCORE_1);
        scoreList.Add(STAR_SCORE_2_MULTIPLIER * targetCellCount);
        scoreList.Add(STAR_SCORE_3_MULTIPLIER * targetCellCount);

        return scoreList.OrderBy(g => g).ToList();
    }
        
}
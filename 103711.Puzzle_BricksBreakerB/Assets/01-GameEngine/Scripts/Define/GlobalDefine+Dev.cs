using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
public static partial class GlobalDefine
{
    private static readonly Dictionary<EObjKinds, KeyValuePair<string, bool>> devComplete = new Dictionary<EObjKinds, KeyValuePair<string, bool>>()
    {
        [EObjKinds.NORM_BRICKS_SQUARE_01]                   = new KeyValuePair<string, bool>("정사각형", true),
        [EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_01]           = new KeyValuePair<string, bool>("직각삼각형", true),
        [EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_02]           = new KeyValuePair<string, bool>("직각삼각형", true),
        [EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_03]           = new KeyValuePair<string, bool>("직각삼각형", true),
        [EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_04]           = new KeyValuePair<string, bool>("직각삼각형", true),
        [EObjKinds.NORM_BRICKS_TRIANGLE_01]                 = new KeyValuePair<string, bool>("정삼각형", true),
        [EObjKinds.NORM_BRICKS_TRIANGLE_02]                 = new KeyValuePair<string, bool>("정삼각형", true),
        [EObjKinds.NORM_BRICKS_TRIANGLE_03]                 = new KeyValuePair<string, bool>("정삼각형", true),
        [EObjKinds.NORM_BRICKS_TRIANGLE_04]                 = new KeyValuePair<string, bool>("정삼각형", true),
        [EObjKinds.NORM_BRICKS_DIAMOND_01]                  = new KeyValuePair<string, bool>("마름모", true),

        [EObjKinds.OBSTACLE_BRICKS_KEY_01]                  = new KeyValuePair<string, bool>("열쇠", false),
        [EObjKinds.OBSTACLE_BRICKS_LOCK_01]                 = new KeyValuePair<string, bool>("자물쇠", false),
        [EObjKinds.OBSTACLE_BRICKS_WARP_IN_01]              = new KeyValuePair<string, bool>("워프 IN", false),
        [EObjKinds.OBSTACLE_BRICKS_WARP_OUT_01]             = new KeyValuePair<string, bool>("워프 OUT", false),
        [EObjKinds.OBSTACLE_BRICKS_WOODBOX_01]              = new KeyValuePair<string, bool>("나무 상자", false),
        [EObjKinds.OBSTACLE_BRICKS_OPEN_01]                 = new KeyValuePair<string, bool>("강철 블럭 (열림)", false),
        [EObjKinds.OBSTACLE_BRICKS_CLOSE_01]                = new KeyValuePair<string, bool>("강철 블럭 (닫힘)", false),
        [EObjKinds.OBSTACLE_BRICKS_FIX_01]                  = new KeyValuePair<string, bool>("고정 블럭", false),
        [EObjKinds.OBSTACLE_BRICKS_FIX_02]                  = new KeyValuePair<string, bool>("고정 나무 상자", false),
        [EObjKinds.OBSTACLE_BRICKS_FIX_03]                  = new KeyValuePair<string, bool>("고정 강철 블럭", false),

        [EObjKinds.SPECIAL_BRICKS_MISSILE_01]               = new KeyValuePair<string, bool>("미사일 x1", false),
        [EObjKinds.SPECIAL_BRICKS_MISSILE_02]               = new KeyValuePair<string, bool>("미사일 x4", false),
        [EObjKinds.SPECIAL_BRICKS_EXPLOSION_HORIZONTAL_01]  = new KeyValuePair<string, bool>("가로 폭탄", true),
        [EObjKinds.SPECIAL_BRICKS_EXPLOSION_VERTICAL_01]    = new KeyValuePair<string, bool>("세로 폭탄", true),
        [EObjKinds.SPECIAL_BRICKS_EXPLOSION_CROSS_01]       = new KeyValuePair<string, bool>("십자 폭탄", true),
        [EObjKinds.SPECIAL_BRICKS_EXPLOSION_AROUND_01]      = new KeyValuePair<string, bool>("3x3 폭탄", true),
        [EObjKinds.SPECIAL_BRICKS_ADD_BALL_01]              = new KeyValuePair<string, bool>("볼 블럭 +10", true),
        [EObjKinds.SPECIAL_BRICKS_ADD_BALL_02]              = new KeyValuePair<string, bool>("볼 블럭 +20", true),
        [EObjKinds.SPECIAL_BRICKS_ADD_BALL_03]              = new KeyValuePair<string, bool>("볼 블럭 +30", true),
        [EObjKinds.SPECIAL_BRICKS_LIGHTNING_01]             = new KeyValuePair<string, bool>("번개", false),
        [EObjKinds.SPECIAL_BRICKS_EARTHQUAKE_01]            = new KeyValuePair<string, bool>("지진", false),
        [EObjKinds.SPECIAL_BRICKS_ARROW_01]                 = new KeyValuePair<string, bool>("화살", true),
        [EObjKinds.SPECIAL_BRICKS_ARROW_02]                 = new KeyValuePair<string, bool>("화살", true),
        [EObjKinds.SPECIAL_BRICKS_ARROW_03]                 = new KeyValuePair<string, bool>("화살", true),
        [EObjKinds.SPECIAL_BRICKS_ARROW_04]                 = new KeyValuePair<string, bool>("화살", true),
        [EObjKinds.SPECIAL_BRICKS_ARROW_05]                 = new KeyValuePair<string, bool>("화살", true),
        [EObjKinds.SPECIAL_BRICKS_ARROW_06]                 = new KeyValuePair<string, bool>("화살", true),
        [EObjKinds.SPECIAL_BRICKS_ARROW_07]                 = new KeyValuePair<string, bool>("화살", true),
        [EObjKinds.SPECIAL_BRICKS_ARROW_08]                 = new KeyValuePair<string, bool>("화살", true),
        [EObjKinds.SPECIAL_BRICKS_MORPH_01]                 = new KeyValuePair<string, bool>("변형", false),
        [EObjKinds.SPECIAL_BRICKS_MORPH_02]                 = new KeyValuePair<string, bool>("변형", false),
        [EObjKinds.SPECIAL_BRICKS_MORPH_03]                 = new KeyValuePair<string, bool>("변형", false),
        [EObjKinds.SPECIAL_BRICKS_MORPH_04]                 = new KeyValuePair<string, bool>("변형", false),
        
        [EObjKinds.SPECIAL_BRICKS_LASER_HORIZONTAL_01]      = new KeyValuePair<string, bool>("가로 레이저", true),
        [EObjKinds.SPECIAL_BRICKS_LASER_VERTICAL_01]        = new KeyValuePair<string, bool>("세로 레이저", true),
        [EObjKinds.SPECIAL_BRICKS_LASER_CROSS_01]           = new KeyValuePair<string, bool>("십자 레이저", true),
        [EObjKinds.SPECIAL_BRICKS_BALL_DIFFUSION_01]        = new KeyValuePair<string, bool>("볼 확산", true),
        [EObjKinds.SPECIAL_BRICKS_BALL_AMPLIFICATION_01]    = new KeyValuePair<string, bool>("볼 증폭", true),
        [EObjKinds.SPECIAL_BRICKS_POWERBALL_01]             = new KeyValuePair<string, bool>("파워 볼", true),

        [EObjKinds.ITEM_BRICKS_BALL_01]                     = new KeyValuePair<string, bool>("볼 추가 +1", true),
        [EObjKinds.ITEM_BRICKS_BALL_02]                     = new KeyValuePair<string, bool>("볼 추가 +2", true),
        [EObjKinds.ITEM_BRICKS_BALL_03]                     = new KeyValuePair<string, bool>("볼 추가 +3", true),
        [EObjKinds.ITEM_BRICKS_BALL_04]                     = new KeyValuePair<string, bool>("볼 추가 +10", true),
        [EObjKinds.ITEM_BRICKS_COINS_01]                    = new KeyValuePair<string, bool>("보석 획득", false),
    };

    private const string formatDevTooltip = "{0}{1}";
    private const string strOnDevelopment = "\n<color=red>(개발중)</color>";

    public static string GetTooltipText(EObjKinds kinds)
    {
        if (devComplete.ContainsKey(kinds))
            return string.Format(formatDevTooltip, devComplete[kinds].Key, devComplete[kinds].Value ? string.Empty : strOnDevelopment);
        else
            return string.Empty;
    }
}
#endif
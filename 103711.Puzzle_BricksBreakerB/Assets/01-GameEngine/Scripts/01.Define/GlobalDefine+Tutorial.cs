using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GlobalDefine
{
    public const int TUTORIAL_LEVEL_BRICKS_DELETE = 8;
    public const int TUTORIAL_LEVEL_ADD_BALL = -1;
    public const int TUTORIAL_LEVEL_EARTHQUAKE = 13;
    public const int TUTORIAL_LEVEL_ADD_LASER_BRICKS = -2;
    public const int TUTORIAL_LEVEL_ADD_STEEL_BRICKS = 26;

    public const int BOOSTER_LEVEL_MISSILE = 41;
    public const int BOOSTER_LEVEL_LIGHTNING = 55;
    public const int BOOSTER_LEVEL_EXPLOSION = 70;

    public static readonly List<int> TUTORIAL_LEVEL_BOTTOM_ITEM = new List<int>()
    {
        TUTORIAL_LEVEL_EARTHQUAKE,
        TUTORIAL_LEVEL_ADD_BALL,
        TUTORIAL_LEVEL_BRICKS_DELETE,
        TUTORIAL_LEVEL_ADD_LASER_BRICKS,
        TUTORIAL_LEVEL_ADD_STEEL_BRICKS
    };

    public static readonly List<string> TUTORIAL_TEXT_BOTTOM_ITEM = new List<string>()
    {
        "Earthquake powerup give 40% damage to all bricks. Try it free!",
        "Add Ball powerup add 30 one-time balls. Try it free!",
        "Line Delete powerup delete bottom lines. Try it free!",
        "Add Laser Bricks add 4 laser bricks. Try it free!",
        "Steel bricks add one-time steel bricks to floor. Try it free!"
    };

    public static readonly List<int> BOOSTER_LEVEL = new List<int>()
    {
        BOOSTER_LEVEL_MISSILE,
        BOOSTER_LEVEL_LIGHTNING,
        BOOSTER_LEVEL_EXPLOSION,
    };
}
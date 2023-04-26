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

    ///<Summary>아이템이 해금되는 레벨.</Summary>
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
        "The Quake power-up\nDeals a little damage to every block. Try it free!",
        "The Bonus Balls power-up\nGives you extra balls on your next shot. Try it free!",
        "The Line Clear power-up\nRemoves the bottom-most row of bricks. Try it free!",
        "The Laser power-up\nRandomly places 4 lasers on the screen. Try it free!",
        "The Iron Blocks power-up\nAdds iron blocks at the bottom of the screen. Try it free!"
    };

    ///<Summary>부스터가 해금되는 레벨.</Summary>
    public static readonly List<int> BOOSTER_LEVEL = new List<int>()
    {
        BOOSTER_LEVEL_MISSILE,
        BOOSTER_LEVEL_LIGHTNING,
        BOOSTER_LEVEL_EXPLOSION,
    };
}
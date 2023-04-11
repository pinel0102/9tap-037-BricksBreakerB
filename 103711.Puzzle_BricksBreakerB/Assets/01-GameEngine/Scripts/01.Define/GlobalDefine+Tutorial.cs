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

    public static readonly List<int> TUTORIAL_LEVEL_BOTTOM_ITEM = new List<int>()
    {
        TUTORIAL_LEVEL_EARTHQUAKE,
        TUTORIAL_LEVEL_ADD_BALL,
        TUTORIAL_LEVEL_BRICKS_DELETE,
        TUTORIAL_LEVEL_ADD_LASER_BRICKS,
        TUTORIAL_LEVEL_ADD_STEEL_BRICKS
    };
}
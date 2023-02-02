using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalDefine
{
    // [Item] Ball Plus
    public readonly static int[] GetItem_BallPlus = new int[4]
    {
        1, 2, 3, 10
    };

    // [Bricks] Color
    public readonly static Color[] BricksColor = new Color[3]
    {
        // Item & Special
        new Color(1f, 1f, 1f, 1f),
        // Bricks
        new Color(64f/255f, 122f/255f, 217f/255f, 1f),
        // Ball
        new Color(1f, 0, 0, 1f)
    };

    public const float SHOOT_BALL_DELAY = 0.09f;

    // [Effect] Laser
    public static Vector3 Rotation_Horizontal = new Vector3(0, 0, 90f);
    public static Vector3 Rotation_Vertictal = Vector3.zero;
    public const float EffectTime_Laser = 0.01f;

    // [Layer] LayerMask
    public const string LAYER_FX = "FX";
    public const string LAYER_BALL = "Ball";
    public const string LAYER_WALL = "Wall";
    public const string LAYER_CELL_EMPTY = "Cell_Empty";
    public const string LAYER_CELL_BRICK = "Cell_Brick";
    public const string LAYER_CELL_OBSTACLE = "Cell_Obstacle";
    public const string LAYER_CELL_ITEM = "Cell_Item";
    public const string LAYER_CELL_SPECIAL = "Cell_Special";
    public const float RAYCAST_DISTANCE = 1000f;
}

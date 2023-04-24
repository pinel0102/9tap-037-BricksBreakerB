using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GlobalDefine
{
    public const float SCREEN_WIDTH = 720f;
    public const float SCREEN_HEIGHT = 1280f;

    // 위 아래 UI 공간.
    public const float GRID_PANEL_HEIGHT_TOP = 180f;
    public const float GRID_PANEL_HEIGHT_BOTTOM = 180f;

    // [GRID] GRID
    public const int GRID_DOWN_OFFSET = 10;
    public readonly static float[] GRID_Y_OFFSET = new float[20]
    {
        0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, -16, -30, 8, -4, -16, 13, 4, -8, -16
    };

    public const int AIM_BOUND_COUNT_NORMAL = 1;
    public const int AIM_BOUND_COUNT_GOLDEN = 3;
    public const float AIM_LENGTH_SHORT = 220.0f;
    public const float AIM_DOT_OFFSET = 35.0f;

    // [Item] Ruby
    public const int GetItem_Ruby = 1;

    // [Item] Ball Plus
    public readonly static int[] GetItem_BallPlus = new int[4]
    {
        1, 2, 3, 10
    };

    // [Item] Ball Plus
    public readonly static int[] GetSpecial_AddBall = new int[3]
    {
        10, 20, 30
    };

    /// <Summary>셀 스프라이트 크기 보정.</Summary>
    public static Vector3 CELL_SPRITE_ADJUSTMENT = new Vector3(-4, -4, 0);
    /// <Summary>셀 루트가 내려오는 속도.</Summary>
    public static Vector3 CELL_ROOT_MOVE_SPEED = new Vector3(0, 10f, 0);

    // [Engine] Timer Delay
    public const float SHOOT_BALL_DELAY = 0.05f;

    

    // [Layer] LayerMask
    public const string LAYER_FX = "FX";
    public const string LAYER_BALL = "Ball";
    public const string LAYER_WALL = "Wall";
    public const string LAYER_CELL_EMPTY = "Cell_Empty";
    public const string LAYER_CELL_BRICK = "Cell_Brick";
    public const string LAYER_CELL_OBSTACLE_REFLECT = "Cell_Obstacle_Reflect";
    public const string LAYER_CELL_OBSTACLE_THROUGH = "Cell_Obstacle_Through";
    public const string LAYER_CELL_SPECIAL_REFLECT = "Cell_Special_Reflect";
    public const string LAYER_CELL_SPECIAL_THROUGH = "Cell_Special_Through";
    public const string LAYER_CELL_ITEM = "Cell_Item";
    public const float RAYCAST_DISTANCE = 3000f;
    public const float ColliderRadius_20 = 20f;
    public const float ColliderRadius_30 = 30f;

    public static Vector2 ColliderOffset_EXPLOSION_ALL = new Vector2(0, -23);
    public static Vector3 CenterOffset_EXPLOSION_ALL = new Vector3(0, -35, 0);
    public static Vector2 GetHPText_Offset(EObjKinds kinds)
    {
        switch(kinds)
        {
            case EObjKinds.SPECIAL_BRICKS_EXPLOSION_ALL_01:
                return CenterOffset_EXPLOSION_ALL;
            default:
                return Vector2.zero;
        }
    }

    public static Vector2 GetEffect_Offset(EObjKinds kinds)
    {
        switch(kinds)
        {
            case EObjKinds.SPECIAL_BRICKS_EXPLOSION_ALL_01:
                return ColliderOffset_EXPLOSION_ALL;
            default:
                return Vector2.zero;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GlobalDefine
{
    // 위 아래 UI 공간.
    public const float GRID_PANEL_HEIGHT_TOP = 180f;
    public const float GRID_PANEL_HEIGHT_BOTTOM = 180f;

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
    public const string LAYER_CELL_OBSTACLE_REFLECT = "Cell_Obstacle_Reflect";
    public const string LAYER_CELL_OBSTACLE_THROUGH = "Cell_Obstacle_Through";
    public const string LAYER_CELL_SPECIAL_REFLECT = "Cell_Special_Reflect";
    public const string LAYER_CELL_SPECIAL_THROUGH = "Cell_Special_Through";
    public const string LAYER_CELL_ITEM = "Cell_Item";
    public const float RAYCAST_DISTANCE = 3000f;

    // [ETC] Sorting Order
    public const int HitEffect_Order = 100;
    public const int HPText_Order = 200;
    public const int NumText_Order = 10;

    // [UI] Strings : Aim Button
    public const string formatAimText = "AIM\n{0}";
    public const string textON = "ON";
    public const string textOFF = "OFF";    

    public static Vector2 CustomSize_Diffusion = new Vector2(30f, 30f);
}

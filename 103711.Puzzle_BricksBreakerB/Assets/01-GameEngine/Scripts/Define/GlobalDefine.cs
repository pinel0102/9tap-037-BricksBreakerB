using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalDefine
{
    // 위 아래 UI 공간.
    public const float GRID_PANEL_HEIGHT_TOP = 180f;
    public const float GRID_PANEL_HEIGHT_BOTTOM = 180f;

    // [Item] Ball Plus
    public readonly static int[] GetItem_BallPlus = new int[4]
    {
        1, 2, 3, 10
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
    public const string LAYER_CELL_OBSTACLE = "Cell_Obstacle";
    public const string LAYER_CELL_ITEM = "Cell_Item";
    public const string LAYER_CELL_SPECIAL = "Cell_Special";
    public const float RAYCAST_DISTANCE = 3000f;

    // [ETC] Sorting Order
    public const int HitEffect_Order = 100;
    public const int HPText_Order = 200;
    public const int NumText_Order = 10;

    // [UI] Strings : Aim Button
    public const string formatAimText = "AIM\n{0}";
    public const string textON = "ON";
    public const string textOFF = "OFF";


    // [JSON] Cell Color
    public const string COLOR_CELL_DEFAULT = "#FFFFFFFF";
    public const string COLOR_BRICKS_DEFAULT = "#407AD9FF";
    public const string COLOR_BALL_DEFAULT = "#FF0000FF";

    public static Color GetCellColor(EObjType cellType, string _colorHex = GlobalDefine.COLOR_CELL_DEFAULT)
    {
        switch(cellType)
        {
            case EObjType.BALL:
                _colorHex = GlobalDefine.COLOR_BALL_DEFAULT;
                break;
            case EObjType.NORM_BRICKS:
                break;
            default:
                _colorHex = GlobalDefine.COLOR_CELL_DEFAULT;
                break;
        }

        //Debug.Log(CodeManager.GetMethodName() + string.Format("{0}", _colorHex));

        return ColorUtility.TryParseHtmlString(_colorHex, out Color _color) ? _color : Color.white;
    }
}

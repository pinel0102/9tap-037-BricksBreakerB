using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GlobalDefine
{
    public const string NOT_SUPPORTED_BRICK = "NOT_SUPPORTED_BRICK";
    public const string OBJECTNAME_SUB_SPRITE = "SubSprite";
    public static readonly string PREFAB_SUB_SPRITE = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_LEVEL_EDITOR_SCENE}SubSprite";
    public static readonly string PREFAB_EDITOR_CURSOR = $"{KCDefine.B_DIR_P_PREFABS}{KCDefine.B_DIR_P_LEVEL_EDITOR_SCENE}Cursor";

    public const string PREFAB_LEVEL_LIST_LEFT = "Prefabs/UI/LevelMap/LevelList_Left";
    public const string PREFAB_LEVEL_LIST_RIGHT = "Prefabs/UI/LevelMap/LevelList_Right";

    public static Vector3 GetSpriteCenter(Vector3 cellSize, Vector3Int objSize)
    {
        if (objSize != Vector3Int.one)
        {
            float width = cellSize.x * objSize.x;
            float height = cellSize.y * objSize.y;
            Vector2 LeftUpPoint = new Vector2(cellSize.x / -KCDefine.B_VAL_2_REAL, cellSize.y / KCDefine.B_VAL_2_REAL);
            Vector2 CenterPoint = new Vector2(LeftUpPoint.x + (width / KCDefine.B_VAL_2_REAL), LeftUpPoint.y - (height / KCDefine.B_VAL_2_REAL));

            return CenterPoint;
        }
        else
        {
            return Vector3.zero;
        }
    }

    public static Vector3 GetSpriteSize(Vector3 cellSize, Vector3Int objSize)
    {
        Vector3 size = cellSize;
        if (objSize != Vector3Int.one)
        {
            size = Vector3.Scale(cellSize, objSize);
        }
        return size;
    }
}
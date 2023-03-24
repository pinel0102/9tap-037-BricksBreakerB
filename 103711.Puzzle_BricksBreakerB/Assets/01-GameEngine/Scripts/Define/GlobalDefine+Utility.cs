using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GlobalDefine
{
    public static float GetAngle(Vector2 from, Vector2 to)
    {
        Vector2 offset = to - from;
        return Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
    }
}

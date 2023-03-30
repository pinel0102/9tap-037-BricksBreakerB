using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GlobalDefine
{
    public const string formatVersion = "v{0}";
    public const string formatVersionWithAppName = "{1} v{0}";

    public static float GetAngle(Vector2 from, Vector2 to)
    {
        Vector2 offset = to - from;
        return Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
    }

    public static string GetVersionText()
    {
        return string.Format(formatVersion, Application.version);
    }

    public static string GetVersionTextWithAppName()
    {
        return string.Format(formatVersionWithAppName, Application.version, Application.productName);
    }
}

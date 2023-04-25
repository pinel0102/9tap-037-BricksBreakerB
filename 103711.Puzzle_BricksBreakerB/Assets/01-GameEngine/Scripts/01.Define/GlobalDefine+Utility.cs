using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GlobalDefine
{
    public static bool isLevelEditor { get; private set; }

    public const string formatVersion = "v{0}";
    public const string formatVersionWithAppName = "{1} v{0}";

    public static void ThisIsLevelEditor(bool _isLevelEditor)
    {
        isLevelEditor = _isLevelEditor;
    }

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

    public static float Root(float num)
    {
        return Mathf.Pow(num, 0.5f);
    }
}

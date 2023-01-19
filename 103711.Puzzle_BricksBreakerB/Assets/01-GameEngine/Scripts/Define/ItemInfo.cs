using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemInfo
{
    public readonly static int[] GetItem_BallPlus = new int[4]
    {
        1, 2, 3, 10
    };

    public readonly static Color[] BricksColor = new Color[3]
    {
        // Item & Special
        new Color(1f, 1f, 1f, 1f),
        // Bricks
        new Color(64f/255f, 122f/255f, 217f/255f, 1f),
        // Ball
        new Color(1f, 0, 0, 1f)
    };
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GlobalDefine
{
    public readonly static List<List<Color>> colorList = new List<List<Color>>()
    {
        new List<Color>(){ GetColor("#496787"), GetColor("#536B8E"), GetColor("#556F93"), GetColor("#5C769C"), GetColor("#607AA0"), GetColor("#6F87AA"), GetColor("#8194B4"), GetColor("#8FA1BE"), GetColor("#9BA9C8"), GetColor("#A5AECF") }, // Gray
        new List<Color>(){ GetColor("#105BE0"), GetColor("#0E64E2"), GetColor("#0072EF"), GetColor("#0080FF"), GetColor("#008CFF"), GetColor("#009EFF"), GetColor("#00A7FF"), GetColor("#00B2FF"), GetColor("#00BBFF"), GetColor("#2CC6FF") }, // Blue
        new List<Color>(){ GetColor("#C70C3D"), GetColor("#DB0D2F"), GetColor("#E61016"), GetColor("#F00E0E"), GetColor("#FC1E0A"), GetColor("#FF371F"), GetColor("#FF4A3C"), GetColor("#FF655A"), GetColor("#FC7777"), GetColor("#FC8697") }, // Red
        new List<Color>(){ GetColor("#096841"), GetColor("#15772E"), GetColor("#0A8742"), GetColor("#199E42"), GetColor("#29AD17"), GetColor("#3FBA3C"), GetColor("#4EC43F"), GetColor("#71CC35"), GetColor("#95D136"), GetColor("#BADD3C") }, // Green
        new List<Color>(){ GetColor("#F47D08"), GetColor("#FF9017"), GetColor("#FF9E1D"), GetColor("#FFAA00"), GetColor("#FFB000"), GetColor("#FFC200"), GetColor("#F7C600"), GetColor("#F0C507"), GetColor("#F0CC3C"), GetColor("#EBCF55") }, // Yellow
        new List<Color>(){ GetColor("#7B25FA"), GetColor("#8430F2"), GetColor("#8C3BF7"), GetColor("#9448F0"), GetColor("#9F4BE8"), GetColor("#AC57F7"), GetColor("#C266F2"), GetColor("#D975F2"), GetColor("#EF89F7"), GetColor("#F993F9") }, // Purple
        new List<Color>(){ GetColor("#A33E1F"), GetColor("#B54612"), GetColor("#CF5015"), GetColor("#DB6616"), GetColor("#ED731F"), GetColor("#F87D25"), GetColor("#FF9224"), GetColor("#FF9C1C"), GetColor("#FFAE2B"), GetColor("#FFB638") }, // Brown
        new List<Color>(){ GetColor("#FFFFFF"), GetColor("#FFFFFF"), GetColor("#FFFFFF"), GetColor("#FFFFFF"), GetColor("#FFFFFF"), GetColor("#FFFFFF"), GetColor("#FFFFFF"), GetColor("#FFFFFF"), GetColor("#FFFFFF"), GetColor("#FFFFFF") }, // White
        //new List<Color>(){ GetColor("#000000"), GetColor("#000000"), GetColor("#000000"), GetColor("#000000"), GetColor("#000000"), GetColor("#000000"), GetColor("#000000"), GetColor("#000000"), GetColor("#000000"), GetColor("#000000") }, // Black
    };

    public const string COLORHEX_BRICKS_DEFAULT = "#407AD9FF";
    public const string COLORHEX_BALL_DEFAULT = "#FF0000FF";
    public const string COLORHEX_BALL_PLUS = "#FF6B3FFF";
    public const string COLORHEX_BALL_AMPLIFICATION = "#39D6E2FF";
    public const string COLORHEX_CELL_APPEAR = "#64C8FFFF";
    public static Color COLOR_CELL_APPEAR = GetColor(COLORHEX_CELL_APPEAR);

    public static Color GetCellColor(EObjKinds kinds, bool isShield, bool isEnableColor, int _colorID = 0, int _HP = 100)
    {
        switch(kinds)
        {
            case EObjKinds.BALL_NORM_01: 
                return GetColor(GlobalDefine.COLORHEX_BALL_DEFAULT);
            case EObjKinds.BALL_NORM_02: 
                return GetColor(GlobalDefine.COLORHEX_BALL_PLUS);
            case EObjKinds.BALL_NORM_03: 
                return GetColor(GlobalDefine.COLORHEX_BALL_AMPLIFICATION);
            default:
                break;
        }

        return (!isShield && isEnableColor) ? colorList[_colorID][GetHPColorIndex(_HP)] : Color.white;
    }

    public static Color GetCellColorEditor(EObjKinds kinds, int _colorID = 0, int _HP = 100)
    {
        bool isEnableColor = false;

        if (CObjInfoTable.Inst.TryGetObjInfo(kinds, out STObjInfo stObjInfo))
            isEnableColor = stObjInfo.m_bIsEnableColor || IsExtraObjEnableColor(stObjInfo.m_oExtraObjKindsList);
        
        return GetCellColor(kinds, false, isEnableColor, _colorID, _HP);
    }

    public static Color COLOR_WHITE = Color.white;

    private static int GetHPColorIndex(int _HP)
    {
        if (_HP > 90)      return 0;
        else if (_HP > 80) return 1;
        else if (_HP > 70) return 2;
        else if (_HP > 60) return 3;
        else if (_HP > 50) return 4;
        else if (_HP > 40) return 5;
        else if (_HP > 30) return 6;
        else if (_HP > 20) return 7;
        else if (_HP > 10) return 8;
        else               return 9;
    }

    private static Color GetColor(string _colorHex)
    {
        return ColorUtility.TryParseHtmlString(_colorHex, out Color _color) ? _color : Color.white;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GlobalDefine
{
    public const string FORMAT_ITEM_COUNT = "x{0}";
    public const string FORMAT_FREE = "FREE";
    public const string FORMAT_LEVEL = "Level {0}";
    public const string FORMAT_TOOLTIP_UNLOCK = "Unlock on level {0}";
    public const string SETTINGS_SOUND_ON = "Sound ON";
    public const string SETTINGS_SOUND_OFF = "Sound OFF";
    public const string PLAYER_NAME_DEFAULT = "My Name";
    public const string STRING_TRUE = "True";
    public const string STRING_FALSE = "False";
    public const string STRING_1 = "1";
    public const string STRING_0 = "0";
    public const string STRING_DATE_EMPTY = "2023/08/01 00:00:00";
    public const string STRING_SOLD_OUT = "SOLD_OUT";
    public const string FORMAT_INT = "{0}";
    public const string FORMAT_BALL_TEXT_EXTRA = "{0}+{1}";
    public const string TAG_WALL = "Wall";

    public const string FORMAT_SUBSCRIPTION_DAYS_LEFT = "{0} Days Left";
    public const string FORMAT_SUBSCRIPTION_1DAY_LEFT = "1 Day Left";
    public const string FORMAT_SUBSCRIPTION_DESC = "Weekly Diamond Membership offers weekly subscription for {0} after 3-day free trial. Monthly Diamond Membership offers monthly subscription for {1}. Yearly Diamond Membership offers yearly subscription for {2}. Automatic renewal within 24 hours before the end. This price is established for United States Customers. Pricing in other countries may vary and actual charges may be converted to your local currency depending on the country of residence. Remove banners and interstitial ads between games.";
    public static readonly string[] FORMAT_SUBSCRIPTION_PRICE = new string[3]
    {
        "Free 3-day trial, then {0} per week",
        "1 MONTH  {0}",
        "1 YEAR  {0}"
    };

    public static STSortingOrderInfo SortingInfo_HPText = new STSortingOrderInfo()  { m_nOrder = 200, m_oLayer = KCDefine.U_SORTING_L_CELL };
    public static STSortingOrderInfo SortingInfo_NumText = new STSortingOrderInfo() { m_nOrder = 200, m_oLayer = KCDefine.U_SORTING_L_BALL };
    public static STSortingOrderInfo SortingInfo_PreviewTooltips = new STSortingOrderInfo(){ m_nOrder = 100, m_oLayer = KCDefine.U_SORTING_L_FOREGROUND };
    public static STSortingOrderInfo SortingInfo_PreviewTips = new STSortingOrderInfo(){ m_nOrder = 101, m_oLayer = KCDefine.U_SORTING_L_FOREGROUND };
    public static STSortingOrderInfo SortingInfo_StoreCanvas = new STSortingOrderInfo(){ m_nOrder = 800, m_oLayer = KCDefine.U_SORTING_L_FOREGROUND };
    public static STSortingOrderInfo SortingInfo_RewardAlertCanvas = new STSortingOrderInfo(){ m_nOrder = 900, m_oLayer = KCDefine.U_SORTING_L_FOREGROUND };
    public static STSortingOrderInfo SortingInfo_RewardCanvas = new STSortingOrderInfo(){ m_nOrder = 1000, m_oLayer = KCDefine.U_SORTING_L_FOREGROUND };

    public static string GetBallText(int ballCount, int extraBallCount)
    {
        return extraBallCount > 0 ? string.Format(FORMAT_BALL_TEXT_EXTRA, ballCount, extraBallCount) : string.Format(FORMAT_INT, ballCount);
    }
}
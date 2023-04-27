using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.IO;
using System.Linq;
using MessagePack;

public partial class CUserInfo : CBaseInfo
{
    private const string KEY_SETTINGS_DARKMODE = "Settings_DarkMode";

    private const string KEY_RUBY = "Ruby";
    private const string KEY_STAR_SUM = "Star_Sum";
    private const string KEY_AD_BLOCK = "AD_Block";
    private const string KEY_GOLDEN_AIM = "Golden_Aim";
    private const string KEY_BOOSTER_MISSILE = "Booster_Missile";
    private const string KEY_BOOSTER_LIGHTNING = "Booster_Lightning";
    private const string KEY_BOOSTER_BOMB = "Booster_Bomb";
    private const string KEY_ITEM_EARTHQUAKE = "Item_Earthquake";
    private const string KEY_ITEM_ADD_BALL = "Item_AddBall";
    private const string KEY_ITEM_BRICKS_DELETE = "Item_BricksDelete";
    private const string KEY_ITEM_ADD_LASER_BRICKS = "Item_AddLaserBricks";
    private const string KEY_ITEM_ADD_STEEL_BRICKS = "Item_AddSteelBricks";
    private const string KEY_LEVEL_CURRENT = "Level_Current";
    private const string KEY_LEVEL_STAR = "Level_Star";
    private const string KEY_LEVEL_SCORE = "Level_Score";
    private const string KEY_LEVEL_SKIP = "Level_Skip";

    [IgnoreMember]
	public bool Settings_DarkMode {
		get { return bool.Parse(m_oStrDict.GetValueOrDefault(KEY_SETTINGS_DARKMODE, GlobalDefine.STRING_FALSE)); }
		set { m_oStrDict.ExReplaceVal(KEY_SETTINGS_DARKMODE, $"{(bool)value}"); }
	}

    [IgnoreMember]
	public int Ruby {
		get { return int.Parse(m_oStrDict.GetValueOrDefault(KEY_RUBY, GlobalDefine.STRING_0)); }
		set { m_oStrDict.ExReplaceVal(KEY_RUBY, $"{(int)value}"); }
	}

    [IgnoreMember]
	public int Star {
		get { return int.Parse(m_oStrDict.GetValueOrDefault(KEY_STAR_SUM, GlobalDefine.STRING_0)); }
		set { m_oStrDict.ExReplaceVal(KEY_STAR_SUM, $"{(int)value}"); }
	}

    [IgnoreMember]
	public bool Item_ADBlock {
		get { return bool.Parse(m_oStrDict.GetValueOrDefault(KEY_AD_BLOCK, GlobalDefine.STRING_FALSE)); }
		set { m_oStrDict.ExReplaceVal(KEY_AD_BLOCK, $"{(bool)value}"); }
	}

    [IgnoreMember]
	public bool Item_GoldenAim {
		get { return bool.Parse(m_oStrDict.GetValueOrDefault(KEY_GOLDEN_AIM, GlobalDefine.STRING_FALSE)); }
		set { m_oStrDict.ExReplaceVal(KEY_GOLDEN_AIM, $"{(bool)value}"); }
	}

    [IgnoreMember]
	public int Item_Earthquake {
		get { return int.Parse(m_oStrDict.GetValueOrDefault(KEY_ITEM_EARTHQUAKE, GlobalDefine.STRING_0)); }
		set { m_oStrDict.ExReplaceVal(KEY_ITEM_EARTHQUAKE, $"{(int)value}"); }
	}

    [IgnoreMember]
	public int Item_AddBall {
		get { return int.Parse(m_oStrDict.GetValueOrDefault(KEY_ITEM_ADD_BALL, GlobalDefine.STRING_0)); }
		set { m_oStrDict.ExReplaceVal(KEY_ITEM_ADD_BALL, $"{(int)value}"); }
	}

    [IgnoreMember]
	public int Item_BricksDelete {
		get { return int.Parse(m_oStrDict.GetValueOrDefault(KEY_ITEM_BRICKS_DELETE, GlobalDefine.STRING_0)); }
		set { m_oStrDict.ExReplaceVal(KEY_ITEM_BRICKS_DELETE, $"{(int)value}"); }
	}

    [IgnoreMember]
	public int Item_AddLaserBricks {
		get { return int.Parse(m_oStrDict.GetValueOrDefault(KEY_ITEM_ADD_LASER_BRICKS, GlobalDefine.STRING_0)); }
		set { m_oStrDict.ExReplaceVal(KEY_ITEM_ADD_LASER_BRICKS, $"{(int)value}"); }
	}

    [IgnoreMember]
	public int Item_AddSteelBricks {
		get { return int.Parse(m_oStrDict.GetValueOrDefault(KEY_ITEM_ADD_STEEL_BRICKS, GlobalDefine.STRING_0)); }
		set { m_oStrDict.ExReplaceVal(KEY_ITEM_ADD_STEEL_BRICKS, $"{(int)value}"); }
	}

    [IgnoreMember]
	public int Booster_Missile {
		get { return int.Parse(m_oStrDict.GetValueOrDefault(KEY_BOOSTER_MISSILE, GlobalDefine.STRING_0)); }
		set { m_oStrDict.ExReplaceVal(KEY_BOOSTER_MISSILE, $"{(int)value}"); }
	}

    [IgnoreMember]
	public int Booster_Lightning {
		get { return int.Parse(m_oStrDict.GetValueOrDefault(KEY_BOOSTER_LIGHTNING, GlobalDefine.STRING_0)); }
		set { m_oStrDict.ExReplaceVal(KEY_BOOSTER_LIGHTNING, $"{(int)value}"); }
	}

    [IgnoreMember]
	public int Booster_Bomb {
		get { return int.Parse(m_oStrDict.GetValueOrDefault(KEY_BOOSTER_BOMB, GlobalDefine.STRING_0)); }
		set { m_oStrDict.ExReplaceVal(KEY_BOOSTER_BOMB, $"{(int)value}"); }
	}

    [IgnoreMember]
	public int LevelCurrent {
		get { return int.Parse(m_oStrDict.GetValueOrDefault(KEY_LEVEL_CURRENT, GlobalDefine.STRING_1)); }
		set { m_oStrDict.ExReplaceVal(KEY_LEVEL_CURRENT, $"{(int)value}"); }
	}

    [IgnoreMember]
	public string LevelStar {
		get { return (m_oStrDict.GetValueOrDefault(KEY_LEVEL_STAR, string.Empty)); }
		set { m_oStrDict.ExReplaceVal(KEY_LEVEL_STAR, $"{value}"); }
	}

    [IgnoreMember]
	public string LevelScore {
		get { return (m_oStrDict.GetValueOrDefault(KEY_LEVEL_SCORE, string.Empty)); }
		set { m_oStrDict.ExReplaceVal(KEY_LEVEL_SCORE, $"{value}"); }
	}

    [IgnoreMember]
	public string LevelSkip {
		get { return (m_oStrDict.GetValueOrDefault(KEY_LEVEL_SKIP, string.Empty)); }
		set { m_oStrDict.ExReplaceVal(KEY_LEVEL_SKIP, $"{value}"); }
	}
}
#endif
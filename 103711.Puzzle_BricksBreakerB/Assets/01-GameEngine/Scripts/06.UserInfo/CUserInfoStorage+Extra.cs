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
    private const string KEY_SETTINGS_DARKMODE = "DarkMode";

    private const string KEY_ITEM_EARTHQUAKE = "Item_Earthquake";
    private const string KEY_ITEM_ADD_BALL = "Item_AddBall";
    private const string KEY_ITEM_BRICKS_DELETE = "Item_BricksDelete";
    private const string KEY_ITEM_ADD_LASER_BRICKS = "Item_AddLaserBricks";
    private const string KEY_ITEM_ADD_STEEL_BRICKS = "Item_AddSteelBricks";

    [IgnoreMember]
	public bool Settings_DarkMode {
		get { return bool.Parse(m_oStrDict.GetValueOrDefault(KEY_SETTINGS_DARKMODE, GlobalDefine.STRING_FALSE)); }
		set { m_oStrDict.ExReplaceVal(KEY_SETTINGS_DARKMODE, $"{(bool)value}"); }
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
}
#endif
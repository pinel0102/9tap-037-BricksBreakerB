using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using MessagePack;

#if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
using Newtonsoft.Json;
#endif // #if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE

public partial class CLevelInfo : CBaseInfo, System.ICloneable
{
#if NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
    [JsonIgnore]
    [IgnoreMember]
	public int ViewSizeX {
		get { return int.Parse(m_oStrDict.GetValueOrDefault(KEY_VIEW_SIZE_X, $"{(int)EViewSize.DEF_SIZE_X}")); }
		set { m_oStrDict.ExReplaceVal(KEY_VIEW_SIZE_X, $"{(int)value}"); }
	}

    [JsonIgnore]
    [IgnoreMember]
	public int ViewSizeY {
		get { return int.Parse(m_oStrDict.GetValueOrDefault(KEY_VIEW_SIZE_Y, $"{(int)EViewSize.DEF_SIZE_Y}")); }
		set { m_oStrDict.ExReplaceVal(KEY_VIEW_SIZE_Y, $"{(int)value}"); }
	}

    [JsonIgnore]
    [IgnoreMember]
	public int ViewSizeZ {
		get { return int.Parse(m_oStrDict.GetValueOrDefault(KEY_VIEW_SIZE_Z, $"{(int)EViewSize.DEF_SIZE_Z}")); }
		set { m_oStrDict.ExReplaceVal(KEY_VIEW_SIZE_Z, $"{(int)value}"); }
	}

    [JsonIgnore]
    [IgnoreMember]
	public int Score1 {
		get { return int.Parse(m_oStrDict.GetValueOrDefault(KEY_SCORE_1, $"{(int)GlobalDefine.Score1_Default}")); }
		set { m_oStrDict.ExReplaceVal(KEY_SCORE_1, $"{(int)value}"); }
	}

    [JsonIgnore]
    [IgnoreMember]
	public int Score2 {
		get { return int.Parse(m_oStrDict.GetValueOrDefault(KEY_SCORE_2, $"{(int)GlobalDefine.Score2_Default}")); }
		set { m_oStrDict.ExReplaceVal(KEY_SCORE_2, $"{(int)value}"); }
	}

    [JsonIgnore]
    [IgnoreMember]
	public int Score3 {
		get { return int.Parse(m_oStrDict.GetValueOrDefault(KEY_SCORE_3, $"{(int)GlobalDefine.Score3_Default}")); }
		set { m_oStrDict.ExReplaceVal(KEY_SCORE_3, $"{(int)value}"); }
	}

    [JsonIgnore] [IgnoreMember] public Vector3Int ViewSize => new Vector3Int(ViewSizeX, ViewSizeY, ViewSizeZ);
#else
    [IgnoreMember]
	public int ViewSizeX {
		get { return int.Parse(m_oStrDict.GetValueOrDefault(KEY_VIEW_SIZE_X, $"{(int)EViewSize.DEF_SIZE_X}")); }
		set { m_oStrDict.ExReplaceVal(KEY_VIEW_SIZE_X, $"{(int)value}"); }
	}

    [IgnoreMember]
	public int ViewSizeY {
		get { return int.Parse(m_oStrDict.GetValueOrDefault(KEY_VIEW_SIZE_Y, $"{(int)EViewSize.DEF_SIZE_Y}")); }
		set { m_oStrDict.ExReplaceVal(KEY_VIEW_SIZE_Y, $"{(int)value}"); }
	}

    [IgnoreMember]
	public int ViewSizeZ {
		get { return int.Parse(m_oStrDict.GetValueOrDefault(KEY_VIEW_SIZE_Z, $"{(int)EViewSize.DEF_SIZE_Z}")); }
		set { m_oStrDict.ExReplaceVal(KEY_VIEW_SIZE_Z, $"{(int)value}"); }
	}

    [IgnoreMember]
	public int Score1 {
		get { return int.Parse(m_oStrDict.GetValueOrDefault(KEY_SCORE_1, $"{(int)GlobalDefine.Score1_Default}")); }
		set { m_oStrDict.ExReplaceVal(KEY_SCORE_1, $"{(int)value}"); }
	}

    [IgnoreMember]
	public int Score2 {
		get { return int.Parse(m_oStrDict.GetValueOrDefault(KEY_SCORE_2, $"{(int)GlobalDefine.Score2_Default}")); }
		set { m_oStrDict.ExReplaceVal(KEY_SCORE_2, $"{(int)value}"); }
	}

    [IgnoreMember]
	public int Score3 {
		get { return int.Parse(m_oStrDict.GetValueOrDefault(KEY_SCORE_3, $"{(int)GlobalDefine.Score3_Default}")); }
		set { m_oStrDict.ExReplaceVal(KEY_SCORE_3, $"{(int)value}"); }
	}

    [IgnoreMember] public Vector3Int ViewSize => new Vector3Int(ViewSizeX, ViewSizeY, ViewSizeZ);
#endif

    private void SetExtraValues()
    {
        this.GridPivot = EGridPivot.DOWN;
        this.ViewSizeX = NumCells.x;
        this.ViewSizeY = NumCells.y;
        this.ViewSizeZ = NumCells.z;
        this.Score1 = Score1;
        this.Score2 = Score2;
        this.Score3 = Score3;
        
        //Debug.Log(CodeManager.GetMethodName() + string.Format("{0} / {1}", this.NumCells, this.ViewSize));
    }
}
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GlobalDefine
{
    public static bool IsExtraObjEnableHit(List<EObjKinds> extraList)
    {
        for (int i=0; i < extraList.Count; i++)
        {
            if(CObjInfoTable.Inst.TryGetObjInfo(extraList[i], out STObjInfo stObjInfo))
            {
                if (stObjInfo.m_bIsEnableHit)
                    return true;
            }
        }

        return false;
    }

    public static bool IsExtraObjEnableColor(List<EObjKinds> extraList)
    {
        for (int i=0; i < extraList.Count; i++)
        {
            if(CObjInfoTable.Inst.TryGetObjInfo(extraList[i], out STObjInfo stObjInfo))
            {
                if (stObjInfo.m_bIsEnableColor)
                    return true;
            }
        }

        return false;
    }
    
    public static bool IsNeedSubSprite(EObjKinds kinds)
    {
        switch(kinds)
        {
            case EObjKinds.OBSTACLE_BRICKS_OPEN_01:
            case EObjKinds.OBSTACLE_BRICKS_FIX_01:
                return true;
            default:
                return false;
        }
    }
    
    public static bool IsShieldCell(EObjKinds kinds)
    {
        switch(kinds)
        {
            case EObjKinds.OBSTACLE_BRICKS_WOODBOX_01:
            case EObjKinds.OBSTACLE_BRICKS_WOODBOX_02:
                return true;
            default:
                return false;
        }
    }
}
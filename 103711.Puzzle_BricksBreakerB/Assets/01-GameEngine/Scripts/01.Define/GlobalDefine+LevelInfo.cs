using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class GlobalDefine
{
    public const int LevelInfo_Default_Score1 = 100;
    public const int LevelInfo_Default_Score2 = 500;
    public const int LevelInfo_Default_Score3 = 1000;
    public const int LevelInfo_Default_LevelType = 0;

    public static int GetTargetCellCount(CLevelInfo levelInfo)
    {
        int targetCellCount = 0;

        for (int row = 0; row < levelInfo.m_oCellInfoDictContainer.Count; row++)
        {
            for (int col = 0; col < levelInfo.m_oCellInfoDictContainer[row].Count; col++)
            {
                var cellInfo = levelInfo.m_oCellInfoDictContainer[row][col];

                for (int layer = 0; layer < cellInfo.m_oCellObjInfoList.Count; layer++)
                {
                    var kinds = cellInfo.m_oCellObjInfoList[layer].ObjKinds;
                    if(CObjInfoTable.Inst.TryGetObjInfo(kinds, out STObjInfo objInfo))
                    {
                        if (objInfo.m_bIsClearTarget)
                        {
                            targetCellCount++;
                            break;
                        }
                    }
                }
            }
        }

        return targetCellCount;
    }
}
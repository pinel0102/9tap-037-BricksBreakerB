using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<Summary>레벨 정보.</Summary>
[System.Serializable]
public struct LevelInfo
{
    ///<Summary>격자 크기.</Summary>
    public Vector2Int gridSize;

    ///<Summary><para>격자 행렬 정보.</para>
    ///<para>외부 딕셔너리 : 행을 의미. 딕셔너리의 개수는 격자 높이와 동일.</para>
    ///<para>내부 딕셔너리 : 열을 의미. 딕셔너리가 비어있으면 빈 행이라는 의미.</para></Summary>
    public Dictionary<int, Dictionary<int, CellInfo>> gridInfo;

}

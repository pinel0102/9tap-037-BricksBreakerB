using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<Summary>셀 정보.</Summary>
[System.Serializable]
public struct CellInfo
{
    ///<Summary>위치 정보 (행, 열). 셀 객체 크기가 1x1이 아닐 경우 좌상단을 기준으로 함.</Summary>
    public Vector2Int position;

    ///<Summary>이 셀에 존재하는 셀 객체 리스트.</Summary>
    public List<CellObject> cellObjects;

    public CellInfo(Vector2Int _position, List<CellObject> _cellObjects)
    {
        position = _position;
        cellObjects = new List<CellObject>();

        for (int i=0; i < _cellObjects.Count; i++)
        {
            cellObjects.Add(_cellObjects[i]);
        }
    }
}

///<Summary>셀 객체 정보.</Summary>
[System.Serializable]
public struct CellObject
{
    ///<Summary>식별자.</Summary>
    public CellType type;

    ///<Summary>크기 (가로, 세로). 1x1이 아닐 경우 좌상단을 기준으로 함.</Summary>
    public Vector2Int size;

    ///<Summary>레이어 (0 : 최하위).</Summary>
    public int layer;

    ///<Summary>프리셋 + 커스텀 제공.</Summary>
    public int HP;

    ///<Summary>프리셋 + 커스텀 제공.</Summary>
    public int ATK;

    public CellObject(CellType _type, Vector2Int _size, int _layer, int _HP, int _ATK)
    {
        type = _type;
        size = _size;
        layer = _layer;
        HP = _HP;
        ATK = _ATK;
    }
}
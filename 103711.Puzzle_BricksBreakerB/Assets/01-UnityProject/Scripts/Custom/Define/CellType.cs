using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CellType
{
    EMPTY = 0,

    ///<Summary>사각형.</Summary>
    SQUARE = 1000000000,

    ///<Summary>일반 삼각형.</Summary>
    TRIANGLE = 1010000000,
    
    ///<Summary>직각 삼각형.</Summary>
    TRIANGLE_R_LEFT_DN = 1020000000,
    ///<Summary>직각 삼각형.</Summary>
    TRIANGLE_R_LEFT_UP = 1020000001,
    ///<Summary>직각 삼각형.</Summary>
    TRIANGLE_R_RIGHT_DN = 1020000002,
    ///<Summary>직각 삼각형.</Summary>
    TRIANGLE_R_RIGHT_UP = 1020000003,
}

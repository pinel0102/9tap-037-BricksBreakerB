using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///<Summary>셀 식별자.</Summary>
public enum CellType
{
    EMPTY = 0,

#region 일반 블록 (부숴야 하는 목표)

    ///<Summary>기본 사각형.</Summary>
    NORMAL = 1000000000,

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

#endregion 일반 블록 (부숴야 하는 목표)


#region 특수 블록 (충돌 O)

    ///<Summary>HP가 있는 상자. (일반 블록 위를 덮는 중첩용).</Summary>
    WOOD_BOX = 1100000000,
    ///<Summary><para>HP가 있는 상자. (일반 블록 위를 덮는 중첩용).</para>
    ///<para>턴이 지나도 내려오지 않음.</para></Summary>
    WOOD_BOX_FIXED = 1100000001,

    ///<Summary><para>턴마다 열고 닫히는 무적 상자 (닫힌 상태로 시작).</para>
    ///<para>닫히면 대미지를 입지 않음.</para></Summary>
    IRON_BOX_OPEN = 1110000000,
    ///<Summary><para>턴마다 열고 닫히는 무적 상자 (열린 상태로 시작).</para>
    ///<para>닫히면 대미지를 입지 않음.</para></Summary>
    IRON_BOX_CLOSE = 1110000001,

    ///<Summary>파괴시 3x3 폭탄 생성.</Summary>
    CREATE_BOMB = 1120000000,


#endregion 특수 블록 (충돌 O)


#region 효과 블록 (충돌 X / 지나가면 발동 / 무한)

    ///<Summary>가로 레이저.</Summary>
    LASER_HORIZONTAL = 1200000000,
    ///<Summary>세로 레이저.</Summary>
    LASER_VERTICAL = 1200000001,

    ///<Summary>방향 랜덤 전환.</Summary>
    MOVE_RANDOM = 1210000000,

    ///<Summary>웜홀 입구.</Summary>
    WORMHOLE_IN = 1220000000,
    ///<Summary>웜홀 출구.</Summary>
    WORMHOLE_OUT = 1220000001,

#endregion 효과 블록 (충돌 X / 지나가면 발동 / 무한)


#region 아이템 블록 (충돌 X / 지나가면 획득 / 1회용)
    
    ///<Summary>볼 추가 (+1). 1회용.</Summary>
    BALL_ADD_1 = 1100000000,
    ///<Summary>볼 추가 (+2). 1회용.</Summary>
    BALL_ADD_2 = 1100000001,
    ///<Summary>볼 추가 (+3). 1회용.</Summary>
    BALL_ADD_3 = 1100000002,
    ///<Summary>볼 추가 (+10). 1회용.</Summary>
    BALL_ADD_10 = 1100000009,

#endregion 아이템 블록 (충돌 X / 지나가면 획득 / 1회용)

}

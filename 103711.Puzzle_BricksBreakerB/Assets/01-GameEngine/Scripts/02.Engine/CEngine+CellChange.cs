using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NSEngine {
    public partial class CEngine : CComponent
    {
        ///<Summary>셀을 변경한다.
        ///<para>(셀 정보 유지) HP, Shield, ColorID</para></Summary>
        public void ChangeCell(CECellObjController target, EObjKinds toKinds)
        {
            ChangeCell(target, toKinds, target.CellObjInfo);
        }

        ///<Summary>셀을 변경한다.
        ///<para>(셀 정보 수정) HP, Shield, ColorID</para></Summary>
        public void ChangeCell(CECellObjController target, EObjKinds toKinds, STCellObjInfo stCellObjInfo)
        {
            Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>{0}</color> -> <color=yellow>{1}</color>", target.CellObjInfo.ObjKinds, toKinds));
            STObjInfo stObjInfo = CObjInfoTable.Inst.GetObjInfo(toKinds);

            target.ResetObjInfo(stObjInfo, stCellObjInfo);

            switch(toKinds)
            {
                case EObjKinds.OBSTACLE_BRICKS_OPEN_01:
                case EObjKinds.OBSTACLE_BRICKS_CLOSE_01:
                    GlobalDefine.PlaySoundFX(ESoundSet.SOUND_SPECIAL_OPEN_CLOSE);
                    break;
            }
        }
    }
}
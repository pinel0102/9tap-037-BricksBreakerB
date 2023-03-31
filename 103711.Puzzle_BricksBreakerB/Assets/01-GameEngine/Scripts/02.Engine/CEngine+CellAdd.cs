using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NSEngine {
    public partial class CEngine : CComponent
    {
        ///<Summary>빈 셀에 특수 블럭을 추가한다.</Summary>
        public void AddCell(Vector3Int gridPos, EObjKinds kinds, STCellObjInfo stCellObjInfo)
        {
            if (this.CellObjLists[gridPos.y, gridPos.x].Count > 0) return;

            Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>[{0}, {1}] {2}</color>", gridPos.x, gridPos.y, kinds));

            var oCellObjList = new List<CEObj>();

            EObjKinds kindsType = (EObjKinds)((int)kinds).ExKindsToCorrectKinds(EKindsGroupType.SUB_KINDS_TYPE);
            STObjInfo stObjInfo = CObjInfoTable.Inst.GetObjInfo(kinds);
            
            var oCellObj = this.CreateCellObj(stObjInfo, null);
            
            oCellObj.transform.localPosition = this.SelGridInfo.m_stPivotPos + gridPos.ExToPos(Access.CellCenterOffset, Access.CellSize);
            oCellObj.GetController<CECellObjController>().ResetObjInfo(stObjInfo, stCellObjInfo);
            oCellObj.GetController<CECellObjController>().SetIdx(gridPos);
            oCellObj.SetCellIdx(gridPos, kinds);
            oCellObj.SetCellObjInfo(stCellObjInfo);
            oCellObj.SetExtraObjKindsList(stObjInfo);
            oCellObj.AddCellEffect(kindsType);
            oCellObj.SetCellActive(true);

            InitCellLayer(oCellObj);

            oCellObjList.ExAddVal(oCellObj);

			this.CellObjLists[gridPos.y, gridPos.x] = oCellObjList;
        }
    }
}
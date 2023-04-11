using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NSEngine {
    public partial class CEngine : CComponent
    {
        ///<Summary>해당 좌표에 블럭을 배치한다.</Summary>
        ///<param name="gridPos">좌표. (x, y, layer)</param>
        ///<param name="overrideCell"><para>true : 기존 셀을 변경한다.</para><para>false : 빈 셀일 때만 배치한다.</para></param>
        public void AddCell(Vector3Int gridPos, EObjKinds kinds, STCellObjInfo stCellObjInfo, bool overrideCell = false)
        {
            if (!overrideCell && this.CellObjLists[gridPos.y, gridPos.x].Count > 0) return;

            for (int i=0; i < this.CellObjLists[gridPos.y, gridPos.x].Count; i++)
            {
                this.CellObjLists[gridPos.y, gridPos.x][i].GetComponent<CECellObjController>().CellDestroy(false);
            }
            
            this.CellObjLists[gridPos.y, gridPos.x].Clear();

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
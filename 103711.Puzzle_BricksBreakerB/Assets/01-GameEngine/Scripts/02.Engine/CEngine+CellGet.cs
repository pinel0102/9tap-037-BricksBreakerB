using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NSEngine {
    public partial class CEngine : CComponent
    {
        public Vector3Int viewSize = KDefine.E_DEF_NUM_CELLS;

        public List<CEObj> GetRandomCells_SkillTarget(int count, List<CEObj> excludeList)
        {
            List<CEObj> cellList = new List<CEObj>();

            for (int row = 0; row < this.CellObjLists.GetLength(KCDefine.B_VAL_0_INT); row++)
            {
                for (int col = 0; col < this.CellObjLists.GetLength(KCDefine.B_VAL_1_INT); col++)
                {
                    int _count = this.CellObjLists[row, col].Count;
                    if (_count > 0)
                    {
                        int _cLastLayer = _count - 1;
                        CEObj target = this.CellObjLists[row, col][_cLastLayer];
                        if (target != null && target.IsActiveCell())
                        {
                            if (target.Params.m_stObjInfo.m_bIsSkillTarget)
                            {
                                cellList.Add(target);
                            }
                        }
                    }
                }
            }

            return cellList.OrderBy(g => System.Guid.NewGuid())
                            .Where(i => !excludeList.Contains(i))
                            .Take(count).ToList();
        }

        public List<CEObj> GetAllCells_SkillTarget(List<CEObj> excludeList)
        {
            List<CEObj> cellList = new List<CEObj>();

            for (int row = 0; row < this.CellObjLists.GetLength(KCDefine.B_VAL_0_INT); row++)
            {
                for (int col = 0; col < this.CellObjLists.GetLength(KCDefine.B_VAL_1_INT); col++)
                {
                    int _count = this.CellObjLists[row, col].Count;
                    if (_count > 0)
                    {
                        int _cLastLayer = _count - 1;
                        CEObj target = this.CellObjLists[row, col][_cLastLayer];
                        if (target != null && target.IsActiveCell())
                        {
                            if (target.Params.m_stObjInfo.m_bIsSkillTarget)
                            {
                                cellList.Add(target);
                            }
                        }
                    }
                }
            }

            return cellList.OrderBy(g => System.Guid.NewGuid())
                            .Where(i => !excludeList.Contains(i)).ToList();
        }

        public List<CEObj> GetAllCells_SkillTarget()
        {
            return GetAllCells_SkillTarget(new List<CEObj>());
        }

        public CEObj GetRandomCell(List<CEObj> cellList)
        {
            return GetRandomCells(cellList, 1)[0];
        }

        public List<CEObj> GetRandomCells(List<CEObj> cellList, int count)
        {
            return cellList.OrderBy(g => System.Guid.NewGuid())
                            .Take(count).ToList();
        }

        public List<CEObj> GetAllCells(EObjKinds kindsToGet)
        {
            List<CEObj> cellList = new List<CEObj>();

            for (int row = 0; row < this.CellObjLists.GetLength(KCDefine.B_VAL_0_INT); row++)
            {
                for (int col = 0; col < this.CellObjLists.GetLength(KCDefine.B_VAL_1_INT); col++)
                {
                    int _count = this.CellObjLists[row, col].Count;
                    if (_count > 0)
                    {
                        int _cLastLayer = _count - 1;
                        CEObj target = this.CellObjLists[row, col][_cLastLayer];
                        if (target != null && target.IsActiveCell())
                        {
                            if (target.Params.m_stObjInfo.m_eObjKinds == kindsToGet)
                            {
                                cellList.Add(target);
                            }
                        }
                    }
                }
            }

            return cellList;
        }

        ///<Summary>화면 내에 있는 랜덤한 빈 셀 좌표를 반환. (Vector3Int(col, row, layer))</Summary>
        public List<Vector3Int> GetRandomEmptyCells(int firstRow, int lastRow, int count)
        {
            Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>[{0} ~ {1}]</color>", firstRow, lastRow - 1));

            List<Vector3Int> cellList = new List<Vector3Int>();

            for (int row = firstRow; row < lastRow; row++)
            {
                for (int col = 0; col < this.CellObjLists.GetLength(KCDefine.B_VAL_1_INT); col++)
                {
                    int _count = this.CellObjLists[row, col].Count;
                    if (_count <= 0)
                    {
                        cellList.Add(new Vector3Int(col, row, 0));
                    }
                }
            }

            return cellList.OrderBy(g => System.Guid.NewGuid())
                            .Take(count).ToList();
        }

        ///<Summary>화면 내에 있는 가장 아래 빈 셀 좌표를 반환. (Vector3Int(col, row, layer))</Summary>
        public List<Vector3Int> GetBottomEmptyCells(int count, bool isDescending)
        {
            //Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>[{0} ~ {1}]</color>", firstRow, lastRow - 1));

            List<Vector3Int> cellList = new List<Vector3Int>();

            var bottomRow = GetBottomRow();

            for (int col = 0; col < this.CellObjLists.GetLength(KCDefine.B_VAL_1_INT); col++)
            {
                cellList.Add(new Vector3Int(col, bottomRow, 0));
            }

            return isDescending ? cellList.OrderByDescending(g => g.x).Take(count).ToList() : cellList.OrderBy(g => g.x).Take(count).ToList();
        }

        public CEObj GetLastClearTarget()
        {
            bool isCellFound = false;

            for(int i = this.CellObjLists.GetLength(KCDefine.B_VAL_0_INT) - 1; i >= 0 ; i--) {
				for(int j = this.CellObjLists.GetLength(KCDefine.B_VAL_1_INT) - 1; j >= 0 ; j--) {
					for(int k = 0; k < this.CellObjLists[i, j].Count; ++k) {
						// 셀이 존재 할 경우
                        CEObj target = this.CellObjLists[i, j][k];
                        if (target != null && target.IsActiveCell())
                        {
                            if (!isCellFound)
                            {
                                if (target.Params.m_stObjInfo.m_bIsClearTarget)
                                {
                                    return target;
                                }
                            }
                        }
					}
				}
			}

            return null;
        }

        public int GetBottomRow()
        {
            var lastClearTarget = GetLastClearTarget();
            int row = lastClearTarget.row;
            
            Vector2 distanceVector = subGameSceneManager.mainCanvas.WorldToCanvas(lastClearTarget.transform.position - subGameSceneManager.deadLine.position);
            float cellsizeY = Access.CellSize.y * SelGridInfo.m_stScale.y;
            float distance = distanceVector.y - (cellsizeY * 0.5f);
            
            while(distance >= cellsizeY)
            {
                distance -= cellsizeY;
                row++;
            }

            return row;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NSEngine {
    public partial class CEngine : CComponent
    {
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
                        if(this.CellObjLists[row, col][_cLastLayer].gameObject.activeSelf) 
                        {
                            CEObj target = this.CellObjLists[row, col][_cLastLayer];
                            if (target != null)
                            {
                                if (target.Params.m_stObjInfo.m_bIsSkillTarget)
                                {
                                    cellList.Add(target);
                                }
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
                        if(this.CellObjLists[row, col][_cLastLayer].gameObject.activeSelf) 
                        {
                            CEObj target = this.CellObjLists[row, col][_cLastLayer];
                            if (target != null)
                            {
                                if (target.Params.m_stObjInfo.m_bIsSkillTarget)
                                {
                                    cellList.Add(target);
                                }
                            }
                        }
                    }
                }
            }

            return cellList.OrderBy(g => System.Guid.NewGuid())
                            .Where(i => !excludeList.Contains(i)).ToList();
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
                        if(this.CellObjLists[row, col][_cLastLayer].gameObject.activeSelf) 
                        {
                            CEObj target = this.CellObjLists[row, col][_cLastLayer];
                            if (target != null)
                            {
                                if (target.Params.m_stObjInfo.m_eObjKinds == kindsToGet)
                                {
                                    cellList.Add(target);
                                }
                            }
                        }
                    }
                }
            }

            return cellList;
        }

        public CEObj GetLastCell()
        {
            bool isLastCellFound = false;

            for(int i = this.CellObjLists.GetLength(KCDefine.B_VAL_0_INT) - 1; i >= 0 ; i--) {
				for(int j = this.CellObjLists.GetLength(KCDefine.B_VAL_1_INT) - 1; j >= 0 ; j--) {
					for(int k = 0; k < this.CellObjLists[i, j].Count; ++k) {
						// 셀이 존재 할 경우
						if(this.CellObjLists[i, j][k].gameObject.activeSelf) {
                            CEObj target = this.CellObjLists[i, j][k];
                            if (target != null && target.TryGetComponent<CECellObjController>(out CECellObjController oController))
                            {
                                if (!isLastCellFound)
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
			}

            return null;
        }
    }
}
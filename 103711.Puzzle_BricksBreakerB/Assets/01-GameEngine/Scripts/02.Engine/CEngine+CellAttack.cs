using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
    public partial class CEngine : CComponent
    {
        ///<Summary>볼이 아닌 특수 효과로 셀을 공격. (셀 효과 미발동.)</Summary>
        public void CellDamage_SkillTarget(CEObj target, CEBallObjController ballController, int _ATK)
        {
            if (target != null && target.IsActiveCell())
            {
                EObjKinds kinds = target.Params.m_stObjInfo.m_eObjKinds;
                EObjKinds kindsType = (EObjKinds)((int)kinds).ExKindsToCorrectKinds(EKindsGroupType.SUB_KINDS_TYPE);
                
                if (target.Params.m_stObjInfo.m_bIsSkillTarget)
                {
                    if (target.Params.m_stObjInfo.m_bIsShieldCell)
                    {
                        if (target.CellObjInfo.SHIELD > _ATK)
                            target.GetComponent<CECellObjController>().GetDamage(ballController, kindsType, kinds, _ATK);
                        else
                        {
                            GlobalDefine.ShowEffect(EFXSet.FX_BREAK_BRICK, target.transform.position, GlobalDefine.GetCellColor(target.CellObjInfo.ObjKinds, true, target.Params.m_stObjInfo.m_bIsEnableColor, target.CellObjInfo.ColorID));
                            CellDestroy(target);
                        }
                    }
                    else
                    {
                        if (target.CellObjInfo.HP > _ATK)
                            target.GetComponent<CECellObjController>().GetDamage(ballController, kindsType, kinds, _ATK);
                        else
                        {
                            GlobalDefine.ShowEffect(EFXSet.FX_BREAK_BRICK, target.transform.position, GlobalDefine.GetCellColor(target.CellObjInfo.ObjKinds, false, target.Params.m_stObjInfo.m_bIsEnableColor, target.CellObjInfo.ColorID));
                            CellDestroy(target);
                        }
                    }
                }
            }
        }

        ///<Summary>볼이 아닌 특수 효과로 셀을 공격. (셀 효과 미발동.)</Summary>
        public void CellDamage_SkillTarget(int row, int col, CEBallObjController ballController, int _ATK)
        {
            int _count = this.CellObjLists[row, col].Count;
            if (_count > 0)
            {
                int _cLastLayer = _count - 1;
                if(this.CellObjLists[row, col][_cLastLayer].IsActiveCell()) 
                {
                    CEObj target = this.CellObjLists[row, col][_cLastLayer];
                    CellDamage_SkillTarget(target, ballController, _ATK);
                }
            }
        }

        ///<Summary>볼이 아닌 특수 효과로 셀을 파괴. (셀 효과 미발동.)</Summary>
        public void CellDestroy_SkillTarget(int row, int col, bool isForce = false)
        {
            int _count = this.CellObjLists[row, col].Count;
            if (_count > 0)
            {
                int _cLastLayer = _count - 1;
                if(this.CellObjLists[row, col][_cLastLayer].IsActiveCell()) 
                {
                    CEObj target = this.CellObjLists[row, col][_cLastLayer];
                    CellDestroy_SkillTarget(target, isForce);
                }
            }
        }

        ///<Summary>볼이 아닌 특수 효과로 셀을 파괴. (셀 효과 미발동.)</Summary>
        public void CellDestroy_SkillTarget(CEObj target, bool isForce = false)
        {
            if (target != null && target.IsActiveCell())
            {
                if (target.Params.m_stObjInfo.m_bIsSkillTarget || (isForce && target.Params.m_stObjInfo.m_bIsClearTarget))
                {
                    GlobalDefine.ShowEffect(EFXSet.FX_BREAK_BRICK, target.transform.position, GlobalDefine.GetCellColor(target.CellObjInfo.ObjKinds, target.Params.m_stObjInfo.m_bIsShieldCell, target.Params.m_stObjInfo.m_bIsEnableColor, target.CellObjInfo.ColorID));
                    if (isForce)
                        CellDestroyForce(target);
                    else
                        CellDestroy(target);
                }
            }
        }

        ///<Summary>셀 파괴. (열쇠 효과만 발동.)</Summary>
        public void CellDestroyForce(CEObj target)
        {
            CellDestroyForce(target.GetComponent<CECellObjController>());
        }

        ///<Summary>셀 파괴. (열쇠 효과만 발동.)</Summary>
        public void CellDestroyForce(CECellObjController target)
        {
            target.CellDestroy(isForce:true);
        }

        ///<Summary>셀 파괴. (열쇠 효과만 발동.)</Summary>
        public void CellDestroy(CEObj target)
        {
            CellDestroy(target.GetComponent<CECellObjController>());
        }

        ///<Summary>셀 파괴. (열쇠 효과만 발동.)</Summary>
        public void CellDestroy(CECellObjController target, bool isForce = false)
        {
            target.CellDestroy();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
    public partial class CECellObjController : CEObjController
    {
        public bool hideReserved;
        public bool missileReserved;

        private Coroutine hitCoroutine;

#region Bricks Collision

        public void GetDamage(CEBallObjController ballController, EObjKinds kindsType, EObjKinds kinds, int _ATK)
        {
            var oCellObj = this.GetOwner<CEObj>();
            
            if (!oCellObj.Params.m_stObjInfo.m_bIsEnableHit) 
                return;

			var stCellObjInfo = oCellObj.CellObjInfo;

            if (oCellObj.Params.m_stObjInfo.m_bIsShieldCell && stCellObjInfo.SHIELD > 0)
            {
                stCellObjInfo.SHIELD = Mathf.Max(KCDefine.B_VAL_0_INT, stCellObjInfo.SHIELD - _ATK);
                oCellObj.HPText.text = $"{stCellObjInfo.SHIELD}";
                oCellObj.SetCellObjInfo(stCellObjInfo);

                if (ballController.isOn_PowerBall)
                {
                    GlobalDefine.ShowEffect(EFXSet.FX_POWER_BALL_HIT, this.transform.position);
                }

                // 실드가 없을 경우
                if(stCellObjInfo.SHIELD <= KCDefine.B_VAL_0_INT) 
                {
                    ShieldAfterEffect(kindsType, kinds);
                    GlobalDefine.ShowEffect(EFXSet.FX_BREAK_BRICK, oCellObj.transform.position, GlobalDefine.GetCellColor(oCellObj.CellObjInfo.ObjKinds, true, false));
                    //CellDestroy();
                }
                else
                {
                    HitEffect();
                }
            }
            else
            {
                stCellObjInfo.HP = Mathf.Max(KCDefine.B_VAL_0_INT, stCellObjInfo.HP - _ATK);
            
                oCellObj.HPText.text = $"{stCellObjInfo.HP}";
                oCellObj.SetCellObjInfo(stCellObjInfo);
                oCellObj.SetSpriteColor(oCellObj.CellObjInfo.ObjKinds);

                if (ballController.isOn_PowerBall)
                {
                    GlobalDefine.ShowEffect(EFXSet.FX_POWER_BALL_HIT, this.transform.position);
                }

                // [특수 블럭] 히트시 발동.
                switch(kindsType)
                {
                    case EObjKinds.SPECIAL_BRICKS_LIGHTNING_01:
                        GetSpecial_Lightning(ballController, kindsType, kinds, _ATK);
                        break;
                }
                
                // 체력이 없을 경우
                if(stCellObjInfo.HP <= KCDefine.B_VAL_0_INT) 
                {
                    CellAfterEffect(ballController, kindsType, kinds);
                    GlobalDefine.ShowEffect(EFXSet.FX_BREAK_BRICK, oCellObj.transform.position, GlobalDefine.GetCellColor(oCellObj.CellObjInfo.ObjKinds, false, oCellObj.Params.m_stObjInfo.m_bIsEnableColor, oCellObj.CellObjInfo.ColorID));
                    CellDestroy();
                }
                else
                {
                    HitEffect();
                }
            }
        }

        private void GetSpecial(CEBallObjController ballController, EObjKinds kindsType, EObjKinds kinds, int _ATK)
        {
            var oCellObj = this.GetOwner<CEObj>();            
            if (oCellObj.Params.m_stObjInfo.m_bIsEnableHit)
            {
                GetDamage(ballController, kindsType, kinds, _ATK);
            }
            else
            {
                // [특수 블럭] 통과시 발동.
                switch(kindsType)
                {
                    case EObjKinds.SPECIAL_BRICKS_LASER_HORIZONTAL_01:
                    case EObjKinds.SPECIAL_BRICKS_LASER_VERTICAL_01:
                    case EObjKinds.SPECIAL_BRICKS_LASER_CROSS_01:
                        GetSpecial_Laser(ballController, kindsType, kinds);
                        break;
                    case EObjKinds.SPECIAL_BRICKS_BALL_DIFFUSION_01:
                    case EObjKinds.SPECIAL_BRICKS_BALL_AMPLIFICATION_01:
                        GetSpecial_Refract(ballController, kindsType, kinds);
                        break;
                    case EObjKinds.SPECIAL_BRICKS_POWERBALL_01:
                        GetSpecial_PowerBall(ballController, kindsType, kinds);
                        break;
                    default:
                        Debug.Log(CodeManager.GetMethodName() + string.Format("<color=red>{0}</color>", kindsType));
                        break;
                }
            }
        }

        private void GetObstacle(CEBallObjController ballController, EObjKinds kindsType, EObjKinds kinds, int _ATK)
        {
            var oCellObj = this.GetOwner<CEObj>();            
            if (oCellObj.Params.m_stObjInfo.m_bIsEnableHit)
            {
                GetDamage(ballController, kindsType, kinds, _ATK);
            }
            else
            {
                switch(kindsType)
                {
                    case EObjKinds.OBSTACLE_BRICKS_WARP_IN_01:
                        GetObstacle_Wormhole(ballController, kindsType, kinds);
                        break;
                    default:
                        //Debug.Log(CodeManager.GetMethodName() + string.Format("<color=red>{0}</color>", kindsType));
                        break;
                }
            }
        }

        private void GetItem(CEBallObjController ballController, EObjKinds kindsType, EObjKinds kinds, int _ATK)
        {
            switch(kindsType)
            {
                case EObjKinds.ITEM_BRICKS_BALL_01:
                    GetItem_BallPlus(ballController, kindsType, kinds);
                    break;
                case EObjKinds.ITEM_BRICKS_COINS_01:
                    GetItem_Ruby(ballController, kindsType, kinds);
                    break;
                default:
                    Debug.Log(CodeManager.GetMethodName() + string.Format("<color=red>{0}</color>", kindsType));
                    break;
            }

            CellDestroy();
        }

#endregion Bricks Collision


#region Turn End Action

        ///<Summary>셀 숨기기가 예약되었으면 셀을 숨긴다.</Summary>
        public void HideReservedCell()
        {
            if (hideReserved)
                CellDestroy();
        }

        ///<Summary>변경되는 셀이면 셀을 변경한다.</Summary>
        public void ChangeCellToExtraKinds()
        {
            if(this.ExtraObjKindsList.ExIsValid()) 
            {
                m_oSubIntDict[ESubKey.EXTRA_OBJ_KINDS_IDX] = (m_oSubIntDict[ESubKey.EXTRA_OBJ_KINDS_IDX] + KCDefine.B_VAL_1_INT) % this.ExtraObjKindsList.Count;
                    
                EObjKinds toKinds = this.ExtraObjKindsList[m_oSubIntDict[ESubKey.EXTRA_OBJ_KINDS_IDX]];
                Engine.ChangeCell(this, toKinds);
			}
        }

#endregion Turn End Action


#region Privates

        ///<Summary>턴 종료시 셀 숨기기를 예약한다.</Summary>
        private void SetHideReserved()
        {
            if (this.GetOwner<CEObj>().Params.m_stObjInfo.m_bIsOnce)
                hideReserved = true;
        }

        private void HitEffect()
        {
            if (hitCoroutine != null)
                StopCoroutine(hitCoroutine);
            
            hitCoroutine = StartCoroutine(CO_HitEffect());
        }

        private IEnumerator CO_HitEffect()
        {
            this.GetOwner<CEObj>().ToggleHitSprite(true);
            
            yield return Engine.hitEffectDelay;
            
            this.GetOwner<CEObj>().ToggleHitSprite(false);
        }

        ///<Summary>셀의 HP가 0이 되어 파괴시 발동하는 효과.</Summary>
        private void CellAfterEffect(CEBallObjController ballController, EObjKinds kindsType, EObjKinds kinds)
        {
            switch(kindsType)
            {
                case EObjKinds.SPECIAL_BRICKS_ADD_BALL_01:
                case EObjKinds.SPECIAL_BRICKS_ADD_BALL_02:
                case EObjKinds.SPECIAL_BRICKS_ADD_BALL_03:
                    GetSpecial_AddBall(ballController, kindsType, kinds);
                    break;
                case EObjKinds.SPECIAL_BRICKS_EXPLOSION_HORIZONTAL_01:
                case EObjKinds.SPECIAL_BRICKS_EXPLOSION_VERTICAL_01:
                case EObjKinds.SPECIAL_BRICKS_EXPLOSION_CROSS_01:
                case EObjKinds.SPECIAL_BRICKS_EXPLOSION_AROUND_01:
                    GetSpecial_Explosion(ballController, kindsType, kinds);
                    break;
                case EObjKinds.SPECIAL_BRICKS_ARROW_01:
                case EObjKinds.SPECIAL_BRICKS_ARROW_02:
                case EObjKinds.SPECIAL_BRICKS_ARROW_03:
                case EObjKinds.SPECIAL_BRICKS_ARROW_04:
                case EObjKinds.SPECIAL_BRICKS_ARROW_05:
                case EObjKinds.SPECIAL_BRICKS_ARROW_06:
                case EObjKinds.SPECIAL_BRICKS_ARROW_07:
                case EObjKinds.SPECIAL_BRICKS_ARROW_08:
                    GetSpecial_Arrow(ballController, kindsType, kinds);
                    break;
                case EObjKinds.SPECIAL_BRICKS_MISSILE_01:
                case EObjKinds.SPECIAL_BRICKS_MISSILE_02:
                    GetSpecial_Missile(ballController, kindsType, kinds);
                    break;
                case EObjKinds.SPECIAL_BRICKS_EARTHQUAKE_01:
                    GetSpecial_Earthquake(ballController, kindsType, kinds);
                    break;
                default:
                    //Debug.Log(CodeManager.GetMethodName() + string.Format("<color=red>{0}</color>", kindsType));
                    break;
            }
        }

        ///<Summary>셀의 실드가 0이 되어 파괴시 발동하는 효과.</Summary>
        private void ShieldAfterEffect(EObjKinds kindsType, EObjKinds kinds)
        {
            switch(kindsType)
            {
                case EObjKinds.OBSTACLE_BRICKS_WOODBOX_01:
                case EObjKinds.OBSTACLE_BRICKS_WOODBOX_02:
                    GetObstacle_WoodBox(kindsType, kinds);
                    break;
                default:
                    //Debug.Log(CodeManager.GetMethodName() + string.Format("<color=red>{0}</color>", kindsType));
                    break;
            }
        }

        ///<Summary>볼이 아닌 특수 효과로 셀을 공격. (셀 효과 미발동.)</Summary>
        private void CellDamage_SkillTarget(CEObj target, CEBallObjController ballController, int _ATK)
        {
            if (target != null)
            {
                EObjKinds kinds = target.Params.m_stObjInfo.m_eObjKinds;
                EObjKinds kindsType = (EObjKinds)((int)kinds).ExKindsToCorrectKinds(EKindsGroupType.SUB_KINDS_TYPE);
                
                if (target.Params.m_stObjInfo.m_bIsSkillTarget)
                {
                    if (target.Params.m_stObjInfo.m_bIsShieldCell && target.CellObjInfo.SHIELD > 0)
                    {
                        if (target.CellObjInfo.SHIELD > _ATK)
                            target.GetComponent<CECellObjController>().GetDamage(ballController, kindsType, kinds, _ATK);
                        else
                        {
                            GlobalDefine.ShowEffect(EFXSet.FX_BREAK_BRICK, target.transform.position, GlobalDefine.GetCellColor(target.CellObjInfo.ObjKinds, true, target.Params.m_stObjInfo.m_bIsEnableColor, target.CellObjInfo.ColorID));
                            target.GetComponent<CECellObjController>().CellDestroy();
                        }
                    }
                    else
                    {
                        if (target.CellObjInfo.HP > _ATK)
                            target.GetComponent<CECellObjController>().GetDamage(ballController, kindsType, kinds, _ATK);
                        else
                        {
                            GlobalDefine.ShowEffect(EFXSet.FX_BREAK_BRICK, target.transform.position, GlobalDefine.GetCellColor(target.CellObjInfo.ObjKinds, false, target.Params.m_stObjInfo.m_bIsEnableColor, target.CellObjInfo.ColorID));
                            target.GetComponent<CECellObjController>().CellDestroy();
                        }
                    }
                }
            }
        }

        ///<Summary>볼이 아닌 특수 효과로 셀을 공격. (셀 효과 미발동.)</Summary>
        private void CellDamage_SkillTarget(int row, int col, CEBallObjController ballController, int _ATK)
        {
            int _count = this.Engine.CellObjLists[row, col].Count;
            if (_count > 0)
            {
                int _cLastLayer = _count - 1;
                if(this.Engine.CellObjLists[row, col][_cLastLayer].gameObject.activeSelf) 
                {
                    CEObj target = this.Engine.CellObjLists[row, col][_cLastLayer];
                    CellDamage_SkillTarget(target, ballController, _ATK);
                }
            }
        }

        ///<Summary>볼이 아닌 특수 효과로 셀을 파괴. (셀 효과 미발동.)</Summary>
        private void CellDestroy_SkillTarget(int row, int col)
        {
            int _count = this.Engine.CellObjLists[row, col].Count;
            if (_count > 0)
            {
                int _cLastLayer = _count - 1;
                if(this.Engine.CellObjLists[row, col][_cLastLayer].gameObject.activeSelf) 
                {
                    CEObj target = this.Engine.CellObjLists[row, col][_cLastLayer];
                    if (target != null)
                    {
                        if (target.Params.m_stObjInfo.m_bIsSkillTarget)
                        {
                            GlobalDefine.ShowEffect(EFXSet.FX_BREAK_BRICK, target.transform.position, GlobalDefine.GetCellColor(target.CellObjInfo.ObjKinds, target.Params.m_stObjInfo.m_bIsShieldCell && target.CellObjInfo.SHIELD > 0, target.Params.m_stObjInfo.m_bIsEnableColor, target.CellObjInfo.ColorID));
                            target.GetComponent<CECellObjController>().CellDestroy();
                        }
                    }
                }
            }
        }

        ///<Summary>셀 파괴. (열쇠 효과만 발동.)</Summary>
        private void CellDestroy()
        {
            StopAllCoroutines();

            CEObj _ceObj = this.GetOwner<CEObj>();
            EObjKinds kinds = _ceObj.Params.m_stObjInfo.m_eObjKinds;
            EObjKinds kindsType = (EObjKinds)((int)kinds).ExKindsToCorrectKinds(EKindsGroupType.SUB_KINDS_TYPE);
            var stCellObjInfo = _ceObj.CellObjInfo;

            switch(kindsType)
            {
                case EObjKinds.OBSTACLE_BRICKS_KEY_01:
                    GetObstacle_Key(kindsType);
                    break;
                case EObjKinds.OBSTACLE_BRICKS_WOODBOX_01:
                case EObjKinds.OBSTACLE_BRICKS_WOODBOX_02:
                    if (_ceObj.Params.m_stObjInfo.m_bIsShieldCell && stCellObjInfo.SHIELD > 0)
                    {
                        GetObstacle_WoodBox(kindsType, kinds);
                        return;
                    }
                    break;
            }
            
            _ceObj.Params.m_stBaseParams.m_oCallbackDict.GetValueOrDefault(CEObjComponent.ECallback.ENGINE_OBJ_EVENT)?.Invoke(this.GetOwner<CEObj>(), EEngineObjEvent.DESTROY, string.Empty);
        }

#endregion Privates

    }
}

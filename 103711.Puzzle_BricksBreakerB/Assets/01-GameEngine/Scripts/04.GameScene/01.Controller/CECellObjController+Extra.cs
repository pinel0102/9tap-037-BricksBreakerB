using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
    public partial class CECellObjController : CEObjController
    {
        public bool hideReserved;

        private Coroutine hitCoroutine;

        private void Initialize(EObjKinds kinds)
        {
            switch(kinds)
            {
                case EObjKinds.OBSTACLE_BRICKS_FIX_03:
                    this.hideReserved = true;
                    break;
                default:
                    this.hideReserved = false;
                    break;
            }
        }

#region Bricks Collision

        public void GetDamage(CEBallObjController ballController, EObjKinds kindsType, EObjKinds kinds, int _ATK, bool isSoundPlay = true)
        {
            var oCellObj = this.GetOwner<CEObj>();
            
            if (!oCellObj.Params.m_stObjInfo.m_bIsEnableHit) 
                return;

			var stCellObjInfo = oCellObj.CellObjInfo;

            if (oCellObj.Params.m_stObjInfo.m_bIsShieldCell)
            {
                stCellObjInfo.SHIELD = Mathf.Max(KCDefine.B_VAL_0_INT, stCellObjInfo.SHIELD - _ATK);
                oCellObj.HPText.text = $"{stCellObjInfo.SHIELD}";
                oCellObj.SetCellObjInfo(stCellObjInfo);

                if (ballController.isOn_PowerBall)
                {
                    GlobalDefine.ShowEffect(EFXSet.FX_POWER_BALL_HIT, oCellObj.centerPosition);
                }

                // 실드가 없을 경우
                if(stCellObjInfo.SHIELD <= KCDefine.B_VAL_0_INT) 
                {
                    ShieldAfterEffect(kindsType, kinds);
                    GlobalDefine.ShowEffect(EFXSet.FX_BREAK_BRICK, oCellObj.centerPosition, GlobalDefine.GetCellColor(oCellObj.CellObjInfo.ObjKinds, true, false));
                }
                else
                {
                    HitEffect(kindsType, kinds, isSoundPlay);
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
                    GlobalDefine.ShowEffect(EFXSet.FX_POWER_BALL_HIT, oCellObj.centerPosition);
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
                    GlobalDefine.ShowEffect(EFXSet.FX_BREAK_BRICK, oCellObj.centerPosition, GlobalDefine.GetCellColor(oCellObj.CellObjInfo.ObjKinds, false, oCellObj.Params.m_stObjInfo.m_bIsEnableColor, oCellObj.CellObjInfo.ColorID));
                    CellDestroy(isSoundPlay);
                }
                else
                {
                    HitEffect(kindsType, kinds, isSoundPlay);
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

                HitEffectNoSound();
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
                        HitEffectNoSound();
                        break;
                    case EObjKinds.OBSTACLE_BRICKS_FIX_01:
                        switch(kinds)
                        {
                            case EObjKinds.OBSTACLE_BRICKS_FIX_02:
                            case EObjKinds.OBSTACLE_BRICKS_FIX_03:
                                GlobalDefine.PlaySoundFX(ESoundSet.SOUND_ATTACK_IRON);
                                break;
                        }
                        break;
                    case EObjKinds.OBSTACLE_BRICKS_LOCK_01:
                    case EObjKinds.OBSTACLE_BRICKS_CLOSE_01:
                        GlobalDefine.PlaySoundFX(ESoundSet.SOUND_ATTACK_IRON);
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

            CellDestroy(false);
        }

#endregion Bricks Collision


#region Turn End Action

        ///<Summary>셀 숨기기가 예약되었으면 셀을 숨긴다.</Summary>
        public void HideReservedCell()
        {
            if (hideReserved)
                CellDestroy(false);
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

        private void HitEffect(EObjKinds kindsType, EObjKinds kinds, bool isSoundPlay = true)
        {
            switch(kindsType)
            {
                case EObjKinds.OBSTACLE_BRICKS_WOODBOX_01:
                    if(isSoundPlay)
                        GlobalDefine.PlaySoundFX(ESoundSet.SOUND_ATTACK_WOOD);
                    break;
                default:
                    if(isSoundPlay)
                        GlobalDefine.PlaySoundFX(ESoundSet.SOUND_ATTACK_NORMAL);
                    break;
            }

            if (hitCoroutine != null)
                StopCoroutine(hitCoroutine);
            
            hitCoroutine = StartCoroutine(CO_HitEffect());
        }

        public void HitEffectNoSound()
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
                    GetSpecial_Explosion(kindsType, kinds);
                    break;
                case EObjKinds.SPECIAL_BRICKS_ARROW_01:
                case EObjKinds.SPECIAL_BRICKS_ARROW_02:
                case EObjKinds.SPECIAL_BRICKS_ARROW_03:
                case EObjKinds.SPECIAL_BRICKS_ARROW_04:
                case EObjKinds.SPECIAL_BRICKS_ARROW_05:
                case EObjKinds.SPECIAL_BRICKS_ARROW_06:
                case EObjKinds.SPECIAL_BRICKS_ARROW_07:
                case EObjKinds.SPECIAL_BRICKS_ARROW_08:
                    GetSpecial_Arrow(kindsType, kinds);
                    break;
                case EObjKinds.SPECIAL_BRICKS_MISSILE_01:
                case EObjKinds.SPECIAL_BRICKS_MISSILE_02:
                    GetSpecial_Missile(kindsType, kinds);
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

        ///<Summary>셀 파괴. (열쇠 효과만 발동.)</Summary>
        public void CellDestroy(bool isSoundPlay = true, bool isForce = false)
        {
            StopAllCoroutines();
            
            CEObj myCell = this.GetOwner<CEObj>();
            EObjKinds kinds = myCell.Params.m_stObjInfo.m_eObjKinds;
            EObjKinds kindsType = (EObjKinds)((int)kinds).ExKindsToCorrectKinds(EKindsGroupType.SUB_KINDS_TYPE);
            
            if(myCell.Params.m_stObjInfo.m_bIsClearTarget)
                Engine.GetScore();

            switch(kindsType)
            {
                case EObjKinds.OBSTACLE_BRICKS_KEY_01:
                    GetObstacle_Key(kindsType);
                    break;
                case EObjKinds.OBSTACLE_BRICKS_WOODBOX_01:
                case EObjKinds.OBSTACLE_BRICKS_WOODBOX_02:
                    if (myCell.Params.m_stObjInfo.m_bIsShieldCell && !isForce)
                    {
                        GetObstacle_WoodBox(kindsType, kinds);
                        return;
                    }
                    else
                    {
                        if (isSoundPlay)
                            GlobalDefine.PlaySoundFX(ESoundSet.SOUND_BRICK_DESTROY);
                    }
                    break;
                case EObjKinds.SPECIAL_BRICKS_EXPLOSION_ALL_01:
                    if (Engine.isExplosionAll)
                    {
                        if (isSoundPlay)
                            GlobalDefine.PlaySoundFX(ESoundSet.SOUND_BRICK_DESTROY);
                    }
                    else
                    {
                        Engine.isExplosionAll = true;
                        GetSpecial_Explosion(kindsType, kinds);
                    }
                    break;
                default:
                    if (isSoundPlay)
                        GlobalDefine.PlaySoundFX(ESoundSet.SOUND_BRICK_DESTROY);
                    break;
            }

            for(int i=0; i < myCell.placeHolder.Count; i++)
            {
                var placeHolderCell = Engine.CellObjLists[myCell.row + myCell.placeHolder[i].y, myCell.col + myCell.placeHolder[i].x];
                int _cLastLayer = placeHolderCell.Count - 1;

                if (placeHolderCell[_cLastLayer] != myCell)
                    placeHolderCell[_cLastLayer].GetComponent<CECellObjController>().CellDestroy(false);
            }

            myCell.SetCellActive(false);
            myCell.Params.m_stBaseParams.m_oCallbackDict.GetValueOrDefault(CEObjComponent.ECallback.ENGINE_OBJ_EVENT)?.Invoke(this.GetOwner<CEObj>(), EEngineObjEvent.DESTROY, string.Empty);
        }

#endregion Privates

    }
}

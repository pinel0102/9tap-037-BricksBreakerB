using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

namespace NSEngine {
	/** 서브 객체 */
	public partial class CEObj : CEObjComponent 
    {
        [Header("★ [Reference] Ball")]
        public GameObject FXPowerBall;

        [Header("★ [Reference] Cell")]
        public Transform FXRoot;
        public SpriteRenderer HitSprite;
        public SpriteRenderer UpperSprite;
        public List<Vector2Int> placeHolder = new List<Vector2Int>();

        [Header("★ [Live] Cell Info")]
        public EObjKinds kinds;
        public Vector3 centerPosition => transform.position + spriteOffset; // World Position
        public int row;
        public int col;
        public int layer;
        private Vector3 spriteOffset;

        public void SetPlaceHolder(Vector3Int cellScale)
        {
            placeHolder.Clear();

            if (cellScale != Vector3Int.one)
            {
                for(int _y = 0; _y < cellScale.y; _y++)
                {
                    for(int _x = 0; _x < cellScale.x; _x++)
                    {
                        if (_y == 0 && _x == 0)
                            continue;
                        
                        placeHolder.Add(new Vector2Int(_x, _y));
                    }
                }
            }
        }

        private void SetTargetSprite()
        {
            InitSprite(GlobalDefine.IsNeedSubSprite(kinds));
        }

        public void InitSprite(bool _needSubSprite)
        {
            TargetSprite.transform.localPosition = spriteOffset = Engine.GetSpriteCenter(Access.CellSize, this.Params.m_stObjInfo.m_stSize);
            
            if (_needSubSprite)
            {
                SetSprite(TargetSprite, EObjKinds.NORM_BRICKS_SQUARE_01);
                SetHitSprite(EObjKinds.NORM_BRICKS_SQUARE_01);

                SetSprite(UpperSprite, kinds);
            }
            else
            {
                SetSprite(TargetSprite, kinds);
                SetHitSprite(kinds);
            }

            ToggleUpperSprite(_needSubSprite);
        }

        private void SetSprite(SpriteRenderer _sprite, EObjKinds _kinds)
        {
            // 스프라이트를 설정한다
            _sprite?.ExSetSprite<SpriteRenderer>(Access.GetSprite(_kinds));
            _sprite?.ExSetSortingOrder(Access.GetSortingOrderInfo(_kinds));

            if (_sprite != null)
            {
                _sprite.size = Engine.GetSpriteSize(Access.CellSize, this.Params.m_stObjInfo.m_stSize) + GlobalDefine.CELL_SPRITE_ADJUSTMENT;
                this.SetSpriteColor(_kinds);
            }
        }

        private void SetHitSprite(EObjKinds _kinds)
        {
            // Cell HitSprite 설정.
            if (this.HitSprite != null)
            {
                this.HitSprite?.ExSetSprite<SpriteRenderer>(Access.GetSprite(_kinds));
                this.HitSprite.sortingOrder = this.TargetSprite.sortingOrder + GlobalDefine.HitEffect_Order;
                this.HitSprite.size = Engine.GetSpriteSize(Access.CellSize, this.Params.m_stObjInfo.m_stSize) + GlobalDefine.CELL_SPRITE_ADJUSTMENT;

                ToggleHitSprite(false);
            }
        }

        public void SetSpriteColor(EObjKinds cellKinds)
        {
            this.TargetSprite.color = GlobalDefine.GetCellColor(cellKinds, Params.m_stObjInfo.m_bIsShieldCell, Params.m_stObjInfo.m_bIsEnableColor, CellObjInfo.ColorID, CellObjInfo.HP);
        }

        public void RefreshText(EObjKinds kinds)
        {
            if (HPText != null)
                HPText.text = Params.m_stObjInfo.m_bIsEnableHit && Params.m_stObjInfo.m_bIsEnableReflect ? ((Params.m_stObjInfo.m_bIsShieldCell) ? CellObjInfo.SHIELD.ToString() : CellObjInfo.HP.ToString()) : string.Empty;
        }

        public void ToggleHitSprite(bool _show)
        {
            if (HitSprite != null)
                HitSprite.gameObject.SetActive(_show);
        }

        public void ToggleUpperSprite(bool _show)
        {
            if (UpperSprite != null)
                UpperSprite.gameObject.SetActive(_show);
        }

        public void AddCellEffect(EObjKinds kindsType)
        {
            switch(kindsType)
            {
                case EObjKinds.SPECIAL_BRICKS_EXPLOSION_HORIZONTAL_01:
                case EObjKinds.SPECIAL_BRICKS_EXPLOSION_VERTICAL_01:
                case EObjKinds.SPECIAL_BRICKS_EXPLOSION_CROSS_01:
                    GlobalDefine.AddCellEffect(EFXSet.FX_BOMB_FLAME, FXRoot, GlobalDefine.FXBombFlame_Position_Default);
                    break;
                case EObjKinds.SPECIAL_BRICKS_EXPLOSION_AROUND_01:
                    GlobalDefine.AddCellEffect(EFXSet.FX_BOMB_FLAME, FXRoot, GlobalDefine.FXBombFlame_Position_3x3);
                    break;
                case EObjKinds.SPECIAL_BRICKS_EXPLOSION_ALL_01:
                    GlobalDefine.AddCellEffect(EFXSet.FX_BOMB_FLAME, FXRoot, GlobalDefine.FXBombFlame_Position_All);
                    break;
            }
        }

        public void SetCellActive(bool _isActive, bool _showEffect = false)
        {
            TargetSprite.gameObject.SetActive(_isActive);
            gameObject.SetActive(_isActive);

            if (_isActive && _showEffect)
            {
                AppearEffect();
            }
        }

        ///<Summary>살아 있는 셀 중에서 인게임 영역 안에 있는 셀.</Summary>
        public bool IsActiveCell()
        {
            return this.gameObject.activeInHierarchy && TargetSprite.gameObject.activeSelf;
        }

        private void AppearEffect()
        {
            Color endColor = this.TargetSprite.color;
            TargetSprite.color = GlobalDefine.COLOR_CELL_APPEAR;
            TargetSprite.DOColor(endColor, GlobalDefine.FXCellAppear_Time);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace NSEngine {
	/** 서브 객체 */
	public partial class CEObj : CEObjComponent 
    {
        [Header("★ [Reference] Ball")]
        public GameObject FXPowerBall;

        [Header("★ [Reference] Cell")]
        public Transform FXRoot;
        public SpriteRenderer HitSprite;

        [Header("★ [Live] Cell Info")]
        public int row;
        public int col;
        public int layer;

        public void SetSpriteColor(EObjKinds cellKinds)
        {
            this.TargetSprite.color = GlobalDefine.GetCellColor(cellKinds, Params.m_stObjInfo.m_bIsEnableColor, CellObjInfo.ColorID, CellObjInfo.HP);
        }

        public void RefreshText(EObjType cellType)
        {
            HPText.text = Params.m_stObjInfo.m_bIsEnableHit && Params.m_stObjInfo.m_bIsEnableReflect ? $"{CellObjInfo.HP}" : string.Empty;
        }

        public void ToggleHitSprite(bool _show)
        {
            HitSprite.gameObject.SetActive(_show);
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
            }
        }
    }
}

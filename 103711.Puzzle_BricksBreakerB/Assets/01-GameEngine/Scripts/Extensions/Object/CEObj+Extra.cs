using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace NSEngine {
	/** 서브 객체 */
	public partial class CEObj : CEObjComponent 
    {
        public SpriteRenderer HitSprite;
        public int row;
        public int column;
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
    }
}

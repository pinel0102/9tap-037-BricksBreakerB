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
            TMPro.TMP_Text _text = m_oSubTextDict.GetValueOrDefault(ESubKey.HP_TEXT);

            if(_text != null)
            {
                switch(cellType)
                {
                    case EObjType.NORM_BRICKS:
                        _text.text = $"{CellObjInfo.HP}";
                        break;
                    case EObjType.OBSTACLE_BRICKS:
                        _text.text = string.Empty;
                        break;                
                    case EObjType.ITEM_BRICKS:
                        switch(CellObjInfo.ObjKinds)
                        {
                            case EObjKinds.ITEM_BRICKS_BALL_01:
                            case EObjKinds.ITEM_BRICKS_BALL_02:
                            case EObjKinds.ITEM_BRICKS_BALL_03:
                            case EObjKinds.ITEM_BRICKS_BALL_04:
                            default:
                                _text.text = string.Empty;
                                break;
                        }
                        break;
                    case EObjType.SPECIAL_BRICKS:
                        switch(CellObjInfo.ObjKinds)
                        {
                            case EObjKinds.SPECIAL_BRICKS_LASER_HORIZONTAL_01:
                            case EObjKinds.SPECIAL_BRICKS_LASER_VERTICAL_01:
                            case EObjKinds.SPECIAL_BRICKS_LASER_CROSS_01:
                            default:
                                _text.text = string.Empty;
                                break;
                        }
                        break;
                    default:
                        Debug.Log(CodeManager.GetMethodName() + string.Format("<color=red>{0}</color>", cellType));
                        _text.text = string.Empty;
                        break;
                }
            }
        }

        public void ToggleHitSprite(bool _show)
        {
            HitSprite.gameObject.SetActive(_show);
        }
    }
}

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

        public void SetSpriteColor()
        {
            EObjKinds kinds = Params.m_stObjInfo.m_eObjKinds;
            EObjType cellType = (EObjType)((int)kinds).ExKindsToType();

            Color _color = new Color();

            switch(cellType)
            {
                case EObjType.BALL:
                    _color = GlobalDefine.BricksColor[2];
                    break;
                case EObjType.NORM_BRICKS:
                    _color = GlobalDefine.BricksColor[1];
                    break;
                case EObjType.OBSTACLE_BRICKS:
                case EObjType.ITEM_BRICKS:
                case EObjType.SPECIAL_BRICKS:
                    _color = GlobalDefine.BricksColor[0];
                    break;
                default:
                    _color = GlobalDefine.BricksColor[0];
                    break;
            }

            this.TargetSprite.color = _color;
        }

        public void RefreshText()
        {
            TMPro.TMP_Text _text = m_oSubTextDict.GetValueOrDefault(ESubKey.HP_TEXT);

            if(_text != null)
            {
                EObjType cellType = (EObjType)((int)CellObjInfo.ObjKinds).ExKindsToType();

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
                                _text.text = $"+{GlobalDefine.GetItem_BallPlus[((int)CellObjInfo.ObjKinds).ExKindsToDetailSubKindsTypeVal()]}";
                                break;
                        }
                        break;
                    case EObjType.SPECIAL_BRICKS:
                        switch(CellObjInfo.ObjKinds)
                        {
                            case EObjKinds.SPECIAL_BRICKS_LASER_HORIZONTAL_01:
                            case EObjKinds.SPECIAL_BRICKS_LASER_VERTICAL_01:
                            case EObjKinds.SPECIAL_BRICKS_LASER_CROSS_01:
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

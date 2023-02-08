using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
using System.Globalization;
using UnityEngine.EventSystems;
using EnhancedUI.EnhancedScroller;
using TMPro;

namespace LevelEditorScene {
    public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate
    {
        public Vector2 currentCellSize => new Vector2(NSEngine.Access.MaxGridSize.x / (float)NSEngine.KDefine.E_DEF_NUM_CELLS.x, NSEngine.Access.MaxGridSize.y / (float)NSEngine.KDefine.E_DEF_NUM_CELLS.y);
        public Color drawCellColor = Color.white;
        public Color drawCellColorOld = Color.white;

        private void SetCellColor(string _newColorHex)
        {
            drawCellColorOld = drawCellColor;

            if(!ColorUtility.TryParseHtmlString(_newColorHex, out drawCellColor))
                drawCellColor = Color.white;
        }

        private void SetCellColor(Color _newColor)
        {
            drawCellColorOld = drawCellColor;
            drawCellColor = _newColor;
        }


#region Initialize

        private void SetupSpriteGrid(EObjKinds kinds, SpriteRenderer a_oOutObjSprite, Sprite _sprite)
        {
            a_oOutObjSprite.drawMode = SpriteDrawMode.Sliced;
            a_oOutObjSprite.sprite = _sprite;

            if (_sprite == null)
                return;
            
            if (a_oOutObjSprite.sprite.textureRect.size.x > currentCellSize.x || a_oOutObjSprite.sprite.textureRect.size.y > currentCellSize.y)
                a_oOutObjSprite.size = currentCellSize + (Vector2)GlobalDefine.CELL_SPRITE_ADJUSTMENT;
            else
                a_oOutObjSprite.size = a_oOutObjSprite.sprite.textureRect.size;

            SetSpriteColor(kinds, a_oOutObjSprite);
        }

        private void SetupSpriteREUIs(EObjKinds kinds, Image _image)
        {
            SetSpriteColor(kinds, _image);
        }

#endregion Initialize

        private void SetSpriteColor(EObjKinds kinds, SpriteRenderer _spriteRenderer)
        {
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

            _spriteRenderer.color = _color;
        }

        private void SetSpriteColor(EObjKinds kinds, Image _image)
        {
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

            _image.color = _color;
        }

        public void RefreshText(STCellObjInfo CellObjInfo, TMP_Text _text)
        {
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
    }
}
#endif
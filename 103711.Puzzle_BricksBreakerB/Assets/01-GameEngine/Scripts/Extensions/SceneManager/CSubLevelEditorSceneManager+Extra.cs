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
        public GameObject tooltipObject;
        public Text tooltipText;
        public GameObject colorPickerObject;
        //public ColorPicker colorPicker;
        public Vector2 currentCellSize => new Vector2(NSEngine.Access.MaxGridSize.x / (float)NSEngine.KDefine.E_DEF_NUM_CELLS.x, NSEngine.Access.MaxGridSize.y / (float)NSEngine.KDefine.E_DEF_NUM_CELLS.y);
        
        public int currentHP;
        public int currentColorID;
        
        //public string drawCellColorHex => string.Format(FORMAT_HEX, ColorUtility.ToHtmlStringRGBA(drawCellColor));
        //public Color drawCellColor = Color.white;
        //public Color drawCellColorOld = Color.white;
        //private const string FORMAT_HEX = "#{0}";

        private List<KeyValuePair<EObjKinds, Button>> listScrollerCellView = new List<KeyValuePair<EObjKinds, Button>>();

#region Initialize

        private void ExtraStart()
        {
            GetDevList();
            
            //OnCloseColorPicker();

            //colorPicker.onColorChanged += SetDrawCellColor;
            //colorPicker.awakeColor = drawCellColor;
        }

        private void SetupSpriteGrid(STCellObjInfo a_stCellObjInfo, EObjKinds cellKinds, SpriteRenderer a_oOutObjSprite, Sprite _sprite)
        {
            a_oOutObjSprite.drawMode = SpriteDrawMode.Sliced;
            a_oOutObjSprite.sprite = _sprite;

            if (_sprite == null)
                return;
            
            if (a_oOutObjSprite.sprite.textureRect.size.x > currentCellSize.x || a_oOutObjSprite.sprite.textureRect.size.y > currentCellSize.y)
                a_oOutObjSprite.size = currentCellSize + (Vector2)GlobalDefine.CELL_SPRITE_ADJUSTMENT;
            else
                a_oOutObjSprite.size = a_oOutObjSprite.sprite.textureRect.size;
            
            a_oOutObjSprite.color = GlobalDefine.GetCellColor(cellKinds, a_stCellObjInfo.ColorID, a_stCellObjInfo.HP);
        }

        private void SetupSpriteREUIs(EObjKinds cellKinds, Image _image)
        {
            _image.color = GlobalDefine.GetCellColor(cellKinds, currentColorID, currentHP);
        }

        private void GetDevList()
        {
            //GlobalDefine.devComplete
        }

#endregion Initialize


#region Cell Color

        public void OnToggleColorPicker()
        {
            //colorPicker.OnClickRevert();
            //colorPickerObject.SetActive(!colorPickerObject.activeInHierarchy);
        }

        public void OnCloseColorPicker()
        {
            //colorPicker.OnClickRevert();
            //colorPickerObject.SetActive(false);
        }

        /*public void SetDrawCellColor(string _newColorHex)
        {
            drawCellColorOld = drawCellColor;

            if(!ColorUtility.TryParseHtmlString(_newColorHex, out drawCellColor))
                drawCellColor = Color.white;

            //Debug.Log(CodeManager.GetMethodName() + string.Format("{0}", drawCellColorHex));

            UpdateUIsState();
            UpdateRightUIsColor();
        }

        public void SetDrawCellColor(Color _newColor)
        {
            drawCellColorOld = drawCellColor;
            drawCellColor = _newColor;

            //Debug.Log(CodeManager.GetMethodName() + string.Format("{0}", drawCellColorHex));

            UpdateUIsState();
            UpdateRightUIsColor();
        }*/

        public void OnChangeHP(string _value)
        {
            if (int.TryParse(_value, out int _intValue) && _intValue > -1)
                currentHP = _intValue;
            }

        public void OnChangeColorID(int _value)
        {
            if (_value > -1 && _value < GlobalDefine.colorList.Count)
                currentColorID = _value;
        }

        private void UpdateRightUIsColor()
        {
            for(int i = 0; i < listScrollerCellView.Count; ++i) {
                SetupSpriteREUIs(listScrollerCellView[i].Key, listScrollerCellView[i].Value.image);
			}
        }

#endregion Cell Color


#region Cell Text

        public void RefreshText(STCellObjInfo CellObjInfo, STObjInfo stObjInfo, TMP_Text _text)
        {
            if (_text != null)
                _text.text = stObjInfo.m_bIsEnableHit && stObjInfo.m_bIsEnableReflect ? $"{CellObjInfo.HP}" : string.Empty;
        }

#endregion Cell Text

    }
}
#endif
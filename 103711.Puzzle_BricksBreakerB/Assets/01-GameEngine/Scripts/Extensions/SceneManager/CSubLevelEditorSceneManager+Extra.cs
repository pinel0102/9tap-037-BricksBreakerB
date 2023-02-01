using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using System.Globalization;
using UnityEngine.EventSystems;
using EnhancedUI.EnhancedScroller;
using TMPro;

namespace LevelEditorScene {
    public partial class CSubLevelEditorSceneManager : CLevelEditorSceneManager, IEnhancedScrollerDelegate
    {
        public Vector2 currentCellSize => new Vector2(NSEngine.Access.MaxGridSize.x / (float)NSEngine.KDefine.E_DEF_NUM_CELLS.x, NSEngine.Access.MaxGridSize.y / (float)NSEngine.KDefine.E_DEF_NUM_CELLS.y);

        private void SetupSpriteSliced(SpriteRenderer a_oOutObjSprite, Sprite _sprite)
        {
            a_oOutObjSprite.drawMode = SpriteDrawMode.Sliced;
            a_oOutObjSprite.sprite = _sprite;

            if (_sprite == null)
                return;
            
            if (a_oOutObjSprite.sprite.textureRect.size.x > currentCellSize.x || a_oOutObjSprite.sprite.textureRect.size.y > currentCellSize.y)
                a_oOutObjSprite.size = currentCellSize;
            else
                a_oOutObjSprite.size = a_oOutObjSprite.sprite.textureRect.size;
        }
    }
}
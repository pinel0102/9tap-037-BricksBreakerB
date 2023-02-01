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

        private void PreSetupSprite(STCellInfo a_stCellInfo, STCellObjInfo a_stCellObjInfo, SpriteRenderer a_oOutObjSprite)
        {
            a_oOutObjSprite.drawMode = SpriteDrawMode.Sliced;
            a_oOutObjSprite.sprite = Access.GetEditorObjSprite(a_stCellObjInfo.ObjKinds, KCDefine.B_PREFIX_LEVEL_EDITOR_SCENE);
            
            if (a_oOutObjSprite.sprite.textureRect.size.x > currentCellSize.x || a_oOutObjSprite.sprite.textureRect.size.y > currentCellSize.y)
                a_oOutObjSprite.size = currentCellSize;
            else
                a_oOutObjSprite.size = a_oOutObjSprite.sprite.textureRect.size;

            a_oOutObjSprite.transform.localPosition = this.SelGridInfo.m_stPivotPos + a_stCellInfo.m_stIdx.ExToPos(NSEngine.Access.CellCenterOffset, NSEngine.Access.CellSize);

			a_oOutObjSprite.ExSetSortingOrder(NSEngine.Access.GetSortingOrderInfo(a_stCellObjInfo.ObjKinds));
        }
    }
}
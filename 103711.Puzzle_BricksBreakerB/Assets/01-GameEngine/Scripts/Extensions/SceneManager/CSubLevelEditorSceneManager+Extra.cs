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
        public ModifyCell modifyCell;
        public GameObject tooltipObject;
        public Text tooltipText;
        public GameObject colorPickerObject;
        //public ColorPicker colorPicker;
        public Vector2 currentCellSize => new Vector2(NSEngine.Access.MaxGridSize.x / (float)NSEngine.KDefine.E_DEF_NUM_CELLS.x, NSEngine.Access.MaxGridSize.y / (float)NSEngine.KDefine.E_DEF_NUM_CELLS.y);
        
        public int currentHP;
        public int currentColorID;

        private SpriteRenderer cursorSubSprite;
        
        //public string drawCellColorHex => string.Format(FORMAT_HEX, ColorUtility.ToHtmlStringRGBA(drawCellColor));
        //public Color drawCellColor = Color.white;
        //public Color drawCellColorOld = Color.white;
        //private const string FORMAT_HEX = "#{0}";

        private List<KeyValuePair<EObjKinds, Button>> listScrollerCellView = new List<KeyValuePair<EObjKinds, Button>>();

#region Initialize

        private void ExtraStart()
        {
            GetDevList();

            modifyCell.onModifyComplete += ModifyCellUpdate;
            
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
            
            a_oOutObjSprite.size = a_oOutObjSprite.size = currentCellSize + (Vector2)GlobalDefine.CELL_SPRITE_ADJUSTMENT;            
            a_oOutObjSprite.color = GlobalDefine.GetCellColorEditor(cellKinds, a_stCellObjInfo.ColorID, a_stCellObjInfo.HP);
        }

        private void SetupSpriteREUIs(EObjKinds cellKinds, Image _image)
        {
            _image.color = GlobalDefine.GetCellColorEditor(cellKinds, currentColorID, currentHP);
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
                _text.text = (stObjInfo.m_bIsEnableHit || GlobalDefine.IsExtraObjEnableHit(stObjInfo.m_oExtraObjKindsList)) && stObjInfo.m_bIsEnableReflect ? ((stObjInfo.m_bIsShieldCell && CellObjInfo.SHIELD > 0) ? CellObjInfo.SHIELD.ToString() : CellObjInfo.HP.ToString()) : string.Empty;
        }
        
#endregion Cell Text

        private void CopyCurrentCellInfo(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			var stPos = a_oEventData.ExGetLocalPos(this.ObjRoot, this.ScreenSize);
			var stIdx = stPos.ExToIdx(this.SelGridInfo.m_stPivotPos, NSEngine.Access.CellSize);

			// 인덱스가 유효 할 경우
			if(this.SelLevelInfo.m_oCellInfoDictContainer.ExIsValidIdx(stIdx) && !stIdx.Equals(m_oVec3IntDict[EKey.PREV_CELL_IDX])) {
				var stCellInfo = this.SelLevelInfo.GetCellInfo(stIdx);

                // 셀 정보 복사.
                if(m_oObjKindsDict[EKey.SEL_OBJ_KINDS].ExIsValid()) {
                    int index = stCellInfo.m_oCellObjInfoList.Count - 1;
                    if (index >= 0)
                    {
                        STCellObjInfo cellInfo = stCellInfo.m_oCellObjInfoList[stCellInfo.m_oCellObjInfoList.Count - 1];                        
                        CObjInfoTable.Inst.TryGetObjInfo(cellInfo.ObjKinds, out STObjInfo stObjInfo);
                        
                        m_oSubInputDict[ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_HP_INPUT].text = cellInfo.HP.ToString();
                        m_oSubInputDict[ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_SHIELD_INPUT].text = stObjInfo.m_bIsShieldCell ? cellInfo.SHIELD.ToString() : 0.ToString();
                        m_oSubInputDict[ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_ATK_INPUT].text = cellInfo.ATK.ToString();

                        if(stObjInfo.m_bIsEnableColor)
                        {
                            m_oSubDropDict[ESubKey.RE_UIS_PAGE_UIS_02_CELL_OBJ_COLOR_DROP].value = cellInfo.ColorID;
                            this.SetREUIsPageUIs02CellObjColor(cellInfo.ColorID);
                        }

                        this.OnTouchREUIsPageUIs02ScrollerCellViewBtn(cellInfo.ObjKinds);
					    this.SetREUIsPageUIs02ObjSize(cellInfo.m_stSize.x, cellInfo.m_stSize.y);
                    }
                }

				this.UpdateUIsState();
				m_oVec3IntDict[EKey.PREV_CELL_IDX] = stIdx;
			}
		}

        private void ModifyCurrentCellInfo(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
			var stPos = a_oEventData.ExGetLocalPos(this.ObjRoot, this.ScreenSize);
			var stIdx = stPos.ExToIdx(this.SelGridInfo.m_stPivotPos, NSEngine.Access.CellSize);

			// 인덱스가 유효 할 경우
			if(this.SelLevelInfo.m_oCellInfoDictContainer.ExIsValidIdx(stIdx) && !stIdx.Equals(m_oVec3IntDict[EKey.PREV_CELL_IDX])) {
				var stCellInfo = this.SelLevelInfo.GetCellInfo(stIdx);

                // 셀 정보 복사.
                if(m_oObjKindsDict[EKey.SEL_OBJ_KINDS].ExIsValid()) {
                    int index = stCellInfo.m_oCellObjInfoList.Count - 1;
                    if (index >= 0)
                    {
                        STCellObjInfo cellInfo = stCellInfo.m_oCellObjInfoList[stCellInfo.m_oCellObjInfoList.Count - 1];
                        
                        modifyCell.OpenModifyWindow(cellInfo);
                    }
                }

				this.UpdateUIsState();
				m_oVec3IntDict[EKey.PREV_CELL_IDX] = stIdx;
			}
		}

        private void ModifyCellUpdate()
        {
            this.UpdateUIsState();
        }
    }
}
#endif
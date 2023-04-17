using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
    public partial class CEngine : CComponent
    {
        private bool isfirstScreenOnly;
        private int firstRow;
        private int lastRow;
        private int activeLine;
        private Vector3 CELL_SPRITE_ADJUSTMENT;
        private Vector3 MaxGridSize;
        private Vector3 CellSize;
        private Vector3 CellCenterOffset;
        private Vector3 GridOffset;
        private STSortingOrderInfo sortingOrderInfoCell;
        private STSortingOrderInfo sortingOrderInfoSub;

        private void InitPreview()
        {
            sortingOrderInfoCell = new STSortingOrderInfo(){ m_nOrder = KCDefine.U_SORTING_O_DEF, m_oLayer = KCDefine.U_SORTING_L_FOREGROUND };
            sortingOrderInfoSub = new STSortingOrderInfo(){ m_nOrder = KCDefine.B_VAL_1_INT, m_oLayer = KCDefine.U_SORTING_L_FOREGROUND };

            isfirstScreenOnly = false;
        }

        public void SetupPreview(RectTransform preview, SpriteMask mask)
        {
            firstRow = -1;
            lastRow = -1;

            mask.ExSetSortingOrder(sortingOrderInfoCell);

            // 레벨 정보가 존재 할 경우
            if(CGameInfoStorage.Inst.PlayLevelInfo != null) {

                if (isfirstScreenOnly)
                {
                    CELL_SPRITE_ADJUSTMENT = new Vector3(-1f, -1f, 0);
                    MaxGridSize = new Vector3(preview.sizeDelta.x, preview.sizeDelta.y, 0);
                    CellSize = new Vector3(MaxGridSize.x / (float)KDefine.E_DEF_NUM_CELLS.x, MaxGridSize.x / (float)KDefine.E_DEF_NUM_CELLS.x, KCDefine.B_VAL_0_REAL);
                    CellCenterOffset = new Vector3(CellSize.x / KCDefine.B_VAL_2_REAL, CellSize.y / -KCDefine.B_VAL_2_REAL, KCDefine.B_VAL_0_REAL);
                    GridOffset = new Vector3(-preview.sizeDelta.x * 0.5f, (preview.sizeDelta.y * 0.5f) + (Mathf.Max(0, CGameInfoStorage.Inst.PlayLevelInfo.m_oCellInfoDictContainer.Count - KDefine.E_DEF_NUM_CELLS.y) * CellSize.y) - CellSize.y, 0);
                    
                    for(int i = Mathf.Max(0, CGameInfoStorage.Inst.PlayLevelInfo.m_oCellInfoDictContainer.Count - KDefine.E_DEF_NUM_CELLS.y); i < CGameInfoStorage.Inst.PlayLevelInfo.m_oCellInfoDictContainer.Count; ++i) {
                        for(int j = 0; j < CGameInfoStorage.Inst.PlayLevelInfo.m_oCellInfoDictContainer[i].Count; ++j) {
                            this.SetupPreviewCell(CGameInfoStorage.Inst.PlayLevelInfo.m_oCellInfoDictContainer[i][j], preview);
                        }
                    }
                }
                else
                {
                    CELL_SPRITE_ADJUSTMENT = new Vector3(-1f, -1f, 0);
                    MaxGridSize = new Vector3(preview.sizeDelta.x, preview.sizeDelta.y, 0);
                    CellSize = new Vector3(MaxGridSize.x / (float)KDefine.E_DEF_NUM_CELLS.x, MaxGridSize.x / (float)KDefine.E_DEF_NUM_CELLS.x, KCDefine.B_VAL_0_REAL);
                    CellSize *= SelGridInfo.m_stScale.x;
                    CellCenterOffset = new Vector3(CellSize.x / KCDefine.B_VAL_2_REAL, CellSize.y / -KCDefine.B_VAL_2_REAL, KCDefine.B_VAL_0_REAL);
                    GridOffset = new Vector3(-preview.sizeDelta.x * 0.5f, 0, 0);
                    
                    for(int i = 0; i < CGameInfoStorage.Inst.PlayLevelInfo.m_oCellInfoDictContainer.Count; ++i) {
                        for(int j = 0; j < CGameInfoStorage.Inst.PlayLevelInfo.m_oCellInfoDictContainer[i].Count; ++j) {
                            
                            if (CGameInfoStorage.Inst.PlayLevelInfo.m_oCellInfoDictContainer[i][j].m_oCellObjInfoList.Count > 0)
                            {
                                if (firstRow == -1) 
                                    firstRow = i;                        
                                lastRow = i;
                            }

                            this.SetupPreviewCell(CGameInfoStorage.Inst.PlayLevelInfo.m_oCellInfoDictContainer[i][j], preview);
                        }
                    }

                    activeLine = lastRow - firstRow + 1;
                    preview.localPosition = new Vector3(0, ((activeLine * 0.5f) + firstRow) * CellSize.y, 0);
                }
            }
        }

        /** 셀을 설정한다 */
        private void SetupPreviewCell(STCellInfo a_stCellInfo, RectTransform preview) {
            
            for(int i = 0; i < a_stCellInfo.m_oCellObjInfoList.Count; ++i) {
                EObjKinds kinds = a_stCellInfo.m_oCellObjInfoList[i].ObjKinds;
                //EObjKinds kindsType = (EObjKinds)((int)kinds).ExKindsToCorrectKinds(EKindsGroupType.SUB_KINDS_TYPE);
                STObjInfo stObjInfo = CObjInfoTable.Inst.GetObjInfo(kinds);
                STCellObjInfo stCellObjInfo = (STCellObjInfo)a_stCellInfo.m_oCellObjInfoList[i].Clone();

                var oCellObj = SpawnPreviewObj(stObjInfo, preview);
                
                oCellObj.transform.localPosition = GridOffset + a_stCellInfo.m_stIdx.ExToPos(CellCenterOffset, CellSize);
                oCellObj.GetController<CECellObjController>().ResetObjInfo(stObjInfo, stCellObjInfo);
                
                oCellObj.SetCellObjInfo(stCellObjInfo);
                //oCellObj.AddCellEffect(kindsType);
                oCellObj.SetCellActive(true);

                oCellObj.TargetSprite.size = CellSize + CELL_SPRITE_ADJUSTMENT;            
                oCellObj.TargetSprite.ExSetSortingOrder(sortingOrderInfoCell);
                oCellObj.TargetSprite.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

                oCellObj.UpperSprite.size = CellSize + CELL_SPRITE_ADJUSTMENT;
                oCellObj.UpperSprite.ExSetSortingOrder(sortingOrderInfoSub);
                oCellObj.UpperSprite.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;

                oCellObj.HPText.text = string.Empty;
            }
        }

        private CEObj SpawnPreviewObj(STObjInfo a_stObjInfo, RectTransform preview) {		
            GameObject oObj = (GameObject)Instantiate(Resources.Load(KDefine.E_OBJ_P_CELL_OBJ), preview);
            oObj.transform.localScale = Vector3.one;
            oObj.transform.localEulerAngles = Vector3.zero;
            oObj.transform.localPosition = Vector3.zero;

            var ceObj = oObj.GetComponent<CEObj>();
            var oController = oObj.ExAddComponent<CECellObjController>();

            ceObj.Init(CEObj.MakeParams(this, a_stObjInfo, null, oController));        
            oController?.Init(CECellObjController.MakeParams(this), a_stObjInfo.m_eObjKinds);

            ceObj.SetOwner(null);
            oController?.SetOwner(ceObj);

            return ceObj;
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
    public partial class CEngine : CComponent
    {
        [Header("★ [Parameter] Options")]
        public bool isGoldAim;

        [Header("★ [Parameter] Live")]
        public Transform lastClearTarget;
        public bool isTutorial;
        public bool isLevelFail;
        public bool isGridMoving;
        public int currentLevel;
        public int currentShootCount = 0;
        public int currentAimLayer;
        public Vector3 startPosition = Vector3.zero;
        public Vector3 shootDirection = Vector3.zero;

        [Header("★ [Parameter] Live Resolution")]
        // 디바이스 해상도.
        public float screenMultiplier;
        public float f_width;
        public float f_height;
        public float overAreaHeight;
        public float currentRatio;
        // 720p 환산.            
        public float uiAreaTop;
        public float uiAreaBottom;
        public float reWidth;
        public float reHeight;
        public float gridWidth;
        public float gridHeight;
        public float cellsizeY;

        /// <Summary>(반사 O) 벽.</Summary>
        [HideInInspector] public int layerWall;
        /// <Summary>(반사 O) 블럭.</Summary>
        [HideInInspector] public int layerBricks;
        /// <Summary>(반사 O) 벽 or 블럭.</Summary>
        [HideInInspector] public int layerReflect;
        /// <Summary>(반사 X) 아이템 블럭 or 스페셜 블럭.</Summary>
        [HideInInspector] public int layerThrough;
        /// <Summary>(반사 O/X) 벽 or 블럭.</Summary>
        [HideInInspector] public int layerAll;
        /// <Summary>(반사 X) 볼.</Summary>
        [HideInInspector] public int layerBall;

        [Header("★ [Parameter] Privates")]
        private WaitForSeconds dropBallsDelay = new WaitForSeconds(KCDefine.B_VAL_0_5_REAL);
        private WaitForSeconds cellRootMoveDelay = new WaitForSeconds(KCDefine.B_VAL_0_0_1_REAL);
        public WaitForSeconds hitEffectDelay = new WaitForSeconds(KCDefine.B_VAL_0_0_2_REAL);
        public WaitForSeconds fxMissileDelay = new WaitForSeconds(GlobalDefine.FXMissile_Time);
        public WaitForSeconds cellAppearDelay = new WaitForSeconds(GlobalDefine.FXCellAppear_Time);

        public GameScene.CSubGameSceneManager subGameSceneManager => CSceneManager.GetSceneManager<GameScene.CSubGameSceneManager>(KCDefine.B_SCENE_N_GAME);

#region Initialize

        private void InitResoulution()
        {
            // 디바이스 해상도.
            screenMultiplier = (float)Screen.height / KCDefine.B_PORTRAIT_SCREEN_HEIGHT;
            f_height = (float)Screen.height;
            f_width = (f_height / (float)Screen.width) < ((float)KCDefine.B_PORTRAIT_SCREEN_HEIGHT / (float)KCDefine.B_PORTRAIT_SCREEN_WIDTH) ? (float)KCDefine.B_PORTRAIT_SCREEN_WIDTH * screenMultiplier : (float)Screen.width;
            overAreaHeight = f_height - (KCDefine.B_PORTRAIT_SCREEN_HEIGHT * screenMultiplier);
            currentRatio = f_height / f_width; 

            // 720p 환산.            
            uiAreaTop = GlobalDefine.GRID_PANEL_HEIGHT_TOP + ((overAreaHeight * 0.5f) / screenMultiplier);
            uiAreaBottom = GlobalDefine.GRID_PANEL_HEIGHT_BOTTOM + ((overAreaHeight * 0.5f) / screenMultiplier);
            reWidth = f_width / screenMultiplier;
            reHeight = f_height / screenMultiplier;
            gridWidth =  Access.CellSize.x * CGameInfoStorage.Inst.PlayLevelInfo.NumCells.x * SelGridInfo.m_stScale.x;
            gridHeight = Access.CellSize.y * CGameInfoStorage.Inst.PlayLevelInfo.NumCells.y * SelGridInfo.m_stScale.y;

            cellsizeY = Access.CellSize.y * SelGridInfo.m_stScale.y;
        }

        private void InitCellRoot()
        {
            isGridMoving = false;
            this.Params.m_oCellRoot.transform.localPosition = new Vector3(0, (((reHeight - Mathf.Min(gridWidth, gridHeight)) * 0.5f) - cellsizeY - uiAreaTop), 0);
            
            // CellRoot 수동 보정.
            if (CGameInfoStorage.Inst.PlayLevelInfo.NumCells.x != KDefine.E_DEF_NUM_CELLS.x 
             && CGameInfoStorage.Inst.PlayLevelInfo.NumCells.x > 0 && CGameInfoStorage.Inst.PlayLevelInfo.NumCells.x <= KDefine.E_MAX_NUM_CELLS.x)
                this.Params.m_oCellRoot.transform.localPosition = new Vector3(0, this.Params.m_oCellRoot.transform.localPosition.y + GlobalDefine.GRID_Y_OFFSET[CGameInfoStorage.Inst.PlayLevelInfo.NumCells.x - 1], 0);
        }

        private void InitLayerMask()
        {
            layerWall = 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_WALL);
            layerBricks = 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_BRICK) | 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_OBSTACLE_REFLECT);
            layerReflect = 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_WALL) | 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_BRICK) | 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_OBSTACLE_REFLECT) | 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_SPECIAL_REFLECT);
            layerThrough = 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_ITEM) | 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_OBSTACLE_THROUGH)| 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_SPECIAL_THROUGH);
            layerAll = 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_WALL) 
                     | 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_BRICK) | 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_ITEM)
                     | 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_OBSTACLE_REFLECT) | 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_SPECIAL_REFLECT)
                     | 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_OBSTACLE_THROUGH)| 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_SPECIAL_THROUGH);
            layerBall = 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_BALL);

            isGoldAim = false;
            SetAimLayer(isGoldAim);
        }

        private void InitCellLayer(CEObj oCellObj)
        {
            switch((EObjType)((int)oCellObj.Params.m_stObjInfo.m_eObjKinds).ExKindsToType()) 
            {
                case EObjType.NORM_BRICKS:
                    ChangeLayer(oCellObj.transform, LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_BRICK));
                    break;
                case EObjType.ITEM_BRICKS:
                    ChangeLayer(oCellObj.transform, LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_ITEM));
                    break;
                case EObjType.OBSTACLE_BRICKS:
                    if (oCellObj.Params.m_stObjInfo.m_bIsEnableReflect)
                        ChangeLayer(oCellObj.transform, LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_OBSTACLE_REFLECT));
                    else
                        ChangeLayer(oCellObj.transform, LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_OBSTACLE_THROUGH));
                    break;
                case EObjType.SPECIAL_BRICKS:
                    if (oCellObj.Params.m_stObjInfo.m_bIsEnableReflect)
                        ChangeLayer(oCellObj.transform, LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_SPECIAL_REFLECT));
                    else
                        ChangeLayer(oCellObj.transform, LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_SPECIAL_THROUGH));
                    break;
                default:
                    Debug.Log(CodeManager.GetMethodName() + string.Format("<color=red>{0}</color>", (EObjType)((int)oCellObj.Params.m_stObjInfo.m_eObjKinds).ExKindsToType()));
                    ChangeLayer(oCellObj.transform, LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_EMPTY));
                    break;
            }
        }

#endregion Initialize

    }
}
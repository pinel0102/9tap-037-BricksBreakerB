using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
    public partial class CEngine : CComponent
    {
        [Header("★ [Parameter] Live")]
        public int currentLevel;
        public EPlayState PlayState;
        public Transform lastClearTarget;
        public bool isShooting;
        public bool isGoldenAimOneTime;
        public bool isGoldenAim;
        public bool isTutorial;
        public bool isLevelClear;
        public bool isLevelFail;
        public bool isGridMoving;
        public bool isWarning;
        public bool isAddSteelBricks;
        public bool isExplosionAll;

        [Header("★ [Parameter] Balls")]
        public Vector3 startPosition = Vector3.zero;
        public Vector3 shootDirection = Vector3.zero;
        public int currentShootCount;
		public List<CEObj> BallObjList = new List<CEObj>();
        public List<CEObj> ExtraBallObjList = new List<CEObj>();
        public List<CEObj> DeleteBallList = new List<CEObj>();

        [Header("★ [Parameter] Resolution")]
        // 디바이스 해상도.
        private float screenMultiplier;
        private float f_width;
        private float f_height;
        private float overAreaHeight;
        private float currentRatio;
        // 720p 환산.            
        private float uiAreaTop;
        private float uiAreaBottom;
        private float reWidth;
        private float reHeight;
        private float gridWidth;
        private float gridHeight;

        [Header("★ [Parameter] Layer Mask")]
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
        private int currentAimLayer;

        [Header("★ [Parameter] Privates")]
        private WaitForSeconds shootDelay = new WaitForSeconds(GlobalDefine.SHOOT_BALL_DELAY);
        private WaitForSeconds clearDelay = new WaitForSeconds(KCDefine.B_VAL_0_5_REAL);
        [HideInInspector] public WaitForSeconds hitEffectDelay = new WaitForSeconds(KCDefine.B_VAL_0_0_2_REAL);
        
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

            isGoldenAim = GlobalDefine.hasGoldenAim;
            SetAimLayer(isGoldenAim);
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
                case EObjType.BG:
                    ChangeLayer(oCellObj.transform, LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_EMPTY));
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
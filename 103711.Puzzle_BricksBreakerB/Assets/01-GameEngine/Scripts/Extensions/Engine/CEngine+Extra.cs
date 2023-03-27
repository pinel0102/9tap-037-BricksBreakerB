using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace NSEngine {
    public partial class CEngine : CComponent
    {
        [Header("★ [Live] Parameter")]
        public Transform lastCell;

        [Header("★ [Parameter] Options")]
        public bool isGoldAim;

        [Header("★ [Parameter] Live")]
        public bool isLevelFail;
        public bool isGridMoving;
        public int currentLevel;
        public int currentShootCount = 0;
        public int currentAimLayer;
        public Vector3 startPosition = Vector3.zero;
        public Vector3 shootDirection = Vector3.zero;
        public Vector3 cellRootMoveVector = Vector3.zero;

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
        }

        private void InitCellRoot()
        {
            isGridMoving = false;
            cellRootMoveVector = new Vector3(0, -(Access.CellSize.y * SelGridInfo.m_stScale.y), 0);

            this.Params.m_oCellRoot.transform.localPosition = new Vector3(0, (((reHeight - Mathf.Min(gridWidth, gridHeight)) * 0.5f) - uiAreaTop), 0);
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


#region Public Methods

        public void ToggleAimLayer()
        {
            isGoldAim = !isGoldAim;
            SetAimLayer(isGoldAim);
        }

        public void SetAimLayer(bool _reflectBricks)
        {
            currentAimLayer = _reflectBricks ? layerReflect : layerWall;
        }

        public void AddBall(int _index)
        {
            var oBallObj = this.CreateBallObj(_index, CObjInfoTable.Inst.GetObjInfo(EObjKinds.BALL_NORM_01), null);
            oBallObj.NumText.text = string.Empty;
            oBallObj.transform.localPosition = startPosition;
            
            this.BallObjList.ExAddVal(oBallObj);
        }

        public void AddShootBalls(int _startIndex, int _count)
        {
            StartCoroutine(CO_WaitShootDelay(_startIndex, _count));
        }

        public void RefreshBallText()
        {
            for (int i=1; i < this.BallObjList.Count; i++)
            {
                this.BallObjList[i].NumText.text = string.Empty;
            }

            this.BallObjList[0].NumText.text = this.BallObjList.Count.ToString();
        }

        public void CheckClear(bool _waitDelay = false)
        {
            RefreshBallText();

            // 클리어했을 경우
            if(this.IsClear()) 
            {
                subGameSceneManager.SetEnableUpdateUIsState(true);

                if (_waitDelay)
                    StartCoroutine(CO_Clear());
                else
                    LevelClear();                
            } 
            else if (!isLevelFail && !isGridMoving)
            {
                this.TurnEndAction();

                this.ExLateCallFunc((a_oFuncSender) => {
                    MoveDownAllCells();
                    }, KCDefine.B_VAL_0_3_REAL);
            }
        }

        public List<CEObj> GetRandomCells_SkillTarget(int count, List<CEObj> excludeList)
        {
            List<CEObj> cellList = new List<CEObj>();

            for (int row = 0; row < this.CellObjLists.GetLength(KCDefine.B_VAL_0_INT); row++)
            {
                for (int col = 0; col < this.CellObjLists.GetLength(KCDefine.B_VAL_1_INT); col++)
                {
                    int _count = this.CellObjLists[row, col].Count;
                    if (_count > 0)
                    {
                        int _cLastLayer = _count - 1;
                        if(this.CellObjLists[row, col][_cLastLayer].gameObject.activeSelf) 
                        {
                            CEObj target = this.CellObjLists[row, col][_cLastLayer];
                            if (target != null)
                            {
                                if (target.Params.m_stObjInfo.m_bIsSkillTarget)
                                {
                                    cellList.Add(target);
                                }
                            }
                        }
                    }
                }
            }

            return cellList.OrderBy(g => System.Guid.NewGuid())
                            .Where(i => !excludeList.Contains(i))
                            .Take(count).ToList();
        }

        public List<CEObj> GetAllCells_SkillTarget(List<CEObj> excludeList)
        {
            List<CEObj> cellList = new List<CEObj>();

            for (int row = 0; row < this.CellObjLists.GetLength(KCDefine.B_VAL_0_INT); row++)
            {
                for (int col = 0; col < this.CellObjLists.GetLength(KCDefine.B_VAL_1_INT); col++)
                {
                    int _count = this.CellObjLists[row, col].Count;
                    if (_count > 0)
                    {
                        int _cLastLayer = _count - 1;
                        if(this.CellObjLists[row, col][_cLastLayer].gameObject.activeSelf) 
                        {
                            CEObj target = this.CellObjLists[row, col][_cLastLayer];
                            if (target != null)
                            {
                                if (target.Params.m_stObjInfo.m_bIsSkillTarget)
                                {
                                    cellList.Add(target);
                                }
                            }
                        }
                    }
                }
            }

            return cellList.OrderBy(g => System.Guid.NewGuid())
                            .Where(i => !excludeList.Contains(i)).ToList();
        }

        public CEObj GetRandomCell(List<CEObj> cellList)
        {
            return GetRandomCells(cellList, 1)[0];
        }

        public List<CEObj> GetRandomCells(List<CEObj> cellList, int count)
        {
            return cellList.OrderBy(g => System.Guid.NewGuid())
                            .Take(count).ToList();
        }

        public List<CEObj> GetAllCells(EObjKinds kindsToGet)
        {
            List<CEObj> cellList = new List<CEObj>();

            for (int row = 0; row < this.CellObjLists.GetLength(KCDefine.B_VAL_0_INT); row++)
            {
                for (int col = 0; col < this.CellObjLists.GetLength(KCDefine.B_VAL_1_INT); col++)
                {
                    int _count = this.CellObjLists[row, col].Count;
                    if (_count > 0)
                    {
                        int _cLastLayer = _count - 1;
                        if(this.CellObjLists[row, col][_cLastLayer].gameObject.activeSelf) 
                        {
                            CEObj target = this.CellObjLists[row, col][_cLastLayer];
                            if (target != null)
                            {
                                if (target.Params.m_stObjInfo.m_eObjKinds == kindsToGet)
                                {
                                    cellList.Add(target);
                                }
                            }
                        }
                    }
                }
            }

            return cellList;
        }

#endregion Public Methods


#region Private Methods
        private void CheckRemoveBalls()
        {
            for(int i=this.BallObjList.Count - 1; i >= 0; i--)
            {
                if (this.BallObjList[i].GetController<CEBallObjController>().isRemoveMoveEnd)
                {
                    GameObject.Destroy(this.BallObjList[i].gameObject);
                    this.BallObjList.Remove(this.BallObjList[i]);
                }
            }

            //Debug.Log("this.BallObjList.Count : " + this.BallObjList.Count);
        }

        private void CreateBall(int _index)
        {
            var oBallObj = this.CreateBallObj(_index, CObjInfoTable.Inst.GetObjInfo(EObjKinds.BALL_NORM_01), null);
            oBallObj.NumText.text = string.Empty;
            //oBallObj.transform.localPosition = this.SelGridInfo.m_stPivotPos + new Vector3(this.SelGridInfo.m_stBounds.size.x / KCDefine.B_VAL_2_REAL, -this.SelGridInfo.m_stBounds.size.y, KCDefine.B_VAL_0_INT) - new Vector3(0, this.SelGridInfo.aimAdjustHeight * 0.5f, 0);
            oBallObj.transform.localPosition = new Vector3(0, (-(reHeight * 0.5f) + uiAreaBottom) / SelGridInfo.m_stScale.y, 0);
            oBallObj.transform.localPosition += new Vector3(KCDefine.B_VAL_0_REAL, oBallObj.TargetSprite.sprite.textureRect.height / KCDefine.B_VAL_2_REAL, KCDefine.B_VAL_0_INT);

            this.BallObjList.ExAddVal(oBallObj);
        }

        private void ShootBalls(int _startIndex, int _count)
        {
            CScheduleManager.Inst.AddTimer(this, GlobalDefine.SHOOT_BALL_DELAY, (uint)_count, () => {
                //Debug.Log(CodeManager.GetMethodName() + string.Format("BallObjList[{0}]", _startIndex));
                this.BallObjList[_startIndex++].GetController<CEBallObjController>().Shoot(shootDirection);
                currentShootCount++;
            });
        }

        private void ChangeLayer(Transform trans, int newLayer)
        {
            ChangeLayersRecursively(trans, newLayer);
        }
        
        private void ChangeLayersRecursively(Transform trans, int newLayer)
        {
            trans.gameObject.ExSetLayer(newLayer);
            foreach(Transform child in trans)
            {
                ChangeLayersRecursively(child, newLayer);
            }
        }

        private void CheckDeadLine()
        {
            if (lastCell != null)
            {
                Vector2 distanceVector = subGameSceneManager.mainCanvas.WorldToCanvas(lastCell.position - subGameSceneManager.deadLine.position);

                float cellsizeY = Access.CellSize.y * SelGridInfo.m_stScale.y;
                float distance = distanceVector.y - (cellsizeY * 0.5f);

                //Debug.Log(CodeManager.GetMethodName() + string.Format("distance : {0} / {1}", distance, cellsizeY));

                if (distance >= (cellsizeY * 2f))
                {
                    subGameSceneManager.warningObject.SetActive(false);
                }            
                else if ((distance >= (cellsizeY * 1f)) && (distance < (cellsizeY * 2f)))
                {
                    subGameSceneManager.warningObject.SetActive(true);
                }
                else
                {
                    LevelFail();
                }
            }
        }

#endregion Private Methods


#region Coroutines

        private IEnumerator CO_WaitShootDelay(int _startIndex, int _count)
        {
            while(currentShootCount < _startIndex)
            {
                yield return null;
            }

            ShootBalls(_startIndex, _count);
        }

        private IEnumerator CO_Clear()
        {
            yield return dropBallsDelay;

            LevelClear();
        }

#endregion Coroutines

    }
}
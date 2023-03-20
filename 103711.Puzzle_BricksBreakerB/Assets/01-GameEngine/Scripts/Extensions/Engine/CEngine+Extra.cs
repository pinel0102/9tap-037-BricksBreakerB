using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
    public partial class CEngine : CComponent
    {
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
        [HideInInspector] public int layerWallAndBricks;
        /// <Summary>(반사 X) 아이템 블럭 or 스페셜 블럭.</Summary>
        [HideInInspector] public int layerThrough;
        /// <Summary>(반사 X) 볼.</Summary>
        [HideInInspector] public int layerBall;

        [Header("★ [Parameter] Privates")]
        private WaitForSeconds dropBallsDelay = new WaitForSeconds(KCDefine.B_VAL_0_5_REAL);
        private WaitForSeconds cellRootMoveDelay = new WaitForSeconds(KCDefine.B_VAL_0_0_1_REAL);
        [HideInInspector] public WaitForSeconds hitEffectDelay = new WaitForSeconds(KCDefine.B_VAL_0_0_2_REAL);

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
            layerBricks = 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_BRICK) | 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_OBSTACLE);
            layerWallAndBricks = 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_BRICK) | 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_OBSTACLE) | 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_WALL);
            layerThrough = 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_ITEM) | 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_SPECIAL);
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
                case EObjType.OBSTACLE_BRICKS:
                    ChangeLayer(oCellObj.transform, LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_OBSTACLE));
                    break;                
                case EObjType.ITEM_BRICKS:
                    ChangeLayer(oCellObj.transform, LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_ITEM));
                    break;
                case EObjType.SPECIAL_BRICKS:
                    ChangeLayer(oCellObj.transform, LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_SPECIAL));
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
            currentAimLayer = _reflectBricks ? layerWallAndBricks : layerWall;
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

        public void CheckClear(bool _waitDelay = false)
        {
            // 클리어했을 경우
            if(this.IsClear()) 
            {
                CSceneManager.GetSceneManager<GameScene.CSubGameSceneManager>(KCDefine.B_SCENE_N_GAME).SetEnableUpdateUIsState(true);

                if (_waitDelay)
                    StartCoroutine(CO_Clear());
                else
                    LevelClear();                
            } 
            else if (!isLevelFail && !isGridMoving)
            {
                this.TurnEndAction();

                this.ExLateCallFunc((a_oFuncSender) => {
                    StartCoroutine(CO_MoveCellRoot(KCDefine.B_VAL_1_INT));
                    }, KCDefine.B_VAL_0_3_REAL);
            }
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

        ///<Summary>턴 종료시 셀이 내려오기 전에 발동.</Summary>
        private void TurnEndAction()
        {
            for(int i = 0; i < this.CellObjLists.GetLength(KCDefine.B_VAL_0_INT); ++i) {
				for(int j = 0; j < this.CellObjLists.GetLength(KCDefine.B_VAL_1_INT); ++j) {
					for(int k = 0; k < this.CellObjLists[i, j].Count; ++k) {
						// 셀이 존재 할 경우
						if(this.CellObjLists[i, j][k].gameObject.activeSelf) {
                            CEObj target = this.CellObjLists[i, j][k];
                            if (target != null)
                            {
                                EObjKinds kindsType = (EObjKinds)((int)target.CellObjInfo.ObjKinds).ExKindsToCorrectKinds(EKindsGroupType.SUB_KINDS_TYPE);
                                
                                switch(kindsType)
                                {
                                    case EObjKinds.SPECIAL_BRICKS_LASER_HORIZONTAL_01:
                                    case EObjKinds.SPECIAL_BRICKS_LASER_VERTICAL_01:
                                    case EObjKinds.SPECIAL_BRICKS_LASER_CROSS_01:
                                        if(target.TryGetComponent<CECellObjController>(out CECellObjController oController))
                                        {
                                            oController.HideReservedCell();
                                        }
                                        break;
                                }
                            }
						}
					}
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

        private IEnumerator CO_MoveCellRoot(int _moveCount = KCDefine.B_VAL_1_INT)
        {
            isGridMoving = true;

            CSceneManager.GetSceneManager<GameScene.CSubGameSceneManager>(KCDefine.B_SCENE_N_GAME).HideShootUIs();

            Vector3 endPosition = this.Params.m_oCellRoot.transform.localPosition + (cellRootMoveVector * _moveCount);

            while(this.Params.m_oCellRoot.transform.localPosition.y - GlobalDefine.CELL_ROOT_MOVE_SPEED.y > endPosition.y)
            {
                yield return cellRootMoveDelay;

                if (isLevelFail)
                    yield break;

                this.Params.m_oCellRoot.transform.localPosition -= GlobalDefine.CELL_ROOT_MOVE_SPEED;
            }

            this.Params.m_oCellRoot.transform.localPosition = endPosition;

            this.PlayState = EPlayState.IDLE;
            CSceneManager.GetSceneManager<GameScene.CSubGameSceneManager>(KCDefine.B_SCENE_N_GAME).SetEnableUpdateUIsState(true);
            
            isGridMoving = false;
        }

#endregion Coroutines


#region Deprecated

        private void SetBallColliders(bool _isEnable)
        {
            for(int i = 0; i < this.BallObjList.Count; ++i) {
				this.BallObjList[i].GetComponent<CEBallObjController>().SetBallCollider(_isEnable);
			}
        }

        private void SetCellColliders(bool _isEnable)
        {
            for(int i = 0; i < this.CellObjLists.GetLength(KCDefine.B_VAL_0_INT); ++i) {
				for(int j = 0; j < this.CellObjLists.GetLength(KCDefine.B_VAL_1_INT); ++j) {
					for(int k = 0; k < this.CellObjLists[i, j].Count; ++k) {
						// 셀이 존재 할 경우
						if(this.CellObjLists[i, j][k].gameObject.activeSelf) {
                            CEObj target = this.CellObjLists[i, j][k];
							
                            if(target.TargetSprite != null && target.TargetSprite.TryGetComponent<PolygonCollider2D>(out PolygonCollider2D oCollider))
                            {
                                oCollider.enabled = _isEnable;
                            }
						}
					}
				}
			}
        }

#endregion Deprecated

    }
}

public enum FXType
{
    NONE,
    LASER,
    FIREBALL,
}
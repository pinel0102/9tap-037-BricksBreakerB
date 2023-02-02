using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
    public partial class CEngine : CComponent
    {
        [Header("★ [Parameter] Live")]
        public int currentLevel;
        public int currentShootCount = 0;
        public int currentAimLayer;
        public Vector3 startPosition = Vector3.zero;
        public Vector3 shootDirection = Vector3.zero;

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

#region Extra Methods

        private void InitLayerMask()
        {
            layerWall = 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_WALL);
            layerBricks = 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_BRICK) | 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_OBSTACLE);
            layerWallAndBricks = 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_BRICK) | 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_OBSTACLE) | 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_WALL);
            layerThrough = 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_ITEM) | 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_CELL_SPECIAL);
            layerBall = 1 << LayerMask.NameToLayer(GlobalDefine.LAYER_BALL);

            SetAimLayer(false);
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

        public void SetAimLayer(bool _reflectBricks)
        {
            currentAimLayer = _reflectBricks ? layerWallAndBricks : layerWall;
        }

        private void CreateBall(int _index)
        {
            var oBallObj = this.CreateBallObj(_index, CObjInfoTable.Inst.GetObjInfo(EObjKinds.BALL_NORM_01), null);
            oBallObj.NumText.text = string.Empty;
            oBallObj.transform.localPosition = this.SelGridInfo.m_stPivotPos + new Vector3(this.SelGridInfo.m_stBounds.size.x / KCDefine.B_VAL_2_REAL, -this.SelGridInfo.m_stBounds.size.y, KCDefine.B_VAL_0_INT);
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

        private IEnumerator CO_WaitShootDelay(int _startIndex, int _count)
        {
            while(currentShootCount < _startIndex)
            {
                yield return null;
            }

            ShootBalls(_startIndex, _count);
        }

        public void CheckClear(bool _waitDelay = false)
        {
            // 클리어했을 경우
            if(this.IsClear()) {
                CSceneManager.GetSceneManager<GameScene.CSubGameSceneManager>(KCDefine.B_SCENE_N_GAME).SetEnableUpdateUIsState(true);

                if (_waitDelay)
                    StartCoroutine(CO_Clear());
                else
                    LevelClear();                
            } else {
                this.ExLateCallFunc((a_oFuncSender) => {
                    this.PlayState = EPlayState.IDLE;
                    CSceneManager.GetSceneManager<GameScene.CSubGameSceneManager>(KCDefine.B_SCENE_N_GAME).SetEnableUpdateUIsState(true);
                    }, KCDefine.B_VAL_0_3_REAL);
            }
        }

        private IEnumerator CO_Clear()
        {
            yield return dropBallsDelay;

            LevelClear();
        }

#endregion Extra Methods


#region Private Methods

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

#endregion Private Methods

    }
}
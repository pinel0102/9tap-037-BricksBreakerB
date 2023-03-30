using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;

namespace NSEngine {
    public partial class CEngine : CComponent
    {
        private void MoveDownAllCells()
        {
            MoveDownAllCellObjs();
        }

        /** 모든 셀 객체를 아래로 이동시킨다 */
		private void MoveDownAllCellObjs() 
        {
            isGridMoving = true;
            subGameSceneManager.HideShootUIs();

            var oAniList = CCollectionManager.Inst.SpawnList<Tween>();

            try {
                for(int i = this.CellObjLists.GetLength(KCDefine.B_VAL_0_INT) - KCDefine.B_VAL_1_INT; i >= KCDefine.B_VAL_0_INT; --i) {
                    for(int j = 0; j < this.CellObjLists.GetLength(KCDefine.B_VAL_1_INT); ++j) {
                        this.TryMoveDownCellObjs(new Vector3Int(j, i, this.SelGridInfoIdx), oAniList);
                    }
                }

                var oAni = CFactory.MakeSequence(oAniList, this.OnCompleteMoveDownAni, a_bIsJoin: true);
                m_oAniList.ExAddVal(oAni);
            } finally {
                CCollectionManager.Inst.DespawnList(oAniList);
            }
		}

        private bool TryMoveDownCellObjs(Vector3Int a_stIdx, List<Tween> a_oOutAniList) 
        {
			var oCellObjList = this.CellObjLists[a_stIdx.y, a_stIdx.x];
            
            if(oCellObjList.ExIsValid() && this.IsEnableMoveDown(oCellObjList)) 
            {
                var newIndex = new Vector3Int(a_stIdx.x, a_stIdx.y + KCDefine.B_VAL_1_INT, a_stIdx.z);

				for(int i = 0; i < oCellObjList.Count; ++i) 
                {
					oCellObjList[i].GetController<CECellObjController>().SetIdx(newIndex);
                    oCellObjList[i].SetCellIdx(newIndex, oCellObjList[i].kinds);
					a_oOutAniList.ExAddVal(oCellObjList[i].transform.DOLocalMoveY(oCellObjList[i].transform.localPosition.y - Access.CellSize.y, KDefine.E_DURATION_MOVE_DOWN_ANI));
				}

				oCellObjList.ExCopyTo(this.CellObjLists[a_stIdx.y + KCDefine.B_VAL_1_INT, a_stIdx.x], (a_oObj) => a_oObj);
				oCellObjList.Clear();

				return true;
			}

			return false;
		}

        private bool IsEnableMoveDown(CEObj a_oObj) {
            var controller = a_oObj.GetController<CECellObjController>();
            var stIdx = new Vector3Int(controller.Idx.x, controller.Idx.y + a_oObj.Params.m_stObjInfo.m_stSize.y, controller.Idx.z);
			var oCellObjList = this.CellObjLists.ExGetVal(stIdx, null);

			bool bIsValid = a_oObj.Params.m_stObjInfo.m_bIsEnableMoveDown;

            return bIsValid && !oCellObjList.ExIsValid() && this.CellObjLists.ExIsValidIdx(stIdx);
		}

		private bool IsEnableMoveDown(List<CEObj> a_oObjList) {
			for(int i = 0; i < a_oObjList.Count; ++i) {
				// 이동이 불가능 할 경우
				if(!this.IsEnableMoveDown(a_oObjList[i])) {
					return false;
				}
			}

			return true;
		}

        /** 이동 애니메이션이 완료 되었을 경우 */
		private void OnCompleteMoveDownAni(DG.Tweening.Sequence a_oSender) {
			a_oSender?.Kill();
			m_oAniList.ExRemoveVal(a_oSender);

			/*for(int i = 0; i < this.CellObjLists.GetLength(KCDefine.B_VAL_0_INT); ++i) {
				for(int j = 0; j < this.CellObjLists.GetLength(KCDefine.B_VAL_1_INT); ++j) {
					this.HandleMoveDownState(this.CellObjLists[i, j]);
				}
			}*/

            this.PlayState = EPlayState.IDLE;
            subGameSceneManager.SetEnableUpdateUIsState(true);
            isGridMoving = false;

            RefreshActiveCells();
            CheckDeadLine();
		}

        ///<Summary>턴 종료시 셀이 내려온 후에 발동.</Summary>
        private void HandleMoveDownState(List<CEObj> a_oObjList) {
			/*for(int i = 0; i < a_oObjList.Count; ++i) {
				a_oObjList[i].GetController<CECellObjController>()?.OnMoveDown();
			}*/
		}

        ///<Summary>턴 종료시 셀이 내려오기 전에 발동.</Summary>
        private void TurnEndAction()
        {
            lastClearTarget = null;
            bool isLastCellAssigned = false;

            for(int i = this.CellObjLists.GetLength(KCDefine.B_VAL_0_INT) - 1; i >= 0 ; i--) {
				for(int j = this.CellObjLists.GetLength(KCDefine.B_VAL_1_INT) - 1; j >= 0 ; j--) {
					for(int k = 0; k < this.CellObjLists[i, j].Count; ++k) {
						// 셀이 존재 할 경우
                        CEObj target = this.CellObjLists[i, j][k];
                        if (target != null && target.IsActiveCell())
                        {
                            var oController = target.GetComponent<CECellObjController>();

                            if (target.Params.m_stObjInfo.m_bIsOnce)
                                oController.HideReservedCell();

                            if (target.Params.m_stObjInfo.m_bIsEnableChange)
                                oController.ChangeCellToExtraKinds();

                            if (!isLastCellAssigned)
                            {
                                if (target.Params.m_stObjInfo.m_bIsClearTarget)
                                {
                                    lastClearTarget = target.transform;
                                    isLastCellAssigned = true;
                                }
                            }
                        }
					}
				}
			}
        }

        public void RefreshActiveCells()
        {
            float cellsizeY = Access.CellSize.y * SelGridInfo.m_stScale.y;

            for(int i = this.CellObjLists.GetLength(KCDefine.B_VAL_0_INT) - 1; i >= 0 ; i--) {
				for(int j = this.CellObjLists.GetLength(KCDefine.B_VAL_1_INT) - 1; j >= 0 ; j--) {
					for(int k = 0; k < this.CellObjLists[i, j].Count; ++k) {
						// 셀이 존재 할 경우
                        CEObj target = this.CellObjLists[i, j][k];
                        if (target != null)
                        {
                            bool activeThis = target.transform.position.y <= subGameSceneManager.startLine.position.y - cellsizeY &&
                                              target.transform.position.y >= subGameSceneManager.deadLine.position.y;

                            target.SetCellActive(activeThis, activeThis && !target.IsActiveCell());
                        }
					}
				}
			}

            lastClearTarget = GetLastClearTarget().transform;
        }

        public void CheckDeadLine(bool isInitialize = false)
        {
            if (lastClearTarget != null)
            {
                Vector2 distanceVector = subGameSceneManager.mainCanvas.WorldToCanvas(lastClearTarget.position - subGameSceneManager.deadLine.position);
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
                    if (isInitialize)
                        subGameSceneManager.warningObject.SetActive(true);
                    else
                        LevelFail();
                }
            }
            else
            {
                subGameSceneManager.warningObject.SetActive(false);
            }
        }
    }
}
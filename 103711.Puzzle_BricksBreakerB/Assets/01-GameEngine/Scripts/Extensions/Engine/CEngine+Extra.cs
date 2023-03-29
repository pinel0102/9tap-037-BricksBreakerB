using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
    public partial class CEngine : CComponent
    {
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
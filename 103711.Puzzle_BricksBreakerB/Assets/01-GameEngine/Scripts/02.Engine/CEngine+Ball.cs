using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Timers;

namespace NSEngine {
    public partial class CEngine : CComponent
    {
#region Public Methods

        ///<Summary>[BallObjList] 턴이 끝나도 유지되는 볼을 n개 추가.</Summary>
        public void AddNormalBalls(Vector3 _startPosition, int _addCount, bool _autoShoot = true)
        {
            int _ballIndex = this.BallObjList.Count;

            for (int i=0; i < _addCount; i++)
            {
                var oBallObj = CreateBall(_ballIndex + i, _startPosition, EObjKinds.BALL_NORM_02);
                oBallObj.TargetSprite.sortingOrder = 9;
                this.BallObjList.ExAddVal(oBallObj);
            }

            if (_autoShoot && PlayState == EPlayState.SHOOT)
                this.ShootBalls(_ballIndex, _addCount);
        }

        ///<Summary>[BallObjList] 1회성 볼을 n개 추가.</Summary>
        public void AddNormalBallsOnce(Vector3 _startPosition, int _addCount, bool _autoShoot = true)
        {
            int _ballIndex = this.BallObjList.Count;

            for (int i=0; i < _addCount; i++)
            {
                var oBallObj = CreateBall(_ballIndex + i, _startPosition, EObjKinds.BALL_NORM_01);
                oBallObj.TargetSprite.sortingOrder = 10;
                this.BallObjList.ExAddVal(oBallObj);
                DeleteBallList.Add(oBallObj);
            }

            if (_autoShoot && PlayState == EPlayState.SHOOT)
                this.ShootBalls(_ballIndex, _addCount);
        }

        ///<Summary>[ExtraBallObjList] 1회성 볼을 1개 추가.</Summary>
        public CEBallObjController AddExtraBall(Vector3 _startPosition, bool _autoShoot = true)
        {
            int _ballIndex = this.ExtraBallObjList.Count + 1000;

            var oBallObj = CreateBall(_ballIndex, _startPosition, EObjKinds.BALL_NORM_03);
            oBallObj.TargetSprite.sortingOrder = 8;
            this.ExtraBallObjList.ExAddVal(oBallObj);
            
            CEBallObjController ballController = oBallObj.GetComponent<CEBallObjController>();

            if (_autoShoot)
                ballController.Shoot(this.shootDirection);

            return ballController;
        }

        public void RefreshBallText()
        {
            for (int i=1; i < this.BallObjList.Count; i++)
            {
                this.BallObjList[i].NumText.text = string.Empty;
            }

            for (int i=0; i < this.ExtraBallObjList.Count; i++)
            {
                this.ExtraBallObjList[i].NumText.text = string.Empty;
            }

            this.BallObjList[0].NumText.text = GlobalDefine.GetBallText(BallObjList.Count - DeleteBallList.Count, DeleteBallList.Count);
        }

#endregion Public Methods


#region Private Methods

        private void CheckRemoveBalls()
        {
            for(int i=this.BallObjList.Count - 1; i >= 0; i--)
            {
                if (this.BallObjList[i].Params.m_stObjInfo.m_bIsOnce || DeleteBallList.Contains(this.BallObjList[i]))
                {
                    GameObject.Destroy(this.BallObjList[i].gameObject);
                    this.BallObjList.Remove(this.BallObjList[i]);
                }
            }

            for(int i=this.ExtraBallObjList.Count - 1; i >= 0; i--)
            {
                if (this.ExtraBallObjList[i].Params.m_stObjInfo.m_bIsOnce || DeleteBallList.Contains(this.ExtraBallObjList[i]))
                {
                    GameObject.Destroy(this.ExtraBallObjList[i].gameObject);
                    this.ExtraBallObjList.Remove(this.ExtraBallObjList[i]);
                }
            }

            DeleteBallList.Clear();

            //Debug.Log("this.BallObjList.Count : " + this.BallObjList.Count);
        }

        private void ChangeToNormalBalls()
        {
            for(int i=this.BallObjList.Count - 1; i >= 0; i--)
            {
                ChangeToNormalBall(this.BallObjList[i]);
            }
        }

        private void ChangeToNormalBall(CEObj oBallObj)
        {
            oBallObj.ChangeKinds(EObjKinds.BALL_NORM_01);
        }

        ///<Summary>[Init] 볼 생성.</Summary>
        private void CreateBall(int _index)
        {
            var oBallObj = this.CreateBallObj(_index, CObjInfoTable.Inst.GetObjInfo(EObjKinds.BALL_NORM_01), null);
            oBallObj.transform.localPosition = new Vector3(0, (-(reHeight * 0.5f) + uiAreaBottom) / SelGridInfo.m_stScale.y, 0);
            oBallObj.transform.localPosition += new Vector3(KCDefine.B_VAL_0_REAL, oBallObj.TargetSprite.sprite.textureRect.height / KCDefine.B_VAL_2_REAL, KCDefine.B_VAL_0_INT);
            oBallObj.NumText.text = string.Empty;

            this.BallObjList.ExAddVal(oBallObj);
        }

        ///<Summary>[InGame] 볼 생성.</Summary>
        private CEObj CreateBall(int _ballIndex, Vector3 _startPosition, EObjKinds kinds)
        {
            var oBallObj = this.CreateBallObj(_ballIndex, CObjInfoTable.Inst.GetObjInfo(kinds), null);
            oBallObj.transform.localPosition = _startPosition;
            oBallObj.NumText.text = string.Empty;

            return oBallObj;
        }

#endregion Private Methods


#region Shoot Balls

        private void ShootBalls(int _startIndex, int _count)
        {
            StartCoroutine(CO_ShootBalls(_startIndex, _count));
        }

        private IEnumerator CO_ShootBalls(int _startIndex, int _count)
        {
            //Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>_startIndex : [{0}] / _count : {1} / Ready</color>", _startIndex, _count));

            yield return new WaitWhile(() => isShooting);

            //Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>_startIndex : [{0}] / _count : {1} / Start</color>", _startIndex, _count));

            isShooting = true;

            for(int i=_startIndex; i < _startIndex + _count; i++)
            {
                //Debug.Log(CodeManager.GetMethodName() + string.Format("BallObjList[{0}]", i));
                this.BallObjList[i].GetController<CEBallObjController>().Shoot(shootDirection);
                currentShootCount++;

                yield return shootDelay;
            }

            isShooting = false;

            //Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>_startIndex : [{0}] / _count : {1} / End</color>", _startIndex, _count));
        }

#endregion Shoot Balls
    }
}
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

        public void CheckClear(bool _waitDelay = false, bool _isBottomItem = false)
        {
            if (isLevelClear) return;

            this.RefreshBallText();
            this.InitCombo();

            // 클리어했을 경우
            if(this.IsClear())
            {
                isLevelClear = true;
                subGameSceneManager.SetEnableUpdateUIsState(true);

                if (_waitDelay)
                    StartCoroutine(CO_Clear());
                else
                    LevelClear();                
            } 
            else if (!isLevelFail && !isGridMoving && !_isBottomItem)
            {
                this.TurnEndAction();

                isAddSteelBricks = false;
                isExplosionAll = false;

                isGridMoving = true;
                this.ExLateCallFunc((a_oFuncSender) => {
                    MoveDownAllCells();
                    }, KCDefine.B_VAL_0_3_REAL);
            }
        }

#endregion Public Methods


#region Private Methods

        private void TurnEnd(bool _waitDelay = false)
        {
            startPosition = SelBallObj.transform.localPosition;
            shootDirection = Vector3.zero;

            subGameSceneManager.InitAimLayer();
            CheckRemoveBalls();
            ChangeToNormalBalls();
            this.SetPlayState(EPlayState.IDLE);
            CheckClear(_waitDelay);
        }

        public void SetAimLayer(bool _reflectBricks)
        {
            currentAimLayer = _reflectBricks ? layerReflect : layerWall;
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

#endregion Private Methods


#region Coroutines

        private IEnumerator CO_Clear()
        {
            yield return clearDelay;

            LevelClear();
        }

#endregion Coroutines

    }
}
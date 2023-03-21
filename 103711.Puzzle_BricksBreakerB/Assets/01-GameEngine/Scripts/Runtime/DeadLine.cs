using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene {
    public class DeadLine : MonoBehaviour
    {
        public CSubGameSceneManager manager;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!manager.Engine.isLevelFail)
            {
                NSEngine.CECellObjController oController = other.GetComponentInParent<NSEngine.CECellObjController>();

                if(oController != null)
                {
                    STObjInfo objInfo = oController.GetOwner<NSEngine.CEObj>().Params.m_stObjInfo;

                    if (objInfo.m_bIsClearTarget)
                        manager.Engine.LevelFail();
                }
            }
        }
    }
}
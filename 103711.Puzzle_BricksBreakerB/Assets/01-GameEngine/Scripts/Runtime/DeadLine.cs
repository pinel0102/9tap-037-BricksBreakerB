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
                    EObjKinds kinds =  oController.GetOwner<NSEngine.CEObj>().Params.m_stObjInfo.m_eObjKinds;
                    EObjType cellType = (EObjType)((int)kinds).ExKindsToType();

                    //Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>{0}</color>", cellType));

                    switch(cellType)
                    {
                        case EObjType.NORM_BRICKS:
                        case EObjType.OBSTACLE_BRICKS:
                            manager.Engine.LevelFail();
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
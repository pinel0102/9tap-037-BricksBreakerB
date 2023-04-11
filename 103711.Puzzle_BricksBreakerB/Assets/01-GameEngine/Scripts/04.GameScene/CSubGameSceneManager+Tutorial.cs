using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GameScene {
    public partial class CSubGameSceneManager : CGameSceneManager
    {
        private void CheckTutorial(int level)
        {
            switch(level)
            {
                case GlobalDefine.TUTORIAL_LEVEL_BRICKS_DELETE:     ShowTutorial_Bricks_Delete(2);       break;
                case GlobalDefine.TUTORIAL_LEVEL_ADD_BALL:          ShowTutorial_Add_Ball(1);            break;
                case GlobalDefine.TUTORIAL_LEVEL_EARTHQUAKE:        ShowTutorial_Earthquake(0);          break;
                case GlobalDefine.TUTORIAL_LEVEL_ADD_LASER_BRICKS:  ShowTutorial_Add_Laser_Bricks(3);    break;
                case GlobalDefine.TUTORIAL_LEVEL_ADD_STEEL_BRICKS:  ShowTutorial_Add_Steel_Bricks(4);    break;
            }
        }

        private void ShowTutorial_Bricks_Delete(int index)
        {
            Debug.Log(CodeManager.GetMethodName());
            bottomItemsText[index].text = GlobalDefine.FORMAT_FREE;
        }

        private void ShowTutorial_Add_Ball(int index)
        {
            Debug.Log(CodeManager.GetMethodName());
            bottomItemsText[index].text = GlobalDefine.FORMAT_FREE;
        }

        private void ShowTutorial_Earthquake(int index)
        {
            Debug.Log(CodeManager.GetMethodName());
            bottomItemsText[index].text = GlobalDefine.FORMAT_FREE;
        }

        private void ShowTutorial_Add_Laser_Bricks(int index)
        {
            Debug.Log(CodeManager.GetMethodName());
            bottomItemsText[index].text = GlobalDefine.FORMAT_FREE;
        }

        private void ShowTutorial_Add_Steel_Bricks(int index)
        {
            Debug.Log(CodeManager.GetMethodName());
            bottomItemsText[index].text = GlobalDefine.FORMAT_FREE;
        }


        private void EndTutorial(int index)
        {
            bottomItemsText[index].text = string.Format(GlobalDefine.FORMAT_BOTTOM_ITEM, 0);
            Engine.isTutorial = false;
        }
    }
}
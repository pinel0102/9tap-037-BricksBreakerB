using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace GameScene {
    public partial class CSubGameSceneManager : CGameSceneManager
    {
        [Header("★ [Reference] Tutorial")]
        public List<Button> tutorialItemsButton = new List<Button>();
        public List<GameObject> tutorialArrow = new List<GameObject>();
        public GameObject tutorialPanel;
        public TMP_Text tutorialText;

        private void SetupTutorialButtons()
        {
            tutorialItemsButton[0]?.ExAddListener(OnClick_Bottom_Earthquake);
            tutorialItemsButton[1]?.ExAddListener(OnClick_Bottom_AddBall);
            tutorialItemsButton[2]?.ExAddListener(OnClick_Bottom_BricksDelete);
            tutorialItemsButton[3]?.ExAddListener(OnClick_Bottom_AddLaserBricks);
            tutorialItemsButton[4]?.ExAddListener(OnClick_Bottom_AddSteelBricks);

            for(int i=0; i < tutorialItemsButton.Count; i++)
            {
                tutorialItemsButton[i].gameObject.SetActive(Engine.currentLevel == GlobalDefine.TUTORIAL_LEVEL_BOTTOM_ITEM[i]);
                tutorialArrow[i].SetActive(Engine.currentLevel == GlobalDefine.TUTORIAL_LEVEL_BOTTOM_ITEM[i]);
            }

            tutorialPanel.SetActive(false);
        }

        public void CheckTutorial()
        {
            switch(Engine.currentLevel)
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
            
            SetTutorial(index);
        }

        private void ShowTutorial_Add_Ball(int index)
        {
            Debug.Log(CodeManager.GetMethodName());

            SetTutorial(index);
        }

        private void ShowTutorial_Earthquake(int index)
        {
            Debug.Log(CodeManager.GetMethodName());

            SetTutorial(index);
        }

        private void ShowTutorial_Add_Laser_Bricks(int index)
        {
            Debug.Log(CodeManager.GetMethodName());

            SetTutorial(index);
        }

        private void ShowTutorial_Add_Steel_Bricks(int index)
        {
            Debug.Log(CodeManager.GetMethodName());

            SetTutorial(index);
        }

        private void SetTutorial(int index)
        {
            tutorialText.text = GlobalDefine.TUTORIAL_TEXT_BOTTOM_ITEM[index];
            tutorialPanel.SetActive(true);
        }

        private void EndTutorial()
        {
            tutorialPanel.SetActive(false);
            Engine.isTutorial = false;
        }
    }
}
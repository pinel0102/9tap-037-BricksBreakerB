using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    public GameObject openObject;
    public GameObject lockObject;
    public List<GameObject> normalObject;
    public List<GameObject> colorObject;
    public List<GameObject> arrowList;
    public List<Button> buttonList;
    public List<TMP_Text> levelText;

    public int level;
    public int levelType;
    public bool canPlay;

    public void Initialize(int _level, int _levelCount)
    {
        if (_level < _levelCount)
        {
            level = _level + 1;

            var levelInfo = CLevelInfoTable.Inst.GetLevelInfo(_level);
            levelType = levelInfo.LevelType;
            
            //TODO: 레벨 잠그기.
            canPlay = true; //level <= 50;

            SetButton();
            SetArrow(_levelCount);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void SetButton()
    {
        for (int i=0; i < levelText.Count; i++)
        {
            levelText[i].text = string.Format("{0}", level);
        }

        for (int i=0; i < buttonList.Count; i++)
        {
            buttonList[i].onClick.AddListener(() => {
                Func.SetupPlayEpisodeInfo(KDefine.G_CHARACTER_ID_COMMON, level - 1, EPlayMode.NORM);
                CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME);
            });
        }

        switch(levelType)
        {
            case 1: // Color
                normalObject[0].SetActive(false);
                normalObject[1].SetActive(false);
                colorObject[0].SetActive(true);
                colorObject[1].SetActive(true);
                break;
            case 0: // Normal
            default:
                normalObject[0].SetActive(true);
                normalObject[1].SetActive(true);
                colorObject[0].SetActive(false);
                colorObject[1].SetActive(false);
                break;
        }

        openObject.SetActive(canPlay);
        lockObject.SetActive(!canPlay);
    }

    private void SetArrow(int _levelCount)
    {
        for(int i=0; i < arrowList.Count; i++)
        {
            arrowList[i].SetActive(false);
        }

        if (level < _levelCount)
        {
            switch(level % 10)
            {
                case 0: arrowList[2].SetActive(true); break;
                case 1:
                case 2:
                case 3:
                case 4: arrowList[1].SetActive(true); break;
                case 5: arrowList[2].SetActive(true); break;
                case 6:
                case 7:
                case 8: 
                case 9: arrowList[0].SetActive(true); break;
            }
        }
    }
}

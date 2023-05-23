using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelButton : MonoBehaviour
{
    [Header("★ [Parameter] CLevelInfo")]
    public bool isValidInfo;
    
    [Header("★ [Parameter] Live")]
    public int level;
    public int levelType;
    public int starCount;
    public int score;
    public bool isOpen;
    //public bool isClear;

    [Header("★ [Reference] Reference")]
    public GameObject openObject;
    public GameObject lockObject;
    public GameObject arrowObject;
    public List<GameObject> normalObject;
    public List<GameObject> colorObject;
    public List<GameObject> arrowList;
    public List<GameObject> starObjectNormal;
    public List<GameObject> starObjectColor;
    public List<TMP_Text> levelText;
    
    public void Initialize(int _level, int _levelCount)
    {
        if (_level < _levelCount)
        {
            level = _level + 1;

            if (isValidInfo = CLevelInfoTable.Inst.TryGetLevelInfo(_level, out CLevelInfo levelInfo))
            {
                levelType = levelInfo.LevelType;
            }
            else
            {
                levelType = 0;
            }

            isOpen = (level == 1) || level <= GlobalDefine.UserInfo.LevelCurrent;
            //isClear = level < GlobalDefine.UserInfo.LevelCurrent;
            starCount = GlobalDefine.UserStarList[_level];
            score = GlobalDefine.UserScoreList[_level];

            SetButton();
            SetArrow(_levelCount);
            SetStar();
        }
        else
        {
            openObject.SetActive(false);
            lockObject.SetActive(false);
            arrowObject.SetActive(false);
        }
    }

    public void OnClick_LevelButton()
    {
        GlobalDefine.PlaySoundFX(ESoundSet.SOUND_BUTTON);
        Func.SetupPlayEpisodeInfo(KDefine.G_CHARACTER_ID_COMMON, level - 1, EPlayMode.NORM);

        LogFunc.Send_I_Scene_Play(level - 1);
        CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME);
        CGameInfoStorage.Inst.SetPlayStartingTime(System.DateTime.Now);
    }

    private void SetButton()
    {
        for (int i=0; i < levelText.Count; i++)
        {
            levelText[i].text = string.Format(GlobalDefine.FORMAT_INT, level);
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

        openObject.SetActive(isOpen);
        lockObject.SetActive(!isOpen);
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

    private void SetStar()
    {
        for(int i=0; i < starObjectNormal.Count; i++)
        {
            starObjectNormal[i].SetActive(i < starCount);
            starObjectColor[i].SetActive(i < starCount);
        }
    }
}

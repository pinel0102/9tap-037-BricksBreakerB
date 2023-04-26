using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public partial class DebugPanel : MonoBehaviour
{
    [Header("★ [Reference] ETC")]
    public Text levelText;

    [Header("★ [Reference] Input Field")]
    public InputField rubyInput;
    public InputField itemInput;
    public InputField boosterInput;
    public InputField setClearInput;
    public InputField levelPlayInput;

    private bool isEngineAssigned;
    private NSEngine.CEngine Engine;

    private void AssignEngine()
    {
        Engine = CSceneManager.GetSceneManager<GameScene.CSubGameSceneManager>(KCDefine.B_SCENE_N_GAME)?.Engine;
        isEngineAssigned = (Engine != null);

        levelText.text = isEngineAssigned ? string.Format(GlobalDefine.FORMAT_INT, Engine.currentLevel) : string.Empty;
    }

    public void OnClick_BackButton()
    {
        callback_backButton?.Invoke();
        
        CSceneManager.GetSceneManager<MainScene.CSubMainSceneManager>(KCDefine.B_SCENE_N_MAIN)?.OnReceiveNavStackEvent(ENavStackEvent.BACK_KEY_DOWN);
        CSceneManager.GetSceneManager<GameScene.CSubGameSceneManager>(KCDefine.B_SCENE_N_GAME)?.OnReceiveNavStackEvent(ENavStackEvent.BACK_KEY_DOWN);
    }

    public void OnClick_SetRuby()
    {
        if (int.TryParse(rubyInput.text, out int value))
        {
            GlobalDefine.SetRuby(value);
            GlobalDefine.RefreshShopText(CSceneManager.GetSceneManager<MainScene.CSubMainSceneManager>(KCDefine.B_SCENE_N_MAIN)?.rubyText);
        }
    }

    public void OnClick_SetItem()
    {
        if (int.TryParse(itemInput.text, out int value))
        {
            GlobalDefine.SetItem(value);
            CSceneManager.GetSceneManager<GameScene.CSubGameSceneManager>(KCDefine.B_SCENE_N_GAME)?.RefreshItemCount();
        }
    }

    public void OnClick_SetBooster()
    {
        if (int.TryParse(boosterInput.text, out int value))
        {
            GlobalDefine.SetBooster(value);
        }
    }

    public void OnClick_LevelPlay()
    {
        if (int.TryParse(levelPlayInput.text, out int value))
        {
            Func.SetupPlayEpisodeInfo(KDefine.G_CHARACTER_ID_COMMON, value - 1, EPlayMode.NORM);
            CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME);
        }
    }

    public void OnClick_LevelClear()
    {
        if (isEngineAssigned)
            Engine.LevelClear();
    }

    public void OnClick_LevelFail()
    {
        if (isEngineAssigned)
            Engine.LevelFail();
    }

    public void OnClick_SetClear()
    {
        if (int.TryParse(setClearInput.text, out int value))
        {
            GlobalDefine.SetClear(value);
            CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MAIN);
        }
    }

    public void OnClick_ResetUserInfo()
    {
        CUserInfo userInfo = new CUserInfo
        {
            Settings_DarkMode = false,
            Ruby = 0,
            StarSum = 0,
            StarReward = 0,
            Item_ADBlock = false, 
            Item_Earthquake = 0,
            Item_AddBall = 0,
            Item_BricksDelete = 0,
            Item_AddLaserBricks = 0,
            Item_AddSteelBricks = 0,
            Booster_Missile = 0,
            Booster_Lightning = 0,
            Booster_Bomb = 0,
            LevelCurrent = 1,
            LevelStar = string.Empty,
            LevelScore = string.Empty,
            LevelSkip = string.Empty,
        };


        CUserInfoStorage.Inst.ResetUserInfo(CExtension.ExToMsgPackBase64Str(userInfo));
        CUserInfoStorage.Inst.SaveUserInfo();
        CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_INIT);
    }
}
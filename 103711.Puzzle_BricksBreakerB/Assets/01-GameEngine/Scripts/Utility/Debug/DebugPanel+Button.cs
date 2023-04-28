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
    public InputField starInput;
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
        Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>[Debug] Back Button</color>"));

        callback_backButton?.Invoke();
        
        CSceneManager.GetSceneManager<MainScene.CSubMainSceneManager>(KCDefine.B_SCENE_N_MAIN)?.OnReceiveNavStackEvent(ENavStackEvent.BACK_KEY_DOWN);
        CSceneManager.GetSceneManager<GameScene.CSubGameSceneManager>(KCDefine.B_SCENE_N_GAME)?.OnReceiveNavStackEvent(ENavStackEvent.BACK_KEY_DOWN);
    }

    public void OnClick_SetRuby()
    {
        if (int.TryParse(rubyInput.text, out int value))
        {
            Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>[Debug] Set Ruby : {0}</color>", value));

            GlobalDefine.SetRuby(value);
            GlobalDefine.RefreshShopText(CSceneManager.GetSceneManager<MainScene.CSubMainSceneManager>(KCDefine.B_SCENE_N_MAIN)?.rubyText);
        }
    }

    public void OnClick_SetStar()
    {
        if (int.TryParse(starInput.text, out int value))
        {
            Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>[Debug] Set Star : {0}</color>", value));

            GlobalDefine.SetStar(value);
            GlobalDefine.RefreshStarText(CSceneManager.GetSceneManager<MainScene.CSubMainSceneManager>(KCDefine.B_SCENE_N_MAIN)?.starText);
        }
    }

    public void OnClick_SetItem()
    {
        if (int.TryParse(itemInput.text, out int value))
        {
            Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>[Debug] Set Item : {0}</color>", value));

            GlobalDefine.SetItem(value);
            CSceneManager.GetSceneManager<GameScene.CSubGameSceneManager>(KCDefine.B_SCENE_N_GAME)?.RefreshItemCount();
        }
    }

    public void OnClick_SetBooster()
    {
        if (int.TryParse(boosterInput.text, out int value))
        {
            Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>[Debug] Set Booster : {0}</color>", value));

            GlobalDefine.SetBooster(value);
        }
    }

    public void OnClick_LevelPlay()
    {
        if (int.TryParse(levelPlayInput.text, out int value))
        {
            Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>[Debug] Level Play : {0}</color>", value));

            Func.SetupPlayEpisodeInfo(KDefine.G_CHARACTER_ID_COMMON, value - 1, EPlayMode.NORM);
            CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME);
        }
    }

    public void OnClick_LevelClear()
    {
        if (isEngineAssigned)
        {
            Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>[Debug] Level Clear</color>"));

            Engine.LevelClear();
        }
    }

    public void OnClick_LevelFail()
    {
        if (isEngineAssigned)
        {
            Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>[Debug] Level Fail</color>"));

            Engine.LevelFail();
        }
    }

    public void OnClick_SetClear()
    {
        if (int.TryParse(setClearInput.text, out int value))
        {
            Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>[Debug] Set Clear</color>"));

            GlobalDefine.SetClear(value);
            CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_MAIN);
        }
    }

    public void OnClick_ResetTime()
    {
        Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>[Debug] Reset Time</color>"));

        var oCharacterGameInfo = CGameInfoStorage.Inst.GetCharacterGameInfo(CGameInfoStorage.Inst.PlayCharacterID);
        oCharacterGameInfo.PrevDailyRewardTime = System.DateTime.Today.AddDays(-KCDefine.B_VAL_1_INT);
        oCharacterGameInfo.PrevFreeRewardTime = System.DateTime.Today.AddDays(-KCDefine.B_VAL_1_INT);
        CGameInfoStorage.Inst.SaveGameInfo();
    }

    public void OnClick_ResetUserInfo()
    {
        Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>[Debug] Reset User Info</color>"));

        GlobalDefine.ResetUserData();
        GlobalDefine.SaveUserData();
        CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_INIT);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using EnhancedUI.EnhancedScroller;

namespace MainScene {
	/** 서브 메인 씬 관리자 - 추가 */
	public partial class CSubMainSceneManager : CMainSceneManager, IEnhancedScrollerDelegate 
    {
        public int shortcutLevel = 1;

        public TMP_Text shortcutText;
        public TMP_Text rubyText;
        public TMP_Text starText;
        public List<TMP_Text> playerNameText = new List<TMP_Text>();

        private const string formatShortcut = "LEVEL {0}";

        private void InitLobbyButtons()
        {
            shortcutLevel = GlobalDefine.UserInfo.LevelCurrent;
            shortcutText.text = string.Format(formatShortcut, shortcutLevel);            
            GlobalDefine.RefreshShopText(rubyText);
            GlobalDefine.RefreshStarText(starText);
            RefreshNameText();
        }

        public void OnClick_PlayShortcut()
        {
            Debug.Log(CodeManager.GetMethodName() + string.Format(GlobalDefine.FORMAT_INT, shortcutLevel));

            Func.SetupPlayEpisodeInfo(KDefine.G_CHARACTER_ID_COMMON, shortcutLevel - 1, EPlayMode.NORM);
            CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME);
        }

        public void RefreshNameText()
        {
            //Debug.Log(CodeManager.GetMethodName() + GlobalDefine.UserInfo.Settings_PlayerName);

            for (int i=0; i < playerNameText.Count; i++)
            {
                playerNameText[i].text = GlobalDefine.UserInfo.Settings_PlayerName;
            }
        }
    }
}
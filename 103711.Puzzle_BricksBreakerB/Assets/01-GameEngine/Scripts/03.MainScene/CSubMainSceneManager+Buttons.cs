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

        private const string formatShortcut = "LEVEL {0}";

        private void InitLobbyButtons()
        {
            shortcutText.text = string.Format(formatShortcut, shortcutLevel);
        }

        public void OnClick_PlayShortcut()
        {
            Debug.Log(CodeManager.GetMethodName() + string.Format("{0}", shortcutLevel));

            Func.SetupPlayEpisodeInfo(KDefine.G_CHARACTER_ID_COMMON, shortcutLevel - 1, EPlayMode.NORM);
            CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME);
        }
    }
}
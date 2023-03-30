using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene {
    public partial class CSubGameSceneManager : CGameSceneManager
    {
        public void SetDarkMode(bool isDarkMode)
        {
            CUserInfoStorage.Inst.UserInfo.Settings_DarkMode = isDarkMode;
            CUserInfoStorage.Inst.SaveUserInfo();

            ApplyDarkMode(isDarkMode);
        }

         private void ApplyDarkMode(bool isDarkMode)
        {
            darkModeButton[0].SetActive(!isDarkMode);
            darkModeButton[1].SetActive(isDarkMode);
            darkModeBackground[0].SetActive(isDarkMode);
            darkModeBackground[1].SetActive(!isDarkMode);
        }
    }
}
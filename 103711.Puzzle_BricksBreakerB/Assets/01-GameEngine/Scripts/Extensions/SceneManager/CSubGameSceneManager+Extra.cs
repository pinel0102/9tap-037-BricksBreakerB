using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene {
    public partial class CSubGameSceneManager : CGameSceneManager
    {
        public NSEngine.CEngine Engine = null;
        
        [Header("★ [Reference] Extra")]
        public GameObject goldenAimOn;
        public GameObject[] darkModeButton;
        public GameObject[] darkModeBackground;
        
        private void AssignEngine()
        {
            Engine = m_oEngine;
            
            ApplyDarkMode(CUserInfoStorage.Inst.UserInfo.Settings_DarkMode);
            goldenAimOn.SetActive(Engine.isGoldAim);
        }

        public void HideShootUIs()
        {
            for(int i = 0; i < m_oShootUIsList.Count; ++i) {
				m_oShootUIsList[i].SetActive(false);
			}
        }

        public void ToggleAimLayer()
        {
            Engine.ToggleAimLayer();

            goldenAimOn.SetActive(Engine.isGoldAim);
        }

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
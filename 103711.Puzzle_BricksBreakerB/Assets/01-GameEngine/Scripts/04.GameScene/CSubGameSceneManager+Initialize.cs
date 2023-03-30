using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene {
    public partial class CSubGameSceneManager : CGameSceneManager
    {
        public NSEngine.CEngine Engine = null;
        
        [Header("★ [Reference] Extra")]
        public Canvas mainCanvas;
        public Transform startLine;
        public Transform deadLine;
        public GameObject warningObject;
        public GameObject goldenAimOn;
        public GameObject[] darkModeButton;
        public GameObject[] darkModeBackground;
        
        private void AssignEngine()
        {
            Engine = m_oEngine;
            
            ApplyDarkMode(CUserInfoStorage.Inst.UserInfo.Settings_DarkMode);
            warningObject.SetActive(false);
            goldenAimOn.SetActive(m_oEngine.isGoldAim);
        }
    }
}
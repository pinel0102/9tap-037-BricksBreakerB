using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene {
    public partial class CSubGameSceneManager : CGameSceneManager
    {
        [Header("★ [Reference] Initialize")]
        public NSEngine.CEngine Engine = null;
        public Canvas mainCanvas;
        public Transform startLine;
        public Transform deadLine;
        public GameObject warningObject;
        public GameObject[] darkModeButton;
        public GameObject[] darkModeBackground;
        public GameObject[] darkModeTopPanel;
        private Camera mainCamera;
        
        private void AssignEngine()
        {
            Engine = m_oEngine;
            
            Initialize();
        }

        private void Initialize()
        {
            GlobalDefine.HideBannerAD();
            
            ApplyDarkMode(GlobalDefine.UserInfo.Settings_DarkMode);
            warningObject.SetActive(false);
            
            InitAimLayer();
            InitCamera();
            InitUITop();
            InitTabs();
            SetupButtons();

            this.ExLateCallFunc(OnClick_OpenPopup_Preview, KCDefine.B_DELTA_T_INTERMEDIATE, true);
        }

        private void SetupButtons()
        {
            SetupBottomButtons();
            SetupTutorialButtons();
            RefreshGoldenAimButton();
        }
    }
}
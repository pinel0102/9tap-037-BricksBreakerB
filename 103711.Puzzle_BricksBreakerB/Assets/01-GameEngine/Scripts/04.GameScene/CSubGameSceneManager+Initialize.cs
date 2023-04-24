using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene {
    public partial class CSubGameSceneManager : CGameSceneManager
    {
        public NSEngine.CEngine Engine = null;
        
        [Header("★ [Reference] Initialize")]
        public Canvas mainCanvas;
        public Transform startLine;
        public Transform deadLine;
        public GameObject warningObject;
        public GameObject goldenAimOn;
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
            ApplyDarkMode(CUserInfoStorage.Inst.UserInfo.Settings_DarkMode);
            warningObject.SetActive(false);
            goldenAimOn.SetActive(Engine.isGoldAim);

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
        }
    }
}
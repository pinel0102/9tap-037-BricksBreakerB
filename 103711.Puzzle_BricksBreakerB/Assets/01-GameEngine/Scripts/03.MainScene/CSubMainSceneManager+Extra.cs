using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using TMPro;
using EnhancedUI.EnhancedScroller;

namespace MainScene {
	/** 서브 메인 씬 관리자 - 추가 */
	public partial class CSubMainSceneManager : CMainSceneManager, IEnhancedScrollerDelegate {
		#region 변수
		public Transform levelButtonParent;
        public UserProfile userProfile;
        public ScrollFocus scrollFocus;
        
        #endregion // 변수

		#region 함수
		/** 씬을 설정한다 */
		private void SubSetupAwake() {

			this.InitTabs();
            this.InitLobbyButtons();
            this.InitLevelMapButtons();
            this.InitRewardButtons();

            userProfile.Initialize();
            CGameInfoStorage.Inst.Initialize();

            GlobalDefine.RequestBannerAD();
            isLoadRewardAds = GlobalDefine.IsEnableRewardVideo();

            if (!GlobalDefine.isLevelEditor && !GlobalDefine.isMainSceneOpened && Access.IsEnableGetDailyReward(CGameInfoStorage.Inst.PlayCharacterID))
            {
                GlobalDefine.isMainSceneOpened = true;
                OnClick_OpenPopup_CheckIn();
            }
		}

        private void InitLevelMapButtons()
        {
            scrollFocus.gameObject.SetActive(true);

            int levelCount = CLevelInfoTable.Inst.levelCount;
            int focusIndex = (GlobalDefine.UserInfo.LevelCurrent - 1) / 5;
            
            Debug.Log(CodeManager.GetMethodName() + string.Format("Level Count : {0}", levelCount));
            
            for (int i=0; i <= ((levelCount - 1) / 5); i++)
            {
                GameObject ga = GameObject.Instantiate(Resources.Load<GameObject>(i%2 == 0 ? GlobalDefine.PREFAB_LEVEL_LIST_LEFT : GlobalDefine.PREFAB_LEVEL_LIST_RIGHT), levelButtonParent);
                ga.GetComponent<LevelList>().Initialize(i * 5, levelCount);

                if (i == focusIndex)
                {
                    scrollFocus.objectToFocus = ga.GetComponent<RectTransform>();
                }
            }

            this.ExLateCallFunc((sender) => {
                scrollFocus.SetFocus();
                scrollFocus.gameObject.SetActive(false);
            }, KCDefine.U_DELAY_INIT);
        }

        #endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

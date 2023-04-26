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
        
#region Popups

        public void OnClick_OpenPopup_Store()
        {
            this.OnTouchStoreBtn();
        }

        public void OnClick_OpenPopup_CheckIn()
        {
            Func.ShowDailyRewardPopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CDailyRewardPopup).Init();
			});
        }

        public void OnClick_OpenPopup_SkipLevel()
        {
            Func.ShowSkipLevelPopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CSkipLevelPopup).Init();
			});
        }

        public void OnClick_OpenPopup_PiggyBank()
        {
            Func.ShowPiggyBankPopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CPiggyBankPopup).Init();
			});
        }

        public void OnClick_OpenPopup_StarterPack()
        {
            Func.ShowStarterPackPopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CStarterPackPopup).Init();
			});
        }

        public void OnClick_OpenPopup_Membership()
        {
            Func.ShowMembershipPopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CMembershipPopup).Init();
			});
        }

        public void OnClick_OpenPopup_GetReward()
        {
            Func.ShowDailyRewardPopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CDailyRewardPopup).Init();
			});
        }

        public void OnClick_OpenPopup_RateUs()
        {
            Func.ShowDailyRewardPopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CDailyRewardPopup).Init();
			});
        }

#endregion Popups

    }
}
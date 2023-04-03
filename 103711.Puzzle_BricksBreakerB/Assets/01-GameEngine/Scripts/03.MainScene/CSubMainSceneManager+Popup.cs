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
            List<STProductTradeInfo> productTradeInfo = new List<STProductTradeInfo>();

            Func.ShowStorePopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CStorePopup).Init(CStorePopup.MakeParams(productTradeInfo));
			});
        }

        public void OnClick_OpenPopup_CheckIn()
        {
            Func.ShowDailyRewardPopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CDailyRewardPopup).Init();
			});
        }

#endregion Popups

    }
}
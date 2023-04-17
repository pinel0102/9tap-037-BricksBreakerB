using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GameScene {
    public partial class CSubGameSceneManager : CGameSceneManager
    {
#region Popups

        public void OnClick_OpenPopup_Preview(MonoBehaviour sender)
        {
            Func.ShowPreviewPopup(this.PopupUIs, (a_oSender) => {
				(a_oSender as CPreviewPopup).Init(CPreviewPopup.MakeParams(new Dictionary<CPreviewPopup.ECallback, System.Action<CPreviewPopup>>() {
                    [CPreviewPopup.ECallback.RESUME] = (a_oPopupSender) => this.OnReceivePopupResult(a_oPopupSender, EPopupResult.RESUME)
                }, this.Engine));
			});
        }

#endregion Popups
    }
}
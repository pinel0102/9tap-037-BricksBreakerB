using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene {
    public partial class CSubGameSceneManager : CGameSceneManager
    {
        [Header("★ [Reference] UI Bottom")]
        public List<Button> bottomButtons = new List<Button>();

        public void HideShootUIs()
        {
            for(int i = 0; i < m_oShootUIsList.Count; ++i) {
				m_oShootUIsList[i].SetActive(false);
			}
        }

        public void ToggleAimLayer()
        {
            m_oEngine.ToggleAimLayer();

            goldenAimOn.SetActive(m_oEngine.isGoldAim);
        }
    }
}
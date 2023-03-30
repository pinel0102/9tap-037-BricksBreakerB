using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene {
    public partial class CSubGameSceneManager : CGameSceneManager
    {
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
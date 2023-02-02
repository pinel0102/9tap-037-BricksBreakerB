using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene {
    public partial class CSubGameSceneManager : CGameSceneManager
    {
        public NSEngine.CEngine Engine = null;

        private void AssignEngine()
        {
            Engine = m_oEngine;
        }

        public void HideShootUIs()
        {
            for(int i = 0; i < m_oShootUIsList.Count; ++i) {
				m_oShootUIsList[i].SetActive(false);
			}
        }
    }
}
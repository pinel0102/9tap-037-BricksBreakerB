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
		/** =====> 객체 <===== */
		[SerializeField] private GameObject m_oTempMenuUIs = null;

        public Transform levelButtonParent;
        #endregion // 변수

		#region 함수
		/** 씬을 설정한다 */
		private void SubSetupAwake() {

			this.SubSetupTestUIs();

            this.InitTabs();
            this.InitLobbyButtons();
            this.InitLevelMapButtons();
		}

        private void InitLevelMapButtons()
        {
            var levelCount = CLevelInfoTable.Inst.levelCount;
            
            for (int i=0; i <= ((levelCount - 1) / 5); i++)
            {
                GameObject ga = GameObject.Instantiate(Resources.Load<GameObject>(i%2 == 0 ? GlobalDefine.PREFAB_LEVEL_LIST_LEFT : GlobalDefine.PREFAB_LEVEL_LIST_RIGHT), levelButtonParent);
                ga.GetComponent<LevelList>().Initialize(i * 5, levelCount);
            }
        }

        #endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

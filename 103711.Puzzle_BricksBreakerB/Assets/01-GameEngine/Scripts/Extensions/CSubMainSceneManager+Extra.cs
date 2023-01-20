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
        #endregion // 변수

		#region 함수
		/** 씬을 설정한다 */
		private void SubSetupAwake() {
#if DEBUG || DEVELOPMENT_BUILD
			this.SubSetupTestUIs();
#endif // #if DEBUG || DEVELOPMENT_BUILD

			// FIXME: 임시
			for(int i = 0; i < m_oTempMenuUIs.transform.childCount; ++i) {
				int nIdx = i;

				m_oTempMenuUIs.transform.GetChild(i).GetComponentInChildren<Button>().onClick.AddListener(() => {
					var oText = m_oTempMenuUIs.transform.GetChild(nIdx).GetComponentInChildren<TMP_Text>();
					Func.SetupPlayEpisodeInfo(KDefine.G_CHARACTER_ID_COMMON, int.Parse(oText.text) - 1, EPlayMode.NORM);
                    Debug.Log(CodeManager.GetMethodName() + string.Format("{0}", int.Parse(oText.text)));

					CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_GAME);
				});
			}
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

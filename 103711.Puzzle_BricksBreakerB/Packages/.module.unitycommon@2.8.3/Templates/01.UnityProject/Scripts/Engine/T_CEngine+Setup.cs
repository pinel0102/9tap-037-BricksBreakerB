#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 엔진 - 설정 */
	public partial class CEngine : CComponent {
#region 함수
		/** 레벨을 설정한다 */
		private void SetupLevel() {
			// 레벨 정보가 존재 할 경우
			if(CGameInfoStorage.Inst.PlayLevelInfo != null) {
				for(int i = 0; i < CGameInfoStorage.Inst.PlayLevelInfo.m_oCellInfoDictContainer.Count; ++i) {
					for(int j = 0; j < CGameInfoStorage.Inst.PlayLevelInfo.m_oCellInfoDictContainer[i].Count; ++j) {
						this.SetupCell(CGameInfoStorage.Inst.PlayLevelInfo.m_oCellInfoDictContainer[i][j], m_oGridInfoList[this.SelGridInfoIdx]);
					}
				}
			}
		}
#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

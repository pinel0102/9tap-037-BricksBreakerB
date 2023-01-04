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
		/** 엔진을 설정한다 */
		private void SetupEngine() {
			m_oGridInfoList.Clear();
			m_oGridInfoList.ExAddVal((CGameInfoStorage.Inst.PlayLevelInfo != null) ? Factory.MakeGridInfo(KCDefine.B_ANCHOR_MID_CENTER, Vector3.zero, Vector3.zero, CGameInfoStorage.Inst.PlayLevelInfo.NumCells) : STGridInfo.INVALID);

			m_oCellObjLists = new List<CEObj>[CGameInfoStorage.Inst.PlayLevelInfo.NumCells.y, CGameInfoStorage.Inst.PlayLevelInfo.NumCells.x];
			CGameInfoStorage.Inst.PlayEpisodeInfo.m_oClearTargetInfoDict.ExCopyTo(m_oClearTargetInfoDict, (a_stTargetInfo) => a_stTargetInfo);

			// 객체 풀을 설정한다
			CSceneManager.ActiveSceneManager.AddObjsPool(KDefine.E_KEY_ITEM_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_ITEM), this.Params.m_oItemRoot, KCDefine.U_SIZE_OBJS_POOL_01, false);
			CSceneManager.ActiveSceneManager.AddObjsPool(KDefine.E_KEY_SKILL_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_SKILL), this.Params.m_oSkillRoot, KCDefine.U_SIZE_OBJS_POOL_01, false);
			CSceneManager.ActiveSceneManager.AddObjsPool(KDefine.E_KEY_OBJ_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_OBJ), this.Params.m_oObjRoot, KCDefine.U_SIZE_OBJS_POOL_01, false);
			CSceneManager.ActiveSceneManager.AddObjsPool(KDefine.E_KEY_FX_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_FX), this.Params.m_oFXRoot, KCDefine.U_SIZE_OBJS_POOL_01, false);

			this.SubSetupEngine();
		}

		/** 레벨을 설정한다 */
		private void SetupLevel() {
			// 레벨 정보가 존재 할 경우
			if(CGameInfoStorage.Inst.PlayLevelInfo != null) {
				for(int i = 0; i < CGameInfoStorage.Inst.PlayLevelInfo.m_oCellInfoDictContainer.Count; ++i) {
					for(int j = 0; j < CGameInfoStorage.Inst.PlayLevelInfo.m_oCellInfoDictContainer[i].Count; ++j) {
						this.SetupCell(CGameInfoStorage.Inst.PlayLevelInfo.m_oCellInfoDictContainer[i][j], this.SelGridInfo);
					}
				}
			}
		}
#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

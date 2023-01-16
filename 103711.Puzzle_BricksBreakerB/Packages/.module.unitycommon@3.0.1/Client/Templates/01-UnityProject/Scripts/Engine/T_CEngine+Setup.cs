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
						this.SetupCell(CGameInfoStorage.Inst.PlayLevelInfo.m_oCellInfoDictContainer[i][j], this.SelGridInfo);
					}
				}
			}
		}

		/** 셀을 설정한다 */
		private void SetupCell(STCellInfo a_stCellInfo, STGridInfo a_stGridInfo) {
			var oCellObjList = new List<CEObj>();

			for(int i = 0; i < a_stCellInfo.m_oCellObjInfoList.Count; ++i) {
#if NEVER_USE_THIS
				// FIXME: dante (비활성 처리 - 필요 시 활성 및 사용 가능) {
				// 객체 종류가 유효 할 경우
				if(a_stCellInfo.m_oCellObjInfoList[i].ObjKinds.ExIsValid() && a_stCellInfo.m_oCellObjInfoList[i].ObjKinds != EObjKinds.BG_PLACEHOLDER_01) {
					var oCellObj = this.CreateCellObj(CObjInfoTable.Inst.GetObjInfo(a_stCellInfo.m_oCellObjInfoList[i].ObjKinds), null);
					oCellObj.transform.localPosition = this.SelGridInfo.m_stPivotPos + a_stCellInfo.m_stIdx.ExToPos(Access.CellCenterOffset, Access.CellSize);				
					
					oCellObj.SetCellIdx(a_stCellInfo.m_stIdx);
					oCellObj.SetCellObjInfo(a_stCellInfo.m_oCellObjInfoList[i]);

					oCellObjList.ExAddVal(oCellObj);
				}
				// FIXME: dante (비활성 처리 - 필요 시 활성 및 사용 가능) }
#endif // #if NEVER_USE_THIS
			}

			this.CellObjLists[a_stCellInfo.m_stIdx.y, a_stCellInfo.m_stIdx.x] = oCellObjList;
			this.SubSetupCell(a_stCellInfo, a_stGridInfo);
		}

		/** 그리드 라인을 설정한다 */
		private void SetupGridLine() {
			this.SubSetupGridLine();
		}

		/** 획득 타겟 정보를 설정한다 */
		private void SetupAcquireTargetInfos(CEObjComponent a_oEObjComponent, Dictionary<ulong, STTargetInfo> a_oOutAcquireTargetInfos) {
			// 아이템 일 경우
			if(a_oEObjComponent.Params.m_stBaseParams.m_oObjsPoolKey.Equals(KDefine.E_KEY_ITEM_OBJS_POOL)) {
				(a_oEObjComponent as CEItem).Params.m_stItemInfo.m_oAcquireTargetInfoDict.ExCopyTo(a_oOutAcquireTargetInfos, (a_stTargetInfo) => a_stTargetInfo);
			}
			// 적 객체 일 경우
			else if(a_oEObjComponent.Params.m_stBaseParams.m_oObjsPoolKey.Equals(KDefine.E_KEY_ENEMY_OBJ_OBJS_POOL)) {
				(a_oEObjComponent as CEObj).Params.m_stObjInfo.m_oAcquireTargetInfoDict.ExCopyTo(a_oOutAcquireTargetInfos, (a_stTargetInfo) => a_stTargetInfo);
			}
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

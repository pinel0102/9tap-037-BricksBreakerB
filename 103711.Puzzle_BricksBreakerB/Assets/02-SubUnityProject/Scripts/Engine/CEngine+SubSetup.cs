using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 서브 엔진 - 설정 */
	public partial class CEngine : CComponent {
		#region 함수
		/** 엔진을 설정한다 */
		private void SubAwake() {
			// Do Something
		}

		/** 엔진을 설정한다 */
		private void SubSetupEngine() {
			// 라인 효과를 설정한다 {
			CFunc.SetupComponents(new List<(ESubKey, string, GameObject, GameObject)>() {
				(ESubKey.LINE_FX, $"{ESubKey.LINE_FX}", this.Params.m_oObjRoot, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_LINE_FX))
			}, m_oSubLineFXDict);

			m_oSubLineFXDict[ESubKey.LINE_FX].ExSetWidth(KCDefine.B_VAL_5_REAL, KCDefine.B_VAL_5_REAL);
			m_oSubLineFXDict[ESubKey.LINE_FX].ExSetColor(Color.white, Color.white);
			m_oSubLineFXDict[ESubKey.LINE_FX].ExSetSortingOrder(KCDefine.U_SORTING_OI_CELL.ExGetExtraOrder(-KCDefine.B_VAL_1_INT));
			// 라인 효과를 설정한다 }

			// 객체 풀을 설정한다 {
			CSceneManager.ActiveSceneManager.AddObjsPool(KDefine.E_KEY_CELL_OBJ_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_CELL_OBJ), this.Params.m_oCellRoot, KCDefine.U_SIZE_OBJS_POOL_01, false);
			CSceneManager.ActiveSceneManager.AddObjsPool(KDefine.E_KEY_PLAYER_OBJ_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_PLAYER_OBJ), this.Params.m_oObjRoot, KCDefine.B_VAL_1_INT, false);
			CSceneManager.ActiveSceneManager.AddObjsPool(KDefine.E_KEY_ENEMY_OBJ_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_ENEMY_OBJ), this.Params.m_oObjRoot, KCDefine.U_SIZE_OBJS_POOL_01, false);

			CSceneManager.ActiveSceneManager.AddObjsPool(KDefine.E_KEY_BALL_OBJ_OBJS_POOL, CResManager.Inst.GetRes<GameObject>(KDefine.E_OBJ_P_BALL_OBJ), this.Params.m_oObjRoot, KCDefine.U_SIZE_OBJS_POOL_01, false);

            foreach (KeyValuePair<EFXSet, KeyValuePair<string, float>> item in GlobalDefine.FXContainer)
            {
                CSceneManager.ActiveSceneManager.AddObjsPool(item.Value.Key, CResManager.Inst.GetRes<GameObject>(string.Format(GlobalDefine.formatFXPath, item.Value.Key)), this.Params.m_oFXRoot, KCDefine.U_SIZE_OBJS_POOL_01, false);
            }

			// 객체 풀을 설정한다 }
		}

		/** 셀을 설정한다 */
		private void SetupCell(STCellInfo a_stCellInfo, STGridInfo a_stGridInfo) {
			var oCellObjList = new List<CEObj>();

			for(int i = 0; i < a_stCellInfo.m_oCellObjInfoList.Count; ++i) {
                EObjKinds kinds = a_stCellInfo.m_oCellObjInfoList[i].ObjKinds;
                EObjKinds kindsType = (EObjKinds)((int)kinds).ExKindsToCorrectKinds(EKindsGroupType.SUB_KINDS_TYPE);
                STObjInfo stObjInfo = CObjInfoTable.Inst.GetObjInfo(kinds);
                STCellObjInfo stCellObjInfo = (STCellObjInfo)a_stCellInfo.m_oCellObjInfoList[i].Clone();

				var oCellObj = this.CreateCellObj(stObjInfo, null);
                
				oCellObj.transform.localPosition = this.SelGridInfo.m_stPivotPos + a_stCellInfo.m_stIdx.ExToPos(Access.CellCenterOffset, Access.CellSize);
                oCellObj.GetController<CECellObjController>().ResetObjInfo(stObjInfo, stCellObjInfo);
                oCellObj.GetController<CECellObjController>().SetIdx(a_stCellInfo.m_stIdx);
                oCellObj.SetCellIdx(a_stCellInfo.m_stIdx, kinds);
				oCellObj.SetCellObjInfo(stCellObjInfo);
                oCellObj.SetExtraObjKindsList(stObjInfo);
                oCellObj.AddCellEffect(kindsType);

                InitCellLayer(oCellObj);

				oCellObjList.ExAddVal(oCellObj);
			}

			this.CellObjLists[a_stCellInfo.m_stIdx.y, a_stCellInfo.m_stIdx.x] = oCellObjList;
		}

        private void SetupOffsetCell()
        {
            for(int i=CGameInfoStorage.Inst.PlayLevelInfo.NumCells.y; i < CGameInfoStorage.Inst.PlayLevelInfo.NumCells.y + GlobalDefine.GRID_DOWN_OFFSET; i++)
            {
                for(int j=0; j < CGameInfoStorage.Inst.PlayLevelInfo.NumCells.x; j++)
                {
                    this.CellObjLists[i, j] = new List<CEObj>();
                }
            }
        }

		/** 그리드 라인을 설정한다 */
		private void SetupGridLine() {
			// Do Something
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

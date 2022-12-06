using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
using GoogleSheetsToUnity;

/** 서비스 관리자 - 구글 시트 */
public partial class CServicesManager : CSingleton<CServicesManager> {
	#region 함수
	/** 구글 시트를 로드한다 */
	public void LoadGoogleSheet(string a_oID, string a_oName, System.Action<CServicesManager, STGoogleSheetLoadInfo, bool> a_oCallback, int a_nSrcIdx = KCDefine.B_VAL_0_INT, int a_nNumRows = KCDefine.U_MAX_NUM_GOOGLE_SHEET_ROWS) {
		CFunc.ShowLog($"CServicesManager.LoadGoogleSheet: {a_oID}, {a_oName}, {a_nSrcIdx}, {Mathf.Min(a_nNumRows, KCDefine.U_MAX_NUM_GOOGLE_SHEET_ROWS)}", KCDefine.B_LOG_COLOR_PLUGIN);
		CAccess.Assert(a_oID.ExIsValid() && a_oName.ExIsValid() && a_nSrcIdx > KCDefine.B_IDX_INVALID);

		var stCellInfo = this.MakeLoadCellInfo(a_nSrcIdx, a_nNumRows);
		m_oCallbackDict02.ExReplaceVal(EServicesCallback.LOAD_GOOGLE_SHEET, a_oCallback);
		SpreadsheetManager.Read(new GSTU_Search(a_oID, a_oName, stCellInfo.Item3, stCellInfo.Item4, KCDefine.U_COL_N_GOOGLE_SHEET_SRC, stCellInfo.Item1), (a_oGoogleSheet) => this.OnLoadGoogleSheet(a_oGoogleSheet, a_oID, a_oName, a_nSrcIdx, stCellInfo.Item2));
	}

	/** 구글 시트를 저장한다 */
	public void SaveGoogleSheet(string a_oID, string a_oName, List<List<string>> a_oDataListContainer, System.Action<CServicesManager, STGoogleSheetSaveInfo, bool> a_oCallback, int a_nSrcIdx = KCDefine.B_VAL_0_INT, int a_nNumRows = KCDefine.U_MAX_NUM_GOOGLE_SHEET_ROWS) {
		CFunc.ShowLog($"CServicesManager.SaveGoogleSheet: {a_oID}, {a_oName}, {a_nSrcIdx}, {Mathf.Min(a_nNumRows, KCDefine.U_MAX_NUM_GOOGLE_SHEET_ROWS)}", KCDefine.B_LOG_COLOR_PLUGIN);
		CAccess.Assert(a_oID.ExIsValid() && a_oName.ExIsValid() && a_oDataListContainer.ExIsValid() && a_nSrcIdx > KCDefine.B_IDX_INVALID);

		var stCellInfo = this.MakeSaveCellInfo(a_nSrcIdx, a_nNumRows);
		m_oCallbackDict03.ExReplaceVal(EServicesCallback.SAVE_GOOGLE_SHEET, a_oCallback);
		SpreadsheetManager.Write(new GSTU_Search(a_oID, a_oName, stCellInfo.Item3, stCellInfo.Item4, KCDefine.U_COL_N_GOOGLE_SHEET_SRC, stCellInfo.Item1), new ValueRange(a_oDataListContainer), () => this.OnSaveGoogleSheet(a_oID, a_oName, a_nSrcIdx, stCellInfo.Item2));
	}

	/** 구글 시트를 로드했을 경우 */
	private void OnLoadGoogleSheet(GstuSpreadSheet a_oGoogleSheet, string a_oID, string a_oName, int a_nSrcIdx, int a_nNumRows) {
		CFunc.ShowLog($"CServicesManager.OnLoadGoogleSheet: {a_oID}, {a_oName}, {!SpreadsheetManager.IsError}", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_SERVICES_M_LOAD_GOOGLE_SHEET_CALLBACK, () => {
			m_oCallbackDict02.GetValueOrDefault(EServicesCallback.LOAD_GOOGLE_SHEET)?.Invoke(this, new STGoogleSheetLoadInfo() {
				m_nSrcIdx = a_nSrcIdx,
				m_nNumRows = a_nNumRows,
				m_oID = a_oID,
				m_oSheetName = a_oName,
				m_oGoogleSheet = a_oGoogleSheet
			}, a_oGoogleSheet.Cells.ExIsValid());
		});
	}

	/** 구글 시트를 저장했을 경우 */
	private void OnSaveGoogleSheet(string a_oID, string a_oName, int a_nSrcIdx, int a_nNumRows) {
		CFunc.ShowLog($"CServicesManager.OnSaveGoogleSheet: {a_oID}, {a_oName}, {!SpreadsheetManager.IsError}", KCDefine.B_LOG_COLOR_PLUGIN);

		CScheduleManager.Inst.AddCallback(KCDefine.U_KEY_SERVICES_M_SAVE_GOOGLE_SHEET_CALLBACK, () => {
			m_oCallbackDict03.GetValueOrDefault(EServicesCallback.SAVE_GOOGLE_SHEET)?.Invoke(this, new STGoogleSheetSaveInfo() {
				m_nSrcIdx = a_nSrcIdx,
				m_nNumRows = a_nNumRows,
				m_oID = a_oID,
				m_oSheetName = a_oName
			}, !SpreadsheetManager.IsError);
		});
	}

	/** 로드 셀 정보를 생성한다 */
	private (int, int, string, string) MakeLoadCellInfo(int a_nSrcIdx, int a_nNumRows) {
		int nSrcIdx = Mathf.Max(KCDefine.B_VAL_1_INT, a_nSrcIdx + KCDefine.B_VAL_1_INT);
		int nNumCells = Mathf.Min(a_nNumRows, KCDefine.U_MAX_NUM_GOOGLE_SHEET_ROWS);

		return (nSrcIdx, nNumCells, string.Format(KCDefine.U_CELL_N_FMT_GOOGLE_SHEET_SRC, nSrcIdx), string.Format(KCDefine.U_CELL_N_FMT_GOOGLE_SHEET_DEST, nSrcIdx + (nNumCells - KCDefine.B_VAL_1_INT)));
	}

	/** 저장 셀 정보를 생성한다 */
	private (int, int, string, string) MakeSaveCellInfo(int a_nSrcIdx, int a_nNumRows) {
		int nSrcIdx = Mathf.Max(KCDefine.B_VAL_2_INT, a_nSrcIdx + KCDefine.B_VAL_2_INT);
		int nNumCells = Mathf.Min(a_nNumRows, KCDefine.U_MAX_NUM_GOOGLE_SHEET_ROWS);

		return (nSrcIdx, nNumCells, string.Format(KCDefine.U_CELL_N_FMT_GOOGLE_SHEET_SRC, nSrcIdx), string.Format(KCDefine.U_CELL_N_FMT_GOOGLE_SHEET_DEST, nSrcIdx + (nNumCells - KCDefine.B_VAL_1_INT)));
	}
	#endregion // 함수
}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)

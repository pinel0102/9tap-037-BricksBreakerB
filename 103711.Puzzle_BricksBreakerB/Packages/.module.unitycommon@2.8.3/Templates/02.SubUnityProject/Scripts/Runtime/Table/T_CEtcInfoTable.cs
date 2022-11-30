#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.IO;

/** 기타 정보 테이블 */
public partial class CEtcInfoTable : CSingleton<CEtcInfoTable> {
	#region 함수
	/** 기타 정보를 리셋한다 */
	public virtual void ResetEtcInfos() {
		CCalcInfoTable.Inst.ResetCalcInfos();
		CEpisodeInfoTable.Inst.ResetEpisodeInfos();
		CTutorialInfoTable.Inst.ResetTutorialInfos();
		CFXInfoTable.Inst.ResetFXInfos();
	}

	/** 기타 정보를 리셋한다 */
	public virtual void ResetEtcInfos(string a_oJSONStr) {
		this.ResetEtcInfos();

		CCalcInfoTable.Inst.ResetCalcInfos(a_oJSONStr);
		CEpisodeInfoTable.Inst.ResetEpisodeInfos(a_oJSONStr);
		CTutorialInfoTable.Inst.ResetTutorialInfos(a_oJSONStr);
		CFXInfoTable.Inst.ResetFXInfos(a_oJSONStr);
	}

	/** 기타 정보를 로드한다 */
	public void LoadEtcInfos() {
		CCalcInfoTable.Inst.LoadCalcInfos();
		CEpisodeInfoTable.Inst.LoadEpisodeInfos();
		CTutorialInfoTable.Inst.LoadTutorialInfos();
		CFXInfoTable.Inst.LoadFXInfos();
	}

	/** 기타 정보를 저장한다 */
	public void SaveEtcInfos(string a_oJSONStr, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oJSONStr != null);

		// JSON 문자열이 존재 할 경우
		if(a_oJSONStr != null) {
			this.ResetEtcInfos(a_oJSONStr);

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
			CFunc.WriteStr(Access.EtcInfoTableSavePath, a_oJSONStr, false);
#else
			CFunc.WriteStr(Access.EtcInfoTableSavePath, a_oJSONStr, true);
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)

#if UNITY_ANDROID && (DEBUG || DEVELOPMENT)
			CUnityMsgSender.Inst.SendShowToastMsg($"CEtcInfoTable.SaveEtcInfos: {File.Exists(Access.EtcInfoTableSavePath)}");
#endif // #if UNITY_ANDROID && (DEBUG || DEVELOPMENT)
		}
	}

	/** 기타 정보 JSON 문자열을 로드한다 */
	private string LoadEtcInfosJSONStr(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
		return File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, false) : CFunc.ReadStrFromRes(a_oFilePath, false);
#else
		return File.Exists(a_oFilePath) ? CFunc.ReadStr(a_oFilePath, true) : CFunc.ReadStrFromRes(a_oFilePath, false);
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
	}
	#endregion // 함수

	#region 조건부 함수
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 기타 정보를 저장한다 */
	public void SaveEtcInfos() {
		var oEtcInfos = SimpleJSON.JSONNode.Parse(this.LoadEtcInfosJSONStr(Access.EtcInfoTableLoadPath));
		CCalcInfoTable.Inst.SaveCalcInfos(oEtcInfos);
		CEpisodeInfoTable.Inst.SaveEpisodeInfos(oEtcInfos);
		CTutorialInfoTable.Inst.SaveTutorialInfos(oEtcInfos);
		CFXInfoTable.Inst.SaveFXInfos(oEtcInfos);

		this.SaveEtcInfos(oEtcInfos.ToString());
	}

	/** 기타 정보 값을 생성한다 */
	public Dictionary<string, List<List<string>>> MakeEtcInfoVals() {
		var oEtcInfos = SimpleJSON.JSONNode.Parse(this.LoadEtcInfosJSONStr(Access.EtcInfoTableSavePath));
		var oEtcInfoValDictContainer = new Dictionary<string, List<List<string>>>();

		CCalcInfoTable.Inst.MakeCalcInfoVals(oEtcInfos, oEtcInfoValDictContainer);
		CEpisodeInfoTable.Inst.MakeEpisodeInfoVals(oEtcInfos, oEtcInfoValDictContainer);
		CTutorialInfoTable.Inst.MakeTutorialInfoVals(oEtcInfos, oEtcInfoValDictContainer);
		CFXInfoTable.Inst.MakeFXInfoVals(oEtcInfos, oEtcInfoValDictContainer);

		return oEtcInfoValDictContainer;
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	#endregion // 조건부 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

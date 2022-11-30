using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** 문자열 테이블 */
public partial class CStrTable : CSingleton<CStrTable> {
	#region 변수
	private Dictionary<string, string> m_oStrDict = new Dictionary<string, string>();
	#endregion // 변수

	#region 함수
	/** 상태를 리셋한다 */
	public override void Reset() {
		base.Reset();
		m_oStrDict?.Clear();
	}

	/** 문자열을 반환한다 */
	public string GetStr(string a_oKey) {
		CAccess.Assert(a_oKey.ExIsValid());
		return m_oStrDict.GetValueOrDefault(a_oKey, a_oKey);
	}

	/** 문자열을 추가한다 */
	public void AddStr(string a_oKey, string a_oStr, bool a_bIsReplace = false) {
		CAccess.Assert(a_oKey.ExIsValid() && a_oStr != null);

		// 문자열 추가가 가능 할 경우
		if(a_bIsReplace || !m_oStrDict.ContainsKey(a_oKey)) {
			m_oStrDict.ExReplaceVal(a_oKey, a_oStr);
		}
	}

	/** 문자열을 제거한다 */
	public void RemoveStr(string a_oKey) {
		CAccess.Assert(a_oKey.ExIsValid());
		m_oStrDict.ExRemoveVal(a_oKey);
	}

	/** 문자열을 로드한다 */
	public Dictionary<string, string> LoadStrs(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		return this.DoLoadStrs(CFunc.ReadStr(a_oFilePath, false));
	}

	/** 문자열을 로드한다 */
	public Dictionary<string, string> LoadStrsFromRes(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

		try {
			return this.DoLoadStrs(CResManager.Inst.GetRes<TextAsset>(a_oFilePath).text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
	}

	/** 문자열을 로드한다 */
	private Dictionary<string, string> DoLoadStrs(string a_oCSVStr) {
		CAccess.Assert(a_oCSVStr.ExIsValid());
		var oStrInfoList = CSVParser.Parse(a_oCSVStr);

		for(int i = 0; i < oStrInfoList.Count; ++i) {
			this.AddStr(oStrInfoList[i][KCDefine.U_KEY_STR_T_ID], oStrInfoList[i][KCDefine.U_KEY_STR_T_STR], int.Parse(oStrInfoList[i][KCDefine.U_KEY_REPLACE]) != KCDefine.B_VAL_0_INT);
		}

		return m_oStrDict;
	}
	#endregion // 함수
}

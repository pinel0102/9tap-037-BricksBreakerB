using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** 값 테이블 */
public partial class CValTable : CSingleton<CValTable> {
	#region 변수
	private Dictionary<string, STValInfo> m_oValInfoDict = new Dictionary<string, STValInfo>();
	#endregion // 변수

	#region 함수
	/** 상태를 리셋한다 */
	public override void Reset() {
		base.Reset();
		m_oValInfoDict.Clear();
	}

	/** 정수를 추가한다 */
	public void AddInt(string a_oKey, long a_nVal, bool a_bIsReplace = false) {
		this.AddValInfo(a_oKey, new STValInfo(EValType.INT, a_nVal), a_bIsReplace);
	}

	/** 실수를 추가한다 */
	public void AddReal(string a_oKey, double a_dblVal, bool a_bIsReplace = false) {
		this.AddValInfo(a_oKey, new STValInfo(EValType.REAL, (decimal)a_dblVal), a_bIsReplace);
	}

	/** 값을 제거한다 */
	public void RemoveVal(string a_oKey) {
		CAccess.Assert(a_oKey.ExIsValid());
		m_oValInfoDict.ExRemoveVal(a_oKey);
	}

	/** 값을 로드한다 */
	public Dictionary<string, STValInfo> LoadVals(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		return this.DoLoadVals(CFunc.ReadStr(a_oFilePath, false));
	}

	/** 값을 로드한다 */
	public Dictionary<string, STValInfo> LoadValsFromRes(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());

		try {
			return this.DoLoadVals(CResManager.Inst.GetRes<TextAsset>(a_oFilePath).text);
		} finally {
			CResManager.Inst.RemoveRes<TextAsset>(a_oFilePath, true);
		}
	}

	/** 값 정보를 추가한다 */
	private void AddValInfo(string a_oKey, STValInfo a_stValInfo, bool a_bIsReplace = false) {
		CAccess.Assert(a_oKey.ExIsValid());

		// 실수 추가가 가능 할 경우
		if(a_bIsReplace || !m_oValInfoDict.ContainsKey(a_oKey)) {
			m_oValInfoDict.ExReplaceVal(a_oKey, a_stValInfo);
		}
	}

	/** 값을 로드한다 */
	private Dictionary<string, STValInfo> DoLoadVals(string a_oCSVStr) {
		CAccess.Assert(a_oCSVStr.ExIsValid());
		var oValInfoList = CSVParser.Parse(a_oCSVStr);

		for(int i = 0; i < oValInfoList.Count; ++i) {
			this.AddValInfo(oValInfoList[i][KCDefine.U_KEY_VAL_T_ID], new STValInfo() {
				m_dmVal = decimal.TryParse(oValInfoList[i][KCDefine.U_KEY_VAL_T_VAL], out decimal dmVal) ? dmVal : KCDefine.B_VAL_0_INT,
				m_eValType = (EValType)int.Parse(oValInfoList[i][KCDefine.U_KEY_VAL_T_VAL_TYPE])
			}, int.Parse(oValInfoList[i][KCDefine.U_KEY_REPLACE]) != KCDefine.B_VAL_0_INT);
		}

		return m_oValInfoDict;
	}
	#endregion // 함수
}

/** 값 테이블 - 접근 */
public partial class CValTable : CSingleton<CValTable> {
	#region 함수
	/** 정수를 반환한다 */
	public long GetInt(string a_oKey, long a_nDefVal = KCDefine.B_VAL_0_INT) {
		return m_oValInfoDict.TryGetValue(a_oKey, out STValInfo stValInfo) ? (long)stValInfo.m_dmVal : a_nDefVal;
	}

	/** 실수를 반환한다 */
	public double GetReal(string a_oKey, double a_dblDefVal = KCDefine.B_VAL_0_REAL) {
		return m_oValInfoDict.TryGetValue(a_oKey, out STValInfo stValInfo) ? (double)stValInfo.m_dmVal : a_dblDefVal;
	}
	#endregion // 함수
}

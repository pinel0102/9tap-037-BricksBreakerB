using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** 지역화 정보 테이블 */
public partial class CLocalizeInfoTable : CScriptableObj<CLocalizeInfoTable> {
	#region 변수
	[Header("=====> Localize Info <=====")]
	[SerializeField] private List<STLocalizeInfo> m_oLocalizeInfoList = new List<STLocalizeInfo>();
	#endregion // 변수

	#region 프로퍼티
	public List<STLocalizeInfo> LocalizeInfoList => m_oLocalizeInfoList;
	#endregion // 프로퍼티

	#region 함수
	/** 지역화 정보를 반환한다 */
	public STLocalizeInfo GetLocalizeInfo(string a_oCountryCode, SystemLanguage a_eSystemLanguage) {
		bool bIsValid = this.TryGetLocalizeInfo(a_oCountryCode, a_eSystemLanguage, out STLocalizeInfo stLocalizeInfo);
		CAccess.Assert(bIsValid);

		return stLocalizeInfo;
	}

	/** 폰트 세트 정보를 반환한다 */
	public STFontSetInfo GetFontSetInfo(EFontSet a_eFontSet) {
		bool bIsValid = this.TryGetFontSetInfo(CCommonAppInfoStorage.Inst.CountryCode, CCommonAppInfoStorage.Inst.SystemLanguage, a_eFontSet, out STFontSetInfo stFontSetInfo);
		return bIsValid ? stFontSetInfo : this.GetFontSetInfo(CCommonAppInfoStorage.Inst.CountryCode, KCDefine.B_DEF_LANGUAGE, a_eFontSet);
	}

	/** 폰트 세트 정보를 반환한다 */
	public STFontSetInfo GetFontSetInfo(string a_oCountryCode, SystemLanguage a_eSystemLanguage, EFontSet a_eFontSet) {
		bool bIsValid = this.TryGetFontSetInfo(a_oCountryCode, a_eSystemLanguage, a_eFontSet, out STFontSetInfo stFontSetInfo);
		CAccess.Assert(bIsValid);

		return stFontSetInfo;
	}

	/** 지역화 정보를 반환한다 */
	public bool TryGetLocalizeInfo(string a_oCountryCode, SystemLanguage a_eSystemLanguage, out STLocalizeInfo a_stOutLocalizeInfo) {
		int nIdx01 = m_oLocalizeInfoList.FindIndex((a_stLocalizeInfo) => a_stLocalizeInfo.m_eSystemLanguage == a_eSystemLanguage);
		int nIdx02 = m_oLocalizeInfoList.FindIndex((a_stLocalizeInfo) => a_stLocalizeInfo.m_oCountryCode.ExIsValid() && a_stLocalizeInfo.m_oCountryCode.Equals(a_oCountryCode));

		a_stOutLocalizeInfo = m_oLocalizeInfoList.ExIsValidIdx(nIdx01) ? m_oLocalizeInfoList[nIdx01] : m_oLocalizeInfoList.ExIsValidIdx(nIdx02) ? m_oLocalizeInfoList[nIdx02] : STLocalizeInfo.INVALID;
		return m_oLocalizeInfoList.ExIsValidIdx(nIdx01) || m_oLocalizeInfoList.ExIsValidIdx(nIdx02);
	}

	/** 폰트 세트 정보를 반환한다 */
	public bool TryGetFontSetInfo(string a_oCountryCode, SystemLanguage a_eSystemLanguage, EFontSet a_eFontSet, out STFontSetInfo a_stOutFontSetInfo) {
		// 지역화 정보가 존재 할 경우
		if(this.TryGetLocalizeInfo(a_oCountryCode, a_eSystemLanguage, out STLocalizeInfo stLocalizeInfo)) {
			int nIdx = stLocalizeInfo.m_oFontSetInfoList.FindIndex((a_stFontSetInfo) => a_stFontSetInfo.m_eSet == a_eFontSet);
			a_stOutFontSetInfo = stLocalizeInfo.m_oFontSetInfoList.ExIsValidIdx(nIdx) ? stLocalizeInfo.m_oFontSetInfoList[nIdx] : STFontSetInfo.INVALID;

			return stLocalizeInfo.m_oFontSetInfoList.ExIsValidIdx(nIdx);
		}

		a_stOutFontSetInfo = STFontSetInfo.INVALID;
		return false;
	}
	#endregion // 함수

	#region 조건부 함수
#if UNITY_EDITOR
	/** 지역화 정보 변경한다 */
	public void SetLocalizeInfos(List<STLocalizeInfo> a_oLocalizeInfoList) {
		m_oLocalizeInfoList = a_oLocalizeInfoList;
	}
#endif // #if UNITY_EDITOR
	#endregion // 조건부 함수
}

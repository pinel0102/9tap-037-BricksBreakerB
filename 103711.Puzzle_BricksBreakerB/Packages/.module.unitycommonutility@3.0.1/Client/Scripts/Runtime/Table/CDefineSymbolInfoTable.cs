using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** 전처리기 심볼 정보 테이블 */
public partial class CDefineSymbolInfoTable : CScriptableObj<CDefineSymbolInfoTable> {
	#region 변수
	private static readonly List<string> m_oModuleVerDefineSymbolList = new List<string>() {
		"MODULE_VER_2_0_0_OR_NEWER",
		"MODULE_VER_2_1_0_OR_NEWER",
		"MODULE_VER_2_2_0_OR_NEWER",
		"MODULE_VER_2_3_0_OR_NEWER",
		"MODULE_VER_2_4_0_OR_NEWER",
		"MODULE_VER_2_5_0_OR_NEWER",
		"MODULE_VER_2_6_0_OR_NEWER",
		"MODULE_VER_2_7_0_OR_NEWER",
		"MODULE_VER_2_8_0_OR_NEWER",
		"MODULE_VER_3_0_0_OR_NEWER"
	};

	[Header("=====> Common Define Symbol <=====")]
	[SerializeField] private List<string> m_oCommonDefineSymbolList = new List<string>();
	[SerializeField] private List<string> m_oSubCommonDefineSymbolList = new List<string>();

	[Header("=====> iOS Define Symbol <=====")]
	[SerializeField] private List<string> m_oiOSAppleDefineSymbolList = new List<string>();

	[Header("=====> Android Define Symbol <=====")]
	[SerializeField] private List<string> m_oAndroidDefineSymbolList = new List<string>();
	[SerializeField] private List<string> m_oAndroidGoogleDefineSymbolList = new List<string>();
	[SerializeField] private List<string> m_oAndroidAmazonDefineSymbolList = new List<string>();

	[Header("=====> Standalone Define Symbol <=====")]
	[SerializeField] private List<string> m_oStandaloneDefineSymbolList = new List<string>();
	[SerializeField] private List<string> m_oStandaloneMacSteamDefineSymbolList = new List<string>();
	[SerializeField] private List<string> m_oStandaloneWndsSteamDefineSymbolList = new List<string>();
	#endregion // 변수

	#region 프로퍼티
	public List<string> iOSDefineSymbolList { get; } = new List<string>();
	public List<string> AndroidDefineSymbolList { get; } = new List<string>();
	public List<string> StandaloneDefineSymbolList { get; } = new List<string>();

#if UNITY_EDITOR
	public List<string> EditorCommonDefineSymbolList => m_oCommonDefineSymbolList;
	public List<string> EditorSubCommonDefineSymbolList => m_oSubCommonDefineSymbolList;

	public List<string> EditoriOSAppleDefineSymbolList => m_oiOSAppleDefineSymbolList;

	public List<string> EditorAndroidDefineSymbolList => m_oAndroidDefineSymbolList;
	public List<string> EditorAndroidGoogleDefineSymbolList => m_oAndroidGoogleDefineSymbolList;
	public List<string> EditorAndroidAmazonDefineSymbolList => m_oAndroidAmazonDefineSymbolList;

	public List<string> EditorStandaloneDefineSymbolList => m_oStandaloneDefineSymbolList;
	public List<string> EditorStandaloneMacSteamDefineSymbolList => m_oStandaloneMacSteamDefineSymbolList;
	public List<string> EditorStandaloneWndsSteamDefineSymbolList => m_oStandaloneWndsSteamDefineSymbolList;
#endif // #if UNITY_EDITOR
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		var oDefineSymbolListContainer = new List<List<string>>() {
			m_oCommonDefineSymbolList, m_oSubCommonDefineSymbolList, CDefineSymbolInfoTable.m_oModuleVerDefineSymbolList
		};

		this.iOSDefineSymbolList.ExAddVals(m_oiOSAppleDefineSymbolList);
		this.AndroidDefineSymbolList.ExAddVals(m_oAndroidDefineSymbolList);
		this.StandaloneDefineSymbolList.ExAddVals(m_oStandaloneDefineSymbolList);

		for(int i = 0; i < oDefineSymbolListContainer.Count; ++i) {
			this.iOSDefineSymbolList.ExAddVals(oDefineSymbolListContainer[i]);
			this.AndroidDefineSymbolList.ExAddVals(oDefineSymbolListContainer[i]);
			this.StandaloneDefineSymbolList.ExAddVals(oDefineSymbolListContainer[i]);
		}

#if ANDROID_AMAZON_PLATFORM
		this.AndroidDefineSymbolList.ExAddVals(m_oAndroidAmazonDefineSymbolList);
#else
		this.AndroidDefineSymbolList.ExAddVals(m_oAndroidGoogleDefineSymbolList);
#endif // #if ANDROID_AMAZON_PLATFORM

#if STANDALONE_WNDS_STEAM_PLATFORM
		this.StandaloneDefineSymbolList.ExAddVals(m_oStandaloneWndsSteamDefineSymbolList);
#else
		this.StandaloneDefineSymbolList.ExAddVals(m_oStandaloneMacSteamDefineSymbolList);
#endif // #if STANDALONE_MAC_STEAM_PLATFORM
	}
	#endregion // 함수

	#region 조건부 함수
#if UNITY_EDITOR
	/** 공용 전처리기 심볼을 변경한다 */
	public void SetCommonDefineSymbols(List<string> a_oDefineSymbolList) {
		m_oCommonDefineSymbolList = a_oDefineSymbolList;
	}

	/** 서브 공용 전처리기 심볼을 변경한다 */
	public void SetSubCommonDefineSymbols(List<string> a_oDefineSymbolList) {
		m_oSubCommonDefineSymbolList = a_oDefineSymbolList;
	}

	/** 독립 플랫폼 전처리기 심볼을 변경한다 */
	public void SetStandaloneDefineSymbols(List<string> a_oDefineSymbolList) {
		m_oStandaloneDefineSymbolList = a_oDefineSymbolList;
	}
#endif // #if UNITY_EDITOR
	#endregion // 조건부 함수
}

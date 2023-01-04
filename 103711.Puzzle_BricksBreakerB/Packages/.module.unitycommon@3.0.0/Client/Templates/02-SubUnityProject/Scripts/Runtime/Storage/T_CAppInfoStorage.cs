#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.IO;
using MessagePack;

/** 앱 정보 */
[MessagePackObject]
[System.Serializable]
public partial class CAppInfo : CBaseInfo {
#region 변수
	[Key(141)] public Dictionary<string, string> m_oTableVerDict = new Dictionary<string, string>();
	[IgnoreMember][System.NonSerialized] public Dictionary<string, System.Version> m_oTableSysVerDict = new Dictionary<string, System.Version>();
#endregion // 변수

#region 상수
#if ADS_MODULE_ENABLE
	private const string KEY_REWARD_ADS_WATCH_TIMES = "RewardAdsWatchTimes";
	private const string KEY_FULLSCREEN_ADS_WATCH_TIMES = "FullscreenAdsWatchTimes";
#endif // #if ADS_MODULE_ENABLE
#endregion // 상수

#region 프로퍼티
#if ADS_MODULE_ENABLE
	[IgnoreMember]
	public int RewardAdsWatchTimes {
		get { return int.Parse(m_oStrDict.GetValueOrDefault(KEY_REWARD_ADS_WATCH_TIMES, KCDefine.B_STR_0_INT)); }
		set { m_oStrDict.ExReplaceVal(KEY_REWARD_ADS_WATCH_TIMES, $"{value}"); }
	}

	[IgnoreMember]
	public int FullscreenAdsWatchTimes {
		get { return int.Parse(m_oStrDict.GetValueOrDefault(KEY_FULLSCREEN_ADS_WATCH_TIMES, KCDefine.B_STR_0_INT)); }
		set { m_oStrDict.ExReplaceVal(KEY_FULLSCREEN_ADS_WATCH_TIMES, $"{value}"); }
	}
#endif // #if ADS_MODULE_ENABLE
#endregion // 프로퍼티

#region IMessagePackSerializationCallbackReceiver
	/** 직렬화 될 경우 */
	public override void OnBeforeSerialize() {
		base.OnBeforeSerialize();

		foreach(var stKeyVal in m_oTableSysVerDict) {
			m_oTableVerDict.ExReplaceVal(stKeyVal.Key, stKeyVal.Value.ToString(KCDefine.B_VAL_3_INT));
		}
	}

	/** 역직렬화 되었을 경우 */
	public override void OnAfterDeserialize() {
		base.OnAfterDeserialize();

		m_oTableVerDict = m_oTableVerDict ?? new Dictionary<string, string>();
		m_oTableSysVerDict = m_oTableSysVerDict ?? new Dictionary<string, System.Version>();

		foreach(var stKeyVal in m_oTableVerDict) {
			m_oTableSysVerDict.TryAdd(stKeyVal.Key, System.Version.Parse(stKeyVal.Value));
		}

		// 버전이 다를 경우
		if(this.Ver.CompareTo(KDefine.G_VER_APP_INFO) < KCDefine.B_COMPARE_EQUALS) {
			// Do Something
		}
	}
#endregion // IMessagePackSerializationCallbackReceiver

#region 함수
	/** 생성자 */
	public CAppInfo() : base(KDefine.G_VER_APP_INFO) {
		// Do Something
	}
#endregion // 함수
}

/** 앱 정보 저장소 */
public partial class CAppInfoStorage : CSingleton<CAppInfoStorage> {
#region 프로퍼티
	public bool IsIgnoreUpdate { get; private set; } = false;
	public bool IsCloseAgreePopup { get; private set; } = false;

	public CAppInfo AppInfo { get; private set; } = new CAppInfo();

#if ADS_MODULE_ENABLE
	public int AdsSkipTimes { get; private set; } = KCDefine.B_VAL_0_INT;
	public System.DateTime PrevRewardAdsTime { get; private set; } = System.DateTime.Now;
	public System.DateTime PrevFullscreenAdsTime { get; private set; } = System.DateTime.Now;

	public bool IsEnableShowFullscreenAds {
		get {
			double dblAdsDelay = CValTable.Inst.GetReal(KCDefine.VT_KEY_DELAY_ADS);
			double dblAdsDeltaTime = CValTable.Inst.GetReal(KCDefine.VT_KEY_DELTA_T_ADS);

			double dblDeltaTime01 = System.DateTime.Now.ExGetDeltaTime(CAppInfoStorage.Inst.PrevRewardAdsTime);
			double dblDeltaTime02 = System.DateTime.Now.ExGetDeltaTime(CAppInfoStorage.Inst.PrevFullscreenAdsTime);

			bool bIsEnable = dblDeltaTime01.ExIsGreateEquals(dblAdsDeltaTime) && dblDeltaTime02.ExIsGreateEquals(dblAdsDelay);
			return bIsEnable && this.AdsSkipTimes >= KDefine.G_MAX_TIMES_ADS_SKIP && CGameInfoStorage.Inst.GetCharacterGameInfo(CGameInfoStorage.Inst.PlayCharacterID).m_oLevelClearInfoDict.Count >= KDefine.G_MAX_NUM_ADS_SKIP_CLEAR_INFOS;
		}
	}
	
	public bool IsEnableUpdateAdsSkipTimes => true;
#endif // #if ADS_MODULE_ENABLE
#endregion // 프로퍼티

#region 함수
	/** 앱 정보를 로드한다 */
	public CAppInfo LoadAppInfo() {
		return this.LoadAppInfo(KDefine.G_DATA_P_APP_INFO);
	}

	/** 앱 정보를 로드한다 */
	public CAppInfo LoadAppInfo(string a_oFilePath) {
		// 파일이 존재 할 경우
		if(File.Exists(a_oFilePath)) {
			this.AppInfo = CFunc.ReadMsgPackObj<CAppInfo>(a_oFilePath, true);
			CAccess.Assert(this.AppInfo != null);
		}

		return this.AppInfo;
	}

	/** 앱 정보를 저장한다 */
	public void SaveAppInfo() {
		this.SaveAppInfo(KDefine.G_DATA_P_APP_INFO);
	}

	/** 앱 정보를 저장한다 */
	public void SaveAppInfo(string a_oFilePath) {
		CFunc.WriteMsgPackObj(a_oFilePath, this.AppInfo, true);
	}
#endregion // 함수

#region 접근자 함수
	/** 업데이트 무시 여부를 변경한다 */
	public void SetIgnoreUpdate(bool a_bIsIgnore) {
		this.IsIgnoreUpdate = a_bIsIgnore;
	}

	/** 동의 팝업 닫힘 여부를 변경한다 */
	public void SetCloseAgreePopup(bool a_bIsClose) {
		this.IsCloseAgreePopup = a_bIsClose;
	}
#endregion // 접근자 함수

#region 조건부 접근자 함수
#if ADS_MODULE_ENABLE
	/** 광고 스킵 횟수를 변경한다 */
	public void SetAdsSkipTimes(int a_nTimes) {
		this.AdsSkipTimes = a_nTimes;
	}

	/** 이전 보상 광고 시간을 변경한다 */
	public void SetPrevRewardAdsTime(System.DateTime a_stTime) {
		this.PrevRewardAdsTime = a_stTime;
	}

	/** 이전 전면 광고 시간을 변경한다 */
	public void SetPrevFullscreenAdsTime(System.DateTime a_stTime) {
		this.PrevFullscreenAdsTime = a_stTime;
	}
#endif // #if ADS_MODULE_ENABLE
#endregion // 조건부 접근자 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

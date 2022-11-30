using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using TMPro;

/** 결과 팝업 */
public partial class CResultPopup : CSubPopup {
#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.SetIgnoreNavStackEvent(true);

		// 객체를 설정한다
		CFunc.SetupObjs(new List<(EKey, string, GameObject)>() {
			(EKey.CLEAR_UIS, $"{EKey.CLEAR_UIS}", this.Contents),
			(EKey.CLEAR_FAIL_UIS, $"{EKey.CLEAR_FAIL_UIS}", this.Contents)
		}, m_oUIsDict);

		// 텍스트를 설정한다
		CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
			(EKey.RECORD_TEXT, $"{EKey.RECORD_TEXT}", this.Contents),
			(EKey.BEST_RECORD_TEXT, $"{EKey.BEST_RECORD_TEXT}", this.Contents)
		}, m_oTextDict);

		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
			(KCDefine.U_OBJ_N_NEXT_BTN, this.Contents, this.OnTouchNextBtn),
			(KCDefine.U_OBJ_N_RETRY_BTN, this.Contents, this.OnTouchRetryBtn),
			(KCDefine.U_OBJ_N_LEAVE_BTN, this.Contents, this.OnTouchLeaveBtn)
		}, false);

#region 추가
		this.SubSetupAwake();
#endregion // 추가
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init();
		this.Params = a_stParams;

#region 추가
		this.SubInit();
#endregion // 추가
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {
		var oClearLevelInfo = Access.GetLevelClearInfo(CGameInfoStorage.Inst.PlayCharacterID, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID01, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID02, CGameInfoStorage.Inst.PlayEpisodeInfo.m_stIDInfo.m_nID03, false);

		// 객체를 갱신한다
		m_oUIsDict.GetValueOrDefault(EKey.CLEAR_UIS)?.SetActive(this.Params.m_stRecordInfo.m_bIsSuccess);
		m_oUIsDict.GetValueOrDefault(EKey.CLEAR_FAIL_UIS)?.SetActive(!this.Params.m_stRecordInfo.m_bIsSuccess);

		// 텍스트를 갱신한다
		m_oTextDict.GetValueOrDefault(EKey.RECORD_TEXT)?.ExSetText($"{this.Params.m_stRecordInfo.m_nIntRecord}", EFontSet._1, false);
		m_oTextDict.GetValueOrDefault(EKey.BEST_RECORD_TEXT)?.ExSetText((oClearLevelInfo != null) ? $"{oClearLevelInfo.m_stBestRecordInfo.m_nIntRecord}" : string.Empty, EFontSet._1, false);

#region 추가
		this.SubUpdateUIsState();
#endregion // 추가
	}
#endregion // 함수
}

/** 서브 결과 팝업 */
public partial class CResultPopup : CSubPopup {
	/** 서브 식별자 */
	private enum ESubKey {
		NONE = -1,
		[HideInInspector] MAX_VAL
	}

#region 변수

#endregion // 변수

#region 프로퍼티

#endregion // 프로퍼티

#region 함수
	/** 팝업을 설정한다 */
	private void SubSetupAwake() {
		// Do Something
	}

	/** 초기화한다 */
	private void SubInit() {
		// Do Something
	}

	/** UI 상태를 갱신한다 */
	private void SubUpdateUIsState() {
		// Do Something
	}
#endregion // 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

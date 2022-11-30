using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.Linq;
using TMPro;

/** 보상 획득 팝업 */
public partial class CRewardAcquirePopup : CSubPopup {
#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		this.SetIgnoreAni(true);
		this.SetIgnoreNavStackEvent(true);

		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
			(EKey.ADS_BTN, $"{EKey.ADS_BTN}", this.Contents, this.OnTouchAdsBtn),
			(EKey.ACQUIRE_BTN, $"{EKey.ACQUIRE_BTN}", this.Contents, this.OnTouchAcquireBtn)
		}, m_oBtnDict);

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
		var oRewardTargetInfoKeyList = this.Params.m_oRewardTargetInfoDict.Keys.ToList();

		// 보상 아이템 UI 상태를 갱신한다
		for(int i = 0; i < m_oItemUIsList.Count; ++i) {
			m_oItemUIsList[i].SetActive(i < oRewardTargetInfoKeyList.Count);
			
			// 보상 정보가 존재 할 경우
			if(i < oRewardTargetInfoKeyList.Count) {
				ulong nUniqueTargetInfoID = oRewardTargetInfoKeyList[i];
				this.UpdateItemUIsState(m_oItemUIsList[i], this.Params.m_oRewardTargetInfoDict.GetValueOrDefault(nUniqueTargetInfoID));
			}
		}

#region 추가
		this.SubUpdateUIsState();
#endregion // 추가
	}
#endregion // 함수
}

/** 서브 보상 획득 팝업 */
public partial class CRewardAcquirePopup : CSubPopup {
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
	
	/** 보상 아이템 UI 상태를 갱신한다 */
	private void UpdateItemUIsState(GameObject a_oItemUIs, STTargetInfo a_stTargetInfo) {
		var oNumText = a_oItemUIs.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_NUM_TEXT);
		oNumText?.ExSetText(string.Format(KCDefine.B_TEXT_FMT_CROSS, a_stTargetInfo.m_stValInfo01.m_dmVal), EFontSet._1, false);
	}
#endregion // 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

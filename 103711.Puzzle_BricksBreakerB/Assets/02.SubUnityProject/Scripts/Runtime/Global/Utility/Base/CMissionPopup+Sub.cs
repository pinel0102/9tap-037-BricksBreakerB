using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 미션 팝업 */
public abstract partial class CMissionPopup : CSubPopup {
#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

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
		// 미션 UI 상태를 갱신한다
		for(int i = 0; i < m_oMissionUIsList.Count; ++i) {
			this.UpdateMissionUIsState(m_oMissionUIsList[i], this.Params.m_oMissionInfoList[i]);
		}

#region 추가
		this.SubUpdateUIsState();
#endregion // 추가
	}
#endregion // 함수
}

/** 서브 미션 팝업 */
public abstract partial class CMissionPopup : CSubPopup {
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

	/** 미션 UI 상태를 갱신한다 */
	private void UpdateMissionUIsState(GameObject a_oMissionUIs, STMissionInfo a_stMissionInfo) {
		// Do Something
	}
#endregion // 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

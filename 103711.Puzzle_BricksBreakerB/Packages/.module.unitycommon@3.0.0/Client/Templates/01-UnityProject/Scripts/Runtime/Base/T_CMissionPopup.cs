#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 미션 팝업 */
public abstract partial class CMissionPopup : CSubPopup {
	/** 매개 변수 */
	public struct STParams {
		public List<STMissionInfo> m_oMissionInfoList;
	}

#region 변수
	/** =====> 객체 <===== */
	[SerializeField] private List<GameObject> m_oMissionUIsList = new List<GameObject>();
#endregion // 변수

#region 프로퍼티
	public STParams Params { get; private set; }
#endregion // 프로퍼티

#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.SubSetupAwake();
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init();
		this.Params = a_stParams;

		this.SubInit();
	}

	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {
		// 미션 UI 상태를 갱신한다
		for(int i = 0; i < m_oMissionUIsList.Count; ++i) {
			this.UpdateMissionUIsState(m_oMissionUIsList[i], this.Params.m_oMissionInfoList[i]);
		}

		this.SubUpdateUIsState();
	}
#endregion // 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

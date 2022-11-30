using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** 내비게이션 스택 관리자 */
public partial class CNavStackManager : CSingleton<CNavStackManager> {
	#region 프로퍼티
	private CListWrapper<STComponentInfo> m_oComponentInfoListWrapper = new CListWrapper<STComponentInfo>();
	#endregion // 프로퍼티

	#region 함수
	/** 상태를 리셋한다 */
	public override void Reset() {
		base.Reset();
		m_oComponentInfoListWrapper.m_oList01.Clear();
	}

	/** 제거 되었을 경우 */
	public override void OnDestroy() {
		base.OnDestroy();

		try {
			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				CSceneManager.SetNavStackManager(null);
			}
		} catch(System.Exception oException) {
			CFunc.ShowLogWarning($"CNavStackManager.OnDestroy Exception: {oException.Message}");
		}
	}

	/** 컴포넌트를 추가한다 */
	public void AddComponent(CComponent a_oComponent) {
		int nID = a_oComponent.GetInstanceID();
		int nIdx = m_oComponentInfoListWrapper.m_oList01.FindIndex((a_stComponentInfo) => nID == a_stComponentInfo.m_nID);

		// 컴포넌트가 없을 경우
		if(!m_oComponentInfoListWrapper.m_oList01.ExIsValidIdx(nIdx)) {
			m_oComponentInfoListWrapper.m_oList01.Add(new STComponentInfo() {
				m_nID = nID, m_oComponent = a_oComponent
			});

			a_oComponent.SetNavStackCallback(this.OnReceiveNavStackCallback);
			a_oComponent.OnReceiveNavStackEvent(ENavStackEvent.TOP);
		}
	}

	/** 컴포넌트를 제거한다 */
	public void RemoveComponent(CComponent a_oComponent) {
		int nID = a_oComponent.GetInstanceID();
		int nIdx = m_oComponentInfoListWrapper.m_oList01.FindIndex((a_stComponentInfo) => nID == a_stComponentInfo.m_nID);

		// 컴포넌트가 존재 할 경우
		if(m_oComponentInfoListWrapper.m_oList01.ExIsValidIdx(nIdx)) {
			for(int i = nIdx; i < m_oComponentInfoListWrapper.m_oList01.Count; ++i) {
				var oComponent = m_oComponentInfoListWrapper.m_oList01[i].m_oComponent as CComponent;
				oComponent.SetNavStackCallback(null);

				// 이벤트 전송이 가능 할 경우
				if(!oComponent.IsDestroy) {
					oComponent.OnReceiveNavStackEvent(ENavStackEvent.REMOVE);
				}
			}

			m_oComponentInfoListWrapper.m_oList01.RemoveRange(nIdx, m_oComponentInfoListWrapper.m_oList01.Count - nIdx);

			// 컴포넌트 정보가 존재 할 경우
			if(m_oComponentInfoListWrapper.m_oList01.ExIsValid()) {
				(m_oComponentInfoListWrapper.m_oList01.LastOrDefault().m_oComponent as CComponent).OnReceiveNavStackEvent(ENavStackEvent.TOP);
			}
		}
	}

	/** 내비게이션 스택 이벤트를 전송한다 */
	public void SendNavStackEvent(ENavStackEvent a_eEvent) {
		// 이벤트 전송이 가능 할 경우
		if(m_oComponentInfoListWrapper.m_oList01.ExIsValid()) {
			int nIdx = m_oComponentInfoListWrapper.m_oList01.Count - KCDefine.B_VAL_1_INT;
			(m_oComponentInfoListWrapper.m_oList01[nIdx].m_oComponent as CComponent).OnReceiveNavStackEvent(a_eEvent);
		}
	}

	/** 내비게이션 스택 콜백을 수신했을 경우 */
	private void OnReceiveNavStackCallback(CComponent a_oSender) {
		this.RemoveComponent(a_oSender);
	}
	#endregion // 함수
}

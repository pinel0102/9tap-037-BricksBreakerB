using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** 위치 보정자 */
public partial class CPosCorrector : CComponent {
	#region 변수
	[SerializeField] private Vector3 m_stOffset = Vector3.zero;

	/** =====> 객체 <===== */
	[SerializeField] private GameObject m_oTarget = null;
	#endregion // 변수

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		CScheduleManager.Inst.AddComponent(this);
	}

	/** 상태를 갱신한다 */
	public override void OnLateUpdate(float a_fDeltaTime) {
		base.OnLateUpdate(a_fDeltaTime);

		// 앱이 실행 중 일 경우
		if(CSceneManager.IsAppRunning) {
			this.transform.position = m_oTarget.transform.position + m_stOffset;
		}
	}

	/** 간격을 변경한다 */
	public void SetOffset(Vector3 a_stOffset) {
		m_stOffset = a_stOffset;
	}

	/** 타겟을 변경한다 */
	public void SetTarget(GameObject a_oTarget) {
		m_oTarget = a_oTarget;
	}
	#endregion // 함수
}

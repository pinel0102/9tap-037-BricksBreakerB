using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** 디바이스 정보 테이블 */
public partial class CDeviceInfoTable : CScriptableObj<CDeviceInfoTable> {
	#region 변수
	[Header("=====> Device Info <=====")]
	[SerializeField] private STDeviceInfo m_stDeviceInfo;
	#endregion // 변수

	#region 프로퍼티
	public STDeviceInfo DeviceInfo => m_stDeviceInfo;
	#endregion // 프로퍼티

	#region 조건부 함수
#if UNITY_EDITOR
	/** 디바이스 정보를 변경한다 */
	public void SetDeviceInfo(STDeviceInfo a_stDeviceInfo) {
		m_stDeviceInfo = a_stDeviceInfo;
	}
#endif // #if UNITY_EDITOR
	#endregion // 조건부 함수
}

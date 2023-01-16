using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** 디바이스 메세지 수신자 */
public partial class CDeviceMsgReceiver : CSingleton<CDeviceMsgReceiver> {
	#region 변수
	private Dictionary<string, System.Action<string, string>> m_oCallbackDict = new Dictionary<string, System.Action<string, string>>();
	#endregion // 변수

	#region 함수
	/** 콜백을 추가한다 */
	public void AddCallback(string a_oKey, System.Action<string, string> a_oCallback) {
		m_oCallbackDict.ExReplaceVal(a_oKey, a_oCallback);
	}

	/** 콜백을 제거한다 */
	public void RemoveCallback(string a_oKey) {
		m_oCallbackDict.ExRemoveVal(a_oKey);
	}

	/** 디바이스 메세지를 수신했을 경우 */
	private void OnReceiveDeviceMsg(string a_oMsg) {
		CFunc.ShowLog($"CDeviceMsgReceiver.OnReceiveDeviceMsg: {a_oMsg}", KCDefine.B_LOG_COLOR_PLUGIN);
		var oJSONNode = SimpleJSON.JSON.Parse(a_oMsg);

		CScheduleManager.Inst.AddCallback(string.Format(KCDefine.U_KEY_FMT_DEVICE_MR_HANDLE_MSG_CALLBACK, oJSONNode[KCDefine.U_KEY_DEVICE_CMD]), () => {
			// 콜백이 존재 할 경우
			if(m_oCallbackDict.TryGetValue(oJSONNode[KCDefine.U_KEY_DEVICE_CMD], out System.Action<string, string> oCallback)) {
				try {
					this.RemoveCallback(oJSONNode[KCDefine.U_KEY_DEVICE_CMD]);
				} finally {
					CFunc.Invoke(ref oCallback, oJSONNode[KCDefine.U_KEY_DEVICE_CMD], oJSONNode[KCDefine.U_KEY_DEVICE_MSG]);
				}
			}
		});
	}
	#endregion // 함수
}

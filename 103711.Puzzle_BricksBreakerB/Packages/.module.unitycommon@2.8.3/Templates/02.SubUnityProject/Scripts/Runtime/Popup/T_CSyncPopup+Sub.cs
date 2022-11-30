#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 동기화 팝업 */
public partial class CSyncPopup : CSubPopup {
#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 객체를 설정한다
		CFunc.SetupObjs(new List<(EKey, string, GameObject)>() {
			(EKey.LOGIN_UIS, $"{EKey.LOGIN_UIS}", this.Contents),
			(EKey.LOGOUT_UIS, $"{EKey.LOGIN_UIS}", this.Contents)
		}, m_oUIsDict);

		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
			(KCDefine.U_OBJ_N_LOGIN_BTN, this.Contents, this.OnTouchLoginBtn),
			(KCDefine.U_OBJ_N_LOGOUT_BTN, this.Contents, this.OnTouchLogoutBtn),
			(KCDefine.U_OBJ_N_LOAD_BTN, this.Contents, this.OnTouchLoadBtn),
			(KCDefine.U_OBJ_N_SAVE_BTN, this.Contents, this.OnTouchSaveBtn)
		}, false);

#region 추가
		this.SubSetupAwake();
#endregion // 추가
	}

	/** 초기화 */
	public override void Init() {
		base.Init();

#region 추가
		this.SubInit();
#endregion // 추가
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {
		// 객체를 갱신한다 {
#if FIREBASE_MODULE_ENABLE
		m_oUIsDict.GetValueOrDefault(EKey.LOGIN_UIS)?.SetActive(CFirebaseManager.Inst.IsLogin);
		m_oUIsDict.GetValueOrDefault(EKey.LOGOUT_UIS)?.SetActive(!CFirebaseManager.Inst.IsLogin);
#endif // #if FIREBASE_MODULE_ENABLE
		// 객체를 갱신한다 }

#region 추가
		this.SubUpdateUIsState();
#endregion // 추가
	}
#endregion // 함수
}

/** 서브 동기화 팝업 */
public partial class CSyncPopup : CSubPopup {
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

	/** 초기화 */
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
#endif // #if SCRIPT_TEMPLATE_ONLY

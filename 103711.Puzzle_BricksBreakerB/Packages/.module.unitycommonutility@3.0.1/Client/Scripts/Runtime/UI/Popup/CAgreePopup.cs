using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** 약관 동의 팝업 */
public partial class CAgreePopup : CPopup {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		IS_AGREE_PRIVACY,
		IS_AGREE_SERVICES,
		KR_UIS_PRIVACY_TEXT,
		KR_UIS_SERVICES_TEXT,
		KR_UIS_PRIVACY_CHECK_IMG,
		KR_UIS_SERVICES_CHECK_IMG,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		public string m_oPrivacy;
		public string m_oServices;

		public EAgreePopup m_eAgreePopup;
	}

	#region 변수
	private Dictionary<EKey, bool> m_oBoolDict = new Dictionary<EKey, bool>() {
		[EKey.IS_AGREE_PRIVACY] = false, [EKey.IS_AGREE_SERVICES] = false
	};

	/** =====> UI <===== */
	private Dictionary<EKey, Text> m_oTextDict = new Dictionary<EKey, Text>();
	private Dictionary<EKey, Image> m_oImgDict = new Dictionary<EKey, Image>();

	/** =====> 객체 <===== */
	[SerializeField] private GameObject m_oKRUIs = null;
	[SerializeField] private GameObject m_oEUUIs = null;
	[SerializeField] private GameObject m_oKRUIsPrivacyUIs = null;
	[SerializeField] private GameObject m_oKRUIsServicesUIs = null;
	#endregion // 변수

	#region 프로퍼티
	public STParams Params { get; private set; }
	public override float ShowTimeScale => KCDefine.B_VAL_1_REAL;
	public override EAniType AniType => EAniType.NONE;
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		this.SetIgnoreAni(true);
		this.SetIgnoreNavStackEvent(true);

		this.SetupKRUIs();
		this.SetupEUUIs();
	}

	/** 초기화 */
	public override void Start() {
		base.Start();

		m_oKRUIs.SetActive(false);
		m_oEUUIs.SetActive(false);
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init();
		this.Params = a_stParams;
	}

	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
	}

	/** 닫혔을 경우 */
	protected override void OnClose() {
		base.OnClose();

		// 앱이 실행 중 일 경우
		if(CSceneManager.IsRunning && !CSceneManager.IsAppQuit) {
			this.gameObject.ExBroadcastMsg(KCDefine.U_FUNC_N_RESET_ANI, null);
		}
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {
		this.UpdateKRUIsState();
		this.UpdateEUUIsState();

		// 객체를 갱신한다
		m_oKRUIs.gameObject.SetActive(this.Params.m_eAgreePopup == EAgreePopup.KR);
		m_oEUUIs.gameObject.SetActive(this.Params.m_eAgreePopup == EAgreePopup.EU);

		// 약관에 동의했을 경우
		if(m_oBoolDict[EKey.IS_AGREE_PRIVACY] && m_oBoolDict[EKey.IS_AGREE_SERVICES]) {
			this.OnTouchCloseBtn();
		}
	}
	#endregion // 함수

	#region 클래스 함수
	/** 매개 변수를 생성한다 */
	public static STParams MakeParams(string a_oPrivacy, string a_oServices, EAgreePopup a_eAgreePopup) {
		return new STParams() {
			m_oPrivacy = a_oPrivacy,
			m_oServices = a_oServices,
			m_eAgreePopup = a_eAgreePopup
		};
	}
	#endregion // 클래스 함수
}

/** 약관 동의 팝업 - 한국 UI */
public partial class CAgreePopup : CPopup {
	#region 함수
	/** 한국 UI 를 설정한다 */
	private void SetupKRUIs() {
		// 텍스트를 설정한다
		CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
			(EKey.KR_UIS_PRIVACY_TEXT, $"{EKey.KR_UIS_PRIVACY_TEXT}", m_oKRUIsPrivacyUIs),
			(EKey.KR_UIS_SERVICES_TEXT, $"{EKey.KR_UIS_SERVICES_TEXT}", m_oKRUIsServicesUIs)
		}, m_oTextDict);

		// 이미지를 설정한다
		CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
			(EKey.KR_UIS_PRIVACY_CHECK_IMG, $"{EKey.KR_UIS_PRIVACY_CHECK_IMG}", m_oKRUIsPrivacyUIs),
			(EKey.KR_UIS_SERVICES_CHECK_IMG, $"{EKey.KR_UIS_SERVICES_CHECK_IMG}", m_oKRUIsServicesUIs)
		}, m_oImgDict);

		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
			(KCDefine.U_OBJ_N_AGREE_BTN, m_oKRUIsPrivacyUIs, this.OnTouchKRUIsPrivacyBtn),
			(KCDefine.U_OBJ_N_AGREE_BTN, m_oKRUIsServicesUIs, this.OnTouchKRUIsServicesBtn)
		});
	}

	/** 한국 UI 상태를 갱신한다 */
	private void UpdateKRUIsState() {
		// 텍스트를 갱신한다
		m_oTextDict[EKey.KR_UIS_PRIVACY_TEXT].text = this.Params.m_oPrivacy;
		m_oTextDict[EKey.KR_UIS_SERVICES_TEXT].text = this.Params.m_oServices;

		// 이미지를 갱신한다
		m_oImgDict[EKey.KR_UIS_PRIVACY_CHECK_IMG].gameObject.SetActive(m_oBoolDict[EKey.IS_AGREE_PRIVACY]);
		m_oImgDict[EKey.KR_UIS_SERVICES_CHECK_IMG].gameObject.SetActive(m_oBoolDict[EKey.IS_AGREE_SERVICES]);
	}

	/** 한국 UI 개인 정보 버튼을 눌렀을 경우 */
	private void OnTouchKRUIsPrivacyBtn() {
		m_oBoolDict[EKey.IS_AGREE_PRIVACY] = !m_oBoolDict[EKey.IS_AGREE_PRIVACY];
		this.UpdateUIsState();
	}

	/** 한국 UI 서비스 버튼을 눌렀을 경우 */
	private void OnTouchKRUIsServicesBtn() {
		m_oBoolDict[EKey.IS_AGREE_SERVICES] = !m_oBoolDict[EKey.IS_AGREE_SERVICES];
		this.UpdateUIsState();
	}
	#endregion // 함수
}

/** 약관 동의 팝업 - 유럽 연합 UI */
public partial class CAgreePopup : CPopup {
	#region 함수
	/** 유럽 연합 UI 를 설정한다 */
	private void SetupEUUIs() {
		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
			(KCDefine.U_OBJ_N_OK_BTN, m_oEUUIs, this.OnTouchEUUIsOKBtn),
			(KCDefine.U_OBJ_N_PRIVACY_BTN, m_oEUUIs, this.OnTouchEUUIsPrivacyBtn),
			(KCDefine.U_OBJ_N_SERVICES_BTN, m_oEUUIs, this.OnTouchEUUIsServicesBtn)
		});
	}

	/** 유렵 연합 UI 상태를 갱신한다 */
	private void UpdateEUUIsState() {
		// Do Something
	}

	/** 유럽 연합 UI 확인 버튼을 눌렀을 경우 */
	private void OnTouchEUUIsOKBtn() {
		this.OnTouchCloseBtn();
	}

	/** 유럽 연합 UI 개인 정보 URL 버튼을 눌렀을 경우 */
	private void OnTouchEUUIsPrivacyBtn() {
		Application.OpenURL(this.Params.m_oPrivacy);
	}

	/** 유럽 연합 UI 서비스 URL 버튼을 눌렀을 경우 */
	private void OnTouchEUUIsServicesBtn() {
		Application.OpenURL(this.Params.m_oServices);
	}
	#endregion // 함수
}

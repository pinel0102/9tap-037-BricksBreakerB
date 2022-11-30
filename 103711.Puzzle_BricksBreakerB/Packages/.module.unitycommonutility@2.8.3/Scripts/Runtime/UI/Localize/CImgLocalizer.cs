using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** 이미지 지역화 */
public partial class CImgLocalizer : CComponent {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		COUNTRY_CODE,
		SYSTEM_LANGUAGE,
		IMG,
		[HideInInspector] MAX_VAL
	}

	#region 변수
	private Dictionary<EKey, string> m_oStrDict = new Dictionary<EKey, string>();
	private Dictionary<EKey, SystemLanguage> m_oSystemLanguageDict = new Dictionary<EKey, SystemLanguage>();

	[SerializeField] private string m_oBasePath = string.Empty;

	/** =====> UI <===== */
	private Dictionary<EKey, Image> m_oImgDict = new Dictionary<EKey, Image>();
	#endregion // 변수

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 이미지를 설정한다
		CFunc.SetupComponents(new List<(EKey, GameObject)>() {
			(EKey.IMG, this.gameObject)
		}, m_oImgDict);

		m_oStrDict.ExReplaceVal(EKey.COUNTRY_CODE, CCommonAppInfoStorage.Inst.CountryCode);
		m_oSystemLanguageDict.ExReplaceVal(EKey.SYSTEM_LANGUAGE, CCommonAppInfoStorage.Inst.SystemLanguage);
	}

	/** 초기화 */
	public override void Start() {
		base.Start();
		this.SetupImg();
	}

	/** 지역화를 리셋한다 */
	public virtual void ResetLocalize() {
		this.SetupImg();
	}

	/** 이미지를 변경한다 */
	public void SetImg(string a_oBasePath) {
		this.SetImg(a_oBasePath, m_oStrDict.GetValueOrDefault(EKey.COUNTRY_CODE), m_oSystemLanguageDict.GetValueOrDefault(EKey.SYSTEM_LANGUAGE));
	}

	/** 이미지를 변경한다 */
	public void SetImg(string a_oBasePath, string a_oCountryCode, SystemLanguage a_eSystemLanguage) {
		m_oBasePath = a_oBasePath;
		m_oStrDict.ExReplaceVal(EKey.COUNTRY_CODE, a_oCountryCode);
		m_oSystemLanguageDict.ExReplaceVal(EKey.SYSTEM_LANGUAGE, a_eSystemLanguage);

		this.SetupImg();
	}

	/** 이미지를 설정한다 */
	private void SetupImg() {
		string oDefPath = CFactory.MakeLocalizePath(m_oBasePath, $"{KCDefine.B_DEF_LANGUAGE}");
		string oImgPath = CFactory.MakeLocalizePath(m_oBasePath, oDefPath, m_oStrDict.GetValueOrDefault(EKey.COUNTRY_CODE), $"{m_oSystemLanguageDict.GetValueOrDefault(EKey.SYSTEM_LANGUAGE)}");

		m_oImgDict.GetValueOrDefault(EKey.IMG).sprite = CResManager.Inst.GetRes<Sprite>(oImgPath);
	}
	#endregion // 함수
}

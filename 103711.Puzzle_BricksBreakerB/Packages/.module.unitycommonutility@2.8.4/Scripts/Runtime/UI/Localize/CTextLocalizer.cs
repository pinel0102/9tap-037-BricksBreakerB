using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

/** 텍스트 지역화 */
public partial class CTextLocalizer : CComponent {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		TEXT,
		TMP_TEXT,
		[HideInInspector] MAX_VAL
	}

	#region 변수
	[SerializeField] private bool m_bIsIgnoreSetupFont = false;
	[SerializeField] private string m_oKey = string.Empty;
	[SerializeField] private EFontSet m_eFontSet = EFontSet._1;

	/** =====> UI <===== */
	private Dictionary<EKey, Text> m_oTextDict = new Dictionary<EKey, Text>();
	private Dictionary<EKey, TMP_Text> m_oTMPTextDict = new Dictionary<EKey, TMP_Text>();
	#endregion // 변수

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 텍스트를 설정한다 {
		CFunc.SetupComponents(new List<(EKey, GameObject)>() {
			(EKey.TEXT, this.gameObject)
		}, m_oTextDict);

		CFunc.SetupComponents(new List<(EKey, GameObject)>() {
			(EKey.TMP_TEXT, this.gameObject)
		}, m_oTMPTextDict);
		// 텍스트를 설정한다 }
	}

	/** 초기화 */
	public override void Start() {
		base.Start();
		this.SetupText();
	}

	/** 지역화를 리셋한다 */
	public virtual void ResetLocalize() {
		this.SetupText();
	}

	/** 텍스트를 변경한다 */
	public void SetText(string a_oKey) {
		m_oKey = a_oKey;
		this.SetupText();
	}

	/** 폰트 세트를 변경한다 */
	public void SetFontSet(EFontSet a_eSet) {
		m_eFontSet = a_eSet;
		this.SetupText();
	}

	/** 텍스트를 설정한다 */
	private void SetupText() {
		// 폰트 설정 무시 모드 일 경우
		if(m_bIsIgnoreSetupFont) {
			m_oTextDict.GetValueOrDefault(EKey.TEXT)?.ExSetText<Text>(CStrTable.Inst.GetStr(m_oKey), false);
			m_oTMPTextDict.GetValueOrDefault(EKey.TMP_TEXT)?.ExSetText<TMP_Text>(CStrTable.Inst.GetStr(m_oKey), false);
		} else {
			m_oTextDict.GetValueOrDefault(EKey.TEXT)?.ExSetText(CStrTable.Inst.GetStr(m_oKey), CLocalizeInfoTable.Inst.GetFontSetInfo(m_eFontSet), false);
			m_oTMPTextDict.GetValueOrDefault(EKey.TMP_TEXT)?.ExSetText(CStrTable.Inst.GetStr(m_oKey), CLocalizeInfoTable.Inst.GetFontSetInfo(m_eFontSet), false);
		}
	}
	#endregion // 함수
}

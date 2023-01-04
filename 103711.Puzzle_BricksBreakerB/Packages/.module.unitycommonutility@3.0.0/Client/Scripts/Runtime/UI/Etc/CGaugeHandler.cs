using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** 게이지 처리자 */
public partial class CGaugeHandler : CComponent {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		MASK_IMG,
		PERCENT_IMG,
		[HideInInspector] MAX_VAL
	}

	#region 변수
	/** =====> UI <===== */
	private Dictionary<EKey, Image> m_oImgDict = new Dictionary<EKey, Image>();
	#endregion // 변수

	#region 프로퍼티
	public float Percent { get; private set; } = 0.0f;
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 이미지를 설정한다 {
		CFunc.SetupComponents(new List<(EKey, GameObject)>() {
			(EKey.MASK_IMG, this.gameObject)
		}, m_oImgDict);

		CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
			(EKey.PERCENT_IMG, $"{EKey.PERCENT_IMG}", this.gameObject)
		}, m_oImgDict);

		m_oImgDict.GetValueOrDefault(EKey.MASK_IMG).fillMethod = Image.FillMethod.Horizontal;
		m_oImgDict.GetValueOrDefault(EKey.MASK_IMG).gameObject.ExAddComponent<Mask>().showMaskGraphic = false;
		// 이미지를 설정한다 }
	}

	/** 초기화 */
	public override void Start() {
		base.Start();
		this.SetPercent(KCDefine.B_VAL_0_REAL);
	}

	/** 퍼센트를 변경한다 */
	public void SetPercent(float a_fPercent) {
		this.Percent = Mathf.Clamp01(a_fPercent);
		m_oImgDict.GetValueOrDefault(EKey.MASK_IMG).fillAmount = KCDefine.B_VAL_1_REAL;

		m_oImgDict.GetValueOrDefault(EKey.PERCENT_IMG).fillAmount = KCDefine.B_VAL_1_REAL;
		m_oImgDict.GetValueOrDefault(EKey.PERCENT_IMG).rectTransform.pivot = (m_oImgDict.GetValueOrDefault(EKey.MASK_IMG).fillOrigin <= (int)EFillOrigin._1) ? KCDefine.B_ANCHOR_MID_RIGHT : KCDefine.B_ANCHOR_MID_LEFT;
		m_oImgDict.GetValueOrDefault(EKey.PERCENT_IMG).rectTransform.anchorMin = (m_oImgDict.GetValueOrDefault(EKey.MASK_IMG).fillOrigin <= (int)EFillOrigin._1) ? KCDefine.B_ANCHOR_MID_LEFT : KCDefine.B_ANCHOR_MID_RIGHT;
		m_oImgDict.GetValueOrDefault(EKey.PERCENT_IMG).rectTransform.anchorMax = (m_oImgDict.GetValueOrDefault(EKey.MASK_IMG).fillOrigin <= (int)EFillOrigin._1) ? KCDefine.B_ANCHOR_MID_LEFT : KCDefine.B_ANCHOR_MID_RIGHT;
		m_oImgDict.GetValueOrDefault(EKey.PERCENT_IMG).rectTransform.sizeDelta = m_oImgDict.GetValueOrDefault(EKey.MASK_IMG).rectTransform.rect.size;

		this.SetupPercent();
	}

	/** 퍼센트를 설정한다 */
	private void SetupPercent() {
		float fPercent = (m_oImgDict.GetValueOrDefault(EKey.MASK_IMG).fillOrigin <= (int)EFillOrigin._1) ? this.Percent : -this.Percent;
		m_oImgDict.GetValueOrDefault(EKey.PERCENT_IMG).gameObject.ExSetAnchorPosX(m_oImgDict.GetValueOrDefault(EKey.MASK_IMG).rectTransform.rect.width * fPercent);
	}
	#endregion // 함수
}

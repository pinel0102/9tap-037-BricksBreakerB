using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

/** 터치 비율 처리자 */
public partial class CTouchScaler : CComponent, IPointerUpHandler, IPointerDownHandler {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		ORIGIN_SCALE,
		SCALE_ANI,
		[HideInInspector] MAX_VAL
	}

	#region 변수
	private Dictionary<EKey, Vector3> m_oVec3Dict = new Dictionary<EKey, Vector3>();
	private Dictionary<EKey, Tween> m_oAniDict = new Dictionary<EKey, Tween>();

	[SerializeField] private float m_fDuration = KCDefine.U_DURATION_ANI;
	[SerializeField] private float m_fTouchScale = KCDefine.U_SCALE_TOUCH;
	#endregion // 변수

	#region IPointerUpHandler
	/** 터치를 종료했을 경우 */
	public virtual void OnPointerUp(PointerEventData a_oEventData) {
		this.StartReleaseAni();
	}
	#endregion // IPointerUpHandler

	#region IPointerDownHandler
	/** 터치를 시작했을 경우 */
	public virtual void OnPointerDown(PointerEventData a_oEventData) {
		this.StartPressAni();
	}
	#endregion // IPointerDownHandler

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.SetOriginScale(this.transform.localScale);
	}

	/** 제거 되었을 경우 */
	public override void OnDestroy() {
		base.OnDestroy();

		try {
			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				this.ResetAni();
			}
		} catch(System.Exception oException) {
			CFunc.ShowLogWarning($"CTouchScaler.OnDestroy Exception: {oException.Message}");
		}
	}

	/** 애니메이션을 리셋한다 */
	public virtual void ResetAni() {
		m_oAniDict.GetValueOrDefault(EKey.SCALE_ANI)?.Kill();
	}

	/** 애니메이션 시간을 변경한다 */
	public void SetDuration(float a_fDuration) {
		m_fDuration = a_fDuration;
	}

	/** 터치 비율을 변경한다 */
	public void SetTouchScale(float a_fScale) {
		m_fTouchScale = a_fScale;
	}

	/** 원본 비율을 변경한다 */
	public void SetOriginScale(Vector3 a_stScale) {
		m_oVec3Dict.ExReplaceVal(EKey.ORIGIN_SCALE, a_stScale);
	}

	/** 비율을 변경한다 */
	public void SetScale(Vector3 a_stScale, bool a_bIsAni = true) {
		this.ResetAni();

		// 애니메이션 모드 일 경우
		if(!this.IsIgnoreAni && a_bIsAni) {
			m_oAniDict.ExReplaceVal(EKey.SCALE_ANI, this.transform.DOScale(a_stScale, m_fDuration).SetAutoKill().SetEase(KCDefine.U_EASE_LINEAR).SetUpdate(true));
		} else {
			this.transform.localScale = a_stScale;
		}
	}

	/** 누르기 시작 애니메이션을 시작한다 */
	private void StartPressAni() {
		this.SetScale(m_oVec3Dict.GetValueOrDefault(EKey.ORIGIN_SCALE, Vector3.one) * m_fTouchScale);
	}

	/** 누르기 종료 애니메이션을 시작한다 */
	private void StartReleaseAni() {
		this.SetScale(m_oVec3Dict.GetValueOrDefault(EKey.ORIGIN_SCALE, Vector3.one));
	}
	#endregion // 함수
}

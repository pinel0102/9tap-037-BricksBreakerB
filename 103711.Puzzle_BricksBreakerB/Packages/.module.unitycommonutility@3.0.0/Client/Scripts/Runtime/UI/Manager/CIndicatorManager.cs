using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;

/** 인디케이터 관리자 */
public partial class CIndicatorManager : CSingleton<CIndicatorManager> {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		REF_COUNT,
		INDICATOR_ANI,
		INFO_TEXT,
		[HideInInspector] MAX_VAL
	}

	#region 변수
	private Dictionary<EKey, int> m_oIntDict = new Dictionary<EKey, int>();
	private Dictionary<EKey, Tween> m_oAniDict = new Dictionary<EKey, Tween>();
	private Dictionary<EKey, TMP_Text> m_oTextDict = new Dictionary<EKey, TMP_Text>();
	#endregion // 변수

	#region 함수
	/** 제거 되었을 경우 */
	public override void OnDestroy() {
		base.OnDestroy();

		try {
			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				m_oAniDict.GetValueOrDefault(EKey.INDICATOR_ANI)?.Kill();
			}
		} catch(System.Exception oException) {
			CFunc.ShowLogWarning($"CIndicatorManager.OnDestroy Exception: {oException.Message}");
		}
	}

	/** 정보 텍스트를 변경한다 */
	public void SetInfoText(string a_oStr) {
		CLocalizeInfoTable.Inst.TryGetFontSetInfo(string.Empty, SystemLanguage.English, EFontSet._1, out STFontSetInfo stFontSetInfo);
		m_oTextDict.GetValueOrDefault(EKey.INFO_TEXT)?.ExSetText(a_oStr, stFontSetInfo, false);
	}

	/** 인디케이터를 출력한다 */
	public void Show(bool a_bIsResetRefCount = false) {
		int nRefCount = m_oIntDict.GetValueOrDefault(EKey.REF_COUNT) + KCDefine.B_VAL_1_INT;
		m_oIntDict.ExReplaceVal(EKey.REF_COUNT, a_bIsResetRefCount ? KCDefine.B_VAL_1_INT : Mathf.Clamp(nRefCount, KCDefine.B_VAL_0_INT, int.MaxValue));

		// 인디케이터 시작이 가능 할 경우
		if(m_oIntDict.GetValueOrDefault(EKey.REF_COUNT) > KCDefine.B_VAL_0_INT) {
			var oParent = CSceneManager.ScreenTopmostUIs ?? CSceneManager.ActiveSceneTopmostUIs;
			var oTouchResponder = CSceneManager.ShowTouchResponder(oParent, KCDefine.U_OBJ_N_INDICATOR_TOUCH_RESPONDER, KCDefine.U_OBJ_P_INDICATOR_TOUCH_RESPONDER, KCDefine.U_COLOR_INDICATOR_BLIND, null, true);

			// 텍스트를 설정한다 {
			CFunc.SetupComponents(new List<(EKey, string, GameObject, GameObject)>() {
				(EKey.INFO_TEXT, $"{EKey.INFO_TEXT}", oTouchResponder, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_G_INFO_TEXT))
			}, m_oTextDict, false);

			m_oTextDict.GetValueOrDefault(EKey.INFO_TEXT).rectTransform.pivot = KCDefine.B_ANCHOR_UP_CENTER;
			m_oTextDict.GetValueOrDefault(EKey.INFO_TEXT).rectTransform.anchorMin = KCDefine.B_ANCHOR_MID_CENTER;
			m_oTextDict.GetValueOrDefault(EKey.INFO_TEXT).rectTransform.anchorMax = KCDefine.B_ANCHOR_MID_CENTER;
			m_oTextDict.GetValueOrDefault(EKey.INFO_TEXT).rectTransform.anchoredPosition = new Vector3(KCDefine.B_VAL_0_REAL, CSceneManager.CanvasSize.y / -KCDefine.B_VAL_4_REAL, KCDefine.B_VAL_0_REAL);
			// 텍스트를 설정한다 }

			this.SetInfoText(string.Empty);

#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
			CUnityMsgSender.Inst.SendIndicatorMsg(true);
#else
			float fSize = Mathf.Min(CSceneManager.CanvasSize.x, CSceneManager.CanvasSize.y) * KCDefine.U_SCALE_INDICATOR_IMG;

			// 이미지를 설정한다
			var oIndicatorImg = CFactory.CreateCloneObj<Image>(KCDefine.U_OBJ_N_IMG, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_IMG), oTouchResponder);
			oIndicatorImg.ExReset();
			oIndicatorImg.sprite = CResManager.Inst.GetRes<Sprite>(KCDefine.U_IMG_P_INDICATOR);
			oIndicatorImg.rectTransform.sizeDelta = Vector3.one * fSize;

			m_oAniDict.ExAssignVal(EKey.INDICATOR_ANI, oIndicatorImg.transform.DOLocalRotate(new Vector3(KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_0_REAL, -KCDefine.B_ANGLE_360_DEG), KCDefine.B_VAL_1_REAL, RotateMode.LocalAxisAdd).SetAutoKill().SetEase(KCDefine.U_EASE_LINEAR).SetLoops(KCDefine.B_TIMES_INT_INFINITE).SetUpdate(true));
#endif // #if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
		}
	}

	/** 인디케이터를 닫는다 */
	public void Close(bool a_bIsResetRefCount = false) {
		int nRefCount = m_oIntDict.GetValueOrDefault(EKey.REF_COUNT) - KCDefine.B_VAL_1_INT;
		m_oIntDict.ExReplaceVal(EKey.REF_COUNT, a_bIsResetRefCount ? KCDefine.B_VAL_0_INT : Mathf.Clamp(nRefCount, KCDefine.B_VAL_0_INT, int.MaxValue));

		// 인디케이터 정지가 가능 할 경우
		if(m_oIntDict.GetValueOrDefault(EKey.REF_COUNT) <= KCDefine.B_VAL_0_INT) {
#if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)
			CUnityMsgSender.Inst.SendIndicatorMsg(false);
#else
			m_oAniDict.GetValueOrDefault(EKey.INDICATOR_ANI)?.Kill();
#endif // #if !UNITY_EDITOR && (UNITY_IOS || UNITY_ANDROID)

			CSceneManager.CloseTouchResponder(KCDefine.U_OBJ_N_INDICATOR_TOUCH_RESPONDER, KCDefine.U_COLOR_TRANSPARENT, null);
		}
	}
	#endregion // 함수
}

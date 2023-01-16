using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if DEBUG || DEVELOPMENT_BUILD
using UnityEngine.Profiling;
using TMPro;

#if INPUT_SYSTEM_MODULE_ENABLE
using UnityEngine.InputSystem;
#endif // #if INPUT_SYSTEM_MODULE_ENABLE

/** 씬 관리자 */
public abstract partial class CSceneManager : CComponent {
	#region 클래스 변수
	private static int m_nNumFrames = 0;
	private static float m_fFPSInfoSkipTime = 0.0f;
	private static float m_fDebugInfoSkipTime = 0.0f;

	private static Dictionary<EKey, System.Text.StringBuilder> m_oStrBuilderDict = new Dictionary<EKey, System.Text.StringBuilder>() {
		[EKey.STATIC_DEBUG_STR_BUILDER] = new System.Text.StringBuilder(),
		[EKey.DYNAMIC_DEBUG_STR_BUILDER] = new System.Text.StringBuilder(),
		[EKey.EXTRA_STATIC_DEBUG_STR_BUILDER] = new System.Text.StringBuilder(),
		[EKey.EXTRA_DYNAMIC_DEBUG_STR_BUILDER] = new System.Text.StringBuilder()
	};
	#endregion // 클래스 변수

	#region 클래스 프로퍼티
	/** =====> UI <===== */
	public static TMP_Text ScreenFPSText { get; private set; } = null;
	public static TMP_Text ScreenFrameTimeText { get; private set; } = null;
	public static TMP_Text ScreenDeviceInfoText { get; private set; } = null;

	public static TMP_Text ScreenStaticDebugText { get; private set; } = null;
	public static TMP_Text ScreenDynamicDebugText { get; private set; } = null;

	public static Button ScreenFPSInfoBtn { get; private set; } = null;
	public static Button ScreenDebugInfoBtn { get; private set; } = null;

	public static Button ScreenTimeScaleUpBtn { get; private set; } = null;
	public static Button ScreenTimeScaleDownBtn { get; private set; } = null;

	/** =====> 객체 <===== */
	public static GameObject ScreenDebugUIs { get; private set; } = null;
	public static GameObject ScreenFPSInfoUIs { get; private set; } = null;
	public static GameObject ScreenDebugInfoUIs { get; private set; } = null;
	#endregion // 클래스 프로퍼티

	#region 함수
	/** 디버그 UI 를 설정한다 */
	private void SetupDebugUIs(GameObject a_oUIsObjs, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oUIsObjs != null);

		// 디버그 UI 가 존재 할 경우
		if(a_oUIsObjs != null && CSceneManager.ScreenStaticDebugText != null) {
			CSceneManager.m_oStrBuilderDict[EKey.STATIC_DEBUG_STR_BUILDER].Clear();
			CSceneManager.m_oStrBuilderDict[EKey.STATIC_DEBUG_STR_BUILDER].AppendFormat(KCDefine.U_TEXT_FMT_STATIC_DEBUG_INFO_01, CAccess.DeviceScreenSize.x, CAccess.DeviceScreenSize.y);
			CSceneManager.m_oStrBuilderDict[EKey.STATIC_DEBUG_STR_BUILDER].AppendFormat(KCDefine.U_TEXT_FMT_STATIC_DEBUG_INFO_02, CSceneManager.CanvasSize.x, CSceneManager.CanvasSize.y);
			CSceneManager.m_oStrBuilderDict[EKey.STATIC_DEBUG_STR_BUILDER].AppendFormat(KCDefine.U_TEXT_FMT_STATIC_DEBUG_INFO_03, CSceneManager.UpSafeAreaOffset, CSceneManager.DownSafeAreaOffset, CSceneManager.LeftSafeAreaOffset, CSceneManager.RightSafeAreaOffset);
			CSceneManager.m_oStrBuilderDict[EKey.STATIC_DEBUG_STR_BUILDER].AppendFormat(KCDefine.U_TEXT_FMT_STATIC_DEBUG_INFO_04, (EQualityLevel)QualitySettings.GetQualityLevel(), Application.targetFrameRate);
			CSceneManager.m_oStrBuilderDict[EKey.STATIC_DEBUG_STR_BUILDER].AppendFormat(KCDefine.U_TEXT_FMT_STATIC_DEBUG_INFO_05, KCDefine.B_DIR_P_WRITABLE);

#if ADS_MODULE_ENABLE
			CSceneManager.m_oStrBuilderDict[EKey.STATIC_DEBUG_STR_BUILDER].AppendFormat(KCDefine.U_TEXT_FMT_STATIC_DEBUG_INFO_06, CAccess.ScreenDPI, CAccess.GetBannerAdsHeight(KCDefine.U_SIZE_BANNER_ADS.y));
#endif // #if ADS_MODULE_ENABLE
		}
	}

	/** 테스트 UI 를 갱신한다 */
	private void UpdateTestUIsState(bool a_bIsOpen) {
		m_oBtnDict.GetValueOrDefault(EKey.TEST_UIS_OPEN_BTN)?.gameObject.SetActive(!a_bIsOpen);
		m_oBtnDict.GetValueOrDefault(EKey.TEST_UIS_CLOSE_BTN)?.gameObject.SetActive(a_bIsOpen);

		m_oUIsDict.GetValueOrDefault(EKey.TEST_CONTENTS_UIS)?.ExSetLocalPosX(a_bIsOpen ? KCDefine.B_VAL_0_REAL : -this.ScreenWidth, false);
	}

	/** 테스트 UI 열기 버튼을 눌렀을 경우 */
	private void OnTouchTestUIsOpenBtn() {
		this.UpdateTestUIsState(true);
	}

	/** 테스트 UI 닫기 버튼을 눌렀을 경우 */
	private void OnTouchTestUIsCloseBtn() {
		this.UpdateTestUIsState(false);
	}
	#endregion // 함수

	#region 클래스 함수
	/** FPS 정보 UI 상태를 갱신한다 */
	private void UpdateFPSInfoUIsState(float a_fDeltaTime) {
		CSceneManager.m_nNumFrames += KCDefine.B_VAL_1_INT;
		CSceneManager.m_fFPSInfoSkipTime += Mathf.Clamp01(Time.unscaledDeltaTime);

		// FPS 정보 갱신 주기가 지났을 경우
		if(CSceneManager.m_fFPSInfoSkipTime.ExIsGreateEquals(KCDefine.B_VAL_1_REAL)) {
			CSceneManager.ScreenFPSText?.ExSetText<TMP_Text>(string.Format(KCDefine.U_TEXT_FMT_FPS, CSceneManager.m_nNumFrames, Screen.currentResolution.refreshRate), false);
			CSceneManager.ScreenFrameTimeText?.ExSetText<TMP_Text>(string.Format(KCDefine.U_TEXT_FMT_FRAME_TIME, (CSceneManager.m_fFPSInfoSkipTime / CSceneManager.m_nNumFrames) * KCDefine.B_UNIT_MILLI_SECS_PER_SEC), false);
			CSceneManager.ScreenDeviceInfoText?.ExSetText<TMP_Text>(string.Format(KCDefine.U_TEXT_FMT_DEVICE_INFO, SystemInfo.graphicsDeviceName, SystemInfo.graphicsDeviceType), false);

			CSceneManager.m_nNumFrames = KCDefine.B_VAL_0_INT;
			CSceneManager.m_fFPSInfoSkipTime -= KCDefine.B_VAL_1_REAL;
		}
	}

	/** 디버그 정보 UI 상태를 갱신한다 */
	private void UpdateDebugInfoUIsState(float a_fDeltaTime) {
		CSceneManager.m_fDebugInfoSkipTime += Mathf.Clamp01(Time.unscaledDeltaTime);

		// 디버그 정보 갱신 주기가 지났을 경우
		if(CSceneManager.m_fDebugInfoSkipTime.ExIsGreateEquals(KCDefine.B_VAL_1_REAL)) {
			CSceneManager.m_fDebugInfoSkipTime = KCDefine.B_VAL_0_REAL;

			CSceneManager.m_oStrBuilderDict[EKey.DYNAMIC_DEBUG_STR_BUILDER].Clear();
			CSceneManager.m_oStrBuilderDict[EKey.DYNAMIC_DEBUG_STR_BUILDER].AppendFormat(KCDefine.U_TEXT_FMT_DYNAMIC_DEBUG_INFO_01, Profiler.usedHeapSizeLong.ExByteToMegaByte(), Profiler.GetAllocatedMemoryForGraphicsDriver().ExByteToMegaByte());
			CSceneManager.m_oStrBuilderDict[EKey.DYNAMIC_DEBUG_STR_BUILDER].AppendFormat(KCDefine.U_TEXT_FMT_DYNAMIC_DEBUG_INFO_02, Profiler.GetMonoHeapSizeLong().ExByteToMegaByte(), Profiler.GetMonoUsedSizeLong().ExByteToMegaByte());
			CSceneManager.m_oStrBuilderDict[EKey.DYNAMIC_DEBUG_STR_BUILDER].AppendFormat(KCDefine.U_TEXT_FMT_DYNAMIC_DEBUG_INFO_03, Profiler.GetTempAllocatorSize().ExByteToMegaByte(), Profiler.GetTotalAllocatedMemoryLong().ExByteToMegaByte());
			CSceneManager.m_oStrBuilderDict[EKey.DYNAMIC_DEBUG_STR_BUILDER].AppendFormat(KCDefine.U_TEXT_FMT_DYNAMIC_DEBUG_INFO_04, Profiler.GetTotalReservedMemoryLong().ExByteToMegaByte(), Profiler.GetTotalUnusedReservedMemoryLong().ExByteToMegaByte());
			CSceneManager.m_oStrBuilderDict[EKey.DYNAMIC_DEBUG_STR_BUILDER].AppendFormat(KCDefine.U_TEXT_FMT_DYNAMIC_DEBUG_INFO_05, Time.timeScale);

			CSceneManager.ScreenStaticDebugText?.ExSetText<TMP_Text>(string.Format(KCDefine.U_TEXT_FMT_STATIC_DEBUG_MSG, CSceneManager.m_oStrBuilderDict[EKey.STATIC_DEBUG_STR_BUILDER].ToString(), CSceneManager.m_oStrBuilderDict[EKey.EXTRA_STATIC_DEBUG_STR_BUILDER].ToString()));
			CSceneManager.ScreenDynamicDebugText?.ExSetText<TMP_Text>(string.Format(KCDefine.U_TEXT_FMT_DYNAMIC_DEBUG_MSG, CSceneManager.m_oStrBuilderDict[EKey.DYNAMIC_DEBUG_STR_BUILDER].ToString(), CSceneManager.m_oStrBuilderDict[EKey.EXTRA_DYNAMIC_DEBUG_STR_BUILDER].ToString()));
		}
	}

	/** 화면 FPS 정보 버튼을 눌렀을 경우 */
	private static void OnTouchScreenFPSInfoBtn() {
		CSceneManager.ScreenFPSInfoUIs.SetActive(!CSceneManager.ScreenFPSInfoUIs.activeSelf);
	}

	/** 화면 디버그 정보 버튼을 눌렀을 경우 */
	private static void OnTouchScreenDebugInfoBtn() {
		CSceneManager.ScreenDebugInfoUIs.SetActive(!CSceneManager.ScreenDebugInfoUIs.activeSelf);
	}

	/** 화면 시간 비율 증가 버튼을 눌렀을 경우 */
	private static void OnTouchScreenTimeScaleUpBtn() {
		Time.timeScale = Mathf.Clamp01(Time.timeScale + (KCDefine.B_VAL_1_REAL / (KCDefine.B_UNIT_DIGITS_PER_TEN * KCDefine.B_VAL_2_REAL)));
	}

	/** 화면 시간 비율 감소 버튼을 눌렀을 경우 */
	private static void OnTouchScreenTimeScaleDownBtn() {
		Time.timeScale = Mathf.Clamp01(Time.timeScale - (KCDefine.B_VAL_1_REAL / (KCDefine.B_UNIT_DIGITS_PER_TEN * KCDefine.B_VAL_2_REAL)));
	}

	/** 화면 디버그 UI 를 변경한다 */
	public static void SetScreenDebugUIs(GameObject a_oUIs) {
		CSceneManager.ScreenDebugUIs = a_oUIs;

		// 텍스트를 변경한다 {
		CSceneManager.ScreenFPSText = a_oUIs.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_SCREEN_FPS_TEXT);
		CSceneManager.ScreenFPSText.raycastTarget = false;

		CSceneManager.ScreenFrameTimeText = a_oUIs.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_SCREEN_FRAME_TIME_TEXT);
		CSceneManager.ScreenFrameTimeText.raycastTarget = false;

		CSceneManager.ScreenDeviceInfoText = a_oUIs.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_SCREEN_DEVICE_INFO_TEXT);
		CSceneManager.ScreenDeviceInfoText.raycastTarget = false;

		CSceneManager.ScreenStaticDebugText = a_oUIs.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_SCREEN_STATIC_DEBUG_TEXT);
		CSceneManager.ScreenStaticDebugText.raycastTarget = false;

		CSceneManager.ScreenDynamicDebugText = a_oUIs.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_SCREEN_DYNAMIC_DEBUG_TEXT);
		CSceneManager.ScreenDynamicDebugText.raycastTarget = false;
		// 텍스트를 변경한다 }

		// 버튼을 설정한다 {
		CSceneManager.ScreenFPSInfoBtn = a_oUIs.ExFindComponent<Button>(KCDefine.U_OBJ_N_SCREEN_FPS_INFO_BTN);
		CSceneManager.ScreenFPSInfoBtn.gameObject.SetActive(false);

		CSceneManager.ScreenDebugInfoBtn = a_oUIs.ExFindComponent<Button>(KCDefine.U_OBJ_N_SCREEN_DEBUG_INFO_BTN);
		CSceneManager.ScreenDebugInfoBtn.gameObject.SetActive(false);

		CSceneManager.ScreenTimeScaleUpBtn = a_oUIs.ExFindComponent<Button>(KCDefine.U_OBJ_N_SCREEN_TIME_SCALE_UP_BTN);
		CSceneManager.ScreenTimeScaleUpBtn.gameObject.SetActive(false);

		CSceneManager.ScreenTimeScaleDownBtn = a_oUIs.ExFindComponent<Button>(KCDefine.U_OBJ_N_SCREEN_TIME_SCALE_DOWN_BTN);
		CSceneManager.ScreenTimeScaleDownBtn.gameObject.SetActive(false);
		// 버튼을 설정한다 }
	}

	/** 화면 FPS 정보 UI 를 변경한다 */
	public static void SetScreenFPSInfoUIs(GameObject a_oUIs) {
		CSceneManager.ScreenFPSInfoUIs = a_oUIs;
	}

	/** 화면 디버그 정보 UI 를 변경한다 */
	public static void SetScreenDebugInfoUIs(GameObject a_oUIs) {
		CSceneManager.ScreenDebugInfoUIs = a_oUIs;
	}
	#endregion // 클래스 함수
}
#endif // #if DEBUG || DEVELOPMENT_BUILD

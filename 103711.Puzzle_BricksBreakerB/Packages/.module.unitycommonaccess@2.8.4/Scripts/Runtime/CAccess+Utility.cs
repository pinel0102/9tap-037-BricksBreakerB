using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

#if UNITY_EDITOR
using UnityEditor;
#endif // #if UNITY_EDITOR

#if UNITY_IOS
using UnityEngine.iOS;
#endif // #if UNITY_IOS

#if UNITY_ANDROID
using UnityEngine.Android;
#endif // #if UNITY_ANDROID

#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif // #if PURCHASE_MODULE_ENABLE

/** 유틸리티 접근자 */
public static partial class CAccess {
	#region 클래스 프로퍼티
	public static bool IsNeedsTrackingConsent {
		get {
#if !UNITY_EDITOR && UNITY_IOS
			var oVer = new System.Version(Device.systemVersion);
			return oVer.CompareTo(KCDefine.U_MIN_VER_TRACKING_CONSENT_VIEW) >= KCDefine.B_COMPARE_EQUALS;
#elif !UNITY_EDITOR && UNITY_ANDROID
			return false;
#else
			return true;
#endif // #if !UNITY_EDITOR && UNITY_IOS
		}
	}

	public static bool IsSupportsHapticFeedback {
		get {
#if !UNITY_EDITOR && UNITY_IOS
			var oVer = new System.Version(Device.systemVersion);

			// 햅틱 피드백 지원 버전 일 경우
			if(oVer.CompareTo(KCDefine.U_MIN_VER_HAPTIC_FEEDBACK) >= KCDefine.B_COMPARE_EQUALS) {
				string oModel = Device.generation.ToString();
				return oModel.Contains(KCDefine.U_MODEL_N_IPHONE) && KCDefine.U_HAPTIC_FEEDBACK_SUPPORTS_MODEL_LIST.Contains(Device.generation);
			}

			return false;
#elif !UNITY_EDITOR && UNITY_ANDROID
			return true;
#else
			return false;
#endif // #if !UNITY_EDITOR && UNITY_IOS
		}
	}

	public static EDeviceType DeviceType {
		get {
#if UNITY_IOS
			return Device.generation.ToString().Contains(KCDefine.U_MODEL_N_IPAD) ? EDeviceType.TABLET : EDeviceType.PHONE;
#elif UNITY_ANDROID
			// TODO: 테블릿 여부 검사 로직 구현 필요
			return EDeviceType.PHONE;
#else
			switch(Application.platform) {
				case RuntimePlatform.PS4:
				case RuntimePlatform.PS5:
				case RuntimePlatform.XboxOne:
					return EDeviceType.CONSOLE;
				case RuntimePlatform.Switch:
				case RuntimePlatform.Stadia:
					return EDeviceType.HANDHELD_CONSOLE;
			}

			return EDeviceType.UNKNOWN;
#endif // #if UNITY_IOS
		}
	}

	public static Vector3 ScreenSize {
		get {
#if UNITY_EDITOR
			return new Vector3(Camera.main.pixelWidth, Camera.main.pixelHeight, KCDefine.B_VAL_0_REAL);
#else
			return new Vector3(Screen.width, Screen.height, KCDefine.B_VAL_0_REAL);
#endif // #if UNITY_EDITOR
		}
	}

	public static Rect SafeArea {
		get {
#if UNITY_EDITOR
			return new Rect(KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_0_REAL, Camera.main.pixelWidth, Camera.main.pixelHeight);
#else
			return Screen.safeArea;
#endif // #if UNITY_EDITOR
		}
	}

	public static float UpSafeAreaScale => (CAccess.ScreenSize.y - (CAccess.SafeArea.y + CAccess.SafeArea.height)) / CAccess.ScreenSize.y;
	public static float DownSafeAreaScale => CAccess.SafeArea.y / CAccess.ScreenSize.y;
	public static float LeftSafeAreaScale => CAccess.SafeArea.x / CAccess.ScreenSize.x;
	public static float RightSafeAreaScale => (CAccess.ScreenSize.x - (CAccess.SafeArea.x + CAccess.SafeArea.width)) / CAccess.ScreenSize.x;

	public static float ResolutionScale => CAccess.ScreenSize.x.ExIsLess(CAccess.ResolutionScreenSize.x) ? CAccess.ScreenSize.x / CAccess.ResolutionScreenSize.x : KCDefine.B_VAL_1_REAL;
	public static float ResolutionUnitScale => KCDefine.B_UNIT_SCALE * CAccess.ResolutionScale;
	public static float DesktopResolutionScale => CAccess.DesktopScreenSize.x.ExIsLess(CAccess.ResolutionDesktopScreenSize.x) ? CAccess.DesktopScreenSize.x / CAccess.ResolutionDesktopScreenSize.x : KCDefine.B_VAL_1_REAL;

	public static string ProductInfoTableLoadPath => File.Exists(KCDefine.U_RUNTIME_TABLE_P_G_PRODUCT_INFO) ? KCDefine.U_RUNTIME_TABLE_P_G_PRODUCT_INFO : KCDefine.U_TABLE_P_G_PRODUCT_INFO;
	public static string ProductInfoTableSavePath => KCDefine.U_RUNTIME_TABLE_P_G_PRODUCT_INFO;

	public static Vector3 DesktopScreenSize => new Vector3(Screen.currentResolution.width, Screen.currentResolution.height, CAccess.ScreenSize.z);
	public static Vector3 CorrectDesktopScreenSize => CAccess.ResulitionCorrectDesktopScreenSize * CAccess.DesktopResolutionScale;

	private static Vector3 ResolutionScreenSize => new Vector3(CAccess.ScreenSize.y * (KCDefine.B_SCREEN_WIDTH / (float)KCDefine.B_SCREEN_HEIGHT), CAccess.ScreenSize.y, CAccess.ScreenSize.z);
	private static Vector3 ResolutionDesktopScreenSize => new Vector3(CAccess.DesktopScreenSize.y * (KCDefine.B_LANDSCAPE_SCREEN_WIDTH / (float)KCDefine.B_LANDSCAPE_SCREEN_HEIGHT), CAccess.DesktopScreenSize.y, CAccess.DesktopScreenSize.z);
	private static Vector3 ResulitionCorrectDesktopScreenSize => CAccess.ResolutionDesktopScreenSize * KCDefine.B_DESKTOP_SCREEN_RATE;

#if UNITY_EDITOR || UNITY_STANDALONE
	public static float ScreenDPI => KCDefine.B_PLATFORM_SCREEN_DPI * (CAccess.ScreenSize.y / KCDefine.B_DPI_SCREEN_HEIGHT);
#else
	public static float ScreenDPI => Screen.dpi;
#endif // #if UNITY_EDITOR || UNITY_STANDALONE
	#endregion // 클래스 프로퍼티

	#region 클래스 함수
	/** 유저 권한 유효 여부를 검사한다 */
	public static bool IsEnableUserPermission(string a_oPermission) {
		CAccess.Assert(a_oPermission.ExIsValid());

#if UNITY_ANDROID
		return Permission.HasUserAuthorizedPermission(a_oPermission);
#else
		return false;
#endif // #if UNITY_ANDROID
	}

	/** 배너 광고 높이를 반환한다 */
	public static float GetBannerAdsHeight(float a_fHeight) {
		CAccess.Assert(a_fHeight.ExIsGreateEquals(KCDefine.B_VAL_0_REAL));
		return (a_fHeight.ExDPIPixelsToPixels() * (KCDefine.B_SCREEN_HEIGHT / CAccess.ScreenSize.y)) / CAccess.ResolutionScale;
	}

	/** iOS 이름을 반환한다 */
	public static string GetiOSName(EiOSType a_eType) {
		return KCDefine.B_PLATFORM_N_IOS_APPLE;
	}

	/** 안드로이드 이름을 반환한다 */
	public static string GetAndroidName(EAndroidType a_eType) {
		switch(a_eType) {
			case EAndroidType.AMAZON: return KCDefine.B_PLATFORM_N_ANDROID_AMAZON;
		}

		return KCDefine.B_PLATFORM_N_ANDROID_GOOGLE;
	}

	/** 독립 플랫폼 이름을 반환한다 */
	public static string GetStandaloneName(EStandaloneType a_eType) {
		switch(a_eType) {
			case EStandaloneType.WNDS_STEAM: return KCDefine.B_PLATFORM_N_STANDALONE_WNDS_STEAM;
		}

		return KCDefine.B_PLATFORM_N_STANDALONE_MAC_STEAM;
	}

	/** 렌더링 파이프라인 경로를 반환한다 */
	public static string GetRenderingPipelinePath(EQualityLevel a_eQualityLevel) {
		switch(a_eQualityLevel) {
			case EQualityLevel.HIGH: return KCDefine.U_ASSET_P_G_HIGH_QUALITY_UNIVERSAL_RP;
			case EQualityLevel.ULTRA: return KCDefine.U_ASSET_P_G_ULTRA_QUALITY_UNIVERSAL_RP;
		}

		return KCDefine.U_ASSET_P_G_NORM_QUALITY_UNIVERSAL_RP;
	}

	/** 포스트 프로세싱 설정 경로를 반환한다 */
	public static string GetPostProcessingSettingsPath(EQualityLevel a_eQualityLevel) {
		switch(a_eQualityLevel) {
			case EQualityLevel.HIGH: return KCDefine.U_ASSET_P_G_HIGH_QUALITY_POST_PROCESSING_SETTINGS;
			case EQualityLevel.ULTRA: return KCDefine.U_ASSET_P_G_ULTRA_QUALITY_POST_PROCESSING_SETTINGS;
		}

		return KCDefine.U_ASSET_P_G_NORM_QUALITY_POST_PROCESSING_SETTINGS;
	}

	/** 값을 할당한다 */
	public static void AssignVal(ref DG.Tweening.Tween a_rLhs, DG.Tweening.Tween a_oRhs, DG.Tweening.Tween a_oDefVal = null) {
		a_rLhs?.Kill();
		a_rLhs = a_oRhs ?? a_oDefVal;
	}

	/** 값을 할당한다 */
	public static void AssignVal(ref Sequence a_rLhs, DG.Tweening.Tween a_oRhs, DG.Tweening.Tween a_oDefVal = null) {
		a_rLhs?.Kill();
		a_rLhs = (a_oRhs ?? a_oDefVal) as Sequence;
	}

	/** DPI 픽셀 => 픽셀로 변환한다 */
	private static float ExDPIPixelsToPixels(this float a_fSender) {
		return a_fSender * (CAccess.ScreenDPI / KCDefine.B_DEF_SCREEN_DPI);
	}
	#endregion // 클래스 함수

	#region 제네릭 클래스 함수
	/** 리소스 존재 여부를 검사한다 */
	public static bool IsExistsRes<T>(string a_oFilePath, bool a_bIsAutoUnload = false) where T : Object {
		CAccess.Assert(a_oFilePath.ExIsValid());

		var oRes = Resources.Load<T>(a_oFilePath);
		bool bIsExistsRes = typeof(T).Equals(typeof(TextAsset)) ? (oRes as TextAsset).ExIsValid() : oRes != null;

		// 자동 제거 모드 일 경우
		if(bIsExistsRes && a_bIsAutoUnload) {
			Resources.UnloadAsset(oRes);
		}

		return bIsExistsRes;
	}
	#endregion // 제네릭 클래스 함수

	#region 조건부 클래스 함수
#if UNITY_EDITOR
	/** 스크립트 순서를 변경한다 */
	public static void SetScriptOrder(MonoScript a_oScript, int a_nOrder, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oScript != null);

		// 스크립트가 존재 할 경우
		if(a_oScript != null) {
			int nOrder = MonoImporter.GetExecutionOrder(a_oScript);

			// 기존 순서와 다를 경우
			if(nOrder != a_nOrder) {
				MonoImporter.SetExecutionOrder(a_oScript, a_nOrder);
			}
		}
	}
#endif // #if UNITY_EDITOR

#if PURCHASE_MODULE_ENABLE
	/** 가격 문자열을 반환한다 */
	public static string GetPriceStr(Product a_oProduct) {
		CAccess.Assert(a_oProduct != null);
		return string.Format(KCDefine.B_TEXT_FMT_2_SPACE_COMBINE, a_oProduct.metadata.isoCurrencyCode, a_oProduct.metadata.localizedPrice);
	}
#endif // #if PURCHASE_MODULE_ENABLE
	#endregion // 조건부 클래스 함수
}

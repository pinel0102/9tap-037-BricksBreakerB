using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using System.IO;
using UnityEngine.Rendering;
using UnityEditor;
using UnityEditor.Build;

#if NOTI_MODULE_ENABLE
using Unity.Notifications.iOS;
#endif // #if NOTI_MODULE_ENABLE

/** 에디터 기본 상수 */
public static partial class KCEditorDefine {
	#region 기본
	// 단위
	public const int B_UNIT_CUSTOM_TAG_START_ID = 10;

	// 정렬 순서 {
	public const int B_SORTING_O_GAME_OBJ_MENU = 0;
	public const int B_SORTING_O_BUILD_MENU = 10000;

	public const int B_SORTING_O_SETUP_MENU = 20000;
	public const int B_SORTING_O_RESET_MENU = KCEditorDefine.B_SORTING_O_SETUP_MENU + 1;

	public const int B_SORTING_O_IMPORT_MENU = 30000;
	public const int B_SORTING_O_EXPORT_MENU = KCEditorDefine.B_SORTING_O_IMPORT_MENU + 1;

	public const int B_SORTING_O_CREATE_MENU = 40000;
	public const int B_SORTING_O_SUB_CREATE_MENU = KCEditorDefine.B_SORTING_O_CREATE_MENU + 1;

	public const int B_SORTING_O_EDITOR_WND_MENU = 50000;
	public const int B_SORTING_O_CHANGE_PLATFORM_MENU = KCEditorDefine.B_SORTING_O_EDITOR_WND_MENU + 1;
	// 정렬 순서 }

	// 토큰
	public const string B_TOKEN_CLIENT = "Client";
	public const string B_TOKEN_APPLE_M_SERIES = "APPLE M";
	public const string B_TOKEN_REPLACE_UNITY_VERSION = "/*** UnityVersion */";

	// 형식
	public const string B_SORTING_OI_FMT = "[{0}:{1}]";

	// 버전
	public const string B_VER_UNITY_MODULE = "3.0.0";

	// 메뉴 {
	public const string B_MENU_TOOLS_BASE = "Tools/Utility/";
	public const string B_MENU_GAME_OBJECT_BASE = "GameObject/";

	public const string B_MENU_TOOLS_BUILD_BASE = KCEditorDefine.B_MENU_TOOLS_BASE + "Build/";
	public const string B_MENU_TOOLS_SETUP_BASE = KCEditorDefine.B_MENU_TOOLS_BASE + "Setup/";
	public const string B_MENU_TOOLS_RESET_BASE = KCEditorDefine.B_MENU_TOOLS_BASE + "Reset/";
	public const string B_MENU_TOOLS_IMPORT_BASE = KCEditorDefine.B_MENU_TOOLS_BASE + "Import/";
	public const string B_MENU_TOOLS_EXPORT_BASE = KCEditorDefine.B_MENU_TOOLS_BASE + "Export/";
	public const string B_MENU_TOOLS_CREATE_BASE = KCEditorDefine.B_MENU_TOOLS_BASE + "Create/";
	public const string B_MENU_TOOLS_SUB_CREATE_BASE = KCEditorDefine.B_MENU_TOOLS_BASE + "SubCreate/";
	public const string B_MENU_TOOLS_EDITOR_WND_BASE = KCEditorDefine.B_MENU_TOOLS_BASE + "EditorWindow/";
	public const string B_MENU_TOOLS_CHANGE_PLATFORM_BASE = KCEditorDefine.B_MENU_TOOLS_BASE + "ChangePlatform/";

	public const string B_MENU_GAME_OBJECT_UI_BASE = KCEditorDefine.B_MENU_GAME_OBJECT_BASE + "UI/Utility/";
	public const string B_MENU_GAME_OBJECT_2D_BASE = KCEditorDefine.B_MENU_GAME_OBJECT_BASE + "2D Object/Utility/";
	public const string B_MENU_GAME_OBJECT_FX_BASE = KCEditorDefine.B_MENU_GAME_OBJECT_BASE + "Effects/Utility/";
	// 메뉴 }

	// 커맨드 라인 {
	public const string B_CMD_LINE_PARAMS_FMT_SHELL = "-c \"{0}\"";
	public const string B_CMD_LINE_PARAMS_FMT_CMD_PROMPT = "/c \"{0}\"";

	public const string B_BUILD_CMD_INTEL_EXPORT_PATH = "export PATH=\"${PATH}:/usr/local/bin\"";
	public const string B_BUILD_CMD_SILICON_EXPORT_PATH = "export PATH=\"${PATH}:/opt/homebrew/bin\"";
	// 커맨드 라인 }

	// 이름 {
	public const string B_DIR_N_SCENES = "Scenes";
	public const string B_DIR_N_RESOURCES = "Resources";
	public const string B_EDITOR_SCENE_N_PATTERN_01 = "EditorMenu";
	public const string B_EDITOR_SCENE_N_PATTERN_02 = "EditorScene";

	public const string B_OBJ_N_SCENE_EDITOR_POPUP = "SceneEditorPopup";

	public const string B_MODULE_N_ADAPTIVE_PERFORMANCE_SETTINGS = "com.unity.adaptiveperformance.loader_settings";
	public const string B_MODULE_N_ADAPTIVE_PERFORMANCE_PROVIDER_SETTINGS = "com.unity.adaptiveperformance.simulator.provider_settings";
	public const string B_MODULE_N_ADAPTIVE_PERFORMANCE_SAMSUNG_PROVIDER_SETTINGS = "com.unity.adaptiveperformance.samsung.android.provider_settings";

	public const string B_MODULE_N_LOCALIZE_SETTINGS = "com.unity.localization.settings";
	public const string B_MODULE_N_ML_AGENTS_SETTINGS = "com.unity.ml-agents.settings";
	public const string B_MODULE_N_INPUT_SYSTEM_SETTINGS = "com.unity.input.settings";

	public const string B_PROPERTY_N_CATEGORY = "applicationCategoryType";
	public const string B_PROPERTY_N_REQUIRE_AR_KIT_SUPPORTS = "requiresARKitSupport";
	public const string B_PROPERTY_N_APPLE_ENABLE_PRO_MOTION = "appleEnableProMotion";
	public const string B_PROPERTY_N_AUTO_ADD_CAPABILITIES = "automaticallyDetectAndAddCapabilities";

	public const string B_PROPERTY_N_VALIDATE_APP_BUNDLE_SIZE = "validateAppBundleSize";
	public const string B_PROPERTY_N_APP_BUNDLE_SIZE_TO_VALIDATE = "appBundleSizeToValidate";
	public const string B_PROPERTY_N_SUPPORTED_ASPECT_RATIO_MODE = "supportedAspectRatioMode";

	public const string B_PROPERTY_N_ATLAS_WIDTH = "atlasWidth";
	public const string B_PROPERTY_N_ATLAS_HEIGHT = "atlasHeight";
	public const string B_PROPERTY_N_ATLAS_RENDER_MODE = "atlasRenderMode";
	public const string B_PROPERTY_N_CLEAR_DYNAMIC_DATA_ON_BUILD = "clearDynamicDataOnBuild";

	public const string B_PROPERTY_N_SORTING_LAYER = "sortingLayerName";
	public const string B_PROPERTY_N_SORTING_ORDER = "sortingOrder";

	public const string B_PROPERTY_N_TAG_M_TAG = "tags";
	public const string B_PROPERTY_N_TAG_M_NAME = "name";
	public const string B_PROPERTY_N_TAG_M_UNIQUE_ID = "uniqueID";
	public const string B_PROPERTY_N_TAG_M_SORTING_LAYER = "m_SortingLayers";

	public const string B_PROPERTY_N_SND_M_GLOBAL_VOLUME = "m_Volume";
	public const string B_PROPERTY_N_SND_M_ROLLOFF_SCALE = "Rolloff Scale";
	public const string B_PROPERTY_N_SND_M_DOPPLER_FACTOR = "Doppler Factor";
	public const string B_PROPERTY_N_SND_M_DISABLE_AUDIO = "m_DisableAudio";
	public const string B_PROPERTY_N_SND_M_VIRTUALIZE_EFFECT = "m_VirtualizeEffects";
	public const string B_PROPERTY_N_SND_M_AMBISONIC_DECODER_PLUGIN = "m_AmbisonicDecoderPlugin";
	public const string B_PROPERTY_N_SND_M_ENABLE_OUTPUT_SUSPENSION = "m_EnableOutputSuspension";

	public const string B_PROPERTY_N_QUALITY_S_NAME = "name";
	public const string B_PROPERTY_N_QUALITY_S_SECOND = "second";
	public const string B_PROPERTY_N_QUALITY_S_SETTINGS = "m_QualitySettings";
	public const string B_PROPERTY_N_QUALITY_S_DEF_QUALITY = "m_PerPlatformDefaultQuality";

	public const string B_SCENE_N_PATTERN = "t:Example t:Scene";
	public const string B_ASSET_N_LIGHTING_SETTINGS_TEMPLATE = "T_LightingSettings";
	public const string B_ASSET_N_POST_PROCESSING_SETTINGS_TEMPLATE = "T_PostProcessingSettings";

	public const string B_FUNC_N_GET_LIGHTMAP_ENCODING_QUALITY = "GetLightmapEncodingQualityForPlatformGroup";
	public const string B_FUNC_N_GET_LIGHTMAP_STREAMING_ENABLE = "GetLightmapStreamingEnabledForPlatformGroup";
	public const string B_FUNC_N_GET_LIGHTMAP_STREAMING_PRIORITY = "GetLightmapStreamingPriorityForPlatformGroup";

	public const string B_FUNC_N_SET_COMPRESSION_TYPE = "SetCompressionType";
	public const string B_FUNC_N_SET_BATCHING_FOR_PLATFORM = "SetBatchingForPlatform";

	public const string B_FUNC_N_SET_LIGHTMAP_ENCODING_QUALITY = "SetLightmapEncodingQualityForPlatformGroup";
	public const string B_FUNC_N_SET_LIGHTMAP_STREAMING_ENABLE = "SetLightmapStreamingEnabledForPlatformGroup";
	public const string B_FUNC_N_SET_LIGHTMAP_STREAMING_PRIORITY = "SetLightmapStreamingPriorityForPlatformGroup";
	// 이름 }

	// 경로 {
	public const string B_TOOL_P_SHELL = "/bin/zsh";
	public const string B_TOOL_P_CMD_PROMPT = "cmd.exe";

	public const string B_DIR_P_ASSETS = "Assets/";
	public const string B_DIR_P_PACKAGES = "Packages/";
	public const string B_DIR_P_AUTO_CREATE = "00-AutoCreate/";
	public const string B_DIR_P_UNITY_PROJ = "01-UnityProject/";
	public const string B_DIR_P_SUB_UNITY_PROJ = "02-SubUnityProject/";
	public const string B_DIR_P_UNITY_PROJ_EDITOR = "03-UnityProjectEditor/";
	public const string B_DIR_P_SUB_UNITY_PROJ_EDITOR = "04-SubUnityProjectEditor/";
	public const string B_DIR_P_PROJ_SETTINGS = "ProjectSettings/";
	public const string B_DIR_P_EDITOR_DEF_RESOURCES = "Editor Default Resources/";

	public const string B_DIR_P_EXPORT_BASE = "Export/";
	public const string B_DIR_P_EDITOR_SCRIPTS = "Scripts/Editor/";
	public const string B_DIR_P_RUNTIME_SCRIPTS = "Scripts/Runtime/";
	// 경로 }

	// iOS {
	public const string B_TEXT_IOS_TRUE = "YES";
	public const string B_TEXT_IOS_FALSE = "NO";

	public const string B_TEXT_IOS_METAL = "metal";
	public const string B_TEXT_IOS_ARM_64 = "arm64";

	public const string B_IPA_EXPORT_METHOD_IOS_DEV = "development";
	public const string B_IPA_EXPORT_METHOD_IOS_STORE = "app-store";

	public const string B_SEARCH_P_IOS_PODS = "$(SRCROOT)/**";
	public const string B_BUILD_FILE_EXTENSION_IOS_IPA = "ipa";

	public const string B_BUILD_P_FMT_IOS = "Builds/iOS/{0}";
	public const string B_PLIST_P_FMT_IOS = "{0}/Info.plist";
	public const string B_BUILD_OUTPUT_P_FMT_IOS = "Builds/iOS/{0}/BuildOutput/Export/{0}BuildOutput.{1}";

	public const string B_DATA_P_FMT_COCOA_PODS = "{0}/Podfile";
	public const string B_PROJ_P_FMT_COCOA_PODS = "{0}/Pods/Pods.xcodeproj/project.pbxproj";
	public const string B_BUILD_CMD_FMT_IOS_COCOA_PODS = "pod update --clean-install --project-directory={0}";

	public const string B_KEY_IOS_ENCRYPTION_ENABLE = "ITSAppUsesNonExemptEncryption";
	public const string B_KEY_IOS_DEVICE_CAPABILITIES = "UIRequiredDeviceCapabilities";
	public const string B_KEY_IOS_USER_TRACKING_USAGE_DESC = "NSUserTrackingUsageDescription";
	public const string B_KEY_IOS_FIREBASE_APP_STORE_RECEIPT_URL_CHECK_ENABLE = "FirebaseAppStoreReceiptURLCheckEnabled";

	public const string B_KEY_IOS_ADS_NETWORK_ID = "SKAdNetworkIdentifier";
	public const string B_KEY_IOS_ADS_NETWORK_ITEMS = "SKAdNetworkItems";

	public const string B_PROPERTY_N_IOS_ENABLE_BITCODE = "ENABLE_BITCODE";
	public const string B_PROPERTY_N_IOS_USER_HEADER_SEARCH_PATHS = "USER_HEADER_SEARCH_PATHS";
	public const string B_PROPERTY_N_IOS_PREPROCESSOR_DEFINITIONS = "GCC_PREPROCESSOR_DEFINITIONS";
	public const string B_PROPERTY_N_IOS_ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES = "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES";
	// iOS }

	// 안드로이드 {
	public const int B_UNIT_VALIDATE_APP_BUNDLE_SIZE = 150;

	public const string B_BUILD_FILE_EXTENSION_ANDROID_APK = "apk";
	public const string B_BUILD_FILE_EXTENSION_ANDROID_AAB = "aab";

	public const string B_BUILD_P_FMT_ANDROID = "Builds/Android/{0}/{1}";
	public const string B_BUILD_FILE_N_FMT_ANDROID = "{0}BuildOutput.{1}";
	public const string B_BUILD_OUTPUT_P_FMT_ANDROID = "Builds/Android/{0}/{0}BuildOutput.{1}";
	// 안드로이드 }

	// 독립 플랫폼 {
	public const string B_BUILD_FILE_EXTENSION_STANDALONE_APP = "app";
	public const string B_BUILD_FILE_EXTENSION_STANDALONE_EXE = "exe";
	public const string B_BUILD_FILE_EXTENSION_STANDALONE_ZIP = "zip";

	public const string B_BUILD_P_FMT_STANDALONE = "Builds/Standalone/{0}/{1}";
	public const string B_BUILD_FILE_N_FMT_STANDALONE = "{0}BuildOutput.{1}";
	public const string B_BUILD_OUTPUT_P_FMT_STANDALONE = "Builds/Standalone/{0}/{0}BuildOutput.{1}";
	// 독립 플랫폼 }

	// 젠킨스 {
	public const string B_KEY_JENKINS_PROJ_ROOT = "ProjRoot";
	public const string B_KEY_JENKINS_MODULE_VER = "ModuleVer";
	public const string B_KEY_JENKINS_BRANCH = "Branch";
	public const string B_KEY_JENKINS_SRC = "Src";
	public const string B_KEY_JENKINS_ANALYTICS_SRC = "AnalyticsSrc";
	public const string B_KEY_JENKINS_PROJ_NAME = "ProjName";
	public const string B_KEY_JENKINS_PROJ_PATH = "ProjPath";
	public const string B_KEY_JENKINS_BUILD_OUTPUT_PATH = "BuildOutputPath";
	public const string B_KEY_JENKINS_BUNDLE_ID = "BundleID";
	public const string B_KEY_JENKINS_PROFILE_ID = "ProfileID";
	public const string B_KEY_JENKINS_PLATFORM = "Platform";
	public const string B_KEY_JENKINS_PROJ_PLATFORM = "ProjPlatform";
	public const string B_KEY_JENKINS_BUILD_VER = "BuildVer";
	public const string B_KEY_JENKINS_BUILD_FUNC = "BuildFunc";
	public const string B_KEY_JENKINS_PIPELINE_NAME = "PipelineName";
	public const string B_KEY_JENKINS_IPA_EXPORT_METHOD = "IPAExportMethod";
	public const string B_KEY_JENKINS_BUILD_FILE_EXTENSION = "BuildFileExtension";

	public const string B_PROJ_PLATFORM_N_IOS = "iOS";
	public const string B_PROJ_PLATFORM_N_SERVER = "Server";
	public const string B_PROJ_PLATFORM_N_IPHONE = "iPhone";
	public const string B_PROJ_PLATFORM_N_ANDROID = "Android";
	public const string B_PROJ_PLATFORM_N_STANDALONE = "Standalone";

	public const string B_PROJ_PLATFORM_N_STANDALONE_MAC = "OSXUniversal";
	public const string B_PROJ_PLATFORM_N_STANDALONE_WNDS = "Win64";

	public const string B_TEX_IMPORTER_PLATFORM_N_DEF = "DefaultTexturePlatform";

	public const string B_BUILD_CMD_FMT_JENKINS = "curl -X POST {0} --user {1}:{2} --data token={3}";
	public const string B_BUILD_DATA_FMT_JENKINS = "--data {0}={1}";

	public const string B_BRANCH_FMT_JENKINS = "origin/{0}";
	public const string B_ANALYTICS_FMT_JENKINS = "{0}/00.Analytics";
	public const string B_IOS_APPLE_BUILD_PROJ_N_JENKINS = "01.iOSApple";

	public const string B_ANDROID_GOOGLE_BUILD_PROJ_N_JENKINS = "11.AndroidGoogle";
	public const string B_ANDROID_AMAZON_BUILD_PROJ_N_JENKINS = "12.AndroidAmazon";

	public const string B_STANDALONE_MAC_STEAM_BUILD_PROJ_N_JENKINS = "41.StandaloneMacSteam";
	public const string B_STANDALONE_WNDS_STEAM_BUILD_PROJ_N_JENKINS = "51.StandaloneWndsSteam";

	public const string B_DEBUG_BUILD_FUNC_JENKINS = "Debug";
	public const string B_RELEASE_BUILD_FUNC_JENKINS = "Release";
	public const string B_STORE_A_BUILD_FUNC_JENKINS = "StoreA";
	public const string B_STORE_B_BUILD_FUNC_JENKINS = "StoreB";
	public const string B_STORE_DIST_BUILD_FUNC_JENKINS = "StoreDist";

	public const string B_IOS_DEBUG_PIPELINE_N_JENKINS = "01.iOSDebug";
	public const string B_IOS_RELEASE_PIPELINE_N_JENKINS = "02.iOSRelease";
	public const string B_IOS_STORE_PIPELINE_N_JENKINS = "03.iOSStore";

	public const string B_ANDROID_DEBUG_PIPELINE_N_JENKINS = "11.AndroidDebug";
	public const string B_ANDROID_RELEASE_PIPELINE_N_JENKINS = "12.AndroidRelease";
	public const string B_ANDROID_STORE_PIPELINE_N_JENKINS = "13.AndroidStore";

	public const string B_STANDALONE_DEBUG_PIPELINE_N_JENKINS = "41.StandaloneDebug";
	public const string B_STANDALONE_RELEASE_PIPELINE_N_JENKINS = "42.StandaloneRelease";
	public const string B_STANDALONE_STORE_PIPELINE_N_JENKINS = "43.StandaloneStore";

#if NINETAP_BUILD_PIPELINE_ENABLE
	public const string B_PIPELINE_GROUP_NAME_FMT_JENKINS = "job/000000.Common/job/{0}/job/01.Pipelines/job";
#else
	public const string B_PIPELINE_GROUP_NAME_FMT_JENKINS = "job/0000000000.Common/job/{0}/job/01.Pipelines/job";
#endif // #if NINETAP_BUILD_PIPELINE_ENABLE
	// 젠킨스 }

	// 계층 뷰
	public const float B_OFFSET_HIERARCHY_TEXT = 15.0f;

	// 에디터 옵션 {
	public const float B_EDITOR_OPTS_CASCADE_BORDER_PERCENT = 0.15f;
	public const float B_EDITOR_OPTS_CASCADE_2_SPLIT_PERCENT = 0.35f;

	public const string B_EDITOR_OPTS_REMOTE_COMPRESSION = "JPEG";
	public const string B_EDITOR_OPTS_REMOTE_RESOLUTION = "Downsize";
	public const string B_EDITOR_OPTS_VER_CONTROL = "Visible Meta Files";
	public const string B_EDITOR_OPTS_JOYSTIC_SRC = "Remote";

#if UNITY_IOS
	public const string B_EDITOR_OPTS_REMOTE_DEVICE = "None";
#elif UNITY_ANDROID
	public const string B_EDITOR_OPTS_REMOTE_DEVICE = "None";
#else
	public const string B_EDITOR_OPTS_REMOTE_DEVICE = "None";
#endif // #if UNITY_IOS

	public static readonly List<string> B_EDITOR_OPTS_EXTENSION_LIST = new List<string>() {
		"txt", "xml", "fnt", "cd", "asmdef", "rsp", "asmref"
	};
	// 에디터 옵션 }

	// 알림 팝업 {
	public const string B_TEXT_ALERT_P_TITLE = "알림";
	public const string B_TEXT_ALERT_P_OK_BTN = "확인";
	public const string B_TEXT_ALERT_P_CANCEL_BTN = "취소";

	public const string B_MSG_ALERT_P_RESET = "해당 속성을 리셋하시겠습니까?";
	public const string B_MSG_ALERT_P_EXPORT_IMG_SUCCESS = "이미지를 추출했습니다.";
	public const string B_MSG_ALERT_P_PLATFORM_BUILD_FAIL = "해당 플랫폼으로 전환 후 빌드해주세요.";

	public const string B_MSG_ALERT_P_EXPORT_TEX_FAIL = "텍스처를 선택해주세요.";
	public const string B_MSG_ALERT_P_EXPORT_SPRITE_FAIL = "스프라이트를 선택해주세요.";

	public const string B_MSG_FMT_ALERT_P_MISSING_PREFAB = "프리팹이 소실 된 {0} 객체를 제거하시겠습니까?";
	// 알림 팝업 }

	// 객체 이름 에디터 윈도우 {
	public const string B_TEXT_APPLY = "적용";
	public const string B_TEXT_SEARCH = "검색";
	public const string B_TEXT_REPLACE = "변경";

	public const string B_TEXT_FONT_REPLACE = "=====> 폰트 변경 <=====";
	public const string B_TEXT_OBJ_NAME_REPLACE = "=====> 객체 이름 변경 <=====";
	// 객체 이름 에디터 윈도우 }

	// 패키지
	public const string B_NAME_DOTWEEN_PRO_PKGS = "DOTweenPro-v1.0.335";
	public const string B_NAME_APPLE_SIGN_IN_PKGS = "AppleSignInUnity-v1.4.2";
	public const string B_NAME_BUILD_REPORT_TOOL_PKGS = "BuildReportTool-v3.9.6";
	public const string B_NAME_ODIN_INSPECTOR_PKGS = "OdinInspectorAndSerializer-v3.1.8";

	// 알림
	public const string B_TEXT_NOTI_PROJ_PROPERTIES = "android.library=true";
	public const string B_TEXT_NOTI_ANDROID_MANIFEST = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<manifest xmlns:android=\"http://schemas.android.com/apk/res/android\" package=\"lkstudio.dante.android.notification\">\n</manifest>";
	#endregion // 기본

	#region 런타임 상수
	// 크기
	public static readonly Vector3 B_MIN_SIZE_EDITOR_WND = new Vector3(350.0f, 350.0f, 0.0f);

	// 에디터 옵션
	public static readonly Vector2 B_EDITOR_OPTS_CASCADE_3_SPLIT_PERCENT = new Vector2(0.15f, 0.15f + (0.15f * 2.0f));
	public static readonly Vector3 B_EDITOR_OPTS_CASCADE_4_SPLIT_PERCENT = new Vector3(0.1f, 0.1f + (0.1f * 2.0f), 0.1f + (0.1f * 2.0f) + (0.1f * 3));

	// 이름 {
	public static readonly List<string> B_OBJ_N_ROOT_OBJ_LIST = new List<string>() {
		KCDefine.U_OBJ_N_SCENE_BASE, KCDefine.U_OBJ_N_SCENE_OBJS_ROOT
	};

	public static readonly List<string> B_OBJ_N_ROOT_PREFAB_OBJ_LIST = new List<string>() {
		KCDefine.U_OBJ_N_SCENE_UIS_ROOT
	};

	public static readonly List<string> B_OBJ_N_STATIC_OBJ_LIST = new List<string>() {
		KCDefine.U_OBJ_N_SCENE_STATIC_OBJS, KCDefine.U_OBJ_N_SCENE_ADDITIONAL_LIGHTS, KCDefine.U_OBJ_N_SCENE_ADDITIONAL_CAMERAS, KCDefine.U_OBJ_N_SCENE_REFLECTION_PROBES, KCDefine.U_OBJ_N_SCENE_LIGHT_PROBE_GROUPS,
	};

	public static readonly List<string> B_OBJ_N_SCENE_EDITOR_LIGHT_LIST = new List<string>() {
		"SceneLight", "PreRenderLight"
	};

	public static readonly List<string> B_OBJ_N_SCENE_EDITOR_CAMERA_LIST = new List<string>() {
		"SceneCamera", "Main Camera", "Preview Camera", "Preview Scene Camera"
	};

	public static readonly List<string> B_NAMED_BUILD_TARGET_LIST = new List<string>() {
		NamedBuildTarget.iOS.TargetName, NamedBuildTarget.Android.TargetName, NamedBuildTarget.Standalone.TargetName
	};

	public static readonly Dictionary<string, string> B_DIR_N_SCENE_DICT = new Dictionary<string, string>() {
		[KCDefine.B_SCENE_N_INIT] = Path.GetDirectoryName(KCEditorDefine.B_DIR_P_AUTO_CREATE).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH),
		[KCDefine.B_SCENE_N_START] = Path.GetDirectoryName(KCEditorDefine.B_DIR_P_AUTO_CREATE).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH),
		[KCDefine.B_SCENE_N_SETUP] = Path.GetDirectoryName(KCEditorDefine.B_DIR_P_AUTO_CREATE).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH),
		[KCDefine.B_SCENE_N_AGREE] = Path.GetDirectoryName(KCEditorDefine.B_DIR_P_AUTO_CREATE).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH),
		[KCDefine.B_SCENE_N_LATE_SETUP] = Path.GetDirectoryName(KCEditorDefine.B_DIR_P_AUTO_CREATE).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH),

		[KCDefine.B_SCENE_N_TITLE] = Path.GetDirectoryName(KCEditorDefine.B_DIR_P_UNITY_PROJ).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH),
		[KCDefine.B_SCENE_N_MAIN] = Path.GetDirectoryName(KCEditorDefine.B_DIR_P_UNITY_PROJ).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH),
		[KCDefine.B_SCENE_N_GAME] = Path.GetDirectoryName(KCEditorDefine.B_DIR_P_UNITY_PROJ).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH),
		[KCDefine.B_SCENE_N_LOADING] = Path.GetDirectoryName(KCEditorDefine.B_DIR_P_UNITY_PROJ).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH),
		[KCDefine.B_SCENE_N_OVERLAY] = Path.GetDirectoryName(KCEditorDefine.B_DIR_P_UNITY_PROJ).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH),
		[KCDefine.B_SCENE_N_TEST] = Path.GetDirectoryName(KCEditorDefine.B_DIR_P_UNITY_PROJ).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH),

		[KCDefine.B_SCENE_N_MENU] = Path.GetDirectoryName(KCEditorDefine.B_DIR_P_AUTO_CREATE).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH),
		[KCDefine.B_SCENE_N_LEVEL_EDITOR] = Path.GetDirectoryName(KCEditorDefine.B_DIR_P_UNITY_PROJ_EDITOR).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH)
	};
	// 이름 }

	// 경로 {
	public static readonly string B_DIR_P_AUTO_SCENES = $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scenes/";
	public static readonly string B_DIR_P_UNITY_PROJ_SCENES = $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scenes/";
	public static readonly string B_DIR_P_SUB_UNITY_PROJ_SCENES = $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scenes/";

	public static readonly string B_DIR_P_UNITY_PROJ_EDITOR_SCENES = $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ_EDITOR}Scenes/";
	public static readonly string B_DIR_P_SUB_UNITY_PROJ_EDITOR_SCENES = $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR}Scenes/";

	public static readonly string B_DIR_P_AUTO_CREATE_RESOURCES = $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Resources/";
	public static readonly string B_DIR_P_UNITY_PROJ_RESOURCES = $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Resources/";
	public static readonly string B_DIR_P_SUB_UNITY_PROJ_RESOURCES = $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Resources/";
	public static readonly string B_DIR_P_UNITY_PROJ_EDITOR_RESOURCES = $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ_EDITOR}Resources/";
	public static readonly string B_DIR_P_SUB_UNITY_PROJ_EDITOR_RESOURCES = $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR}Resources/";

	public static readonly string B_DIR_P_SUB_UNITY_PROJ_PREFABS = $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_RESOURCES}Prefabs/";
	public static readonly string B_DIR_P_SUB_UNITY_PROJ_EDITOR_PREFABS = $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR_RESOURCES}Prefabs/";

	public static readonly string B_DIR_P_TEMPLATES = $"{KCEditorDefine.B_DIR_P_PACKAGES}Module.UnityCommon/Client/Templates/";
	public static readonly string B_ABS_DIR_P_ASSETS = $"{Application.dataPath}/";
	public static readonly string B_ABS_DIR_P_PACKAGES = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../Packages/";
	public static readonly string B_ABS_DIR_P_EXTERNAL_DATAS = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../ExternalDatas/";
	public static readonly string B_ABS_DIR_P_PROJ_SETTINGS = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../ProjectSettings/";
	public static readonly string B_ABS_DIR_P_TABLES = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../../Tables/";
	public static readonly string B_ABS_DIR_P_SCRIPTS = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../../Scripts/";
	public static readonly string B_ABS_DIR_P_UNITY_ENGINE = $"{EditorApplication.applicationPath}/";

	public static readonly string B_ABS_DIR_P_AUTO_CREATE_RESOURCES = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Resources/";
	public static readonly string B_ABS_DIR_P_UNITY_PROJ_RESOURCES = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Resources/";
	public static readonly string B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Resources/";

#if SAMPLE_PROJ
	public static readonly string B_ABS_DIR_P_TEMPLATES = $"{KCEditorDefine.B_ABS_DIR_P_PACKAGES}.module.unitycommon/Client/Templates/";
	public static readonly string B_ABS_DIR_P_STUDY_TEMPLATES = $"{KCEditorDefine.B_ABS_DIR_P_PACKAGES}.module.unitystudy/Client/Templates/";
#else
	public static readonly string B_ABS_DIR_P_TEMPLATES = $"{KCEditorDefine.B_ABS_DIR_P_PACKAGES}.module.unitycommon@{KCEditorDefine.B_VER_UNITY_MODULE}/Client/Templates/";
	public static readonly string B_ABS_DIR_P_STUDY_TEMPLATES = $"{KCEditorDefine.B_ABS_DIR_P_PACKAGES}.module.unitystudy@{KCEditorDefine.B_VER_UNITY_MODULE}/Client/Templates/";
#endif // #if SAMPLE_PROJ

	public static readonly string B_ABS_DIR_P_PLUGINS = $"{Application.dataPath}/Plugins/";
	public static readonly string B_ABS_DIR_P_IOS_PLUGINS = $"{KCEditorDefine.B_ABS_DIR_P_PLUGINS}iOS/";
	public static readonly string B_ABS_DIR_P_ANDROID_PLUGINS = $"{KCEditorDefine.B_ABS_DIR_P_PLUGINS}Android/";
	public static readonly string B_ABS_DIR_P_UNITY_PACKAGES = $"{KCEditorDefine.B_ABS_DIR_P_PACKAGES}UnityPackages/";

	public static readonly string B_ABS_PKGS_P_DOTWEEN_PRO = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Packages/UnityPackages/{KCEditorDefine.B_NAME_DOTWEEN_PRO_PKGS}.unitypackage";
	public static readonly string B_ABS_PKGS_P_APPLE_SIGN_IN = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Packages/UnityPackages/{KCEditorDefine.B_NAME_APPLE_SIGN_IN_PKGS}.unitypackage";
	public static readonly string B_ABS_PKGS_P_BUILD_REPORT_TOOL = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Packages/UnityPackages/{KCEditorDefine.B_NAME_BUILD_REPORT_TOOL_PKGS}.unitypackage";
	public static readonly string B_ABS_PKGS_P_ODIN_INSPECTOR = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Packages/UnityPackages/{KCEditorDefine.B_NAME_ODIN_INSPECTOR_PKGS}.unitypackage";

	public static readonly string B_ASSET_P_SAMPLE_SCENE = $"{KCEditorDefine.B_DIR_P_PACKAGES}Module.UnityCommon/Client/Scenes/{KCDefine.B_SCENE_N_SAMPLE}.unity";
	public static readonly string B_ASSET_P_MENU_SAMPLE_SCENE = $"{KCEditorDefine.B_DIR_P_PACKAGES}Module.UnityStudy/Client/Scenes/{KCDefine.B_SCENE_N_MENU_SAMPLE}.unity";
	public static readonly string B_ASSET_P_STUDY_SAMPLE_SCENE = $"{KCEditorDefine.B_DIR_P_PACKAGES}Module.UnityStudy/Client/Scenes/{KCDefine.B_SCENE_N_STUDY_SAMPLE}.unity";
	public static readonly string B_ASSET_P_EDITOR_SAMPLE_SCENE = $"{KCEditorDefine.B_DIR_P_PACKAGES}Module.UnityCommon/Client/Scenes/{KCDefine.B_SCENE_N_EDITOR_SAMPLE}.unity";

	public static readonly string B_ASSET_P_SAMPLE_SCENE_TEMPLATE = $"{KCEditorDefine.B_DIR_P_PACKAGES}Module.UnityCommon/Client/Scenes/{KCDefine.B_SCENE_N_SAMPLE}.scenetemplate";
	public static readonly string B_ASSET_P_MENU_SAMPLE_SCENE_TEMPLATE = $"{KCEditorDefine.B_DIR_P_PACKAGES}Module.UnityStudy/Client/Scenes/{KCDefine.B_SCENE_N_MENU_SAMPLE}.scenetemplate";
	public static readonly string B_ASSET_P_STUDY_SAMPLE_SCENE_TEMPLATE = $"{KCEditorDefine.B_DIR_P_PACKAGES}Module.UnityStudy/Client/Scenes/{KCDefine.B_SCENE_N_STUDY_SAMPLE}.scenetemplate";
	public static readonly string B_ASSET_P_EDITOR_SAMPLE_SCENE_TEMPLATE = $"{KCEditorDefine.B_DIR_P_PACKAGES}Module.UnityCommon/Client/Scenes/{KCDefine.B_SCENE_N_EDITOR_SAMPLE}.scenetemplate";

	public static readonly string B_ASSET_P_TAG_MANAGER = $"{KCEditorDefine.B_DIR_P_PROJ_SETTINGS}TagManager.asset";
	public static readonly string B_ASSET_P_SND_MANAGER = $"{KCEditorDefine.B_DIR_P_PROJ_SETTINGS}AudioManager.asset";
	public static readonly string B_ASSET_P_QUALITY_SETTINGS = $"{KCEditorDefine.B_DIR_P_PROJ_SETTINGS}QualitySettings.asset";

	public static readonly string B_ASSET_P_OPTS_INFO_TABLE = $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_OPTS_INFO_TABLE}.asset";
	public static readonly string B_ASSET_P_BUILD_INFO_TABLE = $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_BUILD_INFO_TABLE}.asset";
	public static readonly string B_ASSET_P_PROJ_INFO_TABLE = $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_PROJ_INFO_TABLE}.asset";
	public static readonly string B_ASSET_P_LOCALIZE_INFO_TABLE = $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_LOCALIZE_INFO_TABLE}.asset";
	public static readonly string B_ASSET_P_DEFINE_SYMBOL_INFO_TABLE = $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_DEFINE_SYMBOL_INFO_TABLE}.asset";

	public static readonly string B_ASSET_P_FMT_SCRIPTABLE_OBJ = $"{KCEditorDefine.B_DIR_P_ASSETS}{"{0}.asset"}";
	public static readonly string B_ASSET_P_FMT_DEFINE_S_OUTPUT = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../Builds/DefineSymbols/{"{0}DefineSymbols.txt"}";

	public static readonly string B_IMG_P_FMT_EXPORT = $"{KCEditorDefine.B_DIR_P_EXPORT_BASE}{"Images/{0}.png"}";
	public static readonly string B_TEX_P_FMT_EXPORT = $"{KCEditorDefine.B_DIR_P_EXPORT_BASE}{"Textures/{0}.png"}";
	public static readonly string B_DATA_P_BUILD_METHOD = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../Builds/BuildMethod.txt";

	public static readonly string B_ICON_P_STANDALONE_APP = $"{KCDefine.B_DIR_P_ICONS}Standalone/App/Icon";

	public static readonly string B_ICON_P_IOS_APP_76x76 = $"{KCDefine.B_DIR_P_ICONS}iOS/App/Icon76x76";
	public static readonly string B_ICON_P_IOS_APP_120x120 = $"{KCDefine.B_DIR_P_ICONS}iOS/App/Icon120x120";
	public static readonly string B_ICON_P_IOS_APP_152x152 = $"{KCDefine.B_DIR_P_ICONS}iOS/App/Icon152x152";
	public static readonly string B_ICON_P_IOS_APP_167x167 = $"{KCDefine.B_DIR_P_ICONS}iOS/App/Icon167x167";
	public static readonly string B_ICON_P_IOS_APP_180x180 = $"{KCDefine.B_DIR_P_ICONS}iOS/App/Icon180x180";
	public static readonly string B_ICON_P_IOS_APP_1024x1024 = $"{KCDefine.B_DIR_P_ICONS}iOS/App/Icon1024x1024";

	public static readonly string B_ICON_P_IOS_NOTI_20x20 = $"{KCDefine.B_DIR_P_ICONS}iOS/Notification/Icon20x20";
	public static readonly string B_ICON_P_IOS_NOTI_40x40 = $"{KCDefine.B_DIR_P_ICONS}iOS/Notification/Icon40x40";
	public static readonly string B_ICON_P_IOS_NOTI_60x60 = $"{KCDefine.B_DIR_P_ICONS}iOS/Notification/Icon60x60";

	public static readonly string B_ICON_P_ANDROID_APP_36x36 = $"{KCDefine.B_DIR_P_ICONS}Android/App/Icon36x36";
	public static readonly string B_ICON_P_ANDROID_APP_48x48 = $"{KCDefine.B_DIR_P_ICONS}Android/App/Icon48x48";
	public static readonly string B_ICON_P_ANDROID_APP_72x72 = $"{KCDefine.B_DIR_P_ICONS}Android/App/Icon72x72";
	public static readonly string B_ICON_P_ANDROID_APP_96x96 = $"{KCDefine.B_DIR_P_ICONS}Android/App/Icon96x96";
	public static readonly string B_ICON_P_ANDROID_APP_144x144 = $"{KCDefine.B_DIR_P_ICONS}Android/App/Icon144x144";
	public static readonly string B_ICON_P_ANDROID_APP_192x192 = $"{KCDefine.B_DIR_P_ICONS}Android/App/Icon192x192";
	public static readonly string B_ICON_P_ANDROID_APP_512x512 = $"{KCDefine.B_DIR_P_ICONS}Android/App/Icon512x512";

	public static readonly string B_ICON_P_ANDROID_NOTI_24x24 = $"{KCDefine.B_DIR_P_ICONS}Android/Notification/Icon24x24";
	public static readonly string B_ICON_P_ANDROID_NOTI_36x36 = $"{KCDefine.B_DIR_P_ICONS}Android/Notification/Icon36x36";
	public static readonly string B_ICON_P_ANDROID_NOTI_48x48 = $"{KCDefine.B_DIR_P_ICONS}Android/Notification/Icon48x48";
	public static readonly string B_ICON_P_ANDROID_NOTI_72x72 = $"{KCDefine.B_DIR_P_ICONS}Android/Notification/Icon72x72";
	public static readonly string B_ICON_P_ANDROID_NOTI_96x96 = $"{KCDefine.B_DIR_P_ICONS}Android/Notification/Icon96x96";
	public static readonly string B_ICON_P_ANDROID_NOTI_256x256 = $"{KCDefine.B_DIR_P_ICONS}Android/Notification/Icon256x256";

	public static readonly List<string> B_SEARCH_P_SCENE_LIST = new List<string>() {
		Path.GetDirectoryName(KCEditorDefine.B_DIR_P_AUTO_SCENES).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH),
		Path.GetDirectoryName(KCEditorDefine.B_DIR_P_UNITY_PROJ_SCENES).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH),
		Path.GetDirectoryName(KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_SCENES).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH),
		Path.GetDirectoryName(KCEditorDefine.B_DIR_P_UNITY_PROJ_EDITOR_SCENES).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH),
		Path.GetDirectoryName(KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR_SCENES).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH)
	};

	public static readonly List<string> B_SEARCH_P_PREFAB_SCENE_LIST = new List<string>() {
		Path.GetDirectoryName(KCEditorDefine.B_DIR_P_AUTO_SCENES).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH),
		Path.GetDirectoryName(KCEditorDefine.B_DIR_P_UNITY_PROJ_SCENES).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH),
		Path.GetDirectoryName(KCEditorDefine.B_DIR_P_UNITY_PROJ_EDITOR_SCENES).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH)
	};

	public static readonly List<string> B_SEARCH_P_SPRITE_ATLAS_LIST = new List<string>() {
		$"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_EDITOR_DEF_RESOURCES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}",
		$"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_EDITOR_DEF_RESOURCES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR}"
	};

	public static readonly List<string> B_ASSET_P_SPRITE_ATLAS_LIST = new List<string>() {
		KCDefine.U_ASSET_P_SPRITE_ATLAS_01,
		KCDefine.U_ASSET_P_SPRITE_ATLAS_02,
		KCDefine.U_ASSET_P_SPRITE_ATLAS_03,
		KCDefine.U_ASSET_P_SPRITE_ATLAS_04,
		KCDefine.U_ASSET_P_SPRITE_ATLAS_05,
		KCDefine.U_ASSET_P_SPRITE_ATLAS_06,
		KCDefine.U_ASSET_P_SPRITE_ATLAS_07,
		KCDefine.U_ASSET_P_SPRITE_ATLAS_08,
		KCDefine.U_ASSET_P_SPRITE_ATLAS_09,

		KCDefine.U_ASSET_P_G_SPRITE_ATLAS_01,
		KCDefine.U_ASSET_P_G_SPRITE_ATLAS_02,
		KCDefine.U_ASSET_P_G_SPRITE_ATLAS_03,
		KCDefine.U_ASSET_P_G_SPRITE_ATLAS_04,
		KCDefine.U_ASSET_P_G_SPRITE_ATLAS_05,
		KCDefine.U_ASSET_P_G_SPRITE_ATLAS_06,
		KCDefine.U_ASSET_P_G_SPRITE_ATLAS_07,
		KCDefine.U_ASSET_P_G_SPRITE_ATLAS_08,
		KCDefine.U_ASSET_P_G_SPRITE_ATLAS_09,

		KCDefine.U_ASSET_P_ES_SPRITE_ATLAS_01,
		KCDefine.U_ASSET_P_ES_SPRITE_ATLAS_02,
		KCDefine.U_ASSET_P_ES_SPRITE_ATLAS_03,
		KCDefine.U_ASSET_P_ES_SPRITE_ATLAS_04,
		KCDefine.U_ASSET_P_ES_SPRITE_ATLAS_05,
		KCDefine.U_ASSET_P_ES_SPRITE_ATLAS_06,
		KCDefine.U_ASSET_P_ES_SPRITE_ATLAS_07,
		KCDefine.U_ASSET_P_ES_SPRITE_ATLAS_08,
		KCDefine.U_ASSET_P_ES_SPRITE_ATLAS_09
	};

	public static readonly List<LogType> B_LOG_TYPE_LIST = new List<LogType>() {
		LogType.Log, LogType.Warning, LogType.Error, LogType.Assert, LogType.Exception
	};

	public static readonly List<BuildTarget> B_BUILD_TARGET_LIST = new List<BuildTarget>() {
		BuildTarget.iOS, BuildTarget.Android, BuildTarget.StandaloneOSX, BuildTarget.StandaloneWindows, BuildTarget.StandaloneWindows64
	};

	public static readonly List<BuildTargetGroup> B_BUILD_TARGET_GROUP_LIST = new List<BuildTargetGroup>() {
		BuildTargetGroup.iOS, BuildTargetGroup.Android, BuildTargetGroup.Standalone
	};

	public static readonly List<TextureImporterType> B_ENABLE_SRGB_TEX_TYPE_LIST = new List<TextureImporterType>() {
		TextureImporterType.Default, TextureImporterType.Sprite
	};

	public static readonly List<TextureImporterType> B_ENABLE_ALPHA_TRANSPARENCY_TEX_TYPE_LIST = new List<TextureImporterType>() {
		TextureImporterType.Default, TextureImporterType.Sprite, TextureImporterType.GUI, TextureImporterType.Cursor, TextureImporterType.Cookie
	};

	public static readonly List<TextureImporterType> B_IGNORE_RGBA_32_FMT_TEX_TYPE_LIST = new List<TextureImporterType>() {
		TextureImporterType.SingleChannel
	};

	public static readonly List<TextureImporterType> B_IGNORE_WRAP_MODE_TEX_TYPE_LIST = new List<TextureImporterType>() {
		TextureImporterType.NormalMap, TextureImporterType.Lightmap, TextureImporterType.DirectionalLightmap, TextureImporterType.Shadowmask
	};

	public static readonly List<TextureImporterType> B_IGNORE_FILTER_MODE_TEX_TYPE_LIST = new List<TextureImporterType>() {
		// Do Something
	};

	public static readonly List<TextureImporterType> B_IGNORE_NON_POT_SCALE_TEX_TYPE_LIST = new List<TextureImporterType>() {
		TextureImporterType.GUI, TextureImporterType.Sprite
	};

	public static readonly List<(string, string)> B_DIR_P_TABLE_INFO_LIST = new List<(string, string)>() {
		(Path.GetDirectoryName(KCDefine.U_RUNTIME_TABLE_P_G_LEVEL_INFO).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH), Path.GetDirectoryName($"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_LEVEL_INFO}").Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH)),

#if AB_TEST_ENABLE
		(Path.GetDirectoryName(KCDefine.U_RUNTIME_TABLE_P_G_LEVEL_INFO_SET_A).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH), Path.GetDirectoryName($"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_LEVEL_INFO_SET_A}").Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH)),
		(Path.GetDirectoryName(KCDefine.U_RUNTIME_TABLE_P_G_LEVEL_INFO_SET_B).Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH), Path.GetDirectoryName($"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_LEVEL_INFO_SET_B}").Replace(KCDefine.B_TOKEN_R_SLASH, KCDefine.B_TOKEN_SLASH))
#endif // #if AB_TEST_ENABLE
	};

	public static readonly List<(string, string)> B_FILE_P_TABLE_INFO_LIST = new List<(string, string)>() {
		(KCDefine.U_RUNTIME_TABLE_P_G_ETC_INFO, $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_ETC_INFO}.json"),
		(KCDefine.U_RUNTIME_TABLE_P_G_MISSION_INFO, $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_MISSION_INFO}.json"),
		(KCDefine.U_RUNTIME_TABLE_P_G_REWARD_INFO, $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_REWARD_INFO}.json"),
		(KCDefine.U_RUNTIME_TABLE_P_G_RES_INFO, $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_RES_INFO}.json"),

		(KCDefine.U_RUNTIME_TABLE_P_G_ITEM_INFO, $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_ITEM_INFO}.json"),
		(KCDefine.U_RUNTIME_TABLE_P_G_SKILL_INFO, $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_SKILL_INFO}.json"),
		(KCDefine.U_RUNTIME_TABLE_P_G_OBJ_INFO, $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_OBJ_INFO}.json"),
		(KCDefine.U_RUNTIME_TABLE_P_G_ABILITY_INFO, $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_ABILITY_INFO}.json"),
		(KCDefine.U_RUNTIME_TABLE_P_G_PRODUCT_INFO, $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_PRODUCT_INFO}.json"),

#if AB_TEST_ENABLE
		(KCDefine.U_RUNTIME_TABLE_P_G_ETC_INFO_SET_A, $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_ETC_INFO_SET_A}.json"),
		(KCDefine.U_RUNTIME_TABLE_P_G_ETC_INFO_SET_B, $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_ETC_INFO_SET_B}.json")
#endif // #if AB_TEST_ENABLE
	};

	public static readonly List<(string, string)> B_DATA_P_INFO_LIST = new List<(string, string)>() {
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Datas/T_README.md", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_AUTO_CREATE}README.md"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Datas/T_README.md", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}README.md"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Datas/T_README.md", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}README.md"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Datas/T_README.md", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ_EDITOR}README.md"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Datas/T_README.md", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR}README.md"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Datas/T_README.md", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_EDITOR_DEF_RESOURCES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}README.md"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Datas/T_README.md", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_EDITOR_DEF_RESOURCES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR}README.md"),

		// 00-AutoCreate
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Datas/T_Privacy_{SystemLanguage.Korean}.txt", $"{KCEditorDefine.B_ABS_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.AS_DATA_P_PRIVACY}.txt"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Datas/T_Services_{SystemLanguage.Korean}.txt", $"{KCEditorDefine.B_ABS_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.AS_DATA_P_SERVICES}.txt"),

		// 04.UnityPackages {
#if SAMPLE_PROJ || DEVELOPMENT_PROJ
#if UNITY_EDITOR_WIN
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Options/Analytics/DoxyfileWindows", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../../Doxyfile")
#else
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Options/Analytics/DoxyfileWindows", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../../Doxyfile")
#endif // #if UNITY_EDITOR_WIN
#endif // #if SAMPLE_PROJ || DEVELOPMENT_PROJ
		// 04.UnityPackages }
	};

	public static readonly List<(string, string)> B_SCRIPT_P_INFO_LIST = new List<(string, string)>() {
#if SAMPLE_PROJ || DEVELOPMENT_PROJ
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Scripts/Python/DSYMUploader.py", $"{KCEditorDefine.B_ABS_DIR_P_SCRIPTS}Python/DSYMUploader.py"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Scripts/Python/UnityModuleBranchEraser.py", $"{KCEditorDefine.B_ABS_DIR_P_SCRIPTS}Python/UnityModuleBranchEraser.py"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Scripts/Python/UnityModuleBranchMerger.py", $"{KCEditorDefine.B_ABS_DIR_P_SCRIPTS}Python/UnityModuleBranchMerger.py"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Scripts/Python/UnityModuleBranchSwitcher.py", $"{KCEditorDefine.B_ABS_DIR_P_SCRIPTS}Python/UnityModuleBranchSwitcher.py"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Scripts/Python/UnityModuleCmdExecuter.py", $"{KCEditorDefine.B_ABS_DIR_P_SCRIPTS}Python/UnityModuleCmdExecuter.py"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Scripts/Python/UnityModuleCommonImporter.py", $"{KCEditorDefine.B_ABS_DIR_P_SCRIPTS}Python/UnityModuleCommonImporter.py"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Scripts/Python/UnityModuleEraser.py", $"{KCEditorDefine.B_ABS_DIR_P_SCRIPTS}Python/UnityModuleEraser.py"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Scripts/Python/UnityModuleGC.py", $"{KCEditorDefine.B_ABS_DIR_P_SCRIPTS}Python/UnityModuleGC.py"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Scripts/Python/UnityModuleImporter.py", $"{KCEditorDefine.B_ABS_DIR_P_SCRIPTS}Python/UnityModuleImporter.py"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Scripts/Python/UnityModulePluginImporter.py", $"{KCEditorDefine.B_ABS_DIR_P_SCRIPTS}Python/UnityModulePluginImporter.py"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Scripts/Python/UnityModuleRemoteURLUpdater.py", $"{KCEditorDefine.B_ABS_DIR_P_SCRIPTS}Python/UnityModuleRemoteURLUpdater.py"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Scripts/Python/UnityModuleStudyImporter.py", $"{KCEditorDefine.B_ABS_DIR_P_SCRIPTS}Python/UnityModuleStudyImporter.py"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Scripts/Python/UnityModuleTagUpdater.py", $"{KCEditorDefine.B_ABS_DIR_P_SCRIPTS}Python/UnityModuleTagUpdater.py"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Scripts/Python/UnityModuleUpdater.py", $"{KCEditorDefine.B_ABS_DIR_P_SCRIPTS}Python/UnityModuleUpdater.py"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Scripts/Python/UnityModuleUploader.py", $"{KCEditorDefine.B_ABS_DIR_P_SCRIPTS}Python/UnityModuleUploader.py"),
#endif // #if SAMPLE_PROJ || DEVELOPMENT_PROJ

		// 00-AutoCreate {
		// 에디터 상수
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Editor/Define/T_KEditorDefine+Abs.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Editor/Global/Define/KEditorDefine+Abs.cs"),

		// 에디터 팩토리
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Editor/Factory/T_EditorFactory.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Editor/Global/Factory/EditorFactory.cs"),

		// 에디터 씬 관리자
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Editor/Scene/T_CEditorSceneManager.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Editor/EditorScene/CEditorSceneManager.cs"),

		// 유틸리티
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Cloud/T_CloudScript.js", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}Scripts/Cloud/CloudScript.js"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Runtime/External/T_CMsgPackRegister.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Runtime/Global/Utility/External/CMsgPackRegister.cs"),

#if SCENE_TEMPLATES_ENABLE || SCENE_TEMPLATES_MODULE_ENABLE
		// 씬 관리자 {
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Runtime/Scene/T_CSubInitSceneManager.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Runtime/SubInitScene/CSubInitSceneManager.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Runtime/Scene/T_CSubStartSceneManager.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Runtime/SubStartScene/CSubStartSceneManager.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Runtime/Scene/T_CSubSetupSceneManager.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Runtime/SubSetupScene/CSubSetupSceneManager.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Runtime/Scene/T_CSubAgreeSceneManager.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Runtime/SubAgreeScene/CSubAgreeSceneManager.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Runtime/Scene/T_CSubLateSetupSceneManager.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Runtime/SubLateSetupScene/CSubLateSetupSceneManager.cs"),

#if STUDY_ENABLE || STUDY_MODULE_ENABLE
		($"{KCEditorDefine.B_ABS_DIR_P_STUDY_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Runtime/Scene/T_CSSubMenuSceneManager.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Runtime/SubMenuScene/CSSubMenuSceneManager.cs"),
#endif // #if STUDY_ENABLE || STUDY_MODULE_ENABLE
		// 씬 관리자 }
#endif // #if SCENE_TEMPLATES_ENABLE || SCENE_TEMPLATES_MODULE_ENABLE

#if UTILITY_SCRIPT_TEMPLATES_ENABLE || UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		// 접근자
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Runtime/Access/T_Access.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Runtime/Global/Access/Access.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Runtime/Access/T_AccessExtension.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Runtime/Global/Access/AccessExtension.cs"),

		// 팩토리
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Runtime/Factory/T_Factory.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Runtime/Global/Factory/Factory.cs"),

		// 확장 클래스
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Runtime/Extension/T_Extension.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Runtime/Global/Extension/Extension.cs"),

		// 함수
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Runtime/Function/T_Func.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Runtime/Global/Function/Func.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Runtime/Function/T_LogFunc.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scripts/Runtime/Global/Function/LogFunc.cs"),
#endif // #if UTILITY_SCRIPT_TEMPLATES_ENABLE || UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		// 00-AutoCreate }

		// 01-UnityProject {
		// 에디터 팩토리
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Editor/Factory/T_EditorFactory+Global.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Editor/Global/Factory/EditorFactory+Global.cs"),

		// 에디터 유틸리티
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Editor/Build/T_CBuildProcessor.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Editor/Global/Utility/Build/CBuildProcessor.cs"),

#if UTILITY_SCRIPT_TEMPLATES_ENABLE || UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		// 엔진 {
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/T_CEngine.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/CEngine.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/T_CEngine+Setup.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/CEngine+Setup.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/T_CEngine+Access.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/CEngine+Access.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/T_CEngine+Factory.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/CEngine+Factory.cs"),

		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Base/T_CEComponent.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Global/Utility/Base/CEComponent.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Base/T_CEObjComponent.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Global/Utility/Base/CEObjComponent.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Base/T_CEController.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Global/Utility/Base/CEController.cs"),

		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Define/T_KDefine+Engine.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Global/Define/KDefine+Engine.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Define/T_KDefine+EngineType.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Global/Define/KDefine+EngineType.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Define/T_KDefine+EngineEnum.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Global/Define/KDefine+EngineEnum.cs"),

		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Access/T_Access+Engine.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Global/Access/Access+Engine.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Access/T_AccessExtension+Engine.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Global/Access/AccessExtension+Engine.cs"),

		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Factory/T_Factory+Engine.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Global/Factory/Factory+Engine.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Extension/T_Extension+Engine.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Global/Extension/Extension+Engine.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Function/T_Func+Engine.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Global/Function/Func+Engine.cs"),

		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Object/T_CEItem.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Global/Utility/Object/CEItem.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Object/T_CESkill.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Global/Utility/Object/CESkill.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Object/T_CEObj.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Global/Utility/Object/CEObj.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Object/T_CEFX.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Global/Utility/Object/CEFX.cs"),

		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Controller/T_CEItemController.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Global/Utility/Controller/CEItemController.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Controller/T_CESkillController.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Global/Utility/Controller/CESkillController.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Controller/T_CEObjController.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Global/Utility/Controller/CEObjController.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Controller/T_CEFXController.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Global/Utility/Controller/CEFXController.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Controller/T_CECellObjController.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Global/Utility/Controller/CECellObjController.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Controller/T_CEPlayerObjController.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Global/Utility/Controller/CEPlayerObjController.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Controller/T_CEEnemyObjController.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Global/Utility/Controller/CEEnemyObjController.cs"),
		// 엔진 }

		// 기본
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Base/T_CBaseInfo.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Global/Utility/Base/CBaseInfo.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Base/T_CGSComponent.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Global/Utility/Base/CGSComponent.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Base/T_CMissionPopup.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Global/Utility/Base/CMissionPopup.cs"),

		// 상수
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Define/T_KDefine+Type.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Global/Define/KDefine+Type.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Define/T_KDefine+Enum.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Global/Define/KDefine+Enum.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Define/T_KDefine+Global.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Global/Define/KDefine+Global.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Define/T_KDefine+Log.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Global/Define/KDefine+Log.cs"),

		// 접근자
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Access/T_Access+Global.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Global/Access/Access+Global.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Access/T_AccessExtension+Global.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Global/Access/AccessExtension+Global.cs"),

		// 팩토리
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Factory/T_Factory+Global.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Global/Factory/Factory+Global.cs"),

		// 확장 클래스
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Extension/T_Extension+Global.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Global/Extension/Extension+Global.cs"),

		// 함수
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Function/T_Func+Global.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Global/Function/Func+Global.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Function/T_LogFunc+Global.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Global/Function/LogFunc+Global.cs"),

		// 팝업
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Popup/T_CStorePopup.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CStorePopup.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Popup/T_CSettingsPopup.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CSettingsPopup.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Popup/T_CSyncPopup.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CSyncPopup.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Popup/T_CDailyMissionPopup.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CDailyMissionPopup.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Popup/T_CFreeRewardPopup.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CFreeRewardPopup.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Popup/T_CDailyRewardPopup.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CDailyRewardPopup.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Popup/T_CCoinsBoxBuyPopup.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CCoinsBoxBuyPopup.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Popup/T_CRewardAcquirePopup.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CRewardAcquirePopup.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Popup/T_CCoinsBoxAcquirePopup.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CCoinsBoxAcquirePopup.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Popup/T_CContinuePopup.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CContinuePopup.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Popup/T_CResultPopup.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CResultPopup.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Popup/T_CResumePopup.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CResumePopup.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Popup/T_CPausePopup.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CPausePopup.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Popup/T_CProductBuyPopup.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CProductBuyPopup.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Popup/T_CFocusPopup.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CFocusPopup.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Popup/T_CTutorialPopup.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CTutorialPopup.cs"),

		// 씬 관리자
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Scene/T_CSubTitleSceneManager.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/SubTitleScene/CSubTitleSceneManager.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Scene/T_CSubMainSceneManager.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/SubMainScene/CSubMainSceneManager.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Scene/T_CSubGameSceneManager.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/SubGameScene/CSubGameSceneManager.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Scene/T_CSubLoadingSceneManager.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/SubLoadingScene/CSubLoadingSceneManager.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Scene/T_CSubOverlaySceneManager.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/SubOverlayScene/CSubOverlaySceneManager.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/Scene/T_CSubTestSceneManager.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Runtime/SubTestScene/CSubTestSceneManager.cs"),
#endif // #if UTILITY_SCRIPT_TEMPLATES_ENABLE || UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		// 01-UnityProject }
		
		// 02-SubUnityProject {
#if EXTRA_SCRIPT_ENABLE || EXTRA_SCRIPT_MODULE_ENABLE
		// 에디터 상수
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Editor/Define/T_KEditorDefine+SubGlobal.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Editor/Global/Define/KEditorDefine+SubGlobal.cs"),

		// 에디터 팩토리
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Editor/Factory/T_EditorFactory+SubGlobal.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Editor/Global/Factory/EditorFactory+SubGlobal.cs"),

		// 임포터
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Editor/Importer/T_CSubAssetImporter.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Editor/Global/Importer/CSubAssetImporter.cs"),

		// 상수 {
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Define/T_KDefine+SubType.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Define/KDefine+SubType.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Define/T_KDefine+SubEnum.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Define/KDefine+SubEnum.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Define/T_KDefine+SubGlobal.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Define/KDefine+SubGlobal.cs"),

		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Define/T_KDefine+SubValTable.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Define/KDefine+SubValTable.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Define/T_KDefine+SubGameCenter.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Define/KDefine+SubGameCenter.cs"),
		// 상수 }

		// 접근자
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Access/T_Access+SubGlobal.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Access/Access+SubGlobal.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Access/T_AccessExtension+SubGlobal.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Access/AccessExtension+SubGlobal.cs"),

		// 팩토리
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Factory/T_Factory+SubGlobal.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Factory/Factory+SubGlobal.cs"),

		// 확장 클래스
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Extension/T_Extension+SubGlobal.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Extension/Extension+SubGlobal.cs"),

		// 함수
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Function/T_Func+Popup.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Function/Func+Popup.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Function/T_Func+SubGlobal.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Function/Func+SubGlobal.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Function/T_LogFunc+SubGlobal.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Function/LogFunc+SubGlobal.cs"),

		// 씬 관리자
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Scene/T_CSubTitleSceneManager+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/SubTitleScene/CSubTitleSceneManager+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Scene/T_CSubMainSceneManager+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/SubMainScene/CSubMainSceneManager+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Scene/T_CSubGameSceneManager+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/SubGameScene/CSubGameSceneManager+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Scene/T_CSubLoadingSceneManager+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/SubLoadingScene/CSubLoadingSceneManager+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Scene/T_CSubOverlaySceneManager+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/SubOverlayScene/CSubOverlaySceneManager+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Scene/T_CSubTestSceneManager+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/SubTestScene/CSubTestSceneManager+Sub.cs"),
#endif // #if EXTRA_SCRIPT_ENABLE || EXTRA_SCRIPT_MODULE_ENABLE

#if UTILITY_SCRIPT_TEMPLATES_ENABLE || UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		// 엔진 {
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/T_CEngine+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/CEngine+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/T_CEngine+SubSetup.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/CEngine+SubSetup.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/T_CEngine+SubAccess.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/CEngine+SubAccess.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/T_CEngine+SubFactory.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/CEngine+SubFactory.cs"),

		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Base/T_CEComponent+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Global/Utility/Base/CEComponent+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Base/T_CEObjComponent+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Global/Utility/Base/CEObjComponent+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Base/T_CEController+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Global/Utility/Base/CEController+Sub.cs"),

		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Define/T_KDefine+SubEngine.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Global/Define/KDefine+SubEngine.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Define/T_KDefine+SubEngineType.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Global/Define/KDefine+SubEngineType.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Define/T_KDefine+SubEngineEnum.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Global/Define/KDefine+SubEngineEnum.cs"),

		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Access/T_Access+SubEngine.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Global/Access/Access+SubEngine.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Access/T_AccessExtension+SubEngine.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Global/Access/AccessExtension+SubEngine.cs"),

		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Factory/T_Factory+SubEngine.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Global/Factory/Factory+SubEngine.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Extension/T_Extension+SubEngine.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Global/Extension/Extension+SubEngine.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Function/T_Func+SubEngine.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Global/Function/Func+SubEngine.cs"),

		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Object/T_CEItem+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Global/Utility/Object/CEItem+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Object/T_CESkill+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Global/Utility/Object/CESkill+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Object/T_CEObj+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Global/Utility/Object/CEObj+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Object/T_CEFX+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Global/Utility/Object/CEFX+Sub.cs"),

		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Controller/T_CEItemController+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Global/Utility/Controller/CEItemController+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Controller/T_CESkillController+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Global/Utility/Controller/CESkillController+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Controller/T_CEObjController+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Global/Utility/Controller/CEObjController+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Controller/T_CEFXController+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Global/Utility/Controller/CEFXController+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Controller/T_CECellObjController+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Global/Utility/Controller/CECellObjController+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Controller/T_CEPlayerObjController+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Global/Utility/Controller/CEPlayerObjController+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Controller/T_CEEnemyObjController+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Global/Utility/Controller/CEEnemyObjController+Sub.cs"),
		// 엔진 }

		// 기본
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Base/T_CSubPopup.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Base/CSubPopup.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Base/T_CSubAlertPopup.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Base/CSubAlertPopup.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Base/T_CGSComponent+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Base/CGSComponent+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Base/T_CMissionPopup+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Base/CMissionPopup+Sub.cs"),

		// 효과
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/FX/T_CDifficultyCorrector.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/FX/CDifficultyCorrector.cs"),

		// 테이블 {
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Table/T_CEtcInfoTable.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Table/CEtcInfoTable.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Table/T_CLevelInfoTable.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Table/CLevelInfoTable.cs"),

		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Table/T_CCalcInfoTable.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Table/CCalcInfoTable.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Table/T_CMissionInfoTable.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Table/CMissionInfoTable.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Table/T_CRewardInfoTable.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Table/CRewardInfoTable.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Table/T_CEpisodeInfoTable.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Table/CEpisodeInfoTable.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Table/T_CTutorialInfoTable.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Table/CTutorialInfoTable.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Table/T_CResInfoTable.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Table/CResInfoTable.cs"),

		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Table/T_CItemInfoTable.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Table/CItemInfoTable.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Table/T_CSkillInfoTable.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Table/CSkillInfoTable.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Table/T_CObjInfoTable.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Table/CObjInfoTable.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Table/T_CFXInfoTable.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Table/CFXInfoTable.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Table/T_CAbilityInfoTable.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Table/CAbilityInfoTable.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Table/T_CProductTradeInfoTable.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Table/CProductTradeInfoTable.cs"),
		// 테이블 }

		// 저장소
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Storage/T_CAppInfoStorage.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Storage/CAppInfoStorage.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Storage/T_CUserInfoStorage.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Storage/CUserInfoStorage.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Storage/T_CGameInfoStorage.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Storage/CGameInfoStorage.cs"),

		// 팝업
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Popup/T_CStorePopup+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CStorePopup+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Popup/T_CSettingsPopup+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CSettingsPopup+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Popup/T_CSyncPopup+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CSyncPopup+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Popup/T_CDailyMissionPopup+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CDailyMissionPopup+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Popup/T_CFreeRewardPopup+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CFreeRewardPopup+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Popup/T_CDailyRewardPopup+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CDailyRewardPopup+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Popup/T_CCoinsBoxBuyPopup+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CCoinsBoxBuyPopup+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Popup/T_CRewardAcquirePopup+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CRewardAcquirePopup+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Popup/T_CCoinsBoxAcquirePopup+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CCoinsBoxAcquirePopup+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Popup/T_CContinuePopup+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CContinuePopup+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Popup/T_CResultPopup+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CResultPopup+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Popup/T_CResumePopup+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CResumePopup+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Popup/T_CPausePopup+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CPausePopup+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Popup/T_CProductBuyPopup+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CProductBuyPopup+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Popup/T_CFocusPopup+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CFocusPopup+Sub.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Popup/T_CTutorialPopup+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/Popup/CTutorialPopup+Sub.cs"),

		// 스크롤러 셀 뷰
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/ScrollView/T_CLevelScrollerCellView.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/ScrollView/CLevelScrollerCellView.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/ScrollView/T_CStageScrollerCellView.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/ScrollView/CStageScrollerCellView.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/ScrollView/T_CChapterScrollerCellView.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Runtime/Global/Utility/ScrollView/CChapterScrollerCellView.cs"),
#endif // #if UTILITY_SCRIPT_TEMPLATES_ENABLE || UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		// 02-SubUnityProject }

		// 03-UnityProjectEditor {
#if EDITOR_SCENE_TEMPLATES_ENABLE || EDITOR_SCENE_TEMPLATES_MODULE_ENABLE
		// 상수
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ_EDITOR}Scripts/Runtime/Define/T_KDefine+Editor.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ_EDITOR}Scripts/Runtime/Global/Define/KDefine+Editor.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ_EDITOR}Scripts/Runtime/Define/T_KDefine+EditorType.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ_EDITOR}Scripts/Runtime/Global/Define/KDefine+EditorType.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ_EDITOR}Scripts/Runtime/Define/T_KDefine+EditorEnum.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ_EDITOR}Scripts/Runtime/Global/Define/KDefine+EditorEnum.cs"),

		// 접근자
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ_EDITOR}Scripts/Runtime/Access/T_Access+Editor.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ_EDITOR}Scripts/Runtime/Global/Access/Access+Editor.cs"),

		// 팩토리
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ_EDITOR}Scripts/Runtime/Factory/T_Factory+Editor.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ_EDITOR}Scripts/Runtime/Global/Factory/Factory+Editor.cs"),

		// 함수
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ_EDITOR}Scripts/Runtime/Function/T_Func+Editor.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ_EDITOR}Scripts/Runtime/Global/Function/Func+Editor.cs"),

		// 씬 관리자
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ_EDITOR}Scripts/Runtime/Scene/T_CSubLevelEditorSceneManager.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ_EDITOR}Scripts/Runtime/SubLevelEditorScene/CSubLevelEditorSceneManager.cs"),
#endif // #if EDITOR_SCENE_TEMPLATES_ENABLE || EDITOR_SCENE_TEMPLATES_MODULE_ENABLE
		// 03-UnityProjectEditor }

		// 04-SubUnityProjectEditor {
#if EDITOR_SCENE_TEMPLATES_ENABLE || EDITOR_SCENE_TEMPLATES_MODULE_ENABLE
		// 기본
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR}Scripts/Runtime/Base/T_CSubEditorInputPopup.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR}Scripts/Runtime/Global/Utility/Base/CSubEditorInputPopup.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR}Scripts/Runtime/Base/T_CSubEditorCreatePopup.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR}Scripts/Runtime/Global/Utility/Base/CSubEditorCreatePopup.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR}Scripts/Runtime/Base/T_CSubEditorScrollerCellView.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR}Scripts/Runtime/Global/Utility/Base/CSubEditorScrollerCellView.cs"),

		// 상수
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR}Scripts/Runtime/Define/T_KDefine+SubEditor.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR}Scripts/Runtime/Global/Define/KDefine+SubEditor.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR}Scripts/Runtime/Define/T_KDefine+SubEditorType.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR}Scripts/Runtime/Global/Define/KDefine+SubEditorType.cs"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR}Scripts/Runtime/Define/T_KDefine+SubEditorEnum.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR}Scripts/Runtime/Global/Define/KDefine+SubEditorEnum.cs"),

		// 팩토리
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR}Scripts/Runtime/Access/T_Access+SubEditor.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR}Scripts/Runtime/Global/Access/Access+SubEditor.cs"),

		// 팩토리
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR}Scripts/Runtime/Factory/T_Factory+SubEditor.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR}Scripts/Runtime/Global/Factory/Factory+SubEditor.cs"),

		// 함수
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR}Scripts/Runtime/Function/T_Func+SubEditor.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR}Scripts/Runtime/Global/Function/Func+SubEditor.cs"),

		// 씬 관리자
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR}Scripts/Runtime/Scene/T_CSubLevelEditorSceneManager+Sub.cs", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR}Scripts/Runtime/SubLevelEditorScene/CSubLevelEditorSceneManager+Sub.cs")
#endif // #if EDITOR_SCENE_TEMPLATES_ENABLE || EDITOR_SCENE_TEMPLATES_MODULE_ENABLE
		// 04-SubUnityProjectEditor }
	};

	public static readonly List<(string, string)> B_PREFAB_P_INFO_LIST = new List<(string, string)>() {
		// 00-AutoCreate {
		($"{KCEditorDefine.B_DIR_P_PACKAGES}Module.UnityCommonExternals/Client/Runtime/Etc/SmartTimersManager/TimerManager/TimersManager.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_TIMER_MANAGER}.prefab"),

#if PREFAB_TEMPLATES_ENABLE || PREFAB_TEMPLATES_MODULE_ENABLE
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Text/T_Text.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_TEXT}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Button/Text/T_TextBtn.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_TEXT_BTN}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Button/Text/T_TextScaleBtn.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_TEXT_SCALE_BTN}.prefab"),

		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Text/T_TMPText.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_TMP_TEXT}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Button/TextMeshPro/T_TMPTextBtn.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_TMP_TEXT_BTN}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Button/TextMeshPro/T_TMPTextScaleBtn.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_TMP_TEXT_SCALE_BTN}.prefab"),

		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Text/T_LocalizeText.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_LOCALIZE_TEXT}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Button/Text/T_LocalizeTextBtn.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_LOCALIZE_TEXT_BTN}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Button/Text/T_LocalizeTextScaleBtn.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_LOCALIZE_TEXT_SCALE_BTN}.prefab"),

		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Text/T_TMPLocalizeText.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_TMP_LOCALIZE_TEXT}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Button/TextMeshPro/T_TMPLocalizeTextBtn.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_TMP_LOCALIZE_TEXT_BTN}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Button/TextMeshPro/T_TMPLocalizeTextScaleBtn.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_TMP_LOCALIZE_TEXT_SCALE_BTN}.prefab"),

		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Image/T_Img.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_IMG}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Image/T_MaskImg.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_MASK_IMG}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Image/T_FocusImg.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_FOCUS_IMG}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Image/T_GaugeImg.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_GAUGE_IMG}.prefab"),

		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Button/T_ImgBtn.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_IMG_BTN}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Button/T_ImgScaleBtn.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_IMG_SCALE_BTN}.prefab"),

		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Button/Text/T_ImgTextBtn.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_IMG_TEXT_BTN}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Button/Text/T_ImgTextScaleBtn.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_IMG_TEXT_SCALE_BTN}.prefab"),

		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Button/TextMeshPro/T_TMPImgTextBtn.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_TMP_IMG_TEXT_BTN}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Button/TextMeshPro/T_TMPImgTextScaleBtn.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_TMP_IMG_TEXT_SCALE_BTN}.prefab"),

		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Button/Text/T_ImgLocalizeTextBtn.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_IMG_LOCALIZE_TEXT_BTN}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Button/Text/T_ImgLocalizeTextScaleBtn.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_IMG_LOCALIZE_TEXT_SCALE_BTN}.prefab"),

		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Button/TextMeshPro/T_TMPImgLocalizeTextBtn.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_TMP_IMG_LOCALIZE_TEXT_BTN}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Button/TextMeshPro/T_TMPImgLocalizeTextScaleBtn.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_TMP_IMG_LOCALIZE_TEXT_SCALE_BTN}.prefab"),

		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Input/T_Dropdown.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_DROPDOWN}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Input/T_InputField.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_INPUT_FIELD}.prefab"),

		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Input/T_TMPDropdown.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_TMP_DROPDOWN}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Input/T_TMPInputField.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_TMP_INPUT_FIELD}.prefab"),

		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/ScrollView/T_ScrollView.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_SCROLL_VIEW}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/ScrollView/T_RecycleView.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_RECYCLE_VIEW}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/ScrollView/T_PageView.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_PAGE_VIEW}.prefab"),

		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/FX/T_LineFX.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_LINE_FX}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/FX/T_ParticleFX.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_PARTICLE_FX}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/FX/T_ReflectionProbe.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_REFLECTION_PROBE}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/FX/T_LightProbeGroup.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_LIGHT_PROBE_GROUP}.prefab"),

		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/2D/T_Sprite.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_SPRITE}.prefab"),

		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Popup/T_PortraitAgreePopup.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.AS_OBJ_P_PORTRAIT_AGREE_POPUP}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Popup/T_LandscapeAgreePopup.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.AS_OBJ_P_LANDSCAPE_AGREE_POPUP}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Popup/T_TrackingDescPopup.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.LSS_OBJ_P_TRACKING_DESC_POPUP}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Prefabs/UI/Popup/T_Popup.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_G_POPUP}.prefab"),

		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/Sound/T_BGSnd.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_BG_SND}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/Sound/T_FXSnds.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_FX_SNDS}.prefab"),

		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Responder/T_DragResponder.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_DRAG_RESPONDER}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Responder/T_TouchResponder.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_TOUCH_RESPONDER}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Responder/T_TouchResponder.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_INDICATOR_TOUCH_RESPONDER}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/UI/Responder/T_TouchResponder.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.U_OBJ_P_SCREEN_FADE_TOUCH_RESPONDER}.prefab"),

		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/{KCDefine.B_DIR_P_START_SCENE}T_LoadingText.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.SS_OBJ_P_LOADING_TEXT}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Prefabs/{KCDefine.B_DIR_P_START_SCENE}T_LoadingGauge.prefab", $"{KCEditorDefine.B_DIR_P_AUTO_CREATE_RESOURCES}{KCDefine.SS_OBJ_P_LOADING_GAUGE}.prefab"),
#endif // #if PREFAB_TEMPLATES_ENABLE || PREFAB_TEMPLATES_MODULE_ENABLE
		// 00-AutoCreate }

		// 02-SubUnityProject {
#if PREFAB_TEMPLATES_ENABLE || PREFAB_TEMPLATES_MODULE_ENABLE
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Prefabs/UI/Popup/T_AlertPopup.prefab", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_OBJ_P_G_ALERT_POPUP}.prefab"),
#endif // #if PREFAB_TEMPLATES_ENABLE || PREFAB_TEMPLATES_MODULE_ENABLE

#if (PREFAB_TEMPLATES_ENABLE || PREFAB_TEMPLATES_MODULE_ENABLE) && (UTILITY_SCRIPT_TEMPLATES_ENABLE || UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE)
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Prefabs/UI/Popup/T_Popup.prefab", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_OBJ_P_G_STORE_POPUP}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Prefabs/UI/Popup/T_Popup.prefab", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_OBJ_P_G_SETTINGS_POPUP}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Prefabs/UI/Popup/T_Popup.prefab", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_OBJ_P_G_SYNC_POPUP}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Prefabs/UI/Popup/T_Popup.prefab", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_OBJ_P_G_DAILY_MISSION_POPUP}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Prefabs/UI/Popup/T_Popup.prefab", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_OBJ_P_G_FREE_REWARD_POPUP}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Prefabs/UI/Popup/T_Popup.prefab", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_OBJ_P_G_DAILY_REWARD_POPUP}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Prefabs/UI/Popup/T_Popup.prefab", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_OBJ_P_G_COINS_BOX_POPUP}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Prefabs/UI/Popup/T_Popup.prefab", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_OBJ_P_G_REWARD_ACQUIRE_POPUP}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Prefabs/UI/Popup/T_Popup.prefab", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_OBJ_P_G_COINS_BOX_ACQUIRE_POPUP}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Prefabs/UI/Popup/T_Popup.prefab", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_OBJ_P_G_CONTINUE_POPUP}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Prefabs/UI/Popup/T_Popup.prefab", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_OBJ_P_G_RESULT_POPUP}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Prefabs/UI/Popup/T_Popup.prefab", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_OBJ_P_G_RESUME_POPUP}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Prefabs/UI/Popup/T_Popup.prefab", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_OBJ_P_G_PAUSE_POPUP}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Prefabs/UI/Popup/T_Popup.prefab", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_OBJ_P_G_PRODUCT_BUY_POPUP}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Prefabs/UI/Popup/T_FocusPopup.prefab", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_OBJ_P_G_FOCUS_POPUP}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Prefabs/UI/Popup/T_FocusPopup.prefab", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_OBJ_P_G_TUTORIAL_POPUP}.prefab"),
#endif // #if (PREFAB_TEMPLATES_ENABLE || PREFAB_TEMPLATES_MODULE_ENABLE) && (UTILITY_SCRIPT_TEMPLATES_ENABLE || UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE)
		// 02-SubUnityProject }

		// 04-SubUnityProjectEditor {
#if EDITOR_SCENE_TEMPLATES_ENABLE || EDITOR_SCENE_TEMPLATES_MODULE_ENABLE
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR}Prefabs/{KCDefine.B_DIR_P_EDITOR_SCENE}T_EditorInputPopup.prefab", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR_RESOURCES}{KCDefine.ES_OBJ_P_EDITOR_INPUT_POPUP}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR}Prefabs/{KCDefine.B_DIR_P_EDITOR_SCENE}T_EditorCreatePopup.prefab", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR_RESOURCES}{KCDefine.ES_OBJ_P_EDITOR_CREATE_POPUP}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR}Prefabs/{KCDefine.B_DIR_P_EDITOR_SCENE}T_EditorScrollerCellView.prefab", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR_RESOURCES}{KCDefine.ES_OBJ_P_EDITOR_SCROLLER_CELL_VIEW}.prefab"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR}Prefabs/{KCDefine.B_DIR_P_LEVEL_EDITOR_SCENE}T_REUIsPageUIs02ScrollCellView.prefab", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR_RESOURCES}{KCDefine.LES_OBJ_P_RE_UIS_PAGE_UIS_02_SCROLLER_CELL_VIEW}.prefab")
#endif // #if EDITOR_SCENE_TEMPLATES_ENABLE || EDITOR_SCENE_TEMPLATES_MODULE_ENABLE
		// 04-SubUnityProjectEditor }
	};

	public static readonly List<(string, string)> B_TABLE_P_INFO_LIST = new List<(string, string)>() {
#if SAMPLE_PROJ || DEVELOPMENT_PROJ
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/EtcInfo/G_EtcInfoTable.xlsx", $"{KCEditorDefine.B_ABS_DIR_P_TABLES}G_EtcInfoTable.xlsx"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/EtcInfo/G_EtcInfoTable.xlsx", $"{KCEditorDefine.B_ABS_DIR_P_TABLES}A/G_EtcInfoTable.xlsx"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/EtcInfo/G_EtcInfoTable.xlsx", $"{KCEditorDefine.B_ABS_DIR_P_TABLES}B/G_EtcInfoTable.xlsx"),

		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/MissionInfo/G_MissionInfoTable.xlsx", $"{KCEditorDefine.B_ABS_DIR_P_TABLES}G_MissionInfoTable.xlsx"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/RewardInfo/G_RewardInfoTable.xlsx", $"{KCEditorDefine.B_ABS_DIR_P_TABLES}G_RewardInfoTable.xlsx"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/ResourceInfo/G_ResInfoTable.xlsx", $"{KCEditorDefine.B_ABS_DIR_P_TABLES}G_ResInfoTable.xlsx"),

		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/ItemInfo/G_ItemInfoTable.xlsx", $"{KCEditorDefine.B_ABS_DIR_P_TABLES}G_ItemInfoTable.xlsx"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/SkillInfo/G_SkillInfoTable.xlsx", $"{KCEditorDefine.B_ABS_DIR_P_TABLES}G_SkillInfoTable.xlsx"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/ObjectInfo/G_ObjInfoTable.xlsx", $"{KCEditorDefine.B_ABS_DIR_P_TABLES}G_ObjInfoTable.xlsx"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/AbilityInfo/G_AbilityInfoTable.xlsx", $"{KCEditorDefine.B_ABS_DIR_P_TABLES}G_AbilityInfoTable.xlsx"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/ProductInfo/G_ProductInfoTable.xlsx", $"{KCEditorDefine.B_ABS_DIR_P_TABLES}G_ProductInfoTable.xlsx"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/VersionInfo/G_VerInfoTable.xlsx", $"{KCEditorDefine.B_ABS_DIR_P_TABLES}G_VerInfoTable.xlsx"),
#endif // #if SAMPLE_PROJ || DEVELOPMENT_PROJ

		// 03.UnityProject
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/ValueInfo/G_ValTable_Common.csv", $"{KCEditorDefine.B_ABS_DIR_P_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_COMMON_VAL}.csv"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/StringInfo/G_StrTable_Common.csv", $"{KCEditorDefine.B_ABS_DIR_P_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_COMMON_STR}.csv"),

		// 02-SubUnityProject {
#if UTILITY_SCRIPT_TEMPLATES_ENABLE || UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/StringInfo/G_StrTable_Common_{SystemLanguage.Korean}.csv", $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_KOREAN_COMMON_STR}.csv"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/StringInfo/G_StrTable_Common_{SystemLanguage.English}.csv", $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_ENGLISH_COMMON_STR}.csv"),

		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/LevelInfo/G_LevelInfoTable.json", $"{KCEditorDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_LEVEL_INFO}.json"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/LevelInfo/G_LevelInfoTable.json", $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_LEVEL_INFO}.json"),

		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/EtcInfo/G_EtcInfoTable.json", $"{KCEditorDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_ETC_INFO}.json"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/EtcInfo/G_EtcInfoTable.json", $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_ETC_INFO}.json"),

		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/MissionInfo/G_MissionInfoTable.json", $"{KCEditorDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_MISSION_INFO}.json"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/MissionInfo/G_MissionInfoTable.json", $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_MISSION_INFO}.json"),

		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/RewardInfo/G_RewardInfoTable.json", $"{KCEditorDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_REWARD_INFO}.json"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/RewardInfo/G_RewardInfoTable.json", $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_REWARD_INFO}.json"),

		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/ResourceInfo/G_ResInfoTable.json", $"{KCEditorDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_RES_INFO}.json"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/ResourceInfo/G_ResInfoTable.json", $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_RES_INFO}.json"),

		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/ItemInfo/G_ItemInfoTable.json", $"{KCEditorDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_ITEM_INFO}.json"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/ItemInfo/G_ItemInfoTable.json", $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_ITEM_INFO}.json"),

		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/SkillInfo/G_SkillInfoTable.json", $"{KCEditorDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_SKILL_INFO}.json"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/SkillInfo/G_SkillInfoTable.json", $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_SKILL_INFO}.json"),

		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/ObjectInfo/G_ObjInfoTable.json", $"{KCEditorDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_OBJ_INFO}.json"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/ObjectInfo/G_ObjInfoTable.json", $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_OBJ_INFO}.json"),

		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/AbilityInfo/G_AbilityInfoTable.json", $"{KCEditorDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_ABILITY_INFO}.json"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/AbilityInfo/G_AbilityInfoTable.json", $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_ABILITY_INFO}.json"),

		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/ProductInfo/G_ProductInfoTable.json", $"{KCEditorDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_PRODUCT_INFO}.json"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/ProductInfo/G_ProductInfoTable.json", $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_PRODUCT_INFO}.json"),

#if MSG_PACK_SERIALIZE_DESERIALIZE_ENABLE
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/LevelInfo/G_LevelInfo_000000001.bytes", $"{KCEditorDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{string.Format(KCDefine.U_DATA_P_FMT_G_LEVEL_INFO, 1)}.bytes"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/LevelInfo/G_LevelInfo_000000001.bytes", $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{string.Format(KCDefine.U_DATA_P_FMT_G_LEVEL_INFO, 1)}.bytes"),
#elif NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/LevelInfo/G_LevelInfo_000000001.json", $"{KCEditorDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{string.Format(KCDefine.U_DATA_P_FMT_G_LEVEL_INFO, 1)}.json"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/LevelInfo/G_LevelInfo_000000001.json", $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{string.Format(KCDefine.U_DATA_P_FMT_G_LEVEL_INFO, 1)}.json"),
#endif // #if MSG_PACK_SERIALIZE_DESERIALIZE_ENABLE

#if AB_TEST_ENABLE
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/LevelInfo/G_LevelInfoTable.json", $"{KCEditorDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_LEVEL_INFO_SET_A}.json"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/LevelInfo/G_LevelInfoTable.json", $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_LEVEL_INFO_SET_A}.json"),

		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/LevelInfo/G_LevelInfoTable.json", $"{KCEditorDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_LEVEL_INFO_SET_B}.json"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/LevelInfo/G_LevelInfoTable.json", $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_LEVEL_INFO_SET_B}.json"),

		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/EtcInfo/G_EtcInfoTable.json", $"{KCEditorDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_ETC_INFO_SET_A}.json"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/EtcInfo/G_EtcInfoTable.json", $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_ETC_INFO_SET_A}.json"),

		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/EtcInfo/G_EtcInfoTable.json", $"{KCEditorDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{KCDefine.U_TABLE_P_G_ETC_INFO_SET_B}.json"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/EtcInfo/G_EtcInfoTable.json", $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_TABLE_P_G_ETC_INFO_SET_B}.json"),

#if MSG_PACK_SERIALIZE_DESERIALIZE_ENABLE
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/LevelInfo/G_LevelInfo_000000001.bytes", $"{KCEditorDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{string.Format(KCDefine.U_DATA_P_FMT_G_LEVEL_INFO_SET_A, 1)}.bytes"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/LevelInfo/G_LevelInfo_000000001.bytes", $"{KCEditorDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{string.Format(KCDefine.U_DATA_P_FMT_G_LEVEL_INFO_SET_B, 1)}.bytes"),

		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/LevelInfo/G_LevelInfo_000000001.bytes", $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{string.Format(KCDefine.U_DATA_P_FMT_G_LEVEL_INFO_SET_A, 1)}.bytes"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/LevelInfo/G_LevelInfo_000000001.bytes", $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{string.Format(KCDefine.U_DATA_P_FMT_G_LEVEL_INFO_SET_B, 1)}.bytes")
#elif NEWTON_SOFT_JSON_SERIALIZE_DESERIALIZE_ENABLE
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/LevelInfo/G_LevelInfo_000000001.json", $"{KCEditorDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{string.Format(KCDefine.U_DATA_P_FMT_G_LEVEL_INFO_SET_A, 1)}.json"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/LevelInfo/G_LevelInfo_000000001.json", $"{KCEditorDefine.B_ABS_DIR_P_EXTERNAL_DATAS}{string.Format(KCDefine.U_DATA_P_FMT_G_LEVEL_INFO_SET_B, 1)}.json"),

		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/LevelInfo/G_LevelInfo_000000001.json", $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{string.Format(KCDefine.U_DATA_P_FMT_G_LEVEL_INFO_SET_A, 1)}.json"),
		($"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Tables/LevelInfo/G_LevelInfo_000000001.json", $"{KCEditorDefine.B_ABS_DIR_P_SUB_UNITY_PROJ_RESOURCES}{string.Format(KCDefine.U_DATA_P_FMT_G_LEVEL_INFO_SET_B, 1)}.json"),
#endif // #if MSG_PACK_SERIALIZE_DESERIALIZE_ENABLE
#endif // #if AB_TEST_ENABLE
#endif // #if UTILITY_SCRIPT_TEMPLATES_ENABLE || UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		// 02-SubUnityProject }
	};

	public static readonly List<(string, string)> B_ASSET_P_INFO_LIST = new List<(string, string)>() {
		// 01-UnityProject {
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scriptables/T_OptsInfoTable.asset", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_OPTS_INFO_TABLE}.asset"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scriptables/T_BuildInfoTable.asset", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_BUILD_INFO_TABLE}.asset"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scriptables/T_DefineSymbolInfoTable.asset", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_DEFINE_SYMBOL_INFO_TABLE}.asset"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scriptables/T_ProjInfoTable.asset", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_PROJ_INFO_TABLE}.asset"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scriptables/T_DeviceInfoTable.asset", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_DEVICE_INFO_TABLE}.asset"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scriptables/T_LocalizeInfoTable.asset", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_LOCALIZE_INFO_TABLE}.asset"),

		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Settings/T_LightingSettings.lighting", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_NORM_QUALITY_LIGHTING_SETTINGS}.lighting"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Settings/T_LightingSettings.lighting", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_HIGH_QUALITY_LIGHTING_SETTINGS}.lighting"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Settings/T_LightingSettings.lighting", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_ULTRA_QUALITY_LIGHTING_SETTINGS}.lighting"),

#if POST_PROCESSING_MODULE_ENABLE
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Settings/T_PostProcessingSettings.asset", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_NORM_QUALITY_POST_PROCESSING_SETTINGS}.asset"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Settings/T_PostProcessingSettings.asset", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_HIGH_QUALITY_POST_PROCESSING_SETTINGS}.asset"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Settings/T_PostProcessingSettings.asset", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_ULTRA_QUALITY_POST_PROCESSING_SETTINGS}.asset"),
#endif // #if POST_PROCESSING_MODULE_ENABLE

#if ADS_MODULE_ENABLE || FLURRY_MODULE_ENABLE || APPS_FLYER_MODULE_ENABLE
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scriptables/T_PluginInfoTable.asset", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_PLUGIN_INFO_TABLE}.asset"),
#endif // #if ADS_MODULE_ENABLE || FLURRY_MODULE_ENABLE || APPS_FLYER_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scriptables/T_ProductInfoTable.asset", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_PRODUCT_INFO_TABLE}.asset"),
#endif // #if PURCHASE_MODULE_ENABLE
		// 01-UnityProject }

		// 02-SubUnityProject
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}SpriteAtlases/T_SpriteAtlas.spriteatlas", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_SPRITE_ATLAS_01}.spriteatlas"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}SpriteAtlases/T_SpriteAtlas.spriteatlas", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_SPRITE_ATLAS_02}.spriteatlas"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}SpriteAtlases/T_SpriteAtlas.spriteatlas", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_SPRITE_ATLAS_03}.spriteatlas"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}SpriteAtlases/T_SpriteAtlas.spriteatlas", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_SPRITE_ATLAS_04}.spriteatlas"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}SpriteAtlases/T_SpriteAtlas.spriteatlas", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_SPRITE_ATLAS_05}.spriteatlas"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}SpriteAtlases/T_SpriteAtlas.spriteatlas", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_SPRITE_ATLAS_06}.spriteatlas"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}SpriteAtlases/T_SpriteAtlas.spriteatlas", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_SPRITE_ATLAS_07}.spriteatlas"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}SpriteAtlases/T_SpriteAtlas.spriteatlas", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_SPRITE_ATLAS_08}.spriteatlas"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}SpriteAtlases/T_SpriteAtlas.spriteatlas", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_SPRITE_ATLAS_09}.spriteatlas"),

		// 04-SubUnityProjectEditor
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}SpriteAtlases/T_SpriteAtlas.spriteatlas", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR_RESOURCES}{KCDefine.U_ASSET_P_ES_SPRITE_ATLAS_01}.spriteatlas"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}SpriteAtlases/T_SpriteAtlas.spriteatlas", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR_RESOURCES}{KCDefine.U_ASSET_P_ES_SPRITE_ATLAS_02}.spriteatlas"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}SpriteAtlases/T_SpriteAtlas.spriteatlas", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR_RESOURCES}{KCDefine.U_ASSET_P_ES_SPRITE_ATLAS_03}.spriteatlas"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}SpriteAtlases/T_SpriteAtlas.spriteatlas", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR_RESOURCES}{KCDefine.U_ASSET_P_ES_SPRITE_ATLAS_04}.spriteatlas"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}SpriteAtlases/T_SpriteAtlas.spriteatlas", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR_RESOURCES}{KCDefine.U_ASSET_P_ES_SPRITE_ATLAS_05}.spriteatlas"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}SpriteAtlases/T_SpriteAtlas.spriteatlas", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR_RESOURCES}{KCDefine.U_ASSET_P_ES_SPRITE_ATLAS_06}.spriteatlas"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}SpriteAtlases/T_SpriteAtlas.spriteatlas", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR_RESOURCES}{KCDefine.U_ASSET_P_ES_SPRITE_ATLAS_07}.spriteatlas"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}SpriteAtlases/T_SpriteAtlas.spriteatlas", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR_RESOURCES}{KCDefine.U_ASSET_P_ES_SPRITE_ATLAS_08}.spriteatlas"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}SpriteAtlases/T_SpriteAtlas.spriteatlas", $"{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ_EDITOR_RESOURCES}{KCDefine.U_ASSET_P_ES_SPRITE_ATLAS_09}.spriteatlas")
	};

	public static readonly List<(string, string)> B_PIPELINE_P_INFO_LIST = new List<(string, string)>() {
		// 01-UnityProject {
#if UNIVERSAL_RENDERING_PIPELINE_ENABLE || UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Pipelines/T_UniversalRPAsset.asset", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_NORM_QUALITY_UNIVERSAL_RP}.asset"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Pipelines/T_UniversalRPAsset.asset", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_HIGH_QUALITY_UNIVERSAL_RP}.asset"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Pipelines/T_UniversalRPAsset.asset", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCDefine.U_ASSET_P_G_ULTRA_QUALITY_UNIVERSAL_RP}.asset")
#endif // #if UNIVERSAL_RENDERING_PIPELINE_ENABLE || UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE
		// 01-UnityProject }
	};

	public static readonly List<(string, string)> B_SCENE_P_INFO_LIST = new List<(string, string)>() {
		// 00-AutoCreate {
#if SCENE_TEMPLATES_ENABLE || SCENE_TEMPLATES_MODULE_ENABLE
		(KCEditorDefine.B_ASSET_P_SAMPLE_SCENE, $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scenes/{KCDefine.B_SCENE_N_INIT}.unity"),
		(KCEditorDefine.B_ASSET_P_SAMPLE_SCENE, $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scenes/{KCDefine.B_SCENE_N_START}.unity"),
		(KCEditorDefine.B_ASSET_P_SAMPLE_SCENE, $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scenes/{KCDefine.B_SCENE_N_SETUP}.unity"),
		(KCEditorDefine.B_ASSET_P_SAMPLE_SCENE, $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scenes/{KCDefine.B_SCENE_N_AGREE}.unity"),
		(KCEditorDefine.B_ASSET_P_SAMPLE_SCENE, $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scenes/{KCDefine.B_SCENE_N_LATE_SETUP}.unity"),

#if STUDY_ENABLE || STUDY_MODULE_ENABLE
		(KCEditorDefine.B_ASSET_P_MENU_SAMPLE_SCENE, $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_AUTO_CREATE}Scenes/{KCDefine.B_SCENE_N_MENU}.unity"),
#endif // #if STUDY_ENABLE || STUDY_MODULE_ENABLE
#endif // #if SCENE_TEMPLATES_ENABLE || SCENE_TEMPLATES_MODULE_ENABLE
		// 00-AutoCreate }
		
		// 01-UnityProject {
#if UTILITY_SCRIPT_TEMPLATES_ENABLE || UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		(KCEditorDefine.B_ASSET_P_SAMPLE_SCENE, $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scenes/{KCDefine.B_SCENE_N_TITLE}.unity"),
		(KCEditorDefine.B_ASSET_P_SAMPLE_SCENE, $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scenes/{KCDefine.B_SCENE_N_MAIN}.unity"),
		(KCEditorDefine.B_ASSET_P_SAMPLE_SCENE, $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scenes/{KCDefine.B_SCENE_N_GAME}.unity"),
		(KCEditorDefine.B_ASSET_P_SAMPLE_SCENE, $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scenes/{KCDefine.B_SCENE_N_LOADING}.unity"),
		(KCEditorDefine.B_ASSET_P_SAMPLE_SCENE, $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scenes/{KCDefine.B_SCENE_N_OVERLAY}.unity"),
		(KCEditorDefine.B_ASSET_P_SAMPLE_SCENE, $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scenes/{KCDefine.B_SCENE_N_TEST}.unity"),
#endif // #if UTILITY_SCRIPT_TEMPLATES_ENABLE || UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		// 01-UnityProject }

		// 03-UnityProjectEditor {
#if EDITOR_SCENE_TEMPLATES_ENABLE || EDITOR_SCENE_TEMPLATES_MODULE_ENABLE
		(KCEditorDefine.B_ASSET_P_EDITOR_SAMPLE_SCENE, $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ_EDITOR}Scenes/{KCDefine.B_SCENE_N_LEVEL_EDITOR}.unity")
#endif // #if EDITOR_SCENE_TEMPLATES_ENABLE || EDITOR_SCENE_TEMPLATES_MODULE_ENABLE
		// 03-UnityProjectEditor }
	};

	public static readonly List<(string, string)> B_ASSEMBLY_DEFINE_P_INFO_LIST = new List<(string, string)>() {
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}AssemblyDefines/T_Module.UnityIronSrc.asmdef.t", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}IronSource/Module.UnityIronSrc.asmdef"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}AssemblyDefines/T_Module.UnityIronSrc.Editor.asmdef.t", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}IronSource/Editor/Module.UnityIronSrc.Editor.asmdef"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}AssemblyDefines/T_Module.UnityFlurry.asmdef.t", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}Plugins/FlurrySDK/Module.UnityFlurry.asmdef"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}AssemblyDefines/T_Module.UnityUnityPurchasing.asmdef.t", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}Scripts/UnityPurchasing/generated/Module.UnityUnityPurchasing.asmdef"),
		($"{KCEditorDefine.B_ABS_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_AUTO_CREATE}AssemblyDefines/T_Module.UnityPlayfabParty.asmdef.t", $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}PlayFabPartySDK/Module.UnityPlayfabParty.asmdef")
	};

	public static readonly List<(string, string)> B_ICON_P_INFO_LIST = new List<(string, string)>() {
		// 01-UnityProject {
		// iOS
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Icons/iOS/App/T_Icon76x76.png", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCEditorDefine.B_ICON_P_IOS_APP_76x76}.png"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Icons/iOS/App/T_Icon120x120.png", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCEditorDefine.B_ICON_P_IOS_APP_120x120}.png"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Icons/iOS/App/T_Icon152x152.png", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCEditorDefine.B_ICON_P_IOS_APP_152x152}.png"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Icons/iOS/App/T_Icon167x167.png", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCEditorDefine.B_ICON_P_IOS_APP_167x167}.png"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Icons/iOS/App/T_Icon180x180.png", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCEditorDefine.B_ICON_P_IOS_APP_180x180}.png"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Icons/iOS/App/T_Icon1024x1024.png", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCEditorDefine.B_ICON_P_IOS_APP_1024x1024}.png"),

		// 안드로이드
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Icons/Android/App/T_Icon36x36.png", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCEditorDefine.B_ICON_P_ANDROID_APP_36x36}.png"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Icons/Android/App/T_Icon48x48.png", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCEditorDefine.B_ICON_P_ANDROID_APP_48x48}.png"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Icons/Android/App/T_Icon72x72.png", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCEditorDefine.B_ICON_P_ANDROID_APP_72x72}.png"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Icons/Android/App/T_Icon96x96.png", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCEditorDefine.B_ICON_P_ANDROID_APP_96x96}.png"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Icons/Android/App/T_Icon144x144.png", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCEditorDefine.B_ICON_P_ANDROID_APP_144x144}.png"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Icons/Android/App/T_Icon192x192.png", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCEditorDefine.B_ICON_P_ANDROID_APP_192x192}.png"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Icons/Android/App/T_Icon512x512.png", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCEditorDefine.B_ICON_P_ANDROID_APP_512x512}.png"),

		// 독립 플랫폼
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Icons/Standalone/App/T_Icon.png", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCEditorDefine.B_ICON_P_STANDALONE_APP}.png"),

#if NOTI_MODULE_ENABLE
		// iOS
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Icons/iOS/Notification/T_Icon20x20.png", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCEditorDefine.B_ICON_P_IOS_NOTI_20x20}.png"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Icons/iOS/Notification/T_Icon40x40.png", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCEditorDefine.B_ICON_P_IOS_NOTI_40x40}.png"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Icons/iOS/Notification/T_Icon60x60.png", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCEditorDefine.B_ICON_P_IOS_NOTI_60x60}.png"),

		// 안드로이드
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Icons/Android/Notification/T_Icon24x24.png", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCEditorDefine.B_ICON_P_ANDROID_NOTI_24x24}.png"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Icons/Android/Notification/T_Icon36x36.png", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCEditorDefine.B_ICON_P_ANDROID_NOTI_36x36}.png"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Icons/Android/Notification/T_Icon48x48.png", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCEditorDefine.B_ICON_P_ANDROID_NOTI_48x48}.png"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Icons/Android/Notification/T_Icon72x72.png", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCEditorDefine.B_ICON_P_ANDROID_NOTI_72x72}.png"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Icons/Android/Notification/T_Icon96x96.png", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCEditorDefine.B_ICON_P_ANDROID_NOTI_96x96}.png"),
		($"{KCEditorDefine.B_DIR_P_TEMPLATES}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Icons/Android/Notification/T_Icon256x256.png", $"{KCEditorDefine.B_DIR_P_UNITY_PROJ_RESOURCES}{KCEditorDefine.B_ICON_P_ANDROID_NOTI_256x256}.png")
#endif // #if NOTI_MODULE_ENABLE
		// 01-UnityProject }
	};

	public static readonly List<(string, string)> B_ASSET_P_MOVE_INFO_LIST = new List<(string, string)>() {
#if MODULE_VER_2_7_0_OR_NEWER
		($"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Global/Utility/Base/CEItem.cs", $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Global/Utility/Object/CEItem.cs"),
		($"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Global/Utility/Base/CESkill.cs", $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Global/Utility/Object/CESkill.cs"),
		($"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Global/Utility/Base/CEObj.cs", $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Global/Utility/Object/CEObj.cs"),
		($"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Global/Utility/Base/CEFX.cs", $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_UNITY_PROJ}Scripts/Engine/Global/Utility/Object/CEFX.cs"),

		($"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Global/Utility/Base/CEItem+Sub.cs", $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Global/Utility/Object/CEItem+Sub.cs"),
		($"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Global/Utility/Base/CESkill+Sub.cs", $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Global/Utility/Object/CESkill+Sub.cs"),
		($"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Global/Utility/Base/CEObj+Sub.cs", $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Global/Utility/Object/CEObj+Sub.cs"),
		($"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Global/Utility/Base/CEFX+Sub.cs", $"{KCEditorDefine.B_DIR_P_ASSETS}{KCEditorDefine.B_DIR_P_SUB_UNITY_PROJ}Scripts/Engine/Global/Utility/Object/CEFX+Sub.cs")
#endif // #if MODULE_VER_2_7_0_OR_NEWER
	};
	// 경로 }

	// 계층 뷰
	public static readonly Color B_COLOR_HIERARCHY_TEXT = new Color(1.0f, 0.27f, 0.0f, 1.0f);
	public static readonly Color B_COLOR_HIERARCHY_OUTLINE = Color.black;

	// 알림 {
	public static readonly string B_DATA_P_NOTI_PROJ_PROPERTIES = $"{KCEditorDefine.B_ABS_DIR_P_ANDROID_PLUGINS}/AndroidNativePlugin.androidlib/project.properties";
	public static readonly string B_DATA_P_NOTI_ANDROID_MANIFEST = $"{KCEditorDefine.B_ABS_DIR_P_ANDROID_PLUGINS}/AndroidNativePlugin.androidlib/AndroidManifest.xml";

	public static readonly List<(string, string)> B_NOTI_ICON_P_INFO_LIST = new List<(string, string)>() {
		($"{KCEditorDefine.B_ABS_DIR_P_UNITY_PROJ_RESOURCES}{KCEditorDefine.B_ICON_P_ANDROID_NOTI_96x96}.png", $"{KCEditorDefine.B_ABS_DIR_P_ANDROID_PLUGINS}/AndroidNativePlugin.androidlib/res/drawable/{KCDefine.U_ICON_N_ANDROID_NOTI_SMALL}.png"),
		($"{KCEditorDefine.B_ABS_DIR_P_UNITY_PROJ_RESOURCES}{KCEditorDefine.B_ICON_P_ANDROID_APP_192x192}.png", $"{KCEditorDefine.B_ABS_DIR_P_ANDROID_PLUGINS}/AndroidNativePlugin.androidlib/res/drawable/{KCDefine.U_ICON_N_ANDROID_NOTI_LARGE}.png")
	};
	// 알림 }

	// 에셋 임포터 {
	public static List<string> B_AUDIO_IMPORTER_PLATFORM_NAME_LIST = new List<string>() {
		NamedBuildTarget.iOS.TargetName, NamedBuildTarget.Android.TargetName, NamedBuildTarget.Standalone.TargetName
	};

	public static List<string> B_TEX_IMPORTER_PLATFORM_NAME_LIST = new List<string>() {
		NamedBuildTarget.iOS.TargetName, NamedBuildTarget.Android.TargetName, NamedBuildTarget.Standalone.TargetName
	};
	// 에셋 임포터 }

	// iOS {
	public static readonly string B_ABS_BUILD_P_IOS = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../Builds/iOS";
	public static readonly string B_PLUGIN_PROJ_P_IOS = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../NativePlugins/Client/iOS";

	public static readonly string B_SRC_PLUGIN_P_IOS = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../NativePlugins/Client/iOS/Classes/Plugin/";
	public static readonly string B_DEST_PLUGIN_P_IOS = $"{KCEditorDefine.B_ABS_DIR_P_IOS_PLUGINS}iOSNativePlugin/";

	public static readonly string B_ENTITLEMENTS_P_CAPABILITY_IOS = $"{Application.identifier}.entitlements";

	public static readonly List<GraphicsDeviceType> B_GRAPHICS_DEVICE_TYPE_LIST_IOS = new List<GraphicsDeviceType>() {
		GraphicsDeviceType.Metal
	};
	// iOS }

	// 안드로이드 {
	public static readonly string B_ABS_BUILD_P_FMT_ANDROID = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../Builds/Android/{"{0}"}";

	public static readonly string B_SRC_PLUGIN_P_ANDROID = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../NativePlugins/Client/Android/app/build/outputs/aar/app-release.aar";
	public static readonly string B_DEST_PLUGIN_P_ANDROID = $"{KCEditorDefine.B_ABS_DIR_P_ANDROID_PLUGINS}AndroidNativePlugin.aar";

	public static readonly string B_SRC_MANIFEST_P_ANDROID = $"{KCEditorDefine.B_ABS_DIR_P_PACKAGES}Options/Android/AndroidManifest.xml";
	public static readonly string B_DEST_MANIFEST_P_ANDROID = $"{KCEditorDefine.B_ABS_DIR_P_ANDROID_PLUGINS}AndroidManifest.xml";
	public static readonly string B_ORIGIN_SRC_MANIFEST_P_ANDROID = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Options/Android/AndroidManifest.xml";

	public static readonly string B_SRC_MAIN_TEMPLATE_P_ANDROID = $"{KCEditorDefine.B_ABS_DIR_P_PACKAGES}Options/Android/mainTemplate.gradle";
	public static readonly string B_DEST_MAIN_TEMPLATE_P_ANDROID = $"{KCEditorDefine.B_ABS_DIR_P_ANDROID_PLUGINS}mainTemplate.gradle";
	public static readonly string B_ORIGIN_SRC_MAIN_TEMPLATE_P_ANDROID = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Options/Android/mainTemplate.gradle";

	public static readonly string B_SRC_LAUNCHER_TEMPLATE_P_ANDROID = $"{KCEditorDefine.B_ABS_DIR_P_PACKAGES}Options/Android/launcherTemplate.gradle";
	public static readonly string B_DEST_LAUNCHER_TEMPLATE_P_ANDROID = $"{KCEditorDefine.B_ABS_DIR_P_ANDROID_PLUGINS}launcherTemplate.gradle";
	public static readonly string B_ORIGIN_SRC_LAUNCHER_TEMPLATE_P_ANDROID = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Options/Android/launcherTemplate.gradle";

	public static readonly string B_SRC_BASE_PROJ_TEMPLATE_P_ANDROID = $"{KCEditorDefine.B_ABS_DIR_P_PACKAGES}Options/Android/baseProjectTemplate.gradle";
	public static readonly string B_DEST_BASE_PROJ_TEMPLATE_P_ANDROID = $"{KCEditorDefine.B_ABS_DIR_P_ANDROID_PLUGINS}baseProjectTemplate.gradle";
	public static readonly string B_ORIGIN_SRC_BASE_PROJ_TEMPLATE_P_ANDROID = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Options/Android/baseProjectTemplate.gradle";

	public static readonly string B_SRC_GRADLE_TEMPLATE_P_ANDROID = $"{KCEditorDefine.B_ABS_DIR_P_PACKAGES}Options/Android/gradleTemplate.properties";
	public static readonly string B_DEST_GRADLE_TEMPLATE_P_ANDROID = $"{KCEditorDefine.B_ABS_DIR_P_ANDROID_PLUGINS}gradleTemplate.properties";
	public static readonly string B_ORIGIN_SRC_GRADLE_TEMPLATE_P_ANDROID = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Options/Android/gradleTemplate.properties";

#if UNITY_EDITOR_WIN
	public static readonly string B_SRC_UNITY_PLUGIN_P_ANDROID = $"{KCEditorDefine.B_ABS_DIR_P_UNITY_ENGINE}../Data/PlaybackEngines/AndroidPlayer/Variations/il2cpp/Release/Classes/classes.jar";
#else
	public static readonly string B_SRC_UNITY_PLUGIN_P_ANDROID = $"{KCEditorDefine.B_ABS_DIR_P_UNITY_ENGINE}../PlaybackEngines/AndroidPlayer/Variations/il2cpp/Release/Classes/classes.jar";
#endif // #if UNITY_EDITOR_WIN

	public static readonly string B_DEST_UNITY_PLUGIN_P_ANDROID = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../NativePlugins/Client/Android/unityLibrary/libs/unity-classes.jar";

#if UNITY_EDITOR_WIN
	public static readonly string B_SRC_LOCAL_TEMPLATE_P_ANDROID = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Options/Android/localWindows.properties";
	public static readonly string B_DEST_LOCAL_TEMPLATE_P_ANDROID = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../NativePlugins/Client/Android/local.properties";
#else
	public static readonly string B_SRC_LOCAL_TEMPLATE_P_ANDROID = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../UnityPackages/Client/Templates/Options/Android/localMac.properties";
	public static readonly string B_DEST_LOCAL_TEMPLATE_P_ANDROID = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../NativePlugins/Client/Android/local.properties";
#endif // #if UNITY_EDITOR_WIN

	public static readonly List<GraphicsDeviceType> B_GRAPHICS_DEVICE_TYPE_LIST_ANDROID = new List<GraphicsDeviceType>() {
		GraphicsDeviceType.Vulkan, GraphicsDeviceType.OpenGLES3
	};
	// 안드로이드 }

	// 독립 플랫폼
	public static readonly string B_ABS_BUILD_P_FMT_STANDALONE = $"{KCEditorDefine.B_ABS_DIR_P_ASSETS}../Builds/Standalone/{"{0}"}";
	public static readonly string B_DIR_P_FMT_EXTERNAL_DATAS_STANDALONE = $"{"{0}"}/{KCDefine.B_DIR_N_EXTERNAL_DATAS}/";

	// 맥
	public static readonly List<GraphicsDeviceType> B_GRAPHICS_DEVICE_TYPE_LIST_MAC = new List<GraphicsDeviceType>() {
		GraphicsDeviceType.Metal, GraphicsDeviceType.OpenGLCore
	};

	// 윈도우즈
	public static readonly List<GraphicsDeviceType> B_GRAPHICS_DEVICE_TYPE_LIST_WNDS = new List<GraphicsDeviceType>() {
		GraphicsDeviceType.Direct3D12, GraphicsDeviceType.Direct3D11, GraphicsDeviceType.OpenGLCore
	};

	// 젠킨스 {
	public static readonly string B_JENKINS_IOS_PIPELINE = string.Format($"{KCEditorDefine.B_PIPELINE_GROUP_NAME_FMT_JENKINS}/01.iOS", KCEditorDefine.B_VER_UNITY_MODULE);
	public static readonly string B_JENKINS_ANDROID_PIPELINE = string.Format($"{KCEditorDefine.B_PIPELINE_GROUP_NAME_FMT_JENKINS}/11.Android", KCEditorDefine.B_VER_UNITY_MODULE);
	public static readonly string B_JENKINS_STANDALONE_PIPELINE = string.Format($"{KCEditorDefine.B_PIPELINE_GROUP_NAME_FMT_JENKINS}/41.Standalone", KCEditorDefine.B_VER_UNITY_MODULE);

	public static readonly Dictionary<EiOSType, Dictionary<string, string>> B_JENKINS_IOS_SOURCES = new Dictionary<EiOSType, Dictionary<string, string>>() {
		[EiOSType.APPLE] = new Dictionary<string, string>() {
			[KCEditorDefine.B_DEBUG_BUILD_FUNC_JENKINS] = "01.iOSAppleDebug",
			[KCEditorDefine.B_RELEASE_BUILD_FUNC_JENKINS] = "11.iOSAppleRelease",
			[KCEditorDefine.B_STORE_A_BUILD_FUNC_JENKINS] = "21.iOSAppleStoreA",
			[KCEditorDefine.B_STORE_DIST_BUILD_FUNC_JENKINS] = "31.iOSAppleStoreDist"
		}
	};

	public static readonly Dictionary<EAndroidType, Dictionary<string, string>> B_JENKINS_ANDROID_SOURCES = new Dictionary<EAndroidType, Dictionary<string, string>>() {
		[EAndroidType.GOOGLE] = new Dictionary<string, string>() {
			[KCEditorDefine.B_DEBUG_BUILD_FUNC_JENKINS] = "01.AndroidGoogleDebug",
			[KCEditorDefine.B_RELEASE_BUILD_FUNC_JENKINS] = "11.AndroidGoogleRelease",
			[KCEditorDefine.B_STORE_A_BUILD_FUNC_JENKINS] = "21.AndroidGoogleStoreA",
			[KCEditorDefine.B_STORE_B_BUILD_FUNC_JENKINS] = "22.AndroidGoogleStoreB",
			[KCEditorDefine.B_STORE_DIST_BUILD_FUNC_JENKINS] = "31.AndroidGoogleStoreDist"
		},

		[EAndroidType.AMAZON] = new Dictionary<string, string>() {
			[KCEditorDefine.B_DEBUG_BUILD_FUNC_JENKINS] = "01.AndroidAmazonDebug",
			[KCEditorDefine.B_RELEASE_BUILD_FUNC_JENKINS] = "11.AndroidAmazonRelease",
			[KCEditorDefine.B_STORE_A_BUILD_FUNC_JENKINS] = "21.AndroidAmazonStoreA",
			[KCEditorDefine.B_STORE_B_BUILD_FUNC_JENKINS] = "22.AndroidAmazonStoreB",
			[KCEditorDefine.B_STORE_DIST_BUILD_FUNC_JENKINS] = "31.AndroidAmazonStoreDist"
		}
	};

	public static readonly Dictionary<EStandaloneType, Dictionary<string, string>> B_JENKINS_STANDALONE_SOURCES = new Dictionary<EStandaloneType, Dictionary<string, string>>() {
		[EStandaloneType.MAC_STEAM] = new Dictionary<string, string>() {
			[KCEditorDefine.B_DEBUG_BUILD_FUNC_JENKINS] = "01.StandaloneMacSteamDebug",
			[KCEditorDefine.B_RELEASE_BUILD_FUNC_JENKINS] = "11.StandaloneMacSteamRelease",
			[KCEditorDefine.B_STORE_A_BUILD_FUNC_JENKINS] = "21.StandaloneMacSteamStoreA",
			[KCEditorDefine.B_STORE_DIST_BUILD_FUNC_JENKINS] = "31.StandaloneMacSteamStoreDist"
		},

		[EStandaloneType.WNDS_STEAM] = new Dictionary<string, string>() {
			[KCEditorDefine.B_DEBUG_BUILD_FUNC_JENKINS] = "01.StandaloneWndsSteamDebug",
			[KCEditorDefine.B_RELEASE_BUILD_FUNC_JENKINS] = "11.StandaloneWndsSteamRelease",
			[KCEditorDefine.B_STORE_A_BUILD_FUNC_JENKINS] = "21.StandaloneWndsSteamStoreA",
			[KCEditorDefine.B_STORE_DIST_BUILD_FUNC_JENKINS] = "31.StandaloneWndsSteamStoreDist"
		}
	};
	// 젠킨스 }
	#endregion // 런타임 상수

	#region 조건부 상수
#if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE
	// 이름 {
	public const string U_FIELD_N_UNIVERSAL_RP_MSAA_QUALITY = "m_MSAA";
	public const string U_FIELD_N_UNIVERSAL_RP_CASCADE_BORDER = "m_CascadeBorder";
	public const string U_FIELD_N_UNIVERSAL_RP_OPAQUE_DOWN_SAMPLING = "m_OpaqueDownsampling";

	public const string U_FIELD_N_UNIVERSAL_RP_CASCADE_2_SPLIT = "m_Cascade2Split";
	public const string U_FIELD_N_UNIVERSAL_RP_CASCADE_3_SPLIT = "m_Cascade3Split";
	public const string U_FIELD_N_UNIVERSAL_RP_CASCADE_4_SPLIT = "m_Cascade4Split";

	public const string U_FIELD_N_UNIVERSAL_RP_RENDERER_DATAS = "m_RendererDataList";
	public const string U_FIELD_N_UNIVERSAL_RP_SUPPORTS_SOFT_SHADOW = "m_SoftShadowsSupported";
	public const string U_FIELD_N_UNIVERSAL_RP_SUPPORTS_TERRAIN_HOLES = "m_SupportsTerrainHoles";

	public const string U_FIELD_N_UNIVERSAL_RP_REFLECTION_PROBE_BLENDING = "m_ReflectionProbeBlending";
	public const string U_FIELD_N_UNIVERSAL_RP_REFLECTION_PROBE_BOX_PROJECTION = "m_ReflectionProbeBoxProjection";

	public const string U_FIELD_N_UNIVERSAL_RP_MAIN_LIGHT_RENDERING_MODE = "m_MainLightRenderingMode";
	public const string U_FIELD_N_UNIVERSAL_RP_MAIN_LIGHT_SUPPORTS_SHADOW = "m_MainLightShadowsSupported";
	public const string U_FIELD_N_UNIVERSAL_RP_USE_FAST_SRGB_LINEAR_CONVERSION = "m_UseFastSRGBLinearConversion";
	public const string U_FIELD_N_UNIVERSAL_RP_MAIN_LIGHT_SHADOW_MAP_RESOLUTION = "m_MainLightShadowmapResolution";

	public const string U_FIELD_N_UNIVERSAL_RP_ADDITIONAL_LIGHT_COOKIE_FMT = "m_AdditionalLightsCookieFormat";
	public const string U_FIELD_N_UNIVERSAL_RP_ADDITIONAL_LIGHT_PER_OBJ_LIMIT = "m_AdditionalLightsPerObjectLimit";
	public const string U_FIELD_N_UNIVERSAL_RP_ADDITIONAL_LIGHT_RENDERING_MODE = "m_AdditionalLightsRenderingMode";
	public const string U_FIELD_N_UNIVERSAL_RP_ADDITIONAL_LIGHT_SUPPORTS_SHADOW = "m_AdditionalLightShadowsSupported";
	public const string U_FIELD_N_UNIVERSAL_RP_ADDITIONAL_LIGHT_COOKIE_RESOLUTION = "m_AdditionalLightsCookieResolution";
	public const string U_FIELD_N_UNIVERSAL_RP_ADDITIONAL_LIGHT_SHADOW_MAP_RESOLUTION = "m_AdditionalLightsShadowmapResolution";

	public const string B_PROPERTY_N_UNIVERSAL_RP_STRIP_DEBUG_VARIANTS = "m_StripDebugVariants";
	public const string B_PROPERTY_N_UNIVERSAL_RP_STRIP_UNUSED_VARIANTS = "m_StripUnusedVariants";
	public const string B_PROPERTY_N_UNIVERSAL_RP_STRIP_UNUSED_POST_PROCESSING_VARIANTS = "m_StripUnusedPostProcessingVariants";
	// 이름 }
#endif // #if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE

#if BURST_COMPILER_MODULE_ENABLE
	// 식별자 {
	public const string B_KEY_BURST_AS_OPTIMIZE_FOR = "OptimizeFor";
	public const string B_KEY_BURST_AS_MONO_BEHAVIOUR = "MonoBehaviour";
	public const string B_KEY_BURST_AS_USE_PLATFORM_SDK_LINKER = "UsePlatformSDKLinker";

	public const string B_KEY_BURST_AS_ENABLE_OPTIMISATIONS = "EnableOptimisations";
	public const string B_KEY_BURST_AS_ENABLE_BURST_COMPILATION = "EnableBurstCompilation";
	public const string B_KEY_BURST_AS_ENABLE_SAFETY_CHECKS = "EnableSafetyChecks";
	public const string B_KEY_BURST_AS_ENABLE_DEBUG_IN_ALL_BUILDS = "EnableDebugInAllBuilds";
	// 식별자 }
#endif // #if BURST_COMPILER_MODULE_ENABLE

#if NOTI_MODULE_ENABLE
	// 옵션
	public const PresentationOption B_PRESENT_OPTS_REMOTE_NOTI = PresentationOption.Alert | PresentationOption.Badge | PresentationOption.Sound;
	public const AuthorizationOption B_PRESENT_OPTS_AUTHORIZATION_NOTI = AuthorizationOption.Alert | AuthorizationOption.Badge | AuthorizationOption.Sound;

	// 이름
	public const string B_ACTIVITY_N_NOTI = "com.unity3d.player.UnityPlayerActivity";
#endif // #if NOTI_MODULE_ENABLE
	#endregion // 조건부 상수

	#region 조건부 런타임 상수
#if LOCALIZE_MODULE_ENABLE
	// 이름
	public const string B_PROPERTY_N_LOCALIZE_INITIALIZE_SYNCHRONOUSLY = "m_InitializeSynchronously";

	// 경로
	public static readonly string B_ASSET_P_LOCALIZE_SETTINGS = $"{KCEditorDefine.B_DIR_P_ASSETS}LocalizationSettings.asset";
#endif // #if LOCALIZE_MODULE_ENABLE

#if ML_AGENTS_MODULE_ENABLE
	// 기타
	public const int B_PORT_NUMBER_ML_AGENTS_EDITOR = 7080;

	// 이름 {
	public const string B_CLS_N_ML_AGENTS_SETTINGS = "Unity.MLAgents.MLAgentsSettings";

	public const string B_PROPERTY_N_ML_AGENTS_CONNECT_TRAINER = "m_ConnectTrainer";
	public const string B_PROPERTY_N_ML_AGENTS_EDITOR_PORT = "m_EditorPort";

	public const string B_ASSEMBLY_N_ML_AGENTS = "Unity.ML-Agents";
	public const string B_ASSEMBLY_N_ML_AGENTS_EDITOR = "Editor";
	// 이름 }

	// 경로
	public static readonly string B_ASSET_P_ML_AGENTS_SETTINGS = $"{KCEditorDefine.B_DIR_P_ASSETS}MLAgentsSettings.mlagents.asset";
#endif // #if ML_AGENTS_MODULE_ENABLE

#if INPUT_SYSTEM_MODULE_ENABLE
	// 경로
	public static readonly string B_ASSET_P_INPUT_SETTINGS = $"{KCEditorDefine.B_DIR_P_ASSETS}InputSystem.inputsettings.asset";
#endif // #if INPUT_SYSTEM_MODULE_ENABLE

#if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE
	// 경로
	public static readonly string B_ASSET_P_UNIVERSAL_RP_SETTINGS = $"{KCEditorDefine.B_DIR_P_ASSETS}UniversalRenderPipelineGlobalSettings.asset";
#endif // #if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE

#if BURST_COMPILER_MODULE_ENABLE
	// 경로
	public static readonly string B_DATA_P_IOS_BURST_AOT_SETTINGS = $"{KCEditorDefine.B_ABS_DIR_P_PROJ_SETTINGS}BurstAotSettings_iOS.json";
	public static readonly string B_DATA_P_ANDROID_BURST_AOT_SETTINGS = $"{KCEditorDefine.B_ABS_DIR_P_PROJ_SETTINGS}BurstAotSettings_Android.json";
	public static readonly string B_DATA_P_MAC_BURST_AOT_SETTINGS = $"{KCEditorDefine.B_ABS_DIR_P_PROJ_SETTINGS}BurstAotSettings_StandaloneOSX.json";
	public static readonly string B_DATA_P_WNDS_BURST_AOT_SETTINGS = $"{KCEditorDefine.B_ABS_DIR_P_PROJ_SETTINGS}BurstAotSettings_StandaloneWindows.json";
#endif // #if BURST_COMPILER_MODULE_ENABLE
	#endregion // 조건부 런타임 상수
}
#endif // #if UNITY_EDITOR

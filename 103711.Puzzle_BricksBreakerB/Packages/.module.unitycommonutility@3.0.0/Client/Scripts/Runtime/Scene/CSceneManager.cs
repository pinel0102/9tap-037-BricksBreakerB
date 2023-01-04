using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using Coffee.UIExtensions;
using AOP;

#if UNITY_EDITOR
using UnityEditor;
#endif // #if UNITY_EDITOR

#if INPUT_SYSTEM_MODULE_ENABLE
using UnityEngine.InputSystem;
#endif // #if INPUT_SYSTEM_MODULE_ENABLE

#if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE
using UnityEngine.Rendering.Universal;
#endif // #if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE

/** 씬 관리자 */
public abstract partial class CSceneManager : CComponent {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		IS_REBUILD_LAYOUT,
		BG_TOUCH_DISPATCHER,

		MAIN_CAMERA,
		EVENT_SYSTEM,
		BASE_INPUT_MODULE,
		ACTIVE_SCENE_MANAGER,

		// 캔버스
		UIS_CANVAS,
		POPUP_UIS_CANVAS,
		TOPMOST_UIS_CANVAS,

		// UI
		UIS,
		UIS_ROOT,
		UIS_BASE,
		TEST_UIS,
		TEST_CONTENTS_UIS,
		PIVOT_UIS,
		ANCHOR_UIS,
		CORNER_ANCHOR_UIS,
		BG_TOUCH_RESPONDER,
		DESIGN_RESOLUTION_GUIDE_UIS,

		// 고정 UI
		UP_UIS,
		DOWN_UIS,
		LEFT_UIS,
		RIGHT_UIS,

		// 코너 고정 UI
		UP_LEFT_UIS,
		UP_RIGHT_UIS,
		DOWN_LEFT_UIS,
		DOWN_RIGHT_UIS,

		// 팝업 UI
		POPUP_UIS,
		TOPMOST_UIS,

		// 객체
		BASE,
		OBJS,
		OBJS_ROOT,
		PIVOT_OBJS,
		ANCHOR_OBJS,
		STATIC_OBJS,
		ADDITIONAL_LIGHTS,
		ADDITIONAL_CAMERAS,
		REFLECTION_PROBES,
		LIGHT_PROBE_GROUPS,

		// 고정 객체
		UP_OBJS,
		DOWN_OBJS,
		LEFT_OBJS,
		RIGHT_OBJS,

#if DEBUG || DEVELOPMENT_BUILD || PLAY_TEST_ENABLE
		TEST_UIS_OPEN_BTN,
		TEST_UIS_CLOSE_BTN,

#if DEBUG || DEVELOPMENT_BUILD
		STATIC_DEBUG_STR_BUILDER,
		DYNAMIC_DEBUG_STR_BUILDER,
		EXTRA_STATIC_DEBUG_STR_BUILDER,
		EXTRA_DYNAMIC_DEBUG_STR_BUILDER,
#endif // #if DEBUG || DEVELOPMENT_BUILD
#endif // #if DEBUG || DEVELOPMENT_BUILD || PLAY_TEST_ENABLE

#if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE
		MAIN_CAMERA_DATA,
#endif // #if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE

		[HideInInspector] MAX_VAL
	}

	#region 변수
	private Stopwatch m_oStopwatch = new Stopwatch();
	private List<STDespawnObjInfo> m_oDespawnObjInfoList = new List<STDespawnObjInfo>();
	private List<Camera> m_oAdditionalCameraList = new List<Camera>();

	private Dictionary<EKey, bool> m_oBoolDict = new Dictionary<EKey, bool>();
	private Dictionary<EKey, Camera> m_oCameraDict = new Dictionary<EKey, Camera>();
	private Dictionary<EKey, Canvas> m_oCanvasDict = new Dictionary<EKey, Canvas>();

	/** =====> UI <===== */
	private Dictionary<EKey, Button> m_oBtnDict = new Dictionary<EKey, Button>();
	private Dictionary<EKey, CTouchDispatcher> m_oTouchDispatcherDict = new Dictionary<EKey, CTouchDispatcher>();

	/** =====> 객체 <===== */
	private Dictionary<EKey, GameObject> m_oUIsDict = new Dictionary<EKey, GameObject>();
	private Dictionary<EKey, GameObject> m_oObjDict = new Dictionary<EKey, GameObject>();
	private Dictionary<string, ObjectPool> m_oObjsPoolDict = new Dictionary<string, ObjectPool>();
	#endregion // 변수

	#region 클래스 변수
	private static float m_fOffsetSkipTime = 0.0f;
	private static NetworkReachability m_ePrevNetworkReachability = NetworkReachability.NotReachable;
	private static Vector3 m_stPrevScreenSize = Vector3.zero;
	private static Dictionary<string, STTouchResponderInfo> m_oTouchResponderInfoDict = new Dictionary<string, STTouchResponderInfo>();

	private static Dictionary<EKey, Camera> m_oActiveSceneCameraDict = new Dictionary<EKey, Camera>();
	private static Dictionary<EKey, Canvas> m_oActiveSceneCanvasDict = new Dictionary<EKey, Canvas>();
	private static Dictionary<EKey, EventSystem> m_oActiveSceneEventSystemDict = new Dictionary<EKey, EventSystem>();
	private static Dictionary<EKey, BaseInputModule> m_oActiveSceneInputModuleDict = new Dictionary<EKey, BaseInputModule>();
	private static Dictionary<EKey, CSceneManager> m_oActiveSceneManagerDict = new Dictionary<EKey, CSceneManager>();
	private static Dictionary<string, CSceneManager> m_oSceneManagerDict = new Dictionary<string, CSceneManager>();

#if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE
	private static Dictionary<EKey, UniversalAdditionalCameraData> m_oActiveSceneCameraDataDict = new Dictionary<EKey, UniversalAdditionalCameraData>();
#endif // #if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE

	/** =====> 객체 <===== */
	private static Dictionary<EKey, GameObject> m_oActiveSceneUIsDict = new Dictionary<EKey, GameObject>();
	private static Dictionary<EKey, GameObject> m_oActiveSceneObjDict = new Dictionary<EKey, GameObject>();
	#endregion // 클래스 변수

	#region 프로퍼티
	public abstract string SceneName { get; }
	public bool IsActiveScene => CSceneManager.ActiveScene == this.gameObject.scene;

	public virtual bool IsIgnoreTestUIs => true;
	public virtual bool IsIgnoreABTestUIs => true;
	public virtual bool IsIgnoreOverlayScene => true;
	public virtual bool IsIgnoreRebuildLayout => false;
	public virtual bool IsIgnoreBGTouchResponder => true;
	public virtual bool IsIgnoreRenderPostProcessing => true;
	public virtual bool IsIgnoreGlobalPostProcessingVolume => true;

	public virtual bool IsPortrait => this.ScreenWidth.ExIsLessEquals(this.ScreenHeight);
	public virtual bool IsResetMainCameraPos => !Application.isPlaying;
	public virtual bool IsResetMainDirectionalLightAngle => !Application.isPlaying;

	public virtual bool IsRealtimeFadeInAni => true;
	public virtual bool IsRealtimeFadeOutAni => true;

	public virtual float ScreenWidth => KCDefine.B_SCREEN_WIDTH;
	public virtual float ScreenHeight => KCDefine.B_SCREEN_HEIGHT;

	public virtual float ExtraBlindHOffset => KCDefine.B_VAL_0_REAL;
	public virtual float ExtraBlindVOffset => KCDefine.B_VAL_0_REAL;

	public virtual float PlaneDistance => ((this.ScreenHeight / KCDefine.B_VAL_2_REAL) / Mathf.Tan(Mathf.PI / KCDefine.B_VAL_8_REAL)) * KCDefine.B_UNIT_SCALE;
	public virtual float AwakeTimeScale => KCDefine.B_VAL_1_REAL;
	public virtual float FadeOutAniDuration => KCDefine.U_DURATION_ANI;
	public virtual float MainCameraDepth => KCDefine.U_DEPTH_MAIN_CAMERA;

	public virtual Color ClearColor => KCDefine.U_COLOR_CLEAR;
	public virtual Color ScreenFadeInColor => KCDefine.U_COLOR_SCREEN_FADE_IN;
	public virtual Color ScreenFadeOutColor => KCDefine.U_COLOR_SCREEN_FADE_OUT;
	public virtual Vector3 ScreenSize => new Vector3(this.ScreenWidth, this.ScreenHeight, KCDefine.B_VAL_0_INT);
	public virtual Vector3 ObjRootPivotPos => (m_oObjDict.GetValueOrDefault(EKey.OBJS) != null) ? m_oObjDict.GetValueOrDefault(EKey.OBJS).transform.localPosition : Vector3.zero;
	public virtual STSortingOrderInfo UIsCanvasSortingOrderInfo => KCDefine.U_SORTING_OI_UIS_CANVAS;

	public Camera MainCamera => m_oCameraDict.GetValueOrDefault(EKey.MAIN_CAMERA);
	public Canvas UIsCanvas => m_oCanvasDict.GetValueOrDefault(EKey.UIS_CANVAS);
	public Canvas PopupUIsCanvas => m_oCanvasDict.GetValueOrDefault(EKey.POPUP_UIS_CANVAS);
	public Canvas TopmostUIsCanvas => m_oCanvasDict.GetValueOrDefault(EKey.TOPMOST_UIS_CANVAS);

#if MODE_2D_ENABLE
	public virtual EProjection MainCameraProjection => EProjection._2D;
#else
	public virtual EProjection MainCameraProjection => EProjection._3D;
#endif // #if MODE_2D_ENABLE

#if MODE_PORTRAIT_ENABLE
	public virtual bool IsIgnoreBlindH => false;
	public virtual bool IsIgnoreBlindV => true;
#else
	public virtual bool IsIgnoreBlindH => true;
	public virtual bool IsIgnoreBlindV => false;
#endif // #if MODE_PORTRAIT_ENABLE

#if UNITY_EDITOR
	public virtual int ScriptOrder => KCDefine.U_SCRIPT_O_SCENE_MANAGER;
#endif // #if UNITY_EDITOR

	/** =====> UI <===== */
	public CTouchDispatcher BGTouchDispatcher => m_oTouchDispatcherDict.GetValueOrDefault(EKey.BG_TOUCH_DISPATCHER);

	/** =====> 객체 <===== */
	public GameObject UIs => m_oUIsDict.GetValueOrDefault(EKey.UIS);
	public GameObject UIsRoot => m_oUIsDict.GetValueOrDefault(EKey.UIS_ROOT);
	public GameObject UIsBase => m_oUIsDict.GetValueOrDefault(EKey.UIS_BASE);
	public GameObject TestUIs => m_oUIsDict.GetValueOrDefault(EKey.TEST_UIS);
	public GameObject TestContentsUIs => m_oUIsDict.GetValueOrDefault(EKey.TEST_CONTENTS_UIS);
	public GameObject PivotUIs => m_oUIsDict.GetValueOrDefault(EKey.PIVOT_UIS);
	public GameObject AnchorUIs => m_oUIsDict.GetValueOrDefault(EKey.ANCHOR_UIS);
	public GameObject CornerAnchorUIs => m_oUIsDict.GetValueOrDefault(EKey.CORNER_ANCHOR_UIS);
	public GameObject BGTouchResponder => m_oUIsDict.GetValueOrDefault(EKey.BG_TOUCH_RESPONDER);
	public GameObject DesignResolutionGuideUIs => m_oUIsDict.GetValueOrDefault(EKey.DESIGN_RESOLUTION_GUIDE_UIS);

	public GameObject UpUIs => m_oUIsDict.GetValueOrDefault(EKey.UP_UIS);
	public GameObject DownUIs => m_oUIsDict.GetValueOrDefault(EKey.DOWN_UIS);
	public GameObject LeftUIs => m_oUIsDict.GetValueOrDefault(EKey.LEFT_UIS);
	public GameObject RightUIs => m_oUIsDict.GetValueOrDefault(EKey.RIGHT_UIS);

	public GameObject UpLeftUIs => m_oUIsDict.GetValueOrDefault(EKey.UP_LEFT_UIS);
	public GameObject UpRightUIs => m_oUIsDict.GetValueOrDefault(EKey.UP_RIGHT_UIS);
	public GameObject DownLeftUIs => m_oUIsDict.GetValueOrDefault(EKey.DOWN_LEFT_UIS);
	public GameObject DownRightUIs => m_oUIsDict.GetValueOrDefault(EKey.DOWN_RIGHT_UIS);

	public GameObject PopupUIs => m_oUIsDict.GetValueOrDefault(EKey.POPUP_UIS);
	public GameObject TopmostUIs => m_oUIsDict.GetValueOrDefault(EKey.TOPMOST_UIS);

	public GameObject Base => m_oObjDict.GetValueOrDefault(EKey.BASE);
	public GameObject Objs => m_oObjDict.GetValueOrDefault(EKey.OBJS);
	public GameObject ObjsBase => m_oObjDict.GetValueOrDefault(EKey.OBJS_ROOT);
	public GameObject PivotObjs => m_oObjDict.GetValueOrDefault(EKey.PIVOT_OBJS);
	public GameObject AnchorObjs => m_oObjDict.GetValueOrDefault(EKey.ANCHOR_OBJS);
	public GameObject StaticObjs => m_oObjDict.GetValueOrDefault(EKey.STATIC_OBJS);
	public GameObject AdditionalLights => m_oObjDict.GetValueOrDefault(EKey.ADDITIONAL_LIGHTS);
	public GameObject AdditionalCameras => m_oObjDict.GetValueOrDefault(EKey.ADDITIONAL_CAMERAS);
	public GameObject ReflectionProbes => m_oObjDict.GetValueOrDefault(EKey.REFLECTION_PROBES);
	public GameObject LightProbeGroups => m_oObjDict.GetValueOrDefault(EKey.LIGHT_PROBE_GROUPS);

	public GameObject UpObjs => m_oObjDict.GetValueOrDefault(EKey.UP_OBJS);
	public GameObject DownObjs => m_oObjDict.GetValueOrDefault(EKey.DOWN_OBJS);
	public GameObject LeftObjs => m_oObjDict.GetValueOrDefault(EKey.LEFT_OBJS);
	public GameObject RightObjs => m_oObjDict.GetValueOrDefault(EKey.RIGHT_OBJS);
	#endregion // 프로퍼티

	#region 클래스 프로퍼티
	public static bool IsInit { get; private set; } = false;
	public static bool IsSetup { get; private set; } = false;
	public static bool IsLateSetup { get; private set; } = false;

	public static bool IsAwake { get; private set; } = false;
	public static bool IsRunning { get; private set; } = false;
	public static bool IsAppQuit { get; private set; } = false;

	public static float UpSafeAreaOffset { get; private set; } = 0.0f;
	public static float DownSafeAreaOffset { get; private set; } = 0.0f;
	public static float LeftSafeAreaOffset { get; private set; } = 0.0f;
	public static float RightSafeAreaOffset { get; private set; } = 0.0f;

	public static Vector3 CanvasSize { get; private set; } = Vector3.zero;
	public static Vector3 ObjsRootScale { get; private set; } = Vector3.zero;
	public static EQualityLevel QualityLevel { get; private set; } = EQualityLevel.NONE;
	public static System.DateTime ActiveSceneAwakeTime { get; private set; } = System.DateTime.Now;

	public static CScheduleManager ScheduleManager { get; private set; } = null;
	public static CNavStackManager NavStackManager { get; private set; } = null;
	public static CCollectionManager CollectionManager { get; private set; } = null;

	public static bool IsAppInit => CSceneManager.IsInit && CSceneManager.IsSetup && CSceneManager.IsLateSetup;
	public static bool IsAppRunning => CSceneManager.IsAppInit && !CSceneManager.IsAppQuit;
	public static bool IsExistsMainCamera => Camera.main != null && CSceneManager.ActiveSceneMainCamera != null;

	public static Scene ActiveScene => SceneManager.GetActiveScene();
	public static string ActiveSceneName => CSceneManager.ActiveScene.name;
	public static Camera ActiveSceneMainCamera => CSceneManager.m_oActiveSceneCameraDict.GetValueOrDefault(EKey.MAIN_CAMERA);

	public static Canvas ActiveSceneUIsCanvas => CSceneManager.m_oActiveSceneCanvasDict.GetValueOrDefault(EKey.UIS_CANVAS);
	public static Canvas ActiveScenePopupUIsCanvas => CSceneManager.m_oActiveSceneCanvasDict.GetValueOrDefault(EKey.POPUP_UIS_CANVAS);
	public static Canvas ActiveSceneTopmostUIsCanvas => CSceneManager.m_oActiveSceneCanvasDict.GetValueOrDefault(EKey.TOPMOST_UIS_CANVAS);

	public static EventSystem ActiveSceneEventSystem => CSceneManager.m_oActiveSceneEventSystemDict.GetValueOrDefault(EKey.EVENT_SYSTEM);
	public static BaseInputModule ActiveSceneBaseInputModule => CSceneManager.m_oActiveSceneInputModuleDict.GetValueOrDefault(EKey.BASE_INPUT_MODULE);
	public static CSceneManager ActiveSceneManager => CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER);

	/** =====> 객체 <===== */
	public static GameObject ScreenBlindUIs { get; private set; } = null;
	public static GameObject ScreenPopupUIs { get; private set; } = null;
	public static GameObject ScreenTopmostUIs { get; private set; } = null;
	public static GameObject ScreenAbsUIs { get; private set; } = null;

	public static GameObject ActiveSceneUIs => CSceneManager.m_oActiveSceneUIsDict.GetValueOrDefault(EKey.UIS);
	public static GameObject ActiveSceneUIsRoot => CSceneManager.m_oActiveSceneUIsDict.GetValueOrDefault(EKey.UIS_ROOT);
	public static GameObject ActiveSceneUIsBase => CSceneManager.m_oActiveSceneUIsDict.GetValueOrDefault(EKey.UIS_BASE);
	public static GameObject ActiveScenePivotUIs => CSceneManager.m_oActiveSceneUIsDict.GetValueOrDefault(EKey.PIVOT_UIS);
	public static GameObject ActiveSceneAnchorUIs => CSceneManager.m_oActiveSceneUIsDict.GetValueOrDefault(EKey.ANCHOR_UIS);
	public static GameObject ActiveSceneCornerAnchorUIs => CSceneManager.m_oActiveSceneUIsDict.GetValueOrDefault(EKey.CORNER_ANCHOR_UIS);

	public static GameObject ActiveSceneUpUIs => CSceneManager.m_oActiveSceneUIsDict.GetValueOrDefault(EKey.UP_UIS);
	public static GameObject ActiveSceneDownUIs => CSceneManager.m_oActiveSceneUIsDict.GetValueOrDefault(EKey.DOWN_UIS);
	public static GameObject ActiveSceneLeftUIs => CSceneManager.m_oActiveSceneUIsDict.GetValueOrDefault(EKey.LEFT_UIS);
	public static GameObject ActiveSceneRightUIs => CSceneManager.m_oActiveSceneUIsDict.GetValueOrDefault(EKey.RIGHT_UIS);

	public static GameObject ActiveSceneUpLeftUIs => CSceneManager.m_oActiveSceneUIsDict.GetValueOrDefault(EKey.UP_LEFT_UIS);
	public static GameObject ActiveSceneUpRightUIs => CSceneManager.m_oActiveSceneUIsDict.GetValueOrDefault(EKey.UP_RIGHT_UIS);
	public static GameObject ActiveSceneDownLeftUIs => CSceneManager.m_oActiveSceneUIsDict.GetValueOrDefault(EKey.DOWN_LEFT_UIS);
	public static GameObject ActiveSceneDownRightUIs => CSceneManager.m_oActiveSceneUIsDict.GetValueOrDefault(EKey.DOWN_RIGHT_UIS);

	public static GameObject ActiveScenePopupUIs => CSceneManager.m_oActiveSceneUIsDict.GetValueOrDefault(EKey.POPUP_UIS);
	public static GameObject ActiveSceneTopmostUIs => CSceneManager.m_oActiveSceneUIsDict.GetValueOrDefault(EKey.TOPMOST_UIS);

	public static GameObject ActiveSceneBase => CSceneManager.m_oActiveSceneObjDict.GetValueOrDefault(EKey.BASE);
	public static GameObject ActiveSceneObjs => CSceneManager.m_oActiveSceneObjDict.GetValueOrDefault(EKey.OBJS);
	public static GameObject ActiveSceneObjsBase => CSceneManager.m_oActiveSceneObjDict.GetValueOrDefault(EKey.OBJS_ROOT);
	public static GameObject ActiveScenePivotObjs => CSceneManager.m_oActiveSceneObjDict.GetValueOrDefault(EKey.PIVOT_OBJS);
	public static GameObject ActiveSceneAnchorObjs => CSceneManager.m_oActiveSceneObjDict.GetValueOrDefault(EKey.ANCHOR_OBJS);
	public static GameObject ActiveSceneStaticObjs => CSceneManager.m_oActiveSceneObjDict.GetValueOrDefault(EKey.STATIC_OBJS);
	public static GameObject ActiveSceneAdditionalLights => CSceneManager.m_oActiveSceneObjDict.GetValueOrDefault(EKey.ADDITIONAL_LIGHTS);
	public static GameObject ActiveSceneAdditionalCameras => CSceneManager.m_oActiveSceneObjDict.GetValueOrDefault(EKey.ADDITIONAL_CAMERAS);
	public static GameObject ActiveSceneReflectionProbes => CSceneManager.m_oActiveSceneObjDict.GetValueOrDefault(EKey.REFLECTION_PROBES);
	public static GameObject ActiveSceneLightProbeGroups => CSceneManager.m_oActiveSceneObjDict.GetValueOrDefault(EKey.LIGHT_PROBE_GROUPS);

	public static GameObject ActiveSceneUpObjs => CSceneManager.m_oActiveSceneObjDict.GetValueOrDefault(EKey.UP_OBJS);
	public static GameObject ActiveSceneDownObjs => CSceneManager.m_oActiveSceneObjDict.GetValueOrDefault(EKey.DOWN_OBJS);
	public static GameObject ActiveSceneLeftObjs => CSceneManager.m_oActiveSceneObjDict.GetValueOrDefault(EKey.LEFT_OBJS);
	public static GameObject ActiveSceneRightObjs => CSceneManager.m_oActiveSceneObjDict.GetValueOrDefault(EKey.RIGHT_OBJS);
	#endregion // 클래스 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		this.SetIgnoreNavStackEvent(true);

#if DEBUG || DEVELOPMENT_BUILD
		CSceneManager.ScreenDebugUIs?.SetActive(true);
#endif // #if DEBUG || DEVELOPMENT_BUILD

		// 주요 씬 일 경우
		if(this.IsActiveScene) {
			Time.timeScale = this.AwakeTimeScale;
			CSceneManager.ActiveSceneAwakeTime = System.DateTime.Now;
		}

		// 씬이 처음 로딩 되었을 경우
		if(!CSceneManager.IsInit && !CSceneManager.IsAwake) {
#if DEBUG || DEVELOPMENT_BUILD
			CFunc.ShowLog($"Platform: {Application.platform}", KCDefine.B_LOG_COLOR_PLATFORM_INFO);
			CFunc.ShowLog($"Data Path: {Application.dataPath}/", KCDefine.B_LOG_COLOR_PLATFORM_INFO);
			CFunc.ShowLog($"Persistent Data Path: {Application.persistentDataPath}/", KCDefine.B_LOG_COLOR_PLATFORM_INFO);

#if UNITY_EDITOR
			CFunc.ShowLog($"External Data Path: {KCDefine.B_ABS_DIR_P_EXTERNAL_DATAS}", KCDefine.B_LOG_COLOR_PLATFORM_INFO);
#else
			CFunc.ShowLog($"External Data Path: {KCDefine.B_ABS_DIR_P_RUNTIME_EXTERNAL_DATAS}", KCDefine.B_LOG_COLOR_PLATFORM_INFO);
#endif // #if UNITY_EDITOR
#endif // #if DEBUG || DEVELOPMENT_BUILD

			CSceneManager.IsAwake = true;
			CSceneManager.IsRunning = true;

			Screen.sleepTimeout = SleepTimeout.NeverSleep;
			Application.targetFrameRate = KCDefine.B_MIN_TARGET_FRAME_RATE;

			CSceneLoader.Inst.SetAwakeActiveSceneName(CSceneManager.ActiveSceneName);

			// 초기화 씬이 아닐 경우
			if(!CSceneLoader.Inst.AwakeActiveSceneName.Equals(KCDefine.B_SCENE_N_INIT)) {
				this.ExLateCallFunc((a_oSender) => {
					CScheduleManager.Inst.RemoveComponent(this);
					CNavStackManager.Inst.RemoveComponent(this);

#if SCENE_TEMPLATES_MODULE_ENABLE
					CSceneLoader.Inst.LoadScene(KCDefine.B_SCENE_N_INIT, false);
#endif // SCENE_TEMPLATES_MODULE_ENABLE
				}, KCDefine.U_DELAY_INIT);

				CFunc.EnumerateComponents<MonoBehaviour>((a_oComponent) => { a_oComponent.ExSetEnable(false); return true; });
			}
		}

		m_oStopwatch.Start();

		try {
			this.SetupScene(true);
			m_oUIsDict.GetValueOrDefault(EKey.DESIGN_RESOLUTION_GUIDE_UIS)?.gameObject.SetActive(this.IsActiveScene && Application.isEditor);

			// 앱이 실행 중 일 경우
			if(Application.isPlaying) {
				CSceneManager.ScheduleManager = CScheduleManager.Inst;
				CSceneManager.NavStackManager = CNavStackManager.Inst;
				CSceneManager.CollectionManager = CCollectionManager.Inst;

				// 액티브 씬 일 경우
				if(this.IsActiveScene && CSceneManager.IsAppInit) {
					// 터치 응답자 설정이 가능 할 경우
					if(!this.IsIgnoreBGTouchResponder) {
						// 터치 응답자를 설정한다 {
						CFunc.SetupTouchResponders(new List<(EKey, string, GameObject, GameObject)>() {
							(EKey.BG_TOUCH_RESPONDER, $"{EKey.BG_TOUCH_RESPONDER}", this.UIs, Resources.Load<GameObject>(KCDefine.U_OBJ_P_TOUCH_RESPONDER))
						}, CSceneManager.CanvasSize, m_oUIsDict);

						m_oUIsDict.GetValueOrDefault(EKey.BG_TOUCH_RESPONDER)?.transform.SetAsFirstSibling();
						// 터치 응답자를 설정한다 }

						// 터치 전달자를 설정한다
						m_oTouchDispatcherDict.ExReplaceVal(EKey.BG_TOUCH_DISPATCHER, m_oUIsDict.GetValueOrDefault(EKey.BG_TOUCH_RESPONDER)?.GetComponentInChildren<CTouchDispatcher>());
						m_oTouchDispatcherDict.GetValueOrDefault(EKey.BG_TOUCH_DISPATCHER)?.SetBeginCallback(this.OnTouchBegin);
						m_oTouchDispatcherDict.GetValueOrDefault(EKey.BG_TOUCH_DISPATCHER)?.SetMoveCallback(this.OnTouchMove);
						m_oTouchDispatcherDict.GetValueOrDefault(EKey.BG_TOUCH_DISPATCHER)?.SetEndCallback(this.OnTouchEnd);
					}

#if AB_TEST_ENABLE && (DEBUG || DEVELOPMENT_BUILD || PLAY_TEST_ENABLE)
					// AB 테스트 UI 설정이 가능 할 경우
					if(!this.IsIgnoreABTestUIs) {
						var oTextDict = CCollectionManager.Inst.SpawnDict<string, TMP_Text>();
						var oHLayoutGroupDict = CCollectionManager.Inst.SpawnDict<string, HorizontalLayoutGroup>();

						CLocalizeInfoTable.Inst.TryGetFontSetInfo(string.Empty, SystemLanguage.English, EFontSet._1, out STFontSetInfo stFontSetInfo);

						try {
							// 레이아웃을 설정한다 {
							CFunc.SetupLayoutGroups(new List<(string, string, GameObject, GameObject)>() {
								(KCDefine.U_OBJ_N_AB_T_UIS_SET_UIS, KCDefine.U_OBJ_N_AB_T_UIS_SET_UIS, this.UpUIs, null)
							}, oHLayoutGroupDict);

							oHLayoutGroupDict.GetValueOrDefault(KCDefine.U_OBJ_N_AB_T_UIS_SET_UIS).spacing = KCDefine.B_VAL_4_REAL * KCDefine.B_VAL_5_REAL;

							(oHLayoutGroupDict.GetValueOrDefault(KCDefine.U_OBJ_N_AB_T_UIS_SET_UIS).transform as RectTransform).pivot = KCDefine.B_ANCHOR_UP_CENTER;
							(oHLayoutGroupDict.GetValueOrDefault(KCDefine.U_OBJ_N_AB_T_UIS_SET_UIS).transform as RectTransform).anchorMin = KCDefine.B_ANCHOR_UP_CENTER;
							(oHLayoutGroupDict.GetValueOrDefault(KCDefine.U_OBJ_N_AB_T_UIS_SET_UIS).transform as RectTransform).anchorMax = KCDefine.B_ANCHOR_UP_CENTER;

							var oContentsSizeFitter = oHLayoutGroupDict.GetValueOrDefault(KCDefine.U_OBJ_N_AB_T_UIS_SET_UIS).gameObject.ExAddComponent<ContentSizeFitter>();
							oContentsSizeFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
							oContentsSizeFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
							// 레이아웃을 설정한다 }

							// 텍스트를 설정한다 {
							CFunc.SetupComponents(new List<(string, string, GameObject, GameObject)>() {
								(KCDefine.U_OBJ_N_A_SET_BTN, KCDefine.U_OBJ_N_A_SET_BTN, oHLayoutGroupDict.GetValueOrDefault(KCDefine.U_OBJ_N_AB_T_UIS_SET_UIS).gameObject, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_TMP_TEXT_BTN)),
								(KCDefine.U_OBJ_N_B_SET_BTN, KCDefine.U_OBJ_N_B_SET_BTN, oHLayoutGroupDict.GetValueOrDefault(KCDefine.U_OBJ_N_AB_T_UIS_SET_UIS).gameObject, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_TMP_TEXT_BTN))
							}, oTextDict);

							oTextDict.GetValueOrDefault(KCDefine.U_OBJ_N_A_SET_BTN).fontSize = KCDefine.U_DEF_SIZE_FONT;
							oTextDict.GetValueOrDefault(KCDefine.U_OBJ_N_A_SET_BTN).ExSetText(CStrTable.Inst.GetStr(KCDefine.ST_KEY_C_A_SET_TEXT), stFontSetInfo);

							oTextDict.GetValueOrDefault(KCDefine.U_OBJ_N_B_SET_BTN).fontSize = KCDefine.U_DEF_SIZE_FONT;
							oTextDict.GetValueOrDefault(KCDefine.U_OBJ_N_B_SET_BTN).ExSetText(CStrTable.Inst.GetStr(KCDefine.ST_KEY_C_B_SET_TEXT), stFontSetInfo);
							// 텍스트를 설정한다 }

							// 버튼을 설정한다
							CFunc.SetupButtons(new List<(GameObject, UnityAction)>() {
								(oTextDict.GetValueOrDefault(KCDefine.U_OBJ_N_A_SET_BTN).gameObject, () => this.OnTouchABTUIsSetBtn(EUserType.A)),
								(oTextDict.GetValueOrDefault(KCDefine.U_OBJ_N_B_SET_BTN).gameObject, () => this.OnTouchABTUIsSetBtn(EUserType.B))
							});
						} finally {
							CCollectionManager.Inst.DespawnDict(oTextDict);
							CCollectionManager.Inst.DespawnDict(oHLayoutGroupDict);
						}
					}
#endif // #if AB_TEST_ENABLE && (DEBUG || DEVELOPMENT_BUILD || PLAY_TEST_ENABLE)
				}

#if(DEBUG || DEVELOPMENT_BUILD || PLAY_TEST_ENABLE) && (!ROBO_TEST_ENABLE && !STORE_DIST_BUILD && !CREATIVE_DIST_BUILD)
				m_oUIsDict.GetValueOrDefault(EKey.TEST_UIS)?.SetActive(!this.IsIgnoreTestUIs && (this.IsActiveScene && CSceneManager.IsAppInit));
#else
				m_oUIsDict.GetValueOrDefault(EKey.TEST_UIS)?.SetActive(false);
#endif // #if (DEBUG || DEVELOPMENT_BUILD || PLAY_TEST_ENABLE) && (!ROBO_TEST_ENABLE && !STORE_DIST_BUILD && !CREATIVE_DIST_BUILD)
			}
		} finally {
			m_oStopwatch.Stop();

			// 액티브 씬 일 경우
			if(this.IsActiveScene) {
				CSceneManager.SetStaticDebugText($"{this.SceneName}.SetupScene(true): {m_oStopwatch.ElapsedMilliseconds} ms, {!this.IsIgnoreTestUIs}, {CSceneManager.IsAppInit}");
			} else {
				CSceneManager.AddStaticDebugStr($"{this.SceneName}.SetupScene(true): {m_oStopwatch.ElapsedMilliseconds} ms, {!this.IsIgnoreTestUIs}, {CSceneManager.IsAppInit}");
			}
		}
	}

	/** 초기화 */
	public override void Start() {
		base.Start();
		m_oStopwatch.Restart();

		try {
			this.SetupScene(false);

			// 주요 씬 일 경우
			if(this.IsActiveScene) {
				CIndicatorManager.Inst.Close(true);
				CScheduleManager.Inst.AddComponent(this);
				CNavStackManager.Inst.AddComponent(this);

				this.StartScreenFadeOutAni(this.FadeOutAniDuration);

#if UNITY_EDITOR
				var oSelObjList = new List<GameObject>() {
					this.UIsBase, this.Objs, this.gameObject
				};

				this.ExLateCallFunc((a_oSender) => CFunc.SelObjs(oSelObjList), KCDefine.B_VAL_1_REAL, true);
#endif // #if UNITY_EDITOR

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
				// 중첩 씬 활성 모드 일 경우
				if(CSceneManager.IsAppInit && !this.IsIgnoreOverlayScene) {
					CSceneLoader.Inst.LoadAdditiveScene(KCDefine.B_SCENE_N_OVERLAY);
				}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
			}

#if DEBUG || DEVELOPMENT_BUILD || PLAY_TEST_ENABLE
			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				this.SetupTestUIs();
				this.OnTouchTestUIsCloseBtn();

#if DEBUG || DEVELOPMENT_BUILD
				this.SetupDebugUIs(CSceneManager.ScreenDebugUIs, false);
#endif // #if DEBUG || DEVELOPMENT_BUILD
			}
#endif // #if DEBUG || DEVELOPMENT_BUILD || PLAY_TEST_ENABLE

			Canvas.ForceUpdateCanvases();

#if DEBUG || DEVELOPMENT_BUILD
			CSceneManager.ScreenDebugUIs?.ExEnumerateComponents<RectTransform>((a_oTrans) => { LayoutRebuilder.ForceRebuildLayoutImmediate(a_oTrans); return true; }, false, false);
#endif // #if DEBUG || DEVELOPMENT_BUILD
		} finally {
			m_oStopwatch.Stop();
			CSceneManager.AddStaticDebugStr($"{this.SceneName}.SetupScene(false): {m_oStopwatch.ElapsedMilliseconds} ms\n");
		}
	}

	/** 상태를 갱신한다 */
	public override void OnUpdate(float a_fDeltaTime) {
		base.OnUpdate(a_fDeltaTime);

		// 앱이 실행 중 일 경우
		if(CSceneManager.IsAppRunning) {
			// 레이아웃 보정이 가능 할 경우
			if(!this.IsIgnoreRebuildLayout && !m_oBoolDict.GetValueOrDefault(EKey.IS_REBUILD_LAYOUT)) {
				m_oBoolDict.ExReplaceVal(EKey.IS_REBUILD_LAYOUT, true);
				this.gameObject.scene.ExEnumerateComponents<RectTransform>((a_oTrans) => { LayoutRebuilder.ForceRebuildLayoutImmediate(a_oTrans); return true; });
			}

			// 주요 씬 일 경우
			if(this.IsActiveScene) {
				CSceneManager.m_fOffsetSkipTime += Time.deltaTime;

#if DEBUG || DEVELOPMENT_BUILD
				this.UpdateFPSInfoUIsState(a_fDeltaTime);
				this.UpdateDebugInfoUIsState(a_fDeltaTime);
#endif // #if DEBUG || DEVELOPMENT_BUILD

				// 간격 갱신 주기가 지났을 경우
				if(CSceneManager.m_fOffsetSkipTime.ExIsGreateEquals(KCDefine.B_VAL_1_REAL)) {
					CSceneManager.m_fOffsetSkipTime = KCDefine.B_VAL_0_REAL;
					this.SetupOffsets();
				}

				// 앱이 초기화 되었을 경우
				if(CSceneManager.IsAppInit) {
#if INPUT_SYSTEM_MODULE_ENABLE
					bool bIsBackKeyDown = Keyboard.current.escapeKey.wasPressedThisFrame;
#else
					bool bIsBackKeyDown = Input.GetKeyDown(KeyCode.Escape);
#endif // #if INPUT_SYSTEM_MODULE_ENABLE

					// 백 키를 눌렀을 경우
					if(bIsBackKeyDown) {
						CSndManager.Inst.PlayFXSnds(KCDefine.U_SND_P_G_SFX_TOUCH_END);
						CNavStackManager.Inst.SendNavStackEvent(ENavStackEvent.BACK_KEY_DOWN);
					}

					// 네트워크 상태가 갱신 되었을 경우
					if(CSceneManager.m_ePrevNetworkReachability != Application.internetReachability) {
						CSceneManager.m_ePrevNetworkReachability = Application.internetReachability;
						this.OnUpdateNetworkReachability(Application.internetReachability);
					}

					// 화면 크기가 갱신 되었을 경우
					if(!CSceneManager.m_stPrevScreenSize.ExIsEquals(CAccess.DeviceScreenSize)) {
						CSceneManager.m_stPrevScreenSize = CAccess.DeviceScreenSize;
						this.OnUpdateScreenSize(CAccess.DeviceScreenSize);
					}

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
					// 단축키를 눌렀을 경우
					if(Input.GetKey(KeyCode.LeftShift)) {
						this.HandleHotKeys();
					}
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
				}
			}
		}
	}

	/** 상태를 갱신한다 */
	public override void OnLateUpdate(float a_fDeltaTime) {
		base.OnLateUpdate(a_fDeltaTime);

		// 앱이 실행 중 일 경우
		if(CSceneManager.IsAppRunning) {
			var oRemoveDespawnObjInfoList = CCollectionManager.Inst.SpawnList<STDespawnObjInfo>();

			try {
				for(int i = 0; i < m_oDespawnObjInfoList.Count; ++i) {
					// 객체 제거가 가능 할 경우
					if(m_oDespawnObjInfoList[i].m_stDespawnTime.ExGetDeltaTime(System.DateTime.Now).ExIsLessEquals(KCDefine.B_VAL_0_REAL)) {
						oRemoveDespawnObjInfoList.Add(m_oDespawnObjInfoList[i]);
					}
				}

				for(int i = 0; i < oRemoveDespawnObjInfoList.Count; ++i) {
					m_oDespawnObjInfoList.ExRemoveVal((a_stDespawnObjInfo) => oRemoveDespawnObjInfoList[i].m_oObj == a_stDespawnObjInfo.m_oObj);

					// 객체 풀이 존재 할 경우
					if(m_oObjsPoolDict.TryGetValue(oRemoveDespawnObjInfoList[i].m_oObjsPoolKey, out ObjectPool oObjsPool)) {
						oObjsPool.Despawn(oRemoveDespawnObjInfoList[i].m_oObj, oRemoveDespawnObjInfoList[i].m_bIsDestroy);
					}
				}
			} finally {
				CCollectionManager.Inst.DespawnList(oRemoveDespawnObjInfoList);
			}
		}
	}

	/** 제거 되었을 경우 */
	public override void OnDestroy() {
		base.OnDestroy();

		try {
			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				// 주요 씬 일 경우
				if(this.IsActiveScene) {
					Resources.UnloadUnusedAssets();
					CSceneManager.CollectionManager?.Reset();
					CSceneLoader.Inst.SetPrevActiveSceneName(this.SceneName);
					System.GC.Collect(System.GC.MaxGeneration, System.GCCollectionMode.Optimized, false, true);

					// 터치 응답자가 존재 할 경우
					if(CSceneManager.m_oTouchResponderInfoDict.TryGetValue(KCDefine.U_OBJ_N_SCREEN_FADE_TOUCH_RESPONDER, out STTouchResponderInfo stTouchResponderInfo)) {
						stTouchResponderInfo.m_oAni?.Kill();
					}

#if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE
					// 액티브 씬 메인 카메라 데이터가 존재 할 경우
					if(this.GetAdditionalCameras().ExIsValid() && CSceneManager.m_oActiveSceneCameraDataDict.GetValueOrDefault(EKey.MAIN_CAMERA_DATA) != null && QualitySettings.renderPipeline != null && GraphicsSettings.renderPipelineAsset != null) {
						CSceneManager.m_oActiveSceneCameraDataDict.GetValueOrDefault(EKey.MAIN_CAMERA_DATA).cameraStack?.ExRemoveVals((a_oCamera) => this.GetAdditionalCameras().Contains(a_oCamera), false);
					}
#endif // #if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE
				}
			}

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAwake || CSceneManager.IsAppRunning) {
				CSceneManager.ScheduleManager?.RemoveComponent(this);
				CSceneManager.NavStackManager?.RemoveComponent(this);

				CSceneManager.m_oSceneManagerDict.ExRemoveVal(this.SceneName);
			}
		} catch(System.Exception oException) {
			CFunc.ShowLogWarning($"CSceneManager.OnDestroy Exception: {oException.Message}");
		}
	}

	/** 앱이 정지 되었을 경우 */
	public virtual void OnApplicationPause(bool a_bIsPause) {
		// 앱이 실행 중 일 경우
		if(CSceneManager.IsAppRunning) {
			// 정지 되었을 경우
			if(a_bIsPause) {
				CSndManager.Inst.PauseBGSnd();
			} else {
				CSndManager.Inst.ResumeBGSnd();
			}
		}
	}

	/** 앱이 종료 되었을 경우 */
	public virtual void OnApplicationQuit() {
		this.DoQuitApp();
		CFunc.EnumerateComponents<CComponent>((a_oComponent) => { CFactory.RemoveObj(a_oComponent); return true; }, true);
	}

	/** 앱을 종료한다 */
	public void QuitApp() {
		this.DoQuitApp();

#if UNITY_EDITOR
		EditorApplication.ExitPlaymode();
#else
		Application.Quit();
#endif // #if UNITY_EDITOR
	}

	/** 객체 풀을 추가한다 */
	public void AddObjsPool(string a_oKey, ObjectPool a_oObjsPool, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oObjsPool != null && a_oKey.ExIsValid()));

		// 객체 풀이 존재 할 경우
		if(a_oObjsPool != null && a_oKey.ExIsValid()) {
			m_oObjsPoolDict.TryAdd(a_oKey, a_oObjsPool);
		}
	}

	/** 객체 풀을 추가한다 */
	public void AddObjsPool(string a_oKey, string a_oObjPath, GameObject a_oParent, int a_nNumObjs = KCDefine.U_SIZE_OBJS_POOL_01, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oKey.ExIsValid() && a_oObjPath.ExIsValid()));

		// 객체 풀 추가가 가능 할 경우
		if(a_oKey.ExIsValid() && a_oObjPath.ExIsValid()) {
			this.AddObjsPool(a_oKey, CResManager.Inst.GetRes<GameObject>(a_oObjPath), a_oParent, a_nNumObjs, a_bIsEnableAssert);
		}
	}

	/** 객체 풀을 추가한다 */
	public void AddObjsPool(string a_oKey, GameObject a_oOrigin, GameObject a_oParent, int a_nNumObjs = KCDefine.U_SIZE_OBJS_POOL_01, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oOrigin != null && a_oKey.ExIsValid()));

		// 객체 풀 추가가 가능 할 경우
		if(a_oOrigin != null && a_oKey.ExIsValid()) {
			this.AddObjsPool(a_oKey, CFactory.CreateObjsPool(a_oOrigin, a_oParent, a_nNumObjs), a_bIsEnableAssert);
		}
	}

	/** 객체 풀을 제거한다 */
	public void RemoveObjsPool(string a_oKey, bool a_bIsDestroy = true) {
		CAccess.Assert(a_oKey.ExIsValid());

		// 객체 풀이 존재 할 경우
		if(m_oObjsPoolDict.TryGetValue(a_oKey, out ObjectPool oObjsPool)) {
			oObjsPool.DeAllocate(oObjsPool.CountInactive);
			oObjsPool.DespawnAllActiveObjects(a_bIsDestroy);

			m_oObjsPoolDict.ExRemoveVal(a_oKey);
		}
	}

	/** 화면 페이드 인 애니메이션을 시작한다 */
	public void StartScreenFadeInAni(System.Action<GameObject> a_oCallback, float a_fDuration = KCDefine.U_DURATION_ANI) {
		// 애니메이션 모드 일 경우
		if(!this.IsIgnoreAni) {
#if DOTWEEN_MODULE_ENABLE || EXTRA_SCRIPT_MODULE_ENABLE
			CSceneManager.ShowTouchResponder(CSceneManager.ScreenAbsUIs, KCDefine.U_OBJ_N_SCREEN_FADE_TOUCH_RESPONDER, KCDefine.U_OBJ_P_SCREEN_FADE_TOUCH_RESPONDER, this.ScreenFadeInColor, a_oCallback, true, a_fDuration, this.MakeScreenFadeInAni);
#endif // #if DOTWEEN_MODULE_ENABLE || EXTRA_SCRIPT_MODULE_ENABLE
		} else {
			CFunc.Invoke(ref a_oCallback, null);
		}
	}

	/** 화면 페이드 아웃 애니메이션을 시작한다 */
	public void StartScreenFadeOutAni(float a_fDuration = KCDefine.U_DURATION_ANI) {
#if DOTWEEN_MODULE_ENABLE || EXTRA_SCRIPT_MODULE_ENABLE
		CSceneManager.CloseTouchResponder(KCDefine.U_OBJ_N_SCREEN_FADE_TOUCH_RESPONDER, this.ScreenFadeOutColor, null, a_fDuration, this.MakeScreenFadeOutAni);
#endif // #if DOTWEEN_MODULE_ENABLE || EXTRA_SCRIPT_MODULE_ENABLE
	}

	/** 레이아웃을 재배치한다 */
	protected void RebuildLayout(GameObject a_oObj) {
		var oTransList = CCollectionManager.Inst.SpawnList<RectTransform>();

		try {
			// 트랜스 폼을 설정한다 {
			a_oObj.ExEnumerateComponents<RectTransform>((a_oTrans) => { oTransList.ExAddVal(a_oTrans); return true; });

			for(int i = 0; i < oTransList.Count; ++i) {
				LayoutRebuilder.ForceRebuildLayoutImmediate(oTransList[i]);
			}
			// 트랜스 폼을 설정한다 }
		} finally {
			CCollectionManager.Inst.DespawnList(oTransList);
		}
	}

	/** 네트워크 상태가 갱신 되었을 경우 */
	protected virtual void OnUpdateNetworkReachability(NetworkReachability a_eNetworkReachability) {
		CFunc.ShowLog($"CSceneManager.OnUpdateNetworkReachability: {this.SceneName}, {a_eNetworkReachability}", KCDefine.B_LOG_COLOR_INFO);
	}

	/** 화면 크기가 갱신 되었을 경우 */
	protected virtual void OnUpdateScreenSize(Vector3 a_stScreenSize) {
		CFunc.ShowLog($"CSceneManager.OnUpdateScreenSize: {this.SceneName}, {a_stScreenSize}", KCDefine.B_LOG_COLOR_INFO);
		this.ExLateCallFunc((a_oSender) => this.SetupScene());
	}

	/** 비동기 씬 로딩 상태가 갱신 되었을 경우 */
	protected virtual void OnUpdateAsyncSceneLoadingState(AsyncOperation a_oAsyncOperation, bool a_bIsComplete) {
		CFunc.ShowLog($"CSceneManager.OnUpdateAsyncSceneLoadingState: {this.SceneName}, {a_oAsyncOperation.progress}, {a_bIsComplete}", KCDefine.B_LOG_COLOR_INFO);
	}

	/** 터치 이벤트를 처리한다 */
	protected virtual void HandleTouchEvent(CTouchDispatcher a_oSender, PointerEventData a_oEventData, ETouchEvent a_eTouchEvent) {
		// Do Something
	}

	/** 터치를 시작했을 경우 */
	private void OnTouchBegin(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
		this.HandleTouchEvent(a_oSender, a_oEventData, ETouchEvent.BEGIN);
	}

	/** 터치를 이동했을 경우 */
	private void OnTouchMove(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
		this.HandleTouchEvent(a_oSender, a_oEventData, ETouchEvent.MOVE);
	}

	/** 터치를 종료했을 경우 */
	private void OnTouchEnd(CTouchDispatcher a_oSender, PointerEventData a_oEventData) {
		this.HandleTouchEvent(a_oSender, a_oEventData, ETouchEvent.END);
	}

	/** 앱을 종료한다 */
	private void DoQuitApp() {
		DOTween.KillAll();
		CSceneManager.IsAppQuit = true;

#if !UNITY_EDITOR
		Time.timeScale = KCDefine.B_VAL_0_REAL;
#endif // #if !UNITY_EDITOR
	}
	#endregion // 함수

	#region 클래스 함수
	/** 터치 응답자를 출력한다 */
	public static GameObject ShowTouchResponder(GameObject a_oParent, string a_oKey, string a_oObjPath, Color a_stColor, System.Action<GameObject> a_oCallback, bool a_bIsEnableNavStack = false, float a_fDuration = KCDefine.B_VAL_0_REAL, System.Func<GameObject, string, Color, float, Tween> a_oAniCreator = null) {
		// 터치 응답자가 없을 경우
		if(!CSceneManager.m_oTouchResponderInfoDict.ContainsKey(a_oKey)) {
			// 터치 응답자를 설정한다
			var oTouchResponder = CFactory.CreateTouchResponder(string.Format(KCDefine.U_KEY_FMT_SCENE_M_TOUCH_RESPONDER, a_oKey), KCDefine.U_OBJ_P_TOUCH_RESPONDER, a_oParent, CSceneManager.CanvasSize, KCDefine.B_POS_TOUCH_RESPONDER, KCDefine.U_COLOR_TRANSPARENT);
			(oTouchResponder.transform as RectTransform).sizeDelta = Vector3.zero;
			(oTouchResponder.transform as RectTransform).anchorMin = KCDefine.B_ANCHOR_DOWN_LEFT;
			(oTouchResponder.transform as RectTransform).anchorMax = KCDefine.B_ANCHOR_UP_RIGHT;

			// 터치 전달자를 설정한다
			var oTouchDispatcher = oTouchResponder.GetComponentInChildren<CTouchDispatcher>();
			oTouchDispatcher.SetIgnoreNavStackEvent(true);
			oTouchDispatcher.SetDestroyCallback((a_oSender) => CSceneManager.m_oTouchResponderInfoDict.ExRemoveVal(a_oKey));

			Sequence oAni = null;

			// 내비게이션 스택 모드 일 경우
			if(a_bIsEnableNavStack) {
				CNavStackManager.Inst.AddComponent(oTouchDispatcher);
			}

			// 애니메이션 모드 일 경우
			if(a_oAniCreator != null) {
				// 전환 효과가 존재 할 경우
				if(oTouchResponder.TryGetComponent<UITransitionEffect>(out UITransitionEffect oTransitionFX)) {
					oTransitionFX.updateMode = CSceneManager.ActiveSceneManager.IsRealtimeFadeInAni ? AnimatorUpdateMode.UnscaledTime : AnimatorUpdateMode.Normal;
				}

				oAni = CFactory.MakeSequence(a_oAniCreator(oTouchResponder, a_oKey, a_stColor, a_fDuration), (a_oSender) => {
					a_oSender.Kill();
					CSceneManager.OnShowTouchResponder(a_oKey, a_stColor, oTouchResponder, a_oCallback);
				}, KCDefine.B_VAL_0_REAL, false, CSceneManager.ActiveSceneManager.IsRealtimeFadeInAni);
			} else {
				CSceneManager.OnShowTouchResponder(a_oKey, a_stColor, oTouchResponder, a_oCallback);
			}

			CSceneManager.m_oTouchResponderInfoDict.TryAdd(a_oKey, new STTouchResponderInfo() {
				m_oAni = oAni,
				m_oTouchResponder = oTouchResponder,
				m_oCallback = a_oCallback
			});

			return oTouchResponder;
		}

		return null;
	}

	/** 터치 응답자를 닫는다 */
	public static void CloseTouchResponder(string a_oKey, Color a_stColor, System.Action<GameObject> a_oCallback, float a_fDuration = KCDefine.B_VAL_0_REAL, System.Func<GameObject, string, Color, float, Tween> a_oAniCreator = null) {
		// 터치 응답자가 존재 할 경우
		if(CSceneManager.m_oTouchResponderInfoDict.TryGetValue(a_oKey, out STTouchResponderInfo stTouchResponderInfo)) {
			var oTouchDispatcher = stTouchResponderInfo.m_oTouchResponder.GetComponentInChildren<CTouchDispatcher>();
			oTouchDispatcher.SetDestroyCallback(null);

			stTouchResponderInfo.m_oAni?.Kill();
			stTouchResponderInfo.m_oCallback?.Invoke(stTouchResponderInfo.m_oTouchResponder);

			CSceneManager.m_oTouchResponderInfoDict.ExRemoveVal(a_oKey);

			// 내비게이션 스택 콜백이 존재 할 경우
			if(oTouchDispatcher.NavStackCallback != null) {
				CNavStackManager.Inst.RemoveComponent(oTouchDispatcher);
			}

			// 애니메이션 모드 일 경우
			if(a_oAniCreator != null) {
				CFactory.MakeSequence(a_oAniCreator(stTouchResponderInfo.m_oTouchResponder, a_oKey, a_stColor, a_fDuration), (a_oSender) => {
					a_oSender.Kill();
					CSceneManager.OnCloseTouchResponder(stTouchResponderInfo.m_oTouchResponder, a_oCallback);
				}, KCDefine.B_VAL_0_REAL, false, CSceneManager.ActiveSceneManager.IsRealtimeFadeOutAni);
			} else {
				CSceneManager.OnCloseTouchResponder(stTouchResponderInfo.m_oTouchResponder, a_oCallback);
			}
		}
	}

	/** 터치 응답자가 출력 되었을 경우 */
	private static void OnShowTouchResponder(string a_oKey, Color a_stColor, GameObject a_oTouchResponder, System.Action<GameObject> a_oCallback) {
		try {
			// 터치 응답자 정보가 존재 할 경우
			if(CSceneManager.m_oTouchResponderInfoDict.TryGetValue(a_oKey, out STTouchResponderInfo stTouchResponderInfo)) {
				stTouchResponderInfo.m_oCallback = null;
				CSceneManager.m_oTouchResponderInfoDict.ExReplaceVal(a_oKey, stTouchResponderInfo);
			} else {
				a_oTouchResponder.GetComponentInChildren<Image>().color = a_stColor;
			}
		} finally {
			CFunc.Invoke(ref a_oCallback, a_oTouchResponder);
		}
	}

	/** 터치 응답자가 닫혔을 경우 */
	private static void OnCloseTouchResponder(GameObject a_oTouchResponder, System.Action<GameObject> a_oCallback) {
		try {
			CFunc.Invoke(ref a_oCallback, a_oTouchResponder);
		} finally {
			CFactory.RemoveObj(a_oTouchResponder);
		}
	}
	#endregion // 클래스 함수

	#region 조건부 함수
#if UNITY_EDITOR
	/** GUI 를 그린다 */
	public virtual void OnGUI() {
		// 주요 씬 일 경우
		if(this.IsActiveScene && CSceneManager.IsExistsMainCamera) {
			var stPrevColor = GUI.color;

			try {
				// 초기화가 필요 할 경우
				if(!CSceneManager.IsInit) {
					GUI.color = Color.black;
					GUI.DrawTexture(Camera.main.pixelRect, Texture2D.whiteTexture, ScaleMode.StretchToFill);
				}
			} finally {
				GUI.color = stPrevColor;
			}
		}
	}

	/** 기즈모를 그린다 */
	public virtual void OnDrawGizmos() {
		// 메인 카메라가 존재 할 경우
		if(CSceneManager.IsExistsMainCamera) {
			var stPrevColor = Gizmos.color;
			var stMainCameraPos = CSceneManager.ActiveSceneMainCamera.transform.position;
			var stPivotPos = stMainCameraPos + new Vector3(KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_0_REAL, this.PlaneDistance);

			try {
				var oAdsPosList = new List<Vector3>() {
					stPivotPos + new Vector3(CSceneManager.CanvasSize.x / -KCDefine.B_VAL_2_REAL, (CSceneManager.CanvasSize.y / -KCDefine.B_VAL_2_REAL) + (CAccess.GetBannerAdsHeight(KCDefine.U_SIZE_BANNER_ADS.y) * CAccess.ResolutionScale), KCDefine.B_VAL_0_REAL) * CAccess.ResolutionUnitScale,
					stPivotPos + new Vector3(CSceneManager.CanvasSize.x / -KCDefine.B_VAL_2_REAL, CSceneManager.CanvasSize.y / KCDefine.B_VAL_2_REAL, KCDefine.B_VAL_0_REAL) * CAccess.ResolutionUnitScale,
					stPivotPos + new Vector3(CSceneManager.CanvasSize.x / KCDefine.B_VAL_2_REAL, CSceneManager.CanvasSize.y / KCDefine.B_VAL_2_REAL, KCDefine.B_VAL_0_REAL) * CAccess.ResolutionUnitScale,
					stPivotPos + new Vector3(CSceneManager.CanvasSize.x / KCDefine.B_VAL_2_REAL, (CSceneManager.CanvasSize.y / -KCDefine.B_VAL_2_REAL) + (CAccess.GetBannerAdsHeight(KCDefine.U_SIZE_BANNER_ADS.y) * CAccess.ResolutionScale), KCDefine.B_VAL_0_REAL) * CAccess.ResolutionUnitScale
				};

				var oCanvasPosList = new List<Vector3>() {
					stPivotPos + new Vector3(CSceneManager.CanvasSize.x / -KCDefine.B_VAL_2_REAL, CSceneManager.CanvasSize.y / -KCDefine.B_VAL_2_REAL, KCDefine.B_VAL_0_REAL) * CAccess.ResolutionUnitScale,
					stPivotPos + new Vector3(CSceneManager.CanvasSize.x / -KCDefine.B_VAL_2_REAL, CSceneManager.CanvasSize.y / KCDefine.B_VAL_2_REAL, KCDefine.B_VAL_0_REAL) * CAccess.ResolutionUnitScale,
					stPivotPos + new Vector3(CSceneManager.CanvasSize.x / KCDefine.B_VAL_2_REAL, CSceneManager.CanvasSize.y / KCDefine.B_VAL_2_REAL, KCDefine.B_VAL_0_REAL) * CAccess.ResolutionUnitScale,
					stPivotPos + new Vector3(CSceneManager.CanvasSize.x / KCDefine.B_VAL_2_REAL, CSceneManager.CanvasSize.y / -KCDefine.B_VAL_2_REAL, KCDefine.B_VAL_0_REAL) * CAccess.ResolutionUnitScale
				};

				var oScreenPosList = new List<Vector3>() {
					stPivotPos + this.ObjRootPivotPos + new Vector3(this.ScreenSize.x / -KCDefine.B_VAL_2_REAL, this.ScreenSize.y / -KCDefine.B_VAL_2_REAL, KCDefine.B_VAL_0_REAL) * CAccess.ResolutionUnitScale,
					stPivotPos + this.ObjRootPivotPos + new Vector3(this.ScreenSize.x / -KCDefine.B_VAL_2_REAL, this.ScreenSize.y / KCDefine.B_VAL_2_REAL, KCDefine.B_VAL_0_REAL) * CAccess.ResolutionUnitScale,
					stPivotPos + this.ObjRootPivotPos + new Vector3(this.ScreenSize.x / KCDefine.B_VAL_2_REAL, this.ScreenSize.y / KCDefine.B_VAL_2_REAL, KCDefine.B_VAL_0_REAL) * CAccess.ResolutionUnitScale,
					stPivotPos + this.ObjRootPivotPos + new Vector3(this.ScreenSize.x / KCDefine.B_VAL_2_REAL, this.ScreenSize.y / -KCDefine.B_VAL_2_REAL, KCDefine.B_VAL_0_REAL) * CAccess.ResolutionUnitScale
				};

				for(int i = 0; i < oAdsPosList.Count; ++i) {
					int nIdx01 = (i + KCDefine.B_VAL_0_INT) % oAdsPosList.Count;
					int nIdx02 = (i + KCDefine.B_VAL_1_INT) % oAdsPosList.Count;

					Gizmos.color = Color.red;
					Gizmos.DrawLine(oAdsPosList[nIdx01], oAdsPosList[nIdx02]);

					Gizmos.color = Color.white;
					Gizmos.DrawLine(oCanvasPosList[nIdx01], oCanvasPosList[nIdx02]);

					Gizmos.color = Color.green;
					Gizmos.DrawLine(oScreenPosList[nIdx01], oScreenPosList[nIdx02]);
				}
			} finally {
				Gizmos.color = stPrevColor;
			}
		}
	}
#endif // #if UNITY_EDITOR

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
	/** 단축키를 처리한다 */
	private void HandleHotKeys() {
		// 씬 로드 키를 눌렀을 경우
		if(Input.GetKeyDown(KeyCode.U)) {
			Screen.SetResolution((int)CAccess.CorrectDesktopScreenSize.x, (int)CAccess.CorrectDesktopScreenSize.y, FullScreenMode.MaximizedWindow);
			CSceneLoader.Inst.LoadScene(this.SceneName);
		}
		// 시간 비율 키를 눌렀을 경우
		else if(Input.GetKey(KeyCode.Equals) || Input.GetKey(KeyCode.Minus)) {
			float fVelocity = Input.GetKey(KeyCode.Equals) ? KCDefine.B_VAL_1_REAL : -KCDefine.B_VAL_1_REAL;
			Time.timeScale = Mathf.Clamp01(Time.timeScale + (fVelocity * Time.unscaledDeltaTime));
		}
	}
#endif // #if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)

#if AB_TEST_ENABLE && (DEBUG || DEVELOPMENT_BUILD || PLAY_TEST_ENABLE)
	/** AB 테스트 UI 세트 버튼을 눌렀을 경우 */
	protected virtual void OnTouchABTUIsSetBtn(EUserType a_eUserType) {
		// Do Something
	}

	/** AB 세트 팝업 결과를 수신했을 경우 */
	protected virtual void OnReceiveABSetPopupResult(CAlertPopup a_oSender, bool a_bIsOK, EUserType a_eUserType) {
		// 확인 버튼을 눌렀을 경우
		if(a_bIsOK) {
			CCommonUserInfoStorage.Inst.UserInfo.UserType = a_eUserType;
			CCommonUserInfoStorage.Inst.SaveUserInfo();
		}
	}
#endif // #if AB_TEST_ENABLE && (DEBUG || DEVELOPMENT_BUILD || PLAY_TEST_ENABLE)
	#endregion // 조건부 함수
}

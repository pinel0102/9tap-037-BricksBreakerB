using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.EventSystems;

#if INPUT_SYSTEM_MODULE_ENABLE
using UnityEngine.InputSystem.UI;
#endif // #if INPUT_SYSTEM_MODULE_ENABLE

#if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE
using UnityEngine.Rendering.Universal;
#endif // #if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE

#if POST_PROCESSING_MODULE_ENABLE
using UnityEngine.Rendering.PostProcessing;
#endif // #if POST_PROCESSING_MODULE_ENABLE

/** 씬 관리자 - 설정 */
public abstract partial class CSceneManager : CComponent {
	#region 함수
	/** 씬을 설정한다 */
	protected virtual void SetupScene() {
		this.SetupScene(true);
		this.SetupScene(false);
	}

	/** 씬을 설정한다 */
	protected virtual void SetupScene(bool a_bIsMainSetup) {
		// 씬을 설정한다 {
		this.SetupLights();
		this.SetupDefObjs();
		this.SetupOffsets();

		// 주요 씬 일 경우
		if(this.IsActiveScene) {
			this.SetupActiveScene();
		} else {
			this.SetupAdditiveScene();
		}

		this.gameObject.ExReset();

		// 메인 설정이 일 경우
		if(a_bIsMainSetup) {
			// 최상단 객체 순서를 설정한다 {
			this.transform.SetParent(null);
			this.transform.SetAsFirstSibling();

			m_oObjDict.GetValueOrDefault(EKey.OBJS_ROOT).transform.SetParent(null);
			m_oObjDict.GetValueOrDefault(EKey.OBJS_ROOT).transform.SetAsFirstSibling();

			m_oUIsDict.GetValueOrDefault(EKey.UIS_ROOT).transform.SetParent(null);
			m_oUIsDict.GetValueOrDefault(EKey.UIS_ROOT).transform.SetAsFirstSibling();

			m_oObjDict.GetValueOrDefault(EKey.BASE).transform.SetParent(null);
			m_oObjDict.GetValueOrDefault(EKey.BASE).transform.SetAsFirstSibling();
			// 최상단 객체 순서를 설정한다 }

			// 캔버스 순서를 설정한다 {
			m_oCanvasDict.GetValueOrDefault(EKey.POPUP_UIS_CANVAS).overrideSorting = true;
			m_oCanvasDict.GetValueOrDefault(EKey.POPUP_UIS_CANVAS).overridePixelPerfect = false;

			m_oCanvasDict.GetValueOrDefault(EKey.TOPMOST_UIS_CANVAS).overrideSorting = true;
			m_oCanvasDict.GetValueOrDefault(EKey.TOPMOST_UIS_CANVAS).overridePixelPerfect = false;

			m_oCanvasDict.GetValueOrDefault(EKey.UIS_CANVAS).ExSetSortingOrder(this.UIsCanvasSortingOrderInfo);
			m_oCanvasDict.GetValueOrDefault(EKey.POPUP_UIS_CANVAS).ExSetSortingOrder(this.UIsCanvasSortingOrderInfo.ExGetExtraOrder(KCDefine.U_SORTING_O_SCREEN_POPUP_UIS));
			m_oCanvasDict.GetValueOrDefault(EKey.TOPMOST_UIS_CANVAS).ExSetSortingOrder(this.UIsCanvasSortingOrderInfo.ExGetExtraOrder(KCDefine.U_SORTING_O_SCREEN_TOPMOST_UIS));
			// 캔버스 순서를 설정한다 }

			// 카메라를 설정한다 {
			// 메인 카메라가 존재 할 경우
			if(m_oCameraDict.GetValueOrDefault(EKey.MAIN_CAMERA) != null) {
				m_oCameraDict.GetValueOrDefault(EKey.MAIN_CAMERA).clearFlags = !this.IsActiveScene ? CameraClearFlags.Nothing : (this.MainCameraProjection == EProjection._2D) ? CameraClearFlags.SolidColor : CameraClearFlags.Skybox;
				m_oCameraDict.GetValueOrDefault(EKey.MAIN_CAMERA).cullingMask = int.MaxValue;
				m_oCameraDict.GetValueOrDefault(EKey.MAIN_CAMERA).backgroundColor = this.ClearColor;
				m_oCameraDict.GetValueOrDefault(EKey.MAIN_CAMERA).depthTextureMode = DepthTextureMode.DepthNormals;

				m_oCameraDict.GetValueOrDefault(EKey.MAIN_CAMERA).ExReset();
				m_oCameraDict.GetValueOrDefault(EKey.MAIN_CAMERA).gameObject.SetActive(this.IsActiveScene);
				m_oCameraDict.GetValueOrDefault(EKey.MAIN_CAMERA).gameObject.ExSetEnableComponent<AudioListener>(this.IsActiveScene, false);

#if PHYSICS_RAYCASTER_ENABLE
				// 광선 추적자가 없을 경우
				if((this.MainCameraProjection == EProjection._2D && !m_oCameraDict.GetValueOrDefault(EKey.MAIN_CAMERA).gameObject.TryGetComponent<Physics2DRaycaster>(out Physics2DRaycaster oRaycaster2D)) || (this.MainCameraProjection == EProjection._3D && !m_oCameraDict.GetValueOrDefault(EKey.MAIN_CAMERA).gameObject.TryGetComponent<PhysicsRaycaster>(out PhysicsRaycaster oRaycaster3D))) {
					m_oCameraDict.GetValueOrDefault(EKey.MAIN_CAMERA).gameObject.ExRemoveComponent<BaseRaycaster>(false);
				}

				var oRaycaster = (this.MainCameraProjection == EProjection._2D) ? m_oCameraDict.GetValueOrDefault(EKey.MAIN_CAMERA).gameObject.ExAddComponent<Physics2DRaycaster>() : m_oCameraDict.GetValueOrDefault(EKey.MAIN_CAMERA).gameObject.ExAddComponent<PhysicsRaycaster>();
				oRaycaster.eventMask = oRaycaster.eventMask.ExGetLayerMask(int.MaxValue);
#else
				m_oCameraDict.GetValueOrDefault(EKey.MAIN_CAMERA).gameObject.ExRemoveComponent<BaseRaycaster>(false);
#endif // #if PHYSICS_RAYCASTER_ENABLE

#if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE
				var oCameraData = m_oCameraDict.GetValueOrDefault(EKey.MAIN_CAMERA).gameObject.ExAddComponent<UniversalAdditionalCameraData>();
				CSceneManager.m_oActiveSceneCameraDataDict.ExReplaceVal(EKey.MAIN_CAMERA_DATA, CSceneManager.m_oActiveSceneCameraDict.GetValueOrDefault(EKey.MAIN_CAMERA)?.GetComponent<UniversalAdditionalCameraData>());

				m_oCameraDict.GetValueOrDefault(EKey.MAIN_CAMERA).SetVolumeFrameworkUpdateMode(VolumeFrameworkUpdateMode.UsePipelineSettings);

				// 카메라 데이터가 존재 할 경우
				if(oCameraData != null && QualitySettings.renderPipeline != null && GraphicsSettings.renderPipelineAsset != null) {
					oCameraData.renderShadows = true;
					oCameraData.renderType = CameraRenderType.Base;
					oCameraData.volumeLayerMask = oCameraData.volumeLayerMask.ExGetLayerMask(int.MaxValue);

#if POST_PROCESSING_MODULE_ENABLE
					oCameraData.renderPostProcessing = this.IsActiveScene && !this.IsIgnoreRenderPostProcessing;
#else
					oCameraData.renderPostProcessing = false;
#endif // #if POST_PROCESSING_MODULE_ENABLE

					oCameraData.cameraStack?.Clear();
					this.SetupCameraData(oCameraData);
				}

				try {
					// 액티브 씬 메인 카메라 데이터가 존재 할 경우
					if(this.GetAdditionalCameras().ExIsValid() && CSceneManager.m_oActiveSceneCameraDataDict.GetValueOrDefault(EKey.MAIN_CAMERA_DATA) != null && QualitySettings.renderPipeline != null && GraphicsSettings.renderPipelineAsset != null) {
						var oCameraList = CSceneManager.m_oActiveSceneCameraDataDict.GetValueOrDefault(EKey.MAIN_CAMERA_DATA).cameraStack ?? new List<Camera>();
						oCameraList.ExAddVals(this.GetAdditionalCameras());

						oCameraList.Sort((a_oLhs, a_oRhs) => a_oLhs.depth.CompareTo(a_oRhs.depth));
					}
				} catch(System.Exception oException) {
					CFunc.ShowLogWarning($"CSceneManager.SetupScene Exception: {oException.Message}");
				}
#endif // #if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE

#if POST_PROCESSING_MODULE_ENABLE
				var oPostProcessingVolume = m_oCameraDict.GetValueOrDefault(EKey.MAIN_CAMERA).gameObject.ExAddComponent<PostProcessVolume>();
				oPostProcessingVolume?.ExSetEnable(this.IsActiveScene && !this.IsIgnoreGlobalPostProcessingVolume, false);

				// 포스트 프로세싱 볼륨이 존재 할 경우
				if(oPostProcessingVolume != null && CSceneManager.QualityLevel != EQualityLevel.NONE) {
					var oPostProcessingSettingsPathDict = new Dictionary<EQualityLevel, string>() {
						[EQualityLevel.NORM] = KCDefine.U_ASSET_P_G_NORM_QUALITY_POST_PROCESSING_SETTINGS,
						[EQualityLevel.HIGH] = KCDefine.U_ASSET_P_G_HIGH_QUALITY_POST_PROCESSING_SETTINGS,
						[EQualityLevel.ULTRA] = KCDefine.U_ASSET_P_G_ULTRA_QUALITY_POST_PROCESSING_SETTINGS
					};

					bool bIsValid01 = oPostProcessingVolume.sharedProfile != null;
					bool bIsValid02 = oPostProcessingVolume.sharedProfile != null && !oPostProcessingVolume.sharedProfile.name.Equals(oPostProcessingSettingsPathDict.GetValueOrDefault(CSceneManager.QualityLevel, string.Empty).ExGetFileName(false));

					var stResult = oPostProcessingSettingsPathDict.ExFindVal((a_stKeyVal) => {
						return oPostProcessingVolume.sharedProfile != null && a_stKeyVal.Value.ExGetFileName(false).Equals(oPostProcessingVolume.sharedProfile.name);
					});

					oPostProcessingVolume.isGlobal = true;
					oPostProcessingVolume.weight = KCDefine.B_VAL_1_REAL;
					oPostProcessingVolume.priority = KCDefine.B_VAL_0_REAL;

					// 포스트 프로세싱 설정이 없을 경우
					if((!bIsValid01 || (bIsValid02 && stResult.Item1)) && CAccess.IsExistsRes<PostProcessProfile>(oPostProcessingSettingsPathDict.GetValueOrDefault(CSceneManager.QualityLevel, string.Empty), true)) {
						oPostProcessingVolume.sharedProfile = Resources.Load<PostProcessProfile>(oPostProcessingSettingsPathDict.GetValueOrDefault(CSceneManager.QualityLevel, string.Empty));
					}
				}
#endif // #if POST_PROCESSING_MODULE_ENABLE
			}

			// 추가 카메라가 존재 할 경우
			if(this.GetAdditionalCameras().ExIsValid()) {
				try {
					for(int i = 0; i < this.GetAdditionalCameras().Count; ++i) {
						this.GetAdditionalCameras()[i].clearFlags = CameraClearFlags.Depth;
						this.GetAdditionalCameras()[i].backgroundColor = this.ClearColor;
						this.GetAdditionalCameras()[i].depthTextureMode = DepthTextureMode.DepthNormals;

						this.GetAdditionalCameras()[i].ExReset();
						this.GetAdditionalCameras()[i].gameObject.ExSetEnableComponent<AudioListener>(false, false);

						this.SetupAdditionalCamera(this.GetAdditionalCameras()[i]);

						// 컬링 마스크 설정이 가능 할 경우
						if(!this.GetAdditionalCameras()[i].name.Contains(KCDefine.B_NAME_PATTERN_IGNORE_SETUP_CULLING_MASK)) {
							this.GetAdditionalCameras()[i].cullingMask = int.MaxValue;
						}

#if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE
						var oCameraData = this.GetAdditionalCameras()[i].gameObject.ExAddComponent<UniversalAdditionalCameraData>();
						this.GetAdditionalCameras()[i].SetVolumeFrameworkUpdateMode(VolumeFrameworkUpdateMode.UsePipelineSettings);

						// 카메라 데이터가 존재 할 경우
						if(oCameraData != null && QualitySettings.renderPipeline != null && GraphicsSettings.renderPipelineAsset != null) {
							oCameraData.renderShadows = false;
							oCameraData.renderPostProcessing = false;
							oCameraData.renderType = CameraRenderType.Overlay;

							this.SetupCameraData(oCameraData);
							oCameraData.ExSetRuntimeFieldVal<UniversalAdditionalCameraData>(KCDefine.U_FIELD_N_CLEAR_DEPTH, true);
						}
#endif // #if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE

#if POST_PROCESSING_MODULE_ENABLE
						CFunc.RemoveObj(this.GetAdditionalCameras()[i].GetComponentInChildren<PostProcessVolume>(), false, false);
#endif // #if POST_PROCESSING_MODULE_ENABLE
					}
				} catch(System.Exception oException) {
					CFunc.ShowLogWarning($"CSubStartSceneManager.OnDestroy Exception: {oException.Message}");
					this.GetAdditionalCameras().Clear();
				}
			}
			// 카메라를 설정한다 }
		}
	}

	/** 주요 씬을 설정한다 */
	protected virtual void SetupActiveScene() {
		//this.SetupCamera();
		this.SetupRootObjs();

		// 이벤트 시스템을 설정한다
		CSceneManager.m_oActiveSceneUIsDict.GetValueOrDefault(EKey.UIS_ROOT).ExSetEnableComponent<EventSystem>(true, false);
		CSceneManager.m_oActiveSceneUIsDict.GetValueOrDefault(EKey.UIS_ROOT).ExSetEnableComponent<BaseInputModule>(true, false);

		// 앱이 실행 중 일 경우
		if(Application.isPlaying) {
			// 캔버스를 설정한다 {
			this.SetupCanvas(CSceneManager.ScreenBlindUIs?.GetComponentInParent<Canvas>(), new List<string>() {
				KCDefine.U_OBJ_N_SCREEN_BLIND_UIS
			}, false);

			this.SetupCanvas(CSceneManager.ScreenPopupUIs?.GetComponentInParent<Canvas>(), new List<string>() {
				KCDefine.U_OBJ_N_SCREEN_POPUP_UIS
			}, false);

			this.SetupCanvas(CSceneManager.ScreenTopmostUIs?.GetComponentInParent<Canvas>(), new List<string>() {
				KCDefine.U_OBJ_N_SCREEN_TOPMOST_UIS
			}, false);

			this.SetupCanvas(CSceneManager.ScreenAbsUIs?.GetComponentInParent<Canvas>(), new List<string>() {
				KCDefine.U_OBJ_N_SCREEN_ABS_UIS
			}, false);

#if DEBUG || DEVELOPMENT_BUILD
			this.SetupCanvas(CSceneManager.ScreenDebugUIs?.GetComponentInParent<Canvas>(), new List<string>() {
				KCDefine.U_OBJ_N_SCREEN_DEBUG_UIS
			}, false);
#endif // #if DEBUG || DEVELOPMENT_BUILD
			// 캔버스를 설정한다 }

			// 디버그 버튼을 설정한다 {
#if DEBUG || DEVELOPMENT_BUILD
			CSceneManager.ScreenFPSInfoBtn?.gameObject.SetActive(true);
			CSceneManager.ScreenFPSInfoBtn?.ExAddListener(CSceneManager.OnTouchScreenFPSInfoBtn);

			CSceneManager.ScreenDebugInfoBtn?.gameObject.SetActive(true);
			CSceneManager.ScreenDebugInfoBtn?.ExAddListener(CSceneManager.OnTouchScreenDebugInfoBtn);

			CSceneManager.ScreenTimeScaleUpBtn?.gameObject.SetActive(true);
			CSceneManager.ScreenTimeScaleUpBtn?.ExAddListener(CSceneManager.OnTouchScreenTimeScaleUpBtn);

			CSceneManager.ScreenTimeScaleDownBtn?.gameObject.SetActive(true);
			CSceneManager.ScreenTimeScaleDownBtn?.ExAddListener(CSceneManager.OnTouchScreenTimeScaleDownBtn);
#endif // #if DEBUG || DEVELOPMENT_BUILD
			// 디버그 버튼을 설정한다 }
		}
	}

	/** 추가 씬을 설정한다 */
	protected virtual void SetupAdditiveScene() {
		// 카메라를 설정한다
		m_oCameraDict.ExReplaceVal(EKey.MAIN_CAMERA, this.gameObject.scene.ExFindComponent<Camera>(KCDefine.U_OBJ_N_SCENE_MAIN_CAMERA));

		// UI 를 설정한다 {
		m_oUIsDict.ExReplaceVal(EKey.UIS, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_UIS));
		m_oUIsDict.ExReplaceVal(EKey.UIS_ROOT, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_UIS_ROOT));
		m_oUIsDict.ExReplaceVal(EKey.UIS_BASE, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_UIS_BASE));

		m_oUIsDict.ExReplaceVal(EKey.PIVOT_UIS, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_PIVOT_UIS));
		m_oUIsDict.ExReplaceVal(EKey.ANCHOR_UIS, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_ANCHOR_UIS));
		m_oUIsDict.ExReplaceVal(EKey.CORNER_ANCHOR_UIS, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_CORNER_ANCHOR_UIS));

		m_oUIsDict.ExReplaceVal(EKey.UP_UIS, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_UP_UIS));
		m_oUIsDict.ExReplaceVal(EKey.DOWN_UIS, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_DOWN_UIS));
		m_oUIsDict.ExReplaceVal(EKey.LEFT_UIS, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_LEFT_UIS));
		m_oUIsDict.ExReplaceVal(EKey.RIGHT_UIS, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_RIGHT_UIS));

		m_oUIsDict.ExReplaceVal(EKey.UP_LEFT_UIS, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_UP_LEFT_UIS));
		m_oUIsDict.ExReplaceVal(EKey.UP_RIGHT_UIS, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_UP_RIGHT_UIS));
		m_oUIsDict.ExReplaceVal(EKey.DOWN_LEFT_UIS, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_DOWN_LEFT_UIS));
		m_oUIsDict.ExReplaceVal(EKey.DOWN_RIGHT_UIS, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_DOWN_RIGHT_UIS));

		m_oUIsDict.ExReplaceVal(EKey.POPUP_UIS, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_POPUP_UIS));
		m_oUIsDict.ExReplaceVal(EKey.TOPMOST_UIS, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_TOPMOST_UIS));
		// UI 를 설정한다 }

		// 객체를 설정한다 {
		m_oObjDict.ExReplaceVal(EKey.BASE, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_BASE));
		m_oObjDict.ExReplaceVal(EKey.OBJS, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_OBJS));
		m_oObjDict.ExReplaceVal(EKey.OBJS_ROOT, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_OBJS_ROOT));

		m_oObjDict.ExReplaceVal(EKey.PIVOT_OBJS, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_PIVOT_OBJS));
		m_oObjDict.ExReplaceVal(EKey.ANCHOR_OBJS, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_ANCHOR_OBJS));
		m_oObjDict.ExReplaceVal(EKey.STATIC_OBJS, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_STATIC_OBJS));

		m_oObjDict.ExReplaceVal(EKey.ADDITIONAL_LIGHTS, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_ADDITIONAL_LIGHTS));
		m_oObjDict.ExReplaceVal(EKey.ADDITIONAL_CAMERAS, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_ADDITIONAL_CAMERAS));
		m_oObjDict.ExReplaceVal(EKey.REFLECTION_PROBES, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_REFLECTION_PROBES));
		m_oObjDict.ExReplaceVal(EKey.LIGHT_PROBE_GROUPS, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_LIGHT_PROBE_GROUPS));

		m_oObjDict.ExReplaceVal(EKey.UP_OBJS, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_UP_OBJS));
		m_oObjDict.ExReplaceVal(EKey.DOWN_OBJS, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_DOWN_OBJS));
		m_oObjDict.ExReplaceVal(EKey.LEFT_OBJS, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_LEFT_OBJS));
		m_oObjDict.ExReplaceVal(EKey.RIGHT_OBJS, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_RIGHT_OBJS));
		// 객체를 설정한다 }

		// 캔버스를 설정한다
		m_oCanvasDict.ExReplaceVal(EKey.UIS_CANVAS, this.UIsBase.GetComponentInChildren<Canvas>());
		m_oCanvasDict.ExReplaceVal(EKey.POPUP_UIS_CANVAS, this.PopupUIs.GetComponentInChildren<Canvas>());
		m_oCanvasDict.ExReplaceVal(EKey.TOPMOST_UIS_CANVAS, this.TopmostUIs.GetComponentInChildren<Canvas>());

		// 최상단 객체를 설정한다
		//this.SetupCamera();
		this.SetupRootObjs();

		// 고유 객체를 설정한다 {
		var oObjs = this.gameObject.scene.GetRootGameObjects();

		for(int i = 0; i < oObjs.Length; ++i) {
			var oEventSystems = oObjs[i].GetComponentsInChildren<EventSystem>(true);
			var oInputModules = oObjs[i].GetComponentsInChildren<BaseInputModule>(true);
			var oAudioListeners = oObjs[i].GetComponentsInChildren<AudioListener>(true);

			this.SetupUniqueComponents(oEventSystems.ExToTypes<Behaviour>(), false);
			this.SetupUniqueComponents(oInputModules.ExToTypes<Behaviour>(), false);
			this.SetupUniqueComponents(oAudioListeners.ExToTypes<Behaviour>(), false);
		}
		// 고유 객체를 설정한다 }
	}

	/** 광원을 설정한다 */
	protected virtual void SetupLights() {
		// 주요 씬 일 경우
		if(this.IsActiveScene) {
#if UNITY_EDITOR
			var oIsSetupOptsList = new List<bool>() {
				RenderSettings.skybox != null && !RenderSettings.skybox.name.Equals(KCDefine.U_MAT_N_DEF_SKYBOX),
				RenderSettings.ambientMode == AmbientMode.Skybox,
				RenderSettings.defaultReflectionMode == DefaultReflectionMode.Skybox,

				RenderSettings.ambientIntensity.ExIsEquals(KCDefine.B_VAL_1_REAL),
				RenderSettings.reflectionIntensity.ExIsEquals(KCDefine.B_VAL_1_REAL),
				RenderSettings.subtractiveShadowColor.ExIsEquals(Color.white)
			};

			// 설정 갱신이 필요 할 경우
			if(oIsSetupOptsList.Contains(false)) {
				RenderSettings.skybox = Resources.Load<Material>(KCDefine.U_MAT_P_G_SKYBOX);
				RenderSettings.ambientMode = AmbientMode.Skybox;
				RenderSettings.defaultReflectionMode = DefaultReflectionMode.Skybox;

				RenderSettings.ambientIntensity = Mathf.Max(KCDefine.B_VAL_1_REAL, RenderSettings.ambientIntensity);
				RenderSettings.reflectionIntensity = Mathf.Max(KCDefine.B_VAL_1_REAL, RenderSettings.reflectionIntensity);
				RenderSettings.subtractiveShadowColor = Color.white;
			}
#endif // #if UNITY_EDITOR
		}

		this.gameObject.scene.ExEnumerateComponents<Light>((a_oLight) => {
			a_oLight.shadows = a_oLight.name.Equals(KCDefine.U_OBJ_N_SCENE_MAIN_LIGHT) ? LightShadows.Soft : LightShadows.Hard;
			a_oLight.intensity = a_oLight.name.Equals(KCDefine.U_OBJ_N_SCENE_MAIN_LIGHT) ? Mathf.Max(KCDefine.B_VAL_1_REAL, a_oLight.intensity) : Mathf.Clamp(a_oLight.intensity, KCDefine.B_VAL_0_REAL, KCDefine.B_ADDITIONAL_LIGHT_INTENSITY);
			a_oLight.renderMode = a_oLight.name.Equals(KCDefine.U_OBJ_N_SCENE_MAIN_LIGHT) ? LightRenderMode.ForcePixel : LightRenderMode.Auto;
			a_oLight.shadowBias = KCDefine.B_VAL_1_REAL;
			a_oLight.shadowStrength = KCDefine.B_VAL_1_REAL;
			a_oLight.shadowNearPlane = KCDefine.U_DISTANCE_CAMERA_NEAR_PLANE;
			a_oLight.shadowNormalBias = KCDefine.B_VAL_1_REAL;
			a_oLight.shadowResolution = LightShadowResolution.FromQualitySettings;
			a_oLight.transform.localScale = Vector3.one;

#if LIGHT_ENABLE
			// 메인 방향 광원 일 경우
			if(this.IsActiveScene && a_oLight.name.Equals(KCDefine.U_OBJ_N_SCENE_MAIN_LIGHT)) {
				a_oLight.gameObject.SetActive(true);
			} else {
				a_oLight.gameObject.SetActive(this.IsActiveScene ? a_oLight.gameObject.activeSelf : false);
			}
#else
			a_oLight.gameObject.SetActive(false);
#endif // #if LIGHT_ENABLE

			// 메인 방향 광원 일 경우
			if(a_oLight.type == LightType.Directional && a_oLight.name.Equals(KCDefine.U_OBJ_N_SCENE_MAIN_LIGHT)) {
				a_oLight.cullingMask = int.MaxValue;
				a_oLight.transform.localEulerAngles = this.IsResetMainDirectionalLightAngle ? new Vector3(KCDefine.B_ANGLE_45_DEG, KCDefine.B_ANGLE_45_DEG, KCDefine.B_VAL_0_REAL) : a_oLight.transform.localEulerAngles;
				a_oLight.transform.localPosition = Vector3.zero;

#if UNITY_EDITOR
				a_oLight.lightmapBakeType = LightmapBakeType.Realtime;
				RenderSettings.sun = (RenderSettings.sun != a_oLight) ? a_oLight : RenderSettings.sun;
#endif // #if UNITY_EDITOR
			} else {
				a_oLight.cullingMask = a_oLight.name.Contains(KCDefine.B_NAME_PATTERN_IGNORE_SETUP_CULLING_MASK) ? a_oLight.cullingMask : int.MaxValue;

#if UNITY_EDITOR
				a_oLight.lightmapBakeType = (a_oLight.lightmapBakeType == LightmapBakeType.Realtime) ? LightmapBakeType.Mixed : a_oLight.lightmapBakeType;
#endif // #if UNITY_EDITOR
			}

#if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE
			var oLightData = a_oLight.gameObject.ExAddComponent<UniversalAdditionalLightData>();

			// 광원 데이터가 존재 할 경우
			if(oLightData != null && QualitySettings.renderPipeline != null && GraphicsSettings.renderPipelineAsset != null) {
				oLightData.usePipelineSettings = true;
			}
#endif // #if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE

			return true;
		}, true);
	}

	/** 간격을 설정한다 */
	protected virtual void SetupOffsets() {
		// 액티브 씬 UI 캔버스가 존재 할 경우
		if(CSceneManager.m_oActiveSceneCanvasDict.GetValueOrDefault(EKey.UIS_CANVAS) != null) {
			CSceneManager.CanvasSize = (CSceneManager.m_oActiveSceneCanvasDict.GetValueOrDefault(EKey.UIS_CANVAS).transform as RectTransform).sizeDelta.ExTo3D();
			CSceneManager.CanvasScale = (CSceneManager.m_oActiveSceneCanvasDict.GetValueOrDefault(EKey.UIS_CANVAS).transform as RectTransform).localScale;
			CSceneManager.ObjsRootScale = new Vector3(CAccess.ResolutionScale, CAccess.ResolutionScale, CAccess.ResolutionScale);
		}

		// 메인 카메라가 존재 할 경우
		if(CSceneManager.IsExistsMainCamera) {
			CSceneManager.UpSafeAreaOffset = CSceneManager.CanvasSize.y * -CAccess.UpSafeAreaScale;
			CSceneManager.DownSafeAreaOffset = CSceneManager.CanvasSize.y * CAccess.DownSafeAreaScale;
			CSceneManager.LeftSafeAreaOffset = CSceneManager.CanvasSize.x * CAccess.LeftSafeAreaScale;
			CSceneManager.RightSafeAreaOffset = CSceneManager.CanvasSize.x * -CAccess.RightSafeAreaScale;
		}
	}

	/** 최상단 객체를 설정한다 */
	protected virtual void SetupRootObjs() {
		m_oUIsDict.GetValueOrDefault(EKey.UIS).ExReset();
		m_oUIsDict.GetValueOrDefault(EKey.UIS_ROOT).ExReset();
		m_oUIsDict.GetValueOrDefault(EKey.POPUP_UIS).ExReset();
		m_oUIsDict.GetValueOrDefault(EKey.TOPMOST_UIS).ExReset();

		m_oObjDict.GetValueOrDefault(EKey.BASE).ExReset();
		m_oObjDict.GetValueOrDefault(EKey.OBJS).ExReset();
		m_oObjDict.GetValueOrDefault(EKey.ANCHOR_OBJS).ExReset();
		m_oObjDict.GetValueOrDefault(EKey.STATIC_OBJS)?.ExReset(false);
		m_oObjDict.GetValueOrDefault(EKey.ADDITIONAL_LIGHTS)?.ExReset(false);
		m_oObjDict.GetValueOrDefault(EKey.ADDITIONAL_CAMERAS)?.ExReset(false);
		m_oObjDict.GetValueOrDefault(EKey.REFLECTION_PROBES)?.ExReset(false);

		m_oObjDict.GetValueOrDefault(EKey.OBJS_ROOT).ExReset();
		m_oObjDict.GetValueOrDefault(EKey.OBJS_ROOT).transform.localScale = CSceneManager.ObjsRootScale * KCDefine.B_UNIT_SCALE;

		m_oObjDict.GetValueOrDefault(EKey.UP_OBJS).ExReset();
		m_oObjDict.GetValueOrDefault(EKey.UP_OBJS).transform.localPosition = new Vector3(KCDefine.B_VAL_0_REAL, (CSceneManager.CanvasSize.y / KCDefine.B_VAL_2_REAL) + CSceneManager.UpSafeAreaOffset, KCDefine.B_VAL_0_REAL);

		m_oObjDict.GetValueOrDefault(EKey.DOWN_OBJS).ExReset();
		m_oObjDict.GetValueOrDefault(EKey.DOWN_OBJS).transform.localPosition = new Vector3(KCDefine.B_VAL_0_REAL, (CSceneManager.CanvasSize.y / -KCDefine.B_VAL_2_REAL) + CSceneManager.DownSafeAreaOffset, KCDefine.B_VAL_0_REAL);

		m_oObjDict.GetValueOrDefault(EKey.LEFT_OBJS).ExReset();
		m_oObjDict.GetValueOrDefault(EKey.LEFT_OBJS).transform.localPosition = new Vector3((CSceneManager.CanvasSize.x / -KCDefine.B_VAL_2_REAL) + CSceneManager.LeftSafeAreaOffset, KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_0_REAL);

		m_oObjDict.GetValueOrDefault(EKey.RIGHT_OBJS).ExReset();
		m_oObjDict.GetValueOrDefault(EKey.RIGHT_OBJS).transform.localPosition = new Vector3((CSceneManager.CanvasSize.x / KCDefine.B_VAL_2_REAL) + CSceneManager.RightSafeAreaOffset, KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_0_REAL);

		// 기준 객체가 존재 할 경우
		if(m_oObjDict.GetValueOrDefault(EKey.PIVOT_OBJS) != null) {
			m_oObjDict.GetValueOrDefault(EKey.PIVOT_OBJS).ExReset();
			m_oObjDict.GetValueOrDefault(EKey.PIVOT_OBJS).transform.localPosition = Vector3.zero.ExPivotPosToPos();
		}

		// 광원 프로브 그룹이 존재 할 경우
		if(m_oObjDict.GetValueOrDefault(EKey.LIGHT_PROBE_GROUPS) != null) {
			m_oObjDict.GetValueOrDefault(EKey.LIGHT_PROBE_GROUPS).ExReset();
			m_oObjDict.GetValueOrDefault(EKey.LIGHT_PROBE_GROUPS).transform.localScale = Vector3.one * KCDefine.B_UNIT_DIGITS_PER_HUNDRED;
		}
	}

	/** 기본 객체를 설정한다 */
	protected virtual void SetupDefObjs() {
		// 씬 관리자를 설정한다
		CSceneManager.m_oSceneManagerDict.TryAdd(this.SceneName, this);
		CSceneManager.m_oActiveSceneManagerDict.ExReplaceVal(EKey.ACTIVE_SCENE_MANAGER, CSceneManager.ActiveScene.ExFindComponent<CSceneManager>(KCDefine.U_OBJ_N_SCENE_MANAGER));

		// UI 를 설정한다 {
		m_oUIsDict.ExReplaceVal(EKey.UIS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_UIS));
		m_oUIsDict.ExReplaceVal(EKey.UIS_ROOT, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_UIS_ROOT));
		m_oUIsDict.ExReplaceVal(EKey.UIS_BASE, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_UIS_BASE));

		m_oUIsDict.ExReplaceVal(EKey.PIVOT_UIS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_PIVOT_UIS));
		m_oUIsDict.ExReplaceVal(EKey.ANCHOR_UIS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_ANCHOR_UIS));
		m_oUIsDict.ExReplaceVal(EKey.CORNER_ANCHOR_UIS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_CORNER_ANCHOR_UIS));

		m_oUIsDict.ExReplaceVal(EKey.UP_UIS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_UP_UIS));
		m_oUIsDict.ExReplaceVal(EKey.DOWN_UIS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_DOWN_UIS));
		m_oUIsDict.ExReplaceVal(EKey.LEFT_UIS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_LEFT_UIS));
		m_oUIsDict.ExReplaceVal(EKey.RIGHT_UIS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_RIGHT_UIS));

		m_oUIsDict.ExReplaceVal(EKey.UP_LEFT_UIS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_UP_LEFT_UIS));
		m_oUIsDict.ExReplaceVal(EKey.UP_RIGHT_UIS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_UP_RIGHT_UIS));
		m_oUIsDict.ExReplaceVal(EKey.DOWN_LEFT_UIS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_DOWN_LEFT_UIS));
		m_oUIsDict.ExReplaceVal(EKey.DOWN_RIGHT_UIS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_DOWN_RIGHT_UIS));

		m_oUIsDict.ExReplaceVal(EKey.POPUP_UIS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_POPUP_UIS));
		m_oUIsDict.ExReplaceVal(EKey.TOPMOST_UIS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_TOPMOST_UIS));

		m_oUIsDict.ExReplaceVal(EKey.TEST_UIS, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_TEST_UIS));
		m_oUIsDict.ExReplaceVal(EKey.DESIGN_RESOLUTION_GUIDE_UIS, this.gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_DESIGN_RESOLUTION_GUIDE_UIS));

		m_oUIsDict.ExReplaceVal(EKey.TEST_CONTENTS_UIS, m_oUIsDict.GetValueOrDefault(EKey.TEST_UIS)?.ExFindChild($"{EKey.TEST_CONTENTS_UIS}"));
		m_oUIsDict.GetValueOrDefault(EKey.TEST_CONTENTS_UIS)?.ExSetLocalPosX(Application.isPlaying ? -this.ScreenWidth : KCDefine.B_VAL_0_REAL, false);

		CSceneManager.m_oActiveSceneUIsDict.ExReplaceVal(EKey.UIS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_UIS));
		CSceneManager.m_oActiveSceneUIsDict.ExReplaceVal(EKey.UIS_ROOT, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_UIS_ROOT));
		CSceneManager.m_oActiveSceneUIsDict.ExReplaceVal(EKey.UIS_BASE, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_UIS_BASE));

		CSceneManager.m_oActiveSceneUIsDict.ExReplaceVal(EKey.PIVOT_UIS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_PIVOT_UIS));
		CSceneManager.m_oActiveSceneUIsDict.ExReplaceVal(EKey.ANCHOR_UIS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_ANCHOR_UIS));
		CSceneManager.m_oActiveSceneUIsDict.ExReplaceVal(EKey.CORNER_ANCHOR_UIS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_CORNER_ANCHOR_UIS));

		CSceneManager.m_oActiveSceneUIsDict.ExReplaceVal(EKey.UP_UIS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_UP_UIS));
		CSceneManager.m_oActiveSceneUIsDict.ExReplaceVal(EKey.DOWN_UIS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_DOWN_UIS));
		CSceneManager.m_oActiveSceneUIsDict.ExReplaceVal(EKey.LEFT_UIS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_LEFT_UIS));
		CSceneManager.m_oActiveSceneUIsDict.ExReplaceVal(EKey.RIGHT_UIS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_RIGHT_UIS));

		CSceneManager.m_oActiveSceneUIsDict.ExReplaceVal(EKey.UP_LEFT_UIS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_UP_LEFT_UIS));
		CSceneManager.m_oActiveSceneUIsDict.ExReplaceVal(EKey.UP_RIGHT_UIS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_UP_RIGHT_UIS));
		CSceneManager.m_oActiveSceneUIsDict.ExReplaceVal(EKey.DOWN_LEFT_UIS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_DOWN_LEFT_UIS));
		CSceneManager.m_oActiveSceneUIsDict.ExRemoveVal(EKey.DOWN_RIGHT_UIS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_DOWN_RIGHT_UIS));

		CSceneManager.m_oActiveSceneUIsDict.ExReplaceVal(EKey.POPUP_UIS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_POPUP_UIS));
		CSceneManager.m_oActiveSceneUIsDict.ExReplaceVal(EKey.TOPMOST_UIS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_TOPMOST_UIS));
		// UI 를 설정한다 }

		// 객체를 설정한다 {
		m_oObjDict.ExReplaceVal(EKey.BASE, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_BASE));
		m_oObjDict.ExReplaceVal(EKey.OBJS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_OBJS));
		m_oObjDict.ExReplaceVal(EKey.OBJS_ROOT, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_OBJS_ROOT));

		m_oObjDict.ExReplaceVal(EKey.PIVOT_OBJS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_PIVOT_OBJS));
		m_oObjDict.ExReplaceVal(EKey.ANCHOR_OBJS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_ANCHOR_OBJS));
		m_oObjDict.ExReplaceVal(EKey.STATIC_OBJS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_STATIC_OBJS));

		m_oObjDict.ExReplaceVal(EKey.ADDITIONAL_LIGHTS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_ADDITIONAL_LIGHTS));
		m_oObjDict.ExReplaceVal(EKey.ADDITIONAL_CAMERAS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_ADDITIONAL_CAMERAS));
		m_oObjDict.ExReplaceVal(EKey.REFLECTION_PROBES, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_REFLECTION_PROBES));
		m_oObjDict.ExReplaceVal(EKey.LIGHT_PROBE_GROUPS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_LIGHT_PROBE_GROUPS));

		m_oObjDict.ExReplaceVal(EKey.UP_OBJS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_UP_OBJS));
		m_oObjDict.ExReplaceVal(EKey.DOWN_OBJS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_DOWN_OBJS));
		m_oObjDict.ExReplaceVal(EKey.LEFT_OBJS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_LEFT_OBJS));
		m_oObjDict.ExReplaceVal(EKey.RIGHT_OBJS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_RIGHT_OBJS));

		CSceneManager.m_oActiveSceneObjDict.ExReplaceVal(EKey.BASE, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_BASE));
		CSceneManager.m_oActiveSceneObjDict.ExReplaceVal(EKey.OBJS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_OBJS));
		CSceneManager.m_oActiveSceneObjDict.ExReplaceVal(EKey.OBJS_ROOT, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_OBJS_ROOT));

		CSceneManager.m_oActiveSceneObjDict.ExReplaceVal(EKey.PIVOT_OBJS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_PIVOT_OBJS));
		CSceneManager.m_oActiveSceneObjDict.ExReplaceVal(EKey.ANCHOR_OBJS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_ANCHOR_OBJS));
		CSceneManager.m_oActiveSceneObjDict.ExReplaceVal(EKey.STATIC_OBJS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_STATIC_OBJS));

		CSceneManager.m_oActiveSceneObjDict.ExReplaceVal(EKey.ADDITIONAL_LIGHTS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_ADDITIONAL_LIGHTS));
		CSceneManager.m_oActiveSceneObjDict.ExReplaceVal(EKey.ADDITIONAL_CAMERAS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_ADDITIONAL_CAMERAS));
		CSceneManager.m_oActiveSceneObjDict.ExReplaceVal(EKey.REFLECTION_PROBES, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_REFLECTION_PROBES));
		CSceneManager.m_oActiveSceneObjDict.ExReplaceVal(EKey.LIGHT_PROBE_GROUPS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_LIGHT_PROBE_GROUPS));

		CSceneManager.m_oActiveSceneObjDict.ExReplaceVal(EKey.UP_OBJS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_UP_OBJS));
		CSceneManager.m_oActiveSceneObjDict.ExReplaceVal(EKey.DOWN_OBJS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_DOWN_OBJS));
		CSceneManager.m_oActiveSceneObjDict.ExReplaceVal(EKey.LEFT_OBJS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_LEFT_OBJS));
		CSceneManager.m_oActiveSceneObjDict.ExReplaceVal(EKey.RIGHT_OBJS, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindChild(KCDefine.U_OBJ_N_SCENE_RIGHT_OBJS));
		// 객체를 설정한다 }

		// 카메라를 설정한다
		m_oCameraDict.ExReplaceVal(EKey.MAIN_CAMERA, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindComponent<Camera>(KCDefine.U_OBJ_N_SCENE_MAIN_CAMERA));
		CSceneManager.m_oActiveSceneCameraDict.ExReplaceVal(EKey.MAIN_CAMERA, CSceneManager.m_oActiveSceneManagerDict.GetValueOrDefault(EKey.ACTIVE_SCENE_MANAGER).gameObject.scene.ExFindComponent<Camera>(KCDefine.U_OBJ_N_SCENE_MAIN_CAMERA));

		// 캔버스를 설정한다 {
		m_oCanvasDict.ExReplaceVal(EKey.UIS_CANVAS, CSceneManager.m_oActiveSceneUIsDict.GetValueOrDefault(EKey.UIS_BASE).GetComponentInChildren<Canvas>());
		m_oCanvasDict.ExReplaceVal(EKey.POPUP_UIS_CANVAS, CSceneManager.ActiveScenePopupUIs.GetComponentInChildren<Canvas>());
		m_oCanvasDict.ExReplaceVal(EKey.TOPMOST_UIS_CANVAS, CSceneManager.ActiveSceneTopmostUIs.GetComponentInChildren<Canvas>());

		CSceneManager.m_oActiveSceneCanvasDict.ExReplaceVal(EKey.UIS_CANVAS, CSceneManager.m_oActiveSceneUIsDict.GetValueOrDefault(EKey.UIS_BASE).GetComponentInChildren<Canvas>());
		CSceneManager.m_oActiveSceneCanvasDict.ExReplaceVal(EKey.POPUP_UIS_CANVAS, CSceneManager.ActiveScenePopupUIs.GetComponentInChildren<Canvas>());
		CSceneManager.m_oActiveSceneCanvasDict.ExReplaceVal(EKey.TOPMOST_UIS_CANVAS, CSceneManager.ActiveSceneTopmostUIs.GetComponentInChildren<Canvas>());
		// 캔버스를 설정한다 }

		// 이벤트 시스템을 설정한다 {
		var oEventSystem = CSceneManager.m_oActiveSceneUIsDict.GetValueOrDefault(EKey.UIS_ROOT).GetComponentInChildren<EventSystem>();
		oEventSystem?.gameObject.ExReset(false);

		// 이벤트 시스템이 존재 할 경우
		if(this.IsActiveScene && oEventSystem != null) {
			oEventSystem.sendNavigationEvents = false;
			oEventSystem.pixelDragThreshold = KCDefine.U_THRESHOLD_INPUT_M_MOVE;

#if INPUT_SYSTEM_MODULE_ENABLE
			// 입력 모듈이 없을 경우
			if(!oEventSystem.TryGetComponent<InputSystemUIInputModule>(out InputSystemUIInputModule oInputModule)) {
				oEventSystem.gameObject.ExRemoveComponent<BaseInputModule>(false);
			}

			var oInputSystemModule = oEventSystem.gameObject.ExAddComponent<InputSystemUIInputModule>();
			oInputSystemModule.deselectOnBackgroundClick = true;
			oInputSystemModule.moveRepeatRate = KCDefine.U_RATE_INPUT_M_MOVE_REPEAT;
			oInputSystemModule.moveRepeatDelay = KCDefine.U_DELAY_INPUT_M_MOVE_REPEAT;

#if MULTI_TOUCH_ENABLE
			oInputSystemModule.pointerBehavior = UIPointerBehavior.SingleMouseOrPenButMultiTouchAndTrack;
#else
			oInputSystemModule.pointerBehavior = UIPointerBehavior.SingleUnifiedPointer;
#endif // #if MULTI_TOUCH_ENABLE
#else
			// 입력 모듈이 없을 경우
			if(!oEventSystem.TryGetComponent<StandaloneInputModule>(out StandaloneInputModule oInputModule)) {
				oEventSystem.gameObject.ExRemoveComponent<BaseInputModule>(false);
			}

			var oStandaloneInputModule = oEventSystem.gameObject.ExAddComponent<StandaloneInputModule>();
			oStandaloneInputModule.repeatDelay = KCDefine.U_DELAY_INPUT_M_MOVE_REPEAT;
			oStandaloneInputModule.inputActionsPerSecond = KCDefine.U_UNIT_INPUT_M_INPUT_ACTIONS_PER_SEC;
#endif // #if INPUT_SYSTEM_MODULE_ENABLE
		}

		CSceneManager.m_oActiveSceneEventSystemDict.ExReplaceVal(EKey.EVENT_SYSTEM, oEventSystem);
		CSceneManager.m_oActiveSceneInputModuleDict.ExReplaceVal(EKey.BASE_INPUT_MODULE, oEventSystem?.GetComponentInChildren<BaseInputModule>());
		// 이벤트 시스템을 설정한다 }

		// 그래픽 광선 추적자를 설정한다 {
		var oRaycasters = m_oUIsDict.GetValueOrDefault(EKey.UIS_ROOT).GetComponentsInChildren<GraphicRaycaster>();

		for(int i = 0; i < oRaycasters.Length; ++i) {
			oRaycasters[i].ignoreReversedGraphics = true;
			oRaycasters[i].blockingMask = oRaycasters[i].blockingMask.ExGetLayerMask(KCDefine.B_VAL_0_INT);
			oRaycasters[i].blockingObjects = GraphicRaycaster.BlockingObjects.None;
		}
		// 그래픽 광선 추적자를 설정한다 }

		// 최상단 객체를 설정한다
		this.gameObject.SetActive(true);
		m_oObjDict.GetValueOrDefault(EKey.BASE).gameObject.SetActive(true);
		m_oUIsDict.GetValueOrDefault(EKey.UIS_ROOT).gameObject.SetActive(true);
		m_oObjDict.GetValueOrDefault(EKey.OBJS_ROOT).gameObject.SetActive(true);
	}

	/** 캔버스를 설정한다 */
	protected virtual void SetupCanvas(Canvas a_oCanvas, bool a_bIsEnableAssert = true) {
		this.SetupCanvas(a_oCanvas, new List<string>() {
			KCDefine.U_OBJ_N_SCENE_UIS,
			KCDefine.U_OBJ_N_SCENE_TEST_UIS,
			KCDefine.U_OBJ_N_SCENE_PIVOT_UIS,
			KCDefine.U_OBJ_N_SCENE_ANCHOR_UIS,
			KCDefine.U_OBJ_N_SCENE_CORNER_ANCHOR_UIS,

			KCDefine.U_OBJ_N_SCENE_UP_UIS,
			KCDefine.U_OBJ_N_SCENE_DOWN_UIS,
			KCDefine.U_OBJ_N_SCENE_LEFT_UIS,
			KCDefine.U_OBJ_N_SCENE_RIGHT_UIS,

			KCDefine.U_OBJ_N_SCENE_UP_LEFT_UIS,
			KCDefine.U_OBJ_N_SCENE_UP_RIGHT_UIS,
			KCDefine.U_OBJ_N_SCENE_DOWN_LEFT_UIS,
			KCDefine.U_OBJ_N_SCENE_DOWN_RIGHT_UIS,

			KCDefine.U_OBJ_N_SCENE_POPUP_UIS,
			KCDefine.U_OBJ_N_SCENE_TOPMOST_UIS,

			KCDefine.U_OBJ_N_SCREEN_BLIND_UIS,
			KCDefine.U_OBJ_N_SCREEN_POPUP_UIS,
			KCDefine.U_OBJ_N_SCREEN_TOPMOST_UIS,
			KCDefine.U_OBJ_N_SCREEN_ABS_UIS,

#if DEBUG || DEVELOPMENT_BUILD
			KCDefine.U_OBJ_N_SCREEN_DEBUG_UIS
#endif // #if DEBUG || DEVELOPMENT_BUILD
		}, a_bIsEnableAssert);
	}

	/** 캔버스를 설정한다 */
	protected virtual void SetupCanvas(Canvas a_oCanvas, List<string> a_oUIsNameList, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oCanvas != null);

		// 캔버스가 존재 할 경우
		if(a_oCanvas != null) {
			for(int i = 0; i < a_oUIsNameList.Count; ++i) {
				var oUIsObjs = a_oCanvas.gameObject.ExFindChild(a_oUIsNameList[i]);

				// UI 가 존재 할 경우
				if(oUIsObjs != null) {
					this.SetupDefUIs(oUIsObjs, false);

					// 최상단 블라인드 UI 일 경우
					if(oUIsObjs.name.Equals(KCDefine.U_OBJ_N_SCREEN_BLIND_UIS)) {
						this.SetupBlindUIs(oUIsObjs, false);
					}

#if DEBUG || DEVELOPMENT_BUILD
					// 최상단 디버그 UI 일 경우
					if(oUIsObjs.name.Equals(KCDefine.U_OBJ_N_SCREEN_DEBUG_UIS)) {
						this.SetupDebugUIs(oUIsObjs, false);
					}
#endif // #if DEBUG || DEVELOPMENT_BUILD
				}
			}

			this.DoSetupCanvas(a_oCanvas, a_bIsEnableAssert);
		}
	}

	/** 카메라를 설정한다 */
	protected virtual void SetupCamera() {
		m_oCanvasDict.GetValueOrDefault(EKey.UIS_CANVAS).renderMode = RenderMode.ScreenSpaceOverlay;
		m_oCanvasDict.GetValueOrDefault(EKey.UIS_CANVAS).targetDisplay = KCDefine.B_VAL_0_INT;

		// 캔버스, 메인 카메라를 설정한다
		this.SetupCanvas(m_oCanvasDict.GetValueOrDefault(EKey.UIS_CANVAS), false);
		this.SetupMainCamera(m_oCameraDict.GetValueOrDefault(EKey.MAIN_CAMERA), false);
	}

	/** 메인 카메라를 설정한다 */
	protected virtual void SetupMainCamera(Camera a_oCamera, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oCamera != null);

		// 카메라가 존재 할 경우
		if(a_oCamera != null) {
			a_oCamera.depth = this.MainCameraDepth;
			a_oCamera.orthographic = this.MainCameraProjection == EProjection._2D;
			a_oCamera.transform.localEulerAngles = (this.MainCameraProjection == EProjection._2D) ? Vector3.zero : a_oCamera.transform.localEulerAngles;
			a_oCamera.transform.localPosition = this.IsResetMainCameraPos ? new Vector3(KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_0_REAL, -this.PlaneDistance) : a_oCamera.transform.localPosition;

			this.SetupAdditionalCamera(a_oCamera, a_bIsEnableAssert);
		}
	}

	/** 추가 카메라를 설정한다 */
	protected virtual void SetupAdditionalCamera(Camera a_oCamera, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oCamera != null);

		// 카메라가 존재 할 경우
		if(a_oCamera != null) {
			a_oCamera.transform.localScale = Vector3.one;

			// 2 차원 투영 일 경우
			if(a_oCamera.orthographic) {
				a_oCamera.ExSetup2D(this.ScreenHeight * KCDefine.B_UNIT_SCALE);
			} else {
				a_oCamera.ExSetup3D(this.ScreenHeight * KCDefine.B_UNIT_SCALE, this.PlaneDistance);
			}
		}
	}

	/** 기본 UI 를 설정한다 */
	private void SetupDefUIs(GameObject a_oUIsObjs, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oUIsObjs != null);

		// 기본 UI 가 존재 할 경우
		if(a_oUIsObjs != null) {
			var stPos = Vector3.zero;
			var stSize = Vector3.zero;
			var stPivot = KCDefine.B_ANCHOR_MID_CENTER;
			var stAnchorMin = KCDefine.B_ANCHOR_MID_CENTER;
			var stAnchorMax = KCDefine.B_ANCHOR_MID_CENTER;

			bool bIsUpUIs = a_oUIsObjs.name.Equals(KCDefine.U_OBJ_N_SCENE_UP_UIS);
			bool bIsDownUIs = a_oUIsObjs.name.Equals(KCDefine.U_OBJ_N_SCENE_DOWN_UIS);
			bool bIsLeftUIs = a_oUIsObjs.name.Equals(KCDefine.U_OBJ_N_SCENE_LEFT_UIS);
			bool bIsRightUIs = a_oUIsObjs.name.Equals(KCDefine.U_OBJ_N_SCENE_RIGHT_UIS);

			bool bIsUpLeftUIs = a_oUIsObjs.name.Equals(KCDefine.U_OBJ_N_SCENE_UP_LEFT_UIS);
			bool bIsUpRightUIs = a_oUIsObjs.name.Equals(KCDefine.U_OBJ_N_SCENE_UP_RIGHT_UIS);
			bool bIsDownLeftUIs = a_oUIsObjs.name.Equals(KCDefine.U_OBJ_N_SCENE_DOWN_LEFT_UIS);
			bool bIsDownRightUIs = a_oUIsObjs.name.Equals(KCDefine.U_OBJ_N_SCENE_DOWN_RIGHT_UIS);

			// 고정 UI 일 경우
			if(bIsUpUIs || bIsDownUIs || bIsLeftUIs || bIsRightUIs) {
				this.SetupAnchorUIs(a_oUIsObjs, ref stPos, ref stSize, ref stPivot, false);
			}
			// 코너 고정 UI 일 경우
			else if(bIsUpLeftUIs || bIsUpRightUIs || bIsDownLeftUIs || bIsDownRightUIs) {
				this.SetupCornerAnchorUIs(a_oUIsObjs, ref stPos, ref stSize, ref stPivot, ref stAnchorMin, ref stAnchorMax, false);
			} else {
				this.SetupNonAnchorUIs(a_oUIsObjs, ref stPos, ref stSize, ref stAnchorMin, ref stAnchorMax, false);
			}

			(a_oUIsObjs.transform as RectTransform).pivot = stPivot;
			(a_oUIsObjs.transform as RectTransform).anchorMin = stAnchorMin;
			(a_oUIsObjs.transform as RectTransform).anchorMax = stAnchorMax;
			(a_oUIsObjs.transform as RectTransform).sizeDelta = stSize;
			(a_oUIsObjs.transform as RectTransform).anchoredPosition = stPos;

			(a_oUIsObjs.transform as RectTransform).localScale = Vector3.one;
			(a_oUIsObjs.transform as RectTransform).localEulerAngles = Vector3.zero;
		}
	}

	/** 블라인드 UI 를 설정한다 */
	private void SetupBlindUIs(GameObject a_oUIsObjs, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oUIsObjs != null);

		// 블라인드 UI 가 존재 할 경우
		if(a_oUIsObjs != null) {
			var oImgNameList = new List<string>() {
				KCDefine.U_OBJ_N_UP_BLIND_IMG, KCDefine.U_OBJ_N_DOWN_BLIND_IMG, KCDefine.U_OBJ_N_LEFT_BLIND_IMG, KCDefine.U_OBJ_N_RIGHT_BLIND_IMG
			};

			var oPivotList = new List<Vector3>() {
				KCDefine.B_ANCHOR_DOWN_CENTER, KCDefine.B_ANCHOR_UP_CENTER, KCDefine.B_ANCHOR_MID_RIGHT, KCDefine.B_ANCHOR_MID_LEFT
			};

			var oAnchorList = new List<Vector3>() {
				KCDefine.B_ANCHOR_UP_CENTER, KCDefine.B_ANCHOR_DOWN_CENTER, KCDefine.B_ANCHOR_MID_LEFT, KCDefine.B_ANCHOR_MID_RIGHT
			};

			var oPosList = new List<Vector3>() {
				Vector3.zero, Vector3.zero, new Vector3(KCDefine.B_VAL_0_REAL, CSceneManager.DownSafeAreaOffset, KCDefine.B_VAL_0_REAL), new Vector3(KCDefine.B_VAL_0_REAL, CSceneManager.DownSafeAreaOffset, KCDefine.B_VAL_0_REAL)
			};

			var oOffsetList = new List<Vector3>() {
				this.IsIgnoreBlindV ? Vector3.zero : new Vector3(KCDefine.B_VAL_0_REAL, (CSceneManager.CanvasSize.y - this.ScreenHeight) / -KCDefine.B_VAL_2_REAL, KCDefine.B_VAL_0_REAL),
				this.IsIgnoreBlindV ? Vector3.zero : new Vector3(KCDefine.B_VAL_0_REAL, (CSceneManager.CanvasSize.y - this.ScreenHeight) / KCDefine.B_VAL_2_REAL, KCDefine.B_VAL_0_REAL),
				this.IsIgnoreBlindH ? Vector3.zero : new Vector3((CSceneManager.CanvasSize.x - this.ScreenWidth) / KCDefine.B_VAL_2_REAL, KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_0_REAL),
				this.IsIgnoreBlindH ? Vector3.zero : new Vector3((CSceneManager.CanvasSize.x - this.ScreenWidth) / -KCDefine.B_VAL_2_REAL, KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_0_REAL)
			};

			float fDownSafeAreaOffset = Mathf.Clamp(oOffsetList[KCDefine.B_VAL_1_INT].y - CSceneManager.DownSafeAreaOffset, KCDefine.B_VAL_0_REAL, float.MaxValue);
			oOffsetList[KCDefine.B_VAL_1_INT] = new Vector3(oOffsetList[KCDefine.B_VAL_1_INT].x, fDownSafeAreaOffset, oOffsetList[KCDefine.B_VAL_1_INT].z);

			for(int i = 0; i < oImgNameList.Count; ++i) {
				var oImg = a_oUIsObjs.ExFindComponent<Image>(oImgNameList[i]);
				oImg.rectTransform.pivot = oPivotList[i];
				oImg.rectTransform.anchorMin = oAnchorList[i];
				oImg.rectTransform.anchorMax = oAnchorList[i];
				oImg.rectTransform.sizeDelta = CSceneManager.CanvasSize;
				oImg.rectTransform.anchoredPosition = oPosList[i] + oOffsetList[i];

#if UNITY_EDITOR
				oImg.color = KCDefine.U_COLOR_TRANSPARENT;
#else
				oImg.color = KCDefine.U_COLOR_BLIND_UIS;
#endif // #if UNITY_EDITOR
			}
		}
	}

	/** 고정 UI 를 설정한다 */
	private void SetupAnchorUIs(GameObject a_oUIsObjs, ref Vector3 a_rstPos, ref Vector3 a_rstSize, ref Vector3 a_rstPivot, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oUIsObjs != null);

		// 고정 UI 가 존재 할 경우
		if(a_oUIsObjs != null) {
			bool bIsUpUIs = a_oUIsObjs.name.Equals(KCDefine.U_OBJ_N_SCENE_UP_UIS);
			bool bIsDownUIs = a_oUIsObjs.name.Equals(KCDefine.U_OBJ_N_SCENE_DOWN_UIS);
			bool bIsLeftUIs = a_oUIsObjs.name.Equals(KCDefine.U_OBJ_N_SCENE_LEFT_UIS);
			bool bIsRightUIs = a_oUIsObjs.name.Equals(KCDefine.U_OBJ_N_SCENE_RIGHT_UIS);

			// 위쪽, 아래쪽 UI 일 경우
			if(bIsUpUIs || bIsDownUIs) {
				a_rstSize = new Vector3(this.ScreenWidth, KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_0_REAL);
				a_rstPivot = bIsUpUIs ? KCDefine.B_ANCHOR_UP_CENTER : KCDefine.B_ANCHOR_DOWN_CENTER;

				// 위쪽 UI 일 경우
				if(bIsUpUIs) {
					a_rstPos = new Vector3(KCDefine.B_VAL_0_REAL, (CSceneManager.CanvasSize.y / KCDefine.B_VAL_2_REAL) + CSceneManager.UpSafeAreaOffset, KCDefine.B_VAL_0_REAL);
				} else {
					a_rstPos = new Vector3(KCDefine.B_VAL_0_REAL, (CSceneManager.CanvasSize.y / -KCDefine.B_VAL_2_REAL) + CSceneManager.DownSafeAreaOffset, KCDefine.B_VAL_0_REAL);
				}
			}
			// 왼쪽, 오른쪽 UI 일 경우
			else if(bIsLeftUIs || bIsRightUIs) {
				a_rstSize = new Vector3(KCDefine.B_VAL_0_REAL, this.ScreenHeight, KCDefine.B_VAL_0_REAL);
				a_rstPivot = bIsLeftUIs ? KCDefine.B_ANCHOR_MID_LEFT : KCDefine.B_ANCHOR_MID_RIGHT;

				// 왼쪽 UI 일 경우
				if(bIsLeftUIs) {
					a_rstPos = new Vector3((CSceneManager.CanvasSize.x / -KCDefine.B_VAL_2_REAL) + CSceneManager.LeftSafeAreaOffset, KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_0_REAL);
				} else {
					a_rstPos = new Vector3((CSceneManager.CanvasSize.x / KCDefine.B_VAL_2_REAL) + CSceneManager.RightSafeAreaOffset, KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_0_REAL);
				}
			}
		}
	}

	/** 코너 고정 UI 를 설정한다 */
	private void SetupCornerAnchorUIs(GameObject a_oUIsObjs, ref Vector3 a_rstPos, ref Vector3 a_rstSize, ref Vector3 a_rstPivot, ref Vector3 a_rstAnchorMin, ref Vector3 a_rstAnchorMax, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oUIsObjs != null);

		// 코너 UI 가 존재 할 경우
		if(a_oUIsObjs != null) {
			bool bIsUpLeftUIs = a_oUIsObjs.name.Equals(KCDefine.U_OBJ_N_SCENE_UP_LEFT_UIS);
			bool bIsUpRightUIs = a_oUIsObjs.name.Equals(KCDefine.U_OBJ_N_SCENE_UP_RIGHT_UIS);
			bool bIsDownLeftUIs = a_oUIsObjs.name.Equals(KCDefine.U_OBJ_N_SCENE_DOWN_LEFT_UIS);
			bool bIsDownRightUIs = a_oUIsObjs.name.Equals(KCDefine.U_OBJ_N_SCENE_DOWN_RIGHT_UIS);

			a_rstSize = new Vector3(CSceneManager.CanvasSize.x, KCDefine.B_VAL_0_REAL, KCDefine.B_VAL_0_REAL);

			// 좌상단, 우상단 UI 일 경우
			if(bIsUpLeftUIs || bIsUpRightUIs) {
				a_rstPivot = bIsUpLeftUIs ? KCDefine.B_ANCHOR_UP_LEFT : KCDefine.B_ANCHOR_UP_RIGHT;
				a_rstAnchorMin = bIsUpLeftUIs ? KCDefine.B_ANCHOR_UP_LEFT : KCDefine.B_ANCHOR_UP_RIGHT;
				a_rstAnchorMax = bIsUpLeftUIs ? KCDefine.B_ANCHOR_UP_LEFT : KCDefine.B_ANCHOR_UP_RIGHT;

				// 좌상단 UI 일 경우
				if(bIsUpLeftUIs) {
					a_rstPos = new Vector3(CSceneManager.LeftSafeAreaOffset, CSceneManager.UpSafeAreaOffset, KCDefine.B_VAL_0_REAL);
				} else {
					a_rstPos = new Vector3(CSceneManager.RightSafeAreaOffset, CSceneManager.UpSafeAreaOffset, KCDefine.B_VAL_0_REAL);
				}
			}
			// 좌하단, 우하단 UI 일 경우
			else if(bIsDownLeftUIs || bIsDownRightUIs) {
				a_rstPivot = bIsDownLeftUIs ? KCDefine.B_ANCHOR_DOWN_LEFT : KCDefine.B_ANCHOR_DOWN_RIGHT;
				a_rstAnchorMin = bIsDownLeftUIs ? KCDefine.B_ANCHOR_DOWN_LEFT : KCDefine.B_ANCHOR_DOWN_RIGHT;
				a_rstAnchorMax = bIsDownLeftUIs ? KCDefine.B_ANCHOR_DOWN_LEFT : KCDefine.B_ANCHOR_DOWN_RIGHT;

				// 좌하단 UI 일 경우
				if(bIsDownLeftUIs) {
					a_rstPos = new Vector3(CSceneManager.LeftSafeAreaOffset, CSceneManager.DownSafeAreaOffset, KCDefine.B_VAL_0_REAL);
				} else {
					a_rstPos = new Vector3(CSceneManager.RightSafeAreaOffset, CSceneManager.DownSafeAreaOffset, KCDefine.B_VAL_0_REAL);
				}
			}
		}
	}

	/** 유동 UI 를 설정한다 */
	private void SetupNonAnchorUIs(GameObject a_oUIsObjs, ref Vector3 a_rstPos, ref Vector3 a_rstSize, ref Vector3 a_rstAnchorMin, ref Vector3 a_rstAnchorMax, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oUIsObjs != null);

		// 유동 UI 가 존재 할 경우
		if(a_oUIsObjs != null) {
			bool bIsUIs = a_oUIsObjs.name.Equals(KCDefine.U_OBJ_N_SCENE_UIS);
			bool bIsTestUIs = a_oUIsObjs.name.Equals(KCDefine.U_OBJ_N_SCENE_TEST_UIS);
			bool bIsBlindUIs = a_oUIsObjs.name.Equals(KCDefine.U_OBJ_N_SCREEN_BLIND_UIS);
			bool bIsAnchorUIs = a_oUIsObjs.name.Equals(KCDefine.U_OBJ_N_SCENE_ANCHOR_UIS);
			bool bIsCornerAnchorUIs = a_oUIsObjs.name.Equals(KCDefine.U_OBJ_N_SCENE_CORNER_ANCHOR_UIS);

#if DEBUG || DEVELOPMENT_BUILD
			bool bIsDebugUIs = a_oUIsObjs.name.Equals(KCDefine.U_OBJ_N_SCREEN_DEBUG_UIS);
#else
			bool bIsDebugUIs = false;
#endif // #if DEBUG || DEVELOPMENT_BUILD

			// 기본 UI 일 경우
			if(bIsUIs || bIsTestUIs) {
				a_rstSize = new Vector3(this.ScreenWidth, this.ScreenHeight, KCDefine.B_VAL_0_REAL);
			}
			// 크기 보정 UI 일 경우
			else if(bIsBlindUIs || bIsAnchorUIs || bIsCornerAnchorUIs || bIsDebugUIs) {
				a_rstAnchorMin = KCDefine.B_ANCHOR_DOWN_LEFT;
				a_rstAnchorMax = KCDefine.B_ANCHOR_UP_RIGHT;
			}
			// 화면 UI 가 아닐 경우
			else if(!a_oUIsObjs.name.Equals(KCDefine.U_OBJ_N_SCREEN_POPUP_UIS) && !a_oUIsObjs.name.Equals(KCDefine.U_OBJ_N_SCREEN_TOPMOST_UIS) && !a_oUIsObjs.name.Equals(KCDefine.U_OBJ_N_SCREEN_ABS_UIS)) {
				a_rstPos = Vector3.zero.ExPivotPosToPos();
			}
		}
	}

	/** 고유 컴포넌트를 설정한다 */
	private void SetupUniqueComponents(List<Behaviour> a_oComponentList, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oComponentList != null);

		// 고유 컴포넌트가 존재 할 경우
		if(a_oComponentList != null) {
			for(int i = 0; i < a_oComponentList.Count; ++i) {
				a_oComponentList[i].enabled = this.IsActiveScene;
			}
		}
	}

	/** 캔버스를 설정한다 */
	private void DoSetupCanvas(Canvas a_oCanvas, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oCanvas != null);

		// 캔버스가 존재 할 경우
		if(a_oCanvas != null) {
			a_oCanvas.pixelPerfect = false;
			a_oCanvas.planeDistance = this.PlaneDistance;
			a_oCanvas.worldCamera = CSceneManager.ActiveSceneMainCamera;
			a_oCanvas.targetDisplay = KCDefine.B_VAL_0_INT;
			a_oCanvas.additionalShaderChannels = (AdditionalCanvasShaderChannels)int.MaxValue;

			a_oCanvas.ExSetSortingOrder(new STSortingOrderInfo() {
				m_nOrder = a_oCanvas.sortingOrder, m_oLayer = (a_oCanvas.renderMode == RenderMode.ScreenSpaceOverlay) ? KCDefine.U_SORTING_L_DEF : a_oCanvas.sortingLayerName
			});

			// 캔버스 비율 처리자가 존재 할 경우
			if(a_oCanvas.TryGetComponent<CanvasScaler>(out CanvasScaler oCanvasScaler)) {
				oCanvasScaler.uiScaleMode = a_oCanvas.name.Equals(KCDefine.U_OBJ_N_SCENE_UIS_BASE) ? CanvasScaler.ScaleMode.ScaleWithScreenSize : oCanvasScaler.uiScaleMode;
				oCanvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
				oCanvasScaler.referenceResolution = this.ScreenSize;
				oCanvasScaler.referencePixelsPerUnit = KCDefine.B_UNIT_REF_PIXELS_PER_UNIT;
			}
		}
	}
	#endregion // 함수

	#region 클래스 함수
	/** 퀄리티를 설정한다 */
	public static void SetupQuality(EQualityLevel a_eQualityLevel, bool a_bIsEnableExpensiveChange = false) {
		CSceneManager.QualityLevel = a_eQualityLevel;

#if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE
		CSceneManager.DoSetupQuality(a_eQualityLevel, true, true);
#else
		CSceneManager.DoSetupQuality(a_eQualityLevel, false, true);
#endif // #if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE
	}

	/** 퀄리티를 설정한다 */
	private static void DoSetupQuality(EQualityLevel a_eQualityLevel, bool a_bIsEnableSetupRenderingPipeline, bool a_bIsEnableExpensiveChange = false) {
#if UNITY_EDITOR
		var oQualityLevelList = new List<EQualityLevel>() {
			EQualityLevel.NORM, EQualityLevel.HIGH, EQualityLevel.ULTRA
		};

		var oIsSetupOptsList = new List<bool>() {
			GraphicsSettings.logWhenShaderIsCompiled,
			GraphicsSettings.videoShadersIncludeMode == VideoShadersIncludeMode.Always
		};

		// 설정 갱신이 필요 할 경우
		if(oIsSetupOptsList.Contains(false)) {
			GraphicsSettings.logWhenShaderIsCompiled = true;
			GraphicsSettings.videoShadersIncludeMode = VideoShadersIncludeMode.Always;
		}

		for(int i = 0; i < oQualityLevelList.Count; ++i) {
			var stRenderingOptsInfo = CPlatformOptsSetter.OptsInfoTable.GetRenderingOptsInfo(oQualityLevelList[i]);
			QualitySettings.SetQualityLevel((int)oQualityLevelList[i], false);

			var oIsSetupQualityOptsList = new List<bool>() {
				QualitySettings.asyncUploadPersistentBuffer,
				QualitySettings.billboardsFaceCameraPosition,

				QualitySettings.softParticles == false,
				QualitySettings.streamingMipmapsActive == false,
				QualitySettings.realtimeReflectionProbes == CPlatformOptsSetter.OptsInfoTable.QualityOptsInfo.m_bIsEnableRealtimeReflectionProbes,

				QualitySettings.maximumLODLevel == KCDefine.B_VAL_7_INT,
				QualitySettings.pixelLightCount == KCDefine.B_VAL_8_INT,
				QualitySettings.asyncUploadTimeSlice == KCDefine.U_QUALITY_ASYNC_UPLOAD_TIME_SLICE,
				QualitySettings.asyncUploadBufferSize == KCDefine.U_QUALITY_ASYNC_UPLOAD_BUFFER_SIZE,

				QualitySettings.lodBias.ExIsEquals(KCDefine.B_VAL_1_REAL),
				QualitySettings.resolutionScalingFixedDPIFactor.ExIsEquals(KCDefine.B_VAL_1_REAL),

				QualitySettings.shadows == UnityEngine.ShadowQuality.All,
				QualitySettings.vSyncCount == (int)EVSync.NEVER,
				QualitySettings.skinWeights == SkinWeights.FourBones,

				QualitySettings.shadowProjection == ShadowProjection.StableFit,
				QualitySettings.anisotropicFiltering == AnisotropicFiltering.Disable,
				QualitySettings.particleRaycastBudget == ((oQualityLevelList[i] == EQualityLevel.NORM) ? (int)EPOT._64 : (int)EPOT._128),

				QualitySettings.shadowDistance.ExIsEquals(KCDefine.U_DISTANCE_CAMERA_FAR_PLANE / KCDefine.B_VAL_2_REAL),
				QualitySettings.shadowCascade2Split.ExIsEquals(KCEditorDefine.B_EDITOR_OPTS_CASCADE_2_SPLIT_PERCENT),
				QualitySettings.shadowCascade4Split.ExIsEquals(KCEditorDefine.B_EDITOR_OPTS_CASCADE_4_SPLIT_PERCENT),
				QualitySettings.shadowNearPlaneOffset.ExIsEquals(KCDefine.U_DISTANCE_CAMERA_NEAR_PLANE),

				QualitySettings.shadowmaskMode == stRenderingOptsInfo.m_stLightOptsInfo.m_eShadowmaskMode,
				QualitySettings.shadowResolution == stRenderingOptsInfo.m_stLightOptsInfo.m_eShadowResolution,
				QualitySettings.shadowCascades == (int)stRenderingOptsInfo.m_stLightOptsInfo.m_eShadowCascadesOpts,
				QualitySettings.renderPipeline == a_bIsEnableSetupRenderingPipeline ? Resources.Load<RenderPipelineAsset>(CAccess.GetRenderingPipelinePath(oQualityLevelList[i])) : null,

#if ANTI_ALIASING_ENABLE
				QualitySettings.antiAliasing == (int)stRenderingOptsInfo.m_eAAQuality
#else
				QualitySettings.antiAliasing == (int)EAAQuality.DISABLE
#endif // #if ANTI_ALIASING_ENABLE
			};

			// 설정 갱신이 필요 할 경우
			if(oIsSetupQualityOptsList.Contains(false)) {
				QualitySettings.asyncUploadPersistentBuffer = true;
				QualitySettings.billboardsFaceCameraPosition = true;

				QualitySettings.softParticles = false;
				QualitySettings.streamingMipmapsActive = false;
				QualitySettings.realtimeReflectionProbes = CPlatformOptsSetter.OptsInfoTable.QualityOptsInfo.m_bIsEnableRealtimeReflectionProbes;

				QualitySettings.maximumLODLevel = KCDefine.B_VAL_7_INT;
				QualitySettings.pixelLightCount = KCDefine.B_VAL_8_INT;
				QualitySettings.asyncUploadTimeSlice = KCDefine.U_QUALITY_ASYNC_UPLOAD_TIME_SLICE;
				QualitySettings.asyncUploadBufferSize = KCDefine.U_QUALITY_ASYNC_UPLOAD_BUFFER_SIZE;

				QualitySettings.lodBias = KCDefine.B_VAL_1_REAL;
				QualitySettings.resolutionScalingFixedDPIFactor = KCDefine.B_VAL_1_REAL;

				QualitySettings.shadows = UnityEngine.ShadowQuality.All;
				QualitySettings.vSyncCount = (int)EVSync.NEVER;
				QualitySettings.skinWeights = SkinWeights.FourBones;

				QualitySettings.shadowProjection = ShadowProjection.StableFit;
				QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
				QualitySettings.particleRaycastBudget = (oQualityLevelList[i] == EQualityLevel.NORM) ? (int)EPOT._64 : (int)EPOT._128;

				QualitySettings.shadowDistance = KCDefine.U_DISTANCE_CAMERA_FAR_PLANE / KCDefine.B_VAL_2_REAL;
				QualitySettings.shadowCascade2Split = KCEditorDefine.B_EDITOR_OPTS_CASCADE_2_SPLIT_PERCENT;
				QualitySettings.shadowCascade4Split = KCEditorDefine.B_EDITOR_OPTS_CASCADE_4_SPLIT_PERCENT;
				QualitySettings.shadowNearPlaneOffset = KCDefine.U_DISTANCE_CAMERA_NEAR_PLANE;

				QualitySettings.shadowmaskMode = stRenderingOptsInfo.m_stLightOptsInfo.m_eShadowmaskMode;
				QualitySettings.shadowResolution = stRenderingOptsInfo.m_stLightOptsInfo.m_eShadowResolution;
				QualitySettings.shadowCascades = (int)stRenderingOptsInfo.m_stLightOptsInfo.m_eShadowCascadesOpts;
				QualitySettings.renderPipeline = a_bIsEnableSetupRenderingPipeline ? Resources.Load<RenderPipelineAsset>(CAccess.GetRenderingPipelinePath(oQualityLevelList[i])) : null;

#if ANTI_ALIASING_ENABLE
				QualitySettings.antiAliasing = (int)stRenderingOptsInfo.m_eAAQuality;
#else
				QualitySettings.antiAliasing = (int)EAAQuality.DISABLE;
#endif // #if ANTI_ALIASING_ENABLE
			}
		}
#endif // #if UNITY_EDITOR

		GraphicsSettings.renderPipelineAsset = a_bIsEnableSetupRenderingPipeline ? Resources.Load<RenderPipelineAsset>(CAccess.GetRenderingPipelinePath(a_eQualityLevel)) : null;
		GraphicsSettings.defaultRenderPipeline = a_bIsEnableSetupRenderingPipeline ? Resources.Load<RenderPipelineAsset>(CAccess.GetRenderingPipelinePath(a_eQualityLevel)) : null;

		QualitySettings.SetQualityLevel((int)a_eQualityLevel, a_bIsEnableExpensiveChange);
	}
	#endregion // 클래스 함수

	#region 조건부 함수
#if UNITY_EDITOR
	/** 에디터 씬을 설정한다 */
	public void EditorSetupScene() {
		// 상태 갱신이 가능 할 경우
		if(CEditorAccess.IsEnableUpdateState && CSceneManager.ActiveScene.ExFindComponent<CSceneManager>(KCDefine.U_OBJ_N_SCENE_MANAGER) != null) {
			try {
				this.SetupScene();
				CPlatformOptsSetter.SetupQuality();

				CFunc.EnumerateComponents<EventSystem>((a_oEventSystem) => {
					a_oEventSystem.enabled = false;
					a_oEventSystem.gameObject.SetActive(this.IsActiveScene);

					// 입력 모듈이 존재 할 경우
					if(a_oEventSystem.TryGetComponent<BaseInputModule>(out BaseInputModule oInputModule)) {
						oInputModule.enabled = false;
					}

					return true;
				}, true);
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CSceneManager.EditorSetupScene Exception: {oException.Message}");
			}
		}
	}

	/** 스크립트 순서를 설정한다 */
	protected sealed override void SetupScriptOrder() {
		base.SetupScriptOrder();
		this.ExSetScriptOrder(this.ScriptOrder);
	}
#endif // #if UNITY_EDITOR

#if DEBUG || DEVELOPMENT_BUILD || PLAY_TEST_ENABLE
	/** 테스트 UI 를 설정한다 */
	private void SetupTestUIs() {
		// 버튼을 설정한다
		CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
			(EKey.TEST_UIS_OPEN_BTN, $"{EKey.TEST_UIS_OPEN_BTN}", m_oUIsDict.GetValueOrDefault(EKey.TEST_CONTENTS_UIS), this.OnTouchTestUIsOpenBtn),
			(EKey.TEST_UIS_CLOSE_BTN, $"{EKey.TEST_UIS_CLOSE_BTN}", m_oUIsDict.GetValueOrDefault(EKey.TEST_CONTENTS_UIS), this.OnTouchTestUIsCloseBtn)
		}, m_oBtnDict);
	}
#endif // #if DEBUG || DEVELOPMENT_BUILD || PLAY_TEST_ENABLE

#if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE
	/** 카메라 데이터를 설정한다 */
	private void SetupCameraData(UniversalAdditionalCameraData a_oCameraData, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oCameraData != null);

		// 카메라 데이터가 존재 할 경우
		if(a_oCameraData != null) {
			a_oCameraData.antialiasingQuality = AntialiasingQuality.High;

#if ANTI_ALIASING_ENABLE
			a_oCameraData.antialiasing = AntialiasingMode.FastApproximateAntialiasing;
#else
			a_oCameraData.antialiasing = AntialiasingMode.None;
#endif // #if ANTI_ALIASING_ENABLE
		}
	}
#endif // #if UNIVERSAL_RENDERING_PIPELINE_MODULE_ENABLE
	#endregion // 조건부 함수
}

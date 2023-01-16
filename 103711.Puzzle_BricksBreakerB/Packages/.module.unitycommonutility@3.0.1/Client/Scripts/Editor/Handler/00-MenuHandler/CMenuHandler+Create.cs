using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
using TMPro;

#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif // #if PURCHASE_MODULE_ENABLE

/** 메뉴 처리자 - 디렉토리 생성 */
public static partial class CMenuHandler {
	#region 클래스 함수
	/** 추가 프로젝트 디렉토리를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_CREATE_BASE + "Extra Project Directories", false, KCEditorDefine.B_SORTING_O_CREATE_MENU + 1)]
	public static void CreateExtraProjDir() {
		// 프로젝트 정보 테이블이 존재 할 경우
		if(CPlatformOptsSetter.ProjInfoTable != null) {
			string oDirPath = string.Format(KCDefine.B_TEXT_FMT_2_COMBINE, KCEditorDefine.B_DIR_P_ASSETS, CPlatformOptsSetter.ProjInfoTable.CommonProjInfo.m_oExtraProjDirName);

			CEditorFactory.MakeDirectories(string.Format(KCDefine.B_PATH_FMT_2_COMBINE, oDirPath, KCEditorDefine.B_DIR_N_SCENES));
			CEditorFactory.MakeDirectories(string.Format(KCDefine.B_PATH_FMT_2_COMBINE, oDirPath, KCEditorDefine.B_DIR_N_RESOURCES));
			CEditorFactory.MakeDirectories(string.Format(KCDefine.B_PATH_FMT_2_COMBINE, oDirPath, KCEditorDefine.B_DIR_P_EDITOR_SCRIPTS));
			CEditorFactory.MakeDirectories(string.Format(KCDefine.B_PATH_FMT_2_COMBINE, oDirPath, KCEditorDefine.B_DIR_P_ENGINE_SCRIPTS));
			CEditorFactory.MakeDirectories(string.Format(KCDefine.B_PATH_FMT_2_COMBINE, oDirPath, KCEditorDefine.B_DIR_P_RUNTIME_SCRIPTS));
		}
	}
	#endregion // 클래스 함수
}

/** 메뉴 처리자 - 객체 생성 */
public static partial class CMenuHandler {
	#region 클래스 함수
	/** 게임 객체를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_BASE + "Game Object %#[", false, 101)]
	public static void CreateGameObj() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_GAME_OBJ, string.Empty, true);
	}

	/** 게임 객체를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_BASE + "Child Game Object %#]", false, 101)]
	public static void CreateChildGameObj() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_GAME_OBJ, string.Empty);
	}

	/** 팝업을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Popup", false, 101)]
	public static void CreatePopup() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_POPUP, KCDefine.U_OBJ_P_G_POPUP);
	}

	/** 포커스 팝업을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Focus Popup", false, 101)]
	public static void CreateFocusPopup() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_FOCUS_POPUP, KCDefine.U_OBJ_P_G_FOCUS_POPUP);
	}

	/** 텍스트를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Text/Text", false, 101)]
	public static void CreateText() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_TEXT, KCDefine.U_OBJ_P_TEXT);
	}

	/** 텍스트를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Text/Localize Text", false, 101)]
	public static void CreateLocalizeText() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_LOCALIZE_TEXT, KCDefine.U_OBJ_P_LOCALIZE_TEXT);
	}

	/** 텍스트를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Text/TMP Text", false, 101)]
	public static void CreateTMPText() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_TMP_TEXT, KCDefine.U_OBJ_P_TMP_TEXT);
	}

	/** 텍스트를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Text/TMP Localize Text", false, 101)]
	public static void CreateTMPLocalizeText() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_TMP_LOCALIZE_TEXT, KCDefine.U_OBJ_P_TMP_LOCALIZE_TEXT);
	}

	/** 이미지를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Image/Image", false, 101)]
	public static void CreateImg() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_IMG, KCDefine.U_OBJ_P_IMG);
	}

	/** 이미지를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Image/Mask Image", false, 101)]
	public static void CreateMaskImg() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_MASK_IMG, KCDefine.U_OBJ_P_MASK_IMG);
	}

	/** 이미지를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Image/Focus Image", false, 101)]
	public static void CreateFocusImg() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_FOCUS_IMG, KCDefine.U_OBJ_P_FOCUS_IMG);
	}

	/** 이미지를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Image/Gauge Image", false, 101)]
	public static void CreateGaugeImg() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_GAUGE_IMG, KCDefine.U_OBJ_P_GAUGE_IMG);
	}

	/** 이미지 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/Image Button", false, 101)]
	public static void CreateImgBtn() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_IMG_BTN, KCDefine.U_OBJ_P_IMG_BTN);
	}

	/** 이미지 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/Image Scale Button", false, 101)]
	public static void CreateImgScaleBtn() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_IMG_SCALE_BTN, KCDefine.U_OBJ_P_IMG_SCALE_BTN);
	}

	/** 텍스트 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/Text/Text + Button/Text Button", false, 101)]
	public static void CreateTextBtn() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_TEXT_BTN, KCDefine.U_OBJ_P_TEXT_BTN);
	}

	/** 텍스트 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/Text/Text + Button/Text Scale Button", false, 101)]
	public static void CreateTextScaleBtn() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_TEXT_SCALE_BTN, KCDefine.U_OBJ_P_TEXT_SCALE_BTN);
	}

	/** 텍스트 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/Text/Localize Text + Button/Localize Text Button", false, 101)]
	public static void CreateLocalizeTextBtn() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_LOCALIZE_TEXT_BTN, KCDefine.U_OBJ_P_LOCALIZE_TEXT_BTN);
	}

	/** 텍스트 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/Text/Localize Text + Button/Localize Text Scale Button", false, 101)]
	public static void CreateLocalizeTextScaleBtn() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_LOCALIZE_TEXT_SCALE_BTN, KCDefine.U_OBJ_P_LOCALIZE_TEXT_SCALE_BTN);
	}

	/** 이미지 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/Text/Image + Text + Button/Image Text Button", false, 101)]
	public static void CreateImgTextBtn() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_IMG_TEXT_BTN, KCDefine.U_OBJ_P_IMG_TEXT_BTN);
	}

	/** 이미지 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/Text/Image + Text + Button/Image Text Scale Button", false, 101)]
	public static void CreateImgTextScaleBtn() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_IMG_TEXT_SCALE_BTN, KCDefine.U_OBJ_P_IMG_TEXT_SCALE_BTN);
	}

	/** 이미지 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/Text/Image + LocalizeText + Button/Image Localize Text Button", false, 101)]
	public static void CreateImgLocalizeTextBtn() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_IMG_LOCALIZE_TEXT_BTN, KCDefine.U_OBJ_P_IMG_LOCALIZE_TEXT_BTN);
	}

	/** 이미지 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/Text/Image + LocalizeText + Button/Image Localize Text Scale Button", false, 101)]
	public static void CreateImgLocalizeTextScaleBtn() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_IMG_LOCALIZE_TEXT_SCALE_BTN, KCDefine.U_OBJ_P_IMG_LOCALIZE_TEXT_SCALE_BTN);
	}

	/** 텍스트 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/Text Mesh Pro/Text + Button/Text Button", false, 101)]
	public static void CreateTMPTextBtn() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_TMP_TEXT_BTN, KCDefine.U_OBJ_P_TMP_TEXT_BTN);
	}

	/** 텍스트 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/Text Mesh Pro/Text + Button/Text Scale Button", false, 101)]
	public static void CreateTMPTextScaleBtn() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_TMP_TEXT_SCALE_BTN, KCDefine.U_OBJ_P_TMP_TEXT_SCALE_BTN);
	}

	/** 텍스트 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/Text Mesh Pro/LocalizeText + Button/Localize Text Button", false, 101)]
	public static void CreateTMPLocalizeTextBtn() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_TMP_LOCALIZE_TEXT_BTN, KCDefine.U_OBJ_P_TMP_LOCALIZE_TEXT_BTN);
	}

	/** 텍스트 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/Text Mesh Pro/LocalizeText + Button/Localize Text Scale Button", false, 101)]
	public static void CreateTMPLocalizeTextScaleBtn() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_TMP_LOCALIZE_TEXT_SCALE_BTN, KCDefine.U_OBJ_P_TMP_LOCALIZE_TEXT_SCALE_BTN);
	}

	/** 이미지 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/Text Mesh Pro/Image + Text + Button/Image Text Button", false, 101)]
	public static void CreateTMPImgTextBtn() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_TMP_IMG_TEXT_BTN, KCDefine.U_OBJ_P_TMP_IMG_TEXT_BTN);
	}

	/** 이미지 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/Text Mesh Pro/Image + Text + Button/Image Text Scale Button", false, 101)]
	public static void CreateTMPImgTextScaleBtn() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_TMP_IMG_TEXT_SCALE_BTN, KCDefine.U_OBJ_P_TMP_IMG_TEXT_SCALE_BTN);
	}

	/** 이미지 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/Text Mesh Pro/Image + LocalizeText + Button/Image Localize Text Button", false, 101)]
	public static void CreateTMPImgLocalizeTextBtn() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_TMP_IMG_LOCALIZE_TEXT_BTN, KCDefine.U_OBJ_P_TMP_IMG_LOCALIZE_TEXT_BTN);
	}

	/** 이미지 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/Text Mesh Pro/Image + LocalizeText + Button/Image Localize Text Scale Button", false, 101)]
	public static void CreateTMPImgLocalizeTextScaleBtn() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_TMP_IMG_LOCALIZE_TEXT_SCALE_BTN, KCDefine.U_OBJ_P_TMP_IMG_LOCALIZE_TEXT_SCALE_BTN);
	}

	/** 입력 필드를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Input/Dropdown", false, 101)]
	public static void CreateDropdown() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_DROPDOWN, KCDefine.U_OBJ_P_DROPDOWN);
	}

	/** 입력 필드를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Input/Input Field", false, 101)]
	public static void CreateInputField() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_INPUT_FIELD, KCDefine.U_OBJ_P_INPUT_FIELD);
	}

	/** 입력 필드를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Input/TMP Dropdown", false, 101)]
	public static void CreateTMPDropdown() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_TMP_DROPDOWN, KCDefine.U_OBJ_P_TMP_DROPDOWN);
	}

	/** 입력 필드를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Input/TMP Input Field", false, 101)]
	public static void CreateTMPInputField() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_TMP_INPUT_FIELD, KCDefine.U_OBJ_P_TMP_INPUT_FIELD);
	}

	/** 페이지 뷰를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "ScrollView/Page View", false, 101)]
	public static void CreatePageView() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_PAGE_VIEW, KCDefine.U_OBJ_P_PAGE_VIEW);
	}

	/** 스크롤 뷰를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "ScrollView/Scroll View", false, 101)]
	public static void CreateScrollView() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_SCROLL_VIEW, KCDefine.U_OBJ_P_SCROLL_VIEW);
	}

	/** 재사용 뷰를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "ScrollView/Recycle View", false, 101)]
	public static void CreateRecycleView() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_RECYCLE_VIEW, KCDefine.U_OBJ_P_RECYCLE_VIEW);
	}

	/** 드래그 응답자를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Responder/Drag Responder", false, 101)]
	public static void CreateDragResponder() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_DRAG_RESPONDER, KCDefine.U_OBJ_P_DRAG_RESPONDER);
	}

	/** 터치 응답자를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Responder/Touch Responder", false, 101)]
	public static void CreateTouchResponder() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_TOUCH_RESPONDER, KCDefine.U_OBJ_P_TOUCH_RESPONDER);
	}

	/** 객체를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_2D_BASE + "2D Object", false, 101)]
	public static void Create2DObj() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_OBJ, KCDefine.U_OBJ_P_OBJ);
	}

	/** 스프라이트를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_2D_BASE + "Sprite", false, 101)]
	public static void CreateSprite() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_SPRITE, KCDefine.U_OBJ_P_SPRITE);
	}

	/** 텍스트를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_3D_BASE + "TMP Text Mesh", false, 101)]
	public static void CreateTMPTextMesh() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_TMP_TEXT_MESH, KCDefine.U_OBJ_P_TMP_TEXT_MESH);
	}

	/** 라인 효과를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_FX_BASE + "Line FX", false, 101)]
	public static void CreateLineFX() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_LINE_FX, KCDefine.U_OBJ_P_LINE_FX);
	}

	/** 파티클 효과를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_FX_BASE + "Particle FX", false, 101)]
	public static void CreateParticleFX() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_PARTICLE_FX, KCDefine.U_OBJ_P_PARTICLE_FX);
	}

	/** 반사 프로브를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_FX_BASE + "Reflection Probe", false, 101)]
	public static void CreateReflectionProbe() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_REFLECTION_PROBE, KCDefine.U_OBJ_P_REFLECTION_PROBE);
	}

	/** 광원 프로브 그룹을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_FX_BASE + "Light Probe Group", false, 101)]
	public static void CreateLightProbeGroup() {
		CMenuHandler.CreateObj(KCDefine.U_OBJ_N_LIGHT_PROBE_GROUP, KCDefine.U_OBJ_P_LIGHT_PROBE_GROUP);
	}

	/** 객체를 생성한다 */
	public static GameObject CreateObj(string a_oName, string a_oFilePath, bool a_bIsIgnoreParent = false) {
		var oParent = a_bIsIgnoreParent ? null : CEditorAccess.GetActiveObj();
		GameObject oObj = null;

		// 부모가 없을 경우
		if(oParent == null && PrefabStageUtility.GetCurrentPrefabStage() != null) {
			oParent = PrefabStageUtility.GetCurrentPrefabStage().prefabContentsRoot;
		}

		// 파일 경로가 유효 할 경우
		if(a_oFilePath.ExIsValid()) {
			oObj = CFactory.CreateCloneObj(a_oName, a_oFilePath, oParent);
		} else {
			oObj = CFactory.CreateObj(a_oName, oParent);
		}

		oObj.ExEnumerateComponents<Text>((a_oText) => { a_oText.ExReset(); return true; });
		oObj.ExEnumerateComponents<TMP_Text>((a_oTMPText) => { a_oTMPText.ExReset(); return true; });
		oObj.ExEnumerateComponents<Image>((a_oImg) => { a_oImg.ExReset(); return true; });
		oObj.ExEnumerateComponents<Mask>((a_oMask) => { a_oMask.ExReset(); return true; });
		oObj.ExEnumerateComponents<Selectable>((a_oSelectable) => { a_oSelectable.ExReset(); return true; });
		oObj.ExEnumerateComponents<ScrollRect>((a_oScrollRect) => { a_oScrollRect.ExReset(); return true; });
		oObj.ExEnumerateComponents<CanvasRenderer>((a_oCanvasRenderer) => { a_oCanvasRenderer.ExReset(); return true; });
		oObj.ExEnumerateComponents<Renderer>((a_oRenderer) => { a_oRenderer.ExReset(); return true; });
		oObj.ExEnumerateComponents<LineRenderer>((a_oLine) => { a_oLine.ExReset(); return true; });
		oObj.ExEnumerateComponents<ParticleSystem>((a_oParticle) => { a_oParticle.ExReset(); return true; });
		oObj.ExEnumerateComponents<LightProbeGroup>((a_oLightProbeGroup) => { a_oLightProbeGroup.ExReset(); return true; });

		// 페이지 뷰가 아닐 경우
		if(!a_oName.Equals(KCDefine.U_OBJ_N_PAGE_VIEW)) {
			oObj.ExEnumerateComponents<HorizontalOrVerticalLayoutGroup>((a_oLayoutGroup) => { a_oLayoutGroup.ExReset(); return true; });
		}

		// 부모 객체가 존재 할 경우
		if(oParent != null) {
			oObj.layer = oParent.layer;

			// UI 부모 객체 일 경우
			if(oParent.TryGetComponent<RectTransform>(out RectTransform oTrans)) {
				oObj.ExEnumerateComponents<Transform>((a_oTrans) => {
					var oRectTrans = a_oTrans.gameObject.ExAddComponent<RectTransform>();
					oRectTrans.sizeDelta = (oRectTrans.gameObject == oObj) ? Vector3.zero : oRectTrans.sizeDelta;

					return true;
				});
			}
		}

		// 에디터 모드 일 경우
		if(!Application.isPlaying) {
			EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
			CFunc.SelObj(oObj);
		}

		Undo.RegisterCreatedObjectUndo(oObj, a_oName);
		return oObj;
	}
	#endregion // 클래스 함수
}

/** 메뉴 처리자 - 스크립트 객체 생성 */
public static partial class CMenuHandler {
	#region 클래스 함수
	/** 옵션 정보 테이블을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_CREATE_BASE + "Scriptable Object/Options Info Table", false, KCEditorDefine.B_SORTING_O_CREATE_MENU + 101)]
	public static void CreateOptsInfoTable() {
		var oOptsInfoTable = CEditorFactory.CreateScriptableObj<COptsInfoTable>();

		oOptsInfoTable.SetEtcOptsInfo(new STEtcOptsInfo() {
			m_bIsEnableTitleScene = false
		});

		oOptsInfoTable.SetSndOptsInfo(new STSndOptsInfo() {
			m_nNumRealVoices = 32,
			m_nNumVirtualVoices = 512
		});

		oOptsInfoTable.SetBuildOptsInfo(new STBuildOptsInfo() {
			m_bIsPreBakeCollisionMesh = false,
			m_bIsPreserveFrameBufferAlpha = false,

			m_oCameraDesc = string.Empty,
			m_oLocationDesc = string.Empty,
			m_oBluetoothDesc = string.Empty,
			m_oMicrophoneDesc = string.Empty,
			m_oInputSystemMotionDesc = string.Empty,

			m_eNormapMapEncoding = NormalMapEncoding.DXT5nm,
			m_eLightmapper = LightingSettings.Lightmapper.ProgressiveGPU,

			m_stiOSBuildOptsInfo = new STiOSBuildOptsInfo() {
				m_bIsEnableProMotion = false,
				m_bIsEnableInputSystemMotion = false
			},

			m_stAndroidBuildOptsInfo = new STAndroidBuildOptsInfo() {
				m_bIsUseAPKExpansionFiles = false
			},

			m_stStandaloneBuildOptsInfo = new STStandaloneBuildOptsInfo() {
				// Do Something
			}
		});

		oOptsInfoTable.SetQualityOptsInfo(new STQualityOptsInfo() {
			m_bIsEnableRealtimeGI = false,
			m_bIsEnableRealtimeReflectionProbes = false,
			m_bIsEnableRealtimeEnvironmentLighting = false,

			m_eQualityLevel = EQualityLevel.NORM,
			m_eMixedLightingMode = MixedLightingMode.Subtractive,
			m_eLightmapEncodingQuality = ELightmapEncodingQuality.NORM,

			m_stNormQualityRenderingOptsInfo = new STRenderingOptsInfo() {
				m_stLightOptsInfo = new STLightOptsInfo() {
					m_eLightmapMaxSize = EPOT._1024,
					m_eLightmapMode = ELightmapMode.NON_DIRECTIONAL,
					m_eShadowmaskMode = ShadowmaskMode.Shadowmask,
					m_eLightmapCompression = LightmapCompression.NormalQuality,
					m_eShadowResolution = ShadowResolution.Medium
				},

				m_stUniversalRPOptsInfo = new STUniversalRPOptsInfo() {
					m_eLightCookieResolution = EPOT._256,
					m_eMainLightShadowResolution = EPOT._1024,
					m_eAdditionalShadowResolution = EPOT._1024
				}
			},

			m_stHighQualityRenderingOptsInfo = new STRenderingOptsInfo() {
				m_stLightOptsInfo = new STLightOptsInfo() {
					m_eLightmapMaxSize = EPOT._2048,
					m_eLightmapMode = ELightmapMode.COMBINE_DIRECTIONAL,
					m_eShadowmaskMode = ShadowmaskMode.DistanceShadowmask,
					m_eLightmapCompression = LightmapCompression.HighQuality,
					m_eShadowResolution = ShadowResolution.High
				},

				m_stUniversalRPOptsInfo = new STUniversalRPOptsInfo() {
					m_eLightCookieResolution = EPOT._512,
					m_eMainLightShadowResolution = EPOT._2048,
					m_eAdditionalShadowResolution = EPOT._2048
				}
			},

			m_stUltraQualityRenderingOptsInfo = new STRenderingOptsInfo() {
				m_stLightOptsInfo = new STLightOptsInfo() {
					m_eLightmapMaxSize = EPOT._2048,
					m_eLightmapMode = ELightmapMode.COMBINE_DIRECTIONAL,
					m_eShadowmaskMode = ShadowmaskMode.DistanceShadowmask,
					m_eLightmapCompression = LightmapCompression.HighQuality,
					m_eShadowResolution = ShadowResolution.VeryHigh
				},

				m_stUniversalRPOptsInfo = new STUniversalRPOptsInfo() {
					m_eLightCookieResolution = EPOT._512,
					m_eMainLightShadowResolution = EPOT._2048,
					m_eAdditionalShadowResolution = EPOT._2048
				}
			}
		});
	}

	/** 빌드 정보 테이블을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_CREATE_BASE + "Scriptable Object/Build Info Table", false, KCEditorDefine.B_SORTING_O_CREATE_MENU + 101)]
	public static void CreateBuildInfoTable() {
		var oBuildInfoTable = CEditorFactory.CreateScriptableObj<CBuildInfoTable>();

		oBuildInfoTable.SetJenkinsInfo(new STJenkinsInfo() {
			m_oUserID = "dante",
			m_oBranch = "main",
			m_oSrc = "0000000000.Common/0300010101.Module_Unity",
			m_oProjRoot = "Client",
			m_oWorkspace = "/Users/dante/Documents/jenkins/workspace",
			m_oBuildToken = "JenkinsBuild",
			m_oAccessToken = "11769da7a267fd572b450b15e0e71b2f67",
			m_oBuildURLFmt = "http://127.0.0.1:8080/{0}/buildWithParameters"
		});

		oBuildInfoTable.SetCommonBuildInfo(new STCommonBuildInfo() {
			// Do Something
		});

		oBuildInfoTable.SetiOSBuildInfo(new STiOSBuildInfo() {
			m_oTeamID = "58364U6PXL",
			m_oTargetOSVer = "12.0",
			m_oDevProfileID = "",
			m_oStoreProfileID = ""
		});

		oBuildInfoTable.SetAndroidBuildInfo(new STAndroidBuildInfo() {
			m_oKeystorePath = "Keystore.keystore",
			m_oKeyaliasName = "Keystore",
			m_oKeystorePassword = "NSString132!",
			m_oKeyaliasPassword = "NSString132!",
			m_eMinSDKVer = AndroidSdkVersions.AndroidApiLevel22,
			m_eTargetSDKVer = AndroidSdkVersions.AndroidApiLevelAuto
		});

		oBuildInfoTable.SetStandaloneBuildInfo(new STStandaloneBuildInfo() {
			m_oCategory = "public.app-category.games",
			m_oTargetOSVer = "10.13.0"
		});
	}

	/** 전처리기 심볼 정보 테이블을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_CREATE_BASE + "Scriptable Object/Define Symbol Info Table", false, KCEditorDefine.B_SORTING_O_CREATE_MENU + 101)]
	public static void CreateDefineSymbolInfoTable() {
		var oDefineSymbolInfoTable = CEditorFactory.CreateScriptableObj<CDefineSymbolInfoTable>();

		oDefineSymbolInfoTable.SetCommonDefineSymbols(new List<string>() {
			KCEditorDefine.DS_DEFINE_S_MODE_2D_ENABLE,
			KCEditorDefine.DS_DEFINE_S_MODE_PORTRAIT_ENABLE,
			KCEditorDefine.DS_DEFINE_S_TEX_FMT_CORRECT_ENABLE,
			KCEditorDefine.DS_DEFINE_S_MSG_PACK_SERIALIZE_DESERIALIZE_ENABLE,
			KCEditorDefine.DS_DEFINE_S_SPRITE_PIXELS_PER_UNIT_CORRECT_ENABLE,
			KCEditorDefine.DS_DEFINE_S_UNIVERSAL_RENDERING_PIPELINE_ENABLE,
			KCEditorDefine.DS_DEFINE_S_ADDRESSABLES_ENABLE,
			KCEditorDefine.DS_DEFINE_S_BURST_COMPILER_ENABLE,
			KCEditorDefine.DS_DEFINE_S_LOCALIZE_ENABLE
		});

		oDefineSymbolInfoTable.SetSubCommonDefineSymbols(new List<string>() {
			KCEditorDefine.DS_DEFINE_S_DEVELOPMENT_PROJ,
			KCEditorDefine.DS_DEFINE_S_SCENE_TEMPLATES_ENABLE,
			KCEditorDefine.DS_DEFINE_S_EDITOR_SCENE_TEMPLATES_ENABLE,
			KCEditorDefine.DS_DEFINE_S_PREFAB_TEMPLATES_ENABLE,
			KCEditorDefine.DS_DEFINE_S_POPUP_PREFAB_TEMPLATES_ENABLE,
			KCEditorDefine.DS_DEFINE_S_UTILITY_SCRIPT_TEMPLATES_ENABLE
		});
	}

	/** 프로젝트 정보 테이블을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_CREATE_BASE + "Scriptable Object/Project Info Table", false, KCEditorDefine.B_SORTING_O_CREATE_MENU + 101)]
	public static void CreateProjInfoTable() {
		var oProjInfoTable = CEditorFactory.CreateScriptableObj<CProjInfoTable>();

		oProjInfoTable.SetCompanyInfo(new STCompanyInfo() {
			m_oCompany = "LKStudio",
			m_oPrivacyURL = "https://www.ninetap.com/privacy_policy.html",
			m_oServicesURL = "https://www.ninetap.com/terms_of_service.html",
			m_oSupportsMail = "lkstudio.dante@gmail.com"
		});

		oProjInfoTable.SetCommonProjInfo(new STCommonProjInfo() {
			m_oAppID = "lkstudio.dante.sample",
			m_oProjName = "Module_Unity",
			m_oProductName = "Module_Unity",
			m_oExtraProjDirName = "0300010101-Module_Unity"
		});

		oProjInfoTable.SetiOSAppleProjInfo(new STProjInfo() {
			m_stBuildVerInfo = new STBuildVerInfo() {
				m_nNum = 1,
				m_oVer = "0.0.1",
			},

			m_oAppID = string.Empty,
			m_oStoreAppID = "1309472470"
		});

		oProjInfoTable.SetAndroidGoogleProjInfo(new STProjInfo() {
			m_stBuildVerInfo = new STBuildVerInfo() {
				m_nNum = 1,
				m_oVer = "0.0.1",
			},

			m_oAppID = string.Empty,
			m_oStoreAppID = string.Empty
		});

		oProjInfoTable.SetAndroidAmazonProjInfo(new STProjInfo() {
			m_stBuildVerInfo = new STBuildVerInfo() {
				m_nNum = 1,
				m_oVer = "0.0.1",
			},

			m_oAppID = string.Empty,
			m_oStoreAppID = string.Empty
		});

		oProjInfoTable.SetStandaloneMacSteamProjInfo(new STProjInfo() {
			m_stBuildVerInfo = new STBuildVerInfo() {
				m_nNum = 1,
				m_oVer = "0.0.1",
			},

			m_oAppID = string.Empty,
			m_oStoreAppID = string.Empty
		});

		oProjInfoTable.SetStandaloneWndsSteamProjInfo(new STProjInfo() {
			m_stBuildVerInfo = new STBuildVerInfo() {
				m_nNum = 1,
				m_oVer = "0.0.1",
			},

			m_oAppID = string.Empty,
			m_oStoreAppID = string.Empty
		});
	}

	/** 디바이스 정보 테이블을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_CREATE_BASE + "Scriptable Object/Device Info Table", false, KCEditorDefine.B_SORTING_O_CREATE_MENU + 101)]
	public static void CreateDeviceInfoTable() {
		var oDeviceInfoTable = CEditorFactory.CreateScriptableObj<CDeviceInfoTable>();

		oDeviceInfoTable.SetDeviceInfo(new STDeviceInfo() {
#if ADS_MODULE_ENABLE && ADMOB_ADS_ENABLE
			m_oiOSAdmobTestDeviceIDList = new List<string>() {
				// Do Something
			},

			m_oAndroidAdmobTestDeviceIDList = new List<string>() {
				"938274EB10E16F425E5293F48651E5FE"
			}
#endif // #if ADS_MODULE_ENABLE && ADMOB_ADS_ENABLE
		});
	}

	/** 지역화 정보 테이블을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_CREATE_BASE + "Scriptable Object/Localize Info Table", false, KCEditorDefine.B_SORTING_O_CREATE_MENU + 101)]
	public static void CreateLocalizeInfoTable() {
		var oLocalizeInfoTable = CEditorFactory.CreateScriptableObj<CLocalizeInfoTable>();

		oLocalizeInfoTable.SetLocalizeInfos(new List<STLocalizeInfo>() {
			new STLocalizeInfo() {
				m_oCountryCode = string.Empty,
				m_eSystemLanguage = SystemLanguage.Korean,

				m_oFontSetInfoList = new List<STFontSetInfo>() {
					new STFontSetInfo() {
						m_eSet = EFontSet._1, m_oPath = KCDefine.U_FONT_P_G_KOREAN
					},

					new STFontSetInfo() {
						m_eSet = EFontSet._2, m_oPath = KCDefine.U_FONT_P_G_KOREAN
					}
				}
			},

			new STLocalizeInfo() {
				m_oCountryCode = string.Empty,
				m_eSystemLanguage = SystemLanguage.English,

				m_oFontSetInfoList = new List<STFontSetInfo> () {
					new STFontSetInfo() {
						m_eSet = EFontSet._1, m_oPath = KCDefine.U_FONT_P_G_DEF
					},

					new STFontSetInfo() {
						m_eSet = EFontSet._2, m_oPath = KCDefine.U_FONT_P_G_ENGLISH
					}
				}
			}
		});
	}

	/** 플러그인 정보 테이블을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_CREATE_BASE + "Scriptable Object/Plugin Info Table", false, KCEditorDefine.B_SORTING_O_CREATE_MENU + 101)]
	public static void CreatePluginInfoTable() {
		var oPluginInfoTable = CEditorFactory.CreateScriptableObj<CPluginInfoTable>();

#if ADS_MODULE_ENABLE
		oPluginInfoTable.SetAdsPlatform(EAdsPlatform.NONE);
		oPluginInfoTable.SetBannerAdsPos(EBannerAdsPos.NONE);

#if ADMOB_ADS_ENABLE
		oPluginInfoTable.SetiOSAdmobPluginInfo(new STAdmobPluginInfo() {
			m_oBannerAdsID = "ca-app-pub-8822822499150620/8271306569", m_oRewardAdsID = "ca-app-pub-8822822499150620/1962431972", m_oFullscreenAdsID = "ca-app-pub-8822822499150620/7804402708"
		});

		oPluginInfoTable.SetAndroidAdmobPluginInfo(new STAdmobPluginInfo() {
			m_oBannerAdsID = "ca-app-pub-8822822499150620/4769392868", m_oRewardAdsID = "ca-app-pub-8822822499150620/6629269445", m_oFullscreenAdsID = "ca-app-pub-8822822499150620/4386249489"
		});
#endif // #if ADMOB_ADS_ENABLE

#if IRON_SRC_ADS_ENABLE
		oPluginInfoTable.SetEnableIronSrcBannerAds(false);
		oPluginInfoTable.SetEnableIronSrcRewardAds(false);
		oPluginInfoTable.SetEnableIronSrcFullscreenAds(false);

		oPluginInfoTable.SetiOSIronSrcPluginInfo(new STIronSrcPluginInfo() {
			m_oAppKey = "aca5425d"
		});

		oPluginInfoTable.SetAndroidIronSrcPluginInfo(new STIronSrcPluginInfo() {
			m_oAppKey = "b8c2c725"
		});
#endif // #if IRON_SRC_ADS_ENABLE
#endif // #if ADS_MODULE_ENABLE

#if FLURRY_MODULE_ENABLE
		oPluginInfoTable.SetiOSFlurryAPIKey("SNFZB8CP9KFZQTDXK6HG");
		oPluginInfoTable.SetAndroidFlurryAPIKey("9RCTG3VW8J7457JMS7CJ");
#endif // #if FLURRY_MODULE_ENABLE

#if APPS_FLYER_MODULE_ENABLE
		oPluginInfoTable.SetAppsFlyerPluginInfo(new STAppsFlyerPluginInfo() {
			m_oDevKey = "J7eXAem8sBRuHTr3iX58d5"
		});
#endif // #if APPS_FLYER_MODULE_ENABLE
	}
	#endregion // 클래스 함수

	#region 조건부 클래스 함수
#if PURCHASE_MODULE_ENABLE
	/** 상품 정보 테이블을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_CREATE_BASE + "Scriptable Object/Product Info Table", false, KCEditorDefine.B_SORTING_O_CREATE_MENU + 101)]
	public static void CreateProductInfoTable() {
		var oProductInfoTable = CEditorFactory.CreateScriptableObj<CProductInfoTable>();

		oProductInfoTable.SetCommonProductInfos(new List<STProductInfo>() {
			new STProductInfo {
				m_oID = "lkstudio.dante.sample.iap.nc.pkgs.beginner", m_eProductType = ProductType.NonConsumable,

				m_stCommonInfo = new STCommonInfo() {
					m_oName = KCDefine.ST_KEY_C_BEGINNER_PKGS_TEXT, m_oDesc = string.Empty
				}
			},

			new STProductInfo {
				m_oID = "lkstudio.dante.sample.iap.nc.pkgs.expert", m_eProductType = ProductType.NonConsumable,

				m_stCommonInfo = new STCommonInfo() {
					m_oName = KCDefine.ST_KEY_C_EXPERT_PKGS_TEXT, m_oDesc = string.Empty
				}
			},

			new STProductInfo {
				m_oID = "lkstudio.dante.sample.iap.nc.pkgs.pro", m_eProductType = ProductType.NonConsumable,

				m_stCommonInfo = new STCommonInfo() {
					m_oName = KCDefine.ST_KEY_C_PRO_PKGS_TEXT, m_oDesc = string.Empty
				}
			},

			new STProductInfo {
				m_oID = "lkstudio.dante.sample.iap.c.single.coinsbox", m_eProductType = ProductType.Consumable,

				m_stCommonInfo = new STCommonInfo() {
					m_oName = KCDefine.ST_KEY_C_COINS_BOX_TEXT, m_oDesc = string.Empty
				}
			},

			new STProductInfo {
				m_oID = "lkstudio.dante.sample.iap.nc.single.removeads", m_eProductType = ProductType.NonConsumable,

				m_stCommonInfo = new STCommonInfo() {
					m_oName = KCDefine.ST_KEY_C_REMOVE_ADS_TEXT, m_oDesc = string.Empty
				}
			}
		});
	}
#endif // #if PURCHASE_MODULE_ENABLE
	#endregion // 조건부 클래스 함수
}
#endif // #if UNITY_EDITOR

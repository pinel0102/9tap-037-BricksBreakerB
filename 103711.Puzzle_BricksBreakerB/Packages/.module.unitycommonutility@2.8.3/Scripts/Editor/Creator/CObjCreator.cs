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

/** 객체 생성자 */
public static class CObjCreator {
	#region 클래스 함수
	/** 게임 객체를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_BASE + "GameObject %#[", false, 101)]
	public static void CreateGameObj() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_GAME_OBJ, string.Empty, true);
	}

	/** 게임 객체를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_BASE + "ChildGameObject %#]", false, 101)]
	public static void CreateChildGameObj() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_GAME_OBJ, string.Empty);
	}

	/** 텍스트를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Text/Text", false, 101)]
	public static void CreateText() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_TEXT, KCDefine.U_OBJ_P_TEXT);
	}

	/** 텍스트를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Text/LocalizeText", false, 101)]
	public static void CreateLocalizeText() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_LOCALIZE_TEXT, KCDefine.U_OBJ_P_LOCALIZE_TEXT);
	}

	/** 텍스트를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Text/TMPText", false, 101)]
	public static void CreateTMPText() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_TMP_TEXT, KCDefine.U_OBJ_P_TMP_TEXT);
	}

	/** 텍스트를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Text/TMPLocalizeText", false, 101)]
	public static void CreateTMPLocalizeText() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_TMP_LOCALIZE_TEXT, KCDefine.U_OBJ_P_TMP_LOCALIZE_TEXT);
	}

	/** 이미지를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Image/Image", false, 101)]
	public static void CreateImg() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_IMG, KCDefine.U_OBJ_P_IMG);
	}

	/** 이미지를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Image/MaskImage", false, 101)]
	public static void CreateMaskImg() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_MASK_IMG, KCDefine.U_OBJ_P_MASK_IMG);
	}

	/** 이미지를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Image/FocusImage", false, 101)]
	public static void CreateFocusImg() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_FOCUS_IMG, KCDefine.U_OBJ_P_FOCUS_IMG);
	}

	/** 이미지를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Image/GaugeImage", false, 101)]
	public static void CreateGaugeImg() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_GAUGE_IMG, KCDefine.U_OBJ_P_GAUGE_IMG);
	}

	/** 이미지 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/ImageButton", false, 101)]
	public static void CreateImgBtn() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_IMG_BTN, KCDefine.U_OBJ_P_IMG_BTN);
	}

	/** 이미지 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/ImageScaleButton", false, 101)]
	public static void CreateImgScaleBtn() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_IMG_SCALE_BTN, KCDefine.U_OBJ_P_IMG_SCALE_BTN);
	}

	/** 텍스트 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/Text/Text + Button/TextButton", false, 101)]
	public static void CreateTextBtn() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_TEXT_BTN, KCDefine.U_OBJ_P_TEXT_BTN);
	}

	/** 텍스트 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/Text/Text + Button/TextScaleButton", false, 101)]
	public static void CreateTextScaleBtn() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_TEXT_SCALE_BTN, KCDefine.U_OBJ_P_TEXT_SCALE_BTN);
	}

	/** 텍스트 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/Text/LocalizeText + Button/LocalizeTextButton", false, 101)]
	public static void CreateLocalizeTextBtn() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_LOCALIZE_TEXT_BTN, KCDefine.U_OBJ_P_LOCALIZE_TEXT_BTN);
	}

	/** 텍스트 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/Text/LocalizeText + Button/LocalizeTextScaleButton", false, 101)]
	public static void CreateLocalizeTextScaleBtn() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_LOCALIZE_TEXT_SCALE_BTN, KCDefine.U_OBJ_P_LOCALIZE_TEXT_SCALE_BTN);
	}

	/** 이미지 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/Text/Image + Text + Button/ImageTextButton", false, 101)]
	public static void CreateImgTextBtn() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_IMG_TEXT_BTN, KCDefine.U_OBJ_P_IMG_TEXT_BTN);
	}

	/** 이미지 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/Text/Image + Text + Button/ImageTextScaleButton", false, 101)]
	public static void CreateImgTextScaleBtn() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_IMG_TEXT_SCALE_BTN, KCDefine.U_OBJ_P_IMG_TEXT_SCALE_BTN);
	}

	/** 이미지 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/Text/Image + LocalizeText + Button/ImageLocalizeTextButton", false, 101)]
	public static void CreateImgLocalizeTextBtn() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_IMG_LOCALIZE_TEXT_BTN, KCDefine.U_OBJ_P_IMG_LOCALIZE_TEXT_BTN);
	}

	/** 이미지 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/Text/Image + LocalizeText + Button/ImageLocalizeTextScaleButton", false, 101)]
	public static void CreateImgLocalizeTextScaleBtn() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_IMG_LOCALIZE_TEXT_SCALE_BTN, KCDefine.U_OBJ_P_IMG_LOCALIZE_TEXT_SCALE_BTN);
	}

	/** 텍스트 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/TextMeshPro/Text + Button/TextButton", false, 101)]
	public static void CreateTMPTextBtn() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_TMP_TEXT_BTN, KCDefine.U_OBJ_P_TMP_TEXT_BTN);
	}

	/** 텍스트 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/TextMeshPro/Text + Button/TextScaleButton", false, 101)]
	public static void CreateTMPTextScaleBtn() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_TMP_TEXT_SCALE_BTN, KCDefine.U_OBJ_P_TMP_TEXT_SCALE_BTN);
	}

	/** 텍스트 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/TextMeshPro/LocalizeText + Button/LocalizeTextButton", false, 101)]
	public static void CreateTMPLocalizeTextBtn() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_TMP_LOCALIZE_TEXT_BTN, KCDefine.U_OBJ_P_TMP_LOCALIZE_TEXT_BTN);
	}

	/** 텍스트 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/TextMeshPro/LocalizeText + Button/LocalizeTextScaleButton", false, 101)]
	public static void CreateTMPLocalizeTextScaleBtn() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_TMP_LOCALIZE_TEXT_SCALE_BTN, KCDefine.U_OBJ_P_TMP_LOCALIZE_TEXT_SCALE_BTN);
	}

	/** 이미지 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/TextMeshPro/Image + Text + Button/ImageTextButton", false, 101)]
	public static void CreateTMPImgTextBtn() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_TMP_IMG_TEXT_BTN, KCDefine.U_OBJ_P_TMP_IMG_TEXT_BTN);
	}

	/** 이미지 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/TextMeshPro/Image + Text + Button/ImageTextScaleButton", false, 101)]
	public static void CreateTMPImgTextScaleBtn() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_TMP_IMG_TEXT_SCALE_BTN, KCDefine.U_OBJ_P_TMP_IMG_TEXT_SCALE_BTN);
	}

	/** 이미지 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/TextMeshPro/Image + LocalizeText + Button/ImageLocalizeTextButton", false, 101)]
	public static void CreateTMPImgLocalizeTextBtn() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_TMP_IMG_LOCALIZE_TEXT_BTN, KCDefine.U_OBJ_P_TMP_IMG_LOCALIZE_TEXT_BTN);
	}

	/** 이미지 버튼을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Button/TextMeshPro/Image + LocalizeText + Button/ImageLocalizeTextScaleButton", false, 101)]
	public static void CreateTMPImgLocalizeTextScaleBtn() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_TMP_IMG_LOCALIZE_TEXT_SCALE_BTN, KCDefine.U_OBJ_P_TMP_IMG_LOCALIZE_TEXT_SCALE_BTN);
	}

	/** 입력 필드를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Input/Dropdown", false, 101)]
	public static void CreateDropdown() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_DROPDOWN, KCDefine.U_OBJ_P_DROPDOWN);
	}

	/** 입력 필드를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Input/InputField", false, 101)]
	public static void CreateInputField() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_INPUT_FIELD, KCDefine.U_OBJ_P_INPUT_FIELD);
	}

	/** 입력 필드를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Input/TMPDropdown", false, 101)]
	public static void CreateTMPDropdown() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_TMP_DROPDOWN, KCDefine.U_OBJ_P_TMP_DROPDOWN);
	}

	/** 입력 필드를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Input/TMPInputField", false, 101)]
	public static void CreateTMPInputField() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_TMP_INPUT_FIELD, KCDefine.U_OBJ_P_TMP_INPUT_FIELD);
	}

	/** 페이지 뷰를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "ScrollView/PageView", false, 101)]
	public static void CreatePageView() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_PAGE_VIEW, KCDefine.U_OBJ_P_PAGE_VIEW);
	}

	/** 스크롤 뷰를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "ScrollView/ScrollView", false, 101)]
	public static void CreateScrollView() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_SCROLL_VIEW, KCDefine.U_OBJ_P_SCROLL_VIEW);
	}

	/** 재사용 뷰를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "ScrollView/RecycleView", false, 101)]
	public static void CreateRecycleView() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_RECYCLE_VIEW, KCDefine.U_OBJ_P_RECYCLE_VIEW);
	}

	/** 드래그 응답자를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Responder/DragResponder", false, 101)]
	public static void CreateDragResponder() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_DRAG_RESPONDER, KCDefine.U_OBJ_P_DRAG_RESPONDER);
	}

	/** 터치 응답자를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_UI_BASE + "Responder/TouchResponder", false, 101)]
	public static void CreateTouchResponder() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_TOUCH_RESPONDER, KCDefine.U_OBJ_P_TOUCH_RESPONDER);
	}

	/** 스프라이트를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_2D_BASE + "Sprite", false, 101)]
	public static void CreateSprite() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_SPRITE, KCDefine.U_OBJ_P_SPRITE);
	}

	/** 라인 효과를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_FX_BASE + "LineFX", false, 101)]
	public static void CreateLineFX() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_LINE_FX, KCDefine.U_OBJ_P_LINE_FX);
	}

	/** 파티클 효과를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_FX_BASE + "ParticleFX", false, 101)]
	public static void CreateParticleFX() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_PARTICLE_FX, KCDefine.U_OBJ_P_PARTICLE_FX);
	}

	/** 반사 프로브를 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_FX_BASE + "Reflection Probe", false, 101)]
	public static void CreateReflectionProbe() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_REFLECTION_PROBE, KCDefine.U_OBJ_P_REFLECTION_PROBE);
	}

	/** 광원 프로브 그룹을 생성한다 */
	[MenuItem(KCEditorDefine.B_MENU_GAME_OBJECT_FX_BASE + "Light Probe Group", false, 101)]
	public static void CreateLightProbeGroup() {
		CObjCreator.CreateObj(KCDefine.U_OBJ_N_LIGHT_PROBE_GROUP, KCDefine.U_OBJ_P_LIGHT_PROBE_GROUP);
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
#endif // #if UNITY_EDITOR

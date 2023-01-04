using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using TMPro;

/*
Admob 설정
- iOS App ID: ca-app-pub-8822822499150620~1354544428
- iOS Banner Ads ID: ca-app-pub-8822822499150620/8271306569
- iOS Reward Ads ID: ca-app-pub-8822822499150620/1962431972
- iOS Fullscreen Ads ID: ca-app-pub-8822822499150620/7804402708

- Android App ID: ca-app-pub-8822822499150620~8161842966
- Android Banner Ads ID: ca-app-pub-8822822499150620/4769392868
- Android Reward Ads ID: ca-app-pub-8822822499150620/6629269445
- Android Fullscreen Ads ID: ca-app-pub-8822822499150620/4386249489

Iron Source 설정
- iOS App Key: 1707b7945
- Android App Key: 1707b0635

Flurry 설정
- iOS API Key: SNFZB8CP9KFZQTDXK6HG
- Android API Key: 9RCTG3VW8J7457JMS7CJ

Facebook 설정
- App Name: Sample
- App ID: 646060797250571
- Client Token: cea82f0672b06cf906840c3f00fb22c7
*/
/** 메뉴 처리자 */
public static partial class CMenuHandler {
	#region 클래스 함수
	/** 텍스트를 리셋한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_RESET_BASE + "Texts", false, KCEditorDefine.B_SORTING_O_RESET_MENU + 1)]
	public static void ResetTexts() {
		// 확인 버튼을 눌렀을 경우
		if(CEditorFunc.ShowOKCancelAlertPopup(KCEditorDefine.B_TEXT_ALERT_P_TITLE, KCEditorDefine.B_MSG_ALERT_P_RESET)) {
			var oTextList = CEditorFunc.FindComponents<Text>();
			var oTMPTextList = CEditorFunc.FindComponents<TMP_Text>();

			for(int i = 0; i < oTextList.Count; ++i) {
				oTextList[i].ExReset();

				// 에디터 모드 일 경우
				if(!Application.isPlaying) {
					EditorSceneManager.MarkSceneDirty(oTextList[i].gameObject.scene);
				}
			}

			for(int i = 0; i < oTMPTextList.Count; ++i) {
				oTMPTextList[i].ExReset();

				// 에디터 모드 일 경우
				if(!Application.isPlaying) {
					EditorSceneManager.MarkSceneDirty(oTMPTextList[i].gameObject.scene);
				}
			}
		}
	}

	/** 상호 작용자를 리셋한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_RESET_BASE + "Selectables", false, KCEditorDefine.B_SORTING_O_RESET_MENU + 1)]
	public static void ResetSelectables() {
		// 확인 버튼을 눌렀을 경우
		if(CEditorFunc.ShowOKCancelAlertPopup(KCEditorDefine.B_TEXT_ALERT_P_TITLE, KCEditorDefine.B_MSG_ALERT_P_RESET)) {
			var oSelectableList = CEditorFunc.FindComponents<Selectable>();

			for(int i = 0; i < oSelectableList.Count; ++i) {
				oSelectableList[i].ExReset();

				// 에디터 모드 일 경우
				if(!Application.isPlaying) {
					EditorSceneManager.MarkSceneDirty(oSelectableList[i].gameObject.scene);
				}
			}
		}
	}

	/** 스크롤 영역을 리셋한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_RESET_BASE + "Scroll Rects", false, KCEditorDefine.B_SORTING_O_RESET_MENU + 1)]
	public static void ResetScrollRects() {
		// 확인 버튼을 눌렀을 경우
		if(CEditorFunc.ShowOKCancelAlertPopup(KCEditorDefine.B_TEXT_ALERT_P_TITLE, KCEditorDefine.B_MSG_ALERT_P_RESET)) {
			var oScrollRectList = CEditorFunc.FindComponents<ScrollRect>();

			for(int i = 0; i < oScrollRectList.Count; ++i) {
				oScrollRectList[i].ExReset();

				// 에디터 모드 일 경우
				if(!Application.isPlaying) {
					EditorSceneManager.MarkSceneDirty(oScrollRectList[i].gameObject.scene);
				}
			}
		}
	}

	/** 캔버스 렌더러를 리셋한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_RESET_BASE + "Canvas Renderers", false, KCEditorDefine.B_SORTING_O_RESET_MENU + 1)]
	public static void ResetCanvasRenderers() {
		// 확인 버튼을 눌렀을 경우
		if(CEditorFunc.ShowOKCancelAlertPopup(KCEditorDefine.B_TEXT_ALERT_P_TITLE, KCEditorDefine.B_MSG_ALERT_P_RESET)) {
			var oCanvasRendererList = CEditorFunc.FindComponents<CanvasRenderer>();

			for(int i = 0; i < oCanvasRendererList.Count; ++i) {
				oCanvasRendererList[i].ExReset();

				// 에디터 모드 일 경우
				if(!Application.isPlaying) {
					EditorSceneManager.MarkSceneDirty(oCanvasRendererList[i].gameObject.scene);
				}
			}
		}
	}

	/** 레이아웃 그룹을 리셋한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_RESET_BASE + "Horizontal or Vertical Layout Groups", false, KCEditorDefine.B_SORTING_O_RESET_MENU + 1)]
	public static void ResetLayoutGroups() {
		// 확인 버튼을 눌렀을 경우
		if(CEditorFunc.ShowOKCancelAlertPopup(KCEditorDefine.B_TEXT_ALERT_P_TITLE, KCEditorDefine.B_MSG_ALERT_P_RESET)) {
			var oLayoutGroupList = CEditorFunc.FindComponents<HorizontalOrVerticalLayoutGroup>();

			for(int i = 0; i < oLayoutGroupList.Count; ++i) {
				oLayoutGroupList[i].ExReset();

				// 에디터 모드 일 경우
				if(!Application.isPlaying) {
					EditorSceneManager.MarkSceneDirty(oLayoutGroupList[i].gameObject.scene);
				}
			}
		}
	}

	/** 카메라를 리셋한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_RESET_BASE + "Cameras", false, KCEditorDefine.B_SORTING_O_RESET_MENU + 1)]
	public static void ResetCameras() {
		// 확인 버튼을 눌렀을 경우
		if(CEditorFunc.ShowOKCancelAlertPopup(KCEditorDefine.B_TEXT_ALERT_P_TITLE, KCEditorDefine.B_MSG_ALERT_P_RESET)) {
			var oCameraList = CEditorFunc.FindComponents<Camera>();

			for(int i = 0; i < oCameraList.Count; ++i) {
				oCameraList[i].ExReset();

				// 에디터 모드 일 경우
				if(!Application.isPlaying) {
					EditorSceneManager.MarkSceneDirty(oCameraList[i].gameObject.scene);
				}
			}
		}
	}

	/** 렌더러를 리셋한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_RESET_BASE + "Renderers", false, KCEditorDefine.B_SORTING_O_RESET_MENU + 1)]
	public static void ResetRenderers() {
		// 확인 버튼을 눌렀을 경우
		if(CEditorFunc.ShowOKCancelAlertPopup(KCEditorDefine.B_TEXT_ALERT_P_TITLE, KCEditorDefine.B_MSG_ALERT_P_RESET)) {
			var oRendererList = CEditorFunc.FindComponents<Renderer>();

			for(int i = 0; i < oRendererList.Count; ++i) {
				oRendererList[i].ExReset();

				// 에디터 모드 일 경우
				if(!Application.isPlaying) {
					EditorSceneManager.MarkSceneDirty(oRendererList[i].gameObject.scene);
				}
			}
		}
	}

	/** 기준점을 리셋한다 */
	[MenuItem(KCEditorDefine.B_MENU_TOOLS_RESET_BASE + "Pivots", false, KCEditorDefine.B_SORTING_O_RESET_MENU + 1)]
	public static void ResetPivots() {
		// 확인 버튼을 눌렀을 경우
		if(CEditorFunc.ShowOKCancelAlertPopup(KCEditorDefine.B_TEXT_ALERT_P_TITLE, KCEditorDefine.B_MSG_ALERT_P_RESET)) {
			var oBtnList = CEditorFunc.FindComponents<Button>();

			for(int i = 0; i < oBtnList.Count; ++i) {
				var stSrcPivot = (oBtnList[i].transform as RectTransform).pivot.ExTo3D();
				var stDestPivot = KCDefine.B_ANCHOR_MID_CENTER;

				(oBtnList[i].transform as RectTransform).pivot = stDestPivot;
				(oBtnList[i].transform as RectTransform).localPosition = (oBtnList[i].transform as RectTransform).localPosition.ExGetUIsPivotPos(stSrcPivot, stDestPivot, (oBtnList[i].transform as RectTransform).rect.size);

				// 에디터 모드 일 경우
				if(!Application.isPlaying) {
					EditorSceneManager.MarkSceneDirty(oBtnList[i].gameObject.scene);
				}
			}
		}
	}
	#endregion // 클래스 함수
}
#endif // #if UNITY_EDITOR

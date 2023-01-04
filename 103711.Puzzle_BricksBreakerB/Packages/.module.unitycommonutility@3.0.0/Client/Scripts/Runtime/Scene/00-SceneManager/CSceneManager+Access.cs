using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using AOP;

/** 씬 관리자 - 접근 */
public abstract partial class CSceneManager : CComponent {
	#region 함수
	/** 객체 풀을 반환한다 */
	public ObjectPool GetObjsPool(string a_oKey) {
		CAccess.Assert(a_oKey.ExIsValid());
		return m_oObjsPoolDict.GetValueOrDefault(a_oKey);
	}

	/** 추가 카메라를 반환한다 */
	private List<Camera> GetAdditionalCameras() {
		// 추가 카메라가 없을 경우
		if(!m_oAdditionalCameraList.ExIsValid()) {
			this.gameObject.scene.ExEnumerateComponents<Camera>((a_oCamera) => {
				// 메인 카메라가 아닐 경우
				if(!a_oCamera.CompareTag(KCDefine.U_TAG_MAIN_CAMERA) && !a_oCamera.name.Equals(KCDefine.U_OBJ_N_SCENE_MAIN_CAMERA)) {
					m_oAdditionalCameraList.ExAddVal(a_oCamera);
				}

				return true;
			}, true);
		}

		return m_oAdditionalCameraList;
	}
	#endregion // 함수

	#region 클래스 함수
	/** 터치 응답자를 반환한다 */
	public static GameObject GetTouchResponder(string a_oKey) {
		CAccess.Assert(a_oKey.ExIsValid());
		return CSceneManager.m_oTouchResponderInfoDict.TryGetValue(a_oKey, out STTouchResponderInfo stTouchResponderInfo) ? stTouchResponderInfo.m_oTouchResponder : null;
	}

	/** 초기화 여부를 변경한다 */
	public static void SetInit(bool a_bIsInit) {
		CSceneManager.IsInit = a_bIsInit;
	}

	/** 설정 여부를 변경한다 */
	public static void SetSetup(bool a_bIsSetup) {
		CSceneManager.IsSetup = a_bIsSetup;
	}

	/** 지연 설정 여부를 변경한다 */
	public static void SetLateSetup(bool a_bIsSetup) {
		CSceneManager.IsLateSetup = a_bIsSetup;
	}

	/** 정적 디버그 텍스트를 변경한다 */
	public static void SetStaticDebugText(string a_oStr) {
#if DEBUG || DEVELOPMENT_BUILD
		CSceneManager.m_oStrBuilderDict.GetValueOrDefault(EKey.EXTRA_STATIC_DEBUG_STR_BUILDER).Clear();
		CSceneManager.m_oStrBuilderDict.GetValueOrDefault(EKey.EXTRA_STATIC_DEBUG_STR_BUILDER).AppendLine(a_oStr);
#endif // #if DEBUG || DEVELOPMENT_BUILD
	}

	/** 동적 디버그 텍스트를 변경한다 */
	public static void SetDynamicDebugText(string a_oStr) {
#if DEBUG || DEVELOPMENT_BUILD
		CSceneManager.m_oStrBuilderDict.GetValueOrDefault(EKey.EXTRA_DYNAMIC_DEBUG_STR_BUILDER).Clear();
		CSceneManager.m_oStrBuilderDict.GetValueOrDefault(EKey.EXTRA_DYNAMIC_DEBUG_STR_BUILDER).AppendLine(a_oStr);
#endif // #if DEBUG || DEVELOPMENT_BUILD
	}

	/** 스케줄 관리자를 변경한다 */
	public static void SetScheduleManager(CScheduleManager a_oManager) {
		CSceneManager.ScheduleManager = a_oManager;
	}

	/** 네비게이션 스택 관리자를 변경한다 */
	public static void SetNavStackManager(CNavStackManager a_oManager) {
		CSceneManager.NavStackManager = a_oManager;
	}

	/** 컬렉션 관리자를 변경한다 */
	public static void SetCollectionManager(CCollectionManager a_oManager) {
		CSceneManager.CollectionManager = a_oManager;
	}

	/** 화면 블라인드 UI 를 변경한다 */
	public static void SetScreenBlindUIs(GameObject a_oUIs) {
		CSceneManager.ScreenBlindUIs = a_oUIs;
	}

	/** 화면 팝업 UI 를 변경한다 */
	public static void SetScreenPopupUIs(GameObject a_oUIs) {
		CSceneManager.ScreenPopupUIs = a_oUIs;
	}

	/** 화면 최상위 UI 를 변경한다 */
	public static void SetScreenTopmostUIs(GameObject a_oUIs) {
		CSceneManager.ScreenTopmostUIs = a_oUIs;
	}

	/** 화면 절대 UI 를 변경한다 */
	public static void SetScreenAbsUIs(GameObject a_oUIs) {
		CSceneManager.ScreenAbsUIs = a_oUIs;
	}

	/** 정적 디버그 문자열을 추가한다 */
	public static void AddStaticDebugStr(string a_oStr) {
#if DEBUG || DEVELOPMENT_BUILD
		CSceneManager.m_oStrBuilderDict.GetValueOrDefault(EKey.EXTRA_STATIC_DEBUG_STR_BUILDER).AppendLine(a_oStr);
#endif // #if DEBUG || DEVELOPMENT_BUILD
	}

	/** 동적 디버그 문자열을 추가한다 */
	public static void AddDynamicDebugStr(string a_oStr) {
#if DEBUG || DEVELOPMENT_BUILD
		CSceneManager.m_oStrBuilderDict.GetValueOrDefault(EKey.EXTRA_DYNAMIC_DEBUG_STR_BUILDER).AppendLine(a_oStr);
#endif // #if DEBUG || DEVELOPMENT_BUILD
	}
	#endregion // 클래스 함수

	#region 제네릭 클래스 함수
	/** 씬 관리자를 반환한다 */
	public static T GetSceneManager<T>(string a_oKey) where T : CSceneManager {
		CAccess.Assert(a_oKey.ExIsValid());
		return CSceneManager.m_oSceneManagerDict.GetValueOrDefault(a_oKey) as T;
	}
	#endregion // 제네릭 클래스 함수
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 팝업 함수 */
public static partial class Func {
	#region 클래스 함수
	/** 상점 팝업을 출력한다 */
	public static void ShowStorePopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CStorePopup>(KDefine.G_OBJ_N_STORE_POPUP, KCDefine.U_OBJ_P_G_STORE_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 설정 팝업을 출력한다 */
	public static void ShowSettingsPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CSettingsPopup>(KDefine.G_OBJ_N_SETTINGS_POPUP, KCDefine.U_OBJ_P_G_SETTINGS_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 동기화 팝업을 출력한다 */
	public static void ShowSyncPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CSyncPopup>(KDefine.G_OBJ_N_SYNC_POPUP, KCDefine.U_OBJ_P_G_SYNC_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 일일 미션 팝업을 출력한다 */
	public static void ShowDailyMissionPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CDailyMissionPopup>(KDefine.G_OBJ_N_DAILY_MISSION_POPUP, KCDefine.U_OBJ_P_G_DAILY_MISSION_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 무료 보상 팝업을 출력한다 */
	public static void ShowFreeRewardPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CFreeRewardPopup>(KDefine.G_OBJ_N_FREE_REWARD_POPUP, KCDefine.U_OBJ_P_G_FREE_REWARD_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 일일 보상 팝업을 출력한다 */
	public static void ShowDailyRewardPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CDailyRewardPopup>(KDefine.G_OBJ_N_DAILY_REWARD_POPUP, KCDefine.U_OBJ_P_G_DAILY_REWARD_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 코인 상자 구입 팝업을 출력한다 */
	public static void ShowCoinsBoxBuyPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CCoinsBoxBuyPopup>(KDefine.G_OBJ_N_COINS_BOX_POPUP, KCDefine.U_OBJ_P_G_COINS_BOX_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 보상 획득 팝업을 출력한다 */
	public static void ShowRewardAcquirePopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CRewardAcquirePopup>(KDefine.G_OBJ_N_REWARD_ACQUIRE_POPUP, KCDefine.U_OBJ_P_G_REWARD_ACQUIRE_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 코인 상자 획득 팝업을 출력한다 */
	public static void ShowCoinsBoxAcquirePopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CCoinsBoxAcquirePopup>(KDefine.G_OBJ_N_COINS_BOX_ACQUIRE_POPUP, KCDefine.U_OBJ_P_G_COINS_BOX_ACQUIRE_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 이어하기 팝업을 출력한다 */
	public static void ShowContinuePopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CContinuePopup>(KDefine.G_OBJ_N_CONTINUE_POPUP, KCDefine.U_OBJ_P_G_CONTINUE_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 결과 팝업을 출력한다 */
	public static void ShowResultPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CResultPopup>(KDefine.G_OBJ_N_RESULT_POPUP, KCDefine.U_OBJ_P_G_RESULT_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 재개 팝업을 출력한다 */
	public static void ShowResumePopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CResumePopup>(KDefine.G_OBJ_N_RESUME_POPUP, KCDefine.U_OBJ_P_G_RESUME_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 정지 팝업을 출력한다 */
	public static void ShowPausePopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CPausePopup>(KDefine.G_OBJ_N_PAUSE_POPUP, KCDefine.U_OBJ_P_G_PAUSE_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 상품 구입 팝업을 출력한다 */
	public static void ShowProductBuyPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CProductBuyPopup>(KDefine.G_OBJ_N_PRODUCT_BUY_POPUP, KCDefine.U_OBJ_P_G_PRODUCT_BUY_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 포커스 팝업을 출력한다 */
	public static void ShowFocusPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CFocusPopup>(KDefine.G_OBJ_N_FOCUS_POPUP, KCDefine.U_OBJ_P_G_FOCUS_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

	/** 튜토리얼 팝업을 출력한다 */
	public static void ShowTutorialPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CTutorialPopup>(KDefine.G_OBJ_N_TUTORIAL_POPUP, KCDefine.U_OBJ_P_G_TUTORIAL_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}

    /** 프리뷰 팝업을 출력한다 */
	public static void ShowPreviewPopup(GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) {
		Func.ShowPopup<CPreviewPopup>(KDefine.G_OBJ_N_PREVIEW_POPUP, KCDefine.U_OBJ_P_G_PREVIEW_POPUP, a_oParent, a_oInitCallback, a_oShowCallback, a_oCloseCallback);
	}
	#endregion // 클래스 함수

	#region 제니릭 클래스 함수
	/** 팝업을 출력한다 */
	public static void ShowPopup<T>(string a_oName, string a_oObjPath, GameObject a_oParent, System.Action<CPopup> a_oInitCallback, System.Action<CPopup> a_oShowCallback = null, System.Action<CPopup> a_oCloseCallback = null) where T : CPopup {
		// 팝업이 없을 경우
        if(a_oParent.ExFindChild(a_oName) == null && CAccess.IsExistsRes<GameObject>(a_oObjPath)) {
			var oPopup = CPopup.Create<T>(a_oName, a_oObjPath, a_oParent);
			CFunc.Invoke(ref a_oInitCallback, oPopup);

			oPopup.Show(a_oShowCallback, a_oCloseCallback);
		}
	}
	#endregion // 제니릭 클래스 함수
}

/** 팝업 함수 - 알림 */
public static partial class Func {
	#region 클래스 함수
	/** 경고 팝업을 출력한다 */
	public static void ShowAlertPopup(CAlertPopup.STParams a_stParams) {
		// 경고 팝업이 없을 경우
		if(CSceneManager.ScreenPopupUIs.ExFindChild(KCDefine.U_OBJ_N_ALERT_POPUP) == null) {
			var oAlertPopup = CAlertPopup.Create<CAlertPopup>(KCDefine.U_OBJ_N_ALERT_POPUP, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_G_ALERT_POPUP), CSceneManager.ScreenPopupUIs, a_stParams);
			oAlertPopup.Show(null, null);
		}
	}

	/** 경고 팝업을 출력한다 */
	public static void ShowAlertPopup(string a_oMsg, System.Action<CAlertPopup, bool> a_oCallback, bool a_bIsEnableCancelBtn = true) {
		Func.ShowAlertPopup(CAlertPopup.MakeParams(CStrTable.Inst.GetStr(KCDefine.ST_KEY_C_NOTI_TEXT), a_oMsg, CStrTable.Inst.GetStr(KCDefine.ST_KEY_C_OK_TEXT), a_bIsEnableCancelBtn ? CStrTable.Inst.GetStr(KCDefine.ST_KEY_C_CANCEL_TEXT) : string.Empty, new Dictionary<CAlertPopup.ECallback, System.Action<CAlertPopup, bool>>() {
			[CAlertPopup.ECallback.OK_CANCEL] = a_oCallback
		}));
	}
	#endregion // 클래스 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

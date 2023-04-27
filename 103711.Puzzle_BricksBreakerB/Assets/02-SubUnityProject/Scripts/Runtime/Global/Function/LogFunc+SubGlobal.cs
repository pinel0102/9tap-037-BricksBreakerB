using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE
/** 서브 전역 로그 함수 */
public static partial class LogFunc {
	/** 로그 아이템 정보 */
	public struct STLogItemInfo {
		public int m_nNumCoins;

		public int m_nGameItem01;
		public int m_nGameItem02;
		public int m_nGameItem03;
		public int m_nGameItem04;
		public int m_nGameItem05;

		public int m_nBoosterItem01;
		public int m_nBoosterItem02;
		public int m_nBoosterItem03;
	}

	#region 클래스 함수
	/** 유저 로그를 전송한다 */
	public static void SendUserLog() {
		var oDataDict = LogFunc.MakeDefDatas();

#if ANALYTICS_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)
		oDataDict.TryAdd(KCDefine.L_LOG_KEY_USER_TYPE, KCDefine.B_TEXT_UNKNOWN);
#else
		oDataDict.TryAdd(KCDefine.L_LOG_KEY_USER_TYPE, CCommonUserInfoStorage.Inst.UserInfo.UserType.ToString());
#endif // #if ANALYTICS_TEST_ENABLE || (DEBUG || DEVELOPMENT_BUILD)

		LogFunc.SendLog(KDefine.L_LOG_N_USER, oDataDict);
	}

	/** 씬 플레이 로그를 전송한다 */
	public static void Send_I_Scene_Play(int a_nLevelID) {
		var oDataDict = LogFunc.MakeDefDatas();
		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_01, $"{a_nLevelID + KCDefine.B_VAL_1_INT}");

		LogFunc.SendLog(KDefine.L_LOG_N_I_SCENE_PLAY, oDataDict);
	}

	/** 씬 클리어 로그를 전송한다 */
	public static void Send_I_Scene_Clear(int a_nLevelID) {
		var oDataDict = LogFunc.MakeDefDatas();
		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_01, $"{a_nLevelID + KCDefine.B_VAL_1_INT}");

		LogFunc.SendLog(KDefine.L_LOG_N_I_SCENE_CLEAR, oDataDict);
	}

	/** 씬 실패 로그를 전송한다 */
	public static void Send_I_Scene_Fail(int a_nLevelID) {
		var oDataDict = LogFunc.MakeDefDatas();
		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_01, $"{a_nLevelID + KCDefine.B_VAL_1_INT}");

		LogFunc.SendLog(KDefine.L_LOG_N_I_SCENE_FAIL, oDataDict);
	}

	/** 보상 광고 로그를 전송한다 */
	public static void Send_I_Ad_Rewerd_View() {
		LogFunc.SendOnceLog(KDefine.L_LOG_N_I_AD_REWARD_VIEW, LogFunc.MakeDefDatas());
	}

	/** 전면 광고 로그를 전송한다 */
	public static void Send_I_Ad_Interstitial_View() {
		LogFunc.SendOnceLog(KDefine.L_LOG_N_I_AD_INTERSTITIAL_VIEW, LogFunc.MakeDefDatas());
	}

	/** 앱 실행 로그를 전송한다 */
	public static void Send_I_App_Open() {
		LogFunc.SendOnceLog(KDefine.L_LOG_N_I_APP_OPEN, LogFunc.MakeDefDatas());
	}

	/** 플레이 타임 클리어 로그를 전송한다 */
	public static void Send_Playtime_Clear(int a_nLevelID, double a_dblPlayTime) {
		var oDataDict = LogFunc.MakeDefDatas();
		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_01, $"{a_nLevelID + KCDefine.B_VAL_1_INT}");
		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_02, string.Format(KCDefine.B_TEXT_FMT_2_REAL, a_dblPlayTime));

		LogFunc.SendLog(KDefine.L_LOG_N_PLAYTIME_CLEAR, oDataDict);
	}

	/** 플레이 타임 실패 로그를 전송한다 */
	public static void Send_Playtime_Fail(int a_nLevelID, double a_dblPlayTime) {
		var oDataDict = LogFunc.MakeDefDatas();
		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_01, $"{a_nLevelID + KCDefine.B_VAL_1_INT}");
		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_02, string.Format(KCDefine.B_TEXT_FMT_2_REAL, a_dblPlayTime));

		LogFunc.SendLog(KDefine.L_LOG_N_PLAYTIME_FAIL, oDataDict);
	}

	public static void Send_C_Scene_Clear(int a_nLevelID, string a_oBtnName) {
		var oDataDict = LogFunc.MakeDefDatas();
		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_01, $"{a_nLevelID + KCDefine.B_VAL_1_INT}");
		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_02, a_oBtnName.ToLower());

		LogFunc.SendLog(KDefine.L_LOG_N_C_SCENE_CLEAR, oDataDict);
	}

	public static void Send_C_Scene_Fail(int a_nLevelID, string a_oBtnName) {
		var oDataDict = LogFunc.MakeDefDatas();
		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_01, $"{a_nLevelID + KCDefine.B_VAL_1_INT}");
		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_02, a_oBtnName.ToLower());

		LogFunc.SendLog(KDefine.L_LOG_N_C_SCENE_FAIL, oDataDict);
	}

	public static void Send_C_Item_Get(int a_nLevelID, string a_oSceneName, STLogItemInfo a_stItemInfo) {
		var oDataDict = LogFunc.MakeDefDatas();
		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_01, $"{a_nLevelID + KCDefine.B_VAL_1_INT}");
		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_02, a_oSceneName.ToLower());
		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_06, $"{a_stItemInfo.m_nNumCoins}");

		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_07, $"{a_stItemInfo.m_nGameItem01}");
		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_08, $"{a_stItemInfo.m_nGameItem02}");
		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_09, $"{a_stItemInfo.m_nGameItem03}");
		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_10, $"{a_stItemInfo.m_nGameItem04}");
		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_11, $"{a_stItemInfo.m_nGameItem05}");

		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_12, $"{a_stItemInfo.m_nBoosterItem01}");
		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_13, $"{a_stItemInfo.m_nBoosterItem02}");
		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_14, $"{a_stItemInfo.m_nBoosterItem03}");

#if DEBUG || DEVELOPMENT_BUILD
		CFunc.ShowLog($"LogFunc.Send_C_Item_Get: {oDataDict.ExToStr(KCDefine.B_TOKEN_COMMA)}");
#endif // #if DEBUG || DEVELOPMENT_BUILD

		LogFunc.SendLog(KDefine.L_LOG_N_ITEM_GET, oDataDict);
	}

	public static void Send_C_Item_Use(int a_nLevelID, string a_oSceneName, STLogItemInfo a_stItemInfo) {
		var oDataDict = LogFunc.MakeDefDatas();
		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_01, $"{a_nLevelID + KCDefine.B_VAL_1_INT}");
		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_02, a_oSceneName.ToLower());
		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_06, $"{a_stItemInfo.m_nNumCoins}");

		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_07, $"{a_stItemInfo.m_nGameItem01}");
		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_08, $"{a_stItemInfo.m_nGameItem02}");
		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_09, $"{a_stItemInfo.m_nGameItem03}");
		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_10, $"{a_stItemInfo.m_nGameItem04}");
		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_11, $"{a_stItemInfo.m_nGameItem05}");

		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_12, $"{a_stItemInfo.m_nBoosterItem01}");
		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_13, $"{a_stItemInfo.m_nBoosterItem02}");
		oDataDict.TryAdd(KDefine.L_LOG_KEY_OPTION_14, $"{a_stItemInfo.m_nBoosterItem03}");

#if DEBUG || DEVELOPMENT_BUILD
		CFunc.ShowLog($"LogFunc.Send_C_Item_Use: {oDataDict.ExToStr(KCDefine.B_TOKEN_COMMA)}");
#endif // #if DEBUG || DEVELOPMENT_BUILD

		LogFunc.SendLog(KDefine.L_LOG_N_ITEM_USE, oDataDict);
	}
	#endregion // 클래스 함수
}

/** 서브 전역 로그 함수 - 팩토리 */
public static partial class LogFunc {
	#region 클래스 함수
	/** 로그 아이템 정보를 생성한다 */
	public static STLogItemInfo MakeLogItemInfo(Dictionary<ulong, STTargetInfo> a_oTargetInfoDict) {
		var stLogItemInfo = new STLogItemInfo();

		foreach(var stKeyVal in a_oTargetInfoDict) {
			bool bIsValid = CItemInfoTable.Inst.TryGetBuyItemTradeInfo((EItemKinds)stKeyVal.Value.m_nKinds, out STItemTradeInfo stItemTradeInfo);

			switch((EItemKinds)stKeyVal.Value.m_nKinds) {
				case EItemKinds.GOODS_RUBY: stLogItemInfo.m_nNumCoins = (int)stKeyVal.Value.m_stValInfo01.m_dmVal; break;

				case EItemKinds.GAME_ITEM_01_EARTHQUAKE: stLogItemInfo.m_nGameItem01 = (int)(stKeyVal.Value.m_stValInfo01.m_dmVal * stItemTradeInfo.m_oPayTargetInfoDict.ExGetTargetVal(ETargetKinds.ITEM_NUMS, (int)EItemKinds.GOODS_RUBY)); break;
				case EItemKinds.GAME_ITEM_02_ADD_BALLS: stLogItemInfo.m_nGameItem02 = (int)(stKeyVal.Value.m_stValInfo01.m_dmVal * stItemTradeInfo.m_oPayTargetInfoDict.ExGetTargetVal(ETargetKinds.ITEM_NUMS, (int)EItemKinds.GOODS_RUBY)); break;
				case EItemKinds.GAME_ITEM_03_BRICKS_DELETE: stLogItemInfo.m_nGameItem03 = (int)(stKeyVal.Value.m_stValInfo01.m_dmVal * stItemTradeInfo.m_oPayTargetInfoDict.ExGetTargetVal(ETargetKinds.ITEM_NUMS, (int)EItemKinds.GOODS_RUBY)); break;
				case EItemKinds.GAME_ITEM_04_ADD_LASER_BRICKS: stLogItemInfo.m_nGameItem04 = (int)(stKeyVal.Value.m_stValInfo01.m_dmVal * stItemTradeInfo.m_oPayTargetInfoDict.ExGetTargetVal(ETargetKinds.ITEM_NUMS, (int)EItemKinds.GOODS_RUBY)); break;
				case EItemKinds.GAME_ITEM_05_ADD_STEEL_BRICKS: stLogItemInfo.m_nGameItem05 = (int)(stKeyVal.Value.m_stValInfo01.m_dmVal * stItemTradeInfo.m_oPayTargetInfoDict.ExGetTargetVal(ETargetKinds.ITEM_NUMS, (int)EItemKinds.GOODS_RUBY)); break;

				case EItemKinds.BOOSTER_ITEM_01_MISSILE: stLogItemInfo.m_nBoosterItem01 = (int)(stKeyVal.Value.m_stValInfo01.m_dmVal * stItemTradeInfo.m_oPayTargetInfoDict.ExGetTargetVal(ETargetKinds.ITEM_NUMS, (int)EItemKinds.GOODS_RUBY)); break;
				case EItemKinds.BOOSTER_ITEM_02_LIGHTNING: stLogItemInfo.m_nBoosterItem02 = (int)(stKeyVal.Value.m_stValInfo01.m_dmVal * stItemTradeInfo.m_oPayTargetInfoDict.ExGetTargetVal(ETargetKinds.ITEM_NUMS, (int)EItemKinds.GOODS_RUBY)); break;
				case EItemKinds.BOOSTER_ITEM_03_BOMB: stLogItemInfo.m_nBoosterItem03 = (int)(stKeyVal.Value.m_stValInfo01.m_dmVal * stItemTradeInfo.m_oPayTargetInfoDict.ExGetTargetVal(ETargetKinds.ITEM_NUMS, (int)EItemKinds.GOODS_RUBY)); break;
			}
		}

		return stLogItemInfo;
	}
	#endregion // 클래스 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE

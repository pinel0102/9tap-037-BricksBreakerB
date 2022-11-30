using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** 문자열 테이블 상수 */
public static partial class KCDefine {
	#region 기본
	// 종료 팝업
	public const string ST_KEY_QUIT_P_MSG = "QUIT_P_MSG";

	// 그만두기 팝업
	public const string ST_KEY_LEAVE_P_MSG = "LEAVE_P_MSG";

	// 업데이트 팝업
	public const string ST_KEY_UPDATE_P_MSG = "UPDATE_P_MSG";

	// 로드 팝업
	public const string ST_KEY_LOAD_P_MSG = "LOAD_P_MSG";

	// 저장 팝업
	public const string ST_KEY_SAVE_P_MSG = "SAVE_P_MSG";

	// 추적 설명 팝업 {
	public const string ST_KEY_TRACKING_DP_TITLE = "TRACKING_DP_TITLE";
	public const string ST_KEY_TRACKING_DP_MSG = "TRACKING_DP_MSG";

	public const string ST_KEY_TRACKING_DP_DESC_MSG_01 = "TRACKING_DP_DESC_MSG_01";
	public const string ST_KEY_TRACKING_DP_DESC_MSG_02 = "TRACKING_DP_DESC_MSG_02";
	public const string ST_KEY_TRACKING_DP_DESC_MSG_03 = "TRACKING_DP_DESC_MSG_03";
	// 추적 설명 팝업 }

	// 시작 씬 관리자
	public const string ST_KEY_START_SM_LOADING_TEXT = "START_SM_LOADING_TEXT";

	// 메인 씬 관리자
	public const string ST_KEY_MAIN_SM_A_SET_TEXT = "MAIN_SM_A_SET_TEXT";
	public const string ST_KEY_MAIN_SM_B_SET_TEXT = "MAIN_SM_B_SET_TEXT";

	// 공용 {
	public const string ST_KEY_C_ON_LOGIN_MSG = "C_ON_LOGIN_MSG";
	public const string ST_KEY_C_ON_LOGIN_FAIL_MSG = "C_ON_LOGIN_FAIL_MSG";

	public const string ST_KEY_C_ON_LOGOUT_MSG = "C_ON_LOGOUT_MSG";
	public const string ST_KEY_C_ON_LOGOUT_FAIL_MSG = "C_ON_LOGOUT_FAIL_MSG";

	public const string ST_KEY_C_ON_LOAD_MSG = "C_ON_LOAD_MSG";
	public const string ST_KEY_C_ON_LOAD_FAIL_MSG = "C_ON_LOAD_FAIL_MSG";

	public const string ST_KEY_C_ON_SAVE_MSG = "C_ON_SAVE_MSG";
	public const string ST_KEY_C_ON_SAVE_FAIL_MSG = "C_ON_SAVE_FAIL_MSG";

	public const string ST_KEY_C_ON_PURCHASE_MSG = "C_ON_PURCHASE_MSG";
	public const string ST_KEY_C_ON_PURCHASE_FAIL_MSG = "C_ON_PURCHASE_FAIL_MSG";

	public const string ST_KEY_C_ON_RESTORE_MSG = "C_ON_RESTORE_MSG";
	public const string ST_KEY_C_ON_RESTORE_FAIL_MSG = "C_ON_RESTORE_FAIL_MSG";

	public const string ST_KEY_C_ON_TABLE_LOAD_MSG = "C_ON_TABLE_LOAD_MSG";
	public const string ST_KEY_C_ON_TABLE_LOAD_FAIL_MSG = "C_ON_TABLE_LOAD_FAIL_MSG";

	public const string ST_KEY_C_OK_TEXT = "C_OK_TEXT";
	public const string ST_KEY_C_CANCEL_TEXT = "C_CANCEL_TEXT";
	public const string ST_KEY_C_AGREE_TEXT = "C_AGREE_TEXT";
	public const string ST_KEY_C_RESULT_TEXT = "C_RESULT_TEXT";
	public const string ST_KEY_C_GET_TEXT = "C_GET_TEXT";
	public const string ST_KEY_C_STORE_TEXT = "C_STORE_TEXT";
	public const string ST_KEY_C_EVENT_TEXT = "C_EVENT_TEXT";
	public const string ST_KEY_C_NEXT_TEXT = "C_NEXT_TEXT";
	public const string ST_KEY_C_HOME_TEXT = "C_HOME_TEXT";
	public const string ST_KEY_C_PLAY_TEXT = "C_PLAY_TEXT";
	public const string ST_KEY_C_RETRY_TEXT = "C_RETRY_TEXT";
	public const string ST_KEY_C_LEAVE_TEXT = "C_LEAVE_TEXT";
	public const string ST_KEY_C_SYNC_TEXT = "C_SYNC_TEXT";
	public const string ST_KEY_C_LOGIN_TEXT = "C_LOGIN_TEXT";
	public const string ST_KEY_C_APPLE_LOGIN_TEXT = "C_APPLE_LOGIN_TEXT";
	public const string ST_KEY_C_FACEBOOK_LOGIN_TEXT = "C_FACEBOOK_LOGIN_TEXT";
	public const string ST_KEY_C_LOGOUT_TEXT = "C_LOGOUT_TEXT";
	public const string ST_KEY_C_DISCONNECT_TEXT = "C_DISCONNECT_TEXT";
	public const string ST_KEY_C_LOAD_TEXT = "C_LOAD_TEXT";
	public const string ST_KEY_C_SAVE_TEXT = "C_SAVE_TEXT";
	public const string ST_KEY_C_CONTINUE_TEXT = "C_CONTINUE_TEXT";
	public const string ST_KEY_C_NOTI_TEXT = "C_NOTI_TEXT";
	public const string ST_KEY_C_REVIEW_TEXT = "C_REVIEW_TEXT";
	public const string ST_KEY_C_SUPPORTS_TEXT = "C_SUPPORTS_TEXT";
	public const string ST_KEY_C_BG_SND_TEXT = "C_BG_SND_TEXT";
	public const string ST_KEY_C_FX_SNDS_TEXT = "C_FX_SNDS_TEXT";
	public const string ST_KEY_C_WATCH_ADS_TEXT = "C_WATCH_ADS_TEXT";
	public const string ST_KEY_C_REMOVE_ADS_TEXT = "C_REMOVE_ADS_TEXT";
	public const string ST_KEY_C_RESTORE_PAYMENT_TEXT = "C_RESTORE_PAYMENT_TEXT";
	public const string ST_KEY_C_LEVEL_TEXT = "C_LEVEL_TEXT";
	public const string ST_KEY_C_STAGE_TEXT = "C_STAGE_TEXT";
	public const string ST_KEY_C_CHAPTER_TEXT = "C_CHAPTER_TEXT";
	public const string ST_KEY_C_SETTINGS_TEXT = "C_SETTINGS_TEXT";
	public const string ST_KEY_C_DAILY_MISSION_TEXT = "C_DAILY_MISSION_TEXT";
	public const string ST_KEY_C_FREE_REWARD_TEXT = "C_FREE_REWARD_TEXT";
	public const string ST_KEY_C_DAILY_REWARD_TEXT = "C_DAILY_REWARD_TEXT";
	public const string ST_KEY_C_COINS_BOX_TEXT = "C_COINS_BOX_TEXT";
	public const string ST_KEY_C_RESUME_TEXT = "C_RESUME_TEXT";
	public const string ST_KEY_C_PAUSE_TEXT = "C_PAUSE_TEXT";
	public const string ST_KEY_C_BEGINNER_PKGS_TEXT = "C_BEGINNER_PKGS_TEXT";
	public const string ST_KEY_C_EXPERT_PKGS_TEXT = "C_EXPERT_PKGS_TEXT";
	public const string ST_KEY_C_PRO_PKGS_TEXT = "C_PRO_PKGS_TEXT";
	public const string ST_KEY_C_LEVEL_NUM_TEXT_FMT = "C_LEVEL_NUM_TEXT_FMT";
	public const string ST_KEY_C_STAGE_NUM_TEXT_FMT = "C_STAGE_NUM_TEXT_FMT";
	public const string ST_KEY_C_CHAPTER_NUM_TEXT_FMT = "C_CHAPTER_NUM_TEXT_FMT";
	public const string ST_KEY_C_LEVEL_PAGE_TEXT_FMT = "C_LEVEL_PAGE_TEXT_FMT";
	public const string ST_KEY_C_STAGE_PAGE_TEXT_FMT = "C_STAGE_PAGE_TEXT_FMT";
	public const string ST_KEY_C_CHAPTER_PAGE_TEXT_FMT = "C_CHAPTER_PAGE_TEXT_FMT";
	// 공용 }
	#endregion // 기본
}

/** 문자열 테이블 상수 - 에디터 */
public static partial class KCDefine {
	#region 기본
	// 에디터 종료 팝업
	public const string ST_KEY_EDITOR_QUIT_P_MSG = "EDITOR_QUIT_P_MSG";

	// 에디터 리셋 팝업
	public const string ST_KEY_EDITOR_RESET_P_MSG = "EDITOR_RESET_P_MSG";

	// 에디터 A 세트 팝업
	public const string ST_KEY_EDITOR_A_SET_P_MSG = "EDITOR_A_SET_P_MSG";

	// 에디터 B 세트 팝업
	public const string ST_KEY_EDITOR_B_SET_P_MSG = "EDITOR_B_SET_P_MSG";

	// 에디터 테이블 로드 팝업
	public const string ST_KEY_EDITOR_TABLE_LP_MSG = "EDITOR_TABLE_LP_MSG";

	// 에디터 레벨 제거 팝업
	public const string ST_KEY_EDITOR_REMOVE_LP_MSG = "EDITOR_REMOVE_LP_MSG";

	// 에디터 스테이지 제거 팝업
	public const string ST_KEY_EDITOR_REMOVE_SP_MSG = "EDITOR_REMOVE_SP_MSG";

	// 에디터 챕터 제거 팝업
	public const string ST_KEY_EDITOR_REMOVE_CP_MSG = "EDITOR_REMOVE_CP_MSG";
	#endregion // 기본
}

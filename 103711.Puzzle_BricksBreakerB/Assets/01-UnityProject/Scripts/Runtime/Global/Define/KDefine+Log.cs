using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 로그 상수 */
public static partial class KDefine {
	#region 기본
	// 단위
	public const int L_TIMES_APP_OPEN = 10;
	public const int L_TIMES_REWARD_LOG_SKIP = 10;
	public const int L_TIMES_FULLSCREEN_LOG_SKIP = 20;

	// 이름 {
	public const string L_LOG_N_AGREE = "Agree";
	public const string L_LOG_N_SPLASH = "Splash";
	public const string L_LOG_N_LAUNCH = "Launch";
	public const string L_LOG_N_PURCHASE = "Purchase";

	public const string L_LOG_N_USER = "user";
	public const string L_LOG_N_I_SCENE_PLAY = "i_scene_play";
	public const string L_LOG_N_I_SCENE_CLEAR = "i_scene_clear";
	public const string L_LOG_N_I_SCENE_FAIL = "i_scene_fail";

	public const string L_LOG_N_I_AD_INTERSTITIAL_VIEW = "i_ad_interstitial_view";
	public const string L_LOG_N_I_AD_REWARD_VIEW = "i_ad_rewerd_view";
	public const string L_LOG_N_I_APP_OPEN = "i_app_open";

	public const string L_LOG_N_PLAYTIME_CLEAR = "playtime_clear";
	public const string L_LOG_N_PLAYTIME_FAIL = "playtime_fail";

	public const string L_LOG_N_C_SCENE_CLEAR = "c_scene_clear";
	public const string L_LOG_N_C_SCENE_FAIL = "c_scene_fail";
	
	public const string L_LOG_N_ITEM_GET = "c_item_get";
	public const string L_LOG_N_ITEM_USE = "c_item_use";

	public const string L_SCENE_N_MAIN = "main";
	public const string L_SCENE_N_PLAY = "play";
	public const string L_SCENE_N_STORE = "store";
	// 이름 }

	// 식별자 {
	public const string L_LOG_KEY_LOG_DATE = "log_date";
	public const string L_LOG_KEY_INSTALL_DATE = "install_date";

	public const string L_LOG_KEY_OPTION_01 = "option1";
	public const string L_LOG_KEY_OPTION_02 = "option2";
	public const string L_LOG_KEY_OPTION_03 = "option3";
	public const string L_LOG_KEY_OPTION_04 = "option4";
	public const string L_LOG_KEY_OPTION_05 = "option5";
	public const string L_LOG_KEY_OPTION_06 = "option6";
	public const string L_LOG_KEY_OPTION_07 = "option7";
	public const string L_LOG_KEY_OPTION_08 = "option8";
	public const string L_LOG_KEY_OPTION_09 = "option9";
	public const string L_LOG_KEY_OPTION_10 = "option10";
	public const string L_LOG_KEY_OPTION_11 = "option11";
	public const string L_LOG_KEY_OPTION_12 = "option12";
	public const string L_LOG_KEY_OPTION_13 = "option13";
	public const string L_LOG_KEY_OPTION_14 = "option14";
	// 식별자 }
	#endregion // 기본
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

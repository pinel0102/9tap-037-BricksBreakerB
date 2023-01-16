using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 전역 상수 */
public static partial class KDefine {
	#region 기본
	// 개수
	public const int G_MAX_NUM_VALS = 10;
	public const int G_MAX_NUM_VAL_INFOS = 10;
	public const int G_MAX_NUM_TARGET_INFOS = 25;

	// 식별자 {
	public const int G_CHARACTER_ID_COMMON = byte.MaxValue;

	public const string G_GOOGLE_SHEET_ID_VER_INFO = "1CRF4YZKkSAajLI9RAZOYgRsgSU_rbkvNIuHQ6leU2WI";
	public const string G_GOOGLE_SHEET_ID_ETC_INFO = "1U-_9w6Miat7BAPS4KURhjAYvdqrRLETFpzp7TkT9gpA";
	public const string G_GOOGLE_SHEET_ID_MISSION_INFO = "1pGsLNHVx9KlXeH9oqWhnT0UL0JBfR-bp2l1YMvqjeyQ";
	public const string G_GOOGLE_SHEET_ID_REWARD_INFO = "1Y4ytle0IPmmg_YVFa_0BYFNUYmSCuxu8eNUWewnS1ow";
	public const string G_GOOGLE_SHEET_ID_RES_INFO = "13d01iYX9LxZAau56ntjwjm91HTIzgsx_u9xPaj2UIcw";
	public const string G_GOOGLE_SHEET_ID_ITEM_INFO = "13FPLZ1K6Izb5_iF8eKTVyNZg5W3tZmyHmh0rn0xoC7g";
	public const string G_GOOGLE_SHEET_ID_SKILL_INFO = "1uGUgLtpe4dumREa7EocGIuL6YyN7Nzr2tyW8oDjmF7E";
	public const string G_GOOGLE_SHEET_ID_OBJ_INFO = "1AazL4SGftcKgRFLuEiIIJS7vkgxXNXr5wMw08lcON8Q";
	public const string G_GOOGLE_SHEET_ID_ABILITY_INFO = "19qOqfIb5u0YQkWgvXR6rAyYgblKz70ke8U_K0HvkL20";
	public const string G_GOOGLE_SHEET_ID_PRODUCT_INFO = "1Ylk-sHMYuqlRvTS3GJZ-6vNZV2UWOlfmZt6P1K79qlo";
	// 식별자 }
	#endregion // 기본

	#region 런타임 상수
	// 버전 {
	public static readonly System.Version G_VER_APP_INFO = new System.Version(1, 0, 0);
	public static readonly System.Version G_VER_GAME_INFO = new System.Version(1, 0, 0);
	public static readonly System.Version G_VER_USER_INFO = new System.Version(1, 0, 0);

	public static readonly System.Version G_VER_CELL_INFO = new System.Version(1, 0, 0);
	public static readonly System.Version G_VER_CLEAR_INFO = new System.Version(1, 0, 0);
	public static readonly System.Version G_VER_LEVEL_INFO = new System.Version(1, 0, 0);

	public static readonly System.Version G_VER_ITEM_TARGET_INFO = new System.Version(1, 0, 0);
	public static readonly System.Version G_VER_SKILL_TARGET_INFO = new System.Version(1, 0, 0);
	public static readonly System.Version G_VER_OBJ_TARGET_INFO = new System.Version(1, 0, 0);
	public static readonly System.Version G_VER_ABILITY_TARGET_INFO = new System.Version(1, 0, 0);

	public static readonly System.Version G_VER_CHARACTER_OBJ_TARGET_INFO = new System.Version(1, 0, 0);
	public static readonly System.Version G_VER_CHARACTER_GAME_INFO = new System.Version(1, 0, 0);
	// 버전 }

	// 경로
	public static readonly string G_DATA_P_APP_INFO = $"{KCDefine.B_DIR_P_WRITABLE}AppInfo.bytes";
	public static readonly string G_DATA_P_USER_INFO = $"{KCDefine.B_DIR_P_WRITABLE}UserInfo.bytes";
	public static readonly string G_DATA_P_GAME_INFO = $"{KCDefine.B_DIR_P_WRITABLE}GameInfo.bytes";

	// 이름
	public static readonly string G_TABLE_N_VER_INFO = KCDefine.U_TABLE_P_G_VER_INFO.ExGetFileName(false);
	public static readonly string G_TABLE_N_ETC_INFO = KCDefine.U_TABLE_P_G_ETC_INFO.ExGetFileName(false);
	public static readonly string G_TABLE_N_MISSION_INFO = KCDefine.U_TABLE_P_G_MISSION_INFO.ExGetFileName(false);
	public static readonly string G_TABLE_N_REWARD_INFO = KCDefine.U_TABLE_P_G_REWARD_INFO.ExGetFileName(false);
	public static readonly string G_TABLE_N_RES_INFO = KCDefine.U_TABLE_P_G_RES_INFO.ExGetFileName(false);
	public static readonly string G_TABLE_N_ITEM_INFO = KCDefine.U_TABLE_P_G_ITEM_INFO.ExGetFileName(false);
	public static readonly string G_TABLE_N_SKILL_INFO = KCDefine.U_TABLE_P_G_SKILL_INFO.ExGetFileName(false);
	public static readonly string G_TABLE_N_OBJ_INFO = KCDefine.U_TABLE_P_G_OBJ_INFO.ExGetFileName(false);
	public static readonly string G_TABLE_N_ABILITY_INFO = KCDefine.U_TABLE_P_G_ABILITY_INFO.ExGetFileName(false);
	public static readonly string G_TABLE_N_PRODUCT_INFO = KCDefine.U_TABLE_P_G_PRODUCT_INFO.ExGetFileName(false);

	// 분석 {
	public static readonly List<EAnalytics> G_ANALYTICS_LOG_ENABLE_LIST = new List<EAnalytics>() {
		EAnalytics.FLURRY, EAnalytics.FIREBASE, EAnalytics.APPS_FLYER, EAnalytics.PLAYFAB
	};

	public static readonly List<EAnalytics> G_ANALYTICS_PURCHASE_LOG_ENABLE_LIST = new List<EAnalytics>() {
		EAnalytics.FLURRY, EAnalytics.FIREBASE, EAnalytics.APPS_FLYER
	};
	// 분석 }

	// 테이블 정보 {
	public static readonly List<STKeyInfo> G_KEY_INFO_GOOGLE_SHEET_COMMON_LIST = new List<STKeyInfo>() {
		new STKeyInfo(KCDefine.U_KEY_NOEX_T, EKeyType.SINGLE),
		new STKeyInfo(KCDefine.U_KEY_NOEX_ST, EKeyType.SINGLE),
		new STKeyInfo(KCDefine.U_KEY_NOEX_KT, EKeyType.SINGLE),
		new STKeyInfo(KCDefine.U_KEY_NOEX_SKT, EKeyType.SINGLE),
		new STKeyInfo(KCDefine.U_KEY_NOEX_DSKT, EKeyType.SINGLE),
		new STKeyInfo(KCDefine.U_KEY_REPLACE, EKeyType.SINGLE),
		new STKeyInfo(KCDefine.U_KEY_FMT_FLAGS, EKeyType.MULTI),
		new STKeyInfo(KCDefine.U_KEY_NAME, EKeyType.SINGLE),
		new STKeyInfo(KCDefine.U_KEY_DESC, EKeyType.SINGLE)
	};

	public static readonly Dictionary<string, int> G_TABLE_INFO_NUM_ROWS_DICT = new Dictionary<string, int>() {
		[KDefine.G_TABLE_N_VER_INFO] = KCDefine.U_MAX_NUM_GOOGLE_SHEET_ROWS,
		[KDefine.G_TABLE_N_ETC_INFO] = KCDefine.U_MAX_NUM_GOOGLE_SHEET_ROWS,
		[KDefine.G_TABLE_N_MISSION_INFO] = KCDefine.U_MAX_NUM_GOOGLE_SHEET_ROWS,
		[KDefine.G_TABLE_N_REWARD_INFO] = KCDefine.U_MAX_NUM_GOOGLE_SHEET_ROWS,
		[KDefine.G_TABLE_N_RES_INFO] = KCDefine.U_MAX_NUM_GOOGLE_SHEET_ROWS,
		[KDefine.G_TABLE_N_ITEM_INFO] = KCDefine.U_MAX_NUM_GOOGLE_SHEET_ROWS,
		[KDefine.G_TABLE_N_SKILL_INFO] = KCDefine.U_MAX_NUM_GOOGLE_SHEET_ROWS,
		[KDefine.G_TABLE_N_OBJ_INFO] = KCDefine.U_MAX_NUM_GOOGLE_SHEET_ROWS,
		[KDefine.G_TABLE_N_ABILITY_INFO] = KCDefine.U_MAX_NUM_GOOGLE_SHEET_ROWS,
		[KDefine.G_TABLE_N_PRODUCT_INFO] = KCDefine.U_MAX_NUM_GOOGLE_SHEET_ROWS
	};

	public static readonly Dictionary<string, STTableInfo> G_TABLE_INFO_GOOGLE_SHEET_DICT = new Dictionary<string, STTableInfo>() {
		[KDefine.G_TABLE_N_VER_INFO] = new STTableInfo(KDefine.G_GOOGLE_SHEET_ID_VER_INFO, KDefine.G_TABLE_N_VER_INFO, new Dictionary<System.Type, Dictionary<string, string>>() {
			// Do Something
		}, new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			// Do Something
		}, new Dictionary<System.Type, Dictionary<string, List<STKeyInfo>>>() {
			// Do Something
		}),

		[KDefine.G_TABLE_N_ETC_INFO] = new STTableInfo(KDefine.G_GOOGLE_SHEET_ID_ETC_INFO, KDefine.G_TABLE_N_ETC_INFO, new Dictionary<System.Type, Dictionary<string, string>>() {
			[typeof(CCalcInfoTable)] = new Dictionary<string, string>() {
				[KCDefine.B_KEY_COMMON] = KCDefine.U_KEY_CALC
			},

			[typeof(CEpisodeInfoTable)] = new Dictionary<string, string>() {
				[KCDefine.U_KEY_LEVEL_EPISODE] = KCDefine.U_KEY_LEVEL_EPISODE,
				[KCDefine.U_KEY_STAGE_EPISODE] = KCDefine.U_KEY_STAGE_EPISODE,
				[KCDefine.U_KEY_CHAPTER_EPISODE] = KCDefine.U_KEY_CHAPTER_EPISODE
			},

			[typeof(CTutorialInfoTable)] = new Dictionary<string, string>() {
				[KCDefine.B_KEY_COMMON] = KCDefine.U_KEY_TUTORIAL
			},

			[typeof(CFXInfoTable)] = new Dictionary<string, string>() {
				[KCDefine.B_KEY_COMMON] = KCDefine.U_KEY_FX
			}
		}, new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			// Do Something
		}, new Dictionary<System.Type, Dictionary<string, List<STKeyInfo>>>() {
			[typeof(CCalcInfoTable)] = new Dictionary<string, List<STKeyInfo>>() {
				[KCDefine.B_KEY_COMMON] = new List<STKeyInfo>() {
					new STKeyInfo(KCDefine.U_KEY_CALC_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_PREV_CALC_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_NEXT_CALC_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_CALC, EKeyType.SINGLE)
				}
			},

			[typeof(CEpisodeInfoTable)] = new Dictionary<string, List<STKeyInfo>>() {
				[KCDefine.U_KEY_LEVEL_EPISODE] = new List<STKeyInfo>() {
					new STKeyInfo(KCDefine.U_KEY_FMT_ID, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_PREV_ID, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_NEXT_ID, EKeyType.MULTI),

					new STKeyInfo(KCDefine.U_KEY_DIFFICULTY, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_EPISODE_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_TUTORIAL_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_NUM_SUB_EPISODES, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_MAX_NUM_ENEMY_OBJS, EKeyType.SINGLE),

					new STKeyInfo(KCDefine.U_KEY_SIZE, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_FMT_REWARD_KINDS, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_RECORD_VAL_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_CLEAR_TARGET_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_UNLOCK_TARGET_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_DROP_ITEM_TARGET_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_ENEMY_OBJ_TARGET_INFO, EKeyType.MULTI)
				},

				[KCDefine.U_KEY_STAGE_EPISODE] = new List<STKeyInfo>() {
					new STKeyInfo(KCDefine.U_KEY_FMT_ID, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_PREV_ID, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_NEXT_ID, EKeyType.MULTI),

					new STKeyInfo(KCDefine.U_KEY_DIFFICULTY, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_EPISODE_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_TUTORIAL_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_NUM_SUB_EPISODES, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_MAX_NUM_ENEMY_OBJS, EKeyType.SINGLE),

					new STKeyInfo(KCDefine.U_KEY_SIZE, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_FMT_REWARD_KINDS, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_RECORD_VAL_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_CLEAR_TARGET_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_UNLOCK_TARGET_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_DROP_ITEM_TARGET_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_ENEMY_OBJ_TARGET_INFO, EKeyType.MULTI)
				},

				[KCDefine.U_KEY_CHAPTER_EPISODE] = new List<STKeyInfo>() {
					new STKeyInfo(KCDefine.U_KEY_FMT_ID, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_PREV_ID, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_NEXT_ID, EKeyType.MULTI),

					new STKeyInfo(KCDefine.U_KEY_DIFFICULTY, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_EPISODE_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_TUTORIAL_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_NUM_SUB_EPISODES, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_MAX_NUM_ENEMY_OBJS, EKeyType.SINGLE),

					new STKeyInfo(KCDefine.U_KEY_SIZE, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_FMT_REWARD_KINDS, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_RECORD_VAL_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_CLEAR_TARGET_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_UNLOCK_TARGET_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_DROP_ITEM_TARGET_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_ENEMY_OBJ_TARGET_INFO, EKeyType.MULTI)
				},
			},

			[typeof(CTutorialInfoTable)] = new Dictionary<string, List<STKeyInfo>>() {
				[KCDefine.B_KEY_COMMON] = new List<STKeyInfo>() {
					new STKeyInfo(KCDefine.U_KEY_TUTORIAL_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_PREV_TUTORIAL_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_NEXT_TUTORIAL_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_FMT_REWARD_KINDS, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_STRS, EKeyType.MULTI)
				}
			},

			[typeof(CFXInfoTable)] = new Dictionary<string, List<STKeyInfo>>() {
				[KCDefine.B_KEY_COMMON] = new List<STKeyInfo>() {
					new STKeyInfo(KCDefine.U_KEY_FX_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_PREV_FX_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_NEXT_FX_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_TIME_INFO, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_FMT_RES_KINDS, EKeyType.MULTI)
				}
			}
		}),

		[KDefine.G_TABLE_N_MISSION_INFO] = new STTableInfo(KDefine.G_GOOGLE_SHEET_ID_MISSION_INFO, KDefine.G_TABLE_N_MISSION_INFO, new Dictionary<System.Type, Dictionary<string, string>>() {
			[typeof(CMissionInfoTable)] = new Dictionary<string, string>() {
				[KCDefine.B_KEY_COMMON] = KCDefine.B_KEY_COMMON
			}
		}, new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			// Do Something
		}, new Dictionary<System.Type, Dictionary<string, List<STKeyInfo>>>() {
			[typeof(CMissionInfoTable)] = new Dictionary<string, List<STKeyInfo>>() {
				[KCDefine.B_KEY_COMMON] = new List<STKeyInfo>() {
					new STKeyInfo(KCDefine.U_KEY_MISSION_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_PREV_MISSION_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_NEXT_MISSION_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_FMT_REWARD_KINDS, EKeyType.MULTI)
				}
			}
		}),

		[KDefine.G_TABLE_N_REWARD_INFO] = new STTableInfo(KDefine.G_GOOGLE_SHEET_ID_REWARD_INFO, KDefine.G_TABLE_N_REWARD_INFO, new Dictionary<System.Type, Dictionary<string, string>>() {
			[typeof(CRewardInfoTable)] = new Dictionary<string, string>() {
				[KCDefine.B_KEY_COMMON] = KCDefine.B_KEY_COMMON
			}
		}, new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			// Do Something
		}, new Dictionary<System.Type, Dictionary<string, List<STKeyInfo>>>() {
			[typeof(CRewardInfoTable)] = new Dictionary<string, List<STKeyInfo>>() {
				[KCDefine.B_KEY_COMMON] = new List<STKeyInfo>() {
					new STKeyInfo(KCDefine.U_KEY_REWARD_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_PREV_REWARD_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_NEXT_REWARD_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_REWARD_QUALITY, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, EKeyType.MULTI)
				}
			}
		}),

		[KDefine.G_TABLE_N_RES_INFO] = new STTableInfo(KDefine.G_GOOGLE_SHEET_ID_RES_INFO, KDefine.G_TABLE_N_RES_INFO, new Dictionary<System.Type, Dictionary<string, string>>() {
			[typeof(CResInfoTable)] = new Dictionary<string, string>() {
				[KCDefine.B_KEY_COMMON] = KCDefine.B_KEY_COMMON
			}
		}, new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			// Do Something
		}, new Dictionary<System.Type, Dictionary<string, List<STKeyInfo>>>() {
			[typeof(CResInfoTable)] = new Dictionary<string, List<STKeyInfo>>() {
				[KCDefine.B_KEY_COMMON] = new List<STKeyInfo>() {
					new STKeyInfo(KCDefine.U_KEY_RES_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_PREV_RES_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_NEXT_RES_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_RATE, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_RES_PATH, EKeyType.SINGLE)
				}
			}
		}),

		[KDefine.G_TABLE_N_ITEM_INFO] = new STTableInfo(KDefine.G_GOOGLE_SHEET_ID_ITEM_INFO, KDefine.G_TABLE_N_ITEM_INFO, new Dictionary<System.Type, Dictionary<string, string>>() {
			[typeof(CItemInfoTable)] = new Dictionary<string, string>() {
				[KCDefine.B_KEY_COMMON] = KCDefine.B_KEY_COMMON,
				[KCDefine.B_KEY_BUY_TRADE] = KCDefine.B_KEY_BUY_TRADE,
				[KCDefine.B_KEY_SALE_TRADE] = KCDefine.B_KEY_SALE_TRADE,
				[KCDefine.B_KEY_ENHANCE_TRADE] = KCDefine.B_KEY_ENHANCE_TRADE
			}
		}, new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			// Do Something
		}, new Dictionary<System.Type, Dictionary<string, List<STKeyInfo>>>() {
			[typeof(CItemInfoTable)] = new Dictionary<string, List<STKeyInfo>>() {
				[KCDefine.B_KEY_COMMON] = new List<STKeyInfo>() {
					new STKeyInfo(KCDefine.U_KEY_ITEM_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_PREV_ITEM_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_NEXT_ITEM_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_FMT_ATTACH_ITEM_TARGET_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_SKILL_TARGET_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_ABILITY_TARGET_INFO, EKeyType.MULTI)
				},

				[KCDefine.B_KEY_BUY_TRADE] = new List<STKeyInfo>() {
					new STKeyInfo(KCDefine.U_KEY_ITEM_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_PREV_ITEM_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_NEXT_ITEM_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_FMT_PAY_TARGET_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, EKeyType.MULTI)
				},

				[KCDefine.B_KEY_SALE_TRADE] = new List<STKeyInfo>() {
					new STKeyInfo(KCDefine.U_KEY_ITEM_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_PREV_ITEM_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_NEXT_ITEM_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_FMT_PAY_TARGET_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, EKeyType.MULTI)
				},

				[KCDefine.B_KEY_ENHANCE_TRADE] = new List<STKeyInfo>() {
					new STKeyInfo(KCDefine.U_KEY_ITEM_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_PREV_ITEM_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_NEXT_ITEM_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_FMT_PAY_TARGET_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, EKeyType.MULTI)
				}
			}
		}),

		[KDefine.G_TABLE_N_SKILL_INFO] = new STTableInfo(KDefine.G_GOOGLE_SHEET_ID_SKILL_INFO, KDefine.G_TABLE_N_SKILL_INFO, new Dictionary<System.Type, Dictionary<string, string>>() {
			[typeof(CSkillInfoTable)] = new Dictionary<string, string>() {
				[KCDefine.B_KEY_COMMON] = KCDefine.B_KEY_COMMON,
				[KCDefine.B_KEY_BUY_TRADE] = KCDefine.B_KEY_BUY_TRADE,
				[KCDefine.B_KEY_SALE_TRADE] = KCDefine.B_KEY_SALE_TRADE,
				[KCDefine.B_KEY_ENHANCE_TRADE] = KCDefine.B_KEY_ENHANCE_TRADE
			}
		}, new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			// Do Something
		}, new Dictionary<System.Type, Dictionary<string, List<STKeyInfo>>>() {
			[typeof(CSkillInfoTable)] = new Dictionary<string, List<STKeyInfo>>() {
				[KCDefine.B_KEY_COMMON] = new List<STKeyInfo>() {
					new STKeyInfo(KCDefine.U_KEY_ITEM_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_PREV_ITEM_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_NEXT_ITEM_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_SKILL_APPLY_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_MAX_APPLY_TIMES, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_TIME_INFO, EKeyType.SINGLE),

					new STKeyInfo(KCDefine.U_KEY_FMT_FX_KINDS, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_RES_KINDS, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_ABILITY_TARGET_INFO, EKeyType.MULTI)
				},

				[KCDefine.B_KEY_BUY_TRADE] = new List<STKeyInfo>() {
					new STKeyInfo(KCDefine.U_KEY_SKILL_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_PREV_SKILL_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_NEXT_SKILL_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_FMT_PAY_TARGET_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, EKeyType.MULTI)
				},

				[KCDefine.B_KEY_SALE_TRADE] = new List<STKeyInfo>() {
					new STKeyInfo(KCDefine.U_KEY_SKILL_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_PREV_SKILL_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_NEXT_SKILL_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_FMT_PAY_TARGET_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, EKeyType.MULTI)
				},

				[KCDefine.B_KEY_ENHANCE_TRADE] = new List<STKeyInfo>() {
					new STKeyInfo(KCDefine.U_KEY_SKILL_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_PREV_SKILL_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_NEXT_SKILL_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_FMT_PAY_TARGET_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, EKeyType.MULTI)
				}
			}
		}),

		[KDefine.G_TABLE_N_OBJ_INFO] = new STTableInfo(KDefine.G_GOOGLE_SHEET_ID_OBJ_INFO, KDefine.G_TABLE_N_OBJ_INFO, new Dictionary<System.Type, Dictionary<string, string>>() {
			[typeof(CObjInfoTable)] = new Dictionary<string, string>() {
				[KCDefine.B_KEY_COMMON] = KCDefine.B_KEY_COMMON,
				[KCDefine.B_KEY_BUY_TRADE] = KCDefine.B_KEY_BUY_TRADE,
				[KCDefine.B_KEY_SALE_TRADE] = KCDefine.B_KEY_SALE_TRADE,
				[KCDefine.B_KEY_ENHANCE_TRADE] = KCDefine.B_KEY_ENHANCE_TRADE
			}
		}, new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			// Do Something
		}, new Dictionary<System.Type, Dictionary<string, List<STKeyInfo>>>() {
			[typeof(CObjInfoTable)] = new Dictionary<string, List<STKeyInfo>>() {
				[KCDefine.B_KEY_COMMON] = new List<STKeyInfo>() {
					new STKeyInfo(KCDefine.U_KEY_OBJ_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_PREV_OBJ_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_NEXT_OBJ_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_ACTION_SKILL_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_SIZE, EKeyType.SINGLE),

					new STKeyInfo(KCDefine.U_KEY_FMT_RES_KINDS, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_DROP_ITEM_TARGET_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_EQUIP_ITEM_TARGET_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_SKILL_TARGET_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_ABILITY_TARGET_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, EKeyType.MULTI)
				},

				[KCDefine.B_KEY_BUY_TRADE] = new List<STKeyInfo>() {
					new STKeyInfo(KCDefine.U_KEY_OBJ_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_PREV_OBJ_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_NEXT_OBJ_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_FMT_PAY_TARGET_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, EKeyType.MULTI)
				},

				[KCDefine.B_KEY_SALE_TRADE] = new List<STKeyInfo>() {
					new STKeyInfo(KCDefine.U_KEY_OBJ_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_PREV_OBJ_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_NEXT_OBJ_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_FMT_PAY_TARGET_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, EKeyType.MULTI)
				},

				[KCDefine.B_KEY_ENHANCE_TRADE] = new List<STKeyInfo>() {
					new STKeyInfo(KCDefine.U_KEY_OBJ_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_PREV_OBJ_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_NEXT_OBJ_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_FMT_PAY_TARGET_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, EKeyType.MULTI)
				}
			}
		}),

		[KDefine.G_TABLE_N_ABILITY_INFO] = new STTableInfo(KDefine.G_GOOGLE_SHEET_ID_ABILITY_INFO, KDefine.G_TABLE_N_ABILITY_INFO, new Dictionary<System.Type, Dictionary<string, string>>() {
			[typeof(CAbilityInfoTable)] = new Dictionary<string, string>() {
				[KCDefine.B_KEY_COMMON] = KCDefine.B_KEY_COMMON,
				[KCDefine.B_KEY_ENHANCE_TRADE] = KCDefine.B_KEY_ENHANCE_TRADE
			}
		}, new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			// Do Something
		}, new Dictionary<System.Type, Dictionary<string, List<STKeyInfo>>>() {
			[typeof(CAbilityInfoTable)] = new Dictionary<string, List<STKeyInfo>>() {
				[KCDefine.B_KEY_COMMON] = new List<STKeyInfo>() {
					new STKeyInfo(KCDefine.U_KEY_ABILITY_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_PREV_ABILITY_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_NEXT_ABILITY_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_ABILITY_VAL_TYPE, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_VAL_INFO, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_FMT_EXTRA_ABILITY_TARGET_INFO, EKeyType.MULTI)
				},

				[KCDefine.B_KEY_BUY_TRADE] = new List<STKeyInfo>() {
					new STKeyInfo(KCDefine.U_KEY_ABILITY_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_PREV_ABILITY_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_NEXT_ABILITY_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_FMT_PAY_TARGET_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, EKeyType.MULTI)
				},

				[KCDefine.B_KEY_SALE_TRADE] = new List<STKeyInfo>() {
					new STKeyInfo(KCDefine.U_KEY_ABILITY_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_PREV_ABILITY_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_NEXT_ABILITY_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_FMT_PAY_TARGET_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, EKeyType.MULTI)
				},

				[KCDefine.B_KEY_ENHANCE_TRADE] = new List<STKeyInfo>() {
					new STKeyInfo(KCDefine.U_KEY_ABILITY_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_PREV_ABILITY_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_NEXT_ABILITY_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_FMT_PAY_TARGET_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, EKeyType.MULTI)
				}
			}
		}),

		[KDefine.G_TABLE_N_PRODUCT_INFO] = new STTableInfo(KDefine.G_GOOGLE_SHEET_ID_PRODUCT_INFO, KDefine.G_TABLE_N_PRODUCT_INFO, new Dictionary<System.Type, Dictionary<string, string>>() {
			[typeof(CProductTradeInfoTable)] = new Dictionary<string, string>() {
				[KCDefine.B_KEY_COMMON] = KCDefine.B_KEY_BUY_TRADE
			}
		}, new Dictionary<System.Type, Dictionary<string, List<string>>>() {
			[typeof(CProductTradeInfoTable)] = new Dictionary<string, List<string>>() {
				[KCDefine.B_KEY_COMMON] = new List<string>() {
					KCDefine.B_KEY_COMMON, KCDefine.B_PLATFORM_N_IOS_APPLE, KCDefine.B_PLATFORM_N_ANDROID_GOOGLE, KCDefine.B_PLATFORM_N_ANDROID_AMAZON
				}
			}
		}, new Dictionary<System.Type, Dictionary<string, List<STKeyInfo>>>() {
			[typeof(CProductTradeInfoTable)] = new Dictionary<string, List<STKeyInfo>>() {
				[KCDefine.B_KEY_COMMON] = new List<STKeyInfo>() {
					new STKeyInfo(KCDefine.U_KEY_PRODUCT_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_PREV_PRODUCT_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_NEXT_PRODUCT_KINDS, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_PRODUCT_IDX, EKeyType.SINGLE),
					new STKeyInfo(KCDefine.U_KEY_FMT_PAY_TARGET_INFO, EKeyType.MULTI),
					new STKeyInfo(KCDefine.U_KEY_FMT_ACQUIRE_TARGET_INFO, EKeyType.MULTI)
				}
			}
		})
	};
	// 테이블 정보 }
	#endregion // 런타임 상수
}

/** 초기화 씬 상수 */
public static partial class KDefine {
	#region 기본

	#endregion // 기본

	#region 런타임 상수
	// 색상
	public static readonly Color IS_COLOR_CLEAR = new Color(0x29 / (float)KCDefine.B_UNIT_NORM_VAL_TO_BYTE, 0x4c / (float)KCDefine.B_UNIT_NORM_VAL_TO_BYTE, 0x94 / (float)KCDefine.B_UNIT_NORM_VAL_TO_BYTE, 1.0f);
	#endregion // 런타임 상수
}

/** 시작 씬 상수 */
public static partial class KDefine {
	#region 기본

	#endregion // 기본

	#region 런타임 상수
	// 위치
	public static readonly Vector3 SS_POS_LOADING_TEXT = new Vector3(0.0f, 70.0f, 0.0f);
	public static readonly Vector3 SS_POS_LOADING_GAUGE = new Vector3(0.0f, -35.0f, 0.0f);
	#endregion // 런타임 상수
}

/** 설정 씬 상수 */
public static partial class KDefine {
	#region 기본

	#endregion // 기본
}

/** 약관 동의 씬 상수 */
public static partial class KDefine {
	#region 기본

	#endregion // 기본
}

/** 지연 설정 씬 상수 */
public static partial class KDefine {
	#region 기본

	#endregion // 기본
}

/** 타이틀 씬 상수 */
public static partial class KDefine {
	#region 기본

	#endregion // 기본
}

/** 메인 씬 상수 */
public static partial class KDefine {
	#region 기본

	#endregion // 기본
}

/** 게임 씬 상수 */
public static partial class KDefine {
	#region 기본
	// 이름
	public const string GS_OBJ_N_ENGINE = "Engine";
	#endregion // 기본
}

/** 로딩 씬 상수 */
public static partial class KDefine {
	#region 기본

	#endregion // 기본

	#region 런타임 상수
	// 위치
	public static readonly Vector3 LS_POS_LOADING_TEXT = new Vector3(0.0f, 70.0f, 0.0f);
	public static readonly Vector3 LS_POS_LOADING_GAUGE = new Vector3(0.0f, -35.0f, 0.0f);
	#endregion // 런타임 상수
}

/** 중첩 씬 상수 */
public static partial class KDefine {
	#region 기본

	#endregion // 기본
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

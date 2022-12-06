#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#if PURCHASE_MODULE_ENABLE
using UnityEngine.Purchasing;
#endif // #if PURCHASE_MODULE_ENABLE

/** 전역 함수 */
public static partial class Func {
	#region 클래스 함수
	/** 어빌리티 값을 설정한다 */
	public static void SetupAbilityVals(STItemInfo a_stItemInfo, CItemTargetInfo a_oItemTargetInfo, Dictionary<EAbilityKinds, decimal> a_oOutAbilityValDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_stItemInfo.m_eItemKinds != EItemKinds.NONE && a_oOutAbilityValDict != null));

		// 어빌리티 값 설정이 가능 할 경우
		if(a_stItemInfo.m_eItemKinds != EItemKinds.NONE && a_oOutAbilityValDict != null) {
			Func.SetupAbilityVals(a_stItemInfo.m_oAbilityTargetInfoDict, a_oOutAbilityValDict, a_bIsEnableAssert);
			Func.SetupAbilityVals(a_oItemTargetInfo, a_oOutAbilityValDict, false);
		}
	}

	/** 어빌리티 값을 설정한다 */
	public static void SetupAbilityVals(STSkillInfo a_stSkillInfo, CSkillTargetInfo a_oSkillTargetInfo, Dictionary<EAbilityKinds, decimal> a_oOutAbilityValDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_stSkillInfo.m_eSkillKinds != ESkillKinds.NONE && a_oOutAbilityValDict != null));

		// 어빌리티 값 설정이 가능 할 경우
		if(a_stSkillInfo.m_eSkillKinds != ESkillKinds.NONE && a_oOutAbilityValDict != null) {
			Func.SetupAbilityVals(a_stSkillInfo.m_oAbilityTargetInfoDict, a_oOutAbilityValDict, a_bIsEnableAssert);
			Func.SetupAbilityVals(a_oSkillTargetInfo, a_oOutAbilityValDict, false);
		}
	}

	/** 어빌리티 값을 설정한다 */
	public static void SetupAbilityVals(STObjInfo a_stObjInfo, CObjTargetInfo a_oObjTargetInfo, Dictionary<EAbilityKinds, decimal> a_oOutAbilityValDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_stObjInfo.m_eObjKinds != EObjKinds.NONE && a_oOutAbilityValDict != null));

		// 어빌리티 값 설정이 가능 할 경우
		if(a_stObjInfo.m_eObjKinds != EObjKinds.NONE && a_oOutAbilityValDict != null) {
			Func.SetupAbilityVals(a_stObjInfo.m_oAbilityTargetInfoDict, a_oOutAbilityValDict, a_bIsEnableAssert);
			Func.SetupAbilityVals(a_oObjTargetInfo, a_oOutAbilityValDict, false);
		}
	}

	/** 어빌리티 값을 설정한다 */
	public static void SetupAbilityVals(Dictionary<EAbilityKinds, decimal> a_oAbilityValDict, Dictionary<EAbilityKinds, decimal> a_oOutAbilityValDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oAbilityValDict != null && a_oOutAbilityValDict != null));

		// 어빌리티 값 설정이 가능 할 경우
		if(a_oAbilityValDict != null && a_oOutAbilityValDict != null) {
			foreach(var stKeyVal in a_oAbilityValDict) {
				a_oOutAbilityValDict.ExIncrAbilityVal(stKeyVal.Key, stKeyVal.Value);
			}
		}
	}

	/** 어빌리티 값을 설정한다 */
	public static void SetupAbilityVals(Dictionary<ulong, STTargetInfo> a_oAbilityTargetInfoDict, Dictionary<EAbilityKinds, decimal> a_oOutAbilityValDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oAbilityTargetInfoDict != null && a_oOutAbilityValDict != null));

		// 어빌리티 값 설정이 가능 할 경우
		if(a_oAbilityTargetInfoDict != null && a_oOutAbilityValDict != null) {
			foreach(var stKeyVal in a_oAbilityTargetInfoDict) {
				Func.SetupAbilityVals(stKeyVal.Value, a_oOutAbilityValDict);
			}
		}
	}

	/** 플레이 에피소드 정보를 설정한다 */
	public static void SetupPlayEpisodeInfo(int a_nCharacterID, int a_nLevelID, EPlayMode a_ePlayMode, int a_nStageID = KCDefine.B_VAL_0_INT, int a_nChapterID = KCDefine.B_VAL_0_INT) {
		CGameInfoStorage.Inst.SetPlayMode(a_ePlayMode);
		CGameInfoStorage.Inst.SetPlayEpisodeInfo(Access.GetEpisodeInfo(a_nLevelID, a_nStageID, a_nChapterID));

		CUserInfoStorage.Inst.GetCharacterUserInfo(a_nCharacterID).m_stPlayEpisodeIDInfo = Access.GetEpisodeInfo(a_nLevelID, a_nStageID, a_nChapterID).m_stIDInfo;
		CUserInfoStorage.Inst.SaveUserInfo();

#if(UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
		CGameInfoStorage.Inst.SetPlayLevelInfo(CLevelInfoTable.Inst.GetLevelInfo(a_nLevelID, a_nStageID, a_nChapterID));
#else
		CGameInfoStorage.Inst.SetPlayLevelInfo(CLevelInfoTable.Inst.LoadLevelInfo(a_nLevelID, a_nStageID, a_nChapterID));
#endif // #if (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
	}

	/** 다음 일일 보상 식별자를 설정한다 */
	public static void SetupNextDailyRewardID(int a_nCharacterID, bool a_bIsResetDailyRewardTime = true, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out CCharacterGameInfo oCharacterGameInfo));

		// 캐릭터 정보가 존재 할 경우
		if(CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out oCharacterGameInfo)) {
			// 일일 보상 시간 리셋 모드 일 경우
			if(a_bIsResetDailyRewardTime) {
				oCharacterGameInfo.PrevDailyRewardTime = System.DateTime.Today;
			}

			oCharacterGameInfo.DailyRewardID = (oCharacterGameInfo.DailyRewardID + KCDefine.B_VAL_1_INT) % KDefine.G_REWARDS_KINDS_DAILY_REWARD_LIST.Count;
		}
	}

	/** 무료 보상 획득 횟수를 증가시킨다 */
	public static void IncrFreeRewardAcquireTimes(int a_nCharacterID, int a_nRewardTimes, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out CCharacterGameInfo oCharacterGameInfo));

		// 캐릭터 게임 정보가 존재 할 경우
		if(CGameInfoStorage.Inst.TryGetCharacterGameInfo(a_nCharacterID, out oCharacterGameInfo)) {
			oCharacterGameInfo.FreeRewardAcquireTimes = Mathf.Clamp(oCharacterGameInfo.FreeRewardAcquireTimes + a_nRewardTimes, KCDefine.B_VAL_0_INT, KDefine.G_MAX_TIMES_ACQUIRE_FREE_REWARDS);
		}
	}

	/** 지불한다 */
	public static void Pay(int a_nCharacterID, STTargetInfo a_stTargetInfo, bool a_bIsEnableAssert = true) {
		switch(a_stTargetInfo.TargetType) {
			case ETargetType.ITEM: Func.PayItemTarget(a_nCharacterID, a_stTargetInfo, Access.GetItemTargetInfo(a_nCharacterID, (EItemKinds)a_stTargetInfo.Kinds), a_bIsEnableAssert); break;
			case ETargetType.SKILL: Func.PaySkillTarget(a_nCharacterID, a_stTargetInfo, Access.GetSkillTargetInfo(a_nCharacterID, (ESkillKinds)a_stTargetInfo.Kinds), a_bIsEnableAssert); break;
			case ETargetType.OBJ: Func.PayObjTarget(a_nCharacterID, a_stTargetInfo, Access.GetObjTargetInfo(a_nCharacterID, (EObjKinds)a_stTargetInfo.Kinds), a_bIsEnableAssert); break;
			case ETargetType.ABILITY: Func.PayAbilityTarget(a_nCharacterID, a_stTargetInfo, Access.GetAbilityTargetInfo(a_nCharacterID, (EAbilityKinds)a_stTargetInfo.Kinds), a_bIsEnableAssert); break;
		}
	}

	/** 지불한다 */
	public static void Pay(int a_nCharacterID, STTargetInfo a_stTargetInfo, CTargetInfo a_oTargetInfo, bool a_bIsEnableAssert = true) {
		// 어빌리티 타겟 정보가 아닐 경우
		if(a_stTargetInfo.m_eTargetKinds != ETargetKinds.ABILITY) {
			Func.Pay(a_nCharacterID, a_stTargetInfo, a_bIsEnableAssert);
		} else {
			bool bIsValid = CAbilityInfoTable.Inst.TryGetAbilityInfo((EAbilityKinds)a_stTargetInfo.Kinds, out STAbilityInfo stAbilityInfo);
			CAccess.Assert(!a_bIsEnableAssert || (bIsValid && a_oTargetInfo != null));

			// 타겟 정보가 존재 할 경우
			if(bIsValid && a_oTargetInfo != null && (a_stTargetInfo.Kinds != (int)EAbilityKinds.STAT_EXP && a_stTargetInfo.Kinds != (int)EAbilityKinds.STAT_NUMS)) {
				a_oTargetInfo.m_oAbilityTargetInfoDict.ExIncrTargetVal(a_stTargetInfo.m_eTargetKinds, a_stTargetInfo.Kinds, -a_stTargetInfo.m_stValInfo01.m_dmVal, a_bIsEnableAssert);
			}
		}
	}

	/** 지불한다 */
	public static void Pay(int a_nCharacterID, Dictionary<ulong, STTargetInfo> a_oTargetInfoDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oTargetInfoDict != null);

		// 타겟 정보가 존재 할 경우
		if(a_oTargetInfoDict != null) {
			foreach(var stKeyVal in a_oTargetInfoDict) {
				Func.Pay(a_nCharacterID, stKeyVal.Value, a_bIsEnableAssert);
			}
		}
	}

	/** 지불한다 */
	public static void Pay(int a_nCharacterID, Dictionary<ulong, STTargetInfo> a_oTargetInfoDict, CTargetInfo a_oTargetInfo, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oTargetInfoDict != null);

		// 타겟 정보가 존재 할 경우
		if(a_oTargetInfoDict != null) {
			foreach(var stKeyVal in a_oTargetInfoDict) {
				Func.Pay(a_nCharacterID, stKeyVal.Value, a_oTargetInfo, a_bIsEnableAssert);
			}
		}
	}

	/** 획득한다 */
	public static void Acquire(int a_nCharacterID, STTargetInfo a_stTargetInfo, bool a_bIsAutoCreate = true, bool a_bIsEnableAssert = true) {
		switch(a_stTargetInfo.TargetType) {
			case ETargetType.ITEM: Func.AcquireItemTarget(a_nCharacterID, a_stTargetInfo, Access.GetItemTargetInfo(a_nCharacterID, (EItemKinds)a_stTargetInfo.Kinds, a_bIsAutoCreate), a_bIsEnableAssert); break;
			case ETargetType.SKILL: Func.AcquireSkillTarget(a_nCharacterID, a_stTargetInfo, Access.GetSkillTargetInfo(a_nCharacterID, (ESkillKinds)a_stTargetInfo.Kinds, a_bIsAutoCreate), a_bIsEnableAssert); break;
			case ETargetType.OBJ: Func.AcquireObjTarget(a_nCharacterID, a_stTargetInfo, Access.GetObjTargetInfo(a_nCharacterID, (EObjKinds)a_stTargetInfo.Kinds, a_bIsAutoCreate), a_bIsEnableAssert); break;
			case ETargetType.ABILITY: Func.AcquireAbilityTarget(a_nCharacterID, a_stTargetInfo, Access.GetAbilityTargetInfo(a_nCharacterID, (EAbilityKinds)a_stTargetInfo.Kinds, a_bIsAutoCreate), a_bIsEnableAssert); break;
		}
	}

	/** 획득한다 */
	public static void Acquire(int a_nCharacterID, STTargetInfo a_stTargetInfo, CTargetInfo a_oTargetInfo, bool a_bIsAutoCreate = true, bool a_bIsEnableAssert = true) {
		// 어빌리티 타겟 정보가 아닐 경우
		if(a_stTargetInfo.m_eTargetKinds != ETargetKinds.ABILITY) {
			Func.Acquire(a_nCharacterID, a_stTargetInfo, a_bIsAutoCreate, a_bIsEnableAssert);
		} else {
			bool bIsValid = CAbilityInfoTable.Inst.TryGetAbilityInfo((EAbilityKinds)a_stTargetInfo.Kinds, out STAbilityInfo stAbilityInfo);
			CAccess.Assert(!a_bIsEnableAssert || (bIsValid && a_oTargetInfo != null));

			// 타겟 정보가 존재 할 경우
			if(bIsValid && a_oTargetInfo != null && a_stTargetInfo.Kinds != (int)EAbilityKinds.STAT_NUMS) {
				a_oTargetInfo.m_oAbilityTargetInfoDict.ExIncrTargetVal(a_stTargetInfo.m_eTargetKinds, a_stTargetInfo.Kinds, a_stTargetInfo.m_stValInfo01.m_dmVal, a_bIsEnableAssert);
			}
		}
	}

	/** 획득한다 */
	public static void Acquire(int a_nCharacterID, Dictionary<ulong, STTargetInfo> a_oTargetInfoDict, bool a_bIsAutoCreate = true, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oTargetInfoDict != null);

		// 타겟 정보가 존재 할 경우
		if(a_oTargetInfoDict != null) {
			foreach(var stKeyVal in a_oTargetInfoDict) {
				Func.Acquire(a_nCharacterID, stKeyVal.Value, a_bIsAutoCreate, a_bIsEnableAssert);
			}
		}
	}

	/** 획득한다 */
	public static void Acquire(int a_nCharacterID, Dictionary<ulong, STTargetInfo> a_oTargetInfoDict, CTargetInfo a_oTargetInfo, bool a_bIsAutoCreate = true, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oTargetInfoDict != null);

		// 타겟 정보가 존재 할 경우
		if(a_oTargetInfoDict != null) {
			foreach(var stKeyVal in a_oTargetInfoDict) {
				Func.Acquire(a_nCharacterID, stKeyVal.Value, a_oTargetInfo, a_bIsAutoCreate, a_bIsEnableAssert);
			}
		}
	}

	/** 교환한다 */
	public static void Trade(int a_nCharacterID, STItemTradeInfo a_stItemTradeInfo, bool a_bIsAutoCreate = true, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || Access.IsEnableTrade(a_nCharacterID, a_stItemTradeInfo.m_oPayTargetInfoDict));

		// 교환 가능 할 경우
		if(Access.IsEnableTrade(a_nCharacterID, a_stItemTradeInfo.m_oPayTargetInfoDict)) {
			Func.Pay(a_nCharacterID, a_stItemTradeInfo.m_oPayTargetInfoDict, a_bIsEnableAssert);
			Func.Acquire(a_nCharacterID, a_stItemTradeInfo.m_oAcquireTargetInfoDict, a_bIsAutoCreate, a_bIsEnableAssert);
		}
	}

	/** 교환한다 */
	public static void Trade(int a_nCharacterID, STItemTradeInfo a_stItemTradeInfo, CTargetInfo a_oTargetInfo, bool a_bIsAutoCreate = true, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || Access.IsEnableTrade(a_nCharacterID, a_stItemTradeInfo.m_oPayTargetInfoDict, a_oTargetInfo));

		// 교환 가능 할 경우
		if(Access.IsEnableTrade(a_nCharacterID, a_stItemTradeInfo.m_oPayTargetInfoDict, a_oTargetInfo)) {
			Func.Pay(a_nCharacterID, a_stItemTradeInfo.m_oPayTargetInfoDict, a_oTargetInfo, a_bIsEnableAssert);
			Func.Acquire(a_nCharacterID, a_stItemTradeInfo.m_oAcquireTargetInfoDict, a_oTargetInfo, a_bIsAutoCreate, a_bIsEnableAssert);
		}
	}

	/** 교환한다 */
	public static void Trade(int a_nCharacterID, STSkillTradeInfo a_stSkillTradeInfo, bool a_bIsAutoCreate = true, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || Access.IsEnableTrade(a_nCharacterID, a_stSkillTradeInfo.m_oPayTargetInfoDict));

		// 교환 가능 할 경우
		if(Access.IsEnableTrade(a_nCharacterID, a_stSkillTradeInfo.m_oPayTargetInfoDict)) {
			Func.Pay(a_nCharacterID, a_stSkillTradeInfo.m_oPayTargetInfoDict, a_bIsEnableAssert);
			Func.Acquire(a_nCharacterID, a_stSkillTradeInfo.m_oAcquireTargetInfoDict, a_bIsAutoCreate, a_bIsEnableAssert);
		}
	}

	/** 교환한다 */
	public static void Trade(int a_nCharacterID, STSkillTradeInfo a_stSkillTradeInfo, CTargetInfo a_oTargetInfo, bool a_bIsAutoCreate = true, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || Access.IsEnableTrade(a_nCharacterID, a_stSkillTradeInfo.m_oPayTargetInfoDict, a_oTargetInfo));

		// 교환 가능 할 경우
		if(Access.IsEnableTrade(a_nCharacterID, a_stSkillTradeInfo.m_oPayTargetInfoDict, a_oTargetInfo)) {
			Func.Pay(a_nCharacterID, a_stSkillTradeInfo.m_oPayTargetInfoDict, a_oTargetInfo, a_bIsEnableAssert);
			Func.Acquire(a_nCharacterID, a_stSkillTradeInfo.m_oAcquireTargetInfoDict, a_oTargetInfo, a_bIsAutoCreate, a_bIsEnableAssert);
		}
	}

	/** 교환한다 */
	public static void Trade(int a_nCharacterID, STObjTradeInfo a_stObjTradeInfo, bool a_bIsAutoCreate = true, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || Access.IsEnableTrade(a_nCharacterID, a_stObjTradeInfo.m_oPayTargetInfoDict));

		// 교환 가능 할 경우
		if(Access.IsEnableTrade(a_nCharacterID, a_stObjTradeInfo.m_oPayTargetInfoDict)) {
			Func.Pay(a_nCharacterID, a_stObjTradeInfo.m_oPayTargetInfoDict, a_bIsEnableAssert);
			Func.Acquire(a_nCharacterID, a_stObjTradeInfo.m_oAcquireTargetInfoDict, a_bIsAutoCreate, a_bIsEnableAssert);
		}
	}

	/** 교환한다 */
	public static void Trade(int a_nCharacterID, STObjTradeInfo a_stObjTradeInfo, CTargetInfo a_oTargetInfo, bool a_bIsAutoCreate = true, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || Access.IsEnableTrade(a_nCharacterID, a_stObjTradeInfo.m_oPayTargetInfoDict, a_oTargetInfo));

		// 교환 가능 할 경우
		if(Access.IsEnableTrade(a_nCharacterID, a_stObjTradeInfo.m_oPayTargetInfoDict, a_oTargetInfo)) {
			Func.Pay(a_nCharacterID, a_stObjTradeInfo.m_oPayTargetInfoDict, a_oTargetInfo, a_bIsEnableAssert);
			Func.Acquire(a_nCharacterID, a_stObjTradeInfo.m_oAcquireTargetInfoDict, a_oTargetInfo, a_bIsAutoCreate, a_bIsEnableAssert);
		}
	}

	/** 교환한다 */
	public static void Trade(int a_nCharacterID, STProductTradeInfo a_stProductTradeInfo, bool a_bIsAutoCreate = true, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || Access.IsEnableTrade(a_nCharacterID, a_stProductTradeInfo.m_oPayTargetInfoDict));

		// 교환 가능 할 경우
		if(Access.IsEnableTrade(a_nCharacterID, a_stProductTradeInfo.m_oPayTargetInfoDict)) {
			Func.Pay(a_nCharacterID, a_stProductTradeInfo.m_oPayTargetInfoDict, a_bIsEnableAssert);
			Func.Acquire(a_nCharacterID, a_stProductTradeInfo.m_oAcquireTargetInfoDict, a_bIsAutoCreate, a_bIsEnableAssert);
		}
	}

	/** 교환한다 */
	public static void Trade(int a_nCharacterID, STProductTradeInfo a_stProductTradeInfo, CTargetInfo a_oTargetInfo, bool a_bIsAutoCreate = true, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || Access.IsEnableTrade(a_nCharacterID, a_stProductTradeInfo.m_oPayTargetInfoDict, a_oTargetInfo));

		// 교환 가능 할 경우
		if(Access.IsEnableTrade(a_nCharacterID, a_stProductTradeInfo.m_oPayTargetInfoDict, a_oTargetInfo)) {
			Func.Pay(a_nCharacterID, a_stProductTradeInfo.m_oPayTargetInfoDict, a_oTargetInfo, a_bIsEnableAssert);
			Func.Acquire(a_nCharacterID, a_stProductTradeInfo.m_oAcquireTargetInfoDict, a_oTargetInfo, a_bIsAutoCreate, a_bIsEnableAssert);
		}
	}

	/** 어빌리티 값을 설정한다 */
	private static void SetupAbilityVals(STTargetInfo a_stTargetInfo, Dictionary<EAbilityKinds, decimal> a_oOutAbilityValDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oOutAbilityValDict != null);

		// 어빌리티 값 설정이 가능 할 경우
		if(a_oOutAbilityValDict != null && a_stTargetInfo.m_eTargetKinds == ETargetKinds.ABILITY && a_stTargetInfo.Kinds > KCDefine.B_IDX_INVALID) {
			var eAbilityKinds = (EAbilityKinds)a_stTargetInfo.Kinds;
			var stAbilityInfo = CAbilityInfoTable.Inst.GetAbilityInfo(eAbilityKinds);

			// 어빌리티 값 타입이 유효 할 경우
			if(stAbilityInfo.m_eAbilityValType != EAbilityValType.NONE) {
				decimal dmAbilityVal = (stAbilityInfo.m_stValInfo.m_eValType == EValType.INT) ? stAbilityInfo.m_stValInfo.m_dmVal : (decimal)stAbilityInfo.m_stValInfo.m_dmVal / KCDefine.B_UNIT_NORM_VAL_TO_PERCENT;
				a_oOutAbilityValDict.ExReplaceVal(eAbilityKinds, System.Math.Clamp(a_oOutAbilityValDict.GetValueOrDefault(eAbilityKinds) + (dmAbilityVal * a_stTargetInfo.m_stValInfo01.m_dmVal), decimal.MinValue, decimal.MaxValue), a_bIsEnableAssert);
			}

			foreach(var stKeyVal in stAbilityInfo.m_oExtraAbilityTargetInfoDict) {
				Func.SetupAbilityVals(stKeyVal.Value, a_oOutAbilityValDict, a_bIsEnableAssert);
			}
		}
	}

	/** 어빌리티 값을 설정한다 */
	private static void SetupAbilityVals(CTargetInfo a_oTargetInfo, Dictionary<EAbilityKinds, decimal> a_oOutAbilityValDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oTargetInfo != null && a_oOutAbilityValDict != null));

		// 어빌리티 값 설정이 가능 할 경우
		if(a_oTargetInfo != null && a_oOutAbilityValDict != null) {
			Func.SetupAbilityVals(a_oTargetInfo.m_oAbilityTargetInfoDict, a_oOutAbilityValDict, a_bIsEnableAssert);

			for(int i = 0; i < a_oTargetInfo.m_oOwnedTargetInfoList.Count; ++i) {
				// 어빌리티 값 설정이 가능 할 경우
				if(a_oTargetInfo.m_oOwnedTargetInfoList[i].TargetType != ETargetType.SKILL || (ESkillType)a_oTargetInfo.m_oOwnedTargetInfoList[i].Kinds.ExKindsToType() == ESkillType.PASSIVE) {
					Func.SetupAbilityVals(a_oTargetInfo.m_oOwnedTargetInfoList[i], a_oOutAbilityValDict, a_bIsEnableAssert);
				}
			}
		}
	}

	/** 아이템 타겟을 지불한다 */
	private static void PayItemTarget(int a_nCharacterID, STTargetInfo a_stTargetInfo, CTargetInfo a_oTargetInfo, bool a_bIsEnableAssert = true) {
		bool bIsValid = CItemInfoTable.Inst.TryGetItemInfo((EItemKinds)a_stTargetInfo.Kinds, out STItemInfo stItemInfo);
		CAccess.Assert(!a_bIsEnableAssert || (bIsValid && a_oTargetInfo != null));

		// 타겟 정보가 존재 할 경우
		if(bIsValid && a_oTargetInfo != null) {
			Func.DoPay(a_nCharacterID, a_stTargetInfo, a_oTargetInfo, stItemInfo.m_stCommonInfo.m_bIsFlags01, a_bIsEnableAssert);
		}
	}

	/** 스킬 타겟을 지불한다 */
	private static void PaySkillTarget(int a_nCharacterID, STTargetInfo a_stTargetInfo, CTargetInfo a_oTargetInfo, bool a_bIsEnableAssert = true) {
		bool bIsValid = CSkillInfoTable.Inst.TryGetSkillInfo((ESkillKinds)a_stTargetInfo.Kinds, out STSkillInfo stSkillInfo);
		CAccess.Assert(!a_bIsEnableAssert || (bIsValid && a_oTargetInfo != null));

		// 타겟 정보가 존재 할 경우
		if(bIsValid && a_oTargetInfo != null) {
			Func.DoPay(a_nCharacterID, a_stTargetInfo, a_oTargetInfo, stSkillInfo.m_stCommonInfo.m_bIsFlags01, a_bIsEnableAssert);
		}
	}

	/** 객체 타겟을 지불한다 */
	private static void PayObjTarget(int a_nCharacterID, STTargetInfo a_stTargetInfo, CTargetInfo a_oTargetInfo, bool a_bIsEnableAssert = true) {
		bool bIsValid = CObjInfoTable.Inst.TryGetObjInfo((EObjKinds)a_stTargetInfo.Kinds, out STObjInfo stObjInfo);
		CAccess.Assert(!a_bIsEnableAssert || (bIsValid && a_oTargetInfo != null));

		// 타겟 정보가 존재 할 경우
		if(bIsValid && a_oTargetInfo != null) {
			Func.DoPay(a_nCharacterID, a_stTargetInfo, a_oTargetInfo, stObjInfo.m_stCommonInfo.m_bIsFlags01, a_bIsEnableAssert);
		}
	}

	/** 어빌리티 타겟을 지불한다 */
	private static void PayAbilityTarget(int a_nCharacterID, STTargetInfo a_stTargetInfo, CTargetInfo a_oTargetInfo, bool a_bIsEnableAssert = true) {
		bool bIsValid = CAbilityInfoTable.Inst.TryGetAbilityInfo((EAbilityKinds)a_stTargetInfo.Kinds, out STAbilityInfo stAbilityInfo);
		CAccess.Assert(!a_bIsEnableAssert || (bIsValid && a_oTargetInfo != null));

		// 타겟 정보가 존재 할 경우
		if(bIsValid && a_oTargetInfo != null) {
			Func.DoPay(a_nCharacterID, a_stTargetInfo, a_oTargetInfo, false, a_bIsEnableAssert);
		}
	}

	/** 아이템 타겟을 획득한다 */
	private static void AcquireItemTarget(int a_nCharacterID, STTargetInfo a_stTargetInfo, CTargetInfo a_oTargetInfo, bool a_bIsEnableAssert = true) {
		bool bIsValid = CItemInfoTable.Inst.TryGetItemInfo((EItemKinds)a_stTargetInfo.Kinds, out STItemInfo stItemInfo);
		CAccess.Assert(!a_bIsEnableAssert || (bIsValid && a_oTargetInfo != null));

		// 타겟 정보가 존재 할 경우
		if(bIsValid && a_oTargetInfo != null) {
			Func.DoAcquire(a_nCharacterID, a_stTargetInfo, a_oTargetInfo, stItemInfo.m_stCommonInfo.m_bIsFlags01, a_bIsEnableAssert);

			// 광고 제거 아이템 일 경우
			if(a_stTargetInfo.m_eTargetKinds == ETargetKinds.ITEM_NUMS && (EItemKinds)a_stTargetInfo.Kinds == EItemKinds.NON_CONSUMABLE_REMOVE_ADS) {
#if ADS_MODULE_ENABLE
				Func.CloseBannerAds(null);

				CAdsManager.Inst.IsEnableBannerAds = false;
				CAdsManager.Inst.IsEnableFullscreenAds = false;
#endif // #if ADS_MODULE_ENABLE
			}
		}
	}

	/** 스킬 타겟을 획득한다 */
	private static void AcquireSkillTarget(int a_nCharacterID, STTargetInfo a_stTargetInfo, CTargetInfo a_oTargetInfo, bool a_bIsEnableAssert = true) {
		bool bIsValid = CSkillInfoTable.Inst.TryGetSkillInfo((ESkillKinds)a_stTargetInfo.Kinds, out STSkillInfo stSkikllInfo);
		CAccess.Assert(!a_bIsEnableAssert || (bIsValid && a_oTargetInfo != null));

		// 타겟 정보가 존재 할 경우
		if(bIsValid && a_oTargetInfo != null) {
			Func.DoAcquire(a_nCharacterID, a_stTargetInfo, a_oTargetInfo, stSkikllInfo.m_stCommonInfo.m_bIsFlags01, a_bIsEnableAssert);
		}
	}

	/** 객체 타겟을 획득한다 */
	private static void AcquireObjTarget(int a_nCharacterID, STTargetInfo a_stTargetInfo, CTargetInfo a_oTargetInfo, bool a_bIsEnableAssert = true) {
		bool bIsValid = CObjInfoTable.Inst.TryGetObjInfo((EObjKinds)a_stTargetInfo.Kinds, out STObjInfo stObjInfo);
		CAccess.Assert(!a_bIsEnableAssert || (bIsValid && a_oTargetInfo != null));

		// 타겟 정보가 존재 할 경우
		if(bIsValid && a_oTargetInfo != null) {
			Func.DoAcquire(a_nCharacterID, a_stTargetInfo, a_oTargetInfo, stObjInfo.m_stCommonInfo.m_bIsFlags01, a_bIsEnableAssert);
		}
	}

	/** 어빌리티 타겟을 획득한다 */
	private static void AcquireAbilityTarget(int a_nCharacterID, STTargetInfo a_stTargetInfo, CTargetInfo a_oTargetInfo, bool a_bIsEnableAssert = true) {
		bool bIsValid = CAbilityInfoTable.Inst.TryGetAbilityInfo((EAbilityKinds)a_stTargetInfo.Kinds, out STAbilityInfo stAbilityInfo);
		CAccess.Assert(!a_bIsEnableAssert || (bIsValid && a_oTargetInfo != null));

		// 타겟 정보가 존재 할 경우
		if(bIsValid && a_oTargetInfo != null) {
			Func.DoAcquire(a_nCharacterID, a_stTargetInfo, a_oTargetInfo, false, a_bIsEnableAssert);
		}
	}

	/** 지불한다 */
	private static void DoPay(int a_nCharacterID, STTargetInfo a_stTargetInfo, CTargetInfo a_oTargetInfo, bool a_bIsCounting = true, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_stTargetInfo.m_eTargetKinds != ETargetKinds.NONE && a_oTargetInfo != null));

		// 타겟 정보가 존재 할 경우
		if(a_stTargetInfo.m_eTargetKinds != ETargetKinds.NONE && a_oTargetInfo != null) {
			switch(((int)a_stTargetInfo.m_eTargetKinds).ExKindsToSubKindsTypeVal()) {
				case KEnumVal.TK_LV_SUB_KINDS_TYPE_VAL: a_oTargetInfo.m_oAbilityTargetInfoDict.ExIncrTargetVal(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_LV, -a_stTargetInfo.m_stValInfo01.m_dmVal, a_bIsEnableAssert); break;
				case KEnumVal.TK_EXP_SUB_KINDS_TYPE_VAL: a_oTargetInfo.m_oAbilityTargetInfoDict.ExIncrTargetVal(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_EXP, -a_stTargetInfo.m_stValInfo01.m_dmVal, a_bIsEnableAssert); break;
				case KEnumVal.TK_NUMS_SUB_KINDS_TYPE_VAL: a_oTargetInfo.m_oAbilityTargetInfoDict.ExIncrTargetVal(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_NUMS, -a_stTargetInfo.m_stValInfo01.m_dmVal, a_bIsEnableAssert); break;
				case KEnumVal.TK_ENHANCE_SUB_KINDS_TYPE_VAL: a_oTargetInfo.m_oAbilityTargetInfoDict.ExIncrTargetVal(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_ENHANCE, -a_stTargetInfo.m_stValInfo01.m_dmVal, a_bIsEnableAssert); break;
			}
		}
	}

	/** 획득한다 */
	private static void DoAcquire(int a_nCharacterID, STTargetInfo a_stTargetInfo, CTargetInfo a_oTargetInfo, bool a_bIsCounting = true, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_stTargetInfo.m_eTargetKinds != ETargetKinds.NONE && a_oTargetInfo != null));

		// 타겟 정보가 존재 할 경우
		if(a_stTargetInfo.m_eTargetKinds != ETargetKinds.NONE && a_oTargetInfo != null) {
			switch(((int)a_stTargetInfo.m_eTargetKinds).ExKindsToSubKindsTypeVal()) {
				case KEnumVal.TK_LV_SUB_KINDS_TYPE_VAL: a_oTargetInfo.m_oAbilityTargetInfoDict.ExIncrTargetVal(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_LV, a_stTargetInfo.m_stValInfo01.m_dmVal, a_bIsEnableAssert); break;
				case KEnumVal.TK_EXP_SUB_KINDS_TYPE_VAL: a_oTargetInfo.m_oAbilityTargetInfoDict.ExIncrTargetVal(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_EXP, a_stTargetInfo.m_stValInfo01.m_dmVal, a_bIsEnableAssert); break;
				case KEnumVal.TK_NUMS_SUB_KINDS_TYPE_VAL: a_oTargetInfo.m_oAbilityTargetInfoDict.ExIncrTargetVal(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_NUMS, a_stTargetInfo.m_stValInfo01.m_dmVal, a_bIsEnableAssert); break;
				case KEnumVal.TK_ENHANCE_SUB_KINDS_TYPE_VAL: a_oTargetInfo.m_oAbilityTargetInfoDict.ExIncrTargetVal(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_ENHANCE, a_stTargetInfo.m_stValInfo01.m_dmVal, a_bIsEnableAssert); break;
			}

			a_oTargetInfo.m_oAbilityTargetInfoDict.ExTryGetTargetInfo(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_LV, out STTargetInfo stLVAbilityTargetInfo);
			a_oTargetInfo.m_oAbilityTargetInfoDict.ExReplaceTargetVal(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_LV, System.Math.Clamp(stLVAbilityTargetInfo.m_stValInfo01.m_dmVal, KCDefine.B_VAL_1_INT, long.MaxValue), a_bIsEnableAssert);

			a_oTargetInfo.m_oAbilityTargetInfoDict.ExTryGetTargetInfo(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_NUMS, out STTargetInfo stNumsAbilityTargetInfo);
			a_oTargetInfo.m_oAbilityTargetInfoDict.ExReplaceTargetVal(ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_NUMS, System.Math.Clamp(stNumsAbilityTargetInfo.m_stValInfo01.m_dmVal, KCDefine.B_VAL_1_INT, long.MaxValue), a_bIsEnableAssert);
		}
	}
	#endregion // 클래스 함수

	#region 조건부 클래스 함수
#if ADS_MODULE_ENABLE
	/** 광고 누적 횟수를 증가시킨다 */
	public static void IncrAdsSkipTimes(int a_nSkipTimes) {
		// 광고 누적 횟수 갱신이 가능 할 경우
		if(CAppInfoStorage.Inst.IsEnableUpdateAdsSkipTimes) {
			CAppInfoStorage.Inst.SetAdsSkipTimes(Mathf.Clamp(CAppInfoStorage.Inst.AdsSkipTimes + a_nSkipTimes, KCDefine.B_VAL_0_INT, int.MaxValue));
		}
	}

	/** 보상 광고 시청 횟수를 증가시킨다 */
	public static void IncrRewardAdsWatchTimes(int a_nWatchTimes) {
		CAppInfoStorage.Inst.AppInfo.RewardAdsWatchTimes = Mathf.Clamp(CAppInfoStorage.Inst.AppInfo.RewardAdsWatchTimes + a_nWatchTimes, KCDefine.B_VAL_0_INT, int.MaxValue);
	}

	/** 전면 광고 시청 횟수를 증가시킨다 */
	public static void IncrFullscreenAdsWatchTimes(int a_nWatchTimes) {
		CAppInfoStorage.Inst.AppInfo.FullscreenAdsWatchTimes = Mathf.Clamp(CAppInfoStorage.Inst.AppInfo.FullscreenAdsWatchTimes + a_nWatchTimes, KCDefine.B_VAL_0_INT, int.MaxValue);
	}
#endif // #if ADS_MODULE_ENABLE

#if FIREBASE_MODULE_ENABLE
	/** 로그인 되었을 경우 */
	public static void OnLogin(CFirebaseManager a_oSender, bool a_bIsSuccess, System.Action<CAlertPopup, bool> a_oCallback) {
		// 로그아웃 되었을 경우
		if(a_bIsSuccess) {
			Func.ShowOnLoginPopup(a_oCallback);
		} else {
			Func.ShowOnLoginFailPopup(a_oCallback);
		}
	}

	/** 로그아웃 되었을 경우 */
	public static void OnLogout(CFirebaseManager a_oSender, bool a_bIsSuccess, System.Action<CAlertPopup, bool> a_oCallback) {
		// 로그아웃 되었을 경우
		if(a_bIsSuccess) {
			Func.ShowOnLogoutPopup(a_oCallback);
		} else {
			Func.ShowOnLogoutFailPopup(a_oCallback);
		}
	}

	/** 유저 정보가 로드 되었을 경우 */
	public static void OnLoadUserInfo(CFirebaseManager a_oSender, string a_oJSONStr, bool a_bIsSuccess, System.Action<CAlertPopup, bool> a_oCallback) {
		// 로드 되었을 경우
		if(a_bIsSuccess) {
			Func.ShowOnLoadPopup(a_oCallback);
		} else {
			Func.ShowOnLoadFailPopup(a_oCallback);
		}
	}

	/** 유저 정보가 저장 되었을 경우 */
	public static void OnSaveUserInfo(CFirebaseManager a_oSender, bool a_bIsSuccess, System.Action<CAlertPopup, bool> a_oCallback) {
		// 저장 되었을 경우
		if(a_bIsSuccess) {
			Func.ShowOnSavePopup(a_oCallback);
		} else {
			Func.ShowOnSaveFailPopup(a_oCallback);
		}
	}
#endif // #if FIREBASE_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
	/** 상품이 결제 되었을 경우 */
	public static void OnPurchaseProduct(CPurchaseManager a_oSender, string a_oProductID, bool a_bIsSuccess, System.Action<CAlertPopup, bool> a_oCallback) {
		// 결제 되었을 경우
		if(a_bIsSuccess) {
			Func.ShowOnPurchasePopup(a_oCallback);
		} else {
			Func.ShowOnPurchaseFailPopup(a_oCallback);
		}
	}

	/** 상품이 복원 되었을 경우 */
	public static void OnRestoreProducts(CPurchaseManager a_oSender, List<Product> a_oProductList, bool a_bIsSuccess, System.Action<CAlertPopup, bool> a_oCallback) {
		// 복원 되었을 경우
		if(a_bIsSuccess) {
			Func.ShowOnRestorePopup(a_oCallback);
		} else {
			Func.ShowOnRestoreFailPopup(a_oCallback);
		}
	}

	/** 상품을 획득한다 */
	public static void AcquireProduct(string a_oProductID, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oProductID.ExIsValid());

		// 상품이 존재 할 경우
		if(a_oProductID.ExIsValid()) {
			int nIdx = CProductInfoTable.Inst.GetProductInfoIdx(a_oProductID);
			var oProduct = CPurchaseManager.Inst.GetProduct(a_oProductID);
			var stProductTradeInfo = CProductTradeInfoTable.Inst.GetBuyProductTradeTradeInfo(nIdx);

			Func.Acquire(CGameInfoStorage.Inst.PlayCharacterID, stProductTradeInfo.m_oAcquireTargetInfoDict);

			// 비소모 상품 일 경우
			if(oProduct != null && oProduct.definition.type == ProductType.NonConsumable && !CCommonUserInfoStorage.Inst.IsRestoreProduct(a_oProductID)) {
				CCommonUserInfoStorage.Inst.AddRestoreProductID(a_oProductID);
				CCommonUserInfoStorage.Inst.SaveUserInfo();
			}
		}
	}

	/** 복원 상품을 획득한다 */
	public static void AcquireRestoreProducts(List<Product> a_oProductList, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oProductList != null);

		// 상품이 존재 할 경우
		if(a_oProductList != null) {
			for(int i = 0; i < a_oProductList.Count; ++i) {
				// 상품 복원이 가능 할 경우
				if(!CCommonUserInfoStorage.Inst.IsRestoreProduct(a_oProductList[i].definition.id)) {
					int nIdx = CProductInfoTable.Inst.GetProductInfoIdx(a_oProductList[i].definition.id);
					var stProductTradeInfo = CProductTradeInfoTable.Inst.GetBuyProductTradeTradeInfo(nIdx);

					Func.Acquire(CGameInfoStorage.Inst.PlayCharacterID, stProductTradeInfo.m_oAcquireTargetInfoDict);
					CCommonUserInfoStorage.Inst.AddRestoreProductID(a_oProductList[i].definition.id);
				}
			}

			CCommonUserInfoStorage.Inst.SaveUserInfo();
		}
	}
#endif // #if PURCHASE_MODULE_ENABLE

#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 구글 시트를 로드했을 경우 */
	public static void OnLoadGoogleSheets(SimpleJSON.JSONNode a_oVerInfos) {
		for(int i = 0; i < a_oVerInfos.Count; ++i) {
			CAppInfoStorage.Inst.AppInfo.m_oTableSysVerDict.ExReplaceVal(a_oVerInfos[i][KCDefine.U_KEY_NAME], System.Version.Parse(a_oVerInfos[i][KCDefine.U_KEY_VER]));
		}

		CAppInfoStorage.Inst.SaveAppInfo();
	}

	/** 값 정보를 저장한다 */
	public static void SaveValInfos(List<STValInfo> a_oValInfoList, string a_oFmt, SimpleJSON.JSONNode a_oOutValInfos, int a_nSrcIdx = KCDefine.B_VAL_0_INT) {
		for(int i = 0; i < a_oValInfoList.Count; ++i) {
			a_oValInfoList[i].SaveValInfo(a_oOutValInfos[string.Format(a_oFmt, i + KCDefine.B_VAL_1_INT)], a_nSrcIdx);
		}
	}

	/** 타겟 정보를 저장한다 */
	public static void SaveTargetInfos(Dictionary<ulong, STTargetInfo> a_oTargetInfoDict, string a_oFmt, SimpleJSON.JSONNode a_oOutTargetInfos, int a_nSrcIdx = KCDefine.B_VAL_0_INT) {
		int nIdx = KCDefine.B_VAL_0_INT;

		foreach(var stKeyVal in a_oTargetInfoDict) {
			stKeyVal.Value.SaveTargetInfo(a_oOutTargetInfos[string.Format(a_oFmt, ++nIdx)], a_nSrcIdx);
		}
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	#endregion // 조건부 클래스 함수

	#region 조건부 제네릭 클래스 함수
#if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	/** 값을 저장한다 */
	public static void SaveVals<T>(List<T> a_oValList, string a_oFmt, System.Func<T, string> a_oCallback, SimpleJSON.JSONNode a_oOutVals) {
		for(int i = 0; i < a_oValList.Count; ++i) {
			a_oOutVals[string.Format(a_oFmt, i + KCDefine.B_VAL_1_INT)] = a_oCallback(a_oValList[i]);
		}
	}
#endif // #if GOOGLE_SHEET_ENABLE && (DEBUG || DEVELOPMENT_BUILD)
	#endregion // 조건부 제네릭 클래스 함수
}

/** 초기화 씬 함수 */
public static partial class Func {
	#region 클래스 함수

	#endregion // 클래스 함수
}

/** 시작 씬 함수 */
public static partial class Func {
	#region 클래스 함수

	#endregion // 클래스 함수
}

/** 설정 씬 함수 */
public static partial class Func {
	#region 클래스 함수

	#endregion // 클래스 함수
}

/** 약관 동의 씬 함수 */
public static partial class Func {
	#region 클래스 함수

	#endregion // 클래스 함수
}

/** 지연 설정 씬 함수 */
public static partial class Func {
	#region 클래스 함수

	#endregion // 클래스 함수
}

/** 타이틀 씬 함수 */
public static partial class Func {
	#region 클래스 함수

	#endregion // 클래스 함수
}

/** 메인 씬 함수 */
public static partial class Func {
	#region 클래스 함수

	#endregion // 클래스 함수
}

/** 게임 씬 함수 */
public static partial class Func {
	#region 클래스 함수

	#endregion // 클래스 함수
}

/** 로딩 씬 함수 */
public static partial class Func {
	#region 클래스 함수

	#endregion // 클래스 함수
}

/** 중첩 씬 함수 */
public static partial class Func {
	#region 클래스 함수

	#endregion // 클래스 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

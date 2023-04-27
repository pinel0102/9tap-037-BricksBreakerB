using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using TMPro;

using DG.Tweening;

/** 서브 일일 보상 팝업 */
public partial class CDailyRewardPopup : CSubPopup {
	/** 서브 식별자 */
	private enum ESubKey {
		NONE = -1,
		[HideInInspector] MAX_VAL
	}

	#region 변수
    public const int G_MAX_NUM_ACQUIRE_ANI_COINS = 5;
    public const float G_DURATION_DAILY_REWARD_ACQUIRE_ANI = 0.75f;
	#endregion // 변수

	#region 프로퍼티

	#endregion // 프로퍼티

	#region 함수
	/** 팝업이 닫혔을 경우 */
	protected override void OnClose() {
		base.OnClose();
		CScheduleManager.Inst.RemoveTimer(this);
	}

	/** 초기화 */
	private void SubAwake() {
		// Do Something
	}

	/** 초기화 */
	private void SubInit() {
		// Do Something
	}

	/** UI 상태를 갱신한다 */
	private void SubUpdateUIsState() {
		// Do Something
	}

	/** 보상 UI 상태를 갱신한다 */
	private void UpdateRewardUIsState(GameObject a_oRewardUIs, STRewardInfo a_stRewardInfo) {
		int nIdx = m_oRewardUIsList.FindIndex((a_oCompareRewardUIs) => a_oCompareRewardUIs == a_oRewardUIs);

        List<KeyValuePair<int, int>> itemList = new List<KeyValuePair<int, int>>();
        foreach(var item in a_stRewardInfo.m_oAcquireTargetInfoDict)
        {
            //Debug.Log(string.Format("{0} / {1}", (EItemKinds)item.Value.Kinds, item.Value.m_stValInfo01.m_dmVal));
            itemList.Add(new KeyValuePair<int, int>(item.Value.Kinds, (int)item.Value.m_stValInfo01.m_dmVal));
        }

        if (itemList.Count > 0)
        {
            decimal dmVal = a_stRewardInfo.m_oAcquireTargetInfoDict.ExGetTargetVal(ETargetKinds.ITEM_NUMS, itemList[0].Key);

            // 텍스트를 갱신한다
            a_oRewardUIs.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_NUM_TEXT).text = string.Format(GlobalDefine.FORMAT_ITEM_COUNT, dmVal);
        }

		a_oRewardUIs.ExFindChild(KCDefine.U_OBJ_N_GLOW_IMG).SetActive(nIdx == Access.GetDailyRewardID(CGameInfoStorage.Inst.PlayCharacterID));
		a_oRewardUIs.ExFindChild(KCDefine.U_OBJ_N_CHECK_IMG).SetActive(nIdx < Access.GetDailyRewardID(CGameInfoStorage.Inst.PlayCharacterID));
		// 이미지를 갱신한다 }
	}

	/** 획득 애니메이션이 완료 되었을 경우 */
	private void OnCompleteAcquireAni(GameObject a_oSender, Sequence a_oAni, int a_nIdx) {
		a_oAni?.Kill();
		this.AniList.ExRemoveVal(a_oAni);
		//CSndManager.Inst.PlayFXSnds(KDefine.G_SND_P_SFX_DIAMOND);

		// 획득 애니메이션이 모두 완료 되었을 경우
		if(a_nIdx + KCDefine.B_VAL_1_INT >= G_MAX_NUM_ACQUIRE_ANI_COINS) {
			this.ExLateCallFunc((a_oFuncSender) => this.OnCloseRewardAcquirePopup(null), KCDefine.B_VAL_0_5_REAL, true);
		}
	}

	/** 획득 애니메이션을 시작한다 */
	private void StartAcquireAni(GameObject a_oRewardUIs) {
        Debug.Log(CodeManager.GetMethodName());
        
		//int nTimes = 0;
		//var oStoreBtn = CSceneManager.ActiveSceneManager.UIsBase.ExFindChild(KCDefine.U_OBJ_N_STORE_BTN);
		//var oCoinsImg = oStoreBtn.ExFindComponent<Image>(KCDefine.U_OBJ_N_COINS_IMG);

		/*CScheduleManager.Inst.AddTimer(this, KCDefine.B_VAL_0_1_REAL, G_MAX_NUM_ACQUIRE_ANI_COINS, () => {
			var oAcquireCoinsImg = CFactory.CreateCloneObj<Image>(KCDefine.U_OBJ_N_REWARD_IMG, oCoinsImg.gameObject, this.ContentsUIs);
			oAcquireCoinsImg.transform.position = a_oRewardUIs.ExFindChild(KCDefine.U_OBJ_N_ICON_IMG).transform.position;

			int nAcquireIdx = nTimes;
			nTimes += KCDefine.B_VAL_1_INT;

			this.AniList.ExAddVal(oAcquireCoinsImg.gameObject.ExStartWorldPathAni(new List<Vector3>() {
				oAcquireCoinsImg.transform.position + (Vector3.right * (KCDefine.B_UNIT_DIGITS_PER_HUNDRED / KCDefine.B_VAL_3_INT)), oCoinsImg.transform.position
			}, G_DURATION_DAILY_REWARD_ACQUIRE_ANI, (a_oSender, a_oAni) => this.OnCompleteAcquireAni(a_oSender, a_oAni, nAcquireIdx), Ease.Linear, true));
		}, true);*/
	}
	#endregion // 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

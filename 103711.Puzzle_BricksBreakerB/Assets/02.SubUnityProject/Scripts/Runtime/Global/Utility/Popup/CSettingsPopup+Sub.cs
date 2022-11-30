using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 설정 팝업 */
public partial class CSettingsPopup : CSubPopup {
#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 버튼을 설정한다 {
		CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
			(EKey.BG_SND_BTN, $"{EKey.BG_SND_BTN}", this.Contents, this.OnTouchBGSndBtn),
			(EKey.FX_SNDS_BTN, $"{EKey.FX_SNDS_BTN}", this.Contents, this.OnTouchFXSndsBtn),
			(EKey.VIBRATE_BTN, $"{EKey.VIBRATE_BTN}", this.Contents, this.OnTouchVibrateBtn),
			(EKey.NOTI_BTN, $"{EKey.NOTI_BTN}", this.Contents, this.OnTouchNotiBtn)
		}, m_oBtnDict);

		CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
			(KCDefine.U_OBJ_N_REVIEW_BTN, this.Contents, this.OnTouchReviewBtn),
			(KCDefine.U_OBJ_N_SUPPORTS_BTN, this.Contents, this.OnTouchSupportsBtn)
		}, false);
		// 버튼을 설정한다 }

#region 추가
		this.SubSetupAwake();
#endregion // 추가
	}
	
	/** 초기화 */
	public override void Init() {
		base.Init();

#region 추가
		this.SubInit();
#endregion // 추가
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {
		var oBtnKeyInfoList = CCollectionManager.Inst.SpawnList<(EKey, string, string, string, bool)>();

		try {
			CSndManager.Inst.SetIsMuteBGSnd(CCommonGameInfoStorage.Inst.GameInfo.IsMuteBGSnd);
			CSndManager.Inst.SetIsMuteFXSnds(CCommonGameInfoStorage.Inst.GameInfo.IsMuteFXSnds);

			// 버튼을 갱신한다 {
			oBtnKeyInfoList.ExAddVal((EKey.BG_SND_BTN, KCDefine.U_OBJ_N_ICON_IMG, KDefine.G_IMG_P_SETTINGS_P_BG_SND_ON, KDefine.G_IMG_P_SETTINGS_P_BG_SND_OFF, !CCommonGameInfoStorage.Inst.GameInfo.IsMuteBGSnd));
			oBtnKeyInfoList.ExAddVal((EKey.FX_SNDS_BTN, KCDefine.U_OBJ_N_ICON_IMG, KDefine.G_IMG_P_SETTINGS_P_FX_SNDS_ON, KDefine.G_IMG_P_SETTINGS_P_FX_SNDS_OFF, !CCommonGameInfoStorage.Inst.GameInfo.IsMuteFXSnds));
			oBtnKeyInfoList.ExAddVal((EKey.VIBRATE_BTN, KCDefine.U_OBJ_N_ICON_IMG, KDefine.G_IMG_P_SETTINGS_P_VIBRATE_ON, KDefine.G_IMG_P_SETTINGS_P_VIBRATE_OFF, !CCommonGameInfoStorage.Inst.GameInfo.IsDisableVibrate));
			oBtnKeyInfoList.ExAddVal((EKey.NOTI_BTN, KCDefine.U_OBJ_N_ICON_IMG, KDefine.G_IMG_P_SETTINGS_P_NOTI_ON, KDefine.G_IMG_P_SETTINGS_P_NOTI_OFF, !CCommonGameInfoStorage.Inst.GameInfo.IsDisableNoti));

			for(int i = 0; i < oBtnKeyInfoList.Count; ++i) {
				string oImgPath = oBtnKeyInfoList[i].Item5 ? oBtnKeyInfoList[i].Item3 : oBtnKeyInfoList[i].Item4;
				m_oBtnDict.GetValueOrDefault(oBtnKeyInfoList[i].Item1)?.gameObject.ExFindComponent<Image>(oBtnKeyInfoList[i].Item2)?.ExSetSprite<Image>(CResManager.Inst.GetRes<Sprite>(oImgPath));
			}
			// 버튼을 갱신한다 }
		} finally {
			CCollectionManager.Inst.DespawnList(oBtnKeyInfoList);
		}

#region 추가
		this.SubUpdateUIsState();
#endregion // 추가
	}
#endregion // 함수
}

/** 서브 설정 팝업 */
public partial class CSettingsPopup : CSubPopup {
	/** 서브 식별자 */
	private enum ESubKey {
		NONE = -1,
		[HideInInspector] MAX_VAL
	}

#region 변수

#endregion // 변수

#region 프로퍼티

#endregion // 프로퍼티

#region 함수
	/** 팝업을 설정한다 */
	private void SubSetupAwake() {
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
#endregion // 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

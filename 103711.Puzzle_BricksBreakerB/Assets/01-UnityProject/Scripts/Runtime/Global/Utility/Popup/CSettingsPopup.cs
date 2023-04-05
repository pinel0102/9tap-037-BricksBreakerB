using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 설정 팝업 */
public partial class CSettingsPopup : CSubPopup {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		BG_SND_BTN,
		FX_SNDS_BTN,
		VIBRATE_BTN,
		NOTI_BTN,
		[HideInInspector] MAX_VAL
	}

	#region 변수
	/** =====> UI <===== */
    public TMP_Text versionText;
	private Dictionary<EKey, Button> m_oBtnDict = new Dictionary<EKey, Button>();
	#endregion // 변수

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

		// 버튼을 설정한다 {
		CFunc.SetupButtons(new List<(string, GameObject, UnityAction)>() {
			(KCDefine.U_OBJ_N_REVIEW_BTN, this.Contents, this.OnTouchReviewBtn),
			(KCDefine.U_OBJ_N_SUPPORTS_BTN, this.Contents, this.OnTouchSupportsBtn)
		});

		CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
			(EKey.BG_SND_BTN, $"{EKey.BG_SND_BTN}", this.Contents, this.OnTouchBGSndBtn),
			(EKey.FX_SNDS_BTN, $"{EKey.FX_SNDS_BTN}", this.Contents, this.OnTouchFXSndsBtn),
			(EKey.VIBRATE_BTN, $"{EKey.VIBRATE_BTN}", this.Contents, this.OnTouchVibrateBtn),
			(EKey.NOTI_BTN, $"{EKey.NOTI_BTN}", this.Contents, this.OnTouchNotiBtn)
		}, m_oBtnDict);
		// 버튼을 설정한다 }

        versionText.text = GlobalDefine.GetVersionText();

		this.SubAwake();
	}

	/** 초기화 */
	public override void Init() {
		base.Init();
		this.SubInit();
	}

	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
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

		this.SubUpdateUIsState();
	}

	/** 배경음 버튼을 눌렀을 경우 */
	private void OnTouchBGSndBtn() {
		CCommonGameInfoStorage.Inst.GameInfo.IsMuteBGSnd = !CCommonGameInfoStorage.Inst.GameInfo.IsMuteBGSnd;
		CCommonGameInfoStorage.Inst.SaveGameInfo();

		this.UpdateUIsState();
	}

	/** 효과음 버튼을 눌렀을 경우 */
	private void OnTouchFXSndsBtn() {
		CCommonGameInfoStorage.Inst.GameInfo.IsMuteFXSnds = !CCommonGameInfoStorage.Inst.GameInfo.IsMuteFXSnds;
		CCommonGameInfoStorage.Inst.SaveGameInfo();

		this.UpdateUIsState();
	}

	/** 진동 버튼을 눌렀을 경우 */
	private void OnTouchVibrateBtn() {
		CCommonGameInfoStorage.Inst.GameInfo.IsDisableVibrate = !CCommonGameInfoStorage.Inst.GameInfo.IsDisableVibrate;
		CCommonGameInfoStorage.Inst.SaveGameInfo();

		this.UpdateUIsState();
		CSndManager.Inst.Vibrate(KCDefine.U_DURATION_HEAVY_VIBRATE);
	}

	/** 알림 버튼을 눌렀을 경우 */
	private void OnTouchNotiBtn() {
		CCommonGameInfoStorage.Inst.GameInfo.IsDisableNoti = !CCommonGameInfoStorage.Inst.GameInfo.IsDisableNoti;
		CCommonGameInfoStorage.Inst.SaveGameInfo();

		this.UpdateUIsState();
	}

	/** 평가 버튼을 눌렀을 경우 */
	private void OnTouchReviewBtn() {
		CUnityMsgSender.Inst.SendShowReviewMsg();
	}

	/** 지원 버튼을 눌렀을 경우 */
	private void OnTouchSupportsBtn() {
		CUnityMsgSender.Inst.SendMailMsg(string.Empty, string.Empty, CProjInfoTable.Inst.CompanyInfo.m_oSupportsMail);
	}
	#endregion // 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using System.Linq;
using TMPro;

namespace OverlayScene {
	/** 서브 중첩 씬 관리자 */
	public partial class CSubOverlaySceneManager : COverlaySceneManager {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			PURCHASE_PRODUCT_ID,
			NUM_NORM_COINS_TEXT,
			STORE_BTN,
			[HideInInspector] MAX_VAL
		}

		/** 콜백 */
		private enum ECallback {
			NONE = -1,
			PURCHASE,
			[HideInInspector] MAX_VAL
		}

		#region 변수
		private Dictionary<EKey, string> m_oStrDict = new Dictionary<EKey, string>() {
			[EKey.PURCHASE_PRODUCT_ID] = string.Empty
		};

		/** =====> UI <===== */
		private Dictionary<EKey, TMP_Text> m_oTextDict = new Dictionary<EKey, TMP_Text>();
		private Dictionary<EKey, Button> m_oBtnDict = new Dictionary<EKey, Button>();

#if PURCHASE_MODULE_ENABLE
		private Dictionary<ECallback, System.Action<CPurchaseManager, string, bool>> m_oCallbackDict = new Dictionary<ECallback, System.Action<CPurchaseManager, string, bool>>();
#endif // #if PURCHASE_MODULE_ENABLE
		#endregion // 변수

		#region 프로퍼티
		public override STSortingOrderInfo UIsCanvasSortingOrderInfo => KCDefine.U_SORTING_OI_OVERLAY_UIS_CANVAS;
		#endregion // 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				// 텍스트를 설정한다
				CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
					(EKey.NUM_NORM_COINS_TEXT, $"{EKey.NUM_NORM_COINS_TEXT}", this.UIsBase)
				}, m_oTextDict);

				// 버튼을 설정한다
				CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
					(EKey.STORE_BTN, $"{EKey.STORE_BTN}", this.UIsBase, this.OnTouchStoreBtn)
				}, m_oBtnDict);

				this.SubAwake();
			}
		}

		/** 초기화 */
		public override void Start() {
			base.Start();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				this.SubStart();
				this.UpdateUIsState();
			}
		}

		/** 제거 되었을 경우 */
		public override void OnDestroy() {
			base.OnDestroy();

			try {
				// 앱이 실행 중 일 경우
				if(CSceneManager.IsAppRunning) {
					this.SubOnDestroy();
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CSubOverlaySceneManager.OnDestroy Exception: {oException.Message}");
			}
		}

		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				this.SubOnUpdate(a_fDeltaTime);
			}
		}

		/** 상점 팝업을 출력한다 */
		public void ShowStorePopup() {
			Func.ShowStorePopup(CSceneManager.ActiveScenePopupUIs, (a_oSender) => {
				var stParams = CStorePopup.MakeParams(Factory.MakeProductTradeInfos(KDefine.G_PRODUCT_KINDS_STORE_LIST).Values.ToList());

#if ADS_MODULE_ENABLE
				stParams.m_oAdsCallbackDict.TryAdd(CStorePopup.ECallback.ADS, (a_oAdsSender, a_stAdsRewardInfo, a_bIsSuccess) => this.UpdateUIsState());
#endif // #if ADS_MODULE_ENABLE

#if PURCHASE_MODULE_ENABLE
				stParams.m_oPurchaseCallbackDict01.TryAdd(CStorePopup.ECallback.PURCHASE, (a_oPurchaseSender, a_oProductID, a_bIsSuccess) => this.UpdateUIsState());
				stParams.m_oPurchaseCallbackDict02.TryAdd(CStorePopup.ECallback.RESTORE, (a_oRestoreSender, a_oProductList, a_bIsSuccess) => this.UpdateUIsState());
#endif // #if PURCHASE_MODULE_ENABLE

				(a_oSender as CStorePopup).Init(stParams);
			});
		}

		/** UI 상태를 갱신한다 */
		private void UpdateUIsState() {
			// 텍스트를 갱신한다
			m_oTextDict[EKey.NUM_NORM_COINS_TEXT]?.ExSetText($"{Access.GetItemTargetVal(CGameInfoStorage.Inst.PlayCharacterID, EItemKinds.GOODS_NORM_COINS, ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_NUMS)}", EFontSet._1, false);

			// UI 상태를 갱신한다
			CSceneManager.GetSceneManager<MainScene.CSubMainSceneManager>(KCDefine.B_SCENE_N_MAIN)?.gameObject.ExSendMsg(string.Empty, KCDefine.U_FUNC_N_UPDATE_UIS_STATE, null, false);
			CSceneManager.GetSceneManager<GameScene.CSubGameSceneManager>(KCDefine.B_SCENE_N_GAME)?.gameObject.ExSendMsg(string.Empty, KCDefine.U_FUNC_N_UPDATE_UIS_STATE, null, false);
			CSceneManager.GetSceneManager<TitleScene.CSubTitleSceneManager>(KCDefine.B_SCENE_N_TITLE)?.gameObject.ExSendMsg(string.Empty, KCDefine.U_FUNC_N_UPDATE_UIS_STATE, null, false);

			this.SubUpdateUIsState();
		}

		/** 상점 버튼을 눌렀을 경우 */
		private void OnTouchStoreBtn() {
			this.ShowStorePopup();
		}
		#endregion // 함수

		#region 조건부 함수
#if PURCHASE_MODULE_ENABLE
		/** 상품을 결제한다 */
		public void PurchaseProduct(int a_nProductIdx, System.Action<CPurchaseManager, string, bool> a_oCallback) {
			Func.PurchaseProduct(CProductInfoTable.Inst.GetProductInfo(a_nProductIdx).m_oID, a_oCallback);
		}

		/** 상품을 결제한다 */
		public void PurchaseProduct(EProductKinds a_eProductKinds, System.Action<CPurchaseManager, string, bool> a_oCallback) {
			m_oCallbackDict.ExReplaceVal(ECallback.PURCHASE, a_oCallback);
			Func.PurchaseProduct(a_eProductKinds, this.OnPurchaseProduct);
		}

		/** 상품이 결제 되었을 경우 */
		private void OnPurchaseProduct(CPurchaseManager a_oSender, string a_oProductID, bool a_bIsSuccess) {
			// 결제 되었을 경우
			if(a_bIsSuccess) {
				Func.AcquireProduct(a_oProductID);
				m_oStrDict[EKey.PURCHASE_PRODUCT_ID] = a_oProductID;

#if FIREBASE_MODULE_ENABLE
				this.ExLateCallFunc((a_oFuncSender) => Func.SaveUserInfo(this.OnSaveUserInfo));
#else
				Func.OnPurchaseProduct(a_oSender, a_oProductID, a_bIsSuccess, null);
#endif // #if FIREBASE_MODULE_ENABLE
			} else {
				Func.OnPurchaseProduct(a_oSender, a_oProductID, a_bIsSuccess, null);
			}

			this.UpdateUIsState();
			m_oCallbackDict.GetValueOrDefault(ECallback.PURCHASE)?.Invoke(a_oSender, a_oProductID, a_bIsSuccess);
		}

#if FIREBASE_MODULE_ENABLE
		/** 유저 정보를 저장했을 경우 */
		private void OnSaveUserInfo(CFirebaseManager a_oSender, bool a_bIsSuccess) {
			Func.OnPurchaseProduct(CPurchaseManager.Inst, m_oStrDict[EKey.PURCHASE_PRODUCT_ID], true, null);
		}
#endif // #if FIREBASE_MODULE_ENABLE
#endif // #if PURCHASE_MODULE_ENABLE
		#endregion // 조건부 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

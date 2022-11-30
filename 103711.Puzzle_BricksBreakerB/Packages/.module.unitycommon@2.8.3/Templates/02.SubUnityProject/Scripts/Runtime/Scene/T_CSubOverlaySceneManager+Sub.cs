#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace OverlayScene {
	/** 서브 중첩 씬 관리자 */
	public partial class CSubOverlaySceneManager : COverlaySceneManager {
#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();
			
			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				this.SetupAwake();
			}
		}

		/** 초기화 */
		public override void Start() {
			base.Start();

			// 앱이 초기화 되었을 경우
			if(CSceneManager.IsAppInit) {
				this.SetupStart();
				this.UpdateUIsState();
			}
		}

		/** 씬을 설정한다 */
		private void SetupAwake() {
			// 텍스트를 설정한다
			CFunc.SetupComponents(new List<(EKey, string, GameObject)>() {
				(EKey.NUM_COINS_TEXT, $"{EKey.NUM_COINS_TEXT}", this.UIsBase)
			}, m_oTextDict);

			// 버튼을 설정한다
			CFunc.SetupButtons(new List<(EKey, string, GameObject, UnityAction)>() {
				(EKey.STORE_BTN, $"{EKey.STORE_BTN}", this.UIsBase, this.OnTouchStoreBtn)
			}, m_oBtnDict);

#region 추가
			this.SubSetupAwake();
#endregion // 추가
		}

		/** 씬을 설정한다 */
		private void SetupStart() {
#region 추가
			this.SubSetupStart();
#endregion // 추가
		}

		/** UI 상태를 갱신한다 */
		private void UpdateUIsState() {
			// UI 상태를 갱신한다
			CSceneManager.GetSceneManager<MainScene.CSubMainSceneManager>(KCDefine.B_SCENE_N_MAIN)?.gameObject.ExSendMsg(string.Empty, KCDefine.U_FUNC_N_UPDATE_UIS_STATE, null, false);
			CSceneManager.GetSceneManager<GameScene.CSubGameSceneManager>(KCDefine.B_SCENE_N_GAME)?.gameObject.ExSendMsg(string.Empty, KCDefine.U_FUNC_N_UPDATE_UIS_STATE, null, false);
			CSceneManager.GetSceneManager<TitleScene.CSubTitleSceneManager>(KCDefine.B_SCENE_N_TITLE)?.gameObject.ExSendMsg(string.Empty, KCDefine.U_FUNC_N_UPDATE_UIS_STATE, null, false);
			
			// 텍스트를 갱신한다
			m_oTextDict.GetValueOrDefault(EKey.NUM_COINS_TEXT)?.ExSetText($"{Access.GetItemTargetVal(CGameInfoStorage.Inst.PlayCharacterID, EItemKinds.GOODS_NORM_COINS, ETargetKinds.ABILITY, (int)EAbilityKinds.STAT_NUMS)}", EFontSet._1, false);

#region 추가
			this.SubUpdateUIsState();
#endregion // 추가
		}
#endregion // 함수
	}

	/** 서브 중첩 씬 관리자 - 서브 */
	public partial class CSubOverlaySceneManager : COverlaySceneManager {
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
		/** 씬을 설정한다 */
		private void SubSetupAwake() {
			// Do Something
		}

		/** 씬을 설정한다 */
		private void SubSetupStart() {
			// Do Something
		}

		/** UI 상태를 갱신한다 */
		private void SubUpdateUIsState() {
			// Do Something
		}
#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

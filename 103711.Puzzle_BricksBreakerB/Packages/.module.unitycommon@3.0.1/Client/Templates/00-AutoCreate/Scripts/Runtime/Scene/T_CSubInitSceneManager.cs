#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCENE_TEMPLATES_MODULE_ENABLE
namespace InitScene {
	/** 서브 초기화 씬 관리자 */
	public partial class CSubInitSceneManager : CInitSceneManager {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			SPLASH_IMG,
			[HideInInspector] MAX_VAL
		}

#region 변수
		/** =====> UI <===== */
		private Dictionary<EKey, Image> m_oImgDict = new Dictionary<EKey, Image>();
#endregion // 변수

#region 프로퍼티
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		public override Color ClearColor => KDefine.IS_COLOR_CLEAR;
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endregion // 프로퍼티

#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
			// 이미지를 설정한다 {
			CFunc.SetupComponents(new List<(EKey, string, GameObject, GameObject)>() {
				(EKey.SPLASH_IMG, $"{EKey.SPLASH_IMG}", this.UIs, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_IMG))
			}, m_oImgDict);

			m_oImgDict[EKey.SPLASH_IMG].sprite = Resources.Load<Sprite>(KCDefine.U_TEX_P_SPLASH);
			m_oImgDict[EKey.SPLASH_IMG].transform.localPosition = new Vector3(KCDefine.B_VAL_0_REAL, this.ScreenHeight * (KCDefine.B_VAL_1_REAL / (KCDefine.B_VAL_5_REAL * KCDefine.B_VAL_8_REAL)), KCDefine.B_VAL_0_REAL);
			m_oImgDict[EKey.SPLASH_IMG].gameObject.SetActive(false);
			// 이미지를 설정한다 }
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		}

		/** 씬을 설정한다 */
		protected override void Setup() {
			base.Setup();
			CSceneManager.ActiveSceneMainCamera.clearFlags = CameraClearFlags.SolidColor;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
			// 테이블을 생성한다 {
			CEtcInfoTable.Create();
			CLevelInfoTable.Create();

			CCalcInfoTable.Create();
			CMissionInfoTable.Create();
			CRewardInfoTable.Create();
			CEpisodeInfoTable.Create();
			CTutorialInfoTable.Create();
			CResInfoTable.Create();

			CItemInfoTable.Create();
			CSkillInfoTable.Create();
			CObjInfoTable.Create();
			CFXInfoTable.Create();
			CAbilityInfoTable.Create();
			CProductTradeInfoTable.Create();
			// 테이블을 생성한다 }

			// 저장소를 생성한다
			CAppInfoStorage.Create();
			CUserInfoStorage.Create();
			CGameInfoStorage.Create();
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		}

		/** 스플래시를 출력한다 */
		protected override void ShowSplash() {
			m_oImgDict[EKey.SPLASH_IMG].SetNativeSize();
			m_oImgDict[EKey.SPLASH_IMG].gameObject.SetActive(true);

			this.ExLateCallFunc((a_oSender) => this.LoadNextScene(), KCDefine.B_VAL_2_REAL);
		}
#endregion // 함수
	}
}
#endif // #if SCENE_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

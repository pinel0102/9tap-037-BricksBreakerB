#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if SCENE_TEMPLATES_MODULE_ENABLE
using System.Diagnostics;
using TMPro;
using DG.Tweening;

namespace StartScene {
	/** 서브 시작 씬 관리자 */
	public partial class CSubStartSceneManager : CStartSceneManager {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			LOADING_GAUGE_ANI,

			STR_BUILDER_01,
			STR_BUILDER_02,

			LOADING_TEXT,
			SCENE_INFO_TEXT,
			LOADING_GAUGE_HANDLER,

			LOADING_GAUGE,
			[HideInInspector] MAX_VAL
		}

#region 변수
		private Dictionary<EKey, System.Text.StringBuilder> m_oStrBuilderDict = new Dictionary<EKey, System.Text.StringBuilder>() {
			[EKey.STR_BUILDER_01] = new System.Text.StringBuilder(),
			[EKey.STR_BUILDER_02] = new System.Text.StringBuilder()
		};

		private Stopwatch m_oStopwatch = new Stopwatch();
		private Dictionary<EKey, Tween> m_oAniDict = new Dictionary<EKey, Tween>();

		/** =====> UI <===== */
		private Dictionary<EKey, TMP_Text> m_oTextDict = new Dictionary<EKey, TMP_Text>();
		private Dictionary<EKey, CGaugeHandler> m_oGaugeHandlerDict = new Dictionary<EKey, CGaugeHandler>();

		/** =====> 객체 <===== */
		private Dictionary<EKey, GameObject> m_oUIsDict = new Dictionary<EKey, GameObject>();
#endregion // 변수

#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 초기화 되었을 경우
			if(CSceneManager.IsInit) {
				this.SetupAwake();
			}
		}

		/** 제거 되었을 경우 */
		public override void OnDestroy() {
			base.OnDestroy();

			try {
				// 앱이 실행 중 일 경우
				if(CSceneManager.IsAppRunning) {
					m_oAniDict.GetValueOrDefault(EKey.LOADING_GAUGE_ANI)?.Kill();
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CSubStartSceneManager.OnDestroy Exception: {oException.Message}");
			}
		}

		/** 씬을 설정한다 */
		protected override void Setup() {
			base.Setup();
			this.UpdateUIsState();
		}

		/** 시작 씬 이벤트를 수신했을 경우 */
		protected override void OnReceiveStartSceneEvent(EStartSceneEvent a_eEvent) {
#if DEBUG || DEVELOPMENT
			CLocalizeInfoTable.Inst.TryGetFontSetInfo(string.Empty, SystemLanguage.English, EFontSet._1, out STFontSetInfo stFontSetInfo);

			try {
				m_oStrBuilderDict.GetValueOrDefault(EKey.STR_BUILDER_02).AppendLine($"{a_eEvent}: {m_oStopwatch.ElapsedMilliseconds} ms");
				m_oTextDict.GetValueOrDefault(EKey.SCENE_INFO_TEXT).ExSetText(m_oStrBuilderDict.GetValueOrDefault(EKey.STR_BUILDER_02).ToString(), stFontSetInfo);
			} finally {
				m_oStopwatch.Restart();
			}
#endif // #if DEBUG || DEVELOPMENT

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
			float fPercent = Mathf.Clamp01((int)(a_eEvent + KCDefine.B_VAL_1_INT) / (float)EStartSceneEvent.MAX_VAL);
			m_oAniDict.ExAssignVal(EKey.LOADING_GAUGE_ANI, m_oGaugeHandlerDict.GetValueOrDefault(EKey.LOADING_GAUGE_HANDLER).ExStartGaugeAni((a_fVal) => this.UpdateUIsState(), null, m_oGaugeHandlerDict.GetValueOrDefault(EKey.LOADING_GAUGE_HANDLER).Percent, fPercent, KCDefine.U_DURATION_ANI * KCDefine.B_VAL_2_REAL));
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		}

		/** 씬을 설정한다 */
		private void SetupAwake() {
#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
			// 객체를 설정한다 {
			CFunc.SetupObjs(new List<(EKey, string, GameObject, GameObject)>() {
				(EKey.LOADING_GAUGE, $"{EKey.LOADING_GAUGE}", this.UIs, CResManager.Inst.GetRes<GameObject>(KCDefine.SS_OBJ_P_LOADING_GAUGE))
			}, m_oUIsDict);

			m_oUIsDict.GetValueOrDefault(EKey.LOADING_GAUGE).transform.localPosition = KDefine.SS_POS_LOADING_GAUGE;
			// 객체를 설정한다 }

			// 텍스트를 설정한다 {
			CFunc.SetupComponents(new List<(EKey, string, GameObject, GameObject)>() {
				(EKey.LOADING_TEXT, $"{EKey.LOADING_TEXT}", this.UIs, CResManager.Inst.GetRes<GameObject>(KCDefine.SS_OBJ_P_LOADING_TEXT))
			}, m_oTextDict);

			m_oTextDict.GetValueOrDefault(EKey.LOADING_TEXT).transform.localPosition = KDefine.SS_POS_LOADING_TEXT;
			// 텍스트를 설정한다 }

			// 게이지 처리자를 설정한다
			CFunc.SetupComponents(new List<(EKey, GameObject)>() {
				(EKey.LOADING_GAUGE_HANDLER, m_oUIsDict.GetValueOrDefault(EKey.LOADING_GAUGE))
			}, m_oGaugeHandlerDict);

#if DEBUG || DEVELOPMENT
			// 텍스트를 설정한다 {
			CFunc.SetupComponents(new List<(EKey, string, GameObject, GameObject)>() {
				(EKey.SCENE_INFO_TEXT, $"{EKey.SCENE_INFO_TEXT}", this.UpLeftUIs, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_G_INFO_TEXT))
			}, m_oTextDict);

			m_oTextDict.GetValueOrDefault(EKey.SCENE_INFO_TEXT).rectTransform.pivot = KCDefine.B_ANCHOR_UP_LEFT;
			m_oTextDict.GetValueOrDefault(EKey.SCENE_INFO_TEXT).rectTransform.anchorMin = KCDefine.B_ANCHOR_UP_LEFT;
			m_oTextDict.GetValueOrDefault(EKey.SCENE_INFO_TEXT).rectTransform.anchorMax = KCDefine.B_ANCHOR_UP_LEFT;
			m_oTextDict.GetValueOrDefault(EKey.SCENE_INFO_TEXT).rectTransform.anchoredPosition = Vector3.zero;
			// 텍스트를 설정한다 }

			m_oStopwatch.Start();
			this.OnReceiveStartSceneEvent(EStartSceneEvent.LOAD_START_SCENE);
#endif // #if DEBUG || DEVELOPMENT
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
		}

		/** 텍스트 상태를 갱신한다 */
		private void UpdateUIsState() {
			m_oStrBuilderDict.GetValueOrDefault(EKey.STR_BUILDER_01).Clear();
			m_oStrBuilderDict.GetValueOrDefault(EKey.STR_BUILDER_01).Append(CStrTable.Inst.GetStr(KCDefine.ST_KEY_START_SM_LOADING_TEXT));

			string oPercentStr = string.Format(KCDefine.B_TEXT_FMT_1_INT, m_oGaugeHandlerDict.GetValueOrDefault(EKey.LOADING_GAUGE_HANDLER).Percent * KCDefine.B_UNIT_NORM_VAL_TO_PERCENT);
			oPercentStr = string.Format(KCDefine.B_TEXT_FMT_BRACKET, string.Format(KCDefine.B_TEXT_FMT_PERCENT, oPercentStr));

			CLocalizeInfoTable.Inst.TryGetFontSetInfo(string.Empty, SystemLanguage.English, EFontSet._1, out STFontSetInfo stFontSetInfo);
			m_oTextDict.GetValueOrDefault(EKey.LOADING_TEXT).ExSetText(string.Format(KCDefine.B_TEXT_FMT_2_SPACE_COMBINE, m_oStrBuilderDict.GetValueOrDefault(EKey.STR_BUILDER_01).ToString(), oPercentStr), stFontSetInfo);
		}
#endregion // 함수
	}
}
#endif // #if SCENE_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

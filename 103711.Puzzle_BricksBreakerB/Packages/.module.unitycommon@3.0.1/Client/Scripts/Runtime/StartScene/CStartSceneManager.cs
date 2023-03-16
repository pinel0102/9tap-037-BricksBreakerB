using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using DG.Tweening;

namespace StartScene {
	/** 시작 씬 관리자 */
	public abstract partial class CStartSceneManager : CSceneManager {
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

		#region 프로퍼티
		public override string SceneName => KCDefine.B_SCENE_N_START;
		public virtual bool IsIgnoreLoadingGauge => true;

		public virtual Vector3 LoadingTextPos => Vector3.zero;
		public virtual Vector3 LoadingGaugePos => Vector3.zero;

		protected Dictionary<string, int> MaxNumFXSndsDict { get; } = new Dictionary<string, int>();

#if UNITY_EDITOR
		public override int ScriptOrder => KCDefine.U_SCRIPT_O_START_SCENE_MANAGER;
#endif // #if UNITY_EDITOR
		#endregion // 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();

			// 초기화 되었을 경우
			if(CSceneManager.IsInit) {
				CCommonAppInfoStorage.Inst.IncrAppRunningTimes(KCDefine.B_VAL_1_INT);
				CCommonAppInfoStorage.Inst.SaveAppInfo();

				// 객체를 설정한다 {
				CFunc.SetupObjs(new List<(EKey, string, GameObject, GameObject)>() {
					(EKey.LOADING_GAUGE, $"{EKey.LOADING_GAUGE}", this.UIs, CResManager.Inst.GetRes<GameObject>(KCDefine.SS_OBJ_P_LOADING_GAUGE))
				}, m_oUIsDict);

				m_oUIsDict[EKey.LOADING_GAUGE].SetActive(!this.IsIgnoreLoadingGauge);
				m_oUIsDict[EKey.LOADING_GAUGE].transform.localPosition = this.LoadingGaugePos;
				// 객체를 설정한다 }

				// 텍스트를 설정한다 {
				CFunc.SetupComponents(new List<(EKey, string, GameObject, GameObject)>() {
					(EKey.LOADING_TEXT, $"{EKey.LOADING_TEXT}", m_oUIsDict[EKey.LOADING_GAUGE], CResManager.Inst.GetRes<GameObject>(KCDefine.SS_OBJ_P_LOADING_TEXT))
				}, m_oTextDict);

				m_oTextDict[EKey.LOADING_TEXT].transform.localPosition = this.LoadingTextPos;
				// 텍스트를 설정한다 }

				// 게이지 처리자를 설정한다
				CFunc.SetupComponents(new List<(EKey, GameObject)>() {
					(EKey.LOADING_GAUGE_HANDLER, m_oUIsDict[EKey.LOADING_GAUGE])
				}, m_oGaugeHandlerDict);

#if DEBUG || DEVELOPMENT
				// 텍스트를 설정한다 {
				CFunc.SetupComponents(new List<(EKey, string, GameObject, GameObject)>() {
					(EKey.SCENE_INFO_TEXT, $"{EKey.SCENE_INFO_TEXT}", this.UpLeftUIs, CResManager.Inst.GetRes<GameObject>(KCDefine.U_OBJ_P_G_INFO_TEXT))
				}, m_oTextDict);

				m_oTextDict[EKey.SCENE_INFO_TEXT].rectTransform.pivot = KCDefine.B_ANCHOR_UP_LEFT;
				m_oTextDict[EKey.SCENE_INFO_TEXT].rectTransform.anchorMin = KCDefine.B_ANCHOR_UP_LEFT;
				m_oTextDict[EKey.SCENE_INFO_TEXT].rectTransform.anchorMax = KCDefine.B_ANCHOR_UP_LEFT;
				m_oTextDict[EKey.SCENE_INFO_TEXT].rectTransform.anchoredPosition = Vector3.zero;
				// 텍스트를 설정한다 }

				m_oStopwatch.Start();
				this.OnReceiveStartSceneEvent(EStartSceneEvent.LOAD_START_SCENE);
#endif // #if DEBUG || DEVELOPMENT
			}
		}

		/** 초기화 */
		public sealed override void Start() {
			base.Start();

			// 초기화 되었을 경우
			if(CSceneManager.IsInit) {
				StartCoroutine(this.CoStart());
			}
		}

		/** 제거 되었을 경우 */
		public override void OnDestroy() {
			base.OnDestroy();

			try {
				// 앱이 실행 중 일 경우
				if(CSceneManager.IsAppRunning) {
					foreach(var stKeyVal in m_oAniDict) {
						stKeyVal.Value?.Kill();
					}
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CStartSceneManager.OnDestroy Exception: {oException.Message}");
			}
		}

		/** 씬을 설정한다 */
		protected virtual void Setup() {
			foreach(var stKeyVal in this.MaxNumFXSndsDict) {
				CSndManager.Inst.SetMaxNumFXSnds(stKeyVal.Key, stKeyVal.Value);
			}
		}

		/** 시작 씬 이벤트를 수신했을 경우 */
		protected virtual void OnReceiveStartSceneEvent(EStartSceneEvent a_eEvent) {
			float fPercent = Mathf.Clamp01((int)(a_eEvent + KCDefine.B_VAL_1_INT) / (float)EStartSceneEvent.MAX_VAL);
			m_oAniDict.ExAssignVal(EKey.LOADING_GAUGE_ANI, this.StartLoadingGaugeAni(m_oGaugeHandlerDict[EKey.LOADING_GAUGE_HANDLER], (a_fVal) => this.UpdateUIsState(), null, m_oGaugeHandlerDict[EKey.LOADING_GAUGE_HANDLER].Percent, fPercent, KCDefine.U_DURATION_ANI * KCDefine.B_VAL_2_REAL));

#if DEBUG || DEVELOPMENT
			CLocalizeInfoTable.Inst.TryGetFontSetInfo(string.Empty, SystemLanguage.English, EFontSet._1, out STFontSetInfo stFontSetInfo);

			try {
				m_oStrBuilderDict[EKey.STR_BUILDER_02].AppendLine($"{a_eEvent}: {m_oStopwatch.ElapsedMilliseconds} ms");
				m_oTextDict[EKey.SCENE_INFO_TEXT].ExSetText(m_oStrBuilderDict[EKey.STR_BUILDER_02].ToString(), stFontSetInfo);
			} finally {
				m_oStopwatch.Restart();
			}
#endif // #if DEBUG || DEVELOPMENT
		}

		/** 텍스트 상태를 갱신한다 */
		private void UpdateUIsState() {
			m_oStrBuilderDict[EKey.STR_BUILDER_01].Clear();
			m_oStrBuilderDict[EKey.STR_BUILDER_01].Append(CStrTable.Inst.GetStr(KCDefine.ST_KEY_START_SM_LOADING_TEXT));

			string oPercentStr = string.Format(KCDefine.B_TEXT_FMT_1_INT, m_oGaugeHandlerDict[EKey.LOADING_GAUGE_HANDLER].Percent * KCDefine.B_UNIT_NORM_VAL_TO_PERCENT);
			oPercentStr = string.Format(KCDefine.B_TEXT_FMT_BRACKET, string.Format(KCDefine.B_TEXT_FMT_PERCENT, oPercentStr));

			CLocalizeInfoTable.Inst.TryGetFontSetInfo(string.Empty, SystemLanguage.English, EFontSet._3, out STFontSetInfo stFontSetInfo);
			m_oTextDict[EKey.LOADING_TEXT].ExSetText(string.Format(KCDefine.B_TEXT_FMT_2_SPACE_COMBINE, m_oStrBuilderDict[EKey.STR_BUILDER_01].ToString(), oPercentStr), stFontSetInfo);
		}

		/** 로딩 게이지 애니메이션을 시작한다 */
		private Sequence StartLoadingGaugeAni(CGaugeHandler a_oGaugeHandler, System.Action<float> a_oCallback, System.Action<CGaugeHandler, Sequence> a_oCompleteCallback, float a_fStartVal, float a_fEndVal, float a_fDuration, Ease a_eEase = KCDefine.U_EASE_DEF, bool a_bIsRealtime = false, float a_fDelay = KCDefine.B_VAL_0_REAL) {
			CAccess.Assert(a_oGaugeHandler != null);
			return CFactory.MakeSequence(CFactory.MakeAni(() => a_oGaugeHandler.Percent, (a_fVal) => a_oGaugeHandler.SetPercent(a_fVal), () => a_oGaugeHandler.SetPercent(a_fStartVal), a_oCallback, a_fEndVal, a_fDuration, a_eEase, a_bIsRealtime), (a_oSequenceSender) => CFunc.Invoke(ref a_oCompleteCallback, a_oGaugeHandler, a_oSequenceSender), a_fDelay, false, a_bIsRealtime);
		}

		/** 초기화 */
		private IEnumerator CoStart() {
			yield return CFactory.CoCreateWaitForSecs(KCDefine.U_DELAY_INIT);
			this.Setup();

#if SCENE_TEMPLATES_MODULE_ENABLE
			CSceneLoader.Inst.LoadAdditiveScene(KCDefine.B_SCENE_N_SETUP);
#endif // #if SCENE_TEMPLATES_MODULE_ENABLE
		}
		#endregion // 함수
	}
}

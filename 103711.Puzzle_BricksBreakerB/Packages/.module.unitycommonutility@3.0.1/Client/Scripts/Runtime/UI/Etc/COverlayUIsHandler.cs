using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;

namespace GameScene {
	/** 중첩 UI 처리자 */
	public partial class COverlayUIsHandler : CComponent {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			SHOW_ANI,
			CLOSE_ANI,
			[HideInInspector] MAX_VAL
		}

		/** 콜백 */
		public enum ECallback {
			NONE = -1,
			MAKE_SHOW_ANI,
			MAKE_CLOSE_ANI,
			COMPLETE_SHOW_ANI,
			COMPLETE_CLOSE_ANI,
			[HideInInspector] MAX_VAL
		}

		/** 매개 변수 */
		public struct STParams {
			public Dictionary<ECallback, System.Action<COverlayUIsHandler>> m_oCallbackDict;
			public Dictionary<ECallback, System.Func<COverlayUIsHandler, Tween>> m_oFuncCallbackDict;
		}

		#region 변수
		private Dictionary<EKey, Tween> m_oAniDict = new Dictionary<EKey, Tween>();
		#endregion // 변수

		#region 프로퍼티
		public STParams Params { get; private set; }
		#endregion // 프로퍼티

		#region 함수
		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			this.Params = a_stParams;
		}

		/** 애니메이션을 리셋한다 */
		public virtual void ResetAni() {
			foreach(var stKeyVal in m_oAniDict) {
				stKeyVal.Value?.Kill();
			}
		}

		/** 제거 되었을 경우 */
		public override void OnDestroy() {
			base.OnDestroy();

			try {
				// 앱이 실행 중 일 경우
				if(CSceneManager.IsAppRunning) {
					this.ResetAni();
				}
			} catch(System.Exception oException) {
				CFunc.ShowLogWarning($"CPopup.OnDestroy Exception: {oException.Message}");
			}
		}

		/** 중첩 UI 를 출력한다 */
		public virtual void Show(bool a_bIsAni = true) {
			// 애니메이션 모드 일 경우
			if(a_bIsAni && !this.IsIgnoreAni) {
				m_oAniDict.ExAssignVal(EKey.SHOW_ANI, CFactory.MakeSequence(this.Params.m_oFuncCallbackDict.GetValueOrDefault(ECallback.MAKE_SHOW_ANI)?.Invoke(this), (a_oSender) => this.OnCompleteShowAni()));
			} else {
				this.ExLateCallFunc((a_oSender) => this.OnCompleteShowAni());
			}
		}

		/** 중첩 UI 를 닫는다 */
		public virtual void Close(bool a_bIsAni = true) {
			// 애니메이션 모드 일 경우
			if(a_bIsAni && !this.IsIgnoreAni) {
				m_oAniDict.ExAssignVal(EKey.CLOSE_ANI, CFactory.MakeSequence(this.Params.m_oFuncCallbackDict.GetValueOrDefault(ECallback.MAKE_CLOSE_ANI)?.Invoke(this), (a_oSender) => this.OnCompleteCloseAni()));
			} else {
				this.ExLateCallFunc((a_oSender) => this.OnCompleteCloseAni());
			}
		}

		/** 출력 애니메이션이 완료 되었을 경우 */
		protected virtual void OnCompleteShowAni() {
			this.Params.m_oCallbackDict.GetValueOrDefault(ECallback.COMPLETE_SHOW_ANI)?.Invoke(this);
		}

		/** 닫기 애니메이션이 완료 되었을 경우 */
		protected virtual void OnCompleteCloseAni() {
			this.Params.m_oCallbackDict.GetValueOrDefault(ECallback.COMPLETE_CLOSE_ANI)?.Invoke(this);
		}
		#endregion // 함수

		#region 클래스 함수
		/** 매개 변수를 생성한다 */
		public static STParams MakeParams(Dictionary<ECallback, System.Action<COverlayUIsHandler>> a_oCallbackDict = null, Dictionary<ECallback, System.Func<COverlayUIsHandler, Tween>> a_oFuncCallbackDict = null) {
			return new STParams() {
				m_oCallbackDict = a_oCallbackDict ?? new Dictionary<ECallback, System.Action<COverlayUIsHandler>>(),
				m_oFuncCallbackDict = a_oFuncCallbackDict ?? new Dictionary<ECallback, System.Func<COverlayUIsHandler, Tween>>()
			};
		}
		#endregion // 클래스 함수
	}
}

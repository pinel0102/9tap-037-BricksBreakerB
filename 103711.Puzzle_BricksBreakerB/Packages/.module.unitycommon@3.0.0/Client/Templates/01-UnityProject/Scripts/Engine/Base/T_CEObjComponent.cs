#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 엔진 객체 컴포넌트 */
	public abstract partial class CEObjComponent : CEComponent {
		/** 콜백 */
		public enum ECallback {
			NONE = -1,
			ENGINE_OBJ_EVENT,
			[HideInInspector] MAX_VAL
		}

		/** 매개 변수 */
		public new struct STParams {
			public CEComponent.STParams m_stBaseParams;
			public CEController m_oController;

			public Dictionary<ECallback, System.Action<CEObjComponent, EEngineObjEvent, string>> m_oCallbackDict;
		}

#region 변수

#endregion // 변수

#region 프로퍼티
		public new STParams Params { get; private set; }
		public CDictWrapper<EAbilityKinds, decimal> AbilityValDictWrapper { get; } = new CDictWrapper<EAbilityKinds, decimal>();
#endregion // 프로퍼티

#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();
			this.SubSetupAwake();
		}

		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			base.Init(a_stParams.m_stBaseParams);
			this.Params = a_stParams;

			this.SubInit();
		}

		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);
			this.Params.m_oController?.OnUpdate(a_fDeltaTime);
		}

		/** 어빌리티 값을 설정한다 */
		public virtual void SetupAbilityVals(bool a_bIsReset = true) {
			this.DoSetupAbilityVals(a_bIsReset);
			global::Func.SetupAbilityVals(this.AbilityValDictWrapper.m_oDict03, this.AbilityValDictWrapper.m_oDict02);

			// 리셋 모드 일 경우
			if(a_bIsReset) {
				this.AbilityValDictWrapper.m_oDict02.ExCopyTo(this.AbilityValDictWrapper.m_oDict01, (a_dmAbilityVal) => a_dmAbilityVal);
			} else {
				foreach(var stKeyVal in this.AbilityValDictWrapper.m_oDict02) {
					decimal dmAbilityVal = this.AbilityValDictWrapper.m_oDict01.GetValueOrDefault(stKeyVal.Key);
					this.AbilityValDictWrapper.m_oDict01.ExReplaceVal(stKeyVal.Key, CAbilityInfoTable.Inst.GetAbilityInfo(stKeyVal.Key).m_stCommonInfo.m_bIsFlags01 ? System.Math.Min(dmAbilityVal, stKeyVal.Value) : stKeyVal.Value);
				}
			}
		}

		/** 어빌리티 값을 설정한다 */
		protected virtual void DoSetupAbilityVals(bool a_bIsReset = true) {
			// 리셋 모드 일 경우
			if(a_bIsReset) {
				this.AbilityValDictWrapper.m_oDict01.Clear();
			}

			this.AbilityValDictWrapper.m_oDict02.Clear();
		}
#endregion // 함수

#region 제네릭 접근자 함수
		/** 제어자를 반환한다 */
		public T GetController<T>() where T : CEController {
			return this.Params.m_oController as T;
		}
#endregion // 제네릭 접근자 함수

#region 클래스 함수
		/** 매개 변수를 생성한다 */
		public static STParams MakeParams(CEngine a_oEngine, CEController a_oController, string a_oObjsPoolKey, Dictionary<CEObjComponent.ECallback, System.Action<CEObjComponent, EEngineObjEvent, string>> a_oCallbackDict = null) {
			return new STParams() {
				m_stBaseParams = CEComponent.MakeParams(a_oEngine, a_oObjsPoolKey), m_oController = a_oController, m_oCallbackDict = a_oCallbackDict ?? new Dictionary<CEObjComponent.ECallback, System.Action<CEObjComponent, EEngineObjEvent, string>>()
			};
		}
#endregion // 클래스 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

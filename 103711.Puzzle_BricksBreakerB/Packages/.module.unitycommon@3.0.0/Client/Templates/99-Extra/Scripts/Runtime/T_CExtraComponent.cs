#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE
/** 추가 컴포넌트 */
public partial class CExtraComponent : CComponent {
	/** 식별자 */
	private enum EKey {
		NONE = -1,
		[HideInInspector] MAX_VAL
	}

	/** 매개 변수 */
	public struct STParams {
		// Do Something
	}

#region 변수

#endregion // 변수

#region 프로퍼티
	public STParams Params { get; private set; }
#endregion // 프로퍼티

#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		this.Params = a_stParams;
	}
#endregion // 함수

#region 클래스 함수
	/** 매개 변수를 생성한다 */
	public static STParams MakeParams() {
		return new STParams() {
			// Do Something
		};
	}
#endregion // 클래스 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** 싱글턴 */
public abstract partial class CSingleton<T> : CComponent where T : CSingleton<T> {
	#region 클래스 변수
	private static T m_tInst = null;
	#endregion // 클래스 변수

	#region 클래스 프로퍼티
	public static T Inst {
		get {
			// 인스턴스가 없을 경우
			if(!CSceneManager.IsAppQuit && CSingleton<T>.m_tInst == null) {
				CSingleton<T>.m_tInst = CFactory.CreateObj<T>(typeof(T).ToString(), null);
				CAccess.Assert(CSingleton<T>.m_tInst != null);
			}

			return CSingleton<T>.m_tInst;
		}
	}
	#endregion // 클래스 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
		CSingleton<T>.m_tInst = this as T;

		DontDestroyOnLoad(CSingleton<T>.m_tInst.gameObject);
	}
	#endregion // 함수

	#region 클래스 함수
	/** 인스턴스를 생성한다 */
	public static T Create() {
		return CSingleton<T>.Inst;
	}
	#endregion // 클래스 함수

	#region 조건부 함수
#if UNITY_EDITOR
	/** 스크립트 순서를 설정한다 */
	protected override void SetupScriptOrder() {
		base.SetupScriptOrder();
		this.ExSetScriptOrder(KCDefine.U_SCRIPT_O_SINGLETON);
	}
#endif // #if UNITY_EDITOR
	#endregion // 조건부 함수
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** 스크립트 객체 */
public abstract partial class CScriptableObj<T> : ScriptableObject where T : CScriptableObj<T> {
	#region 클래스 변수
	private static T m_tInst = null;
	private static string m_oFilePath = string.Empty;
	#endregion // 클래스 변수

	#region 클래스 프로퍼티
	public static T Inst {
		get {
			// 인스턴스가 없을 경우
			if(!CSceneManager.IsAppQuit && CScriptableObj<T>.m_tInst == null) {
				CScriptableObj<T>.m_tInst = CResManager.Inst.GetScriptableObj<T>(CScriptableObj<T>.m_oFilePath);
				CScriptableObj<T>.m_tInst.Awake();
			}

			return CScriptableObj<T>.m_tInst;
		}
	}
	#endregion // 클래스 프로퍼티

	#region 함수
	/** 초기화 */
	public virtual void Awake() {
		// Do Something
	}
	#endregion // 함수

	#region 클래스 함수
	/** 인스턴스를 생성한다 */
	public static T Create(string a_oFilePath) {
		CAccess.Assert(a_oFilePath.ExIsValid());
		CScriptableObj<T>.m_oFilePath = a_oFilePath;

		return CScriptableObj<T>.Inst;
	}
	#endregion // 클래스 함수
}

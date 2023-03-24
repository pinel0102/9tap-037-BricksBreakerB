using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using AOP;

/** 씬 관리자 - 팩토리 */
public abstract partial class CSceneManager : CComponent {
	#region 함수
	/** 객체를 활성화한다 */
	public GameObject SpawnObj(string a_oName, string a_oObjsPoolKey) {
		CAccess.Assert(a_oName.ExIsValid() && a_oObjsPoolKey.ExIsValid());
		return this.SpawnObj(a_oName, a_oObjsPoolKey, Vector3.zero);
	}

	/** 객체를 활성화한다 */
	public GameObject SpawnObj(string a_oName, string a_oObjsPoolKey, Vector3 a_stPos) {
		CAccess.Assert(a_oName.ExIsValid() && a_oObjsPoolKey.ExIsValid());
		return this.SpawnObj(a_oName, a_oObjsPoolKey, Vector3.one, Vector3.zero, a_stPos);
	}

    /** 객체를 활성화한다 */
	public GameObject SpawnObj(string a_oName, string a_oObjsPoolKey, Vector3 a_stPos, bool _isWorldPosition = false) {
		CAccess.Assert(a_oName.ExIsValid() && a_oObjsPoolKey.ExIsValid() && m_oObjsPoolDict.ContainsKey(a_oObjsPoolKey));

		var oObj = m_oObjsPoolDict[a_oObjsPoolKey].Spawn(a_oName);
		
        if (_isWorldPosition)
            oObj.transform.position = a_stPos;
        else
		    oObj.transform.localPosition = a_stPos;

		return oObj;
	}

    /** 객체를 활성화한다 */
	public GameObject SpawnObj(string a_oName, string a_oObjsPoolKey, Vector3 a_stScale, Vector3 a_stPos, bool _isWorldPosition = false) {
		CAccess.Assert(a_oName.ExIsValid() && a_oObjsPoolKey.ExIsValid() && m_oObjsPoolDict.ContainsKey(a_oObjsPoolKey));

		var oObj = m_oObjsPoolDict[a_oObjsPoolKey].Spawn(a_oName);
		oObj.transform.localScale = a_stScale;
		
        if (_isWorldPosition)
            oObj.transform.position = a_stPos;
        else
		    oObj.transform.localPosition = a_stPos;

		return oObj;
	}

	/** 객체를 활성화한다 */
	public GameObject SpawnObj(string a_oName, string a_oObjsPoolKey, Vector3 a_stScale, Vector3 a_stAngle, Vector3 a_stPos, bool _isWorldPosition = false) {
		CAccess.Assert(a_oName.ExIsValid() && a_oObjsPoolKey.ExIsValid() && m_oObjsPoolDict.ContainsKey(a_oObjsPoolKey));

		var oObj = m_oObjsPoolDict[a_oObjsPoolKey].Spawn(a_oName);
		oObj.transform.localScale = a_stScale;
		oObj.transform.localEulerAngles = a_stAngle;

        if (_isWorldPosition)
            oObj.transform.position = a_stPos;
        else
		    oObj.transform.localPosition = a_stPos;

		return oObj;
	}

	/** 객체를 비활성화한다 */
	public void DespawnObj(string a_oObjsPoolKey, GameObject a_oObj, float a_fDelay = KCDefine.B_VAL_0_REAL, bool a_bIsDestroy = false, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || (a_oObjsPoolKey.ExIsValid() && m_oObjsPoolDict.ContainsKey(a_oObjsPoolKey)));

		// 객체 풀이 존재 할 경우
		if(a_oObjsPoolKey.ExIsValid() && m_oObjsPoolDict.TryGetValue(a_oObjsPoolKey, out ObjectPool oObjsPool)) {
			m_oDespawnObjInfoList.Add(new STDespawnObjInfo() {
				m_bIsDestroy = a_bIsDestroy,
				m_oObjsPoolKey = a_oObjsPoolKey,
				m_stDespawnTime = System.DateTime.Now.AddSeconds(a_fDelay),
				m_oObj = a_oObj
			});
		}
	}
	#endregion // 함수

	#region 조건부 함수
#if DOTWEEN_MODULE_ENABLE || EXTRA_SCRIPT_MODULE_ENABLE
	/** 화면 페이드 인 애니메이션을 생성한다 */
	protected virtual Tween MakeScreenFadeInAni(GameObject a_oTarget, string a_oKey, Color a_stColor, float a_fDuration) {
		CAccess.Assert(a_oTarget != null && a_oKey.ExIsValid());
		return a_oTarget.GetComponentInChildren<Image>().DOColor(a_stColor, a_fDuration).SetEase(KCDefine.U_EASE_DEF).SetUpdate(this.IsRealtimeFadeInAni);
	}

	/** 화면 페이드 아웃 애니메이션을 생성한다 */
	protected virtual Tween MakeScreenFadeOutAni(GameObject a_oTarget, string a_oKey, Color a_stColor, float a_fDuration) {
		CAccess.Assert(a_oTarget != null && a_oKey.ExIsValid());
		return a_oTarget.GetComponentInChildren<Image>().DOColor(a_stColor, a_fDuration).SetEase(KCDefine.U_EASE_DEF).SetUpdate(this.IsRealtimeFadeOutAni);
	}
#endif // #if DOTWEEN_MODULE_ENABLE || EXTRA_SCRIPT_MODULE_ENABLE
	#endregion // 조건부 함수

	#region 제네릭 함수
	/** 객체를 활성화한다 */
	public T SpawnObj<T>(string a_oName, string a_oObjsPoolKey) where T : Component {
		CAccess.Assert(a_oName.ExIsValid() && a_oObjsPoolKey.ExIsValid());
		return this.SpawnObj<T>(a_oName, a_oObjsPoolKey, Vector3.zero);
	}

	/** 객체를 활성화한다 */
	public T SpawnObj<T>(string a_oName, string a_oObjsPoolKey, Vector3 a_stPos) where T : Component {
		CAccess.Assert(a_oName.ExIsValid() && a_oObjsPoolKey.ExIsValid());
		return this.SpawnObj<T>(a_oName, a_oObjsPoolKey, Vector3.one, Vector3.zero, a_stPos);
	}

    /** 객체를 활성화한다 */
	public T SpawnObj<T>(string a_oName, string a_oObjsPoolKey, Vector3 a_stPos, bool _isWorldPosition = false) where T : Component {
		CAccess.Assert(a_oName.ExIsValid() && a_oObjsPoolKey.ExIsValid());
		return this.SpawnObj(a_oName, a_oObjsPoolKey, a_stPos, _isWorldPosition)?.GetComponentInChildren<T>();
	}

    /** 객체를 활성화한다 */
	public T SpawnObj<T>(string a_oName, string a_oObjsPoolKey, Vector3 a_stScale, Vector3 a_stPos, bool _isWorldPosition = false) where T : Component {
		CAccess.Assert(a_oName.ExIsValid() && a_oObjsPoolKey.ExIsValid());
		return this.SpawnObj(a_oName, a_oObjsPoolKey, a_stScale, a_stPos, _isWorldPosition)?.GetComponentInChildren<T>();
	}

	/** 객체를 활성화한다 */
	public T SpawnObj<T>(string a_oName, string a_oObjsPoolKey, Vector3 a_stScale, Vector3 a_stAngle, Vector3 a_stPos, bool _isWorldPosition = false) where T : Component {
		CAccess.Assert(a_oName.ExIsValid() && a_oObjsPoolKey.ExIsValid());
		return this.SpawnObj(a_oName, a_oObjsPoolKey, a_stScale, a_stAngle, a_stPos, _isWorldPosition)?.GetComponentInChildren<T>();
	}
	#endregion // 제네릭 함수
}

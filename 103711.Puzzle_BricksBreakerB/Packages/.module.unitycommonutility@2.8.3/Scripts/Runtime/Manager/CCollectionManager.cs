using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/** 컬렉션 관리자 */
public partial class CCollectionManager : CSingleton<CCollectionManager> {
	#region 변수
	private Dictionary<System.Type, List<IEnumerable>> m_oSetContainer = new Dictionary<System.Type, List<IEnumerable>>();
	private Dictionary<System.Type, List<IEnumerable>> m_oListContainer = new Dictionary<System.Type, List<IEnumerable>>();
	private Dictionary<System.Type, List<IEnumerable>> m_oDictContainer = new Dictionary<System.Type, List<IEnumerable>>();
	#endregion // 변수

	#region 함수
	/** 상태를 리셋한다 */
	public override void Reset() {
		base.Reset();

		m_oSetContainer.Clear();
		m_oListContainer.Clear();
		m_oDictContainer.Clear();
	}

	/** 제거 되었을 경우 */
	public override void OnDestroy() {
		base.OnDestroy();

		try {
			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				CSceneManager.SetCollectionManager(null);
			}
		} catch(System.Exception oException) {
			CFunc.ShowLogWarning($"CCollectionManager.OnDestroy Exception: {oException.Message}");
		}
	}

	/** 셋을 활성화한다 */
	public HashSet<T> SpawnSet<T>(HashSet<T> a_oDefValSet = null) {
		var oSetContainer = this.GetContainer(m_oSetContainer, typeof(HashSet<T>));
		CAccess.Assert(oSetContainer != null);

		var oSet = oSetContainer.ExGetVal(KCDefine.B_VAL_0_INT, null) ?? new HashSet<T>();
		a_oDefValSet?.ExCopyTo(oSet as HashSet<T>, (a_tVal) => a_tVal, true, false);

		oSetContainer.ExRemoveVal(oSet);
		return oSet as HashSet<T>;
	}

	/** 리스트를 활성화한다 */
	public List<T> SpawnList<T>(List<T> a_oDefValList = null) {
		var oListContainer = this.GetContainer(m_oListContainer, typeof(List<T>));
		CAccess.Assert(oListContainer != null);

		var oList = oListContainer.ExGetVal(KCDefine.B_VAL_0_INT, null) ?? new List<T>();
		a_oDefValList?.ExCopyTo(oList as List<T>, (a_tVal) => a_tVal, true, false);

		oListContainer.ExRemoveVal(oList);
		return oList as List<T>;
	}

	/** 딕셔너리를 활성화한다 */
	public Dictionary<K, V> SpawnDict<K, V>(Dictionary<K, V> a_oDefValDict = null) {
		var oDictContainer = this.GetContainer(m_oDictContainer, typeof(Dictionary<K, V>));
		CAccess.Assert(oDictContainer != null);

		var oDict = oDictContainer.ExGetVal(KCDefine.B_VAL_0_INT, null) ?? new Dictionary<K, V>();
		a_oDefValDict?.ExCopyTo(oDict as Dictionary<K, V>, (a_tVal) => a_tVal, true, false);

		oDictContainer.ExRemoveVal(oDict);
		return oDict as Dictionary<K, V>;
	}

	/** 셋을 비활성화한다 */
	public void DespawnSet<T>(HashSet<T> a_oSet, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oSet != null);

		// 셋이 존재 할 경우
		if(a_oSet != null) {
			a_oSet.Clear();
			this.GetContainer(m_oSetContainer, typeof(HashSet<T>)).ExAddVal(a_oSet);
		}
	}

	/** 리스트를 비활성화한다 */
	public void DespawnList<T>(List<T> a_oList, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oList != null);

		// 리스트가 존재 할 경우
		if(a_oList != null) {
			a_oList.Clear();
			this.GetContainer(m_oListContainer, typeof(List<T>)).ExAddVal(a_oList);
		}
	}

	/** 딕셔너리를 비활성화한다 */
	public void DespawnDict<K, V>(Dictionary<K, V> a_oDict, bool a_bIsEnableAssert = true) {
		CAccess.Assert(!a_bIsEnableAssert || a_oDict != null);

		// 딕셔너리가 존재 할 경우
		if(a_oDict != null) {
			a_oDict.Clear();
			this.GetContainer(m_oDictContainer, typeof(Dictionary<K, V>)).ExAddVal(a_oDict);
		}
	}

	/** 컨테이너를 반환한다 */
	private List<IEnumerable> GetContainer(Dictionary<System.Type, List<IEnumerable>> a_oDictContainer, System.Type a_oType) {
		var oContainer = a_oDictContainer.GetValueOrDefault(a_oType) ?? new List<IEnumerable>();
		a_oDictContainer.TryAdd(a_oType, oContainer);

		return oContainer;
	}
	#endregion // 함수
}

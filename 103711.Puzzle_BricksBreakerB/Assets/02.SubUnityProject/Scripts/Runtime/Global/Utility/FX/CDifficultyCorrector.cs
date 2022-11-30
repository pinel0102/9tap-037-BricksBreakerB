using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 난이도 보정자 */
public partial class CDifficultyCorrector : CComponent {
	/** 서브 식별자 */
	private enum ESubKey {
		NONE = -1,
		[HideInInspector] MAX_VAL
	}

#region 변수
	[SerializeField] private string m_oBasePath = string.Empty;
	[SerializeField] private EDifficulty m_eDifficulty = EDifficulty.NONE;
#endregion // 변수

#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
	}

	/** 초기화 */
	public override void Start() {
		base.Start();
		this.SetupDifficulty();
	}

	/** 난이도를 리셋한다 */
	public virtual void ResetDifficulty() {
		this.SetupDifficulty();
	}

	/** 이미지를 변경한다 */
	public void SetImg(string a_oBasePath) {
		m_oBasePath = a_oBasePath;
		this.SetupDifficulty();
	}

	/** 난이도를 변경한다 */
	public void SetDifficulty(EDifficulty a_eMode) {
		m_eDifficulty = a_eMode;
		this.SetupDifficulty();
	}

	/** 난이도를 설정한다 */
	private void SetupDifficulty() {
		// Do Something
	}
#endregion // 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

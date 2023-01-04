using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)
/** 서브 에디터 생성 팝업 */
public partial class CSubEditorCreatePopup : CEditorCreatePopup {
	/** 매개 변수 */
	public new struct STParams {
		public CEditorCreatePopup.STParams m_stBaseParams;
	}

#region 변수

#endregion // 변수

#region 프로퍼티
	public new STParams Params { get; private set; }
#endregion // 프로퍼티

#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init(a_stParams.m_stBaseParams);
		this.Params = a_stParams;
	}

	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {
		// Do Something
	}

	/** 에디터 레벨 생성 정보를 생성한다 */
	protected override CEditorCreateInfo CreateEditorCreateInfo() {
		var oCreateInfo = base.CreateEditorCreateInfo();

		return new CSubEditorCreateInfo() {
			m_nNumLevels = oCreateInfo.m_nNumLevels, m_stMinNumCells = oCreateInfo.m_stMinNumCells, m_stMaxNumCells = oCreateInfo.m_stMaxNumCells
		};
	}
#endregion // 함수
}
#endif // #if EDITOR_SCENE_TEMPLATES_MODULE_ENABLE && (UNITY_EDITOR || UNITY_STANDALONE) && (DEBUG || DEVELOPMENT_BUILD)

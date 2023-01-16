#if SCRIPT_TEMPLATE_ONLY
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using TMPro;
using EnhancedUI.EnhancedScroller;

/** 챕터 스크롤러 셀 뷰 */
public partial class CChapterScrollerCellView : CScrollerCellView {
	/** 매개 변수 */
	public new struct STParams {
		public CScrollerCellView.STParams m_stBaseParams;
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

		for(int i = 0; i < this.ScrollerCellList.Count; ++i) {
			var stIDInfo = new STIDInfo(KCDefine.B_VAL_0_INT, KCDefine.B_VAL_0_INT, i + base.Params.m_nID.ExULevelIDToChapterID());

			this.UpdateScrollerCellState(this.ScrollerCellList[i], stIDInfo);
			this.ScrollerCellList[i]?.SetActive(stIDInfo.m_nID03 < Access.NumChapterEpisodes);
		}
	}

	/** 스크롤러 셀 상태를 갱신한다 */
	private void UpdateScrollerCellState(GameObject a_oScrollerCell, STIDInfo a_stIDInfo) {
		// 버튼을 갱신한다 {
		var oSelBtn = a_oScrollerCell.GetComponentInChildren<Button>();
		oSelBtn?.ExAddListener(() => base.Params.m_oCallbackDict.GetValueOrDefault(ECallback.SEL)?.Invoke(this, CFactory.MakeULevelID(a_stIDInfo.m_nID01, a_stIDInfo.m_nID02, a_stIDInfo.m_nID03)), true, false);

#if PLAY_TEST_ENABLE
		oSelBtn?.ExSetInteractable(true, false);
#else
		oSelBtn?.ExSetInteractable(a_stIDInfo.m_nID03 <= Access.GetNumChapterClearInfos(CGameInfoStorage.Inst.PlayCharacterID), false);
#endif // #if PLAY_TEST_ENABLE
		// 버튼을 갱신한다 }
		
		// 챕터 정보가 존재 할 경우
		if(a_stIDInfo.m_nID03 < Access.NumChapterEpisodes) {
			CEpisodeInfoTable.Inst.TryGetChapterEpisodeInfo(a_stIDInfo.m_nID03, out STEpisodeInfo stChapterEpisodeInfo);

			// 텍스트를 갱신한다
			var oChapterText = a_oScrollerCell.ExFindComponent<TMP_Text>(KCDefine.U_OBJ_N_CHAPTER_TEXT);
			oChapterText?.ExSetText($"{a_stIDInfo.m_nID03 + KCDefine.B_VAL_1_INT}", EFontSet._1, false);
		}
	}
#endregion // 함수

#region 클래스 함수
	/** 매개 변수를 생성한다 */
	public new static STParams MakeParams(ulong a_nID, EnhancedScroller a_oScroller, Dictionary<ECallback, System.Action<CScrollerCellView, ulong>> a_oCallbackDict = null) {
		return new STParams() {
			m_stBaseParams = CScrollerCellView.MakeParams(a_nID, a_oScroller, a_oCallbackDict ?? new Dictionary<ECallback, System.Action<CScrollerCellView, ulong>>())
		};
	}
#endregion // 클래스 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
#endif // #if SCRIPT_TEMPLATE_ONLY

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
/** 튜토리얼 팝업 */
public partial class CTutorialPopup : CFocusPopup {
	/* 매개 변수 */
	public new struct STParams {
		public CFocusPopup.STParams m_stBaseParams;
		public ETutorialKinds m_eTutorialKinds;
	}
    
	#region 변수
    public List<GameObject> pages;
    public GameObject prevButton;
    public GameObject nextButton;
    public TMP_Text pageText;
    public int currentPage;

	#endregion // 변수

	#region 프로퍼티
	public new STParams Params { get; private set; }
	#endregion // 프로퍼티

	#region 함수
	/** 초기화 */
	public override void Awake() {
		base.Awake();

        this.SetPage(0);

        this.SubAwake();
	}

	/** 초기화 */
	public virtual void Init(STParams a_stParams) {
		base.Init(a_stParams.m_stBaseParams);
		this.Params = a_stParams;

        this.SubInit();
	}

    public void OnTouchNextBtn()
    {
        if (currentPage < pages.Count - 1)
            this.SetPage(currentPage + 1);
    }

    public void OnTouchPrevBtn()
    {
        if (currentPage > 0)
            this.SetPage(currentPage - 1);
    }

    private void SetPage(int index)
    {
        currentPage = index;
        for (int i=0; i < pages.Count; i++)
        {
            pages[i].SetActive(i == currentPage);
        }

        prevButton.SetActive(currentPage > 0);
        nextButton.SetActive(currentPage < pages.Count - 1);

        pageText.text = $"{currentPage+1}/{pages.Count}";
    }

	/** 팝업 컨텐츠를 설정한다 */
	protected override void SetupContents() {
		base.SetupContents();
		this.UpdateUIsState();
	}

	/** UI 상태를 갱신한다 */
	private void UpdateUIsState() {
		this.SubUpdateUIsState();
	}
	#endregion // 함수
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE







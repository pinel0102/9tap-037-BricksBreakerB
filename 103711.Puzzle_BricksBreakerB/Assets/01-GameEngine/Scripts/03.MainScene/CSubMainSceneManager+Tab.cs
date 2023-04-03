using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using EnhancedUI.EnhancedScroller;

namespace MainScene {
	/** 서브 메인 씬 관리자 - 추가 */
	public partial class CSubMainSceneManager : CMainSceneManager, IEnhancedScrollerDelegate 
    {
        public List<TabItem> tabList = new List<TabItem>();

        public void InitTabs()
        {
            for(int i=0; i < tabList.Count; i++)
            {
                tabList[i].HideItemInit();
            }
        }

        public void CloseTabs()
        {
            for(int i=0; i < tabList.Count; i++)
            {
                tabList[i].HideItem();
            }
        }

        public void OpenTab(int index)
        {
            for(int i=0; i < tabList.Count; i++)
            {
                if (i == index)
                {
                    tabList[i].ShowItem();
                }
                else
                {
                    tabList[i].HideItem();
                }
            }
        }
    }
}

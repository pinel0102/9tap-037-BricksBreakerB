using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GameScene {
    public partial class CSubGameSceneManager : CGameSceneManager
    {
        public List<TabItem> tabList = new List<TabItem>();
        public int currentTab = -1;

        public void InitTabs()
        {
            for(int i=0; i < tabList.Count; i++)
            {
                tabList[i].HideItemInit();
            }

            currentTab = -1;
        }

        public void CloseTabs()
        {
            for(int i=0; i < tabList.Count; i++)
            {
                tabList[i].HideItem();
            }

            currentTab = -1;
        }

        public void OpenTab(int index)
        {
            for(int i=0; i < tabList.Count; i++)
            {
                if (i == index)
                {
                    tabList[i].ShowItem();
                    currentTab = i;
                }
                else
                {
                    tabList[i].HideItem();
                }
            }
        }

        public bool IsTabMoving()
        {
            for(int i=0; i < tabList.Count; i++)
            {
                if (tabList[i].isMoving)
                    return true;
            }

            return false;
        }
    }
}
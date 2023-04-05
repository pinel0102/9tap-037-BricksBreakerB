using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelList : MonoBehaviour
{
    public List<LevelButton> levelList;

    public void Initialize(int _startLevel, int _levelCount)
    {
        for (int i=0; i < levelList.Count; i++)
        {
            levelList[i].Initialize(_startLevel + i, _levelCount);
        }
    }
}

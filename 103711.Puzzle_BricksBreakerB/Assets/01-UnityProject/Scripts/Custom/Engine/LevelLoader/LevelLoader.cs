using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class LevelLoader : CSingleton<LevelLoader>
{
    public LevelInfo levelInfo;

    public override void Awake()
    {
        base.Awake();

        Initialize();
    }

    public void Initialize()
    {
        //
    }

    public void LoadLevelData(int _level, LevelType _type = LevelType.DEFAULT)
    {
        levelInfo = new LevelInfo();

        levelInfo.type = _type;

        //JSONNode topNode = JSON.Parse(userData.UserInfo.UpgradeInfo);
        
        //levelInfo.gridSize = 
        //levelInfo.gridInfo = 
    }
}

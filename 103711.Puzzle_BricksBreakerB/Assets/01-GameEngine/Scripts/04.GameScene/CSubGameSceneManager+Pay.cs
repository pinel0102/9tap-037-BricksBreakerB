using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GameScene {
    public partial class CSubGameSceneManager : CGameSceneManager
    {
        public void GetContinueBonus()
        {
            OnClick_Bottom_BricksDelete();
            OnClick_Bottom_BricksDelete();
            OnClick_Bottom_BricksDelete();

            Engine.RefreshActiveCells();
            Engine.CheckDeadLine();
            Engine.isLevelFail = false;
        }
    }
}
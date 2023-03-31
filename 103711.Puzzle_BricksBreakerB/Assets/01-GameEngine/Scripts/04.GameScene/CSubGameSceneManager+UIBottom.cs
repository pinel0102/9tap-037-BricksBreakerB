using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GameScene {
    public partial class CSubGameSceneManager : CGameSceneManager
    {
        [Header("★ [Reference] UI Bottom")]
        public List<Button> bottomItemsButton = new List<Button>();
        public List<GameObject> bottomItemsDisabled = new List<GameObject>();

        private void SetupBottomButtons()
        {
            bottomItemsButton[0]?.ExAddListener(OnClick_Bottom_Earthquake);
            bottomItemsButton[1]?.ExAddListener(OnClick_Bottom_AddBall);
            bottomItemsButton[2]?.ExAddListener(OnClick_Bottom_BricksDelete);
            bottomItemsButton[3]?.ExAddListener(OnClick_Bottom_AddLaserBricks);
            bottomItemsButton[4]?.ExAddListener(OnClick_Bottom_AddSteelBricks);
        }

        public void HideShootUIs()
        {
            for(int i = 0; i < m_oShootUIsList.Count; ++i) {
				m_oShootUIsList[i].SetActive(false);
			}
        }

#region Buttons

        public void OnClick_Bottom_Earthquake()
        {
            var targetList = Engine.GetAllCells_SkillTarget();

            for(int i=0; i < targetList.Count; i++)
            {
                var target = targetList[i];
                int damage = target.Params.m_stObjInfo.m_bIsShieldCell ? Mathf.Max(1, (int)(target.CellObjInfo.SHIELD * 0.4f)) : Mathf.Max(1, (int)(target.CellObjInfo.HP * 0.4f));

                Engine.CellDamage_SkillTarget(target, Engine.BallObjList[0].GetComponent<NSEngine.CEBallObjController>(), damage);
            }
        }

        public void OnClick_Bottom_AddBall()
        {
            int addCount = 30;

            Engine.AddNormalBallsOnce(Engine.BallObjList[0].transform.position, addCount, false);

            Engine.BallObjList[0].NumText.text = string.Format("{0}", Engine.BallObjList.Count);
        }

        public void OnClick_Bottom_BricksDelete()
        {
            var lastClearTarget = Engine.GetLastClearTarget();

            for(int i = 0; i < Engine.CellObjLists.GetLength(KCDefine.B_VAL_1_INT); ++i) 
            {
                Engine.CellDestroy_SkillTarget(lastClearTarget.row, i);
            }

            Engine.CheckClear(true, true);
        }

        public void OnClick_Bottom_AddLaserBricks()
        {
            //
        }

        public void OnClick_Bottom_AddSteelBricks()
        {
            //
        }

        public void ToggleAimLayer()
        {
            m_oEngine.ToggleAimLayer();

            goldenAimOn.SetActive(m_oEngine.isGoldAim);
        }

#endregion Buttons
    }
}
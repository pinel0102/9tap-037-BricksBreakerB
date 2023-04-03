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
            if (Engine.PlayState == NSEngine.CEngine.EPlayState.SHOOT) return;
            if (isShaking) return;

            float damageRatio = 0.4f;

            ShakeCamera(() => {

                var targetList = Engine.GetAllCells_SkillTarget();
                for(int i=0; i < targetList.Count; i++)
                {
                    var target = targetList[i];
                    int damage = target.Params.m_stObjInfo.m_bIsShieldCell ? Mathf.Max(1, (int)(target.CellObjInfo.SHIELD * damageRatio)) : Mathf.Max(1, (int)(target.CellObjInfo.HP * damageRatio));

                    Engine.CellDamage_SkillTarget(target, Engine.BallObjList[0].GetComponent<NSEngine.CEBallObjController>(), damage);
                }

                Engine.CheckClear(true, true);

            });
        }

        public void OnClick_Bottom_AddBall()
        {
            if (Engine.PlayState == NSEngine.CEngine.EPlayState.SHOOT) return;

            int addCount = 30;

            Engine.AddNormalBallsOnce(Engine.BallObjList[0].transform.position, addCount, false);

            Engine.BallObjList[0].NumText.text = string.Format("{0}", Engine.BallObjList.Count);
        }

        public void OnClick_Bottom_BricksDelete()
        {
            if (Engine.PlayState == NSEngine.CEngine.EPlayState.SHOOT) return;

            var lastClearTarget = Engine.GetLastClearTarget();
            if (lastClearTarget != null)
            {
                for(int i = 0; i < Engine.CellObjLists.GetLength(KCDefine.B_VAL_1_INT); ++i) 
                {
                    Engine.CellDestroy_SkillTarget(lastClearTarget.row, i);
                }
            }

            Engine.CheckClear(true, true);
        }

        public void OnClick_Bottom_AddLaserBricks()
        {
            if (Engine.PlayState == NSEngine.CEngine.EPlayState.SHOOT) return;

            int addCount = 4;

            var targetList = Engine.GetRandomEmptyCells(Mathf.Max(0, Engine.viewSize.y - Engine.viewSize.x), Engine.viewSize.y, addCount);
            for(int i=0; i < targetList.Count; i++)
            {
                EObjKinds kinds = Random.Range(0, 100) < 50 ? EObjKinds.SPECIAL_BRICKS_LASER_HORIZONTAL_01 : EObjKinds.SPECIAL_BRICKS_LASER_VERTICAL_01;
                STCellObjInfo cellObjInfo = new STCellObjInfo(null) { ObjKinds = kinds, ATK = 0, HP = 0, SHIELD = 0, ColorID = 0, SizeX = 1, SizeY = 1, SizeZ = 1 };

                Engine.AddCell(targetList[i], kinds, cellObjInfo);
            }
        }

        public void OnClick_Bottom_AddSteelBricks()
        {
            if (Engine.PlayState == NSEngine.CEngine.EPlayState.SHOOT) return;

            int addCount = Mathf.CeilToInt(Engine.viewSize.x / 2f) - 1;

            var targetList = Engine.GetBottomEmptyCells(addCount, Engine.SelBallObj.transform.position.x < 0);
            for(int i=0; i < targetList.Count; i++)
            {
                EObjKinds kinds = EObjKinds.OBSTACLE_BRICKS_FIX_03;
                STCellObjInfo cellObjInfo = new STCellObjInfo(null) { ObjKinds = kinds, ATK = 0, HP = 0, SHIELD = 0, ColorID = 0, SizeX = 1, SizeY = 1, SizeZ = 1 };

                Engine.AddCell(targetList[i], kinds, cellObjInfo);
            }
        }

        public void ToggleAimLayer()
        {
            m_oEngine.ToggleAimLayer();

            goldenAimOn.SetActive(m_oEngine.isGoldAim);
        }

#endregion Buttons
    }
}
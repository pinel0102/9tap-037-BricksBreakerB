using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace GameScene {
    public partial class CSubGameSceneManager : CGameSceneManager
    {
        [Header("★ [Reference] UI Bottom")]
        public GameObject itemLayer;
        public List<Button> bottomItemsButton = new List<Button>();
        public List<GameObject> bottomItemsDisabled = new List<GameObject>();
        public List<GameObject> bottomItemsRuby = new List<GameObject>();
        public List<TMP_Text> bottomItemsText = new List<TMP_Text>();
        public List<TMP_Text> bottomItemsCost = new List<TMP_Text>();
        
        private void SetupBottomButtons()
        {
            bottomItemsButton[0]?.ExAddListener(OnClick_Bottom_Earthquake);
            bottomItemsButton[1]?.ExAddListener(OnClick_Bottom_AddBall);
            bottomItemsButton[2]?.ExAddListener(OnClick_Bottom_BricksDelete);
            bottomItemsButton[3]?.ExAddListener(OnClick_Bottom_AddLaserBricks);
            bottomItemsButton[4]?.ExAddListener(OnClick_Bottom_AddSteelBricks);

            for(int i=0; i < bottomItemsButton.Count; i++)
            {
                bottomItemsButton[i].gameObject.SetActive(Engine.currentLevel >= GlobalDefine.TUTORIAL_LEVEL_BOTTOM_ITEM[i]);
                bottomItemsDisabled[i].SetActive(Engine.currentLevel < GlobalDefine.TUTORIAL_LEVEL_BOTTOM_ITEM[i]);
            }

            RefreshItemCount();
        }

        public void HideShootUIs()
        {
            for(int i = 0; i < m_oShootUIsList.Count; ++i) {
				m_oShootUIsList[i].SetActive(false);
			}
        }

        public void RefreshItemCount()
        {
            for(int index = 0; index < bottomItemsText.Count; index++)
            {
                int count = 0;
                switch(index)
                {
                    case 0: count = GlobalDefine.UserInfo.Item_Earthquake; break;
                    case 1: count = GlobalDefine.UserInfo.Item_AddBall; break;
                    case 2: count = GlobalDefine.UserInfo.Item_BricksDelete; break;
                    case 3: count = GlobalDefine.UserInfo.Item_AddLaserBricks; break;
                    case 4: count = GlobalDefine.UserInfo.Item_AddSteelBricks; break;
                }

                bottomItemsCost[index].text = string.Format(GlobalDefine.FORMAT_INT, GlobalDefine.CostRuby_BottomItem);
                bottomItemsText[index].text = string.Format(GlobalDefine.FORMAT_ITEM_COUNT, count);
                bottomItemsText[index].gameObject.SetActive(count > 0);
                bottomItemsRuby[index].gameObject.SetActive(count <= 0);
            }
        }

#region Buttons

        public void OnClick_Bottom_Earthquake()
        {
            if (!CanUseBottomItem()) return;
            if (!GlobalDefine.isLevelEditor && !Engine.isTutorial)
            {
                if (GlobalDefine.UserInfo.Item_Earthquake < 1)
                {
                    if (GlobalDefine.UserInfo.Ruby < GlobalDefine.CostRuby_BottomItem)
                    {
                        OpenPopup_Store();
                        return;
                    }
                    else
                        PurchaseBottomItem();
                }
                else
                {
                    GlobalDefine.UserInfo.Item_Earthquake = Mathf.Max(0, GlobalDefine.UserInfo.Item_Earthquake - 1);                    
                    GlobalDefine.SaveUserData();
                }
            }
            
            float damageRatio = 0.4f;

            ShakeCamera(() => {

                var targetList = Engine.GetAllCells_EnableHit();
                for(int i=0; i < targetList.Count; i++)
                {
                    var target = targetList[i];
                    int damage = target.Params.m_stObjInfo.m_bIsShieldCell ? Mathf.Max(1, (int)(target.CellObjInfo.SHIELD * damageRatio)) : Mathf.Max(1, (int)(target.CellObjInfo.HP * damageRatio));

                    Engine.CellDamage_EnableHit(target, Engine.BallObjList[0].GetComponent<NSEngine.CEBallObjController>(), damage);
                }

                Engine.RefreshActiveCells();
                Engine.CheckDeadLine();
                Engine.CheckClear(true, true);
            });

            GlobalDefine.PlaySoundFX(ESoundSet.SOUND_ITEM_EARTHQUAKE);

            EndTutorial();
            RefreshItemCount();
        }

        public void OnClick_Bottom_AddBall()
        {
            if (!CanUseBottomItem()) return;
            if (!GlobalDefine.isLevelEditor && !Engine.isTutorial)
            {
                if (GlobalDefine.UserInfo.Item_AddBall < 1) 
                {
                    if (GlobalDefine.UserInfo.Ruby < GlobalDefine.CostRuby_BottomItem)
                    {
                        OpenPopup_Store();
                        return;
                    }
                    else
                        PurchaseBottomItem();
                }
                else
                {
                    GlobalDefine.UserInfo.Item_AddBall = Mathf.Max(0, GlobalDefine.UserInfo.Item_AddBall - 1);
                    GlobalDefine.SaveUserData();
                }
            }

            int addCount = 30;

            Engine.AddNormalBallsOnce(Engine.BallObjList[0].transform.position, addCount, false);

            Engine.BallObjList[0].NumText.text = GlobalDefine.GetBallText(Engine.BallObjList.Count - Engine.DeleteBallList.Count, Engine.DeleteBallList.Count);

            GlobalDefine.PlaySoundFX(ESoundSet.SOUND_ITEM_ADD_BALL);

            EndTutorial();
            RefreshItemCount();
        }

        public void OnClick_Bottom_BricksDelete()
        {
            if (!CanUseBottomItem()) return;
            if (!GlobalDefine.isLevelEditor && !Engine.isTutorial)
            {
                if (GlobalDefine.UserInfo.Item_BricksDelete < 1) 
                {
                    if (GlobalDefine.UserInfo.Ruby < GlobalDefine.CostRuby_BottomItem)
                    {
                        OpenPopup_Store();
                        return;
                    }
                    else
                        PurchaseBottomItem();
                }
                else
                {
                    GlobalDefine.UserInfo.Item_BricksDelete = Mathf.Max(0, GlobalDefine.UserInfo.Item_BricksDelete - 1);
                    GlobalDefine.SaveUserData();
                }
            }

            var lastClearTarget = Engine.GetLastClearTarget();
            if (lastClearTarget != null)
            {
                GlobalDefine.ShowEffect_Laser_Red(EFXSet.FX_LASER_RED, new Vector3(0, lastClearTarget.transform.position.y, lastClearTarget.transform.position.z), GlobalDefine.FXLaser_Rotation_Horizontal, Vector3.one, GlobalDefine.FXLaserRed_Time);

                for(int i = 0; i < Engine.CellObjLists.GetLength(KCDefine.B_VAL_1_INT); ++i) 
                {
                    Engine.CellDestroy_SkillTarget(lastClearTarget.row, i, true);
                }

                GlobalDefine.PlaySoundFX(ESoundSet.SOUND_ITEM_BRICKS_DELETE);
            }

            Engine.RefreshActiveCells();
            Engine.CheckDeadLine();
            Engine.CheckClear(true, true);

            EndTutorial();
            RefreshItemCount();
        }

        public void OnClick_Bottom_AddLaserBricks()
        {
            if (!CanUseBottomItem()) return;
            if (!GlobalDefine.isLevelEditor && !Engine.isTutorial)
            {
                if (GlobalDefine.UserInfo.Item_AddLaserBricks < 1) 
                {
                    if (GlobalDefine.UserInfo.Ruby < GlobalDefine.CostRuby_BottomItem)
                    {
                        OpenPopup_Store();
                        return;
                    }
                    else
                        PurchaseBottomItem();
                }
                else
                {
                    GlobalDefine.UserInfo.Item_AddLaserBricks = Mathf.Max(0, GlobalDefine.UserInfo.Item_AddLaserBricks - 1);
                    GlobalDefine.SaveUserData();
                }
            }

            int addCount = 4;

            var targetList = Engine.GetRandomEmptyCells(Mathf.Max(0, Engine.viewSize.y - Engine.viewSize.x), Engine.viewSize.y, addCount);
            for(int i=0; i < targetList.Count; i++)
            {
                EObjKinds kinds = Random.Range(0, 100) < 50 ? EObjKinds.SPECIAL_BRICKS_LASER_HORIZONTAL_01 : EObjKinds.SPECIAL_BRICKS_LASER_VERTICAL_01;
                STCellObjInfo cellObjInfo = new STCellObjInfo(null) { ObjKinds = kinds, ATK = 0, HP = 0, SHIELD = 0, ColorID = 0, SizeX = 1, SizeY = 1, SizeZ = 1 };

                Engine.AddCell(targetList[i], kinds, cellObjInfo, false);
            }

            GlobalDefine.PlaySoundFX(ESoundSet.SOUND_ITEM_ADD_LASER_BRICKS);

            EndTutorial();
            RefreshItemCount();
        }

        public void OnClick_Bottom_AddSteelBricks()
        {
            if (!CanUseBottomItem() || Engine.isAddSteelBricks) return;
            if (!GlobalDefine.isLevelEditor && !Engine.isTutorial)
            {
                if (GlobalDefine.UserInfo.Item_AddSteelBricks < 1) 
                {
                    if (GlobalDefine.UserInfo.Ruby < GlobalDefine.CostRuby_BottomItem)
                    {
                        OpenPopup_Store();
                        return;
                    }
                    else
                        PurchaseBottomItem();
                }
                else
                {
                    GlobalDefine.UserInfo.Item_AddSteelBricks = Mathf.Max(0, GlobalDefine.UserInfo.Item_AddSteelBricks - 1);
                    GlobalDefine.SaveUserData();
                }
            }

            int addCount = Mathf.CeilToInt(Engine.viewSize.x / 2f) - 1;

            var targetList = Engine.GetBottomEmptyCells(addCount, Engine.SelBallObj.transform.position.x < 0);
            for(int i=0; i < targetList.Count; i++)
            {
                EObjKinds kinds = EObjKinds.OBSTACLE_BRICKS_FIX_03;
                STCellObjInfo cellObjInfo = new STCellObjInfo(null) { ObjKinds = kinds, ATK = 0, HP = 0, SHIELD = 0, ColorID = 0, SizeX = 1, SizeY = 1, SizeZ = 1 };

                Engine.AddCell(targetList[i], kinds, cellObjInfo, true);
            }

            Engine.isAddSteelBricks = true;

            GlobalDefine.PlaySoundFX(ESoundSet.SOUND_ITEM_ADD_STEEL_BRICKS);

            EndTutorial();
            RefreshItemCount();
        }

        private bool CanUseBottomItem()
        {
            return Engine.PlayState != NSEngine.CEngine.EPlayState.SHOOT && !Engine.isGridMoving && !Engine.isExplosionAll && !isShaking;
        }

        private void PurchaseBottomItem()
        {
            GlobalDefine.AddRuby(-GlobalDefine.CostRuby_BottomItem);
        }

        private void OpenPopup_Store()
        {
            GlobalDefine.OpenShop();
        }

#endregion Buttons
    }
}
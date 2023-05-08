using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Timers;

namespace NSEngine {
    public partial class CEngine : CComponent
    {
        [Header("★ [Parameter] Booster")]
        public List<bool> boosterList = new List<bool>();
        public List<bool> boosterItem = new List<bool>();
        public List<bool> boosterRuby = new List<bool>();

        private void InitBooster()
        {
            boosterList = new List<bool>() { false, false, false };
            boosterItem = new List<bool>() { false, false, false };
            boosterRuby = new List<bool>() { false, false, false };
        }

        public void ChangeBooster(int index, bool isOn, bool _useItem, bool _useRuby)
        {
            if (index >= 0 && index < boosterList.Count)
            {    
                boosterList[index] = isOn;
                boosterItem[index] = isOn ? _useItem : false;
                boosterRuby[index] = isOn ? _useRuby : false;
            }
        }

        public void SetBooster()
        {
            for(int i=0; i < boosterList.Count; i++)
            {
                if (boosterList[i] == true)
                {
                    List<CEObj> targetList = GetRandomCells(GetAllCells(EObjType.NORM_BRICKS), 2);
                    if (targetList.Count > 0)
                    {
                        if (boosterItem[i])
                        {
                            switch(i)
                            {
                                case 0: Debug.Log(CodeManager.GetMethodName() + string.Format("[USE BOOSTER] {0} ({1} -> {2})", EItemKinds.BOOSTER_ITEM_01_MISSILE, GlobalDefine.UserInfo.Booster_Missile, GlobalDefine.UserInfo.Booster_Missile - 1));
                                        GlobalDefine.UserInfo.Booster_Missile = Mathf.Max(0, GlobalDefine.UserInfo.Booster_Missile - 1); 
                                        LogFunc.Send_C_Item_Use(currentLevel - 1, global::KDefine.L_SCENE_N_PLAY, LogFunc.MakeLogItemInfo(EItemKinds.BOOSTER_ITEM_01_MISSILE, 1));
                                        break;
                                case 1: Debug.Log(CodeManager.GetMethodName() + string.Format("[USE BOOSTER] {0} ({1} -> {2})", EItemKinds.BOOSTER_ITEM_02_LIGHTNING, GlobalDefine.UserInfo.Booster_Lightning, GlobalDefine.UserInfo.Booster_Lightning - 1)); 
                                        GlobalDefine.UserInfo.Booster_Lightning = Mathf.Max(0, GlobalDefine.UserInfo.Booster_Lightning - 1); 
                                        LogFunc.Send_C_Item_Use(currentLevel - 1, global::KDefine.L_SCENE_N_PLAY, LogFunc.MakeLogItemInfo(EItemKinds.BOOSTER_ITEM_02_LIGHTNING, 1));
                                        break;
                                case 2: Debug.Log(CodeManager.GetMethodName() + string.Format("[USE BOOSTER] {0} ({1} -> {2})", EItemKinds.BOOSTER_ITEM_03_BOMB, GlobalDefine.UserInfo.Booster_Bomb, GlobalDefine.UserInfo.Booster_Bomb - 1));
                                        GlobalDefine.UserInfo.Booster_Bomb = Mathf.Max(0, GlobalDefine.UserInfo.Booster_Bomb - 1); 
                                        LogFunc.Send_C_Item_Use(currentLevel - 1, global::KDefine.L_SCENE_N_PLAY, LogFunc.MakeLogItemInfo(EItemKinds.BOOSTER_ITEM_03_BOMB, 1));
                                        break;
                            }
                        }
                        else if (boosterRuby[i])
                        {
                            GlobalDefine.AddRuby(-GlobalDefine.CostRuby_Booster);
                            LogFunc.Send_C_Item_Use(currentLevel - 1, global::KDefine.L_SCENE_N_PLAY, LogFunc.MakeLogItemInfo(EItemKinds.GOODS_RUBY, GlobalDefine.CostRuby_Booster));

                            switch(i)
                            {
                                case 0: Debug.Log(CodeManager.GetMethodName() + string.Format("[BUY BOOSTER] {0} ({1} -> {2})", EItemKinds.BOOSTER_ITEM_01_MISSILE, GlobalDefine.UserInfo.Ruby, GlobalDefine.UserInfo.Ruby - GlobalDefine.CostRuby_Booster)); 
                                        LogFunc.Send_C_Item_Use(currentLevel - 1, global::KDefine.L_SCENE_N_PLAY, LogFunc.MakeLogItemInfo(EItemKinds.BOOSTER_ITEM_01_MISSILE, 1));
                                        break;
                                case 1: Debug.Log(CodeManager.GetMethodName() + string.Format("[BUY BOOSTER] {0} ({1} -> {2})", EItemKinds.BOOSTER_ITEM_02_LIGHTNING, GlobalDefine.UserInfo.Ruby, GlobalDefine.UserInfo.Ruby - GlobalDefine.CostRuby_Booster)); 
                                        LogFunc.Send_C_Item_Use(currentLevel - 1, global::KDefine.L_SCENE_N_PLAY, LogFunc.MakeLogItemInfo(EItemKinds.BOOSTER_ITEM_02_LIGHTNING, 1));
                                        break;
                                case 2: Debug.Log(CodeManager.GetMethodName() + string.Format("[BUY BOOSTER] {0} ({1} -> {2})", EItemKinds.BOOSTER_ITEM_03_BOMB, GlobalDefine.UserInfo.Ruby, GlobalDefine.UserInfo.Ruby - GlobalDefine.CostRuby_Booster)); 
                                        LogFunc.Send_C_Item_Use(currentLevel - 1, global::KDefine.L_SCENE_N_PLAY, LogFunc.MakeLogItemInfo(EItemKinds.BOOSTER_ITEM_03_BOMB, 1));
                                        break;
                            }
                        }

                        for (int j=0; j < targetList.Count; j++)
                        {
                            ChangeCell(targetList[j].GetComponent<CECellObjController>(), GetBoosterBrick(i));
                            GlobalDefine.ShowEffect(EFXSet.FX_BOOSTER, targetList[j].centerPosition);
                            GlobalDefine.PlaySoundFX(ESoundSet.SOUND_GET_ITEM);
                        }
                    }
                }
            }

            GlobalDefine.SaveUserData();
        }

        private EObjKinds GetBoosterBrick(int index)
        {
            switch(index)
            {
                case 0: return EObjKinds.SPECIAL_BRICKS_MISSILE_02;
                case 1: return EObjKinds.SPECIAL_BRICKS_LIGHTNING_01;
                case 2: return EObjKinds.SPECIAL_BRICKS_EXPLOSION_CROSS_01;
                default: return EObjKinds.NONE;
            }
        }
    }
}
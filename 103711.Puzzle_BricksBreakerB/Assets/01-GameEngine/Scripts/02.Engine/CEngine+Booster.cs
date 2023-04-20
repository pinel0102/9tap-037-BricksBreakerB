using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Timers;

namespace NSEngine {
    public partial class CEngine : CComponent
    {
        [Header("★ [Parameter] Booster")]
        public bool[] boosterList = new bool[3];

        private void InitBooster()
        {
            for(int i=0; i < boosterList.Length; i++)
            {
                boosterList[i] = false;
            }
        }

        public void ChangeBooster(int index, bool isOn)
        {
            if (index >= 0 && index < boosterList.Length)
                boosterList[index] = isOn;
        }

        public void SetBooster()
        {
            for(int i=0; i < boosterList.Length; i++)
            {
                if (boosterList[i] == true)
                {
                    Debug.Log(CodeManager.GetMethodName() + i);

                    List<CEObj> targetList = GetRandomCells(GetAllCells(EObjType.NORM_BRICKS), 2);

                    for (int j=0; j < targetList.Count; j++)
                    {
                        ChangeCell(targetList[j].GetComponent<CECellObjController>(), GetBoosterBrick(i));
                        GlobalDefine.ShowEffect(EFXSet.FX_BOOSTER, targetList[j].centerPosition);
                        GlobalDefine.PlaySoundFX(ESoundSet.SOUND_GET_ITEM);
                    }
                }
            }
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
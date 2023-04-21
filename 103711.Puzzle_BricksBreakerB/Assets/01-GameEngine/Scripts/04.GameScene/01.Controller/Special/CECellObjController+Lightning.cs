using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
	/** 서브 셀 객체 제어자 */
	public partial class CECellObjController : CEObjController {

        private void GetSpecial_Lightning(CEBallObjController ballController, EObjKinds kindsType, EObjKinds kinds, int _ATK = KCDefine.B_VAL_1_INT)
        {
            //Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>{0}</color>", kinds));

            List<CEObj> excludeList = new List<CEObj>();
            excludeList.Add(this.GetOwner<CEObj>());
            
            switch(kinds)
            {
                case EObjKinds.SPECIAL_BRICKS_LIGHTNING_01: Lightning(ballController, _ATK, KCDefine.B_VAL_4_INT, excludeList); break;
                default: break;
            }


            this.SetHideReserved();
        }

        ///<Summary>번개.</Summary>
        private void Lightning(CEBallObjController ballController, int _ATK, int targetCount, List<CEObj> excludeList)
        {
            List<CEObj> targetList = Engine.GetRandomCells_EnableHit(targetCount, excludeList);
            CEObj myCell = this.GetOwner<CEObj>();

            for(int i=0; i < targetList.Count; i++)
            {
                Engine.CellDamage_EnableHit(targetList[i], ballController, _ATK);
                ShowEffect_Lightning(myCell.centerPosition, targetList[i]);
            }
        }

        private void ShowEffect_Lightning(Vector3 centerPosition, CEObj target)
        {
            Vector3 fromPosition = centerPosition;
            Vector3 toPosition = target.centerPosition;
            float distance = Vector2.Distance(fromPosition, toPosition);

            Vector3 fxPosition = (fromPosition + toPosition) * 0.5f;
            Vector3 fxScale = new Vector3(distance, distance, 1);
            float fxAngle = GlobalDefine.GetAngle(fromPosition, toPosition);

            float fxHitScale = Mathf.Min(target.Params.m_stObjInfo.m_stSize.x, target.Params.m_stObjInfo.m_stSize.y);
            Vector3 fxHitScaleVector = new Vector3(fxHitScale, fxHitScale, 1);

            float fxStartSizeY_Min = GlobalDefine.FXLightning_StartSizeY_Min * (GlobalDefine.SCREEN_WIDTH / distance);
            float fxStartSizeY_Max = fxStartSizeY_Min * GlobalDefine.FXLightning_StartSizeY_Max_Multiplier;

            //Debug.Log(CodeManager.GetMethodName() + string.Format("distance : {0} / min : {1} / max : {2}", distance, fxStartSizeY_Min, fxStartSizeY_Max));

            GlobalDefine.ShowEffect_Lightning(EFXSet.FX_LIGHTNING, fxPosition, fxAngle, fxScale, fxStartSizeY_Min, fxStartSizeY_Max);
            GlobalDefine.ShowEffect(EFXSet.FX_LIGHTNING_HIT, toPosition, Vector3.zero, fxHitScaleVector);
            GlobalDefine.PlaySoundFX(ESoundSet.SOUND_SPECIAL_LIGHTNING);
        }
    }
}
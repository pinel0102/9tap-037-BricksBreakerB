using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
	/** 서브 셀 객체 제어자 */
	public partial class CECellObjController : CEObjController {

        private void GetSpecial_Lightning(CEBallObjController ballController, EObjKinds kindsType, EObjKinds kinds, int _ATK = KCDefine.B_VAL_1_INT)
        {
            Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>{0}</color>", kinds));

            List<CEObj> excludeList = new List<CEObj>();
            excludeList.Add(this.GetOwner<CEObj>());
            
            switch(kinds)
            {
                case EObjKinds.SPECIAL_BRICKS_LIGHTNING_01: Lightning(ballController, _ATK, KCDefine.B_VAL_4_INT, excludeList); break;
                default: break;
            }


            //this.SetHideReserved();
        }

        ///<Summary>번개.</Summary>
        private void Lightning(CEBallObjController ballController, int _ATK, int targetCount, List<CEObj> excludeList)
        {
            List<CEObj> targetList = this.Engine.GetRandomCells_SkillTarget(targetCount, excludeList);

            for(int i=0; i < targetList.Count; i++)
            {
                CellDamage_SkillTarget(targetList[i], ballController, _ATK);
                ShowEffect_Lightning(targetList[i]);
            }
        }

        private void ShowEffect_Lightning(CEObj target)
        {
            Vector3 fxPosition = (this.transform.position + target.transform.position) * 0.5f;
            float fxAngle = GetAngle(this.transform.position, target.transform.position);
            float distance = Vector2.Distance(this.transform.position, target.transform.position);
            Vector3 fxScale = new Vector3(distance, distance, 1);

            float fxStartSizeY_Min = GlobalDefine.FXLightning_StartSizeY_Min * (GlobalDefine.SCREEN_WIDTH / distance);
            float fxStartSizeY_Max = fxStartSizeY_Min * GlobalDefine.FXLightning_StartSizeY_Max_Multiplier;

            Debug.Log(CodeManager.GetMethodName() + string.Format("distance : {0} / min : {1} / max : {2}", distance, fxStartSizeY_Min, fxStartSizeY_Max));

            GlobalDefine.ShowEffect(EFXSet.FX_LIGHTNING, fxPosition, fxAngle, fxScale,fxStartSizeY_Min, fxStartSizeY_Max);
            GlobalDefine.ShowEffect(EFXSet.FX_LIGHTNING_HIT, target.transform.position);
        }

        public float GetAngle(Vector2 from, Vector2 to)
        {
            Vector2 offset = to - from;
            return Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        }
    }
}
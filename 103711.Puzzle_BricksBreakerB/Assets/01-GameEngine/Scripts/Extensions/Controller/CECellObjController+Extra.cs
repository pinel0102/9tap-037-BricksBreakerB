using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
    public partial class CECellObjController : CEObjController
    {
        public bool hideReserved;

        private Coroutine hitCoroutine;

        public void GetDamage(int _ATK)
        {
            var oCellObj = this.GetOwner<CEObj>();
			var stCellObjInfo = oCellObj.CellObjInfo;

            stCellObjInfo.HP = Mathf.Max(KCDefine.B_VAL_0_INT, stCellObjInfo.HP - _ATK);

			this.GetOwner<CEObj>().HPText.text = $"{stCellObjInfo.HP}";
			this.GetOwner<CEObj>().SetCellObjInfo(stCellObjInfo);

			// 체력이 없을 경우
			if(stCellObjInfo.HP <= KCDefine.B_VAL_0_INT) {
				CellDestroy();
			}
            else
            {
                HitEffect();
            }
        }

        private void GetObstacle(EObjKinds kinds)
        {
            //
        }

        private void GetItem(EObjKinds kinds)
        {
            EObjKinds kindsType = (EObjKinds)((int)kinds).ExKindsToCorrectKinds(EKindsGroupType.SUB_KINDS_TYPE);

            switch(kindsType)
            {
                case EObjKinds.ITEM_BRICKS_BALL_01:
                    GetItem_BallPlus(kindsType, kinds);
                    break;
                default:
                    Debug.Log(CodeManager.GetMethodName() + string.Format("<color=red>{0}</color>", kindsType));
                    break;
            }

            CellDestroy();
        }

        private void GetSpecial(EObjKinds kinds)
        {
            EObjKinds kindsType = (EObjKinds)((int)kinds).ExKindsToCorrectKinds(EKindsGroupType.SUB_KINDS_TYPE);

            switch(kindsType)
            {
                case EObjKinds.SPECIAL_BRICKS_LASER_HORIZONTAL_01:
                case EObjKinds.SPECIAL_BRICKS_LASER_VERTICAL_01:
                case EObjKinds.SPECIAL_BRICKS_LASER_CROSS_01:
                    GetSpecial_Laser(kindsType, kinds);
                    break;
                default:
                    Debug.Log(CodeManager.GetMethodName() + string.Format("<color=red>{0}</color>", kindsType));
                    break;
            }
        }

        ///<Summary>셀 숨기기가 예약되었으면 셀을 숨긴다.</Summary>
        public void HideReservedCell()
        {
            if (hideReserved)
                CellDestroy();
        }

        ///<Summary>턴 종료시 셀 숨기기를 예약한다.</Summary>
        private void SetHideReserved(bool _hideAfterTurnEnd = true)
        {
            if (hideReserved != _hideAfterTurnEnd)
                hideReserved = _hideAfterTurnEnd;
        }
        
        private void HitEffect()
        {
            if (hitCoroutine != null)
                StopCoroutine(hitCoroutine);
            
            hitCoroutine = StartCoroutine(CO_HitEffect());
        }

        private IEnumerator CO_HitEffect()
        {
            this.GetOwner<CEObj>().ToggleHitSprite(true);
            
            yield return Engine.hitEffectDelay;
            
            this.GetOwner<CEObj>().ToggleHitSprite(false);
        }

        private void CellDestroy()
        {
            if (hitCoroutine != null)
                StopCoroutine(hitCoroutine);
            
            this.GetOwner<CEObj>().Params.m_stBaseParams.m_oCallbackDict.GetValueOrDefault(CEObjComponent.ECallback.ENGINE_OBJ_EVENT)?.Invoke(this.GetOwner<CEObj>(), EEngineObjEvent.DESTROY, string.Empty);
        }
    }
}

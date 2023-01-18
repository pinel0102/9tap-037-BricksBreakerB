using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 서브 셀 객체 제어자 */
	public partial class CECellObjController : CEObjController {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			[HideInInspector] MAX_VAL
		}

		#region 변수

		#endregion // 변수

		#region 프로퍼티

		#endregion // 프로퍼티

		#region 함수
		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(CSceneManager.IsAppRunning) {
				// Do Something
			}
		}

		/** 히트 되었을 경우 */
		public void OnHit(CEObj a_oTarget) {
            var oCellObj = this.GetOwner<CEObj>();
			var stCellObjInfo = oCellObj.CellObjInfo;

            EObjKinds kinds = oCellObj.Params.m_stObjInfo.m_eObjKinds;
            EObjType cellType = (EObjType)((int)kinds).ExKindsToType();

            switch(cellType) 
            {
                case EObjType.NORM_BRICKS: this.GetDamage(stCellObjInfo, kinds, KCDefine.B_VAL_1_INT); break;
                case EObjType.OBSTACLE_BRICKS: this.GetObstacle(stCellObjInfo, kinds); break;
                case EObjType.ITEM_BRICKS: this.GetItem(stCellObjInfo, kinds); break;
                case EObjType.SPECIAL_BRICKS: this.GetSpecial(stCellObjInfo, kinds); break;
                default : 
                    Debug.Log(CodeManager.GetMethodName() + string.Format("{0} / {1}", cellType, kinds));
                    break;
            }
		}

		/** 셀 객체를 설정한다 */
		private void SubAwake() {
			// Do Something
		}

		/** 초기화한다 */
		private void SubInit() {
			// Do Something
		}
		#endregion // 함수

        private void GetDamage(STCellObjInfo stCellObjInfo, EObjKinds kinds, int _ATK)
        {
            stCellObjInfo.HP = Mathf.Max(KCDefine.B_VAL_0_INT, stCellObjInfo.HP - _ATK);

			this.GetOwner<CEObj>().HPText.text = $"{stCellObjInfo.HP}";
			this.GetOwner<CEObj>().SetCellObjInfo(stCellObjInfo);

			// 체력이 없을 경우
			if(stCellObjInfo.HP <= KCDefine.B_VAL_0_INT) {
				CellDestroy();
			}
        }

        private void GetObstacle(STCellObjInfo stCellObjInfo, EObjKinds kinds)
        {
            //
        }

        private void GetItem(STCellObjInfo stCellObjInfo, EObjKinds kinds)
        {
            EObjKinds kindsType = (EObjKinds)((int)kinds).ExKindsToCorrectKinds(EKindsGroupType.SUB_KINDS_TYPE);

            switch(kindsType)
            {
                case EObjKinds.ITEM_BRICKS_BALL_01:
                    GetItem_BallPlus(kinds);
                    break;
                default:
                    break;
            }

            CellDestroy();
        }

        private void GetSpecial(STCellObjInfo stCellObjInfo, EObjKinds kinds)
        {
            switch(kinds)
            {
                case EObjKinds.SPECIAL_BRICKS_LAZER_HORIZONTAL_01:
                    break;
                default:
                    break;
            }
        }

        private void CellDestroy()
        {
            this.GetOwner<CEObj>().Params.m_stBaseParams.m_oCallbackDict.GetValueOrDefault(CEObjComponent.ECallback.ENGINE_OBJ_EVENT)?.Invoke(this.GetOwner<CEObj>(), EEngineObjEvent.DESTROY, string.Empty);
        }
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

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
                case EObjType.NORM_BRICKS: this.GetDamage(KCDefine.B_VAL_1_INT); oCellObj.SetSpriteColor(oCellObj.CellObjInfo.ObjKinds); break;
                case EObjType.OBSTACLE_BRICKS: this.GetObstacle(kinds); break;
                case EObjType.ITEM_BRICKS: this.GetItem(kinds); break;
                case EObjType.SPECIAL_BRICKS: this.GetSpecial(kinds); break;
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
			this.SetHideReserved(false);
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

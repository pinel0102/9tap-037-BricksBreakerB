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
            IS_HIT,
            EXTRA_OBJ_KINDS_IDX,
			[HideInInspector] MAX_VAL
		}

		#region 변수
        private Dictionary<ESubKey, bool> m_oSubBoolDict = new Dictionary<ESubKey, bool>() {
			[ESubKey.IS_HIT] = false
		};

		private Dictionary<ESubKey, int> m_oSubIntDict = new Dictionary<ESubKey, int>() {
			[ESubKey.EXTRA_OBJ_KINDS_IDX] = KCDefine.B_VAL_0_INT
		};
		#endregion // 변수

		#region 프로퍼티
        public List<EObjKinds> ExtraObjKindsList { get; } = new List<EObjKinds>();
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
		public void OnHit(CEObj a_oTarget, CEBallObjController ballController) {
            var oCellObj = this.GetOwner<CEObj>();
			var stCellObjInfo = oCellObj.CellObjInfo;

            EObjKinds kinds = oCellObj.Params.m_stObjInfo.m_eObjKinds;
            EObjKinds kindsType = (EObjKinds)((int)kinds).ExKindsToCorrectKinds(EKindsGroupType.SUB_KINDS_TYPE);
            EObjType cellType = (EObjType)((int)kinds).ExKindsToType();

            int ATK = KCDefine.B_VAL_1_INT + ballController.extraATK;
            
            switch(cellType) 
            {
                case EObjType.NORM_BRICKS: this.GetDamage(ballController, kindsType, kinds, ATK); break;
                case EObjType.OBSTACLE_BRICKS: this.GetObstacle(ballController, kindsType, kinds, ATK); break;
                case EObjType.ITEM_BRICKS: this.GetItem(ballController, kindsType, kinds, ATK); break;
                case EObjType.SPECIAL_BRICKS: this.GetSpecial(ballController, kindsType, kinds, ATK); break;
                default : 
                    Debug.Log(CodeManager.GetMethodName() + string.Format("{0} / {1} / {2}", cellType, kindsType, kinds));
                    break;
            }
		}

		/** 셀 객체를 설정한다 */
		private void SubAwake() {
			// Do Something
		}

		/** 초기화한다 */
		private void SubInit(EObjKinds kinds) {
			this.hideReserved = false;
            this.missileReserved = false;
            this.isShieldCell = GlobalDefine.IsShieldCell(kinds);
		}

        /** 객체 정보를 리셋한다 */
		private void SubResetObjInfo(STObjInfo a_stObjInfo, STCellObjInfo a_stCellObjInfo) {
			// Do Something
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

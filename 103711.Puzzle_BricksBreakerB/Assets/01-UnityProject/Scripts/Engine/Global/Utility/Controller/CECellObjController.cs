using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 셀 객체 제어자 */
	public partial class CECellObjController : CEObjController {
        /** 식별자 */
		private enum EKey {
			NONE = -1,
			IDX,
			CELL_OBJ_INFO,
			[HideInInspector] MAX_VAL
		}

		/** 매개 변수 */
		public new struct STParams {
			public CEObjController.STParams m_stBaseParams;
		}

		#region 변수
        private Dictionary<EKey, Vector3Int> m_oVec3IntDict = new Dictionary<EKey, Vector3Int>() {
			[EKey.IDX] = new Vector3Int(KCDefine.B_IDX_INVALID, KCDefine.B_IDX_INVALID, KCDefine.B_IDX_INVALID)
		};

		private Dictionary<EKey, STCellObjInfo> m_oCellObjInfoDict = new Dictionary<EKey, STCellObjInfo>() {
			[EKey.CELL_OBJ_INFO] = STCellObjInfo.INVALID
		};
		#endregion // 변수

		#region 프로퍼티
		public new STParams Params { get; private set; }

        public Vector3Int Idx => m_oVec3IntDict[EKey.IDX];
		public STCellObjInfo CellObjInfo => m_oCellObjInfoDict[EKey.CELL_OBJ_INFO];
		#endregion // 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();
			this.SubAwake();
		}

		/** 초기화 */
		public virtual void Init(STParams a_stParams, EObjKinds kinds) {
			base.Init(a_stParams.m_stBaseParams);
			this.Params = a_stParams;

            this.SubInit(kinds);
		}

        /** 객체 정보를 리셋한다 */
		public virtual void ResetObjInfo(STObjInfo a_stObjInfo, STCellObjInfo a_stCellObjInfo) {
			base.ResetObjInfo(a_stObjInfo);

            this.SetCellObjInfo(a_stObjInfo.m_eObjKinds, a_stCellObjInfo);
			this.SubResetObjInfo(a_stObjInfo, a_stCellObjInfo);
		}
		#endregion // 함수

		#region 클래스 함수
		/** 매개 변수를 생성한다 */
		public new static STParams MakeParams(CEngine a_oEngine) {
			return new STParams() {
				m_stBaseParams = CEObjController.MakeParams(a_oEngine)
			};
		}
		#endregion // 클래스 함수
	}

    /** 셀 객체 제어자 - 접근 */
	public partial class CECellObjController : CEObjController {
		#region 함수
		/** 인덱스를 변경한다 */
		public void SetIdx(Vector3Int a_stIdx) {
			m_oVec3IntDict[EKey.IDX] = a_stIdx;
		}

		/** 셀 객체 정보를 변경한다 */
		public void SetCellObjInfo(EObjKinds kinds, STCellObjInfo a_stCellObjInfo) {
			a_stCellObjInfo.ObjKinds = kinds;
			m_oCellObjInfoDict[EKey.CELL_OBJ_INFO] = a_stCellObjInfo;
		}
		#endregion // 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

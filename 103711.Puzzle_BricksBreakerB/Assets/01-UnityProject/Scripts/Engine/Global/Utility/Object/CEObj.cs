using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 객체 */
	public partial class CEObj : CEObjComponent {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			CELL_IDX,
			[HideInInspector] MAX_VAL
		}

		/** 매개 변수 */
		public new struct STParams {
			public CEObjComponent.STParams m_stBaseParams;
			public STObjInfo m_stObjInfo;
			public CObjTargetInfo m_oObjTargetInfo;
		}

		#region 변수
		private Dictionary<EKey, Vector3Int> m_oVec3IntDict = new Dictionary<EKey, Vector3Int>() {
			[EKey.CELL_IDX] = new Vector3Int(KCDefine.B_IDX_INVALID, KCDefine.B_IDX_INVALID, KCDefine.B_IDX_INVALID)
		};

        public int row;
        public int column;
        public int layer;

		#endregion // 변수

		#region 프로퍼티
		public new STParams Params { get; private set; }
		public Vector3Int CellIdx => m_oVec3IntDict.GetValueOrDefault(EKey.CELL_IDX);
		#endregion // 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();
			this.SubAwake();
		}

		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			base.Init(a_stParams.m_stBaseParams);
			this.Params = a_stParams;

			// 스프라이트를 설정한다
			this.TargetSprite?.ExSetSprite<SpriteRenderer>(Access.GetSprite(a_stParams.m_stObjInfo.m_eObjKinds));
			this.TargetSprite?.ExSetSortingOrder(Access.GetSortingOrderInfo(a_stParams.m_stObjInfo.m_eObjKinds));
            
            SetSpriteColor();

			this.SubInit();
		}

		/** 어빌리티 값을 설정한다 */
		protected override void DoSetupAbilityVals(bool a_bIsReset = true) {
			base.DoSetupAbilityVals(a_bIsReset);

			// 객체 정보가 존재 할 경우
			if(this.Params.m_stObjInfo.m_eObjKinds.ExIsValid()) {
				global::Func.SetupAbilityVals(this.Params.m_stObjInfo, this.Params.m_oObjTargetInfo, this.AbilityValDictWrapper.m_oDict02);
			}
		}

		/** 셀 인덱스를 변경한다 */
		public void SetCellIdx(Vector3Int a_stCellIdx) {
			m_oVec3IntDict.ExReplaceVal(EKey.CELL_IDX, a_stCellIdx);

            column = a_stCellIdx.x;
            row = a_stCellIdx.y;
            layer = a_stCellIdx.z;
		}

        public void SetSpriteColor()
        {
            EObjKinds kinds = Params.m_stObjInfo.m_eObjKinds;
            EObjType cellType = (EObjType)((int)kinds).ExKindsToType();

            Color _color = new Color();

            switch(cellType)
            {
                case EObjType.BALL:
                    _color = GlobalDefine.BricksColor[2];
                    break;
                case EObjType.NORM_BRICKS:
                    _color = GlobalDefine.BricksColor[1];
                    break;
                case EObjType.OBSTACLE_BRICKS:
                case EObjType.ITEM_BRICKS:
                case EObjType.SPECIAL_BRICKS:
                    _color = GlobalDefine.BricksColor[0];
                    break;
                default:
                    _color = GlobalDefine.BricksColor[0];
                    break;
            }

            this.TargetSprite.color = _color;
        }
		#endregion // 함수

		#region 클래스 함수
		/** 매개 변수를 생성한다 */
		public static STParams MakeParams(CEngine a_oEngine, STObjInfo a_stObjInfo, CObjTargetInfo a_oObjTargetInfo, CEController a_oController = null, string a_oObjsPoolKey = KCDefine.B_TEXT_EMPTY) {
			return new STParams() {
				m_stBaseParams = CEObjComponent.MakeParams(a_oEngine, a_oController, a_oObjsPoolKey), m_stObjInfo = a_stObjInfo, m_oObjTargetInfo = a_oObjTargetInfo
			};
		}
		#endregion // 클래스 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

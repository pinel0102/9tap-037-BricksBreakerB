#if SCRIPT_TEMPLATE_ONLY
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
			OBJ_SPRITE,
			[HideInInspector] MAX_VAL
		}

		/** 매개 변수 */
		public new struct STParams {
			public CEObjComponent.STParams m_stBaseParams;
			public STObjInfo m_stObjInfo;
			public CObjTargetInfo m_oObjTargetInfo;
		}

		#region 변수
		private Dictionary<EKey, Vector3Int> m_oVec3IntDict = new Dictionary<EKey, Vector3Int>();
		private Dictionary<EKey, SpriteRenderer> m_oSpriteDict = new Dictionary<EKey, SpriteRenderer>();
		#endregion // 변수

		#region 프로퍼티
		public new STParams Params { get; private set; }
		public Vector3Int CellIdx => m_oVec3IntDict.GetValueOrDefault(EKey.CELL_IDX);
		#endregion // 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();
			m_oVec3IntDict.ExReplaceVal(EKey.CELL_IDX, KCDefine.B_IDX_INVALID_3D);

			// 스프라이트를 설정한다
			CFunc.SetupSprites(new List<(EKey, string, GameObject)>() {
				(EKey.OBJ_SPRITE, $"{EKey.OBJ_SPRITE}", this.gameObject)
			}, m_oSpriteDict);

			this.SubSetupAwake();
		}

		/** 초기화 */
		public virtual void Init(STParams a_stParams) {
			base.Init(a_stParams.m_stBaseParams);
			this.Params = a_stParams;

			// 객체 스프라이트가 존재 할 경우
			if(m_oSpriteDict.GetValueOrDefault(EKey.OBJ_SPRITE) != null) {
				m_oSpriteDict.GetValueOrDefault(EKey.OBJ_SPRITE).sprite = Access.GetObjSprite(a_stParams.m_stObjInfo.m_eObjKinds);
				m_oSpriteDict.GetValueOrDefault(EKey.OBJ_SPRITE).ExSetSortingOrder(Access.GetSortingOrderInfo(a_stParams.m_stObjInfo.m_eObjKinds));
			}

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
#endif // #if SCRIPT_TEMPLATE_ONLY

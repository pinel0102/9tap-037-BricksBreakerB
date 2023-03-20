using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
using TMPro;

namespace NSEngine {
	/** 서브 객체 */
	public partial class CEObj : CEObjComponent {
		/** 서브 식별자 */
		private enum ESubKey {
			NONE = -1,
			HP_TEXT,
			NUM_TEXT,
			[HideInInspector] MAX_VAL
		}

		#region 변수
		/** =====> UI <===== */
		private Dictionary<ESubKey, TMP_Text> m_oSubTextDict = new Dictionary<ESubKey, TMP_Text>();
		#endregion // 변수

		#region 프로퍼티
		public STCellObjInfo CellObjInfo { get; private set; }

		/** =====> UI <===== */
		public TMP_Text HPText => m_oSubTextDict[ESubKey.HP_TEXT];
		public TMP_Text NumText => m_oSubTextDict[ESubKey.NUM_TEXT];
		#endregion // 프로퍼티

		#region 함수
		/** 컴포넌트를 설정한다 */
		private void SubAwake() {
			this.SetCellObjInfo(STCellObjInfo.INVALID);

			// 텍스트를 설정한다
			CFunc.SetupComponents(new List<(ESubKey, string, GameObject)>() {
				(ESubKey.HP_TEXT, $"{ESubKey.HP_TEXT}", this.gameObject),
				(ESubKey.NUM_TEXT, $"{ESubKey.NUM_TEXT}", this.gameObject)
			}, m_oSubTextDict);
		}

		/** 초기화한다 */
		private void SubInit() {
			this.SetupAbilityVals();

			// 텍스트를 갱신한다 {
			(m_oSubTextDict.GetValueOrDefault(ESubKey.HP_TEXT) as TextMeshPro)?.ExSetSortingOrder(new STSortingOrderInfo() {
				m_nOrder = this.TargetSprite.sortingOrder + GlobalDefine.HPText_Order, m_oLayer = KCDefine.U_SORTING_L_CELL
			});

			(m_oSubTextDict.GetValueOrDefault(ESubKey.NUM_TEXT) as TextMeshPro)?.ExSetSortingOrder(new STSortingOrderInfo() {
				m_nOrder = this.TargetSprite.sortingOrder + GlobalDefine.NumText_Order, m_oLayer = KCDefine.U_SORTING_L_DEF
			});
			// 텍스트를 갱신한다 }

			// 충돌체가 존재 할 경우
			if(this.TargetSprite != null && this.TargetSprite.TryGetComponent<PolygonCollider2D>(out PolygonCollider2D oCollider)) {
				var oPosList = CCollectionManager.Inst.SpawnList<Vector2>();
				oCollider.pathCount = KCDefine.B_VAL_1_INT;

				try {
					switch((EObjKinds)((int)this.Params.m_stObjInfo.m_eObjKinds).ExKindsToCorrectKinds(EKindsGroupType.SUB_KINDS_TYPE)) {
						case EObjKinds.NORM_BRICKS_TRIANGLE_01: this.SetupTriangleCollider(oPosList); break;
						case EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_01: this.SetupRightTriangleCollider(oPosList); break;
                        case EObjKinds.NORM_BRICKS_DIAMOND_01: this.SetupDiamondCollider(oPosList); break;
                        case EObjKinds.SPECIAL_BRICKS_BALL_DIFFUSION_01: this.SetupDiamondCollider(oPosList, GlobalDefine.CustomSize_Diffusion); break;
                        case EObjKinds.SPECIAL_BRICKS_BALL_AMPLIFICATION_01: this.SetupTriangleCollider(oPosList); break;
                        default: this.SetupSquareCollider(oPosList); break;
					}

					oCollider.SetPath(KCDefine.B_VAL_0_INT, oPosList);
				} finally {
					CCollectionManager.Inst.DespawnList(oPosList);
				}
			}
		}

		/** 사각형 충돌체를 설정한다 */
		private void SetupSquareCollider(List<Vector2> a_oOutPosList) {
			a_oOutPosList.ExAddVal(new Vector2(Access.CellSize.x / -KCDefine.B_VAL_2_REAL, Access.CellSize.y / -KCDefine.B_VAL_2_REAL));
			a_oOutPosList.ExAddVal(new Vector2(Access.CellSize.x / -KCDefine.B_VAL_2_REAL, Access.CellSize.y / KCDefine.B_VAL_2_REAL));
			a_oOutPosList.ExAddVal(new Vector2(Access.CellSize.x / KCDefine.B_VAL_2_REAL, Access.CellSize.y / KCDefine.B_VAL_2_REAL));
			a_oOutPosList.ExAddVal(new Vector2(Access.CellSize.x / KCDefine.B_VAL_2_REAL, Access.CellSize.y / -KCDefine.B_VAL_2_REAL));
		}

        /** 마름모 충돌체를 설정한다 */
		private void SetupDiamondCollider(List<Vector2> a_oOutPosList) {
            a_oOutPosList.ExAddVal(new Vector2(0, Access.CellSize.y / -KCDefine.B_VAL_2_REAL));
            a_oOutPosList.ExAddVal(new Vector2(Access.CellSize.x / KCDefine.B_VAL_2_REAL, 0));
			a_oOutPosList.ExAddVal(new Vector2(0, Access.CellSize.y / KCDefine.B_VAL_2_REAL));
			a_oOutPosList.ExAddVal(new Vector2(-Access.CellSize.x / KCDefine.B_VAL_2_REAL, 0));
		}

        /** 마름모 충돌체를 설정한다 */
		private void SetupDiamondCollider(List<Vector2> a_oOutPosList, Vector2 customSize) {
            a_oOutPosList.ExAddVal(new Vector2(0, customSize.y / -KCDefine.B_VAL_2_REAL));
            a_oOutPosList.ExAddVal(new Vector2(customSize.x / KCDefine.B_VAL_2_REAL, 0));
			a_oOutPosList.ExAddVal(new Vector2(0, customSize.y / KCDefine.B_VAL_2_REAL));
			a_oOutPosList.ExAddVal(new Vector2(-customSize.x / KCDefine.B_VAL_2_REAL, 0));
		}

		/** 삼각형 충돌체를 설정한다 */
		private void SetupTriangleCollider(List<Vector2> a_oOutPosList) {
			switch(this.Params.m_stObjInfo.m_eObjKinds) {
				case EObjKinds.NORM_BRICKS_TRIANGLE_01:
                case EObjKinds.SPECIAL_BRICKS_BALL_AMPLIFICATION_01: {
					a_oOutPosList.ExAddVal(new Vector2(Access.CellSize.x / -KCDefine.B_VAL_2_REAL, Access.CellSize.y / -KCDefine.B_VAL_2_REAL));
					a_oOutPosList.ExAddVal(new Vector2(KCDefine.B_VAL_0_REAL, Access.CellSize.y / KCDefine.B_VAL_2_REAL));
					a_oOutPosList.ExAddVal(new Vector2(Access.CellSize.x / KCDefine.B_VAL_2_REAL, Access.CellSize.y / -KCDefine.B_VAL_2_REAL));

					break;
				}
				case EObjKinds.NORM_BRICKS_TRIANGLE_02: {
					a_oOutPosList.ExAddVal(new Vector2(Access.CellSize.x / -KCDefine.B_VAL_2_REAL, Access.CellSize.y / KCDefine.B_VAL_2_REAL));
					a_oOutPosList.ExAddVal(new Vector2(Access.CellSize.x / KCDefine.B_VAL_2_REAL, Access.CellSize.y / KCDefine.B_VAL_2_REAL));
					a_oOutPosList.ExAddVal(new Vector2(KCDefine.B_VAL_0_REAL, Access.CellSize.y / -KCDefine.B_VAL_2_REAL));

					break;
				}
				case EObjKinds.NORM_BRICKS_TRIANGLE_03: {
					a_oOutPosList.ExAddVal(new Vector2(Access.CellSize.x / -KCDefine.B_VAL_2_REAL, Access.CellSize.y / -KCDefine.B_VAL_2_REAL));
					a_oOutPosList.ExAddVal(new Vector2(Access.CellSize.x / -KCDefine.B_VAL_2_REAL, Access.CellSize.y / KCDefine.B_VAL_2_REAL));
					a_oOutPosList.ExAddVal(new Vector2(Access.CellSize.x / KCDefine.B_VAL_2_REAL, KCDefine.B_VAL_0_REAL));

					break;
				}
				case EObjKinds.NORM_BRICKS_TRIANGLE_04: {
					a_oOutPosList.ExAddVal(new Vector2(Access.CellSize.x / -KCDefine.B_VAL_2_REAL, KCDefine.B_VAL_0_REAL));
					a_oOutPosList.ExAddVal(new Vector2(Access.CellSize.x / KCDefine.B_VAL_2_REAL, Access.CellSize.y / KCDefine.B_VAL_2_REAL));
					a_oOutPosList.ExAddVal(new Vector2(Access.CellSize.x / KCDefine.B_VAL_2_REAL, Access.CellSize.y / -KCDefine.B_VAL_2_REAL));

					break;
				}
			}
		}

		/** 직각 삼각형 충돌체를 설정한다 */
		private void SetupRightTriangleCollider(List<Vector2> a_oOutPosList) {
			switch(this.Params.m_stObjInfo.m_eObjKinds) {
				case EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_01: {
					a_oOutPosList.ExAddVal(new Vector2(Access.CellSize.x / -KCDefine.B_VAL_2_REAL, Access.CellSize.y / -KCDefine.B_VAL_2_REAL));
					a_oOutPosList.ExAddVal(new Vector2(Access.CellSize.x / -KCDefine.B_VAL_2_REAL, Access.CellSize.y / KCDefine.B_VAL_2_REAL));
					a_oOutPosList.ExAddVal(new Vector2(Access.CellSize.x / KCDefine.B_VAL_2_REAL, Access.CellSize.y / -KCDefine.B_VAL_2_REAL));

					break;
				}
				case EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_02: {
					a_oOutPosList.ExAddVal(new Vector2(Access.CellSize.x / -KCDefine.B_VAL_2_REAL, Access.CellSize.y / -KCDefine.B_VAL_2_REAL));
					a_oOutPosList.ExAddVal(new Vector2(Access.CellSize.x / -KCDefine.B_VAL_2_REAL, Access.CellSize.y / KCDefine.B_VAL_2_REAL));
					a_oOutPosList.ExAddVal(new Vector2(Access.CellSize.x / KCDefine.B_VAL_2_REAL, Access.CellSize.y / KCDefine.B_VAL_2_REAL));

					break;
				}
				case EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_03: {
					a_oOutPosList.ExAddVal(new Vector2(Access.CellSize.x / -KCDefine.B_VAL_2_REAL, Access.CellSize.y / -KCDefine.B_VAL_2_REAL));
					a_oOutPosList.ExAddVal(new Vector2(Access.CellSize.x / KCDefine.B_VAL_2_REAL, Access.CellSize.y / KCDefine.B_VAL_2_REAL));
					a_oOutPosList.ExAddVal(new Vector2(Access.CellSize.x / KCDefine.B_VAL_2_REAL, Access.CellSize.y / -KCDefine.B_VAL_2_REAL));

					break;
				}
				case EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_04: {
					a_oOutPosList.ExAddVal(new Vector2(Access.CellSize.x / -KCDefine.B_VAL_2_REAL, Access.CellSize.y / KCDefine.B_VAL_2_REAL));
					a_oOutPosList.ExAddVal(new Vector2(Access.CellSize.x / KCDefine.B_VAL_2_REAL, Access.CellSize.y / KCDefine.B_VAL_2_REAL));
					a_oOutPosList.ExAddVal(new Vector2(Access.CellSize.x / KCDefine.B_VAL_2_REAL, Access.CellSize.y / -KCDefine.B_VAL_2_REAL));

					break;
				}
			}
		}
		#endregion // 함수

		#region 접근자 함수
		/** 셀 객체 정보를 변경한다 */
		public void SetCellObjInfo(STCellObjInfo a_stCellObjInfo) {
			this.CellObjInfo = a_stCellObjInfo;
            
            if (a_stCellObjInfo.ObjKinds != EObjKinds.NONE)
            {
                EObjType cellType = (EObjType)((int)CellObjInfo.ObjKinds).ExKindsToType();
                
                SetSpriteColor(CellObjInfo.ObjKinds);
                RefreshText(cellType);
            }
		}
		#endregion // 접근자 함수
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

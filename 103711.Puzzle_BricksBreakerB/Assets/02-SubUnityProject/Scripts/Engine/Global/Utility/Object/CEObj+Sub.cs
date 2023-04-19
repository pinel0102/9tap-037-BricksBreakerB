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

            // 텍스트를 설정한다
			CFunc.SetupComponents(new List<(ESubKey, string, GameObject)>() {
				(ESubKey.HP_TEXT, $"{ESubKey.HP_TEXT}", this.gameObject),
				(ESubKey.NUM_TEXT, $"{ESubKey.NUM_TEXT}", this.gameObject)
			}, m_oSubTextDict);

            this.SetCellObjInfo(STCellObjInfo.INVALID);
		}

		/** 초기화한다 */
		private void SubInit() {
			this.SetupAbilityVals();

			// 텍스트를 갱신한다 {
			(m_oSubTextDict.GetValueOrDefault(ESubKey.HP_TEXT) as TextMeshPro)?.ExSetSortingOrder(GlobalDefine.SortingInfo_HPText);
			(m_oSubTextDict.GetValueOrDefault(ESubKey.NUM_TEXT) as TextMeshPro)?.ExSetSortingOrder(GlobalDefine.SortingInfo_NumText);
			// 텍스트를 갱신한다 }

			// 충돌체가 존재 할 경우
			if(this.TargetSprite != null && this.TargetSprite.TryGetComponent<PolygonCollider2D>(out PolygonCollider2D oCollider)) {
				var oPosList = CCollectionManager.Inst.SpawnList<Vector2>();
				oCollider.pathCount = KCDefine.B_VAL_1_INT;

				try {
					switch((EObjKinds)((int)this.Params.m_stObjInfo.m_eObjKinds).ExKindsToCorrectKinds(EKindsGroupType.SUB_KINDS_TYPE)) {
						case EObjKinds.NORM_BRICKS_TRIANGLE_01: 
                            this.SetupTriangleCollider(oPosList, this.Params.m_stObjInfo.m_stSize); break;

						case EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_01: 
                            this.SetupRightTriangleCollider(oPosList, this.Params.m_stObjInfo.m_stSize); break;

                        case EObjKinds.NORM_BRICKS_DIAMOND_01: 
                            this.SetupDiamondCollider(oPosList, this.Params.m_stObjInfo.m_stSize); break;

                        case EObjKinds.SPECIAL_BRICKS_BALL_AMPLIFICATION_01: 
                            this.SetupTriangleCollider(oPosList, this.Params.m_stObjInfo.m_stSize); break;

                        case EObjKinds.SPECIAL_BRICKS_BALL_DIFFUSION_01:
                        case EObjKinds.SPECIAL_BRICKS_LASER_HORIZONTAL_01:
                        case EObjKinds.SPECIAL_BRICKS_LASER_VERTICAL_01:
                        case EObjKinds.SPECIAL_BRICKS_LASER_CROSS_01:
                        case EObjKinds.ITEM_BRICKS_BALL_01:
                            this.SetupCircleCollider(oPosList, this.Params.m_stObjInfo.m_stSize.x * GlobalDefine.ColliderRadius_20); break;
                        
                        case EObjKinds.OBSTACLE_BRICKS_WARP_IN_01:
                        case EObjKinds.SPECIAL_BRICKS_POWERBALL_01:
                        case EObjKinds.ITEM_BRICKS_COINS_01:
                            this.SetupCircleCollider(oPosList, this.Params.m_stObjInfo.m_stSize.x * GlobalDefine.ColliderRadius_30); break;

                        case EObjKinds.SPECIAL_BRICKS_EXPLOSION_ALL_01:
                            this.SetupCircleCollider(oPosList, this.Params.m_stObjInfo.m_stSize.x * GlobalDefine.ColliderRadius_30, GlobalDefine.ColliderOffset_EXPLOSION_ALL); break;

                        default: 
                            this.SetupSquareCollider(oPosList, this.Params.m_stObjInfo.m_stSize); break;
					}

					oCollider.SetPath(KCDefine.B_VAL_0_INT, oPosList);
				} finally {
					CCollectionManager.Inst.DespawnList(oPosList);
				}
			}
		}

        /** 원 -> 팔각형 충돌체를 설정한다 */
		private void SetupCircleCollider(List<Vector2> a_oOutPosList, float radius) {            
            float root = radius / Mathf.Pow(2, 0.5f);

			a_oOutPosList.ExAddVal(new Vector2(radius, 0));
            a_oOutPosList.ExAddVal(new Vector2(root, -root));
            a_oOutPosList.ExAddVal(new Vector2(0, -radius));
            a_oOutPosList.ExAddVal(new Vector2(-root, -root));
            a_oOutPosList.ExAddVal(new Vector2(-radius, 0));
            a_oOutPosList.ExAddVal(new Vector2(-root, root));
            a_oOutPosList.ExAddVal(new Vector2(0, radius));
            a_oOutPosList.ExAddVal(new Vector2(root, root));
		}

        /** 원 -> 팔각형 충돌체를 설정한다 */
		private void SetupCircleCollider(List<Vector2> a_oOutPosList, float radius, Vector2 offset) {            
            float root = radius / Mathf.Pow(2, 0.5f);

			a_oOutPosList.ExAddVal(new Vector2(radius, 0) + offset);
            a_oOutPosList.ExAddVal(new Vector2(root, -root) + offset);
            a_oOutPosList.ExAddVal(new Vector2(0, -radius) + offset);
            a_oOutPosList.ExAddVal(new Vector2(-root, -root) + offset);
            a_oOutPosList.ExAddVal(new Vector2(-radius, 0) + offset);
            a_oOutPosList.ExAddVal(new Vector2(-root, root) + offset);
            a_oOutPosList.ExAddVal(new Vector2(0, radius) + offset);
            a_oOutPosList.ExAddVal(new Vector2(root, root) + offset);
		}

        /** 사각형 충돌체를 설정한다 */
		private void SetupSquareCollider(List<Vector2> a_oOutPosList, Vector3Int objSize) {
            float width = Access.CellSize.x * objSize.x;
            float height = Access.CellSize.y * objSize.y;
            
            a_oOutPosList.ExAddVal(new Vector2(width / -KCDefine.B_VAL_2_REAL, height / -KCDefine.B_VAL_2_REAL));
			a_oOutPosList.ExAddVal(new Vector2(width / -KCDefine.B_VAL_2_REAL, height / KCDefine.B_VAL_2_REAL));
			a_oOutPosList.ExAddVal(new Vector2(width / KCDefine.B_VAL_2_REAL, height / KCDefine.B_VAL_2_REAL));
			a_oOutPosList.ExAddVal(new Vector2(width / KCDefine.B_VAL_2_REAL, height / -KCDefine.B_VAL_2_REAL));
		}

        /** 마름모 충돌체를 설정한다 */
		private void SetupDiamondCollider(List<Vector2> a_oOutPosList, Vector3Int objSize) {
            float width = Access.CellSize.x * objSize.x;
            float height = Access.CellSize.y * objSize.y;

            a_oOutPosList.ExAddVal(new Vector2(0, height / -KCDefine.B_VAL_2_REAL));
            a_oOutPosList.ExAddVal(new Vector2(width / KCDefine.B_VAL_2_REAL, 0));
			a_oOutPosList.ExAddVal(new Vector2(0, height / KCDefine.B_VAL_2_REAL));
			a_oOutPosList.ExAddVal(new Vector2(width / -KCDefine.B_VAL_2_REAL, 0));
		}

        /** 삼각형 충돌체를 설정한다 */
		private void SetupTriangleCollider(List<Vector2> a_oOutPosList, Vector3Int objSize) {
            float width = Access.CellSize.x * objSize.x;
            float height = Access.CellSize.y * objSize.y;

			switch(this.Params.m_stObjInfo.m_eObjKinds) {
				case EObjKinds.NORM_BRICKS_TRIANGLE_01:
                case EObjKinds.SPECIAL_BRICKS_BALL_AMPLIFICATION_01: {
					a_oOutPosList.ExAddVal(new Vector2(width / -KCDefine.B_VAL_2_REAL, height / -KCDefine.B_VAL_2_REAL));
					a_oOutPosList.ExAddVal(new Vector2(KCDefine.B_VAL_0_REAL, height / KCDefine.B_VAL_2_REAL));
					a_oOutPosList.ExAddVal(new Vector2(width / KCDefine.B_VAL_2_REAL, height / -KCDefine.B_VAL_2_REAL));

					break;
				}
				case EObjKinds.NORM_BRICKS_TRIANGLE_02: {
					a_oOutPosList.ExAddVal(new Vector2(width / -KCDefine.B_VAL_2_REAL, height / KCDefine.B_VAL_2_REAL));
					a_oOutPosList.ExAddVal(new Vector2(width / KCDefine.B_VAL_2_REAL, height / KCDefine.B_VAL_2_REAL));
					a_oOutPosList.ExAddVal(new Vector2(KCDefine.B_VAL_0_REAL, height / -KCDefine.B_VAL_2_REAL));

					break;
				}
				case EObjKinds.NORM_BRICKS_TRIANGLE_03: {
					a_oOutPosList.ExAddVal(new Vector2(width / -KCDefine.B_VAL_2_REAL, height / -KCDefine.B_VAL_2_REAL));
					a_oOutPosList.ExAddVal(new Vector2(width / -KCDefine.B_VAL_2_REAL, height / KCDefine.B_VAL_2_REAL));
					a_oOutPosList.ExAddVal(new Vector2(width / KCDefine.B_VAL_2_REAL, KCDefine.B_VAL_0_REAL));

					break;
				}
				case EObjKinds.NORM_BRICKS_TRIANGLE_04: {
					a_oOutPosList.ExAddVal(new Vector2(width / -KCDefine.B_VAL_2_REAL, KCDefine.B_VAL_0_REAL));
					a_oOutPosList.ExAddVal(new Vector2(width / KCDefine.B_VAL_2_REAL, height / KCDefine.B_VAL_2_REAL));
					a_oOutPosList.ExAddVal(new Vector2(width / KCDefine.B_VAL_2_REAL, height / -KCDefine.B_VAL_2_REAL));

					break;
				}
			}
		}

		/** 직각 삼각형 충돌체를 설정한다 */
		private void SetupRightTriangleCollider(List<Vector2> a_oOutPosList, Vector3Int objSize) {
            float width = Access.CellSize.x * objSize.x;
            float height = Access.CellSize.y * objSize.y;

			switch(this.Params.m_stObjInfo.m_eObjKinds) {
				case EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_01: {
					a_oOutPosList.ExAddVal(new Vector2(width / -KCDefine.B_VAL_2_REAL, height / -KCDefine.B_VAL_2_REAL));
					a_oOutPosList.ExAddVal(new Vector2(width / -KCDefine.B_VAL_2_REAL, height / KCDefine.B_VAL_2_REAL));
					a_oOutPosList.ExAddVal(new Vector2(width / KCDefine.B_VAL_2_REAL, height / -KCDefine.B_VAL_2_REAL));

					break;
				}
				case EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_02: {
					a_oOutPosList.ExAddVal(new Vector2(width / -KCDefine.B_VAL_2_REAL, height / -KCDefine.B_VAL_2_REAL));
					a_oOutPosList.ExAddVal(new Vector2(width / -KCDefine.B_VAL_2_REAL, height / KCDefine.B_VAL_2_REAL));
					a_oOutPosList.ExAddVal(new Vector2(width / KCDefine.B_VAL_2_REAL, height / KCDefine.B_VAL_2_REAL));

					break;
				}
				case EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_03: {
					a_oOutPosList.ExAddVal(new Vector2(width / -KCDefine.B_VAL_2_REAL, height / -KCDefine.B_VAL_2_REAL));
					a_oOutPosList.ExAddVal(new Vector2(width / KCDefine.B_VAL_2_REAL, height / KCDefine.B_VAL_2_REAL));
					a_oOutPosList.ExAddVal(new Vector2(width / KCDefine.B_VAL_2_REAL, height / -KCDefine.B_VAL_2_REAL));

					break;
				}
				case EObjKinds.NORM_BRICKS_RIGHT_TRIANGLE_04: {
					a_oOutPosList.ExAddVal(new Vector2(width / -KCDefine.B_VAL_2_REAL, height / KCDefine.B_VAL_2_REAL));
					a_oOutPosList.ExAddVal(new Vector2(width / KCDefine.B_VAL_2_REAL, height / KCDefine.B_VAL_2_REAL));
					a_oOutPosList.ExAddVal(new Vector2(width / KCDefine.B_VAL_2_REAL, height / -KCDefine.B_VAL_2_REAL));

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
                switch(cellType)
                {
                    case EObjType.NORM_BRICKS:
                    case EObjType.ITEM_BRICKS:
                    case EObjType.SPECIAL_BRICKS:
                    case EObjType.OBSTACLE_BRICKS:
                        SetSpriteColor(CellObjInfo.ObjKinds);
                        RefreshText(CellObjInfo.ObjKinds);
                        break;
                    default:
                        RefreshText(CellObjInfo.ObjKinds);
                        break;
                }
            }
		}
		#endregion // 접근자 함수

        /** 원본 객체 정보를 설정한다 */
		/*public void SetOriginObjInfo(STObjInfo a_stObjInfo) {
			this.OriginObjInfo = a_stObjInfo;
		}*/

        public void SetExtraObjKindsList(STObjInfo a_stObjInfo)
        {
            if(base.Params.m_stBaseParams.m_oObjsPoolKey.Equals(KDefine.E_KEY_CELL_OBJ_OBJS_POOL)) {
				a_stObjInfo.m_oExtraObjKindsList.ExCopyTo(this.GetController<CECellObjController>().ExtraObjKindsList, (a_eObjKinds) => a_eObjKinds);
                this.GetController<CECellObjController>().ExtraObjKindsList.ExRemoveVals((a_eObjKinds) => a_eObjKinds == EObjKinds.NONE);

                // 랜덤 모드 일 경우
				if(a_stObjInfo.m_bIsRand) {
					this.GetController<CECellObjController>().ExtraObjKindsList.ExShuffle();
				}
			}
        }
	}
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

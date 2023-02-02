using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

#if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE
namespace NSEngine {
	/** 공 객체 제어자 */
	public partial class CEBallObjController : CEObjController {
		/** 식별자 */
		private enum EKey {
			NONE = -1,
			SPEED,
			//DIRECTION,
			[HideInInspector] MAX_VAL
		}

		/** 매개 변수 */
		public new struct STParams {
			public CEObjController.STParams m_stBaseParams;
		}

		#region 변수
		private Dictionary<EKey, float> m_oRealDict = new Dictionary<EKey, float>() {
			[EKey.SPEED] = KCDefine.B_VAL_0_REAL
		};

		/*private Dictionary<EKey, Vector3> m_oVec3Dict = new Dictionary<EKey, Vector3>() {
			[EKey.DIRECTION] = Vector3.zero
		};*/
		#endregion // 변수

		#region 프로퍼티
		public new STParams Params { get; private set; }

        #endregion // 프로퍼티

		#region 함수
		/** 초기화 */
		public override void Awake() {
			base.Awake();
		}

		/** 초기화 */
		public virtual void Init(STParams a_stParams, int _index) {
			base.Init(a_stParams.m_stBaseParams);
			this.Params = a_stParams;

            this.myIndex = _index;
		}

		/** 상태를 갱신한다 */
		public override void OnUpdate(float a_fDeltaTime) {
			base.OnUpdate(a_fDeltaTime);

			// 앱이 실행 중 일 경우
			if(this.IsActive && CSceneManager.IsAppRunning) {
				// Do Something
			}
		}

		/** 발사한다 */
		public void Shoot(Vector3 a_stVelocity) {
			this.SetState(EState.MOVE, true);
            this.SetMoveDirection(a_stVelocity.normalized);

			m_oRealDict[EKey.SPEED] = a_stVelocity.magnitude;
		}

		/** 이동 상태를 처리한다 */
		protected override void HandleMoveState(float a_fDeltaTime) {
			base.HandleMoveState(a_fDeltaTime);

            var stVelocity = (this.MoveDirection * m_oRealDict[EKey.SPEED]) * a_fDeltaTime;
            var stWorldPos = (this.GetOwner<CEObj>().transform.localPosition + stVelocity.normalized).ExToWorld(this.Engine.Params.m_oObjRoot);
			var stRaycastHit = Physics2D.CircleCast(stWorldPos, this.GetOwner<CEObj>().TargetSprite.sprite.textureRect.size.ExToWorld(this.Engine.Params.m_oObjRoot).x / KCDefine.B_VAL_2_REAL, stVelocity.normalized, GlobalDefine.RAYCAST_DISTANCE, Engine.layerWallAndBricks);
            
			var stHitPos = (stWorldPos + (stVelocity.normalized * stRaycastHit.distance)).ExToLocal(this.Engine.Params.m_oObjRoot);
			var stHitDelta = (this.GetOwner<CEObj>().transform.localPosition + stVelocity.normalized) - stHitPos;

			// 충돌체가 존재 할 경우
			if(stRaycastHit.collider != null && stHitDelta.magnitude.ExIsLessEquals(stVelocity.magnitude)) {
				var oCellObj = stRaycastHit.collider.GetComponentInParent<CEObj>();

                // 셀이 존재 할 경우
				if(oCellObj != null && oCellObj.TryGetComponent<CECellObjController>(out CECellObjController oController)) 
                {
                    switch((EObjType)((int)oCellObj.Params.m_stObjInfo.m_eObjKinds).ExKindsToType()) 
                    {
                        case EObjType.NORM_BRICKS:
                        case EObjType.OBSTACLE_BRICKS:
                            this.GetOwner<CEObj>().transform.localPosition = stHitPos;
                            this.SetMoveDirection(Vector3.Reflect(stVelocity.normalized, stRaycastHit.normal).normalized);
                            break;
                        default:
                            Debug.Log(CodeManager.GetMethodName() + string.Format("<color=red>{0}</color>", (EObjType)((int)oCellObj.Params.m_stObjInfo.m_eObjKinds).ExKindsToType()));
                            this.GetOwner<CEObj>().transform.localPosition += stVelocity;
                            break;
                    }

					oController.OnHit(this.GetOwner<CEObj>());
				}
				// 하단 영역 일 경우
				else if(stRaycastHit.collider.gameObject == this.Engine.DownBoundsSprite.gameObject) 
                {
                    this.GetOwner<CEObj>().transform.localPosition = stHitPos;
                    this.SetMoveDirection(Vector3.Reflect(stVelocity.normalized, stRaycastHit.normal).normalized);

					this.SetState(EState.IDLE, true);
					this.GetOwner<CEObj>().Params.m_stBaseParams.m_oCallbackDict.GetValueOrDefault(CEObjComponent.ECallback.ENGINE_OBJ_EVENT)?.Invoke(this.GetOwner<CEObj>(), EEngineObjEvent.MOVE_COMPLETE, string.Empty);
				}
                else // 상단 or 좌우 벽일 경우.
                {
                    this.GetOwner<CEObj>().transform.localPosition = stHitPos;
                    this.SetMoveDirection(Vector3.Reflect(stVelocity.normalized, stRaycastHit.normal).normalized);
                }
			} else {
                this.GetOwner<CEObj>().transform.localPosition += stVelocity;
			}
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
}
#endif // #if EXTRA_SCRIPT_MODULE_ENABLE && UTILITY_SCRIPT_TEMPLATES_MODULE_ENABLE

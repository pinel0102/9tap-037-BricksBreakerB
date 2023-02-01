using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
    public partial class CEBallObjController : CEObjController
    {
        public CircleCollider2D _collider;
        public int myIndex;

        public Vector3 moveVector = Vector3.zero;
        private float deltaTime;

        /// <summary>
        /// Sent when an incoming collider makes contact with this object's
        /// collider (2D physics only).
        /// </summary>
        /// <param name="other">The Collision2D data associated with this collision.</param>
        private void OnCollisionEnter2D(Collision2D other)
        {
            EObjKinds kinds =  other.gameObject.GetComponent<CEObj>().Params.m_stObjInfo.m_eObjKinds;
            EObjType cellType = (EObjType)((int)kinds).ExKindsToType();

            Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>{0}</color>", kinds));
        }

        /// <summary>
        /// Sent when another object enters a trigger collider attached to this
        /// object (2D physics only).
        /// </summary>
        /// <param name="other">The other Collider2D involved in this collision.</param>
        private void OnTriggerEnter2D(Collider2D other)
        {
            EObjKinds kinds =  other.GetComponentInParent<CEObj>().Params.m_stObjInfo.m_eObjKinds;
            EObjType cellType = (EObjType)((int)kinds).ExKindsToType();

            Debug.Log(CodeManager.GetMethodName() + string.Format("<color=yellow>{0}</color>", kinds));

            // 충돌체가 존재 할 경우
			/*if(stRaycastHit.collider != null && stHitDelta.magnitude.ExIsLessEquals(stVelocity.magnitude)) {
				var oCellObj = stRaycastHit.collider.GetComponentInParent<CEObj>();

                // 셀이 존재 할 경우
				if(oCellObj != null && oCellObj.TryGetComponent<CECellObjController>(out CECellObjController oController)) 
                {
                    switch(cellType) 
                    {
                        case EObjType.NORM_BRICKS:
                        case EObjType.OBSTACLE_BRICKS:
                            this.GetOwner<CEObj>().transform.localPosition = stHitPos;
                            this.SetMoveDirection(Vector3.Reflect(stVelocity.normalized, stRaycastHit.normal).normalized);
                            break;
                        
                        case EObjType.ITEM_BRICKS:
                        case EObjType.SPECIAL_BRICKS:
                            // Do Not Reflect
                            this.GetOwner<CEObj>().transform.localPosition += stVelocity;
                            break;
                        
                        default : 
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
			}*/
        }

        public void SetBallCollider(bool _enabled)
        {
            return;
            
            if (_collider == null)
                _collider = GetComponent<CircleCollider2D>();
            
            _collider.enabled = _enabled;
        }
    }
}
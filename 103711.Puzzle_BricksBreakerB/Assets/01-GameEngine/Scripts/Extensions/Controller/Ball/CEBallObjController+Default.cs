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

        private void OnTriggerEnter2D(Collider2D other)
        {
            CECellObjController oController = other.GetComponentInParent<CECellObjController>();

            if(oController != null)
            {
                EObjKinds kinds =  oController.GetOwner<CEObj>().Params.m_stObjInfo.m_eObjKinds;
                EObjType cellType = (EObjType)((int)kinds).ExKindsToType();

                switch(cellType) 
                {
                    case EObjType.ITEM_BRICKS:
                    case EObjType.SPECIAL_BRICKS:
                        oController.OnHit(this.GetOwner<CEObj>());
                        break;
                    default:
                        Debug.Log(CodeManager.GetMethodName() + string.Format("<color=red>{0}</color>", cellType));
                        break;
                }
            }
        }

        public void SetBallCollider(bool _enabled)
        {
            if (_collider == null)
                _collider = GetComponent<CircleCollider2D>();
            
            _collider.enabled = _enabled;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
    public partial class CEBallObjController : CEObjController
    {
        public float defaultRadius;
        public Vector3 defaultBallScale;

        public bool isRemoveMoveEnd;
        public bool isNotAffect;
        public int extraATK;
        
        public CircleCollider2D _collider;
        public int myIndex;

        //public Vector3 moveVector = Vector3.zero;
        //private float deltaTime;

        private CEObj _ceObj;

        private void SaveDefaultState(CEObj oObj)
        {
            _ceObj = oObj;

            if (_collider == null)
                _collider = GetComponent<CircleCollider2D>();

            defaultRadius = _collider.radius;
            defaultBallScale = _ceObj.TargetSprite.transform.localScale;
            
            isRemoveMoveEnd = false;
        }

        public void Initialize()
        {
            SetState(EState.IDLE, true);
            
            SetBallSize(1f);            
            isNotAffect = false;
            extraATK = 0;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (this.State == EState.IDLE || isNotAffect) return;

            CECellObjController oController = other.GetComponentInParent<CECellObjController>();

            if(oController != null)
            {
                EObjKinds kinds =  oController.GetOwner<CEObj>().Params.m_stObjInfo.m_eObjKinds;
                EObjType cellType = (EObjType)((int)kinds).ExKindsToType();

                switch(cellType) 
                {
                    case EObjType.ITEM_BRICKS:
                    case EObjType.SPECIAL_BRICKS:
                        oController.OnHit(this.GetOwner<CEObj>(), this);
                        break;
                    default:
                        //Debug.Log(CodeManager.GetMethodName() + string.Format("<color=red>{0}</color>", cellType));
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

        public void SetBallSize(float multiplier)
        {
            _collider.radius = defaultRadius * multiplier;
            _ceObj.TargetSprite.transform.localScale = defaultBallScale * multiplier;
        }
    }
}
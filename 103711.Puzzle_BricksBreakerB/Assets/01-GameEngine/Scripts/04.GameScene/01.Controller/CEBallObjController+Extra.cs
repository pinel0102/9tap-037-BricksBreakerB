using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NSEngine {
    public partial class CEBallObjController : CEObjController
    {
        public int myIndex;
        public int extraATK;
        public bool isOn_PowerBall;
        public bool isOn_Amplification;
        public List<CECellObjController> usedWormholes = new List<CECellObjController>();
        
        private CEObj _ceObj;
        private CircleCollider2D _collider;
        private float defaultRadius;

        private void SaveDefaultState(CEObj oObj)
        {
            _ceObj = oObj;

            if (_collider == null)
                _collider = GetComponent<CircleCollider2D>();

            defaultRadius = _collider.radius;
        }

        public void Initialize()
        {
            SetState(EState.IDLE, true);
            
            FXToggle_PowerBall(false);
            isOn_Amplification = false;
            isOn_PowerBall = false;
            usedWormholes.Clear();
            extraATK = 0;
        }

        public void ChangeToNormalBall()
        {
            _ceObj.ChangeKinds(EObjKinds.BALL_NORM_01);
        }

        // 반사되지 않는 셀은 1번만 효과를 발동해야 하므로 여기에서 처리한다.
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (this.State == EState.IDLE) return;

            var oCellObj = other.GetComponentInParent<CEObj>();
            if(oCellObj != null && oCellObj.TryGetComponent<CECellObjController>(out CECellObjController oController)) 
            {
                if (oCellObj.Params.m_stObjInfo.m_bIsEnableReflect)
                    return;
               
                EObjKinds kinds =  oCellObj.Params.m_stObjInfo.m_eObjKinds;
                EObjType cellType = (EObjType)((int)kinds).ExKindsToType();
                switch(cellType)
                {
                    case EObjType.ITEM_BRICKS:
                    case EObjType.SPECIAL_BRICKS:
                    case EObjType.OBSTACLE_BRICKS:
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

        public void FXToggle_PowerBall(bool _isOn)
        {
            _ceObj.FXPowerBall.SetActive(_isOn);
            _collider.radius = _isOn ? defaultRadius * GlobalDefine.FXPowerBall_Size : defaultRadius;
        }
    }
}
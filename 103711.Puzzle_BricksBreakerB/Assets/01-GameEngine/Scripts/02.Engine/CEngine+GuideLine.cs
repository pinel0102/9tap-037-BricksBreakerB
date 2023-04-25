using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Timers;

namespace NSEngine {
    public partial class CEngine : CComponent
    {
        public void ResetGuideLine()
        {
			for(int i = 0; i < m_oAimDotList.Count; ++i) {
				m_oAimDotList[i].gameObject.SetActive(false);
				CSceneManager.ActiveSceneManager.DespawnObj(KDefine.E_KEY_AIM_DOT_OBJS_POOL, m_oAimDotList[i]);
			}

			m_oAimDotList.Clear();
		}

        public void DrawGuideLine(Vector3 stPos)
        {
            // 조준 가능 할 경우
            if(this.IsEnableAiming(stPos)) 
            {
                subGameSceneManager.warningObject.SetActive(false);
                currentShootCount = 0;

                SetupGuideLine(this.SelBallObj.transform.localPosition, stPos, this.SelBallObj.TargetSprite.sprite.textureRect.size.ExToWorld(this.Params.m_oAimRoot).x / KCDefine.B_VAL_2_REAL, isGoldAim ? GlobalDefine.AIM_BOUND_COUNT_GOLDEN : GlobalDefine.AIM_BOUND_COUNT_NORMAL);
            } 
            else 
            {
                ResetGuideLine();
            }
        }

        private void SetupGuideLine(Vector3 fromPosition, Vector3 toPosition, float radius, int boundCount)
        {
            var stDirection = (toPosition - fromPosition).normalized;
            //Debug.Log(CodeManager.GetMethodName() + string.Format("{0} -> {1} / {2}", fromPosition, toPosition, stDirection));

            //float fAngle = Vector3.Angle(stDirection, Vector3.right * Mathf.Sign(stDirection.x));
            //fAngle = fAngle.ExIsLess(KDefine.E_MIN_ANGLE_AIMING) ? KDefine.E_MIN_ANGLE_AIMING : fAngle;            
            //stDirection = new Vector3(Mathf.Cos(fAngle * Mathf.Deg2Rad) * Mathf.Sign(stDirection.x), Mathf.Sin(fAngle * Mathf.Deg2Rad), KCDefine.B_VAL_0_REAL);

            var stWorldPos = (fromPosition + (stDirection)).ExToWorld(this.Params.m_oAimRoot);
            var stRaycastHit = Physics2D.CircleCast(stWorldPos, radius, stDirection, GlobalDefine.RAYCAST_DISTANCE, currentAimLayer);            
            if (stRaycastHit.collider != null) 
            {
                var stHitPos = (stWorldPos + (stDirection * ((!isGoldAim && boundCount == 0) ? Mathf.Min(stRaycastHit.distance, GlobalDefine.AIM_LENGTH_SHORT) : stRaycastHit.distance))).ExToLocal(this.Params.m_oAimRoot);
                    
                this.SetupDots(fromPosition, stHitPos, m_oAimDotList);

                if(boundCount > 0 && stRaycastHit.collider.gameObject != DownBoundsSprite.gameObject)
                {
                    var stReflect = Vector3.Reflect(stDirection, stRaycastHit.normal).normalized;

                    var stWorldPos2 = (fromPosition + stReflect).ExToWorld(this.Params.m_oAimRoot, false);
                    var stRaycastHit2 = Physics2D.CircleCast(stWorldPos2, radius, stReflect, GlobalDefine.RAYCAST_DISTANCE, currentAimLayer);
                    if (stRaycastHit2.collider != null)
                    {
                        var stReflectHitPos = (stWorldPos2 + (stReflect * stRaycastHit2.distance)).ExToLocal(this.Params.m_oAimRoot);

                        this.SetupGuideLine(stHitPos, stHitPos + (stReflect * (stReflectHitPos - stHitPos).magnitude), radius, boundCount - 1);
                    }
                }
            }
        }
        
        private void SetupDots(Vector3 fromPosition, Vector3 toPosition, List<GameObject> a_oOutAimDotList) {
			var stDirection = toPosition - fromPosition;

            for(int i = 0; i <= (int)(stDirection.magnitude / GlobalDefine.AIM_DOT_OFFSET); i++) {
				var oAimingDot = CSceneManager.ActiveSceneManager.SpawnObj(KDefine.E_OBJ_N_AIM, KDefine.E_KEY_AIM_DOT_OBJS_POOL);
				oAimingDot.transform.localPosition = fromPosition + (stDirection.normalized * (i * GlobalDefine.AIM_DOT_OFFSET));

                var aimingDot = oAimingDot.GetComponent<AimingDot>();
                aimingDot.TargetSpriteNormal.gameObject.SetActive(i != 0);
                aimingDot.TargetSpriteEdge.gameObject.SetActive(i == 0);

				a_oOutAimDotList.Add(oAimingDot);
			}
		}
    }
}
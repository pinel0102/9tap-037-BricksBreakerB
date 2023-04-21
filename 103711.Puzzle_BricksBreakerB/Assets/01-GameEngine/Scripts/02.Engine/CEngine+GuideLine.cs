using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Timers;

namespace NSEngine {
    public partial class CEngine : CComponent
    {
        private void DrawGuideLine(Vector3 stPos)
        {
            // 조준 가능 할 경우
            if(this.IsEnableAiming(stPos)) {

                subGameSceneManager.warningObject.SetActive(false);

                currentShootCount = 0;

                var oPosList = CCollectionManager.Inst.SpawnList<Vector3>();
                var stDirection = stPos - this.SelBallObj.transform.localPosition;

                try {
                    float fAngle = Vector3.Angle(stDirection, Vector3.right * Mathf.Sign(stDirection.x));
                    fAngle = fAngle.ExIsLess(KDefine.E_MIN_ANGLE_AIMING) ? KDefine.E_MIN_ANGLE_AIMING : fAngle;
                    
                    stDirection = new Vector3(Mathf.Cos(fAngle * Mathf.Deg2Rad) * Mathf.Sign(stDirection.x), Mathf.Sin(fAngle * Mathf.Deg2Rad), KCDefine.B_VAL_0_REAL);
                    
                    var stWorldPos = (this.SelBallObj.transform.localPosition + stDirection.normalized).ExToWorld(this.Params.m_oObjRoot);
                    var stRaycastHit = Physics2D.CircleCast(stWorldPos, this.SelBallObj.TargetSprite.sprite.textureRect.size.ExToWorld(this.Params.m_oObjRoot).x / KCDefine.B_VAL_2_REAL, stDirection.normalized, GlobalDefine.RAYCAST_DISTANCE, currentAimLayer);
                    
                    oPosList.ExAddVal(this.SelBallObj.transform.localPosition);

                    // 충돌체가 존재 할 경우
                    if(stRaycastHit.collider != null) {
                        //Debug.Log(string.Format("stRaycastHit.collider == {0}", stRaycastHit.collider.name));
                        var stHitPos = (stWorldPos + (stDirection.normalized * stRaycastHit.distance)).ExToLocal(this.Params.m_oObjRoot);
                        var stReflect = Vector3.Reflect(stDirection.normalized, stRaycastHit.normal);

                        stWorldPos = (stHitPos + stReflect.normalized).ExToWorld(this.Params.m_oObjRoot, false);
                        stRaycastHit = Physics2D.CircleCast(stWorldPos, this.SelBallObj.TargetSprite.sprite.textureRect.size.ExToWorld(this.Params.m_oObjRoot).x / KCDefine.B_VAL_2_REAL, stReflect.normalized, GlobalDefine.RAYCAST_DISTANCE, currentAimLayer);

                        oPosList.ExAddVal(stHitPos);

                        // 반사 가능 할 경우
                        if(!Vector3.Dot(stDirection.normalized, stReflect.normalized).ExIsEquals(-KCDefine.B_VAL_1_REAL)) {
                            var stReflectHitPos = (stWorldPos + (stReflect.normalized * stRaycastHit.distance)).ExToLocal(this.Params.m_oObjRoot);
                            oPosList.ExAddVal(stHitPos + (stReflect.normalized * Mathf.Min((stReflectHitPos - stHitPos).magnitude, KDefine.E_LENGTH_LINE)));
                        }
                    }

                    m_oSubLineFXDict[ESubKey.LINE_FX].ExSetPositions(oPosList);
                    m_oSubLineFXDict[ESubKey.LINE_FX].gameObject.SetActive(true);
                } finally {
                    CCollectionManager.Inst.DespawnList(oPosList);
                }
            } else {
                m_oSubLineFXDict[ESubKey.LINE_FX].gameObject.SetActive(false);
            }
        }
    }
}
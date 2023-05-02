using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

namespace GameScene {
    public partial class CSubGameSceneManager : CGameSceneManager
    {
        [Header("★ [Reference] Aim mode")]
        public List<Image> aimDisableRaycast = new List<Image>();
        public GameObject aimLayer;
        public GameObject aimLayerOn;
        public RectTransform aimDragArea;
        public RectTransform aimControl;
        public Image goldenAimButton;
        public bool isAimLayerOn;
        public bool isAimDragOn;

        private float aimAreaChangeArea;
        private float aimAreaWidthHalf;
        private Vector3 aimControlSize = new Vector3(1.2f, 1.2f, 1f);
        private Vector3 aimControlDefaultPosition = Vector3.zero;

        private void Update()
        {
            if (isAimLayerOn && isAimDragOn)
            {
                var worldPosition = ExGetWorldPos(Input.mousePosition, CSceneManager.ActiveSceneManager.ScreenSize);
                var localPosition = ExToLocal(worldPosition, Engine.Params.m_oObjRoot);
                var stPos = GetAimPosition(localPosition);

                SetAimControl(new Vector3(Mathf.Clamp(localPosition.x * Engine.SelGridInfo.m_stScale.x, -aimAreaWidthHalf, aimAreaWidthHalf), 0, 0));

                if (Engine.IsEnableAiming(stPos))
                {
                    Engine.ResetGuideLine();
                    Engine.DrawGuideLine(stPos);
                }
            }
        }

        public void InitAimLayer()
        {
            isAimLayerOn = false;
            isAimDragOn = false;

            aimAreaChangeArea = aimDragArea.sizeDelta.x * 0.2f;
            aimAreaWidthHalf = aimDragArea.sizeDelta.x * 0.5f;

            aimLayerOn.SetActive(isAimLayerOn);
            aimLayer.SetActive(isAimLayerOn);
            itemLayer.SetActive(!isAimLayerOn);
            
            SetAimDisableRaycast(!isAimDragOn);
            SetAimControl(aimControlDefaultPosition);
        }

#region Buttons

        public void ToggleAimLayer()
        {
            isAimLayerOn = !isAimLayerOn;
            isAimDragOn = false;

            aimLayerOn.SetActive(isAimLayerOn);
            aimLayer.SetActive(isAimLayerOn);
            itemLayer.SetActive(!isAimLayerOn);
            
            SetAimDisableRaycast(!isAimDragOn);
            SetAimControl(aimControlDefaultPosition);
        }

        public void ToggleGoldenAim()
        {
            if (GlobalDefine.isLevelEditor || GlobalDefine.UserInfo.Item_GoldenAim || Engine.isGoldAimOneTime)
            {
                m_oEngine.ToggleAimLayer();
                RefreshGoldenAimButton();
            }
            else
            {
                GlobalDefine.OpenShop();
            }
        }

        public void RefreshGoldenAimButton()
        {
            goldenAimButton.color = m_oEngine.isGoldAim ? GlobalDefine.COLOR_GOLDEN_AIM_ON : GlobalDefine.COLOR_GOLDEN_AIM_OFF;
        }

        public void OnAimDragBegin()
        {
            //Debug.Log(CodeManager.GetMethodName());
            isAimDragOn = true;
            SetAimDisableRaycast(!isAimDragOn);
        }

        public void OnAimDragEnd()
        {
            if (isAimLayerOn && isAimDragOn)
            {
                Debug.Log(CodeManager.GetMethodName());
                
                var worldPosition = ExGetWorldPos(Input.mousePosition, CSceneManager.ActiveSceneManager.ScreenSize);
                var localPosition = ExToLocal(worldPosition, Engine.Params.m_oObjRoot);
                var stPos = GetAimPosition(localPosition);
                
                Engine.CallShoot(stPos);
                
                isAimDragOn = false;
                SetAimDisableRaycast(!isAimDragOn);
                SetAimControl(aimControlDefaultPosition);
            }
        }

        public void OnAimDragCancel()
        {
            //Debug.Log(CodeManager.GetMethodName());
            Engine.ResetGuideLine();
            
            isAimDragOn = false;
            SetAimDisableRaycast(!isAimDragOn);
            SetAimControl(aimControlDefaultPosition);
        }

        private void SetAimDisableRaycast(bool value)
        {
            for(int i=0; i < aimDisableRaycast.Count; i++)
            {
                aimDisableRaycast[i].raycastTarget = value;
            }
        }

#endregion Buttons


#region Aim Mode

        private void SetAimControl(Vector3 position)
        {
            Vector3 scale = (isAimLayerOn && isAimDragOn) ? aimControlSize : Vector3.one;

            aimControl.localScale = scale;
            aimControl.localPosition = position;
        }

        private Vector3 GetAimPosition(Vector3 mousePosition)
        {
            float xPosition = 0;
            float yPosition = 0;
            float scaleAdjust = Engine.SelGridInfo.m_stScale.x;

            if(Mathf.Abs(mousePosition.x) < aimAreaChangeArea)
            {
                xPosition = mousePosition.x * 2f;
                yPosition = GlobalDefine.aimYPositionDefault;
            }
            else
            {
                xPosition = Mathf.Clamp(mousePosition.x, -aimAreaWidthHalf / scaleAdjust, aimAreaWidthHalf / scaleAdjust) * 2f;

                float distance = Mathf.Min(Mathf.Abs(xPosition * 0.5f) - aimAreaChangeArea, aimAreaWidthHalf) * 2.5f;
                yPosition = Mathf.Max(GlobalDefine.aimYPositionDefault - distance, GlobalDefine.aimYPositionMin);
            }

            return new Vector3(xPosition * scaleAdjust, yPosition / scaleAdjust, 0);
        }

        private Vector3 ExGetWorldPos(Vector3 a_stSender, Vector3 a_stScreenSize) 
        {
            float fNormPosX = ((a_stSender.x * KCDefine.B_VAL_2_REAL) / CAccess.DeviceScreenSize.x) - KCDefine.B_VAL_1_REAL;
            float fNormPosY = ((a_stSender.y * KCDefine.B_VAL_2_REAL) / CAccess.DeviceScreenSize.y) - KCDefine.B_VAL_1_REAL;

            float fScreenWidth = a_stScreenSize.y * (CAccess.DeviceScreenSize.x / CAccess.DeviceScreenSize.y);
            return new Vector3(fNormPosX * (fScreenWidth / KCDefine.B_VAL_2_REAL), fNormPosY * (a_stScreenSize.y / KCDefine.B_VAL_2_REAL), a_stSender.z) * KCDefine.B_UNIT_SCALE;
        }

        private Vector3 ExToLocal(Vector3 a_stSender, GameObject a_oParent, bool a_bIsCoord = true) 
        {
		    return a_bIsCoord ? a_oParent.transform.InverseTransformPoint(a_stSender) : a_oParent.transform.InverseTransformDirection(a_stSender);
	    }
    }

#endregion Aim Mode
}
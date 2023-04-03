using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace GameScene {
    public partial class CSubGameSceneManager : CGameSceneManager
    {
        private bool isShaking;
        private float shakeTime = 0.3f;
        private float shakeSpeed = 30.0f;
        private float shakeAmountX = 40.0f;
        private Vector3 _initCameraPosition;
        private WaitForSeconds wShakeFinishDelay = new WaitForSeconds(0.2f);

        private void InitCamera()
        {
            mainCamera = Camera.main;
            _initCameraPosition = mainCamera.transform.position;
            isShaking = false;
        }

        public void ShakeCamera(UnityAction callback)
        {
            Debug.Log(CodeManager.GetMethodName());

            StartCoroutine(CO_Shake(callback));
        }

        private IEnumerator CO_Shake(UnityAction callback)
        {
            isShaking = true;

            Vector3 leftPosition1 = new Vector3(shakeAmountX, _initCameraPosition.y, _initCameraPosition.z);
            Vector3 rightPosition1 = new Vector3(-shakeAmountX, _initCameraPosition.y, _initCameraPosition.z);
            Vector3 leftPosition2 = new Vector3(shakeAmountX * 0.6f, _initCameraPosition.y, _initCameraPosition.z);
            Vector3 rightPosition2 = new Vector3(-shakeAmountX * 0.6f, _initCameraPosition.y, _initCameraPosition.z);
            
            float leftTime1 = shakeTime * 0.2f;
            float rightTime1 = shakeTime * 0.4f;
            float leftTime2 = shakeTime * 0.6f;
            float rightTime2 = shakeTime * 0.8f;

            float elaspsedTime = 0;
            while (elaspsedTime < shakeTime)
            {
                if (elaspsedTime < leftTime1)
                    mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, leftPosition1, Time.deltaTime * shakeSpeed);
                else if (elaspsedTime < rightTime1)
                    mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, rightPosition1, Time.deltaTime * shakeSpeed);
                else if (elaspsedTime < leftTime2)
                    mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, leftPosition2, Time.deltaTime * shakeSpeed);
                else if (elaspsedTime < rightTime2)
                    mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, rightPosition2, Time.deltaTime * shakeSpeed);
                else
                    mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, _initCameraPosition, Time.deltaTime * shakeSpeed);

                yield return null;

                elaspsedTime += Time.deltaTime;
            }

            mainCamera.transform.position = _initCameraPosition;

            yield return wShakeFinishDelay;

            ShakeFinished(callback);
        }

        private void ShakeFinished(UnityAction callback)
        {
            isShaking = false;
            callback.Invoke();
        }
    }
}
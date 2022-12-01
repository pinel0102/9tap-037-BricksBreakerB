using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTimeScale : MonoBehaviour
{
    public bool setOnEnable = true;
    [Range(0, 1f)]
    public float timeScaleOnEnable = 1f;
    public bool setOnDisable = false;
    [Range(0, 1f)]
    public float timeScaleOnDisable = 1f;

    private void OnEnable()
    {
        if (setOnEnable)
        {            
            Time.timeScale = timeScaleOnEnable;
            //Debug.Log(CodeManager.GetMethodName() + Time.timeScale);
        }
    }

    private void OnDisable()
    {
        if (setOnDisable)
        {
            Time.timeScale = timeScaleOnDisable;
            Debug.Log(CodeManager.GetMethodName() + Time.timeScale);
        }        
    }
}

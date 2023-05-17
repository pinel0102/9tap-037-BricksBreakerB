using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyzeChange : MonoBehaviour
{
    public RectTransform rt;
    private Vector2 oldSize = Vector2.zero;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (!rt)
        {
            if(TryGetComponent(typeof(RectTransform), out var _rt))
            {
                rt = _rt as RectTransform;
            }

            CheckSize();
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        CheckSize();
    }

    private void CheckSize()
    {
        if (oldSize != rt.sizeDelta)
        {
            Debug.Log(CodeManager.GetMethodName() + string.Format("{0}", rt.sizeDelta));
            oldSize = rt.sizeDelta;
        }
    }
}

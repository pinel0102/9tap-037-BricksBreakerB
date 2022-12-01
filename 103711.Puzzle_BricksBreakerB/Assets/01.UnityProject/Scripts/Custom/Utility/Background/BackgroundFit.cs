using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class BackgroundFit : MonoBehaviour
{
    [Header("★ [Parameter] Live")]
    public float expectedWidth = 720f;
    public float expectedHeight = 1280f;
    public float expectedRatio = 1.777778f;
    public float currentRatio = 1.777778f;
    public float currentScale = 1;
    public Vector3 currentScaleVector;
    public Vector3 tabletScaleVector;
    public bool isTablet;
    RectTransform rt;

    private void Awake()
    {
        rt = (RectTransform)transform;
        Initialize(rt.sizeDelta.x, rt.sizeDelta.y);
    }

    private void Initialize(float _expectedWidth, float _expectedHeight)
    {
        expectedWidth = _expectedWidth;
        expectedHeight = _expectedHeight;
        expectedRatio = expectedHeight/expectedWidth;

        GetScreenSize();
    }

    private void GetScreenSize()
    {
        float i_width = Screen.width;
        float i_height = Screen.height;

        currentRatio = i_height/i_width;

        if (Mathf.Round(currentRatio * 10000f)/10000f >= expectedRatio)
        {
            Debug.Log(CodeManager.GetMethodName() + string.Format("Screen Ratio : {0} (Phone)", currentRatio));

            currentScale = currentRatio / expectedRatio;
            currentScaleVector = new Vector3(currentScale, currentScale, 1);
            tabletScaleVector = Vector3.one;
            isTablet = false;

            SetScale(currentScaleVector);
        }
        else
        {
            Debug.Log(CodeManager.GetMethodName() + string.Format("Screen Ratio : {0} (Tablet)", currentRatio));

            currentScale = 1f;
            currentScaleVector = Vector3.one;
            tabletScaleVector = new Vector3(currentScale, currentScale, 1);
            isTablet = true;

            SetScale(tabletScaleVector);
        }
    }

    private void SetScale(Vector3 scaleVector)
    {
        transform.localScale = scaleVector;
    }
}

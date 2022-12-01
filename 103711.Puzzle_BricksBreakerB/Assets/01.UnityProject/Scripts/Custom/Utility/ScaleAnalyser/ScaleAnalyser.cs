using UnityEngine;

public class ScaleAnalyser : SingletonMono<ScaleAnalyser>
{
    [Header("★ [Settings] Expected Resolution")]
    public float expectedWidth = 720f;
    public float expectedHeight = 1280f;
    
    [Header("★ [Parameter] Live")]
    public float expectedRatio = 1.777778f;
    public float currentRatio = 1.777778f;
    public float currentScale = 1;
    public Vector3 currentScaleVector;
    public Vector3 tabletScaleVector;
    public bool isTablet;

    private void Awake()
    {
        Initialize(expectedWidth, expectedHeight);
    }

    public void Initialize(float _expectedWidth, float _expectedHeight)
    {
        expectedWidth = _expectedWidth;
        expectedHeight = _expectedHeight;
        expectedRatio = expectedHeight/expectedWidth;

        GetCurrentResolution();
    }

    public void GetCurrentResolution(bool standaloneMode = false, bool landscapeMode = false, float manualRatio = 1.777778f)
    {
        float i_width = Screen.width;
        float i_height = Screen.height;

        currentRatio = i_height/i_width;

        if (standaloneMode)
        {
            if (landscapeMode)
            {
                currentScale = 1;
                currentScaleVector = Vector3.one;
                tabletScaleVector = Vector3.one;
                isTablet = false;
            }
            else
            {
                currentScale = expectedRatio / manualRatio;
                currentScaleVector = new Vector3(currentScale, currentScale, 1);
                tabletScaleVector = Vector3.one;
                isTablet = false;
            }
        }
        else
        {
            if (Mathf.Round(currentRatio * 10000f)/10000f >= expectedRatio)
            {
                Debug.Log(CodeManager.GetMethodName() + string.Format("Screen Ratio : {0} (Phone)", currentRatio));

                currentScale = expectedRatio / currentRatio;
                currentScaleVector = new Vector3(currentScale, currentScale, 1);
                tabletScaleVector = Vector3.one;
                isTablet = false;
            }
            else
            {
                Debug.Log(CodeManager.GetMethodName() + string.Format("Screen Ratio : {0} (Tablet)", currentRatio));

                currentScale = expectedRatio / currentRatio;
                currentScaleVector = Vector3.one;
                tabletScaleVector = new Vector3(currentScale, 1, 1);
                isTablet = true;
            }
        }
    }
}

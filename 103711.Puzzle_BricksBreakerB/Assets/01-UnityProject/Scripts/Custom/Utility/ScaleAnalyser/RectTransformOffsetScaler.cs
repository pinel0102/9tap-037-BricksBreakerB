using UnityEngine;

public class RectTransformOffsetScaler : MonoBehaviour
{
    public bool useScaler = true;
    public bool useTabletWidth = false;

    private RectTransform _rtransform;    
    private Vector3 currentScale;
    private Vector2 defaultOffsetMin = Vector2.zero;
    private Vector2 defaultOffsetMax = Vector2.zero;
    
    private ScaleAnalyser scaleAnalyser { get { return ScaleAnalyser.Instance; } }
    
    private void Awake()
    {
        Initialize();
        CheckPadding();
    }

    private void Update()
    {
        CheckPadding();
    }

    private void Initialize()
    {
        if (!_rtransform)
        {
            _rtransform = (RectTransform)transform;
            defaultOffsetMin = _rtransform.offsetMin;
            defaultOffsetMax = _rtransform.offsetMax;        
        }        
    }

    private void CheckPadding()
    {
        if (useScaler)
        {
            if (useTabletWidth && scaleAnalyser.isTablet)
            {
                if (currentScale != scaleAnalyser.tabletScaleVector)
                {
                    SetRectPadding(scaleAnalyser.tabletScaleVector);
                }
            }
            else
            {
                if (currentScale != scaleAnalyser.currentScaleVector)
                {
                    SetRectPadding(scaleAnalyser.currentScaleVector);
                }
            }
        }
    }

    private void SetRectPadding(Vector3 scaleVector)
    {
        currentScale = scaleVector;
        _rtransform.offsetMin = defaultOffsetMin * currentScale;
        _rtransform.offsetMax = defaultOffsetMax * currentScale;
    }
}

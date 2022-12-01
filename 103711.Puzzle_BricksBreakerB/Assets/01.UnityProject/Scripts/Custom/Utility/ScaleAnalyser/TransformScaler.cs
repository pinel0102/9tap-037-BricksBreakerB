using UnityEngine;

public class TransformScaler : MonoBehaviour
{
    public bool useScaler = true;
    public bool useTabletWidth = false;
    public Transform[] scalerOthers;

    private ScaleAnalyser scaleAnalyser { get { return ScaleAnalyser.Instance; } }

    private void Awake()
    {
        CheckScale();
    }

    private void Update()
    {
        CheckScale();
    }

    private void CheckScale()
    {
        if (useScaler)
        {
            if (useTabletWidth && scaleAnalyser.isTablet)
            {
                if (transform.localScale != scaleAnalyser.tabletScaleVector)
                {
                    SetScale(scaleAnalyser.tabletScaleVector);
                }
            }
            else
            {
                if (transform.localScale != scaleAnalyser.currentScaleVector)
                {
                    SetScale(scaleAnalyser.currentScaleVector);
                }
            }
        }
    }

    private void SetScale(Vector3 scaleVector)
    {
        transform.localScale = scaleVector;
        
        for (int i=0; i < scalerOthers.Length; i++)
        {
            if (scalerOthers[i])
                scalerOthers[i].localScale = scaleVector;
        }
    }
}

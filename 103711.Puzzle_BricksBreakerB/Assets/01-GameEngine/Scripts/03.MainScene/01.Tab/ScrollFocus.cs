using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollFocus : MonoBehaviour
{
    [Header("★ [Reference] ScrollRect")]
    public ScrollRect scrollRect;
    public RectTransform objectToFocus;

    private void OnEnable()
    {
        SetFocus();
    }

    public void SetFocus()
    {
        if (!objectToFocus) return;

        scrollRect.FocusOnItem(objectToFocus);
    }

}

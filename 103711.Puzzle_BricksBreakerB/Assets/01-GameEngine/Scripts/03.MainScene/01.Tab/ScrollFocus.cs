using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollFocus : MonoBehaviour
{
    [Header("★ [Reference] ScrollRect")]
    public ScrollRect scrollRect;
    public RectTransform objectToFocus;

    [Header("★ [Reference] Extra")]
    public TabItem tabItem;

    private void Awake()
    {
        tabItem.callbackOnActive += SetFocus;
    }

    private void OnDestroy()
    {
        tabItem.callbackOnActive -= SetFocus;
    }

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

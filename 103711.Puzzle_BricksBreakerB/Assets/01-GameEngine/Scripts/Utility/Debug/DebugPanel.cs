using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DebugPanel : MonoBehaviour
{
    [Header("★ [Settings] Debug Panel")]
    public bool hideOnAwake = true;
    public Vector2 showPosition = new Vector2(0, 100);
    public Vector2 hidePosition = new Vector2(-720, 100);

    [Header("★ [Settings] Hide items in build")]
    public GameObject[] hideItemsInBuild;

    [Header("★ [Reference] Transform")]
    private RectTransform debugPanel;
    private bool isShown;

    public event Action callback_backButton;

    private void Awake()
    {
        debugPanel = GetComponent<RectTransform>();

#if !UNITY_EDITOR
        for (int i=0; i < hideItemsInBuild.Length; i++)
        {
            hideItemsInBuild[i].SetActive(false);
        }
#endif

        if (hideOnAwake)
            Panel_Hide();

        isShown = gameObject.activeInHierarchy;
    }

    public void Panel_Show()
    {
        isShown = true;
        debugPanel.anchoredPosition = showPosition;
        gameObject.SetActive(true);
    }

    public void Panel_Hide()
    {
        isShown = false;
        debugPanel.anchoredPosition = hidePosition;
        gameObject.SetActive(false);
    }

    public void Panel_Toggle()
    {
        if (isShown)
            Panel_Hide();
        else
            Panel_Show();
    }

    public void OnClick_BackButton()
    {
        callback_backButton?.Invoke();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabItem : MonoBehaviour
{
    public Vector2 showPosition = new Vector2(0, 0);
    public Vector2 hidePosition = new Vector2(0, 0);

    private bool isMoving;
    private float moveSpeed = 15f;
    private float moveTime = 0.3f;
    private RectTransform rt => transform as RectTransform;
    
    public void ShowItem()
    {
        if (!gameObject.activeInHierarchy && !isMoving)
        {
            gameObject.SetActive(true);
            StartCoroutine(CO_ShowItem());
        }
    }

    public void HideItem()
    {
        if (gameObject.activeInHierarchy && !isMoving)
        {
            StartCoroutine(CO_HideItem());
        }
    }

    public void HideItemInit()
    {
        rt.anchoredPosition = hidePosition;
        isMoving = false;

        gameObject.SetActive(false);
    }

    private IEnumerator CO_ShowItem()
    {
        isMoving = true;
        
        float elaspsedTime = 0;
        while (elaspsedTime < moveTime)
        {
            rt.anchoredPosition = Vector3.Lerp(rt.anchoredPosition, showPosition, Time.deltaTime * moveSpeed);
            yield return null;

            elaspsedTime += Time.deltaTime;
        }

        rt.anchoredPosition = showPosition;
        isMoving = false;
    }

    private IEnumerator CO_HideItem()
    {
        isMoving = true;

        float elaspsedTime = 0;
        while (elaspsedTime < moveTime)
        {
            rt.anchoredPosition = Vector3.Lerp(rt.anchoredPosition, hidePosition, Time.deltaTime * moveSpeed);
            yield return null;

            elaspsedTime += Time.deltaTime;
        }

        rt.anchoredPosition = hidePosition;
        isMoving = false;

        gameObject.SetActive(false);
    }
}

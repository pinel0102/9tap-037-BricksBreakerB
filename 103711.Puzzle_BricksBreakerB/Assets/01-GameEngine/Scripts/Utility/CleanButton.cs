// Copyright (C) 2015-2021 ricimi - All rights reserved.
// This code can only be used under the standard Unity Asset Store End User License Agreement.
// A Copy of the Asset Store EULA is available at http://unity3d.com/company/legal/as_terms.

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Fundamental button class used throughout the demo.
/// </summary>
public class CleanButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    public float fadeTime = 0.1f;
    [Range(0,1f)]
    public float onHoverAlpha = 0.95f;
    [Range(0,1f)]
    public float onClickAlpha = 0.9f;
    public float onClickScale = 0.95f;
    [Tooltip("If false, changes child scale only.")]
    public bool scaleSelf = false;
    private bool onPress = false;
    private bool onFocus = false;
    private Vector2 _iniScale = Vector2.one;
    private List<Vector2> _iniScaleChild = new List<Vector2>();
    private List<Vector2> _clickScaleChild = new List<Vector2>();
    private Vector2 _clickScale = Vector2.one;

    [Serializable]
    public class ButtonClickedEvent : UnityEvent { }

    [SerializeField]
    public ButtonClickedEvent onClicked = new ButtonClickedEvent();
    /*[SerializeField]
    public ButtonClickedEvent onFocusIn = new ButtonClickedEvent();
    [SerializeField]
    public ButtonClickedEvent onFocusOut = new ButtonClickedEvent();
    [SerializeField]
    public ButtonClickedEvent onPressed = new ButtonClickedEvent();
    [SerializeField]
    public ButtonClickedEvent onReleased = new ButtonClickedEvent();*/

    private CanvasGroup canvasGroup;
    private List<Transform> _childTransform = new List<Transform>();
    
    private void Awake()
    {
        if (!gameObject.TryGetComponent<CanvasGroup>(out canvasGroup))
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        
        _childTransform.Clear();
        _iniScaleChild.Clear();
        _clickScaleChild.Clear();

        for (int i=0; i < transform.childCount; i++)
        {
            _childTransform.Add(transform.GetChild(i));
            _iniScaleChild.Add(transform.GetChild(i).localScale);
            _clickScaleChild.Add(new Vector2(_iniScaleChild[i].x * onClickScale, _iniScaleChild[i].y * onClickScale));
        }

        _iniScale = transform.localScale;
        _clickScale = new Vector2(_iniScale.x * onClickScale, _iniScale.y * onClickScale);
        onPress = false;
        onFocus = false;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        canvasGroup.alpha = 1.0f;
        if (scaleSelf)
        {
            transform.localScale = _iniScale;
        }
        else
        {
            for (int i=0; i < _childTransform.Count; i++)
            {
                _childTransform[i].localScale = _iniScaleChild[i];
            }
        }
        transform.localScale = _iniScale;
        onPress = false;
        onFocus = false;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        
        //Debug.Log(CodeManager.GetMethodName() + gameObject.name);

        StopAllCoroutines();

        if (onPress)
        {
            canvasGroup.alpha = onClickAlpha;
            
            if (scaleSelf)
                SetScaleSelf(_clickScale, fadeTime);
            else
                SetScaleChild(_clickScaleChild, fadeTime);
        }
        else
        {
#if UNITY_EDITOR || UNITY_STANDALONE
            SetFade(canvasGroup, onHoverAlpha, fadeTime);
#endif
        }
        
        onFocus = true;
        //onFocusIn.Invoke();
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        
        //Debug.Log(CodeManager.GetMethodName() + gameObject.name);

        StopAllCoroutines();
        SetFade(canvasGroup, 1.0f, fadeTime);

        if (scaleSelf)
            SetScaleSelf(_iniScale, fadeTime);
        else
            SetScaleChild(_iniScaleChild, fadeTime);
        
        onFocus = false;
        //onFocusOut.Invoke();
    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        
        //Debug.Log(CodeManager.GetMethodName() + gameObject.name);

        canvasGroup.alpha = onClickAlpha;

        if (scaleSelf)
            SetScaleSelf(_clickScale, fadeTime);
        else
            SetScaleChild(_clickScaleChild, fadeTime);

        onPress = true;
        onFocus = true;
        //onPressed.Invoke();
    }

    public virtual void OnPointerUp(PointerEventData eventData)
    {   
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        
        StopAllCoroutines();
        canvasGroup.alpha = 1.0f;

        if (scaleSelf)
            SetScaleSelf(_iniScale, fadeTime);
        else
            SetScaleChild(_iniScaleChild, fadeTime);

        if (onPress && onFocus)
        {
            if (eventData.dragging)
            {
                //Debug.Log(CodeManager.GetMethodName() + gameObject.name + " (Drag)");
            }
            else
            {
                //Debug.Log(CodeManager.GetMethodName() + gameObject.name);
                onClicked.Invoke();
            }                
        }
        
        onPress = false;
        onFocus = false;
        //onReleased.Invoke();
    }


    private void SetScaleSelf(Vector2 _newScale, float duration)
    {
        StartCoroutine(Co_SetScaleSelf(_newScale, duration));
    }

    private void SetScaleChild(List<Vector2> _newScale, float duration)
    {
        StartCoroutine(Co_SetScaleChild(_newScale, duration));
    }

    private IEnumerator Co_SetScaleSelf(Vector2 _newScale, float duration)
    {
        var time = 0.0f;
        float durationInverse = 1f / duration;
        Vector2 originalScale = transform.localScale;


        while (time < duration)
        {
            time += Time.unscaledDeltaTime;

            float _currentScaleX = Mathf.Lerp(originalScale.x, _newScale.x, time * durationInverse);
            float _currentScaleY = Mathf.Lerp(originalScale.y, _newScale.y, time * durationInverse);

            transform.localScale = new Vector2(_currentScaleX, _currentScaleY);
            
            yield return new WaitForEndOfFrame();
        }

        transform.localScale = _newScale;
    }

    private IEnumerator Co_SetScaleChild(List<Vector2> _newScale, float duration)
    {
        var time = 0.0f;
        float durationInverse = 1f / duration;
        List<Vector2> originalScale = new List<Vector2>();

        for (int i=0; i < _childTransform.Count; i++)
        {
            originalScale.Add(_childTransform[i].localScale);
        }

        while (time < duration)
        {
            time += Time.unscaledDeltaTime;

            for (int i=0; i < _childTransform.Count; i++)
            {
                float _currentScaleX = Mathf.Lerp(originalScale[i].x, _newScale[i].x, time * durationInverse);
                float _currentScaleY = Mathf.Lerp(originalScale[i].y, _newScale[i].y, time * durationInverse);
                
                _childTransform[i].localScale = new Vector2(_currentScaleX, _currentScaleY);
            }
            
            yield return new WaitForEndOfFrame();
        }
        
        for (int i=0; i < _childTransform.Count; i++)
        {
            _childTransform[i].localScale = _newScale[i];
        }
    }

    private void SetFade(CanvasGroup group, float alpha, float duration)
    {
        StartCoroutine(Co_SetFade(group, alpha, duration));
    }

    private IEnumerator Co_SetFade(CanvasGroup group, float alpha, float duration)
    {
        var time = 0.0f;
        var originalAlpha = group.alpha;
        while (time < duration)
        {
            time += Time.unscaledDeltaTime;
            group.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);

            yield return new WaitForEndOfFrame();
        }

        group.alpha = alpha;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExtraTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltipObject;
    public Vector3 Offset;
    public Text tooltipText;
    public string textRaw;    

    private void Awake()
    {
        if (tooltipObject != null)
            tooltipObject.SetActive(false);
    }

    public void Initialize(GameObject _tooltipObject, Text _tooltipText, string _textRaw)
    {
        if (_tooltipObject != null)
            tooltipObject = _tooltipObject;
        if (_tooltipText != null)
            tooltipText = _tooltipText;
            
        textRaw = _textRaw;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (tooltipObject != null)
        {
            tooltipObject.transform.position = transform.position + Offset;
            tooltipText.text = textRaw;
            tooltipObject.SetActive(true);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (tooltipObject != null)
        {
            tooltipObject.SetActive(false);
        }
    }
}

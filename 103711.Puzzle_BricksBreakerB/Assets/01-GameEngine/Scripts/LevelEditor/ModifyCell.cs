using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ModifyCell : MonoBehaviour
{
    [Header("★ [Reference] Modify Cell")]
    public GameObject modifyWindow;
    public Image cellImage;
    public Text cellHPText;
    public Dropdown colorDropdown;
    public InputField HPInputField;    

    [Header("★ [Live] Parameter")]
    public EObjKinds currentKinds;
    public bool isEnableColor;
    public bool isEnableHit;
    public int currentColorID;
    public int currentHP;

    public event Action onModifyComplete;    
    private STCellObjInfo currentCellInfo;    

    private void Awake()
    {
        Initialize();
    }

    private void Initialize()
    {
        currentKinds = EObjKinds.NONE;
        isEnableColor = false;
        isEnableHit = false;
        currentColorID = 0;
        currentHP = 0;
        
        modifyWindow.SetActive(false);
    }

    public void OpenModifyWindow(STCellObjInfo cellInfo)
    {
        currentCellInfo = cellInfo;
        currentKinds = currentCellInfo.ObjKinds;
        currentHP = currentCellInfo.HP;
        currentColorID = currentCellInfo.ColorID;

        if(CObjInfoTable.Inst.TryGetObjInfo(currentKinds, out STObjInfo stObjInfo))
        {
            isEnableHit = stObjInfo.m_bIsEnableHit;
            isEnableColor = stObjInfo.m_bIsEnableColor;
        }

        colorDropdown.value = isEnableColor ? currentColorID : 0;
        HPInputField.text = isEnableHit ? currentHP.ToString() : 0.ToString();

        RefreshCellImage();

        modifyWindow.SetActive(true);
    }

    private void RefreshCellImage()
    {
        cellImage.sprite = Access.GetEditorObjSprite(currentKinds, KCDefine.B_PREFIX_LEVEL_EDITOR_SCENE);
        cellImage.color = GlobalDefine.GetCellColor(currentKinds, isEnableColor, currentColorID, currentHP);
        cellHPText.text = currentHP.ToString();
        cellHPText.gameObject.SetActive(isEnableHit);
    }

    private void ApplyCellInfo()
    {
        if (isEnableColor)
            currentCellInfo.ColorID = currentColorID;

        if (isEnableHit)
            currentCellInfo.HP = currentHP;

        RefreshCellImage();

        onModifyComplete.Invoke();
    }


#region Buttons

    public void OnClick_Close()
    {
        Initialize();
    }
    public void OnClick_Apply()
    {
        ApplyCellInfo();
    }

    public void OnClick_Revert()
    {
        OpenModifyWindow(currentCellInfo);
    }

    public void OnValueChanged_Color(int valueInt)
    {
        currentColorID = valueInt;
    }

    public void OnValueChanged_HP(string value)
    {
        if(int.TryParse(value, out int valueInt))
        {
            currentHP = valueInt;
        }
    }

#endregion Buttons

}

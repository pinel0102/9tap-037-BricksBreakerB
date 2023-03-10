using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXObj : MonoBehaviour
{
    public FXType fxType;
    
    private NSEngine.CEObj ceObj;
    private string _colorHex = GlobalDefine.COLOR_CELL_DEFAULT;

    public void Initialize()
    {
        if (ceObj == null)
            ceObj = GetComponent<NSEngine.CEObj>();
        
        SetSpriteColor();
    }

    private void SetSpriteColor()
    {
        switch(fxType)
        {
            case FXType.LASER:
                _colorHex = GlobalDefine.COLOR_FX_LASER;
                break;
            default:
                break;
        }

        ceObj.TargetSprite.color = ColorUtility.TryParseHtmlString(_colorHex, out Color _color) ? _color : Color.white;
    }
}


using UnityEngine;
using UnityEngine.UI;
using System.Globalization;
using System.Linq;

public class ColorPreview : MonoBehaviour
{
    public ColorPicker colorPicker;
    public Graphic oldGraphic;
    public Graphic previewGraphic;

    [Header("★ [Reference] Input Field")]
    public InputField input_R;
    public InputField input_G;
    public InputField input_B;
    public InputField input_Hex;

    private bool isInitialized;
    private const string FORMAT_HEX = "#{0}";
    private const string hexCheck = "0123456789ABCDEF";
    
    private void Awake()
    {
        isInitialized = false;
        input_Hex.onValidateInput += delegate (string input, int charIndex, char addedChar) { return SetToUpper(addedChar); };
    }

    private void Start()
    {
        Initialize(colorPicker.color);

        colorPicker.onColorChanged += OnColorChanged;
        colorPicker.onColorApply += ApplyColor;
        colorPicker.onColorRevert += RevertColor;

        isInitialized = true;
    }

    private void OnEnable()
    {
        if (isInitialized)
            Initialize(colorPicker.color);
    }

    private void Initialize(Color _color)
    {
        previewGraphic.color = _color;
        oldGraphic.color = _color;
    }

    private void OnDestroy()
    {
        if (colorPicker != null)
            colorPicker.onColorChanged -= OnColorChanged;
    }


#region On Value Changed

    public void OnColorChanged(Color c)
    {
        previewGraphic.color = c;

        if(!input_R.isFocused)
            input_R.text = (c.r * 255f).ToString("0");
        if(!input_G.isFocused)
            input_G.text = (c.g * 255f).ToString("0");
        if(!input_B.isFocused)
            input_B.text = (c.b * 255f).ToString("0");
        if(!input_Hex.isFocused)
            input_Hex.text = ColorUtility.ToHtmlStringRGBA(c);
    }

    public void OnChanged_R(string _value)
    {
        if (input_R.isFocused)
        {
            if (!IsInt(_value)) return;
            
            int _valueRaw = int.Parse(_value);
            int _valueResult = LimitRGBRange(_valueRaw);
            if (_valueRaw != _valueResult)
            {
                input_R.text = _valueResult.ToString();
                return;
            }

            colorPicker.color = new Color(GetRGB_0to1(_valueResult), colorPicker.color.g, colorPicker.color.b, colorPicker.color.a);        
            //ApplyColor();
        }
    }

    public void OnChanged_G(string _value)
    {
        if (input_G.isFocused)
        {
            if (!IsInt(_value)) return;

            int _valueRaw = int.Parse(_value);
            int _valueResult = LimitRGBRange(_valueRaw);
            if (_valueRaw != _valueResult)
            {
                input_G.text = _valueResult.ToString();
                return;
            }

            colorPicker.color = new Color(colorPicker.color.r, GetRGB_0to1(_valueResult), colorPicker.color.b, colorPicker.color.a);
            //ApplyColor();
        }
    }

    public void OnChanged_B(string _value)
    {
        if (input_B.isFocused)
        {
            if (!IsInt(_value)) return;

            int _valueRaw = int.Parse(_value);
            int _valueResult = LimitRGBRange(_valueRaw);
            if (_valueRaw != _valueResult)
            {
                input_B.text = _valueResult.ToString();
                return;
            }

            colorPicker.color = new Color(colorPicker.color.r, colorPicker.color.g, GetRGB_0to1(_valueResult), colorPicker.color.a);
            //ApplyColor();
        }
    }

    public void OnChanged_Hex(string _value)
    {
        if (input_Hex.isFocused)
        {
            if (!IsValid_Hex(_value)) return;

            if (ColorUtility.TryParseHtmlString(string.Format(FORMAT_HEX, _value), out Color _color))
            {
                colorPicker.color = _color;
                //ApplyColor();
            }
        }
    }

#endregion On Value Changed


    private void ApplyColor()
    {
        oldGraphic.color = previewGraphic.color;
    }

    private void RevertColor()
    {
        colorPicker.color = oldGraphic.color;
        Initialize(oldGraphic.color);
    }

    private bool IsInt(string _value)
    {
        return int.TryParse(_value, out int _valueInt);
    }

    private int LimitRGBRange(int _valueInt)
    {
        return (_valueInt < 0) ? 0 : ((_valueInt > 255) ? 255 : _valueInt);
    }

    private bool IsValid_Hex(string _value)
    {
        return _value.ToUpper().All(hexCheck.Contains);
    }

    private float GetRGB_0to1(int _rgb255)
    {
        return ((float)_rgb255)/255f;
    }

    private char SetToUpper(char c)
    {
        string str = c.ToString().ToUpper();
        char[] chars = str.ToCharArray();
        return chars[0];
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundChanger : SingletonMono<BackgroundChanger>
{
    [Header("★ [Reference]")]
    public Image backgroundImage;

    private void Awake()
    {
        if (backgroundImage == null)
            backgroundImage = transform.GetComponent<Image>();
    }

    public void ChangeSprite(Sprite _sprite)
    {
        backgroundImage.sprite = _sprite;
    }

    public void ChangeSprite(string _resourcePath)
    {
        backgroundImage.sprite = Resources.Load<Sprite>(_resourcePath);
    }
}

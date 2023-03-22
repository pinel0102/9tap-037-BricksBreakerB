using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXObj : MonoBehaviour
{
    public List<ParticleSystem> _particleList = new List<ParticleSystem>();
    public List<SpriteRenderer> _spriteList = new List<SpriteRenderer>();
    
    public void SetColor(Color _startColor)
    {
        for (int i=0; i < _particleList.Count; i++)
        {
            var main = _particleList[i].main;
            main.startColor = _startColor;
        }

        for (int i=0; i < _spriteList.Count; i++)
        {
            _spriteList[i].color = _startColor;
        }
    }
}

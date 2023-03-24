using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FXSettings : MonoBehaviour
{
    public List<ParticleSystem> _particleList = new List<ParticleSystem>();
    public List<SpriteRenderer> _spriteList = new List<SpriteRenderer>();
    
    public void SetColor(Color startColor)
    {
        for (int i=0; i < _particleList.Count; i++)
        {
            var main = _particleList[i].main;
            main.startColor = startColor;
        }

        for (int i=0; i < _spriteList.Count; i++)
        {
            _spriteList[i].color = startColor;
        }
    }

    public void SetRotation(float angle)
    {
        for (int i=0; i < _particleList.Count; i++)
        {
            var main = _particleList[i].main;
            main.startRotation = angle * -Mathf.Deg2Rad;
        }
    }

    public void SetStartSizeY(float min, float max)
    {
        for (int i=0; i < _particleList.Count; i++)
        {
            var main = _particleList[i].main;
            main.startSizeY = new ParticleSystem.MinMaxCurve(min, max);
        }
    }

    public void SetMove(Transform target, float moveTime)
    {
        var oAni = transform.DOMove(target.position, moveTime);
		oAni.SetAutoKill().SetEase(Ease.OutQuad);
    }
}

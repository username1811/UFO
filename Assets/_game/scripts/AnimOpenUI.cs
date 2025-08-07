using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimOpenUI : MonoBehaviour
{
    public Tween scaleTween;
    public float duration = 0.3f;
    public float smallScale = 0.7f;

    private void OnEnable()
    {
        scaleTween?.Kill();
        this.transform.localScale = smallScale * Vector3.one;
        scaleTween = this.transform.DOScale(1f, duration).SetEase(Ease.OutBack);
    }
}

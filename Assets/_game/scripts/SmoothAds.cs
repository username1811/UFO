using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothAds : Singleton<SmoothAds>
{
    public CanvasGroup canvasGroup;
    public float duration;
    public float delay;


    private void Start()
    {
        Hide();
    }

    public void Show(Action OnComplete)
    {
        canvasGroup.alpha = 0f;
        canvasGroup.gameObject.SetActive(true);
        canvasGroup.DOFade(1f, duration).SetEase(Ease.OutSine).OnComplete(() =>
        {
            DOVirtual.DelayedCall(delay, () =>
            {
                OnComplete?.Invoke();
            });
        });
    }

    public void Hide()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.gameObject.SetActive(false);
    }
}

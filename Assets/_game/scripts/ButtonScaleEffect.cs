using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonScaleEffect : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector3 smallScale => Vector3.one * 0.94f;
    public Tween scaleTween;
    
    public void OnPointerDown(PointerEventData eventData)
    {
        scaleTween?.Kill();
        transform.localScale = Vector3.one;
        scaleTween = transform.DOScale(smallScale, 0.07f).SetEase(Ease.OutSine);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        scaleTween?.Kill();
        transform.localScale = smallScale;
        scaleTween = transform.DOScale(Vector3.one, 0.07f).SetEase(Ease.OutSine);
    }
}

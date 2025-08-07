using DG.Tweening;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TargetObject : MonoBehaviour
{
    public TargetObjectType TargetObjectType;
    public Image img;
    public TextMeshProUGUI amountText;
    public RectTransform rectTransform;
    public int amount;
    public Tween scaleTween;



    public void OnInitt(TargetObjectInfo targetObjectInfo)
    {
        this.TargetObjectType = targetObjectInfo.targetObjectType;
        img.sprite = ObjectSpriteManager.Ins.GetSprite(targetObjectInfo.targetObjectType);
        img.ResizeImgKeepHeight();
        amountText.text = targetObjectInfo.amount.ToString();
        amount = targetObjectInfo.amount;
    }

    public void Decrease()
    {
        Scale();
        amount -= 1;
        if(amount < 0) amount = 0;
        amountText.text = amount.ToString();
        if(amount == 0)
        {
            this.transform.DOScale(0.1f, 0.3f).SetDelay(0.1f).SetEase(Ease.InBack).OnComplete(() =>
            {
                PoolManager.Ins.Despawn(PoolType.TargetObject, this.gameObject);
            });
        }
    }

    public void Scale()
    { 
        scaleTween?.Kill();
        this.transform.localScale = Vector3.one * 1.1f;
        scaleTween = this.transform.DOScale(1f, 0.15f);
    }
}

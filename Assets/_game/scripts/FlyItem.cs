using DG.Tweening;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FlyItem : MonoBehaviour
{
    public TargetObjectType targetObjectType;
    public Image img;
    public RectTransform rectTransform;
    public float duration;



    public void OnInitt(TargetObjectType targetObjectType, Vector2 screenPos)
    {
        this.targetObjectType = targetObjectType;
        img.sprite = ObjectSpriteManager.Ins.GetSprite(targetObjectType);
        img.ResizeImgKeepHeight();
        this.transform.SetParent(UIManager.Ins.canvasFly.transform);
        duration = 0.4f;
        Scale();
        Fly(screenPos);
    }

    public void Scale()
    {
        this.transform.localScale = Vector3.one * 0.1f;
        this.transform.DOScale(1f, duration).SetEase(Ease.OutSine);
    }

    public void Fly(Vector2 screenPos)
    {
        Vector2 targetFly = UIManager.Ins.GetUI<GamePlay>().targetObjects.FirstOrDefault(x => x.TargetObjectType == this.targetObjectType).rectTransform.position;
        this.rectTransform.position = targetFly;
        Vector2 targetAnchorPOs = this.rectTransform.anchoredPosition;
        rectTransform.position = screenPos;
        this.rectTransform.DOAnchorPosX(targetAnchorPOs.x, duration).SetEase(Ease.Linear);
        this.rectTransform.DOAnchorPosY(targetAnchorPOs.y, duration).SetEase(Ease.InQuint).OnComplete(() =>
        {
            PoolManager.Ins.Despawn(PoolType.FlyItem, this.gameObject);
            UIManager.Ins.GetUI<GamePlay>().targetObjects.FirstOrDefault(x => x.TargetObjectType == this.targetObjectType)?.Decrease();
        });
    }
}

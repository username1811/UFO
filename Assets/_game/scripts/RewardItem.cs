using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardItem : MonoBehaviour
{
    public RewardInfo rewardInfo;
    public Image img;
    public TextMeshProUGUI amountText;
    public CanvasGroup canvasGroup;
    public float duration;
    public RectTransform rectTransform;
    public GameObject glowObj;



    public void OnInitt(RewardInfo rewardInfo, bool isFlyUp, bool isGlow)
    {
        this.rewardInfo = rewardInfo;
        this.img.sprite = RewardManager.Ins.GetSprite(rewardInfo.rewardType);
        this.img.ResizeImgKeepHeight();
        this.amountText.text = rewardInfo.amount.ToString();
        if (rewardInfo.rewardType == RewardType.BoosterHint || rewardInfo.rewardType == RewardType.BoosterTime) this.amountText.text = "x"+this.amountText.text;
        canvasGroup.alpha = 1f;
        glowObj.SetActive(isGlow);
        if (isFlyUp) AnimFlyUp();
        this.transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

    private void OnDisable()
    {
        this.transform.localScale = Vector3.one;
        transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }

    public void AnimFlyUp()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.DOFade(0f, duration * 0.25f).SetDelay(duration * 0.75f).SetEase(Ease.OutSine);
        rectTransform.DOAnchorPosY(rectTransform.anchoredPosition.y + 50f, duration).SetEase(Ease.OutSine);
        DOVirtual.DelayedCall(duration + 0.2f, () =>
        {
            PoolManager.Ins.Despawn(PoolType.RewardItem, this.gameObject);
        });
    }

    public void Fly(Vector2 dir, float distance, float delay)
    {
        this.rectTransform.DOAnchorPos(this.rectTransform.anchoredPosition + dir.normalized * distance, 0.3f).SetEase(Ease.OutSine).SetDelay(delay);
        this.transform.DOScale(2f, 0.3f).SetEase(Ease.OutSine).SetDelay(delay);
    }
}

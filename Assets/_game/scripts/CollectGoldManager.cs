using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectGoldManager : Singleton<CollectGoldManager>
{
    public List<RectTransform> coins = new();
    public List<Vector2> coinInitialPoses = new();
    public RectTransform targetFlyRectTF;
    public float spawnDelay;
    public float flyDuration;
    public float flyDelay;
    public float flyEachDelay;
    public float randomDistance;
    public AnimationCurve curve;



    private void Start()
    {
        foreach (var coin in coins)
        {
            coin.gameObject.SetActive(false);
            coinInitialPoses.Add(coin.anchoredPosition);
        }
    }

    [Button]
    public void Test()
    {
        OnInitt(100, UIManager.Ins.GetUI<Home>().gold.icon.rectTransform.position, () =>
        {
            GoldManager.Ins.AnimGold(DataManager.Ins.playerData.gold, DataManager.Ins.playerData.gold + 100, true);
        });
    }

    public void OnInitt(int goldAmount, Vector2 targetFly, Action OnFirstCoinDoneFly)
    {
        StartCoroutine(IEOnInitt(goldAmount, targetFly));
        IEnumerator IEOnInitt(int goldAmount, Vector2 targetFly)
        {
            SoundManager.Ins.PlaySFX(SFXType.ClaimReward);
            DOVirtual.DelayedCall(flyDelay + flyDuration, () =>
            {
                OnFirstCoinDoneFly?.Invoke();
            });
            targetFlyRectTF.position = targetFly;   
            foreach (var coin in coins)
            {
                coin.gameObject.SetActive(true);
                coin.transform.SetParent(UIManager.Ins.canvasFly.transform);
                int index = coins.IndexOf(coin);
                coin.anchoredPosition = coinInitialPoses[index];
                FlyCoin(coin,targetFlyRectTF.anchoredPosition, flyDelay + flyEachDelay * index);
                yield return new WaitForSeconds(spawnDelay);
            }
            yield return null;
        }

        void FlyCoin(RectTransform coin, Vector2 targetAnchorPos, float delay)
        {
            /*coin.DOAnchorPos(targetFlyRectTF.anchoredPosition, flyDuration).SetDelay(flyDelay).SetEase(Ease.InCubic).OnComplete(() =>
            {
                UIManager.Ins.GetUI<Home>().AnimGoldCollect();
                coin.gameObject.SetActive(false);
                SoundManager.Ins.PlaySFX(SFXType.Coin);
            });*/
            coin.DOAnchorPosX(targetFlyRectTF.anchoredPosition.x, flyDuration).SetDelay(delay).SetEase(Ease.Linear).OnComplete(() =>
            {
                UIManager.Ins.GetUI<Home>().AnimGoldCollect();
                coin.gameObject.SetActive(false);
                SoundManager.Ins.PlaySFX(SFXType.Coin);
            });
            coin.DOAnchorPosY(targetFlyRectTF.anchoredPosition.y, flyDuration).SetDelay(delay).SetEase(curve).OnComplete(() =>
            {
            });

        }
    }
}

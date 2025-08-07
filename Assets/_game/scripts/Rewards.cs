using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Rewards : UICanvas
{
    public GridLayoutGroup grid;
    public List<RewardInfo> rewardInfos = new List<RewardInfo>();
    public List<RewardItem> rewardItems = new List<RewardItem>();
    public GameObject buttonClaimObj;
    public static Action OnClaim = () => { };
    public CanvasGroup blackCanvasGroup;


    public override void Open()
    {
        base.Open();
        buttonClaimObj.SetActive(false);
        blackCanvasGroup.alpha = 0f;
        blackCanvasGroup.DOFade(1f, 0.3f).SetEase(Ease.OutSine);
    }

    public void OnInitt(List<RewardInfo> rewardInfos)
    {
        this.rewardInfos = rewardInfos;
        rewardItems.Clear();
        StartCoroutine(IESpawnItems(rewardInfos));
    }

    IEnumerator IESpawnItems(List<RewardInfo> rewardInfos)
    {

        OptimizeRewardInfos(rewardInfos);
        foreach (var rewardInfo in rewardInfos)
        {
            RewardItem rewardItem = PoolManager.Ins.Spawn<RewardItem>(PoolType.RewardItem);
            rewardItem.transform.SetParent(grid.transform);
            rewardItem.OnInitt(rewardInfo, false, false);
            rewardItem.transform.localScale = Vector3.one * 0.01f;
            rewardItem.transform.DOScale(1.7f, 0.5f).SetEase(Ease.OutBack);
            rewardItems.Add(rewardItem);
            yield return new WaitForSeconds(0.3f);
        }
        buttonClaimObj.SetActive(true);
        yield return null;
    }

    public void OptimizeRewardInfos(List<RewardInfo> rewardInfos)
    {
        List<RewardInfo> newList = new();
        for (int i = 0; i < rewardInfos.Count; i++)
        {
            RewardInfo rewardInfo = rewardInfos[i];
            if (newList.Any(x => x.rewardType == rewardInfo.rewardType)) continue;
            for (int j = i + 1; j < rewardInfos.Count; j++)
            {
                RewardInfo rewardInfo1 = rewardInfos[j];
                if (rewardInfo.rewardType == rewardInfo1.rewardType)
                {
                    rewardInfo.amount += rewardInfo1.amount;
                }
            }
            newList.Add(rewardInfo);
        }

        // Clear and add all items instead of reassigning
        rewardInfos.Clear();
        rewardInfos.AddRange(newList);
    }

    public override void CloseDirectly()
    {
        foreach(var rewardItem in rewardItems)
        {
            PoolManager.Ins.Despawn(PoolType.RewardItem, rewardItem.gameObject);
        }
        base.CloseDirectly();
    }

    public void ButtonClaim()
    {
        UIManager.Ins.CloseUI<Rewards>();    
        foreach(var rewardInfo in rewardInfos)
        {
            rewardInfo.OnClaim();
        }
        OnClaim?.Invoke();
    }
}

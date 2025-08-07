using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RewardManager : Singleton<RewardManager>
{
    public List<RewardSprite> rewardSprites = new List<RewardSprite>();

    public Sprite GetSprite(RewardType rewardType)
    {
        return rewardSprites.FirstOrDefault(x => x.rewardType == rewardType).sprite;
    }

    public RewardItem SpawnRewardItem(RewardInfo rewardInfo, Vector2 screenPos)
    {
        RewardItem rewardItem = PoolManager.Ins.Spawn<RewardItem>(PoolType.RewardItem);
        rewardItem.transform.SetParent(UIManager.Ins.canvasFly.transform);
        rewardItem.transform.localScale = Vector3.one * 0.8f;
        rewardItem.rectTransform.position = screenPos + Vector2.up * 40f;
        rewardItem.OnInitt(rewardInfo, true, true);
        SoundManager.Ins.PlaySFX(SFXType.ClaimReward);
        return rewardItem;
    }
}

[Serializable]
public class RewardSprite
{
    public RewardType rewardType;
    public Sprite sprite;
}
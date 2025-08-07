using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LevelChestManager : Singleton<LevelChestManager>
{
    public List<LevelChestInfo> levelChestInfos = new List<LevelChestInfo>();



    IEnumerator Start()
    {
        yield return new WaitUntil(() => DataManager.Ins.isLoaded);
        ExpandLevelChestList();
    }

    public void ExpandLevelChestList()
    {
        int maxLevelIndex = DataManager.Ins.playerData.maxLevelIndex;
        int startIndex = 5;
        if (maxLevelIndex >= 25)
        {
            for (int i = maxLevelIndex - 1; i > maxLevelIndex - 25; i--)
            {
                if((i - 5) % 20 == 0)
                {
                    startIndex = i;
                    break;
                }
            }
        }
        for (int i = 0; i < 16; i++)
        {
            levelChestInfos.Add(new LevelChestInfo(0, levelChestInfos[i % 4].rewardInfos));
        }
        for (int i = 0; i < levelChestInfos.Count; i++)
        {
            LevelChestInfo levelChestInfo = levelChestInfos[i];
            levelChestInfo.levelIndex = startIndex + i * 5;
        }
    }

    public void OnWin()
    {
        Image progressImg = UIManager.Ins.GetUI<Win>().giftProgressImg;
        progressImg.fillAmount = (float)DataManager.Ins.playerData.currentGiftProgress / 5;
        DataManager.Ins.playerData.currentGiftProgress += 1;
        BlockUI.Ins.Block();
        progressImg.DOFillAmount((float)DataManager.Ins.playerData.currentGiftProgress / 5, 0.5f).SetDelay(0.3f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            BlockUI.Ins.UnBlock();
            if (DataManager.Ins.playerData.currentGiftProgress == 5)
            {
                Claim();
            }
        });
    }

    public LevelChestInfo GetCurrentLevelChestInfo()
    {
        return levelChestInfos.FirstOrDefault(x => x.levelIndex >= DataManager.Ins.playerData.maxLevelIndex);
    }

    public void Claim()
    {
        DataManager.Ins.playerData.currentGiftProgress = 0;
        DataManager.Ins.SaveData();
    }
}

[Serializable]
public class LevelChestInfo
{
    public int levelIndex;
    public List<RewardInfo> rewardInfos = new List<RewardInfo>();   

    public LevelChestInfo(int levelIndex, List<RewardInfo> rewardInfos)
    {
        this.levelIndex = levelIndex;
        this.rewardInfos = rewardInfos;
    }
}

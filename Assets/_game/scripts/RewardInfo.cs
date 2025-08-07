using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[Serializable]
public class RewardInfo 
{
    public RewardType rewardType;
    public int amount;

    public RewardInfo (RewardType rewardType, int amount)
    {
        this.rewardType = rewardType;
        this.amount = amount;
    }

    public void OnClaim()
    {
        if (rewardType == RewardType.Gold)
        {
            GoldManager.Ins.AnimGold(DataManager.Ins.playerData.gold, DataManager.Ins.playerData.gold+amount,true);
        }
        if (rewardType == RewardType.BoosterHint)
        {
            DataManager.Ins.playerData.boosterHintAmount += amount;
        }
        if (rewardType == RewardType.BoosterTime)
        {
            DataManager.Ins.playerData.boosterTimeAmount += amount;
        }
        Debug.Log("claim reward info: " + amount.ToString() + " " + rewardType.ToString());
    }
}

public enum RewardType
{
    Gold, BoosterHint, BoosterTime, 
}
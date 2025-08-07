using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldManager : Singleton<GoldManager>
{
    public List<TextMeshProUGUI> goldTexts = new();
    public int remainGoldAmount;




    public void OnOpenHome()
    {
        if (remainGoldAmount > 0)
        {
            BlockUI.Ins.Block();
            int remainGoldAmount = this.remainGoldAmount;
            this.remainGoldAmount = 0;
            DOVirtual.DelayedCall(0.8f, () =>
            {
                if(DataManager.Ins.playerData.maxLevelIndex >= GameManager.Ins.continuousLevelAmount)
                CollectGoldManager.Ins.OnInitt(remainGoldAmount, UIManager.Ins.GetUI<Home>().gold.icon.rectTransform.position, () =>
                {
                    //BlockUI.Ins.UnBlock();
                    AnimGold(DataManager.Ins.playerData.gold, DataManager.Ins.playerData.gold + remainGoldAmount, true);
                });
            });
        }
    }

    public void AnimGold(int start, int end, bool isAnim)
    {
        goldTexts.Clear();
        if (UIManager.Ins.IsOpened<Home>())
        {
            goldTexts.Add(UIManager.Ins.GetUI<Home>().gold.txt);
        }
        if (UIManager.Ins.IsOpened<Revive>())
        {
            goldTexts.Add(UIManager.Ins.GetUI<Revive>().gold.txt);
        }
        if (UIManager.Ins.IsOpened<ReviveTime>())
        {
            goldTexts.Add(UIManager.Ins.GetUI<ReviveTime>().gold.txt);
        }
        if (UIManager.Ins.IsOpened<GetMoreBooster>())
        {
            goldTexts.Add(UIManager.Ins.GetUI<GetMoreBooster>().gold.txt);
        }
        float duration = isAnim ? CollectGoldManager.Ins.coins.Count * CollectGoldManager.Ins.spawnDelay + 0.1f : 0f;
        DataManager.Ins.playerData.gold = end;
        foreach (var goldText in goldTexts)
        {
            DOVirtual.Int(start, end, duration, v =>
            {
                goldText.text = v.ToString();
            }).OnComplete(() => {
                DataManager.Ins.playerData.gold = end;
            });
        }
    }
}

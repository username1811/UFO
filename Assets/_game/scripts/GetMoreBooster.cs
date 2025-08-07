using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using TMPro;
using UnityEngine;

public class GetMoreBooster : UICanvas
{
    public BoosterType boosterType;
    public TextMeshProUGUI buttonBuyText;
    public int price;
    public GameObject hintContentObj;
    public GameObject timeContentObj;
    public Gold gold;


    public override void Open()
    {
        base.Open();
        LevelCountDown.Ins.StopCountDown();
        LevelTimer.Ins.StopTimer();
        GoldManager.Ins.AnimGold(DataManager.Ins.playerData.gold, DataManager.Ins.playerData.gold, false);
    }

    public override void CloseDirectly()
    {
        LevelCountDown.Ins.StartCountDown();
        LevelTimer.Ins.ContinueTimer();
        base.CloseDirectly();
    }

    public void OnInitt(BoosterType boosterType)
    {
        this.boosterType = boosterType;
        hintContentObj.SetActive(boosterType==BoosterType.Hint);
        timeContentObj.SetActive(boosterType==BoosterType.Time);
        price = boosterType == BoosterType.Hint? 300:500;
        buttonBuyText.text = price.ToString();
    }

    private void Update()
    {
        buttonBuyText.color = DataManager.Ins.playerData.gold < price ? Constant.COLOR_NOT_ENOUGH_GOLD : Color.white;
    }

    public void Buy()
    {
        if (boosterType == BoosterType.Hint)
        {
            DataManager.Ins.playerData.boosterHintAmount += 1;
        }
        if (boosterType == BoosterType.Time)
        {
            DataManager.Ins.playerData.boosterTimeAmount += 1;
        }
        LevelCountDown.Ins.StartCountDown();
        LevelTimer.Ins.ContinueTimer();
        UIManager.Ins.GetUI<GamePlay>().Refresh();
        UIManager.Ins.GetUI<GamePlay>().buttonBoosters.FirstOrDefault(x => x.boosterType == boosterType).AnimBounceAmount();
        SoundManager.Ins.PlaySFX(SFXType.ClaimBooster);
        UIManager.Ins.CloseUI<GetMoreBooster>();
        
    }

    public void ButtonBuy()
    {
        if (DataManager.Ins.playerData.gold < price) return;
        GoldManager.Ins.AnimGold(DataManager.Ins.playerData.gold, DataManager.Ins.playerData.gold - price, true);
        Buy();
    }

    public void ButtonBuyAds()
    {
        Buy();
    }

    public void ButtonClose()
    {
        UIManager.Ins.CloseUI<GetMoreBooster>();
    }
}

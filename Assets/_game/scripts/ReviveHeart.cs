using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Revive : UICanvas
{
    public TextMeshProUGUI buttonContinueText;
    public Gold gold;
    public int reviveGoldAmount;




    public override void Open()
    {
        base.Open();
        LevelCountDown.Ins.StopCountDown();
        LevelTimer.Ins.StopTimer();
        GoldManager.Ins.AnimGold(DataManager.Ins.playerData.gold, DataManager.Ins.playerData.gold, false);
        SoundManager.Ins.PlaySFX(SFXType.Lose);
    }

    private void Update()
    {
        buttonContinueText.text = reviveGoldAmount.ToString();
        buttonContinueText.color = DataManager.Ins.playerData.gold < reviveGoldAmount ? Constant.COLOR_NOT_ENOUGH_GOLD : Color.white;
    }

    public void Continue()
    {
        LevelManager.Ins.isEndLevel = false;
        LevelManager.Ins.currentLevel.heartAmount = LevelManager.Ins.currentLevel.initialHeartAmount;
        UIManager.Ins.GetUI<GamePlay>().Refresh();
        LevelCountDown.Ins.StartCountDown();
        LevelTimer.Ins.ContinueTimer();
        UIManager.Ins.CloseUI<Revive>();
    }

    public void ButtonContinueAds()
    {
        Continue();
    }

    public void ButtonContinue()
    {
        if (DataManager.Ins.playerData.gold < reviveGoldAmount) return;
        GoldManager.Ins.AnimGold(DataManager.Ins.playerData.gold, DataManager.Ins.playerData.gold-reviveGoldAmount,true);
        Continue();
    }

    public void ButtonClose()
    {
        SceneManagerrr.Ins.ChangeScene(SceneType.Game, () =>
        {
            UIManager.Ins.CloseUI<Revive>();
            LevelManager.Ins.LoadCurrentLevel();
        });
    }
}

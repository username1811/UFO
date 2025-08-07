using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class ReviveTime : UICanvas
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
        SoundManager.Ins.StopBackgroundMusic();
    }

    public override void CloseDirectly()
    {
        LevelCountDown.Ins.StartCountDown();
        LevelTimer.Ins.ContinueTimer();
        base.CloseDirectly();
    }

    private void Update()
    {
        buttonContinueText.text = reviveGoldAmount.ToString();
        buttonContinueText.color = DataManager.Ins.playerData.gold < reviveGoldAmount ? Constant.COLOR_NOT_ENOUGH_GOLD : Color.white;
    }

    public void Continue()
    {
        LevelManager.Ins.isEndLevel = false;
        LevelCountDown.Ins.AddSeconds(60);
        LevelCountDown.Ins.StartCountDown();
        LevelTimer.Ins.ContinueTimer();
        UIManager.Ins.GetUI<GamePlay>().Refresh();
        UIManager.Ins.GetUI<GamePlay>().ResetCountDownText();
        UIManager.Ins.GetUI<GamePlay>().levelCountDownObj.transform.DOScale(1.1f, 0.12f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
        UIManager.Ins.CloseUI<ReviveTime>();
        SoundManager.Ins.PlayBackgroundMusic();
        SoundManager.Ins.PlaySFX(SFXType.BoosterTime);
    }

    public void ButtonContinueAds()
    {
        Continue();
    }

    public void ButtonContinue()
    {
        if (DataManager.Ins.playerData.gold < reviveGoldAmount) return;
        GoldManager.Ins.AnimGold(DataManager.Ins.playerData.gold, DataManager.Ins.playerData.gold - reviveGoldAmount, true);
        Continue();
    }

    public void ButtonClose()
    {
        SceneManagerrr.Ins.ChangeScene(SceneType.Game, () =>
        {
            UIManager.Ins.CloseUI<ReviveTime>();
            LevelManager.Ins.LoadCurrentLevel();
        });
    }
}

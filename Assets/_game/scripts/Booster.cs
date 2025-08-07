using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;

[Serializable]
public class Booster
{
    public BoosterType boosterType;
    public Sprite icon;
    public int levelIndexToIntroduce;
    public int price;

    public virtual void Use()
    {
        BoosterManager.Ins.isUsingBooster = true;
        if (LevelManager.Ins.isEndLevel) return;
    }

    public virtual void OnUpdate()
    {

    }

    public virtual void FinishUse()
    {
        BoosterManager.Ins.currentBooster = null;
        BoosterManager.Ins.isUsingBooster = false;
    }

    public void OnUnlock()
    {
        /*if (DataManager.Ins.playerData.unlockedBoosterTypes.Contains(boosterType)) return;
        DataManager.Ins.playerData.unlockedBoosterTypes.Add(boosterType);
        switch (boosterType)
        {
            case BoosterType.Hint:
                DataManager.Ins.playerData.boosterHintAmount = 1;
                break;
            case BoosterType.Bomb:
                DataManager.Ins.playerData.boosterBombAmount = 1;
                break;
        }*/
    }

}

public class BoosterHint : Booster
{
    public override void Use()
    {
        base.Use();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void FinishUse()
    {
        base.FinishUse();
    }
}


public class BoosterTime : Booster
{
    public override void Use()
    {
        base.Use();
        LevelCountDown.Ins.remainingTime += 60;
        LevelCountDown.Ins.countDownStr = LevelCountDown.Ins.SecToString(LevelCountDown.Ins.remainingTime);
        UIManager.Ins.GetUI<GamePlay>().levelCountDownObj.transform.DOScale(1.1f, 0.12f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
        DataManager.Ins.playerData.boosterTimeAmount -= 1;
        SoundManager.Ins.PlaySFX(SFXType.BoosterTime);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
    }

    public override void FinishUse()
    {
        base.FinishUse();
    }
}
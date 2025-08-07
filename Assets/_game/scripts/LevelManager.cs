using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.ProbeAdjustmentVolume;

public class LevelManager : Singleton<LevelManager>
{
    public Level currentLevel;
    public LevelWrapperrr levelWrapperrr;
    public bool isEndLevel;
    public int levelIndexToReturn;
    public LevelInfooo currentLevelInfooo;
    public static bool isLoadedLevel;


    public void DestroyCurrentLevel()
    {
        if (currentLevel != null)
        {
            PoolManager.Ins.DespawnAll();
            Destroy(currentLevel.gameObject);
            currentLevel = null;
        }
    }

    public int GetLoopLevelIndex(int levelIndex)
    {
        if (levelIndex >= levelWrapperrr.levels.Count)
            return ((levelIndex - levelIndexToReturn) % (levelWrapperrr.levels.Count - levelIndexToReturn) + levelIndexToReturn);
        else return levelIndex;
    }

    public Action OnCompleteLoadLevel = () =>
    {
        DOVirtual.DelayedCall(Time.deltaTime * 3f, () =>
        {
            UIManager.Ins.OpenUI<GamePlay>();
            UIManager.Ins.GetUI<GamePlay>().OnLoadLevel();
            isLoadedLevel = true;
            BlockUI.Ins.Block();
            BoosterManager.Ins.OnLoadLevel();
            if (NoInternet.Check())
            {
                LevelCountDown.Ins.OnLoadLevel();
                LevelTimer.Ins.OnLoadLevel();
            }
            DOVirtual.DelayedCall(Time.deltaTime*2, () =>
            {
                AnimEnterLevel.Ins.OnLoadLevel();

            });
        });
    };

    public void LoadLevel(int levelIndex)
    {
        Debug.Log("load level");
        isLoadedLevel = false;
        isEndLevel = false;
        DestroyCurrentLevel();
        currentLevelInfooo = GetLevelInfo(levelIndex);
        CreateLevelFromLevelInfooo(currentLevelInfooo);
        currentLevel.OnInitt(currentLevelInfooo.heartAmount);
        OnCompleteLoadLevel?.Invoke();
    }

    public void LoadLevel(LevelInfooo levelInfooo)
    {
        isLoadedLevel = false;
        isEndLevel = false;
        DestroyCurrentLevel();
        currentLevelInfooo = levelInfooo;
        CreateLevelFromLevelInfooo(currentLevelInfooo);
        currentLevel.OnInitt(currentLevelInfooo.heartAmount);
        OnCompleteLoadLevel?.Invoke();
    }

    public void LoadNextLevel()
    {
        DataManager.Ins.playerData.currentLevelIndex += 1;
        DataManager.Ins.playerData.maxLevelIndex = Math.Max(DataManager.Ins.playerData.currentLevelIndex+1, DataManager.Ins.playerData.maxLevelIndex);
        LoadLevel(DataManager.Ins.playerData.currentLevelIndex);
    }

    public void LoadCurrentLevel()
    {
        LoadLevel(DataManager.Ins.playerData.currentLevelIndex);
    }

    public void LoadLevelTut()
    {
        LoadLevel(levelWrapperrr.tutLevelInfo);
    }

    public void CheckWinLose()
    {
        StartCoroutine(IECheckWinLose());
        IEnumerator IECheckWinLose()
        {
            yield return null;
            if (isEndLevel)
            {
                yield break;
            }
            bool isWin = true;
            bool isLose = currentLevel.heartAmount <= 0 && DataManager.Ins.playerData.currentLevelIndex >= 1;
            if (isWin)
            {
                Win();
            }
            else if (isLose)
            {
                Lose(LoseType.OutOfHeart);
            }
        }
    }

    public void Win()
    {
        if (isEndLevel) return;
        isEndLevel = true;
        LevelCountDown.Ins.StopCountDown();
        LevelTimer.Ins.StopTimer();
        SoundManager.Ins.StopBackgroundMusic();
    }

    public void Lose(LoseType loseType)
    {
        if (isEndLevel) return;
        isEndLevel = true;
        DOVirtual.DelayedCall(0.1f, () =>
        {
            if (loseType == LoseType.OutOfHeart)
            {
                UIManager.Ins.OpenUI<Revive>();
            }
            if (loseType == LoseType.OutOfTime)
            {
                UIManager.Ins.OpenUI<ReviveTime>();
            }
        });
        UIManager.Ins.GetUI<GamePlay>().OnLose();
    }

    public void WinImmediately()
    {
        Win();
    }

    public void LoseImmediately()
    {
        Lose(LoseType.OutOfHeart);
    }

    public LevelInfooo GetLevelInfo(int levelIndex)
    {
        return levelWrapperrr.levels[GetLoopLevelIndex(levelIndex)];
    }

    public void CreateLevelFromLevelInfooo(LevelInfooo levelInfo)
    {
        
    }
}

public enum LoseType
{
    OutOfHeart, OutOfTime
}

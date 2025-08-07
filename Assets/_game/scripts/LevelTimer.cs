using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelTimer : Singleton<LevelTimer>
{
    public float currentTime;
    public int levelIndex;

    public void Start()
    {
        levelIndex = -1;
    }

    public void OnLoadLevel()
    {
        StartTimer();
    }

    IEnumerator IEStartTimer()
    {
        while (true)
        {
            currentTime += Time.deltaTime;
            yield return null;
        }
    }

    public void StartTimer()
    {
        levelIndex = DataManager.Ins.playerData.currentLevelIndex;
        currentTime = 0;
        StartCoroutine(IEStartTimer());
    }

    public void ContinueTimer()
    {
        StartCoroutine(IEStartTimer());
    }

    public void StopTimer()
    {
        StopAllCoroutines();
    }
}

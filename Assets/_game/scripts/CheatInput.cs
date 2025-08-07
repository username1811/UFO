using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CheatInput : Singleton<CheatInput> 
{
    public bool isCheatDateTime; 
    public bool isBlockAds;
    public bool isCheat;
    public bool isCreative;
    public bool isStaticCameraLoadLevel;

    void Update()
    {
        if (SceneManagerrr.Ins.currentSceneType != SceneType.Game) return;
        if (!isCheat) return;
        if (Input.GetKeyDown(KeyCode.A))
        {
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            AutoPlay();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Win();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            BoosterManager.Ins.UseBooster(BoosterType.Hint);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            SceneManagerrr.Ins.ChangeScene(SceneType.Game, () =>
            {
                LevelManager.Ins.LoadNextLevel();
            });
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            SceneManagerrr.Ins.ChangeScene(SceneType.Game, () =>
            {
                LevelManager.Ins.LoadCurrentLevel();
            });
        }
    }

    public void AutoPlay()
    {
        LevelManager.Ins.currentLevel.heartAmount = 999;
        StartCoroutine(IEAutoPlay());
        IEnumerator IEAutoPlay()
        {
            LevelManager.Ins.CheckWinLose();
            yield return null;
        }
    }

    public void Win()
    {
        LevelManager.Ins.CheckWinLose();
    }
}

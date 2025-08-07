using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // Thêm namespace để sử dụng UI components

public class CheatPanel : UICanvas
{
    [SerializeField] private TMP_InputField levelInputField; // InputField để nhập level index



    public override void Open()
    {
        base.Open();
        levelInputField.text = "";
    }

    public void ButtonAutoPlay()
    {
        CheatInput.Ins.AutoPlay();
        UIManager.Ins.CloseUI<CheatPanel>();
    }

    public void ButtonResourcess()
    {
        DataManager.Ins.playerData.gold = 9999; 
        DataManager.Ins.playerData.boosterHintAmount = 99;
        DataManager.Ins.playerData.boosterTimeAmount = 99;
        GoldManager.Ins.AnimGold(DataManager.Ins.playerData.gold, DataManager.Ins.playerData.gold,true);
        if (UIManager.Ins.IsOpened<GamePlay>())
        {
            UIManager.Ins.GetUI<GamePlay>().Refresh();
        }
        UIManager.Ins.CloseUI<CheatPanel>();
    }

    public void ButtonClose()
    {
        PlayLevel();
        UIManager.Ins.CloseUI<CheatPanel>();
    }

    public void PlayLevel()
    {
        if (levelInputField != null && !string.IsNullOrEmpty(levelInputField.text))
        {
            if (int.TryParse(levelInputField.text, out int levelIndex))
            {
                SceneManagerrr.Ins.ChangeScene(SceneType.Game, () =>
                {
                    // Gọi hàm để set level với levelIndex
                    DataManager.Ins.playerData.currentLevelIndex = levelIndex-1;
                    DataManager.Ins.playerData.maxLevelIndex = levelIndex-1;
                    LevelManager.Ins.LoadCurrentLevel();
                });
            }
            else
            {
                Debug.LogWarning("Invalid level index input!");
            }
        }
        else
        {
            Debug.LogWarning("Level input field is empty or not assigned!");
        }
    }
}
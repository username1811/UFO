using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;
using System.Linq;

[Serializable]
public class DataManager : Singleton<DataManager>
{
    public bool isLoaded = false;
    public PlayerData playerData;
    public const string PLAYER_DATA = "PLAYER_DATA";

    private void OnApplicationPause(bool pause) { SaveData(); }
    private void OnApplicationQuit() { SaveData(); }



    [Button]
    public void LoadData(bool isShowAOA = false)
    {
        Debug.Log("START LOAD DATA");
        string d = PlayerPrefs.GetString(PLAYER_DATA, "");
        if (d != "")
        {
            playerData = JsonUtility.FromJson<PlayerData>(d);
        }
        else
        {
            playerData = new PlayerData();
        }

        if (DataManager.Ins.playerData.firstOpenDateTimeStr == null || DataManager.Ins.playerData.firstOpenDateTimeStr == "")
        {
            DataManager.Ins.playerData.firstOpenDateTimeStr = WorldTimeAPI.Ins.GetCurrentDateTime().ToString();
        }
        isLoaded = true;


    }

    public float checkNewDayInterval = -1f;
    private void Update()
    {
        if (!isLoaded) return;
        if(checkNewDayInterval > 0)
        {
            checkNewDayInterval -= Time.deltaTime;
            return;
        }
        if(checkNewDayInterval < 0)
        {
            checkNewDayInterval = 1f;
            CheckNewDay();
            CheckNewWeek();
        }
    }

    [Button]
    public void InitNewEndOfDayEndOfWeek()
    {
        CalculateNewEndOfDay();
        CalculateNewEndOfWeek();
    }

    public void CheckNewDay()
    {
        DateTime timeNow = WorldTimeAPI.Ins.GetCurrentDateTime();
        if (DateTime.Compare(timeNow, DateTime.Parse(playerData.endOfDayStr)) > 0)
        {
            Debug.Log("time now: " + timeNow.ToString());
            Debug.Log("end of day: " + playerData.endOfDayStr.ToString());
            NewDay();
            CalculateNewEndOfDay();
        }
    }

    public void NewDay()
    {
        playerData.retentionDay += 1;
    }

    public void CheckNewWeek()
    {
        DateTime timeNow = WorldTimeAPI.Ins.GetCurrentDateTime();
        if (DateTime.Compare(timeNow, DateTime.Parse(playerData.endOfWeekStr)) > 0)
        {
            NewWeek();
            CalculateNewEndOfWeek();
        }
    }

    public void NewWeek()
    {
        
    }

    public void CalculateNewEndOfDay()
    {
        playerData.endOfDayStr = WorldTimeAPI.GetEndOfDay().ToString();
    }

    public void CalculateNewEndOfWeek()
    {
        playerData.endOfWeekStr = WorldTimeAPI.GetEndOfWeek().ToString();
    }

    public void SaveData()
    {
        if (!isLoaded) return;
        string json = JsonUtility.ToJson(playerData);
        PlayerPrefs.SetString(PLAYER_DATA, json);
        PlayerPrefs.Save();
        Debug.Log("SAVE DATA");
    }
}

[System.Serializable]
public class PlayerData
{
    [Header("time:")]
    public string firstOpenDateTimeStr;
    public string endOfDayStr;
    public string endOfWeekStr;

    [Header("--------- Game Params ---------")]
    [Header("resource:")]
    public int gold;
    [Header("level:")]
    public int currentLevelIndex;
    public int maxLevelIndex;
    public List<LevelTrackingData> levelTrackingDatas = new();
    public bool isPassedTutClickStar;
    public bool isPassedTutClickButtonLevel;
    public bool isPassedLevelTut;
    [Header("booster:")]
    public List<BoosterType> unlockedBoosterTypes = new();
    public int boosterHintAmount;
    public int boosterTimeAmount;
    [Header("IAP:")]
    public bool isPurchasedRemoveAds;
    public List<string> purchasedIDs;
    [Header("aflyer:")]
    public int interAdCount;
    public int rewardAdCount;
    [Header("settings:")]
    public bool isSoundOn;
    public bool isMusicOn;
    public bool isVibrateOn;
    [Header("Daily Reward:")]
    public List<int> claimedDailyRewardDataIds = new();
    public string dailyRewardCoolDownExpire;
    [Header("Win Gift:")]
    public int currentGiftProgress;
    [Header("Spin:")]
    public string spinCoolDownExpire;
    public bool isSpinedFreeToday;

    [Header("FIREBASE")]
    #region firebase
    public bool isFirstOpen = false;
    public string lastExitTime;
    public int currentSession;
    public int maxCheckPointStartIndex;
    public int maxCheckPointEndIndex;
    public int retentionDay;
    public int winstreak;
    #endregion


    public PlayerData()
    {
        //time
        firstOpenDateTimeStr = DateTime.Now.ToString();
        endOfDayStr = DateTime.Now.AddDays(-999).ToString();
        endOfWeekStr = DateTime.Now.AddDays(-999).ToString();
        //resource
        gold = 100;
        //level
        currentLevelIndex = 0;
        maxLevelIndex = 0;
        levelTrackingDatas = new();
        isPassedTutClickStar = false;
        //IAP
        purchasedIDs = new List<string>();
        //booster
        unlockedBoosterTypes = new();
        boosterHintAmount = 2;
        boosterTimeAmount = 2;
        //settings
        isSoundOn = true;
        isMusicOn = true;
        isVibrateOn = true;
        //dailyreward
        claimedDailyRewardDataIds = new();

        //fire base
        isFirstOpen = true;
        lastExitTime = new DateTime(1970, 1, 1).ToString();
        currentSession = 0;
        maxCheckPointStartIndex = 0;
        maxCheckPointEndIndex = -1;
    }
}

[Serializable]
public class LevelTrackingData
{
    public int levelIndex;
    public int duration;
    public int retryCount;
    public int heartCount;
    public int boosterSpendCount;

    public LevelTrackingData()
    {
    }

    public LevelTrackingData(int levelIndex, int duration, int retryCount, int heartCount, int boosterSpendCount)
    {
        this.levelIndex = levelIndex;
        this.duration = duration;
        this.retryCount = retryCount;
        this.heartCount = heartCount;
        this.boosterSpendCount = boosterSpendCount;
    }
}

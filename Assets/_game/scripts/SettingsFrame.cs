using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsFrame : MonoBehaviour
{
    public SettingsType settingsType;
    public GameObject offObj;
    public GameObject onObj;
    public Button button;


    public void OnInitt()
    {
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            OnClick();
        });
        Refresh();
    }

    public bool IsOn()
    {
        if (settingsType == SettingsType.Sound)
        {
            return DataManager.Ins.playerData.isSoundOn;
        }
        if (settingsType == SettingsType.Music)
        {
            return DataManager.Ins.playerData.isMusicOn;
        }
        if (settingsType == SettingsType.Vibration)
        {
            return DataManager.Ins.playerData.isVibrateOn;
        }
        return false;
    }

    public void OnClick()
    {
        if (settingsType == SettingsType.Sound)
        {
            DataManager.Ins.playerData.isSoundOn = !DataManager.Ins.playerData.isSoundOn;
        }
        if (settingsType == SettingsType.Music)
        {
            DataManager.Ins.playerData.isMusicOn = !DataManager.Ins.playerData.isMusicOn;
            SoundManager.Ins.TurnMusic(DataManager.Ins.playerData.isMusicOn);
        }
        if (settingsType == SettingsType.Vibration)
        {
            DataManager.Ins.playerData.isVibrateOn = !DataManager.Ins.playerData.isVibrateOn;
        }
        Refresh();
    }

    public void Refresh()
    {
        offObj.SetActive(!IsOn());
        onObj.SetActive(IsOn());
    }
}

public enum SettingsType
{
    Sound, Music, Vibration,
}
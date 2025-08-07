using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : UICanvas
{
    public List<SettingsFrame> settingsFrames = new List<SettingsFrame>();
    public GameObject buttonRemoveAdsObj;



    public override void Start()
    {
        base.Start();
        foreach(var frame in settingsFrames)
        {
            frame.OnInitt();
        }
    }

    public override void Open()
    {
        base.Open();
        buttonRemoveAdsObj.gameObject.SetActive(!DataManager.Ins.playerData.isPurchasedRemoveAds);
    }

    public void ButtonClose()
    {
        UIManager.Ins.CloseUI<Settings>();
    }
}

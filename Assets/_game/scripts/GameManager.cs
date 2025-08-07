//using AppsFlyerSDK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public bool isInited = false;

    [Header("Config:")]
    public float timeScale = 1f;
    public float polygonMinOpacity=0.2f;
    public float polygonMaxOpacity=0.5f;
    public float colorBrighterOffset = 1.1f;


    [Header("LayerMasks:")]
    public LayerMask starMask;

    [Header("Level:")]
    public int continuousLevelAmount;


    private void Start()
    {
        Application.targetFrameRate = 60;
        Input.multiTouchEnabled = true;
        StartCoroutine(LoadAllData());
    }

    private void Update()
    {
        Time.timeScale = timeScale;
    }

    public IEnumerator LoadAllData()
    {
        yield return null;
        yield return null;
        UIManager.Ins.InitWidthHeight();
        yield return null;
        WorldTimeAPI.Ins.OnInit();
        yield return null;
        DataManager.Ins.LoadData();
        yield return null;
        //IAPManager.Ins.InitializeIAP();
        yield return null;
        isInited = true;
        yield return null;
    }
}

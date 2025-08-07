using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Win : UICanvas
{
    public int goldAmount;
    public TextMeshProUGUI claimText;
    public TextMeshProUGUI claimTextMultiplier;
    public GameObject buttonContinueAdsObj;
    public GameObject buttonContinueObj;
    public GameObject buttonContinueNoCoinObj;
    public RectTransform frameRectTF;
    public Image giftProgressImg;


    public override void Open()
    {
        base.Open();
        DataManager.Ins.playerData.currentLevelIndex += 1;
        SoundManager.Ins.PlaySFX(SFXType.Win);
        buttonContinueAdsObj.SetActive(DataManager.Ins.playerData.currentLevelIndex >= GameManager.Ins.continuousLevelAmount);
        buttonContinueObj.SetActive(DataManager.Ins.playerData.currentLevelIndex >= GameManager.Ins.continuousLevelAmount);
        buttonContinueNoCoinObj.SetActive(DataManager.Ins.playerData.currentLevelIndex < GameManager.Ins.continuousLevelAmount);
        frameRectTF.sizeDelta = DataManager.Ins.playerData.currentLevelIndex >= GameManager.Ins.continuousLevelAmount ? new Vector2(885f, 1068f) : new Vector2(885f, 900f);
        SoundManager.Ins.StopBackgroundMusic();
    }

    public void Continue() 
    {
        SceneManagerrr.Ins.ChangeScene(SceneType.Game, () =>
        {
            UIManager.Ins.CloseAll();
            DOVirtual.DelayedCall(Time.deltaTime * 2f, () =>
            {
                if (DataManager.Ins.playerData.currentLevelIndex >= GameManager.Ins.continuousLevelAmount)
                {
                    UIManager.Ins.OpenUI<Home>();
                }
                else
                {
                    LevelManager.Ins.LoadCurrentLevel();
                }
            });
        });
    }

    public void ButtonContinue()
    {
        GoldManager.Ins.remainGoldAmount = goldAmount;
        Continue();
    }
}

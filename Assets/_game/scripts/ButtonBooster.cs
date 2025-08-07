using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class ButtonBooster : MonoBehaviour
{
    public BoosterType boosterType;
    public RectTransform rectTransform;
    public Button button;


    [Header("con:")]
    public GameObject numberParent;
    public TextMeshProUGUI numberText;

    [Header("ko con:")]
    public GameObject addObj;

    [Header("coming soon:")]
    public GameObject lockObj;
    public TextMeshProUGUI lockText;
    public bool isLock => lockObj.gameObject.activeInHierarchy;


    public Booster booster => BoosterManager.Ins.boosters.FirstOrDefault(x => x.boosterType == boosterType);


    public void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            OnClick();
        });
    }

    public void OnInitt()
    {
        Refresh();
    }

    public void Refresh()
    {
        RefreshAmount();
        RefreshLock();
    }

    public void RefreshLock()
    {
        bool isShowLock = DataManager.Ins.playerData.maxLevelIndex < booster.levelIndexToIntroduce;
        ShowLock(isShowLock);
    }

    public void ShowLock(bool isShow)
    {
        lockObj.SetActive(isShow);
        lockText.text = "Lv " + (booster.levelIndexToIntroduce + 1).ToString();
    }

    public void RefreshAmount()
    {
        if (booster.levelIndexToIntroduce > DataManager.Ins.playerData.maxLevelIndex)
        {
            numberParent.SetActive(false);
            addObj.SetActive(false);
            return;
        }
        int currentAmount = 0;
        switch (boosterType)
        {
            case BoosterType.Hint:
                currentAmount = DataManager.Ins.playerData.boosterHintAmount; break;
            case BoosterType.Time:
                currentAmount = DataManager.Ins.playerData.boosterTimeAmount; break;
        }
        if (currentAmount > 0 /*&& DataManager.Ins.playerData.unlockedBoosterTypes.Contains(boosterType)*/)
        {
            numberParent.SetActive(true);
            addObj.SetActive(false);
            numberText.text = currentAmount.ToString();
        }
        else
        {
            numberParent.SetActive(false);
            //addObj.SetActive(DataManager.Ins.playerData.unlockedBoosterTypes.Contains(boosterType));
            addObj.SetActive(true);
        }
    }

    public void OnClick()
    {
        if (isLock) return;
        int currentAmount = 0;
        switch (boosterType)
        {
            case BoosterType.Hint:
                currentAmount = DataManager.Ins.playerData.boosterHintAmount; break;
            case BoosterType.Time:
                currentAmount = DataManager.Ins.playerData.boosterTimeAmount; break;
        }
        if (currentAmount <= 0)
        {
            UIManager.Ins.OpenUI<GetMoreBooster>().OnInitt(boosterType);
        }
        else
        {
            BoosterManager.Ins.UseBooster(boosterType);
            if (boosterType == BoosterType.Time) {
                LevelCountDown.Ins.StartCountDown();
                UIManager.Ins.GetUI<GamePlay>().EffectAddTime();
            } 
        }
        UIManager.Ins.GetUI<GamePlay>().Refresh();
    }

    IEnumerator IERestoreButton()
    {
        yield return new WaitForSeconds(0.2f);
        button.enabled = true;
    }

    public void AnimBounceAmount()
    {
        Vector3 oldScale = numberParent.transform.localScale;
        numberParent.transform.DOScale(1.4f, 0.1f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            numberParent.transform.DOScale(oldScale, 0.1f).SetEase(Ease.InSine);
        });
    }
}

using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Home : UICanvas
{
    public static bool isFirstOpened;
    [Title("ANIMATION:")]
    public RectTransform leftRectTF;
    public RectTransform rightRectTF;
    [Title("GOLD:")]
    public Gold gold;
    [Title("NOTI:")]
    public Noti dailyQuestNoti;
    public Noti leaderboardNoti;
    public Noti dailyRewardNoti;
    public Noti spinNoti;
    [Title("BUTTONS:")]
    public GameObject buttonDailyQuestObj;
    public GameObject buttonLeaderboardObj;
    public GameObject buttonDailyRewardObj;
    public GameObject buttonSpinObj;
    [Title("BUTTON PLAY:")]
    public Image buttonPlayImg;
    public TextMeshProUGUI buttonPlayText;
    [Title("LEVEL TREE:")]
    public GameObject mask;
    public GameObject bgMask;


    public override void Start()
    {
        base.Start();
    }

    public override void Open()
    {
        base.Open();
        GoldManager.Ins.AnimGold(DataManager.Ins.playerData.gold, DataManager.Ins.playerData.gold, false);
        GoldManager.Ins.OnOpenHome();
        isFirstOpened = true;
        RefreshNotis();
        buttonLeaderboardObj.SetActive(DataManager.Ins.playerData.maxLevelIndex >= 15);
        DataManager.Ins.playerData.currentLevelIndex = DataManager.Ins.playerData.maxLevelIndex;
        LevelTree.Ins.OnOpenHome(() => {
            buttonPlayText.text = "Level " + (DataManager.Ins.playerData.maxLevelIndex + 1).ToString();
        });
        AnimShowElements();
        bgMask.transform.SetParent(mask.transform);
    }

    public override void CloseDirectly()
    {
        base.CloseDirectly();
    }

    [Button]
    public void AnimShowElements()
    {
        float delay = 0.6f;
        float duration = 0.45f;
        float distance = 230f;
        //left
        Vector2 oldLeftPos = new Vector2(0, leftRectTF.anchoredPosition.y);
        leftRectTF.anchoredPosition = new Vector2(-distance, leftRectTF.anchoredPosition.y);
        leftRectTF.DOAnchorPos(oldLeftPos, duration).SetDelay(delay).SetEase(Ease.OutBack);
        //right
        Vector2 oldRightPos = new Vector2(0, leftRectTF.anchoredPosition.y);
        rightRectTF.anchoredPosition = new Vector2(distance, rightRectTF.anchoredPosition.y);
        rightRectTF.DOAnchorPos(oldRightPos, duration).SetDelay(delay).SetEase(Ease.OutBack);
    }

    [Button]
    public void AnimHideElements(Action OnCOmplete)
    {
        float delay = 0f;
        float duration = 0.4f;
        float distance = 230f;
        //left
        leftRectTF.anchoredPosition = new Vector2(0, leftRectTF.anchoredPosition.y);
        leftRectTF.DOAnchorPos(new Vector2(-distance, leftRectTF.anchoredPosition.y), duration).SetDelay(delay).SetEase(Ease.InBack);
        //right
        rightRectTF.anchoredPosition = new Vector2(0, rightRectTF.anchoredPosition.y);
        rightRectTF.DOAnchorPos(new Vector2(distance, rightRectTF.anchoredPosition.y), duration).SetDelay(delay).SetEase(Ease.InBack).OnComplete(() =>
        {
            OnCOmplete?.Invoke();
        });
    }

    public void AnimOpen(Action OnComplete)
    {
        AnimShowElements();
        buttonPlayImg.gameObject.SetActive(true);
        OnComplete?.Invoke();
    }

    public Tween goldScaleTween;
    public void AnimGoldCollect()
    {
        goldScaleTween?.Kill();
        gold.transform.localScale = Vector3.one * 1.1f;
        goldScaleTween = gold.transform.DOScale(1f, 0.1f);
    }

    public void RefreshNotis()
    {
    }

    public void ButtonSettings()
    {
        UIManager.Ins.OpenUI<Settings>();
    }

    public void ButtonPlay()
    {
        if (LevelTree.Ins.currentButtonLevelTree.levelIndex > DataManager.Ins.playerData.maxLevelIndex) return;
        if (LevelTree.Ins.isDoingAnim) return;
        SceneManagerrr.Ins.ChangeScene(SceneType.Game, () =>
        {
            UIManager.Ins.CloseAll();
            UIManager.Ins.OpenUI<GamePlay>();
            LevelManager.Ins.LoadCurrentLevel();
        });
    }

}

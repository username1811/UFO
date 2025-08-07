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

public class GamePlay : UICanvas
{
    public TextMeshProUGUI levelText;
    public HorizontalLayoutGroup heartHorizontalLayoutGroup;
    public TextMeshProUGUI levelCountDownText;
    public GameObject levelCountDownObj;
    public List<ButtonBooster> buttonBoosters = new List<ButtonBooster>();
    public RectTransform handRectTF;
    public GameObject tutObj;
    public GameObject buttonHomeObj;
    public GameObject buttonReplayObj;
    public Color initialCountDownTextColor;
    public CanvasGroup addTimeEffect;
    [Title("TUTORIAL BOOSTER:")]
    public CanvasGroup blackCover;
    public RectTransform buttonHintRectTF;
    public RectTransform buttonTimeRectTF;
    [Title("TARGET OBJECTS:")]
    public Transform targetObjectsParent;
    public List<TargetObject> targetObjects = new List<TargetObject>(); 




    public override void Start()
    {
        base.Start();
        levelText?.AddComponent<Button>().onClick.AddListener(() =>
        {
            UIManager.Ins.OpenUI<CheatPanel>();
        });
    }

    public override void Awake()
    {
        base.Awake();
        initialCountDownTextColor = levelCountDownText.color;
        buttonBoosters = GetComponentsInChildren<ButtonBooster>().ToList();
    }

    public void InitTargetObject(TargetObjectInfo targetObjectInfo)
    {
        TargetObject targetObject = PoolManager.Ins.Spawn<TargetObject>(PoolType.TargetObject);
        targetObject.OnInitt(targetObjectInfo);
        targetObject.transform.SetParent(targetObjectsParent);
        targetObject.transform.localScale = Vector3.one;
        targetObject.GetComponent<RectTransform>().localPosition = new Vector2(160f * targetObjects.Count, 0);
        targetObjects.Add(targetObject);
    }

    public void InitTargetObjects()
    {
        targetObjects.Clear();
        PoolManager.Ins.GetPool(PoolType.TargetObject).ReturnAll();
        InitTargetObject(new TargetObjectInfo(TargetObjectType.Box, 100));
    }

    private void Update()
    {
        levelCountDownText.text = LevelCountDown.Ins.countDownStr;
        if(LevelCountDown.Ins.isCountingDown && LevelCountDown.Ins.remainingTime < 20f)
        {
            CountDownTextRed();
        }
    }

    public void OnLoadLevel()
    {
        levelText.text = "Level " + (DataManager.Ins.playerData.currentLevelIndex+1).ToString();
        levelCountDownObj.SetActive(LevelCountDown.Ins.isActive);
        handRectTF.gameObject.SetActive(false);
        buttonHomeObj.SetActive(DataManager.Ins.playerData.maxLevelIndex >= GameManager.Ins.continuousLevelAmount);
        buttonReplayObj.SetActive(DataManager.Ins.playerData.maxLevelIndex >= GameManager.Ins.continuousLevelAmount);
        Refresh();
        ResetCountDownText();
        addTimeEffect.gameObject.SetActive(false);
    }

    public override void CloseDirectly()
    {
        base.CloseDirectly();
    }

    public void RefreshTut()
    {
        tutObj.SetActive(!DataManager.Ins.playerData.isPassedTutClickStar);
    }

    public void UpdateHearts()
    {
        
    }

    public void OnWin() { 
    
    }

    public void OnLose()
    {

    }

    public void ShowConfetti(bool isShow)
    {

    }

    public void Refresh()
    {
        UpdateHearts();
        foreach(var buttonBooster in buttonBoosters)
        {
            buttonBooster.Refresh();
        }
    }

    public void ResetCountDownText()
    {
        levelCountDownText.color = initialCountDownTextColor;
    }

    public void CountDownTextRed()
    {
        levelCountDownText.color = Color.red;
    }

    Tween addTimeMoveTween;
    Tween addTimeFadeTween;
    Tween addTimeScaleTween;
    public void EffectAddTime()
    {
        addTimeMoveTween?.Kill();
        addTimeFadeTween?.Kill();
        addTimeScaleTween?.Kill();
        float duration = 1.7f;
        RectTransform addTimeEffectRectTF = addTimeEffect.GetComponent<RectTransform>();
        addTimeEffect.gameObject.SetActive(true);
        addTimeEffectRectTF.anchoredPosition = new Vector2(173, -147);
        addTimeEffect.alpha = 1f;
        addTimeFadeTween = addTimeEffect.DOFade(0f, duration / 2).SetDelay(duration / 2).SetEase(Ease.OutSine);
        addTimeMoveTween = addTimeEffectRectTF.DOAnchorPosY(addTimeEffectRectTF.anchoredPosition.y + 10f, duration).SetEase(Ease.OutSine).OnComplete(() =>
        {
            addTimeEffect.gameObject.SetActive(false); 
        });
        addTimeEffect.transform.localScale = Vector3.one * 0.3f;
        addTimeScaleTween = addTimeEffect.transform.DOScale(1f,0.3f).SetEase(Ease.OutBack);
    }

    public void ButtonHome()
    {
        if (LevelManager.Ins.isEndLevel) return;
        LevelCountDown.Ins.StopCountDown();
        LevelTimer.Ins.StopTimer();
        SceneManagerrr.Ins.ChangeScene(SceneType.Game, () =>
        {
            UIManager.Ins.CloseAll();
            UIManager.Ins.OpenUI<Home>();
        });
    }

    public void ButtonReplay()
    {
        LevelCountDown.Ins.StopCountDown();
        LevelTimer.Ins.StopTimer();
        SceneManagerrr.Ins.ChangeScene(SceneType.Game, () =>
        {
            LevelManager.Ins.LoadCurrentLevel();
        });
    }

    public void RemindUsingBooster(BoosterType boosterType)
    {
        UIManager.Ins.GetUI<GamePlay>().buttonBoosters.FirstOrDefault(x => x.boosterType == boosterType).transform.DOScale(1.07f, 0.5f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
    }
}

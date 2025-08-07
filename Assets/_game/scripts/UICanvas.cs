using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UICanvas : MonoBehaviour
{
    //public bool IsAvoidBackKey = false;
    [Title("UI CANVAS:")]
    public bool IsDestroyOnClose = false;
    public bool isAllButtonEffect = true;
    public RectTransform rectTransform;
    public Image bgImg;
    public CanvasGroup canvasGroup;

    public virtual void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        if (isAllButtonEffect)
        {
            this.gameObject.AddComponent<AddButtonEffectsToButtons>();
        }
    }

    public virtual void Start()
    {
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.sizeDelta = new Vector2(UIManager.Ins.screenWidth, UIManager.Ins.screenHeight);
        //rectTransform.anchoredPosition = Vector2.zero;
    }

    //Setup canvas to avoid flash UI
    //set up mac dinh cho UI de tranh truong hop bi nhay' hinh
    public virtual void Setup()
    {
        UIManager.Ins.AddBackUI(this);
        //UIManager.Ins.PushBackAction(this, BackKey);

        //SetRectTransformToStretch(this.GetComponent<RectTransform>());

    }

    public void SetRectTransformToStretch(RectTransform RectTransform)
    {
        // Set anchors to stretch-stretch
        RectTransform.anchorMin = new Vector2(0, 0);
        RectTransform.anchorMax = new Vector2(1, 1);

        // Reset offsets to zero
        RectTransform.offsetMin = new Vector2(0, 0);
        RectTransform.offsetMax = new Vector2(0, 0);
    }

    //back key in android device
    //back key danh cho android
    public virtual void BackKey()
    {

    }

    //Open canvas
    //mo canvas
    public Tween canvasGroupFadeTween;
    public Tween bgFadeTween;
    public virtual void Open()
    {
        if (canvasGroup == null) canvasGroup = this.gameObject.AddComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = this.gameObject.GetComponent<CanvasGroup>();
        gameObject.SetActive(true);
        canvasGroupFadeTween?.Kill();
        canvasGroup.alpha = 0;
        canvasGroupFadeTween = canvasGroup.DOFade(1, 0.08f).SetEase(Ease.OutSine);
        if (bgImg != null)
        {
            bgFadeTween?.Kill();
            bgImg.color = new Color(0, 0, 0, 0.5f);
            bgFadeTween = bgImg.DOColor(new Color(0, 0, 0, 0.99f), 0.2f).SetEase(Ease.OutSine);
        }
        /*if(addSoundToButtons == null) addSoundToButtons = this.gameObject.AddComponent<AddSoundToButtons>();
DOVirtual.DelayedCall(0.1f, () => {
  addSoundToButtons.AddSound();
});*/
    }

    //close canvas directly
    //dong truc tiep, ngay lap tuc
    public virtual void CloseDirectly()
    {
        UIManager.Ins.RemoveBackUI(this);
        canvasGroup.DOFade(0, 0.04f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
        if (IsDestroyOnClose)
        {
            Destroy(gameObject);
        }

    }

    //close canvas with delay time, used to anim UI action
    //dong canvas sau mot khoang thoi gian delay
    public virtual void Close(float delayTime)
    {
        Invoke(nameof(CloseDirectly), delayTime);
    }

}

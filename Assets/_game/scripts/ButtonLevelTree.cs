using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLevelTree : MonoBehaviour
{
    public int levelIndex;
    public TextMeshProUGUI levelText;
    public RectTransform rectTransform;
    public Image bubbleImg;
    public Image levelIconImg;
    public Image starImg;
    public Sprite starOffSprite;
    public Sprite starOnSprite;
    public Sprite lockSprite;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnInitt(int levelIndex)
    {
        this.levelIndex = levelIndex;
        levelText.text = (levelIndex+1).ToString();
        levelIconImg.sprite = LevelManager.Ins.GetLevelInfo(levelIndex).iconOff;
        levelIconImg.ResizeImgKeepHeight();
        starImg.transform.localScale = Vector3.one;
        //lockSprite = SpriteUtility.GetSpriteFromSolidColor(new Color(0.5f, 0.5f, 0.5f, 1f), LevelManager.Ins.GetLevelInfo(this.levelIndex).sprite.texture);
        lockSprite = LevelManager.Ins.GetLevelInfo(this.levelIndex).sprite;
    }

    public void OnFirstOpenHome(float delay)
    {
        bubbleImg.gameObject.SetActive(false);
        starImg.gameObject.SetActive(false);
        DOVirtual.DelayedCall(delay, () =>
        {
            RefreshColor(levelIndex);
            if (bubbleImg.gameObject.activeInHierarchy)
            {
                bubbleImg.transform.localScale = Vector3.one * 0.4f;
                bubbleImg.transform.DOScale(1f, 0.3f).SetEase(Ease.OutSine);
            }
            if (starImg.gameObject.activeInHierarchy)
            {
                starImg.transform.localScale = Vector3.one * 0.4f;
                starImg.transform.DOScale(1f, 0.3f).SetEase(Ease.OutSine);
            }
        });
    }

    public void RefreshColor(int levelIndex)
    {
        if (DataManager.Ins.playerData.maxLevelIndex > levelIndex)
        {
            OnPassed();
        }
        if (DataManager.Ins.playerData.maxLevelIndex == levelIndex)
        {
            OnCurrent();
        }
        if (DataManager.Ins.playerData.maxLevelIndex < levelIndex)
        {
            OnLock();
        }
    }

    [Button]
    public void ShowIcon(Action OnComplete)
    {
        /*levelIconImg.DOColor(Color.white, 0f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            OnComplete?.Invoke();
        });*/
        levelIconImg.sprite = LevelManager.Ins.GetLevelInfo(levelIndex).iconOn;
        DOVirtual.DelayedCall(0.01f, () =>
        {
            ShineIfPassed(() =>
            {
                OnComplete?.Invoke();
            });
        });
    }

    public void SetPos()
    {
        rectTransform.anchorMax = new Vector2(0.5f, 0);
        rectTransform.anchorMin = new Vector2(0.5f, 0);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = Vector2.up * LevelTree.SPACING * LevelTree.Ins.buttonLevelTrees.Count;
    }

    public void OnPassed()
    {
        //bubbleImg.color = Color.softYellow;
        starImg.sprite = starOnSprite;
        levelIconImg.sprite = LevelManager.Ins.GetLevelInfo(this.levelIndex).iconOn;
        starImg.gameObject.SetActive(true);
        bubbleImg.gameObject.SetActive(false);
        starImg.transform.localScale = Vector3.one;
    }

    public void OnCurrent()
    {
        //bubbleImg.color = Color.softYellow;
        starImg.sprite = starOnSprite;
        levelIconImg.sprite = LevelManager.Ins.GetLevelInfo(this.levelIndex).iconOff;
        starImg.gameObject.SetActive(false);
        bubbleImg.gameObject.SetActive(true);
        bubbleImg.transform.localScale = Vector3.one;
        LevelTree.Ins.currentButtonLevelTree = this;
    }

    public void OnLock()
    {
        //bubbleImg.color = Color.darkGray;
        starImg.sprite = starOffSprite;
        levelIconImg.sprite = LevelManager.Ins.GetLevelInfo(this.levelIndex).iconOff;
        starImg.gameObject.SetActive(true);
        bubbleImg.gameObject.SetActive(false);
        starImg.transform.localScale = Vector3.one;
    }

    [Button]
    public void OnOld(Action OnComplete)
    {
        bubbleImg.transform.SetAsFirstSibling();
        bubbleImg.gameObject.SetActive(true);
        starImg.gameObject.gameObject.SetActive(true);
        bubbleImg.transform.localScale = Vector3.one;
        bubbleImg.transform.DOScale(0.1f, 0f).SetEase(Ease.Linear);
        starImg.gameObject.transform.localScale = 0.1f * Vector3.one;
        starImg.gameObject.transform.DOScale(1f, 0f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            bubbleImg.gameObject.SetActive(false);
            OnComplete?.Invoke();
        });
    }

    [Button]
    public void OnOldSlow(Action OnComplete)
    {
        bubbleImg.transform.SetAsFirstSibling();
        bubbleImg.gameObject.SetActive(true);
        bubbleImg.transform.localScale = Vector3.one;
        bubbleImg.transform.DOScale(0.1f, 0.4f).SetEase(Ease.InBack).OnComplete(() =>
        {
            OnComplete?.Invoke();
            starImg.gameObject.gameObject.SetActive(true);
            starImg.gameObject.transform.localScale = 0.5f * Vector3.one;
            starImg.gameObject.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                bubbleImg.gameObject.SetActive(false);
            });
        });
    }

    [Button]
    public void OnNew(Action OnComplete)
    {
        LevelTree.Ins.isDoingAnim = true;
        starImg.gameObject.transform.SetAsFirstSibling();
        starImg.gameObject.gameObject.SetActive(true);
        bubbleImg.gameObject.SetActive(true);
        starImg.gameObject.transform.localScale = Vector3.one * 0.01f;
        bubbleImg.transform.localScale = 0.1f * Vector3.one;
        ShineIfPassed(null);
        bubbleImg.transform.DOScale(1f, 0.3f).SetEase(Ease.OutSine).OnComplete(() =>
        {
            starImg.gameObject.gameObject.SetActive(false);
            LevelTree.Ins.isDoingAnim = false;
            OnComplete?.Invoke();
        });
        LevelTree.Ins.currentButtonLevelTree = this;
    }

    [Button]
    public void OnNewSlow(Action OnComplete)
    {
        BlockUI.Ins.Block();
        LevelTree.Ins.isDoingAnim = true;
        starImg.gameObject.transform.SetAsFirstSibling();
        starImg.gameObject.gameObject.SetActive(true);
        bubbleImg.transform.localScale = 0.5f * Vector3.one;
        bubbleImg.gameObject.SetActive(false);
        starImg.gameObject.transform.localScale = Vector3.one;
        starImg.sprite = starOnSprite;
        starImg.gameObject.transform.DOScale(0.1f, 0.3f).SetDelay(0.2f).SetEase(Ease.InBack).OnComplete(() =>
        {
            bubbleImg.gameObject.SetActive(true);
            DOVirtual.DelayedCall(0.2f, () =>
            {
                ShineIfPassed(null);
            });
            bubbleImg.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                starImg.gameObject.gameObject.SetActive(false);
                LevelTree.Ins.isDoingAnim = false;
                OnComplete?.Invoke();
            });
            BlockUI.Ins.UnBlock();
            LevelTree.Ins.ExpandSpacing(true);
        });
        LevelTree.Ins.currentButtonLevelTree = this;
    }

    [Button]
    public void ShineIfPassed(Action OnComplete)
    {
        if (DataManager.Ins.playerData.maxLevelIndex < levelIndex) return;
    }

}

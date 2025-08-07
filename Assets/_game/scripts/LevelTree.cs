using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Properties;
using UnityEngine;
using UnityEngine.UI;

public class LevelTree : Singleton<LevelTree>   
{
    public List<ButtonLevelTree> buttonLevelTrees = new List<ButtonLevelTree>();
    public Transform buttonLevelTreeParent;
    public Image backLineImg;
    public Image frontLineImg;
    public static float SPACING = 340f;
    public static float EXPAND_SPACING = 60f;
    public static bool isWin;
    public RectTransform contentRectTF;
    public ButtonLevelTree currentButtonLevelTree;
    public bool isDoingAnim;
    public bool isFirstOpened;
    public CanvasGroup lineCanvasGroup;


    [Button]
    public void OnOpenHome(Action OnComplete)
    {
        ResetContentHeight();
        InitButtonLevelTree();
        contentRectTF.sizeDelta = new Vector2(contentRectTF.sizeDelta.x, 0);
        backLineImg.rectTransform.sizeDelta = new Vector2(backLineImg.rectTransform.sizeDelta.x, SPACING * (buttonLevelTrees.Count - 1));
        backLineImg.rectTransform.anchoredPosition = new Vector2(0, 370f);
        if (DataManager.Ins.playerData.maxLevelIndex < 10)
        {
            backLineImg.rectTransform.anchoredPosition -= new Vector2(0, SPACING * DataManager.Ins.playerData.maxLevelIndex);
            frontLineImg.rectTransform.sizeDelta = new Vector2(frontLineImg.rectTransform.sizeDelta.x, SPACING * DataManager.Ins.playerData.maxLevelIndex);
        }
        else
        {
            backLineImg.rectTransform.anchoredPosition -= new Vector2(0, SPACING * 10);
            frontLineImg.rectTransform.sizeDelta = new Vector2(frontLineImg.rectTransform.sizeDelta.x, SPACING * 10);
        }
            if (isWin)
            {
                RefreshButtonLevelsColor(true);
                RunAnim(() =>
                {
                    SetContentHeight();
                    RefreshButtonLevelsColor(false);
                    ButtonLevelTree buttonLevelTree = buttonLevelTrees.FirstOrDefault(x => x.levelIndex == DataManager.Ins.playerData.maxLevelIndex);
                    buttonLevelTree.OnNewSlow(null);
                    isWin = false;
                    OnComplete?.Invoke();
                });
                UIManager.Ins.GetUI<Home>().buttonPlayText.text = "Level " + (DataManager.Ins.playerData.maxLevelIndex + 1).ToString();
                //UIManager.Ins.GetUI<Home>().buttonPlayText.text = "Play";
            }
            else
            {
                OnComplete?.Invoke();
                SetContentHeight();
                RefreshButtonLevelsColor(false);
                UIManager.Ins.GetUI<Home>().buttonPlayText.text = "Level " + (DataManager.Ins.playerData.maxLevelIndex + 1).ToString();
                //UIManager.Ins.GetUI<Home>().buttonPlayText.text = "Play";
            }
        ExpandSpacing(false);
    }

    /*public void OnFirstOpen()
    {
        DOVirtual.DelayedCall(Time.deltaTime * 2, () =>
        {
            float delay = 2.2f;
            foreach (var buttonLevelTree in buttonLevelTrees)
            {
                buttonLevelTree.OnFirstOpenHome(delay);
                buttonLevelTree.transform.SetParent(contentRectTF.transform);
            }
            lineCanvasGroup.alpha = 0f;
            lineCanvasGroup.DOFade(1f, 0.2f).SetDelay(delay + 0.2f).SetEase(Ease.OutSine).OnComplete(() =>
            {
                Debug.Log(234);
                foreach (var buttonLevelTree in buttonLevelTrees)
                {
                    buttonLevelTree.transform.SetParent(buttonLevelTreeParent);
                }
            });
            lineCanvasGroup.transform.localScale = new Vector3(0, 1, 1);
            lineCanvasGroup.transform.DOScaleX(1f, 0.2f).SetDelay(delay + 0.2f).SetEase(Ease.OutSine).OnComplete(() =>
            {
            });
        });
    }*/

    [Button]
    public void InitButtonLevelTree()
    {
        buttonLevelTrees.Clear();
        if (DataManager.Ins.playerData.maxLevelIndex < 10)
        {
            for (int i = 0; i < LevelManager.Ins.levelWrapperrr.levels.Count; i++)
            {
                ButtonLevelTree buttonLevelTree = PoolManager.Ins.Spawn<ButtonLevelTree>(PoolType.ButtonLevelTree);
                buttonLevelTree.OnInitt(i);
                buttonLevelTree.transform.SetParent(buttonLevelTreeParent);
                buttonLevelTree.transform.localScale = Vector3.one;
                buttonLevelTree.SetPos();
                buttonLevelTrees.Add(buttonLevelTree);
            }
        }
        if (DataManager.Ins.playerData.maxLevelIndex >= 10)
        {
            for (int i = 0; i < 20; i++)
            {
                ButtonLevelTree buttonLevelTree = PoolManager.Ins.Spawn<ButtonLevelTree>(PoolType.ButtonLevelTree);
                buttonLevelTree.OnInitt(DataManager.Ins.playerData.maxLevelIndex - 10 + i);
                buttonLevelTree.transform.SetParent(buttonLevelTreeParent);
                buttonLevelTree.transform.localScale = Vector3.one;
                buttonLevelTree.SetPos();
                buttonLevelTrees.Add(buttonLevelTree);
            }
        }
    }

    public void RefreshButtonLevelsColor(bool isAnim)
    {
        for (int i = 0; i < buttonLevelTrees.Count; i++)
        {
            ButtonLevelTree buttonLevelTree = buttonLevelTrees[i];
            if (isAnim)
            {
                buttonLevelTree.RefreshColor(buttonLevelTree.levelIndex+1);
            }
            else
            {
                buttonLevelTree.RefreshColor(buttonLevelTree.levelIndex);
            }
        }
    }

    public void RunAnim(Action OnCOmplete)
    {
        isDoingAnim = true;
        BlockUI.Ins.Block();
        backLineImg.rectTransform.anchoredPosition += new Vector2(0, SPACING);
        frontLineImg.rectTransform.sizeDelta = new Vector2(frontLineImg.rectTransform.sizeDelta.x, frontLineImg.rectTransform.sizeDelta.y - SPACING);
        ButtonLevelTree buttonLevelTree = buttonLevelTrees.FirstOrDefault(x => x.levelIndex == DataManager.Ins.playerData.maxLevelIndex - 1);
        float delayShowIcon = 0f;
        DOVirtual.DelayedCall(delayShowIcon, () =>
        {
            buttonLevelTree.ShowIcon(() =>
            {
                DOVirtual.DelayedCall(0.1f, () =>
                {
                    ResetSpacing();
                    buttonLevelTree.OnOldSlow(() =>
                    {
                        float duration = 0.6f;
                        float delayMoveLine = 0.3f;
                        backLineImg.rectTransform.DOAnchorPosY(backLineImg.rectTransform.anchoredPosition.y - SPACING, duration).SetDelay(delayMoveLine).SetEase(Ease.OutSine).OnComplete(() =>
                        {
                            BlockUI.Ins.UnBlock();
                            isDoingAnim = false;
                            OnCOmplete?.Invoke();
                        });
                        DOVirtual.Float(frontLineImg.rectTransform.sizeDelta.y, frontLineImg.rectTransform.sizeDelta.y + SPACING, duration, v =>
                        {
                            frontLineImg.rectTransform.sizeDelta = new Vector2(frontLineImg.rectTransform.sizeDelta.x, v);
                        }).SetDelay(delayMoveLine).SetEase(Ease.OutSine);
                    });
                });
            });
        });

    }

    public void ResetContentHeight()
    {
        contentRectTF.anchoredPosition = Vector2.zero;
        backLineImg.rectTransform.transform.SetParent(contentRectTF.parent);
        backLineImg.rectTransform.pivot = new Vector2(0.5f, 0);
        backLineImg.rectTransform.anchorMin = new Vector2(0.5f, 0);
        backLineImg.rectTransform.anchorMax = new Vector2(0.5f, 0);
        contentRectTF.anchorMax = new Vector2(0.5f, 0);
        contentRectTF.anchorMin = new Vector2(0.5f, 0);
        contentRectTF.pivot = new Vector2(0.5f, 0);
        contentRectTF.sizeDelta = new Vector2(contentRectTF.sizeDelta.x, 0f);
        backLineImg.rectTransform.transform.SetParent(contentRectTF.transform);
    }

    public void SetContentHeight()
    {
        StartCoroutine(IESet());
        IEnumerator IESet()
        {
            backLineImg.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            backLineImg.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            backLineImg.rectTransform.transform.SetParent(contentRectTF.parent);
            contentRectTF.anchorMax = new Vector2(0.5f, 0.5f);
            contentRectTF.anchorMin = new Vector2(0.5f, 0.5f);
            contentRectTF.pivot = new Vector2(0.5f, 0.5f);
            //contentRectTF.sizeDelta = new Vector2(contentRectTF.sizeDelta.x, 3100f);
            contentRectTF.sizeDelta = new Vector2(contentRectTF.sizeDelta.x, 0f);
            backLineImg.rectTransform.transform.SetParent(contentRectTF.transform);
            yield return new WaitForSeconds(0);
        }
    }

    public void ExpandSpacing(bool isAnim)
    {
        ButtonLevelTree currrentButtonLevelTree = buttonLevelTrees.FirstOrDefault(x => x.bubbleImg.gameObject.activeInHierarchy);
        int index = buttonLevelTrees.IndexOf(currrentButtonLevelTree);
        float duration = isAnim ? 0.39f : 0.01f;
        for (int i = 0; i < index; i++)
        {
            ButtonLevelTree belowButtonLevelTree = buttonLevelTrees[i];
            Vector2 targetPos = belowButtonLevelTree.rectTransform.anchoredPosition - new Vector2(0, EXPAND_SPACING);
            belowButtonLevelTree.rectTransform.DOAnchorPos(targetPos, duration).SetEase(Ease.OutBack);
            Debug.Log("below");
        }
        for (int i = index + 1; i < buttonLevelTrees.Count; i++)
        {
            ButtonLevelTree aboveButtonLevelTree = buttonLevelTrees[i];
            Vector2 targetPos = aboveButtonLevelTree.rectTransform.anchoredPosition + new Vector2(0, EXPAND_SPACING);
            aboveButtonLevelTree.rectTransform.DOAnchorPos(targetPos, duration).SetEase(Ease.OutBack);
            Debug.Log("above");
        }
    }

    public void ResetSpacing()
    {
        ButtonLevelTree currrentButtonLevelTree = buttonLevelTrees.FirstOrDefault(x => x.bubbleImg.gameObject.activeInHierarchy);
        int index = buttonLevelTrees.IndexOf(currrentButtonLevelTree);
        DOVirtual.DelayedCall(0, () =>
        {
            for (int i = 0; i < index; i++)
            {
                ButtonLevelTree belowButtonLevelTree = buttonLevelTrees[i];
                Vector2 targetPos = belowButtonLevelTree.rectTransform.anchoredPosition + new Vector2(0, EXPAND_SPACING);
                belowButtonLevelTree.rectTransform.DOAnchorPos(targetPos, 0.39f).SetEase(Ease.InBack);
                Debug.Log("below");
            }
            for (int i = index + 1; i < buttonLevelTrees.Count; i++)
            {
                ButtonLevelTree aboveButtonLevelTree = buttonLevelTrees[i];
                Vector2 targetPos = aboveButtonLevelTree.rectTransform.anchoredPosition - new Vector2(0, EXPAND_SPACING);
                aboveButtonLevelTree.rectTransform.DOAnchorPos(targetPos, 0.39f).SetEase(Ease.InBack);
                Debug.Log("above");
            }
        });
    }
}

using UnityEngine;
using UnityEngine.UI;

public static class ImageUtility
{
    public static void ResizeImgKeepWidth(this Image img)
    {
        if (img == null || img.rectTransform == null)
        {
            Debug.LogError("Image or RectTransform is null.");
            return;
        }

        float originalWidth = img.rectTransform.sizeDelta.x;
        img.SetNativeSize();

        // Check if width is non-zero to avoid division by zero
        if (img.rectTransform.sizeDelta.x != 0)
        {
            float heightOnWidth = img.rectTransform.sizeDelta.y / img.rectTransform.sizeDelta.x;
            img.rectTransform.sizeDelta = new Vector2(originalWidth, originalWidth * heightOnWidth);
        }
        else
        {
            Debug.LogWarning("Original width is zero; cannot maintain aspect ratio.");
        }
    }

    public static void ResizeImgKeepHeight(this Image img)
    {
        if (img == null || img.rectTransform == null)
        {
            Debug.LogError("Image or RectTransform is null.");
            return;
        }

        float originalHeight = img.rectTransform.sizeDelta.y;
        img.SetNativeSize();

        // Check if heightOnWidth is non-zero to avoid division by zero
        if (img.rectTransform.sizeDelta.x != 0)
        {
            float heightOnWidth = img.rectTransform.sizeDelta.y / img.rectTransform.sizeDelta.x;
            img.rectTransform.sizeDelta = new Vector2(originalHeight / heightOnWidth, originalHeight);
        }
        else
        {
            Debug.LogWarning("Original width is zero; cannot maintain aspect ratio.");
        }
    }

    public static void SetLeft(this RectTransform rt, float left)
    {
        rt.offsetMin = new Vector2(left, rt.offsetMin.y);
    }

    public static void SetRight(this RectTransform rt, float right)
    {
        rt.offsetMax = new Vector2(-right, rt.offsetMax.y);
    }

    public static void SetTop(this RectTransform rt, float top)
    {
        rt.offsetMax = new Vector2(rt.offsetMax.x, -top);
    }

    public static void SetBottom(this RectTransform rt, float bottom)
    {
        rt.offsetMin = new Vector2(rt.offsetMin.x, bottom);
    }
}

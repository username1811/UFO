using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AddButtonEffectsToButtons : MonoBehaviour
{
    void Start()
    {
        Add();
    }

    public void Add()
    {
        DOVirtual.DelayedCall(0.3f, () =>
        {
            Button[] buttons = GetComponentsInChildren<Button>(true);
            foreach (Button button in buttons)
            {
                button.onClick.AddListener(() =>
                {
                    SoundManager.Ins.PlaySFX(SFXType.UIClick);
                    Vibrator.VibrateDevice(VibrateType.Fast);
                });
                if (button.GetComponent<NoButtonScaleEffect>() == null)
                    button.AddComponent<ButtonScaleEffect>();
            }
        });
    }
}

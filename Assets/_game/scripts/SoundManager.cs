using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SoundManager;

public class SoundManager : Singleton<SoundManager>
{
    public AudioSource bgAudioSource;
    public SFXData[] sfxDatas;
    public static AudioConfiguration audioConfiguration;


    [System.Serializable]
    public class SFXData
    {
        public SFXType sfxType;
        public AudioSource audioSource;
        public AudioClip audioClip;
    }

    IEnumerator Start()
    {
        InitAudioSources();
        yield return new WaitUntil(()=>DataManager.Ins.isLoaded);
        OnInit();
    }

    public void InitAudioSources()
    {
        foreach (var sfxData in sfxDatas)
        {
            GameObject audioSourceObj = new GameObject(sfxData.sfxType.ToString());
            audioSourceObj.AddComponent<AudioSource>();
            audioSourceObj.transform.SetParent(this.transform);
            sfxData.audioSource = audioSourceObj.GetComponent<AudioSource>();
        }
    }

    public void OnInit()
    {
        TurnMusic(DataManager.Ins.playerData.isMusicOn);
    }

    public void TurnMusic(bool isOn)
    {
        float value = isOn ? 0.5f : 0f;
        bgAudioSource.volume = value;
    }

    public void PlayBackgroundMusic()
    {
        bgAudioSource.Play();
    }

    public void StopBackgroundMusic()
    {
        bgAudioSource.Stop();
    }

    public void PlaySFX(SFXType sfxType)
    {
        if (!DataManager.Ins.playerData.isSoundOn) { return; }
        SFXData sfxData = sfxDatas.FirstOrDefault(x => x.sfxType == sfxType);
        if (sfxType == SFXType.Coin || sfxType == SFXType.ClaimItem || sfxType == SFXType.ClaimItem2
            || sfxType == SFXType.ClaimItem3 || sfxType == SFXType.ClaimItemHidden)
        {
            sfxData.audioSource.volume = 0.3f;
        }
        if (sfxData.audioSource.isPlaying)
        {
            sfxData.audioSource.Stop();
        }
        sfxData.audioSource.PlayOneShot(sfxData.audioClip);
    }

    public void StopSFX(SFXType sfxType)
    {
        if (!DataManager.Ins.playerData.isSoundOn) { return; }
        SFXData sfxData = sfxDatas.FirstOrDefault(x => x.sfxType == sfxType);
        sfxData.audioSource.Stop();
    }
}

public enum SFXType
{
    BoosterHint, BoosterTime, Coin, Fail, FinishItem, Firework, ClaimItem, Lose, 
    PointClick1, PointClick2, PointClick3, PointClick4, PointClick5, PointClick6, PointClick7, PointClick8,
    StartGame, Win, UIClick, ClaimItem2, ClaimItem3, ClaimReward, ClaimBooster, SpinTick, 
    ClaimItemHidden, StarTwinkle, 
}

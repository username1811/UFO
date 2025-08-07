using UnityEngine;
using System.Collections;
using MoreMountains.NiceVibrations;

public static class Vibrator
{

#if UNITY_ANDROID && !UNITY_EDITOR
    public static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    public static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    public static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#else
    public static AndroidJavaClass unityPlayer;
    public static AndroidJavaObject currentActivity;
    public static AndroidJavaObject vibrator;
#endif

    public static void VibrateDevice(VibrateType vibrateType)
    {
        if (!DataManager.Ins.playerData.isVibrateOn) return;

        if (vibrateType == VibrateType.Slow)
        {
            if (IsAndroid())
            {
                long[] pattern = { 0, 50, 50 };
                Vibrate(pattern, -1);
            }
            else
            {
                MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
            }
        }
        if (vibrateType == VibrateType.Fast)
        {
            if (IsAndroid())
            {
                long[] pattern = { 0, 10, 10 };
                Vibrate(pattern, -1);
            }
            else
            {
                MMVibrationManager.Haptic(HapticTypes.MediumImpact);
            }
        }
    }

    public static void Vibrate(long[] pattern, int repeat)
    {
        if (!DataManager.Ins.playerData.isVibrateOn) return;
        vibrator.Call("vibrate", pattern, repeat);
    }

    private static bool IsAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
	    return true;
#else
        return false;
#endif
    }
}

public enum VibrateType
{
    Fast, Slow
}
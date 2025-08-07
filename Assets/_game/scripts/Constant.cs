using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constant
{
    public const string TAG_DOT = "dot";
    public const string TAG_DUST = "dust";
    public static readonly DateTime ORIGINAL_TIME = new DateTime(2024, 1, 1);
    public static string FormatTimeCounter(int seconds)
    {
        var span = TimeSpan.FromSeconds(seconds); //Or TimeSpan.FromSeconds(seconds); (see Jakob C´s answer)
        if (seconds < 60)
            return seconds.ToString("00");
        else if (seconds < 3600)
            return string.Format("{0:D2}:{1:D2}", span.Minutes, span.Seconds);
        else if (seconds < 3600 * 24)
            return string.Format("{0:D2}:{1:D2}:{2:D2}", (int)span.TotalHours, span.Minutes, span.Seconds);

        else
            return string.Format("{0:D2}d:{1:D2}:{2:D2}", (int)span.Days, span.Hours, span.Minutes);

    }
    public static Color COLOR_NOT_ENOUGH_GOLD = new Color(1f,0.77f,0.54f);
}

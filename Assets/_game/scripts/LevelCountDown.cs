using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCountDown : Singleton<LevelCountDown>
{
    public string countDownStr;
    public int remainingTime; // Thời gian còn lại (giây)
    public bool isCountingDown; // Trạng thái đếm ngược
    public Action OnStopCountDown = () => { };
    //public bool isOnRemoteConfig;
    public bool isActive => /*isOnRemoteConfig && */IsLevelExpected();
    public int levelSmallDurationRemoteConfig;
    public int levelLargeDurationRemoteConfig;

    private void Start()
    {
        countDownStr = "";
    }

    // Khởi tạo khi tải level
    public void OnLoadLevel()
    {
        if (!isActive) return;
        OnInitt();
    }

    // Khởi tạo đếm ngược
    public void OnInitt()
    {
        if (!isActive) return;
        int sec = GetSec();
        remainingTime = sec;
        countDownStr = SecToString(sec);
        isCountingDown = false; // Mặc định không chạy đếm ngược
    }

    public int GetSec()
    {
        float timePerStar = 5f;
        float totalSeconds = timePerStar * 60;
        return (int)Math.Ceiling(totalSeconds / 60.0f) * 60;
    }

    // Bắt đầu đếm ngược
    public void StartCountDown()
    {
        if (!isActive) return;
        /*if (UIManager.Ins.IsOpened<Settings>()) return;
        if (UIManager.Ins.IsOpened<RemoveAdsBundle>()) return;*/
        if (!isCountingDown) // Kiểm tra nếu chưa chạy đếm ngược
        {
            isCountingDown = true;
            StartCoroutine(CountDownCoroutine());
        }
    }

    // Dừng đếm ngược
    public void StopCountDown()
    {
        if (!isActive) return;
        isCountingDown = false; // Ngừng đếm ngược
        StopAllCoroutines(); // Dừng tất cả các coroutine đang chạy
    }

    // Coroutine đếm ngược
    private IEnumerator CountDownCoroutine()
    {
        countDownStr = SecToString(remainingTime); // Cập nhật chuỗi đếm ngược
        while (isCountingDown && remainingTime > 0)
        {
            yield return new WaitForSeconds(1); // Đợi 1 giây
            remainingTime--; // Giảm thời gian còn lại
            if(remainingTime == 20)
            {
                UIManager.Ins.GetUI<GamePlay>().RemindUsingBooster(BoosterType.Time);
            }
            countDownStr = SecToString(remainingTime); // Cập nhật chuỗi đếm ngược
        }
        if (LevelManager.Ins.isEndLevel) yield break;
        if (remainingTime <= 0)
        {
            isCountingDown = false;
            // Thực hiện hành động khi đếm ngược kết thúc (nếu cần)
            LevelManager.Ins.Lose(LoseType.OutOfTime);
            Debug.Log("Countdown Finished!");
        }
    }

    public void AddSeconds(int sec)
    {
        if (!isActive) return;
        remainingTime += sec;
    }

    public bool IsLevelExpected()
    {
        return LevelManager.Ins.currentLevelInfooo != null && (DataManager.Ins.playerData.currentLevelIndex >= 3);
    }

    // Chuyển giây thành định dạng mm:ss
    public string SecToString(int sec)
    {
        int minutes = sec / 60;
        int seconds = sec % 60;
        return $"{minutes:D2}:{seconds:D2}";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [SerializeField] private TMP_Text startCounter;
    [SerializeField] private TMP_Text lapTimerText;
    [SerializeField] private TMP_Text bestTimeText;

    [SerializeField] private TMP_Text carSpeedText;

    [SerializeField] private TMP_Text currentLapText;
    [SerializeField] private TMP_Text maxLapText;

    [SerializeField] private TMP_Text lapAllTimesText;

    [SerializeField] private GameObject startGameUI;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject endGameUI;
    [SerializeField] private GameObject pauseScreenUI;

    [SerializeField] private RectTransform rectTransform;

    private void Awake()
    {
        instance = this;
    }
    /// <summary>
    /// Formats time to the correct minute / second format
    /// </summary>
    /// <param name="time">The seconds that have passed</param>
    /// <returns>The formatted time</returns>
    private string FormatTime(float time)
    {
        int intTime = (int)time;
        int minutes = intTime / 60;
        int seconds = intTime - (minutes * 60);
        string timeText = string.Format("{0:00}:{1:00}", minutes, seconds);
        return timeText;
    }
    /// <summary>
    /// Update count down text
    /// </summary>
    /// <param name="text">countdown text</param>
    public void UpdateStartCounter(string text)
    {
        startCounter.text = text;
    }

    /// <summary>
    /// Update lap time
    /// </summary>
    /// <param name="time">laptime</param>
    public void UpdateLapTimer(float time)
    {
        lapTimerText.text = FormatTime(time);
    }
    /// <summary>
    /// Updates the amount of laps passed
    /// </summary>
    /// <param name="currentLap">Current amount of laps passed</param>
    /// <param name="maxLaps">The max amount of laps to finish.</param>
    public void UpdateLapCount(string currentLap, string maxLaps)
    {
        currentLapText.text = currentLap;
        maxLapText.text = maxLaps;
    }

    /// <summary>
    /// Updates the speed meter
    /// </summary>
    /// <param name="carSpeed"></param>
    public void UpdateCarSpeed(float carSpeed)
    {
        carSpeedText.text = Mathf.RoundToInt(carSpeed).ToString() + " KM/H";
        rectTransform.rotation = Quaternion.Euler(0,0,Mathf.Lerp(115, -112, Mathf.InverseLerp(0, 180, carSpeed)));
    }

    /// <summary>
    /// Updates the laptime list
    /// </summary>
    /// <param name="lapTimes">The times from the all the laps you drove</param>
    /// <param name="bestTime">Best time on any lap you drove</param>
    public void UpdateLapTimeMenu(List<float> lapTimes, float bestTime)
    {
        lapAllTimesText.text = null;
        for (int i = 0; i < lapTimes.Count; i++)
        {
            string timeText = (i + 1) + " " + FormatTime(lapTimes[i]) + "\n";
            lapAllTimesText.text += timeText;
        }
        bestTimeText.text = FormatTime(bestTime);
    }

    /// <summary>
    /// Check if pausescreen is active
    /// </summary>
    public bool IfPauseScreen()
    {
        if (pauseScreenUI.activeSelf == true)
            return true;
        else
            return false;
    }
    /// <summary>
    /// Pause screen ingame
    /// </summary>
    public void PauseScreen()
    {
        if (endGameUI.activeSelf == false)
        {
            if (pauseScreenUI.activeSelf == false)
            {
                startGameUI.SetActive(false);
                gameUI.SetActive(false);
                pauseScreenUI.SetActive(true);
                Time.timeScale = 0f;
            }
            else
            {
                startGameUI.SetActive(true);
                gameUI.SetActive(true);
                pauseScreenUI.SetActive(false);
                Time.timeScale = 1f;
            }
        }
    }
    /// <summary>
    /// Disable game ui and enable end ui
    /// </summary>
    /// <param name="lapTimes">The times from the all the laps you drove</param>
    /// <param name="bestTime">Best time on any lap you drove</param>
    public void EndGameUI(List<float> lapTimes, float bestTime)
    {
        startGameUI.SetActive(false);
        gameUI.SetActive(false);
        endGameUI.SetActive(true);
        UpdateLapTimeMenu(lapTimes, bestTime);
    }
}

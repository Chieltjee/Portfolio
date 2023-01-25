using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LapManager : MonoBehaviour
{
    [SerializeField] private float lapTimer;
    public bool activeTimer;
    [SerializeField] private List<float> lapTimes;
    [SerializeField] private float bestTime = 2f;

    public GameObject[] checkPoints;
    public int currentCheckPoint;
    [SerializeField] private int maxLaps;

    private void Start()
    {
        //If playerprefs has the value
        if (PlayerPrefs.HasKey("bestTime"))
        {
            bestTime = PlayerPrefs.GetFloat("bestTime");
        }
        UIManager.instance.UpdateLapCount((lapTimes.Count + 1).ToString(), maxLaps.ToString());
    }
    private void Update()
    {
        //Timer for lap time
        if (activeTimer)
        {
            lapTimer += Time.deltaTime;
            UIManager.instance.UpdateLapTimer(lapTimer);
        }
    }
    /// <summary>
    /// Clears lap stats for a new race
    /// </summary>
    public void ClearStats()
    {
        lapTimer = 0f;
        lapTimes.Clear();
    }

    /// <summary>
    /// Starts a new lap
    /// </summary>
    public void NewLap()
    {
        if (currentCheckPoint == checkPoints.Length)
        {
            //Not the max amount of laps yet
            if (lapTimes.Count < maxLaps)
            {
                //If the current laptime is lower then the best time
                if (lapTimer < bestTime)
                    bestTime = lapTimer;
                //Save the best time in playerprefes
                PlayerPrefs.SetFloat("bestTime", bestTime);
                PlayerPrefs.Save();
                //Add time to laptimes and start new timer
                lapTimes.Add(lapTimer);
                lapTimer = 0f;
                UIManager.instance.UpdateLapTimeMenu(lapTimes, bestTime);
                //Reset checkpoints
                for (int i = 0; i < checkPoints.Length; i++)
                {
                    checkPoints[i].SetActive(false);
                }
                checkPoints[0].SetActive(true);
                currentCheckPoint = 0;
                //If the max amount of laps now
                if (lapTimes.Count == maxLaps)
                {
                    //End game
                    StartCoroutine(GameManager.instance.EndGame(lapTimes, bestTime));
                    for (int i = 0; i < 5; i++)
                    {
                        PlayerPrefs.DeleteKey("laptime" + i);
                    }
                    for (int i = 0; i < lapTimes.Count; i++)
                    {
                        string lapText = "laptime" + i;
                        PlayerPrefs.SetFloat(lapText, lapTimes[i]);
                    }
                    PlayerPrefs.Save();
                }
                if (lapTimes.Count != maxLaps)
                    UIManager.instance.UpdateLapCount((lapTimes.Count + 1).ToString(), maxLaps.ToString());
            }
        }
    }

    /// <summary>
    /// Changes the active checkpoint to the next one.
    /// </summary>
    public void NextCheckPoint()
    {
        //Loop checkpoints
        for (int i = 0; i < checkPoints.Length; i++)
        {
            //If checkpoint active
            if (checkPoints[i].activeSelf == true)
            {
                //Disable checkpoint
                checkPoints[i].gameObject.SetActive(false);
                currentCheckPoint = i + 1;
                
                //If the checkpoint isnt the last one
                if (i + 1 < checkPoints.Length)
                {
                    checkPoints[i + 1].gameObject.SetActive(true);
                }
                return;
            }
        }
    }
}

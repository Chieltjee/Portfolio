using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private TMP_InputField pushupField;
    [SerializeField] private TMP_InputField dailyPushupGoalField;
    public int pushups;
    public int dailyPushupGoal;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        PlayFabCode.instance.GetLeaderBoard();
        PlayFabCode.instance.GetStatistics();
        UIManager.instance.UpdatePushupCounter();
    }
    public void AddPushups()
    {
        pushups += int.Parse(pushupField.text);
        UIManager.instance.UpdatePushupCounter();
        pushupField.text = "";
        PlayFabCode.instance.PushToDatabase();
    }

    public void SetDailyPushupGoal()
    {
        dailyPushupGoal = int.Parse(pushupField.text);
        UIManager.instance.UpdatePushupCounter();
        dailyPushupGoalField.text = "";
        PlayFabCode.instance.PushToDatabase();
    }

    public void ReloadLeaderboard()
    {
        PlayFabCode.instance.SendLeaderBoard();
        PlayFabCode.instance.GetLeaderBoard();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

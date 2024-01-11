using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using TMPro;

public class PlayFabCode : MonoBehaviour
{
    public static PlayFabCode instance;
    public TMP_InputField Username, Email, Password;
    public TMP_Text ErrorMessage;
    public string SceneName = "";

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        ErrorMessage.text = "";
    }

    public void RegisterClick()
    {
        var register = new RegisterPlayFabUserRequest
        { Username = Username.text, Email = Email.text, Password = Password.text };
        PlayFabClientAPI.RegisterPlayFabUser(register, OnRegisterSuccess, OnRegisterFailure);
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        ErrorMessage.text = "";
        Debug.Log("Registered succesfully");
        GetStatistics();
        UIManager.instance.LoadScene();
    }

    private void OnRegisterFailure(PlayFabError error)
    {
        if (error.ErrorDetails != null && error.ErrorDetails.Count > 0)
        {
            using (var iter = error.ErrorDetails.Keys.GetEnumerator())
            {
                iter.MoveNext();
                string key = iter.Current;
                ErrorMessage.text = error.ErrorDetails[key][0];
                Debug.Log("Registering failed");
            }
        }
        else
        {
            ErrorMessage.text = error.ErrorMessage;
        }
    }

    public void LoginClick()
    {
        var login = new LoginWithPlayFabRequest
        { Username = Username.text, Password = Password.text };
        PlayFabClientAPI.LoginWithPlayFab(login, OnLoginSuccess, OnLoginFailure);
    }

    private void OnLoginSuccess(LoginResult result)
    {
        ErrorMessage.text = "";
        Debug.Log("Logged in succesfully");
        GetStatistics();
        UIManager.instance.LoadScene();
    }

    private void OnLoginFailure(PlayFabError error)
    {
        if (error.ErrorDetails != null && error.ErrorDetails.Count > 0)
        {
            using (var iter = error.ErrorDetails.Keys.GetEnumerator())
            {
                iter.MoveNext();
                string key = iter.Current;
                ErrorMessage.text = error.ErrorDetails[key][0];
                Debug.Log("Logging in failed");
            }
        }
        else
        {
            ErrorMessage.text = error.ErrorMessage;
        }
    }

    public void PushToDatabase()
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate { StatisticName = "pushups", Value = GameManager.instance.pushups},
                new StatisticUpdate { StatisticName = "dailypushupgoal", Value = GameManager.instance.dailyPushupGoal },
            }
        },
        result => { Debug.Log("User statistics updated"); },
        error => { Debug.LogError(error.GenerateErrorReport()); });
    }

    public void GetStatistics()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStatistics,
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }

    private void OnGetStatistics(GetPlayerStatisticsResult result)
    {
        Debug.Log("Received the following Statistics:");
        foreach (var eachStat in result.Statistics)
        {
            Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);
            if (eachStat.StatisticName == "pushups")
                GameManager.instance.pushups = eachStat.Value;
            if (eachStat.StatisticName == "dailypushupgoal")
                GameManager.instance.dailyPushupGoal = eachStat.Value;
        }
        UIManager.instance.UpdatePushupCounter();
    }

    public void SendLeaderBoard()
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "pushups",
                    Value = GameManager.instance.pushups
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnLoginFailure);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Succesfully updated leaderboard");
    }

    public void GetLeaderBoard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "pushups",
            StartPosition = 0,
            MaxResultsCount = 10
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnLoginFailure);
    }

    public void OnLeaderboardGet(GetLeaderboardResult result)
    {
        UIManager.instance.pushupLeaderboard.text = null;
        foreach (var item in result.Leaderboard)
        {
            UIManager.instance.pushupLeaderboard.text += (item.Position + 1) + " " + item.PlayFabId + " " + item.StatValue + "\n";
            if (item.StatValue == GameManager.instance.pushups)
                UIManager.instance.playfabid.text = item.PlayFabId;
        }
        Debug.Log("Succesfully got leaderboard");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    [SerializeField] private TMP_Text pushupCounter;
    [SerializeField] private TMP_Text DailyPushupGoal;
    public TMP_Text pushupLeaderboard;
    public TMP_Text playfabid;

    private void Awake()
    {
        instance = this;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void UpdatePushupCounter()
    {
        pushupCounter.text = GameManager.instance.pushups.ToString();
    }

    public void UpdateDailyPushupGoal()
    {
        DailyPushupGoal.text = GameManager.instance.dailyPushupGoal.ToString();
    }
}

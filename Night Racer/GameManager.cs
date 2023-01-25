using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private LapManager lapManager;
    private OldCarController carController;

    [SerializeField] private UnityEvent onStartGame;
    public UnityEvent OnStartGame => onStartGame;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        lapManager = FindObjectOfType<LapManager>();
        carController = FindObjectOfType<OldCarController>();
        StartCoroutine("StartGame");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            UIManager.instance.PauseScreen();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            RespawnCar();
        }

        UIManager.instance.UpdateCarSpeed(carController.carKMHView);
    }

    /// <summary>
    /// Respawn the car to the last checkpoint
    /// </summary>
    public void RespawnCar()
    {
        if (lapManager.currentCheckPoint > 0)
        {
            carController.transform.position = lapManager.checkPoints[lapManager.currentCheckPoint - 1].transform.position;
            carController.transform.rotation = Quaternion.LookRotation(lapManager.checkPoints[lapManager.currentCheckPoint - 1].transform.right, Vector3.up);
            if (UIManager.instance.IfPauseScreen())
            {
                UIManager.instance.PauseScreen();
            }
        }
        else
        {
            SceneManager.LoadSceneAsync(1);
        }
    }
    /// <summary>
    /// Starts new race
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartGame()
    {
        //Clear stats
        lapManager.ClearStats();
        //Count down from 5
        int timer = 5;
        while (timer > 0)
        {
            //Update timer text
            UIManager.instance.UpdateStartCounter(timer.ToString());
            timer--;
            yield return new WaitForSeconds(1f);
        }
        //Update timer text and activate timer
        UIManager.instance.UpdateStartCounter("GO");
        yield return new WaitForSeconds(1f);
        UIManager.instance.UpdateStartCounter("");
        lapManager.activeTimer = true;
        if (carController != null)
            carController.SetDriveBool(true);
        
    }
    /// <summary>
    /// End the current game
    /// </summary>
    /// <param name="lapTimes">The times from the all the laps you drove</param>
    /// <param name="bestTime">Best time on any lap you drove</param>
    /// <returns></returns>
    public IEnumerator EndGame(List<float> lapTimes, float bestTime)
    {
        //Disable timer and update ui
        lapManager.activeTimer = false;
        if (carController != null)
            carController.SetDriveBool(false);
        UIManager.instance.EndGameUI(lapTimes, bestTime);
        yield return new WaitForSeconds(0.8f);
        Time.timeScale = 0f;
        yield return null;
    }
}

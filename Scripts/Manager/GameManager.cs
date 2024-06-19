using com.ootii.Messages;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private string _nameUserGet;
    private string _nameLevel;
    public Button ButtonConfirm;
    public int Stage;
    public int Retry;
    private static GameManager instance;
    public static GameManager Instance { get => instance; }
    public float Timer;
    public bool IsCheck;
    private int _tryAttempt;
    public int RetryCount;
    public float TimeLastLevel;
    private DateTime DateTimeOfPause;

    [Header("Managers")]
    public RankManager RankingManager;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        Application.targetFrameRate = 144;
        RetryCount = 0;
        _nameLevel = null;
        _nameUserGet = null;
    }

    private void Update()
    {
        _nameLevel = GameController.Instance.NameLevel.Replace(" ", "").ToUpper();
        _nameUserGet = ItemManager.Instance.GetName();
        if (!IsCheck)
        {
            Timer += Time.deltaTime;
            UIManager.Instance.ShowTimer(Timer);
        }

        if (_nameLevel.Length == _nameUserGet.Length && !IsCheck)
        {
            StartCoroutine(CheckGame());
        }
    }

    IEnumerator CheckGame()
    {
        IsCheck = true;
        yield return new WaitForSeconds(1f);
        CheckGameOver();
        CheckGameWin();
    }

    private void CheckGameOver()
    {
        if (_nameUserGet != _nameLevel)
        {
            Debug.Log("Over");
            Timer = 0;
            MessageDispatcher.SendMessage(Message.GameOver);
            Retry = PlayerPrefs.GetInt("retry", 0);
            Retry++;
            PlayerPrefs.SetInt("retry", Retry);
            PlayerPrefs.Save();
            _tryAttempt = PlayerPrefs.GetInt("_tryAttempt", 1);
            _tryAttempt++;
            PlayerPrefs.SetInt("_tryAttempt", _tryAttempt);
        }
    }

    private void CheckGameWin()
    {
        if (_nameUserGet == _nameLevel)
        {
            TimeLastLevel = PlayerPrefs.GetFloat("timer");
            PlayerPrefs.SetFloat("timer", (Timer + TimeLastLevel));
            float timerPresent = PlayerPrefs.GetFloat("timer");
            Stage = PlayerPrefs.GetInt("stage", 0);
            Retry = PlayerPrefs.GetInt("retry", 0);
            _tryAttempt = PlayerPrefs.GetInt("_tryAttempt", 1);

            GameManager.Instance.RankingManager.LastPlayerData = new UserData(Player.Instance.DataUser().Name, Stage, Retry, timerPresent, -1);

            Stage++;
            PlayerPrefs.SetInt("stage", Stage);
            Retry++;
            PlayerPrefs.SetInt("retry", Retry);
            PlayerPrefs.SetInt("RetryCount", _tryAttempt);
            _tryAttempt = 1;
            PlayerPrefs.SetInt("_tryAttempt", _tryAttempt);
            MessageDispatcher.SendMessage(Message.GameWin);
            Timer = 0;
        }
    }

    private void OnApplicationPause(bool pause)
    {
        int timeCheckValue = Mathf.RoundToInt(Timer % 60);
        if (timeCheckValue > 3600)
            Timer = 0;

        if (pause)
            DateTimeOfPause = DateTime.Now;
        else
            Timer += (float)(DateTime.Now - DateTimeOfPause).TotalSeconds;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.ootii.Collections;
using com.ootii.Messages;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using Unity.Mathematics;

public class UIManager : MonoBehaviour
{
    public GameObject WinPanel, OverPanel, NoticePanel;
    public Button ButtonRePlay;
    public Button BTNextLevel;
    public Button BTNotice;
    public Button ExitPanelRank;
    private static UIManager _instance;
    public static UIManager Instance { get => _instance; }
    public TextMeshProUGUI TryAttempt;
    public TextMeshProUGUI TimerText;

    private void Awake()
    {
        _instance = this;
    }

    private void OnEnable()
    {
        MessageDispatcher.AddListener(Message.GameOver, GameOver, true);
        MessageDispatcher.AddListener(Message.GameWin, GameWin, true);
    }

    private void Start()
    {
        ButtonRePlay.onClick.AddListener(ButtonReplay);
        BTNextLevel.onClick.AddListener(ButtonNextLevel);
    }

    private void Update()
    {
        int tryAttempt = PlayerPrefs.GetInt("_tryAttempt", 1);
        TryAttempt.text = tryAttempt.ToString();
    }

    private void OnDisable()
    {
        MessageDispatcher.RemoveListener(Message.GameOver, GameOver, true);
        MessageDispatcher.RemoveListener(Message.GameWin, GameWin, true);
    }

    private void GameOver(IMessage message)
    {
        OverPanel.SetActive(true);
    }

    private void GameWin(IMessage message)
    {
        WinPanel.SetActive(true);
    }

    public void ButtonReplay()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        GameManager.Instance.IsCheck = false;
        SceneManager.LoadScene(sceneIndex);
    }

    private void ButtonNextLevel()
    {
        foreach(var item in GameController.Instance.Cells)
        {
            Destroy(item);
        }
        foreach(var item in GameController.Instance.Items)
        {
            Destroy(item);  
        }
        GameManager.Instance.IsCheck = false;
        GameController.Instance.Cells.Clear();
        GameController.Instance.Items.Clear();
        ItemManager.Instance.Alphabets.Clear();
        GameController.Instance.GenLevel();
        GameManager.Instance.RankingManager.CreateNewRankPanel();
    }

    public void ShowTimer(float valueTime)
    {   
        int minues = Mathf.RoundToInt(valueTime / 60);
        int seconds = Mathf.RoundToInt(valueTime % 60);
        TimerText.text = string.Format("{0:00}:{1:00}", minues, seconds); 
    }
}

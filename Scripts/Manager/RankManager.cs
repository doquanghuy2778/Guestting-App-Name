using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;


public class RankManager : MonoBehaviour
{
    [SerializeField] private ItemUserController ItemUserPrefab;
    [Space(10f)]
    [SerializeField] Transform RankPanel;
    [SerializeField] ScrollRect ScrollView;
    [SerializeField] Transform ContentTransform;
    [SerializeField] GridLayoutGroup RankGridLayoutGroup;
    [SerializeField] private UserRankPoolManager UserRankPoolManager;

    [SerializeField] private DataUsers DataUsers;
    [SerializeField] private ContentSizeFitter ContentSizeFitter;
    [Space(5f)]
    [SerializeField] private ItemUserController HighlightPlayerRankingUI;
    private UserData PlayerData { get => Player.Instance.DataUser(); }
    private readonly int MAX_USER_DISPLAY = 30;

    private int _currentPlayerRank = 0;
    private int _lastPlayerRankSaved = 0;
    private ItemUserController _currentPlayerRankUI;

    private List<UserData> _userDatas;
    private List<UserData> _displayingUsers;

    private List<ItemUserController> _cachedUIs;

    public UserData LastPlayerData;  

    private void Start()
    {
        //CreateNewRankPanel();
    }

    public void CreateNewRankPanel()
    {
        RankPanel.gameObject.SetActive(true);
        UpdatePlayerRank();
        UpdateDisplayingUser();
        UpdatePanel();
    }

    private void UpdatePlayerRank()
    {
        if (DataUsers.Users.Contains(PlayerData))
        {
            DataUsers.Users.Remove(PlayerData);
        }

        _userDatas = new List<UserData>();

        foreach (var user in DataUsers.Users)
        {
            _userDatas.Add(new UserData(user.Name, user.Stages, user.RetryAttempt, user.TotalTime, user.Ranking));
        }
        _userDatas.Add(PlayerData);
        _userDatas = _userDatas.OrderByDescending(x => x.Stages).ThenBy(x => x.RetryAttempt).ThenBy(x => x.TotalTime).ToList();

        for (int i = 0; i < _userDatas.Count; i++)
        {
            _userDatas[i].Ranking = i;

            if (_userDatas[i].Name == PlayerData.Name)
            {
                _currentPlayerRank = i;
            }
        }

        if (!PlayerPrefs.HasKey("PlayerRank"))
        {
            PlayerPrefs.SetInt("PlayerRank", _currentPlayerRank);
            _lastPlayerRankSaved = _currentPlayerRank;
        }
        else if (PlayerPrefs.GetInt("PlayerRank") > _currentPlayerRank)
        {
            _lastPlayerRankSaved = PlayerPrefs.GetInt("PlayerRank");
            PlayerPrefs.SetInt("PlayerRank", _currentPlayerRank);
        }
        else
        {
            _lastPlayerRankSaved = PlayerPrefs.GetInt("PlayerRank");
            _currentPlayerRank = PlayerPrefs.GetInt("PlayerRank");
        }
        ListExtensions.MoveElement(_userDatas, _currentPlayerRank, _lastPlayerRankSaved);
    }

    private void UpdateDisplayingUser()
    {
        if (_currentPlayerRank == _lastPlayerRankSaved)
        {
            int startIndex = Mathf.Max(0, _currentPlayerRank + 1 - MAX_USER_DISPLAY);
            int actualCount = Mathf.Min(MAX_USER_DISPLAY, _currentPlayerRank - startIndex + 1);
            _displayingUsers = _userDatas.GetRange(startIndex, actualCount);

            int remainingCount = MAX_USER_DISPLAY - actualCount;
            if (remainingCount > 0)
            {
                int endIndex = Mathf.Min(_userDatas.Count - 1, _currentPlayerRank + remainingCount);
                int addCount = endIndex - _displayingUsers.Count;
                _displayingUsers.AddRange(_userDatas.GetRange(_displayingUsers.Count, addCount));
            }
        }
        else
        {
            int startIndex = Mathf.Max(0, _currentPlayerRank + 1 - MAX_USER_DISPLAY);
            _displayingUsers = _userDatas.GetRange(startIndex, _lastPlayerRankSaved - startIndex + 1);
        }
    }

    private void UpdatePanel()
    {
        if(_cachedUIs != null)
            foreach (var cachedUI in _cachedUIs)
            {
                UserRankPoolManager.DisableToPool(cachedUI);
            }

        _cachedUIs = new List<ItemUserController>();

        foreach (var user in _displayingUsers)
        {
            ItemUserController userRankUI = UserRankPoolManager.GetUserRankUI();
            userRankUI.transform.SetParent(ContentTransform);
            userRankUI.transform.localScale = Vector3.one;
            userRankUI.GenData(user.Name, user.Stages, user.RetryAttempt, user.TotalTime, user.Ranking);
            userRankUI.gameObject.SetActive(true);
            
            _cachedUIs.Add(userRankUI);

            if (user.Name == PlayerPrefs.GetString("name"))
                _currentPlayerRankUI = userRankUI;
        }

        Action onCompleteFitter = () => {};
        onCompleteFitter += FocusPlayer;
        onCompleteFitter += () => StartCoroutine(PlayerRankTween(_lastPlayerRankSaved, _currentPlayerRank));
        ContentSizeFitter.Fitter(onCompleteFitter);
    }

    private void FocusPlayer()
    {
        ScrollView.verticalNormalizedPosition = 0f;
        Canvas.ForceUpdateCanvases();
    }

    private IEnumerator PlayerRankTween(int lastPlayerRankSaved, int newPlayerRank)
    {
        RankGridLayoutGroup.enabled = false;

        if(LastPlayerData != null)
            HighlightPlayerRankingUI.GenData($"{LastPlayerData.Name} <size=120%><#21bf82>(You)", LastPlayerData.Stages, LastPlayerData.RetryAttempt, LastPlayerData.TotalTime, lastPlayerRankSaved);
        else 
            HighlightPlayerRankingUI.GenData($"{PlayerData.Name} <size=120%><#21bf82>(You)", PlayerData.Stages, PlayerData.RetryAttempt, PlayerData.TotalTime, lastPlayerRankSaved);

        if (lastPlayerRankSaved == newPlayerRank)
        {
            RankGridLayoutGroup.enabled = true;
            HighlightPlayerRankingUI.GenData($"{PlayerData.Name} <size=120%><#21bf82>(You)", PlayerData.Stages, PlayerData.RetryAttempt, PlayerData.TotalTime, lastPlayerRankSaved);
            yield break;
        }

        HighlightPlayerRankingUI.transform.DOScale(1.1f, 0.2f).SetEase(Ease.InOutSine);
        yield return _currentPlayerRankUI.transform.DOScale(1.1f, 0.2f).SetEase(Ease.InOutSine).WaitForCompletion();

        float deltaY = -RankGridLayoutGroup.cellSize.y * (lastPlayerRankSaved - newPlayerRank) - RankGridLayoutGroup.spacing.y * (lastPlayerRankSaved - newPlayerRank - 1);

        RectTransform rect = ContentTransform.GetComponent<RectTransform>();

        yield return rect.DOAnchorPos3D(rect.anchoredPosition3D + new Vector3(0, deltaY, 0), 2f).SetEase(Ease.InOutSine).WaitForCompletion();

        foreach (var UI in _cachedUIs)
        {
            if (UI.Name == _userDatas[newPlayerRank - 1].Name)
            {
                int childIndex = UI.transform.GetSiblingIndex();
                _currentPlayerRankUI.transform.SetSiblingIndex(childIndex + 1);
            }
        }

        HighlightPlayerRankingUI.GenData($"{PlayerData.Name} <size=120%><#21bf82>(You)", PlayerData.Stages, PlayerData.RetryAttempt, PlayerData.TotalTime, newPlayerRank);
        RankGridLayoutGroup.enabled = true;

        HighlightPlayerRankingUI.transform.DOScale(1f, 0.2f).SetEase(Ease.OutCirc);
        yield return _currentPlayerRankUI.transform.DOScale(1f, 0.2f).SetEase(Ease.OutCirc).WaitForCompletion();
    }
}

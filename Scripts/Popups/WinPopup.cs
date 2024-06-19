using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class WinPopup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI StageText;
    [SerializeField] TextMeshProUGUI TryAttemptText;
    [SerializeField] TextMeshProUGUI TimeText;
    private float _oldTime;
    private UserData PlayerData => Player.Instance.DataUser();

    private void OnEnable()
    {
        StageText.text = GameManager.Instance.RankingManager.LastPlayerData.Stages.ToString();
        TryAttemptText.text = (PlayerData.RetryAttempt - PlayerPrefs.GetInt("RetryCount")).ToString();
        _oldTime = GameManager.Instance.TimeLastLevel;
        float minues = Mathf.RoundToInt(_oldTime / 60);
        float seconds = Mathf.RoundToInt(_oldTime % 60);
        TimeText.text = string.Format("{0:00}:{1:00}", minues, seconds);
    }

    public void StatsUpdateTween()
    {
        StartCoroutine(StatsUpdate());
    }

    private IEnumerator StatsUpdate()
    {
        float _minuesTotalTime = Mathf.RoundToInt(PlayerData.TotalTime / 60);
        float _secondsTotalTime = Mathf.RoundToInt(PlayerData.TotalTime % 60);
        float timeCound = PlayerData.TotalTime - _oldTime;
        float _secondsTimeCound = Mathf.RoundToInt(timeCound % 60);

        //stage
        yield return StageText.DOFade(0, 0.15f).WaitForCompletion();
        StageText.text = "+1";
        yield return StageText.DOFade(1, 0.15f).WaitForCompletion();
        yield return new WaitForSeconds(0.3f);
        yield return StageText.DOFade(0, 0.15f).WaitForCompletion();
        StageText.text = PlayerData.Stages.ToString();
        yield return StageText.DOFade(1, 0.15f).WaitForCompletion();

        //time
        yield return TimeText.DOFade(0, 0.15f).WaitForCompletion();
        TimeText.text = "+" + _secondsTimeCound.ToString() + "s";
        yield return TimeText.DOFade(1, 0.15f).WaitForCompletion();
        yield return new WaitForSeconds(0.3f);
        yield return TimeText.DOFade(0, 0.15f).WaitForCompletion();
        TimeText.text = string.Format("{0:00}:{1:00}", _minuesTotalTime, _secondsTotalTime);
        yield return TimeText.DOFade(1, 0.15f).WaitForCompletion();

        //tryAttempt
        yield return TryAttemptText.DOFade(0, 0.15f).WaitForCompletion();
        TryAttemptText.text = "+" + PlayerPrefs.GetInt("RetryCount");
        yield return TryAttemptText.DOFade(1, 0.15f).WaitForCompletion();
        yield return new WaitForSeconds(0.3f);
        yield return TryAttemptText.DOFade(0, 0.15f).WaitForCompletion();
        TryAttemptText.text = PlayerData.RetryAttempt.ToString();
        yield return TryAttemptText.DOFade(1, 0.15f).WaitForCompletion();
    }
}

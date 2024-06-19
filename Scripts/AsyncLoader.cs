using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class AsyncLoader : MonoBehaviour
{
    [SerializeField] private Image loadingFill;
    [SerializeField] private float _loadTime = 1f;
    [SerializeField] private TextMeshProUGUI _percentText;
    private int _percent = 0;

    private void Start()
    {
        LoadLeverBtn();
    }

    private void Update()
    {
        _percentText.text = _percent.ToString();    
    }

    public void LoadLeverBtn()
    {
        int NumOfGetName = PlayerPrefs.GetInt("IsGetName");
        if(NumOfGetName == 0)
        {
            StartCoroutine(LoadLevelASync(1));
        }
        else
        {
            StartCoroutine(LoadLevelASync(2));
        }
    }

    IEnumerator LoadLevelASync(int levelToLoad)
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);
        loadOperation.allowSceneActivation = false;
        DOTween.To(() => 0, value => _percent = value, 100, _loadTime).SetEase(Ease.OutQuad);
        yield return loadingFill.DOFillAmount(1, _loadTime).SetEase(Ease.OutQuad).WaitForCompletion();
        yield return new WaitForSeconds(0.5f);
        loadOperation.allowSceneActivation = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemUserController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _name;
    [SerializeField]
    private TextMeshProUGUI _stages;
    [SerializeField]
    private TextMeshProUGUI _retryAttempt;
    [SerializeField]
    private TextMeshProUGUI _totalTime;
    [SerializeField]
    private TextMeshProUGUI _stt;
    [SerializeField]
    private Image _rankImage;
    [SerializeField]
    private Image _backgroundImage;

    public string Name => _name.text;

    [System.Serializable]
    public class RankSprite
    {
        public int Rank;
        public Sprite Sprite;
        public Sprite BackgroundSprite;
        public Color Color;
    }

    [Space(10f)]
    [SerializeField] private Color NormalColor;
    [SerializeField] Sprite NormalBackground;
    [SerializeField] List<RankSprite> RankSprites;

    public void GenData(string name, int stage, int retry, float time, int stt)
    {
        _stt.text = stt.ToString();
        _name.text = name;
        _stages.text = stage.ToString();
        _retryAttempt.text = retry.ToString();
        float minues = Mathf.RoundToInt(time / 60);
        float seconds = Mathf.RoundToInt(time % 60);
        _totalTime.text = string.Format("{0:00}:{1:00}", minues, seconds);

        if (stt <= 3)
        {
            _stt.color = RankSprites[stt - 1].Color;
            _rankImage.sprite = RankSprites[stt - 1].Sprite;
            _backgroundImage.sprite = RankSprites[stt - 1].BackgroundSprite;
            _rankImage.enabled = true;
            _stt.text = "";
        }
        else
        {
            _stt.color = NormalColor;
            _rankImage.enabled = false;
            _backgroundImage.sprite = NormalBackground;
            _stt.text = stt.ToString();
        }
    }
}

using DG.Tweening;
using UnityEngine;

public class ButtonSetting : MonoBehaviour
{
    [SerializeField] private Transform toggle;
    [SerializeField] private GameObject sprOff;
    [SerializeField] private GameObject sprOn;
    [SerializeField] private GameObject sprOffBG;
    [SerializeField] private GameObject sprOnBG;
    protected bool value = false;

    protected bool isTweening = false;
    protected string buttonKey;

    private void OnEnable()
    {
        buttonKey = GenerateUniqueKey();
        if (PlayerPrefs.HasKey(buttonKey))
        {
            value = PlayerPrefs.GetInt(buttonKey) == 1;
        }
        float x = value ? Mathf.Abs(toggle.localPosition.x) : -Mathf.Abs(toggle.localPosition.x);
        toggle.localPosition = new Vector3(x, 0, 0);
        SetSprIcon();
    }

    public virtual void OnClick()
    {
        value = !value;
        SetStatusButton();
        Toggle();
    }

    private void Toggle()
    {
        if (isTweening)
            return;
        isTweening = true;

        toggle.DOLocalMoveX(-toggle.localPosition.x, 0.1f).SetEase(Ease.InOutQuad).OnComplete(() => isTweening = false);
        SetSprIcon();
    }

    private void SetStatusButton()
    {
        PlayerPrefs.SetInt(buttonKey, value ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void SetSprIcon()
    {
        if (PlayerPrefs.HasKey(buttonKey))
        {
            value = PlayerPrefs.GetInt(buttonKey) == 1;
        }

        sprOn.SetActive(value);
        sprOff.SetActive(!value);

        sprOnBG.SetActive(value);
        sprOffBG.SetActive(!value);
    }

    private string GenerateUniqueKey()
    {
        return gameObject.name + "_" + GetType().Name + "_isPressed";
    }
}

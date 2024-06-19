using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ContentSizeFitter : MonoBehaviour
{
    [SerializeField] RectTransform RectTransform;
    [SerializeField] GridLayoutGroup LayoutGroup;

    public void Fitter(Action onComplete)
    {
        StartCoroutine(Fit(onComplete));
    }

    private IEnumerator Fit(Action onComplete)
    {
        yield return null;
        Debug.Log(LayoutGroup.enabled);
        RectTransform.sizeDelta = new Vector2(RectTransform.sizeDelta.x, LayoutGroup.preferredHeight);
        Debug.Log(LayoutGroup.preferredHeight);
        yield return null;
        onComplete?.Invoke();
    }
}

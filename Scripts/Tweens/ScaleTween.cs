using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaleTween : MonoBehaviour
{
    public ScaleMode scaleMode;
    public StartMode startMode;
    [SerializeField] float startValue = 1;
    [SerializeField] float duration = 1;

    private void OnEnable()
    {
        switch (scaleMode)
        {
            case ScaleMode.XYZ:
                if (startMode == StartMode.To)
                    transform.DOScale(new Vector3(startValue, startValue, startValue), duration).SetEase(Ease.Linear);
                else
                    transform.DOScale(new Vector3(startValue, startValue, startValue), duration).SetEase(Ease.Linear).From();
                break;
            case ScaleMode.X:
                if(startMode == StartMode.To)
                    transform.DOScaleX(startValue, duration).SetEase(Ease.Linear);
                else
                    transform.DOScaleX(startValue, duration).SetEase(Ease.Linear).From();
                break;
            case ScaleMode.Y:
                if (startMode == StartMode.To)
                    transform.DOScaleY(startValue, duration).SetEase(Ease.Linear);
                else
                    transform.DOScaleY(startValue, duration).SetEase(Ease.Linear).From();
                break;
            case ScaleMode.Z:
                if (startMode == StartMode.To)
                    transform.DOScaleZ(startValue, duration).SetEase(Ease.Linear);
                else
                    transform.DOScaleZ(startValue, duration).SetEase(Ease.Linear).From();
                break;
        }
    }

    public enum ScaleMode
    {
        XYZ,
        X,
        Y,
        Z,
    }    

    public enum StartMode
    {
        From,
        To
    }
}

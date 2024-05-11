using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BarUI : UI<BarUI>
{
    [SerializeField]
    private Image imageFillInstant;

    [SerializeField]
    private Image imageFillInterpolated;

    [SerializeField]
    private float animationDuration;

    private Tween fillTween;

    public float Value { get; set; }

    protected override void OnRefresh()
    {
        if (Value > imageFillInstant.fillAmount)
        {
            RefreshFill(imageFillInstant, imageFillInterpolated);
        }
        else
        {
            RefreshFill(imageFillInterpolated, imageFillInstant);
        }
    }

    private void RefreshFill(Image imageInstant, Image imageInterpolated)
    {
        if (fillTween != null && fillTween.IsPlaying())
        {
            fillTween.Kill();
        }

        imageInstant.fillAmount = Value;

        fillTween = imageInterpolated.DOFillAmount(Value, animationDuration).SetEase(Ease.OutCubic);
    }
}

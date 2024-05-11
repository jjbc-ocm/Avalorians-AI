using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameEffectUI : UI<GameEffectUI>
{
    [SerializeField] private Image imageIcon;
    [SerializeField] private Image imageCooldown;

    private AppliedEffectInfo data;

    public AppliedEffectInfo Data { get => data; set => data = value; }

    protected override void OnRefresh()
    {
        imageIcon.sprite = data.Effect.Icon;
        imageCooldown.fillAmount = 1 - (data.Duration / data.Effect.Duration);
    }
}

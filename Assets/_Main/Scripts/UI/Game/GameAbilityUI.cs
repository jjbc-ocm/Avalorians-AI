 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameAbilityUI : UI<GameAbilityUI>
{
    [SerializeField] private Image imageIcon;
    [SerializeField] private Image imageCooldown;

    private AbilityCooldownInfo data;

    public AbilityCooldownInfo Data { get => data; set => data = value; }

    protected override void OnRefresh()
    {
        imageIcon.sprite = data.Ability.Icon;
        imageCooldown.fillAmount = 1 - (data.Cooldown / data.Ability.Cooldown);
    }
}

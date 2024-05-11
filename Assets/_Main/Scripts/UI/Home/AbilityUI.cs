using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityUI : UI<AbilityUI>
{
    [SerializeField] private Image imageIcon;
    [SerializeField] private TMP_Text textName;
    [SerializeField] private TMP_Text textDesc;
    [SerializeField] private GameObject uiCost;
    [SerializeField] private GameObject uiCooldown;
    [SerializeField] private TMP_Text textCost;
    [SerializeField] private TMP_Text textCooldown;

    private AbilitySO ability;
    private PassiveInfo passive;

    public AbilitySO Ability { get => ability; set => ability = value; }

    public PassiveInfo Passive { get => passive; set => passive = value; }

    protected override void OnRefresh()
    {
        if (ability)
        {
            imageIcon.sprite = ability.Icon;
            textName.text = ability.AbilityName;
            textDesc.text = ability.Info;
            uiCost.SetActive(true);
            uiCooldown.SetActive(true);
            textCost.text = ability.Mana.ToString();
            textCooldown.text = ability.Cooldown.ToString("F2") + "s";
        }

        if (passive != null)
        {
            imageIcon.sprite = passive.Icon;
            textName.text = passive.PassiveName;
            textDesc.text = passive.Info;
            uiCost.SetActive(false);
            uiCooldown.SetActive(false);
        }
    }
}

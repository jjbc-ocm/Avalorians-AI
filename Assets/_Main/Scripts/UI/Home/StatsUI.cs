using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : SingletonUI<StatsUI>
{
    [SerializeField] private TMP_Text textHealth;
    [SerializeField] private TMP_Text textMana;
    [SerializeField] private TMP_Text textAttack;
    [SerializeField] private TMP_Text textDefense;
    [SerializeField] private TMP_Text textSpeed;

    private ClassSO selected;

    public ClassSO Selected { get => selected; set => selected = value; }

    protected override void OnRefresh()
    {
        textHealth.text = selected.BaseHealth.ToString();
        textMana.text = selected.BaseMana.ToString();
        textAttack.text = selected.BaseAttack.ToString();
        textDefense.text = selected.BaseDefense.ToString();
        textSpeed.text = selected.BaseSpeed.ToString("F2") + "/s";
    }
}


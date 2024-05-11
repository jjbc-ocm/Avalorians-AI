using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilitiesUI : SingletonUI<AbilitiesUI>
{
    [SerializeField] private AbilityUI uiAttack;
    [SerializeField] private AbilityUI uiAbility;
    [SerializeField] private AbilityUI uiPassive;

    private ClassSO selected;

    public ClassSO Selected { get => selected; set => selected = value; }

    protected override void OnRefresh()
    {
        uiAttack.Refresh(self =>
        {
            self.Ability = selected.AbilityInputs[0].Ability;
        });

        uiAbility.Refresh(self =>
        {
            self.Ability = selected.AbilityInputs[1].Ability;
        });

        uiPassive.Refresh(self =>
        {
            self.Passive = selected.Passives[0];
        });
    }
}

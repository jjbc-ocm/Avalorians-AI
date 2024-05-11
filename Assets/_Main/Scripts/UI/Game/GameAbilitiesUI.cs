using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAbilitiesUI : SingletonListViewUI<GameAbilityUI, GameAbilitiesUI>
{
    private Character data;

    private bool isInitialize;

    public Character Data { get => data; set => data = value; }

    protected override void OnRefresh()
    {
        data.Stats.OnAbilityCooldownsUpdated += Stats_OnAbilityCooldownsUpdated;
    }

    private void Stats_OnAbilityCooldownsUpdated(List<AbilityCooldownInfo> abilityCooldowns)
    {
        RefreshItems(abilityCooldowns, (item, data, index) =>
        {
            item.Data = data;
        },
        isInitialize);

        isInitialize = true;
    }
}

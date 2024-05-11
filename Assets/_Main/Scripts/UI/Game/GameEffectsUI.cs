using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEffectsUI : SingletonListViewUI<GameEffectUI, GameEffectsUI>
{
    private Character data;

    public Character Data { get => data; set => data = value; }

    protected override void OnRefresh()
    {
        // TODO: fix when needed, this causes null error
        //data.Stats.OnAddAppliedEffect += Stats_OnAddAppliedEffect;
        //data.Stats.OnRemoveAppliedEffect += Stats_OnRemoveAppliedEffect;
        //data.Stats.OnAppliedEffectsUpdated += Stats_OnAppliedEffectsUpdated;
    }

    

    private void Stats_OnAddAppliedEffect(AppliedEffectInfo data)
    {
        AddItem(data, (self, data, index) =>
        {
            self.Data = data;
        });
    }

    private void Stats_OnRemoveAppliedEffect(AppliedEffectInfo data)
    {
        var removedItems = new List<GameEffectUI>();

        foreach (var item in items)
        {
            if (item.Data.Effect == data.Effect)
            {
                Destroy(item.gameObject);
            }
            
        }

        foreach (var removedItem in removedItems)
        {
            items.Remove(removedItem);
        }
    }

    private void Stats_OnAppliedEffectsUpdated(List<AppliedEffectInfo> appliedEffects)
    {
        RefreshItems(appliedEffects, (item, data, index) =>
        {
            item.Data = data;
        },
        true);
    }
}

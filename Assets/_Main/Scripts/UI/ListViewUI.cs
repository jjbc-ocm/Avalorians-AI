using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ListViewUI<S, T> : UI<T> 
    where S : UI<S> 
    where T : MonoBehaviour
{
    protected List<S> items;

    [SerializeField]
    private S prefabItem;

    [SerializeField]
    private Transform uiItemView;

    protected void DeleteItems()
    {
        if (items == null)
        {
            items = new List<S>();
        }

        foreach (var item in items)
        {
            Destroy(item.gameObject);
        }

        items.Clear();
    }

    protected void RefreshItems<Data>(IEnumerable<Data> data, Action<S, Data, int> onBeforeRefresh, bool isInitialized = false)
    {
        if (items == null)
        {
            items = new List<S>();
        }

        var index = 0;

        foreach (var datum in data)
        {
            if (!isInitialized)
            {
                var newItem = Instantiate(prefabItem, uiItemView);

                newItem.Refresh((self) => onBeforeRefresh(self, datum, index));

                items.Add(newItem);
            }
            else
            {
                items[index].Refresh((self) => onBeforeRefresh(self, datum, index));
            }

            index++;
        }
    }
}

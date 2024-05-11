using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectClassesUI : SingletonListViewUI<SelectClassUI, SelectClassesUI>
{
    private List<ClassSO> data;

    public List<ClassSO> Data { get => data; set => data = value; }

    protected override void OnRefresh()
    {
        DeleteItems();

        RefreshItems(data, (item, data, index) =>
        {
            item.Data = data;
        });
    }
}

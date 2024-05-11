using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPreviewsUI : SingletonListViewUI<PlayerPreviewUI, PlayerPreviewsUI>
{
    private Dictionary<int, Player>.ValueCollection data;

    public Dictionary<int, Player>.ValueCollection Data { get => data; set => data = value; }

    protected override void OnRefresh()
    {
        DeleteItems();

        RefreshItems(data, (item, data, index) =>
        {
            item.Data = data;
        });
    }
}

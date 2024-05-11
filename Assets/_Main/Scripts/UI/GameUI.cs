using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUI : SingletonUI<GameUI>
{
    [SerializeField]
    private PlayerUI prefabPlayerUI;

    [SerializeField]
    private Transform uiPlayers;


    public void AddPlayerUI(Character character)
    {
        var uiPlayer = Instantiate(prefabPlayerUI, uiPlayers);

        uiPlayer.Initialize(character);
    }

    protected override void OnRefresh()
    {

    }
}

using Photon.Pun;
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

        if (character.photonView.IsMine)
        {
            GameAbilitiesUI.Instance.Refresh(self =>
            {
                self.Data = character;
            });

            GameEffectsUI.Instance.Refresh(self =>
            {
                self.Data = character;
            });
        }
    }

    public void HomeClick()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties != null)
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
            {
                { "index", null },
                { "isReady", null },
                { "classId", null }
            });
        }

        PhotonNetwork.LeaveRoom();
    }

    protected override void OnRefresh()
    {

    }
}

using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MatchMakingManager : PunSingleton<MatchMakingManager>
{
    [SerializeField]
    private string gameVersion = "1";

    public event Action<bool> OnProgress;
    public event Action<Dictionary<int, Player>.ValueCollection> OnPlayersChanged;

    private MatchMakingInfo matchMakingInfo;

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void OnDestroy()
    {
        OnProgress = null;
        OnPlayersChanged = null;
    }

    public void Play(MatchMakingInfo matchMakingInfo)
    {
        this.matchMakingInfo = matchMakingInfo;

        OnProgress?.Invoke(true);

        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameVersion;
        }
    }

    public void Leave()
    {
        matchMakingInfo = null;

        PhotonNetwork.LeaveRoom();
    }

    










    public override void OnConnectedToMaster()
    {
        if (matchMakingInfo != null)
        {
            PhotonNetwork.JoinRandomRoom(null, matchMakingInfo.PlayersCount);
        }
        
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        OnProgress?.Invoke(false);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = matchMakingInfo.PlayersCount });
    }

    public override void OnJoinedRoom()
    {
        OnPlayersChanged?.Invoke(PhotonNetwork.CurrentRoom.Players.Values);

        TryLoadGame();
    }

    public override void OnLeftRoom()
    {
        OnPlayersChanged?.Invoke(PhotonNetwork.CurrentRoom.Players.Values);

        OnProgress?.Invoke(false);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        OnPlayersChanged?.Invoke(PhotonNetwork.CurrentRoom.Players.Values);

        TryLoadGame();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        OnPlayersChanged?.Invoke(PhotonNetwork.CurrentRoom.Players.Values);
    }










    private void TryLoadGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                AssignIndicesToAllPlayers();
            }
        }
    }

    private void AssignIndicesToAllPlayers()
    {
        int index = 0;

        foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            var props = new ExitGames.Client.Photon.Hashtable();

            props.Add("index", index);

            player.SetCustomProperties(props);

            index++;
        }

        StartCoroutine(YieldLoadGame());
    }

    IEnumerator YieldLoadGame()
    {
        // Wait until all players have their indices set
        var hasAllIndex = false;

        while (!hasAllIndex)
        {
            hasAllIndex = true;

            foreach (var player in PhotonNetwork.CurrentRoom.Players.Values)
            {
                //if (!player.CustomProperties.ContainsKey("index"))
                if (!player.CustomProperties.ContainsKey("isReady"))
                {
                    hasAllIndex = false;

                    break;
                }
            }
            yield return new WaitForSeconds(0.1f); // Check every 0.1 seconds
        }

        Debug.Log(PhotonNetwork.CurrentRoom.Players.Count);

        PhotonNetwork.LoadLevel("Game");
    }
}

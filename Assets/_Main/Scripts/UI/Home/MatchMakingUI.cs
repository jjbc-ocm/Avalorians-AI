using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchMakingUI : UI<MatchMakingUI>
{
    [SerializeField]
    private TMP_InputField inputName;

    private void Start()
    {
        inputName.text = PlayerPrefs.GetString("playerName", "");

        MatchMakingManager.Instance.OnProgress += Instance_OnProgress;
    }

    private void Instance_OnProgress(bool isActive)
    {
        if (isActive)
        {
            PickPhaseUI.Instance.Show(self => 
            {
                self.Selected = null;
            });
        }
        else
        {
            PickPhaseUI.Instance.Hide();
        }
    }

    public void OnNameChange(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            Debug.LogError("Player Name is null or empty");
            return;
        }

        PhotonNetwork.NickName = value;

        PlayerPrefs.SetString("playerName", value);
    }

    public void OnPracticeClick()
    {
        MatchMakingManager.Instance.Play(new MatchMakingInfo
        {
            PlayersCount = 1
        });
    }

    public void OnDuelClick()
    {
        MatchMakingManager.Instance.Play(new MatchMakingInfo
        {
            PlayersCount = 2
        });
    }

    public void OnArenaClick()
    {
        MatchMakingManager.Instance.Play(new MatchMakingInfo
        {
            PlayersCount = 4
        });
    }

    public void OnLeaveClick()
    {
        MatchMakingManager.Instance.Leave();
    }

    protected override void OnRefresh()
    {
        throw new System.NotImplementedException();
    }
}
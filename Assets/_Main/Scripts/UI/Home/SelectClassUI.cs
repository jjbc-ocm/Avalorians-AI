using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectClassUI : UI<SelectClassUI>
{
    [SerializeField] private Image imageIcon;
    [SerializeField] private Button button;

    private ClassSO data;

    public ClassSO Data { get => data; set => data = value; }

    private void Update()
    {
        // Need to be updated in realtime
        button.interactable = !(PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("isReady", out object isReadyValue) && (bool)isReadyValue);
    }

    public void OnClick()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
        {
            { "classId", data.Id }
        });

        PickPhaseUI.Instance.Refresh(self =>
        {
            self.Selected = data;
        });

        AbilitiesUI.Instance.Show(self =>
        {
            self.Selected = data;
        });

        StatsUI.Instance.Show(self =>
        {
            self.Selected = data;
        });
    }

    protected override void OnRefresh()
    {
        imageIcon.sprite = data.Icon;
    }
}

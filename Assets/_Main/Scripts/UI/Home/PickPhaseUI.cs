using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickPhaseUI : SingletonUI<PickPhaseUI>
{
    [SerializeField] private TMP_Text textNickname;
    [SerializeField] private TMP_Text textClass;
    [SerializeField] private Image imageIcon;
    [SerializeField] private Button buttonReady;

    private ClassSO selected;

    public ClassSO Selected { get => selected; set => selected = value; }

    private void Start()
    {
        MatchMakingManager.Instance.OnPlayersChanged += Instance_OnPlayersChanged;
    }

    private void Instance_OnPlayersChanged(Dictionary<int, Player>.ValueCollection data)
    {
        PlayerPreviewsUI.Instance.Refresh(self =>
        {
            self.Data = data;
        });
    }

    public void OnReadyClick()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable
        {
            { "isReady", true }
        });
    }

    protected override void OnRefresh()
    {
        textNickname.text = PhotonNetwork.NickName;
        buttonReady.interactable = selected;

        textClass.gameObject.SetActive(selected);
        imageIcon.gameObject.SetActive(selected);

        if (selected)
        {
            textClass.text = selected.ClassName;
            imageIcon.sprite = selected.Icon;
        }

        SelectClassesUI.Instance.Refresh(self =>
        {
            self.Data = DataManager.Instance.Classes;
        });
    }
}

using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPreviewUI : UI<PlayerPreviewUI>
{
    [SerializeField] private TMP_Text textNickname;
    [SerializeField] private Image imageIcon;

    private Player data;

    public Player Data { get => data; set => data = value; }

    private void Update()
    {
        // Need to be updated in realtime
        if (data.CustomProperties.TryGetValue("classId", out object classIdValue))
        {
            var classId = classIdValue as string;

            var clazz = DataManager.Instance.GetClass(classId);

            imageIcon.sprite = clazz.Icon;
        }
    }

    protected override void OnRefresh()
    {
        textNickname.text = data.NickName;
    }
}

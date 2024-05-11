using Cinemachine;
using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : PunSingleton<GameManager>
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private Transform[] playerSpawners;

    public CinemachineVirtualCamera VirtualCamera => virtualCamera;

    private void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("index", out object indexValue) &&
            PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("classId", out object classIdValue))
        {
            int spawnIndex = (int)indexValue;
            var classId = (string)classIdValue;

            Transform spawnPoint = playerSpawners[spawnIndex]; // Assume you have an array of Transform for spawn points

            PhotonNetwork.Instantiate(DataManager.Instance.GetClass(classId).PrefabName, spawnPoint.position, spawnPoint.rotation);
        } 
        else
        {
            Debug.Log("Failed to retrieve index. Custom properties available: " + PhotonNetwork.LocalPlayer.CustomProperties.Count);

            foreach (var key in PhotonNetwork.LocalPlayer.CustomProperties.Keys)
            {
                Debug.Log("Property key: " + key + " value: " + PhotonNetwork.LocalPlayer.CustomProperties[key]);
            }
        }
    }

    public void ResetPosition(PhotonView photonView)
    {
        if (photonView.Owner.CustomProperties.TryGetValue("index", out object indexValue))
        {
            int spawnIndex = (int)indexValue;

            photonView.transform.position = playerSpawners[spawnIndex].position;
        }
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Home");
    }
}

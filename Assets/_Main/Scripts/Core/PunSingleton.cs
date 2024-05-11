using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunSingleton<T> : MonoBehaviourPunCallbacks where T : MonoBehaviourPunCallbacks
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>(true);
            }

            return instance;
        }
    }
}

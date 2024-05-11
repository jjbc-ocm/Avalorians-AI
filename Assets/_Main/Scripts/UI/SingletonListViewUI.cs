using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonListViewUI<S, T> : ListViewUI<S, T> 
    where S : UI<S>
    where T : MonoBehaviour
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

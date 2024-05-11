using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonPoolManager<S, T> : PoolManager<T> 
    where S : MonoBehaviour
    where T : Poolable<T>
{
    private static S instance;

    public static S Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<S>(true);
            }

            return instance;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonUI<T> : UI<T> where T : UI<T>
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

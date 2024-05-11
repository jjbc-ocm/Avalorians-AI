using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonBarUI<T> : BarUI where T : BarUI
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

 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class Poolable<T> : MonoBehaviour where T : Object
{
    private IObjectPool<T> pool;

    public void Initialize(IObjectPool<T> pool)
    {
        this.pool = pool;

        OnInitialize();
    }

    public void Release(T self)
    {
        pool.Release(self);
    }

    public abstract void OnInitialize();
}


using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

public abstract class PoolManager<T> : MonoBehaviour where T : Poolable<T>
{
    private Dictionary<string, ObjectPool<T>> pools;

    private T prefab;

    private Vector3 position;

    private Quaternion rotation;

    private List<T> activeObjects;

    public List<T> ActiveObjects => activeObjects;

    private void Awake()
    {
        pools = new Dictionary<string, ObjectPool<T>>();

        activeObjects = new List<T>();
    }

    public T Get(T prefab, Vector3 position, Quaternion rotation)
    {
        this.prefab = prefab;

        this.position = position;

        this.rotation = rotation;

        if (pools.TryGetValue(prefab.name, out ObjectPool<T> pool))
        {
            return pool.Get();
        }

        var newPool = new ObjectPool<T>(OnCreate, OnGet, OnRelease, OnDelete);

        pools.Add(prefab.name, newPool);

        return newPool.Get();
    }

    private T OnCreate()
    {
        var obj = Instantiate(prefab, position, rotation);

        obj.name = prefab.name;

        return obj.GetComponent<T>();
    }

    private void OnGet(T obj)
    {
        obj.transform.position = position;

        obj.transform.rotation = rotation;

        obj.Initialize(pools[obj.gameObject.name]);

        obj.gameObject.SetActive(true);

        activeObjects.Add(obj);
    }

    private void OnRelease(T obj)
    {
        obj.gameObject.SetActive(false);

        activeObjects.Remove(obj);
    }

    private void OnDelete(T obj)
    {
        Destroy(obj.gameObject);
    }

    

    

    
}

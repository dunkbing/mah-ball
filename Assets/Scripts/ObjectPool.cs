using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// object pooling implementation
/// </summary>
public class ObjectPool : MonoBehaviour
{
    [Serializable]
    public struct PoolItem
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPool Instance { get; private set; }
    public List<PoolItem> poolItems;
    private IDictionary<string, Queue<GameObject>> _pools;

    private void Awake()
    {
        Instance = this;
        _pools = new Dictionary<string, Queue<GameObject>>();
        foreach (var poolItem in poolItems)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (var i = 0; i < poolItem.size; i++)
            {
                var go = Instantiate(poolItem.prefab);
                go.SetActive(false);
                objectPool.Enqueue(go);
            }

            _pools[poolItem.tag] = objectPool;
        }
    }

    public GameObject Spawn(string tag, Vector3 position, Quaternion rotation, Action<GameObject> callback = null)
    {
        if (!_pools.ContainsKey(tag))
        {
            return null;
        }

        var go = _pools[tag].Dequeue();
        callback?.Invoke(go);
        go.SetActive(true);
        go.transform.position = position;
        go.transform.rotation = rotation;
        // go.GetComponent<ISpawn>()?.Spawn();

        _pools[tag].Enqueue(go);

        return go;
    }

    public GameObject Spawn(string tag, Action<GameObject> callback = null)
    {
        if (!_pools.ContainsKey(tag))
        {
            return null;
        }

        var go = _pools[tag].Dequeue();
        callback?.Invoke(go);
        go.SetActive(true);
        // go.GetComponent<ISpawn>()?.Spawn();

        _pools[tag].Enqueue(go);

        return go;
    }

    public void RetrieveAll()
    {
        foreach (var pool in _pools)
        {
            foreach (var go in pool.Value)
            {
                go.SetActive(false);
            }
        }
    }

    public void Retrieve(string tag)
    {
        foreach (var go in _pools[tag])
        {
            go.SetActive(false);
        }
    }

}
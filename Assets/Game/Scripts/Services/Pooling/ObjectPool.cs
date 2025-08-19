using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private readonly string poolName;
    private readonly GameObject prefab;
    private readonly Transform container;
    private readonly Queue<GameObject> available = new Queue<GameObject>();
    private readonly HashSet<GameObject> all = new HashSet<GameObject>();
    private readonly int maxSize;
    private readonly bool autoExpand;

    public ObjectPool(string poolName, GameObject prefab, Transform container, int initialSize, int maxSize, bool autoExpand)
    {
        this.poolName = poolName;
        this.prefab = prefab;
        this.container = container;
        this.maxSize = maxSize;
        this.autoExpand = autoExpand;
        for (int i = 0; i < initialSize; i++)
        {
            CreateNew();
        }
    }

    private GameObject CreateNew()
    {
        if(all.Count >= maxSize && !autoExpand)
        {
            Debug.LogWarning($"ObjectPool '{poolName}' has reached its maximum size of {maxSize}. Cannot create new object.");
            return null;
        }

        //Debug.Log($"Creating new object in pool '{poolName}'");

        GameObject obj = Object.Instantiate(prefab, container);
        obj.SetActive(false);
        obj.GetComponent<IPoolable>()?.OnCreate();
        all.Add(obj);
        available.Enqueue(obj);
        return obj;
    }

    public GameObject Get(Vector3 postion, Quaternion rotation, Transform parent)
    {
        if (available.Count == 0)
        {
            if (autoExpand)
            {
                CreateNew();
            }
            else
            {
                Debug.LogWarning($"ObjectPool '{poolName}' is empty. Cannot get object.");
                return null;
            }
        }
        GameObject obj = available.Dequeue();
        obj.transform.SetPositionAndRotation(postion, rotation);
        obj.transform.SetParent(parent);
        obj.SetActive(true);
        obj.GetComponent<IPoolable>()?.OnSpawned();
        return obj;
    }

    public void Return(GameObject obj)
    {
        if (obj == null || !all.Contains(obj))
        {
            return;
        }
        obj.SetActive(false);
        obj.transform.SetParent(container);
        obj.GetComponent<IPoolable>()?.OnDespawned();
        available.Enqueue(obj);
    }
}
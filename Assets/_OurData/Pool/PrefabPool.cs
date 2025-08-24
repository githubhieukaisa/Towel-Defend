using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrefabPool
{
    public Transform prefab;
    public int preloadAmount = 0;

    private Queue<Transform> inactiveInstances = new Queue<Transform>();
    private int instanceCounter = 0;

    public PrefabPool(Transform prefab)
    {
        this.prefab = prefab;
    }

    public void Preload(Transform parent)
    {
        for (int i = 0; i < preloadAmount; i++)
        {
            Transform obj = GameObject.Instantiate(prefab, parent);
            obj.gameObject.SetActive(false);
            obj.name = $"{prefab.name}(Clone){instanceCounter.ToString("D3")}";
            instanceCounter++;
            var pooled = obj.gameObject.AddComponent<PooledObject>();
            pooled.prefabKey = prefab.name;
            inactiveInstances.Enqueue(obj);
        }
    }

    public Transform Spawn(Vector3 position, Quaternion rotation, Transform parent = null)
    {
        Transform obj;

        if (inactiveInstances.Count > 0)
        {
            obj = inactiveInstances.Dequeue();
        }
        else
        {
            obj = GameObject.Instantiate(prefab);
            var pooled = obj.gameObject.AddComponent<PooledObject>();
            pooled.prefabKey = prefab.name;
        }

        obj.position = position;
        obj.rotation = rotation;
        obj.localScale = Vector3.one;
        if (parent != null) obj.SetParent(parent);
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Despawn(Transform obj)
    {
        obj.gameObject.SetActive(false);
        inactiveInstances.Enqueue(obj);
    }
}
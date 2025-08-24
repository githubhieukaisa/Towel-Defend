using System.Collections.Generic;
using UnityEngine;

public class SpawnPool : HieuMonoBehavior
{
    public string poolName = "SpawnPool";

    protected Dictionary<string, PrefabPool> prefabPoolsByName = new Dictionary<string, PrefabPool>();

    public PrefabPool GetPrefabPool(Transform prefab)
    {
        return prefabPoolsByName.ContainsKey(prefab.name) ? prefabPoolsByName[prefab.name] : null;
    }

    public void CreatePrefabPool(PrefabPool prefabPool)
    {
        if (prefabPool == null || prefabPool.prefab == null) return;

        string key = prefabPool.prefab.name;

        if (prefabPoolsByName.ContainsKey(key)) return;

        prefabPool.Preload(this.transform);
        prefabPoolsByName.Add(key, prefabPool);
    }

    public Transform Spawn(string objName, Vector3 pos, Quaternion rot, Transform parent = null)
    {
        var pool = prefabPoolsByName.ContainsKey(objName) ? prefabPoolsByName[objName] : null;

        if (pool == null)
        {
            return null;
        }

        return pool.Spawn(pos, rot, parent);
    }

    public void Despawn(Transform obj)
    {
        var pooled = obj.GetComponent<PooledObject>();
        if (pooled == null)
        {
            return;
        }

        string key = pooled.prefabKey;

        var pool = prefabPoolsByName.ContainsKey(key) ? prefabPoolsByName[key] : null;
        if (pool != null)
        {
            pool.Despawn(obj);
        }
    }
}
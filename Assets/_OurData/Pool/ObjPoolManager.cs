using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ObjPoolManager : HieuMonoBehavior
{
    public static ObjPoolManager instance;
    [SerializeField] protected string pollName = "ObjPool";
    [SerializeField] protected SpawnPool pool;
    [SerializeField] protected List<Transform> objs = new List<Transform>();

    private void Awake()
    {
        if (instance != null) return;
        instance = this;

        this.AddObjToPool();
    }
    private void Start()
    {
         //this.AddObjToPool();
    }

    protected override void LoadComponents()
    {
        this.LoadPool();
        this.LoadObjs();
    }

    protected virtual void LoadPool()
    {
        if (this.pool != null) return;

        GameObject obj = GameObject.Find(this.pollName);
        this.pool = obj.GetComponent<SpawnPool>();
        this.pool.poolName = this.pollName;
        Debug.Log(transform.name + ": LoadPool");
    }

    protected virtual void LoadObjs()
    {
        if(this.objs.Count > 0) return;
        foreach (Transform child in this.transform)
        {
            this.objs.Add(child);
            child.gameObject.SetActive(false);
        }
    }

    protected virtual void AddObjToPool()
    {
        foreach (Transform obj in this.objs)
        {
            PrefabPool prefabPool = new PrefabPool(obj)
            {
                preloadAmount = 3
            };
            bool isAlreadyPool = this.pool.GetPrefabPool(prefabPool.prefab) != null;
            if (!isAlreadyPool)
            {
                this.pool.CreatePrefabPool(prefabPool);
            }
        }
    }

    public virtual SpawnPool Pool()
    {
        this.LoadPool();
        return this.pool;
    }

    public virtual Transform Spawn(string objName, Transform transform)
    {
        return this.Pool().Spawn(objName, transform.position, transform.rotation, transform);
    }

    public virtual Transform Spawn(string objName, Vector3 position, Quaternion rotation)
    {
        return this.Pool().Spawn(objName, position, rotation);
    }
    public virtual Transform Spawn(string objName, Vector3 position, Quaternion rotation, Transform parent)
    {
        return this.Pool().Spawn(objName, position, rotation, parent);
    }

    public virtual void Despawn(Transform obj)
    {
        this.Pool().Despawn(obj);
    }
}

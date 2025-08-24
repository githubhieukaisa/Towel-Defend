using UnityEngine;

public class Spawner : HieuMonoBehavior
{
    [Header("Spawner")]
    [SerializeField] protected string enemyName = "Cube";
    [SerializeField] protected int spawnLimit = 5;
    [SerializeField] protected int finalSpawnLimit = 5;
    [SerializeField] protected float spawnDelay = 2;
    [SerializeField] protected float finalSpawnDelay = 2;
    [SerializeField] protected float spawnTimer = 0;

    private void Start()
    {
        //this.Spawning();
    }

    private void FixedUpdate()
    {
        this.Spawning();
    }

    protected virtual void Spawning()
    {
        //Invoke("Spawning", spawnDelay);

        if (!this.CanSpawn()) return;
        this.spawnTimer += Time.fixedDeltaTime;
        if (this.spawnTimer < this.SpawnDelay()) return;
        this.spawnTimer = 0;

        this.BeforeSpawn();
        Vector3 spawnPos = this.SpawnPos();
        Transform obj = ObjPoolManager.instance.Spawn(this.enemyName, spawnPos, transform.rotation, transform);
        obj.gameObject.SetActive(true);
        this.AfterSpawn(obj);
    }

    protected virtual string EnemyName()
    {
        return this.enemyName;
    }

    protected virtual void BeforeSpawn()
    {
        // Override this method to add any logic before spawning an object
    }

    protected virtual void AfterSpawn(Transform obj)
    {
        // Override this method to add any logic after spawning an object
    }

    protected virtual Vector3 SpawnPos()
    {
        float x = Random.Range(-7.0f, 7.0f);
        float y = Random.Range(4.0f, 6.0f);

        return new Vector3(x, y, 0);
    }

    protected virtual bool CanSpawn()
    {
        int childCount = this.CountActiveObject();
        return childCount < this.SpawnLimit();
    }

    protected virtual int SpawnLimit()
    {
        this.finalSpawnLimit = this.spawnLimit;
        return this.finalSpawnLimit;
    }

    protected virtual float SpawnDelay()
    {
        this.finalSpawnDelay = this.spawnDelay;
        return this.finalSpawnDelay;
    }

    protected virtual int CountActiveObject()
    {
        int count = 0;
        foreach (Transform child in transform)
        {
            if (child.gameObject.activeInHierarchy)
            {
                count++;
            }
        }
        return count;
    }
}
